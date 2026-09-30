using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private float minSensitivity = 0.02f;
    [SerializeField] private float maxSensitivity = 0.5f;

    [Header("Volume")]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
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

        if (sfxVolumeSlider != null && AudioManager.Instance != null)
        {
            sfxVolumeSlider.minValue = 0f;
            sfxVolumeSlider.maxValue = 1f;
            sfxVolumeSlider.value = AudioManager.Instance.GetSFXVolume();
            sfxVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

    }

    private void OnSensitivityChanged(float value)
    {
        if (playerMovement != null)
            playerMovement.MouseSensitivity = value;
    }

    private void OnSFXVolumeChnaged(float value)
    {
        AudioManager.Instance?.SetSFXVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
    }
}
