// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Player-Surface Binding Purity (C1 Plan 16 — INV-16.3, INV-16.4;
// source-plan tasks 16B.8 and 16B.13).
//
// Gate 1 — no production fixture IDs in player-surface binding code:
//   the exact demo literals named by the source plan ("inc_default", "tag_1",
//   "sig_distress", "sv_cohort_demo", fallback "surv_01") may not appear in
//   src/UI/** (production panels) or src/Main.PlayerSurfaces.cs (the player
//   bind configuration). Explicit selftest/demo paths live OUTSIDE this scan
//   scope by design: src/Host/PanelBindLifecycleSelfTest.cs,
//   src/Host/DoseLedgerHostSession.cs *Demo methods, and the Phase-0 dev
//   surface src/Dose/DoseRegisterSurface.cs.
//
// Gate 2 — no throwaway authority construction at bind time:
//   src/Main.PlayerSurfaces.cs bind actions must route through the Ensure*/Setup*
//   composition seams. Constructing a Core type, or any *System/*Engine/
//   *HostSession, inside the bind configuration is a fresh-authority defect
//   (one authority per fact — INV-16.1/INV-16.3).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PlayerSurfaceBindingPurityGateTests
    {
        private static readonly string[] FixtureIdLiterals =
        {
            "inc_default",
            "tag_1",
            "sig_distress",
            "sv_cohort_demo",
            "surv_01"
        };

        private static readonly Regex LineComment =
            new(@"//.*$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new(@"/\*.*?\*/", RegexOptions.Singleline | RegexOptions.Compiled);

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                    && Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException(
                "Could not locate repository root from " + Directory.GetCurrentDirectory());
        }

        private static string StripComments(string code)
        {
            code = BlockComment.Replace(code, string.Empty);
            return LineComment.Replace(code, string.Empty);
        }

        /// <summary>
        /// Production player-surface binding code: every panel under src/UI plus
        /// the host bind configuration in src/Main.PlayerSurfaces.cs.
        /// </summary>
        private static List<(string RelativePath, string StrippedCode)> LoadPlayerSurfaceBindingSources()
        {
            string root = FindRepoRoot();
            var files = new List<string>();

            string playerSurfaces = Path.Combine(root, "src", "Main.PlayerSurfaces.cs");
            Assert.True(File.Exists(playerSurfaces),
                $"Missing player-surface bind configuration at {playerSurfaces}");
            files.Add(playerSurfaces);

            string uiDir = Path.Combine(root, "src", "UI");
            Assert.True(Directory.Exists(uiDir), $"Could not find src/UI at {uiDir}");
            files.AddRange(Directory.EnumerateFiles(uiDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") && !f.Replace('\\', '/').Contains("/bin/"))
                .OrderBy(f => f, StringComparer.Ordinal));

            string doseDir = Path.Combine(root, "src", "Dose");
            if (Directory.Exists(doseDir))
            {
                files.AddRange(Directory.EnumerateFiles(doseDir, "*.cs", SearchOption.AllDirectories)
                    .Where(f => !f.Replace('\\', '/').Contains("/obj/") && !f.Replace('\\', '/').Contains("/bin/"))
                    .OrderBy(f => f, StringComparer.Ordinal));
            }

            var list = new List<(string RelativePath, string StrippedCode)>(files.Count);
            foreach (var f in files)
            {
                string rel = Path.GetRelativePath(root, f).Replace('\\', '/');
                list.Add((rel, StripComments(File.ReadAllText(f))));
            }
            return list;
        }

        [Fact]
        public void PlayerSurfaceBindingCode_ContainsNoProductionFixtureIds()
        {
            var files = LoadPlayerSurfaceBindingSources();
            var violations = new List<string>();

            foreach (var (rel, stripped) in files)
            {
                var lines = stripped.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    foreach (var fixture in FixtureIdLiterals)
                    {
                        // Exact quoted literal only: "tag_1" must not match "tag_10".
                        if (line.Contains("\"" + fixture + "\"", StringComparison.Ordinal))
                        {
                            violations.Add($"{rel}:{i + 1} -> \"{fixture}\" in: {line.Trim()}");
                            break;
                        }
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Production player-surface binding code must not contain the fixture IDs " +
                $"[{string.Join(", ", FixtureIdLiterals)}] (INV-16.4). Bind from live campaign " +
                $"selection/state instead; demo literals belong only in explicit selftest/demo " +
                $"paths outside this scan scope.\n" + string.Join("\n", violations));
        }

        [Fact]
        public void PlayerSurfaceBindConfiguration_ConstructsNoAuthorityAtBindTime()
        {
            string root = FindRepoRoot();
            string playerSurfaces = Path.Combine(root, "src", "Main.PlayerSurfaces.cs");
            string stripped = StripComments(File.ReadAllText(playerSurfaces));

            // A Core-namespace construction inside the bind configuration is a
            // throwaway authority (INV-16.3), e.g. `?? new Ashfall.Core.Inventory.Inventory()`.
            var coreConstruction = new Regex(
                @"new\s+Ashfall\.Core\.[A-Za-z0-9_\.]+\s*\(",
                RegexOptions.Compiled);

            // A system/engine/host-session construction inside the bind configuration
            // bypasses the campaign composition seams (INV-16.1).
            var systemConstruction = new Regex(
                @"new\s+[A-Za-z0-9_]+(?:System|Engine|HostSession)\s*\(",
                RegexOptions.Compiled);

            var violations = new List<string>();
            var lines = stripped.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (coreConstruction.IsMatch(line))
                    violations.Add($"src/Main.PlayerSurfaces.cs:{i + 1} [core construction] -> {line.Trim()}");
                if (systemConstruction.IsMatch(line))
                    violations.Add($"src/Main.PlayerSurfaces.cs:{i + 1} [system/session construction] -> {line.Trim()}");
            }

            Assert.True(violations.Count == 0,
                "Player-surface bind configuration must not construct Core authorities or " +
                "host sessions at bind time (INV-16.1/INV-16.3). Route through the Ensure*/Setup* " +
                "composition seams so panels receive the campaign-owned instance.\n" +
                string.Join("\n", violations));
        }
    }
}
