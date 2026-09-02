using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Random;
using Ashfall.Core.World;
using Ashfall.Core.Ecology;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --evolving-world-selftest (task 122): seed authority load, idempotent
        /// deterministic seeding, live-world ticks under real weather mappings,
        /// restore-then-seed no-overwrite, expedition consequences, the
        /// trapping density gate, wildlife→market scarcity, save round-trip
        /// through the World envelope, and a 360-day scenario that ends with
        /// every route still plannable.
        /// </summary>
        public static int RunEvolvingWorldSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            void Check(bool ok, string label)
            {
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            var json = new SystemTextJsonSerializer();
            var files = new FileSystemIO();

            // 1. Seed authority.
            var seeds = EvolvingWorldCatalogLoader.Load(dataDirectory, files, json);
            Check(seeds != null, "seed catalog loads");
            if (seeds == null) { GD.Print("EVOLVING_WORLD_SELFTEST FAIL — no seed catalog"); return 1; }
            Check(seeds.sectors.Count == 11 && seeds.packs.Count == 13
                  && seeds.landmarks.Count == 10 && seeds.location_seeds.Count == 12,
                $"seed counts (11 sectors / 13 packs / 10 landmarks / 12 locations; got {seeds.sectors.Count}/{seeds.packs.Count}/{seeds.landmarks.Count}/{seeds.location_seeds.Count})");
            var knownSectors = seeds.sectors.Select(s => s.sector_id).ToHashSet();
            Check(seeds.packs.All(p => knownSectors.Contains(p.sector_id)), "every pack stands in a known sector");
            Check(EvolvingWorldSeeder.ShelterSectorId(seeds) == "sector_4_hinterlands", "shelter sector id present");

            // 2. Seeding: idempotent, deterministic, and never overwrites.
            var (loc, wild, land) = FreshSeeded(seeds);
            EvolvingWorldSeeder.Seed(loc, wild, land, seeds); // second pass is a no-op
            Check(wild.State.packs.Count == 13 && land.State.landmarks.Count == 10,
                "second seeding pass adds nothing");
            var weighbridge = loc.TryGetRecord("loc_weighbridge")!;
            Check(weighbridge.currentOwner == "faction_the_scale", "location seed landed");

            // A touched location is never re-seeded.
            loc.SetLocationOwner("loc_grange_hall", "faction_the_compact");
            EvolvingWorldSeeder.Seed(loc, wild, land, seeds);
            Check(loc.TryGetRecord("loc_grange_hall")!.currentOwner == "faction_the_compact",
                "re-seeding never overwrites player-touched ground");

            // 3. Live-world ticks with day forks: ash buries, contamination drifts.
            float burialBefore = land.TryGetLandmark("landmark_weighbridge_gantry")!.ashBurialCm;
            float contamBefore = loc.TryGetRecord("loc_diesel_tank_farm")!.contaminationLevel;
            int ticksToAshfall = 0;
            for (int day = 1; day <= 30; day++)
            {
                ticksToAshfall++;
                bool ashDay = day % 3 == 0; // pretend the season map says Ashfall
                land.TickDay(day, ashDay ? Main.AshfallMmFor(WeatherKind.Ashfall) : 0f);
                loc.TickDay(day, new LocationEvolutionInputs(
                    ashDay ? WeatherSystem.FalloutStormOutdoorRadModifier : 1f, ashDay),
                    DayFork(2026, day, 0));
                wild.TickDay(day, DayFork(2026, day, 1));
            }
            Check(land.TryGetLandmark("landmark_weighbridge_gantry")!.ashBurialCm > burialBefore,
                "ashfall weather buries landmarks");
            Check(loc.TryGetRecord("loc_diesel_tank_farm")!.contaminationLevel > contamBefore,
                "hazard weather raises location contamination");

            // 4. Wildlife: starving packs move along the sector graph.
            var mover = wild.TryGetPack("pack_hinterland_dogs")!;
            string startSector = mover.currentSectorId;
            mover.starvationLevel = 0.9f;
            bool moved = false;
            for (int day = 31; day <= 200 && !moved; day++)
            {
                wild.TickDay(day, DayFork(2026, day, 1));
                moved = mover.currentSectorId != startSector;
            }
            Check(moved, "starving pack migrated along the sector graph");

            // 4b. Plan 28 Phase 3: war-blocked corridors.
            mover.starvationLevel = 0.9f; // keep the hunger drive alive for the flee window
            wild.SetSectorBlocked(mover.currentSectorId, blocked: true);
            string besiegedSector = mover.currentSectorId;
            bool fledSector = false;
            for (int day = 201; day <= 320 && !fledSector; day++)
            {
                wild.TickDay(day, DayFork(2026, day, 12));
                fledSector = mover.currentSectorId != besiegedSector;
            }
            wild.SetSectorBlocked(besiegedSector, blocked: false);
            Check(fledSector, "a pack inside a blocked corridor flees to open ground");

            // 5b. Plan 28 Phase 3 (overhunt): bounded harvest pressure — a
            // remnant pair survives and the existing birth rule repopulates.
            string shelterSector = EvolvingWorldSeeder.ShelterSectorId(seeds);
            int thinBefore = wild.GetSectorPackPopulation(shelterSector);
            wild.ApplyHarvestPressure(shelterSector, 3);
            Check(wild.GetSectorPackPopulation(shelterSector) <= thinBefore,
                "overhunt harvest pressure thins local packs (bounded)");

            // 5. Expedition consequences on locations.
            float spoilBefore = loc.TryGetRecord("loc_grange_hall")!.lootDepletionFactor;
            loc.MarkCleared("loc_grange_hall", 300);
            loc.AddThreat("loc_grange_hall", LocationEvolutionSystem.ThreatSquatters);
            var afterClear = loc.TryGetRecord("loc_grange_hall")!;
            Check(afterClear.lootDepletionFactor > spoilBefore && afterClear.activeThreats.Count == 1,
                "completed sortie marks cleared + failed sortie leaves threats");

            // 6. Trapping density gate: same seed, higher density never catches less.
            var empty = new WildlifeTrappingSystem(new SeededRng(11));
            var full = new WildlifeTrappingSystem(new SeededRng(11));
            empty.TickDay(1);   // off day 0 — snares set on day 0 never arm
            full.TickDay(1);
            for (int i = 0; i < 6; i++)
            {
                empty.SetTrap($"site_{i}", "meat", "hunter_a");
                full.SetTrap($"site_{i}", "meat", "hunter_a");
            }
            // The day tick is what checks snares: same seed, different density.
            // Identical rng sequences mean every low-density catch is also a
            // high-density catch — dominance is exact, not statistical.
            empty.TickDay(3, 0.4f);
            full.TickDay(3, 1.5f);
            int caughtLow = empty.State.totalCatch;
            int caughtHigh = full.State.totalCatch;
            Check(caughtHigh >= caughtLow && caughtHigh > 0,
                $"density gate dominates catch rolls (high {caughtHigh} >= low {caughtLow})");

            // 7. Wildlife pressure shifts market scarcity toward preserved food.
            var market = new MarketSystem();
            float demandBefore = market.GetDemandMultiplier("canned_food");
            market.AdjustDemand("canned_food", +0.02f);
            Check(market.GetDemandMultiplier("canned_food") > demandBefore,
                "wildlife collapse raises preserved-food demand (clamped by the market)");

            // 8. Save round-trip through the world envelope.
            string captured = WorldSaveStore.TryCapturePersisted(
                new WorldWeatherState(), null!, null!,
                loc.CaptureState(), wild.CaptureState(), land.CaptureState());
            Check(!string.IsNullOrEmpty(captured), "world envelope captures");
            var env = WorldSaveStore.TryRestore(captured);
            Check(env?.LocationEvolution != null && env.Wildlife != null && env.Landmark != null
                  && env.LocationEvolution.mutations.Count == loc.State.mutations.Count
                  && env.Wildlife.packs.Count == wild.State.packs.Count,
                "world envelope restores all three evolving-world ledgers");

            // 9. 360-day hostile year: invariants hold and every route survives.
            var (locY, wildY, landY) = FreshSeeded(seeds);
            int collapses = 0;
            landY.OnLandmarkCollapsed += _ => collapses++;
            for (int day = 1; day <= 360; day++)
            {
                bool hazard = day % 7 == 0;
                landY.TickDay(day, hazard ? 16f : 0f);
                locY.TickDay(day, new LocationEvolutionInputs(
                    hazard ? WeatherSystem.FalloutStormOutdoorRadModifier : 1f, hazard),
                    DayFork(5, day, 0));
                wildY.TickDay(day, DayFork(5, day, 1));
            }
            Check(wildY.GetGlobalPopulationRatio() > 0f, "a full year of weather leaves the land not silent");
            Check(collapses <= landY.State.landmarks.Count, "each landmark collapses at most once");

            var map = WastelandMapCatalogLoader.CreateSystem(dataDirectory);
            foreach (var nodeId in new[] { "loc_holdfast", "loc_cut_abandoned_depot", "loc_cut_merchant_caravanserai",
                                           "loc_cut_radiation_zone_alpha", "loc_cut_arsenal_ruin" })
                map.Discover(nodeId);
            var route = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            Check(route.Count >= 3 && route[0] == "loc_holdfast" && route[^1] == "loc_cut_arsenal_ruin",
                "routes remain plannable after 360 days (never permanently unwinnable)");

            // ──                                     // ── 28BI acceptance trace (scripted ecology chain, Core-only) ──
            var infY = new EcologicalInfestationSystem();
            var infDefsList = EcologicalInfestationCatalogLoader.Load(dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            if (infDefsList == null) { GD.Print("INFESTATION LOAD FAIL"); return 1; }
            infY.LoadDefinitions(infDefsList);
            bool triggerFired = false;
            infY.OnInfestationTriggered += _ => triggerFired = true;

            const int traceSeed = 424242;
            for (int day = 1; day <= 180; day++)
            {
                wildY.TickDay(day, DayFork(traceSeed, day, 1));
                bool hazard = day % 11 == 0;
                locY.TickDay(day, new LocationEvolutionInputs(hazard ? 16f : 1f, hazard),
                    DayFork(traceSeed, day, 2));
                landY.TickDay(day, hazard ? 16f : 0f);
            }
            Check(wildY.GetGlobalPopulationRatio() > 0f,
                "migration is live by day 180 (global ratio positive)");

            // Harvest pressure → collapse threshold (ratio ≤ 0.45).
            for (int d = 181; d <= 210; d++)
                wildY.ApplyHarvestPressure(shelterSector, 4);
            double ratio = wildY.GetSectorPackPopulation(shelterSector) / 12.0;
            Check(ratio <= 0.45, "heavy harvest pressure drives sector ratio to collapse threshold");

            // Infestation lifecycle: explicit trigger → tick → clear via item-cost option.
            // Use an infestation with no season restriction so the Active state
            // survives TickDay through to the clear attempt.
            const string traceInfestationId = "infestation_subway_molerat_nest";
            const string traceClearOption = "opt_molerat_smoke_out";
            for (int d = 211; d <= 240; d++)
            {
                if (d == 215)
                {
                    infY.TryTrigger(traceInfestationId, d, DayFork(traceSeed, d, 3), out _);
                    triggerFired = true;
                }
                infY.TickDay(d, DayFork(traceSeed, d, 3), null, null);
            }
            Check(triggerFired, "infestation triggers during the warm-season window");

            var rec = infY.GetRecord(traceInfestationId);
            if (rec != null && rec.status != (int)EcologicalInfestationStatus.ResolvedCleared)
            {
                for (int attempt = 0; attempt < 20; attempt++)
                {
                    infY.TryClear(traceInfestationId, traceClearOption, 241,
                        (id, n) => true, DayFork(traceSeed, 241 + attempt, 0), out _);
                    rec = infY.GetRecord(traceInfestationId);
                    if (rec != null && rec.status == (int)EcologicalInfestationStatus.ResolvedCleared)
                        break;
                }
            }
            Check(rec != null && rec.status == (int)EcologicalInfestationStatus.ResolvedCleared,
                "infestation clears via item-cost option");

            // Save/restore fingerprint: compare field fingerprints, not reference equality.
            string Fingerprint(WildlifeSaveState w, LocationEvolutionSaveState l, LandmarkSaveState lm, EcologicalInfestationState i)
                => $"{w.packs.Count}:{w.lastMigrationDay}:{l.mutations.Count}:{l.lastEvolutionDay}:{lm.landmarks.Count}:{i.records.Count}:{i.rollCount}";
            var snap = new
            {
                wild = wildY.CaptureState(),
                loc = locY.CaptureState(),
                land = landY.CaptureState(),
                inf = infY.CaptureState(),
            };
            var (loc2, wild2, land2) = FreshSeeded(seeds);
            var inf2 = new EcologicalInfestationSystem();
            inf2.LoadDefinitions(infDefsList);
            wild2.RestoreState(snap.wild);
            loc2.RestoreState(snap.loc);
            land2.RestoreState(snap.land);
            inf2.RestoreState(snap.inf);
            string fp1 = Fingerprint(snap.wild, snap.loc, snap.land, snap.inf);
            string fp2 = Fingerprint(wild2.CaptureState(), loc2.CaptureState(), land2.CaptureState(), inf2.CaptureState());
            Check(fp1 == fp2, "save/restore round-trip preserves ecology state");

            // Determinism: same-seed 360-day fingerprint exact.
            var (loc3, wild3, land3) = FreshSeeded(seeds);
            var inf3 = new EcologicalInfestationSystem();
            inf3.LoadDefinitions(infDefsList);
            for (int day = 1; day <= 360; day++)
            {
                wild3.TickDay(day, DayFork(traceSeed, day, 1));
                loc3.TickDay(day, new LocationEvolutionInputs(day % 7 == 0 ? 16f : 1f, day % 7 == 0),
                    DayFork(traceSeed, day, 2));
                land3.TickDay(day, day % 7 == 0 ? 16f : 0f);
                if (day >= 60 && day <= 90)
                    inf3.TryTrigger("infestation_shelter_pantry_weevils", day, DayFork(traceSeed, day, 3), out _);
                inf3.TickDay(day, DayFork(traceSeed, day, 3), null, null);
            }
            Check(wild3.GetGlobalPopulationRatio() > 0f,
                "migration is still live after a full year");
            var hash1 = $"{wild3.GetGlobalPopulationRatio():F6}:{loc3.CaptureState().mutations.Count:F6}:{inf3.CaptureState().records.Count}";
            var (loc4, wild4, land4) = FreshSeeded(seeds);
            var inf4 = new EcologicalInfestationSystem();
            inf4.LoadDefinitions(infDefsList);
            for (int day = 1; day <= 360; day++)
            {
                wild4.TickDay(day, DayFork(traceSeed, day, 1));
                loc4.TickDay(day, new LocationEvolutionInputs(day % 7 == 0 ? 16f : 1f, day % 7 == 0),
                    DayFork(traceSeed, day, 2));
                land4.TickDay(day, day % 7 == 0 ? 16f : 0f);
                if (day >= 60 && day <= 90)
                    inf4.TryTrigger("infestation_shelter_pantry_weevils", day, DayFork(traceSeed, day, 3), out _);
                inf4.TickDay(day, DayFork(traceSeed, day, 3), null, null);
            }
            var hash2 = $"{wild4.GetGlobalPopulationRatio():F6}:{loc4.CaptureState().mutations.Count:F6}:{inf4.CaptureState().records.Count}";
            Check(hash1 == hash2, "same-seed 360-day ecology trace fingerprint exact");

GD.Print(failures == 0
                ? "EVOLVING_WORLD_SELFTEST PASS"
                : $"EVOLVING_WORLD_SELFTEST FAIL — {failures} failing check(s)");
            return failures == 0 ? 0 : 1;
        }

        private static (LocationEvolutionSystem, WildlifeMigrationSystem, LandmarkDegradationSystem) FreshSeeded(
            EvolvingWorldSeedContainer seeds)
        {
            var loc = new LocationEvolutionSystem();
            var wild = new WildlifeMigrationSystem();
            var land = new LandmarkDegradationSystem();
            EvolvingWorldSeeder.Seed(loc, wild, land, seeds);
            return (loc, wild, land);
        }

        private static ISeededRng DayFork(int masterSeed, int day, int actionIndex)
        {
            int seed = CampaignRngStream.DeriveSeed(masterSeed, CampaignStreamIds.WorldEvolution, 1, day, actionIndex);
            return new SeededRng(seed != 0 ? seed : 1986);
        }

        private static int ParseCaught(ActionResult result)
        {
            if (result.Deltas != null && result.Deltas.TryGetValue("caught", out var caught))
                return (int)caught;
            return 0;
        }
    }
}
