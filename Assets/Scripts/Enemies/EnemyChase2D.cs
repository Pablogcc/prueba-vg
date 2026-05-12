using UnityEngine;

namespace AdventureSurvival.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyChase2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private Transform target;

        [Header("Chase")]
        [SerializeField] private float detectionRadius = 4f;
        [SerializeField] private float stopDistance = 0.5f;
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private bool forgetTargetWhenOutOfRange = true;
        [SerializeField] private bool flipSpriteWithDirection = true;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private bool chaseEnabled = true;

        public bool IsChasing { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            TryGetComponent(out spriteRenderer);
        }

        private void FixedUpdate()
        {
            if (!chaseEnabled)
            {
                StopChasing();
                return;
            }

            EnsureTarget();

            if (target == null)
            {
                StopChasing();
                return;
            }

            float distanceToTarget = Vector2.Distance(rb.position, target.position);

            if (distanceToTarget > detectionRadius)
            {
                if (forgetTargetWhenOutOfRange)
                {
                    target = null;
                }

                StopChasing();
                return;
            }

            IsChasing = true;

            if (distanceToTarget <= stopDistance)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.velocity = direction * moveSpeed;
            UpdateSpriteFacing(direction.x);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void SetChaseEnabled(bool enabled)
        {
            chaseEnabled = enabled;

            if (!enabled)
            {
                StopChasing();
            }
        }

        private void EnsureTarget()
        {
            if (target != null)
            {
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null)
            {
                target = player.transform;
            }
        }

        private void StopChasing()
        {
            IsChasing = false;

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }

        private void UpdateSpriteFacing(float horizontalDirection)
        {
            if (!flipSpriteWithDirection || spriteRenderer == null || Mathf.Approximately(horizontalDirection, 0f))
            {
                return;
            }

            spriteRenderer.flipX = horizontalDirection < 0f;
        }

        private void OnValidate()
        {
            detectionRadius = Mathf.Max(0.1f, detectionRadius);
            stopDistance = Mathf.Max(0f, stopDistance);
            moveSpeed = Mathf.Max(0f, moveSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stopDistance);
        }
    }
}
