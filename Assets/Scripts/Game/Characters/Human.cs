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

        public enum State
        {
            Idle,       // 站立
            Patrol,     // 巡逻，在要塞附近移动
            Warning,    // 警戒，有敌人靠近要塞时触发
            Attack,     // 攻击，敌人进入攻击范围内触发
            Retreat     // 撤退，自身血量为 1 时触发（暂定）
        }

        public FSM<State> FSM = new FSM<State>();

        private void Start()
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
                    if (FSM.FrameCountOfCurrentState >= 60 * mRandomTime)
                        FSM.ChangeState(State.Patrol);
                });

            FSM.StartState(State.Patrol);

            HitHurtBox.OnCollisionEnter2DEvent(collision2D =>
            {
                HitHurtBox hurtBox = collision2D.gameObject.GetComponent<HitHurtBox>();

                if (hurtBox != null)
                {
                    if (hurtBox.Owner.CompareTag("Enemy"))
                    {
                        HP--;
                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                        enemy.GetHurt(1);
                    }
                }

                if (collision2D.gameObject.CompareTag("Human"))
                {
                    //
                }
            });
        }

        private void Update()
        {
            FSM.Update();

            if (HP <= 0)
            {
                this.DestroyGameObjGracefully();
            }
        }

        private void FixedUpdate()
        {
            FSM.FixedUpdate();
        }
    }
}
