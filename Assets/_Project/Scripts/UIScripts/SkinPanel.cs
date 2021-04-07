using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanel : MonoBehaviour
{
    public Texture2D mysteryThumb;
    public Texture2D[] thumbnails;
    public SkinIcon[] skinIcons;
    public IntRef currentSkin;
    public AdsWatchedData adsData;

    [SerializeField] GameObject popUpDialog;


    void Start()
    {
#if UNITY_EDITOR
        Saver.SaveAdsWatchedData(adsData.array);
#else
        adsData.array = Saver.LoadAdsWatchedData();
#endif

        skinIcons = new SkinIcon[transform.childCount];
        currentSkin.value = Saver.LoadSkin();
        LoadDataInChildren();
    }

    void LoadDataInChildren()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            SkinIcon skinIcon = child.GetComponent<SkinIcon>();
            skinIcons[i] = skinIcon;
            if (adsData.array[i] <= 0)
                skinIcon.unlocked = true;
            else
                skinIcon.unlocked = false;
            ++i;
        }
    }

    public void DisableAllGreenChecks()
    {
        foreach(SkinIcon icon in skinIcons)
        {
            icon.RemoveCheck();
        }
    }
}
