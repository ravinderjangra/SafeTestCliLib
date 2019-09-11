using SafeCliCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SafeCliBindings
{
    public partial class AppBindings
    {
#if __IOS__
        private const string DllName = "__Internal";
#else
        private const string DllName = "safe_cli";
#endif

        public void AppRegistered(
          string appId,
          string authCredentials,
          Action<FfiResult, IntPtr, GCHandle> oCb)
        {
            var userData = BindingUtils.ToHandlePtr(oCb);
            AppRegisteredNative(appId, authCredentials, userData, DelegateOnAppCreateCb);
        }

        [DllImport(DllName, EntryPoint = "safe_connect")]
        private static extern void AppRegisteredNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            [MarshalAs(UnmanagedType.LPStr)] string authCredentials,
            IntPtr userData,
            FfiResultAppCb oCb);

        public Task<List<byte>> SafeFetch(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            SafeFetchNative(app, url, userData, DelegateOnFfiResultByteListULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "safe_fetch")]
        private static extern void SafeFetchNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultByteListULongCb oCb);



        public Task<string> EncodeXorUrlAsync(byte[] name, ulong typeTag, ulong dataType, ulong contentType, string path, string subNames, ulong contentVersion, string baseEncoding)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            EncodeXorUrlNative(name, typeTag, dataType, contentType, path, subNames, contentVersion, baseEncoding, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_xor_url")]
        private static extern void EncodeXorUrlNative(
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] name,
            ulong typeTag,
            ulong dataType,
            ulong contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            [MarshalAs(UnmanagedType.LPStr)] string baseEncoding,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task AppInitLoggingAsync(string outputFileNameOverride)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AppInitLoggingNative(outputFileNameOverride, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_init_logging")]
        private static extern void AppInitLoggingNative(
            [MarshalAs(UnmanagedType.LPStr)] string outputFileNameOverride,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<string> AppOutputLogPathAsync(string outputFileName)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AppOutputLogPathNative(outputFileName, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_output_log_path")]
        private static extern void AppOutputLogPathNative(
            [MarshalAs(UnmanagedType.LPStr)] string outputFileName,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> AppExeFileStemAsync()
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AppExeFileStemNative(userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_exe_file_stem")]
        private static extern void AppExeFileStemNative(IntPtr userData, FfiResultStringCb oCb);

        public Task AppSetAdditionalSearchPathAsync(string newPath)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AppSetAdditionalSearchPathNative(newPath, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_set_additional_search_path")]
        private static extern void AppSetAdditionalSearchPathNative(
            [MarshalAs(UnmanagedType.LPStr)] string newPath,
            IntPtr userData,
            FfiResultCb oCb);
    }
}
