using UnityEngine;
using System.Collections;

#if UNITY_IOS
using System.Runtime.InteropServices;

namespace EasyMobile.Internal.StoreReview.iOS
{
    internal static class iOSNativeUtility
    {
        [DllImport("__Internal")]
        [return:MarshalAs(UnmanagedType.Bool)]
        private static extern bool _IsBuiltinRequestReviewAvail();

        [DllImport("__Internal")]
        private static extern void _RequestReview();

        internal static bool CanUseBuiltinRequestReview()
        {
            return _IsBuiltinRequestReviewAvail();
        }

        internal static void RequestReview()
        {
            _RequestReview();
        }
    }
}
#endif
