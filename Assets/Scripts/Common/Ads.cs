using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour, IUnityAdsListener
{
    public static Ads Instance;
    //public string gameId = "3255661";
    //public string pId = "video";

    public string gameId = "3288674";
    public string pId = "Ski";
    public bool testMode = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);//销毁新创建的go而保留第一个go
            return;
        }


        Instance = this;
        DontDestroyOnLoad(this);//防止重新加载场景时销毁

        EventCenter.AddListener(EventType.ShowAds, Show);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowAds, Show);
    }

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
        //StartCoroutine(ShowBannerWhenReady());
    }

    public bool GetIsAdvertisementReady()
    {
        if (GameManager.GetInstance().GetIsAds())
            return Advertisement.IsReady(pId);
        else
            return false;
    }
    
    private void Show()
    {
        Advertisement.Show(pId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            EventCenter.Broadcast(EventType.DoAfterAds);
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.Log("The ad did not finish due to an error");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == pId)
        {
            //GameManager.GetInstance().SetIsAdsReady(true);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}