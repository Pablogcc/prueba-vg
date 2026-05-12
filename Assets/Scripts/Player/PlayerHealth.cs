using System;
using System.Collections;
using UnityEngine;

namespace AdventureSurvival.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private float invulnerabilityTime = 1f;

        [Header("Respawn")]
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float respawnDelay = 1f;
        [SerializeField] private bool resetHealthOnRespawn = true;

        private Rigidbody2D rb;
        private PlayerMovement2D movement;
        private Collider2D playerCollider;
        private Vector3 initialSpawnPosition;
        private int currentHealth;
        private bool isInvulnerable;
        private bool isDead;

        public event Action<int, int> HealthChanged;
        public event Action Died;
        public event Action Respawned;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public bool IsDead => isDead;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            TryGetComponent(out movement);
            TryGetComponent(out playerCollider);
            initialSpawnPosition = transform.position;
            currentHealth = maxHealth;
        }

        private void Start()
        {
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || isDead || isInvulnerable)
            {
                return;
            }

            currentHealth = Mathf.Max(currentHealth - amount, 0);
            HealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            StartCoroutine(InvulnerabilityRoutine());
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

        public void SetRespawnPoint(Transform newRespawnPoint)
        {
            respawnPoint = newRespawnPoint;
        }

        public void SetRespawnPosition(Vector3 newRespawnPosition)
        {
            respawnPoint = null;
            initialSpawnPosition = newRespawnPosition;
        }

        private void Die()
        {
            isDead = true;
            isInvulnerable = false;
            movement?.SetMovementEnabled(false);
            rb.velocity = Vector2.zero;

            if (playerCollider != null)
            {
                playerCollider.enabled = false;
            }

            Died?.Invoke();
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator InvulnerabilityRoutine()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityTime);
            isInvulnerable = false;
        }

        private IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnDelay);
            Respawn();
        }

        private void Respawn()
        {
            Vector3 targetPosition = respawnPoint != null ? respawnPoint.position : initialSpawnPosition;
            transform.position = targetPosition;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (resetHealthOnRespawn)
            {
                currentHealth = maxHealth;
                HealthChanged?.Invoke(currentHealth, maxHealth);
            }

            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }

            movement?.SetMovementEnabled(true);
            isDead = false;
            isInvulnerable = false;
            Respawned?.Invoke();
        }

        private void OnValidate()
        {
            maxHealth = Mathf.Max(1, maxHealth);
            invulnerabilityTime = Mathf.Max(0f, invulnerabilityTime);
            respawnDelay = Mathf.Max(0f, respawnDelay);
        }
    }
}
