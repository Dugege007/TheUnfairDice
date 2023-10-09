using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class UIGameStartController : ViewController
    {
        private void Awake()
        {
        }

        private void Start()
        {
            UIKit.CloseAllPanel();

            UIKit.OpenPanel<UIGameStartPanel>();
        }
    }
}
