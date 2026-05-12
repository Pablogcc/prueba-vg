using AdventureSurvival.Player;
using UnityEngine;

namespace AdventureSurvival.Gameplay
{
    public class RespawnPoint2D : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag) || !other.TryGetComponent(out PlayerHealth playerHealth))
            {
                return;
            }

            playerHealth.SetRespawnPoint(transform);
        }
    }
}
