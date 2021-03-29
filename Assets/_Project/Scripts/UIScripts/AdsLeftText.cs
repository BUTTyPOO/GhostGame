using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsLeftText : MonoBehaviour
{
    [SerializeField] AdsWatchedData skinData;
    Text txt;
    int skinIndex;

    void Start()
    {
        txt = GetComponent<Text>();
        skinIndex = transform.parent.parent.GetComponent<SkinIcon>().skinIndex;
        UpdateAdsLeftTxt();
    }

    public void UpdateAdsLeftTxt()
    {
        if (skinData.array[skinIndex] <= 0)
            txt.text = "";
        else    
            txt.text = skinData.array[skinIndex].ToString();
    }
}
