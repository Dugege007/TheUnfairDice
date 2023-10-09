using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace TheUnfairDice
{
    public partial class HolyFire : ViewController
    {
        public float HolyFireRange = 0.5f;  // �뾶
        public BindableProperty<int> HolyFireCount = new(0);

        private List<FireBall> mFireBalls = new List<FireBall>();

        private void Start()
        {
            FireBall.Hide();

            // ��ʼʱ����һ����
            CreateBalls();
        }

        private void CreateBalls()
        {
            int ballCount2Create = HolyFireCount.Value - mFireBalls.Count;

            for (int i = 0; i < ballCount2Create; i++)
            {
                mFireBalls.Add((FireBall)FireBall.Instantiate()
                    .SyncPosition2DFrom(this)
                    .Show());
            }
        }
    }
}
