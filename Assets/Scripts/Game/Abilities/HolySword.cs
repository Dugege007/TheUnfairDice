using UnityEngine;
using QFramework;
using QAssetBundle;
using System.Linq;

namespace TheUnfairDice
{
    public partial class HolySword : ViewController
    {
        private float mCurrentSeconds = 0;

        private void Start()
        {
        }

        private void Update()
        {
            mCurrentSeconds += Time.deltaTime;

            if (mCurrentSeconds >= Global.HolySwordCDTime.Value)
            {
                mCurrentSeconds = 0;

                if (Player.Default)
                {
                    // ��ȡ��ǰ���ڵĵ����б�
                    var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                        .OrderBy(e => Player.Default.Distance2D(e))
                        .Take(Global.HolySwordCount.Value);

                    int i = 0;
                    foreach (Enemy enemy in enemies)
                    {
                        if (i < 4)
                        {
                            ActionKit.DelayFrame(10 * i, () => AudioKit.PlaySound(Sfx.SWORD)).StartGlobal();
                            i++;
                        }

                        if (enemy)
                        {
                            Sword.Instantiate()
                                .Position(this.Position())
                                .Show()
                                .Self(self =>
                                {
                                    Collider2D selfCache = self;

                                    // �����˶�����
                                    Vector2 direction = (enemy.Position() - Player.Default.Position()).normalized;
                                    self.transform.up = direction;
                                    Rigidbody2D rigidbody2D = self.GetComponent<Rigidbody2D>();
                                    rigidbody2D.velocity = direction * 10;

                                    int attackCount = 0;
                                    // �����ײ�¼�
                                    self.OnTriggerEnter2DEvent(collider =>
                                    {
                                        HitHurtBox hurtBox = collider.GetComponent<HitHurtBox>();
                                        if (hurtBox)
                                        {
                                            // �������˾Ͷ�������˺�
                                            if (hurtBox.Owner.CompareTag("Enemy"))
                                            {
                                                AudioKit.PlaySound(Sfx.SWORDIMPACT);
                                                Enemy e = hurtBox.Owner.GetComponent<Enemy>();
                                                DamageSystem.CalculateDamage(Global.HolySwordDamage.Value, e);
                                                attackCount++;

                                                if (attackCount >= Global.HolySwordAttackCount.Value)
                                                {
                                                    selfCache.DestroyGameObjGracefully();
                                                }
                                            }
                                        }

                                    }).UnRegisterWhenGameObjectDestroyed(self);

                                    // ע������Զ���������¼�
                                    ActionKit.OnUpdate.Register(() =>
                                    {
                                        if (Player.Default)
                                        {
                                            if (Vector3.Distance(Player.Default.Position(), selfCache.Position()) > 20)
                                            {
                                                selfCache.DestroyGameObjGracefully();
                                            }
                                        }

                                    }).UnRegisterWhenGameObjectDestroyed(self);
                                });
                        }
                    }
                }
            }
        }
    }
}
