using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    [Tooltip("If true, the player can only pass through once ObjectiveManager reports all steps are done.")]
    [SerializeField] private bool requireObjectivesComplete = true;

    [Tooltip("Exact name of the scene to load, e.g. 'Part2_TestScene'. Must be added to Build Settings.")]
    [SerializeField] private string nextSceneName;

    [Tooltip("Time to wait in seconds before loading the next scene.")]
    [SerializeField] private float transitionDelay = 2.0f;

    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        if (requireObjectivesComplete && ObjectiveManager.Instance != null && !ObjectiveManager.Instance.AllComplete)        
        {
        Debug.Log("Objectives arn't finished yet.");
        return;
        }

        hasTriggered = true;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("Level End reached (no next scene yet).");
        }
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(nextSceneName);
    }
}
