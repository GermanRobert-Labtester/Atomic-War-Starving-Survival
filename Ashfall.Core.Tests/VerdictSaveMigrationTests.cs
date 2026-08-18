using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — save-migration + core unit tests.
    /// Item 9: v1→v2 migration must backfill NPC state and keep phase/evidence.
    /// Also re-covers MachineLog / Reckoning / Evidence / NPC behaviors that the
    /// consolidation pass dropped.
    /// </summary>
    public class VerdictSaveMigrationTests
    {
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        // ── Item 9: v1 → v2 save migration ────────────────────────────────────

        [Fact]
        public void V1Save_WithoutNpcField_MigratesToV3_AndBackfillsNpcs()
        {
            // A genuine v1 save: hashed over the v1 field set (no npcs/radio/quests).
            var v1 = new VerdictSaveV1
            {
                saveVersion = 1,
                simDay = 241,
                machineLog = new MachineLogSystemState(),
                reckoning = new ReckoningState { phase = ReckoningPhase.Counted, countPresented = true, callResolved = true },
                evidence = new EvidenceLedgerState { enrolled = new List<string> { "evidence_fuse_linen" } },
                censusLastWindowDay = 7
            };
            v1.Checksum = SaveChecksum.Compute(v1);

            string json = s_json.Serialize(v1);
            Assert.True(VerdictSaveCodec.TryDecode(json, s_json, out var loaded), "v1 save must decode");
            Assert.Equal(3, loaded.saveVersion);             // migrated to current v3
            Assert.Equal(ReckoningPhase.Counted, loaded.reckoning.phase);
            Assert.True(loaded.reckoning.countPresented);
            Assert.Contains("evidence_fuse_linen", loaded.evidence.enrolled);
            Assert.NotNull(loaded.npcs);                     // backfilled empty state
            Assert.Empty(loaded.npcs.spokenNpcIds);
            Assert.NotNull(loaded.quests);                   // quest section backfilled
            Assert.Empty(loaded.quests.active);

            // Restore into a fresh session survives.
            var ml = new MachineLogSystem();
            var rec = new ReckoningSystem();
            var ev = new EvidenceLedger();
            var npcs = new VerdictNpcSystem();
            VerdictSaveCodec.Restore(loaded, ml, rec, ev, npcs);
            Assert.Equal(ReckoningPhase.Counted, rec.Phase);
            Assert.True(ev.IsEnrolled("evidence_fuse_linen"));
        }

        [Fact]
        public void V2Save_MigratesToV3_WithEmptyQuestSection()
        {
            // A genuine v2 save: npcs + radio present, no quest section, hashed
            // over the v2 field set.
            var v2 = new VerdictSaveV2
            {
                saveVersion = 2,
                simDay = 250,
                reckoning = new ReckoningState { phase = ReckoningPhase.Counted },
                npcs = new VerdictNpcState()
            };
            v2.Checksum = SaveChecksum.Compute(v2);

            string json = s_json.Serialize(v2);
            Assert.True(VerdictSaveCodec.TryDecode(json, s_json, out var loaded), "v2 save must decode");
            Assert.Equal(3, loaded.saveVersion);
            Assert.NotNull(loaded.quests);
            Assert.Empty(loaded.quests.active);
            Assert.Equal(ReckoningPhase.Counted, loaded.reckoning.phase);
        }

        [Fact]
        public void GenuineLegacyV2Save_ChecksumValidatedOverLegacyShape_NotRejectedAsTampered()
        {
            // Regression (H1): a true pre-v3 save carries no `quests` key and was
            // hashed over the v2 field set. It must migrate, not be rejected as
            // tampered by the current-shape checksum.
            var v2 = new VerdictSaveV2
            {
                saveVersion = 2,
                simDay = 300,
                reckoning = new ReckoningState { phase = ReckoningPhase.Counted, countPresented = true, callResolved = true },
                evidence = new EvidenceLedgerState { enrolled = new List<string> { "evidence_eden_log" } }
            };
            v2.Checksum = SaveChecksum.Compute(v2);
            string json = s_json.Serialize(v2);
            Assert.DoesNotContain("\"quests\"", json, StringComparison.Ordinal);

            Assert.True(VerdictSaveCodec.TryDecode(json, s_json, out var loaded), "genuine v2 save must migrate");
            Assert.Equal(3, loaded.saveVersion);
            Assert.True(loaded.reckoning.countPresented);
            Assert.Contains("evidence_eden_log", loaded.evidence.enrolled);
            Assert.Empty(loaded.quests.active);
        }

        [Fact]
        public void TooOldSave_BelowMigrationFloor_IsRejected()
        {
            var v0 = new VerdictSave { saveVersion = 0 };
            v0.Checksum = SaveChecksum.Compute(v0);
            string json = s_json.Serialize(v0);
            Assert.False(VerdictSaveCodec.TryDecode(json, s_json, out _));
        }

        [Fact]
        public void NewerSaveVersion_IsRejected()
        {
            var v99 = new VerdictSave { saveVersion = 99 };
            v99.Checksum = SaveChecksum.Compute(v99);
            string json = s_json.Serialize(v99);
            Assert.False(VerdictSaveCodec.TryDecode(json, s_json, out _));
        }

        // ── MachineLog unit coverage ───────────────────────────────────────────

        [Fact]
        public void MachineLog_CorruptionMarker_ConsumesDataDrivenCorpus()
        {
            var sys = new MachineLogSystem();
            var corpus = new List<string> { "[CENSUS WINDOW: garbled]" };
            Assert.True(sys.InsertCorruptionMarker(213, new SeededRng(7), corpus));
            Assert.Equal("[CENSUS WINDOW: garbled]", sys.Entries[0].bodyShort);
            Assert.Equal("anomaly", sys.Entries[0].kind);
        }

        [Fact]
        public void MachineLog_SaveCapturesReadFlags()
        {
            var sys = new MachineLogSystem();
            sys.Post("loc_geophone_pit_1", 170, "operating", "a tap", "evidence_geophone_hymn");
            sys.ReadEntry(0);
            var snap = sys.CaptureState();
            var fresh = new MachineLogSystem();
            fresh.RestoreState(snap);
            Assert.True(fresh.Entries[0].read);
            Assert.Equal(1, fresh.ReadCount());
        }

        // ── Reckoning unit coverage ────────────────────────────────────────────

        [Fact]
        public void Reckoning_NoEvidence_StaysDormantPastCulpableDay()
        {
            var sys = new ReckoningSystem();
            sys.Poll(160, 14, 0, 0); // Knowing
            var fired = sys.Poll(230, 14, 0, 0); // 230 but zero evidence
            Assert.Equal(ReckoningPhase.Knowing, sys.Phase);
            Assert.DoesNotContain(fired, e => e == "phase_culpable");
        }

        [Fact]
        public void Reckoning_CensusWindowOpen_OnlyFromCulpable()
        {
            var sys = new ReckoningSystem();
            Assert.False(sys.IsCensusWindowOpen(200));
            sys.EnrollEvidence(1);
            sys.Poll(160, 14, 1, 1);
            sys.Poll(212, 14, 1, 1);
            Assert.True(sys.IsCensusWindowOpen(212));
        }

        // ── Evidence unit coverage ─────────────────────────────────────────────

        [Fact]
        public void Evidence_EnrollIdempotent_FiresOnce()
        {
            var ledger = new EvidenceLedger();
            int fires = 0;
            ledger.OnEnrolled += _ => fires++;
            Assert.True(ledger.Enroll("evidence_geophone_hymn", 170));
            Assert.False(ledger.Enroll("evidence_geophone_hymn", 170));
            Assert.Equal(1, fires);
            Assert.Equal(1, ledger.Count);
        }

        // ── NPC system coverage (item 3) ───────────────────────────────────────

        [Fact]
        public void VerdictNpcSystem_GatesOnFlagAndPhase()
        {
            var sys = new VerdictNpcSystem();
            sys.Register(new VerdictNpcEntry
            {
                id = "npc_maro_veen",
                name = "Maro Veen",
                kind = "tape_echo",
                gatingFlag = "flag_verdict_call_resolved",
                locationId = "loc_archive_tape_silo",
                phaseMin = 3,
                dialogue = new List<string> { "The count is open." }
            });

            // Flag not set → unavailable.
            Assert.Empty(sys.GetAvailable(new[] { "other_flag" }, 3, "loc_archive_tape_silo"));
            // Flag set, phase too low → unavailable.
            Assert.Empty(sys.GetAvailable(new[] { "flag_verdict_call_resolved" }, 2, "loc_archive_tape_silo"));
            // Flag set + phase 3 → available; Speak is one-shot.
            var avail = sys.GetAvailable(new[] { "flag_verdict_call_resolved" }, 3, "loc_archive_tape_silo");
            Assert.Single(avail);
            Assert.True(sys.Speak("npc_maro_veen", "loc_archive_tape_silo"));
            Assert.False(sys.Speak("npc_maro_veen", "loc_archive_tape_silo")); // one-shot
        }

        // ── Endings authoritative state (item-9 adjacency) ─────────────────────

        [Fact]
        public void VerdictEnding_MigratedSave_StillSelectsFromState()
        {
            var v1 = new VerdictSaveV1 { saveVersion = 1 };
            v1.reckoning.phase = ReckoningPhase.Counted;
            v1.reckoning.callResolved = true;
            v1.Checksum = SaveChecksum.Compute(v1);
            Assert.True(VerdictSaveCodec.TryDecode(s_json.Serialize(v1), s_json, out var loaded));

            var rec = new ReckoningSystem();
            VerdictSaveCodec.Restore(loaded, new MachineLogSystem(), rec, new EvidenceLedger(), new VerdictNpcSystem());
            Assert.Equal("ending_verdict_the_count_is_held", VerdictEndingEvaluator.DecideEnding(rec.State, 0, 300));
        }
    }
}
