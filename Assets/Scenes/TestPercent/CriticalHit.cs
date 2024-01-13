using UnityEngine;

public class CriticalHit : MonoBehaviour
{
    // 初始暴击率
    public double InitCritPercent = 0.1785274;
    // 当前暴击概率
    private double currentCritPercent;

    private double deltaCritPercent;

    private double dynamicCritPercent;

    // 当前总攻击次数
    private int attackTotalCount = 0;
    // 当前总暴击过的次数
    private int critTotalCount = 0;
    // 连续未出现暴击的次数
    private int noCritStreakCount = 0;

    private void Start()
    {
        currentCritPercent = InitCritPercent;
        dynamicCritPercent = InitCritPercent;
    }

    private void Update()
    {
        // 监听鼠标左键输入
        if (Input.GetMouseButtonDown(0))
        {
            // 测试一次
            PerformAttack();
            Debug.Log("当前暴击率：" + currentCritPercent);
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

    // 每十次攻击 至少一次暴击 动态暴击率
    private void PerformAttack()
    {
        attackTotalCount++;
        bool isCritical = false;

        if (attackTotalCount > 0)
        {
            // 计算当前暴击概率 = 总暴击数 / 总攻击数
            currentCritPercent = (double)critTotalCount / attackTotalCount;

            deltaCritPercent = InitCritPercent - currentCritPercent;

            dynamicCritPercent = (attackTotalCount * deltaCritPercent + currentCritPercent) * deltaCritPercent;
        }

        // 检查是否需要强制暴击
        if (noCritStreakCount < 9)
        {
            float percent = Random.Range(0f, 1f);
            if (percent < InitCritPercent)
            {
                isCritical = true;
                noCritStreakCount = 0; // 重置计数器
            }
            else
            {
                noCritStreakCount++;
            }
        }
        else
        {
            isCritical = true;
            noCritStreakCount = 0; // 重置计数器
        }

        if (isCritical) critTotalCount++;

        // 执行攻击，如果 isCritical 为 true，则为暴击
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

    // 暴击率 0.2，无其他限制条件
    private void PerformAttack2()
    {
        attackTotalCount++;
        bool isCritical = false;

        if (attackTotalCount > 0)
        {
            // 计算当前暴击概率 = 总暴击数 / 总攻击数
            currentCritPercent = (float)critTotalCount / attackTotalCount;
        }

        float percent = Random.Range(0f, 1f);
        if (percent < InitCritPercent)
            isCritical = true;
        else
            isCritical = false;

        if (isCritical) critTotalCount++;

        // 执行攻击，如果 isCritical 为 true，则为暴击
        if (isCritical)
            Debug.Log("Critical Hit!");
        else
            Debug.Log("Normal Hit.");
    }

    // 每十次攻击 至少一次暴击
    private void PerformAttack3()
    {
        attackTotalCount++;
        bool isCritical = false;

        if (attackTotalCount > 0)
        {
            // 计算当前暴击概率 = 总暴击数 / 总攻击数
            currentCritPercent = (float)critTotalCount / attackTotalCount;
        }

        // 检查是否需要强制暴击
        if (noCritStreakCount < 9)
        {
            float percent = Random.Range(0f, 1f);
            if (percent < InitCritPercent)
            {
                isCritical = true;
                noCritStreakCount = 0; // 重置计数器
            }
            else
            {
                noCritStreakCount++;
            }
        }
        else
        {
            isCritical = true;
            noCritStreakCount = 0; // 重置计数器
        }

        if (isCritical) critTotalCount++;

        // 执行攻击，如果 isCritical 为 true，则为暴击
        if (isCritical)
            Debug.Log("Critical Hit!");
        else
            Debug.Log("Normal Hit.");
    }
}
