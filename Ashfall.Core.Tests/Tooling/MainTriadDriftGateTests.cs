// SPDX-License-Identifier: MIT
// Audit #28/#29 — Setup/Save/Flush triad drift gate + FlushEndgame enrollment.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins the intentional shape of Main's Setup/Save/Flush triad so silent
    /// drift (new Setup without SaveAll enrollment, or FlushEndgame falling
    /// out of _Process) fails CI. Full Flush coverage is not required — many
    /// SaveXxx methods are SaveAll-only by design.
    /// </summary>
    public sealed class MainTriadDriftGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        /// <summary>
        /// Setup methods that intentionally have no Save twin (construct-only,
        /// alias into another Save*, or Bind/wire helpers).
        /// </summary>
        private static readonly HashSet<string> SetupWithoutSaveAllowlist =
            new HashSet<string>(StringComparer.Ordinal)
            {
                "DailyBriefingModal",
                "DeepCoast",
                "EncounterChoiceResolver",
                "EventsHost",
                "EvolvingWorldInfluence",
                "ExpandedShelterSystems",
                "Expansions",
                "ExpeditionCombatHandoff",
                "IceRoad",
                "NpcArcs",
                "Phantom",
                "Plans94To97Panel",
                "ShelterFireHazard", // Save twin is SaveShelterFire
                "UtilityAi",
                "WeatherSonde",
                "WildlifeTrappingIfBound",
            };

        [Fact]
        public void SetupWithoutSave_IsAllowlistedOrHasSaveTwin()
        {
            var (setup, save, _) = ScanTriad();
            var orphans = setup
                .Where(s => !save.Contains(s)
                            && !save.Any(sv => sv.StartsWith(s, StringComparison.Ordinal)
                                               || s.StartsWith(sv, StringComparison.Ordinal)))
                .Where(s => !SetupWithoutSaveAllowlist.Contains(s))
                .OrderBy(s => s, StringComparer.Ordinal)
                .ToList();

            Assert.True(orphans.Count == 0,
                "Setup without Save twin (add SaveXxx or allowlist with disposition):\n  "
                + string.Join("\n  ", orphans));
        }

        [Fact]
        public void FlushEndgameIfDirty_IsCalledFromProcessFlushList()
        {
            // Audit #29 — endgame dirty flush must remain in the live _Process set.
            string app = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Application.cs"));
            Assert.Contains("FlushEndgameIfDirty()", app);
            Assert.Matches(
                new Regex(@"FlushMoralChoiceIfDirty\(\);\s*FlushEndgameIfDirty\(\);", RegexOptions.Singleline),
                app);
        }

        [Fact]
        public void SaveAll_EnrollsShelterFire_MoralChoice_Collectibles()
        {
            string orch = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.SaveOrchestrator.cs"));
            Assert.Contains("SaveMoralChoice()", orch);
            Assert.Contains("SaveShelterFire()", orch);
            Assert.Contains("SaveCollectibles()", orch);
        }

        [Fact]
        public void ProcessFlushList_IncludesShelterFire_Collectibles_MoralChoice()
        {
            string app = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Application.cs"));
            Assert.Contains("FlushMoralChoiceIfDirty()", app);
            Assert.Contains("FlushShelterFireIfDirty()", app);
            Assert.Contains("FlushCollectiblesIfDirty()", app);
        }

        private static (HashSet<string> Setup, HashSet<string> Save, HashSet<string> Flush) ScanTriad()
        {
            var setup = new HashSet<string>(StringComparer.Ordinal);
            var save = new HashSet<string>(StringComparer.Ordinal);
            var flush = new HashSet<string>(StringComparer.Ordinal);
            var rx = new Regex(
                @"(?:private|public|internal|protected)\s+(?:static\s+)?(?:async\s+)?(?:void|bool|Task(?:<[^>]+>)?|[\w.<>,\s\[\]]+)\s+(Setup|Save|Flush)([A-Za-z0-9_]+)",
                RegexOptions.Compiled);

            foreach (string path in Directory.GetFiles(Path.Combine(RepoRoot(), "src"), "Main*.cs"))
            {
                string text = File.ReadAllText(path);
                foreach (Match m in rx.Matches(text))
                {
                    string kind = m.Groups[1].Value;
                    string name = m.Groups[2].Value;
                    if (kind == "Setup") setup.Add(name);
                    else if (kind == "Save") save.Add(name);
                    else flush.Add(name);
                }
            }

            Assert.True(setup.Count >= 80, $"triad scan rotted? Setup count={setup.Count}");
            Assert.True(save.Count >= 80, $"triad scan rotted? Save count={save.Count}");
            return (setup, save, flush);
        }
    }
}
