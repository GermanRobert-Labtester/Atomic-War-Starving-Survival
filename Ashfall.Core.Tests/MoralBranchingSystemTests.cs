using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests
{
    public class MoralBranchingSystemTests
    {
        private static MoralBranchState MakeSurvivor(string id, bool alive = true)
            => new MoralBranchState { SurvivorId = id, IsAlive = alive };

        // ── Basic branching ────────────────────────────────────────────

        [Fact]
        public void FreshSurvivor_NoBranch()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");
            Assert.Equal(MoralBranchDirection.Neutral, sv.BranchDirection);
            Assert.False(sv.HasMoralBranch);
            Assert.Equal(0, sv.MoralChoiceCount);
        }

        [Fact]
        public void ChoicesBelowThreshold_NoBranchDecided()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            // 4 empathy choices — below the 5-choice threshold
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch - 1; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            Assert.Equal(MoralBranchDirection.Neutral, sv.BranchDirection);
            Assert.Equal(4, sv.MoralChoiceCount);
        }

        [Fact]
        public void FiveEmpathyChoices_BurdenedCompassion()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            Assert.Equal(MoralBranchDirection.BurdenedCompassion, sv.BranchDirection);
            Assert.Equal(MoralBranchingSystem.CompassionBaseLevel, sv.BurdenedCompassionLevel);
            Assert.True(sv.HasMoralBranch);
        }

        [Fact]
        public void FivePragmatismChoices_NumbedResilience()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: false);

            Assert.Equal(MoralBranchDirection.NumbedResilience, sv.BranchDirection);
            Assert.Equal(MoralBranchingSystem.NumbedBaseLevel, sv.NumbedResilienceLevel);
        }

        [Fact]
        public void BranchIsLockedIn_ExtraChoicesIgnored()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            // Branch to BurdenedCompassion
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            // Now try to push toward Numbed — should be ignored
            sys.RegisterMoralChoice(sv, isEmpathyChoice: false);
            sys.RegisterMoralChoice(sv, isEmpathyChoice: false);

            Assert.Equal(MoralBranchDirection.BurdenedCompassion, sv.BranchDirection);
            Assert.Equal(MoralBranchingSystem.ChoicesToBranch, sv.MoralChoiceCount);
        }

        // ── Mixed choices: last choice decides ─────────────────────────

        [Fact]
        public void MixedChoices_LastChoiceDecidesBranch()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            // 4 pragmatism + 1 empathy (the 5th)
            for (int i = 0; i < 4; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: false);
            sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            Assert.Equal(MoralBranchDirection.BurdenedCompassion, sv.BranchDirection);
        }

        // ── Dead survivor guards ───────────────────────────────────────

        [Fact]
        public void DeadSurvivor_ChoicesIgnored()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01", alive: false);

            for (int i = 0; i < 10; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            Assert.Equal(MoralBranchDirection.Neutral, sv.BranchDirection);
            Assert.Equal(0, sv.MoralChoiceCount);
        }

        [Fact]
        public void NullSurvivor_NoException()
        {
            var sys = new MoralBranchingSystem();
            sys.RegisterMoralChoice(null, true);
            sys.OnHelpedOthers(null);
            sys.OnTragedyWitnessed(null);
            Assert.False(sys.IsComfortBlocked(null));
            Assert.False(sys.IsDeathMoraleImmune(null));
        }

        // ── Burdened Compassion effects ────────────────────────────────

        [Fact]
        public void OnHelpedOthers_CompassionSurvivor_ShelterBuffApplied()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            float shelterDelta = 0f;
            sys.ApplyShelterMoraleDelta = delta => shelterDelta = delta;

            // Branch to compassion
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            sys.OnHelpedOthers(sv);
            Assert.Equal(MoralBranchingSystem.BurdenedCompassionShelterMoraleBuff, shelterDelta);
        }

        [Fact]
        public void OnHelpedOthers_NumbedSurvivor_NoBuff()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            float shelterDelta = 0f;
            sys.ApplyShelterMoraleDelta = delta => shelterDelta = delta;

            // Branch to numbed
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: false);

            sys.OnHelpedOthers(sv);
            Assert.Equal(0f, shelterDelta);
        }

        [Fact]
        public void OnTragedyWitnessed_CompassionSurvivor_MoralePenaltyApplied()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            float moraleDelta = 0f;
            MoralBranchState target = null;
            sys.ApplyMoraleDelta = (s, delta) => { target = s; moraleDelta = delta; };

            // Branch to compassion
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            sys.OnTragedyWitnessed(sv);
            Assert.Same(sv, target);
            Assert.Equal(MoralBranchingSystem.BurdenedCompassionTragedyPenalty, moraleDelta);
        }

        [Fact]
        public void OnTragedyWitnessed_NumbedSurvivor_NoPenalty()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            bool called = false;
            sys.ApplyMoraleDelta = (s, delta) => called = true;

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: false);

            sys.OnTragedyWitnessed(sv);
            Assert.False(called);
        }

        // ── Numbed Resilience effects ──────────────────────────────────

        [Fact]
        public void IsComfortBlocked_NumbedSurvivor_ReturnsTrue()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: false);

            Assert.True(sys.IsComfortBlocked(sv));
        }

        [Fact]
        public void IsComfortBlocked_CompassionSurvivor_ReturnsFalse()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            Assert.False(sys.IsComfortBlocked(sv));
        }

        [Fact]
        public void IsDeathMoraleImmune_NumbedSurvivor_ReturnsTrue()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: false);

            Assert.True(sys.IsDeathMoraleImmune(sv));
        }

        [Fact]
        public void IsDeathMoraleImmune_CompassionSurvivor_ReturnsFalse()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, isEmpathyChoice: true);

            Assert.False(sys.IsDeathMoraleImmune(sv));
        }

        // ── Shelter morale buff calculation ────────────────────────────

        [Fact]
        public void GetShelterMoraleBuff_MultipleCompassionSurvivors()
        {
            var sys = new MoralBranchingSystem();

            var sv1 = MakeSurvivor("sv_01");
            var sv2 = MakeSurvivor("sv_02");
            var sv3 = MakeSurvivor("sv_03"); // numbed — should not contribute

            foreach (var sv in new[] { sv1, sv2, sv3 })
                sys.Register(sv);

            // Branch sv1 and sv2 to compassion
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
            {
                sys.RegisterMoralChoice(sv1, true);
                sys.RegisterMoralChoice(sv2, true);
                sys.RegisterMoralChoice(sv3, false);
            }

            float buff = sys.GetShelterMoraleBuff();
            float expected = 2f * MoralBranchingSystem.BurdenedCompassionShelterMoraleBuff
                             * MoralBranchingSystem.CompassionBaseLevel;
            Assert.True(MathfCompat.Approximately(buff, expected),
                $"Expected ~{expected}, got {buff}");
        }

        [Fact]
        public void GetShelterMoraleBuff_DeadSurvivorsExcluded()
        {
            var sys = new MoralBranchingSystem();
            var sv1 = MakeSurvivor("sv_01");
            var sv2 = MakeSurvivor("sv_02");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
            {
                sys.RegisterMoralChoice(sv1, true);
                sys.RegisterMoralChoice(sv2, true);
            }

            sv2.IsAlive = false;
            var list = new List<MoralBranchState> { sv1, sv2 };

            float buff = sys.GetShelterMoraleBuff(list);
            float expected = MoralBranchingSystem.BurdenedCompassionShelterMoraleBuff
                             * MoralBranchingSystem.CompassionBaseLevel;
            Assert.True(MathfCompat.Approximately(buff, expected));
        }

        [Fact]
        public void GetShelterMoraleBuff_NullList_ReturnsZero()
        {
            var sys = new MoralBranchingSystem();
            Assert.Equal(0f, sys.GetShelterMoraleBuff((IReadOnlyList<MoralBranchState>)null));
        }

        // ── Events ─────────────────────────────────────────────────────

        [Fact]
        public void OnBranchDecided_FiresOnFifthChoice()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            MoralBranchState firedFor = null;
            MoralBranchDirection? firedDir = null;
            sys.OnBranchDecided += (s, d) => { firedFor = s; firedDir = d; };

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, true);

            Assert.Same(sv, firedFor);
            Assert.Equal(MoralBranchDirection.BurdenedCompassion, firedDir);
        }

        [Fact]
        public void OnBurdenedCompassionActivated_FiresOnHelp()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, true);

            float firedDelta = 0f;
            sys.OnBurdenedCompassionActivated += (s, d) => firedDelta = d;

            sys.OnHelpedOthers(sv);
            Assert.Equal(MoralBranchingSystem.BurdenedCompassionShelterMoraleBuff, firedDelta);
        }

        [Fact]
        public void OnNumbedComfortBlocked_Fires()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, false);

            bool fired = false;
            sys.OnNumbedComfortBlocked += s => fired = true;

            sys.IsComfortBlocked(sv);
            Assert.True(fired);
        }

        [Fact]
        public void OnStateChanged_FiresOnBranchAndEffects()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");

            int changeCount = 0;
            sys.OnStateChanged += () => changeCount++;

            // Branch decision fires once
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, true);
            Assert.Equal(1, changeCount);

            // OnHelpedOthers fires once
            sys.OnHelpedOthers(sv);
            Assert.Equal(2, changeCount);
        }

        // ── Save / Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestoreState_RoundTrip()
        {
            var sys = new MoralBranchingSystem();
            var sv1 = MakeSurvivor("sv_01");
            var sv2 = MakeSurvivor("sv_02");
            sys.Register(sv1);
            sys.Register(sv2);

            // Branch sv1 to compassion, leave sv2 neutral
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv1, true);

            var snapshot = sys.CaptureState();

            // Mutate originals to verify restore doesn't alias
            sv1.BranchDirection = MoralBranchDirection.Neutral;
            sv1.BurdenedCompassionLevel = 0f;

            // Restore into a fresh system
            var sys2 = new MoralBranchingSystem();
            sys2.RestoreState(snapshot);

            var restored = sys2.CaptureState();
            Assert.Equal(2, restored.Survivors.Count);

            // Find sv_01 in restored
            MoralBranchState r1 = null;
            for (int i = 0; i < restored.Survivors.Count; i++)
                if (restored.Survivors[i].SurvivorId == "sv_01") r1 = restored.Survivors[i];

            Assert.NotNull(r1);
            Assert.Equal(MoralBranchDirection.BurdenedCompassion, r1.BranchDirection);
            Assert.Equal(MoralBranchingSystem.CompassionBaseLevel, r1.BurdenedCompassionLevel);
            Assert.Equal(MoralBranchingSystem.ChoicesToBranch, r1.MoralChoiceCount);
        }

        [Fact]
        public void RestoreState_NullSnapshot_ClearsTracked()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");
            sys.Register(sv);

            sys.RestoreState(null);
            Assert.Equal(0f, sys.GetShelterMoraleBuff());
        }

        [Fact]
        public void RestoreStateInto_PatchesExistingObjects()
        {
            var sys = new MoralBranchingSystem();
            var sv1 = MakeSurvivor("sv_01");
            var sv2 = MakeSurvivor("sv_02");

            // Branch sv1
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv1, true);

            var snapshot = sys.CaptureState();

            // Now create fresh target objects
            var target1 = MakeSurvivor("sv_01");
            var target2 = MakeSurvivor("sv_02");
            var targets = new List<MoralBranchState> { target1, target2 };

            sys.RestoreStateInto(targets, snapshot);

            Assert.Equal(MoralBranchDirection.BurdenedCompassion, target1.BranchDirection);
            Assert.Equal(MoralBranchingSystem.ChoicesToBranch, target1.MoralChoiceCount);
            Assert.Equal(MoralBranchDirection.Neutral, target2.BranchDirection);
        }

        [Fact]
        public void CaptureState_DeepCopy_MutationsDontAffectSnapshot()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");
            sys.Register(sv);

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, true);

            var snapshot = sys.CaptureState();

            // Mutate original
            sv.BranchDirection = MoralBranchDirection.Neutral;
            sv.MoralChoiceCount = 0;

            // Snapshot should be unaffected
            Assert.Equal(MoralBranchDirection.BurdenedCompassion, snapshot.Survivors[0].BranchDirection);
            Assert.Equal(MoralBranchingSystem.ChoicesToBranch, snapshot.Survivors[0].MoralChoiceCount);
        }

        // ── Register / Unregister ──────────────────────────────────────

        [Fact]
        public void Register_SameSurvivorTwice_NotDuplicated()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");
            sys.Register(sv);
            sys.Register(sv);

            // If duplicated, GetShelterMoraleBuff would double-count
            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, true);

            float buff = sys.GetShelterMoraleBuff();
            float expected = MoralBranchingSystem.BurdenedCompassionShelterMoraleBuff
                             * MoralBranchingSystem.CompassionBaseLevel;
            Assert.True(MathfCompat.Approximately(buff, expected));
        }

        [Fact]
        public void Unregister_RemovesFromTracking()
        {
            var sys = new MoralBranchingSystem();
            var sv = MakeSurvivor("sv_01");
            sys.Register(sv);

            for (int i = 0; i < MoralBranchingSystem.ChoicesToBranch; i++)
                sys.RegisterMoralChoice(sv, true);

            sys.Unregister(sv);
            Assert.Equal(0f, sys.GetShelterMoraleBuff());
        }
    }
}
