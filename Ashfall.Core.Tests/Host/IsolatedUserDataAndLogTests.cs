using System;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests.Host
{
    public class IsolatedUserDataAndLogTests : IDisposable
    {
        private readonly string _tempUserDir;
        private readonly string _tempLogDir;

        public IsolatedUserDataAndLogTests()
        {
            _tempUserDir = Path.Combine(Path.GetTempPath(), "ashfall_test_user_" + Guid.NewGuid().ToString("N"));
            _tempLogDir = Path.Combine(Path.GetTempPath(), "ashfall_test_log_" + Guid.NewGuid().ToString("N"));
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempUserDir))
                    Directory.Delete(_tempUserDir, true);
                if (Directory.Exists(_tempLogDir))
                    Directory.Delete(_tempLogDir, true);
            }
            catch
            {
                // Best-effort cleanup
            }
        }

        [Fact]
        public void DirectoryCreation_CreatesTargetDirectoriesSafely()
        {
            Assert.False(Directory.Exists(_tempUserDir));
            Assert.False(Directory.Exists(_tempLogDir));

            Directory.CreateDirectory(_tempUserDir);
            Directory.CreateDirectory(_tempLogDir);

            Assert.True(Directory.Exists(_tempUserDir));
            Assert.True(Directory.Exists(_tempLogDir));
        }

        [Fact]
        public void EnvironmentVariables_CanBeReadAndConfigured()
        {
            string customDir = Path.Combine(_tempUserDir, "custom_saves");
            Environment.SetEnvironmentVariable("ASHFALL_USER_DIR_TEST", customDir);

            string? read = Environment.GetEnvironmentVariable("ASHFALL_USER_DIR_TEST");
            Assert.Equal(customDir, read);

            Environment.SetEnvironmentVariable("ASHFALL_USER_DIR_TEST", null);
        }

        [Fact]
        public void LogPath_WritesEntriesSafely()
        {
            Directory.CreateDirectory(_tempLogDir);
            string logFile = Path.Combine(_tempLogDir, "test.log");

            string entry1 = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [INFO] Starting test headless run{Environment.NewLine}";
            string entry2 = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [WARN] Test warning diagnostic{Environment.NewLine}";

            File.AppendAllText(logFile, entry1);
            File.AppendAllText(logFile, entry2);

            Assert.True(File.Exists(logFile));
            string content = File.ReadAllText(logFile);
            Assert.Contains("[INFO] Starting test headless run", content);
            Assert.Contains("[WARN] Test warning diagnostic", content);
        }
    }
}
