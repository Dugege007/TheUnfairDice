using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Dice : ViewController
    {
        public Sprite[] DiceSprites;
        public int Point;

        private void Start()
        {
            // 用于设置动画的起始帧的随机数
            float randomStartPoint = Random.Range(0f, 1f);

            // 开始播放动画，从随机的起始点开始
            SelfAnimator.Play("DiceRoll", -1, randomStartPoint);
        }
    }
}
