// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class HostSelfTestExitContractTests
    {
        private static readonly string[] TwelveCheckProbes =
        {
            "CultureCreationSelfTest.cs",
            "PsychologicalProfileSelfTest.cs",
            "SkillCertificationSelfTest.cs",
            "ChildDevelopmentSelfTest.cs",
            "BestiarySelfTest.cs",
            "HealthHistorySelfTest.cs",
            "LeadershipSuccessionSelfTest.cs"
        };

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
        public void FixedCountProbes_FailWhenNotAllChecksWereExecuted()
        {
            string hostDirectory = Path.Combine(RepoRoot(), "src", "Host");

            foreach (string fileName in TwelveCheckProbes)
            {
                string source = File.ReadAllText(Path.Combine(hostDirectory, fileName));
                Assert.Contains("if (passed != 12)", source);
                Assert.Contains("Expected 12 checks", source);
            }
        }
    }
}
