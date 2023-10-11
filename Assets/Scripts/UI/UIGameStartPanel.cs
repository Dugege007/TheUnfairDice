using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;
using QAssetBundle;

namespace TheUnfairDice
{
    public class UIGameStartPanelData : UIPanelData
    {
    }
    public partial class UIGameStartPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIGameStartPanelData ?? new UIGameStartPanelData();
            // please add init code here

            Time.timeScale = 1.0f;

            // 开始游戏
            StartGameBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                UIKit.ClosePanel(this);
                Global.ResetData();
                SceneManager.LoadScene("Game");
            });

            // 退出游戏
            QuitBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                UIKit.ClosePanel(this);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif               
            });
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
