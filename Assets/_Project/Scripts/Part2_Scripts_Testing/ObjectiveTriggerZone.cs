using UnityEngine;

public class ObjectiveTriggerZone : MonoBehaviour
{
    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteCurrentStep();
        }
        else
        {
            Debug.LogWarning("ObjectiveTriggerZone: no ObjectiveManager found in scene.");
        }
    }
}
