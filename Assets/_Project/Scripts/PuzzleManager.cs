using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Puzzle Setup")]
    [Tooltip("The correct order items must be clicked in, using their Item ID.")]
    [SerializeField] private string[] correctOrder;

    private List<string> playerOrder = new List<string>();
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

    public void SubmitItem(string itemId, string displayLabel)
    {
        if (puzzleSolved) return;

        playerOrder.Add(itemId);
        int index = playerOrder.Count - 1;

        // Wrong item at this point in the sequence
        if (index >= correctOrder.Length || playerOrder[index] != correctOrder[index])
        {
            Debug.Log($"Wrong order! Reset. You picked: {displayLabel}");
            playerOrder.Clear();
            UpdateStatusText("Wrong order - try again!");
            return;
        }

        UpdateStatusText($"Selected: {string.Join(", ", playerOrder)}");

        if (playerOrder.Count == correctOrder.Length)
        {
            puzzleSolved = true;
            UpdateStatusText("Puzzle Solved!");
            Debug.Log("Puzzle solved correctly!");
        }
    }

    private void UpdateStatusText(string overrideMessage = null)
    {
        if (statusText == null) return;
        statusText.text = overrideMessage ?? "Click the memories in the order they happened...";
    }
}