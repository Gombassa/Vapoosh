// In AudioManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    public AudioClip diceRollSound;
    public AudioClip placeCounterSound;
    public AudioClip captureSound;
    public AudioClip winnerSound;
    public AudioClip vapooshSound;
    public AudioClip wrongTileSound;
    public AudioClip buttonClickSound;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private string currentScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if(gameObject.GetComponent<AudioListener>() == null)
            {
                gameObject.AddComponent<AudioListener>();
            }

            AudioSource[] sources = GetComponents<AudioSource>();
            sfxSource = sources[0];
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentScene)
        {
            if (scene.name == "GameScene") { PlayMusic(gameMusic); }
            else { PlayMusic(menuMusic); }
            currentScene = scene.name;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null) { sfxSource.PlayOneShot(clip); }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}