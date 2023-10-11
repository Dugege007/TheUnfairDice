using UnityEngine;
using QFramework;
using QAssetBundle;

namespace TheUnfairDice
{
    public partial class Enemy : ViewController
    {
        public EnemyConfig EnemyConfig;

        private float mHP;
        private float mDamage;
        private float mSpeed;

        [HideInInspector]
        public bool IsTargetFortress;
        private float mChangeTargetDistanceMax = 30f;
        private float mChangeTargetDistanceMin = 12f;
        private bool mFaceRight;

        private float mWaitTime = 3f;
        private float mRandomTime = 0;
        private float mChangeToFortressTimer = 0;

        private bool mIgnoreHurt = false;

        public enum State
        {
            Idle,           // 站立
            Chace,          // 追击玩家
            AttackFortress, // 攻击要塞
        }

        public FSM<State> FSM = new FSM<State>();

        private void InitFSM()
        {
            FSM.State(State.Chace)
                .OnFixedUpdate(() =>
                {
                    if (Player.Default)
                    {
                        Vector3 direction = (Player.Default.Position() - this.Position()).normalized;
                        Move(direction);

                        // 如果与玩家距离过远
                        if (this.Distance2D(Player.Default) > mChangeTargetDistanceMax)
                        {
                            mChangeTargetDistanceMax = Mathf.Max(5, mChangeTargetDistanceMax - 5);

                            FSM.ChangeState(State.Idle);
                        }
                    }
                    else
                    {
                        SelfRigidbody2D.velocity = Vector2.zero;
                    }
                });

            FSM.State(State.Idle)
                .OnEnter(() =>
                {
                    SelfRigidbody2D.velocity = Vector2.zero;
                    mRandomTime = Random.Range(mWaitTime - 1, mWaitTime + 1);

                    Animator.Play("IdleRight");
                })
                .OnUpdate(() =>
                {
                    mChangeToFortressTimer += Time.deltaTime;

                    // 一段时间之后
                    if (mChangeToFortressTimer > mRandomTime)
                    {
                        mChangeToFortressTimer = 0;

                        // 目标为要塞
                        IsTargetFortress = true;
                        FSM.ChangeState(State.AttackFortress);
                    }
                });

            FSM.State(State.AttackFortress)
                .OnFixedUpdate(() =>
                {
                    if (Player.Default && Fortress.Default)
                    {
                        Vector3 direction = (Fortress.Default.Position() - this.Position()).normalized;
                        Move(direction);

                        // 如果玩家靠近
                        if (this.Distance2D(Player.Default) < mChangeTargetDistanceMin)
                        {
                            mChangeTargetDistanceMin = Mathf.Max(3, mChangeTargetDistanceMin - 3);

                            // 目标改为玩家
                            IsTargetFortress = false;
                            FSM.ChangeState(State.Chace);
                        }
                    }
                    else
                    {
                        SelfRigidbody2D.velocity = Vector2.zero;
                    }
                });

            FSM.StartState(State.Chace);
        }

        private void Start()
        {
            mHP = EnemyConfig.HP;
            mDamage = EnemyConfig.Damage;
            mSpeed = EnemyConfig.Speed;
            IsTargetFortress = EnemyConfig.IsTargetFortress;

            // 初始化状态机
            InitFSM();

            EnemyGenerator.CurrentEnemyCount.Value++;
        }

        private void FixedUpdate()
        {
            FSM.FixedUpdate();
        }

        private void Update()
        {
            FSM.Update();

            if (mHP <= 0)
            {
                Global.GeneratePowerUp(gameObject);
                this.DestroyGameObjGracefully();
            }
        }

        private void Move(Vector3 direction)
        {
            if (direction.x > 0.1f)
                mFaceRight = true;
            else if (direction.x < 0.1f)
                mFaceRight = false;

            Animator.Play("WalkRight");

            if (mFaceRight)
                Sprite.LocalScaleX(1);
            else
                Sprite.LocalScaleX(-1);

            SelfRigidbody2D.velocity = direction * mSpeed;
        }

        public void GetHurt(float hurtValue, bool force = false, bool critical = false)
        {
            if (mIgnoreHurt && !force) return;

            Color cacheColor = Sprite.color;

            // 停止移动
            SelfRigidbody2D.velocity = Vector3.zero;
            // 忽略伤害
            mIgnoreHurt = true;
            // 变为红色
            Sprite.color = Color.red;
            // 伤害飘字
            FloatingTextController.Play(transform.position + Vector3.up * 0.5f, hurtValue.ToString("0"), critical);
            // 播放音效
            AudioKit.PlaySound(Sfx.ENEMYHURT);

            // 延时执行
            ActionKit.Delay(0.15f, () =>
            {
                // 减血
                mHP -= hurtValue;
                // 变回白色
                Sprite.color = cacheColor;
                // 在受伤期间不再受到伤害，避免冲突
                mIgnoreHurt = false;

            }).Start(this);   // 自身执行
        }

        private void OnDestroy()
        {
            EnemyGenerator.CurrentEnemyCount.Value--;
        }
    }
}
