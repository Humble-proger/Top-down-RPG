using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quest")]
public class QuestData : ScriptableObject
{
    public string QuestID;
    public string QuestName;
    [TextArea] public string Description;

    public List<QuestObjective> Objectives;
    public List<ItemReward> ItemRewards;
    public int ExperienceReward;
    public int GoldReward;
}