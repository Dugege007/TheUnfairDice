using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Dice : ViewController
    {
        public Sprite[] DiceSprites;
        public int Point;

        private void Start()
        {
            // �������ö�������ʼ֡�������
            float randomStartPoint = Random.Range(0f, 1f);

            // ��ʼ���Ŷ��������������ʼ�㿪ʼ
            SelfAnimator.Play("DiceRoll", -1, randomStartPoint);
        }
    }
}
