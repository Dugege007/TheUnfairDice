using UnityEngine;

public class CriticalHit : MonoBehaviour
{
    // ��ʼ������
    public double InitCritPercent = 0.1785274;
    // ��ǰ��������
    private double currentCritPercent;

    private double deltaCritPercent;

    private double dynamicCritPercent;

    // ��ǰ�ܹ�������
    private int attackTotalCount = 0;
    // ��ǰ�ܱ������Ĵ���
    private int critTotalCount = 0;
    // ����δ���ֱ����Ĵ���
    private int noCritStreakCount = 0;

    private void Start()
    {
        currentCritPercent = InitCritPercent;
        dynamicCritPercent = InitCritPercent;
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
            for (int i = 0; i < 100000; i++) PerformAttack();
        }
    }

    private void TestCount(int times)
    {
        for (int i = 0; i < times; i++) PerformAttack();
    }

    // ÿʮ�ι��� ����һ�α��� ��̬������
    private void PerformAttack()
    {
        attackTotalCount++;
        bool isCritical = false;

        if (attackTotalCount > 0)
        {
            // ���㵱ǰ�������� = �ܱ����� / �ܹ�����
            currentCritPercent = (double)critTotalCount / attackTotalCount;

            deltaCritPercent = InitCritPercent - currentCritPercent;

            dynamicCritPercent = (attackTotalCount * deltaCritPercent + currentCritPercent) * deltaCritPercent;
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

    private int GetOptimalN(float percent)
    {
        float oneMinusP = 1 - percent;

        for (int i = 0; i < 500; i++)
        {
            if (Mathf.Pow(oneMinusP, 2) <= 0.05f)
                return i;
        }

        return 500;
    }

    // ������ 0.2����������������
    private void PerformAttack2()
    {
        attackTotalCount++;
        bool isCritical = false;

        if (attackTotalCount > 0)
        {
            // ���㵱ǰ�������� = �ܱ����� / �ܹ�����
            currentCritPercent = (float)critTotalCount / attackTotalCount;
        }

        float percent = Random.Range(0f, 1f);
        if (percent < InitCritPercent)
            isCritical = true;
        else
            isCritical = false;

        if (isCritical) critTotalCount++;

        // ִ�й�������� isCritical Ϊ true����Ϊ����
        if (isCritical)
            Debug.Log("Critical Hit!");
        else
            Debug.Log("Normal Hit.");
    }

    // ÿʮ�ι��� ����һ�α���
    private void PerformAttack3()
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
