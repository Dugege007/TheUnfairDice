using UnityEngine;
using QFramework;
using QAssetBundle;

namespace TheUnfairDice
{
    public partial class Exp : PowerUp
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
            Global.Exp.Value++;

            AudioKit.PlaySound(Sfx.PICKUP);
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            this.DestroyGameObjGracefully();
        }
    }
}
