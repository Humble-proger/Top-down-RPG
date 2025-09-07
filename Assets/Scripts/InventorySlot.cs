using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour, IDragContainer<ItemData>
{
    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private TextMeshProUGUI _count;
    
    public ItemData Item;
    public int Quantity;

    private int _slotIndex;
    public string _inventoryId;
    private IInventoryHandler<ItemData> _inventorySystem;

    public bool IsEmpty() => Item == null;
    public bool Equal(ItemData item) => item.ItemID == Item.ItemID;

    public void Initialized(int slotIndex, IInventoryHandler<ItemData> system)
    {
       _slotIndex = slotIndex;
        _inventorySystem = system;
    }

    public void UpdateVisual(ItemData item)
    {
        Item = item;
        if (item != null)
        {
            _icon.sprite = item.Icon;
            _icon.enabled = true;
        }
        else {
            RemoveItem();
        }
    }

    public void UpdateCount(int newCount)
    {
        Quantity = newCount;
        if (newCount > 0)
        {
            _count.text = newCount.ToString();
        }
        else {
            RemoveItem();
        }
    }
    public int AddItem(InventoryEntry<ItemData> item)
    {
        if (_inventorySystem != null) {
            return _inventorySystem.TryAddItem(item, _slotIndex);
        }
        return -1;
    }
    public int AddItem(InventoryEntry<ItemData> item, int senderSlot)
    {
        if (_inventorySystem != null) {
            int passedObjectsCount = _inventorySystem.TryAddItem(item, _slotIndex);
            if (passedObjectsCount == 0)
            {
                if (_inventorySystem.TrySwapItems(_slotIndex, senderSlot))
                    return item.Count;
            }
            return passedObjectsCount;
        }
        return -1;
    }

    public int RemoveItem(int diff)
    {
        if (_inventorySystem != null) {
            return _inventorySystem.TrySubItem(diff, _slotIndex);
        }
        return -1;
    }

    public InventoryEntry<ItemData> GetItem()
    {
        return new InventoryEntry<ItemData>() { Item = Item, Count = Quantity };
    }

    public void RemoveItem()
    {
        _icon.enabled = false;
        Quantity = 0;
        _count.text = string.Empty;
        Item = null;
    }

    public ContainerInformation GetDestinationInformation()
    {
        return new ContainerInformation() { ContainerId = _inventoryId, ContainerType = _inventorySystem.ContainerType, Permitted = _inventorySystem.AllowedSenders, NumSlot = _slotIndex};
    }

    public ContainerInformation GetSenderInformation()
    {
        return new ContainerInformation() { ContainerId = _inventoryId, ContainerType=_inventorySystem.ContainerType, Permitted=_inventorySystem.AllowedSenders, NumSlot = _slotIndex };
    }
}
