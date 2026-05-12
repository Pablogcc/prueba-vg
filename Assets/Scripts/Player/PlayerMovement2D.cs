using UnityEngine;

namespace AdventureSurvival.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private bool allowDiagonalMovement = true;

        [Header("Animation Parameters")]
        [SerializeField] private bool updateAnimator = true;
        [SerializeField] private string speedParameter = "Speed";
        [SerializeField] private string horizontalParameter = "Horizontal";
        [SerializeField] private string verticalParameter = "Vertical";
        [SerializeField] private string isMovingParameter = "IsMoving";

        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 movementInput;
        private Vector2 lastFacingDirection = Vector2.down;
        private bool movementEnabled = true;

        public Vector2 LastFacingDirection => lastFacingDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            TryGetComponent(out animator);
        }

        private void Update()
        {
            if (!movementEnabled)
            {
                movementInput = Vector2.zero;
                UpdateAnimation(Vector2.zero);
                return;
            }

            movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (!allowDiagonalMovement && Mathf.Abs(movementInput.x) > 0f)
            {
                movementInput.y = 0f;
            }

            movementInput = movementInput.normalized;

            if (movementInput.sqrMagnitude > 0.01f)
            {
                lastFacingDirection = movementInput;
            }

            UpdateAnimation(movementInput);
        }

        private void FixedUpdate()
        {
            rb.velocity = movementInput * moveSpeed;
        }

        public void SetMovementEnabled(bool enabled)
        {
            movementEnabled = enabled;

            if (!enabled)
            {
                movementInput = Vector2.zero;
                rb.velocity = Vector2.zero;
            }
        }

        private void UpdateAnimation(Vector2 input)
        {
            if (!updateAnimator || animator == null)
            {
                return;
            }

            animator.SetFloat(speedParameter, input.sqrMagnitude);
            animator.SetFloat(horizontalParameter, lastFacingDirection.x);
            animator.SetFloat(verticalParameter, lastFacingDirection.y);
            animator.SetBool(isMovingParameter, input.sqrMagnitude > 0.01f);
        }
    }
}
