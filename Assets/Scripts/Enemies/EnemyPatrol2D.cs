using UnityEngine;

namespace AdventureSurvival.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyPatrol2D : MonoBehaviour
    {
        [Header("Patrol")]
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float arriveDistance = 0.1f;
        [SerializeField] private bool startMovingToPointB = true;
        [SerializeField] private bool flipSpriteWithDirection = true;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Transform currentTarget;
        private bool patrolEnabled = true;

        public bool PatrolEnabled => patrolEnabled;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            TryGetComponent(out spriteRenderer);
        }

        private void Start()
        {
            currentTarget = startMovingToPointB ? pointB : pointA;
        }

        private void FixedUpdate()
        {
            if (!patrolEnabled || pointA == null || pointB == null || currentTarget == null)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            Vector2 direction = ((Vector2)currentTarget.position - rb.position).normalized;
            rb.velocity = direction * moveSpeed;

            if (Vector2.Distance(rb.position, currentTarget.position) <= arriveDistance)
            {
                currentTarget = currentTarget == pointA ? pointB : pointA;
            }

            UpdateSpriteFacing(direction.x);
        }

        public void SetPatrolEnabled(bool enabled)
        {
            patrolEnabled = enabled;

            if (!enabled && rb != null)
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
            moveSpeed = Mathf.Max(0f, moveSpeed);
            arriveDistance = Mathf.Max(0.01f, arriveDistance);
        }
    }
}
