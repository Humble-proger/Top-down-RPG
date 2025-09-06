public interface IInventoryHandler<DataType>
{
    ContainerType ContainerType { get; }
    ContainerType[] AllowedSenders { get; }
    ContainerType[] PermittedRecipients { get; }

    int TryAddItem(InventoryEntry<DataType> item, int slotIndex);
    int TrySubItem(int diff, int slotIndex);
    bool TryRemoveItem(int slotIndex);
    bool TrySwapItems(int fromSlot, int toSlot);
}