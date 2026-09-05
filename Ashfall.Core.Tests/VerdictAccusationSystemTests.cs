using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VerdictAccusationSystemTests
    {
        // ── Helpers ──────────────────────────────────────────────────────────

        private static (ReckoningSystem reckoning, MachineLogSystem machineLog, EvidenceLedger ledger) BuildCulpableReckoning(int evidence = 1)
        {
            var reckoning = new ReckoningSystem();
            var machineLog = new MachineLogSystem();
            var ledger = new EvidenceLedger();

            // Advance to Culpable
            reckoning.Poll(160, 100, 0, 0); // Knowing
            for (int i = 0; i < evidence; i++)
                reckoning.EnrollEvidence(1);
            reckoning.Poll(210, 100, evidence, evidence); // Culpable
            return (reckoning, machineLog, ledger);
        }

        private static VerdictAccusationSystem BuildSystem(ReckoningSystem reckoning)
        {
            var sys = new VerdictAccusationSystem();
            sys.Bind(reckoning);
            return sys;
        }

        // ── CanAccuse — phase gate ────────────────────────────────────────────

        [Fact]
        public void CanAccuse_BeforeCulpable_ReturnsPhaseLocked()
        {
            var reckoning = new ReckoningSystem();
            // Phase = Dormant — no poll
            var sys = BuildSystem(reckoning);
            var result = sys.CanAccuse("case_the_census_machine", "suspect_the_bureau_clerk", 200);
            Assert.Equal(AccusationAllowed.PhaseNotReached, result.Status);
        }

        [Fact]
        public void CanAccuse_KnowingPhaseNoEvidence_ReturnsPhaseLocked()
        {
            var reckoning = new ReckoningSystem();
            reckoning.Poll(160, 100, 0, 0); // Knowing
            var sys = BuildSystem(reckoning);
            var result = sys.CanAccuse("case_the_census_machine", "suspect_the_bureau_clerk", 165);
            Assert.Equal(AccusationAllowed.PhaseNotReached, result.Status);
        }

        [Fact]
        public void CanAccuse_CulpableNoEvidence_ReturnsMissingEvidence()
        {
            var reckoning = new ReckoningSystem();
            reckoning.Poll(160, 100, 0, 0);
            // Force culpable without evidence via internal state restore
            var state = reckoning.CaptureState();
            state.phase = ReckoningPhase.Culpable;
            state.enrolledEvidence = 0;
            reckoning.RestoreState(state);

            var sys = BuildSystem(reckoning);
            var result = sys.CanAccuse("case_the_census_machine", "suspect_the_bureau_clerk", 215);
            Assert.Equal(AccusationAllowed.MissingEvidence, result.Status);
        }

        [Fact]
        public void CanAccuse_CulpableWithEvidence_ReturnsAllowed()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(1);
            var sys = BuildSystem(reckoning);
            var result = sys.CanAccuse("case_the_census_machine", "suspect_the_bureau_clerk", 215);
            Assert.Equal(AccusationAllowed.Allowed, result.Status);
        }

        [Fact]
        public void CanAccuse_UnknownCase_ReturnsUnknownCase()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var sys = BuildSystem(reckoning);
            var result = sys.CanAccuse("case_invented_nonexistent", "suspect_nobody", 215);
            Assert.Equal(AccusationAllowed.UnknownCase, result.Status);
        }

        [Fact]
        public void CanAccuse_AlreadyResolved_ReturnsAlreadyResolved()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var sys = BuildSystem(reckoning);
            // Resolve first
            sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, null);
            // Try again
            var result = sys.CanAccuse("case_the_census_machine", "suspect_the_bureau_clerk", 215);
            Assert.Equal(AccusationAllowed.AlreadyResolved, result.Status);
        }

        // ── ResolveTribunal ───────────────────────────────────────────────────

        [Fact]
        public void ResolveTribunal_IneligibleNoEvidence_ReturnsNull()
        {
            var reckoning = new ReckoningSystem();
            var sys = BuildSystem(reckoning);
            var verdict = sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, null);
            Assert.Null(verdict);
        }

        [Fact]
        public void ResolveTribunal_TwoEvidenceGuilty_SetsEndingKey()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var sys = BuildSystem(reckoning);
            var verdict = sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, null);
            Assert.NotNull(verdict);
            Assert.True(verdict!.Guilty);
            Assert.Equal("ending_verdict_the_sector_recounts", verdict.EndingKey);
        }

        [Fact]
        public void ResolveTribunal_OneEvidenceNotGuilty_SetsNotGuiltyEnding()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(1);
            var sys = BuildSystem(reckoning);
            var verdict = sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, null);
            Assert.NotNull(verdict);
            Assert.False(verdict!.Guilty);
            Assert.Equal("ending_verdict_the_count_is_held", verdict.EndingKey);
        }

        [Fact]
        public void ResolveTribunal_AppliesMoralFlag()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var seededRng = new SeededRng(42);
            var moralSys = new MoralChoiceSystem(seededRng);
            var sys = BuildSystem(reckoning);

            var verdict = sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, moralSys);
            Assert.NotNull(verdict);
            Assert.True(verdict!.MoralDelta != 0);
            // Moral flag should be set on the moralSystem
            Assert.True(moralSys.HasFlag("flag_tribunal_guilty_case_the_census_machine"));
        }

        [Fact]
        public void ResolveTribunal_SetsJournalEntry()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var sys = BuildSystem(reckoning);
            var verdict = sys.ResolveTribunal("case_the_long_silence", "suspect_the_broadcast_director", 215, null);
            Assert.NotNull(verdict);
            Assert.False(string.IsNullOrWhiteSpace(verdict!.JournalEntry));
        }

        [Fact]
        public void ResolveTribunal_MarksResolved()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var sys = BuildSystem(reckoning);
            Assert.False(sys.IsResolved("case_the_census_machine"));
            sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, null);
            Assert.True(sys.IsResolved("case_the_census_machine"));
        }

        // ── Save round-trip ───────────────────────────────────────────────────

        [Fact]
        public void AccusationState_CaptureRestore_RoundTrip()
        {
            var (reckoning, _, _) = BuildCulpableReckoning(2);
            var sys = BuildSystem(reckoning);
            sys.ResolveTribunal("case_the_census_machine", "suspect_the_bureau_clerk", 215, null);
            sys.ResolveTribunal("case_the_long_silence", "suspect_the_broadcast_director", 220, null);

            var captured = sys.CaptureState();
            var sys2 = new VerdictAccusationSystem();
            sys2.RestoreState(captured);

            Assert.True(sys2.IsResolved("case_the_census_machine"));
            Assert.True(sys2.IsResolved("case_the_long_silence"));
            Assert.False(sys2.IsResolved("case_the_missing_count"));
        }

        // ── Save migration ────────────────────────────────────────────────────

        [Fact]
        public void VerdictSave_OldVersionMigration_AccusationsEmpty()
        {
            // Build synthetic v3 save
            var v3 = new VerdictSaveV3 { saveVersion = 3, simDay = 210 };
            v3.Checksum = SaveChecksum.Compute(v3);

            var json = new SystemTextJsonSerializer();
            string encoded = json.Serialize(v3);

            bool ok = VerdictSaveCodec.TryDecode(encoded, json, out var migrated);
            Assert.True(ok);
            Assert.Equal(4, migrated.saveVersion);
            Assert.NotNull(migrated.accusations);
            Assert.Empty(migrated.accusations.resolvedCaseIds);
        }

        [Fact]
        public void VerdictSave_CurrentVersion_AccusationsPreserved()
        {
            var accusationState = new VerdictAccusationState();
            accusationState.resolvedCaseIds.Add("case_the_census_machine");
            accusationState.caseVerdicts["case_the_census_machine"] = "ending_verdict_the_sector_recounts";

            var save = new VerdictSave
            {
                saveVersion = 4,
                simDay = 215,
                accusations = accusationState
            };
            save.Checksum = SaveChecksum.Compute(save);

            var json = new SystemTextJsonSerializer();
            string encoded = json.Serialize(save);

            bool ok = VerdictSaveCodec.TryDecode(encoded, json, out var decoded);
            Assert.True(ok);
            Assert.Contains("case_the_census_machine", decoded.accusations.resolvedCaseIds);
            Assert.Equal("ending_verdict_the_sector_recounts", decoded.accusations.caseVerdicts["case_the_census_machine"]);
        }
    }
}
