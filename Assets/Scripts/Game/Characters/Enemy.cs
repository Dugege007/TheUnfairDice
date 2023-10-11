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
                });

            FSM.State(State.Idle)
                .OnEnter(() =>
                {
                    SelfRigidbody2D.velocity = Vector2.zero;
                    mRandomTime = Random.Range(mWaitTime - 1, mWaitTime + 1);

                    Sprite.Play("IdleRight");
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
                    if (Player.Default)
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

                AudioKit.PlaySound(Sfx.ENEMYDEAD);
            }
        }

        private void Move(Vector3 direction)
        {
            float velocityX = SelfRigidbody2D.velocity.x;

            if (velocityX > 0.1f)
                mFaceRight = true;
            else if (velocityX < 0.1f)
                mFaceRight = false;

            Sprite.Play("WalkRight");

            if (mFaceRight)
                Sprite.LocalScaleX(1);
            else
                Sprite.LocalScaleX(-1);

            SelfRigidbody2D.velocity = direction * mSpeed;
        }

        public void GetHurt(float hurtValue)
        {
            mHP -= hurtValue;

            AudioKit.PlaySound(Sfx.ENEMYHURT);
        }

        private void OnDestroy()
        {
            EnemyGenerator.CurrentEnemyCount.Value--;
        }
    }
}
