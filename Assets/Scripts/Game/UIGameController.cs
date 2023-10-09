using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class UIGameController : ViewController
    {
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
