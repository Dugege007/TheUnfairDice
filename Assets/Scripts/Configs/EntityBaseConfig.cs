using UnityEngine;

namespace TheUnfairDice
{
    [CreateAssetMenu(menuName = "Config/EntityBase Config")]
    public class EntityBaseConfig : ScriptableObject
    {
        [Header("名称")]
        public string Name;
        public string Key;

        [TextArea]
        [Header("说明")]
        public string Description = string.Empty;

        [Header("基本属性")]
        public float MaxHP = 3;
        public float HP = 3;
        public float Damage = 1;
        public float Speed = 0;
    }
}
