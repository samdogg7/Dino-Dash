#if UNITY_IOS
using System;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using EasyMobile.Internal;

namespace EasyMobile.Internal.GameServices.iOS
{
    #region iOSSavedGame Native API
    internal static class iOSSavedGameNative
    {
        //----------------------------------------------------------
        // Saved Games API
        //----------------------------------------------------------

        internal delegate void OpenSavedGameCallback(IntPtr response,IntPtr callbackPtr);

        internal delegate void SaveGameDataCallback(IntPtr response,IntPtr callbackPtr);

        internal delegate void LoadSavedGameDataCallback(IntPtr response,IntPtr callbackPtr);

        internal delegate void FetchSavedGamesCallback(IntPtr response,IntPtr callbackPtr);

        internal delegate void ResolveConflictingSavedGamesCallback(IntPtr response,IntPtr callbackPtr);

        internal delegate void DeleteSavedGameCallback(IntPtr response,IntPtr callbackPtr);

        [DllImport("__Internal")]
        internal static extern void _OpenSavedGame(
            string name,
            OpenSavedGameCallback callback,
            IntPtr secondaryCallback
        );

        [DllImport("__Internal")]
        internal static extern void _SaveGameData(
            IntPtr gkSavedGamePtr,
            byte[] data, 
            int dataLength, 
            SaveGameDataCallback callback, 
            IntPtr secondaryCallback);

        [DllImport("__Internal")]
        internal static extern void _LoadSavedGameData(
            IntPtr gkSavedGamePtr, 
            LoadSavedGameDataCallback callback, 
            IntPtr secondaryCallback);

        [DllImport("__Internal")]
        internal static extern void _FetchSavedGames(
            FetchSavedGamesCallback callback, 
            IntPtr secondaryCallback);

        [DllImport("__Internal")]
        internal static extern void _ResolveConflictingSavedGames(
            IntPtr[] conflictingSavedGamePtrs, 
            int savedGamesCount, 
            byte[] data, 
            int dataLength, 
            ResolveConflictingSavedGamesCallback callback, 
            IntPtr secondaryCallback);

        [DllImport("__Internal")]
        internal static extern void _DeleteSavedGame(
            string name, 
            DeleteSavedGameCallback callback, 
            IntPtr secondaryCallback);
    }

    #endregion // iOSSavedGame Native API

    #region iOSGKSavedGame

    internal class iOSGKSavedGame : InteropObject
    {
        [DllImport("__Internal")]
        internal static extern void _GKSavedGame_Ref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern void _GKSavedGame_Unref(HandleRef self);

        [DllImport("__Internal")]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal static extern bool _GKSavedGame_IsOpen(HandleRef self);

        [DllImport("__Internal")]
        internal static extern int _GKSavedGame_Name(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        internal static extern int _GKSavedGame_DeviceName(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        internal static extern long _GKSavedGame_ModificationDate(HandleRef self);

        internal iOSGKSavedGame(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _GKSavedGame_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _GKSavedGame_Unref(selfPointer);
        }

        internal static iOSGKSavedGame FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new iOSGKSavedGame(pointer);
        }

        internal bool IsOpen
        { 
            get
            {
                return _GKSavedGame_IsOpen(SelfPtr());
            }
        }

        internal string Name
        { 
            get
            { 
                return PInvokeUtil.GetNativeString((strBuffer, strLen) => 
                    _GKSavedGame_Name(SelfPtr(), strBuffer, strLen));
            } 
        }

        internal string DeviceName
        { 
            get
            { 
                return PInvokeUtil.GetNativeString((strBuffer, strLen) => 
                    _GKSavedGame_DeviceName(SelfPtr(), strBuffer, strLen));
            } 
        }

        internal DateTime ModificationDate
        { 
            get
            {
                return Util.FromMillisSinceUnixEpoch(
                    _GKSavedGame_ModificationDate(SelfPtr()));
            } 
        }
    }

    #endregion // iOSGKSavedGame

    #region OpenSavedGameResponse

