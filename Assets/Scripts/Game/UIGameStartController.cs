using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class UIGameStartController : ViewController
    {
        private void Awake()
        {
            UIKit.Root.SetResolution(1920, 1080, 0.5f);
        }

        private void Start()
        {
            UIKit.OpenPanel<UIGameStartPanel>();
        }
    }
}
