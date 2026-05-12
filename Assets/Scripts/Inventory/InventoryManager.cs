using System;
using System.Collections.Generic;
using AdventureSurvival.Player;
using UnityEngine;

namespace AdventureSurvival.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager instance;

        [SerializeField] private bool keepBetweenScenes = true;
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        public static InventoryManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = FindObjectOfType<InventoryManager>();

                if (instance != null)
                {
                    return instance;
                }

                GameObject managerObject = new GameObject(nameof(InventoryManager));
                instance = managerObject.AddComponent<InventoryManager>();
                return instance;
            }
        }

        public event Action<IReadOnlyList<InventorySlot>> InventoryChanged;

        public IReadOnlyList<InventorySlot> Slots => slots;

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

        private void Start()
        {
            NotifyInventoryChanged();
        }

        public bool TryAddItem(ItemData item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
            {
                return false;
            }

            int remainingQuantity = quantity;

            foreach (InventorySlot slot in slots)
            {
                if (slot.Item == item && !slot.IsFull)
                {
                    remainingQuantity = slot.Add(remainingQuantity);

                    if (remainingQuantity <= 0)
                    {
                        NotifyInventoryChanged();
                        return true;
                    }
                }
            }

            while (remainingQuantity > 0)
            {
                int amountForNewSlot = Mathf.Min(remainingQuantity, item.MaxStack);
                slots.Add(new InventorySlot(item, amountForNewSlot));
                remainingQuantity -= amountForNewSlot;
            }

            NotifyInventoryChanged();
            return true;
        }

        public bool HasItem(ItemData item, int quantity = 1)
        {
            return GetItemQuantity(item) >= quantity;
        }

        public int GetItemQuantity(ItemData item)
        {
            if (item == null)
            {
                return 0;
            }

            int totalQuantity = 0;

            foreach (InventorySlot slot in slots)
            {
                if (slot.Item == item)
                {
                    totalQuantity += slot.Quantity;
                }
            }

            return totalQuantity;
        }

        public bool TryConsumeItem(ItemData item, PlayerHealth targetPlayer = null)
        {
            if (item == null || !item.IsConsumable || !HasItem(item))
            {
                return false;
            }

            if (targetPlayer != null && item.HealAmount > 0)
            {
                targetPlayer.Heal(item.HealAmount);
            }

            if (item.RemoveWhenConsumed)
            {
                RemoveItem(item, 1);
            }
            else
            {
                NotifyInventoryChanged();
            }

            return true;
        }

        public bool RemoveItem(ItemData item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
            {
                return false;
            }

            int remainingQuantity = quantity;

            for (int i = slots.Count - 1; i >= 0; i--)
            {
                InventorySlot slot = slots[i];

                if (slot.Item != item)
                {
                    continue;
                }

                remainingQuantity -= slot.Remove(remainingQuantity);

                if (slot.IsEmpty)
                {
                    slots.RemoveAt(i);
                }

                if (remainingQuantity <= 0)
                {
                    NotifyInventoryChanged();
                    return true;
                }
            }

            NotifyInventoryChanged();
            return false;
        }

        public void Clear()
        {
            slots.Clear();
            NotifyInventoryChanged();
        }

        private void NotifyInventoryChanged()
        {
            InventoryChanged?.Invoke(slots);
        }
    }
}
