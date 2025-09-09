using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryManager Instance { get; private set; }

    private Dictionary<string, Inventory> _inventories;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _inventories = new Dictionary<string, Inventory>();
        LoggerService.Info("(Inventory Manager) Object Init");
    }

    public string CreateInventory(InventoryConfig config) 
    {
        string newInventoryId = Guid.NewGuid().ToString();
        while (_inventories.TryAdd(newInventoryId, new Inventory(config, newInventoryId)))
            newInventoryId = Guid.NewGuid().ToString();
        LoggerService.Info($"(Inventory Manager) The inventory with id {newInventoryId} has been successfully created.");
        return newInventoryId;
    }

    public bool RemoveInventory(string inventoryId) 
    {
        bool result = _inventories.Remove(inventoryId);
        LoggerService.Debug($"(Inventory Manager) Remove inventory {inventoryId} - {result}");
        return result;
    }
    public bool TryGetInventory(string inventoryId, out Inventory inventory) {
        return _inventories.TryGetValue(inventoryId, out inventory);
    }

    public bool Contain(string inventoryId) {
        bool result = _inventories.ContainsKey(inventoryId);
        LoggerService.Info($"(Inventory Manager) Contain inventory {inventoryId} - {result}");
        return result;
    }

    public bool CanSwapBetweenInventories(ItemType fromType, ItemType toType,
        string fromInventoryId, string toInventoryId)
    {
        return _inventories[fromInventoryId].CanAddItem(toType) && 
            _inventories[toInventoryId].CanAddItem(fromType);
    }

    public bool TrySwapBetweenInventories(int fromSlot, int toSlot,
        string fromInventoryId, string toInventoryId)
    {
        if (fromInventoryId != toInventoryId)
        {
            if (!TryGetInventory(fromInventoryId, out Inventory fromInventory) ||
                !TryGetInventory(toInventoryId, out Inventory toInventory))
                return false;
            if (!fromInventory.TryGetItem(fromSlot, out InventoryEntry<ItemData> fromItem) ||
                !toInventory.TryGetItem(toSlot, out InventoryEntry<ItemData> toItem))
                return false;
            if (CanSwapBetweenInventories(fromItem.Item.Type, toItem.Item.Type,
                fromInventoryId, toInventoryId))
            {
                fromInventory.SetItem(fromSlot, toItem);
                toInventory.SetItem(toSlot, fromItem);
                return true;
            }
            return false;
        }
        else {
            if (!TryGetInventory(fromInventoryId, out Inventory inventory))
                return false;
            if (inventory.TrySwapItems(fromSlot, toSlot))
                return true;
            return false;
        }
    }

    private bool SendItem(int fromSlot, int toSlot, 
        ref Inventory fromInventory, ref Inventory toInventory, int countAdd = 1) {
        if (!fromInventory.TryGetItem(fromSlot, out InventoryEntry<ItemData> fromItem))
            return false;
        if (fromItem.Count < countAdd)
        {
            LoggerService.Debug($"(Inventory Manager) The requested number of objects to transfer ({countAdd}) exceeds the number currently stored ({fromItem.Count})");
            return false;
        }
        if (!toInventory.TryAddItem(new InventoryEntry<ItemData> { Item = fromItem.Item, Count = countAdd }, toSlot, out int passedObjectsCount) || passedObjectsCount <= 0)
            return false;
        LoggerService.Info($"(Inventory Manager) {passedObjectsCount} objects transferred.");
        int removedObjectCount = fromInventory.SubItem(passedObjectsCount, fromSlot);
        if (removedObjectCount < passedObjectsCount)
        {
            LoggerService.Error($"(Inventory Manager) Remove {removedObjectCount} object(s), but {passedObjectsCount} were expected.");
        }
        return true;
    }

    public bool TrySendBetweenInventories(int fromSlot, int toSlot,
        string fromInventoryId, string toInventoryId, int countAdd = 1)
    {
        if (fromInventoryId != toInventoryId)
        {
            if (!TryGetInventory(fromInventoryId, out Inventory fromInventory) ||
                !TryGetInventory(toInventoryId, out Inventory toInventory))
                return false;
            return SendItem(fromSlot, toSlot, ref fromInventory, ref toInventory, countAdd);
        }
        else {
            if (!TryGetInventory(fromInventoryId, out Inventory inventory))
                return false;
            return SendItem(fromSlot, toSlot, ref inventory, ref inventory, countAdd);
        }
    }
}