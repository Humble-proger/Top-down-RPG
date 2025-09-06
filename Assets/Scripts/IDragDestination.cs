public interface IDragDestination<Item>
{
    /*
     ≈сли действие происходит дл€ разрешЄнного отправител€, однако отправитель
    не €вл€етс€ тем же самым контейнером, что и приниматель.
     */
    int AddItem(InventoryEntry<Item> item);
    
    /*
     ≈сли действие происходит внутри одного и того же контейнера,
    то передаЄтс€ senderSlot дл€ того, чтобы расмотреть вариант Switch между слотами.
     */
    int AddItem(InventoryEntry<Item> item, int senderSlot);
    bool Equal(Item item);
    bool IsEmpty();
    ContainerInformation GetDestinationInformation();
}