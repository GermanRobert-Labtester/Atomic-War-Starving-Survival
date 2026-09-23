// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Verdict
{
    /// <summary>
    /// Cross-system integration tests for Wave 40 Batch 5:
    /// - Plan 93 (DEC-263): Verdict NPCs Expansion (6 -> 18 investigation-site NPCs)
    /// - Plan 101 (DEC-264): Radiation Dose Quests Expansion (4 -> 12 dose-ledger questlines)
    ///
    /// Validates referential integrity, investigation site coverage, phase gating,
    /// dose quest stage transitions, and thematic alignment between archival
    /// dosimetry records and clinical shelter radiation dilemmas.
    /// </summary>
    public sealed class Plan93_101VerdictDoseQuestIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(candidate)) return candidate;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }

            string current = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(current, out string found))
                return found;

            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data directory.");
        }

        [Fact]
        public void Plan93_VerdictNpcCatalog_LoadsAll18Npcs_WithValidGatingAndDialogue()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new VerdictNpcSystem();

            int loadedCount = VerdictNpcCatalogLoader.LoadAndRegister(system, dataDir, io, json);

            Assert.Equal(18, loadedCount);
            Assert.Equal(18, system.Catalog.Count);

            var validKinds = new HashSet<string>(StringComparer.Ordinal)
            {
                "tape_echo",
                "paper_ghost",
                "living",
                "readings"
            };

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var npc in system.Catalog)
            {
                Assert.True(npc.id.StartsWith("npc_"), $"NPC id must start with npc_: {npc.id}");
                Assert.False(string.IsNullOrWhiteSpace(npc.name), $"Name must not be empty for {npc.id}");
                Assert.False(string.IsNullOrWhiteSpace(npc.role), $"Role must not be empty for {npc.id}");
                Assert.Contains(npc.kind, validKinds);
                Assert.True(npc.phaseMin >= 1 && npc.phaseMin <= 3, $"phaseMin must be 1..3 for {npc.id}");
                Assert.True(npc.gatingFlag.StartsWith("flag_verdict_"), $"gatingFlag must start with flag_verdict_: {npc.id}");
                Assert.False(string.IsNullOrWhiteSpace(npc.locationId), $"locationId must not be empty for {npc.id}");
                Assert.NotNull(npc.dialogue);
                Assert.NotEmpty(npc.dialogue);
                Assert.True(seenIds.Add(npc.id), $"Duplicate NPC id: {npc.id}");
            }

            // Verify original baseline NPCs
            var eden = system.Find("npc_eden_vale");
            Assert.NotNull(eden);
            Assert.Equal("Eden Vale", eden.name);
            Assert.Equal("tape_echo", eden.kind);

            var voss = system.Find("npc_ferris_voss");
            Assert.NotNull(voss);
            Assert.Equal("Ferris Voss", voss.name);
            Assert.Equal("paper_ghost", voss.kind);
        }

        [Fact]
        public void Plan101_DoseQuestCatalog_LoadsAll12Questlines_WithValidTransitions()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = DoseContentCatalogLoader.Load(dataDir, io, json);

            Assert.NotNull(catalog.quests);
            Assert.Equal(12, catalog.quests.Count);

            var questMap = catalog.quests.ToDictionary(q => q.questlineId, StringComparer.Ordinal);

            // Verify all canonical questline IDs match DoseQuestMigration
            Assert.Equal(12, DoseQuestMigration.CanonicalQuestlineIds.Length);
            foreach (var canonicalId in DoseQuestMigration.CanonicalQuestlineIds)
            {
                Assert.True(questMap.ContainsKey(canonicalId), $"Missing canonical dose questline: {canonicalId}");
                Assert.True(DoseQuestMigration.IsDoseQuestline(canonicalId));
            }

            // Verify stage transitions and terminal integrity
            foreach (var quest in catalog.quests)
            {
                Assert.False(string.IsNullOrWhiteSpace(quest.title), $"Title empty for {quest.questlineId}");
                Assert.False(string.IsNullOrWhiteSpace(quest.synopsis), $"Synopsis empty for {quest.questlineId}");
                Assert.NotNull(quest.stages);
                Assert.NotEmpty(quest.stages);

                var stageIds = quest.stages.Select(s => s.stageId).ToHashSet(StringComparer.Ordinal);
                bool hasTerminal = false;

                foreach (var stage in quest.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.stageId));
                    Assert.False(string.IsNullOrWhiteSpace(stage.narrativePrompt));

                    if (stage.isTerminal)
                    {
                        hasTerminal = true;
                    }
                    else
                    {
                        Assert.NotNull(stage.choices);
                        Assert.NotEmpty(stage.choices);
                        foreach (var choice in stage.choices)
                        {
                            Assert.False(string.IsNullOrWhiteSpace(choice.choiceId));
                            Assert.False(string.IsNullOrWhiteSpace(choice.text));
                            Assert.True(stageIds.Contains(choice.nextStageId),
                                $"nextStageId '{choice.nextStageId}' in {quest.questlineId} does not resolve to an existing stage.");
                        }
                    }
                }

                Assert.True(hasTerminal, $"Questline {quest.questlineId} must have at least one terminal stage.");
            }
        }

        [Fact]
        public void CrossSystem_VerdictArchivistsAndDosimetryQuests_ExhibitNarrativeCoherence()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var system = new VerdictNpcSystem();
            VerdictNpcCatalogLoader.LoadAndRegister(system, dataDir, io, json);
            var doseCatalog = DoseContentCatalogLoader.Load(dataDir, io, json);
            var questMap = doseCatalog.quests.ToDictionary(q => q.questlineId, StringComparer.Ordinal);

            // 1. Archival dosimetrist at archive tape silo aligns with calibration dispute quests
            var kasper = system.Find("npc_kasper_holt");
            Assert.NotNull(kasper);
            Assert.Equal("loc_archive_tape_silo", kasper.locationId);
            Assert.Contains("custodian", kasper.role.ToLowerInvariant());

            var calibrationQuest = questMap["quest_the_broken_calibration_chain"];
            Assert.NotNull(calibrationQuest);
            Assert.Contains("calibration", calibrationQuest.title.ToLowerInvariant());

            // 2. Radiobiology researcher at marine lab aligns with acute exposure hospice dilemmas
            var sena = system.Find("npc_sena_korr");
            Assert.NotNull(sena);
            Assert.Equal("loc_sealed_marine_laboratory", sena.locationId);
            Assert.Contains("researcher", sena.role.ToLowerInvariant());

            var sickRoomQuest = questMap["quest_the_sick_of_room_seven"];
            Assert.NotNull(sickRoomQuest);
            Assert.Contains("sick", sickRoomQuest.title.ToLowerInvariant());

            // 3. Census clerk tracking population count aligns with register audits
            var selya = system.Find("npc_selya_saltmarsh");
            Assert.NotNull(selya);
            Assert.Equal("loc_twelve_gauge_array", selya.locationId);
            Assert.Contains("census", selya.role.ToLowerInvariant());

            var auditQuest = questMap["quest_the_register_audit"];
            Assert.NotNull(auditQuest);
            Assert.Contains("audit", auditQuest.title.ToLowerInvariant());
        }

        [Fact]
        public void CrossSystem_DeterministicExecution_UnderSimulationPasses()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Run 50 reloads/queries to confirm deterministic stability
            for (int i = 0; i < 50; i++)
            {
                var system = new VerdictNpcSystem();
                int npcCount = VerdictNpcCatalogLoader.LoadAndRegister(system, dataDir, io, json);
                Assert.Equal(18, npcCount);

                var tapeSiloNpcs = system.Catalog.Where(n => n.locationId == "loc_archive_tape_silo").ToList();
                Assert.Equal(3, tapeSiloNpcs.Count); // Maro Veen, Elena Vane, Kasper Holt

                var doseCatalog = DoseContentCatalogLoader.Load(dataDir, io, json);
                Assert.Equal(12, doseCatalog.quests.Count);
            }
        }
    }
}
