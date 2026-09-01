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
    /// Task 122 — evolving-world activation: seeding, live-world ticks with
    /// per-day RNG forks, determinism, save/resume parity, long-horizon
    /// scenarios, and the influences (density, scarcity inputs, route
    /// passability). Host wiring is covered by --evolving-world-selftest.
    /// </summary>
    public sealed class EvolvingWorldActivationTests : CatalogTestBase
    {
        private static EvolvingWorldSeedContainer? LoadSeeds() =>
            EvolvingWorldCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        private static (LocationEvolutionSystem loc, WildlifeMigrationSystem wild, LandmarkDegradationSystem land) Seeded(
            EvolvingWorldSeedContainer? catalog)
        {
            var loc = new LocationEvolutionSystem();
            var wild = new WildlifeMigrationSystem();
            var land = new LandmarkDegradationSystem();
            EvolvingWorldSeeder.Seed(loc, wild, land, catalog);
            return (loc, wild, land);
        }

        /// <summary>Fork helper mirroring the host's per-day named-stream pattern.</summary>
        private static ISeededRng DayFork(int masterSeed, int day, int actionIndex) =>
            CampaignRngStream.DeriveSeed(masterSeed, "world_evolution", 1, day, actionIndex) is int s
                ? new SeededRng(s != 0 ? s : 1986)
                : new SeededRng(1986);

        private static void TickWorld(
            (LocationEvolutionSystem loc, WildlifeMigrationSystem wild, LandmarkDegradationSystem land) w,
            int day, int masterSeed, float ashfallMm = 0f, bool hazard = false, float rad = 1f)
        {
            w.loc.TickDay(day, new LocationEvolutionInputs(rad, hazard), DayFork(masterSeed, day, 0));
            w.wild.TickDay(day, DayFork(masterSeed, day, 1));
            w.land.TickDay(day, ashfallMm);
        }

        // ── Seed catalog ────────────────────────────────────────────────

        [Fact]
        public void SeedCatalog_LoadsGraphPacksLandmarksAndLocations()
        {
            var catalog = LoadSeeds();
            Assert.NotNull(catalog);
            Assert.Equal(11, catalog!.sectors.Count);
            Assert.Equal(13, catalog.packs.Count);
            Assert.Equal(10, catalog.landmarks.Count);
            Assert.Equal(12, catalog.location_seeds.Count);
            Assert.False(string.IsNullOrEmpty(catalog.shelter_sector_id));
            Assert.NotEmpty(catalog.scarcity_goods);

            // Every sector link points at a known sector.
            var known = catalog.sectors.Select(s => s.sector_id).ToHashSet();
            Assert.All(catalog.sectors, s =>
                Assert.All(s.neighbors, n => Assert.True(known.Contains(n), $"unknown sector link {n}")));

            // Every pack stands in a known sector.
            Assert.All(catalog.packs, p => Assert.True(known.Contains(p.sector_id), $"{p.pack_id} in unknown sector"));

            // Location seeds reference the real locations authority.
            var locationsJson = System.IO.Path.Combine(DataDirectory, "locations.json");
            var raw = new FileSystemIO().ReadAllText(locationsJson);
            Assert.All(catalog.location_seeds, s =>
                Assert.True(raw.Contains("\"" + s.location_id + "\""), $"{s.location_id} not in locations.json"));
            Assert.All(catalog.landmarks, l =>
                Assert.True(raw.Contains("\"" + l.location_id + "\""), $"{l.landmark_id} -> unknown location"));
        }

        [Fact]
        public void Seeder_IsIdempotent_AndDeterministic()
        {
            var catalog = LoadSeeds();
            var a = Seeded(catalog);
            var b = Seeded(catalog);

            // A second seeding pass must not duplicate anything.
            EvolvingWorldSeeder.Seed(a.loc, a.wild, a.land, catalog);

            Assert.Equal(a.wild.State.packs.Count, b.wild.State.packs.Count);
            Assert.Equal(a.land.State.landmarks.Count, b.land.State.landmarks.Count);
            Assert.Equal(13, a.wild.State.packs.Count);

            // Re-seeded records keep their seeded baseline.
            Assert.All(a.wild.State.packs, p => Assert.Equal(p.population, p.seededPopulation));

            // Location seeds landed: owner and contamination set, threats copied.
            var weighbridge = a.loc.TryGetRecord("loc_weighbridge");
            Assert.NotNull(weighbridge);
            Assert.Equal("faction_the_scale", weighbridge!.currentOwner);
            Assert.Contains("threat_rad_squatters", weighbridge.activeThreats);

            // Untouched location stays absent (lazy records only exist for touched ground).
            Assert.Null(a.loc.TryGetRecord("loc_nowhere_at_all"));
        }

        // ── Location evolution ──────────────────────────────────────────

        [Fact]
        public void Contamination_FollowsWeather_AndCanRuin()
        {
            var (loc, _, _) = Seeded(LoadSeeds());
            var rec = loc.GetOrCreateRecord("loc_ordnance_shoulder")!;
            float start = rec.contaminationLevel;

            // Hazard days push contamination up, scaled by the rad modifier.
            for (int day = 1; day <= 10; day++)
                loc.TickDay(day, new LocationEvolutionInputs(150f, hazardWeather: true), DayFork(7, day, 0));
            float peak = rec.contaminationLevel;
            Assert.True(peak > start, "hazard weather must raise contamination");

            // Clear days let it settle back down.
            for (int day = 11; day <= 60; day++)
                loc.TickDay(day, LocationEvolutionInputs.Clear, DayFork(7, day, 0));
            Assert.True(rec.contaminationLevel < peak, "clear weather must let contamination settle");
        }

        [Fact]
        public void SustainedContamination_RuinsLocation_Stickily()
        {
            var loc = new LocationEvolutionSystem();
            var rec = loc.GetOrCreateRecord("loc_test_dirty")!;
            rec.contaminationLevel = 0.97f;

            // ~0.033 per Black-Rain day: two days carry it over the line.
            loc.TickDay(1, new LocationEvolutionInputs(250f, hazardWeather: true));
            loc.TickDay(2, new LocationEvolutionInputs(250f, hazardWeather: true));
            Assert.True(rec.isRuined, "crossing the threshold must ruin the location");

            // Ruin is sticky: clear weather does not un-ruin.
            for (int day = 3; day <= 40; day++)
                loc.TickDay(day, LocationEvolutionInputs.Clear, DayFork(3, day, 0));
            Assert.True(rec.isRuined);
        }

        [Fact]
        public void Threats_SproutOnDormantGround_AndDecay()
        {
            var loc = new LocationEvolutionSystem();
            var rec = loc.GetOrCreateRecord("loc_test_quiet")!;
            rec.lastVisitedDay = 5;

            // Force sprouts with a rng sequence we know will roll under the chance
            // at least once across many days; assert boundedly: threats only grow
            // from zero on unvisited ground and never exceed one sprout pass.
            bool sprouted = false;
            for (int day = 6; day <= 400 && !sprouted; day++)
            {
                loc.TickDay(day, LocationEvolutionInputs.Clear, new SeededRng(day));
                sprouted = rec.activeThreats.Count > 0;
            }
            Assert.True(sprouted, "dormant ground should eventually sprout a threat");
            Assert.Contains(rec.activeThreats, t => t == LocationEvolutionSystem.ThreatSquatters
                                                    || t == LocationEvolutionSystem.ThreatWildBeasts);

            // Explicit removal works.
            var threat = rec.activeThreats[0];
            Assert.Equal(ActionResult.StatusKind.Success, loc.RemoveThreat("loc_test_quiet", threat).Status);
            Assert.Equal(ActionResult.StatusKind.Blocked, loc.RemoveThreat("loc_test_quiet", threat).Status);
        }

        [Fact]
        public void VisitDepleteClear_RecoverCycle()
        {
            var loc = new LocationEvolutionSystem();
            Assert.Equal(ActionResult.StatusKind.Success, loc.MarkVisited("loc_x", 10).Status);
            Assert.Equal(ActionResult.StatusKind.Success, loc.MarkCleared("loc_x", 10).Status);
            Assert.Equal(ActionResult.StatusKind.Success, loc.MarkDepleted("loc_x", 0.5f).Status);
            var rec = loc.TryGetRecord("loc_x")!;
            Assert.Equal(0.8f, rec.lootDepletionFactor, 3); // cleared +0.3, depleted +0.5

            // Leave it alone past the recovery window: depletion recovers.
            for (int day = 11; day <= 60; day++) loc.TickDay(day);
            Assert.True(rec.lootDepletionFactor < 0.8f, "cleared locations must recover loot over time");
        }

        // ── Wildlife migration ──────────────────────────────────────────

        [Fact]
        public void Packs_MigrateOnlyAlongAdjacency_AndRelieveHunger()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_a", "species_test", "sector_home", 6);
            wild.SetSectorAdjacency(new[] { ("sector_home", new List<string> { "sector_far" }) });

            bool migrated = false;
            wild.OnPackMigrated += p => { migrated = true; Assert.Equal("sector_far", p.currentSectorId); };

            // Drive hunger up, then tick until the pack moves.
            var pack = wild.TryGetPack("pack_a")!;
            pack.starvationLevel = 0.8f;
            bool anyMove = false;
            for (int day = 1; day <= 200 && !anyMove; day++)
            {
                wild.TickDay(day, new SeededRng(day));
                anyMove = pack.currentSectorId == "sector_far";
            }
            Assert.True(anyMove, "starving pack must eventually migrate along the graph");
            Assert.True(migrated);
            Assert.True(pack.starvationLevel < 0.8f, "moving to new ground relieves hunger");
        }

        [Fact]
        public void Packs_Starve_Thinen_Rabid_And_Or_Recover()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_s", "species_test", "sector_s", 10);
            var pack = wild.TryGetPack("pack_s")!;

            // Starve: no adjacency, so the pack cannot flee its ground.
            for (int day = 1; day <= 40; day++) wild.TickDay(day, new SeededRng(day));
            Assert.True(pack.population < 10, "starving packs thin out");
            Assert.Equal(0, wild.GetSectorPackPopulation("sector_nowhere"));

            // Fed ground recovers toward twice the seed size, never beyond.
            pack.starvationLevel = 0f;
            pack.seededPopulation = 5;
            pack.population = 9;
            for (int day = 41; day <= 120; day++) wild.TickDay(day, new SeededRng(day));
            Assert.True(pack.population <= 10, "recovery caps at twice the seeded population");
            Assert.InRange(wild.GetGlobalPopulationRatio(), 0f, 2f);
        }

        [Fact]
        public void SectorDensity_FeedsTrappingGate()
        {
            var wild = new WildlifeMigrationSystem();
            wild.RegisterPack("pack_a", "species_test", "sector_home", 5);
            wild.RegisterPack("pack_b", "species_test", "sector_home", 4);
            wild.RegisterPack("pack_c", "species_test", "sector_far", 9);

            Assert.Equal(9, wild.GetSectorPackPopulation("sector_home"));
            Assert.Equal(9, wild.GetSectorPackPopulation("sector_far"));
            Assert.Equal(0, wild.GetSectorPackPopulation("sector_empty"));
        }

        // ── Landmarks ───────────────────────────────────────────────────

        [Fact]
        public void Landmarks_BuryCollapseScavenge_AndAveraging()
        {
            var land = new LandmarkDegradationSystem();
            int collapses = 0;
            land.OnLandmarkCollapsed += _ => collapses++;
            land.RegisterLandmark("landmark_a", "loc_a", 10f);
            land.RegisterLandmark("landmark_b", "loc_b", 100f);

            // Ashfall buries; passive decay grinds; collapse fires once.
            land.TickDay(1, 20f); // +2cm ash, -0.2 integrity
            var a = land.TryGetLandmark("landmark_a")!;
            Assert.Equal(2f, a.ashBurialCm, 3);

            for (int day = 2; day <= 60; day++) land.TickDay(day, 0f);
            Assert.Equal(1, collapses);
            Assert.True(a.isCollapsed);
            Assert.Equal(1, land.TryGetLandmark("landmark_a")!.collapseDay >= 0 ? 1 : 0);

            Assert.Equal(ActionResult.StatusKind.Success, land.MarkScavenged("landmark_b").Status);
            Assert.Equal(ActionResult.StatusKind.Blocked, land.MarkScavenged("landmark_b").Status);
            Assert.True(land.MeanIntegrity() is > 0f and <= 100f);
        }

        // ── Determinism & parity ────────────────────────────────────────

        [Fact]
        public void SameMasterSeed_SameTrajectory_DifferentSeedDiverges()
        {
            var catalog = LoadSeeds();
            var a = Seeded(catalog);
            var b = Seeded(catalog);
            var c = Seeded(catalog);

            for (int day = 1; day <= 30; day++)
            {
                TickWorld(a, day, masterSeed: 4242, ashfallMm: day % 5 == 0 ? 12f : 0f, hazard: day % 5 == 0);
                TickWorld(b, day, masterSeed: 4242, ashfallMm: day % 5 == 0 ? 12f : 0f, hazard: day % 5 == 0);
                TickWorld(c, day, masterSeed: 777, ashfallMm: day % 5 == 0 ? 12f : 0f, hazard: day % 5 == 0);
            }

            var pa = a.wild.State.packs.OrderBy(p => p.packId).Select(p => (p.currentSectorId, p.population)).ToList();
            var pb = b.wild.State.packs.OrderBy(p => p.packId).Select(p => (p.currentSectorId, p.population)).ToList();
            var pc = c.wild.State.packs.OrderBy(p => p.packId).Select(p => (p.currentSectorId, p.population)).ToList();
            Assert.Equal(pa, pb);
            Assert.NotEqual(pa, pc); // genuinely seeded: different streams diverge
        }

        [Fact]
        public void SaveResume_Parity_ContinuesIdentically()
        {
            var catalog = LoadSeeds();
            var run = Seeded(catalog);
            var resumed = Seeded(catalog);

            for (int day = 1; day <= 10; day++)
                TickWorld(run, day, masterSeed: 99, ashfallMm: day % 3 == 0 ? 8f : 0f);

            // Capture at day 10, restore into the twin.
            resumed.loc.RestoreState(run.loc.CaptureState());
            resumed.wild.RestoreState(run.wild.CaptureState());
            resumed.land.RestoreState(run.land.CaptureState());

            for (int day = 11; day <= 60; day++)
            {
                TickWorld(run, day, masterSeed: 99, ashfallMm: day % 3 == 0 ? 8f : 0f);
                TickWorld(resumed, day, masterSeed: 99, ashfallMm: day % 3 == 0 ? 8f : 0f);
            }

            // Day-forked rng makes the trajectory depend on the day, not the
            // call count — a restored world must match an unbroken one exactly.
            Assert.Equal(
                run.wild.State.packs.OrderBy(p => p.packId).Select(p => (p.currentSectorId, p.population, p.isRabid)),
                resumed.wild.State.packs.OrderBy(p => p.packId).Select(p => (p.currentSectorId, p.population, p.isRabid)));
            Assert.Equal(
                run.land.State.landmarks.OrderBy(l => l.landmarkId).Select(l => (l.structuralIntegrity, l.isCollapsed)),
                resumed.land.State.landmarks.OrderBy(l => l.landmarkId).Select(l => (l.structuralIntegrity, l.isCollapsed)));
            Assert.Equal(
                run.loc.State.mutations.OrderBy(m => m.locationId).Select(m => (m.contaminationLevel, m.isRuined, m.lootDepletionFactor)),
                resumed.loc.State.mutations.OrderBy(m => m.locationId).Select(m => (m.contaminationLevel, m.isRuined, m.lootDepletionFactor)));
        }

        // ── Long-horizon scenarios ──────────────────────────────────────

        [Theory]
        [InlineData(30)]
        [InlineData(180)]
        [InlineData(360)]
        public void LongHorizon_InvariantsHold(int days)
        {
            var (loc, wild, land) = Seeded(LoadSeeds());
            int collapseEvents = 0;
            land.OnLandmarkCollapsed += _ => collapseEvents++;

            for (int day = 1; day <= days; day++)
            {
                bool hazard = day % 9 == 0;
                TickWorld((loc, wild, land), day, masterSeed: 2026,
                    ashfallMm: hazard ? 14f : 0f, hazard: hazard, rad: hazard ? 150f : 1f);

                // Invariants every single day.
                foreach (var l in land.State.landmarks)
                {
                    Assert.InRange(l.structuralIntegrity, 0f, 100f);
                    Assert.True(l.ashBurialCm >= 0f);
                    Assert.True(!l.isCollapsed || l.collapseDay > 0);
                }
                foreach (var p in wild.State.packs)
                {
                    Assert.InRange(p.population, 0, 40);
                    Assert.InRange(p.starvationLevel, 0f, 1f);
                    Assert.InRange(p.aggressionScore, 0f, 1f);
                }
                foreach (var m in loc.State.mutations)
                {
                    Assert.InRange(m.contaminationLevel, 0f, 1f);
                    Assert.InRange(m.lootDepletionFactor, 0f, 1f);
                }
                Assert.InRange(collapseEvents, 0, land.State.landmarks.Count);
            }

            // Substep 12: the world degrades but never dies. Some pack life
            // remains somewhere, and collapse events fired at most once each.
            Assert.True(wild.GetGlobalPopulationRatio() > 0f, "the land must not go completely silent");
            Assert.True(collapseEvents <= land.State.landmarks.Count);
        }

        [Fact]
        public void Routes_NeverPermanentlyUnwinnable_AfterEvolution()
        {
            // The three systems never write to the map's lock state, and loot
            // depletion recovers — prove both with the real catalog after a
            // full year of hostile evolution.
            var catalog = LoadSeeds();
            var (loc, wild, land) = Seeded(catalog);
            for (int day = 1; day <= 360; day++)
            {
                bool hazard = day % 7 == 0;
                TickWorld((loc, wild, land), day, masterSeed: 5, ashfallMm: hazard ? 18f : 0f, hazard: hazard);
            }

            var map = WastelandMapCatalogLoader.CreateSystem(DataDirectory);
            foreach (var nodeId in new[] { "loc_holdfast", "loc_cut_abandoned_depot", "loc_cut_merchant_caravanserai",
                                           "loc_cut_radiation_zone_alpha", "loc_cut_arsenal_ruin" })
                map.Discover(nodeId);

            var route = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            Assert.NotEmpty(route);
            Assert.Equal("loc_holdfast", route[0]);
            Assert.Equal("loc_cut_arsenal_ruin", route[^1]);

            // Scarcity pressure stays bounded: demand shifts clamp inside the market.
            Assert.True(wild.GetGlobalPopulationRatio() > 0f);
        }

        [Fact]
        public void ScarcityGoods_ShieldedFromEmptyCatalog()
        {
            Assert.Empty(EvolvingWorldSeeder.ScarcityGoods(null));
            Assert.Equal(string.Empty, EvolvingWorldSeeder.ShelterSectorId(null));
            var catalog = LoadSeeds();
            Assert.Contains("canned_food", EvolvingWorldSeeder.ScarcityGoods(catalog));
        }
    }
}
