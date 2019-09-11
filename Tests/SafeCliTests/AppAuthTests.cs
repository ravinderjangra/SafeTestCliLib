using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeCli;
using SafeCliCore;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SafeCliTests
{
    [TestClass]
    public class UnitTest1
    {
        static readonly string _appId = "net.maidsafe.cli";
        static readonly string _appName = "SafeTestApp";
        static readonly string _vendor = "maidsafe";
        static readonly string _authReq = "bAAAAAAGBJZURSAAAAAAA2AAAAAAAAAAAONQWMZJNORSXG5BNMFYHAAIAAAAAAAAAAAAAWAAAAAAAAAAAKNQWMZKUMVZXIQLQOAEAAAAAAAAAAADNMFUWI43BMZSQCAAAAAAAAAAAAAAA";
        static readonly string _authResponse = "bAEAAAAF2AYDSAAAAAAAAAAAAAAAQAAAAVIN763BUYSOJBVXIPQ4Z3CKJAUWII5ODZNRJV64E55LHRHII4OMDMRDZ4BQKLKCYVDIPNTTH7AKYQIAAAAAAAAAAABZMFC2S4JGR4IYTAEZD3FLTQMVHJQ2YGRFFU6A2GLU6M5YNTXY4GIAAAAAAAAAAAB7IA523DV4OPWP5L7SC4XJ2ABG45UWHAYWCXCVON5HNHXZ7MH2VMQAAAAAAAAAAACMMPE5S7B3HKHLDZRWNGHBRBA7SQ63SYQBYQ5EEYKOZIOX6D47CY7UAO5NR26HH3H6V7ZBOLU5AATOO2LDQMLBLRKXG6TWT347WD5KWEAAAAAAAAAAAAUGLVSK6JTGHOYHJBXSMKZLKYSOXVWWX3GMABJYFRVBFI3TIBYISEAAAAAAAAAAABFNONDAQX3TGNK6G7NJZ4K7SVXCQF4J6VG2HTMX7E65ZAEXEKNUJZL32ZHOPJSHA7FF5JFD7EQDPTAW65BXZYI3P3JWTMZKTT46DRU426YMTBCXOSRRKZEPYJXOYTELR4ISD56J2VR2INGTKMLJHNFAPKO4P64BCXBLORNIYKRIMSMM5H647AAAAAAAAAAAAAWLQWLWVEGJSRGBH5XVA2M2ZDWECSX2Y5HXVZ73WBHWM2RIV7UWTTA5AAAAAAAAAAGAAAAAAAAAAAD3WMYKIPF4E6RKKONAHVTG7P52A5MPSMDQD54YTAAAAAAAAAAAAAAI";
        static readonly string _exampleFileLink = "safe://hbyyyydubnbnycsoira3ifqjrijo9ydfhzi8ory6h15domidz5r7xt7kae";

        [TestMethod]
        public async Task AuthTestAsync()
        {
            var session = await Session.AppRegisteredAsync(_appId, _authResponse);
            var appPtr = session.SafeApPtr();
            Assert.AreNotEqual(IntPtr.Zero, appPtr);
            session.Dispose();
        }

        [TestMethod]
        public void AuhFailedTestAsyns()
        {
            var wrongAuthCredentials = $"addanything{_authResponse}";
            Assert.ThrowsExceptionAsync<FfiException>(
                () => Session.AppRegisteredAsync(_appId, wrongAuthCredentials));
        }

        [TestMethod]
        public async Task SafeFetchTestAsync()
        {
            try
            {
                var session = await CreateTestAppAsync();
                var data = await session.SafeFetch(_exampleFileLink);
                Debug.WriteLine(data.Count);
                var content = Encoding.UTF8.GetString(data.ToArray(), 0, data.Count);
                Debug.WriteLine(content);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task XorUrlEncodeTestAsync()
        {
            try
            {
                Random rnd = new Random();
                Byte[] b = new Byte[32];
                rnd.NextBytes(b);
                var data = await Session.EncodeXorName(
                    b,
                    16000,
                    3,
                    0,
                    null,
                    null,
                    0,
                    "base32");
                Debug.WriteLine(data.Length);
                //var content = Encoding.UTF8.GetString(data.ToArray(), 0, data.Count);
                Debug.WriteLine(data);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task RustLoggingTest()
        {
            try
            {
                await Session.SetAdditionalSearchPathAsync(@"D:\GitHub\SafeCli\Tests\SafeCliTests\bin\Debug\netcoreapp2.2");
                await Session.InitLoggingAsync("Client.log");
                Random rnd = new Random();
                Byte[] b = new Byte[32];
                rnd.NextBytes(b);
                await Assert.ThrowsExceptionAsync<FfiException>(() => Session.EncodeXorName(
                     b,
                     16000,
                     3,
                     0,
                     null,
                     null,
                     0,
                     "base32"));
                var session = await CreateTestAppAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Assert.Fail(ex.Message);
            }
        }

        public async Task<Session> CreateTestAppAsync()
        {
            return await Session.AppRegisteredAsync(_appId, _authResponse);
        }
    }
}
