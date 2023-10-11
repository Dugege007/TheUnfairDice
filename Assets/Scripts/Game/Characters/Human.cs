using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Human : ViewController
    {
        public float HP = 3f;
        public float WalkSpeed = 1f;
        public float RunSpeed = 3f;

        public float PatrolTime = 2f;
        public float WaitTime = 5f;
        private float mRandomTime;

        private bool mFaceRight;

        public enum State
        {
            Idle,       // 站立
            Patrol,     // 巡逻，在要塞附近移动
            Warning,    // 警戒，有敌人靠近要塞时触发
            Attack,     // 攻击，敌人进入攻击范围内触发
            Retreat     // 撤退，自身血量为 1 时触发（暂定）
        }

        public FSM<State> FSM = new FSM<State>();

        private void InitFSM()
        {
            FSM.State(State.Patrol)
                .OnEnter(() =>
                {
                    mRandomTime = Random.Range(PatrolTime - 1, PatrolTime + 1);
                })
                .OnFixedUpdate(() =>
                {
                    // 朝要塞反方向的 60 度范围内移动
                    float randomDegrees = Random.Range(-30, 30);
                    Vector2 direction = Quaternion.Euler(0, 0, randomDegrees) * (this.Position() - Fortress.Default.Position()).normalized;

                    SelfRigidbody2D.velocity = direction * WalkSpeed;

                    if (FSM.FrameCountOfCurrentState >= 60 * mRandomTime)
                        FSM.ChangeState(State.Idle);
                });

            FSM.State(State.Idle)
                .OnEnter(() =>
                {
                    SelfRigidbody2D.velocity = Vector2.zero;
                    mRandomTime = Random.Range(WaitTime - 1, WaitTime + 1);
                })
                .OnUpdate(() =>
                {
                    if (Player.Default)
                    {
                        if (FSM.FrameCountOfCurrentState >= 60 * mRandomTime)
                            FSM.ChangeState(State.Patrol);
                    }
                });

            FSM.StartState(State.Patrol);
        }

        private void Start()
        {
            // 初始化状态机
            InitFSM();

            HitHurtBox.OnCollisionEnter2DEvent(collision2D =>
            {
                HitHurtBox hurtBox = collision2D.gameObject.GetComponentInChildren<HitHurtBox>();

                if (hurtBox != null)
                {
                    if (hurtBox.Owner.CompareTag("Enemy"))
                    {
                        HP--;
                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                        enemy.GetHurt(1);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (HP <= 0)
            {
                this.DestroyGameObjGracefully();
            }

            FSM.Update();
        }

        private void FixedUpdate()
        {
            FSM.FixedUpdate();

            float velocityX = SelfRigidbody2D.velocity.x;
            float velocityY = SelfRigidbody2D.velocity.y;

            if (velocityX > 0.1f)
                mFaceRight = true;
            else if (velocityX < 0.1f)
                mFaceRight = false;

            if (velocityX < 0.1f && velocityY < 0.1f)
                Sprite.Play("IdleRight");
            else
                Sprite.Play("WalkRight");

            if (mFaceRight)
                Sprite.LocalScaleX(1);
            else
                Sprite.LocalScaleX(-1);
        }
    }
}
