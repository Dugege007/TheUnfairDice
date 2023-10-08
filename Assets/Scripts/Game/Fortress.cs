using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Fortress : ViewController
    {
        public static Fortress Default;

        public BindableProperty<float> HP = new(20f);

        private float mCurrentGenerateSec = 0;
        public float GenerateSec = 3f;
        public float GenerateRadius = 3f;
        public int MaxHumanCount = 15;
        public int InitHumanCount = 5;
        public BindableProperty<int> CurrentHumanCount = new(0);

        private void Awake()
        {
            Default = this;
        }

        private void Start()
        {
            for (int i = 0; i < InitHumanCount; i++)
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
                        HP.Value--;
                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                        enemy.GetHurt(1);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (HP.Value <= 0 || CurrentHumanCount.Value <= 2)
            {
                Debug.Log("游戏结束");
            }

            if (CurrentHumanCount.Value >= MaxHumanCount) return;

            mCurrentGenerateSec += Time.deltaTime;

            if (mCurrentGenerateSec >= GenerateSec)
            {
                mCurrentGenerateSec = 0;

                GenerateHuman();
            }
        }

        private void GenerateHuman()
        {
            // 在 GenerateRadius 范围的边缘随机位置生成
            Vector2 randomPosition = new Vector2(this.Position().x, this.Position().y) + Random.insideUnitCircle.normalized * GenerateRadius;

            Human.InstantiateWithParent(this)
                .Position(new Vector3(randomPosition.x, randomPosition.y, 0))
                .Show();

            CurrentHumanCount.Value++;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
