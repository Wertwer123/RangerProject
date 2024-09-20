using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "ScriptableObjects/NPC", order = 1)]
public class NPC : ScriptableObject
{
    public string npcName;

    [TextArea(3, 15)]
    public string[] npcDialogue;

    [TextArea(3, 15)]
    public string[] playerResponse;
}
