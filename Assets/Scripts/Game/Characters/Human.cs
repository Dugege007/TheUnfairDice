using UnityEngine;
using QFramework;
using QAssetBundle;
using System.Linq;

namespace TheUnfairDice
{
    public partial class Human : ViewController
    {
        public float HP = 3f;
        public float Damage = 3f;
        public float WalkSpeed = 2f;
        public float RunSpeed = 3f;

        private float mSpearDamage = 3f;
        private float mSpearRange = 3f;
        private float mSpearCDTime = 3f;
        private float mAttackTimer = 0;

        public float PatrolTime = 2f;
        public float WaitTime = 3f;
        private float mRandomTime;

        private float mRandomDegrees = 0;
        private bool mTargetToFortress = false;
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
                    // 朝要塞反方向的 60 度范围内移动
                    mRandomDegrees = Random.Range(-30, 30);
                })
                .OnFixedUpdate(() =>
                {
                    Vector2 direction = Vector2.zero;

                    if (mTargetToFortress == false)
                        direction = Quaternion.Euler(0, 0, mRandomDegrees) * (this.Position() - Fortress.Default.Position()).normalized;
                    else
                        direction = this.Direction2DTo(Fortress.Default).normalized;

                    Move(direction);

                    if (FSM.FrameCountOfCurrentState >= 60 * mRandomTime)
                        FSM.ChangeState(State.Idle);

                    //Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                    //if (enemies
                    //        .OrderBy(e => e.Direction2DFrom(this).magnitude)
                    //        .Where(e => e.Direction2DFrom(this).magnitude <= mSpearRange) != null)
                    //{
                    //    FSM.ChangeState(State.Attack);
                    //}
                });

            FSM.State(State.Idle)
                .OnEnter(() =>
                {
                    SelfRigidbody2D.velocity = Vector2.zero;
                    mRandomTime = Random.Range(WaitTime - 1, WaitTime + 1);

                    Animator.Play("IdleRight");
                })
                .OnUpdate(() =>
                {
                    if (Player.Default)
                    {
                        if (this.Distance2D(Fortress.Default) > 25f)
                        {
                            mTargetToFortress = true;
                            FSM.ChangeState(State.Patrol);
                        }

                        if (FSM.FrameCountOfCurrentState >= 60 * mRandomTime)
                            FSM.ChangeState(State.Patrol);
                    }
                });

            //    FSM.State(State.Attack)
            //        .OnUpdate(() =>
            //        {
            //            mAttackTimer += Time.deltaTime;
            //            if (mAttackTimer >= mSpearCDTime)
            //            {
            //                Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            //                foreach (Enemy enemy in enemies
            //                    .OrderBy(e => e.Direction2DFrom(this).magnitude)
            //                    .Where(e => e.Direction2DFrom(this).magnitude <= mSpearRange)
            //                    .Take(1))
            //                {
            //                    Spear.Instantiate()
            //                        .Position(Player.Default.Position())
            //                        .Show()
            //                        .Self(self =>
            //                        {
            //                            Collider2D selfCache = self;
            //                            selfCache.OnTriggerEnter2DEvent(collider2D =>
            //                            {
            //                                HitHurtBox hurtBox = collider2D.GetComponent<HitHurtBox>();
            //                                if (hurtBox != null)
            //                                {
            //                                    if (hurtBox.Owner.CompareTag("Enemy"))
            //                                    {
            //                                        Enemy e = hurtBox.Owner.GetComponent<Enemy>();
            //                                        DamageSystem.CalculateDamage(mSpearDamage, e);
            //                                    }
            //                                }

            //                            }).UnRegisterWhenGameObjectDestroyed(selfCache);

            //                            float range = Global.HolyWaterRange.Value / 1.7f;

            //                            // 劈砍动画
            //                            ActionKit.Sequence()    //开启一个队列
            //                                .Callback(() =>
            //                                {
            //                                    // 先取消碰撞
            //                                    selfCache.enabled = false;
            //                                })
            //                                .Parallel(p =>     // 开一个并行的任务
            //                                {
            //                                    // 抬起
            //                                    p.Lerp(0, 10, 0.2f, z => selfCache.LocalEulerAnglesZ(z));

            //                                    p.Append(ActionKit.Sequence()
            //                                        // 放大
            //                                        .Lerp(0, 1.25f, 0.1f, scale => selfCache.LocalScale(scale))
            //                                        // 稍微缩小
            //                                        .Lerp(1.25f, 1f, 0.1f, scale => selfCache.LocalScale(scale))
            //                                    );
            //                                })
            //                                .Callback(() =>
            //                                {
            //                                    // 打开碰撞
            //                                    selfCache.enabled = true;
            //                                })
            //                                .Parallel(p =>
            //                                {
            //                                    // 向下砍
            //                                    p.Lerp(10, -180, 0.2f, z => selfCache.LocalEulerAnglesZ(z));

            //                                    p.Append(ActionKit.Sequence()
            //                                        // 稍微放大
            //                                        .Lerp(1, 1.25f, 0.1f, scale => selfCache.LocalScale(scale))
            //                                        // 稍微缩小
            //                                        .Lerp(1.25f, 1f, 0.1f, scale => selfCache.LocalScale(scale))
            //                                    );
            //                                })
            //                                .Callback(() =>
            //                                {
            //                                    // 关闭碰撞
            //                                    selfCache.enabled = false;
            //                                })
            //                                .Lerp(-180, 0, 0.3f, z =>
            //                                {
            //                                    selfCache.LocalEulerAnglesZ(z);
            //                                    selfCache.LocalScale(z.Abs() / 180);
            //                                })
            //                                .Start(this, () =>
            //                                {
            //                                    selfCache.DestroyGameObjGracefully();
            //                                });
            //                        });
            //                }
            //            }

            //            FSM.ChangeState(State.Patrol);
            //        });

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
                        DamageSystem.CalculateDamage(Damage, enemy);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (HP <= 0)
            {
                this.DestroyGameObjGracefully();

                AudioKit.PlaySound(Sfx.HUMANHURT);
            }

            if (this.Distance2D(Fortress.Default) <= 3)
            {
                FSM.ChangeState(State.Idle);
            }

            FSM.Update();
        }

        private void FixedUpdate()
        {
            FSM.FixedUpdate();
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

            SelfRigidbody2D.velocity = direction * WalkSpeed;
        }
    }
}
