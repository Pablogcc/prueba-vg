using AdventureSurvival.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureSurvival.Levels
{
    public class LevelManager : MonoBehaviour
    {
        private const string LastSceneKey = "AdventureSurvival.LastScene";
        private const string LastSpawnPointKey = "AdventureSurvival.LastSpawnPoint";

        private static LevelManager instance;
        private static string pendingSpawnPointId;

        [Header("Player")]
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private bool placePlayerOnSceneStart = true;

        [Header("Default Spawn")]
        [SerializeField] private string fallbackSpawnPointId = "Default";
        [SerializeField] private bool useSavedSpawnWhenNoPendingSpawn = true;

        [Header("Persistence")]
        [SerializeField] private bool keepBetweenScenes = true;
        [SerializeField] private bool saveSpawnInPlayerPrefs = true;

        public static LevelManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = FindObjectOfType<LevelManager>();

                if (instance != null)
                {
                    return instance;
                }

                GameObject managerObject = new GameObject(nameof(LevelManager));
                instance = managerObject.AddComponent<LevelManager>();
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            if (keepBetweenScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Start()
        {
            if (placePlayerOnSceneStart)
            {
                PlacePlayerAtBestSpawnPoint();
            }
        }

        public void LoadLevel(string targetSceneName, string targetSpawnPointId = "Default")
        {
            if (string.IsNullOrWhiteSpace(targetSceneName))
            {
                Debug.LogWarning($"{nameof(LevelManager)} cannot load a level because the target scene name is empty.", this);
                return;
            }

            pendingSpawnPointId = NormalizeSpawnId(targetSpawnPointId);
            SaveLastSpawn(targetSceneName, pendingSpawnPointId);
            SceneManager.LoadScene(targetSceneName);
        }

        public void SaveCurrentSpawnPoint(SpawnPoint2D spawnPoint)
        {
            if (spawnPoint == null)
            {
                return;
            }

            SaveLastSpawn(SceneManager.GetActiveScene().name, spawnPoint.SpawnPointId);
        }

        public void RestartCurrentLevel()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SaveLastSpawn(sceneName, GetBestSpawnPointIdForCurrentScene());
            SceneManager.LoadScene(sceneName);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (placePlayerOnSceneStart)
            {
                PlacePlayerAtBestSpawnPoint();
            }
        }

        private void PlacePlayerAtBestSpawnPoint()
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player == null)
            {
                return;
            }

            SpawnPoint2D spawnPoint = FindBestSpawnPoint();

            if (spawnPoint == null)
            {
                return;
            }

            player.transform.position = spawnPoint.transform.position;

            if (player.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            if (player.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.SetRespawnPosition(spawnPoint.transform.position);
            }

            SaveCurrentSpawnPoint(spawnPoint);
            pendingSpawnPointId = null;
        }

        private SpawnPoint2D FindBestSpawnPoint()
        {
            SpawnPoint2D[] spawnPoints = FindObjectsOfType<SpawnPoint2D>();

            if (spawnPoints.Length == 0)
            {
                return null;
            }

            string wantedSpawnId = GetBestSpawnPointIdForCurrentScene();
            SpawnPoint2D defaultSpawn = null;

            foreach (SpawnPoint2D spawnPoint in spawnPoints)
            {
                if (spawnPoint.SpawnPointId == wantedSpawnId)
                {
                    return spawnPoint;
                }

                if (spawnPoint.IsDefaultSpawn)
                {
                    defaultSpawn = spawnPoint;
                }
            }

            return defaultSpawn != null ? defaultSpawn : spawnPoints[0];
        }

        private string GetBestSpawnPointIdForCurrentScene()
        {
            if (!string.IsNullOrWhiteSpace(pendingSpawnPointId))
            {
                return NormalizeSpawnId(pendingSpawnPointId);
            }

            if (useSavedSpawnWhenNoPendingSpawn && saveSpawnInPlayerPrefs)
            {
                string activeSceneName = SceneManager.GetActiveScene().name;
                string savedSceneName = PlayerPrefs.GetString(LastSceneKey, string.Empty);

                if (activeSceneName == savedSceneName)
                {
                    return NormalizeSpawnId(PlayerPrefs.GetString(LastSpawnPointKey, fallbackSpawnPointId));
                }
            }

            return NormalizeSpawnId(fallbackSpawnPointId);
        }

        private void SaveLastSpawn(string sceneName, string spawnPointId)
        {
            if (!saveSpawnInPlayerPrefs)
            {
                return;
            }

            PlayerPrefs.SetString(LastSceneKey, sceneName);
            PlayerPrefs.SetString(LastSpawnPointKey, NormalizeSpawnId(spawnPointId));
            PlayerPrefs.Save();
        }

        private string NormalizeSpawnId(string spawnPointId)
        {
            return string.IsNullOrWhiteSpace(spawnPointId) ? fallbackSpawnPointId : spawnPointId;
        }
    }
}
