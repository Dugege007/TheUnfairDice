using UnityEngine;

public class CriticalHit : MonoBehaviour
{
    // ��ʼ������
    public float InitCritPercent = 0.2f;
    // ��ǰ��������
    private float currentCritPercent;

    // ��ǰ�ܹ�������
    private int attackTotalCount = 0;
    // ��ǰ�ܱ������Ĵ���
    private int critTotalCount = 0;
    // ����δ���ֱ����Ĵ���
    private int noCritStreakCount = 0;

    private void Start()
    {
        currentCritPercent = InitCritPercent;
    }

    private void Update()
    {
        // ��������������
        if (Input.GetMouseButtonDown(0))
        {
            // ����һ��
            PerformAttack();
            Debug.Log("��ǰ�����ʣ�" + currentCritPercent);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ����һ���
            for (int i = 0; i < 10000; i++) PerformAttack();
        }
    }

    private void PerformAttack()
    {
        attackTotalCount++;
        bool isCritical = false;

        if (attackTotalCount > 0)
        {
            // ���㵱ǰ�������� = �ܱ����� / �ܹ�����
            currentCritPercent = (float)critTotalCount / attackTotalCount;
        }

        // ����Ƿ���Ҫǿ�Ʊ���
        if (noCritStreakCount < 9)
        {
            float percent = Random.Range(0f, 1f);
            if (percent < InitCritPercent)
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
