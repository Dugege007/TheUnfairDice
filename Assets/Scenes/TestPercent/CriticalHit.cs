using UnityEngine;

namespace Test
{
    public class CriticalHit : MonoBehaviour
    {
        // ��ʼ������
        public float InitCritPercent = 0.2f;
        // ��̬������
        private float dynamicCritPercent;
        // ��ǰ��������
        private float currentCritPercent;
        // ��������
        private float deltaCritPercent = 0;

        // ��ǰ�ܹ�������
        private int attackTotalCount = 0;
        // ��ǰ�ܱ������Ĵ���
        private int critTotalCount = 0;
        // ����δ���ֱ����Ĵ���
        private int noCritStreakCount = 0;

        private void Start()
        {
            dynamicCritPercent = InitCritPercent;
            currentCritPercent = InitCritPercent;
        }

        private void Update()
        {
            // ��������������
            if (Input.GetMouseButtonDown(0))
            {
                // ����һ��
                PerformAttack2();

                Debug.Log("��ǰ�����ʣ�" + currentCritPercent);
                Debug.Log("�������ȣ�" + deltaCritPercent);
                Debug.Log("��̬�����ʣ�" + dynamicCritPercent);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ����һ���
                for (int i = 0; i < 10000; i++) PerformAttack2();
            }
        }

        private void PerformAttack2()
        {
            attackTotalCount++;
            bool isCritical = false;

            if (attackTotalCount > 0)
            {
                // ���㵱ǰ�������� = �ܱ����� / �ܹ�����
                currentCritPercent = (float)critTotalCount / attackTotalCount;
                // ��Ҫ�����ķ��� = ��ʼ�趨�ı����� - ��ǰ��������
                //deltaCritPercent = InitCritPercent - currentCritPercent;
                // ��̬������ += ��������
                dynamicCritPercent = 2 * InitCritPercent - currentCritPercent;
                //dynamicCritPercent = Mathf.Clamp(dynamicCritPercent, 0.1f, 0.3f);

                // �ٸ����ӣ������ܹ������� 100���� 24 �α�������ô����ǰ�������ʡ�Ϊ 24%�����������ȡ�Ϊ -4%
                // ��ô����̬�����ʡ���Ӧ�õ��� �ϴεġ���̬�����ʡ� - 4%
            }

            // ����Ƿ���Ҫǿ�Ʊ���
            if (noCritStreakCount < 9)
            {
                float percent = Random.Range(0f, 1f);
                if (percent < dynamicCritPercent)
                {
                    isCritical = true;
                    Debug.Log("������δ������" + noCritStreakCount + "��");
                    noCritStreakCount = 0; // ���ü�����
                }
                else
                {
                    noCritStreakCount++;
                }
            }
            else
            {
                isCritical = true;
                Debug.Log("������δ������" + noCritStreakCount + "��");
                noCritStreakCount = 0; // ���ü�����
            }

            if (isCritical) critTotalCount++;

            // ִ�й�������� isCritical Ϊ true����Ϊ����
            if (isCritical)
                Debug.Log("Critical Hit!");
            else
                Debug.Log("Normal Hit.");
        }

        private void PerformAttack1()
        {
            attackTotalCount++;
            bool isCritical = false;

            // ����Ƿ���Ҫǿ�Ʊ���
            if (noCritStreakCount < 9)
            {
                float percent = Random.Range(0f, 1f);
                if (percent <= dynamicCritPercent)
                {
                    isCritical = true;
                    noCritStreakCount = 0; // ���ü�����
                }
                else
                {
                    noCritStreakCount++;
                }
            }
            else
            {
                isCritical = true;
                noCritStreakCount = 0; // ���ü�����
            }

            if (isCritical) critTotalCount++;

            // ִ�й�������� isCritical Ϊ true����Ϊ����
            if (isCritical)
                Debug.Log("Critical Hit!");
            else
                Debug.Log("Normal Hit.");
        }
    }
}
