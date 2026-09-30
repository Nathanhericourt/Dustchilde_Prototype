using UnityEngine;

public class PuzzleDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredItem = "BrassKey";
    [SerializeField] private string lockedMessage = "Door is locked. Requires key.";

    private bool isUnlocked = false;

    public void Interact()
    {
        if (isUnlocked) return;

        // Check inventory for item
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItem))
        {
            isUnlocked = true;
            Debug.Log($"Door unlocked using {requiredItem}");

            // Opene/Hide door object
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"Door locked! You need: {requiredItem}");
        }
    }

    public string GetInteractPrompt()
    {
        if (isUnlocked) return "";

        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItem))
        {
            return $"Press to unlock door with {requiredItem}";
        }

        return lockedMessage;
    }
}
