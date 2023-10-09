using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Player : ViewController
    {
        public static Player Default;

        public float MovementSpeed = 3.5f;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            // 为自己的 HitHurtBox 注册碰撞事件
            HitHurtBox.OnTriggerEnter2DEvent(collider2D =>
            {
                HitHurtBox hurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hurtBox != null)
                {
                    if (hurtBox.Owner.CompareTag("Enemy"))
                    {
                        Global.HP.Value--;
                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                        enemy.GetHurt(1);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Rigidbody2D.velocity = new Vector2(horizontal, vertical).normalized * MovementSpeed;
        }

        private void Update()
        {
            if (Global.HP.Value <= 0)
            {
                this.DestroyGameObjGracefully();

                UIKit.OpenPanel<UIGameOverPanel>();
            }
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
