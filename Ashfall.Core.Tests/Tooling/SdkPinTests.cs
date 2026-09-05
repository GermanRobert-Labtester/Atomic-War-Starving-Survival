// SPDX-License-Identifier: MIT
// Audit #45 — pin global.json SDK policy (document float risk).
using System;
using System.IO;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Documents the intentional SDK pin. <c>rollForward: latestMajor</c> lets
    /// local/CI float across major SDKs; changing it is a deliberate release
    /// decision, not a silent edit.
    /// </summary>
    public sealed class SdkPinTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "global.json")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        [Fact]
        public void GlobalJson_PinsSdk8100_WithDocumentedRollForward()
        {
            string path = Path.Combine(RepoRoot(), "global.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var sdk = doc.RootElement.GetProperty("sdk");
            Assert.Equal("8.0.100", sdk.GetProperty("version").GetString());
            Assert.Equal("latestMajor", sdk.GetProperty("rollForward").GetString());
            Assert.False(sdk.GetProperty("allowPrerelease").GetBoolean());
        }
    }
}
