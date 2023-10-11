using UnityEngine;
using QFramework;
using System.Linq;

namespace TheUnfairDice
{
    public partial class HolyWater : ViewController
    {
        private float mCurrentSec = 0;

        private void Start()
        {
            Ripple.Hide();
        }

        private void Update()
        {
            mCurrentSec += Time.deltaTime;

            if (mCurrentSec >= Global.HolyWaterCDTime.Value)
            {
                mCurrentSec = 0;

                Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                foreach (Enemy enemy in enemies
                    .OrderBy(e => e.Direction2DFrom(Player.Default).magnitude)
                    .Where(e => e.Direction2DFrom(Player.Default).magnitude <= Global.HolyWaterRange.Value + 1)
                    .Take(1))
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
                                        Enemy e = hurtBox.Owner.GetComponent<Enemy>();
                                        DamageSystem.CalculateDamage(Global.HolyWaterDamage.Value, e);
                                    }
                                }

                            }).UnRegisterWhenGameObjectDestroyed(selfCache);

                            float range = Global.HolyWaterRange.Value / 1.7f;

                            // ��Ӷ���
                            ActionKit.Sequence()
                                // �𽥱��
                                .Lerp(0.5f, range, Global.HolyWaterDuration.Value, scale => selfCache.LocalScale(scale))
                                .Callback(() =>
                                {
                                    // �ر���ײ
                                    selfCache.enabled = false;
                                })
                                .Parallel(p =>
                                {
                                    // ��΢��С
                                    p.Lerp(range, range - 0.5f, 0.3f, scale => selfCache.LocalScale(scale));

                                    float alpha = selfCache.GetComponent<SpriteRenderer>().color.a;
                                    p.Append(ActionKit.Sequence()
                                        // ��͸��
                                        .Lerp(alpha, 0, 0.3f, a =>
                                        {
                                            Color color = selfCache.GetComponent<SpriteRenderer>().color;
                                            color.a = a;
                                            selfCache.GetComponent<SpriteRenderer>().color = color;
                                        }));
                                })
                                .Start(this, () =>
                                {
                                    // ��������
                                    selfCache.DestroyGameObjGracefully();
                                });
                        });
                }
            }
        }
    }
}
