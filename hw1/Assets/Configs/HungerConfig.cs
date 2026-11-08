using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "HungerConfig", menuName = "Configs/HungerConfig")]
    public class HungerConfig : ScriptableObject
    {
        [SerializeField] private int maxHunger = 20;
        [SerializeField] private int hungerDecreaseAmount = 1;
        [SerializeField] private int hungerDecreasePeriodSeconds = 10;
        [SerializeField] private int starvationThreshold = 0;
        [SerializeField] private int starvationDamage = 1;
        [SerializeField] private int starvationPeriodSeconds = 4;

        public int MaxHunger => maxHunger;
        public int HungerDecreaseAmount => hungerDecreaseAmount;
        public int HungerDecreasePeriodSeconds => hungerDecreasePeriodSeconds;
        public int StarvationThreshold => starvationThreshold;
        public int StarvationDamage => starvationDamage;
        public int StarvationPeriodSeconds => starvationPeriodSeconds;
    }
}