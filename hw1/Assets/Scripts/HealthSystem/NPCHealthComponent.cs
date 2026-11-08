using System.Collections;
using Configs;
using UnityEngine;

namespace HealthSystem
{
    public class NpcHealthComponent : MonoBehaviour
    {
        [SerializeField] private HealthConfig healthConfig;
        [SerializeField] private HungerConfig hungerConfig;

        private int _currentHp;
        private int _currentHunger;
        private bool _isAlive = true;

        public int CurrentHp
        {
            get => _currentHp;
            private set
            {
                _currentHp = Mathf.Clamp(value, 0, healthConfig.MaxHp);
                if (_currentHp <= 0 && _isAlive)
                {
                    _isAlive = false;
                    OnDeath();
                }
            }
        }

        public int CurrentHunger
        {
            get => _currentHunger;
            private set => _currentHunger = Mathf.Clamp(value, 0, hungerConfig.MaxHunger);
        }

        private void Awake()
        {
            CurrentHp = healthConfig.MaxHp;
            CurrentHunger = hungerConfig.MaxHunger;
        }

        private void Start()
        {
            StartCoroutine(HungerRoutine());
            StartCoroutine(HealthBalanceRoutine());
        }

        public void TakeDamage(int amount)
        {
            if (!_isAlive || amount <= 0)
                return;
            CurrentHp -= amount;
        }

        public void Heal(int amount)
        {
            if (!_isAlive || amount <= 0)
                return;
            CurrentHp += amount;
        }

        public void Eat(int nutrition)
        {
            if (!_isAlive || nutrition <= 0)
                return;
            CurrentHunger += nutrition;
        }

        public void Exhaust(int amount)
        {
            if (!_isAlive || amount <= 0)
                return;
            CurrentHunger -= amount;
        }

        private IEnumerator HungerRoutine()
        {
            while (_isAlive)
            {
                yield return new WaitForSeconds(hungerConfig.HungerDecreasePeriodSeconds);
                Exhaust(hungerConfig.HungerDecreaseAmount);
            }
        }

        private IEnumerator HealthBalanceRoutine()
        {
            while (_isAlive)
            {
                if (CurrentHunger >= healthConfig.RegenThreshold && CurrentHp < healthConfig.MaxHp)
                {
                    yield return new WaitForSeconds(healthConfig.RegenPeriodSeconds);
                    if (CurrentHunger >= healthConfig.RegenThreshold)
                        Heal(healthConfig.RegenAmount);
                }
                else if (CurrentHunger <= hungerConfig.StarvationThreshold)
                {
                    yield return new WaitForSeconds(hungerConfig.StarvationPeriodSeconds);
                    if (CurrentHunger <= hungerConfig.StarvationThreshold)
                        TakeDamage(hungerConfig.StarvationDamage);
                }
                else
                    yield return null;
            }
        }

        private void OnDeath()
        {
            StopAllCoroutines();
            Debug.Log($"{gameObject.name} has died.");
        }
    }
}