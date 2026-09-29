using UnityEngine;
using System.Collections;

public class DoorHinge : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openDuration = 1f;

    [Header("Lock")]
    [Tooltip("If true, this door won't open until Unlock() is called (e.g. from an NPC's On Dialogue Complete event).")]
    [SerializeField] private bool startsLocked = false;
    [SerializeField] private string lockedPrompt = "It's locked...";

    private bool isLocked;

    private bool hasOpened;
    private Coroutine rotateRoutine;

    public void Interact()
    {
        if (isLocked) return;
        if (hasOpened) return; // already open - do nothing further

        hasOpened = true;
        Quaternion targetRotation = Quaternion.Euler(0f, openAngle, 0f);

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
        if (isLocked) return lockedPrompt;
        return hasOpened ? "" : "Press to open door";
    }

    private void Awake()
    {
        isLocked = startsLocked;
    }

    public void Unlock()
    {
        isLocked = false;
    }
}