using UnityEngine;
using UnityEngine.Events;

public class CrowdMoveTrigger : MonoBehaviour, IInteractable
{
    [Header("On Used")]
    [Tooltip("Runs after this trigger's crowd(s) have moved. Drag in a DoorHinge > Unlock() here if this should unlock something.")]
    [SerializeField] private UnityEvent onUsed;
    [SerializeField] private string itemName = "Item";

    [Header("Crowd Members")]
    [SerializeField] private SlideMover[] targetMovers;

    [Header("Destinations")]
    [SerializeField] private Transform[] targetDestinations;

    public void Interact()
    {
        int count = Mathf.Min(targetMovers.Length, targetDestinations.Length);

        for (int i = 0; i < count; i++)
        {
            if (targetMovers[i] != null && targetDestinations[i] != null)
            {
                targetMovers[i].MoveTo(targetDestinations[i].position);
            }
        }

        Debug.Log($"Crowd trigger used: {itemName}. Moved {count} crowd members.");
        onUsed.Invoke();

        gameObject.SetActive(false);
    }

    public string GetInteractPrompt()
    {
        return $"Press to use {itemName}";
    }
}