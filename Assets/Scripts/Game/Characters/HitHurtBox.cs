using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class HitHurtBox : ViewController
    {
        public GameObject Owner;

        private void Awake()
        {
            if (Owner == null)
            {
                Owner = transform.parent.gameObject;
            }
        }
    }
}
