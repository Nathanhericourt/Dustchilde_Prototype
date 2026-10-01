using UnityEngine;
using UnityEngine.Events;

public class CrowdNavMoveTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Item";
    [SerializeField] private CrowdNavGroup[] targetGroups;
    [SerializeField] private Transform[] targetDestinations;
    [SerializeField] private AudioClip pickupSound;

    [Header("On Used")]
    [SerializeField] private UnityEvent onUsed;

    public void Interact()
    {
        int count = Mathf.Min(targetGroups.Length, targetDestinations.Length);

        for (int i = 0; i < count; i++)
        {
            if (targetGroups[i] != null && targetDestinations[i] != null)
            {
                targetGroups[i].MoveTo(targetDestinations[i].position);
            }
        }

        Debug.Log($"Crowd trigger used: {itemName}. Moved {count} crowd members.");

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(pickupSound);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemName);
        }

        onUsed.Invoke();

        gameObject.SetActive(false);
    }

    public string GetInteractPrompt()
    {
        return $"Press to use {itemName}";
    }
}
