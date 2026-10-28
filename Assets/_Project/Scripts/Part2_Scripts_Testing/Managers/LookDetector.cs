using UnityEngine;
using UnityEngine.InputSystem;

public class LookDetector : MonoBehaviour
{
    [SerializeField] private int stepIndex = 0;
    [SerializeField] private float lookThreshold = 500f;
    [SerializeField] private float startDelay = 1f;

    private float accumulated;
    private float timer;

    private void OnEnable()
    {
        accumulated = 0f;
        timer = 0f;
    }

    private void Update()
    {
        if (ObjectiveManager.Instance == null || !ObjectiveManager.Instance.IsCurrentStep(stepIndex))
            return;

        // Short grace period so the camera snap and any leftover mouse movement don't count
        timer += Time.deltaTime;
        if (timer < startDelay) return;

        if (Mouse.current == null) return;

        accumulated += Mouse.current.delta.ReadValue().magnitude;

        if (accumulated >= lookThreshold)
        {
            ObjectiveManager.Instance.CompleteStepIfCurrent(stepIndex);
            enabled = false;
        }
    }
}