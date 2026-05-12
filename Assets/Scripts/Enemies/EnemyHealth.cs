using System;
using UnityEngine;

namespace AdventureSurvival.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private bool destroyOnDeath = true;
        [SerializeField] private float destroyDelay = 0f;

        private int currentHealth;
        private bool isDead;

        public event Action<int, int> HealthChanged;
        public event Action<EnemyHealth> Died;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public bool IsDead => isDead;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Start()
        {
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || isDead)
            {
                return;
            }

            currentHealth = Mathf.Max(currentHealth - amount, 0);
            HealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || isDead)
            {
                return;
            }

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void ResetHealth()
        {
            isDead = false;
            currentHealth = maxHealth;
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void Die()
        {
            isDead = true;
            Died?.Invoke(this);

            if (destroyOnDeath)
            {
                Destroy(gameObject, destroyDelay);
            }
        }

        private void OnValidate()
        {
            maxHealth = Mathf.Max(1, maxHealth);
            destroyDelay = Mathf.Max(0f, destroyDelay);
        }
    }
}
