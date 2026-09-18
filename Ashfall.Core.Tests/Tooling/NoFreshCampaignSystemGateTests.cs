// SPDX-License-Identifier: MIT
// Plan 27A §27A.10-§27A.12 — Source-scan gate banning fresh campaign-owned system
// construction in host selftests and UI test journeys (INV-27.3).
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class NoFreshCampaignSystemGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src"))) return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        /// <summary>
        /// Campaign-owned system types that must be obtained from campaign ownership
        /// rather than instantiated freshly inside host selftests and journey tests.
        /// </summary>
        public static readonly string[] CampaignOwnedSystems =
        {
            "DutyRosterSystem",
            "StartingLevelSystem",
            "RadiationSystem",
            "NeedsSystem",
            "ExpeditionSystem",
        };

        /// <summary>
        /// Explicit allowlist of existing test-only files permitted to construct fresh systems.
        /// </summary>
        public static readonly HashSet<string> AllowedSites = new HashSet<string>(StringComparer.Ordinal)
        {
            // Panel lifecycle tests isolated node bindings with mock/minimal sessions
            "src/Host/PanelBindLifecycleSelfTest.cs",
            "src/Host/WeatherSaveSelfTest.cs",
        };

        public static List<(string file, int line, string type)> ScanFiles(string rootDir)
        {
            var hits = new List<(string, int, string)>();
            string hostDir = Path.Combine(rootDir, "src", "Host");
            string mainDir = Path.Combine(rootDir, "src");

            var candidateFiles = new List<string>();
            if (Directory.Exists(hostDir))
            {
                candidateFiles.AddRange(Directory.GetFiles(hostDir, "*SelfTest*.cs", SearchOption.AllDirectories));
            }
            if (Directory.Exists(mainDir))
            {
                candidateFiles.AddRange(Directory.GetFiles(mainDir, "Main.UiTests*.cs", SearchOption.TopDirectoryOnly));
            }

            foreach (string path in candidateFiles)
            {
                string rel = Path.GetRelativePath(rootDir, path).Replace('\\', '/');
                if (AllowedSites.Contains(rel))
                    continue;

                string[] lines = File.ReadAllLines(path);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // Skip comments
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith("//") || trimmed.StartsWith("/*") || trimmed.StartsWith("*"))
                        continue;

                    foreach (string sysType in CampaignOwnedSystems)
                    {
                        string pattern = $@"\bnew\s+{sysType}\s*\(";
                        if (Regex.IsMatch(line, pattern))
                        {
                            hits.Add((rel, i + 1, sysType));
                        }
                    }
                }
            }
            return hits;
        }

        [Fact]
        public void NoFreshCampaignSystemsInHostSelfTests()
        {
            var hits = ScanFiles(RepoRoot());
            var offenders = new List<string>();
            foreach (var (file, line, type) in hits)
            {
                offenders.Add($"{file}:{line} constructs fresh campaign-owned '{type}'");
            }

            Assert.True(offenders.Count == 0,
                "Fresh campaign-owned system instantiation found in host selftest(s) (INV-27.3). " +
                "Selftests must obtain instances from campaign composition or use explicit CampaignFixture:\n  " +
                string.Join("\n  ", offenders));
        }

        [Fact]
        public void GateSelfTest_DetectsForbiddenConstruction()
        {
            // Plan 27 §27A.12: Prove that the gate actually fails when an unapproved fresh system is added.
            string badCode = "var roster = new DutyRosterSystem(); // unapproved fresh system";
            bool detected = false;
            foreach (string sysType in CampaignOwnedSystems)
            {
                if (Regex.IsMatch(badCode, $@"\bnew\s+{sysType}\s*\("))
                {
                    detected = true;
                    break;
                }
            }
            Assert.True(detected, "Gate regex failed to detect forbidden new DutyRosterSystem()");
        }
    }
}
