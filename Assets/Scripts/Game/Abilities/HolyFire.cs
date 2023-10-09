using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace TheUnfairDice
{
    public partial class HolyFire : ViewController
    {
        public static BindableProperty<float> Damage = new(ConfigManager.Default.AbilityConfigs[1].InitDamage);
        public static BindableProperty<float> CDTime = new(ConfigManager.Default.AbilityConfigs[1].InitCDTime);
        public static BindableProperty<float> Duration = new(ConfigManager.Default.AbilityConfigs[1].InitDuration);
        public static BindableProperty<float> Range = new(ConfigManager.Default.AbilityConfigs[1].InitRange);  // 半径
        public static BindableProperty<float> Speed = new(ConfigManager.Default.AbilityConfigs[1].InitSpeed);
        public static BindableProperty<int> Count = new(ConfigManager.Default.AbilityConfigs[1].InitCount);
        public static BindableProperty<int> AttackCount = new(ConfigManager.Default.AbilityConfigs[1].InitAttackCount);

        private List<FireBall> mFireBalls = new List<FireBall>();

        private void Start()
        {
            FireBall.Hide();

            // 开始时创建一次球
            CreateBalls();
        }

        private void CreateBalls()
        {
            int ballCount2Create = Count.Value - mFireBalls.Count;

            for (int i = 0; i < ballCount2Create; i++)
            {
                mFireBalls.Add((FireBall)FireBall.Instantiate()
                    .SyncPosition2DFrom(this)
                    .Show());
            }
        }
    }
}
