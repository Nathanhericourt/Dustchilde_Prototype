using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "BrassKey";

    [Tooltip("Optional events: e.g., ObjectiveManager.Instance.AddProgress()")]
    [SerializeField] private UnityEvent onPickUp;

    public void Interact()
    {
        // Add item to player inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemName);
        }

        // Fire any extra events
        onPickUp?.Invoke();

        // Hide item from world
        gameObject.SetActive(false);
    }

    public string GetInteractPrompt()
    {
        return $"Press to pick up {itemName}";
    }
}