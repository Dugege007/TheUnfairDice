using System.Collections.Generic;
using UnityEngine;

public class ChangePercent : MonoBehaviour
{
    // 例子 骰子六面
    public string[] DiceFaces = new string[]
    { "Face1", "Face2", "Face3", "Face4", "Face5", "Face6" };

    // 样本点集合 Sample Point List
    public List<string> SamplePointList = new List<string>();
    // 样本空间 Sample Space
    private Dictionary<string, float> SampleSpaceDic = new Dictionary<string, float>();

    private void Start()
    {
        ResetWeight();

        for (int i = 0; i < SampleSpaceDic.Count; i++)
        {
            Debug.Log("事件：" + DiceFaces[i] + "；" + "概率：" + SampleSpaceDic[DiceFaces[i]]);
        }
    }

    // 按目标概率调整权重
    private void AdjustWeightByTargetPercent(string outcome, float targetPercent)
    {
        float totalWeight = GetTotalWeight(SampleSpaceDic);
        float currentWeight = SampleSpaceDic[outcome];
        float weightAdjustment = (targetPercent * totalWeight - currentWeight) / 1 - targetPercent;

        SampleSpaceDic[outcome] = currentWeight + weightAdjustment;
    }
    // 等此算法确定后，可以将样本空间封装成一个类，方便进行各种操作

    // 按倍率调整权重
    private void AdjustWeightByRate(string outcome, float rate)
    {
        float currentWeight = SampleSpaceDic[outcome];

        SampleSpaceDic[outcome] = currentWeight * rate;
    }

    // 重置权重
    private void ResetWeight()
    {
        SampleSpaceDic.Clear();

        for (int i = 0; i < DiceFaces.Length; i++)
        {
            SampleSpaceDic.Add(DiceFaces[i], 1f);
        }
    }

    // 获取总权重
    private float GetTotalWeight(Dictionary<string, float> sampleSpaceDic)
    {
        float totalWeight = 0f;
        foreach (float weight in sampleSpaceDic.Values)
        {
            totalWeight += weight;
        }

        return totalWeight;
    }

    private void AddCases()
    {

    }
}
