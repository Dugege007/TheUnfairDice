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

            // 开始时先关掉
            ExpUpgradePanel.Hide();
            Tips.Hide();

            Global.HP.RegisterWithInitValue(hp =>
            {
                PlayerHPText.text = "玩家HP：" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Exp.RegisterWithInitValue(exp =>
            {
                CurrentExpText.text = "当前经验：" + exp + "/" + Global.ExpToNextLevel();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 升级机制
            Global.Exp.RegisterWithInitValue(exp =>
            {
                if (exp >= Global.ExpToNextLevel())
                {
                    Global.Exp.Value -= Global.ExpToNextLevel();
                    Global.Level.Value++;
                    CurrentExpText.text = "当前经验：" + exp + "/" + Global.ExpToNextLevel();
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Level.RegisterWithInitValue(lv =>
            {
                PlayerLevelText.text = "玩家等级：" + lv;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Level.Register(level =>
            {
                AudioKit.PlaySound(Sfx.LEVELUP);

                Time.timeScale = 0;
                ExpUpgradePanel.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.FortressHP.RegisterWithInitValue(hp =>
            {
                FortressrHPText.text = "要塞HP：" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentHumanCount.RegisterWithInitValue(currentHumanCount =>
            {
                HumanCountText.text = "人类：" + currentHumanCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentEnemyCount.RegisterWithInitValue(enemyCount =>
            {
                EnemyCountText.text = "敌人：" + enemyCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentSec.RegisterWithInitValue(curerntSec =>
            {
                // 每 10 帧更新一次
                if (Time.frameCount % 10 == 0)
                {
                    int currentSecondsInt = Mathf.FloorToInt(curerntSec);
                    int seconds = currentSecondsInt % 60;
                    int minutes = currentSecondsInt / 60;

                    CurrentTimeText.text = "时间：" + $"{minutes:00}:{seconds:00}";
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 注册计时的任务
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
                        CreateTips("人类 获得升级");
                        break;
                    case 2:
                        CreateTips("玩家 获得提升");
                        break;
                    case 3:
                        CreateTips("人类 获得升级");
                        break;
                    case 4:
                        CreateTips("玩家 获得提升");
                        break;
                    case 5:
                        CreateTips("要塞 获得提升");
                        break;
                    case 6:
                        CreateTips("玩家 能力提升");
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
                        CreateTips("人类 获得升级");
                        break;
                    case 2:
                        CreateTips("玩家 获得提升");
                        break;
                    case 3:
                        CreateTips("人类 获得升级");
                        break;
                    case 4:
                        CreateTips("玩家 获得提升");
                        break;
                    case 5:
                        CreateTips("要塞 获得提升");
                        break;
                    case 6:
                        CreateTips("玩家 能力提升");
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
            //            CreateTips("人类 获得升级");
            //            break;
            //        case 2:
            //            CreateTips("玩家 获得提升");
            //            break;
            //        case 3:
            //            CreateTips("人类 获得升级");
            //            break;
            //        case 4:
            //            CreateTips("玩家 获得提升");
            //            break;
            //        case 5:
            //            CreateTips("要塞 获得提升");
            //            break;
            //        case 6:
            //            CreateTips("玩家 能力提升");
            //            break;
            //        default:
            //            break;
            //    }

            //}).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.IsAll6.RegisterWithInitValue(isAll6 =>
            {
                if (isAll6)
                {
                    CreateTips("恶魔出现");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            float lastHP = 20;

            Global.FortressHP.Register(hp =>
            {
                if (hp < lastHP)
                {
                    CreateTips("要塞正在被攻击");
                    lastHP = hp;
                }

                if (hp <= 3)
                {
                    CreateTips("要塞危险");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HolyWaterUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                {
                    CreateTips("获得水之力");
                    CreateTips("靠近敌人施放");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HolyFireUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                {
                    CreateTips("获得火之力");
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HolySwordUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                {
                    CreateTips("获得剑之力");
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

                    // 添加动画
                    ActionKit.Sequence()
                        // 逐渐变大
                        .Lerp(0.5f, 0.9f, 0.5f, scale => selfCache.LocalScale(scale))
                        .Lerp(0.9f, 1f, 2.5f, scale => selfCache.LocalScale(scale))
                        .Parallel(p =>
                        {
                            // 稍微变小
                            p.Lerp(1f, 0f, 0.3f, scale => selfCache.LocalScale(scale));

                            float alpha = selfCache.GetComponent<Text>().color.a;
                            p.Append(ActionKit.Sequence()
                                // 变透明
                                .Lerp(1, 0, 0.3f, a =>
                                {
                                    Color color = selfCache.GetComponent<Text>().color;
                                    color.a = a;
                                    selfCache.GetComponent<Text>().color = color;
                                }));
                        })
                        .Start(this, () =>
                        {
                            // 销毁自身
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
