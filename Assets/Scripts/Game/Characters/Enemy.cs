using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Enemy : ViewController
    {
        public float HP = 1f;
        public float MovementSpeed = 1.5f;

        public bool IsTargetFortress = false;

        private void Start()
        {
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

                SelfRgidbody2D.velocity = direction * MovementSpeed;
            }
            else
            {
                SelfRgidbody2D.velocity = Vector2.zero;
            }
        }

        private void Update()
        {
            if (HP <= 0)
            {
                Global.GeneratePowerUp(gameObject);
                this.DestroyGameObjGracefully();
            }
        }

        public void GetHurt(float hurtValue)
        {
            HP -= hurtValue;
        }

        private void OnDestroy()
        {
            EnemyGenerator.CurrentEnemyCount.Value--;
        }
    }
}
