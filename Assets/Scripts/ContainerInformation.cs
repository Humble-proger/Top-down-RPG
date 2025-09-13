[System.Serializable]
public struct ContainerInformation 
{
    public string ContainerId;
    public int NumSlot;
    public int Quantity;
    public ItemType ItemType;

    public void Clear() {
        ContainerId = string.Empty;
        NumSlot = 0;
        Quantity = 0;
        ItemType = default;
    }

    public readonly bool IsEmpty() => ContainerId == string.Empty;
}