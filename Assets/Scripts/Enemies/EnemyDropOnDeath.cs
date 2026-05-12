using UnityEngine;

namespace AdventureSurvival.Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyDropOnDeath : MonoBehaviour
    {
        [System.Serializable]
        private class DropEntry
        {
            [SerializeField] private GameObject prefab;
            [Range(0f, 1f)]
            [SerializeField] private float dropChance = 1f;
            [SerializeField] private int minQuantity = 1;
            [SerializeField] private int maxQuantity = 1;
            [SerializeField] private Vector2 randomOffset = new Vector2(0.25f, 0.25f);

            public GameObject Prefab => prefab;
            public float DropChance => dropChance;
            public int MinQuantity => Mathf.Max(0, minQuantity);
            public int MaxQuantity => Mathf.Max(MinQuantity, maxQuantity);
            public Vector2 RandomOffset => randomOffset;
        }

        [SerializeField] private DropEntry[] drops;

        private EnemyHealth enemyHealth;

        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnEnable()
        {
            if (enemyHealth == null)
            {
                enemyHealth = GetComponent<EnemyHealth>();
            }

            enemyHealth.Died += HandleEnemyDied;
        }

        private void OnDisable()
        {
            if (enemyHealth != null)
            {
                enemyHealth.Died -= HandleEnemyDied;
            }
        }

        private void HandleEnemyDied(EnemyHealth deadEnemy)
        {
            if (drops == null)
            {
                return;
            }

            foreach (DropEntry drop in drops)
            {
                if (drop == null || drop.Prefab == null || Random.value > drop.DropChance)
                {
                    continue;
                }

                int quantity = Random.Range(drop.MinQuantity, drop.MaxQuantity + 1);

                for (int i = 0; i < quantity; i++)
                {
                    Vector2 offset = new Vector2(
                        Random.Range(-drop.RandomOffset.x, drop.RandomOffset.x),
                        Random.Range(-drop.RandomOffset.y, drop.RandomOffset.y));

                    Instantiate(drop.Prefab, (Vector2)deadEnemy.transform.position + offset, Quaternion.identity);
                }
            }
        }
    }
}
