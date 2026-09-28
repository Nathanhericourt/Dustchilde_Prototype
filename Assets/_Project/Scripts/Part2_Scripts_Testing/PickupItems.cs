using System;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Item";

    [Tooltip("Runs when the item is picked up. Drag in ObjectiveManager > AddProgress to count it.")]
    [SerializeField] private UnityEvent onPickUp;

    public void Interact()
    {
        Debug.Log($"Picked up: {itemName}");
        onPickUp.Invoke();
        gameObject.SetActive(false); // Item disappears when picked up
    }

    public string GetInteractPrompt()
    {
        return $"Press to pick up {itemName}";
    }
}