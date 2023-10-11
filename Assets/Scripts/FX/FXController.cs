using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class FXController : ViewController
    {
        private static FXController mDefault;

        private void Awake()
        {
            mDefault = this;
        }

        public static void Play(SpriteRenderer sprite, Color dissolveColor)
        {
            mDefault.DeathFX.Instantiate()
                .Position(sprite.Position())
                .LocalScale(sprite.Scale())
                .Self(s =>
                {
                    s.GetComponent<Dissolve>().DissolveColor = dissolveColor;
                    s.sprite = sprite.sprite;
                })
                .Show();
        }

        private void OnDestroy()
        {
            mDefault = null;
        }
    }
}
