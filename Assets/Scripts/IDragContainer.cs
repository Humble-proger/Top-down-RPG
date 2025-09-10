public interface IDragContainer {
    protected string ItemId { get; }
    ContainerInformation GetItem();
    public bool Equal(IDragContainer item) => ItemId == item.ItemId;
    bool IsEmpty();
}