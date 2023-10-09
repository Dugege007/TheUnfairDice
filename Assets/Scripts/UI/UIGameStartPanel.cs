using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

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

            StartGameBtn.onClick.AddListener(() =>
            {
                UIKit.ClosePanel(this);
                Global.ResetData();
                SceneManager.LoadScene("Game");
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
