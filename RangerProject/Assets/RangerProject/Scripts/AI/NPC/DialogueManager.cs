using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public NPC npc; // Updated to use NPC ScriptableObject

    bool isTalking = false;
    float distance;
    float curResponseTracker = 0;

    public GameObject player;
    public GameObject dialogueUI;

    public TMP_Text npcName;
    public TMP_Text npcDialogueBox;
    public TMP_Text playerResponse;

    // Start is called before the first frame update
    void Start()
    {
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance <= 2.5f)
        {
            // Scroll down to go to the next response
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                curResponseTracker++;
                if (curResponseTracker >= npc.playerResponse.Length)
                {
                    curResponseTracker = npc.playerResponse.Length - 1;
                }
            }
            // Scroll up to go to the previous response
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                curResponseTracker--;
                if (curResponseTracker < 0)
                {
                    curResponseTracker = 0;
                }
            }

            // Trigger dialogue with the NPC
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                StartConversation();
            }
            else if (Input.GetKeyDown(KeyCode.E) && isTalking)
            {
                EndDialogue();
            }

            // Display player's response based on the current response tracker index
            if (curResponseTracker < npc.playerResponse.Length)
            {
                playerResponse.text = npc.playerResponse[(int)curResponseTracker];
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    int npcDialogueIndex = Mathf.Clamp((int)curResponseTracker + 1, 0, npc.npcDialogue.Length - 1);
                    npcDialogueBox.text = npc.npcDialogue[npcDialogueIndex];
                }
            }
        }
    }

    void StartConversation()
    {
        isTalking = true;
        curResponseTracker = 0;
        dialogueUI.SetActive(true);
        npcName.text = npc.npcName;
        npcDialogueBox.text = npc.npcDialogue[0];
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);
    }
}
