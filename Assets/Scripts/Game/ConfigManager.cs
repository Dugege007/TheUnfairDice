using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class ConfigManager : ViewController
    {
        public static ConfigManager Default;

        public PlayerConfig PlayerConfig;

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
