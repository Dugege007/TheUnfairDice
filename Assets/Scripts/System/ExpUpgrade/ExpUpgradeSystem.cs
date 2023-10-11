using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;

namespace TheUnfairDice
{
    public class ExpUpgradeSystem : AbstractSystem
    {
        public static EasyEvent OnCoinUpgradeSystemChanged = new EasyEvent();

        public List<ExpUpgradeItem> Items { get; } = new();

        private AbilityConfig[] mAbilityConfigs;

        protected override void OnInit()
        {
            mAbilityConfigs = ConfigManager.Default.AbilityConfigs;

            ResetData();

            Global.Level.Register(_ => Roll());
        }

        public ExpUpgradeItem Add(ExpUpgradeItem item)
        {
            Items.Add(item);
            return item;
        }

        public void ResetData()
        {
            Items.Clear();

            // 0 水
            AddNewExpUpgradeItem(mAbilityConfigs[0]);
            // 1 火
            AddNewExpUpgradeItem(mAbilityConfigs[1]);
            //// 2 树
            //AddNewExpUpgradeItem(mAbilityConfigs[2]);
            // 3 剑
            AddNewExpUpgradeItem(mAbilityConfigs[3]);
            //// 4 土
            //AddNewExpUpgradeItem(mAbilityConfigs[4]);
            //// 5 光
            //AddNewExpUpgradeItem(mAbilityConfigs[5]);
        }

        private void AddNewExpUpgradeItem(AbilityConfig abilityConfig)
        {
            Add(new ExpUpgradeItem()
                .WithKey(abilityConfig.Key)
                .WithName(abilityConfig.Name)
                .WithIsWeapon(abilityConfig.IsWeapon)
                .WithDescription(lv =>
                {
                    return UpgradeDiscription(lv, abilityConfig);
                })
                .WithMaxLevel(abilityConfig.Powers.Count)
                .OnUpgrade((_, lv) =>
                {
                    UpgradePowerValue(lv, abilityConfig);
                }));
        }

        private string UpgradeDiscription(int lv, AbilityConfig abilityConfig)
        {
            if (lv == 1 && abilityConfig.IsWeapon)
            {
                return $"{abilityConfig.Name} Lv1" + "\n" + abilityConfig.Description;
            }

            for (int i = 1; i < abilityConfig.Powers.Count + 1; i++)
            {
                if (lv == i)
                    return abilityConfig.Powers[lv - 1].GetPowerUpInfo(abilityConfig.Name);
            }

            return "未知等级";   // 不加这行会报错
        }

        private void UpgradePowerValue(int lv, AbilityConfig abilityConfig)
        {
            // 解锁能力
            if (lv == 1)
            {
                if (abilityConfig.Name == mAbilityConfigs[0].Name)
                    Global.HolyWaterUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[1].Name)
                    Global.HolyFireUnlocked.Value = true;
                //else if (abilityConfig.Name == mAbilityConfigs[2].Name)
                //    Global.HolyTreeUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[3].Name)
                    Global.HolySwordUnlocked.Value = true;
                //else if (abilityConfig.Name == mAbilityConfigs[4].Name)
                //    Global.HolyLandUnlocked.Value = true;
                //else if (abilityConfig.Name == mAbilityConfigs[5].Name)
                //    Global.HolyLightUnlocked.Value = true;
            }

            // 升级能力
            for (int i = 1; i < abilityConfig.Powers.Count + 1; i++)
            {
                if (lv == i)
                {
                    Debug.Log("当前升级：" + abilityConfig.Name + abilityConfig.Powers[lv - 1].Lv);

                    foreach (PowerData powerData in abilityConfig.Powers[lv - 1].PowerDatas)
                    {
                        // 0 水
                        if (abilityConfig.Name == mAbilityConfigs[0].Name)
                        {
                            switch (powerData.Type)
                            {
                                case AbilityPower.PowerType.Damage:
                                    Global.HolyWaterDamage.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.CDTime:
                                    Global.HolyWaterCDTime.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Duration:
                                    Global.HolyWaterDuration.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Range:
                                    Global.HolyWaterRange.Value += powerData.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        // 1 火
                        if (abilityConfig.Name == mAbilityConfigs[1].Name)
                        {
                            switch (powerData.Type)
                            {
                                case AbilityPower.PowerType.Damage:
                                    Global.HolyFireDamage.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.CDTime:
                                    Global.HolyFireCDTime.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Duration:
                                    Global.HolyFireDuration.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Range:
                                    Global.HolyFireRange.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Speed:
                                    Global.HolyFireSpeed.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Count:
                                    Global.HolyFireCount.Value += (int)powerData.Value;
                                    break;
                                case AbilityPower.PowerType.AttackCount:
                                    Global.HolyFireAttackCount.Value += (int)powerData.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        // 3 剑
                        if (abilityConfig.Name == mAbilityConfigs[3].Name)
                        {
                            switch (powerData.Type)
                            {
                                case AbilityPower.PowerType.Damage:
                                    Global.HolyFireDamage.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.CDTime:
                                    Global.HolyFireCDTime.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Count:
                                    Global.HolyFireCount.Value += (int)powerData.Value;
                                    break;
                                case AbilityPower.PowerType.AttackCount:
                                    Global.HolyFireAttackCount.Value += (int)powerData.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        Debug.Log($"升级{powerData.Type} " + powerData.Value);
                    }
                }
            }
        }

        public void Roll()
        {
            if (Items.Count >= 0)
            {
                foreach (ExpUpgradeItem expUpgradeItem in Items)
                {
                    expUpgradeItem.Visible.Value = false;
                }
            }

            // 随机取几个可升级的能力
            List<ExpUpgradeItem> list = Items.Where(item => !item.UpgradeFinish).ToList();
            if (list.Count >= 2)
            {
                list.GetAndRemoveRandomItem().Visible.Value = true;
                list.GetAndRemoveRandomItem().Visible.Value = true;
            }
            else if (list.Count > 0)
            {
                foreach (ExpUpgradeItem item in list)
                {
                    item.Visible.Value = true;
                }
            }
            else
            {
                Debug.Log("没有可用的升级项");
                return;
            }
        }
    }
}
