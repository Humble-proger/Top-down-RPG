using System;
using System.Linq;

[System.Serializable]
public class Inventory : IInventoryHandler<ItemData>
{
    public ContainerType ContainerType => _config.ContainerType;
    public ContainerType[] AllowedSenders => _config.AllowedSenders;
    public ContainerType[] AllowedRecipients => _config.AllowedRecipients;
    public ItemType[] AllowedItems => _config.AllowedItems;
    public string InventoryId => _inventoryId;
    public int Capacity => _config.Capacity;

    public event Action<int, InventoryEntry<ItemData>> InventorySlotUpdate;

    private readonly InventoryConfig _config;
    private readonly InventoryEntry<ItemData>[] _items;
    private readonly string _inventoryId;

    public Inventory(InventoryConfig config, string inventoryId)
    {
        _config = config;
        _items = new InventoryEntry<ItemData>[_config.Capacity];
        _inventoryId = inventoryId;
    }

    public bool IndexRange(int index) {
        if (index > 0 && index < Capacity)
            return true;
        LoggerService.Debug($"(Inventory) Index {index} out of range. The current inventory capacity is {Capacity}.");
        return false;
    }
    public bool CanAddItem(ItemType type) {
        if (AllowedItems.Contains(type))
            return true;
        LoggerService.Debug($"(Inventory) The {type} type is prohibited in the current inventory");
        return false;
    }
    public bool CanGetItem(int slotIndex) {
        if (!IndexRange(slotIndex))
            return false;
        if (!GetItem(slotIndex)) {
            LoggerService.Debug("(Inventory) The passed element is empty. Use the TryRemoveItem method to remove an item.");
            return false;
        }
        return true;
    }

    public bool TryGetItem(int slotIndex, out InventoryEntry<ItemData> item) {
        if (CanGetItem(slotIndex)) {
            item = GetItem(slotIndex);
            return true;
        }
        item = new InventoryEntry<ItemData>();
        return false;
    }

    public InventoryEntry<ItemData> GetItem(int slotIndex) {
        return _items[slotIndex];
    }

    public bool TrySetItem(int slotIndex, InventoryEntry<ItemData> item) {
        if (IndexRange(slotIndex) && CanAddItem(item.Item.Type)) {
            SetItem(slotIndex, item);
            return true;
        }
        return false;
    }

    public void SetItem(int slotIndex, InventoryEntry<ItemData> item) {
        _items[slotIndex] = item;
        InventorySlotUpdate?.Invoke(slotIndex, item);
    }

    public bool TryAddItem(InventoryEntry<ItemData> item, int slotIndex, out int passedObjectsCount)
    {
        passedObjectsCount = -1;
        if (!IndexRange(slotIndex))
            return false;

        if (!item) 
        {
            LoggerService.Debug("(Inventory) The passed element is empty. Use the TryRemoveItem method to remove an item.");
            return false;
        }

        if (!CanAddItem(item.Item.Type))
            return false;
        passedObjectsCount = AddItem(item, slotIndex);
        return true;
    }

    public int AddItem(InventoryEntry<ItemData> fromItem, int slotIndex) {
        InventoryEntry<ItemData> toItem = GetItem(slotIndex);
        if (toItem & fromItem)
        {
            int newCount = toItem.Count + fromItem.Count;
            int diff = Math.Clamp(newCount - toItem.Item.MaxStack, 0, fromItem.Count);
            toItem.Count = newCount - diff;
            SetItem(slotIndex, toItem);
            return fromItem.Count - diff;
        }
        else if (!toItem)
        {
            int diff = Math.Clamp(fromItem.Count - fromItem.Item.MaxStack, 0, fromItem.Count);
            fromItem.Count -= diff;
            SetItem(slotIndex, fromItem);
            return fromItem.Count;
        }
        return -1;
    }

    public bool TryRemoveItem(int slotIndex)
    {
        if (CanGetItem(slotIndex)) {
            RemoveItem(slotIndex);
            return true;
        }
        return false;
    }

    public void RemoveItem(int slotIndex) {
        InventoryEntry<ItemData> item = GetItem(slotIndex);
        item.Clear();
        SetItem(slotIndex, item);
    }

    public bool TrySubItem(int diff, int slotIndex, out int removeObjectCount)
    {
        removeObjectCount = -1;
        if (!CanGetItem(slotIndex))
            return false;
        
        if (diff <= 0) {
            LoggerService.Debug("(Inventory) This action does not make sense. The \"diff\" parameter must be greater than 0. Use the \"TryAddItem\" method.");
            return false;
        }
        removeObjectCount = SubItem(diff, slotIndex);
        return true;
    }

    public int SubItem(int diff, int slotIndex) {
        InventoryEntry<ItemData> item = GetItem(slotIndex);
        int newCount = item.Count - diff;
        if (newCount <= 0)
        {
            if (!TryRemoveItem(slotIndex)) return -1;
            return diff + newCount;
        }
        else
        {
            item.Count = newCount;
            SetItem(slotIndex, item);
            return diff;
        }
    }

    public bool TrySwapItems(int fromSlot, int toSlot)
    {
        if (!IndexRange(fromSlot) || !IndexRange(toSlot))
            return false;
        if (fromSlot == toSlot) {
            LoggerService.Debug($"(Inventory) The fromSlot index matches the toSlot. The operation makes no sense. (fromSlot = {fromSlot}, toSlot = {toSlot})");
            return false;
        }
        if (!_items[fromSlot] && !_items[toSlot]) {
            LoggerService.Debug($"(Inventory) The elements corresponding to the passed indices are empty. The operation makes no sense.");
            return false;
        }
        SwapItems(fromSlot, toSlot);
        return true;
    }

    public void SwapItems(int fromSlot, int toSlot) {
        (_items[fromSlot], _items[toSlot]) = (_items[toSlot], _items[fromSlot]);
        InventorySlotUpdate?.Invoke(fromSlot, _items[fromSlot]);
        InventorySlotUpdate?.Invoke(toSlot, _items[toSlot]);
    }

    public void Reset() {
        for (int i = 0; i < Capacity; i++)
        {
            InventoryEntry<ItemData> item = GetItem(i);
            item.Clear();
            SetItem(i, item);
        }
    }
}
