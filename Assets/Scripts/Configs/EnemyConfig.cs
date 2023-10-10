using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/Enemy Config")]
    public class EnemyConfig : EntityBaseConfig
    {
        [Header("初始值")]
        public float InitExpPercent = 0;
        public float InitGoldPercent = 0;
        public float InitSpiritPercent = 0;

        [Header("是否攻击要塞")]
        public bool IsTargetFortress = false;
    }
}
