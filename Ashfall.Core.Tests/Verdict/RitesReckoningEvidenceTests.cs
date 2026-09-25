// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W10 — Memorial rites leave a trace in the Reckoning record
//                (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W10-RITES-RECKONING-EVIDENCE (cases AV.10).
//
// The P0 evidence-vocabulary gate resolved as follows: the Reckoning's evidence
// vocabulary is an UNTYPED counter whose meaning is "read machine-log fragments",
// and VerdictAccusationSystem reads it to build culpability. Enrolling rites
// into that counter would make grief a prosecutorial instrument. So the record
// gains a DISTINCT additive trace kind (riteTraceTotal), and these tests pin
// that separation as hard as they pin the enrollment itself.
// ============================================================================

using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RitesReckoningEvidenceTests
    {
        private static ReckoningSystem Fresh() => new ReckoningSystem();

        [Fact]
        public void APERFORMEDRite_LeavesATrace()
        {
            var reckoning = Fresh();
            int before = reckoning.RiteTraceCount;
            reckoning.EnrollRiteTrace();
            Assert.Equal(before + 1, reckoning.RiteTraceCount);
        }

        [Fact]
        public void RitesDoNotInflateMachineLogEvidence()   // the P0 gate, pinned
        {
            var reckoning = Fresh();
            int evidenceBefore = reckoning.State.enrolledEvidence;
            reckoning.EnrollRiteTrace(1);
            reckoning.EnrollRiteTrace(1);
            Assert.Equal(evidenceBefore, reckoning.State.enrolledEvidence);
            Assert.Equal(2, reckoning.RiteTraceCount);
        }

        [Fact]
        public void RiteTraceSurvivesSaveAndLoad()           // capture is field-by-field
        {
            var reckoning = Fresh();
            reckoning.EnrollRiteTrace(3);
            var captured = reckoning.CaptureState();

            var reloaded = Fresh();
            reloaded.RestoreState(captured);
            Assert.Equal(3, reloaded.RiteTraceCount);
        }

        [Fact]
        public void OldSaveWithoutTheField_RestsAsZero()     // legacy tolerance
        {
            var legacy = new ReckoningState(); // no field set: an old save
            var reckoning = Fresh();
            reckoning.RestoreState(legacy);
            Assert.Equal(0, reckoning.RiteTraceCount);
        }

        [Fact]
        public void EvidenceEnrollment_IsUnaffected()
        {
            var reckoning = Fresh();
            reckoning.EnrollEvidence(2);
            Assert.Equal(2, reckoning.State.enrolledEvidence);
            Assert.Equal(0, reckoning.RiteTraceCount);
        }

        [Fact]
        public void AmountIsClampedToAtLeastOne()
        {
            var reckoning = Fresh();
            reckoning.EnrollRiteTrace(0);
            Assert.Equal(1, reckoning.RiteTraceCount);
            reckoning.EnrollRiteTrace(-5);
            Assert.Equal(2, reckoning.RiteTraceCount);
        }

        [Fact]
        public void RegisterLine_IsEmptyWhenNothingWasRecorded()
        {
            Assert.Equal(string.Empty, VerdictReadout.RiteTraceLine(0));
        }

        [Fact]
        public void RegisterLine_AppearsOnceRitesAreRecorded()
        {
            string line = VerdictReadout.RiteTraceLine(1);
            Assert.False(string.IsNullOrEmpty(line));
            Assert.Contains("memorial register", line);
        }

        [Fact]
        public void RegisterLine_IsDeterministic()            // no per-frame RNG
        {
            Assert.Equal(VerdictReadout.RiteTraceLine(4, 2), VerdictReadout.RiteTraceLine(4, 2));
        }

        [Fact]
        public void InstrumentReadout_IsUnchangedByRites()    // the machine does not mourn
        {
            var reckoning = Fresh();
            string before = VerdictReadout.LineFor(reckoning.State, 0, 0);
            reckoning.EnrollRiteTrace(5);
            string after = VerdictReadout.LineFor(reckoning.State, 0, 0);
            Assert.Equal(before, after);
        }

        [Fact]
        public void PhaseGate_StillKeysOnEvidenceNotRites()   // rites must not open the census gate
        {
            var reckoning = Fresh();
            int evidenceBefore = reckoning.State.enrolledEvidence;
            reckoning.EnrollRiteTrace(10);
            // The Reckoning's evidence gate reads enrolledEvidence only; a campaign
            // that held many vigils but read no logs has still opened no gate.
            Assert.Equal(evidenceBefore, reckoning.State.enrolledEvidence);
        }
    }
}
