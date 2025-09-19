using System;

[Serializable]
public class DialogueOption
{
    public string Text;
    public string NextNodeID; // Куда ведёт этот выбор
    public QuestData RequiredQuest; // Квест required для показа опции
    public QuestStatus RequiredQuestStatus; // required статус квеста
    public bool IsAvailable => CheckAvailability();

    private bool CheckAvailability()
    {
        // Проверка условий для показа опции
        return true;
    }
}