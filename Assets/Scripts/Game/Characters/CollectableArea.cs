using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class CollectableArea : ViewController
    {
        private void Start()
        {
            Global.CollectableRange.RegisterWithInitValue(range =>
            {
                GetComponent<CircleCollider2D>().radius = range;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
    }
}
