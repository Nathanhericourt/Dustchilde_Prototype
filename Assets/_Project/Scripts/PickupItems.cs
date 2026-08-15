using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Item";

    public void Interact()
    {
        Debug.Log($"Picked up: {itemName}");
        gameObject.SetActive(false); // temp behavior: item disappears when picked up
    }

    public string GetInteractPrompt()
    {
        return $"Press to pick up {itemName}";
    }
}