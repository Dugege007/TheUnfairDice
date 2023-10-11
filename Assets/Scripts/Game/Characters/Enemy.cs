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
            Idle,           // վ��
            Chace,          // ׷�����
            AttackFortress, // ����Ҫ��
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

                        // �������Ҿ����Զ
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

                    // һ��ʱ��֮��
                    if (mChangeToFortressTimer > mRandomTime)
                    {
                        mChangeToFortressTimer = 0;

                        // Ŀ��ΪҪ��
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

                        // �����ҿ���
                        if (this.Distance2D(Player.Default) < mChangeTargetDistanceMin)
                        {
                            mChangeTargetDistanceMin = Mathf.Max(3, mChangeTargetDistanceMin - 3);

                            // Ŀ���Ϊ���
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

            // ��ʼ��״̬��
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

            // ֹͣ�ƶ�
            SelfRigidbody2D.velocity = Vector3.zero;
            // �����˺�
            mIgnoreHurt = true;
            // ��Ϊ��ɫ
            Sprite.color = Color.red;
            // �˺�Ʈ��
            FloatingTextController.Play(transform.position + Vector3.up * 0.5f, hurtValue.ToString("0"), critical);
            // ������Ч
            AudioKit.PlaySound(Sfx.ENEMYHURT);

            // ��ʱִ��
            ActionKit.Delay(0.15f, () =>
            {
                // ��Ѫ
                mHP -= hurtValue;
                // ��ذ�ɫ
                Sprite.color = cacheColor;
                // �������ڼ䲻���ܵ��˺��������ͻ
                mIgnoreHurt = false;

            }).Start(this);   // ����ִ��
        }

        private void OnDestroy()
        {
            EnemyGenerator.CurrentEnemyCount.Value--;
        }
    }
}
