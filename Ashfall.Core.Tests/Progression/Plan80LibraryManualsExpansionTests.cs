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
    public sealed class Plan80LibraryManualsExpansionTests
    {
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
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = LibraryManualCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(defs);
            return defs;
        }

        private static HashSet<string> LoadKnowledgeIds()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            string raw = io.ReadAllText(Path.Combine(dataDir, "research_knowledge.json"));
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

        private static LibraryStudySystem CreateSystem(out SkillProgressionSystem skills, out ResearchSystem res, out JournalSystem journal)
        {
            skills = new SkillProgressionSystem();
            res = new ResearchSystem();
            journal = new JournalSystem();
            var roster = new DutyRosterSystem();
            return new LibraryStudySystem(skills, res, journal, roster);
        }

        [Fact]
        public void Catalog_LoadsExact15Manuals()
        {
            var manuals = LoadManuals();
            Assert.Equal(15, manuals.Count);
        }

        [Fact]
        public void Catalog_OriginalThreeManualsPreserved()
        {
            var manuals = LoadManuals();
            var map = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
            foreach (var m in manuals) map[m.manual_id] = m;

            Assert.Contains("manual_water_filtration", map.Keys);
            Assert.Contains("manual_rad_first_aid", map.Keys);
            Assert.Contains("manual_improvised_weapons", map.Keys);

            var water = map["manual_water_filtration"];
            Assert.Equal("Field Water Filtration", water.display_name);
            Assert.Equal("technical", water.category);
            Assert.Equal(10, water.studyHoursRequired);
            Assert.Empty(water.prerequisites);
            Assert.True(water.requiresPower);

            var rad = map["manual_rad_first_aid"];
            Assert.Equal("Radiation First Aid", rad.display_name);
            Assert.Equal("medical", rad.category);
            Assert.Equal(12, rad.studyHoursRequired);
            Assert.Empty(rad.prerequisites);
            Assert.False(rad.requiresPower);

            var combat = map["manual_improvised_weapons"];
            Assert.Equal("Improvised Weapons Fabrication", combat.display_name);
            Assert.Equal("military", combat.category);
            Assert.Equal(14, combat.studyHoursRequired);
            Assert.Contains("manual_water_filtration", combat.prerequisites);
        }

        [Fact]
        public void Catalog_AllFifteenIdsUniqueAndPrefixed()
        {
            var manuals = LoadManuals();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var m in manuals)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.manual_id));
                Assert.StartsWith("manual_", m.manual_id);
                Assert.True(seen.Add(m.manual_id), $"Duplicate manual ID: {m.manual_id}");
            }
            Assert.Equal(15, seen.Count);
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
            Assert.Equal(15, seen.Count);
        }

        [Fact]
        public void Catalog_CategoriesCoverAllSixDomains()
        {
            var manuals = LoadManuals();
            var categories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var m in manuals)
                categories.Add(m.category);

            Assert.Contains("technical", categories);
            Assert.Contains("medical", categories);
            Assert.Contains("military", categories);
            Assert.Contains("survival", categories);
            Assert.Contains("scientific", categories);
            Assert.Contains("social", categories);
        }

        [Fact]
        public void Catalog_PrerequisiteGraphIsAcyclic()
        {
            var manuals = LoadManuals();
            var map = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
            foreach (var m in manuals) map[m.manual_id] = m;

            // Cycle detection using DFS with 3 states: 0=unvisited, 1=visiting, 2=visited
            var state = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var id in map.Keys) state[id] = 0;

            void Dfs(string current, List<string> path)
            {
                state[current] = 1; // visiting
                path.Add(current);

                if (map.TryGetValue(current, out var def))
                {
                    foreach (var prereq in def.prerequisites)
                    {
                        Assert.True(map.ContainsKey(prereq), $"Missing prerequisite '{prereq}' for manual '{current}'");
                        if (state[prereq] == 1)
                        {
                            path.Add(prereq);
                            Assert.Fail($"Cycle detected in manual prerequisites: {string.Join(" -> ", path)}");
                        }
                        if (state[prereq] == 0)
                        {
                            Dfs(prereq, path);
                        }
                    }
                }

                path.RemoveAt(path.Count - 1);
                state[current] = 2; // visited
            }

            foreach (var id in map.Keys)
            {
                if (state[id] == 0)
                    Dfs(id, new List<string>());
            }
        }

        [Fact]
        public void Catalog_AllManualsReachableFromFoundations()
        {
            var manuals = LoadManuals();
            var map = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
            var foundations = new HashSet<string>(StringComparer.Ordinal);

            foreach (var m in manuals)
            {
                map[m.manual_id] = m;
                if (m.prerequisites == null || m.prerequisites.Count == 0)
                    foundations.Add(m.manual_id);
            }

            Assert.True(foundations.Count >= 4, $"Expected at least 4 foundation manuals, found {foundations.Count}");

            // Verify that starting from foundations, all 15 manuals can be completed
            var completed = new HashSet<string>(foundations, StringComparer.Ordinal);
            bool progress = true;

            while (progress)
            {
                progress = false;
                foreach (var m in manuals)
                {
                    if (completed.Contains(m.manual_id)) continue;
                    bool canComplete = true;
                    foreach (var prereq in m.prerequisites)
                    {
                        if (!completed.Contains(prereq))
                        {
                            canComplete = false;
                            break;
                        }
                    }
                    if (canComplete)
                    {
                        completed.Add(m.manual_id);
                        progress = true;
                    }
                }
            }

            Assert.Equal(15, completed.Count);
        }

        [Fact]
        public void Catalog_AllSkillXpGrantsValid()
        {
            var manuals = LoadManuals();
            var validDisciplines = new HashSet<string>(SkillProgressionSystem.Disciplines, StringComparer.Ordinal);

            foreach (var m in manuals)
            {
                Assert.NotNull(m.skillXpGrants);
                Assert.True(m.skillXpGrants.Count % 2 == 0,
                    $"Manual {m.manual_id} skillXpGrants has odd length {m.skillXpGrants.Count}");

                for (int i = 0; i < m.skillXpGrants.Count; i += 2)
                {
                    string discipline = m.skillXpGrants[i];
                    Assert.True(validDisciplines.Contains(discipline),
                        $"Manual {m.manual_id} references invalid discipline '{discipline}'");

                    bool isFloat = float.TryParse(m.skillXpGrants[i + 1], out float xp);
                    Assert.True(isFloat, $"Manual {m.manual_id} has invalid XP amount '{m.skillXpGrants[i + 1]}'");
                    Assert.True(xp > 0f, $"Manual {m.manual_id} XP amount must be > 0");
                }
            }
        }

        [Fact]
        public void Catalog_AllResearchAndKnowledgeUnlocksResolve()
        {
            var manuals = LoadManuals();
            var validKnowledge = LoadKnowledgeIds();

            foreach (var m in manuals)
            {
                Assert.NotNull(m.researchUnlocks);
                Assert.NotNull(m.knowledgeUnlocks);

                foreach (var r in m.researchUnlocks)
                {
                    Assert.True(validKnowledge.Contains(r),
                        $"Manual {m.manual_id} research unlock '{r}' not found in research_knowledge.json");
                }

                foreach (var k in m.knowledgeUnlocks)
                {
                    Assert.True(validKnowledge.Contains(k),
                        $"Manual {m.manual_id} knowledge unlock '{k}' not found in research_knowledge.json");
                }
            }
        }

        [Fact]
        public void Catalog_NumericBoundsValid()
        {
            var manuals = LoadManuals();
            foreach (var m in manuals)
            {
                Assert.InRange(m.studyHoursRequired, 5, 25);
                Assert.InRange(m.fatiguePerHour, 0.10f, 0.60f);
                Assert.InRange(m.moraleEffect, -1.0f, 1.0f);
            }
        }

        [Fact]
        public void Runtime_StudyProgressionCompletesAndGrantsRewards()
        {
            var sys = CreateSystem(out var skills, out var res, out var journal);
            string dataDir = FindDataDir();
            LibraryManualCatalogLoader.LoadAndRegister(sys, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Study manual_cold_weather_survival (Foundation, 10 hours)
            var startRes = sys.StartStudy("manual_cold_weather_survival", "dweller_alec");
            Assert.True(startRes.IsSuccess);
            Assert.Single(sys.State.activeJobs);

            // Advance 8 hours on day 1 (8h / 10h -> incomplete)
            sys.TickDay(1);
            Assert.False(sys.IsManualCompleted("manual_cold_weather_survival"));

            // Advance another 8 hours on day 2 (16h / 10h -> completed)
            sys.TickDay(2);
            Assert.True(sys.IsManualCompleted("manual_cold_weather_survival"));
            Assert.True(res.IsManualUnlocked("knowledge_shelter_insulation"));
        }

        [Fact]
        public void Runtime_PrerequisiteEnforcement()
        {
            var sys = CreateSystem(out _, out _, out _);
            string dataDir = FindDataDir();
            LibraryManualCatalogLoader.LoadAndRegister(sys, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // manual_radiation_monitoring requires manual_rad_first_aid
            var blocked = sys.StartStudy("manual_radiation_monitoring", "survivor_bob");
            Assert.False(blocked.IsSuccess);
            Assert.Equal("missing_prerequisite", blocked.FailureCode);

            // Complete prerequisite
            sys.StartStudy("manual_rad_first_aid", "survivor_bob");
            sys.TickDay(1); // 8h
            sys.TickDay(2); // 16h >= 12h -> complete
            Assert.True(sys.IsManualCompleted("manual_rad_first_aid"));

            // Now manual_radiation_monitoring can start
            var startRes = sys.StartStudy("manual_radiation_monitoring", "survivor_bob");
            Assert.True(startRes.IsSuccess);
        }

        [Fact]
        public void Runtime_SaveRestoreRoundTrip()
        {
            var sys1 = CreateSystem(out _, out _, out _);
            string dataDir = FindDataDir();
            LibraryManualCatalogLoader.LoadAndRegister(sys1, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            sys1.StartStudy("manual_conflict_mediation", "reader_alice");
            sys1.TickDay(1);

            var state = sys1.CaptureState();
            Assert.Single(state.activeJobs);
            Assert.Equal("manual_conflict_mediation", state.activeJobs[0].manualId);
            Assert.Equal(8f, state.activeJobs[0].progressHours);

            var sys2 = CreateSystem(out _, out _, out _);
            sys2.RestoreState(state);
            Assert.Single(sys2.State.activeJobs);
            Assert.Equal("manual_conflict_mediation", sys2.State.activeJobs[0].manualId);
            Assert.Equal(8f, sys2.State.activeJobs[0].progressHours);
        }
    }
}
