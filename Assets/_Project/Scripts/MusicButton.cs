using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [SerializeField] SettingsVars settingsVars;
    [SerializeField] Texture2D musicOn;
    [SerializeField] Texture2D musicOff;
    RawImage img;

    void Start()
    {
        img = GetComponent<RawImage>();
        if (settingsVars.musicEnabled)
            img.texture = musicOn;
        else
            img.texture = musicOff;
    }

    public void ButtonPressed()
    {
        if (settingsVars.musicEnabled)
        {
            img.texture = musicOff;
            settingsVars.musicEnabled = false;
        }
        else
        {
            img.texture = musicOn;
            settingsVars.musicEnabled = true;
        }
        MusicMan.instance.UpdateMusicSetting();
    }
}
