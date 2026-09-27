// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: XP-08-F6 — seasonal human migration consequence engine.
// Focused contract tests over the sealed Core engine against the live migration
// engine it consumes.

using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class PlanXp08F6MigrationConsequenceHostIntegrationTests
    {
        private static readonly string[] Regions = { "settlement", "iron_basin", "ash_flats", "deep_coast", "industrial_belt" };

        private static MigrationConsequenceEngine BuildEngine(out SeasonalHumanMigrationEngine migration)
        {
            migration = new SeasonalHumanMigrationEngine(BuildCatalog(), Regions);
            return new MigrationConsequenceEngine(migration);
        }

        private static SeasonalMigrationCatalog BuildCatalog() => new()
        {
            schema_version = 1,
            DwellDays = 1,
            Factions =
            {
                new FactionMigrationSchedule
                {
                    FactionId = "test_faction",
                    Schedule =
                    {
                        new SeasonalMigrationEntry { Phase = "deep_winter", RegionId = "ash_flats", PopulationDelta = 40 },
                        new SeasonalMigrationEntry { Phase = "thaw", RegionId = "ash_flats", PopulationDelta = -30 }
                    }
                }
            }
        };

        private static void MoveWeightOffBaseline(SeasonalHumanMigrationEngine migration)
        {
            migration.TickDay(1, "deep_winter");
            migration.TickDay(10, "thaw");
        }

        [Fact]
        public void PopulationWeight_IsReadFromTheLiveMigrationEngine()
        {
            var engine = BuildEngine(out var migration);
            migration.TickDay(30, "deep_winter");

            foreach (string region in Regions)
                Assert.Equal(migration.GetRegionPopulationWeight(region), engine.GetRegionPopulationWeight(region));
        }

        [Fact]
        public void EssentialDemand_TracksPopulationWeight_WithAnAuthoredFloor()
        {
            var engine = BuildEngine(out _);

            int food = engine.GetMarketDemandMultiplierPermille("settlement", "food");
            int fuel = engine.GetMarketDemandMultiplierPermille("settlement", "fuel");
            int medicine = engine.GetMarketDemandMultiplierPermille("settlement", "medicine");

            Assert.True(food >= 200);
            Assert.True(fuel >= 200);
            Assert.True(medicine >= 200);
        }

        [Fact]
        public void CategoryClasses_Differ_FoodLuxuryLaborAndGeneralGoods()
        {
            var engine = BuildEngine(out var migration);
            MoveWeightOffBaseline(migration);
            const string region = "ash_flats";

            int food = engine.GetMarketDemandMultiplierPermille(region, "food");
            int luxury = engine.GetMarketDemandMultiplierPermille(region, "luxury");
            int labor = engine.GetMarketDemandMultiplierPermille(region, "labor");
            int general = engine.GetMarketDemandMultiplierPermille(region, "tools");

            Assert.NotEqual(food, luxury);
            Assert.NotEqual(food, labor);
            Assert.NotEqual(food, general);
            Assert.True(labor >= 400);
            Assert.True(general >= 500);
        }

        [Fact]
        public void LaborPoolAndFrictionMultipliers_AreBounded()
        {
            var engine = BuildEngine(out _);

            foreach (string region in Regions)
            {
                Assert.InRange(engine.GetLaborPoolSizeMultiplierPermille(region), 100, 3000);
                Assert.True(engine.GetTerritorialFrictionMultiplierPermille(region) >= 400);
            }
        }

        [Fact]
        public void CaravanDemandPriority_UsesOnlyAuthoredBands()
        {
            var engine = BuildEngine(out _);

            foreach (string region in Regions)
            {
                string priority = engine.GetCaravanDemandPriority(region);
                Assert.Contains(priority, new[] { "food_and_fuel", "defense_and_labor", "balanced_trade" });
            }
        }

        [Fact]
        public void PhaseConsequence_IsExactlyOnce_PerRegionPhaseAndDay()
        {
            var engine = BuildEngine(out _);

            Assert.True(engine.TryApplyPhaseConsequence(10, "thaw", "ash_flats"));
            Assert.False(engine.TryApplyPhaseConsequence(10, "thaw", "ash_flats"));
            // A different day or region is a distinct consequence.
            Assert.True(engine.TryApplyPhaseConsequence(11, "thaw", "ash_flats"));
            Assert.True(engine.TryApplyPhaseConsequence(10, "thaw", "iron_basin"));
        }

        [Fact]
        public void ExactlyOnceLedger_SurvivesASaveRoundTrip()
        {
            var engine = BuildEngine(out _);
            engine.TryApplyPhaseConsequence(10, "thaw", "ash_flats");
            var captured = engine.CaptureState();

            string json = new SystemTextJsonSerializer().Serialize(captured);
            var restored = new SystemTextJsonSerializer().Deserialize<MigrationConsequenceSaveState>(json);
            Assert.NotNull(restored);

            var reload = BuildEngine(out _);
            reload.RestoreState(restored);

            Assert.False(reload.TryApplyPhaseConsequence(10, "thaw", "ash_flats"));
        }

        [Fact]
        public void BlankPhaseOrRegion_IsRefused_WithoutRecordingAConsequence()
        {
            var engine = BuildEngine(out _);

            Assert.False(engine.TryApplyPhaseConsequence(10, "  ", "ash_flats"));
            Assert.False(engine.TryApplyPhaseConsequence(10, "thaw", ""));
            Assert.Empty(engine.AppliedConsequenceKeys);
        }

        [Fact]
        public void NullMigrationEngine_IsRejected()
        {
            Assert.Throws<System.ArgumentNullException>(() => new MigrationConsequenceEngine(null!));
        }
    }
}
