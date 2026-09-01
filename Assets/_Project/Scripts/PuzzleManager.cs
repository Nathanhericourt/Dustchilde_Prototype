using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Puzzle Setup")]
    [SerializeField] private string[] correctOrder;

    private List<string> playerOrder = new List<string>();
    private List<PuzzleItem> correctlySelectedItems = new List<PuzzleItem>();
    private bool puzzleSolved;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        UpdateStatusText();
    }

    public void SubmitItem(PuzzleItem item, string itemId, string displayLabel)
    {
        if (puzzleSolved) return;

        playerOrder.Add(itemId);
        int index = playerOrder.Count - 1;

        if (index >= correctOrder.Length || playerOrder[index] != correctOrder[index])
        {
            item.FlashWrong();
            ResetPuzzleAttempt();
            UpdateStatusText("Wrong order - try again!");
            return;
        }

        item.MarkCorrect();
        correctlySelectedItems.Add(item);
        UpdateStatusText($"Selected: {string.Join(", ", playerOrder)}");

        if (playerOrder.Count == correctOrder.Length)
        {
            puzzleSolved = true;
            UpdateStatusText("Puzzle Solved!");
            Debug.Log("Puzzle solved correctly!");
        }
    }

    private void ResetPuzzleAttempt()
    {
        playerOrder.Clear();
        foreach (var item in correctlySelectedItems)
        {
            item.ResetHighlight();
        }
        correctlySelectedItems.Clear();
    }

    private void UpdateStatusText(string overrideMessage = null)
    {
        if (statusText == null) return;
        statusText.text = overrideMessage ?? "Click the memories in the order they happened...";
    }
}