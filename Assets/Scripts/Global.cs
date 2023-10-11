using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Unity.VisualScripting;

namespace TheUnfairDice
{
    public class Global : Architecture<Global>
    {
        private static PlayerConfig mPlayerConfig;

        // ��������
        public static BindableProperty<float> HP = new(ConfigManager.Default.PlayerConfig.HP);
        public static BindableProperty<float> MaxHP = new(ConfigManager.Default.PlayerConfig.MaxHP);
        public static BindableProperty<float> Damage = new(ConfigManager.Default.PlayerConfig.Damage);
        public static BindableProperty<float> Speed = new(ConfigManager.Default.PlayerConfig.Speed);

        // ��������
        public static BindableProperty<int> Gold = new(ConfigManager.Default.PlayerConfig.InitGold);

        // ��Ϸ��
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
            Damage.Value = ConfigManager.Default.PlayerConfig.Damage;
            CurrentSec.Value = ConfigManager.Default.PlayerConfig.InitCurrentSec;
            Fortress.HP.Value = 20;
            Fortress.CurrentHumanCount.Value = 0;
            EnemyGenerator.TotalEnemyCount.Value = 0;
            EnemyGenerator.CurrentEnemyCount.Value = 0;

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
                // ���侭��ֵ
                PowerUpManager.Default.Exp.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }

            percent = Random.Range(0, 1f);
            if (percent <= GoldPercent.Value)
            {
                // ������
                PowerUpManager.Default.Gold.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }

            percent = Random.Range(0, 1f);
            if (percent <= HPPercent.Value && !Object.FindObjectOfType<HP>())
            {
                // ��������ֵ
                PowerUpManager.Default.HP.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }

            percent = Random.Range(0, 1f);
            if (percent <= GetAllExpPercent.Value && !Object.FindObjectOfType<GetAllExp>())
            {
                // �������վ���
                PowerUpManager.Default.GetAllExp.Instantiate()
                    .Position(generaterObj.Position())
                    .Show();

                return;
            }
        }
    }
}
