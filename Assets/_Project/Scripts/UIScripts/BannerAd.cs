using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    string myGameIdAndroid = "4048568";
    string iosGameID = "4048569";
    string placementID = "BannerAd";
    bool testMode = true;

    static BannerAd instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Initialize()
    {
#if UNITY_IOS
        Advertisement.Initialize(iosGameID, testMode);
#else
        Advertisement.Initialize(myGameIdAndroid, testMode);
#endif
        StartCoroutine(WaitThenShowBannerAd());
    }

    void Start()
    {
        Initialize();
    }

    IEnumerator WaitThenShowBannerAd()
    {
        while (!Advertisement.IsReady(placementID))
        {
            yield return null;
        }
        DisplayBannerAd();
    }

    public void DisplayBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementID);
    }
}
