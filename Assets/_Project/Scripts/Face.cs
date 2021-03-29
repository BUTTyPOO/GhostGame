using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Face : MonoBehaviour
{
    public IntRef score;
    public IntRef highScore;

    [SerializeField] Texture2D[] faces;
    RawImage faceImage;

    float spinTime = 2.5f;

    void OnEnable()
    {
        faceImage = GetComponent<RawImage>();
        StartCoroutine(SpinToCorrectFace());
    }

    IEnumerator SpinToCorrectFace()
    {
        int faceLength = faces.Length;
        float elapsedTime = 0.0f;
        int i = 0;
        while (elapsedTime < spinTime)
        {
            if (i >= faceLength) i = 0;
            faceImage.texture = faces[i];
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
            ++i;
        }
        faceImage.texture = GetProperFaceTexture();
        // bounce face
    }

    Texture2D GetProperFaceTexture()
    {
        if (Enforcer.Instance.beatHighScore)    // Always be happy if plr beats high score
        {
            SoundMan.instance.PlaySound(3);
            return faces[2];
        }

        int points = score.value;
        if (points < 7)
        {
            SoundMan.instance.PlaySound(5);
            return faces[0];    // Sad face
        }
        else if (points > 20)
        {
            SoundMan.instance.PlaySound(3);
            return faces[2];    // happy face
        }
        else
        {
            SoundMan.instance.PlaySound(4);
            return faces[1];    // netrual face
        }
    }
}
