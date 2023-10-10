using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TheUnfairDice
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : Editor
    {
        // ���۵���ѡ�״̬���ֵ�
        private Dictionary<EnemyWave, bool> mEnemyEnhanceFoldoutStatus = new Dictionary<EnemyWave, bool>();

        public override void OnInspectorGUI()
        {
            // ����Ĭ�ϵ�Inspector
            DrawDefaultInspector();

            LevelConfig config = (LevelConfig)target;

            // ����
            EditorGUILayout.LabelField("���˲�������", EditorStyles.boldLabel);

            if (config.WaveGroups.Count <= 1)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("��ʾ�������һ���������������Ȼ�������Ͻ�������Ҫ�Ĳ���������");
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("�������� 1 ������Ϊģ����ٴ���");
                GUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("��ǰ���� " + config.WaveGroups.Count + " ��������", GUILayout.Width(120));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

            // ����EnemyWaveGroups�б�
            for (int i = 0; i < config.WaveGroups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                // ����һ����ʾ��ǰ���˲�����ı��⣬�Ӵ�
                EditorGUILayout.LabelField((i + 1) + " �����飺", EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                // ���Ʋ������ǩ
                EditorGUILayout.LabelField("������", GUILayout.Width(80));
                // ���Ʋ��������������
                config.WaveGroups[i].Name = EditorGUILayout.TextField(config.WaveGroups[i].Name);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                // ���Ʋ������ǩ
                EditorGUILayout.LabelField("�Ƿ񼤻", GUILayout.Width(80));
                // ���Ʋ��������������
                config.WaveGroups[i].IsActive = EditorGUILayout.Toggle(config.WaveGroups[i].IsActive);
                EditorGUILayout.EndHorizontal();

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                EditorGUI.indentLevel++;

                // ����Waves�б�
                for (int j = 0; j < config.WaveGroups[i].Waves.Count; j++)
                {
                    // �ӱ���
                    GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
                    // ������ʼλ��
                    GUILayout.BeginVertical(backgroundStyle);

                    EditorGUILayout.BeginHorizontal();
                    // ���Ʋ��α�ǩ
                    EditorGUILayout.LabelField((j + 1) + " " + config.WaveGroups[i].Waves[j].Name + "��", GUILayout.Width(100));
                    // ���Ʋ������������
                    config.WaveGroups[i].Waves[j].Name = EditorGUILayout.TextField(config.WaveGroups[i].Waves[j].Name);
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;

                    // ���Ʋ��ε���������
                    config.WaveGroups[i].Waves[j].IsActive = EditorGUILayout.Toggle("�Ƿ񼤻", config.WaveGroups[i].Waves[j].IsActive);

                    config.WaveGroups[i].Waves[j].EnemyObj = (GameObject)EditorGUILayout.ObjectField("����Ԥ���壺", config.WaveGroups[i].Waves[j].EnemyObj, typeof(GameObject), false);
                    config.WaveGroups[i].Waves[j].GenerateCDTime = EditorGUILayout.FloatField("������ȴʱ�䣺", config.WaveGroups[i].Waves[j].GenerateCDTime);
                    config.WaveGroups[i].Waves[j].WaveDurationSec = EditorGUILayout.FloatField("���γ���ʱ�䣺", config.WaveGroups[i].Waves[j].WaveDurationSec);

                    EnemyWave currentWave = config.WaveGroups[i].Waves[j];
                    // ��ʼ���۵�״̬
                    if (!mEnemyEnhanceFoldoutStatus.ContainsKey(currentWave))
                        mEnemyEnhanceFoldoutStatus[currentWave] = false;

                    mEnemyEnhanceFoldoutStatus[currentWave] =
                        EditorGUILayout.Foldout(mEnemyEnhanceFoldoutStatus[currentWave], "��ǿ��1Ϊ������");
                    // ����ѡ��Ƿ�չ��
                    if (mEnemyEnhanceFoldoutStatus[currentWave])
                    {
                        EditorGUI.indentLevel++;

                        // ��ѡ�չ��ʱ����ʾ����ı༭���ֶ�
                        config.WaveGroups[i].Waves[j].HPScale = EditorGUILayout.FloatField("Ѫ����ǿ��", config.WaveGroups[i].Waves[j].HPScale);
                        config.WaveGroups[i].Waves[j].SpeedScale = EditorGUILayout.FloatField("�ٶȼ�ǿ��", config.WaveGroups[i].Waves[j].SpeedScale);

                        EditorGUI.indentLevel--;
                    }

                    // ��Ӻ�ɾ�����εİ�ť
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

                    // ��������λ��
                    GUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(config.WaveGroups[i].Waves.Count + " ������", GUILayout.Width(80));
                GUILayout.EndHorizontal();

                if (config.WaveGroups[i].Waves.Count == 0)
                {
                    // ��Ӻ�ɾ�����εİ�ť���������ʾ
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("���һ�� ����", GUILayout.Width(120)))
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
                EditorGUILayout.LabelField("��ǰ���� " + config.WaveGroups.Count + " ��������", GUILayout.Width(120));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            // ��Ӻ�ɾ��������İ�ť
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("���һ�� ������"))
            {
                Undo.RecordObject(config, "Add New EnemyWaveGroup");
                EnemyWaveGroup newGroup = new EnemyWaveGroup();
                config.WaveGroups.Add(newGroup);
                EditorUtility.SetDirty(config);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("�Ƴ����һ�� ������"))
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
