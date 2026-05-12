using System.Collections.Generic;
using System.Text;
using AdventureSurvival.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureSurvival.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private InventoryManager inventoryManager;

        [Header("Row UI")]
        [SerializeField] private Transform rowsContainer;
        [SerializeField] private InventoryItemRowUI rowPrefab;
        [SerializeField] private Text emptyText;

        [Header("Fallback Text UI")]
        [SerializeField] private Text inventoryListText;

        private readonly List<InventoryItemRowUI> spawnedRows = new List<InventoryItemRowUI>();

        private void Awake()
        {
            if (inventoryManager == null)
            {
                inventoryManager = InventoryManager.Instance;
            }
        }

        private void OnEnable()
        {
            if (inventoryManager == null)
            {
                inventoryManager = InventoryManager.Instance;
            }

            inventoryManager.InventoryChanged += Refresh;
            Refresh(inventoryManager.Slots);
        }

        private void OnDisable()
        {
            if (inventoryManager != null)
            {
                inventoryManager.InventoryChanged -= Refresh;
            }
        }

        public void Refresh(IReadOnlyList<InventorySlot> slots)
        {
            bool hasItems = slots != null && slots.Count > 0;

            if (emptyText != null)
            {
                emptyText.gameObject.SetActive(!hasItems);
            }

            RefreshRows(slots);
            RefreshFallbackText(slots);
        }

        private void RefreshRows(IReadOnlyList<InventorySlot> slots)
        {
            if (rowsContainer == null || rowPrefab == null)
            {
                return;
            }

            int slotCount = slots == null ? 0 : slots.Count;

            while (spawnedRows.Count < slotCount)
            {
                InventoryItemRowUI row = Instantiate(rowPrefab, rowsContainer);
                spawnedRows.Add(row);
            }

            for (int i = 0; i < spawnedRows.Count; i++)
            {
                if (i < slotCount)
                {
                    spawnedRows[i].SetSlot(slots[i]);
                }
                else
                {
                    spawnedRows[i].gameObject.SetActive(false);
                }
            }
        }

        private void RefreshFallbackText(IReadOnlyList<InventorySlot> slots)
        {
            if (inventoryListText == null)
            {
                return;
            }

            if (slots == null || slots.Count == 0)
            {
                inventoryListText.text = "Inventario vacío";
                return;
            }

            StringBuilder builder = new StringBuilder();

            foreach (InventorySlot slot in slots)
            {
                if (slot == null || slot.Item == null)
                {
                    continue;
                }

                string itemType = slot.Item.IsKey ? "Clave" : "Consumible";
                builder.AppendLine($"{slot.Item.DisplayName} x{slot.Quantity} ({itemType})");
            }

            inventoryListText.text = builder.ToString();
        }
    }
}
