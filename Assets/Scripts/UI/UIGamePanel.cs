using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
    public class UIGamePanelData : UIPanelData
    {
    }
    public partial class UIGamePanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIGamePanelData ?? new UIGamePanelData();
            // please add init code here

            Global.HP.RegisterWithInitValue(hp =>
            {
                PlayerHPText.text = "玩家HP：" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Fortress.HP.RegisterWithInitValue(hp =>
            {
                FortressrHPText.text = "要塞HP：" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Fortress.CurrentHumanCount.RegisterWithInitValue(currentHumanCount =>
            {
                HumanCountText.text = "人类：" + currentHumanCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            EnemyGenerator.CurrentEnemyCount.RegisterWithInitValue(enemyCount =>
            {
                EnemyCountText.text = "敌人：" + enemyCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentSec.RegisterWithInitValue(curerntSec =>
            {
                // 每 10 帧更新一次
                if (Time.frameCount % 10 == 0)
                {
                    int currentSecondsInt = Mathf.FloorToInt(curerntSec);
                    int seconds = currentSecondsInt % 60;
                    int minutes = currentSecondsInt / 60;

                    CurrentTimeText.text = "时间：" + $"{minutes:00}:{seconds:00}";
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 注册计时的任务
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSec.Value += Time.deltaTime;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
