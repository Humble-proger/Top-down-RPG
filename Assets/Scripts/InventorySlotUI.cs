using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IDragContainer
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _count;
    
    private int _slotIndex;
    private string _inventoryId;
    private string _itemId = string.Empty;
    private Sprite _iconSlot = null;
    private int _quantity = 0;
    private ItemType _itemType;

    string IDragContainer.ItemId { get => _itemId; }

    public ContainerInformation GetItem()
    {
        return new ContainerInformation { ContainerId=_inventoryId, NumSlot=_slotIndex, Quantity=_quantity, ItemType=_itemType };
    }

    public void Initialized(int slotIndex, string inventoryId)
    {
        _slotIndex = slotIndex;
        _inventoryId = inventoryId;
    }

    public bool IsEmpty() => string.IsNullOrEmpty(_itemId);

    public void UpdateVisual(InventoryEntry<ItemData> item)
    {
        if (item) {
            _itemId = item.Item.ItemID;
            _iconSlot = item.Item.Icon;
            _quantity = item.Count;
            _itemType = item.Item.Type;

            _icon.sprite = _iconSlot;
            if (_quantity > 1)
                _count.text = _quantity.ToString();
            else
                _count.text = string.Empty;
            return;
        }
        _itemId = string.Empty;
        _quantity = 0;
        _iconSlot = null;
        _icon.sprite = null;
        _count.text = string.Empty;
        _itemType = default;
    }
}
