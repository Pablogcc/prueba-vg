using AdventureSurvival.Audio;
using UnityEngine;

namespace AdventureSurvival.Inventory
{
    [RequireComponent(typeof(Collider2D))]
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int quantity = 1;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private bool destroyAfterPickup = true;

        [Header("Feedback")]
        [SerializeField] private AudioClip pickupSound;
        [Range(0f, 1f)]
        [SerializeField] private float pickupSoundVolume = 1f;
        [SerializeField] private GameObject pickupEffectPrefab;

        private bool pickedUp;

        private void Reset()
        {
            Collider2D pickupCollider = GetComponent<Collider2D>();
            pickupCollider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (pickedUp || !other.CompareTag(playerTag))
            {
                return;
            }

            if (!InventoryManager.Instance.TryAddItem(item, quantity))
            {
                return;
            }

            pickedUp = true;
            PlayPickupFeedback();

            if (destroyAfterPickup)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void PlayPickupFeedback()
        {
            if (pickupSound != null)
            {
                GameAudioManager.Instance.PlaySfxAtPosition(pickupSound, transform.position, pickupSoundVolume);
            }

            if (pickupEffectPrefab != null)
            {
                Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            }
        }

        private void OnValidate()
        {
            quantity = Mathf.Max(1, quantity);
            pickupSoundVolume = Mathf.Clamp01(pickupSoundVolume);
        }
    }
}
