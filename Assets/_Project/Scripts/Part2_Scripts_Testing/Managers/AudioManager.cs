using System;
using System.Threading;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Background Msuic")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Volume (0 to 1)")]
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.5f;

    private void Start()
    {
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }    
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ApplyVolumes();
    }

    // Calls this for once-off sounds(pickups, doors etx)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Calls this to start/chnage background/ambient music
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if  (clip == null || musicSource == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public float GetSFXVolume() => sfxVolume;
    public float GetMusicVolume() => musicVolume;

    private void ApplyVolumes()
    {
        if (musicSource != null) musicSource.volume = musicVolume;
        // sfxSource volume is applied per-clip via PlayOneShot(clip, volume) above
    }
}
