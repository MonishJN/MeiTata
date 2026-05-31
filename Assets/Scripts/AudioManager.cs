using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // The static instance that other scripts will call
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
        // Simple Singleton pattern logic
        if (Instance == null)
        {
            Instance = this;
            // Keeps the audio playing smoothly between level scenes
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start playing the background music automatically when the game starts
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            //musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void PlayLevelComplete()
    {
        sfxSource.PlayOneShot(levelCompleteSound);
    }
    // Call this from anywhere using: AudioManager.Instance.PlayPortalEnter();
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
        // sfxSource.PlayOneShot(forceExecutionSound);
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
