using QFramework;
using UnityEngine;

namespace TheUnfairDice
{
    public abstract class PowerUp : ViewController
    {
        public bool FlyingToPlayer { get; set; }
        private int mFlyingToPlayerFrameCount = 0;

        protected abstract void Excute();

        private void Update()
        {
            if (FlyingToPlayer)
            {
                if (mFlyingToPlayerFrameCount == 0)
                    GetComponent<SpriteRenderer>().sortingOrder = 5;

                mFlyingToPlayerFrameCount++;

                if (Player.Default)
                {
                    Vector2 direction = Player.Default.Direction2DFrom(this);
                    float distance = this.Distance2D(Player.Default);

                    if (mFlyingToPlayerFrameCount <= 12)
                        transform.Translate(-direction.normalized * Time.deltaTime);
                    else
                        transform.Translate(direction.normalized * 7.5f * Time.deltaTime);

                    if (distance < 0.5f)
                        Excute();
                }
            }
        }
    }
}
