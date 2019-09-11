using SafeCliBindings;
using SafeCliCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SafeCli
{
    public sealed class Session : IDisposable
    {
        private static readonly AppBindings bindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        private Session()
        {
            _appPtr = new SafeAppPtr();
        }

        public IntPtr SafeApPtr()
        {
            return _appPtr;
        }

        public static Task<Session> AppRegisteredAsync(string appId, string authCredentials)
        {
            return Task.Run(
                () =>
                {
                    var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
                    var session = new Session();
                    Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedHandle) =>
                    {
                        if (result.ErrorCode != 0)
                        {
                            disconnectedHandle.Free();

                            tcs.SetException(result.ToException());
                            return;
                        }

                        session.Init(ptr);
                        tcs.SetResult(session);
                    };

                    bindings.AppRegistered(appId, authCredentials, acctCreatedCb);
                    return tcs.Task;
                });
        }

        public Task<List<byte>> SafeFetch(string xorUrl)
        {
            return bindings.SafeFetch(_appPtr, xorUrl);
        }

        public static Task<string> EncodeXorName(
            byte[] name, 
            ulong typeTag, 
            ulong dataType, 
            ulong contentType, 
            string path, 
            string subNames, 
            ulong contentVersion, 
            string baseEncoding)
        {
            return bindings.EncodeXorUrlAsync(
                name,
                typeTag,
                dataType,
                contentType,
                path,
                subNames,
                contentVersion,
                baseEncoding);
        }

        public static async Task InitLoggingAsync([Optional] string outputLogFileName)
        {
            await bindings.AppInitLoggingAsync(outputLogFileName);
        }

        public static Task SetAdditionalSearchPathAsync(string path)
        {
            return bindings.AppSetAdditionalSearchPathAsync(path);
        }

        public static Task<string> GetExeFileStemAsync()
        {
            return bindings.AppExeFileStemAsync();
        }

        private void Init(IntPtr appPtr)
        {
            _appPtr = new SafeAppPtr(appPtr);
        }

        public void Dispose()
        {
            FreeApp();
            GC.SuppressFinalize(this);
        }

        ~Session()
        {
            FreeApp();
        }

        private void FreeApp()
        {
            if (_appPtr == IntPtr.Zero)
            {
                return;
            }

            // AppBindings.AppFree(_appPtr);
            _appPtr.Clear();
        }
    }
}
