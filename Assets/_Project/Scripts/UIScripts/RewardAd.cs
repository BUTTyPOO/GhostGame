using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardAd : MonoBehaviour, IUnityAdsListener
{
    public string myGameIDAndroid = "4048568";
    public string myGameIDIOS = "4048569";
    public string myVideoPlacement = "rewardedVideo";
    public string myAdStatus = "";

    public bool adStarted;

    bool testMode = true;

    public delegate void DoneDelegate();
    public DoneDelegate functionToCallWhenDone; //set by whoever instantiates
    [SerializeField] GameObject popUpDialog;

    public void OnUnityAdsDidError(string message)
    {
        myAdStatus = message;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Failed)
        {
            GameObject a = GameObject.Find("popUpParent");
            a.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            functionToCallWhenDone();
        }
        Advertisement.RemoveListener(this);
        Destroy(gameObject);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        adStarted = true;
    }

    public void OnUnityAdsReady(string placementId)
    {
        Advertisement.Show(myVideoPlacement);
    }

    void OnEnable()
    {
        Advertisement.AddListener(this);
#if UNITY_IOS
        Advertisement.Initialize(myGameIDIOS, testMode);
#else
        Advertisement.Initialize(myGameIDAndroid, testMode);
#endif
        Advertisement.Show(myVideoPlacement);
    }

    void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
