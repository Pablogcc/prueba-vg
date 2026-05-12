using System;
using UnityEngine;

namespace AdventureSurvival.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int quantity;

        public InventorySlot(ItemData item, int quantity)
        {
            this.item = item;
            this.quantity = Mathf.Max(0, quantity);
        }

        public ItemData Item => item;
        public int Quantity => quantity;
        public bool IsEmpty => item == null || quantity <= 0;
        public bool IsFull => item != null && quantity >= item.MaxStack;
        public int FreeSpace => item == null ? 0 : Mathf.Max(0, item.MaxStack - quantity);

        public int Add(int amount)
        {
            if (item == null || amount <= 0)
            {
                return amount;
            }

            int acceptedAmount = Mathf.Min(amount, FreeSpace);
            quantity += acceptedAmount;
            return amount - acceptedAmount;
        }

        public int Remove(int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            int removedAmount = Mathf.Min(amount, quantity);
            quantity -= removedAmount;
            return removedAmount;
        }
    }
}
