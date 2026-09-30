using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Pauses/UnPaused the game
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Freeze time
            Cursor.lockState =CursorLockMode.None;
            Cursor.visible = true; 
        }
        else
        {
            Time.timeScale = 1f; // Resume normal time
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
