// SPDX-License-Identifier: MIT
// Plan 114 × Plan 79 cross-system integration test
// Plan 114 — Year of Ash Questlines Expansion (8 → 15 questlines) [data already complete]
// Plan 79  — Advanced Clinical Autopsy Procedures Expansion (3 → 12 procedures) [data already complete]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class Plan114_79YearOfAshAutopsyIntegrationTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        // ── Year of Ash Questlines (Plan 114) ────────────────────────────────

        [Fact]
        public void YearOfAsh_HasFifteenQuestlinesWithUniqueIds()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io  = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = YearOfAshCatalogLoader.LoadQuestlines(dataDir, io, json);

            Assert.Equal(15, catalog.Count);

            var ids = new HashSet<string>();
            foreach (var q in catalog)
            {
                Assert.False(string.IsNullOrEmpty(q.questlineId), "Questline has empty questlineId");
                Assert.False(string.IsNullOrEmpty(q.title),       $"Questline '{q.questlineId}' has empty title");
                Assert.False(string.IsNullOrEmpty(q.firstStageId),$"Questline '{q.questlineId}' has empty firstStageId");
                Assert.True(ids.Add(q.questlineId),               $"Duplicate questlineId: {q.questlineId}");
            }
        }

        [Fact]
        public void YearOfAsh_AllStagesAndChoicesAreWellFormed()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io  = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = YearOfAshCatalogLoader.LoadQuestlines(dataDir, io, json);

            foreach (var q in catalog)
            {
                Assert.True(q.stages != null && q.stages.Count > 0,
                    $"Questline '{q.questlineId}' has no stages");

                // firstStageId must resolve to a known stageId
                var stageIds = q.stages.Select(s => s.stageId).ToHashSet();
                Assert.True(stageIds.Contains(q.firstStageId),
                    $"Questline '{q.questlineId}' firstStageId '{q.firstStageId}' not in stages");

                // All non-terminal nextStageIds must resolve within this questline
                foreach (var stage in q.stages)
                {
                    foreach (var choice in stage.choices ?? new List<QuestChoice>())
                    {
                        if (!string.IsNullOrEmpty(choice.nextStageId))
                        {
                            Assert.True(stageIds.Contains(choice.nextStageId),
                                $"Questline '{q.questlineId}' stage '{stage.stageId}' " +
                                $"choice '{choice.choiceId}' nextStageId '{choice.nextStageId}' " +
                                $"not found in questline stages");
                        }
                    }
                }
            }
        }

        // ── Autopsy Procedures (Plan 79) ──────────────────────────────────────

        [Fact]
        public void AutopsyProcedures_HasTwelveProceduresWithUniqueIds()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io  = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var procedures = AutopsyProcedureCatalogLoader.Load(dataDir, io, json);

            Assert.Equal(12, procedures.Count);

            var ids = new HashSet<string>();
            foreach (var p in procedures)
            {
                Assert.False(string.IsNullOrEmpty(p.procedure_id), "Procedure has empty procedure_id");
                Assert.True(ids.Add(p.procedure_id), $"Duplicate procedure_id: {p.procedure_id}");
                // Risk values in valid ranges (camelCase properties via JsonPropertyName)
                Assert.True(p.airborneRisk  >= 0f && p.airborneRisk  <= 1f,
                    $"Procedure '{p.procedure_id}' airborneRisk {p.airborneRisk} out of [0,1]");
                Assert.True(p.pathogenRisk  >= 0f && p.pathogenRisk  <= 1f,
                    $"Procedure '{p.procedure_id}' pathogenRisk {p.pathogenRisk} out of [0,1]");
                Assert.True(p.procedureHours >= 1,
                    $"Procedure '{p.procedure_id}' procedureHours {p.procedureHours} < 1");
            }
        }

        // ── Cross-system coherence ─────────────────────────────────────────────

        [Fact]
        public void CrossSystem_BothCatalogsLoadIndependentlyAndMeetMinimums()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io  = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var questlines  = YearOfAshCatalogLoader.LoadQuestlines(dataDir, io, json);
            var procedures  = AutopsyProcedureCatalogLoader.Load(dataDir, io, json);

            // Plan 114: Year of Ash questlines at target count
            Assert.True(questlines.Count >= 15,
                $"Year of Ash catalog must have >= 15 questlines; got {questlines.Count}");

            // Plan 79: Autopsy procedures at target count
            Assert.True(procedures.Count >= 12,
                $"Autopsy procedures catalog must have >= 12 entries; got {procedures.Count}");

            // Cross-system: at least 12 of 15 questlines must have non-empty factionTag
            // (internal shelter-politics questlines like quest_survivor_mutiny legitimately omit it)
            int questlinesWithFaction = questlines.Count(q => !string.IsNullOrEmpty(q.factionTag));
            Assert.True(questlinesWithFaction >= 12,
                $"Expected >= 12 questlines with factionTag; only {questlinesWithFaction} have one");
        }
    }
}
