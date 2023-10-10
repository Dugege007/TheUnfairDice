using UnityEngine;
using QFramework;
using System.Linq;

namespace TheUnfairDice
{
    public partial class AbilitiesController : ViewController, IController
    {
        private void Start()
        {
            // 先隐藏所有能力
            HideAllAbilities();

            #region 监听能力的解锁
            // 水
            Global.HolyWaterUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyWater.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 火
            Global.HolyFireUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyFire.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 树
            Global.HolyTreeUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyTree.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 剑
            Global.HolySwordUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolySword.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 土
            Global.HolyLandUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyLand.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 光
            Global.HolyLightUnlocked.RegisterWithInitValue(unlocked =>
            {
                if (unlocked)
                    HolyLight.Show();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            #endregion

            // 随机升级一个
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
