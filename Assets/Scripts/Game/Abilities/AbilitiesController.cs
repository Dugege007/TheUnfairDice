using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class AbilitiesController : ViewController
    {
        private void Start()
        {
            HideAllAbilities();

            HolyWater.Show();
        }

        private void HideAllAbilities()
        {
            HolyWater.Hide();
            HolyFire.Hide();
            HolyTree.Hide();
            HolySword.Hide();
            HolyLand.Hide();
            HolyLight.Hide();
        }
    }
}
