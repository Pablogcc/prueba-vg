using AdventureSurvival.Levels;
using AdventureSurvival.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureSurvival.UI
{
    public class DeathScreenUI : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private string playerTag = "Player";

        [Header("UI")]
        [SerializeField] private GameObject deathPanel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool pauseGameOnDeath;

        [Header("Scenes")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private void Awake()
        {
            HideDeathScreen();
        }

        private void OnEnable()
        {
            FindPlayerIfNeeded();
            SubscribeToPlayer();
        }

        private void OnDisable()
        {
            UnsubscribeFromPlayer();
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            LevelManager.Instance.RestartCurrentLevel();
        }

        public void BackToMainMenu()
        {
            if (string.IsNullOrWhiteSpace(mainMenuSceneName))
            {
                Debug.LogWarning($"{nameof(DeathScreenUI)} cannot return to the main menu because the scene name is empty.", this);
                return;
            }

            Time.timeScale = 1f;
            AudioListener.pause = false;
            SceneManager.LoadScene(mainMenuSceneName);
        }

        private void HandlePlayerDied()
        {
            ShowDeathScreen();
        }

        private void HandlePlayerRespawned()
        {
            HideDeathScreen();
        }

        private void ShowDeathScreen()
        {
            if (deathPanel != null)
            {
                deathPanel.SetActive(true);
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            if (pauseGameOnDeath)
            {
                Time.timeScale = 0f;
            }
        }

        private void HideDeathScreen()
        {
            if (deathPanel != null)
            {
                deathPanel.SetActive(false);
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        private void FindPlayerIfNeeded()
        {
            if (playerHealth != null)
            {
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null)
            {
                player.TryGetComponent(out playerHealth);
            }
        }

        private void SubscribeToPlayer()
        {
            if (playerHealth == null)
            {
                return;
            }

            playerHealth.Died += HandlePlayerDied;
            playerHealth.Respawned += HandlePlayerRespawned;
        }

        private void UnsubscribeFromPlayer()
        {
            if (playerHealth == null)
            {
                return;
            }

            playerHealth.Died -= HandlePlayerDied;
            playerHealth.Respawned -= HandlePlayerRespawned;
        }
    }
}
