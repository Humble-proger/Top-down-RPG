using System;
using System.Collections.Generic;

[System.Serializable]
public struct InventoryEntry<DataType> {
    public DataType Item;
    public int Count;

    public void Clear() {
        Item = default;
        Count = 0;
    }
    public static InventoryEntry<DataType> operator ++(InventoryEntry<DataType> item) {
        item.Count++;
        return item;
    }
    public static InventoryEntry<DataType> operator --(InventoryEntry<DataType> item)
    {
        item.Count--;
        return item;
    }
    public static bool operator true(InventoryEntry<DataType> item)
    {
        return item.Count > 0 && item.Item != null;
    }
    public static bool operator false(InventoryEntry<DataType> item)
    {
        return item.Count <= 0 || item.Item == null;
    }
    public static bool operator !(InventoryEntry<DataType> item)
    {
        return item.Count <= 0 || item.Item == null;
    }
    public static bool operator ==(InventoryEntry<DataType> item1, InventoryEntry<DataType> item2)
    {
        return item1.Item.Equals(item2.Item) && item1.Count == item2.Count;
    }
    public static bool operator !=(InventoryEntry<DataType> item1, InventoryEntry<DataType> item2)
    {
        return !item1.Item.Equals(item2.Item) || item1.Count != item2.Count;
    }
    public static bool operator &(InventoryEntry<DataType> item1, InventoryEntry<DataType> item2)
    {
        return item1.Equals(item2);
    }

    public override readonly bool Equals(object obj)
    {
        return obj is InventoryEntry<DataType> entry &&
               EqualityComparer<DataType>.Default.Equals(Item, entry.Item);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Item, Count);
    }
}