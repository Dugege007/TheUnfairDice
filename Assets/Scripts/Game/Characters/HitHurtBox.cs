using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class HitHurtBox : ViewController
    {
        public GameObject Owner;

        private void Start()
        {
            if (Owner == null)
            {
                Owner = transform.parent.gameObject;
            }
        }
    }
}
