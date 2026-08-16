using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Queue<string> lineQueue = new Queue<string>();
    private bool dialogueActive;

    public bool IsDialogueActive => dialogueActive;

    private void Awake()
    {
        // Simple singleton so any NPC can call DialogueManager.Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialogue(string speakerName, string[] lines)
    {
        dialogueActive = true;
        dialoguePanel.SetActive(true);
        speakerNameText.text = speakerName;

        lineQueue.Clear();
        foreach (string line in lines)
        {
            lineQueue.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lineQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lineQueue.Dequeue();
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}