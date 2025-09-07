public interface IInventoryHandler<DataType>
{
    string InventoryId { get; }
    ContainerType ContainerType { get; }
    ContainerType[] AllowedSenders { get; }
    ContainerType[] AllowedRecipients { get; }
    InventoryEntry<DataType>[] Items { get; }
    int Capacity { get; }

    int TryAddItem(InventoryEntry<DataType> item, int slotIndex);
    int TrySubItem(int diff, int slotIndex);
    bool TryRemoveItem(int slotIndex);
    bool TrySwapItems(int fromSlot, int toSlot);
}