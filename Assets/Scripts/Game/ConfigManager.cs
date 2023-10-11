using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class ConfigManager : ViewController
    {
        public static ConfigManager Default;

        [Header("Ҫ������")]
        public EntityBaseConfig FortressConfig;

        [Header("�������")]
        public PlayerConfig PlayerConfig;

        [Header("�����б�")]
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
