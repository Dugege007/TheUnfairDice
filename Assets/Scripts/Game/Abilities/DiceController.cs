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
            // ��һ������
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
                            // �������һ������
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

            // �ڶ�������
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
                            // �������һ������
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

            // ����������
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
                            // �������һ������
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
            // �����չʾ����������ӵ�������������
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
            // ���� Roll
            IsRolling = true;

            AudioKit.PlaySound(Sfx.DICEROLL);

            // ��һ������λ�� X ���ƫ��ֵ
            float offsetX = (DiceCount % 2 == 0) ?
                            -(DiceCount / 2 - 0.5f) * mDicePosSpaceScale :
                            -(DiceCount / 2) * mDicePosSpaceScale;

            //float playerPosX = Player.Default.Position().x;
            //float playerPosY = Player.Default.Position().y;

            // ������Ҫƫ�Ƶ�����
            Vector3 offsetV3 = Vector3.zero;

            // ��ӵ�����ӵ����� Roll
            for (int i = 0; i < DiceCount; i++)
            {
                offsetV3 = new Vector3(offsetX + i * mDicePosSpaceScale, 0, 0);

                // ��������
                Dice.InstantiateWithParent(this)
                    .LocalPosition(offsetV3)
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

        private void ResetDice()
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

        private void GenerateBoss()
        {
            if (Player.Default)
            {
                Vector2 pos = Vector2.zero;

                float ldx = CameraController.LDTrans.position.x;    // ��� ���µ� X
                float ldy = CameraController.LDTrans.position.y;    // ��� ���µ� Y
                float rux = CameraController.RUTrans.position.x;    // ��� ���ϵ� X
                float ruy = CameraController.RUTrans.position.y;    // ��� ���ϵ� Y

                int xOry = RandomUtility.Choose(-1, 1);

                // �����ɵ��λ�þ���Ҫ��С�� 8 ʱ����Ҫ���¼���
                while (true)
                {
                    if (xOry > 0)
                    {
                        // ��߻��ұ�
                        pos.x = RandomUtility.Choose(ldx, rux);
                        pos.y = Random.Range(ldy, ruy);
                    }
                    else
                    {
                        // �ϱ߻��±�
                        pos.x = Random.Range(ldx, rux);
                        pos.y = RandomUtility.Choose(ldy, ruy);
                    }

                    // ����Ҫ������ 8 ʱ������ѭ��
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
