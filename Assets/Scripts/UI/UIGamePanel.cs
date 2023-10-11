using UnityEngine;
using UnityEngine.UI;
using QFramework;
using QAssetBundle;

namespace TheUnfairDice
{
    public class UIGamePanelData : UIPanelData
    {
    }
    public partial class UIGamePanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIGamePanelData ?? new UIGamePanelData();
            // please add init code here

            // ��ʼʱ�ȹص�
            ExpUpgradePanel.Hide();
            Tips.Hide();

            Global.HP.RegisterWithInitValue(hp =>
            {
                PlayerHPText.text = "���HP��" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Exp.RegisterWithInitValue(exp =>
            {
                CurrentExpText.text = "��ǰ���飺" + exp + "/" + Global.ExpToNextLevel();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��������
            Global.Exp.RegisterWithInitValue(exp =>
            {
                if (exp >= Global.ExpToNextLevel())
                {
                    Global.Exp.Value -= Global.ExpToNextLevel();
                    Global.Level.Value++;
                    CurrentExpText.text = "��ǰ���飺" + exp + "/" + Global.ExpToNextLevel();
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Level.RegisterWithInitValue(lv =>
            {
                PlayerLevelText.text = "��ҵȼ���" + lv;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Level.Register(level =>
            {
                AudioKit.PlaySound(Sfx.LEVELUP);

                Time.timeScale = 0;
                ExpUpgradePanel.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.FortressHP.RegisterWithInitValue(hp =>
            {
                FortressrHPText.text = "Ҫ��HP��" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentHumanCount.RegisterWithInitValue(currentHumanCount =>
            {
                HumanCountText.text = "���ࣺ" + currentHumanCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentEnemyCount.RegisterWithInitValue(enemyCount =>
            {
                EnemyCountText.text = "���ˣ�" + enemyCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentSec.RegisterWithInitValue(curerntSec =>
            {
                // ÿ 10 ֡����һ��
                if (Time.frameCount % 10 == 0)
                {
                    int currentSecondsInt = Mathf.FloorToInt(curerntSec);
                    int seconds = currentSecondsInt % 60;
                    int minutes = currentSecondsInt / 60;

                    CurrentTimeText.text = "ʱ�䣺" + $"{minutes:00}:{seconds:00}";
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ע���ʱ������
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSec.Value += Time.deltaTime;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);


            // Tips
            Global.Dice1Point.RegisterWithInitValue(point =>
            {
                switch (point)
                {
                    case 1:
                        CreateTips("���� �������");
                        break;
                    case 2:
                        CreateTips("��� �������");
                        break;
                    case 3:
                        CreateTips("���� �������");
                        break;
                    case 4:
                        CreateTips("��� �������");
                        break;
                    case 5:
                        CreateTips("Ҫ�� �������");
                        break;
                    case 6:
                        CreateTips("��� ��������");
                        break;
                    default:
                        break;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Dice2Point.RegisterWithInitValue(point =>
            {
                switch (point)
                {
                    case 1:
                        CreateTips("���� �������");
                        break;
                    case 2:
                        CreateTips("��� �������");
                        break;
                    case 3:
                        CreateTips("���� �������");
                        break;
                    case 4:
                        CreateTips("��� �������");
                        break;
                    case 5:
                        CreateTips("Ҫ�� �������");
                        break;
                    case 6:
                        CreateTips("��� ��������");
                        break;
                    default:
                        break;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            //Global.Dice3Point.RegisterWithInitValue(point =>
            //{
            //    switch (point)
            //    {
            //        case 1:
            //            CreateTips("���� �������");
            //            break;
            //        case 2:
            //            CreateTips("��� �������");
            //            break;
            //        case 3:
            //            CreateTips("���� �������");
            //            break;
            //        case 4:
            //            CreateTips("��� �������");
            //            break;
            //        case 5:
            //            CreateTips("Ҫ�� �������");
            //            break;
            //        case 6:
            //            CreateTips("��� ��������");
            //            break;
            //        default:
            //            break;
            //    }

            //}).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.IsAll6.RegisterWithInitValue(isAll6 =>
            {
                if (isAll6)
                {
                    CreateTips("��ħ����");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            float lastHP = 20;

            Global.FortressHP.Register(hp =>
            {
                if (hp < lastHP)
                {
                    CreateTips("Ҫ�����ڱ�����");
                    lastHP = hp;
                }

                if (hp <= 3)
                {
                    CreateTips("Ҫ��Σ��");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HolyWaterUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                {
                    CreateTips("���ˮ֮��");
                    CreateTips("��������ʩ��");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HolyFireUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                {
                    CreateTips("��û�֮��");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HolySwordUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                {
                    CreateTips("��ý�֮��");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void CreateTips(string tipsText)
        {
            Tips.InstantiateWithParent(TipsRoot)
                .Self(self =>
                {
                    Text selfCache = self;
                    selfCache.text = tipsText;

                    // ��Ӷ���
                    ActionKit.Sequence()
                        // �𽥱��
                        .Lerp(0.5f, 0.9f, 0.5f, scale => selfCache.LocalScale(scale))
                        .Lerp(0.9f, 1f, 2.5f, scale => selfCache.LocalScale(scale))
                        .Parallel(p =>
                        {
                            // ��΢��С
                            p.Lerp(1f, 0f, 0.3f, scale => selfCache.LocalScale(scale));

                            float alpha = selfCache.GetComponent<Text>().color.a;
                            p.Append(ActionKit.Sequence()
                                // ��͸��
                                .Lerp(1, 0, 0.3f, a =>
                                {
                                    Color color = selfCache.GetComponent<Text>().color;
                                    color.a = a;
                                    selfCache.GetComponent<Text>().color = color;
                                }));
                        })
                        .Start(this, () =>
                        {
                            // ��������
                            selfCache.DestroyGameObjGracefully();
                        });
                })
                .Show();
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
