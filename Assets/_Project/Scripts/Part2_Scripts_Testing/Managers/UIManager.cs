using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject inventoryPanel;

    [Header("Inventory Display")]
    public TMP_Text inventoryText; // Assign a UI Text element to lisy items

    void Update()
    {
        // Toggle Pause Menu with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                // Back from settings to pause panel
                CloseSettings(); 
            }
            else
            {
                TogglePauseMenu();
            }
            
        }

        // Toggle Inventory UI with 'I'
        if (Input.GetKeyDown(KeyCode.I) && !GameManager.Instance.isPaused)
        {
            ToggleInventory();
        }
        
    }

    public void TogglePauseMenu()
    {
        GameManager.Instance.TogglePause();
        bool paused = GameManager.Instance.isPaused;

        pausePanel.SetActive(paused);
        settingsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(true);
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI()
    {
        if (InventoryManager.Instance.items.Count == 0)
        {
            inventoryText.text = "Inventory is empty.";
            return;
        }

        inventoryText.text = "Items:\n";
        foreach (string item in InventoryManager.Instance.items)
        {
            inventoryText.text += "- " + item + "\n";
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
