using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "RPG/Item")]
public class ItemData : ScriptableObject
{
    public string ItemID;
    public string ItemName;
    public ItemType Type;
    public Sprite Icon;
    public int MaxStack = 1;
    public int Value;

    [TextArea] public string Description;

    // ��� ����������
    public int Damage;
    public int Defense;

    // ��� consumable
    public int HealthRestore;
    public int ManaRestore;
}