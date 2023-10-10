using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public List<EnemyWaveGroup> WaveGroups = new List<EnemyWaveGroup>();
    }

    [System.Serializable]
    public class EnemyWaveGroup
    {
        public string Name;
        public bool IsActive = true;

        [TextArea]
        public string Description = string.Empty;

        public List<EnemyWave> Waves = new List<EnemyWave>();
    }

    [System.Serializable]
    public class EnemyWave
    {
        [Header("波次配置")]
        public string Name;
        public bool IsActive = true;

        [Header("敌人配置")]
        public GameObject EnemyPrefab;
        public float GenerateCDTime = 1f;
        public float WaveDurationSec = 10f;

        [Header("加强（1为正常）")]
        public float HPScale = 1.0f;
        public float SpeedScale = 1.0f;
    }
}
