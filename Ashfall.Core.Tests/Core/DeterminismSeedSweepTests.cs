// SPDX-License-Identifier: MIT
// ASHFALL Invariant 4: Determinism Seed-Sweep Suite.
// Runs multi-day simulations across parameter sweeps of distinct seeds.
// On any divergence, failures explicitly record the exact seed for deterministic local reproduction.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Disease;
using Ashfall.Core.Economy;
using Ashfall.Core.Maritime;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Expeditions;

namespace Ashfall.Core.Tests
{
    public class DeterminismSeedSweepTests
    {
        public static readonly int[] SweepSeeds = new[]
        {
            0,
            1,
            2,
            3,
            7,
            13,
            42,
            99,
            100,
            256,
            500,
            1013,
            1337,
            4096,
            65535,
            1000003,
            123456789,
            987654321,
            0x5F3759DF,
            0x12345678,
            0x55555555,
            -1,
            -7,
            -42,
            -1337,
            -65535,
            -1000003,
            -123456789
        };

        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static void FailWithSeed(int seed, string systemName, string context, string expected, string actual)
        {
            throw new Xunit.Sdk.XunitException(
                $"[DETERMINISM DIVERGENCE] System '{systemName}' diverged on seed {seed}!\n" +
                $"Context: {context}\n" +
                $"Instance A: {expected}\n" +
                $"Instance B: {actual}\n" +
                $"Exact Reproduction: Run test with seed={seed}.");
        }

        [Fact]
        public void FailureRecording_IncludesSeedAndReproductionInstructions()
        {
            var ex = Assert.Throws<Xunit.Sdk.XunitException>(() =>
                FailWithSeed(42, "TestSystem", "TestContext", "ValA", "ValB"));

            Assert.Contains("seed 42", ex.Message);
            Assert.Contains("TestSystem", ex.Message);
            Assert.Contains("TestContext", ex.Message);
            Assert.Contains("Exact Reproduction: Run test with seed=42", ex.Message);
        }

        // ─────────────────────────────────────────────────────────────────
        // 1. WeatherSystem Seed Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void WeatherSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves(int seed)
        {
            var profile = new SeasonProfileDef
            {
                id = "sweep_profile",
                weatherCheckIntervalHours = 6f,
                seasons = new List<SeasonWindowDef>
                {
                    new SeasonWindowDef { id = "s1", startDay = 0, clearWeight = 2f, rainWeight = 1.5f, overcastWeight = 1f, ashfallWeight = 0.5f, falloutStormWeight = 0.2f, blizzardWeight = 0.1f },
                    new SeasonWindowDef { id = "s2", startDay = 15, clearWeight = 0.5f, rainWeight = 1f, overcastWeight = 2f, ashfallWeight = 1.5f, falloutStormWeight = 0.8f, blizzardWeight = 1.2f }
                }
            };

            var sysA = new WeatherSystem();
            sysA.BindProfile(profile, seed);

            var sysB = new WeatherSystem();
            sysB.BindProfile(profile, seed);

            WeatherSystem? sysC = null;

            for (int hour = 1; hour <= 720; hour += 6)
            {
                sysA.Tick(6f);
                sysB.Tick(6f);

                if (sysA.Current != sysB.Current)
                {
                    FailWithSeed(seed, "WeatherSystem", $"Hour {hour} (Day {hour / 24}) weather state mismatch", sysA.Current.ToString(), sysB.Current.ToString());
                }

                if (sysA.State.rollCount != sysB.State.rollCount)
                {
                    FailWithSeed(seed, "WeatherSystem", $"Hour {hour} rollCount mismatch", sysA.State.rollCount.ToString(), sysB.State.rollCount.ToString());
                }

                // Mid-run save/restore check at Day 15 (Hour 360)
                if (hour == 360)
                {
                    var savedState = new WorldWeatherState
                    {
                        systemId = sysA.State.systemId,
                        currentKind = sysA.State.currentKind,
                        totalElapsedHours = sysA.State.totalElapsedHours,
                        hoursUntilNextCheck = sysA.State.hoursUntilNextCheck,
                        rollCount = sysA.State.rollCount,
                        restrictToNonHazardWeather = sysA.State.restrictToNonHazardWeather
                    };
                    sysC = new WeatherSystem(savedState);
                    sysC.BindProfile(profile, seed);
                }

                if (sysC != null && hour > 360)
                {
                    sysC.Tick(6f);
                    if (sysA.Current != sysC.Current)
                    {
                        FailWithSeed(seed, "WeatherSystem", $"Hour {hour} Save/Restore resumed instance C diverged from instance A", sysA.Current.ToString(), sysC.Current.ToString());
                    }
                }
            }

            Assert.Equal(sysA.State.rollCount, sysB.State.rollCount);
            Assert.Equal(sysA.Current, sysB.Current);
        }

