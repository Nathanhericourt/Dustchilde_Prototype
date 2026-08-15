using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer = ~0;
    [SerializeField] private Transform cameraTransform;

    [Header("UI")]
    [SerializeField] private GameObject interactPromptObject;
    [SerializeField] private TextMeshProUGUI interactPromptText;

    private PlayerControls controls;
    private IInteractable currentInteractable;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                ShowPrompt(currentInteractable.GetInteractPrompt());
                return;
            }
        }

        currentInteractable = null;
        HidePrompt();
    }

    private void TryInteract()
    {
        currentInteractable?.Interact();
    }

    private void ShowPrompt(string message)
    {
        if (interactPromptObject == null) return;
        interactPromptObject.SetActive(true);
        interactPromptText.text = message;
    }

    private void HidePrompt()
    {
        if (interactPromptObject == null) return;
        interactPromptObject.SetActive(false);
    }
}