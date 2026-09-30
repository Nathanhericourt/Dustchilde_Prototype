using UnityEngine;
using TMPro;
using System.Collections;

public class CrowdPuzzleStation : MonoBehaviour, IInteractable
{
    public enum Role { Pickup, Option }

    [Header("Role - what is this object?")]
    [SerializeField] private Role role = Role.Option;

    // PickUp Object Settings (for pickup object only)
    [Header("PICKUP ONLY - Item")]
    [SerializeField] private string itemName = "Item";

    [Header("PICKUP ONLY - Puzzle")]
    [Tooltip("Must match the Option Id of exactly one option")]
    [SerializeField] private string correctOptionId;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private float wrongFlashDuration = 1.5f;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("PICKUP ONLY - Crowd")]
    [SerializeField] private SlideMover crowdMover;
    [SerializeField] private Transform crowdDestination;

    [Header("PICKUP ONLY - Ordering rule")]
    [Tooltip("Tick this on the final crowd that must wait for the smaller ones.")]
    [SerializeField] private bool isFinalCrowd;
    [SerializeField] private int crowdsNeededFirst;

    // Option Object Settings
    [Header("OPTION ONLY")]
    [SerializeField] private string optionId;
    [SerializeField] private string displayLabel = "Option";
    [Tooltip("Drag the Pickup object in here")]
    [SerializeField] private CrowdPuzzleStation pickupStation;

    // Internal State
    private bool isUnlocked;
    private bool puzzleSolved;
    private OutlineHighlight outline;
    private Coroutine flashRoutine;

    // Shared by every ststion in the level - counts how many crowds have moved
    private static int crowdsMoved;

    private void Awake()
    {
        outline = GetComponent<OutlineHighlight>();
        crowdsMoved = 0; // reset at level start
    }

    public void Interact()
    {   
        if (role == Role.Pickup) HandlePickup();
        else HandleOption();
    }

    public string GetInteractPrompt()
    {
        if (role == Role.Pickup) return $"Press to pick up {itemName}";

        if (pickupStation != null && pickupStation.isUnlocked) return $"Press to choose: {displayLabel}";
        return "You need to find something first...";
    }

    // PickUp
    private void HandlePickup()
    {
        isUnlocked = true;
        UpdateStatus("You found something. Choose carefully...?");
        Debug.Log($"Picked up: {itemName}");

        // Hide the pickup instead of deleting it, because the options still need to talk to it
        foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = false;
    }

    // Option
    private void HandleOption()
    {
        if (pickupStation == null)
        {
            Debug.LogWarning($"{name}: Pickup Station is not assigned!");
            return;
        }
        pickupStation.CheckOption(this);
    }

    // Runs on the Pickup Onject
    public void CheckOption(CrowdPuzzleStation option)
    {
        if (!isUnlocked)
        {
            UpdateStatus("You need to find something first...");
            return;
        }

        if (puzzleSolved) return;

        // Wrong Option
        if (option.optionId != correctOptionId)
        {
            option.FlashHighlight(wrongColor, wrongFlashDuration);
            UpdateStatus("wrong choice - try again.");
            return;
        }

        // Right Option
        if (isFinalCrowd && crowdsMoved < crowdsNeededFirst)
        {
            Debug.Log("The crowd ignores you - move the smaller crowds first.");
            UpdateStatus("They won't listen yet - move the smaller crowds first.");
            return;
        }

        // Success
        puzzleSolved = true;
        option.ShowHighlight(correctColor);
        UpdateStatus($"{option.displayLabel} was right!");

        if (crowdMover != null && crowdDestination != null)
        {
            crowdMover.MoveTo(crowdDestination.position);
        }

        crowdsMoved++;
    }

    // Highlight Helpers
    public void ShowHighlight(Color color)
    {
        if (outline != null) outline.Show(color);
    }

    public void ResetHighlight()
    {
        if (outline != null) outline.Hide();
    }

    public void FlashHighlight(Color color, float duration)
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine(color, duration));
    }

    private IEnumerator FlashRoutine(Color color, float duration)
    {
        ShowHighlight(color);
        yield return new WaitForSeconds(duration);
        ResetHighlight();
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null) statusText.text = message;
    }
}
