using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(AnimateGameOverUI());
    }

    IEnumerator AnimateGameOverUI()
    {
        Image gameOverImage = GetComponent<Image>();
        Color bgColor = gameOverImage.color;
        bgColor.a = 0;
        gameOverImage.color = bgColor;
        ZeroAllChildsScale(transform);
        yield return new WaitForSeconds(1.3f);  //delay
        PlayProperSoundEffect();
        bgColor.a = 0.7f;
        StartCoroutine(ColorLerp(gameOverImage, bgColor, 0.4f));

        foreach (Transform element in transform)
        {
            element.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutElastic);
        }
    }

    void PlayProperSoundEffect()    // This shouldn't be here this is horrible practice I'm stupid as hell
    {
        if (Enforcer.Instance.beatHighScore)
        {
            //SoundMan.instance.PlaySound(11);    // TODO: Add uplifing sound
        }
        else
            SoundMan.instance.PlaySound(10, 2);
    }

    void ZeroAllChildsScale(Transform parent)
    {
        foreach (Transform element in parent)
            element.localScale = Vector3.zero;
    }

    IEnumerator ColorLerp(Image img, Color targetColor, float duration)
    {
        Color startColor = img.color;
        float elapsedTime = 0;
        while (elapsedTime <= duration)
        {
            img.color = Color.Lerp(startColor, targetColor, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
