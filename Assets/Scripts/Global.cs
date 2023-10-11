using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Unity.VisualScripting;
using UnityEngine.Rendering;

namespace TheUnfairDice
{
    public class Global : Architecture<Global>
    {
        private static PlayerConfig mPlayerConfig;

        // 要塞属性
        public static BindableProperty<float> FortressHP = new(ConfigManager.Default.FortressConfig.HP);
        public static BindableProperty<float> FortressDamage = new(ConfigManager.Default.FortressConfig.Damage);
        public static BindableProperty<float> CurrentHumanCount = new(ConfigManager.Default.FortressConfig.CurrentHumanCount);

        // 玩家属性
        public static BindableProperty<float> HP = new(ConfigManager.Default.PlayerConfig.HP);
        public static BindableProperty<float> MaxHP = new(ConfigManager.Default.PlayerConfig.MaxHP);
        public static BindableProperty<float> Damage = new(ConfigManager.Default.PlayerConfig.Damage);
        public static BindableProperty<float> Speed = new(ConfigManager.Default.PlayerConfig.Speed);
        public static BindableProperty<float> CriticalPercent = new(ConfigManager.Default.PlayerConfig.InitCriticalPercent);
        public static BindableProperty<int> RollCount = new(0);
        public static BindableProperty<int> DiceCount = new(0);

        // 升级道具
        public static BindableProperty<int> Gold = new(ConfigManager.Default.PlayerConfig.InitGold);

        // 游戏中
        public static BindableProperty<int> Level = new(ConfigManager.Default.PlayerConfig.Level);
        public static BindableProperty<int> Exp = new(ConfigManager.Default.PlayerConfig.InitExp);
        public static BindableProperty<int> Spirit = new(ConfigManager.Default.PlayerConfig.InitSpirit);
        public static BindableProperty<float> CollectableRange = new(ConfigManager.Default.PlayerConfig.InitCollectableRange);
        public static BindableProperty<float> ExpPercent = new(ConfigManager.Default.PlayerConfig.InitExpPercent);
        public static BindableProperty<float> HPPercent = new(ConfigManager.Default.PlayerConfig.InitHPPercent);
        public static BindableProperty<float> GoldPercent = new(ConfigManager.Default.PlayerConfig.InitGoldPercent);
        public static BindableProperty<float> SpiritPercent = new(ConfigManager.Default.PlayerConfig.InitSpiritPercent);
        public static BindableProperty<float> GetAllExpPercent = new(ConfigManager.Default.PlayerConfig.InitGetAllExpPercent);
        public static BindableProperty<float> CurrentSec = new(ConfigManager.Default.PlayerConfig.InitCurrentSec);

        // 击杀数据
        public static BindableProperty<int> TotalEnemyCount = new(0);
        public static BindableProperty<int> CurrentEnemyCount = new(0);

        #region 玩家能力
        // 0 水
        public static BindableProperty<bool> HolyWaterUnlocked = new(false);
        public static BindableProperty<float> HolyWaterDamage = new(ConfigManager.Default.AbilityConfigs[0].InitDamage);
        public static BindableProperty<float> HolyWaterDuration = new(ConfigManager.Default.AbilityConfigs[0].InitDuration);
        public static BindableProperty<float> HolyWaterCDTime = new(ConfigManager.Default.AbilityConfigs[0].InitCDTime);
        public static BindableProperty<float> HolyWaterRange = new(ConfigManager.Default.AbilityConfigs[0].InitRange);  // 半径

        // 1 火
        public static BindableProperty<bool> HolyFireUnlocked = new(false);
        public static BindableProperty<float> HolyFireDamage = new(ConfigManager.Default.AbilityConfigs[1].InitDamage);
        public static BindableProperty<float> HolyFireCDTime = new(ConfigManager.Default.AbilityConfigs[1].InitCDTime);
        public static BindableProperty<float> HolyFireDuration = new(ConfigManager.Default.AbilityConfigs[1].InitDuration);
        public static BindableProperty<float> HolyFireRange = new(ConfigManager.Default.AbilityConfigs[1].InitRange);  // 半径
        public static BindableProperty<float> HolyFireSpeed = new(ConfigManager.Default.AbilityConfigs[1].InitSpeed);
        public static BindableProperty<int> HolyFireCount = new(ConfigManager.Default.AbilityConfigs[1].InitCount);
        public static BindableProperty<int> HolyFireAttackCount = new(ConfigManager.Default.AbilityConfigs[1].InitAttackCount);

        // 2 树
        public static BindableProperty<bool> HolyTreeUnlocked = new(false);

        // 3 剑
        public static BindableProperty<bool> HolySwordUnlocked = new(false);
        public static BindableProperty<float> HolySwordDamage = new(ConfigManager.Default.AbilityConfigs[3].InitDamage);
        public static BindableProperty<float> HolySwordCDTime = new(ConfigManager.Default.AbilityConfigs[3].InitCDTime);
        public static BindableProperty<int> HolySwordCount = new(ConfigManager.Default.AbilityConfigs[3].InitCount);
        public static BindableProperty<int> HolySwordAttackCount = new(ConfigManager.Default.AbilityConfigs[3].InitAttackCount);

        // 4 土
        public static BindableProperty<bool> HolyLandUnlocked = new(false);

        // 5 光
        public static BindableProperty<bool> HolyLightUnlocked = new(false);

        #endregion

        protected override void Init()
        {
            // 不要忘记将系统注册
            this.RegisterSystem(new ExpUpgradeSystem());
        }

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // 设置音频播放模式为：相同音频在 10 帧内不重复播放
            AudioKit.PlaySoundMode = AudioKit.PlaySoundModes.IgnoreSameSoundInGlobalFrames;
            // 初始化资源
            ResKit.Init();
            // 设置帧率
            Application.targetFrameRate = 60;
            // 设置 UI 适配分辨率
            UIKit.Root.SetResolution(1920, 1080, 0.5f);

            // 主动初始化
            IArchitecture _ = Interface;
        }

        public static void ResetData()
        {
            // 要塞属性
            FortressHP.Value = ConfigManager.Default.FortressConfig.HP;
            FortressDamage.Value = ConfigManager.Default.FortressConfig.Damage;

            // 玩家属性
            HP.Value = ConfigManager.Default.PlayerConfig.HP;
            MaxHP.Value = ConfigManager.Default.PlayerConfig.MaxHP;
            Damage.Value = ConfigManager.Default.PlayerConfig.Damage;
            CurrentSec.Value = ConfigManager.Default.PlayerConfig.InitCurrentSec;

            // 击杀数据
            CurrentEnemyCount.Value = 0;

            Interface.GetSystem<ExpUpgradeSystem>().ResetData();
        }

        public static int ExpToNextLevel()
        {
            return Level.Value * 5;
        }

        public static void GeneratePowerUp(GameObject generaterObj)
        {
            float percent = Random.Range(0, 1f);
            if (percent <= ExpPercent.Value)
            {
                // 掉落经验值
                PowerUpManager.Default.Exp.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }

            percent = Random.Range(0, 1f);
            if (percent <= GoldPercent.Value)
            {
                // 掉落金币
                PowerUpManager.Default.Gold.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }

            percent = Random.Range(0, 1f);
            if (percent <= HPPercent.Value && !Object.FindObjectOfType<HP>())
            {
                // 掉落生命值
                PowerUpManager.Default.HP.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }

            percent = Random.Range(0, 1f);
            if (percent <= GetAllExpPercent.Value && !Object.FindObjectOfType<GetAllExp>())
            {
                // 掉落吸收经验
                PowerUpManager.Default.GetAllExp.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }
        }
    }
}
