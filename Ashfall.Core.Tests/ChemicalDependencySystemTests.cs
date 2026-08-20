using Ashfall.Core;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ChemicalDependencySystemTests
    {
        [Fact]
        public void Consumption_FormsDependencyAtThreshold()
        {
            var sys = new ChemicalDependencySystem();
            int formed = 0;
            sys.OnDependencyFormed += (sv, item) => formed++;
            // Unity parity: first dose creates the entry at 0.15; the second
            // dose reaches 0.30 >= threshold and fires the formation event.
            sys.OnSubstanceConsumed("sv_mae", "opioid_painkillers", ChemicalDependencyKind.Opioid);
            Assert.Equal(0, formed);
            sys.OnSubstanceConsumed("sv_mae", "opioid_painkillers", ChemicalDependencyKind.Opioid);
            Assert.Equal(1, formed);
            Assert.True(sys.DependencyLevel("sv_mae", "opioid_painkillers") >= ChemicalDependencySystem.DependencyThreshold);
        }

        [Fact]
        public void Consumption_FirstDoseCreatesEntry()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed("sv_mae", "vodka", ChemicalDependencyKind.Alcohol);
            Assert.Equal(ChemicalDependencySystem.DependencyIncreasePerDose,
                sys.DependencyLevel("sv_mae", "vodka"));
        }

        [Fact]
        public void Consumption_UnknownSurvivorIgnored()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed(null, "vodka", ChemicalDependencyKind.Alcohol);
            sys.OnSubstanceConsumed("", "vodka", ChemicalDependencyKind.Alcohol);
            sys.OnSubstanceConsumed("sv_mae", "", ChemicalDependencyKind.Alcohol);
            Assert.Empty(sys.DependenciesFor("sv_mae"));
        }

        [Fact]
        public void ManagedDetox_CompletesAfterThresholdHours()
        {
            var sys = new ChemicalDependencySystem();
            for (int i = 0; i < 3; i++)
                sys.OnSubstanceConsumed("sv_mae", "opioid_painkillers", ChemicalDependencyKind.Opioid);
            Assert.True(sys.BeginManagedDetox("sv_mae", "opioid_painkillers"));
            int completed = 0, failed = 0;
            sys.OnDetoxCompleted += (sv, item) => completed++;
            sys.OnDetoxFailed += (sv, item) => failed++;

            sys.TickHours("sv_mae", 24f);
            Assert.True(sys.HasActiveWithdrawal("sv_mae"));
            sys.TickHours("sv_mae", 96f); // total 120 >= 96 success threshold
            Assert.Equal(1, completed);
            Assert.Equal(0, failed);
            Assert.False(sys.HasActiveWithdrawal("sv_mae"));
            Assert.Equal(0f, sys.DependencyLevel("sv_mae", "opioid_painkillers"));
        }

        [Fact]
        public void ManagedDetox_BelowThresholdRefused()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed("sv_mae", "vodka", ChemicalDependencyKind.Alcohol);
            Assert.False(sys.BeginManagedDetox("sv_mae", "vodka"));
        }

        [Fact]
        public void ColdTurkey_AppliesPenaltiesAndCompletes()
        {
            var sys = new ChemicalDependencySystem();
            for (int i = 0; i < 4; i++)
                sys.OnSubstanceConsumed("sv_ged", "vodka", ChemicalDependencyKind.Alcohol);
            Assert.True(sys.BeginColdTurkey("sv_ged", "vodka"));

            float crafting = 0f, morale = 0f;
            sys.OnCraftingPenaltyChanged += (sv, f) => crafting = f;
            sys.OnMoraleDrainRequested += (sv, m) => morale += m;

            sys.TickHours("sv_ged", 24f);
            Assert.Equal(ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty, crafting);
            Assert.True(morale > 0f);

            int completed = 0;
            sys.OnDetoxCompleted += (sv, item) => completed++;
            sys.TickHours("sv_ged", 72f); // 24 + 72 = 96 >= 72 duration
            Assert.Equal(1, completed);
            Assert.False(sys.HasActiveWithdrawal("sv_ged"));
        }

        [Fact]
        public void CleanDecay_RemovesDependency()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed("sv_wren", "sedatives", ChemicalDependencyKind.Sedative);
            int completed = 0;
            sys.OnDetoxCompleted += (sv, item) => completed++;
            sys.TickHours("sv_wren", 24f * 10f); // 10 clean days: 0.15 - 0.5 <= 0
            Assert.Equal(1, completed);
            Assert.Empty(sys.DependenciesFor("sv_wren"));
        }

        [Fact]
        public void Severity_ScalesMoraleDrainByKind()
        {
            var sys = new ChemicalDependencySystem();
            for (int i = 0; i < 4; i++) sys.OnSubstanceConsumed("sv_a", "opioid", ChemicalDependencyKind.Opioid);
            for (int i = 0; i < 4; i++) sys.OnSubstanceConsumed("sv_b", "sedative", ChemicalDependencyKind.Sedative);
            sys.BeginColdTurkey("sv_a", "opioid");
            sys.BeginColdTurkey("sv_b", "sedative");

            float opioidDrain = 0f, sedativeDrain = 0f;
            sys.OnMoraleDrainRequested += (sv, m) =>
            {
                if (sv == "sv_a") opioidDrain += m;
                if (sv == "sv_b") sedativeDrain += m;
            };
            sys.TickHours("sv_a", 1f);
            sys.TickHours("sv_b", 1f);
            Assert.True(opioidDrain > sedativeDrain); // 0.9 vs 0.5 severity
        }

        [Fact]
        public void ProgramSwitch_ColdTurkeyToManagedUsesManagedProfile()
        {
            var sys = new ChemicalDependencySystem();
            for (int i = 0; i < 4; i++)
                sys.OnSubstanceConsumed("sv_mae", "opioid_painkillers", ChemicalDependencyKind.Opioid);
            Assert.True(sys.BeginColdTurkey("sv_mae", "opioid_painkillers"));
            Assert.True(sys.BeginManagedDetox("sv_mae", "opioid_painkillers"));

            float morale = 0f, crafting = 0f;
            sys.OnMoraleDrainRequested += (sv, m) => morale += m;
            sys.OnCraftingPenaltyChanged += (sv, f) => crafting = f;
            sys.TickHours("sv_mae", 24f);

            // Managed profile: 1/hr drain (not 3/hr), no tremor penalties, completes at 96h.
            Assert.Equal(0f, crafting);
            Assert.True(morale <= ChemicalDependencySystem.ManagedDetoxMoraleDrainPerHour * 24f * 0.95f + 0.01f);
            Assert.False(sys.DependenciesFor("sv_mae")[0].inColdTurkey);
            Assert.True(sys.DependenciesFor("sv_mae")[0].inManagedDetox);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed("sv_mae", "vodka", ChemicalDependencyKind.Alcohol);
            var snapshot = sys.CaptureState();
            snapshot.survivors[0].dependencies[0].dependencyLevel = 99f;
            Assert.Equal(ChemicalDependencySystem.DependencyIncreasePerDose,
                sys.DependencyLevel("sv_mae", "vodka"));
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed("sv_zed", "a", ChemicalDependencyKind.Alcohol);
            sys.OnSubstanceConsumed("sv_a", "b", ChemicalDependencyKind.Opioid);
            var snapshot = sys.CaptureState();
            Assert.Equal("sv_a", snapshot.survivors[0].survivorId);
            Assert.Equal("sv_zed", snapshot.survivors[1].survivorId);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = new ChemicalDependencySystem();
            for (int i = 0; i < 4; i++) sys.OnSubstanceConsumed("sv_mae", "opioid", ChemicalDependencyKind.Opioid);
            sys.BeginManagedDetox("sv_mae", "opioid");
            sys.TickHours("sv_mae", 24f);

            var restored = new ChemicalDependencySystem();
            restored.RestoreState(sys.CaptureState());

            Assert.True(restored.HasActiveWithdrawal("sv_mae"));
            Assert.Equal(sys.DependencyLevel("sv_mae", "opioid"), restored.DependencyLevel("sv_mae", "opioid"));
            Assert.True(restored.DependenciesFor("sv_mae")[0].inManagedDetox);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = new ChemicalDependencySystem();
            sys.OnSubstanceConsumed("sv_a", "x", ChemicalDependencyKind.Opioid);
            sys.OnSubstanceConsumed("sv_b", "y", ChemicalDependencyKind.Alcohol);
            sys.TickHours("sv_a", 12f);
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new ChemicalDependencySystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }
    }
}
