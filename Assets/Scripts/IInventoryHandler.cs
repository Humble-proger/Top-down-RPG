public interface IInventoryHandler<DataType>
{
    string InventoryId { get; }
    ContainerType ContainerType { get; }
    ContainerType[] AllowedSenders { get; }
    ContainerType[] AllowedRecipients { get; }
    ItemType[] AllowedItems { get; }
    int Capacity { get; }

    bool TryGetItem(int slotIndex, out InventoryEntry<DataType> item);
    bool TryAddItem(InventoryEntry<ItemData> item, int slotIndex, out int passedObjectsCount);
    bool TrySubItem(int diff, int slotIndex, out int removeObjectCount);
    bool TryRemoveItem(int slotIndex);
    bool TrySwapItems(int fromSlot, int toSlot);
}