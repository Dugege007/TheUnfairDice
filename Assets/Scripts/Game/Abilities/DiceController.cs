using UnityEngine;
using QFramework;
using System.Collections.Generic;
using QAssetBundle;
using System.Linq;

namespace TheUnfairDice
{
    public partial class DiceController : ViewController, IController
    {
        public static DiceController Default;

        public List<Dice> Dices = new List<Dice>();

        public Enemy EnemyBoss;

        public int DiceCount = 1;
        public bool IsRolling = false;
        public float RollingSec = 1f;
        public float ShowDicePointSec = 3f;

        private float mDicePosSpaceScale = 0.8f;
        private float mShowDicePointSecRemain = 3f;
        private int mShowingDiceCount = 0;


        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            // 第一个骰子
            Global.Dice1Point.RegisterWithInitValue(point =>
            {
                switch (point)
                {
                    case 1:
                        Human[] humans = FindObjectsByType<Human>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                        foreach (Human human in humans)
                        {
                            human.HP++;
                        }
                        break;

                    case 2:
                        Global.MaxHP.Value++;
                        break;

                    case 3:
                        humans = FindObjectsByType<Human>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                        foreach (Human human in humans)
                        {
                            human.WalkSpeed += 0.33f;
                        }
                        break;

                    case 4:
                        Global.Damage.Value++;
                        break;

                    case 5:
                        Global.FortressHP.Value++;
                        break;

                    case 6:
                        float percent = Random.Range(0, 1f);
                        if (percent <= 0.5f)
                        {
                            // 随机升级一个技能
                            this.GetSystem<ExpUpgradeSystem>().Items
                                .Where(item => item.IsWeapon && item != null)
                                .ToList()
                                .GetRandomItem()
                                .Upgrade();
                        }
                        break;

                    default:
                        break;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 第二个骰子
            Global.Dice1Point.RegisterWithInitValue(point =>
            {
                switch (point)
                {
                    case 1:
                        Human[] humans = FindObjectsByType<Human>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                        foreach (Human human in humans)
                        {
                            human.HP++;
                        }
                        break;

                    case 2:
                        Global.HP.Value++;
                        break;

                    case 3:
                        humans = FindObjectsByType<Human>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                        foreach (Human human in humans)
                        {
                            human.WalkSpeed += 0.33f;
                        }
                        break;

                    case 4:
                        Global.CriticalPercent.Value += 0.05f;
                        break;

                    case 5:
                        Global.FortressDamage.Value += 10;
                        break;

                    case 6:
                        float percent = Random.Range(0, 1f);
                        if (percent <= 0.5f)
                        {
                            // 随机升级一个技能
                            this.GetSystem<ExpUpgradeSystem>().Items
                                .Where(item => item.IsWeapon && item != null)
                                .ToList()
                                .GetRandomItem()
                                .Upgrade();
                        }
                        break;

                    default:
                        break;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 第三个骰子
            Global.Dice1Point.RegisterWithInitValue(point =>
            {
                switch (point)
                {
                    case 1:
                        Human[] humans = FindObjectsByType<Human>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                        foreach (Human human in humans)
                        {
                            human.HP++;
                        }
                        break;

                    case 2:
                        Global.CollectableRange.Value++;
                        break;

                    case 3:
                        humans = FindObjectsByType<Human>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                        foreach (Human human in humans)
                        {
                            human.WalkSpeed += 0.33f;
                        }
                        break;

                    case 4:
                        Global.CriticalPercent.Value *= 1.2f;
                        break;

                    case 5:
                        Global.FortressDamage.Value += 10;
                        break;

                    case 6:
                        float percent = Random.Range(0, 1f);
                        if (percent <= 0.5f)
                        {
                            // 随机升级一个技能
                            this.GetSystem<ExpUpgradeSystem>().Items
                                .Where(item => item.IsWeapon && item != null)
                                .ToList()
                                .GetRandomItem()
                                .Upgrade();
                        }
                        break;

                    default:
                        break;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            // 如果已展示骰子数量和拥有骰子数量相等
            if (mShowingDiceCount == DiceCount)
            {
                Global.Dice1Point.Value = Dices[0].Point;
                Global.Dice2Point.Value = Dices[1].Point;
                Global.Dice3Point.Value = Dices[2].Point;

                bool isAll6 = true;

                foreach (Dice dice in Dices)
                {
                    if (dice.Point != 6)
                    {
                        isAll6 = false;
                    }
                }

                if (isAll6)
                {
                    GenerateBoss();
                }

                ResetDice();
            }

            //if (Input.GetKeyDown(KeyCode.Space) && IsRolling == false)
            //{
            //    RollDice();
            //}
        }

        public void RollDice()
        {
            // 正在 Roll
            IsRolling = true;

            AudioKit.PlaySound(Sfx.DICEROLL);

            // 第一颗骰子位置 X 轴的偏移值
            float offsetX = (DiceCount % 2 == 0) ?
                            -(DiceCount / 2 - 0.5f) * mDicePosSpaceScale :
                            -(DiceCount / 2) * mDicePosSpaceScale;

            //float playerPosX = Player.Default.Position().x;
            //float playerPosY = Player.Default.Position().y;

            // 骰子需要偏移的向量
            Vector3 offsetV3 = Vector3.zero;

            // 按拥有骰子的数量 Roll
            for (int i = 0; i < DiceCount; i++)
            {
                offsetV3 = new Vector3(offsetX + i * mDicePosSpaceScale, 0, 0);

                // 生成骰子
                Dice.InstantiateWithParent(this)
                    .LocalPosition(offsetV3)
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

        private void ResetDice()
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

        private void GenerateBoss()
        {
            if (Player.Default)
            {
                Vector2 pos = Vector2.zero;

                float ldx = CameraController.LDTrans.position.x;    // 相机 左下点 X
                float ldy = CameraController.LDTrans.position.y;    // 相机 左下点 Y
                float rux = CameraController.RUTrans.position.x;    // 相机 右上点 X
                float ruy = CameraController.RUTrans.position.y;    // 相机 右上点 Y

                int xOry = RandomUtility.Choose(-1, 1);

                // 当生成点的位置距离要塞小于 8 时，需要重新计算
                while (true)
                {
                    if (xOry > 0)
                    {
                        // 左边或右边
                        pos.x = RandomUtility.Choose(ldx, rux);
                        pos.y = Random.Range(ldy, ruy);
                    }
                    else
                    {
                        // 上边或下边
                        pos.x = Random.Range(ldx, rux);
                        pos.y = RandomUtility.Choose(ldy, ruy);
                    }

                    // 距离要塞大于 8 时，跳出循环
                    if (Vector2.Distance(pos, Fortress.Default.Position()) > 8f) break;
                }

                EnemyBoss.Instantiate()
                    .Position(pos)
                    .Show();

                Global.TotalEnemyCount.Value++;
            }

        }

        private void OnDestroy()
        {
            Default = null;
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}
