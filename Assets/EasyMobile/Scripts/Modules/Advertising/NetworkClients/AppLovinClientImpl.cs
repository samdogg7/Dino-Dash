using UnityEngine;
using System;
using System.Collections.Generic;

namespace EasyMobile
{
#if EM_APPLOVIN
    using EasyMobile.Internal;
#endif

    public class AppLovinClientImpl : AdClientImpl
    {
        private const string NO_SDK_MESSAGE = "SDK missing. Please import the AppLovin plugin.";
        private const string DATA_PRIVACY_CONSENT_KEY = "EM_Ads_AppLovin_DataPrivacyConsent";

#if EM_APPLOVIN       

        private AppLovinSettings mAdSettings = null;
        private bool shouldReceiveRewarded = false;
        private BannerAdPosition mDefaultBannerPosition = BannerAdPosition.Bottom;
        private AdPlacement currentShowingInterstitial = null;
        private AdPlacement currentShowingRewarded = null;

        /// <summary>
        /// We're gonna save all the loaded custom banners here.
        /// </summary>
        /// Key: The AdPlacement used to load the banner.
        /// Value: Loaded banner's position & size.
        private Dictionary<AdPlacement, KeyValuePair<BannerAdPosition, BannerAdSize>> mCustomBanners;

        /// <summary>
        /// We're gonna save all the loaded custom interstitial ads here.
        /// </summary>
        private List<AdPlacement> mCustomInterstitialAds;

        /// <summary>
        /// We're gonna save all the loaded custom rewarded video ads here.
        /// </summary>
        private List<AdPlacement> mCustomRewardedVideoAds;


#endif
        #region Singleton

        private static AppLovinClientImpl sInstance;

        private AppLovinClientImpl()
        {
        }

        /// <summary>
        /// Returns the singleton client.
        /// </summary>
        /// <returns>The client.</returns>
        public static AppLovinClientImpl CreateClient()
        {
            if (sInstance == null)
            {
                sInstance = new AppLovinClientImpl();
            }
            return sInstance;
        }

        #endregion  // Object Creators

        #region AppLovin Events

#if EM_APPLOVIN

        /// <summary>
        /// Occurs when a banner ad is loaded or failed to be loaded.
        /// </summary>
        public event Action<bool> BannerLoadedEvent;

        /// <summary>
        /// Occurs when a banner ad is displayed.
        /// </summary>
        public event Action BannerDisplayedEvent;

        /// <summary>
        /// Occurs when an interstitial ad is loaded or failed to be loaded.
        /// </summary>
        public event Action<bool> InterstitialLoadedEvent;

        /// <summary>
        /// Occurs when an interstitial ad is displayed.
        /// </summary>
        public event Action InterstitialDisplayedEvent;

        /// <summary>
        /// Occurs when an interstitial ad is closed.
        /// </summary>
        public event Action InterstitialHiddenEvent;

        /// <summary>
        /// Occurs when a rewarded ad is loaded or failed to be loaded.
        /// </summary>
        public event Action<bool> RewardedLoadedEvent;

        /// <summary>
        /// Occurs when a rewarded ad is displayed.
        /// </summary>
        public event Action RewardedDisplayedEvent;

        /// <summary>
        /// Occurs when a rewarded a is closed.
        /// </summary>
        public event Action RewardedHiddenEvent;

        /// <summary>
        /// Occurs when a rewarded ad is completed.
        /// </summary>
        public event Action<double, string> RewardedCompletedEvent;

        /// <summary>
        /// Occurs when a rewarded ad is canceled/skipped.
        /// </summary>
        public event Action RewardedCanceledEvent;

        /// <summary>
        /// Occurs when an ad is clicked. This means the user will be leaving your app soon.
        /// </summary>
        public event Action AdClickedEvent;

#endif

        #endregion

        #region IAdClient Overrides

        public override AdNetwork Network { get { return AdNetwork.AppLovin; } }

        public override bool IsBannerAdSupported { get { return true; } }

        public override bool IsInterstitialAdSupported { get { return true; } }

        public override bool IsRewardedAdSupported { get { return true; } }


        public override bool IsSdkAvail
        {
            get
            {
#if EM_APPLOVIN
                return true;
#else
                return false;
#endif
            }
        }

