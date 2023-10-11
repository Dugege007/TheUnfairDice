using System.Collections.Generic;
using UnityEngine;
using static TheUnfairDice.AbilityPower;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/Ability Config")]
    public class AbilityConfig : ScriptableObject
    {
        [Header("名称")]
        public string Name;
        public string Key;
        [Header("是否为武器")]
        public bool IsWeapon;

        [TextArea]
        [Header("说明")]
        public string Description = string.Empty;

        [Header("初始值")]
        public float InitDamage = 0;
        public float InitCDTime = 0;
        public float InitDuration = 0;
        public float InitRange = 0;
        public float InitSpeed = 0;
        public int InitCount = 0;
        public int InitAttackCount = 0;

        public List<AbilityPower> Powers = new List<AbilityPower>();
    }

    [System.Serializable]
    public struct PowerData
    {
        public PowerType Type;
        public float Value;
    }

    [System.Serializable]
    public class AbilityPower
    {
        public enum PowerType
        {
            Damage,         // 伤害
            CDTime,         // 冷却时间
            Duration,       // 持续时间
            Range,          // 范围
            Speed,          // 速度
            Count,          // 数量
            AttackCount,    // 攻击数
        }

        public string Lv;
        public PowerData[] PowerDatas = new PowerData[2];

        public void AddNewPowerType()
        {
            List<PowerData> powerDataList = new List<PowerData>(PowerDatas)
            {
                new PowerData { Type = PowerType.Damage, Value = 0 }
            };

            PowerDatas = powerDataList.ToArray();
        }

        public void RemoveLastPowerType()
        {
            if (PowerDatas.Length > 0)
            {
                List<PowerData> powerDataList = new List<PowerData>(PowerDatas);
                powerDataList.RemoveAt(powerDataList.Count - 1);
                PowerDatas = powerDataList.ToArray();
            }
        }

        public string GetPowerUpInfo(string name)
        {
            string info = $"{name} Lv{Lv}\n";

            foreach (var data in PowerDatas)
            {
                string powerTypeStr = GetPowerTypeName(data.Type);

                switch (data.Type)
                {
                    case PowerType.Damage:
                    case PowerType.CDTime:
                    case PowerType.Duration:
                    case PowerType.Range:
                    case PowerType.Speed:
                    case PowerType.Count:
                    case PowerType.AttackCount:
                        info += $"{powerTypeStr}+{data.Value} ";
                        break;
                    default:
                        break;
                }
            }

            return info.Trim();
        }

        public string GetPowerTypeName(PowerType powerType)
        {
            switch (powerType)
            {
                case PowerType.Damage:
                    return "攻击力";
                case PowerType.CDTime:
                    return "冷却时间";
                case PowerType.Duration:
                    return "持续时间";
                case PowerType.Range:
                    return "范围";
                case PowerType.Speed:
                    return "速度";
                case PowerType.Count:
                    return "数量";
                case PowerType.AttackCount:
                    return "攻击数";
                default:
                    return "";
            }
        }
    }
}
