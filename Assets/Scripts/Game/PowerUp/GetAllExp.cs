using UnityEngine;
using QFramework;
using QAssetBundle;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TheUnfairDice
{
    public partial class GetAllExp : PowerUp
    {
        static IEnumerator FlyToPlayerStart()
        {
            // �ҵ����е� PowerUp ���� Exp �� Coin
            IEnumerable<PowerUp> exps = FindObjectsByType<Exp>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            IEnumerable<PowerUp> coins = FindObjectsByType<Gold>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            int count = 0;
            // ������ƴ�ӣ�ʹ������Ʒ������˳�������ң��������ȷ���һ���ٷ���һ��
            foreach (var powerUp in exps.Concat(coins)
                .OrderByDescending(e => e.InScreen)
                .ThenBy(e => e.Distance2D(Player.Default)))
            {
                if (powerUp.InScreen)
                {
                    if (count > 8)
                    {
                        count = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    if (count > 2)
                    {
                        count = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }

                count++;
                ActionKit.OnUpdate.Register(() =>
                {
                    Player player = Player.Default;
                    if (player)
                    {
                        // �� Exp �������
                        Vector3 direction = (player.Position() - powerUp.Position()).normalized;
                        powerUp.transform.Translate(direction * 12f * Time.deltaTime);
                    }

                }).UnRegisterWhenGameObjectDestroyed(powerUp);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<CollectableArea>())
            {
                FlyingToPlayer = true;
            }
        }

        protected override void Excute()
        {
            Global.Exp.Value++;
            PowerUpManager.Default.StartCoroutine(FlyToPlayerStart());

            GetComponent<SpriteRenderer>().sortingOrder = 1;
            // ��������
            this.DestroyGameObjGracefully();

            AudioKit.PlaySound(Sfx.PICKUP);
        }
    }
}
