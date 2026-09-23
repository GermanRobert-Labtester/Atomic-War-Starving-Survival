// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Unwired-code audit tripwire: every catalog loader exposing
    /// <c>LoadAndRegister</c> must be invoked from production host code
    /// (<c>src/</c>) or appear on the dispositioned allowlist below.
    ///
    /// Production call sites may use either <c>LoadAndRegister</c> or
    /// <c>Load(...)</c> — both count as wired (audit #16). A separate fact
    /// pins formerly-allowlisted Load-only feeders that are now production-wired.
    /// </summary>
    public sealed class LoaderWiringGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "Ashfall.Core")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        /// <summary>
        /// Loaders deliberately without a production call site today. Every
        /// entry states the disposition; removing an entry requires actually
        /// wiring the loader (or deleting dead code).
        /// </summary>
        private static readonly Dictionary<string, string> Allowlist =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                // Load-only dormant catalogs — activation tickets in
                // docs/remediation/tickets/DORMANT_EXPANSION_ACTIVATION_TICKETS.md (audit #50).
                ["SkyLayerArmorCatalogLoader"] = "DX-01 Expansion 11 'Orbital Harrow' designed-dormant; wire on expansion activation.",
                ["HoldfastNpcCatalogLoader"] = "DX-03 Holdfast NPC definitions await holdfast quest-loop integration.",
            };

        [Fact]
        public void EveryLoadAndRegisterLoader_IsCalledFromProduction_OrAllowlisted()
        {
            string root = RepoRoot();
            var coreFiles = Directory.GetFiles(Path.Combine(root, "Assets", "Ashfall.Core"), "*.cs", SearchOption.AllDirectories);
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var allFiles = coreFiles.Concat(srcFiles).ToDictionary(f => f, File.ReadAllText, StringComparer.Ordinal);

            // 1. Find every LoadAndRegister declaration and its enclosing class.
            var loaders = new List<(string Class, string File)>();
            foreach (var (file, text) in allFiles)
            {
                foreach (Match m in Regex.Matches(text, @"public static int LoadAndRegister\("))
                {
                    var classMatches = Regex.Matches(text.Substring(0, m.Index), @"(?:class|struct)\s+(\w+)");
                    var cls = classMatches.Count > 0 ? classMatches[classMatches.Count - 1].Groups[1].Value : null;
                    if (cls != null)
                        loaders.Add((cls, file));
                }
            }

            Assert.True(loaders.Count >= 10, $"expected the repo's loader set, found only {loaders.Count} — scan rotted?");

            // 2. Require a non-defining production reference via LoadAndRegister OR Load(...).
            var failures = new List<string>();
            foreach (var (cls, file) in loaders.OrderBy(l => l.Class, StringComparer.Ordinal))
            {
                bool wired = allFiles.Any(kv =>
                    kv.Key != file
                    && !kv.Key.Contains("Tests", StringComparison.Ordinal)
                    && (Regex.IsMatch(kv.Value, @"\b" + Regex.Escape(cls) + @"\s*\.\s*LoadAndRegister\b")
                        || Regex.IsMatch(kv.Value, @"\b" + Regex.Escape(cls) + @"\s*\.\s*Load\s*\(")));
                if (wired) continue;
                if (Allowlist.TryGetValue(cls, out _))
                    continue;
                failures.Add($"{cls} ({Path.GetFileName(file)}) — no production LoadAndRegister/Load call site and no allowlist disposition");
            }

            Assert.True(failures.Count == 0,
                "Unwired catalog loaders detected (the 'loader landed, feeder never wired' defect class):\n  "
                + string.Join("\n  ", failures));
        }

        [Fact]
        public void FormerlyAllowlistedLoadFeeders_AreProductionWired()
        {
            // Audit #15/#16: DebtTemplate + Collectible were allowlisted despite
            // already having production .Load(...) call sites. Atmosphere +
            // Environmental now LoadAndRegister from WorldHostSession.
            string root = RepoRoot();
            var srcText = string.Join("\n",
                Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories)
                    .Select(File.ReadAllText));

            Assert.True(Regex.IsMatch(srcText, @"\bDebtTemplateCatalogLoader\s*\.\s*Load\s*\("),
                "DebtTemplateCatalogLoader.Load must remain called from production src/");
            Assert.True(Regex.IsMatch(srcText, @"\bCollectibleCatalogLoader\s*\.\s*Load\s*\("),
                "CollectibleCatalogLoader.Load must remain called from production src/");
            Assert.True(Regex.IsMatch(srcText, @"\bAtmosphereCatalogLoader\s*\.\s*LoadAndRegister\b"),
                "AtmosphereCatalogLoader.LoadAndRegister must be called from production src/");
            Assert.True(Regex.IsMatch(srcText, @"\bEnvironmentalTextCatalogLoader\s*\.\s*LoadAndRegister\b"),
                "EnvironmentalTextCatalogLoader.LoadAndRegister must be called from production src/");
            Assert.True(Regex.IsMatch(srcText, @"\bDynamicQuestlineCatalogLoader\s*\.\s*LoadAndRegister\b"),
                "DynamicQuestlineCatalogLoader.LoadAndRegister must be called from production src/ (audit #26)");
        }

        [Fact]
        public void RecentSystemCatalogs_AreBoundFromMainHostPartials()
        {
            // Defect class (2026-09 audit): a Core system exposes LoadCatalog,
            // an authored catalog ships in StreamingAssets, and the integration
            // test loads that JSON directly into the Core system — but the Main
            // host partial constructs the system without binding the catalog,
            // leaving the authored content dead at runtime. This tripwire pins
            // the repaired host bindings so removing one fails CI.
            var expected = new (string Catalog, string System, string Call)[]
            {
                ("hidden_agendas.json", "HiddenAgendaSystem", ".LoadCatalog("),
                ("relationship_decay_profiles.json", "RelationshipDecaySystem", ".LoadCatalog("),
                ("rumor_hubs.json", "RumorSystem", ".LoadCatalog("),
                ("atmosphere_profiles.json", "ShelterAtmosphereSystem", ".LoadCatalog("),
                ("noise_sources.json", "ShelterNoiseSystem", ".LoadCatalog("),
                ("reputation_dimensions.json", "ShelterReputationSystem", ".LoadCatalog("),
                ("shelter_security_zones.json", "ShelterSecuritySystem", ".LoadCatalog("),
                ("death_legacy_templates.json", "SurvivorDeathLegacySystem", ".LoadCatalog("),
                ("time_capsules.json", "TimeCapsuleSystem", ".LoadCatalog("),
                ("propaganda_templates.json", "PropagandaSystem", ".LoadTemplates("),
                ("autonomy_actions.json", "SurvivorAutonomySystem", ".LoadCatalog("),
                ("nuclear_winter_phases.json", "NuclearWinterProgressionSystem", ".LoadCatalog("),
                ("shelter_celebrations.json", "SeasonalCelebrationSystem", ".LoadCatalog("),
                ("disaster_templates.json", "DisasterResponseSystem", ".LoadCatalog("),
                ("communications_networks.json", "CommunicationsSystem", ".LoadCatalog("),
                ("colony_blueprints.json", "ColonySystem", ".LoadCatalog("),
                ("hobby_definitions.json", "HobbySystem", ".LoadCatalog("),
                ("education_curriculum.json", "SurvivorEducationSystem", ".LoadCatalog("),
                ("shelter_construction.json", "ShelterExpansionSystem", ".LoadCatalog("),
                ("confession_secrets.json", "ConfessionSecretCatalog", ".Load("),
            };

            string root = RepoRoot();
            var mainPartials = Directory
                .GetFiles(Path.Combine(root, "src"), "Main*.cs", SearchOption.TopDirectoryOnly)
                .Select(path => (Path: path, Text: File.ReadAllText(path)))
                .ToList();

            var failures = new List<string>();
            foreach (var (catalog, system, call) in expected)
            {
                bool bound = mainPartials.Any(p =>
                    p.Text.Contains(catalog, StringComparison.Ordinal)
                    && p.Text.Contains(system, StringComparison.Ordinal)
                    && p.Text.Contains(call, StringComparison.Ordinal));
                if (!bound)
                {
                    failures.Add($"{system} never binds {catalog} from a Main host partial");
                }
            }

            Assert.True(failures.Count == 0,
                "Host catalog binding regressed (authored content dead at runtime):\n  "
                + string.Join("\n  ", failures));
        }

        [Fact]
        public void AllowlistEntries_StillExist_AsLoaders()
        {
            // An allowlist entry whose loader was deleted must be pruned — the
            // list documents living dispositions, not history.
            string root = RepoRoot();
            var coreFiles = Directory.GetFiles(Path.Combine(root, "Assets", "Ashfall.Core"), "*.cs", SearchOption.AllDirectories);
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var allText = string.Join("\n", coreFiles.Concat(srcFiles).Select(File.ReadAllText));

            var stale = Allowlist.Keys
                .Where(cls => !Regex.IsMatch(allText, @"class\s+" + Regex.Escape(cls) + @"\b"))
                .OrderBy(k => k, StringComparer.Ordinal)
                .ToList();

            Assert.True(stale.Count == 0, "prune stale allowlist entries: " + string.Join(", ", stale));
        }
    }
}
