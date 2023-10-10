using UnityEngine;
using QFramework;
using System.Linq;

namespace TheUnfairDice
{
    public partial class AbilitiesController : ViewController, IController
    {
        private void Start()
        {
            // ��������������
            HideAllAbilities();

            #region ���������Ľ���
            // ˮ
            Global.HolyWaterUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyWater.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��
            Global.HolyFireUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyFire.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��
            Global.HolyTreeUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyTree.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��
            Global.HolySwordUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolySword.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��
            Global.HolyLandUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyLand.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ��
            Global.HolyLightUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyLight.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            #endregion

            // �������һ��
            this.GetSystem<ExpUpgradeSystem>().Items
                .Where(item => item.IsWeapon)
                .ToList()
                .GetRandomItem()
                .Upgrade();
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

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}
