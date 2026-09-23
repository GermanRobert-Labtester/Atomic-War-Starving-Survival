// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class SeasonalHumanMigrationEngineTests
    {
        [Fact]
        public void InitialWeights_DefaultTo100()
        {
            var engine = new SeasonalHumanMigrationEngine(knownRegions: new[] { "region_valley", "region_hills" });

            Assert.Equal(100, engine.GetRegionPopulationWeight("region_valley"));
            Assert.Equal(100, engine.GetRegionPopulationWeight("region_hills"));
            Assert.Equal(100, engine.GetRegionPopulationWeight("region_unregistered"));
            Assert.Equal(10, engine.DwellDays);
        }

        [Fact]
        public void TickDay_AppliesScheduledDeltas_WhenPhaseTransitions()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                schema_version = 1,
                DwellDays = 10,
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "faction_lowlanders",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "deep_winter", RegionId = "region_valley", PopulationDelta = 25 },
                            new SeasonalMigrationEntry { Phase = "thaw", RegionId = "region_valley", PopulationDelta = -15 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "region_valley" });

            // Day 1: Deep winter arrives
            bool applied = engine.TickDay(currentDay: 1, currentSeasonPhase: "deep_winter");
            Assert.True(applied);
            Assert.Equal(125, engine.GetRegionPopulationWeight("region_valley"));

            // Day 2: Same phase is a no-op
            bool secondTick = engine.TickDay(currentDay: 2, currentSeasonPhase: "deep_winter");
            Assert.False(secondTick);
            Assert.Equal(125, engine.GetRegionPopulationWeight("region_valley"));

            // Day 5: Phase changes to thaw, but within dwell days (5 - 1 = 4 < 10) -> suppressed
            bool premature = engine.TickDay(currentDay: 5, currentSeasonPhase: "thaw");
            Assert.False(premature);
            Assert.Equal(125, engine.GetRegionPopulationWeight("region_valley"));

            // Day 12: Phase changes to thaw, after dwell days (12 - 1 = 11 >= 10) -> applied
            bool mature = engine.TickDay(currentDay: 12, currentSeasonPhase: "thaw");
            Assert.True(mature);
            Assert.Equal(110, engine.GetRegionPopulationWeight("region_valley"));
        }

        [Fact]
        public void SaveAndRestore_PreservesWeightsAndTransitions()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "faction_test",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "summer", RegionId = "region_coast", PopulationDelta = 50 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "region_coast" });
            engine.TickDay(currentDay: 10, currentSeasonPhase: "summer");
            Assert.Equal(150, engine.GetRegionPopulationWeight("region_coast"));

            var state = engine.CaptureState();
            var restored = new SeasonalHumanMigrationEngine(catalog);
            restored.RestoreState(state);

            Assert.Equal(150, restored.GetRegionPopulationWeight("region_coast"));
            Assert.Equal("summer", restored.LastAppliedPhase);
            Assert.Equal(10, restored.LastTransitionDay);

            // Re-ticking same phase on restored engine is suppressed
            Assert.False(restored.TickDay(currentDay: 15, currentSeasonPhase: "summer"));
        }
    }
}
