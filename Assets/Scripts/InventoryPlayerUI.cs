using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayerUI : MonoBehaviour 
{
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private Transform _itemsInventoryContainer;
    [SerializeField] private Transform _armorInventoryContainer;
    [SerializeField] private Transform _ammoInventoryContainer;
    [SerializeField] private Transform _inventoryContainer;
    [SerializeField] private InventoryConfig _configPlayerInventory;
    [SerializeField] private InventoryConfig _configArmorInventory;
    [SerializeField] private InventoryConfig _configAmmoInventory;
    
    private string _inventoryItemId = string.Empty;
    private string _inventoryArmorId = string.Empty;
    private string _inventoryAmmoId = string.Empty;
    private string _inventoryAdditionalItemId = string.Empty;
    private List<InventorySlotUI> _inventorySlots = new ();
    private List<InventorySlotUI> _armorSlots = new();
    private List<InventorySlotUI> _ammoSlots = new();

    private void Start()
    {
        
        _inventoryItemId = InventoryManager.Instance.CreateInventory(_configPlayerInventory);
        _inventoryAmmoId = InventoryManager.Instance.CreateInventory(_configAmmoInventory);
        _inventoryArmorId = InventoryManager.Instance.CreateInventory(_configArmorInventory);

        InitializedSlotsForInventory(ref _inventoryItemId, ref _itemsInventoryContainer, ref _inventorySlots, _configPlayerInventory.Capacity);
        InitializedSlotsForInventory(ref _inventoryArmorId, ref _armorInventoryContainer, ref _armorSlots, _configArmorInventory.Capacity);
        InitializedSlotsForInventory(ref _inventoryAmmoId, ref _ammoInventoryContainer, ref _ammoSlots, _configAmmoInventory.Capacity);

        if (InventoryManager.Instance.TryGetInventory(_inventoryItemId, out Inventory itemInventory))
            itemInventory.InventorySlotUpdate += UpdatePlayerInventory;
        else
        {
            LoggerService.Critical("(InventoryPlayerUI) Couldn't get the created inventory.");
            return;
        }
        if (InventoryManager.Instance.TryGetInventory(_inventoryAmmoId, out Inventory ammoInventory))
            ammoInventory.InventorySlotUpdate += UpdateAmmoInventory;
        else
        {
            LoggerService.Critical("(InventoryPlayerUI) Couldn't get the created inventory.");
            return;
        }
        if (InventoryManager.Instance.TryGetInventory(_inventoryArmorId, out Inventory armorInventory))
            armorInventory.InventorySlotUpdate += UpdateArmorInventory;
        else
        {
            LoggerService.Critical("(InventoryPlayerUI) Couldn't get the created inventory.");
            return;
        }
    }

    public void Enable() => _inventoryContainer.gameObject.SetActive(true);

    public void Disable() => _inventoryContainer.gameObject.SetActive(false);

    private void UpdatePlayerInventory(int arg1, InventoryEntry<ItemData> entry)
    {
        _inventorySlots[arg1].UpdateVisual(entry);
    }
    private void UpdateAmmoInventory(int arg1, InventoryEntry<ItemData> entry)
    {
        _ammoSlots[arg1].UpdateVisual(entry);
    }
    private void UpdateArmorInventory(int arg1, InventoryEntry<ItemData> entry)
    {
        _armorSlots[arg1].UpdateVisual(entry);
    }

    private void InitializedSlotsForInventory(ref string inventoryId, ref Transform itemContainer, ref List<InventorySlotUI> list, int capacity) {
        InventorySlotUI slot;
        for (int i = 0; i < capacity; i++)
        {
            slot = Instantiate(_slotPrefab, itemContainer);
            slot.Initialized(i, inventoryId);
            list.Add(slot);
        }
    }
}