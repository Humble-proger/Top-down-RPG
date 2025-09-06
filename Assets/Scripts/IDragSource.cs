public interface IDragSource<Item> 
{
    // �������� �������� ������� � ��� ���������� ��� ��������
    InventoryEntry<Item> GetItem();
    // ����� � ������ ���� � ���������� ����� ���������� ������������.
    int RemoveItem(int diff);
    //������� �����
    void RemoveItem();
    ContainerInformation GetSenderInformation();
}