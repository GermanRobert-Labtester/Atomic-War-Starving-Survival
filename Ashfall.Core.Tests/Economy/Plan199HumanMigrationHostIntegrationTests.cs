// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan199HumanMigrationHostIntegrationTests
    {
        [Fact]
        public void Census_ReflectsTrackedRegionsAndWeights()
        {
            var engine = new SeasonalHumanMigrationEngine(knownRegions: new[] { "region_north", "region_south" });
            var census = engine.GetCensus();

            Assert.Equal(2, census.TotalTrackedRegions);
            Assert.Equal(200, census.TotalPopulationWeight);
            Assert.Equal(0, census.LastTransitionDay);
        }

        [Fact]
        public void InitialWeights_MatchDefault100ForKnownRegions()
        {
            var engine = new SeasonalHumanMigrationEngine(knownRegions: new[] { "settlement", "iron_basin", "ash_flats" });

            Assert.Equal(100, engine.GetRegionPopulationWeight("settlement"));
            Assert.Equal(100, engine.GetRegionPopulationWeight("iron_basin"));
            Assert.Equal(100, engine.GetRegionPopulationWeight("ash_flats"));
            Assert.Equal(100, engine.GetRegionPopulationWeight("unknown_region"));
        }

        [Fact]
        public void PhaseTransition_AppliesScheduledDeltas()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                schema_version = 1,
                DwellDays = 10,
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "faction_compact",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "deep_winter", RegionId = "settlement", PopulationDelta = 30 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "settlement" });
            bool applied = engine.TickDay(currentDay: 1, currentSeasonPhase: "deep_winter");

            Assert.True(applied);
            Assert.Equal(130, engine.GetRegionPopulationWeight("settlement"));
            Assert.Equal("deep_winter", engine.LastAppliedPhase);
            Assert.Equal(1, engine.LastTransitionDay);
        }

        [Fact]
        public void SamePhase_IsIdempotent()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "faction_scale",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "thaw", RegionId = "ash_flats", PopulationDelta = 20 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "ash_flats" });
            Assert.True(engine.TickDay(1, "thaw"));
            Assert.Equal(120, engine.GetRegionPopulationWeight("ash_flats"));

            // Same phase on day 2 is a no-op
            Assert.False(engine.TickDay(2, "thaw"));
            Assert.Equal(120, engine.GetRegionPopulationWeight("ash_flats"));
        }

        [Fact]
        public void PrematurePhaseTransition_IsSuppressedByDwellHysteresis()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                DwellDays = 10,
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "f1",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "phase_a", RegionId = "r1", PopulationDelta = 10 },
                            new SeasonalMigrationEntry { Phase = "phase_b", RegionId = "r1", PopulationDelta = 20 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "r1" });
            Assert.True(engine.TickDay(1, "phase_a"));
            Assert.Equal(110, engine.GetRegionPopulationWeight("r1"));

            // Day 5 is only 4 days elapsed < 10 dwell days -> suppressed
            Assert.False(engine.TickDay(5, "phase_b"));
            Assert.Equal(110, engine.GetRegionPopulationWeight("r1"));
            Assert.Equal("phase_a", engine.LastAppliedPhase);
        }

        [Fact]
        public void MaturePhaseTransition_IsAppliedAfterDwellDays()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                DwellDays = 10,
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "f1",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "winter", RegionId = "r1", PopulationDelta = 20 },
                            new SeasonalMigrationEntry { Phase = "spring", RegionId = "r1", PopulationDelta = -15 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "r1" });
            Assert.True(engine.TickDay(1, "winter"));
            Assert.Equal(120, engine.GetRegionPopulationWeight("r1"));

            // Day 12 is 11 days elapsed >= 10 dwell days -> applied
            Assert.True(engine.TickDay(12, "spring"));
            Assert.Equal(105, engine.GetRegionPopulationWeight("r1"));
            Assert.Equal("spring", engine.LastAppliedPhase);
            Assert.Equal(12, engine.LastTransitionDay);
        }

        [Fact]
        public void MinimumWeight_IsClampedAt10()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                DwellDays = 1,
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "f_drain",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "famine", RegionId = "r_depleted", PopulationDelta = -200 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "r_depleted" });
            engine.TickDay(1, "famine");

            Assert.Equal(10, engine.GetRegionPopulationWeight("r_depleted"));
        }

        [Fact]
        public void SaveAndRestore_PreservesWeightsAndAppliedTransitions()
        {
            var catalog = new SeasonalMigrationCatalog
            {
                DwellDays = 10,
                Factions = new List<FactionMigrationSchedule>
                {
                    new FactionMigrationSchedule
                    {
                        FactionId = "f1",
                        Schedule = new List<SeasonalMigrationEntry>
                        {
                            new SeasonalMigrationEntry { Phase = "dry_heat", RegionId = "iron_basin", PopulationDelta = 40 }
                        }
                    }
                }
            };

            var engine = new SeasonalHumanMigrationEngine(catalog, new[] { "iron_basin" });
            engine.TickDay(10, "dry_heat");

            var state = engine.CaptureState();
            Assert.Equal("dry_heat", state.LastAppliedPhase);
            Assert.Equal(10, state.LastTransitionDay);

            var restored = new SeasonalHumanMigrationEngine(catalog);
            restored.RestoreState(state);

            Assert.Equal(140, restored.GetRegionPopulationWeight("iron_basin"));
            Assert.Equal("dry_heat", restored.LastAppliedPhase);
            Assert.Equal(10, restored.LastTransitionDay);

            // Re-ticking same phase is suppressed
            Assert.False(restored.TickDay(15, "dry_heat"));
        }

        [Fact]
        public void CatalogLoader_LoadsAuthoredJsonCorrectly()
        {
            string json = @"{
  ""schema_version"": 1,
  ""dwell_days"": 10,
  ""factions"": [
    {
      ""faction_id"": ""faction_the_compact"",
      ""schedule"": [
        { ""phase"": ""deep_winter"", ""region_id"": ""settlement"", ""population_delta"": 25 }
      ]
    }
  ]
}";
            var result = SeasonalMigrationCatalogLoader.LoadFromJson(json);
            Assert.True(result.Success);
            Assert.NotNull(result.Catalog);
            Assert.Equal(10, result.Catalog.DwellDays);
            Assert.Single(result.Catalog.Factions);
            Assert.Equal("faction_the_compact", result.Catalog.Factions[0].FactionId);
            Assert.Single(result.Catalog.Factions[0].Schedule);
            Assert.Equal(25, result.Catalog.Factions[0].Schedule[0].PopulationDelta);
        }
    }
}
