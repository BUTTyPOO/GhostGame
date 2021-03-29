using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMan : MonoBehaviour
{
    [SerializeField] AudioClip[] soundClips;
    
    AudioSource audSrc;
    public static SoundMan instance;
    [SerializeField] SettingsVars settingsVars;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        audSrc = GetComponent<AudioSource>();
        UpdateSoundSetting();
    }

    public void PlaySound(int soundID, float vol = 1.0f)
    {
        audSrc.pitch = Random.Range(0.80f, 1.30f);
        audSrc.PlayOneShot(soundClips[soundID], vol); 
    }

    public void UpdateSoundSetting()
    {
        if (settingsVars.soundEnabled)
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
