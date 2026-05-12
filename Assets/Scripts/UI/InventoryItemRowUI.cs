using AdventureSurvival.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureSurvival.UI
{
    public class InventoryItemRowUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Text nameText;
        [SerializeField] private Text quantityText;
        [SerializeField] private Text typeText;

        public void SetSlot(InventorySlot slot)
        {
            if (slot == null || slot.Item == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            ItemData item = slot.Item;

            if (iconImage != null)
            {
                iconImage.sprite = item.Icon;
                iconImage.enabled = item.Icon != null;
            }

            if (nameText != null)
            {
                nameText.text = item.DisplayName;
            }

            if (quantityText != null)
            {
                quantityText.text = slot.Quantity > 1 ? $"x{slot.Quantity}" : string.Empty;
            }

            if (typeText != null)
            {
                typeText.text = item.IsKey ? "Clave" : "Consumible";
            }
        }
    }
}
