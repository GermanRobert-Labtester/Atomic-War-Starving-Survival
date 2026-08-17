using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    public class CombatHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemo_Passes()
        {
            var report = CombatHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 15, "demo covers a broad surface");
        }

        [Fact]
        public void Demo_ProducesFinalStateAndSnapshot()
        {
            var report = CombatHeadlessDemo.Run();
            Assert.NotNull(report.FinalState);
            Assert.True(report.FinalState.Resolved, "demo resolves the encounter");
            Assert.NotNull(report.Snapshot);
            Assert.True(report.Snapshot.Combatants.Count > 0);
        }

        [Fact]
        public void DemoSnapshot_IsActivEOnlyInCombat()
        {
            var report = CombatHeadlessDemo.Run();
            // After resolution IsActive is false.
            Assert.False(report.Snapshot.IsActive);
        }
    }

    public class CombatPerkTests
    {
        [Fact]
        public void Perks_GrantAtThresholds()
        {
            var perks = new CombatPerks(1);
            for (int i = 0; i < 3; i++)
                perks.RecordWeaponJamSurvived("sv1");
            Assert.True(perks.Has("sv1", CombatPerks.TapRackBangId));
            Assert.Equal(CombatPerks.TapRackBangJamClearTicks, perks.GetJamClearTicks("sv1"));
        }

        [Fact]
        public void TapRackBang_ReducesJamClearTicks()
        {
            var perks = new CombatPerks(1);
            Assert.Equal(CombatPerks.DefaultJamClearTicks, perks.GetJamClearTicks("sv2"));
            perks.Grant("sv2", CombatPerks.TapRackBangId);
            Assert.Equal(CombatPerks.TapRackBangJamClearTicks, perks.GetJamClearTicks("sv2"));
        }

        [Fact]
        public void ColdBore_AddsFirstShotCrit()
        {
            var perks = new CombatPerks(1);
            Assert.Equal(0f, perks.GetFirstShotCritBonus("sv3"));
            perks.Grant("sv3", CombatPerks.ColdBoreId);
            Assert.Equal(CombatPerks.ColdBoreFirstShotCritBonus, perks.GetFirstShotCritBonus("sv3"));
        }

        [Fact]
        public void Desensitized_ImmuneToKillMorale()
        {
            var perks = new CombatPerks(1);
            Assert.False(perks.IsImmuneToKillMorale("sv4"));
            Assert.Equal(CombatPerks.HumanKillMoralePenalty, perks.ApplyHumanKillMorale("sv4"));
            perks.Grant("sv4", CombatPerks.DesensitizedId);
            Assert.True(perks.IsImmuneToKillMorale("sv4"));
            Assert.Equal(0f, perks.ApplyHumanKillMorale("sv4"));
        }

        [Fact]
        public void PerkCounters_SurviveSaveRestore()
        {
            var perks = new CombatPerks(1);
            perks.RecordHumanKill("sv5");
            perks.RecordHumanKill("sv5");
            perks.RecordHumanKill("sv5");
            perks.RecordHumanKill("sv5");
            perks.RecordHumanKill("sv5");
            Assert.True(perks.Has("sv5", CombatPerks.DesensitizedId));

            var save = perks.CaptureState();
            var restored = new CombatPerks(1);
            restored.RestoreState(save);
            Assert.Equal(5, restored.GetEntry("sv5").HumanKills);
        }
    }
}
