using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Lost sorting names of collected items
    public List<string> items = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Adds item to inventory
    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Added item to inventory: " + itemName);
    }

    // Checks if the player has a specific item(for puzzles/doors)
    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }
}
