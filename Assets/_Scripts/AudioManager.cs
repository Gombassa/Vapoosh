// In AudioManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public AudioClip[] menuMusicLoops;
    public AudioClip[] gameMusicLoops;

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private Coroutine musicCoroutine;
    private enum MusicState { Menu, Game, None }
    private MusicState currentMusicState = MusicState.None;

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
            musicSource.loop = false;
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
        if (mode == LoadSceneMode.Additive)
        {
            return;
        }

        MusicState newMusicState = (scene.name == "GameScene") ? MusicState.Game : MusicState.Menu;
        
        if (newMusicState != currentMusicState)
        {
            currentMusicState = newMusicState;
            
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
            }

            if (currentMusicState == MusicState.Game)
            {
                musicCoroutine = StartCoroutine(PlayMusicLoop(gameMusicLoops));
            }
            else
            {
                musicCoroutine = StartCoroutine(PlayMusicLoop(menuMusicLoops));
            }
        }
    }

    private IEnumerator PlayMusicLoop(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            yield break;
        }

        int lastClipIndex = -1;
        while (true)
        {
            int clipIndex;
            do
            {
                clipIndex = Random.Range(0, clips.Length);
            } while (clips.Length > 1 && clipIndex == lastClipIndex);

            lastClipIndex = clipIndex;
            AudioClip clipToPlay = clips[clipIndex];
            
            musicSource.clip = clipToPlay;
            musicSource.Play();

            yield return new WaitForSeconds(clipToPlay.length);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null) { sfxSource.PlayOneShot(clip); }
    }
    
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public bool IsMusicMuted()
    {
        return musicSource.mute;
    }
}