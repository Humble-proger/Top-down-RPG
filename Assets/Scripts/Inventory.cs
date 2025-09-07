using System;

[System.Serializable]
public class Inventory : IInventoryHandler<ItemData>
{
    public ContainerType ContainerType => _containerType;
    public ContainerType[] AllowedSenders => _allowedSenders;
    public ContainerType[] AllowedRecipients => _allowedRecipients;
    public string InventoryId => _inventoryId;
    public InventoryEntry<ItemData>[] Items => _items;
    public int Capacity => _capacity;

    private readonly ContainerType _containerType;
    private readonly ContainerType[] _allowedSenders;
    private readonly ContainerType[] _allowedRecipients;
    private readonly InventoryEntry<ItemData>[] _items;
    private readonly string _inventoryId;
    private readonly int _capacity;

    public Inventory(ContainerType containerType, ContainerType[] allowedSenders, 
        ContainerType[] allowedRecipients, int capacity = 1, string inventoryId = null)
    {
        _containerType = containerType;
        _allowedSenders = allowedSenders;
        _allowedRecipients = allowedRecipients;
        _capacity = capacity;
        _items = new InventoryEntry<ItemData>[capacity];
        _inventoryId = string.IsNullOrEmpty(inventoryId) ? Guid.NewGuid().ToString() : inventoryId;
    }

    public int TryAddItem(InventoryEntry<ItemData> item, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Capacity)
        {
            LoggerService.Error($"Inventory Error! (TryAddItem) Index {slotIndex} out of range. The current inventory capacity is {Capacity}.");
            return -1;
        }

        if (!item) 
        {
            LoggerService.Warning("Inventory Warning! (TryAddItem) The passed element is empty. Use the TryRemoveItem method to remove an item.");
            return -1;
        }

        if (_items[slotIndex] & item)
        {
            int newCount = _items[slotIndex].Count + item.Count;
            int diff = Math.Clamp(newCount - _items[slotIndex].Item.MaxStack, 0, item.Count);
            _items[slotIndex].Count = newCount - diff;
            return item.Count - diff;
        }
        else if (!_items[slotIndex]) {
            int diff = Math.Clamp(item.Count - item.Item.MaxStack, 0, item.Count);
            item.Count -= diff;
            _items[slotIndex] = item;
            return item.Count;
        }
        return -1;
    }

    public bool TryRemoveItem(int slotIndex)
    {
        if (slotIndex > 0 || slotIndex >= Capacity) {
            LoggerService.Error($"Inventory Error! (TryRemoveItem) Index {slotIndex} out of range. The current inventory capacity is {Capacity}.");
            return false;
        }

        if (!_items[slotIndex]) {
            LoggerService.Info("Inventory Info! (TryRemoveItem) The item has already been cleared.");
            return true;
        }
        _items[slotIndex].Clear();
        return true;
    }

    public int TrySubItem(int diff, int slotIndex)
    {
        if (slotIndex > 0 || slotIndex >= Capacity)
        {
            LoggerService.Error($"Inventory Error! (TrySubItem) Index {slotIndex} out of range. The current inventory capacity is {Capacity}.");
            return -1;
        }
        if (!_items[slotIndex]) {
            LoggerService.Error("Inventory Error! (TrySubItem) The operation cannot be performed because the specified element is empty.");
            return -1;
        }
        if (diff <= 0) {
            LoggerService.Warning("Inventory Warning! (TrySubItem) This action does not make sense. The \"diff\" parameter must be greater than 0. Use the \"TryAddItem\" method.");
            return -1;
        }
        int newCount = _items[slotIndex].Count - diff;
        if (newCount <= 0)
        {
            if (!TryRemoveItem(slotIndex)) return -1;
            return diff + newCount;
        }
        else {
            _items[slotIndex].Count = newCount;
            return diff;
        }
        
    }

    public bool TrySwapItems(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= Capacity) {
            LoggerService.Error($"Inventory Error! (TrySwapItems) Index {fromSlot} out of range. The current inventory capacity is {Capacity}.");
            return false;
        }
        if (toSlot < 0 || toSlot >= Capacity) {
            LoggerService.Error($"Inventory Error! (TrySwapItems) Index {toSlot} out of range. The current inventory capacity is {Capacity}.");
            return false;
        }
        if (fromSlot == toSlot) {
            LoggerService.Error($"Inventory Error! (TrySwapItems) The fromSlot index matches the toSlot. The operation makes no sense. (fromSlot = {fromSlot}, toSlot = {toSlot})");
            return false;
        }
        if (!_items[fromSlot] && !_items[toSlot]) {
            LoggerService.Warning($"Inventory Warning! (TrySwapItems) The elements corresponding to the passed indices are empty. The operation makes no sense.");
            return false;
        }
        (_items[fromSlot], _items[toSlot]) = (_items[toSlot], _items[fromSlot]);
        return true;
    }
}