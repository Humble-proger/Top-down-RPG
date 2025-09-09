using TMPro;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour, IDragContainer
{
    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private TextMeshProUGUI _count;
    
    private int _slotIndex;
    private string _inventoryId;
    private string _itemId = string.Empty;
    private Sprite _iconSlot = null;
    private int _quantity = 0;

    public bool Equal(InventoryEntry<string> item) => item.Item == _itemId;

    public ContainerInformation GetItem()
    {
        throw new System.NotImplementedException();
    }

    public void Initialized(int slotIndex, string inventoryId)
    {
        _slotIndex = slotIndex;
        _inventoryId = inventoryId;
    }

    public bool IsEmpty()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateVisual(InventoryEntry<ItemData> item)
    {
        _itemId = item.Item.ItemID;
        _iconSlot = item.Item.Icon;
        _quantity = item.Count;

    }
}
