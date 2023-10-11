using UnityEngine;

namespace TheUnfairDice
{
    public class DamageSystem
    {
        public static void CalculateDamage(float baseDamage, Enemy enemy, int maxNormalDamage = 2, float criticalDamageTimes = 3)
        {
            baseDamage += baseDamage + Global.Damage.Value;

            if (Random.Range(0, 1.0f) < Global.CriticalPercent.Value)
            {
                enemy.GetHurt(baseDamage * Random.Range(1.5f, criticalDamageTimes), false, true);
            }
            else
            {
                enemy.GetHurt(Mathf.Max(1, baseDamage + Random.Range(-1, maxNormalDamage)));
            }
        }
    }
}
