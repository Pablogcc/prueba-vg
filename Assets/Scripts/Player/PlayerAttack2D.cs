using System.Collections.Generic;
using AdventureSurvival.Enemies;
using UnityEngine;

namespace AdventureSurvival.Player
{
    public class PlayerAttack2D : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private int damage = 1;
        [SerializeField] private float attackRange = 0.75f;
        [SerializeField] private float attackCooldown = 0.35f;
        [SerializeField] private LayerMask enemyLayers = ~0;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private string attackButton = "Fire1";

        [Header("Animation Parameters")]
        [SerializeField] private bool updateAnimator = true;
        [SerializeField] private string attackTriggerParameter = "Attack";

        private PlayerMovement2D movement;
        private Animator animator;
        private float nextAttackTime;

        private void Awake()
        {
            TryGetComponent(out movement);
            TryGetComponent(out animator);
        }

        private void Update()
        {
            if (Time.time < nextAttackTime || !Input.GetButtonDown(attackButton))
            {
                return;
            }

            Attack();
        }

        public void Attack()
        {
            nextAttackTime = Time.time + attackCooldown;

            if (updateAnimator && animator != null)
            {
                animator.SetTrigger(attackTriggerParameter);
            }

            Vector2 center = GetAttackCenter();
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRange, enemyLayers);
            HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

            foreach (Collider2D hit in hits)
            {
                EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();

                if (enemyHealth != null && damagedEnemies.Add(enemyHealth))
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }

        private Vector2 GetAttackCenter()
        {
            if (attackPoint != null)
            {
                return attackPoint.position;
            }

            Vector2 facingDirection = movement != null ? movement.LastFacingDirection : Vector2.down;
            return (Vector2)transform.position + facingDirection.normalized * attackRange;
        }

        private void OnValidate()
        {
            damage = Mathf.Max(1, damage);
            attackRange = Mathf.Max(0.1f, attackRange);
            attackCooldown = Mathf.Max(0f, attackCooldown);

            if (string.IsNullOrWhiteSpace(attackButton))
            {
                attackButton = "Fire1";
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetAttackCenter(), attackRange);
        }
    }
}
