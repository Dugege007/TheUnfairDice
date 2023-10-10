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
            // ����ģʽ��ȷ��ֻ��һ�� ConfigManager ʵ��
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
