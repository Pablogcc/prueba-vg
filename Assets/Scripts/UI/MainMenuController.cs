using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureSurvival.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private string firstGameSceneName = "Level1";
        [SerializeField] private int firstGameSceneBuildIndex = 1;
        [SerializeField] private bool loadBySceneName = true;

        public void StartGame()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;

            if (loadBySceneName && !string.IsNullOrWhiteSpace(firstGameSceneName))
            {
                SceneManager.LoadScene(firstGameSceneName);
                return;
            }

            SceneManager.LoadScene(firstGameSceneBuildIndex);
        }

        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogWarning($"{nameof(MainMenuController)} cannot load an empty scene name.", this);
                return;
            }

            Time.timeScale = 1f;
            AudioListener.pause = false;
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnValidate()
        {
            firstGameSceneBuildIndex = Mathf.Max(0, firstGameSceneBuildIndex);
        }
    }
}
