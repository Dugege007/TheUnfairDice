using UnityEngine;
using QFramework;
using QAssetBundle;

namespace TheUnfairDice
{
    public partial class Player : ViewController
    {
        public static Player Default;

        private bool mFaceRight;

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

                        AudioKit.PlaySound(Sfx.PLAYERHURT);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HP.RegisterWithInitValue(hp =>
            {
                UpdateHP();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.MaxHP.RegisterWithInitValue(maxHP =>
            {
                UpdateHP();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal > 0)
                mFaceRight = true;
            else if (horizontal < 0)
                mFaceRight = false;

            if (horizontal == 0 && vertical == 0)
                Sprite.Play("IdleRight");
            else
                Sprite.Play("FlyRight");

            if (mFaceRight)
                Sprite.LocalScaleX(1);
            else
                Sprite.LocalScaleX(-1);

            Rigidbody2D.velocity = new Vector2(horizontal, vertical).normalized * Global.Speed.Value;
        }

        private void Update()
        {
            if (Global.HP.Value <= 0)
            {
                Death.Instantiate()
                    .Position(this.Position())
                    .LocalScale(Sprite.LocalScale())
                    .Show()
                    .Play("Death");

                this.DestroyGameObjGracefully();

                UIKit.OpenPanel<UIGameOverPanel>();
            }
        }

        private void UpdateHP()
        {
            HPValue.fillAmount = Global.HP.Value / (float)Global.MaxHP.Value;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
