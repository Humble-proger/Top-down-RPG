public interface IDragDestination<Item>
{
    /*
     ���� �������� ���������� ��� ������������ �����������, ������ �����������
    �� �������� ��� �� ����� �����������, ��� � �����������.
     */
    int AddItem(InventoryEntry<Item> item);
    
    /*
     ���� �������� ���������� ������ ������ � ���� �� ����������,
    �� ��������� senderSlot ��� ����, ����� ���������� ������� Switch ����� �������.
     */
    int AddItem(InventoryEntry<Item> item, int senderSlot);
    bool Equal(Item item);
    bool IsEmpty();
    ContainerInformation GetDestinationInformation();
}