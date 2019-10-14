using UnityEngine;
using System.Collections;

#if UNITY_IOS
using System.Runtime.InteropServices;

namespace EasyMobile.Internal.NativeAPIs.iOS
{
    internal static class iOSNativeAlert
    {
        [DllImport("__Internal")]
        private static extern void _AlertWithThreeButtons(string title, string message, string button1, string button2, string button3);

        [DllImport("__Internal")]
        private static extern void _AlertWithTwoButtons(string title, string message, string button1, string button2);

        [DllImport("__Internal")]
        private static extern void _Alert(string title, string message, string button);

        internal static void ShowThreeButtonsAlert(string title, string message, string button1, string button2, string button3)
        {
            _AlertWithThreeButtons(title, message, button1, button2, button3);
        }

        internal static void ShowTwoButtonsAlert(string title, string message, string button1, string button2)
        {
            _AlertWithTwoButtons(title, message, button1, button2);
        }

        internal static void ShowOneButtonAlert(string title, string message, string button)
        {
            _Alert(title, message, button);
        }
    }
}
#endif
