using UnityEngine;

namespace Test
{
    public class CriticalHit : MonoBehaviour
    {
        // 初始暴击率
        public float InitCritPercent = 0.2f;
        // 动态暴击率
        private float dynamicCritPercent;
        // 当前暴击概率
        private float currentCritPercent;
        // 修正幅度
        private float deltaCritPercent = 0;

        // 当前总攻击次数
        private int attackTotalCount = 0;
        // 当前总暴击过的次数
        private int critTotalCount = 0;
        // 连续未出现暴击的次数
        private int noCritStreakCount = 0;

        private void Start()
        {
            dynamicCritPercent = InitCritPercent;
            currentCritPercent = InitCritPercent;
        }

        private void Update()
        {
            // 监听鼠标左键输入
            if (Input.GetMouseButtonDown(0))
            {
                // 测试一次
                PerformAttack2();

                Debug.Log("当前暴击率：" + currentCritPercent);
                Debug.Log("修正幅度：" + deltaCritPercent);
                Debug.Log("动态暴击率：" + dynamicCritPercent);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 测试一万次
                for (int i = 0; i < 10000; i++) PerformAttack2();
            }
        }

        private void PerformAttack2()
        {
            attackTotalCount++;
            bool isCritical = false;

            if (attackTotalCount > 0)
            {
                // 计算当前暴击概率 = 总暴击数 / 总攻击数
                currentCritPercent = (float)critTotalCount / attackTotalCount;
                // 需要修正的幅度 = 初始设定的暴击率 - 当前暴击概率
                //deltaCritPercent = InitCritPercent - currentCritPercent;
                // 动态暴击率 += 修正幅度
                dynamicCritPercent = 2 * InitCritPercent - currentCritPercent;
                //dynamicCritPercent = Mathf.Clamp(dynamicCritPercent, 0.1f, 0.3f);

                // 举个例子：现在总攻击数是 100，有 24 次暴击，那么“当前暴击概率”为 24%，“修正幅度”为 -4%
                // 那么“动态暴击率”就应该等于 上次的“动态暴击率” - 4%
            }

            // 检查是否需要强制暴击
            if (noCritStreakCount < 9)
            {
                float percent = Random.Range(0f, 1f);
                if (percent < dynamicCritPercent)
                {
                    isCritical = true;
                    Debug.Log("已连续未暴击：" + noCritStreakCount + "次");
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
                Debug.Log("已连续未暴击：" + noCritStreakCount + "次");
                noCritStreakCount = 0; // 重置计数器
            }

            if (isCritical) critTotalCount++;

            // 执行攻击，如果 isCritical 为 true，则为暴击
            if (isCritical)
                Debug.Log("Critical Hit!");
            else
                Debug.Log("Normal Hit.");
        }

        private void PerformAttack1()
        {
            attackTotalCount++;
            bool isCritical = false;

            // 检查是否需要强制暴击
            if (noCritStreakCount < 9)
            {
                float percent = Random.Range(0f, 1f);
                if (percent <= dynamicCritPercent)
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
}
