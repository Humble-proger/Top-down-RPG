using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string DialogueID;
    public List<DialogueNode> Nodes;
    public string StartNodeID = "start";

    public DialogueNode GetNodeByID(string nodeID)
    {
        return Nodes.Find(node => node.NodeID == nodeID);
    }
}