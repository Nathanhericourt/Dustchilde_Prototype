using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private float minSensitivity = 0.02f;
    [SerializeField] private float maxSensitivity = 0.5f;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSensitivity;
            sensitivitySlider.maxValue = maxSensitivity;

            if (playerMovement != null)
                sensitivitySlider.value = playerMovement.MouseSensitivity;

            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged); 
        }

    }

    private void OnSensitivityChanged(float value)
    {
        if (playerMovement != null)
            playerMovement.MouseSensitivity = value;
    }
}
