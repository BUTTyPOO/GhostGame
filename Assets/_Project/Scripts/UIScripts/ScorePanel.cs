using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScorePanel : MonoBehaviour
{
    public IntRef score;
    public IntRef highScore;

    [SerializeField] Texture2D newScorePanel;
    [SerializeField] Texture2D normalPanel;

    [SerializeField] GameObject scorePan;
    [SerializeField] RawImage scorePanImg;

    [SerializeField] GameObject newLabel;

    [SerializeField] Text scoreTxt;
    [SerializeField] Text highScoreTxt;

    
    void OnEnable()
    {
        SetCorrectPanel();
        scoreTxt.text = score.value.ToString();
        highScoreTxt.text = highScore.value.ToString();
    }

    void SetCorrectPanel()
    {
        if (score.value == 0)   //If didn't kill, don't display the UI score Panel at all!
            scorePan.SetActive(false);
        if (Enforcer.Instance.beatHighScore)
        {
            EnableNewScorePanel();
            //SoundMan.instance.PlaySound();    // Play uplifing sound WE BEAT HIGH SCORE
        }
        else
        {
            EnableScorePanel();
            //SoundMan.instance.PlaySound();    // Play failure sound cuz we failed
        }
    }

    void EnableScorePanel()
    {
        scorePanImg.texture = normalPanel;
    }

    void EnableNewScorePanel()
    {
        scorePanImg.texture = newScorePanel;
        scorePan.transform.GetChild(1).gameObject.SetActive(false); // disable score text
        newLabel.SetActive(true);
    }
}
