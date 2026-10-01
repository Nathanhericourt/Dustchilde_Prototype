using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneAdvance : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 3f;
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        Invoke(nameof(LoadNext), delaySeconds);
    }

    private void LoadNext()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
