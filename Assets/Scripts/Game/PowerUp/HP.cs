using UnityEngine;
using QFramework;
using QAssetBundle;

namespace TheUnfairDice
{
    public partial class HP : PowerUp
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<CollectableArea>())
            {
                if (Global.HP.Value == Global.MaxHP.Value) return;

                FlyingToPlayer = true;
            }
        }

        protected override void Excute()
        {
            Global.HP.Value++;

            AudioKit.PlaySound(Sfx.PICKUP);
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            // Ïú»Ù×ÔÉí
            this.DestroyGameObjGracefully();
        }
    }
}
