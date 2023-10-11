using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/Fortress Config")]
    public class FortressConfig : EntityBaseConfig
    {
        [Header("≥ı º÷µ")]
        public int InitHumanCount = 5;
        public int MaxHumanCount = 15;
        public int CurrentHumanCount = 0;

        public float GenerateSec = 3f;
        public float GenerateRadius = 3f;
    }
}
