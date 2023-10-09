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
            // �����չʾ����������ӵ�������������
            if (mShowingDiceCount == DiceCount)
            {
                // ��ʼ����ʱ
                mShowDicePointSecRemain -= Time.deltaTime;

                // �������ʱ����
                if (mShowDicePointSecRemain < 0)
                {
                    // ɾ����������
                    foreach (Dice dice in Dices)
                    {
                        dice.DestroyGameObjGracefully();
                    }

                    // ����б�
                    Dices.Clear();

                    // ��������
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
            // ���� Roll
            IsRolling = true;

            float playerPosX = Player.Default.Position().x;
            float playerPosY = Player.Default.Position().y;

            // ��һ������λ�� X ���ƫ��ֵ
            float offsetX = (DiceCount % 2 == 0) ?
                            -(DiceCount / 2 - 0.5f) * mDicePosSpaceScale :
                            -(DiceCount / 2) * mDicePosSpaceScale;

            // ������Ҫƫ�Ƶ�����
            Vector3 offsetV3 = Vector3.zero;

            // ��ӵ�����ӵ����� Roll
            for (int i = 0; i < DiceCount; i++)
            {
                offsetV3 = new Vector3(playerPosX + offsetX + i * mDicePosSpaceScale, playerPosY + 1, 0);

                // ��������
                Dice.Instantiate()
                    .Position(Player.Default.Position())
                    .Show()
                    .Self(self =>
                    {
                        // ����
                        SpriteRenderer selfCache = self;
                        Dice dice = selfCache.GetComponent<Dice>();
                        // ��������ʱ��
                        float randomTime = Random.Range(RollingSec, RollingSec + 1f);

                        //// ��̫�üӵĶ���
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

                        // ��ʱ����
                        ActionKit.Delay(randomTime, () =>
                        {
                            // ȡ�������
                            int randomIndex = Random.Range(0, dice.DiceSprites.Length);
                            // �ض���
                            dice.SelfAnimator.enabled = false;
                            // �� Sprite
                            selfCache.sprite = dice.DiceSprites[randomIndex];
                            // �õ�������ӵĵ���
                            dice.Point = randomIndex + 1;

                            // ��������Ӽ����б�
                            Dices.Add(dice);
                            // ��չʾ���������� +1
                            mShowingDiceCount++;

                        }).Start(dice);
                    });
            }
        }
    }
}
