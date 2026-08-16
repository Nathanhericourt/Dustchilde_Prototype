using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string speakerName = "NPC";
    [TextArea(2, 4)]
    [SerializeField] private string[] dialogueLines;

    public void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive)
        {
            // Already talking - treat interact as "next line"
            DialogueManager.Instance.DisplayNextLine();
        }
        else
        {
            DialogueManager.Instance.StartDialogue(speakerName, dialogueLines);
        }
    }

    public string GetInteractPrompt()
    {
        return $"Press to talk to {speakerName}";
    }
}