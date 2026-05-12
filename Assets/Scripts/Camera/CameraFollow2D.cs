using UnityEngine;

namespace AdventureSurvival.CameraSystem
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
        [SerializeField] private float smoothTime = 0.15f;
        [SerializeField] private bool findPlayerByTagOnStart = true;

        private Vector3 velocity;

        private void Start()
        {
            if (target == null && findPlayerByTagOnStart)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    target = player.transform;
                }
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