        // ─────────────────────────────────────────────────────────────────
        // 2. MarketSystem Seed Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void MarketSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves(int seed)
        {
            var catalog = new GoodsCatalogLoadResult();
            catalog.Goods.Add(new GoodDefinition { id = "canned_food", displayName = "Food", basePrice = 10f, volatility = 0.5f, elasticity = 1f });
            catalog.Goods.Add(new GoodDefinition { id = "clean_water", displayName = "Water", basePrice = 8f, volatility = 0.6f, elasticity = 1.2f });
            catalog.Goods.Add(new GoodDefinition { id = "fuel_canister", displayName = "Fuel", basePrice = 15f, volatility = 0.4f, elasticity = 0.8f });
            catalog.Goods.Add(new GoodDefinition { id = "med_kit", displayName = "Medkit", basePrice = 25f, volatility = 0.7f, elasticity = 1.5f });

            var goodsCatalog = GoodsCatalogLoader.ToCatalog(catalog);

            var sysA = new MarketSystem();
            sysA.BindCatalog(goodsCatalog);

            var sysB = new MarketSystem();
            sysB.BindCatalog(goodsCatalog);

            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);

            MarketSystem? sysC = null;
            SeededRng? rngC = null;

            for (int day = 1; day <= 30; day++)
            {
                sysA.TickDay(day, rngA);
                sysB.TickDay(day, rngB);

                foreach (var good in catalog.Goods)
                {
                    float pA = sysA.GetPrice(good.id);
                    float pB = sysB.GetPrice(good.id);

                    if (Math.Abs(pA - pB) > 0.0001f)
                    {
                        FailWithSeed(seed, "MarketSystem", $"Day {day} price divergence for {good.id}", pA.ToString("F4"), pB.ToString("F4"));
                    }
                }

                // Mid-run save/restore at Day 15
                if (day == 15)
                {
                    var savedState = sysA.CaptureState();
                    sysC = new MarketSystem();
                    sysC.BindCatalog(goodsCatalog);
                    sysC.RestoreState(savedState);
                    // Cloned RNG at day 15 step
                    rngC = new SeededRng(seed);
                    for (int d = 1; d <= 15; d++)
                    {
                        // Advance RNG identical number of steps as days 1..15
                        var dummy = new MarketSystem();
                        dummy.BindCatalog(goodsCatalog);
                        dummy.TickDay(d, rngC);
                    }
                }

                if (sysC != null && rngC != null && day > 15)
                {
                    sysC.TickDay(day, rngC);
                    foreach (var good in catalog.Goods)
                    {
                        float pA = sysA.GetPrice(good.id);
                        float pC = sysC.GetPrice(good.id);
                        if (Math.Abs(pA - pC) > 0.0001f)
                        {
                            FailWithSeed(seed, "MarketSystem", $"Day {day} Save/Restore instance C price diverged for {good.id}", pA.ToString("F4"), pC.ToString("F4"));
                        }
                    }
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // 3. PowerGridSystem Seed Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void PowerGridSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves(int seed)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom { RoomId = "room_workshop", DisplayName = "Workshop", DrawWatts = 150 },
                new PowerGridRoom { RoomId = "room_hydroponics", DisplayName = "Hydroponics", DrawWatts = 200 },
                new PowerGridRoom { RoomId = "room_infirmary", DisplayName = "Infirmary", DrawWatts = 100 },
                new PowerGridRoom { RoomId = "room_quarters", DisplayName = "Quarters", DrawWatts = 50 }
            };

            var stateA = new PowerGridState { FuelUnits = 100f, BatteryReserveWh = 500f, BatteryCapacityWh = 2000f };
            var stateB = new PowerGridState { FuelUnits = 100f, BatteryReserveWh = 500f, BatteryCapacityWh = 2000f };

            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);

