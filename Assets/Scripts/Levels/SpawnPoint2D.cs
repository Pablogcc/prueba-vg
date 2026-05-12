using AdventureSurvival.Player;
using UnityEngine;

namespace AdventureSurvival.Levels
{
    public class SpawnPoint2D : MonoBehaviour
    {
        [SerializeField] private string spawnPointId = "Default";
        [SerializeField] private bool isDefaultSpawn;
        [SerializeField] private bool saveWhenPlayerTouches = true;
        [SerializeField] private string playerTag = "Player";

        public string SpawnPointId => spawnPointId;
        public bool IsDefaultSpawn => isDefaultSpawn;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!saveWhenPlayerTouches || !other.CompareTag(playerTag))
            {
                return;
            }

            if (other.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.SetRespawnPosition(transform.position);
            }

            LevelManager.Instance.SaveCurrentSpawnPoint(this);
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(spawnPointId))
            {
                spawnPointId = "Default";
            }
        }
    }
}
