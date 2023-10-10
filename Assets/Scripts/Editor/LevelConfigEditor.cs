using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TheUnfairDice
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : Editor
    {
        // 可折叠的选项卡状态的字典
        private Dictionary<EnemyWave, bool> mEnemyEnhanceFoldoutStatus = new Dictionary<EnemyWave, bool>();

        public override void OnInspectorGUI()
        {
            // 绘制默认的Inspector
            DrawDefaultInspector();

            LevelConfig config = (LevelConfig)target;

            // 标题
            EditorGUILayout.LabelField("敌人波次配置", EditorStyles.boldLabel);

            if (config.WaveGroups.Count <= 1)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("提示：先完成一个波次组的制作，然后在右上角输入需要的波次组数量");
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("这样能以 1 波次组为模板快速创建");
                GUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("当前共有 " + config.WaveGroups.Count + " 个波次组", GUILayout.Width(120));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

            // 遍历EnemyWaveGroups列表
            for (int i = 0; i < config.WaveGroups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                // 绘制一个表示当前敌人波次组的标题，加粗
                EditorGUILayout.LabelField((i + 1) + " 波次组：", EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                // 绘制波次组标签
                EditorGUILayout.LabelField("组名：", GUILayout.Width(80));
                // 绘制波次组名称输入框
                config.WaveGroups[i].Name = EditorGUILayout.TextField(config.WaveGroups[i].Name);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                // 绘制波次组标签
                EditorGUILayout.LabelField("是否激活：", GUILayout.Width(80));
                // 绘制波次组名称输入框
                config.WaveGroups[i].IsActive = EditorGUILayout.Toggle(config.WaveGroups[i].IsActive);
                EditorGUILayout.EndHorizontal();

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                EditorGUI.indentLevel++;

                // 遍历Waves列表
                for (int j = 0; j < config.WaveGroups[i].Waves.Count; j++)
                {
                    // 加背景
                    GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
                    // 背景开始位置
                    GUILayout.BeginVertical(backgroundStyle);

                    EditorGUILayout.BeginHorizontal();
                    // 绘制波次标签
                    EditorGUILayout.LabelField((j + 1) + " " + config.WaveGroups[i].Waves[j].Name + "：", GUILayout.Width(100));
                    // 绘制波次名称输入框
                    config.WaveGroups[i].Waves[j].Name = EditorGUILayout.TextField(config.WaveGroups[i].Waves[j].Name);
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;

                    // 绘制波次的其他属性
                    config.WaveGroups[i].Waves[j].IsActive = EditorGUILayout.Toggle("是否激活：", config.WaveGroups[i].Waves[j].IsActive);

                    config.WaveGroups[i].Waves[j].EnemyObj = (GameObject)EditorGUILayout.ObjectField("敌人预制体：", config.WaveGroups[i].Waves[j].EnemyObj, typeof(GameObject), false);
                    config.WaveGroups[i].Waves[j].GenerateCDTime = EditorGUILayout.FloatField("生成冷却时间：", config.WaveGroups[i].Waves[j].GenerateCDTime);
                    config.WaveGroups[i].Waves[j].WaveDurationSec = EditorGUILayout.FloatField("波次持续时间：", config.WaveGroups[i].Waves[j].WaveDurationSec);

                    EnemyWave currentWave = config.WaveGroups[i].Waves[j];
                    // 初始化折叠状态
                    if (!mEnemyEnhanceFoldoutStatus.ContainsKey(currentWave))
                        mEnemyEnhanceFoldoutStatus[currentWave] = false;

                    mEnemyEnhanceFoldoutStatus[currentWave] =
                        EditorGUILayout.Foldout(mEnemyEnhanceFoldoutStatus[currentWave], "加强（1为正常）");
                    // 控制选项卡是否展开
                    if (mEnemyEnhanceFoldoutStatus[currentWave])
                    {
                        EditorGUI.indentLevel++;

                        // 当选项卡展开时，显示更多的编辑器字段
                        config.WaveGroups[i].Waves[j].HPScale = EditorGUILayout.FloatField("血量加强：", config.WaveGroups[i].Waves[j].HPScale);
                        config.WaveGroups[i].Waves[j].SpeedScale = EditorGUILayout.FloatField("速度加强：", config.WaveGroups[i].Waves[j].SpeedScale);

                        EditorGUI.indentLevel--;
                    }

                    // 添加和删除波次的按钮
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        Undo.RecordObject(config, "Insert New Wave");

                        EnemyWave newWave = new EnemyWave();
                        config.WaveGroups[i].Waves.Insert(j + 1, newWave);

                        EditorUtility.SetDirty(config);
                    }
                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        Undo.RecordObject(config, "Remove This Wave");

                        if (config.WaveGroups[i].Waves.Count > 0)
                            config.WaveGroups[i].Waves.RemoveAt(j);

                        EditorUtility.SetDirty(config);
                    }
                    EditorGUILayout.EndHorizontal();

                    // 背景结束位置
                    GUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(config.WaveGroups[i].Waves.Count + " 个波次", GUILayout.Width(80));
                GUILayout.EndHorizontal();

                if (config.WaveGroups[i].Waves.Count == 0)
                {
                    // 添加和删除波次的按钮，在最后显示
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("添加一个 波次", GUILayout.Width(120)))
                    {
                        Undo.RecordObject(config, "Add New Wave");

                        EnemyWave newWave = new EnemyWave();
                        config.WaveGroups[i].Waves.Add(newWave);

                        EditorUtility.SetDirty(config);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            }

            if (config.WaveGroups.Count > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("当前共有 " + config.WaveGroups.Count + " 个波次组", GUILayout.Width(120));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            // 添加和删除波次组的按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加一个 波次组"))
            {
                Undo.RecordObject(config, "Add New EnemyWaveGroup");
                EnemyWaveGroup newGroup = new EnemyWaveGroup();
                config.WaveGroups.Add(newGroup);
                EditorUtility.SetDirty(config);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("移除最后一个 波次组"))
            {
                Undo.RecordObject(config, "Remove Last EnemyWaveGroup");
                if (config.WaveGroups.Count > 0)
                    config.WaveGroups.RemoveAt(config.WaveGroups.Count - 1);
                EditorUtility.SetDirty(config);
            }
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(config);
            }
        }
    }
}
