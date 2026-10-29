using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryHint : MonoBehaviour
{
    [SerializeField] private GameObject hintUI;
    [SerializeField] private bool showOnStart = false;
    [Tooltip("Hide the hint after this many seconds. 0 = only hide when I is pressed.")]
    [SerializeField] private float autoHideSeconds = 0f;

    private bool showing;
    private bool dismissed;
    private float timer;

    private void Start()
    {
        if (hintUI != null) hintUI.SetActive(false);
        if (showOnStart) ShowHint();
    }

    // Hook this up to the pickups' On Pick Up event
    public void ShowHint()
    {
        if (dismissed || showing) return;
        showing = true;
        timer = 0f;
        if (hintUI != null) hintUI.SetActive(true);
    }

    private void Update()
    {
        if (!showing) return;

        timer += Time.deltaTime;

        bool pressedI = Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame;
        bool timedOut = autoHideSeconds > 0f && timer >= autoHideSeconds;

        if (pressedI || timedOut)
        {
            showing = false;
            dismissed = true;
            if (hintUI != null) hintUI.SetActive(false);
        }
    }
}