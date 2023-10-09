using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

namespace TheUnfairDice
{
    public class UIGamePassPanelData : UIPanelData
    {
    }
    public partial class UIGamePassPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIGamePassPanelData ?? new UIGamePassPanelData();
            // please add init code here

            Time.timeScale = 0;

            BackToHomeBtn.onClick.AddListener(() =>
            {
                Global.ResetData();
                this.CloseSelf();
                SceneManager.LoadScene("GameStart");
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
