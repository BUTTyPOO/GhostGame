using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] SettingsVars settingsVars;
    [SerializeField] Texture2D soundOn;
    [SerializeField] Texture2D soundOff;
    RawImage img;

    void Start()
    {
        img = GetComponent<RawImage>();
        if (settingsVars.soundEnabled)
            img.texture = soundOn;
        else
            img.texture = soundOff;
        Saver.SaveSettings(settingsVars);
    }

    public void ButtonPressed()
    {
        if (settingsVars.soundEnabled)
        {
            img.texture = soundOff;
            settingsVars.soundEnabled = false;
        }
        else
        {
            img.texture = soundOn;
            settingsVars.soundEnabled = true;
        }

        SoundMan.instance.UpdateSoundSetting();
    }
}
