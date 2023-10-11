using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class ConfigManager : ViewController
    {
        public static ConfigManager Default;

        [Header("要塞配置")]
        public EntityBaseConfig FortressConfig;

        [Header("玩家配置")]
        public PlayerConfig PlayerConfig;

        [Header("能力列表")]
        public AbilityConfig[] AbilityConfigs = new AbilityConfig[0];

        private void Awake()
        {
            // 单例模式，确保只有一个 ConfigManager 实例
            if (Default == null)
            {
                Default = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
