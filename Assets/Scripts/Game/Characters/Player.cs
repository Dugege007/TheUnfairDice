using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
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
                        hurtBox.Owner.GetComponent<Enemy>().GetHurt(1);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector2 velocity = new Vector2(horizontal, vertical).normalized * MovementSpeed;
            Rigidbody2D.velocity = velocity;
        }

        private void Update()
        {
            if (Global.HP.Value <= 0)
            {
                this.DestroyGameObjGracefully();
            }
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
