using UnityEditor;
using UnityEngine;

namespace TheUnfairDice
{
    [CustomEditor(typeof(AbilityConfig))]
    public class AbilityConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // 绘制默认的Inspector
            DrawDefaultInspector();

            AbilityConfig config = (AbilityConfig)target;

            // 添加一个按钮用于重新排序AbilityPowers
            if (GUILayout.Button("重新编号"))
            {
                Undo.RecordObject(config, "Reorder Levels");

                for (int i = 0; i < config.Powers.Count; i++)
                {
                    config.Powers[i].Lv = (i + 1).ToString();
                }

                EditorUtility.SetDirty(config);
            }

            // 标题
            EditorGUILayout.LabelField("升级（调整数值）", EditorStyles.boldLabel);

            // 遍历Powers列表
            for (int i = 0; i < config.Powers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                // 绘制一个表示当前等级的标题，加粗
                EditorGUILayout.LabelField("等级 " + config.Powers[i].Lv, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < config.Powers[i].PowerDatas.Length; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    // 设置动态标签名称
                    EditorGUILayout.LabelField(
                        config.Powers[i].GetPowerTypeName(config.Powers[i].PowerDatas[j].Type) + "：", 
                        GUILayout.Width(60));
                    // 绘制PowerType下拉菜单和对应的数值输入框在同一行
                    config.Powers[i].PowerDatas[j].Type = (AbilityPower.PowerType)EditorGUILayout.EnumPopup(
                        config.Powers[i].PowerDatas[j].Type,
                        GUILayout.MinWidth(80));

                    // 留空
                    GUILayout.FlexibleSpace();

                    Undo.RecordObject(config, "Input Value");
                    //EditorGUILayout.LabelField("值 " + (j + 1) + ":", GUILayout.Width(30));
                    config.Powers[i].PowerDatas[j].Value = EditorGUILayout.FloatField(config.Powers[i].PowerDatas[j].Value, GUILayout.MinWidth(60));
                    EditorUtility.SetDirty(config);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                // 绘制两个按钮，一个 “+” 号按钮用于增加 PowerType；一个 “-” 号按钮用于减少 PowerType，两个按钮靠右放置
                // 填充空白区域，使按钮靠右显示
                GUILayout.FlexibleSpace();

                // "+"按钮用于增加PowerType
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    Undo.RecordObject(config, "Add New PowerType");
                    config.Powers[i].AddNewPowerType();
                    EditorUtility.SetDirty(config);
                }

                // "-"按钮用于减少PowerType
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    Undo.RecordObject(config, "Remove Last PowerType");
                    config.Powers[i].RemoveLastPowerType();
                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.EndHorizontal();
            }

            // 添加一个按钮用于添加新的AbilityPower
            if (GUILayout.Button("添加 AbilityPower"))
            {
                Undo.RecordObject(config, "Add New AbilityPower");
                AbilityPower newPower = new AbilityPower();
                newPower.Lv = (config.Powers.Count + 1).ToString();
                config.Powers.Add(newPower);
                EditorUtility.SetDirty(config);
            }

            // 添加一个按钮用于移除最后一个AbilityPower
            if (GUILayout.Button("移除最后一个 AbilityPower"))
            {
                Undo.RecordObject(config, "Remove Last AbilityPower");
                if (config.Powers.Count > 0)
                    config.Powers.RemoveAt(config.Powers.Count - 1);
                EditorUtility.SetDirty(config);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(config);
            }
        }
    }
}
