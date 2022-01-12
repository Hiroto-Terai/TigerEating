﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GoogleBannerAdsTop : MonoBehaviour
{
  private BannerView bannerView;

  // Start is called before the first frame update
  void Start()
  {
    // Google Mobile Ads SDKの初期化
    MobileAds.Initialize(initStatus => { });

    this.RequestBanner();
    SceneManager.activeSceneChanged += OnActiveSceneChanged;
  }

  void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
  {
    this.bannerView.Destroy();
  }
  private void RequestBanner()
  {
#if UNITY_IPHONE
    string adUnitId = "ca-app-pub-1161341815062676/1471699507";

#else
    string adUnitId = "unexpected_platform";
#endif

    // Create a 320x50 banner at the top of the screen.
    this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
    this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();

    // Load the banner with the request.
    this.bannerView.LoadAd(request);
  }
  public void HandleOnAdLoaded(object sender, EventArgs args)
  {
    MonoBehaviour.print("HandleAdLoaded event received");
  }
}
