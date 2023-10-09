using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public class Global : Architecture<Global>
    {
        public static BindableProperty<float> HP = new(3);
        public static BindableProperty<float> MaxHP = new(3);

        public static BindableProperty<float> CurrentSec = new(0);

        protected override void Init()
        {

        }

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // ��ʼ����Դ
            ResKit.Init();
            // ����֡��
            Application.targetFrameRate = 60;
            // ���� UI ����ֱ���
            UIKit.Root.SetResolution(1920, 1080, 0.5f);

            // ������ʼ��
            IArchitecture _ = Interface;
        }

        public static void ResetData()
        {
            HP.Value = 3;
            MaxHP.Value = 3;
            CurrentSec.Value = 0;
            Fortress.HP.Value = 20;
            Fortress.CurrentHumanCount.Value = 0;
            EnemyGenerator.TotalEnemyCount.Value = 0;
            EnemyGenerator.CurrentEnemyCount.Value = 0;
        }
    }
}
