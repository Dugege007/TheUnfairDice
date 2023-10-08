using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class EnemyGenerator : ViewController
    {
        private float mCurrentGenerateSec = 0;
        public float GenerateSec = 1f;
        public int MaxEnemyCount = 50;
        public static BindableProperty<int> EnemyCount = new(0);

        private void Update()
        {
            if (EnemyCount.Value >= MaxEnemyCount) return;

            mCurrentGenerateSec += Time.deltaTime;

            if (mCurrentGenerateSec >= GenerateSec)
            {
                mCurrentGenerateSec = 0;

                if (Player.Default)
                {
                    Vector2 pos = Vector2.zero;

                    float ldx = CameraController.LDTrans.position.x;    // ��� ���µ� X
                    float ldy = CameraController.LDTrans.position.y;    // ��� ���µ� Y
                    float rux = CameraController.RUTrans.position.x;    // ��� ���ϵ� X
                    float ruy = CameraController.RUTrans.position.y;    // ��� ���ϵ� Y

                    int xOry = RandomUtility.Choose(-1, 1);
                    if (xOry > 0)
                    {
                        // ��߻��ұ�
                        pos.x = RandomUtility.Choose(ldx, rux);
                        pos.y = Random.Range(ldy, ruy);
                    }
                    else
                    {
                        // �ϱ߻��±�
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
