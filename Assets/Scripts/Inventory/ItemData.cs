using UnityEngine;

namespace AdventureSurvival.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Adventure Survival/Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private string itemId = "new_item";
        [SerializeField] private string displayName = "New Item";
        [TextArea]
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private ItemType itemType = ItemType.Consumable;

        [Header("Stacking")]
        [SerializeField] private int maxStack = 1;

        [Header("Consumable Effects")]
        [SerializeField] private int healAmount;
        [SerializeField] private bool removeWhenConsumed = true;

        public string ItemId => itemId;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public ItemType ItemType => itemType;
        public int MaxStack => Mathf.Max(1, maxStack);
        public int HealAmount => healAmount;
        public bool RemoveWhenConsumed => removeWhenConsumed;
        public bool IsConsumable => itemType == ItemType.Consumable;
        public bool IsKey => itemType == ItemType.Key;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                itemId = name;
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = itemId;
            }

            maxStack = Mathf.Max(1, maxStack);
            healAmount = Mathf.Max(0, healAmount);
        }
    }
}
