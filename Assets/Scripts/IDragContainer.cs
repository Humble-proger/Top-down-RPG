public interface IDragContainer {
    ContainerInformation GetItem();
    bool Equal(InventoryEntry<string> item);
    bool IsEmpty();
}