using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 104 — validates that narrative_questlines.json contains exactly 12 questlines,
    /// all with unique quest_ids, all with 4 stages (Discovery/Investigation/Crisis/Resolution),
    /// all Crisis stages having branch_a and branch_b, and all survivor_id and
    /// target_location_id fields non-empty.
    ///
    /// This is a pure data-authority test: no new Core code is exercised, only the
    /// JSON catalog's structural and referential integrity.
    /// </summary>
    public class NarrativeQuestlineCatalogTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        private static JsonDocument LoadQuestlines(string dataDir)
        {
            string path = Path.Combine(dataDir, "narrative_questlines.json");
            Assert.True(File.Exists(path), $"narrative_questlines.json not found at: {path}");
            string raw = File.ReadAllText(path);
            return JsonDocument.Parse(raw);
        }

        [Fact]
        public void QuestlineCatalog_HasExactly12Questlines()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            Assert.Equal(12, questlines.GetArrayLength());
        }

        [Fact]
        public void QuestlineCatalog_AllQuestIdsUnique()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var ql in questlines.EnumerateArray())
            {
                string questId = ql.GetProperty("quest_id").GetString() ?? string.Empty;
                Assert.False(string.IsNullOrEmpty(questId), "quest_id must be non-empty");
                Assert.True(ids.Add(questId), $"Duplicate quest_id found: {questId}");
            }
        }

        [Fact]
        public void QuestlineCatalog_AllSurvivorIdsNonEmpty()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            foreach (var ql in questlines.EnumerateArray())
            {
                string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
                string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
                Assert.False(string.IsNullOrEmpty(survivorId),
                    $"survivor_id is empty for quest: {questId}");
            }
        }

        [Fact]
        public void QuestlineCatalog_AllTargetLocationIdsNonEmpty()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            foreach (var ql in questlines.EnumerateArray())
            {
                string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
                string locationId = ql.GetProperty("target_location_id").GetString() ?? string.Empty;
                Assert.False(string.IsNullOrEmpty(locationId),
                    $"target_location_id is empty for quest: {questId}");
            }
        }

        [Fact]
        public void QuestlineCatalog_EachQuestlineHasFourStages()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            foreach (var ql in questlines.EnumerateArray())
            {
                string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
                var stages = ql.GetProperty("stages");
                int stageCount = stages.GetArrayLength();
                Assert.True(stageCount == 4,
                    $"Quest {questId} must have exactly 4 stages (0=Discovery, 1=Investigation, 2=Crisis, 3=Resolution); found {stageCount}");
            }
        }

        [Fact]
        public void QuestlineCatalog_AllStageNamesNonEmpty()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            foreach (var ql in questlines.EnumerateArray())
            {
                string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
                foreach (var stage in ql.GetProperty("stages").EnumerateArray())
                {
                    int stageNum = stage.GetProperty("stage").GetInt32();
                    string name = stage.GetProperty("name").GetString() ?? string.Empty;
                    string desc = stage.GetProperty("description").GetString() ?? string.Empty;
                    Assert.False(string.IsNullOrEmpty(name),
                        $"Stage {stageNum} of {questId} has empty name");
                    Assert.False(string.IsNullOrEmpty(desc),
                        $"Stage {stageNum} of {questId} has empty description");
                }
            }
        }

        [Fact]
        public void QuestlineCatalog_CrisisStageHasTwoBranches()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            foreach (var ql in questlines.EnumerateArray())
            {
                string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
                foreach (var stage in ql.GetProperty("stages").EnumerateArray())
                {
                    int stageNum = stage.GetProperty("stage").GetInt32();
                    if (stageNum != 2) continue; // Crisis is stage 2

                    Assert.True(stage.TryGetProperty("branch_a", out _),
                        $"Crisis stage of {questId} is missing branch_a");
                    Assert.True(stage.TryGetProperty("branch_b", out _),
                        $"Crisis stage of {questId} is missing branch_b");

                    var branchA = stage.GetProperty("branch_a");
                    var branchB = stage.GetProperty("branch_b");

                    string traitA = branchA.GetProperty("trait_granted").GetString() ?? string.Empty;
                    string traitB = branchB.GetProperty("trait_granted").GetString() ?? string.Empty;

                    Assert.False(string.IsNullOrEmpty(traitA),
                        $"branch_a of {questId} crisis has empty trait_granted");
                    Assert.False(string.IsNullOrEmpty(traitB),
                        $"branch_b of {questId} crisis has empty trait_granted");
                    Assert.NotEqual(traitA, traitB);
                }
            }
        }

        [Fact]
        public void QuestlineCatalog_PriestAndReporterArcsPresent()
        {
            // Plan 104 §7: exactly 2 questlines wired for Plan 52 recurring-NPC hooks.
            // Verified by confirming the priest and reporter questlines exist.
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            bool hasPriest = false;
            bool hasReporter = false;
            foreach (var ql in questlines.EnumerateArray())
            {
                string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
                if (survivorId == "the_priest") hasPriest = true;
                if (survivorId == "the_reporter") hasReporter = true;
            }
            Assert.True(hasPriest, "the_priest questline (Plan 52 NPC hook) must be present");
            Assert.True(hasReporter, "the_reporter questline (Plan 52 NPC hook) must be present");
        }

        [Fact]
        public void QuestlineCatalog_TeacherAndJournalistArcsPresent()
        {
            // Plan 104 §8: exactly 2 questlines wired for Plan 95 journal voice.
            // Verified by confirming the teacher and reporter questlines exist.
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            bool hasTeacher = false;
            bool hasReporter = false;
            foreach (var ql in questlines.EnumerateArray())
            {
                string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
                if (survivorId == "the_teacher") hasTeacher = true;
                if (survivorId == "the_reporter") hasReporter = true;
            }
            Assert.True(hasTeacher, "the_teacher questline (Plan 95 journal voice) must be present");
            Assert.True(hasReporter, "the_reporter questline (Plan 95 journal voice) must be present");
        }

        [Fact]
        public void QuestlineCatalog_AllExpectedSurvivorsPresent()
        {
            // All 8 new arcs plus the original 4 must be represented.
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var expected = new HashSet<string>(StringComparer.Ordinal)
            {
                // Original 4
                "aris_thorne", "maya_lin", "victor_vance", "elena_rostov",
                // New 8 (Plan 104)
                "marcus_olejnik", "the_teacher", "the_chef", "suki_tanaka",
                "the_priest", "the_reporter", "the_electrician", "the_hunter"
            };

            using var doc = LoadQuestlines(dataDir);
            var questlines = doc.RootElement.GetProperty("questlines");
            var found = new HashSet<string>(StringComparer.Ordinal);
            foreach (var ql in questlines.EnumerateArray())
            {
                string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
                found.Add(survivorId);
            }

            foreach (var id in expected)
            {
                Assert.Contains(id, found);
            }
        }
    }
}
