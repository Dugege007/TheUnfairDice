using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public class Global : Architecture<Global>
    {
        private static PlayerConfig mPlayerConfig;

        // ��������
        public static BindableProperty<float> HP = new(ConfigManager.Default.PlayerConfig.HP);
        public static BindableProperty<float> MaxHP = new(ConfigManager.Default.PlayerConfig.MaxHP);
        public static BindableProperty<float> Speed = new(ConfigManager.Default.PlayerConfig.Speed);

        // ��������
        public static BindableProperty<int> Gold = new(ConfigManager.Default.PlayerConfig.InitGold);

        // ��Ϸ��
        public static BindableProperty<int> Level = new(ConfigManager.Default.PlayerConfig.Level);
        public static BindableProperty<int> Exp = new(ConfigManager.Default.PlayerConfig.InitExp);
        public static BindableProperty<int> Spirit = new(ConfigManager.Default.PlayerConfig.InitSpirit);
        public static BindableProperty<float> CollectableRange = new(ConfigManager.Default.PlayerConfig.InitCollectableRange);
        public static BindableProperty<float> ExpPercent = new(ConfigManager.Default.PlayerConfig.InitExpPercent);
        public static BindableProperty<float> GoldPercent = new(ConfigManager.Default.PlayerConfig.InitGoldPercent);
        public static BindableProperty<float> SpiritPercent = new(ConfigManager.Default.PlayerConfig.InitSpiritPercent);
        public static BindableProperty<float> CurrentSec = new(ConfigManager.Default.PlayerConfig.InitCurrentSec);

        #region �������
        // ˮ
        public static BindableProperty<bool> HolyWaterUnlocked = new(false);
        public static BindableProperty<float> HolyWaterDamage = new(ConfigManager.Default.AbilityConfigs[0].InitDamage);
        public static BindableProperty<float> HolyWaterDuration = new(ConfigManager.Default.AbilityConfigs[0].InitDuration);
        public static BindableProperty<float> HolyWaterCDTime = new(ConfigManager.Default.AbilityConfigs[0].InitCDTime);
        public static BindableProperty<float> HolyWaterRange = new(ConfigManager.Default.AbilityConfigs[0].InitRange);  // �뾶

        // ��
        public static BindableProperty<bool> HolyFireUnlocked = new(false);
        public static BindableProperty<float> HolyFireDamage = new(ConfigManager.Default.AbilityConfigs[1].InitDamage);
        public static BindableProperty<float> HolyFireCDTime = new(ConfigManager.Default.AbilityConfigs[1].InitCDTime);
        public static BindableProperty<float> HolyFireDuration = new(ConfigManager.Default.AbilityConfigs[1].InitDuration);
        public static BindableProperty<float> HolyFireRange = new(ConfigManager.Default.AbilityConfigs[1].InitRange);  // �뾶
        public static BindableProperty<float> HolyFireSpeed = new(ConfigManager.Default.AbilityConfigs[1].InitSpeed);
        public static BindableProperty<int> HolyFireCount = new(ConfigManager.Default.AbilityConfigs[1].InitCount);
        public static BindableProperty<int> HolyFireAttackCount = new(ConfigManager.Default.AbilityConfigs[1].InitAttackCount);

        // ��
        public static BindableProperty<bool> HolyTreeUnlocked = new(false);

        // ��
        public static BindableProperty<bool> HolySwordUnlocked = new(false);

        // ��
        public static BindableProperty<bool> HolyLandUnlocked = new(false);

        // ��
        public static BindableProperty<bool> HolyLightUnlocked = new(false);

        #endregion

        protected override void Init()
        {
            // ��Ҫ���ǽ�ϵͳע��
            this.RegisterSystem(new ExpUpgradeSystem());
        }

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // ��ʼ����Դ
            ResKit.Init();
            // ����֡��
            Application.targetFrameRate = 60;
            // ���� UI ����ֱ���
            UIKit.Root.SetResolution(1920, 1080, 0.5f);

            // ������ʼ��
            IArchitecture _ = Interface;
        }

        public static void ResetData()
        {
            HP.Value = ConfigManager.Default.PlayerConfig.HP;
            MaxHP.Value = ConfigManager.Default.PlayerConfig.MaxHP;
            CurrentSec.Value = ConfigManager.Default.PlayerConfig.InitCurrentSec;
            Fortress.HP.Value = 20;
            Fortress.CurrentHumanCount.Value = 0;
            EnemyGenerator.TotalEnemyCount.Value = 0;
            EnemyGenerator.CurrentEnemyCount.Value = 0;
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
                // ���侭��ֵ
                PowerUpManager.Default.Exp.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }
        }
    }
}
