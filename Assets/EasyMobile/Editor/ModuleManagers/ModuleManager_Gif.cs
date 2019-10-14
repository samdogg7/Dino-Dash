using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace EasyMobile.Editor
{
    internal class ModuleManager_Gif : ModuleManager
    {
        #region Singleton

        private static ModuleManager_Gif sInstance;

        private ModuleManager_Gif()
        {
        }

        public static ModuleManager_Gif Instance
        {
            get
            {
                if (sInstance == null)
                    sInstance = new ModuleManager_Gif();
                return sInstance;
            }
        }

        #endregion

        #region implemented abstract members of ModuleManager

        protected override void InternalEnableModule()
        {
            // Nothing.
        }

        protected override void InternalDisableModule()
        {
            // Nothing.
        }

        public override List<string> AndroidManifestTemplatePaths
        {
            get
            {
                return null;
            }
        }

        public override IAndroidPermissionRequired AndroidPermissionsHolder
        {
            get
            {
                return null;
            }
        }

        public override IIOSInfoItemRequired iOSInfoItemsHolder
        {
            get
            {
                return null;
            }
        }

        public override Module SelfModule
        {
            get
            {
                return Module.Gif;
            }
        }

        #endregion
    }
}
