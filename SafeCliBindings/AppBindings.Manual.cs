using SafeCliCore;
using System;
using System.Runtime.InteropServices;

namespace SafeCliBindings
{
    public partial class AppBindings
    {
        private delegate void FfiResultAppCb(IntPtr userData, IntPtr result, IntPtr app);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultAppCb))]
#endif
        private static void OnAppCreateCb(IntPtr userData, IntPtr result, IntPtr app)
        {
            var action = BindingUtils.FromHandlePtr<Action<FfiResult, IntPtr, GCHandle>>(userData, false);

            action(Marshal.PtrToStructure<FfiResult>(result), app, GCHandle.FromIntPtr(userData));
        }

        private static readonly FfiResultAppCb DelegateOnAppCreateCb = OnAppCreateCb;

        private delegate void FfiResultByteListULongCb(
            IntPtr userData,
            IntPtr result,
            IntPtr contentPtr,
            UIntPtr contentLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteListULongCb))]
#endif
        private static void OnFfiResultByteListULongCb(IntPtr userData, IntPtr result, IntPtr contentPtr, UIntPtr contentLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteList(contentPtr, (int)contentLen));
        }

        private static readonly FfiResultByteListULongCb DelegateOnFfiResultByteListULongCb = OnFfiResultByteListULongCb;

        private delegate void FfiResultStringCb(IntPtr userData, IntPtr result, string encodedXorUrl);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringCb))]
#endif
        private static void OnFfiResultStringCb(IntPtr userData, IntPtr result, string encodedXorUrl)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => encodedXorUrl);
        }

        private static readonly FfiResultStringCb DelegateOnFfiResultStringCb = OnFfiResultStringCb;

        private delegate void FfiResultCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultCb))]
#endif
        private static void OnFfiResultCb(IntPtr userData, IntPtr result)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result));
        }

        private static readonly FfiResultCb DelegateOnFfiResultCb = OnFfiResultCb;
    }
}
