// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Factions;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MusterWarfareTests
    {
        private static List<MusterSoldier> MakeTestRoster()
        {
            return new List<MusterSoldier>
            {
                new MusterSoldier { SurvivorId = "survivor_daniels", Role = MusterSoldierRole.Infantry, CombatStrength = 15f },
                new MusterSoldier { SurvivorId = "survivor_vask", Role = MusterSoldierRole.Scout, CombatStrength = 12f },
                new MusterSoldier { SurvivorId = "survivor_nora", Role = MusterSoldierRole.Heavy, CombatStrength = 20f },
                new MusterSoldier { SurvivorId = "survivor_bram", Role = MusterSoldierRole.Medic, CombatStrength = 10f }
            };
        }

        private static MusterSupplyReservation MakeTestSupplies()
        {
            return new MusterSupplyReservation
            {
                FoodRations = 30,
                CleanWaterUnits = 30,
                AmmoUnits = 40,
                MedicalUnits = 10,
                FuelUnits = 15
            };
        }

        [Fact]
        public void CanMobilize_ValidatesEmptyAndDuplicateSurvivors()
        {
            var engine = new MusterWarfareEngine();

            // Empty roster fails
            bool ok = engine.CanMobilize("branch_mil_1_loyal_soldier", null, new List<MusterSoldier>(), MakeTestSupplies(), out var reason);
            Assert.False(ok);
            Assert.Equal("empty_roster_no_soldiers", reason);

            // Duplicate survivor fails
            var dupRoster = new List<MusterSoldier>
            {
                new MusterSoldier { SurvivorId = "survivor_daniels" },
                new MusterSoldier { SurvivorId = "survivor_daniels" }
            };
            ok = engine.CanMobilize("branch_mil_1_loyal_soldier", null, dupRoster, MakeTestSupplies(), out reason);
            Assert.False(ok);
            Assert.Contains("duplicate_survivor", reason);
        }

        [Fact]
        public void CanMobilize_BlocksRivalFactionWhenCommitted()
        {
            var milCatalog = new MilitaryBranchCatalog();
            milCatalog.Register(new MilitaryBranchEntry { id = "branch_mil_1_loyal_soldier" });
            var rebCatalog = new RebelBranchCatalog();
            rebCatalog.Register(new RebelBranchEntry { id = "branch_rebel_1_insurgent" });

            var coordinator = new FactionBranchCoordinator(milCatalog, rebCatalog);
            // Simulate military commitment
            coordinator.Military.State.branch.committed = true;
            coordinator.Military.State.branch.branchId = "branch_mil_1_loyal_soldier";

            var engine = new MusterWarfareEngine();
            // Attempting to muster rebel branch fails due to exclusivity
            bool ok = engine.CanMobilize("branch_rebel_1_insurgent", coordinator, MakeTestRoster(), MakeTestSupplies(), out var reason);
            Assert.False(ok);
            Assert.Contains("already committed to Military", reason);
        }

        [Fact]
        public void ConflictLifecycle_HappyPath_IdleToAftermathToRecovering()
        {
            var engine = new MusterWarfareEngine();
            var roster = MakeTestRoster();
            var supplies = MakeTestSupplies();

            // 1. Mobilize
            var res = engine.Mobilize("branch_mil_1_loyal_soldier", roster, supplies, "warlord_doctrine_procedure", "sector_4_depot");
            Assert.True(res.IsSuccess);
            Assert.Equal(MusterConflictPhase.Mobilizing, engine.Phase);

            // 2. SetReady
            res = engine.SetReady();
            Assert.True(res.IsSuccess);
            Assert.Equal(MusterConflictPhase.Ready, engine.Phase);

            // 3. Deploy
            res = engine.Deploy();
            Assert.True(res.IsSuccess);
            Assert.Equal(MusterConflictPhase.Deployed, engine.Phase);

            // 4. Engage
            res = engine.Engage();
            Assert.True(res.IsSuccess);
            Assert.Equal(MusterConflictPhase.Engaged, engine.Phase);

            // 5. Resolve
            var factionWar = new FactionWarSystem();
            int initialTension = factionWar.WarTension;
            var outcome = engine.ResolveEngagement(42, 10, oppositionPower: 30f, factionWar: factionWar);

            Assert.Equal(MusterConflictPhase.Aftermath, engine.Phase);
            Assert.NotNull(outcome);
            Assert.True(outcome.Outcome == "Victory" || outcome.Outcome == "Stalemate" || outcome.Outcome == "Defeat");
            Assert.True(factionWar.WarTension >= initialTension); // Tension increased or held

            // 6. Complete Aftermath -> Recovering
            res = engine.CompleteAftermath(recoveryDays: 2);
            Assert.True(res.IsSuccess);
            Assert.Equal(MusterConflictPhase.Recovering, engine.Phase);

            // 7. Tick recovery
            engine.TickDay(11);
            Assert.Equal(MusterConflictPhase.Recovering, engine.Phase);
            engine.TickDay(12);
            Assert.Equal(MusterConflictPhase.Idle, engine.Phase);
        }

        [Fact]
        public void CancelMobilization_RestoresToIdle()
        {
            var engine = new MusterWarfareEngine();
            engine.Mobilize("branch_mil_1_loyal_soldier", MakeTestRoster(), MakeTestSupplies(), "warlord_doctrine_toll", "sector_4");
            Assert.Equal(MusterConflictPhase.Mobilizing, engine.Phase);

            var cancel = engine.CancelMobilization();
            Assert.True(cancel.IsSuccess);
            Assert.Equal(MusterConflictPhase.Cancelled, engine.Phase);
            Assert.Empty(engine.ActiveRoster);
        }

        [Fact]
        public void SaveRestore_RoundtripPreservesWarfareState()
        {
            var system = new MusterSystem();
            system.Warfare.Mobilize("branch_mil_1_loyal_soldier", MakeTestRoster(), MakeTestSupplies(), "warlord_doctrine_annexation", "sector_7");
            system.Warfare.SetReady();

            var saved = system.CaptureState();

            var restoredSystem = new MusterSystem();
            restoredSystem.RestoreState(saved);

            Assert.Equal(MusterConflictPhase.Ready, restoredSystem.Warfare.Phase);
            Assert.Equal("branch_mil_1_loyal_soldier", restoredSystem.Warfare.State.CommittedBranchId);
            Assert.Equal("warlord_doctrine_annexation", restoredSystem.Warfare.State.ActiveDoctrineId);
            Assert.Equal(4, restoredSystem.Warfare.ActiveRoster.Count);
        }
    }
}
