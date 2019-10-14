using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace EasyMobile
{
#if EM_HEYZAP
    using Heyzap;
#endif

    public class HeyzapClientImpl : AdClientImpl
    {
        private const string NO_SDK_MESSAGE = "SDK missing. Please import the Heyzap plugin.";

#if EM_HEYZAP
        private const string HEY_ZAP_DEFAULT_TAG = "default";

        private HeyzapSettings mGlobalAdSettings = null;
#endif

        #region Heyzap Events

#if EM_HEYZAP
        /// <summary>
        /// Event for banner ad activities.
        /// </summary>
        public event HZBannerAd.AdDisplayListener BannerAdCallbackListener;

        /// <summary>
        /// Event for interstitial ad activities.
        /// </summary>
        public event HZInterstitialAd.AdDisplayListener InterstitialAdCallbackListener;

        /// <summary>
        /// Event for rewarded ad activities.
        /// </summary>
        public event HZIncentivizedAd.AdDisplayListener RewardedAdCallbackListener;
#endif

        #endregion  // Heyzap Events

        #region Singleton

        private static HeyzapClientImpl sInstance;

        private HeyzapClientImpl()
        {
        }

        /// <summary>
        /// Returns the singleton client.
        /// </summary>
        /// <returns>The client.</returns>
        public static HeyzapClientImpl CreateClient()
        {
            if (sInstance == null)
            {
                sInstance = new HeyzapClientImpl();
            }
            return sInstance;
        }

        #endregion  // Object Creators

        #region AdClient Overrides

        public override AdNetwork Network { get { return AdNetwork.Heyzap; } }

        public override bool IsBannerAdSupported { get { return true; } }

        public override bool IsInterstitialAdSupported { get { return true; } }

        public override bool IsRewardedAdSupported { get { return true; } }

        public override bool IsSdkAvail
        {
            get
            {
#if EM_HEYZAP
                return true;
#else
                return false;
#endif
            }
        }

        public override bool IsValidPlacement(AdPlacement placement, AdType type)
        {
#if EM_HEYZAP         
            return true;
#else
            return false;
#endif
        }

        protected override Dictionary<AdPlacement, AdId> CustomInterstitialAdsDict
        {
            get
            {
#if EM_HEYZAP
                return mGlobalAdSettings == null ? null : mGlobalAdSettings.CustomInterstitialPlacements.ToDictionary(key => key, _ => new AdId("", ""));
#else
                return null;
#endif
            }
        }

        protected override Dictionary<AdPlacement, AdId> CustomRewardedAdsDict
        {
            get
            {
#if EM_HEYZAP
                return mGlobalAdSettings == null ? null : mGlobalAdSettings.CustomRewardedPlacements.ToDictionary(key => key, _ => new AdId("", ""));
#else
                return null;
#endif
            }
        }

        protected override string NoSdkMessage { get { return NO_SDK_MESSAGE; } }

        protected override void InternalInit()
        {
#if EM_HEYZAP
            // Store a reference to the global settings.
            mGlobalAdSettings = EM_Settings.Advertising.Heyzap;

            // Set GPDR consent (if any) *before* starting the SDK
            // https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements
            var consent = GetApplicableDataPrivacyConsent();
            ApplyDataPrivacyConsent(consent);

            // Start Heyzap with no automatic fetching since we'll handle ad loading.
            HeyzapAds.Start(mGlobalAdSettings.PublisherId, HeyzapAds.FLAG_DISABLE_AUTOMATIC_FETCHING);

            // Add callback handlers
            HZBannerAd.SetDisplayListener(BannerAdCallback);
            HZInterstitialAd.SetDisplayListener(InterstitialAdCallback);
            HZIncentivizedAd.SetDisplayListener(RewardedAdCallback);

            mIsInitialized = true;
            Debug.Log("Heyzap client has been initialized.");
#else
            Debug.LogError(NO_SDK_MESSAGE);
#endif
        }

        //------------------------------------------------------------
        // Show Test Suite (not IAdClient method)
        //------------------------------------------------------------

        public void ShowTestSuite()
        {
#if EM_HEYZAP
            HeyzapAds.ShowMediationTestSuite();
#else
            Debug.LogError(NO_SDK_MESSAGE);
#endif
        }

        //------------------------------------------------------------
        // Banner Ads.
        //------------------------------------------------------------

        protected override void InternalShowBannerAd(AdPlacement placement, BannerAdPosition position, BannerAdSize __)
        {
#if EM_HEYZAP
            HZBannerShowOptions showOptions = new HZBannerShowOptions
            {
                Position = ToHeyzapAdPosition(position),
                Tag = ToHeyzapAdTag(placement)
            };
            HZBannerAd.ShowWithOptions(showOptions);
#endif
        }

        protected override void InternalHideBannerAd(AdPlacement _)
        {
#if EM_HEYZAP
            HZBannerAd.Hide();
#endif
        }

        protected override void InternalDestroyBannerAd(AdPlacement _)
        {
#if EM_HEYZAP
            HZBannerAd.Destroy();
#endif
        }

        //------------------------------------------------------------
        // Interstitial Ads.
        //------------------------------------------------------------

        protected override void InternalLoadInterstitialAd(AdPlacement placement)
        {
#if EM_HEYZAP
            if (placement == AdPlacement.Default)
                HZInterstitialAd.Fetch();
            else
                HZInterstitialAd.Fetch(ToHeyzapAdTag(placement));
#endif
        }

        protected override bool InternalIsInterstitialAdReady(AdPlacement placement)
        {
#if EM_HEYZAP
            if (placement == AdPlacement.Default)
                return HZInterstitialAd.IsAvailable();
            else
                return HZInterstitialAd.IsAvailable(ToHeyzapAdTag(placement));
#else
            return false;
#endif
        }

        protected override void InternalShowInterstitialAd(AdPlacement placement)
        {
#if EM_HEYZAP
            if (placement == AdPlacement.Default)
            {
                HZInterstitialAd.Show();
            }
            else
            {
                var options = new HZShowOptions() { Tag = ToHeyzapAdTag(placement) };
                HZInterstitialAd.ShowWithOptions(options);
            }
#endif
        }

        //------------------------------------------------------------
        // Rewarded Ads.
        //------------------------------------------------------------

        protected override void InternalLoadRewardedAd(AdPlacement placement)
        {
#if EM_HEYZAP
            if (placement == AdPlacement.Default)
                HZIncentivizedAd.Fetch();
            else
                HZIncentivizedAd.Fetch(ToHeyzapAdTag(placement));
#endif
        }

        protected override bool InternalIsRewardedAdReady(AdPlacement placement)
        {
#if EM_HEYZAP
            if (placement == AdPlacement.Default)
                return HZIncentivizedAd.IsAvailable();
            else
                return HZIncentivizedAd.IsAvailable(ToHeyzapAdTag(placement));
#else
            return false;
#endif
        }

        protected override void InternalShowRewardedAd(AdPlacement placement)
        {
#if EM_HEYZAP
            if (placement == AdPlacement.Default)
            {
                HZIncentivizedAd.Show();
            }
            else
            {
                var options = new HZIncentivizedShowOptions() { Tag = ToHeyzapAdTag(placement) };
                HZIncentivizedAd.ShowWithOptions(options);
            }
#endif
        }

        #endregion  // AdClient Overrides

        #region IConsentRequirable Overrides

        private const string DATA_PRIVACY_CONSENT_KEY = "EM_Ads_Heyzap_DataPrivacyConsent";

        protected override string DataPrivacyConsentSaveKey { get { return DATA_PRIVACY_CONSENT_KEY; } }

        protected override void ApplyDataPrivacyConsent(ConsentStatus consent)
        {
#if EM_HEYZAP
            // Only set the GDPR consent if an explicit one is specified.
            switch (consent)
            {
                case ConsentStatus.Granted:
                    HeyzapAds.SetGdprConsent(true);
                    break;
                case ConsentStatus.Revoked:
                    HeyzapAds.SetGdprConsent(false);
                    break;
                case ConsentStatus.Unknown:
                default:
                    break;
            }
#endif
        }

        #endregion

        #region Helpers

#if EM_HEYZAP

        private string ToHeyzapAdPosition(BannerAdPosition pos)
        {
            switch (pos)
            {
                case BannerAdPosition.TopLeft:
                case BannerAdPosition.TopRight:
                case BannerAdPosition.Top:
                    return HZBannerShowOptions.POSITION_TOP;
                case BannerAdPosition.BottomLeft:
                case BannerAdPosition.BottomRight:
                case BannerAdPosition.Bottom:
                    return HZBannerShowOptions.POSITION_BOTTOM;
                default:
                    return HZBannerShowOptions.POSITION_TOP;
            }
        }

        private string ToHeyzapAdTag(AdPlacement placement)
        {
            return AdPlacement.GetPrintableName(placement);
        }

        private AdPlacement ToAdPlacement(string heyzapAdTag)
        {
            return (string.IsNullOrEmpty(heyzapAdTag) || heyzapAdTag.Equals(HEY_ZAP_DEFAULT_TAG))
                    ? AdPlacement.Default
                    : AdPlacement.PlacementWithName(heyzapAdTag);
        }

#endif

        #endregion

        #region Ad Event Handlers

#if EM_HEYZAP

        private void BannerAdCallback(string adState, string adTag)
        {
            if (adState == "loaded")
            {
                // Do something when the banner ad is loaded
                Debug.Log("Heyzap banner ad has been loaded.");
            }
            if (adState == "error")
            {
                // Do something when the banner ad fails to load (they can fail when refreshing after successfully loading)
                Debug.Log("Heyzap banner ad failed to load.");
            }
            if (adState == "click")
            {
                // Do something when the banner ad is clicked, like pause your game
            }

            if (BannerAdCallbackListener != null)
                BannerAdCallbackListener(adState, adTag);
        }

        private void InterstitialAdCallback(string adState, string adTag)
        {
            if (adState.Equals("show"))
            {
                // Sent when an ad has been displayed.
                // This is a good place to pause your app, if applicable.
            }
            else if (adState.Equals("hide"))
            {
                // Sent when an ad has been removed from view.
                // This is a good place to unpause your app, if applicable.
                OnInterstitialAdCompleted(ToAdPlacement(adTag));
            }
            else if (adState.Equals("failed"))
            {
                // Sent when you call `show`, but there isn't an ad to be shown.
                // Some of the possible reasons for show errors:
                //    - `HeyzapAds.PauseExpensiveWork()` was called, which pauses 
                //      expensive operations like SDK initializations and ad
                //      fetches, andand `HeyzapAds.ResumeExpensiveWork()` has not
                //      yet been called
                //    - The given ad tag is disabled (see your app's Publisher
                //      Settings dashboard)
                //    - An ad is already showing
                //    - A recent IAP is blocking ads from being shown (see your
                //      app's Publisher Settings dashboard)
                //    - One or more of the segments the user falls into are
                //      preventing an ad from being shown (see your Segmentation
                //      Settings dashboard)
                //    - Incentivized ad rate limiting (see your app's Publisher
                //      Settings dashboard)
                //    - One of the mediated SDKs reported it had an ad to show
                //      but did not display one when asked (a rare case)
                //    - The SDK is waiting for a network request to return before an
                //      ad can show
            }
            else if (adState.Equals("available"))
            {
                // Sent when an ad has been loaded and is ready to be displayed,
                //   either because we autofetched an ad or because you called
                //   `Fetch`.
                Debug.Log("Heyzap interstitial ad has been loaded.");
            }
            else if (adState.Equals("fetch_failed"))
            {
                // Sent when an ad has failed to load.
                // This is sent with when we try to autofetch an ad and fail, and also
                //    as a response to calls you make to `Fetch` that fail.
                // Some of the possible reasons for fetch failures:
                //    - Incentivized ad rate limiting (see your app's Publisher
                //      Settings dashboard)
                //    - None of the available ad networks had any fill
                //    - Network connectivity
                //    - The given ad tag is disabled (see your app's Publisher
                //      Settings dashboard)
                //    - One or more of the segments the user falls into are
                //      preventing an ad from being fetched (see your
                //      Segmentation Settings dashboard)
                Debug.Log("Heyzap interstitial ad failed to load.");
            }
            else if (adState.Equals("audio_starting"))
            {
                // The ad about to be shown will need audio.
                // Mute any background music.
            }
            else if (adState.Equals("audio_finished"))
            {
                // The ad being shown no longer needs audio.
                // Any background music can be resumed.
            }

            if (InterstitialAdCallbackListener != null)
                InterstitialAdCallbackListener(adState, adTag);
        }

        private void RewardedAdCallback(string adState, string adTag)
        {
            if (adState.Equals("incentivized_result_complete"))
            {
                // The user has watched the entire video and should be given a reward.
                OnRewardedAdCompleted(ToAdPlacement(adTag));
            }
            else if (adState.Equals("incentivized_result_incomplete"))
            {
                // The user did not watch the entire video and should not be given a reward.
                OnRewardedAdSkipped(ToAdPlacement(adTag));
            }

            if (RewardedAdCallbackListener != null)
                RewardedAdCallbackListener(adState, adTag);
        }

#endif

        #endregion  // Ad Event Handlers
    }
}