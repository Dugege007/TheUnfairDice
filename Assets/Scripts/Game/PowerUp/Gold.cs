using UnityEngine;
using QFramework;
using QAssetBundle;

namespace TheUnfairDice
{
    public partial class Gold : PowerUp
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<CollectableArea>())
            {
                FlyingToPlayer = true;
            }
        }

        protected override void Excute()
        {
            Global.Gold.Value++;

            GetComponent<SpriteRenderer>().sortingOrder = 1;
            // Ïú»Ù×ÔÉí
            this.DestroyGameObjGracefully();

            AudioKit.PlaySound(Sfx.PICKUP);
        }
    }
}
