// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class HostSessionContractsSourceGateTests
    {
        private static string RepoRoot()
        {
            string? directory = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 12 && directory != null; i++)
            {
                if (File.Exists(Path.Combine(directory, "Ashfall.csproj")))
                    return directory;
                directory = Directory.GetParent(directory)?.FullName;
            }

            throw new DirectoryNotFoundException("Could not locate the repository root from the test output directory.");
        }

        [Fact]
        public void WiringValidator_FailsClosedForNullAndMalformedReports()
        {
            string source = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Host", "HostSessionContracts.cs"));

            Assert.Contains("if (reporter == null)", source);
            Assert.Contains("GetWiringReport returned null.", source);
            Assert.Contains("private static void NormalizeReport(", source);
            Assert.Contains("report.RequiredCollaborators ??= Array.Empty<string>();", source);
            Assert.Contains("report.ActiveFallbacks ??= Array.Empty<NamedFallback>();", source);
            Assert.Contains("string errorContext = $\"{ex.GetType().Name}: {ex.Message}\";", source);
            Assert.Contains("GetWiringReport failed ({errorContext}).", source);
        }
    }
}
