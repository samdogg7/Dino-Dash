using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace EasyMobile.Editor
{
    // Partial editor class for Advertising module.
    internal partial class EM_SettingsEditor
    {
        const string AdModuleLabel = "ADVERTISING";
        const string AdModuleIntro = "The Advertising module offers a unified API for a wide range of ad networks and other features that enable fast and flexible ads integration for your game.";
        const string AdColonyImportInstruction = "AdColony plugin not found. Please download and import it to show ads from AdColony.";
        const string AdColonyAvailMsg = "AdColony plugin was imported.";
        const string AdMobImportInstruction = "Google Mobile Ads (AdMob) plugin not found. Please download and import it to show ads from AdMob.";
        const string AdMobAvailMsg = "Google Mobile Ads (AdMob) plugin was imported.";
        const string SetupGoogleMobileAdsMsg = "Click the below button to setup the Google Mobile Ads plugin with your App IDs. The entered IDs will be reflected here. IMPORTANT: failure to do this will cause AdMob ads to not function properly.";
        const string AppLovinImportInstruction = "AppLovin plugin not found. Please download and import it to show ads from AppLovin.";
        const string AppLovinAvailMsg = "AppLovin plugin was imported.";
        const string ChartboostImportInstruction = "Chartboost plugin not found. Please download and import it to show ads from Chartboost.";
        const string ChartboostAvailMsg = "Chartboost plugin was imported.";
        const string ChartboostDefaultAdPlacementMsg = "Easy Mobile's Default ad placement is directly translated into Chartboost's Default ad location, no need to enter any associated ID here. The Default placement is loaded automatically if LoadAllDefinedPlacements mode is enabled.";
        const string ChartboostCustomAdPlacementMsg = "Here you can register the custom placements to be automatically loaded with LoadAllDefinedPlacements mode. These placements are directly translated into Chartboost's ad locations of the same name, no need to enter any associated IDs here.";
        const string FBAudienceImportInstruction = "Facebook Audience Network plugin not found. Please download and import it to show ads from FB Audience.";
        const string FBAudienceAvailMsg = "Facebook Audience Network plugin was imported.";
        const string HeyzapImportInstruction = "Heyzap plugin not found. Please download and import it to show ads from Heyzap.";
        const string HeyzapAvailMsg = "Heyzap plugin was imported.";
        const string HeyzapDefaultAdPlacementMsg = "Easy Mobile's Default ad placement is directly translated into Heyzap's default ad tag, no need to enter any associated ID here. The Default placement is loaded automatically if LoadAllDefinedPlacements mode is enabled.";
        const string HeyzapCustomAdPlacementMsg = "Here you can register the custom placements to be automatically loaded with LoadAllDefinedPlacements mode. These placements are directly translated into Heyzap's ad tags of the same name, no need to enter any associated IDs here.";
        const string MoPubImportInstruction = "MoPub plugin not found. Please download and import it to show ads from MoPub.";
        const string MoPubAvailMsg = "MoPub plugin was imported.";
        const string IronSourceImportInstruction = "IronSource plugin not found. Please download and import it to show ads from IronSource.";
        const string IronSourceAvailMsg = "IronSource plugin was imported.";
        const string IronSourceAdPlacementMsg = "ironSource doesn't require placements when loading ads. Their ads are loaded automatically if auto ad loading is enabled, no need to register any placement here. You can specify a placement when showing ads if needed.";
        const string TapJoyImportInstruction = "Tapjoy plugin not found. Please download and import it to show ads from Tapjoy.";
        const string TapJoyAvailMsg = "Tapjoy plugin was imported.";
        const string UnityAdsUnvailableWarning = "Unity Ads service is disabled or not available for the current platform. To enable it go to Window > Services and make sure the current platform is iOS or Android.";
        const string UnityAdsAvailableMsg = "Unity Ads service is enabled.";
        const string UnityAdsDefaultPlacementsMsg = "The below interstitial and rewarded ad placement IDs match the default IDs generated automatically by Unity Ads . " +
                                                    "These IDs are not editable and you should avoid repeating them when creating custom placements.";
        const string UnityAdsMonetizationSDKRequiredMsg = "The current built-in Unity Ads service doesn't support banner ads. Please import the Unity Monetization SDK from Assets Store to use Unity's banner ads.";
        const string AdvertisingConstantGenerationIntro = "Generate the static class " + EM_Constants.RootNameSpace + "." + EM_Constants.AdvertisingConstantsClassName + " that contains the constants of the above ad IDs." +
                                                          " Remember to regenerate if you make changes to these IDs.";


#if EM_MOPUB
        bool mopubMediatedNetworksFoldout = false;
        GUIContent mopubAdapterNameContent = new GUIContent("Adapter Configuration Class Name", "Specify the class name that implements the AdapterConfiguration interface.");
        GUIContent mopubMediationNameContent = new GUIContent("Mediation Settings Class Name", "Specify the class name that implements the MediationSettings interface.\n Note: Custom network mediation settings are currently not supported on Android.");
        GUIContent mopubNetworkConfigurationContent = new GUIContent("Network Configuration", "Network adapter configuration settings (initialization).");
        GUIContent mopubRequestOptionContent = new GUIContent("Request Option", "Additional options to pass to the MoPub servers (per ad request).");

#endif

        void AdModuleGUI()
        {
            DrawModuleHeader();

            // Now draw the GUI.
            if (!isAdModuleEnable.boolValue)
                return;

            // Ads auto-load setup
            EditorGUILayout.Space();
            DrawUppercaseSection("AUTO_AD_LOADING_CONFIG_FOLDOUT_KEY", "AUTO AD-LOADING", () =>
                {
                    EditorGUILayout.PropertyField(AdProperties.autoLoadAdsMode.property, AdProperties.autoLoadAdsMode.content);
                    if (EM_Settings.Advertising.AutoAdLoadingMode != AutoAdLoadingMode.None)
                    {
                        EditorGUILayout.PropertyField(AdProperties.adCheckingInterval.property, AdProperties.adCheckingInterval.content);
                        EditorGUILayout.PropertyField(AdProperties.adLoadingInterval.property, AdProperties.adLoadingInterval.content);
                    }
                });

            EditorGUILayout.Space();
            DrawUppercaseSection("DEFAULT_AD_NETWORKS_FOLDOUT_KEY", "DEFAULT AD NETWORKS", () =>
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.iosDefaultAdNetworks.property, AdProperties.iosDefaultAdNetworks.content, true);
                    EditorGUILayout.PropertyField(AdProperties.androidDefaultAdNetworks.property, AdProperties.androidDefaultAdNetworks.content, true);
                    EditorGUI.indentLevel--;

                    // Now check if there's any default ad network that doesn't have plugin imported and show warnings.
                    AdSettings.DefaultAdNetworks iosDefault = EM_Settings.Advertising.IosDefaultAdNetworks;
                    AdSettings.DefaultAdNetworks androidDefault = EM_Settings.Advertising.AndroidDefaultAdNetworks;
                    List<AdNetwork> usedNetworks = new List<AdNetwork>();
                    AddWithoutRepeat(usedNetworks, (AdNetwork)iosDefault.bannerAdNetwork);
                    AddWithoutRepeat(usedNetworks, (AdNetwork)iosDefault.interstitialAdNetwork);
                    AddWithoutRepeat(usedNetworks, (AdNetwork)iosDefault.rewardedAdNetwork);
                    AddWithoutRepeat(usedNetworks, (AdNetwork)androidDefault.bannerAdNetwork);
                    AddWithoutRepeat(usedNetworks, (AdNetwork)androidDefault.interstitialAdNetwork);
                    AddWithoutRepeat(usedNetworks, (AdNetwork)androidDefault.rewardedAdNetwork);

                    bool addedSpace = false;

                    foreach (AdNetwork network in usedNetworks)
                    {
                        if (!IsPluginAvail(network))
                        {
                            if (!addedSpace)
                            {
                                EditorGUILayout.Space();
                                addedSpace = true;
                            }
                            EditorGUILayout.HelpBox("Default ad network " + network.ToString() + " has no SDK. Please import its plugin.", MessageType.Warning);
                        }
                    }

                    if (((AdNetwork)androidDefault.bannerAdNetwork == AdNetwork.UnityAds) || ((AdNetwork)iosDefault.bannerAdNetwork == AdNetwork.UnityAds))
                    {
                        if (IsPluginAvail(AdNetwork.UnityAds) && !IsUnityMonetizationAvail())
                        {
                            if (!addedSpace)
                            {
                                EditorGUILayout.Space();
                                addedSpace = true;
                            }
                            EditorGUILayout.HelpBox(UnityAdsMonetizationSDKRequiredMsg, MessageType.Warning);
                        }
                    }
                });

            // AdColony setup
            DrawAdColonySettings();

            // AdMob setup
            DrawAdMobSettings();

            // AppLovin setup
            DrawAppLovinSettings();

            // Audience Network setup
            DrawAudienceNetworkSettings();

            // Chartboost setup
            DrawChartboostSettings();

            // Heyzap setup
            DrawHeyzapSettings();

            // IronSource setup
            DrawIronSourceSettings();

            // MoPub setup
            DrawMopubSettings();

            // TapJoy setup
            DrawTapjoySettings();

            // UnityAds setup
            DrawUnityAdsSettings();
            CheckUnityAdsAutoInit();
        }

        static string AdMobManifestPath
        {
            get
            {
                return FileIO.ToAbsolutePath("Plugins/Android/GoogleMobileAdsPlugin/AndroidManifest.xml");
            }
        }

        void DrawAdColonySettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("ADCOLONY_SETUP_FOLDOUT_KEY", "ADCOLONY", () =>
                {
#if !EM_ADCOLONY
                    EditorGUILayout.HelpBox(AdColonyImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download AdColony Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadAdColonyPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(AdColonyAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download AdColony Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadAdColonyPlugin();
                    }

                    // App ID.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("App ID", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.adColonyAppId.property, AdProperties.adColonyAppId.content, true);
                    EditorGUI.indentLevel--;

                    // Default Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.adColonyDefaultInterstitialAdId.property, AdProperties.adColonyDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.adColonyDefaultRewardedAdId.property, AdProperties.adColonyDefaultRewardedAdId.content, true);
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.adColonyCustomInterstitialAdIds.property, AdProperties.adColonyCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.adColonyCustomRewardedAdIds.property, AdProperties.adColonyCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;

                    // Ad settings.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Ad Settings", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.adColonyAdOrientation.property, AdProperties.adColonyAdOrientation.content);
                    EditorGUILayout.PropertyField(AdProperties.adColonyEnableRewardedAdPrePopup.property, AdProperties.adColonyEnableRewardedAdPrePopup.content);
                    EditorGUILayout.PropertyField(AdProperties.adColonyEnableRewardedAdPostPopup.property, AdProperties.adColonyEnableRewardedAdPostPopup.content);
#endif
                });
        }

        void DrawAdMobSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("ADMOB_SETUP_FOLDOUT_KEY", "ADMOB", () =>
                {
#if !EM_ADMOB
                    EditorGUILayout.HelpBox(AdMobImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download Google Mobile Ads Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadGoogleMobileAdsPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(AdMobAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download Google Mobile Ads Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadGoogleMobileAdsPlugin();
                    }

                    // Setup GoogleMobileAds.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox(SetupGoogleMobileAdsMsg, MessageType.Info);

                    // Get the App IDs from GoogleMobileAdsSettings.
                    var iOSAppId = GoogleMobileAds.Editor.GoogleMobileAdsSettings.Instance.AdMobIOSAppId;
                    var androidAppId = GoogleMobileAds.Editor.GoogleMobileAdsSettings.Instance.AdMobAndroidAppId;

                    if (!EM_Settings.Advertising.AdMob.AppId.IosId.Equals(iOSAppId) ||
                    !EM_Settings.Advertising.AdMob.AppId.AndroidId.Equals(androidAppId))
                        EM_Settings.Advertising.AdMob.AppId = new AdId(iOSAppId, androidAppId);

                    // Display the App IDs as readonly.
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.admobAppId.property, AdProperties.admobAppId.content, true);
                    EditorGUI.indentLevel--;
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.Space();
                    if (GUILayout.Button("Setup Google Mobile Ads", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        GoogleMobileAds.Editor.GoogleMobileAdsSettingsEditor.OpenInspector();
                        EditorWindow.GetWindow(EM_EditorUtil.GetInspectorWindowType()).Focus();
                    }

                    // Default placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.admobDefaultBannerAdId.property, AdProperties.admobDefaultBannerAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.admobDefaultInterstitialAdId.property, AdProperties.admobDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.admobDefaultRewardedAdId.property, AdProperties.admobDefaultRewardedAdId.content, true);
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.admobCustomBannerAdIds.property, AdProperties.admobCustomBannerAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.admobCustomInterstitialAdIds.property, AdProperties.admobCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.admobCustomRewardedAdIds.property, AdProperties.admobCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;

                    // Ad targeting settings.
                    EditorGUILayout.Space();
                    var childDirected = AdProperties.admobTargetingSettings.property.FindPropertyRelative("mTagForChildDirectedTreatment");
                    var extras = AdProperties.admobTargetingSettings.property.FindPropertyRelative("mExtraOptions");
                    EditorGUILayout.LabelField(AdProperties.admobTargetingSettings.content, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(childDirected, new GUIContent("Tag For Child Directed Treatment"));
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(extras, new GUIContent("Extra Options"));
                    EditorGUI.indentLevel--;

                    // Test mode.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Test Mode", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.admobEnableTestMode.property, AdProperties.admobEnableTestMode.content);

#endif
                });
        }

        void UpdateAdMobAppIdInManifest()
        {
            if (!File.Exists(AdMobManifestPath))
            {
                EM_EditorUtil.Alert("AndroidManifest Not Found", "Can't locate GoogleMobileAds AndroidManifest.xml at " + AdMobManifestPath);
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(AdMobManifestPath);

            string baseErrorMsg = "Failed to update Google Mobile Ads' AndroidManifest.xml with your AdMob Android App ID: ";

            XmlNode manifest = xmlDocument.SelectSingleNode("manifest");
            if (manifest == null)
            {
                Debug.Log(baseErrorMsg + "Couldn't find any manifest element in the AndroidManifest.xml at " + AdMobManifestPath);
                return;
            }

            XmlNode application = manifest.SelectSingleNode("application");
            if (application == null)
            {
                Debug.Log(baseErrorMsg + "Couldn't find any valid application element in the AndroidManifest.xml at " + AdMobManifestPath);
                return;
            }

            XmlNodeList metadatas = application.SelectNodes("meta-data");
            if (metadatas == null)
            {
                Debug.Log(baseErrorMsg + "Couldn't find any valid meta-data element in the AndroidManifest.xml at " + AdMobManifestPath);
                return;
            }

            for (int i = 0; i < metadatas.Count; i++)
            {
                foreach (XmlAttribute attribute in metadatas[i].Attributes)
                {
                    if (attribute.Name == "android:value")
                    {
                        string androidAppId = AdProperties.admobAppId.property.FindPropertyRelative("mAndroidId").stringValue;
                        attribute.Value = androidAppId;
                        xmlDocument.Save(AdMobManifestPath);

                        EM_EditorUtil.Alert("AndroidManifest Updated", "Google Mobile Ads' AndroidManifest.xml has been updated with your AdMob Android App ID.");
                        return;
                    }
                }
            }

            EM_EditorUtil.Alert("AndroidManifest Update Failed", baseErrorMsg + "Couldn't find any valid attribute in the AndroidManifest.xml at " + AdMobManifestPath);
        }

        void DrawAppLovinSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("APPLOVIN_SETUP_FOLDOUT_KEY", "APPLOVIN", () =>
                {
#if !EM_APPLOVIN
                    EditorGUILayout.HelpBox(AppLovinImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download AppLovin Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadAppLovinPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(AppLovinAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download AppLovin Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadAppLovinPlugin();
                    }

                    // App ID.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("SDK Key", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.appLovinSDKKey.property, AdProperties.appLovinSDKKey.content, true);
                    EditorGUI.indentLevel--;

                    // Default placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.appLovinDefaultBannerAdId.property, AdProperties.appLovinDefaultBannerAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.appLovinDefaultInterstitialAdId.property, AdProperties.appLovinDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.appLovinDefaultRewardedAdId.property, AdProperties.appLovinDefaultRewardedAdId.content, true);
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.appLovinCustomBannerAdIds.property, AdProperties.appLovinCustomBannerAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.appLovinCustomInterstitialAdIds.property, AdProperties.appLovinCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.appLovinCustomRewardedAdIds.property, AdProperties.appLovinCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;

                    // Test mode.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Test Mode", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.appLovinEnableTestMode.property, AdProperties.appLovinEnableTestMode.content);

                    // Age-restricted.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Age-restricted", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.appLovinEnableAgeRestricted.property, AdProperties.appLovinEnableAgeRestricted.content);

#endif
                });
        }

        void DrawChartboostSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("CHARTBOOST_SETUP_FOLDOUT_KEY", "CHARTBOOST", () =>
                {
#if !EM_CHARTBOOST
                    EditorGUILayout.HelpBox(ChartboostImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download Chartboost Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadChartboostPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(ChartboostAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download Chartboost Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadChartboostPlugin();
                    }

                    // Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox(ChartboostDefaultAdPlacementMsg, MessageType.None);

                    // Custom Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox(ChartboostCustomAdPlacementMsg, MessageType.None);

                    EditorGUI.indentLevel++;
                    if (DrawPropertyAsResizableArray(AdProperties.chartboostCustomInterstitialPlacements.property, AdProperties.chartboostCustomInterstitialPlacements.content, null, null, true))
                        DrawAllElementsInArrayProperty(AdProperties.chartboostCustomInterstitialPlacements.property);
                    EditorGUI.indentLevel--;

                    EditorGUI.indentLevel++;
                    if (DrawPropertyAsResizableArray(AdProperties.chartboostCustomRewardedPlacements.property, AdProperties.chartboostCustomRewardedPlacements.content, null, null, true))
                        DrawAllElementsInArrayProperty(AdProperties.chartboostCustomRewardedPlacements.property);
                    EditorGUI.indentLevel--;

                    // Setup.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox("Chartboost can be setup in dedicated window.", MessageType.None);
                    if (GUILayout.Button("Setup Chartboost", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        // Open Chartboost settings window.
                        ChartboostSDK.CBSettings.Edit();
                        EditorWindow.GetWindow(EM_EditorUtil.GetInspectorWindowType()).Focus();
                    }
#endif
                });
        }

        void DrawAudienceNetworkSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("FACEBOOK_AUDIENCE_NETWORK_SETUP_FOLDOUT_KEY", "AUDIENCE NETWORK", () =>
                {
#if !EM_FBAN
                    EditorGUILayout.HelpBox(FBAudienceImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download FB Audience Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadFacebookAudiencePlugin();
                    }
#else
                    EditorGUILayout.HelpBox(FBAudienceAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download FB Audience Network Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadFacebookAudiencePlugin();
                    }

                    // Default placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceDefaultBannerAdId.property, AdProperties.fbAudienceDefaultBannerAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceDefaultInterstitialAdId.property, AdProperties.fbAudienceDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceDefaultRewardedAdId.property, AdProperties.fbAudienceDefaultRewardedAdId.content, true);
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceCustomBannerAdIds.property, AdProperties.fbAudienceCustomBannerAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceCustomInterstitialAdIds.property, AdProperties.fbAudienceCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceCustomRewardedAdIds.property, AdProperties.fbAudienceCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;

                    // Test mode.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Test Mode", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.fbAudienceEnableTestMode.property, AdProperties.fbAudienceEnableTestMode.content);
                    if (AdProperties.fbAudienceEnableTestMode.property.boolValue)
                    {
                        DrawPropertyAsResizableArray(AdProperties.fbAudienceTestDeviceIds.property, "Test Device IDs");
                        DrawAllElementsInArrayProperty(AdProperties.fbAudienceTestDeviceIds.property);
                    }
                    else
                    {
                        AdProperties.fbAudienceTestDeviceIds.property.ClearArray();
                    }
#endif
                });
        }

        void DrawHeyzapSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("HEYZAP_SETUP_FOLDOUT_KEY", "HEYZAP (FYBER)", () =>
                {
#if !EM_HEYZAP
                    EditorGUILayout.HelpBox(HeyzapImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download Heyzap Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadHeyzapPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(HeyzapAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download Heyzap Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadHeyzapPlugin();
                    }
                    EditorGUILayout.Space();

                    // Publisher ID.
                    EditorGUILayout.LabelField("Publisher ID", EditorStyles.boldLabel);
                    AdProperties.heyzapPublisherId.property.stringValue = EditorGUILayout.TextField(AdProperties.heyzapPublisherId.content, AdProperties.heyzapPublisherId.property.stringValue);

                    // Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox(HeyzapDefaultAdPlacementMsg, MessageType.None);

                    // Custom Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox(HeyzapCustomAdPlacementMsg, MessageType.None);

                    EditorGUI.indentLevel++;
                    if (DrawPropertyAsResizableArray(AdProperties.heyzapCustomInterstitialPlacements.property, AdProperties.heyzapCustomInterstitialPlacements.content, null, null, true))
                        DrawAllElementsInArrayProperty(AdProperties.heyzapCustomInterstitialPlacements.property);
                    EditorGUI.indentLevel--;

                    EditorGUI.indentLevel++;
                    if (DrawPropertyAsResizableArray(AdProperties.heyzapCustomRewardedPlacements.property, AdProperties.heyzapCustomRewardedPlacements.content, null, null, true))
                        DrawAllElementsInArrayProperty(AdProperties.heyzapCustomRewardedPlacements.property);
                    EditorGUI.indentLevel--;

                    // Test mode.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Test Mode", EditorStyles.boldLabel);
                    AdProperties.heyzapShowTestSuite.property.boolValue = EditorGUILayout.Toggle(AdProperties.heyzapShowTestSuite.content, AdProperties.heyzapShowTestSuite.property.boolValue);
#endif
                });
        }

        void DrawIronSourceSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("IRONSOURCE_SETUP_FOLDOUT_KEY", "IRONSOURCE", () =>
                {
#if !EM_IRONSOURCE
                    EditorGUILayout.HelpBox(IronSourceImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download IronSource Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadIronSourcePlugin();
                    }
#else
                    EditorGUILayout.HelpBox(IronSourceAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download IronSource Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadIronSourcePlugin();
                    }

                    // App keys.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("App Key", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.ironSourceAppKey.property, AdProperties.ironSourceAppKey.content, true);
                    EditorGUI.indentLevel--;

                    // Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Placements", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox(IronSourceAdPlacementMsg, MessageType.None);

                    // Advanced setting
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.ironSourceUseAdvancedSetting.property, AdProperties.ironSourceUseAdvancedSetting.content);
                    if (AdProperties.ironSourceUseAdvancedSetting.property.boolValue)
                    {
                        EditorGUILayout.Space();

                        // Draw segment settings.
                        var age = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("age");
                        var gender = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("gender");
                        var level = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("level");
                        var isPaying = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("isPaying");
                        var userCreationDate = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("userCreationDate");
                        var iapt = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("iapt");
                        var segmentName = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("segmentName");
                        var customParams = AdProperties.ironSourceSegmentSettings.property.FindPropertyRelative("customParams");

                        EditorGUILayout.PropertyField(segmentName);
                        EditorGUILayout.PropertyField(age);
                        EditorGUILayout.PropertyField(gender);
                        EditorGUILayout.PropertyField(level);
                        EditorGUILayout.PropertyField(isPaying);
                        EditorGUILayout.PropertyField(userCreationDate);
                        EditorGUILayout.PropertyField(iapt);

                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(customParams);
                        EditorGUI.indentLevel--;
                    }
#endif
                });
        }

        #region MoPub

        void DrawMopubSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("MOPUB_ADS_SETUP_FOLDOUT_KEY", "MOPUB ADS", () =>
                {
#if !EM_MOPUB
                    EditorGUILayout.HelpBox(MoPubImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download MoPub Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadMoPubPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(MoPubAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download MoPub Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadMoPubPlugin();
                    }

                    // Default placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.mopubDefaultBannerAdId.property, AdProperties.mopubDefaultBannerAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.mopubDefaultInterstitialAdId.property, AdProperties.mopubDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.mopubDefaultRewardedAdId.property, AdProperties.mopubDefaultRewardedAdId.content, true);
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.mopubCustomBannerAdIds.property, AdProperties.mopubCustomBannerAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.mopubCustomInterstitialAdIds.property, AdProperties.mopubCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.mopubCustomRewardedAdIds.property, AdProperties.mopubCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;

                    if (AdProperties.mopubReportAppOpen.property.boolValue)
                    {                 
                        /// We only need to provide app ID on IOS,
                        /// on Android the report app open feature can be called without any parameter.
#if UNITY_IOS
                        EditorGUILayout.PropertyField(AdProperties.mopubITuneAppID.property, AdProperties.mopubITuneAppID.content, true);
#else
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("App ID is only required on iOS.", EditorStyles.miniBoldLabel);
                        EditorGUILayout.EndVertical();
#endif
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.mopubEnableLocationPassing.property, AdProperties.mopubEnableLocationPassing.content);

                    EditorGUILayout.PropertyField(AdProperties.mopubEnableAdvancedSetting.property, AdProperties.mopubEnableAdvancedSetting.content);
                    if (AdProperties.mopubEnableAdvancedSetting.property.boolValue)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(AdProperties.mopubAllowLegitimateInterest.property, AdProperties.mopubAllowLegitimateInterest.content);
                        EditorGUILayout.PropertyField(AdProperties.mopubLogLevel.property, AdProperties.mopubLogLevel.content);
                        mopubMediatedNetworksFoldout = DrawResizableArray(
                            AdProperties.mopubMediatedNetworks.property,
                            mopubMediatedNetworksFoldout,
                            "Mediated Networks",
                            (property, index) => DrawArrayElement(AdProperties.mopubMediatedNetworks.property, index, "", 0, () => 0, i =>
                                {
                                }, DrawMediatedNetwork),
                            property =>
                            {
                                property.FindPropertyRelative("mIsSupportedNetwork").boolValue = false;
                                property.FindPropertyRelative("mAdapterConfigurationClassName").stringValue = "";
                                property.FindPropertyRelative("mMediationSettingsClassName").stringValue = "";
                                property.FindPropertyRelative("mSupportedNetworkName").intValue = 0;
                            });
                        EditorGUILayout.EndVertical();
                    }
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("GDPR Consent", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.mopubAutoRequestConsent.property, AdProperties.mopubAutoRequestConsent.content);
                    EditorGUILayout.PropertyField(AdProperties.mopubForceGdprApplicable.property, AdProperties.mopubForceGdprApplicable.content);
#endif
                });
        }


        void DrawMediatedNetwork(SerializedProperty networkProperty)
        {
#if EM_MOPUB
            var isSupportedNetworkProperty = networkProperty.FindPropertyRelative("mIsSupportedNetwork");
            EditorGUILayout.PropertyField(isSupportedNetworkProperty);

            if (isSupportedNetworkProperty.boolValue)
            {
                EditorGUILayout.PropertyField(networkProperty.FindPropertyRelative("mSupportedNetworkName"));
            }
            else
            {
                EditorGUILayout.PropertyField(networkProperty.FindPropertyRelative("mAdapterConfigurationClassName"), mopubAdapterNameContent);
                EditorGUILayout.PropertyField(networkProperty.FindPropertyRelative("mMediationSettingsClassName"), mopubMediationNameContent);
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(networkProperty.FindPropertyRelative("mNetworkConfiguration"), mopubNetworkConfigurationContent);
            EditorGUILayout.PropertyField(networkProperty.FindPropertyRelative("mMediationSettings"), mopubMediationNameContent);
            EditorGUILayout.PropertyField(networkProperty.FindPropertyRelative("mMoPubRequestOption"), mopubRequestOptionContent);
            EditorGUI.indentLevel--;
#endif
        }

        #endregion

        void DrawTapjoySettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("TAPJOY_SETUP_FOLDOUT_KEY", "TAPJOY", () =>
                {
#if !EM_TAPJOY
                    EditorGUILayout.HelpBox(TapJoyImportInstruction, MessageType.Warning);
                    if (GUILayout.Button("Download TapJoy Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadTapJoyPlugin();
                    }
#else
                    EditorGUILayout.HelpBox(TapJoyAvailMsg, MessageType.Info);
                    if (GUILayout.Button("Download Tapjoy Plugin", GUILayout.Height(EM_GUIStyleManager.buttonHeight)))
                    {
                        EM_ExternalPluginManager.DownloadTapJoyPlugin();
                    }

                    // Default placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.tapjoyDefaultInterstitialAdId.property, AdProperties.tapjoyDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.tapjoyDefaultRewardedAdId.property, AdProperties.tapjoyDefaultRewardedAdId.content, true);
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.tapjoyCustomInterstitialAdIds.property, AdProperties.tapjoyCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.tapjoyCustomRewardedAdIds.property, AdProperties.tapjoyCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Auto Reconnect Settings", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.tapjoyAutoReconnect.property, AdProperties.tapjoyAutoReconnect.content, true);
                    if (AdProperties.tapjoyAutoReconnect.property.boolValue)
                    {
                        EditorGUILayout.PropertyField(AdProperties.tapjoyAutoReconnectInterval.property, AdProperties.tapjoyAutoReconnectInterval.content, true);

                        if (AdProperties.tapjoyAutoReconnectInterval.property.floatValue < 0.1)
                            AdProperties.tapjoyAutoReconnectInterval.property.floatValue = 0.1f;
                    }

                    EditorGUILayout.Space();
                    if (GUILayout.Button("Open Tapjoy Setup Window"))
                    {
                        EditorWindow.GetWindow<TapjoyEditor.TapjoyWindow>().BeginWindows();
                    }

                    if (GUILayout.Button("Generate Android Manifest"))
                    {
                        GenerateTapJoyAndroidManifest();
                    }
#endif
                });
        }

        void DrawUnityAdsSettings()
        {
            EditorGUILayout.Space();
            DrawUppercaseSection("UNITY_ADS_SETUP_FOLDOUT_KEY", "UNITY ADS", () =>
                {
#if (!UNITY_ADS && !UNITY_MONETIZATION)
                    EditorGUILayout.HelpBox(UnityAdsUnvailableWarning, MessageType.Warning);
#else
                    EditorGUILayout.HelpBox(UnityAdsAvailableMsg, MessageType.Info);

#if UNITY_MONETIZATION
                    // APP ID
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("App ID", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.unityAdsAppId.property, AdProperties.unityAdsAppId.content, true);
                    EditorGUI.indentLevel--;
#endif
                    // Default Placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Default Placement", EditorStyles.boldLabel);

#if UNITY_MONETIZATION
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(AdProperties.unityAdsDefaultBannerAdId.property, AdProperties.unityAdsDefaultBannerAdId.content, true);
                    EditorGUI.indentLevel--;
#endif
                    EditorGUILayout.HelpBox(UnityAdsDefaultPlacementsMsg, MessageType.None);
                    EditorGUI.indentLevel++;
                    // not allowing modifying default UnityAds default placement IDs
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(AdProperties.unityAdsDefaultInterstitialAdId.property, AdProperties.unityAdsDefaultInterstitialAdId.content, true);
                    EditorGUILayout.PropertyField(AdProperties.unityAdsDefaultRewardedAdId.property, AdProperties.unityAdsDefaultRewardedAdId.content, true);
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;

                    // Custom placements.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Custom Placements", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
#if UNITY_MONETIZATION
                    EditorGUILayout.PropertyField(AdProperties.unityAdsCustomBannerAdId.property, AdProperties.unityAdsCustomBannerAdId.content, true);
#endif
                    EditorGUILayout.PropertyField(AdProperties.unityAdsCustomInterstitialAdIds.property, AdProperties.unityAdsCustomInterstitialAdIds.content, true);
                    EditorGUILayout.PropertyField(AdProperties.unityAdsCustomRewardedAdIds.property, AdProperties.unityAdsCustomRewardedAdIds.content, true);
                    EditorGUI.indentLevel--;
#if UNITY_MONETIZATION
                    // Test mode.
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Test Mode", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(AdProperties.unityAdsEnableTestMode.property, AdProperties.unityAdsEnableTestMode.content);
#endif
#endif
                });
        }

        void CheckUnityAdsAutoInit()
        {
#if UNITY_EDITOR
#if (UNITY_ADS && !UNITY_MONETIZATION)
            UnityEditor.Advertisements.AdvertisementSettings.initializeOnStartup = true;
#elif (UNITY_MONETIZATION)
            UnityEditor.Advertisements.AdvertisementSettings.initializeOnStartup = false;
#endif
#endif
        }

        void AddWithoutRepeat<T>(List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }
        }

        bool IsPluginAvail(AdNetwork adNetwork)
        {
            switch (adNetwork)
            {
                case AdNetwork.AdColony:
#if EM_ADCOLONY
                    return true;
#else
                    return false;
#endif
                case AdNetwork.AdMob:
#if EM_ADMOB
                    return true;
#else
                    return false;
#endif
                case AdNetwork.AppLovin:
#if EM_APPLOVIN
                    return true;
#else
                    return false;
#endif
                case AdNetwork.Chartboost:
#if EM_CHARTBOOST
                    return true;
#else
                    return false;
#endif
                case AdNetwork.AudienceNetwork:
#if EM_FBAN
                    return true;
#else
                    return false;
#endif
                case AdNetwork.Heyzap:
#if EM_HEYZAP
                    return true;
#else
                    return false;
#endif
                case AdNetwork.MoPub:
#if EM_MOPUB
                    return true;
#else
                    return false;
#endif
                case AdNetwork.IronSource:
#if EM_IRONSOURCE
                    return true;
#else
                    return false;
#endif
                case AdNetwork.TapJoy:
#if EM_TAPJOY
                    return true;
#else
                    return false;
#endif
                case AdNetwork.UnityAds:
#if UNITY_ADS
                    return true;
#else
                    return false;
#endif
                case AdNetwork.None:
                    return true;
                default:
                    return false;
            }
        }

        bool IsUnityMonetizationAvail()
        {
#if UNITY_MONETIZATION
            return true;
#else
            return false;
#endif
        }

        #region Generate Custom Ads Constants

        private void GenerateAdIdsConstants()
        {
            Dictionary<string, string> finalResult = new Dictionary<string, string>();

            if (IsPluginAvail(AdNetwork.AdColony))
            {
                var adColonySettings = AdProperties.adColonySettings.GetTargetObject() as AdColonySettings;
                AddCustomAdsResource(finalResult, adColonySettings.CustomInterstitialAdIds, "AdColonyInterstitialAd");
                AddCustomAdsResource(finalResult, adColonySettings.CustomRewardedAdIds, "AdColonyRewardedAd");
            }

            if (IsPluginAvail(AdNetwork.AdMob))
            {
                var admobSettings = AdProperties.admobSettings.GetTargetObject() as AdMobSettings;
                AddCustomAdsResource(finalResult, admobSettings.CustomBannerAdIds, "AdmobBanner");
                AddCustomAdsResource(finalResult, admobSettings.CustomInterstitialAdIds, "AdmobInterstitialAd");
                AddCustomAdsResource(finalResult, admobSettings.CustomRewardedAdIds, "AdmobRewardedAd");
            }

            if (IsPluginAvail(AdNetwork.AudienceNetwork))
            {
                var fbAudienceSettings = AdProperties.fbAudienceSettings.GetTargetObject() as AudienceNetworkSettings;
                AddCustomAdsResource(finalResult, fbAudienceSettings.CustomBannerIds, "FBAudienceBanner");
                AddCustomAdsResource(finalResult, fbAudienceSettings.CustomInterstitialAdIds, "FBAudienceInterstitialAd");
                AddCustomAdsResource(finalResult, fbAudienceSettings.CustomRewardedAdIds, "FBAudienceRewardedAd");
            }

            if (IsPluginAvail(AdNetwork.MoPub))
            {
                var mopubSettings = AdProperties.mopubSettings.GetTargetObject() as MoPubSettings;
                AddCustomAdsResource(finalResult, mopubSettings.CustomBannerIds, "MopubBanner");
                AddCustomAdsResource(finalResult, mopubSettings.CustomInterstitialAdIds, "MopubInterstitialAd");
                AddCustomAdsResource(finalResult, mopubSettings.CustomRewardedAdIds, "MopubRewardedAd");
            }

            if (IsPluginAvail(AdNetwork.TapJoy))
            {
                var tapjoySettings = AdProperties.tapjoySettings.GetTargetObject() as TapjoySettings;
                AddCustomAdsResource(finalResult, tapjoySettings.CustomInterstitialAdIds, "TapjoyInterstitialAd");
                AddCustomAdsResource(finalResult, tapjoySettings.CustomRewardedAdIds, "TapjoyRewardedAd");
            }

            if (finalResult.Count > 0)
            {
                var hashtable = new Hashtable(finalResult);

                EM_EditorUtil.GenerateConstantsClass(
                    EM_Constants.GeneratedFolder,
                    EM_Constants.RootNameSpace + "." + EM_Constants.AdvertisingConstantsClassName,
                    new Hashtable(hashtable),
                    true
                );
            }
            else
            {
                Debug.Log("There is no custom ad to generate.");
            }
        }

        private Dictionary<string, string> AddCustomAdsResource(Dictionary<string, string> baseDict, Dictionary<AdPlacement, AdId> source,
                                                                string sourceName)
        {
            if (baseDict == null || source == null)
                return baseDict;

            foreach (var pair in source)
            {
                baseDict.Add(sourceName + "_Android_" + pair.Key.Name, pair.Value.AndroidId);
                baseDict.Add(sourceName + "_iOS_" + pair.Key.Name, pair.Value.IosId);
            }

            return baseDict;
        }

        #endregion Generate Custom Ads Constants

        #region Other stuff

        void DrawEnumArrayProperty(SerializedProperty property, Type enumType, string displayName)
        {
            Predicate<SerializedProperty> extraCondition = prop => prop.arraySize < Enum.GetValues(enumType).Length;
            DrawPropertyAsResizableArray(property, displayName, extraCondition);
            DrawAllElementsInArrayProperty(property);
        }

        void DrawMediationSettingProperty(SerializedProperty property, string displayName)
        {
            DrawPropertyAsResizableArray(property, displayName, _ => true);
            EditorGUI.indentLevel++;
            for (int i = 0; i < property.arraySize; i++)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                var setting = property.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(setting.FindPropertyRelative("mAdVendor"), new GUIContent("Ad Vendor"));
                EditorGUILayout.PropertyField(setting.FindPropertyRelative("mExtraOptions"), new GUIContent("Extra Options"));
                EditorGUILayout.EndVertical();
            }
            EditorGUI.indentLevel--;
        }

        bool DrawPropertyAsResizableArray(SerializedProperty property, string displayName,
                                          Predicate<SerializedProperty> addCondition = null, Predicate<SerializedProperty> removeCondition = null, bool drawAsFoldout = false)
        {
            return DrawPropertyAsResizableArray(property, new GUIContent(displayName), addCondition, removeCondition, drawAsFoldout);
        }

        bool DrawPropertyAsResizableArray(SerializedProperty property, GUIContent displayName,
                                          Predicate<SerializedProperty> addCondition = null, Predicate<SerializedProperty> removeCondition = null, bool drawAsFoldout = false)
        {
            EditorGUILayout.BeginHorizontal();

            bool isFoldOut = true;

            if (!drawAsFoldout)
            {
                EditorGUILayout.LabelField(displayName);
                isFoldOut = true;
            }
            else
            {
                property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, displayName);
                isFoldOut = property.isExpanded;
            }

            GUILayout.FlexibleSpace();  // this helps prevent the foldout from overlapping the +- buttons and making them unclickable.
            EditorGUILayout.EndHorizontal();

            // Only draw add & remove buttons if is folding out.
            // We're using GUI instead of GUILayout to control the button rects manually as we wish,
            // which helps fix issue in which foldout label flickers when the two buttons' visibility is being toggled.
            if (isFoldOut)
            {
                Rect linePosition = GUILayoutUtility.GetLastRect();

                bool addFlag = addCondition == null ? true : addCondition(property);
                bool removeFlag = (removeCondition == null ? true : removeCondition(property)) && property.arraySize > 0;

                GUIContent plusContent = EditorGUIUtility.IconContent("Toolbar Plus");
                GUIContent minusContent = EditorGUIUtility.IconContent("Toolbar Minus");
                GUIStyle buttonStyle = new GUIStyle(GUIStyle.none);

                var plusButtonWidth = buttonStyle.CalcSize(plusContent).x;
                var minusButtonWidth = buttonStyle.CalcSize(minusContent).x;

                var minusPosition = new Rect(linePosition);
                minusPosition.xMax = linePosition.xMax;
                minusPosition.xMin = linePosition.xMax - minusButtonWidth;
                minusPosition.width = minusButtonWidth;

                var plusPosition = new Rect(linePosition);
                plusPosition.xMax = linePosition.xMax - minusButtonWidth;
                plusPosition.xMin = linePosition.xMax - minusButtonWidth - plusButtonWidth;
                plusPosition.width = plusButtonWidth;

                /// Draw plus button.
                EditorGUI.BeginDisabledGroup(!addFlag);
                if (GUI.Button(plusPosition, plusContent, buttonStyle))
                    property.arraySize++;
                EditorGUI.EndDisabledGroup();

                /// Draw minus button.
                EditorGUI.BeginDisabledGroup(!removeFlag);
                if (GUI.Button(minusPosition, minusContent, buttonStyle))
                    property.arraySize--;
                EditorGUI.EndDisabledGroup();
            }

            return isFoldOut;
        }

        void DrawAllElementsInArrayProperty(SerializedProperty property, bool indent = true, bool noLabel = false)
        {
            if (indent)
                EditorGUI.indentLevel++;

            for (int i = 0; i < property.arraySize; i++)
            {
                if (!noLabel)
                    EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), true);
                else
                    EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(""), true);
            }

            if (indent)
                EditorGUI.indentLevel--;
        }

        void GenerateTapJoyAndroidManifest()
        {
            string targetFilePath = EM_Constants.AssetsPluginsAndroidFolder + "/AndroidManifest.xml";
            string templateFilePath = EM_Constants.TemplateFolder + "/Template_TapJoyAndroidManifest.xml";

            if (File.Exists(targetFilePath))
            {
                Debug.Log("An AndroidManifest file is already existed in Android plugins folder.");
                return;
            }

            if (!File.Exists(templateFilePath))
            {
                Debug.LogError("Couldn't find any template file at: " + templateFilePath);
                return;
            }

            string templateSource = File.ReadAllText(templateFilePath);
            FileStream newFile = File.Create(targetFilePath);
            using (TextWriter textWriter = new StreamWriter(newFile))
            {
                textWriter.Write(templateSource);
                Debug.Log("Created an AndroidManifest: " + targetFilePath);
            }

            AssetDatabase.Refresh();
        }

        #endregion
    }
}