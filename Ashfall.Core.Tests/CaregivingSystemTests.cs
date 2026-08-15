using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CaregivingSystemTests
    {
        // ── Helper: create a system with permissive host hooks ─────────

        static CaregivingSystem CreateSystem()
        {
            var sys = new CaregivingSystem();
            sys.IsAlive = _ => true;
            sys.CanProvideCare = _ => true;
            sys.NeedsCare = _ => true;
            return sys;
        }

        // ── 1. Assign caregiver succeeds ───────────────────────────────

        [Fact]
        public void AssignCaregiver_ValidPair_ReturnsTrue()
        {
            var sys = CreateSystem();
            Assert.True(sys.AssignCaregiver("sv_caregiver", "sv_patient"));
            Assert.Equal("sv_patient", sys.GetPatientForCaregiver("sv_caregiver"));
            Assert.Equal("sv_caregiver", sys.GetCaregiverForPatient("sv_patient"));
        }

        // ── 2. Cannot assign self as caregiver ─────────────────────────

        [Fact]
        public void AssignCaregiver_SameId_ReturnsFalse()
        {
            var sys = CreateSystem();
            Assert.False(sys.AssignCaregiver("sv_1", "sv_1"));
            Assert.Null(sys.GetPatientForCaregiver("sv_1"));
        }

        // ── 3. Null/empty IDs rejected ─────────────────────────────────

        [Fact]
        public void AssignCaregiver_NullOrEmpty_ReturnsFalse()
        {
            var sys = CreateSystem();
            Assert.False(sys.AssignCaregiver(null, "sv_2"));
            Assert.False(sys.AssignCaregiver("sv_1", null));
            Assert.False(sys.AssignCaregiver("", "sv_2"));
            Assert.False(sys.AssignCaregiver("sv_1", ""));
        }

        // ── 4. Dead caregiver or patient rejected ──────────────────────

        [Fact]
        public void AssignCaregiver_DeadSurvivor_ReturnsFalse()
        {
            var sys = CreateSystem();
            sys.IsAlive = id => id != "sv_dead";
            Assert.False(sys.AssignCaregiver("sv_dead", "sv_2"));
            Assert.False(sys.AssignCaregiver("sv_1", "sv_dead"));
        }

        // ── 5. Caregiver not able to provide care rejected ─────────────

        [Fact]
        public void AssignCaregiver_CaregiverCannotProvideCare_ReturnsFalse()
        {
            var sys = CreateSystem();
            sys.CanProvideCare = id => id != "sv_incap";
            Assert.False(sys.AssignCaregiver("sv_incap", "sv_patient"));
        }

        // ── 6. Patient not needing care rejected ───────────────────────

        [Fact]
        public void AssignCaregiver_PatientDoesNotNeedCare_ReturnsFalse()
        {
            var sys = CreateSystem();
            sys.NeedsCare = id => id != "sv_healthy";
            Assert.False(sys.AssignCaregiver("sv_caregiver", "sv_healthy"));
        }

        // ── 7. Reassigning patient fires ended for previous caregiver ──

        [Fact]
        public void AssignCaregiver_ReplacingCaregiver_FiresEndedForPrevious()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver1", "sv_patient");

            string endedCaregiver = null, endedPatient = null;
            sys.OnCaregivingEnded += (c, p) => { endedCaregiver = c; endedPatient = p; };

            sys.AssignCaregiver("sv_caregiver2", "sv_patient");

            Assert.Equal("sv_caregiver1", endedCaregiver);
            Assert.Equal("sv_patient", endedPatient);
            Assert.Equal("sv_caregiver2", sys.GetCaregiverForPatient("sv_patient"));
            Assert.Null(sys.GetPatientForCaregiver("sv_caregiver1"));
        }

        // ── 8. Unassign caregiver clears both sides ────────────────────

        [Fact]
        public void UnassignCaregiver_ClearsBothDirections()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            sys.UnassignCaregiver("sv_patient");

            Assert.Null(sys.GetPatientForCaregiver("sv_caregiver"));
            Assert.Null(sys.GetCaregiverForPatient("sv_patient"));
            Assert.False(sys.HasCaregiver("sv_patient"));
            Assert.False(sys.IsCaregiver("sv_caregiver"));
        }

        // ── 9. Tick applies health recovery bonus ──────────────────────

        [Fact]
        public void Tick_AppliesHealthRecoveryBonus()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            string bonusPatient = null;
            float bonusAmount = 0f;
            sys.ApplyHealthRecoveryBonus = (id, amt) => { bonusPatient = id; bonusAmount = amt; };

            sys.Tick(24f); // 1 day

            Assert.Equal("sv_patient", bonusPatient);
            Assert.Equal(CaregivingSystem.RecoverySpeedBonus, bonusAmount, 4);
        }

        // ── 10. Tick applies caregiver fatigue ─────────────────────────

        [Fact]
        public void Tick_AppliesCaregiverFatigue()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            string fatiguedId = null;
            float fatigueDelta = 0f;
            sys.ApplyFatigueDelta = (id, d) => { fatiguedId = id; fatigueDelta = d; };

            sys.Tick(24f);

            Assert.Equal("sv_caregiver", fatiguedId);
            Assert.Equal(CaregivingSystem.CaregiverFatigueDrain * 24f, fatigueDelta, 4);
        }

        // ── 11. Tick grows bond strength ───────────────────────────────

        [Fact]
        public void Tick_GrowsBondStrength()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            float before = sys.GetBondStrength("sv_patient");
            sys.Tick(24f);
            float after = sys.GetBondStrength("sv_patient");

            Assert.True(after > before);
            Assert.Equal(CaregivingSystem.BondGrowthPerDay, after - before, 4);
        }

        // ── 12. Bond strength capped at 1.0 ────────────────────────────

        [Fact]
        public void Tick_BondStrengthCappedAtOne()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            // 100 days → 0.02 * 100 = 2.0, capped at 1.0
            sys.Tick(2400f);

            Assert.Equal(1f, sys.GetBondStrength("sv_patient"), 4);
        }

        // ── 13. Tick calls AdjustAffinity hook ─────────────────────────

        [Fact]
        public void Tick_CallsAdjustAffinity()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            string affA = null, affB = null;
            float affDelta = 0f;
            sys.AdjustAffinity = (a, b, d) => { affA = a; affB = b; affDelta = d; };

            sys.Tick(24f);

            Assert.Equal("sv_caregiver", affA);
            Assert.Equal("sv_patient", affB);
            Assert.Equal(CaregivingSystem.AffinityGainPerDay, affDelta, 4);
        }

        // ── 14. Dialogue unlock fires at threshold ─────────────────────

        [Fact]
        public void Tick_FiresDialogueUnlockAtThreshold()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            var dialogueEvents = new List<(string c, string p)>();
            sys.OnCaregivingDialogueUnlocked += (c, p) => dialogueEvents.Add((c, p));

            // Bond growth per day = 0.02. Threshold = 0.5. Need 25 days to cross.
            // Tick 24 days (bond = 0.48) — no event
            sys.Tick(24f * 24f);
            Assert.Empty(dialogueEvents);

            // Tick 1 more day (bond = 0.50) — event fires
            sys.Tick(24f);
            Assert.Single(dialogueEvents);
            Assert.Equal("sv_caregiver", dialogueEvents[0].c);
            Assert.Equal("sv_patient", dialogueEvents[0].p);
        }

        // ── 15. Dialogue unlock fires only once ────────────────────────

        [Fact]
        public void Tick_DoesNotFireDialogueUnlockRepeatedly()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            int count = 0;
            sys.OnCaregivingDialogueUnlocked += (_, __) => count++;

            // Tick enough to cross threshold and beyond
            sys.Tick(24f * 30f);
            Assert.Equal(1, count);
        }

        // ── 16. Tick removes dead caregiver ────────────────────────────

        [Fact]
        public void Tick_RemovesDeadCaregiver()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            bool alive = true;
            sys.IsAlive = id => id != "sv_caregiver" || alive;

            string endedC = null, endedP = null;
            sys.OnCaregivingEnded += (c, p) => { endedC = c; endedP = p; };

            alive = false;
            sys.Tick(1f);

            Assert.False(sys.IsCaregiver("sv_caregiver"));
            Assert.Equal("sv_caregiver", endedC);
            Assert.Equal("sv_patient", endedP);
        }

        // ── 17. Tick removes dead patient ──────────────────────────────

        [Fact]
        public void Tick_RemovesDeadPatient()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            bool patientAlive = true;
            sys.IsAlive = id => id != "sv_patient" || patientAlive;

            patientAlive = false;
            sys.Tick(1f);

            Assert.False(sys.HasCaregiver("sv_patient"));
            Assert.Null(sys.GetPatientForCaregiver("sv_caregiver"));
        }

        // ── 18. Zero/negative tick does nothing ────────────────────────

        [Fact]
        public void Tick_ZeroOrNegative_DoesNothing()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_caregiver", "sv_patient");

            float bondBefore = sys.GetBondStrength("sv_patient");
            sys.Tick(0f);
            sys.Tick(-1f);
            Assert.Equal(bondBefore, sys.GetBondStrength("sv_patient"), 4);
        }

        // ── 19. CaptureState / RestoreState roundtrip ──────────────────

        [Fact]
        public void CaptureRestore_Roundtrip_PreservesAssignments()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_c1", "sv_p1");
            sys.AssignCaregiver("sv_c2", "sv_p2");
            sys.Tick(24f * 10f); // grow bonds

            var save = sys.CaptureState();

            var restored = CreateSystem();
            restored.RestoreState(save);

            Assert.Equal(sys.ActiveAssignmentCount, restored.ActiveAssignmentCount);
            Assert.Equal(sys.GetPatientForCaregiver("sv_c1"),
                restored.GetPatientForCaregiver("sv_c1"));
            Assert.Equal(sys.GetPatientForCaregiver("sv_c2"),
                restored.GetPatientForCaregiver("sv_c2"));
            Assert.Equal(sys.GetBondStrength("sv_p1"),
                restored.GetBondStrength("sv_p1"), 4);
            Assert.Equal(sys.GetBondStrength("sv_p2"),
                restored.GetBondStrength("sv_p2"), 4);
        }

        // ── 20. RestoreState(null) clears all state ────────────────────

        [Fact]
        public void RestoreState_Null_ClearsAll()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_c1", "sv_p1");
            Assert.Equal(1, sys.ActiveAssignmentCount);

            sys.RestoreState(null);
            Assert.Equal(0, sys.ActiveAssignmentCount);
            Assert.Null(sys.GetPatientForCaregiver("sv_c1"));
        }

        // ── 21. CaptureState is deep copy ──────────────────────────────

        [Fact]
        public void CaptureState_IsDeepCopy_MutatingOriginalDoesNotAffectSave()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_c1", "sv_p1");
            sys.Tick(24f);

            var save = sys.CaptureState();
            float savedBond = save.Assignments[0].BondStrength;

            // Mutate original
            sys.Tick(24f);

            Assert.Equal(savedBond, save.Assignments[0].BondStrength, 4);
        }

        // ── 22. OnStateChanged fires on assign/unassign/tick ───────────

        [Fact]
        public void OnStateChanged_FiresOnAssignUnassignTick()
        {
            var sys = CreateSystem();
            int stateChanges = 0;
            sys.OnStateChanged += () => stateChanges++;

            sys.AssignCaregiver("sv_c1", "sv_p1");
            Assert.Equal(1, stateChanges);

            sys.Tick(24f);
            Assert.Equal(2, stateChanges);

            sys.UnassignCaregiver("sv_p1");
            Assert.Equal(3, stateChanges);
        }

        // ── 23. UnassignCaregiverByCaregiver works ─────────────────────

        [Fact]
        public void UnassignCaregiverByCaregiver_ClearsAssignment()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_c1", "sv_p1");

            sys.UnassignCaregiverByCaregiver("sv_c1");

            Assert.False(sys.IsCaregiver("sv_c1"));
            Assert.False(sys.HasCaregiver("sv_p1"));
        }

        // ── 24. Caregiver reassignment cleans up old patient ───────────

        [Fact]
        public void AssignCaregiver_CaregiverAlreadyAssigned_UnassignsOldPatient()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_c1", "sv_p1");

            var endedEvents = new List<(string c, string p)>();
            sys.OnCaregivingEnded += (c, p) => endedEvents.Add((c, p));

            sys.AssignCaregiver("sv_c1", "sv_p2");

            Assert.Equal("sv_p2", sys.GetPatientForCaregiver("sv_c1"));
            Assert.False(sys.HasCaregiver("sv_p1"));
            Assert.Single(endedEvents);
            Assert.Equal("sv_c1", endedEvents[0].c);
            Assert.Equal("sv_p1", endedEvents[0].p);
        }

        // ── 25. ActiveAssignmentCount tracks correctly ─────────────────

        [Fact]
        public void ActiveAssignmentCount_TracksCorrectly()
        {
            var sys = CreateSystem();
            Assert.Equal(0, sys.ActiveAssignmentCount);

            sys.AssignCaregiver("sv_c1", "sv_p1");
            Assert.Equal(1, sys.ActiveAssignmentCount);

            sys.AssignCaregiver("sv_c2", "sv_p2");
            Assert.Equal(2, sys.ActiveAssignmentCount);

            sys.UnassignCaregiver("sv_p1");
            Assert.Equal(1, sys.ActiveAssignmentCount);
        }

        // ── 26. No host hooks set — tick doesn't throw ─────────────────

        [Fact]
        public void Tick_NoHostHooks_DoesNotThrow()
        {
            var sys = new CaregivingSystem();
            // No IsAlive, no hooks
            sys.AssignCaregiver("sv_c1", "sv_p1");
            sys.Tick(24f); // Should not throw
        }

        // ── 27. OnCaregivingStarted event fires ────────────────────────

        [Fact]
        public void AssignCaregiver_FiresStartedEvent()
        {
            var sys = CreateSystem();
            string startedC = null, startedP = null;
            sys.OnCaregivingStarted += (c, p) => { startedC = c; startedP = p; };

            sys.AssignCaregiver("sv_c1", "sv_p1");

            Assert.Equal("sv_c1", startedC);
            Assert.Equal("sv_p1", startedP);
        }

        // ── 28. OnCaregivingBondDeepened fires each tick ───────────────

        [Fact]
        public void Tick_FiresBondDeepenedEvent()
        {
            var sys = CreateSystem();
            sys.AssignCaregiver("sv_c1", "sv_p1");

            var bondEvents = new List<(string c, string p, float b)>();
            sys.OnCaregivingBondDeepened += (c, p, b) => bondEvents.Add((c, p, b));

            sys.Tick(24f);
            sys.Tick(24f);

            Assert.Equal(2, bondEvents.Count);
            Assert.True(bondEvents[1].b > bondEvents[0].b);
        }
    }
}
