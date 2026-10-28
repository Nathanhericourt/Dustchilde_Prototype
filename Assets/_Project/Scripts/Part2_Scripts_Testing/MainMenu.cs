using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The exact name of the first level scene to load.")]
    [SerializeField] private string firstLevelSceneName;

    private void Start()
    {
        // Unlocks the cursor and makes it visible when returning to the Main Menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Resets time back to normal speed in case the gameplay was paused
        Time.timeScale = 1f;
    }

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