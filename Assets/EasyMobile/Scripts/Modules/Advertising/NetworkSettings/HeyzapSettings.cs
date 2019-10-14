using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyMobile
{
    [Serializable]
    public class HeyzapSettings
    {
        /// <summary>
        /// Gets or sets Heyzap's publisher identifier.
        /// </summary>
        public string PublisherId
        {
            get { return mPublisherId; }
            set { mPublisherId = value; }
        }

        /// <summary>
        /// Gets or sets Heyzap's custom interstitial placements (used for auto ad loading).
        /// </summary>
        public AdPlacement[] CustomInterstitialPlacements
        {
            get { return mCustomInterstitialPlacements; }
            set { mCustomInterstitialPlacements = value; }
        }

        /// <summary>
        /// Gets or sets Heyzap's custom rewarded ad placements (used for auto ad loading).
        /// </summary>
        public AdPlacement[] CustomRewardedPlacements
        {
            get { return mCustomRewardedPlacements; }
            set { mCustomRewardedPlacements = value; }
        }

        /// <summary>
        /// Enables or disables test suite.
        /// </summary>
        public bool ShowTestSuite
        {
            get { return mShowTestSuite; }
            set { mShowTestSuite = value; }
        }

        [SerializeField]
        private string mPublisherId;
        [SerializeField]
        private bool mShowTestSuite;

        [SerializeField]
        private AdPlacement[] mCustomInterstitialPlacements;
        [SerializeField]
        private AdPlacement[] mCustomRewardedPlacements;
    }
}
