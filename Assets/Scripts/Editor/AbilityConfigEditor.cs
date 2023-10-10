using UnityEditor;
using UnityEngine;

namespace TheUnfairDice
{
    [CustomEditor(typeof(AbilityConfig))]
    public class AbilityConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // ����Ĭ�ϵ�Inspector
            DrawDefaultInspector();

            AbilityConfig config = (AbilityConfig)target;

            // ���һ����ť������������AbilityPowers
            if (GUILayout.Button("���±��"))
            {
                Undo.RecordObject(config, "Reorder Levels");

                for (int i = 0; i < config.Powers.Count; i++)
                {
                    config.Powers[i].Lv = (i + 1).ToString();
                }

                EditorUtility.SetDirty(config);
            }

            // ����
            EditorGUILayout.LabelField("������������ֵ��", EditorStyles.boldLabel);

            // ����Powers�б�
            for (int i = 0; i < config.Powers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                // ����һ����ʾ��ǰ�ȼ��ı��⣬�Ӵ�
                EditorGUILayout.LabelField("�ȼ� " + config.Powers[i].Lv, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < config.Powers[i].PowerDatas.Length; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    // ���ö�̬��ǩ����
                    EditorGUILayout.LabelField(
                        config.Powers[i].GetPowerTypeName(config.Powers[i].PowerDatas[j].Type) + "��", 
                        GUILayout.Width(60));
                    // ����PowerType�����˵��Ͷ�Ӧ����ֵ�������ͬһ��
                    config.Powers[i].PowerDatas[j].Type = (AbilityPower.PowerType)EditorGUILayout.EnumPopup(
                        config.Powers[i].PowerDatas[j].Type,
                        GUILayout.MinWidth(80));

                    // ����
                    GUILayout.FlexibleSpace();

                    Undo.RecordObject(config, "Input Value");
                    //EditorGUILayout.LabelField("ֵ " + (j + 1) + ":", GUILayout.Width(30));
                    config.Powers[i].PowerDatas[j].Value = EditorGUILayout.FloatField(config.Powers[i].PowerDatas[j].Value, GUILayout.MinWidth(60));
                    EditorUtility.SetDirty(config);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                // ����������ť��һ�� ��+�� �Ű�ť�������� PowerType��һ�� ��-�� �Ű�ť���ڼ��� PowerType��������ť���ҷ���
                // ���հ�����ʹ��ť������ʾ
                GUILayout.FlexibleSpace();

                // "+"��ť��������PowerType
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    Undo.RecordObject(config, "Add New PowerType");
                    config.Powers[i].AddNewPowerType();
                    EditorUtility.SetDirty(config);
                }

                // "-"��ť���ڼ���PowerType
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    Undo.RecordObject(config, "Remove Last PowerType");
                    config.Powers[i].RemoveLastPowerType();
                    EditorUtility.SetDirty(config);
                }
                EditorGUILayout.EndHorizontal();
            }

            // ���һ����ť��������µ�AbilityPower
            if (GUILayout.Button("��� AbilityPower"))
            {
                Undo.RecordObject(config, "Add New AbilityPower");
                AbilityPower newPower = new AbilityPower();
                newPower.Lv = (config.Powers.Count + 1).ToString();
                config.Powers.Add(newPower);
                EditorUtility.SetDirty(config);
            }

            // ���һ����ť�����Ƴ����һ��AbilityPower
            if (GUILayout.Button("�Ƴ����һ�� AbilityPower"))
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
