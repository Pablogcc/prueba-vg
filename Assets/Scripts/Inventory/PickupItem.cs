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

            if (destroyAfterPickup)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnValidate()
        {
            quantity = Mathf.Max(1, quantity);
        }
    }
}
