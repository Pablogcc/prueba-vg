using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureSurvival.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

        [Header("UI")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private bool pauseAudioListener = true;

        [Header("Scenes")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        public bool IsPaused { get; private set; }

        private void Start()
        {
            SetPaused(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            SetPaused(!IsPaused);
        }

        public void ResumeGame()
        {
            SetPaused(false);
        }

        public void RestartLevel()
        {
            SetPaused(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void BackToMainMenu()
        {
            if (string.IsNullOrWhiteSpace(mainMenuSceneName))
            {
                Debug.LogWarning($"{nameof(PauseMenuController)} cannot return to the main menu because the scene name is empty.", this);
                return;
            }

            SetPaused(false);
            SceneManager.LoadScene(mainMenuSceneName);
        }

        private void SetPaused(bool paused)
        {
            IsPaused = paused;
            Time.timeScale = paused ? 0f : 1f;

            if (pauseAudioListener)
            {
                AudioListener.pause = paused;
            }

            if (pausePanel != null)
            {
                pausePanel.SetActive(paused);
            }
        }

        private void OnDestroy()
        {
            if (IsPaused)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }
        }
    }
}
