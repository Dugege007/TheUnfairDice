using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class EnemyGenerator : ViewController
    {
        private float mCurrentGenerateSec = 0;
        public float MaxEnemyCount = 50;
        public float EnemyCount = 0;

        private void Update()
        {
            if (EnemyCount >= MaxEnemyCount) return;

            mCurrentGenerateSec += Time.deltaTime;

            if (mCurrentGenerateSec >= 1)
            {
                mCurrentGenerateSec = 0;

                if (Player.mDefault)
                {
                    EnemyCount++;

                    Vector2 pos = Vector2.zero;

                    float ldx = CameraController.LDTrans.position.x;
                    float ldy = CameraController.LDTrans.position.y;
                    float rux = CameraController.RUTrans.position.x;
                    float ruy = CameraController.RUTrans.position.y;

                    int xOry = RandomUtility.Choose(-1, 1);
                    if (xOry > 0)
                    {
                        pos.x = RandomUtility.Choose(ldx, rux);
                        pos.y = Random.Range(ldy, ruy);
                    }
                    else
                    {
                        pos.x = Random.Range(ldx, rux);
                        pos.y = RandomUtility.Choose(ldy, ruy);
                    }

                    Enemy.InstantiateWithParent(this)
                        .Position(pos)
                        .Show();
                }
            }
        }
    }
}
