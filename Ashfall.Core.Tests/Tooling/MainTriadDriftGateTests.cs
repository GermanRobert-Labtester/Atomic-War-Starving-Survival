// SPDX-License-Identifier: MIT
// Audit #28/#29 — Setup/Save/Flush triad drift gate + FlushEndgame enrollment.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.Save;
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
                "ChildDevelopment", // Plan 183 child state persists through the survivor_social aggregate; no duplicate child-development save section.
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
                "Plans166To169", // Composite setup; save twins are SaveEspionage, SaveFluidLogistics, SaveProceduralNarrative
                "Plans50To53", // Composite setup; save twins are SaveVehicleGarage, SaveShelterEspionage, SaveSurvivorMentalHealth
                "Plans62To65", // Composite setup; save twins are SaveFoodPreservation, SavePrewarArchives (Plan 63 ShelterPrisonerSystem retired by ORPHAN-SEAL-W1; legacy section migrated into prisoner_management)
                "Plans78To81", // Composite setup; child systems own registered save sections.
                "Plans110To113", // Composite setup; child systems own registered save sections.
                "Plans130To133", // Composite setup; child systems own registered save sections.
                "Plans146To149", // Composite setup; child systems own registered save sections.
                "FlagshipInstitutions", // Composite setup; each institution owns a registered save section.
                "Plans130To133Panel",
                "OrphanSealWave1", // ORPHAN-SEAL-W1 composite; child SaveXxx methods own the twelve registered sections (declarative triad gate owns Save registration)
                "Plans94To97Panel",
                "PersonalQuestPanel", // Plan 200 — panel binder only; the quest system persists via SavePersonalQuests
                "PersonalBelongingsPanel", // Plan 210 — panel binder only; claims persist inside the survivor_social aggregate via SurvivorSocialCoordinator
                "RumorBoardPanel", // Plan 203 — read-only rumor board; the network persists via SaveRumorNetwork
                "TimeCapsulePanel", // Plan 212 — panel binder only; the capsule system persists via SaveTimeCapsules
                "ShelterAcoustics", // Audio presentation / acoustic direction; transient simulation facts
                "ShelterFireHazard", // Save twin is SaveShelterFire
                "UtilityAi",
                "WeatherSonde",
                "WildlifeTrappingIfBound",
                "Enrichment", // Read-only static catalog projection + journal knowledge persistence; no standalone save store
                "Codex", // Read-only projection (CodexProjectionBuilder); zero persistent state — unlocks derive from journal/field-guide/research/faction-standing, which persist themselves
                "Cascade", // D1 2026-09-17: derived cascade-rule projection over the day's served/shed power outcome; the coordinator is built from static cascade_rules.json and holds no persisted state (journal already records its transitions)
                "ContentCertification", // Plans 46/42 Wave 2026-09-23: Plan 49 cargo certification is a recomputed audit verdict over the live composition (catalogs loaded + owners constructed); it holds no persisted campaign state and its journal line is the published evidence
                "FitnessForDuty", // D1 2026-09-17: Plan 24A derived fitness verdicts over existing persisted survivor authorities; the model is intentionally not a save section or a second survivor ledger
                "Difficulty", // XP-01 difficulty selection and persistence is stored in the campaign envelope manifest, not a standalone save section
                "NeedsPerformance", // Plan 137: NeedsPerformanceBridge is a pure domain projection over the live survivor needs state; modifiers are calculated dynamically with zero persistent state, avoiding parallel needs stores per Rule 5
                "AudioAccessibility", // Plan 169: Audio accessibility coordinator binds to live AudioManager and UserSettingsStore; persistent preferences belong to user settings, not campaign save slots
                "TunnelNetwork", // Plan 167: TunnelNetworkSystem is owned and persisted through WastelandMapSystem.Tunnels inside the canonical world-map save section; no duplicate save store
                "DynamicQuestGeneration", // Plan 171: DynamicQuestGenerator is candidate generator; accepted quests persist via QuestRuntimeCoordinator; no duplicate save section
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

        [Fact]
        public void SaveSectionRegistry_MethodsExist_AndSaveAllReachesEverySave()
        {
            var methods = ScanMainMethods();
            var missing = new List<string>();

            foreach (var section in SaveSectionRegistry.All)
            {
                if (!methods.ContainsKey(section.SaveMethod))
                    missing.Add($"{section.SectionKey}: missing {section.SaveMethod}");

                if (section.RequiresSetup && string.IsNullOrWhiteSpace(section.SetupMethod))
                    missing.Add($"{section.SectionKey}: registry requires setup but has no SetupMethod");
                else if (!string.IsNullOrWhiteSpace(section.SetupMethod)
                         && !methods.ContainsKey(section.SetupMethod!))
                    missing.Add($"{section.SectionKey}: missing {section.SetupMethod}");
            }

            var reachable = ReachableMethods(methods, "SaveAll");
            foreach (var section in SaveSectionRegistry.All)
            {
                if (methods.ContainsKey(section.SaveMethod) && !reachable.Contains(section.SaveMethod))
                    missing.Add($"{section.SectionKey}: {section.SaveMethod} is not reachable from SaveAll");
            }

            Assert.True(missing.Count == 0,
                "SaveSectionRegistry triad/orchestration drift:\n  "
                + string.Join("\n  ", missing.OrderBy(x => x, StringComparer.Ordinal)));
        }

        [Fact]
        public void FlushMethods_HaveDirtyGuard_OrDocumentedTransientDisposition()
        {
            var methods = ScanMainMethods();
            var documentedTransient = new HashSet<string>(StringComparer.Ordinal)
            {
                "FlushDirtyStoresForDayAdvance",
                "FlushFactionBranch",
                "FlushPlans50To53",
                "FlushContextualTutorialQueue",
            };

            var findings = methods
                .Where(pair => pair.Key.StartsWith("Flush", StringComparison.Ordinal))
                .Where(pair => !documentedTransient.Contains(pair.Key))
                .Where(pair => pair.Value.All(body =>
                    body.IndexOf("dirty", StringComparison.OrdinalIgnoreCase) < 0
                    && body.IndexOf("Save", StringComparison.Ordinal) < 0))
                .Select(pair => pair.Key)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();

            Assert.True(findings.Count == 0,
                "Flush method has no dirty guard/save call or documented disposition:\n  "
                + string.Join("\n  ", findings));
        }

        [Fact]
        public void ArchitectureCitedFiles_Exist()
        {
            string root = RepoRoot();
            foreach (string relative in new[]
            {
                "docs/architecture/MAIN_DECOMPOSITION_MAP.md",
                "docs/architecture/UTILITY_AI_UNIFICATION.md",
                "docs/architecture/WORN_GEAR_CONSOLIDATION.md",
                "src/Main.Application.cs",
                "src/Main.Lifecycle.cs",
            })
            {
                Assert.True(File.Exists(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar))),
                    $"architecture citation is missing: {relative}");
            }
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

        private static Dictionary<string, List<string>> ScanMainMethods()
        {
            string sourceRoot = Path.Combine(RepoRoot(), "src");
            string source = string.Join("\n", Directory.GetFiles(sourceRoot, "Main*.cs")
                .OrderBy(path => path, StringComparer.Ordinal)
                .Select(File.ReadAllText));
            var methods = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var header = new Regex(
                @"(?m)^\s*(?:private|public|internal|protected)\s+(?:(?:static|async|sealed|override|virtual|new)\s+)*[^\r\n\{;]+?\s+(?<name>(?:Setup|Save|Flush|Persist)[A-Za-z0-9_]+)\s*\([^;{}]*\)\s*\{",
                RegexOptions.Compiled);

            foreach (Match match in header.Matches(source))
            {
                int openBrace = match.Index + match.Length - 1;
                string body = ExtractBalancedBody(source, openBrace);
                if (!methods.TryGetValue(match.Groups["name"].Value, out var overloads))
                {
                    overloads = new List<string>();
                    methods.Add(match.Groups["name"].Value, overloads);
                }
                overloads.Add(body);
            }

            var expression = new Regex(
                @"(?m)^\s*(?:private|public|internal|protected)\s+(?:(?:static|async|sealed|override|virtual|new)\s+)*[^\r\n\{;]+?\s+(?<name>(?:Setup|Save|Flush|Persist)[A-Za-z0-9_]+)\s*\([^;{}]*\)\s*=>\s*(?<body>[^;]+);",
                RegexOptions.Compiled);
            foreach (Match match in expression.Matches(source))
            {
                if (!methods.TryGetValue(match.Groups["name"].Value, out var overloads))
                {
                    overloads = new List<string>();
                    methods.Add(match.Groups["name"].Value, overloads);
                }
                overloads.Add(match.Groups["body"].Value);
            }
            return methods;
        }

        private static HashSet<string> ReachableMethods(Dictionary<string, List<string>> methods, string root)
        {
            var reachable = new HashSet<string>(StringComparer.Ordinal);
            var pending = new Queue<string>();
            pending.Enqueue(root);
            var call = new Regex(@"\b(?<name>(?:Save|Persist)[A-Za-z0-9_]+)\s*\(", RegexOptions.Compiled);

            while (pending.Count > 0)
            {
                string current = pending.Dequeue();
                if (!reachable.Add(current) || !methods.TryGetValue(current, out var bodies)) continue;
                foreach (string body in bodies)
                {
                    foreach (Match match in call.Matches(body))
                    {
                        string called = match.Groups["name"].Value;
                        if (methods.ContainsKey(called) && !reachable.Contains(called))
                            pending.Enqueue(called);
                    }
                }
            }
            return reachable;
        }

        private static string ExtractBalancedBody(string source, int openBrace)
        {
            int depth = 0;
            bool lineComment = false;
            bool blockComment = false;
            bool stringLiteral = false;
            bool charLiteral = false;
            bool escaped = false;

            for (int i = openBrace; i < source.Length; i++)
            {
                char c = source[i];
                char next = i + 1 < source.Length ? source[i + 1] : '\0';

                if (lineComment)
                {
                    if (c == '\n') lineComment = false;
                    continue;
                }
                if (blockComment)
                {
                    if (c == '*' && next == '/') { blockComment = false; i++; }
                    continue;
                }
                if (stringLiteral)
                {
                    if (escaped) { escaped = false; continue; }
                    if (c == '\\') { escaped = true; continue; }
                    if (c == '"') stringLiteral = false;
                    continue;
                }
                if (charLiteral)
                {
                    if (escaped) { escaped = false; continue; }
                    if (c == '\\') { escaped = true; continue; }
                    if (c == '\'') charLiteral = false;
                    continue;
                }
                if (c == '/' && next == '/') { lineComment = true; i++; continue; }
                if (c == '/' && next == '*') { blockComment = true; i++; continue; }
                if (c == '"') { stringLiteral = true; continue; }
                if (c == '\'') { charLiteral = true; continue; }
                if (c == '{') depth++;
                else if (c == '}' && --depth == 0)
                    return source.Substring(openBrace, i - openBrace + 1);
            }

            throw new InvalidOperationException("unbalanced Main partial method body");
        }
    }
}
