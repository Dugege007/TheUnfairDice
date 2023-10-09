using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace TheUnfairDice
{
    public partial class DiceController : ViewController
    {
        public List<Dice> Dices = new List<Dice>();

        public int DiceCount = 1;
        public bool IsRolling = false;
        public float RollingSec = 1f;
        public float ShowDicePointSec = 3f;

        private float mDicePosSpaceScale = 0.8f;
        private float mShowDicePointSecRemain = 3f;
        private int mShowingDiceCount = 0;

        private void Update()
        {
            // 如果已展示骰子数量和拥有骰子数量相等
            if (mShowingDiceCount == DiceCount)
            {
                // 开始倒计时
                mShowDicePointSecRemain -= Time.deltaTime;

                // 如果倒计时结束
                if (mShowDicePointSecRemain < 0)
                {
                    // 删除所有骰子
                    foreach (Dice dice in Dices)
                    {
                        dice.DestroyGameObjGracefully();
                    }

                    // 清空列表
                    Dices.Clear();

                    // 重置数据
                    mShowDicePointSecRemain = ShowDicePointSec;
                    mShowingDiceCount = 0;
                    IsRolling = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsRolling == false)
            {
                RollDice();
            }
        }

        private void RollDice()
        {
            // 正在 Roll
            IsRolling = true;

            float playerPosX = Player.Default.Position().x;
            float playerPosY = Player.Default.Position().y;

            // 第一颗骰子位置 X 轴的偏移值
            float offsetX = (DiceCount % 2 == 0) ?
                            -(DiceCount / 2 - 0.5f) * mDicePosSpaceScale :
                            -(DiceCount / 2) * mDicePosSpaceScale;

            // 骰子需要偏移的向量
            Vector3 offsetV3 = Vector3.zero;

            // 按拥有骰子的数量 Roll
            for (int i = 0; i < DiceCount; i++)
            {
                offsetV3 = new Vector3(playerPosX + offsetX + i * mDicePosSpaceScale, playerPosY + 1, 0);

                // 生成骰子
                Dice.Instantiate()
                    .Position(Player.Default.Position())
                    .Show()
                    .Self(self =>
                    {
                        // 缓存
                        SpriteRenderer selfCache = self;
                        Dice dice = selfCache.GetComponent<Dice>();
                        // 动画播放时间
                        float randomTime = Random.Range(RollingSec, RollingSec + 1f);

                        //// 不太好加的动画
                        //ActionKit.Sequence()
                        //    .Parallel(p =>
                        //    {
                        //        p.Lerp(0.3f, 0.8f, 0.3f, scale => dice.LocalScale(scale));
                        //        p.Append(ActionKit.Sequence()
                        //            .Lerp(playerPosX, offsetX, 0.3f, posX => dice.PositionX(posX)));
                        //        p.Append(ActionKit.Sequence()
                        //            .Lerp(playerPosY, playerPosY + 1, 0.3f, posY => dice.PositionY(posY)));
                        //    })
                        //    .Callback(() =>
                        //    {
                        //    });

                        // 延时任务
                        ActionKit.Delay(randomTime, () =>
                        {
                            // 取随机点数
                            int randomIndex = Random.Range(0, dice.DiceSprites.Length);
                            // 关动画
                            dice.SelfAnimator.enabled = false;
                            // 换 Sprite
                            selfCache.sprite = dice.DiceSprites[randomIndex];
                            // 得到这个骰子的点数
                            dice.Point = randomIndex + 1;

                            // 将这个骰子加入列表
                            Dices.Add(dice);
                            // 已展示的骰子数量 +1
                            mShowingDiceCount++;

                        }).Start(dice);
                    });
            }
        }
    }
}
