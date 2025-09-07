using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "RPG/Inventory")]
public class InventoryConfig : ScriptableObject 
{
    public ContainerType ContainerType;
    public ContainerType[] AllowedSenders;
    public ContainerType[] AllowedRecipients;
    public ItemType[] AllowedItems;
    public int Capacity;
}