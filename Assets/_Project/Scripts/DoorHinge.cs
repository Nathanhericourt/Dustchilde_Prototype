using UnityEngine;
using System.Collections;

public class DoorHinge : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private Transform doorPivot; // the object that actually rotates (parent of model + collider)
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openDuration = 1f;

    private bool isOpen;
    private Coroutine rotateRoutine;

    public void Interact()
    {
        isOpen = !isOpen;

        float targetY = isOpen ? openAngle : 0f;
        Quaternion targetRotation = Quaternion.Euler(0f, targetY, 0f);

        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
        rotateRoutine = StartCoroutine(RotateTo(targetRotation));
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        Quaternion startRotation = doorPivot.localRotation;
        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / openDuration;
            doorPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        doorPivot.localRotation = targetRotation;
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "Press to close door" : "Press to open door";
    }
}