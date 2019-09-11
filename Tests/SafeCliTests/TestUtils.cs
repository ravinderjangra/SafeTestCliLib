using SafeCli;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SafeCliTests
{
    public static class TestUtils
    {
        public static async Task<string> InitRustLogging()
        {
#if __IOS__
            var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            using (var reader = new StreamReader(Path.Combine(".", "log.toml")))
            {
#elif __ANDROID__
            var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            using (var reader = new StreamReader(Application.Context.Assets.Open("log.toml")))
            {
#else
            var configPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(configPath);
            var srcPath = Path.Combine(Directory.GetParent(typeof(TestUtils).Assembly.Location).FullName, "log.toml");
            using (var reader = new StreamReader(srcPath))
            {
#endif
                using (var writer = new StreamWriter(Path.Combine(configPath, "log.toml")))
                {
                    writer.Write(reader.ReadToEnd());
                    writer.Close();
                }

                reader.Close();
            }

            await Session.SetAdditionalSearchPathAsync(configPath);
            await Session.InitLoggingAsync();
            return configPath;
        }
    }
}
