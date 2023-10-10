using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/Enemy Config")]
    public class EnemyConfig : EntityBaseConfig
    {
        [Header("��ʼֵ")]
        public float InitExpPercent = 0;
        public float InitGoldPercent = 0;
        public float InitSpiritPercent = 0;

        [Header("�Ƿ񹥻�Ҫ��")]
        public bool IsTargetFortress = false;
    }
}
