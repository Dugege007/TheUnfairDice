using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class CameraController : ViewController
    {
        private static CameraController mDefault;
        public static Transform LDTrans => mDefault.LD;
        public static Transform RUTrans => mDefault.RU;

        private void Awake()
        {
            mDefault = this;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void OnDestroy()
        {
            mDefault = null;
        }
    }
}
