using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The exact name of the first level scene to load.")]
    [SerializeField] private string firstLevelSceneName;

    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(firstLevelSceneName))
        {
            SceneManager.LoadScene(firstLevelSceneName);
        }
        else
        {
            Debug.LogError("MainMenu: First level scene name is empty! Please assign it in the Inspector.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game Exiting...");
        Application.Quit();
    }
}