    internal class OpenSavedGameResponse : InteropObject
    {
        [DllImport("__Internal")]
        internal static extern void _OpenSavedGameResponse_Ref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern void _OpenSavedGameResponse_Unref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern int _OpenSavedGameResponse_ErrorDescription(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        internal static extern int _OpenSavedGameResponse_GetData_Length(HandleRef self);

        [DllImport("__Internal")]
        internal static extern IntPtr _OpenSavedGameResponse_GetData_GetElement(HandleRef self, int index);

        internal OpenSavedGameResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _OpenSavedGameResponse_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _OpenSavedGameResponse_Unref(selfPointer);
        }

        internal static OpenSavedGameResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new OpenSavedGameResponse(pointer);
        }

        internal string GetError()
        {
            return PInvokeUtil.GetNativeString((strBuffer, strLen) => 
                _OpenSavedGameResponse_ErrorDescription(SelfPtr(), strBuffer, strLen));
        }

        internal iOSGKSavedGame[] GetResultSavedGames()
        {
            return PInvokeUtil.ToEnumerable<iOSGKSavedGame>(
                _OpenSavedGameResponse_GetData_Length(SelfPtr()),
                index => 
                iOSGKSavedGame.FromPointer(_OpenSavedGameResponse_GetData_GetElement(SelfPtr(), index))
            ).Cast<iOSGKSavedGame>().ToArray();
        }
    }

    #endregion  // OpenSavedGameResponse

    #region SaveGameDataResponse

    internal class SaveGameDataResponse : InteropObject
    {
        [DllImport("__Internal")]
        internal static extern void _SaveGameDataResponse_Ref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern void _SaveGameDataResponse_Unref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern int _SaveGameDataResponse_ErrorDescription(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        internal static extern IntPtr _SaveGameDataResponse_GetData(HandleRef self);

        internal SaveGameDataResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _SaveGameDataResponse_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _SaveGameDataResponse_Unref(selfPointer);
        }

        internal static SaveGameDataResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new SaveGameDataResponse(pointer);
        }

        internal string GetError()
        {
            return PInvokeUtil.GetNativeString((strBuffer, strLen) =>
                _SaveGameDataResponse_ErrorDescription(SelfPtr(), strBuffer, strLen));
        }

