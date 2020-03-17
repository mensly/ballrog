using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

public class EnableAds : MonoBehaviour
{
#if UNITY_ANDROID
    string bannerAdId = "ca-app-pub-5907807220437805/1752462307";
    string interstitialAdId = "ca-app-pub-5907807220437805/4776358693";
#elif UNITY_IPHONE
    string bannerAdId = "ca-app-pub-5907807220437805/7958778001";
    string interstitialAdId = "ca-app-pub-5907807220437805/9837113681";
#else
    string bannerAdId = null;
    string interstitialAdId = null;
#endif
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private bool nextLevel;

    // Start is called before the first frame update
    void Awake()
    {
        if (bannerAdId == null) { return; }
        MobileAds.Initialize(initStatus => {
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView(bannerAdId, AdSize.Banner, AdPosition.TopLeft);
            RefreshBanner();
            RefreshInterstitial();
        });
    }

    private void OnDestroy()
    {
        bannerView.Destroy();
        interstitial.Destroy();
    }

    public void RefreshBanner()
    {
        if (bannerAdId == null) { return; }
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice("C71C2A5400AF8DC9B7CCA3B3A2FC84C8")
            .Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    void RefreshInterstitial()
    {
        if (interstitialAdId == null) { return; }
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
        interstitial = new InterstitialAd(interstitialAdId);
        interstitial.OnAdFailedToLoad += Interstitial_OnAdFailedToLoad;
        interstitial.OnAdClosed += Interstitial_OnAdClosed;
        interstitial.OnAdLeavingApplication += Interstitial_OnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice("C71C2A5400AF8DC9B7CCA3B3A2FC84C8")
            .Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void ShowInterstitial(bool nextLevel)
    {
        this.nextLevel = nextLevel;
        if (interstitialAdId == null)
        {
            StartCoroutine(OnAdFinished());
            return;
        }
        if (!interstitial.IsLoaded())
        {
            StartCoroutine(OnAdFinished());
            return;
        }
        interstitial.Show();
    }


    public void ShowAdAndNextLevel()
    {
        ShowInterstitial(true);
    }

    public void ShowAdAndRetryLevel()
    {
        ShowInterstitial(false);
    }

    private IEnumerator OnAdFinished()
    {
        yield return new WaitForFixedUpdate();
        if (nextLevel)
        {
            GetComponent<GameController>().NextLevel();
        }
        else
        {
            GetComponent<GameController>().RetryLevel();
        }
        RefreshInterstitial();
    }

    private void Interstitial_OnAdClosed(object sender, EventArgs e)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(OnAdFinished());
    }

    private void Interstitial_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(OnAdFinished());
    }

    private void Interstitial_OnAdLeavingApplication(object sender, EventArgs e)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(OnAdFinished());
    }
}
