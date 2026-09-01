using UnityEngine;
using System.Collections;

public class PuzzleItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemId;
    [SerializeField] private string displayLabel = "Memory";

    [Header("Highlight Colors")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private float wrongFlashDuration = 1.5f;

    private OutlineHighlight outline;
    private Coroutine flashRoutine;

    private void Awake()
    {
        outline = GetComponent<OutlineHighlight>();
    }

    public void Interact()
    {
        PuzzleManager.Instance.SubmitItem(this, itemId, displayLabel);
    }

    public string GetInteractPrompt()
    {
        return $"Press to select: {displayLabel}";
    }

    public void MarkCorrect()
    {
        outline?.Show(correctColor);
    }

    public void FlashWrong()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashWrongRoutine());
    }

    private IEnumerator FlashWrongRoutine()
    {
        outline?.Show(wrongColor);
        yield return new WaitForSeconds(wrongFlashDuration);
        ResetHighlight();
    }

    public void ResetHighlight()
    {
        outline?.Hide();
    }
}