using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip gameMusic;
    AudioClip curMusicClip;
    AudioSource audSrc;

    [SerializeField] SettingsVars settingsVars;

    const float pitchSpeed = 5.0f;

    static public MusicMan instance;

    void Awake()
    {
        audSrc = GetComponent<AudioSource>();
        
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        OnGameStateChange(Enforcer.Instance.gameState);
        UpdateMusicSetting();
    }

    void OnEnable()
    {
        Enforcer.Instance.GameStateChanged += OnGameStateChange;
    }

    void OnDisable()
    {
        Enforcer.Instance.GameStateChanged -= OnGameStateChange;
    }

    void OnGameStateChange(IGameState newState)
    {
        if (newState == Enforcer.playingState)
        {
            PlayMusic(gameMusic);
        }

        if (newState == Enforcer.mainMenuState)
        {
            PlayMusic(mainMenuMusic);
        }

        if (newState == Enforcer.gameOverState)
        {
            audSrc.Stop();
        }
    }

    void PlayGameMusicThatWillSpeedUp()
    {
        PlayMusic(gameMusic);
        StartCoroutine(IncreaseAudSrcPitchOverTime());
    }

    void PlayMusic(AudioClip clip)
    {
        LoadClipInAudSrc(clip);
        audSrc.Play();
    }

    void LoadClipInAudSrc(AudioClip clip)
    {
        if (curMusicClip == clip) return;
        else curMusicClip = clip;
        audSrc.clip = clip;
        audSrc.pitch = 1.0f;
        audSrc.loop = true;
    }

    IEnumerator IncreaseAudSrcPitchOverTime()
    {
        while (Enforcer.Instance.gameState == Enforcer.playingState)
        {
            yield return new WaitForSeconds(0.5f);
            audSrc.pitch += 1.5f * Time.deltaTime;
        }
    }

    public void UpdateMusicSetting()
    {
        if (settingsVars.musicEnabled)
            EnableAudSrc();
        else
            DisableAudSrc();
        Saver.SaveSettings(settingsVars);
    }

    public void DisableAudSrc() //called by enforcer
    {
        audSrc.enabled = false;
    }

    public void EnableAudSrc()  //called by enforcer
    {
        audSrc.enabled = true;
    }
}