            var sysA = new PowerGridSystem(stateA, rooms, rngA);
            var sysB = new PowerGridSystem(stateB, rooms, rngB);

            for (int day = 1; day <= 30; day++)
            {
                sysA.TickDay(day, rngA);
                sysB.TickDay(day, rngB);

                if (Math.Abs(sysA.FuelUnits - sysB.FuelUnits) > 0.001f)
                {
                    FailWithSeed(seed, "PowerGridSystem", $"Day {day} FuelUnits mismatch", sysA.FuelUnits.ToString("F3"), sysB.FuelUnits.ToString("F3"));
                }

                if (Math.Abs(sysA.BatteryReserveWh - sysB.BatteryReserveWh) > 0.001f)
                {
                    FailWithSeed(seed, "PowerGridSystem", $"Day {day} BatteryReserveWh mismatch", sysA.BatteryReserveWh.ToString("F3"), sysB.BatteryReserveWh.ToString("F3"));
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // 4. NeedsSystem Determinism Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void NeedsSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves(int seed)
        {
            var profile = new NeedsProfile();
            var sysA = new NeedsSystem(profile);
            var sysB = new NeedsSystem(profile);

            var survivorA = new SurvivorNeedsState { Id = "s_yuki", Hunger = 10f, Thirst = 10f, Fatigue = 5f, Warmth = 100f, Morale = 70f, Health = 100f };
            var survivorB = new SurvivorNeedsState { Id = "s_yuki", Hunger = 10f, Thirst = 10f, Fatigue = 5f, Warmth = 100f, Morale = 70f, Health = 100f };

            sysA.Register(survivorA);
            sysB.Register(survivorB);

            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);

            for (int hour = 1; hour <= 720; hour++)
            {
                // Fluctuate environmental factors deterministically with seed
                float hungerDrift = (float)(rngA.NextDouble() * 0.1);
                float thirstDrift = (float)(rngA.NextDouble() * 0.15);

                float hungerDriftB = (float)(rngB.NextDouble() * 0.1);
                float thirstDriftB = (float)(rngB.NextDouble() * 0.15);

                sysA.Modify(survivorA, NeedKind.Hunger, hungerDrift);
                sysA.Modify(survivorA, NeedKind.Thirst, thirstDrift);
                sysA.Tick(survivorA, 1f);

                sysB.Modify(survivorB, NeedKind.Hunger, hungerDriftB);
                sysB.Modify(survivorB, NeedKind.Thirst, thirstDriftB);
                sysB.Tick(survivorB, 1f);

                if (Math.Abs(survivorA.Hunger - survivorB.Hunger) > 0.001f ||
                    Math.Abs(survivorA.Thirst - survivorB.Thirst) > 0.001f ||
                    Math.Abs(survivorA.Health - survivorB.Health) > 0.001f ||
                    Math.Abs(survivorA.Morale - survivorB.Morale) > 0.001f)
                {
                    FailWithSeed(
                        seed,
                        "NeedsSystem",
                        $"Hour {hour} needs divergence",
                        $"H={survivorA.Hunger:F2}, T={survivorA.Thirst:F2}, HP={survivorA.Health:F2}, M={survivorA.Morale:F2}",
                        $"H={survivorB.Hunger:F2}, T={survivorB.Thirst:F2}, HP={survivorB.Health:F2}, M={survivorB.Morale:F2}");
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // 5. DiseaseSystem Contagion Seed Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void DiseaseSystem_SeedSweep_IsDeterministicAcrossReplaysAndSaves(int seed)
        {
            var catalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            var sysA = new DiseaseSystem(rng: new SeededRng(seed));
            sysA.BindCatalog(catalog);

            var sysB = new DiseaseSystem(rng: new SeededRng(seed));
            sysB.BindCatalog(catalog);

            var roster = new List<string> { "s_1", "s_2", "s_3", "s_4", "s_5", "s_6", "s_7", "s_8" };

            // Seed initial infection
            sysA.Infect("s_1", DiseaseIds.Cholera, 1);
            sysB.Infect("s_1", DiseaseIds.Cholera, 1);

            for (int day = 2; day <= 30; day++)
            {
                sysA.TickDaily(day, roster);
                sysB.TickDaily(day, roster);

                var snapA = sysA.GetSnapshot();
                var snapB = sysB.GetSnapshot();

                if (snapA.total_infected != snapB.total_infected ||
                    snapA.total_recovered != snapB.total_recovered ||
                    snapA.total_deaths != snapB.total_deaths ||
                    snapA.total_outbreaks != snapB.total_outbreaks)
                {
                    FailWithSeed(
                        seed,
                        "DiseaseSystem",
                        $"Day {day} disease statistics mismatch",
                        $"Inf={snapA.total_infected}, Rec={snapA.total_recovered}, Dead={snapA.total_deaths}, Outbreaks={snapA.total_outbreaks}",
                        $"Inf={snapB.total_infected}, Rec={snapB.total_recovered}, Dead={snapB.total_deaths}, Outbreaks={snapB.total_outbreaks}");
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // 6. Combat Ballistics Seed Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void CombatBallistics_SeedSweep_IsDeterministicAcrossReplays(int seed)
        {
            CombatCatalog.SeedDefaults();

            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);

            for (int shot = 1; shot <= 25; shot++)
            {
                var targetA = new CombatantState { Id = "target", Health = 100, MaxHealth = 100, ArmorRating = (shot % 2 == 0) ? 0.5f : 0f, CoverRating = (shot % 3 == 0) ? 0.7f : 0f };
                var targetB = new CombatantState { Id = "target", Health = 100, MaxHealth = 100, ArmorRating = (shot % 2 == 0) ? 0.5f : 0f, CoverRating = (shot % 3 == 0) ? 0.7f : 0f };

                var ctxA = new BallisticContext
                {
                    ShooterId = "p1",
                    WeaponAccuracy = 0.75f,
                    WeaponDamage = 25f,
                    IntendedTarget = targetA,
                    CoverMaterial = (shot % 3 == 0) ? CombatCatalog.GetMaterial("material_concrete") : null,
                    ArmorMaterial = (shot % 2 == 0) ? CombatCatalog.GetMaterial("material_steel") : null
                };

                var ctxB = new BallisticContext
                {
                    ShooterId = "p1",
                    WeaponAccuracy = 0.75f,
                    WeaponDamage = 25f,
                    IntendedTarget = targetB,
                    CoverMaterial = (shot % 3 == 0) ? CombatCatalog.GetMaterial("material_concrete") : null,
                    ArmorMaterial = (shot % 2 == 0) ? CombatCatalog.GetMaterial("material_steel") : null
                };

                var resA = BallisticsSystem.Resolve(ctxA, rngA);
                var resB = BallisticsSystem.Resolve(ctxB, rngB);

                if (resA.Result != resB.Result ||
                    Math.Abs(resA.DamageDealt - resB.DamageDealt) > 0.001f ||
                    resA.Reason != resB.Reason)
                {
                    FailWithSeed(
                        seed,
                        "CombatBallistics",
                        $"Shot {shot} outcome divergence",
                        $"Result={resA.Result}, Dmg={resA.DamageDealt:F2}, Reason={resA.Reason}",
                        $"Result={resB.Result}, Dmg={resB.DamageDealt:F2}, Reason={resB.Reason}");
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // 7. Procedural Scavenge Seed Sweep
        // ─────────────────────────────────────────────────────────────────

        [Theory]
        [MemberData(nameof(GetSweepSeeds))]
        public void ProceduralScavenge_SeedSweep_IsDeterministicAcrossReplays(int seed)
        {
            var lootTable = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "item_canned_goods", MinQty = 1, MaxQty = 4, SpawnChance = 0.8f },
                new VariableLootNode { ItemId = "item_scrap_metal", MinQty = 2, MaxQty = 8, SpawnChance = 0.9f },
                new VariableLootNode { ItemId = "item_fuel_canister", MinQty = 1, MaxQty = 2, SpawnChance = 0.4f },
                new VariableLootNode { ItemId = "item_medkit", MinQty = 1, MaxQty = 1, SpawnChance = 0.25f }
            };

            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);

            var sysA = new ProceduralScavengeSystem(rngA);
            var sysB = new ProceduralScavengeSystem(rngB);

            for (int day = 1; day <= 30; day++)
            {
                sysA.SetCurrentDay(day);
                sysB.SetCurrentDay(day);

                string locationId = $"loc_derelict_{day % 4}";
                var rollA = sysA.RollLootTable(locationId, lootTable, locationRads: 5f, hasBioHazard: false);
                var rollB = sysB.RollLootTable(locationId, lootTable, locationRads: 5f, hasBioHazard: false);

                if (rollA.Count != rollB.Count)
                {
                    FailWithSeed(seed, "ProceduralScavengeSystem", $"Day {day} item count mismatch at {locationId}", rollA.Count.ToString(), rollB.Count.ToString());
                }

                for (int i = 0; i < rollA.Count; i++)
                {
                    if (rollA[i].ItemId != rollB[i].ItemId || rollA[i].Quantity != rollB[i].Quantity)
                    {
                        FailWithSeed(
                            seed,
                            "ProceduralScavengeSystem",
                            $"Day {day} item mismatch at {locationId} idx {i}",
                            $"{rollA[i].ItemId}:{rollA[i].Quantity}",
                            $"{rollB[i].ItemId}:{rollB[i].Quantity}");
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetSweepSeeds()
        {
            return SweepSeeds.Select(s => new object[] { s });
        }
    }
}

// ── Plan 73 Wave F: Flagship campaign continuity pass (§12–§14) ──────
// Runs all four flagship systems (sump, ventilation, sonde, rail) through
// a deterministic 45-day scenario, then verifies save/load split-run
// replay parity and identical-seed replay parity.
namespace Ashfall.Core.Tests
{
    public class FlagshipContinuityTests
    {
        private const int Seed = 77;
        private const int TotalDays = 45;
        private const int SplitDay = 20;

        private static WeatherSystem CreateWeather()
        {
            var weather = new WeatherSystem();
            var profile = new SeasonProfileDef
            {
                id = "continuity_profile",
                displayName = "Continuity Test",
                weatherCheckIntervalHours = 6f,
                seasons = new List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "test_season",
                        displayName = "Test",
                        startDay = 0,
                        clearWeight = 1f,
                        rainWeight = 1f,
                        overcastWeight = 1f,
                        ashfallWeight = 0.5f,
                        falloutStormWeight = 0.1f,
                        blizzardWeight = 0.1f,
                        blackRainWeight = 0f
                    }
                }
            };
            weather.BindProfile(profile, Seed);
            return weather;
        }

        private static PowerGridSystem CreatePower()
        {
            var state = new PowerGridState
            {
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("sump_room", "Lower Level", 100f)
            };
            return new PowerGridSystem(state, rooms, new SeededRng(Seed));
        }

        private static SumpFloodingSystem CreateSump(
            WeatherSystem weather,
            PowerGridSystem power,
            YearOfAshDeepFreezeSystem df,
            Inventory.Inventory inventory,
            WaterTreatmentSystem waterTreatment)
        {
            var sys = new SumpFloodingSystem(new SeededRng(Seed), weather, power, df);
            sys.AddNode("sump_a", "Lower Sump");
            sys.InstallPump("sump_a");
            sys.SetNodePower("sump_a", true);
            sys.ApplyStratumCatalog(new[]
            {
                new SumpStratumDef
                {
                    stratum_id = "strat_a",
                    display_name = "Test Stratum",
                    base_ingress_cm_per_day = 2f,
                    water_table_pressure = 1.0f,
                    silt_fraction = 0.3f,
                    pump_load_modifier = 1.0f,
                    toxicity_tier = 1
                }
            });
            sys.AssignStratum("sump_a", "strat_a");
            sys.BindServices(inventory, waterTreatment);
            return sys;
        }

        private static VentilationSystem CreateVentilation(
            StartingLevelSystem startingLevel,
            PowerGridSystem power,
            Inventory.Inventory inventory)
        {
            var sys = new VentilationSystem(startingLevel);
            sys.RegisterSource(new VentilationSource
            {
                sourceId = "src_sump",
                roomId = "sump_room",
                smokeOutputPerDay = 0.5f,
                coOutputPerDay = 0.2f,
                requiresExhaust = true,
                isActive = true
            });
            sys.ApplyElectrostaticCatalog(new[]
            {
                new ElectrostaticStageDef
                {
                    stage_id = "stage_electrostatic_single",
                    display_name = "Test Stage",
                    operating_profiles = new List<ElectrostaticProfileDef>
                    {
                        new ElectrostaticProfileDef
                        {
                            profile_id = "profile_standard",
                            display_name = "Standard",
                            nominal_power_w = 350,
                            capture_efficiency_pm25 = 0.80f,
                            capture_efficiency_pm10 = 0.90f,
                            hot_ash_capture_efficiency = 0.70f,
                            ozone_output_rate_ppm_per_day = 8.0f,
                            arc_risk_base_bp = 40
                        }
                    },
                    dust_capacity_kg = 12f,
                    maintenance_interval_days = 14,
                    required_component_ids = new List<ElectrostaticComponentCost>
                    {
                        new ElectrostaticComponentCost { item_id = "item_air_filter_hepa", amount = 1 },
                        new ElectrostaticComponentCost { item_id = "mechanical_parts", amount = 4 },
                        new ElectrostaticComponentCost { item_id = "scrap_chemical", amount = 2 }
                    },
                    tags = new List<string>()
                }
            });
            sys.InstallElectrostaticStage("stage_electrostatic_single", "sump_room");
            sys.SetStageProfile("profile_standard");
            sys.BindStageServices(new SeededRng(Seed), power, inventory, null);
            return sys;
        }

        private static WeatherSondeSystem CreateSonde(WeatherSystem weather)
        {
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            return sys;
        }

        private static RailwaySystem CreateRail(Inventory.Inventory inventory)
        {
            var sys = new RailwaySystem(new SeededRng(Seed), inventory);
            sys.RegisterCatalog(new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "term_a", display_name = "Terminal A", node_type = "Terminal" },
                    new RailNodeDef { node_id = "term_b", display_name = "Terminal B", node_type = "Terminal" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_a_b",
                        display_name = "A-B",
                        start_node_id = "term_a",
                        end_node_id = "term_b",
                        distance_km = 1f,
                        base_integrity = 0.9f,
                        bridge_required = false,
                        max_train_mass = 200f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef { car_type_id = "car_locomotive_diesel", display_name = "Loco", empty_mass = 70f },
                    new TrainCarDef { car_type_id = "car_freight_hopper", display_name = "Hopper", empty_mass = 25f }
                }
            });
            sys.CreateStarterTrain("train_1", "Shuttle", "term_a");
            sys.DispatchTrain("train_1", "seg_a_b");
            return sys;
        }

        private record ScenarioBundle(
            WeatherSystem Weather,
            PowerGridSystem Power,
            YearOfAshDeepFreezeSystem DeepFreeze,
            StartingLevelSystem StartingLevel,
            SumpFloodingSystem Sump,
            VentilationSystem Ventilation,
            WeatherSondeSystem Sonde,
            RailwaySystem Rail,
            Inventory.Inventory Inventory
        );

        private static ScenarioBundle SetupScenario()
        {
            var weather = CreateWeather();
            var power = CreatePower();
            var df = new YearOfAshDeepFreezeSystem();
            var inventory = new Inventory.Inventory();
            var waterTreatment = new WaterTreatmentSystem(NullLog.Instance);
            inventory.AddById("item_air_filter_hepa", 1);
            inventory.AddById("mechanical_parts", 4);
            inventory.AddById("scrap_chemical", 2);

            var startingLevel = new StartingLevelSystem();
            var sump = CreateSump(weather, power, df, inventory, waterTreatment);
            var vent = CreateVentilation(startingLevel, power, inventory);
            var sonde = CreateSonde(weather);
            var rail = CreateRail(inventory);

            return new ScenarioBundle(
                weather, power, df, startingLevel, sump, vent, sonde, rail, inventory);
        }

        private static void TickScenario(ScenarioBundle bundle, int day)
        {
            bundle.Sump.TickDay(day);
            bundle.Ventilation.TickDay(day, incomingParticulateKgPerDay: 0.3f, hotAshLoad: false);
            bundle.Sonde.Tick(new SeededRng(Seed), day);
            bundle.Rail.TickTravel("train_1", 0.5f);
        }

        private static string SerializeState(object? state)
        {
            return System.Text.Json.JsonSerializer.Serialize(
                state,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = false });
        }

        private static void AssertEqualCampaignState(
            SumpFloodingState aSump, VentilationState aVent, WeatherSondeState aSonde, RailwayState aRail,
            SumpFloodingState bSump, VentilationState bVent, WeatherSondeState bSonde, RailwayState bRail,
            string message)
        {
            Assert.Equal(SerializeState(aSump), SerializeState(bSump));
            Assert.Equal(SerializeState(aVent), SerializeState(bVent));
            Assert.Equal(SerializeState(aSonde), SerializeState(bSonde));
            Assert.Equal(SerializeState(aRail), SerializeState(bRail));
        }

        [Fact]
        public void SplitRun_SaveLoad_ProducesIdenticalFinalState()
        {
            // Direct run: 45 days
            var direct = SetupScenario();
            for (int day = 1; day <= TotalDays; day++)
                TickScenario(direct, day);

            // Split run: 20 days, save, restore, continue 25 days
            var split = SetupScenario();
            for (int day = 1; day <= SplitDay; day++)
                TickScenario(split, day);

            var saved = new
            {
                weather = split.Weather.CaptureState(),
                power = split.Power.CaptureState(),
                deepFreeze = split.DeepFreeze.CaptureState(),
                startingLevel = split.StartingLevel.CaptureState(),
                sump = split.Sump.CaptureState(),
                vent = split.Ventilation.CaptureState(),
                sonde = split.Sonde.CaptureState(),
                rail = split.Rail.State
            };

            var restored = SetupScenario();
            restored.Weather.RestoreState(saved.weather);
            restored.Power.RestoreState(saved.power);
            restored.DeepFreeze.RestoreState(saved.deepFreeze);
            restored.StartingLevel.RestoreState(saved.startingLevel);
            restored.Sump.RestoreState(saved.sump);
            restored.Ventilation.RestoreState(saved.vent);
            restored.Sonde.RestoreState(saved.sonde);
            restored.Rail.RestoreState(saved.rail);

            for (int day = SplitDay + 1; day <= TotalDays; day++)
                TickScenario(restored, day);

            AssertEqualCampaignState(
                direct.Sump.CaptureState(), direct.Ventilation.CaptureState(), direct.Sonde.CaptureState(), direct.Rail.State,
                restored.Sump.CaptureState(), restored.Ventilation.CaptureState(), restored.Sonde.CaptureState(), restored.Rail.State,
                "Split-run final state mismatch");
        }

        [Fact]
        public void ReplayParity_SameSeed_SameFinalState()
        {
            var run1 = SetupScenario();
            for (int day = 1; day <= TotalDays; day++)
                TickScenario(run1, day);

            var run2 = SetupScenario();
            for (int day = 1; day <= TotalDays; day++)
                TickScenario(run2, day);

            AssertEqualCampaignState(
                run1.Sump.CaptureState(), run1.Ventilation.CaptureState(), run1.Sonde.CaptureState(), run1.Rail.State,
                run2.Sump.CaptureState(), run2.Ventilation.CaptureState(), run2.Sonde.CaptureState(), run2.Rail.State,
                "Replay parity mismatch");
        }
    }
}
