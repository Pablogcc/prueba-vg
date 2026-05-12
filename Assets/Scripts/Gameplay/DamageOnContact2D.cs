using System.Collections.Generic;
using AdventureSurvival.Player;
using UnityEngine;

namespace AdventureSurvival.Gameplay
{
    public class DamageOnContact2D : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private string targetTag = "Player";
        [SerializeField] private bool damageOnTrigger = true;
        [SerializeField] private bool damageOnCollision = true;
        [SerializeField] private float repeatedDamageCooldown = 0.75f;

        private readonly Dictionary<PlayerHealth, float> lastDamageTimes = new Dictionary<PlayerHealth, float>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (damageOnTrigger)
            {
                TryDamage(other);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (damageOnTrigger)
            {
                TryDamage(other);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (damageOnCollision)
            {
                TryDamage(collision.collider);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (damageOnCollision)
            {
                TryDamage(collision.collider);
            }
        }

        private void TryDamage(Collider2D other)
        {
            if (!other.CompareTag(targetTag) || !other.TryGetComponent(out PlayerHealth health))
            {
                return;
            }

            if (lastDamageTimes.TryGetValue(health, out float lastDamageTime) &&
                Time.time < lastDamageTime + repeatedDamageCooldown)
            {
                return;
            }

            lastDamageTimes[health] = Time.time;
            health.TakeDamage(damage);
        }

        private void OnValidate()
        {
            damage = Mathf.Max(1, damage);
            repeatedDamageCooldown = Mathf.Max(0f, repeatedDamageCooldown);
        }
    }
}
