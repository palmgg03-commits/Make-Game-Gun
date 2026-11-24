using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Default SFX")]
    [SerializeField] public Slider volumeSlider;

    private void Start()
    {
        ApplySavedVolume();
    }
    private void OnEnable()
    {
       
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (volumeSlider == null)
        {
            Slider foundSlider = FindAnyObjectByType<Slider>();
            if (foundSlider != null)
            {
                volumeSlider = foundSlider;
            }
        }

        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            volumeSlider.value = savedVolume;

            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }
    }

    private void ApplySavedVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
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
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);

        AudioListener.volume = clampedVolume;

        PlayerPrefs.SetFloat("MasterVolume", clampedVolume);
        PlayerPrefs.Save();
    }

}
