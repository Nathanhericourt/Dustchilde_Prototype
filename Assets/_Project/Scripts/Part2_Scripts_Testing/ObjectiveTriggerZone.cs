using UnityEngine;

public class ObjectiveTriggerZone : MonoBehaviour
{
    [Tooltip("Which objective step (by position in the ObjectiveManager's Steps list, starting at 0) " + "must be active for this zone to work. -1 = always allow, regardless of step.")]    
    [SerializeField] private int requiredStepIndex = -1;
    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (ObjectiveManager.Instance == null) return;

        bool wrongStep = requiredStepIndex >= 0 && ObjectiveManager.Instance.CurrentStepIndex != requiredStepIndex;

        if (wrongStep)
        {
            Debug.Log("Not ready for this yet - finish earlier objectives first.");
            return; // don't comsume the trigger - player can come back later
        }

        hasTriggered = true;
        ObjectiveManager.Instance.CompleteCurrentStep();
    }
}
