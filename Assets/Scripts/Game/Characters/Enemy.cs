using UnityEngine;
using QFramework;

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

        private float mChangeTargetTime;
        private float mChangeToFortressTimer = 0;
        private float mChangeToPlayerTimer = 0;

        private void Start()
        {
            mHP = EnemyConfig.HP;
            mDamage = EnemyConfig.Damage;
            mSpeed = EnemyConfig.Speed;
            IsTargetFortress = EnemyConfig.IsTargetFortress;

            mChangeTargetTime = Random.Range(3f, 5f);

            EnemyGenerator.CurrentEnemyCount.Value++;
        }

        private void FixedUpdate()
        {
            if (Player.Default)
            {
                Vector3 direction;
                if (IsTargetFortress)
                    direction = (Fortress.Default.Position() - this.Position()).normalized;
                else
                    direction = (Player.Default.Position() - this.Position()).normalized;

                SelfRgidbody2D.velocity = direction * mSpeed;
            }
            else
            {
                SelfRgidbody2D.velocity = Vector2.zero;
            }
        }

        private void Update()
        {
            if (Player.Default)
            {
                // �������Ҿ����Զ
                if (this.Distance2D(Player.Default) > 30f)
                {
                    mChangeToFortressTimer += Time.deltaTime;
                    // һ��ʱ��֮��
                    if (mChangeToFortressTimer > mChangeTargetTime)
                    {
                        mChangeToPlayerTimer = 0;
                        mChangeToFortressTimer = 0;
                        // Ŀ��ΪҪ��
                        IsTargetFortress = true;
                    }
                }
                // �������Ҹ���
                else if(this.Distance2D(Player.Default) < 15f)
                {
                    mChangeToPlayerTimer += Time.deltaTime;
                    // һ��ʱ��֮��
                    if (mChangeToPlayerTimer > mChangeTargetTime)
                    {
                        mChangeToPlayerTimer = 0;
                        mChangeToFortressTimer = 0;
                        // Ŀ��Ϊ���
                        IsTargetFortress = false;
                    }
                }
            }

            if (mHP <= 0)
            {
                Global.GeneratePowerUp(gameObject);
                this.DestroyGameObjGracefully();
            }
        }

        public void GetHurt(float hurtValue)
        {
            mHP -= hurtValue;
        }

        private void OnDestroy()
        {
            EnemyGenerator.CurrentEnemyCount.Value--;
        }
    }
}
