using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Fortress : ViewController
    {
        private float mCurrentGenerateSec = 0;
        public float GenerateSec = 3f;
        public float GenerateRadius = 3f;
        public int MaxHumanCount = 15;
        public int InitHumanCount = 5;
        public int CurrentHumanCount = 0;

        private void Start()
        {
            for (int i = 0;i<InitHumanCount;i++)
            {
                GenerateHuman();
            }
        }

        private void Update()
        {
            if (CurrentHumanCount >= MaxHumanCount) return;

            mCurrentGenerateSec += Time.deltaTime;

            if (mCurrentGenerateSec >= GenerateSec)
            {
                mCurrentGenerateSec = 0;

                GenerateHuman();
            }
        }

        private void GenerateHuman()
        {
            // �� GenerateRadius ��Χ�ı�Ե���λ������
            Vector2 randomPosition = new Vector2(this.Position().x, this.Position().y) + Random.insideUnitCircle.normalized * GenerateRadius;

            Human.InstantiateWithParent(this)
                .Position(new Vector3(randomPosition.x, randomPosition.y, 0))
                .Show();

            CurrentHumanCount++;
        }
    }
}
