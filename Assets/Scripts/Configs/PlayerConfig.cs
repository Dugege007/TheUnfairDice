using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/Player Config")]
    public class PlayerConfig : EntityBaseConfig
    {
        [Header("��ʼֵ")]
        public int InitGold = 0;
        public int InitExp = 0;
        public int InitSpirit = 0;
        public float InitCollectableRange = 0;
        public float InitExpPercent = 0;
        public float InitGoldPercent = 0;
        public float InitSpiritPercent = 0;
        public float InitCurrentSec = 0;
    }
}