        public override bool IsValidPlacement(AdPlacement placement, AdType type)
        {
#if EM_APPLOVIN
            string id;
            if (placement == AdPlacement.Default)
            {
                switch (type)
                {
                    case AdType.Rewarded:
                        id = mAdSettings.DefaultRewardedAdId.Id;
                        break;
                    case AdType.Interstitial:
                        id = mAdSettings.DefaultInterstitialAdId.Id;
                        break;
                    default:
                        id = mAdSettings.DefaultBannerAdId.Id;
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case AdType.Rewarded:
                        id = FindIdForPlacement(mAdSettings.CustomRewardedAdIds, placement);
                        break;
                    case AdType.Interstitial:
                        id = FindIdForPlacement(mAdSettings.CustomInterstitialAdIds, placement);
                        break;
                    default:
                        id = FindIdForPlacement(mAdSettings.CustomBannerAdIds, placement);
                        break;
                }
            }


            return true;
#else
            return false;
#endif
        }


        protected override string NoSdkMessage
        {
            get { return NO_SDK_MESSAGE; }
        }

        protected override Dictionary<AdPlacement, AdId> CustomInterstitialAdsDict
        {
            get
            {
#if EM_APPLOVIN
                return mAdSettings == null ? null : mAdSettings.CustomInterstitialAdIds;
#else
                return null;
#endif
            }
        }

        protected override Dictionary<AdPlacement, AdId> CustomRewardedAdsDict
        {
            get
            {
#if EM_APPLOVIN
                return mAdSettings == null ? null : mAdSettings.CustomRewardedAdIds;
#else
                return null;
#endif
            }
        }

        protected override void InternalInit()
        {
#if EM_APPLOVIN
            mAdSettings = EM_Settings.Advertising.AppLovin;
            mCustomRewardedVideoAds = new List<AdPlacement>();
            mCustomInterstitialAds = new List<AdPlacement>();
            mCustomBanners = new Dictionary<AdPlacement, KeyValuePair<BannerAdPosition, BannerAdSize>>();

            // Set GDPR consent if any.
            var consent = GetApplicableDataPrivacyConsent();
            ApplyDataPrivacyConsent(consent);

            AppLovin.SetSdkKey(mAdSettings.SDKKey);
            AppLovin.InitializeSdk();

            // Test Mode
            if (mAdSettings.EnableTestMode)
                AppLovin.SetTestAdsEnabled("true");
            else
                AppLovin.SetTestAdsEnabled("false");

            // Age-Restricted 
            if (mAdSettings.AgeRestrictMode)
                AppLovin.SetIsAgeRestrictedUser("true");
            else
                AppLovin.SetIsAgeRestrictedUser("false");

            AppLovin.LoadRewardedInterstitial();

            // Setup Events Listener
            GameObject listener = new GameObject();
            listener.name = "EM_AppLovinAdsListenerObject";
            UnityEngine.Object.Instantiate(listener, Vector3.zero, Quaternion.identity);
            listener.AddComponent<EM_AppLovinAdsListener>();
            listener.GetComponent<EM_AppLovinAdsListener>().setAdClient(this);
            AppLovin.SetUnityAdListener("EM_AppLovinAdsListenerObject");

            // Done Initialization.
            mIsInitialized = true;
            Debug.Log("AppLovin client has been initialized.");
#endif
        }

        protected override string DataPrivacyConsentSaveKey { get { return DATA_PRIVACY_CONSENT_KEY; } }

        protected override void ApplyDataPrivacyConsent(ConsentStatus consent)
        {
#if EM_APPLOVIN
            switch (consent)
            {
                case ConsentStatus.Granted:
                    AppLovin.SetHasUserConsent("true");
                    break;
                case ConsentStatus.Revoked:
                    AppLovin.SetHasUserConsent("false");
                    break;
                case ConsentStatus.Unknown:
                default:
                    break;
            }
#endif
        }

        //------------------------------------------------------------
        // Banner Ads.
        //------------------------------------------------------------

        protected override void InternalShowBannerAd(AdPlacement placement, BannerAdPosition position, BannerAdSize size)
        {
#if EM_APPLOVIN
            string id = placement == AdPlacement.Default ?
                mAdSettings.DefaultBannerAdId.Id :
                FindIdForPlacement(mAdSettings.CustomBannerAdIds, placement);

            if (placement.Equals(AdPlacement.Default)) // Default banner...
            {
                /// Create a new banner if user request a
                /// banner ad with different position or size.
                if (position != mDefaultBannerPosition)
                {
                    Debug.Log("Creating new default banner...");
                    mDefaultBannerPosition = position;
                }
                AppLovinAdPosition pos = ToAppLovinAdPosition(position);
                AppLovin.SetAdPosition(pos.x, pos.y);
                if (string.IsNullOrEmpty(id))
                    AppLovin.ShowAd();
                else
                    AppLovin.ShowAd(id);
            }
            else // Custom banner...
            {
                /// Create a new banner if the banner with this key hasn't been initialized or
                /// user request new banner with existed key but different position or size.
                bool shouldCreateFlag = !mCustomBanners.ContainsKey(placement) ||
                                        mCustomBanners[placement].Key != position ||
                                        mCustomBanners[placement].Value != size;

                if (shouldCreateFlag)
                {
                    Debug.Log("Creating new custom banner...");
                    mCustomBanners[placement] = new KeyValuePair<BannerAdPosition, BannerAdSize>(position, size);
                }

                AppLovinAdPosition pos = ToAppLovinAdPosition(position);
                AppLovin.SetAdPosition(pos.x, pos.y);
                if (string.IsNullOrEmpty(id))
                    AppLovin.ShowAd();
                else
                    AppLovin.ShowAd(id);
            }
#endif
        }

