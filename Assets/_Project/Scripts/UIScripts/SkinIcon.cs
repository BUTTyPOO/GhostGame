using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.
using UnityEngine.UI;

public class SkinIcon : MonoBehaviour
{
    RawImage thumbnail;
    SkinPanel skinPan;
    UIManager UIman;
    RawImage check;

    [SerializeField] AdsLeftText adsLeftTxt;
    [SerializeField] GameObject rewardAdObj;

    public bool unlocked;
    public int skinIndex;   // Vars get loaded by SkinPanel

    void Start()
    {
        skinPan = GetComponentInParent<SkinPanel>();
        UIman = GameObject.Find("UI").GetComponent<UIManager>();
        thumbnail = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        check = transform.GetChild(0).GetChild(1).GetComponent<RawImage>();

        SetThumbNail();
        SetCheck();
    }

    void SetThumbNail()
    {
        if (unlocked)
            thumbnail.texture = skinPan.thumbnails[skinIndex];
        else
            thumbnail.texture = skinPan.mysteryThumb;
    }

    void SetCheck()
    {
        if (skinIndex == skinPan.currentSkin.value) // If skin equiped is this skin
            AddCheck();
        else
            RemoveCheck();
    }

    public void PtrDown()
    {
        UIman.PlayButtonDownSound();
        UIman.Jiggle(transform.GetChild(0));
    }

    public void OnClick()
    {
        if (unlocked)
        {
            SelectSkin();
        }
        else
            WatchAdForSkin();
    }

    void SelectSkin()
    {
        skinPan.DisableAllGreenChecks();
        SetEquipedSkin();
        AddCheck();
    }

    void SetEquipedSkin()
    {
        skinPan.currentSkin.value = skinIndex;
        Saver.SaveSkin(skinPan.currentSkin.value);
    }

    void WatchAdForSkin()
    {
        //TODO: add reward add code
        Instantiate(rewardAdObj).GetComponent<RewardAd>().functionToCallWhenDone = DecrementAdsLeft;
    }

    void DecrementAdsLeft()
    {
        --skinPan.adsData.array[skinIndex];
        adsLeftTxt.UpdateAdsLeftTxt();
        if (skinPan.adsData.array[skinIndex] <= 0)
            Unlock();
        Saver.SaveAdsWatchedData(skinPan.adsData.array);
    }

    void Unlock()
    {
        unlocked = true;
        thumbnail.texture = skinPan.thumbnails[skinIndex];
    }

    public void RemoveCheck()
    {
        check.enabled = false;
    }

    void AddCheck()
    {
        check.enabled = true;
    }

}