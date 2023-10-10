using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace TheUnfairDice
{
    public partial class EnemyGenerator : ViewController
    {
        public LevelConfig LevelConfig;

        private float mCurrentGenerateSec = 0;
        public static BindableProperty<int> TotalEnemyCount = new(0);
        public static BindableProperty<int> CurrentEnemyCount = new(0);

        private float mCurrentWaveSec = 0;
        private Queue<EnemyWave> mEnemyWavesQueue = new Queue<EnemyWave>();

        public int WaveCount = 0;
        private int mTotalWaveCount = 0;
        public bool IsLastWave => WaveCount == mTotalWaveCount;

        private EnemyWave mCurrentWave = null;
        public EnemyWave CurrentWave => mCurrentWave;

        private bool mAttackFortress = false;

        private void Start()
        {
            foreach (var group in LevelConfig.WaveGroups)
            {
                foreach (var wave in group.Waves)
                {
                    mEnemyWavesQueue.Enqueue(wave);
                    mTotalWaveCount++;
                }
            }

            CurrentEnemyCount.Register(enemyCount =>
            {
                if (enemyCount >= 20)
                    mAttackFortress = true;
                else
                    mAttackFortress = false;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (mCurrentWave == null)
            {
                if (mEnemyWavesQueue.Count > 0)
                {
                    WaveCount++;
                    mCurrentWave = mEnemyWavesQueue.Dequeue();

                    mCurrentGenerateSec = 0;
                    mCurrentWaveSec = 0;
                }
            }

            if (mCurrentWave != null)
            {
                mCurrentGenerateSec += Time.deltaTime;
                mCurrentWaveSec += Time.deltaTime;

                if (mCurrentGenerateSec > mCurrentWave.GenerateCDTime)
                {
                    mCurrentGenerateSec = 0;

                    if (Player.Default)
                    {
                        Vector2 pos = Vector2.zero;

                        float ldx = CameraController.LDTrans.position.x;    // 相机 左下点 X
                        float ldy = CameraController.LDTrans.position.y;    // 相机 左下点 Y
                        float rux = CameraController.RUTrans.position.x;    // 相机 右上点 X
                        float ruy = CameraController.RUTrans.position.y;    // 相机 右上点 Y

                        int xOry = RandomUtility.Choose(-1, 1);

                        // 当生成点的位置距离要塞小于 8 时，需要重新计算
                        while (true)
                        {
                            if (xOry > 0)
                            {
                                // 左边或右边
                                pos.x = RandomUtility.Choose(ldx, rux);
                                pos.y = Random.Range(ldy, ruy);
                            }
                            else
                            {
                                // 上边或下边
                                pos.x = Random.Range(ldx, rux);
                                pos.y = RandomUtility.Choose(ldy, ruy);
                            }

                            // 距离要塞大于 8 时，跳出循环
                            if (Vector2.Distance(pos, Fortress.Default.Position()) > 8f) break;
                        }

                        mCurrentWave.EnemyPrefab.InstantiateWithParent(this)
                            .Position(pos)
                            .Self(self =>
                            {
                                if (mAttackFortress)
                                    self.GetComponent<Enemy>().IsTargetFortress = true;
                            })
                            .Show();

                        TotalEnemyCount.Value++;
                    }
                }

                if (mCurrentWaveSec > mCurrentWave.WaveDurationSec)
                {
                    mCurrentWave = null;
                }
            }
        }
    }
}