        protected override void InternalHideBannerAd(AdPlacement placement)
        {
#if EM_APPLOVIN
            AppLovin.HideAd();
#endif
        }

        protected override void InternalDestroyBannerAd(AdPlacement placement)
        {
#if EM_APPLOVIN
            AppLovin.HideAd();
#endif
        }

        //------------------------------------------------------------
        // Interstitial Ads.
        //------------------------------------------------------------

        protected override bool InternalIsInterstitialAdReady(AdPlacement placement)
        {
#if EM_APPLOVIN
            string id = placement == AdPlacement.Default ?
               mAdSettings.DefaultInterstitialAdId.Id :
               FindIdForPlacement(mAdSettings.CustomInterstitialAdIds, placement);

            if (placement == AdPlacement.Default) // Default interstitial ad...
            {
                if (string.IsNullOrEmpty(id))
                    return AppLovin.HasPreloadedInterstitial();
                else
                    return AppLovin.HasPreloadedInterstitial(mAdSettings.DefaultInterstitialAdId.Id);
            }
            else // Custom interstitial ad...
            {
                if (mCustomInterstitialAds != null &&
                mCustomInterstitialAds.Contains(placement))
                {
                    if (string.IsNullOrEmpty(id))
                        return AppLovin.HasPreloadedInterstitial();
                    else
                        return AppLovin.HasPreloadedInterstitial(id);
                }
                else
                {
                    return false;
                }
            }
#else
            return false;
#endif
        }

        protected override void InternalLoadInterstitialAd(AdPlacement placement)
        {
#if EM_APPLOVIN
            string id = placement == AdPlacement.Default ?
                mAdSettings.DefaultInterstitialAdId.Id :
                FindIdForPlacement(mAdSettings.CustomInterstitialAdIds, placement);

            if (placement.Equals(AdPlacement.Default)) // Default interstitial ad...
            {
                if (string.IsNullOrEmpty(id))
                    AppLovin.PreloadInterstitial();
                else
                    AppLovin.PreloadInterstitial(id);
            }
            else // Custom interstitial ad...
            {
                if (!mCustomInterstitialAds.Contains(placement))
                    mCustomInterstitialAds.Add(placement);

                if (string.IsNullOrEmpty(id))
                    AppLovin.PreloadInterstitial();
                else
                    AppLovin.PreloadInterstitial(id);

            }
#endif
        }

        protected override void InternalShowInterstitialAd(AdPlacement placement)
        {
#if EM_APPLOVIN
            string id = placement == AdPlacement.Default ?
                mAdSettings.DefaultInterstitialAdId.Id :
                FindIdForPlacement(mAdSettings.CustomInterstitialAdIds, placement);

            if (placement.Equals(AdPlacement.Default)) // Default interstitial ad...
            {
                if (string.IsNullOrEmpty(id))
                    AppLovin.ShowInterstitial();
                else
                    AppLovin.ShowInterstitialForZoneId(id);

                currentShowingInterstitial = placement;
            }
            else // Custom interstitial ad...
            {
                if (!mCustomInterstitialAds.Contains(placement))
                    return;
                if (string.IsNullOrEmpty(id))
                    AppLovin.ShowInterstitial();
                else
                    AppLovin.ShowInterstitialForZoneId(id);
                currentShowingInterstitial = placement;
            }
#endif
        }

        //------------------------------------------------------------
        // Rewarded Ads.
        //------------------------------------------------------------

