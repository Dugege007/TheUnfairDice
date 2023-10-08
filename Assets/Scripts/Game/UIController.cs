using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class UIController : ViewController
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            // …Ë÷√ UI
            UIKit.Root.SetResolution(1920, 1080, 0.5f);
        }

        private void Start()
        {
            UIKit.OpenPanel<UIGamePanel>();
        }

        private void OnDestroy()
        {
            UIKit.ClosePanel<UIGamePanel>();
        }
    }
}
