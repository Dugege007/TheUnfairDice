using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class Enemy : ViewController
    {
        private float MovementSpeed = 2f;

        private void Update()
        {
            if (Player.mDefault)
            {
                Vector3 direction = (Player.mDefault.Position() - this.Position()).normalized;

                SelfRgidbody2D.velocity = direction * MovementSpeed;
            }
        }
    }
}
