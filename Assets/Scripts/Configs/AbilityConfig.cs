using System.Collections.Generic;
using UnityEngine;
using static TheUnfairDice.AbilityPower;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/AbilityConfig")]
    public class AbilityConfig : ScriptableObject
    {
        [Header("����")]
        public string Name;
        [Header("�Ƿ�Ϊ����")]
        public bool IsWeapon;

        [TextArea]
        [Header("˵��")]
        public string Description = string.Empty;

        [Header("��ʼֵ")]
        public float InitDamage = 0;
        public float InitCDTime = 0;
        public float InitDuration = 0;
        public float InitRange = 0;
        public float InitSpeed = 0;
        public int InitCount = 0;
        public int InitAttackCount = 0;

        [Header("����������˳��")]
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
            Damage,         // �˺�
            CDTime,         // ��ȴʱ��
            Duration,       // ����ʱ��
            Range,          // ��Χ
            Speed,          // �ٶ�
            Count,          // ����
            AttackCount,    // ������
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
                string powerTypeStr = "";

                switch (data.Type)
                {
                    case PowerType.Damage:
                        powerTypeStr = "������";
                        break;
                    case PowerType.CDTime:
                        powerTypeStr = "��ȴʱ��";
                        break;
                    case PowerType.Duration:
                        powerTypeStr = "����ʱ��";
                        break;
                    case PowerType.Range:
                        powerTypeStr = "��Χ";
                        break;
                    case PowerType.Speed:
                        powerTypeStr = "�ٶ�";
                        break;
                    case PowerType.Count:
                        powerTypeStr = "����";
                        break;
                    case PowerType.AttackCount:
                        powerTypeStr = "������";
                        break;
                }

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
    }
}
