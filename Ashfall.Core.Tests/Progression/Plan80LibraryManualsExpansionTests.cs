// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    /// <summary>
    /// Plan 80 — library manual catalog expansion quality gates.
    ///
    /// Repository-truth note (2026-09-06 recon): the library_manuals.json catalog
    /// was expanded by concurrent streams well past the original 3-manual
    /// baseline and now holds 24 manuals across all six canonical categories
    /// (survival / engineering / medical / science / scavenging / combat —
    /// four each). These tests pin that live catalog per the Plan 80 test
    /// catalogue: count, anchor-manual parity, reference resolution, DAG
    /// validity, reachability, tier depth, runtime prerequisite enforcement,
    /// reward atomicity, repeat-study blocking, and save round-trips.
    ///
    /// Category note: the plan draft expected "technical"/"scientific"/"social"
    /// category strings. The canonical categories are the six accepted by
    /// LibraryStudySystem.NormalizeDiscipline and used by every authored
    /// manual; "social" has no discipline mapping and no authored manual.
    /// The plan draft's claim that manual_improvised_weapons requires
    /// manual_water_filtration is stale — current data has it as a foundation
    /// with no prerequisites, and that is what is pinned here.
    /// </summary>
    public sealed class Plan80LibraryManualsExpansionTests
    {
        private static readonly HashSet<string> CanonicalCategories = new(StringComparer.Ordinal)
        {
            "survival", "engineering", "medical", "science", "scavenging", "combat"
        };

        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static List<ManualDefinition> LoadManuals()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
            var defs = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(defs);
            return defs;
        }

        private static HashSet<string> LoadKnowledgeIds()
        {
            string dataDir = FindDataDir();
            string raw = new FileSystemIO().ReadAllText(Path.Combine(dataDir, "research_knowledge.json"));
            using var doc = JsonDocument.Parse(raw);
            var set = new HashSet<string>(StringComparer.Ordinal);
            if (doc.RootElement.TryGetProperty("knowledge_nodes", out var arr) && arr.ValueKind == JsonValueKind.Array)
            {
                foreach (var it in arr.EnumerateArray())
                {
                    if (it.TryGetProperty("id", out var idProp))
                    {
                        string id = idProp.GetString() ?? "";
                        if (!string.IsNullOrEmpty(id)) set.Add(id);
                    }
                }
            }
            return set;
        }

        private static LibraryStudySystem CreateSystem(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal)
        {
            skills = new SkillProgressionSystem();
            research = new ResearchSystem();
            journal = new JournalSystem();
            var roster = new DutyRosterSystem();
            return new LibraryStudySystem(skills, research, journal, roster);
        }

        private static Dictionary<string, ManualDefinition> IndexById(List<ManualDefinition> manuals)
        {
            var map = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
            foreach (var m in manuals) map[m.manual_id] = m;
            return map;
        }

        // ---------------------------------------------------------------
        // Catalog shape
        // ---------------------------------------------------------------

        [Fact]
        public void Catalog_LoadsAtLeast24Manuals()
        {
            var manuals = LoadManuals();
            Assert.True(manuals.Count >= 24, $"Expected >= 24 manuals, found {manuals.Count}");
        }

        [Fact]
        public void Catalog_AnchorManualsPreserved()
        {
            var map = IndexById(LoadManuals());

            // Original baseline anchors keep their live identity (IDs, hours,
            // prerequisites, power flags). Values match library_manuals.json.
            Assert.True(map.TryGetValue("manual_water_filtration", out var water));
            Assert.Equal("Field Water Filtration", water.display_name);
            Assert.Equal("survival", water.category);
            Assert.Equal(10, water.studyHoursRequired);
            Assert.Equal(0.3f, water.fatiguePerHour);
            Assert.Equal(-0.5f, water.moraleEffect);
            Assert.Empty(water.prerequisites);
            Assert.True(water.requiresPower);
            Assert.Contains("knowledge_water_basics", water.researchUnlocks);
            Assert.Contains("knowledge_water_basics", water.knowledgeUnlocks);

            Assert.True(map.TryGetValue("manual_rad_first_aid", out var rad));
            Assert.Equal("Radiation First Aid & Dose Mitigation", rad.display_name);
            Assert.Equal("medical", rad.category);
            Assert.Equal(12, rad.studyHoursRequired);
            Assert.Empty(rad.prerequisites);
            Assert.False(rad.requiresPower);

            Assert.True(map.TryGetValue("manual_improvised_weapons", out var weapons));
            Assert.Equal("Improvised Weapons Fabrication", weapons.display_name);
            Assert.Equal("combat", weapons.category);
            Assert.Equal(14, weapons.studyHoursRequired);
            // Stale plan draft claimed a dependency on manual_water_filtration;
            // the authored catalog intentionally keeps this a foundation manual.
            Assert.Empty(weapons.prerequisites);
        }

        [Fact]
        public void Catalog_AllIdsUniqueAndCanonicalPrefix()
        {
            var manuals = LoadManuals();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var m in manuals)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.manual_id));
                Assert.StartsWith("manual_", m.manual_id);
                Assert.True(seen.Add(m.manual_id), $"Duplicate manual ID: {m.manual_id}");
            }
        }

        [Fact]
        public void Catalog_AllDisplayNamesNonEmptyAndUnique()
        {
            var manuals = LoadManuals();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var m in manuals)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.display_name));
                Assert.True(seen.Add(m.display_name), $"Duplicate display name: {m.display_name}");
            }
        }

        [Fact]
        public void Catalog_CategoriesAreCanonicalAndCoverAllSixDomains()
        {
            var manuals = LoadManuals();
            var categories = new HashSet<string>(StringComparer.Ordinal);
            foreach (var m in manuals)
            {
                Assert.Contains(m.category, CanonicalCategories);
                categories.Add(m.category);
            }
            Assert.Equal(CanonicalCategories.Count, categories.Count);
        }

        [Fact]
        public void Catalog_NumericBoundsValid()
        {
            foreach (var m in LoadManuals())
            {
                Assert.InRange(m.studyHoursRequired, 5, 25);
                Assert.InRange(m.fatiguePerHour, 0.10f, 0.60f);
                Assert.InRange(m.moraleEffect, -1.0f, 1.0f);
                Assert.InRange(m.technicalComplexityTier, 1, 4);
            }
        }

        // ---------------------------------------------------------------
        // Reference resolution
        // ---------------------------------------------------------------

        [Fact]
        public void Catalog_AllSkillXpGrantsAreValidDisciplineXpPairs()
        {
            var validDisciplines = new HashSet<string>(SkillProgressionSystem.Disciplines, StringComparer.Ordinal);
            foreach (var m in LoadManuals())
            {
                Assert.NotNull(m.skillXpGrants);
                Assert.True(m.skillXpGrants.Count % 2 == 0,
                    $"Manual {m.manual_id} skillXpGrants has odd length {m.skillXpGrants.Count}");
                for (int i = 0; i < m.skillXpGrants.Count; i += 2)
                {
                    Assert.True(validDisciplines.Contains(m.skillXpGrants[i]),
                        $"Manual {m.manual_id} references invalid discipline '{m.skillXpGrants[i]}'");
                    Assert.True(float.TryParse(m.skillXpGrants[i + 1], out float xp) && xp > 0f,
                        $"Manual {m.manual_id} has invalid XP amount '{m.skillXpGrants[i + 1]}'");
                }
            }
        }

        [Fact]
        public void Catalog_AllResearchAndKnowledgeUnlocksResolve()
        {
            var validKnowledge = LoadKnowledgeIds();
            Assert.NotEmpty(validKnowledge);
            foreach (var m in LoadManuals())
            {
                foreach (var r in m.researchUnlocks)
                    Assert.True(validKnowledge.Contains(r),
                        $"Manual {m.manual_id} research unlock '{r}' not found in research_knowledge.json");
                foreach (var k in m.knowledgeUnlocks)
                    Assert.True(validKnowledge.Contains(k),
                        $"Manual {m.manual_id} knowledge unlock '{k}' not found in research_knowledge.json");
            }
        }

        [Fact]
        public void Catalog_NoDuplicateReferencesWithinOneManual()
        {
            foreach (var m in LoadManuals())
            {
                AssertNoDuplicates(m.manual_id, "prerequisites", m.prerequisites);
                AssertNoDuplicates(m.manual_id, "research_unlocks", m.researchUnlocks);
                AssertNoDuplicates(m.manual_id, "knowledge_unlocks", m.knowledgeUnlocks);
                AssertNoDuplicates(m.manual_id, "loot_table_ids", m.lootTableIds);
                AssertNoDuplicates(m.manual_id, "expedition_reward_ids", m.expeditionRewardIds);
                AssertNoDuplicates(m.manual_id, "trader_pool_ids", m.traderPoolIds);
            }
        }

        private static void AssertNoDuplicates(string manualId, string field, List<string> values)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var v in values)
                Assert.True(seen.Add(v), $"Manual {manualId} has duplicate '{v}' in {field}");
        }

        // ---------------------------------------------------------------
        // Prerequisite graph (DAG / reachability / tiers)
        // ---------------------------------------------------------------

        [Fact]
        public void Graph_PrerequisiteReferencesResolve()
        {
            var map = IndexById(LoadManuals());
            foreach (var m in map.Values)
                foreach (var p in m.prerequisites)
                    Assert.True(map.ContainsKey(p),
                        $"Missing prerequisite '{p}' for manual '{m.manual_id}'");
        }

        [Fact]
        public void Graph_IsAcyclic()
        {
            var map = IndexById(LoadManuals());
            var state = new Dictionary<string, int>(StringComparer.Ordinal); // 0=unvisited 1=visiting 2=done
            foreach (var id in map.Keys) state[id] = 0;

            void Dfs(string current, List<string> path)
            {
                state[current] = 1;
                path.Add(current);
                foreach (var prereq in map[current].prerequisites)
                {
                    if (state[prereq] == 1)
                    {
                        path.Add(prereq);
                        Assert.Fail($"Cycle in manual prerequisites: {string.Join(" -> ", path)}");
                    }
                    if (state[prereq] == 0)
                        Dfs(prereq, path);
                }
                path.RemoveAt(path.Count - 1);
                state[current] = 2;
            }

            foreach (var id in map.Keys)
                if (state[id] == 0)
                    Dfs(id, new List<string>());
        }

        [Fact]
        public void Graph_AllManualsReachableFromFoundations()
        {
            var manuals = LoadManuals();
            var map = IndexById(manuals);
            var foundations = new HashSet<string>(StringComparer.Ordinal);
            foreach (var m in manuals)
                if (m.prerequisites.Count == 0)
                    foundations.Add(m.manual_id);

            Assert.True(foundations.Count >= 4, $"Expected >= 4 foundation manuals, found {foundations.Count}");

            var completed = new HashSet<string>(foundations, StringComparer.Ordinal);
            bool progress = true;
            while (progress)
            {
                progress = false;
                foreach (var m in manuals)
                {
                    if (completed.Contains(m.manual_id)) continue;
                    bool canComplete = true;
                    foreach (var p in m.prerequisites)
                        if (!completed.Contains(p)) { canComplete = false; break; }
                    if (canComplete)
                    {
                        completed.Add(m.manual_id);
                        progress = true;
                    }
                }
            }
            Assert.Equal(map.Count, completed.Count);
        }

        [Fact]
        public void Graph_HasIntermediateAndAdvancedDepth()
        {
            // Longest-prerequisite-chain depth: foundations = 0. The authored
            // graph carries meaningful depth beyond a single flat tier —
            // e.g. manual_water_filtration -> manual_bunker_hydroponics ->
            // manual_apiculture_and_pollination (depth 2), and
            // manual_rad_first_aid -> manual_quarantine_epidemiology ->
            // manual_pharmacology_synthesis (depth 2).
            var map = IndexById(LoadManuals());
            var memo = new Dictionary<string, int>(StringComparer.Ordinal);

            int Depth(string id)
            {
                if (memo.TryGetValue(id, out var d)) return d;
                memo[id] = 0; // guard (graph is acyclic; safe default)
                var prereqs = map[id].prerequisites;
                int result = prereqs.Count == 0 ? 0 : 1 + MaxDepth(prereqs, Depth);
                memo[id] = result;
                return result;
            }

            static int MaxDepth(List<string> ids, Func<string, int> f)
            {
                int max = 0;
                foreach (var id in ids) max = Math.Max(max, f(id));
                return max;
            }

            int maxDepth = 0;
            foreach (var id in map.Keys) maxDepth = Math.Max(maxDepth, Depth(id));

            Assert.True(maxDepth >= 2, $"Expected prerequisite depth >= 2, found {maxDepth}");
            Assert.True(Depth("manual_apiculture_and_pollination") == 2, "Apiculture should be a depth-2 advanced manual");
            Assert.True(Depth("manual_pharmacology_synthesis") == 2, "Pharmacology should be a depth-2 advanced manual");
        }

        // ---------------------------------------------------------------
        // Runtime study behavior
        // ---------------------------------------------------------------

        [Fact]
        public void Runtime_PrerequisiteEnforcement_WithRealChain()
        {
            var sys = CreateSystem(out _, out _, out _);
            LibraryManualCatalogLoader.LoadAndRegister(
                sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            // manual_bunker_hydroponics requires manual_water_filtration.
            var blocked = sys.StartStudy("manual_bunker_hydroponics", "survivor_bob");
            Assert.False(blocked.IsSuccess);
            Assert.Equal("missing_prerequisite", blocked.FailureCode);

            // Complete the foundation (10h at 8h/day -> completes day 2).
            Assert.True(sys.StartStudy("manual_water_filtration", "survivor_bob").IsSuccess);
            sys.TickDay(1);
            Assert.False(sys.IsManualCompleted("manual_water_filtration"));
            sys.TickDay(2);
            Assert.True(sys.IsManualCompleted("manual_water_filtration"));

            // The intermediate manual is now eligible.
            Assert.True(sys.StartStudy("manual_bunker_hydroponics", "survivor_bob").IsSuccess);
        }

        [Fact]
        public void Runtime_CompletionGrantsSkillXpResearchAndKnowledge()
        {
            var sys = CreateSystem(out var skills, out var research, out var journal);
            LibraryManualCatalogLoader.LoadAndRegister(
                sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            int codexBefore = journal.CodexUnlockCount;
            Assert.True(sys.StartStudy("manual_water_filtration", "dweller_alec").IsSuccess);
            sys.TickDay(1);
            Assert.False(sys.IsManualCompleted("manual_water_filtration"));
            sys.TickDay(2);
            Assert.True(sys.IsManualCompleted("manual_water_filtration"));

            // Skill XP granted to the reader in the manual's discipline (survival, 25).
            Assert.True(skills.GetXp("dweller_alec", "survival") >= 25f);
            // Research unlock applied.
            Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
            // Knowledge evidence recorded via the codex ledger.
            Assert.True(journal.CodexUnlockCount > codexBefore);
        }

        [Fact]
        public void Runtime_RewardsApplyExactlyOnce_RepeatStudyBlocked()
        {
            var sys = CreateSystem(out var skills, out var research, out _);
            LibraryManualCatalogLoader.LoadAndRegister(
                sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(sys.StartStudy("manual_water_filtration", "dweller_alec").IsSuccess);
            sys.TickDay(1);
            sys.TickDay(2);
            Assert.True(sys.IsManualCompleted("manual_water_filtration"));
            float xpAfterCompletion = skills.GetXp("dweller_alec", "survival");

            // Extra ticks must not re-grant anything.
            sys.TickDay(3);
            Assert.Equal(xpAfterCompletion, skills.GetXp("dweller_alec", "survival"));
            Assert.True(research.IsManualUnlocked("knowledge_water_basics"));

            // Restudying a completed manual is blocked (no infinite XP farm).
            var again = sys.StartStudy("manual_water_filtration", "dweller_alec");
            Assert.Equal(ActionResult.StatusKind.Blocked, again.Status);
            Assert.Equal("already_completed", again.FailureCode);
        }

        [Fact]
        public void Runtime_PartialStudyProgressRoundTripsThroughSave()
        {
            var sys1 = CreateSystem(out _, out _, out _);
            LibraryManualCatalogLoader.LoadAndRegister(
                sys1, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(sys1.StartStudy("manual_water_filtration", "reader_alice").IsSuccess);
            sys1.TickDay(1); // 8h of 10h

            var state = sys1.CaptureState();
            Assert.Single(state.activeJobs);
            Assert.Equal("manual_water_filtration", state.activeJobs[0].manualId);
            Assert.Equal(8f, state.activeJobs[0].progressHours);

            var sys2 = CreateSystem(out _, out _, out _);
            // Production loads the catalog at startup before applying a save,
            // so the restored system must have the catalog registered too.
            LibraryManualCatalogLoader.LoadAndRegister(
                sys2, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            sys2.RestoreState(state);
            Assert.Single(sys2.State.activeJobs);
            Assert.Equal("manual_water_filtration", sys2.State.activeJobs[0].manualId);
            Assert.Equal(8f, sys2.State.activeJobs[0].progressHours);
            Assert.False(sys2.IsManualCompleted("manual_water_filtration"));

            // Resumed study completes normally on the next tick.
            sys2.TickDay(2);
            Assert.True(sys2.IsManualCompleted("manual_water_filtration"));
        }

        [Fact]
        public void Runtime_ThreeTierBranchCompletesDeterministically()
        {
            var sys = CreateSystem(out var skills, out var research, out _);
            LibraryManualCatalogLoader.LoadAndRegister(
                sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            string[] branch = { "manual_water_filtration", "manual_bunker_hydroponics", "manual_apiculture_and_pollination" };
            var captured = new List<LibraryStudyState>();

            // Prerequisite gating is campaign-wide: before any study happens,
            // tier-2/3 manuals must be blocked for any reader.
            foreach (var manualId in new[] { branch[1], branch[2] })
            {
                var probe = sys.StartStudy(manualId, "probe_reader");
                Assert.Equal(ActionResult.StatusKind.Blocked, probe.Status);
                Assert.Equal("missing_prerequisite", probe.FailureCode);
            }

            for (int i = 0; i < branch.Length; i++)
            {
                Assert.True(sys.StartStudy(branch[i], "dweller_alec").IsSuccess);
                // Snapshot save-state between tiers.
                captured.Add(sys.CaptureState());

                // Advance until completion (max 3 days; none of these exceed 24h at rate 1.0).
                for (int day = 1; day <= 3 && !sys.IsManualCompleted(branch[i]); day++)
                    sys.TickDay(i * 10 + day);
                Assert.True(sys.IsManualCompleted(branch[i]), $"{branch[i]} should complete within 3 study days");
            }

            Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
            Assert.True(research.IsManualUnlocked("knowledge_hydroponics"));
            Assert.True(research.IsManualUnlocked("knowledge_apiculture_ecology"));
            Assert.True(skills.GetXp("dweller_alec", "survival") >= 25f + 30f + 35f);
        }
    }
}
