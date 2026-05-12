using System.Collections;
using AdventureSurvival.Audio;
using AdventureSurvival.Player;
using UnityEngine;

namespace AdventureSurvival.Feedback
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDamageFeedback : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private int flashCount = 3;
        [SerializeField] private GameObject damageEffectPrefab;

        [Header("Audio")]
        [SerializeField] private AudioClip damageSound;
        [Range(0f, 1f)]
        [SerializeField] private float damageSoundVolume = 1f;

        private PlayerHealth playerHealth;
        private Color[] originalColors;
        private Coroutine flashRoutine;

        private void Awake()
        {
            playerHealth = GetComponent<PlayerHealth>();

            if (spriteRenderers == null || spriteRenderers.Length == 0)
            {
                spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            }

            CacheOriginalColors();
        }

        private void OnEnable()
        {
            playerHealth.Damaged += HandlePlayerDamaged;
            playerHealth.Respawned += HandlePlayerRespawned;
        }

        private void OnDisable()
        {
            playerHealth.Damaged -= HandlePlayerDamaged;
            playerHealth.Respawned -= HandlePlayerRespawned;
        }

        private void HandlePlayerDamaged(int damageAmount)
        {
            if (damageSound != null)
            {
                GameAudioManager.Instance.PlaySfx(damageSound, damageSoundVolume);
            }

            if (damageEffectPrefab != null)
            {
                Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
            }

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashRoutine());
        }

        private void HandlePlayerRespawned()
        {
            RestoreOriginalColors();
        }

        private IEnumerator FlashRoutine()
        {
            for (int i = 0; i < flashCount; i++)
            {
                SetSpriteColor(damageColor);
                yield return new WaitForSeconds(flashDuration);
                RestoreOriginalColors();
                yield return new WaitForSeconds(flashDuration);
            }

            flashRoutine = null;
        }

        private void CacheOriginalColors()
        {
            if (spriteRenderers == null)
            {
                originalColors = new Color[0];
                return;
            }

            originalColors = new Color[spriteRenderers.Length];

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                originalColors[i] = spriteRenderers[i] != null ? spriteRenderers[i].color : Color.white;
            }
        }

        private void SetSpriteColor(Color color)
        {
            if (spriteRenderers == null)
            {
                return;
            }

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = color;
                }
            }
        }

        private void RestoreOriginalColors()
        {
            if (spriteRenderers == null || originalColors == null)
            {
                return;
            }

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null && i < originalColors.Length)
                {
                    spriteRenderers[i].color = originalColors[i];
                }
            }
        }

        private void OnValidate()
        {
            flashDuration = Mathf.Max(0.01f, flashDuration);
            flashCount = Mathf.Max(1, flashCount);
            damageSoundVolume = Mathf.Clamp01(damageSoundVolume);
        }
    }
}
