using UnityEngine;

public class PuzzleItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemId;
    [SerializeField] private string displayLabel = "Memory";

    public void Interact()
    {
        PuzzleManager.Instance.SubmitItem(itemId, displayLabel);
    }

    public string GetInteractPrompt()
    {
        return $"Press to select: {displayLabel}";
    }
}