        protected override bool InternalIsRewardedAdReady(AdPlacement placement)
        {
#if EM_APPLOVIN
            string id = placement == AdPlacement.Default ?
                mAdSettings.DefaultRewardedAdId.Id :
                FindIdForPlacement(mAdSettings.CustomRewardedAdIds, placement);

            if (placement.Equals(AdPlacement.Default)) // Default rewarded ad...
            {
                if (string.IsNullOrEmpty(id))
                    return AppLovin.IsIncentInterstitialReady();
                else
                    return AppLovin.IsIncentInterstitialReady(mAdSettings.DefaultRewardedAdId.Id);
            }
            else // Custom rewarded ad...
            {
                if (mAdSettings.CustomRewardedAdIds.ContainsKey(placement))
                {
                    if (string.IsNullOrEmpty(id))
                        return AppLovin.IsIncentInterstitialReady();
                    else
                        return AppLovin.IsIncentInterstitialReady(id);
                }
                else
                {
                    return false;
                }
            }
#else
            return false;
#endif
        }

        protected override void InternalLoadRewardedAd(AdPlacement placement)
        {
#if EM_APPLOVIN
            string id = placement == AdPlacement.Default ?
                mAdSettings.DefaultRewardedAdId.Id :
                FindIdForPlacement(mAdSettings.CustomRewardedAdIds, placement);

            if (placement.Equals(AdPlacement.Default)) // Default interstitial ad...
            {
                if (string.IsNullOrEmpty(id))
                    AppLovin.LoadRewardedInterstitial();
                else
                    AppLovin.LoadRewardedInterstitial(id);

            }
            else // Custom interstitial ad...
            {
                // Add a placement if needed
                if (!mCustomRewardedVideoAds.Contains(placement))
                    mCustomRewardedVideoAds.Add(placement);

                if (string.IsNullOrEmpty(id))
                    AppLovin.LoadRewardedInterstitial();
                else
                    AppLovin.LoadRewardedInterstitial(id);
            }
#endif
        }

        protected override void InternalShowRewardedAd(AdPlacement placement)
        {
#if EM_APPLOVIN
            if (placement.Equals(AdPlacement.Default))
            {
                if (string.IsNullOrEmpty(mAdSettings.DefaultRewardedAdId.Id))
                    AppLovin.ShowRewardedInterstitial();
                else
                    AppLovin.ShowRewardedInterstitialForZoneId(mAdSettings.DefaultRewardedAdId.Id);
                currentShowingRewarded = placement;
            }
            else
            {
                if (mAdSettings.CustomRewardedAdIds.ContainsKey(placement))
                {
                    if (string.IsNullOrEmpty(mAdSettings.CustomRewardedAdIds[placement].Id))
                        AppLovin.ShowRewardedInterstitial();
                    else
                        AppLovin.ShowRewardedInterstitialForZoneId(mAdSettings.CustomRewardedAdIds[placement].Id);
                    currentShowingRewarded = placement;
                }
            }
#endif
        }
        #endregion

