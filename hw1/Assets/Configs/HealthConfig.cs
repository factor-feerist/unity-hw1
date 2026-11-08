using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "HealthConfig", menuName = "Configs/HealthConfig")]
    public class HealthConfig : ScriptableObject
    {
        [SerializeField] private int maxHp = 20;
        [SerializeField] private int regenThreshold = 18;
        [SerializeField] private int regenAmount = 1;
        [SerializeField] private int regenPeriodSeconds = 4;

        public int MaxHp => maxHp;
        public int RegenThreshold => regenThreshold;
        public int RegenAmount => regenAmount;
        public int RegenPeriodSeconds => regenPeriodSeconds;
    }
}