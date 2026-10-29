using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CarClickPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptUI;
    [SerializeField] private string nextSceneName;
    [Tooltip("Only works while this objective step is current. Set to -1 to ignore objectives.")]
    [SerializeField] private int requiredStepIndex = 1;

    private bool playerInside;
    private bool loading;

    private void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void Update()
    {
        if (loading) return;

        bool stepOk = requiredStepIndex < 0 ||
                      (ObjectiveManager.Instance != null && ObjectiveManager.Instance.IsCurrentStep(requiredStepIndex));
        bool canInteract = playerInside && stepOk;

        if (promptUI != null && promptUI.activeSelf != canInteract)
            promptUI.SetActive(canInteract);

        if (canInteract && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            loading = true;
            if (promptUI != null) promptUI.SetActive(false);
            if (requiredStepIndex >= 0 && ObjectiveManager.Instance != null)
                ObjectiveManager.Instance.CompleteStepIfCurrent(requiredStepIndex);
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }
}