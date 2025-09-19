using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode 
{
    public string NodeID;
    [TextArea(3, 5)] public string Text;
    public List<DialogueOption> Options;
    public string SpeakerName; // Имя говорящего
    public Sprite SpeakerIcon; // Иконка NPC
}