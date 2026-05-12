using UnityEngine;

namespace AdventureSurvival.Levels
{
    public class LevelExit2D : MonoBehaviour
    {
        [SerializeField] private string targetSceneName;
        [SerializeField] private string targetSpawnPointId = "Default";
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private bool disableAfterUse = true;

        private bool isLoading;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isLoading || !other.CompareTag(playerTag))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(targetSceneName))
            {
                Debug.LogWarning($"{nameof(LevelExit2D)} cannot load a level because the target scene name is empty.", this);
                return;
            }

            isLoading = true;

            if (disableAfterUse)
            {
                enabled = false;
            }

            LevelManager.Instance.LoadLevel(targetSceneName, targetSpawnPointId);
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(targetSpawnPointId))
            {
                targetSpawnPointId = "Default";
            }
        }
    }
}
