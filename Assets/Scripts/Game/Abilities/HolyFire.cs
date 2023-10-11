using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace TheUnfairDice
{
    public partial class HolyFire : ViewController
    {
        private List<FireBall> mFireBalls = new List<FireBall>();

        private void Start()
        {
            FireBall.Hide();

            // 开始时创建一次球
            CreateBalls();
        }

        private void CreateBalls()
        {
            int ballCount2Create = Global.HolyFireCount.Value - mFireBalls.Count;
            float range = Global.HolyFireRange.Value * 0.4f;

            for (int i = 0; i < ballCount2Create; i++)
            {
                mFireBalls.Add((FireBall)FireBall.Instantiate()
                    .SyncPosition2DFrom(this)
                    .Show()
                    .LocalScale(range));
            }
        }
    }
}
