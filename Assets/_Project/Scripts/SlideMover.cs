using UnityEngine;
using System.Collections;

public class SlideMover : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1.5f;

    private Coroutine moveRoutine;

    public void MoveTo(Vector3 targetPosition)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(SlideToPosition(targetPosition));
    }

    private IEnumerator SlideToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
    }
}