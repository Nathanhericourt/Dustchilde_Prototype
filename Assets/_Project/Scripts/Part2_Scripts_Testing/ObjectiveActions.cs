using UnityEngine;

public class ObjectiveActions : MonoBehaviour
{
    public void CompleteCurrentObjective()
    {
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogWarning("ObjectiveManager is missing.");
            return;
        }

        ObjectiveManager.Instance.CompleteCurrentStep();
    }

    public void AddObjectiveProgress()
    {
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogWarning("ObjectiveManager is missing.");
            return;
        }

        ObjectiveManager.Instance.AddProgress();
    }
}
