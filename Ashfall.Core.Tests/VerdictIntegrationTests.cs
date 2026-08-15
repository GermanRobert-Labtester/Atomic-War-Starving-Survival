using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — end-to-end integration tests.
    /// These protect the things JSON validity cannot: reachability and
    /// playability. They exercise quest eligibility/activation/advancement
    /// (the stageId-DAG runtime), the merged door-encounter catalog, evidence
    /// enrollment into the Reckoning gates, and the ending priorities.
    /// </summary>
    public class VerdictIntegrationTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            return string.Empty;
        }

        private static QuestlineSystem BuildQuestSystem()
        {
            var system = new QuestlineSystem();
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return system;
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            VerdictQuestCatalogLoader.LoadAndRegister(system, dataDir, io, json);
            return system;
        }

        // ── Quest reachability (the critical §13/§36 gap) ──────────────────────

        [Fact]
        public void VerdictQuests_AllLoad_AndArePlayable()
        {
            var system = BuildQuestSystem();
            // All 8 verdict questlines must be registered…
            Assert.Contains(system.GetAvailableQuestlines(250), q => q.questlineId == "quest_verdict_the_warm_range");
            Assert.Contains(system.GetAvailableQuestlines(250), q => q.questlineId == "quest_verdict_the_reckoning_call");
            Assert.Contains(system.GetAvailableQuestlines(250), q => q.questlineId == "quest_verdict_the_hold");

            // … and each must be PLAYABLE (stageId-DAG with choices), unlike the
            // legacy flat-shape catalog which strands quests (see IsPlayable doc).
            var verdictIds = new[]
            {
                "quest_verdict_the_warm_range",
                "quest_verdict_the_reckoning_call",
                "quest_verdict_the_hold",
                "quest_verdict_eden_grabs",
                "quest_verdict_the_tape_silo",
                "quest_verdict_the_mortars_timetable",
                "quest_verdict_the_shift_charter",
                "quest_verdict_the_summons"
            };
            foreach (var id in verdictIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);                     // registered
                Assert.True(system.IsPlayable(def), id); // has first stage + choices
            }
        }

        [Fact]
        public void VerdictQuest_WarmRange_AdvancesToResolve_AndGrantsKey()
        {
            var system = BuildQuestSystem();
            int day = 200;
            Assert.True(system.StartQuestline("quest_verdict_the_warm_range", day));

            var result = system.TakeChoice("quest_verdict_the_warm_range", "choice_warm_follow_cable", day);
            Assert.NotNull(result);
            Assert.Equal("stage_warm_fuse", result.nextStageId);
            Assert.Equal("evidence_fuse_linen", result.grantItemId);

            var result2 = system.TakeChoice("quest_verdict_the_warm_range", "choice_warm_resolve_facets", day);
            Assert.NotNull(result2);
            Assert.Equal("item_archive_tape_silo_key", result2.grantItemId);
            Assert.Equal(QuestlineStatus.Completed, result2.newQuestStatus);
            Assert.Contains(system.State.completedQuestlineIds, id => id == "quest_verdict_the_warm_range");
        }

        [Fact]
        public void VerdictQuest_Hold_AllThreeBranchesResolve()
        {
            int day = 250;
            foreach (var choice in new[] { "choice_hold_release", "choice_hold_retain", "choice_hold_count" })
            {
                var s = BuildQuestSystem(); // fresh per branch
                var def = s.FindDefinition("quest_verdict_the_hold");
                Assert.NotNull(def);
                Assert.True(s.StartQuestline("quest_verdict_the_hold", day));
                var r = s.TakeChoice("quest_verdict_the_hold", choice, day);
                Assert.NotNull(r);
                Assert.Equal(QuestlineStatus.Completed, r.newQuestStatus);
            }
        }

        // ── Evidence → Reckoning gate (authoritative chain) ────────────────────

        [Fact]
        public void EvidenceEnrollment_DrivesCulpableGate_AndEnding()
        {
            var reckoning = new ReckoningSystem();
            var evidence = new EvidenceLedger();

            // Day 210 without evidence: no Culpable promotion.
            reckoning.Poll(210, 14, 0, 0);
            Assert.Equal(ReckoningPhase.Knowing, reckoning.Phase);

            // Reading a machine log enrolls evidence, which opens Culpable.
            evidence.Enroll("evidence_geophone_hymn", 162);
            var fired = reckoning.Poll(212, 14, 1, evidence.Count);
            Assert.Equal(ReckoningPhase.Culpable, reckoning.Phase);

            // Counted + ending derives from authoritative state.
            reckoning.Poll(241, 14, 1, evidence.Count);
            string ending = VerdictEndingEvaluator.DecideEnding(reckoning.State, evidence.Count, 241);
            Assert.Equal("ending_verdict_the_count_is_held", ending); // sparse evidence => held

            evidence.Enroll("evidence_fuse_linen", 180);
            evidence.Enroll("evidence_census_draft", 190);
            evidence.Enroll("evidence_uxo_register", 200);
            evidence.Enroll("evidence_call_plain", 210);
            string endingRich = VerdictEndingEvaluator.DecideEnding(reckoning.State, evidence.Count, 241);
            Assert.Equal("ending_verdict_the_sector_recounts", endingRich); // >= 4 => recount
        }

        [Fact]
        public void EndingSelection_IsAuthoritative_NotTextDriven()
        {
            var reckoning = new ReckoningSystem();
            reckoning.Poll(160, 14, 1, 0);
            reckoning.EnrollEvidence(1);
            reckoning.Poll(211, 14, 1, 1);
            reckoning.Poll(241, 14, 1, 1);
            Assert.True(reckoning.SelectEnding("ending_verdict_the_offer_is_a_lease", 241));

            // ResolvedEnding reads the state, not strings scattered in callers.
            Assert.Equal("ending_verdict_the_offer_is_a_lease", VerdictEndingEvaluator.ResolvedEnding(reckoning.State));
            Assert.True(VerdictEndingEvaluator.IsTempestDecommissioned(reckoning.State) == false);
        }

        // ── Merged door-encounter catalog (reachability) ───────────────────────

        [Fact]
        public void VerdictDoorEncounters_AreLoaded_FromCanonicalCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new DoorEncounterSystem();
            int loaded = DoorEncounterCatalogLoader.LoadAndRegister(system, dataDir, io, json);

            Assert.True(loaded >= 66, $"expected >= 66 entries (60 base + 8 verdict), got {loaded}");

            var verdictIds = new[]
            {
                "door_encounter_verdict_tape_seller",
                "door_encounter_verdict_relay_repair",
                "door_encounter_verdict_census_clerk",
                "door_encounter_verdict_sound_engineer",
                "door_encounter_verdict_salt_gatherer",
                "door_encounter_verdict_soil_sampler",
                "door_encounter_verdict_tape_exchange",
                "door_encounter_verdict_clock_parasite"
            };
            foreach (var id in verdictIds)
                Assert.Contains(system.Catalog, e => e.encounterId == id);
        }

        [Fact]
        public void VerdictMergedRadio_AreLoaded_ByYearOfAshRadio()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var radio = YearOfAshCatalogLoader.LoadRadioBroadcasts(dataDir, io, json);

            var verdictIds = new[]
            {
                "radio_verdict_call_open",
                "radio_verdict_call_taken",
                "radio_verdict_eden_was_here",
                "radio_verdict_carrier_on_window"
            };
            foreach (var id in verdictIds)
                Assert.Contains(radio, r => r.id == id);
        }

        // ── Epilogue corpus (ending prose reachability) ─────────────────────────

        [Fact]
        public void VerdictEndings_AreInLiveEpilogueCorpus()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, io, json);

            foreach (var key in VerdictEndingEvaluator.EndingKeys)
                Assert.Contains(epilogues, e => e.endingKey == key);
        }
    }
}
