using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 28 — seasonal ecology calendar: archetype table, Plan 19 season
    /// binding, hunger/abundance factors, water-bound fish runs, notices,
    /// legacy neutrality, and save round-trip.
    /// </summary>
    public sealed class WildlifeSeasonalCalendarTests : CatalogTestBase
    {
        private const int MasterSeed = 1986;

        private static EvolvingWorldSeedContainer? LoadSeeds() =>
            EvolvingWorldCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        private static SeasonProfileDef? LoadProfile() =>
            WeatherProfileLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        private static WildlifeMigrationSystem SeededWildlife(EvolvingWorldSeedContainer? seeds, SeasonProfileDef? profile)
        {
            var wild = new WildlifeMigrationSystem();
            if (profile != null) wild.BindSeasonProfile(profile);
            EvolvingWorldSeeder.Seed(new LocationEvolutionSystem(), wild, new LandmarkDegradationSystem(), seeds);
            return wild;
        }

        private static ISeededRng DayFork(int day, int actionIndex, int master = MasterSeed) =>
            CampaignRngStream.DeriveSeed(master, "world_evolution", 1, day, actionIndex) is int s
                ? new SeededRng(s != 0 ? s : master)
                : new SeededRng(master);

        private static SeasonWindowDef Window(string id, int startDay = 0) => new SeasonWindowDef
        {
            id = id,
            displayName = id,
            startDay = startDay
        };

        // ── Archetype table ─────────────────────────────────────────

        [Fact]
        public void ArchetypeTable_CoversAllSeededSpecies_WithDistinctRoles()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var species = seeds!.packs.Select(p => p.species_id).Distinct().ToList();
            Assert.Equal(12, species.Count);

            Assert.Equal(MigrationArchetype.Resident, WildlifeSeasonalCalendar.ArchetypeOf("species_rad_dog"));
            Assert.Equal(MigrationArchetype.Resident, WildlifeSeasonalCalendar.ArchetypeOf("species_wolf"));
            Assert.Equal(MigrationArchetype.Resident, WildlifeSeasonalCalendar.ArchetypeOf("species_dust_lynx"));
            Assert.Equal(MigrationArchetype.HerdGrazer, WildlifeSeasonalCalendar.ArchetypeOf("species_feral_goat"));
            Assert.Equal(MigrationArchetype.BurrowSwarm, WildlifeSeasonalCalendar.ArchetypeOf("species_blight_rat"));
            Assert.Equal(MigrationArchetype.BurrowSwarm, WildlifeSeasonalCalendar.ArchetypeOf("species_cotton_hare"));
            Assert.Equal(MigrationArchetype.Sounder, WildlifeSeasonalCalendar.ArchetypeOf("species_ash_boar"));
            Assert.Equal(MigrationArchetype.PassageFlock, WildlifeSeasonalCalendar.ArchetypeOf("species_iron_crow"));
            Assert.Equal(MigrationArchetype.PassageFlock, WildlifeSeasonalCalendar.ArchetypeOf("species_ash_gull"));
            Assert.Equal(MigrationArchetype.CoastalRunner, WildlifeSeasonalCalendar.ArchetypeOf("species_gray_heron"));
            Assert.Equal(MigrationArchetype.CoastalRunner, WildlifeSeasonalCalendar.ArchetypeOf("species_mirror_carp"));
            Assert.Equal(MigrationArchetype.SwarmBlight, WildlifeSeasonalCalendar.ArchetypeOf("species_ghost_moth"));

            Assert.Equal(MigrationArchetype.Resident, WildlifeSeasonalCalendar.ArchetypeOf("species_unknown"));
            Assert.Equal(MigrationArchetype.Resident, WildlifeSeasonalCalendar.ArchetypeOf(""));
        }

        [Fact]
        public void SeasonWindowForDay_MatchesTheWeatherAuthority()
        {
            var profile = LoadProfile();
            Assert.NotNull(profile);

            Assert.Equal("window_ashfall", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 0)!.id);
            Assert.Equal("window_ashfall", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 59)!.id);
            Assert.Equal("window_deep_freeze", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 60)!.id);
            Assert.Equal("window_thaw", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 120)!.id);
            Assert.Equal("window_black_bloom", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 200)!.id);
            Assert.Equal("window_high_cold", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 240)!.id);
            Assert.Equal("window_the_turning", WildlifeSeasonalCalendar.SeasonWindowForDay(profile, 500)!.id);

            var weather = new WeatherSystem();
            weather.BindProfile(profile, 7);
            for (int day = 0; day <= 500; day += 3)
                Assert.Equal(weather.GetSeasonForDay(day).id,
                    WildlifeSeasonalCalendar.SeasonWindowForDay(profile, day)!.id);
        }

        [Fact]
        public void Factors_AreBounded_ForEveryArchetypeAndWindow()
        {
            var windows = new List<SeasonWindowDef?> { null };
            var profile = LoadProfile();
            if (profile != null)
                foreach (var w in profile.seasons)
                    windows.Add(w);

            foreach (var window in windows)
                foreach (MigrationArchetype a in Enum.GetValues(typeof(MigrationArchetype)))
                {
                    float hunger = WildlifeSeasonalCalendar.HungerFactor(window, a);
                    float abundance = WildlifeSeasonalCalendar.AbundanceFactor(window, a);
                    Assert.InRange(hunger, WildlifeSeasonalCalendar.HungerFactorMin, WildlifeSeasonalCalendar.HungerFactorMax);
                    Assert.InRange(abundance, WildlifeSeasonalCalendar.AbundanceFactorMin, WildlifeSeasonalCalendar.AbundanceFactorMax);
                }

            // Neutral defaults: no profile → authored cadence everywhere.
            foreach (MigrationArchetype a in Enum.GetValues(typeof(MigrationArchetype)))
            {
                Assert.Equal(1f, WildlifeSeasonalCalendar.HungerFactor(null, a), 5);
                Assert.Equal(1f, WildlifeSeasonalCalendar.AbundanceFactor(null, a), 5);
            }
        }

        [Fact]
        public void AbundancePeaks_AreStaggered_AcrossTheYear()
        {
            var archetypeIds = new[]
            {
                MigrationArchetype.HerdGrazer, MigrationArchetype.BurrowSwarm,
                MigrationArchetype.Sounder, MigrationArchetype.PassageFlock,
                MigrationArchetype.CoastalRunner, MigrationArchetype.SwarmBlight
            };
            var windowIds = new[]
            {
                "window_ashfall", "window_deep_freeze", "window_thaw",
                "window_black_bloom", "window_high_cold", "window_the_turning"
            };

            var peakWindows = new List<string>();
            foreach (var archetype in archetypeIds)
            {
                string best = string.Empty;
                float bestValue = -1f;
                foreach (var id in windowIds)
                {
                    float v = WildlifeSeasonalCalendar.AbundanceFactor(Window(id), archetype);
                    if (v > bestValue) { bestValue = v; best = id; }
                }
                peakWindows.Add(best);
            }

            Assert.True(peakWindows.Distinct().Count() >= 3,
                $"abundance peaks must stagger across seasons; got [{string.Join(", ", peakWindows)}]");
            Assert.NotEqual(1, peakWindows.Distinct().Count());
        }

        [Fact]
        public void FishRun_PeaksInThawAndBloom_EmptiesInDeepFreeze()
        {
            Assert.Equal(0.2f, WildlifeSeasonalCalendar.AbundanceFactor(Window("window_deep_freeze"), MigrationArchetype.CoastalRunner), 3);
            Assert.Equal(1.5f, WildlifeSeasonalCalendar.AbundanceFactor(Window("window_thaw"), MigrationArchetype.CoastalRunner), 3);
            Assert.Equal(1.4f, WildlifeSeasonalCalendar.AbundanceFactor(Window("window_black_bloom"), MigrationArchetype.CoastalRunner), 3);
        }

        [Fact]
        public void HungerFactor_WinterStarvesHerds_FasterThanThaw()
        {
            float freeze = WildlifeSeasonalCalendar.HungerFactor(Window("window_deep_freeze"), MigrationArchetype.HerdGrazer);
            float thaw = WildlifeSeasonalCalendar.HungerFactor(Window("window_thaw"), MigrationArchetype.HerdGrazer);
            Assert.InRange(freeze, 1.2f, 1.5f);
            Assert.InRange(thaw, 0.6f, 0.9f);
            Assert.True(freeze > thaw);
        }

        // ── Legacy neutrality & determinism ─────────────────────────

        [Fact]
        public void UnboundCalendar_IsExactlyLegacyNeutral()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var a = SeededWildlife(seeds, null);
            var b = SeededWildlife(seeds, null);
            for (int day = 1; day <= 120; day++)
            {
                a.TickDay(day, DayFork(day, 1));
                b.TickDay(day, DayFork(day, 1));
            }
            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(a.CaptureState()),
                new SystemTextJsonSerializer().Serialize(b.CaptureState()));
        }

        [Fact]
        public void BoundCalendar_ChangesTrajectory_UnderIdenticalRolls()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var profile = LoadProfile();
            Assert.NotNull(profile);

            var unbound = SeededWildlife(seeds, null);
            var bound = SeededWildlife(seeds, profile);
            for (int day = 1; day <= 90; day++)
            {
                unbound.TickDay(day, DayFork(day, 1));
                bound.TickDay(day, DayFork(day, 1));
            }

            bool diverged = unbound.State.packs.Zip(bound.State.packs, (x, y) =>
                Math.Abs(x.starvationLevel - y.starvationLevel) > 0.0001f).Any(v => v);
            Assert.True(diverged, "a bound season profile must pace hunger differently from the legacy cadence");
        }

        [Fact]
        public void SameSeed_ProducesIdenticalSeasonalTrajectory()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var profile = LoadProfile();

            var a = SeededWildlife(seeds, profile);
            var b = SeededWildlife(seeds, profile);
            for (int day = 1; day <= 360; day++)
            {
                a.TickDay(day, DayFork(day, 1));
                b.TickDay(day, DayFork(day, 1));
            }

            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(a.CaptureState()),
                new SystemTextJsonSerializer().Serialize(b.CaptureState()));
        }

        // ── The fish run stays on real water ────────────────────────

        [Fact]
        public void FishRun_NeverStandsOnDryGround()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var water = seeds!.sectors.Where(s => s.water).Select(s => s.sector_id).ToHashSet();
            Assert.Equal(2, water.Count);
            Assert.Contains("sector_4_river", water);
            Assert.Contains("sector_8_estuary", water);

            var wild = SeededWildlife(seeds, LoadProfile());
            for (int day = 1; day <= 360; day++)
                wild.TickDay(day, DayFork(day, 1));

            var carp = wild.TryGetPack("pack_river_carp_run");
            Assert.NotNull(carp);
            Assert.Contains(carp!.currentSectorId, water);

            var heron = wild.TryGetPack("pack_river_herons");
            Assert.NotNull(heron);
            Assert.Contains(heron!.currentSectorId, water);
        }

        [Fact]
        public void NeighborFilter_BoundsWaterRunners_WithoutStranding()
        {
            var neighbors = new List<string> { "sector_dry", "sector_water" };
            var water = new HashSet<string> { "sector_water" };

            var inWater = WildlifeSeasonalCalendar.FilterNeighbors(
                MigrationArchetype.CoastalRunner, "sector_water", neighbors, new HashSet<string> { "sector_water" });
            Assert.Single(inWater);
            Assert.Equal("sector_water", inWater[0]);

            var heading = WildlifeSeasonalCalendar.FilterNeighbors(
                MigrationArchetype.CoastalRunner, "sector_dry", neighbors, new HashSet<string> { "sector_water" });
            Assert.Single(heading);
            Assert.Equal("sector_water", heading[0]);

            var noWater = WildlifeSeasonalCalendar.FilterNeighbors(
                MigrationArchetype.CoastalRunner, "sector_dry", neighbors, new HashSet<string>());
            Assert.Equal(2, noWater.Count);

            var grazer = WildlifeSeasonalCalendar.FilterNeighbors(
                MigrationArchetype.HerdGrazer, "sector_dry", neighbors, water);
            Assert.Equal(2, grazer.Count);
        }

        // ── Sector abundance (trapping density input) ───────────────

        [Fact]
        public void SectorAbundance_IsDeterministic_AndObservationNeverMutates()
        {
            var profile = LoadProfile();
            Assert.NotNull(profile);
            var seeds = LoadSeeds();
            var wild = SeededWildlife(seeds, profile);

            var before = wild.CaptureState();
            float thawFactor = WildlifeSeasonalCalendar.SectorAbundanceFactor(profile, 150, "sector_4_hinterlands", wild.State.packs);
            float again = WildlifeSeasonalCalendar.SectorAbundanceFactor(profile, 150, "sector_4_hinterlands", wild.State.packs);
            Assert.Equal(thawFactor, again, 5);

            Assert.Equal(1f, WildlifeSeasonalCalendar.SectorAbundanceFactor(null, 150, "sector_4_hinterlands", wild.State.packs), 5);
            Assert.Equal(1f, WildlifeSeasonalCalendar.SectorAbundanceFactor(profile, 150, "sector_unknown", wild.State.packs), 5);

            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(before),
                new SystemTextJsonSerializer().Serialize(wild.CaptureState()));
        }

        // ── Player-facing notices ───────────────────────────────────

        [Fact]
        public void MigrationNotice_IsPlausible_AndNeverExposesExactPopulation()
        {
            string herd = WildlifeSeasonalCalendar.MigrationNotice(
                MigrationArchetype.HerdGrazer, "species_feral_goat",
                "sector_4_hills", "sector_4_hinterlands", 45)!;
            Assert.Contains("herd", herd);
            Assert.DoesNotContain("population", herd);

            string run = WildlifeSeasonalCalendar.MigrationNotice(
                MigrationArchetype.CoastalRunner, "species_mirror_carp", "sector_4_river", "sector_8_estuary", 140)!;
            Assert.Contains("fish", run);

            Assert.NotNull(WildlifeSeasonalCalendar.MigrationNotice(MigrationArchetype.SwarmBlight, "species_ghost_moth", "a", "b", 200));
            Assert.Null(WildlifeSeasonalCalendar.MigrationNotice(MigrationArchetype.Resident, "species_wolf", "a", "b", 1));
            Assert.Null(WildlifeSeasonalCalendar.MigrationNotice(MigrationArchetype.Resident, "", "a", "b", 1));
        }

        // ── Save round-trip with a bound calendar ───────────────────

        [Fact]
        public void SaveRestore_WithBoundCalendar_RoundTripsExactly()
        {
            var profile = LoadProfile();
            Assert.NotNull(profile);
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);

            var wild = SeededWildlife(seeds, profile);
            for (int day = 1; day <= 100; day++)
                wild.TickDay(day, DayFork(day, 1));

            var json = new SystemTextJsonSerializer().Serialize(wild.CaptureState());

            var restored = new WildlifeMigrationSystem();
            restored.BindSeasonProfile(profile);
            restored.RestoreState(new SystemTextJsonSerializer().Deserialize<WildlifeSaveState>(json));

            Assert.Equal(json, new SystemTextJsonSerializer().Serialize(restored.CaptureState()));
        }

        [Fact]
        public void SeasonalCalendar_DoesNotAlterTheSectorGraph()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var wild = SeededWildlife(seeds, LoadProfile());
            Assert.All(wild.State.packs,
                p => Assert.True(wild.TryGetNeighbors(p.currentSectorId, out _),
                    $"{p.packId} stands in unlinked sector {p.currentSectorId}"));
        }

        [Fact]
        public void SeededCatalog_KeepsWaterFlagsOnTheWaterwayPair()
        {
            var seeds = LoadSeeds();
            Assert.NotNull(seeds);
            var water = seeds!.sectors.Where(s => s.water).Select(s => s.sector_id).ToList();
            Assert.Equal(2, water.Count);
            Assert.Contains("sector_4_river", water);
            Assert.Contains("sector_8_estuary", water);
        }

        private static WildlifePackRecord? Pack(WildlifeMigrationSystem wild, string packId) => wild.TryGetPack(packId);
    }
}
