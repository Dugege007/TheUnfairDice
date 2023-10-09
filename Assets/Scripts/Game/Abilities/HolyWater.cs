using UnityEngine;
using QFramework;
using System.Linq;

namespace TheUnfairDice
{
    public partial class HolyWater : ViewController
    {
        public float HolyWaterDamage = 1f;
        public float HolyWaterDuration = 1f;
        public float HolyWaterCDTime = 2f;
        public float HolyWaterRange = 2.5f;     // 半径
        public float HolyWaterImpulse = 200f;   // 击退的力

        private float mCurrentSec = 0;

        private void Start()
        {
            Ripple.Hide();
        }

        private void Update()
        {
            mCurrentSec += Time.deltaTime;

            if (mCurrentSec >= HolyWaterCDTime)
            {
                mCurrentSec = 0;

                Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                foreach (Enemy enemy in enemies
                    .OrderBy(e => e.Direction2DFrom(Player.Default).magnitude)
                    .Where(e => e.Direction2DFrom(Player.Default).magnitude <= 4f))
                {
                    Ripple.Instantiate()
                        .Position(Player.Default.Position())
                        .Show()
                        .Self(self =>
                        {
                            CircleCollider2D selfCache = self;
                            selfCache.OnTriggerEnter2DEvent(collider2D =>
                            {
                                HitHurtBox hurtBox = collider2D.GetComponent<HitHurtBox>();
                                if (hurtBox != null)
                                {
                                    if (hurtBox.Owner.CompareTag("Enemy"))
                                    {
                                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                                        enemy.GetHurt(HolyWaterDamage);
                                    }
                                }

                            }).UnRegisterWhenGameObjectDestroyed(selfCache);

                            // 添加动画
                            ActionKit.Sequence()
                                // 逐渐变大
                                .Lerp(1f, HolyWaterRange * 2, HolyWaterDuration, scale => selfCache.LocalScale(scale))
                                .Callback(() =>
                                {
                                    // 关闭碰撞
                                    selfCache.enabled = false;
                                })
                                .Parallel(p =>
                                {
                                    // 稍微变大
                                    p.Lerp(5f, 6f, 0.3f, scale => selfCache.LocalScale(scale));

                                    float alpha = selfCache.GetComponent<SpriteRenderer>().color.a;
                                    p.Append(ActionKit.Sequence()
                                        // 变透明
                                        .Lerp(alpha, 0, 0.3f, a =>
                                        {
                                            Color color = selfCache.GetComponent<SpriteRenderer>().color;
                                            color.a = a;
                                            selfCache.GetComponent<SpriteRenderer>().color = color;
                                        }));
                                })
                                .Start(this, () =>
                                {
                                    // 销毁自身
                                    selfCache.DestroyGameObjGracefully();
                                });
                        });
                }
            }
        }
    }
}
