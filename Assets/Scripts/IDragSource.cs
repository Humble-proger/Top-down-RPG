public interface IDragSource<Item> 
{
    // Получить хранимый предмет и его количество без очищения
    InventoryEntry<Item> GetItem();
    // Нужно в случае если в переданном слоте происходит переполнение.
    int RemoveItem(int diff);
    //Очистка слота
    void RemoveItem();
    ContainerInformation GetSenderInformation();
}