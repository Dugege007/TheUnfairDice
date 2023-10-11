using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Fortress : ViewController
    {
        public static Fortress Default;
        private FortressConfig mFortressConfig;

        private int mInitHumanCount;
        private int mMaxHumanCount;
        private float mGenerateRadius;

        private float mCurrentGenerateSec = 0;

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            mFortressConfig = ConfigManager.Default.FortressConfig;
            mInitHumanCount = mFortressConfig.InitHumanCount;
            mMaxHumanCount = mFortressConfig.MaxHumanCount;
            mGenerateRadius = mFortressConfig.GenerateRadius;

            for (int i = 0; i < mInitHumanCount; i++)
            {
                GenerateHuman();
            }

            // 为自己的 HitHurtBox 注册碰撞事件
            HitHurtBox.OnTriggerEnter2DEvent(collider2D =>
            {
                HitHurtBox hurtBox = collider2D.GetComponent<HitHurtBox>();

                if (hurtBox != null)
                {
                    if (hurtBox.Owner.CompareTag("Enemy"))
                    {
                        Global.FortressHP.Value--;
                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                        enemy.GetHurt(1);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (Global.FortressHP.Value <= 0 || Global.CurrentHumanCount.Value <= 2)
            {
                UIKit.OpenPanel<UIGameOverPanel>();
            }

            if (Global.CurrentHumanCount.Value >= mMaxHumanCount) return;

            mCurrentGenerateSec += Time.deltaTime;

            if (mCurrentGenerateSec >= mFortressConfig.GenerateSec)
            {
                mCurrentGenerateSec = 0;

                GenerateHuman();
            }
        }

        private void GenerateHuman()
        {
            // 在 GenerateRadius 范围的边缘随机位置生成
            Vector2 randomPosition = new Vector2(this.Position().x, this.Position().y) + Random.insideUnitCircle.normalized * mGenerateRadius;

            Human.InstantiateWithParent(this)
                .Position(new Vector3(randomPosition.x, randomPosition.y, 0))
                .Show();

            Global.CurrentHumanCount.Value++;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
