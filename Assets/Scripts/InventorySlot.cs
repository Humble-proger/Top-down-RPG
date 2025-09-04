using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private TextMeshProUGUI _count;
    
    public ItemData Item;
    public int Quantity;
    public bool IsEmpty => Item == null || Quantity < 1;
}