        internal iOSGKSavedGame GetSavedGame()
        {
            return iOSGKSavedGame.FromPointer(_SaveGameDataResponse_GetData(SelfPtr()));
        }
    }

    #endregion  // SavedGameDataResponse

    #region LoadSavedGameDataResponse

    internal class LoadSavedGameDataResponse : InteropObject
    {
        [DllImport("__Internal")]
        internal static extern void _LoadSavedGameDataResponse_Ref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern void _LoadSavedGameDataResponse_Unref(HandleRef self);

        [DllImport("__Internal")]
        internal static extern int _LoadSavedGameDataResponse_ErrorDescription(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        internal static extern int _LoadSavedGameDataResponse_GetData(HandleRef self, [In, Out] byte[] buffer, int byteCount);

        internal LoadSavedGameDataResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _LoadSavedGameDataResponse_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _LoadSavedGameDataResponse_Unref(selfPointer);
        }

        internal static LoadSavedGameDataResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new LoadSavedGameDataResponse(pointer);
        }

        internal string GetError()
        {
            return PInvokeUtil.GetNativeString((strBuffer, strLen) =>
                _LoadSavedGameDataResponse_ErrorDescription(SelfPtr(), strBuffer, strLen));
        }

        internal byte[] GetData()
        {
            return PInvokeUtil.GetNativeArray<byte>((buffer, length) =>
                _LoadSavedGameDataResponse_GetData(SelfPtr(), buffer, length));
        }
    }

    #endregion  // LoadSavedGameDataResponse

    #region FetchSavedGamesResponse

    internal class FetchSavedGamesResponse : InteropObject
    {
        [DllImport("__Internal")]
        private static extern void _FetchSavedGamesResponse_Ref(HandleRef self);

        [DllImport("__Internal")]
        private static extern void _FetchSavedGamesResponse_Unref(HandleRef self);

        [DllImport("__Internal")]
        private static extern int _FetchSavedGamesResponse_ErrorDescription(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        private static extern int _FetchSavedGamesResponse_GetData_Length(HandleRef self);

        [DllImport("__Internal")]
        private static extern IntPtr _FetchSavedGamesResponse_GetData_GetElement(HandleRef self, int index);

        internal FetchSavedGamesResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _FetchSavedGamesResponse_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _FetchSavedGamesResponse_Unref(selfPointer);
        }

        internal static FetchSavedGamesResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new FetchSavedGamesResponse(pointer);
        }

        internal string GetError()
        {
            return PInvokeUtil.GetNativeString((strBuffer, strLen) => 
                _FetchSavedGamesResponse_ErrorDescription(SelfPtr(), strBuffer, strLen));
        }

        internal iOSGKSavedGame[] GetFetchedSavedGames()
        {
            return PInvokeUtil.ToEnumerable<iOSGKSavedGame>(
                _FetchSavedGamesResponse_GetData_Length(SelfPtr()),
                index => 
                iOSGKSavedGame.FromPointer(_FetchSavedGamesResponse_GetData_GetElement(SelfPtr(), index))
            ).Cast<iOSGKSavedGame>().ToArray();
        }
    }

    #endregion

    #region ResolveConflictResponse

    internal class ResolveConflictResponse : InteropObject
    {
        [DllImport("__Internal")]
        private static extern void _ResolveConflictResponse_Ref(HandleRef self);

        [DllImport("__Internal")]
        private static extern void _ResolveConflictResponse_Unref(HandleRef self);

        [DllImport("__Internal")]
        private static extern int _ResolveConflictResponse_ErrorDescription(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        [DllImport("__Internal")]
        private static extern int _ResolveConflictResponse_GetData_Length(HandleRef self);

        [DllImport("__Internal")]
        private static extern IntPtr _ResolveConflictResponse_GetData_GetElement(HandleRef self, int index);

        internal ResolveConflictResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _ResolveConflictResponse_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _ResolveConflictResponse_Unref(selfPointer);
        }

        internal static ResolveConflictResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new ResolveConflictResponse(pointer);
        }

        internal string GetError()
        {
            return PInvokeUtil.GetNativeString((strBuffer, strLen) => 
                _ResolveConflictResponse_ErrorDescription(SelfPtr(), strBuffer, strLen));
        }

        internal iOSGKSavedGame[] GetSavedGames()
        {
            return PInvokeUtil.ToEnumerable<iOSGKSavedGame>(
                _ResolveConflictResponse_GetData_Length(SelfPtr()),
                index => 
                iOSGKSavedGame.FromPointer(_ResolveConflictResponse_GetData_GetElement(SelfPtr(), index))
            ).Cast<iOSGKSavedGame>().ToArray();
        }
    }

    #endregion  // ResolveConflictResponse

    #region DeleteSavedGameResponse

    internal class DeleteSavedGameResponse : InteropObject
    {
        [DllImport("__Internal")]
        private static extern void _DeleteSavedGameResponse_Ref(HandleRef self);

        [DllImport("__Internal")]
        private static extern void _DeleteSavedGameResponse_Unref(HandleRef self);

        [DllImport("__Internal")]
        private static extern int _DeleteSavedGameResponse_ErrorDescription(HandleRef self, [In,Out] byte[] strBuffer, int strLen);

        internal DeleteSavedGameResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        protected override void AttachHandle(HandleRef selfPointer)
        {
            _DeleteSavedGameResponse_Ref(selfPointer);
        }

        protected override void ReleaseHandle(HandleRef selfPointer)
        {
            _DeleteSavedGameResponse_Unref(selfPointer);
        }

        internal static DeleteSavedGameResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new DeleteSavedGameResponse(pointer);
        }

        internal string GetError()
        {
            return PInvokeUtil.GetNativeString((strBuffer, strLen) => 
                _DeleteSavedGameResponse_ErrorDescription(SelfPtr(), strBuffer, strLen));
        }
    }

    #endregion  // DeleteSavedGameResponse
}
#endif
