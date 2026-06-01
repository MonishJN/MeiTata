using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

     [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip levelCompleteSound;
    [SerializeField] private AudioClip portalEnterSound;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip forceExecutionSound;
    [SerializeField] private AudioClip shootSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }
    public void BackgroundMusic(bool isEnabled) {
        if (backgroundMusic != null)
        {
          musicSource.volume = isEnabled ? 1f : 0f;
        }
    }
    public void SetBGVolume(float volume) {
        musicSource.volume = volume;
    }
    public void SFX(bool isEnabled) {
        sfxSource.volume = isEnabled ? 1f : 0f;
    }
    public void SetSFXVolume(float volume) {
        sfxSource.volume = volume;
    }
    public void PlayLevelComplete()
    {
        sfxSource.PlayOneShot(levelCompleteSound);
    }
    public void PlayPortalEnter()
    {
        sfxSource.PlayOneShot(portalEnterSound);
    }
    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickSound);
    }

    public void PlayPickup()
    {
        sfxSource.PlayOneShot(pickupSound);
    }

    public void PlayForceExecution()
    {
        sfxSource.clip = forceExecutionSound;
        sfxSource.Play();
    }
    public void PlayShoot()
    {
        sfxSource.PlayOneShot(shootSound);
    }
    public void PlayKey()
    {
        sfxSource.PlayOneShot(keySound);
    }
}