        #region Other Methods

#if EM_APPLOVIN
        private AppLovinAdPosition ToAppLovinAdPosition(BannerAdPosition pos)
        {

            switch (pos)
            {
                case BannerAdPosition.Top:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_TOP);
                case BannerAdPosition.Bottom:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);
                case BannerAdPosition.TopLeft:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_LEFT, AppLovin.AD_POSITION_TOP);
                case BannerAdPosition.TopRight:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_RIGHT, AppLovin.AD_POSITION_TOP);
                case BannerAdPosition.BottomLeft:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_LEFT, AppLovin.AD_POSITION_BOTTOM);
                case BannerAdPosition.BottomRight:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_RIGHT, AppLovin.AD_POSITION_BOTTOM);
                default:
                    return new AppLovinAdPosition(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_TOP);
            }
        }


        public class AppLovinAdPosition
        {
            public float x;
            public float y;

            public AppLovinAdPosition()
            {
                this.x = AppLovin.AD_POSITION_CENTER;
                this.y = AppLovin.AD_POSITION_BOTTOM;
            }

            public AppLovinAdPosition(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
        }
#endif


        #endregion

        #region  Ad Event Handlers

        public class EM_AppLovinAdsListener : MonoBehaviour
        {
#if EM_APPLOVIN            
            private AppLovinClientImpl adClient;

            private void Start()
            {
                DontDestroyOnLoad(gameObject);
            }

            public void setAdClient(AppLovinClientImpl inputClient)
            {
                this.adClient = inputClient;
            }

            void onAppLovinEventReceived(string ev)
            {
                // Banner
                if (ev.Contains("LOADEDBANNER"))
                {
                    adClient.OnBannerLoadedEvent(true);
                }
                else if (ev.Contains("LOADBANNERFAILED"))
                {
                    adClient.OnBannerLoadedEvent(false);
                }
                else if (ev.Contains("DISPLAYEDBANNER"))
                {
                    adClient.OnBannerDisplayedEvent();
                }

                // Interstitial

                if (ev.Contains("DISPLAYEDINTER"))
                {
                    adClient.OnInterstitialDisplayedEvent();
                }
                else if (ev.Contains("HIDDENINTER"))
                {
                    adClient.OnInterstitialHiddenEvent();
                }
                else if (ev.Contains("LOADEDINTER"))
                {
                    adClient.OnInterstitialLoadedEvent(true);
                }
                else if (string.Equals(ev, "LOADINTERFAILED"))
                {
                    adClient.OnInterstitialLoadedEvent(false);
                }

                // Rewarded

                if (ev.Contains("REWARDAPPROVEDINFO"))
                {
                    // The format would be "REWARDAPPROVEDINFO|AMOUNT|CURRENCY" so "REWARDAPPROVEDINFO|10|Coins" for example
                    char delimeter = '|';

                    string[] split = ev.Split(delimeter);

                    double amount = double.Parse(split[1]);

                    string currencyName = split[2];

                    adClient.OnRewardedCompletedEvent(amount, currencyName);
                }
                else if (ev.Contains("LOADEDREWARDED"))
                {
                    adClient.OnRewardedLoadedEvent(true);
                }
                else if (ev.Contains("LOADREWARDEDFAILED"))
                {
                    adClient.OnRewardedLoadedEvent(false);
                }
                else if (ev.Contains("DISPLAYEDREWARDED"))
                {
                    adClient.OnRewardedDisplayedEvent();
                }
                else if (ev.Contains("HIDDENREWARDED"))
                {
                    adClient.OnRewardedHiddenEvent();
                }
                else if (ev.Contains("USERCLOSEDEARLY"))
                {
                    adClient.OnRewardedCanceledEvent();
                }

                // Ad clicked
                if (ev.Contains("CLICKED"))
                {
                    adClient.OnAdClicked();
                }
            }
#endif
        }

#if EM_APPLOVIN
        private void OnBannerLoadedEvent(bool success)
        {
            if (success)
            {
                Debug.Log("AppLovin: Banner ad loaded.");
            }
            else
            {
                Debug.Log("AppLovin: Banner ad failed to load.");
            }

            if (BannerLoadedEvent != null)
                BannerLoadedEvent(success);
        }

        private void OnBannerDisplayedEvent()
        {
            if (BannerDisplayedEvent != null)
                BannerDisplayedEvent();
        }

        private void OnInterstitialLoadedEvent(bool success)
        {
            if (success)
            {
                Debug.Log("AppLovin: Interstitial ad loaded.");
            }
            else
            {
                Debug.Log("AppLovin: Interstitial ad failed to load.");
            }

            if (InterstitialLoadedEvent != null)
                InterstitialLoadedEvent(success);
        }

        private void OnInterstitialHiddenEvent()
        {
            if (currentShowingInterstitial != null)
            {
                OnInterstitialAdCompleted(currentShowingInterstitial);
                currentShowingInterstitial = null;
            }

            if (InterstitialHiddenEvent != null)
                InterstitialHiddenEvent();
        }

        private void OnInterstitialDisplayedEvent()
        {
            if (InterstitialDisplayedEvent != null)
                InterstitialDisplayedEvent();
        }

        private void OnRewardedLoadedEvent(bool success)
        {
            if (success)
            {
                Debug.Log("AppLovin: Rewarded ad loaded.");
            }
            else
            {
                Debug.Log("AppLovin: Rewarded ad failed to load.");
            }

            if (RewardedLoadedEvent != null)
                RewardedLoadedEvent(success);
        }

        private void OnRewardedHiddenEvent()
        {
            if (currentShowingRewarded != null)
            {
                if (shouldReceiveRewarded)
                {
                    OnRewardedAdCompleted(currentShowingRewarded);
                    currentShowingRewarded = null;
                    shouldReceiveRewarded = false;
                }
                else
                {
                    OnRewardedAdSkipped(currentShowingRewarded);
                    currentShowingRewarded = null;
                }
            }

            if (RewardedHiddenEvent != null)
                RewardedHiddenEvent();
        }

        private void OnRewardedDisplayedEvent()
        {
            if (RewardedDisplayedEvent != null)
                RewardedDisplayedEvent();
        }

        private void OnRewardedCompletedEvent(double amount, string currency)
        {
            shouldReceiveRewarded = true;
            if (RewardedCompletedEvent != null)
                RewardedCompletedEvent(amount, currency);
        }

        private void OnRewardedCanceledEvent()
        {
            shouldReceiveRewarded = false;
            if (RewardedCanceledEvent != null)
                RewardedCanceledEvent();
        }

        private void OnAdClicked()
        {
            if (AdClickedEvent != null)
                AdClickedEvent();
        }

#endif
        #endregion
    }
}