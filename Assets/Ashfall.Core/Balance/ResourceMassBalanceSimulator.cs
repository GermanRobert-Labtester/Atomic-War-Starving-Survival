// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Inventory;
using Ashfall.Core.Production;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Balance
{
    public sealed class ResourceMassBalanceConfig
    {
        public int Seed { get; set; } = 42;
        public int Days { get; set; } = 30;
        public int CrewSize { get; set; } = 4;
        public float DailyRawWaterInflow { get; set; } = 12f;
        public float InitialCleanWater { get; set; } = 25f;
        public float InitialRawWater { get; set; } = 30f;
        public float InitialFuel { get; set; } = 80f;
        public int InitialCannedFood { get; set; } = 80;
        public int InitialRawMeat { get; set; } = 12;
        public bool EnableTrapping { get; set; } = true;
        public bool EnableGreenhouse { get; set; } = true;
        public bool EnableKitchen { get; set; } = true;
        public bool EnablePowerGrid { get; set; } = true;
        public string ScenarioName { get; set; } = "Baseline";
    }

    public sealed class ResourceMassBalanceDailyTelemetry
    {
        public int Day { get; set; }
        public float AvgHealth { get; set; }
        public float AvgHunger { get; set; }
        public float AvgThirst { get; set; }
        public float AvgMorale { get; set; }
        public float AvgWarmth { get; set; }
        public int AliveCrew { get; set; }
        public double StoredWaterTotal { get; set; }
        public int CleanWaterBottles { get; set; }
        public double WaterDiscrepancy { get; set; }
        public int FoodInventoryCount { get; set; }
        public int PantryMealPortions { get; set; }
        public int MealsServedToday { get; set; }
        public int FoodSpoiledToday { get; set; }
        public float FuelUnitsRemaining { get; set; }
        public float BatteryReserveWh { get; set; }
        public float BrownoutHours { get; set; }
        public int TrappingCatchesToday { get; set; }
        public int GreenhouseHarvestsToday { get; set; }
    }

    public sealed class ResourceMassBalanceResult
    {
        public bool Success { get; set; } = true;
        public string ScenarioName { get; set; } = string.Empty;
        public int Seed { get; set; }
        public int DaysSimulated { get; set; }
        public float FinalSurvivalRate { get; set; }
        public int SurvivorsAlive { get; set; }
        public int TotalDeaths { get; set; }
        public float AvgSurvivorHealth { get; set; }
        public float AvgSurvivorHunger { get; set; }
        public float AvgSurvivorThirst { get; set; }
        public float AvgSurvivorMorale { get; set; }
        public double TotalWaterInflow { get; set; }
        public double TotalWaterConsumedCrew { get; set; }
        public double TotalWaterCropTranspiration { get; set; }
        public double TotalWaterFilterWaste { get; set; }
        public double FinalWaterStored { get; set; }
        public double MaxWaterDiscrepancy { get; set; }
        public int TotalMeatProduced { get; set; }
        public int TotalCropsHarvested { get; set; }
        public int TotalMealsServed { get; set; }
        public int TotalFoodSpoiled { get; set; }
        public float TotalFuelBurned { get; set; }
        public float TotalBrownoutHours { get; set; }
        public List<ResourceMassBalanceDailyTelemetry> Telemetry { get; } = new List<ResourceMassBalanceDailyTelemetry>();
        public List<string> InvariantViolations { get; } = new List<string>();
    }

    /// <summary>
    /// Headless mass-balance simulation engine across unified survival systems:
    ///   - Water treatment, bottling, and crew hydration consumption.
    ///   - Wildlife trapping output delivery via IOutputSink.
    ///   - Greenhouse plot irrigation, lighting, and crop harvest.
    ///   - Kitchen nutrition prep jobs, refrigeration, spoilage, and meal service.
    ///   - Power grid fuel burn, battery reserve, and brownout consequences.
    ///   - Needs system hourly/daily drift and health/starvation evaluation.
    /// </summary>
    public static class ResourceMassBalanceSimulator
    {
        public static ResourceMassBalanceResult Run(ResourceMassBalanceConfig config, ILog? log = null)
        {
            log ??= NullLog.Instance;
            var result = new ResourceMassBalanceResult
            {
                ScenarioName = config.ScenarioName,
                Seed = config.Seed,
                DaysSimulated = config.Days
            };

            var rng = new SeededRng(config.Seed);
            var inventory = new Inventory.Inventory { Capacity = 100, MaxWeight = 500f };

            // Seed inventory with initial items
            inventory.Add(new ItemDefinition { id = "clean_water", displayName = "Clean Water", stackMax = 99, weight = 0.5f }, 10);
            inventory.Add(new ItemDefinition { id = "canned_food", displayName = "Canned Food", stackMax = 99, weight = 0.5f }, config.InitialCannedFood);
            inventory.Add(new ItemDefinition { id = "raw_meat", displayName = "Raw Meat", stackMax = 99, weight = 0.5f }, config.InitialRawMeat);
            inventory.Add(new ItemDefinition { id = "leather_strap", displayName = "Leather Strap", stackMax = 99, weight = 0.2f }, 4);
            inventory.Add(new ItemDefinition { id = "fuel_canister", displayName = "Fuel", stackMax = 99, weight = 1.0f }, 10);
            inventory.Add(new ItemDefinition { id = GreenhouseExpansionCatalog.Items.SeedMushroom, displayName = "Spore Culture", stackMax = 99, weight = 0.1f }, Math.Max(30, config.Days));
            inventory.Add(new ItemDefinition { id = GreenhouseExpansionCatalog.Items.CropMushroom, displayName = "Ash Mushroom", stackMax = 99, weight = 0.3f }, 0);

            // 1. Water Treatment System
            var waterSys = new WaterTreatmentSystem();
            waterSys.AddWater(WaterType.Clean, config.InitialCleanWater);
            waterSys.AddWater(WaterType.Raw, config.InitialRawWater);
            waterSys.State.charcoalSupply = Math.Max(500f, config.Days * 20f);
            waterSys.State.distillationFuel = Math.Max(500f, config.Days * 20f);

            // 2. Power Grid System
            var powerRooms = new List<PowerGridRoom>
            {
                new PowerGridRoom { RoomId = "room_water_filtration", DisplayName = "Water Plant", DrawWatts = 150 },
                new PowerGridRoom { RoomId = "room_kitchen", DisplayName = "Kitchen & Cold Storage", DrawWatts = 120 },
                new PowerGridRoom { RoomId = "room_greenhouse", DisplayName = "Grow Lights", DrawWatts = 200 },
                new PowerGridRoom { RoomId = "room_heating", DisplayName = "Shelter Heating", DrawWatts = 180 }
            };
            var powerState = new PowerGridState
            {
                GenerationWatts = 700f,
                FuelUnits = config.InitialFuel,
                BatteryReserveWh = 1200f,
                BatteryCapacityWh = 3000f
            };
            var powerSys = new PowerGridSystem(powerState, powerRooms, rng);

            // 3. Needs System & Crew Setup
            var crew = new List<SurvivorNeedsState>();
            var needsSys = new NeedsSystem(
                profile: new NeedsProfile
                {
                    hungerPerHour = 0.8f,
                    thirstPerHour = 1.2f,
                    fatiguePerHour = 0.4f,
                    warmthLossPerHourInCold = 0.6f,
                    warmthRestorePerHourNearHeat = 3.0f,
                    hungerCritical = 85f,
                    thirstCritical = 85f,
                    warmthCritical = 25f
                },
                isNearHeatSource: _ => powerSys.IsRoomPowered("room_heating")
            );

            for (int c = 1; c <= config.CrewSize; c++)
            {
                var survivor = new SurvivorNeedsState
                {
                    Id = $"survivor_{c}",
                    Health = 100f,
                    Hunger = 15f + (c * 2f),
                    Thirst = 15f + (c * 2f),
                    Warmth = 90f,
                    Morale = 80f,
                    Fatigue = 10f
                };
                crew.Add(survivor);
                needsSys.Register(survivor);
            }

            // 4. Kitchen & Nutrition System
            var kitchenSys = new KitchenNutritionSystem(rng, inventory, needsSys, log);
            kitchenSys.SetCellar(true, 8f);

            // 5. Wildlife Trapping System
            var trappingSys = new WildlifeTrappingSystem(rng, log);
            trappingSys.RegisterQuarry(new QuarrySpecies
            {
                speciesId = "rabbit",
                displayName = "Ash Rabbit",
                baseYieldKg = 1.2f,
                toxicChance = 0.1f,
                hideYield = 1.0f,
                hideItemId = "leather_strap",
                preferredTrapType = "snare"
            });
            trappingSys.RegisterQuarry(new QuarrySpecies
            {
                speciesId = "pheasant",
                displayName = "Cinder Pheasant",
                baseYieldKg = 1.8f,
                toxicChance = 0.05f,
                hideYield = 1.0f,
                hideItemId = "leather_strap",
                preferredTrapType = "snare"
            });

            // Set up 4 snares for 2 trappers
            trappingSys.SetTrap("snare_1", "scrap_meat", "survivor_1", "snare", "trap_snare", checkIntervalDays: 1, durabilityChecks: 20);
            trappingSys.SetTrap("snare_2", "scrap_meat", "survivor_1", "snare", "trap_snare", checkIntervalDays: 1, durabilityChecks: 20);
            trappingSys.SetTrap("snare_3", "scrap_meat", "survivor_2", "snare", "trap_snare", checkIntervalDays: 1, durabilityChecks: 20);
            trappingSys.SetTrap("snare_4", "scrap_meat", "survivor_2", "snare", "trap_snare", checkIntervalDays: 1, durabilityChecks: 20);

            // 6. Greenhouse System
            var greenhouseSys = new GreenhouseSystem(config.Seed);
            greenhouseSys.EnsurePlots(4);

            // Initial Water Accounting
            double initialWaterTotal = waterSys.TotalWater
                + inventory.CountById("clean_water")
                + inventory.CountById("dirty_water")
                + inventory.CountById("irradiated_water");

            double totalIncomingWater = 0.0;
            double totalWaterWaste = 0.0;
            double totalWaterConsumedCrew = 0.0;
            double totalCropTranspiration = 0.0;

            int totalMeatProduced = 0;
            int totalCropsHarvested = 0;
            int totalMealsServed = 0;
            int totalFoodSpoiled = 0;
            float totalFuelBurned = 0f;
            float totalBrownoutHours = 0f;
            double maxWaterDiscrepancy = 0.0;

            // ──────────────── Daily Simulation Loop ────────────────
            for (int day = 1; day <= config.Days; day++)
            {
                int meatProducedToday = 0;
                int cropsHarvestedToday = 0;
                int mealsServedToday = 0;
                int foodSpoiledToday = 0;
                int catchesToday = 0;

                // A. POWER GRID TICK
                float dailyBrownout = 0f;
                bool kitchenPower = true;
                bool waterPower = true;
                bool greenhousePower = true;
                float growLightHours = 6f;

                if (config.EnablePowerGrid)
                {
                    var powerSummary = powerSys.TickDay(day, rng);
                    totalFuelBurned += powerSummary.FuelConsumed;
                    totalBrownoutHours += powerSummary.BrownoutHours;
                    dailyBrownout = powerSummary.BrownoutHours;

                    kitchenPower = powerSys.IsRoomPowered("room_kitchen");
                    waterPower = powerSys.IsRoomPowered("room_water_filtration");
                    greenhousePower = powerSys.IsRoomPowered("room_greenhouse");
                    growLightHours = greenhousePower ? 6f : 0f;
                }
                kitchenSys.SetRefrigeration(kitchenPower);

                // B. WATER INFLOW & TREATMENT
                if (config.DailyRawWaterInflow > 0f)
                {
                    // Stochastic variation ±20%
                    float variance = (float)(0.8 + 0.4 * rng.NextDouble());
                    float todayInflow = config.DailyRawWaterInflow * variance;
                    waterSys.AddWater(WaterType.Raw, todayInflow);
                    totalIncomingWater += todayInflow;
                }

                // If water filtration is powered and idle, process raw water into clean
                float batchAmount = Math.Min(waterSys.RawWater, 25f);
                if (waterPower && !waterSys.IsProcessing && batchAmount >= 5f && waterSys.State.charcoalSupply >= batchAmount * 0.05f)
                {
                    waterSys.StartTreatment(TreatmentMode.CharcoalFiltration, batchAmount);
                }

                int jobsBefore = waterSys.State.completedJobs.Count;
                waterSys.TickDay(day);
                int jobsAfter = waterSys.State.completedJobs.Count;
                if (jobsAfter > jobsBefore)
                {
                    for (int j = jobsBefore; j < jobsAfter; j++)
                    {
                        totalWaterWaste += waterSys.State.completedJobs[j].wasteAmount;
                    }
                }

                // Bottle clean water if shelter bulk is high and inventory has capacity
                if (waterSys.CleanWater >= 15f && inventory.CountById("clean_water") < 25)
                {
                    var drawnRes = waterSys.RemoveWater(WaterType.Clean, 4f);
                    if (drawnRes.IsSuccess && drawnRes.Deltas != null && drawnRes.Deltas.TryGetValue("removed", out var drawnVal))
                    {
                        inventory.AddById("clean_water", (int)drawnVal);
                    }
                }

                // C. WILDLIFE TRAPPING TICK & BUTCHERY
                if (config.EnableTrapping)
                {
                    trappingSys.TickDay(day);

                    // Check both trap sites
                    foreach (var site in trappingSys.State.trapSites)
                    {
                        if (site.hasCatch && !site.isMeatProcessed)
                        {
                            catchesToday++;
                            var butcherRes = trappingSys.Butcher(site.siteId, "survivor_1");
                            if (butcherRes.IsSuccess && butcherRes.Deltas != null && butcherRes.Deltas.TryGetValue("yield", out var yieldVal))
                            {
                                int meatDelivered = Math.Max(1, (int)Math.Round(yieldVal));
                                inventory.AddById("raw_meat", meatDelivered);
                                totalMeatProduced += meatDelivered;
                                meatProducedToday += meatDelivered;
                            }
                            trappingSys.SetTrap(site.siteId, "scrap_meat", site.assignedHunterId, site.trapType, site.trapId, checkIntervalDays: 1, durabilityChecks: 20);
                        }

                        // Reset trap if broken or depleted
                        if (site.remainingDurability <= 2 || site.isBroken)
                        {
                            site.remainingDurability = 20;
                            site.isBroken = false;
                            site.baitType = "scrap_meat";
                        }
                    }
                }

                // D. GREENHOUSE AGRICULTURE TICK
                if (config.EnableGreenhouse)
                {
                    for (int p = 0; p < greenhouseSys.PlotCount; p++)
                    {
                        var plot = greenhouseSys.Plots[p];
                        // Clear if failed
                        if (plot.stage == (int)GreenhouseStage.Failed)
                        {
                            totalCropTranspiration += plot.water;
                            greenhouseSys.Clear(p);
                        }

                        // Harvest if mature
                        if (plot.stage == (int)GreenhouseStage.Mature)
                        {
                            totalCropTranspiration += plot.water; // residual plot moisture cleared on harvest
                            var harvest = greenhouseSys.Harvest(p);
                            if (harvest.success)
                            {
                                inventory.AddById(harvest.yieldItemId, harvest.amount);
                                inventory.AddById(GreenhouseExpansionCatalog.Items.SeedMushroom, 1);
                                totalCropsHarvested += harvest.amount;
                                cropsHarvestedToday += harvest.amount;
                            }
                        }

                        // Treat blight if detected
                        if (plot.blight > 0f)
                        {
                            greenhouseSys.TreatBlight(p, out _);
                        }

                        // Plant if fallow
                        if (GreenhouseSystem.IsFallow(plot))
                        {
                            greenhouseSys.Plant(p, GreenhouseExpansionCatalog.Items.SeedMushroom, day, out _);
                        }

                        // Water plot if low (Plot water threshold)
                        if (plot.water < 30f)
                        {
                            // Draw clean water from bulk treatment plant (directly, no packaging)
                            float irrigateLiters = 10f;
                            if (waterSys.GetWater(WaterType.Clean) >= irrigateLiters)
                            {
                                waterSys.RemoveWater(WaterType.Clean, irrigateLiters);
                                greenhouseSys.Water(p, irrigateLiters, tainted: false);
                            }
                            else if (inventory.CountById("clean_water") >= 2)
                            {
                                inventory.RemoveById("clean_water", 2);
                                greenhouseSys.Water(p, 2f, tainted: false);
                            }
                        }
                    }

                    // Measure exact water consumed across all plots during tick
                    double ghWaterBefore = 0.0;
                    for (int p = 0; p < greenhouseSys.PlotCount; p++)
                        ghWaterBefore += greenhouseSys.Plots[p].water;

                    greenhouseSys.TickDay(day, growLightHours: growLightHours, ashContaminationRate: 0f);

                    double ghWaterAfter = 0.0;
                    for (int p = 0; p < greenhouseSys.PlotCount; p++)
                        ghWaterAfter += greenhouseSys.Plots[p].water;

                    totalCropTranspiration += (ghWaterBefore - ghWaterAfter);
                }

                // E. KITCHEN PREP & SPOILAGE TICK
                if (config.EnableKitchen)
                {
                    // If pantry has low meals, cook meals from raw meat or mushrooms
                    int currentPantryPortions = 0;
                    foreach (var pi in kitchenSys.State.pantry)
                    {
                        if (!pi.isSpoiled) currentPantryPortions += pi.portionCount;
                    }

                    if (currentPantryPortions < 16)
                    {
                        if (inventory.CountById("raw_meat") >= 2)
                        {
                            var reqs = new Dictionary<string, int> { { "raw_meat", 2 } };
                            kitchenSys.StartPrepJob("cooked_meat_stew", "survivor_3", reqs);
                        }
                        if (inventory.CountById(GreenhouseExpansionCatalog.Items.CropMushroom) >= 2)
                        {
                            var reqs = new Dictionary<string, int> { { GreenhouseExpansionCatalog.Items.CropMushroom, 2 } };
                            kitchenSys.StartPrepJob("mushroom_broth", "survivor_4", reqs);
                        }
                    }

                    int spoiledBefore = kitchenSys.State.pantry.FindAll(p => p.isSpoiled).Count;
                    kitchenSys.TickDay(day);
                    int spoiledAfter = kitchenSys.State.pantry.FindAll(p => p.isSpoiled).Count;
                    foodSpoiledToday = spoiledAfter - spoiledBefore;
                    totalFoodSpoiled += foodSpoiledToday;
                }

                // F. CREW HYDRATION, NUTRITION, & NEEDS
                foreach (var survivor in crew)
                {
                    if (!survivor.IsAliveState) continue;

                    // 1. Water Ration
                    float waterNeeded = 2.5f;
                    var rationRes = waterSys.ConsumeRation(waterNeeded);
                    if (rationRes.Deltas != null && rationRes.Deltas.TryGetValue("consumed", out var consumedAmount))
                    {
                        totalWaterConsumedCrew += consumedAmount;
                        // Consuming water restores thirst
                        needsSys.Modify(survivor, NeedKind.Thirst, -35f);
                    }

                    // 2. Meal Serving
                    bool served = false;
                    foreach (var p in kitchenSys.State.pantry)
                    {
                        if (p.portionCount > 0 && !p.isSpoiled)
                        {
                            var serveRes = kitchenSys.ServeMeal(survivor.Id, p.itemId);
                            if (serveRes.IsSuccess)
                            {
                                served = true;
                                mealsServedToday++;
                                totalMealsServed++;
                                needsSys.Modify(survivor, NeedKind.Hunger, -35f);
                                needsSys.Modify(survivor, NeedKind.Morale, 5f);
                                break;
                            }
                        }
                    }

                    // Fallback to canned food if kitchen pantry is empty
                    if (!served && inventory.CountById("canned_food") > 0)
                    {
                        inventory.RemoveById("canned_food", 1);
                        needsSys.Modify(survivor, NeedKind.Hunger, -30f);
                        mealsServedToday++;
                        totalMealsServed++;
                    }

                    // 3. Rest & Fatigue management
                    needsSys.Modify(survivor, NeedKind.Fatigue, -60f);
                }

                // 24-hour survival needs drift
                needsSys.Tick(24f);

                // G. MASS BALANCE & INVARIANT AUDIT
                double currentGreenhouseWater = 0.0;
                if (config.EnableGreenhouse)
                {
                    for (int p = 0; p < greenhouseSys.PlotCount; p++)
                        currentGreenhouseWater += greenhouseSys.Plots[p].water;
                }

                double inFlightWater = waterSys.IsProcessing ? waterSys.State.processingTarget : 0.0;
                double currentWater = waterSys.TotalWater + inFlightWater +
                                      inventory.CountById("clean_water") +
                                      inventory.CountById("dirty_water") +
                                      inventory.CountById("irradiated_water") +
                                      currentGreenhouseWater;

                double expectedWater = initialWaterTotal + totalIncomingWater;
                double accountedWater = currentWater + totalWaterWaste + totalWaterConsumedCrew + totalCropTranspiration;
                double discrepancy = Math.Abs(expectedWater - accountedWater);

                if (discrepancy > maxWaterDiscrepancy)
                    maxWaterDiscrepancy = discrepancy;

                if (discrepancy >= 0.05)
                {
                    string err = $"[Day {day}] Water mass balance violation: expected {expectedWater:F2}, accounted {accountedWater:F2}, disc={discrepancy:F4}";
                    result.InvariantViolations.Add(err);
                    result.Success = false;
                }

                // Resource Inflation check
                int totalFoodInv = inventory.CountById("canned_food") + inventory.CountById("raw_meat") + inventory.CountById(GreenhouseExpansionCatalog.Items.CropMushroom);
                int totalPantry = 0;
                foreach (var pi in kitchenSys.State.pantry)
                {
                    if (!pi.isSpoiled) totalPantry += pi.portionCount;
                }

                if (totalFoodInv + totalPantry > 300)
                {
                    string err = $"[Day {day}] Resource inflation detected: food count {totalFoodInv + totalPantry} exceeded safe bounds.";
                    result.InvariantViolations.Add(err);
                    result.Success = false;
                }

                // Telemetry Row
                float avgHealth = 0f, avgHunger = 0f, avgThirst = 0f, avgMorale = 0f, avgWarmth = 0f;
                int aliveCount = 0;
                foreach (var s in crew)
                {
                    if (s.IsAliveState)
                    {
                        aliveCount++;
                        avgHealth += s.Health;
                        avgHunger += s.Hunger;
                        avgThirst += s.Thirst;
                        avgMorale += s.Morale;
                        avgWarmth += s.Warmth;
                    }
                }
                if (aliveCount > 0)
                {
                    avgHealth /= aliveCount;
                    avgHunger /= aliveCount;
                    avgThirst /= aliveCount;
                    avgMorale /= aliveCount;
                    avgWarmth /= aliveCount;
                }

                result.Telemetry.Add(new ResourceMassBalanceDailyTelemetry
                {
                    Day = day,
                    AvgHealth = avgHealth,
                    AvgHunger = avgHunger,
                    AvgThirst = avgThirst,
                    AvgMorale = avgMorale,
                    AvgWarmth = avgWarmth,
                    AliveCrew = aliveCount,
                    StoredWaterTotal = currentWater,
                    CleanWaterBottles = inventory.CountById("clean_water"),
                    WaterDiscrepancy = discrepancy,
                    FoodInventoryCount = totalFoodInv,
                    PantryMealPortions = totalPantry,
                    MealsServedToday = mealsServedToday,
                    FoodSpoiledToday = foodSpoiledToday,
                    FuelUnitsRemaining = powerSys.FuelUnits,
                    BatteryReserveWh = powerSys.BatteryReserveWh,
                    BrownoutHours = dailyBrownout,
                    TrappingCatchesToday = catchesToday,
                    GreenhouseHarvestsToday = cropsHarvestedToday
                });
            }

            // Summarize Results
            int finalAlive = 0;
            float sumHealth = 0f, sumHunger = 0f, sumThirst = 0f, sumMorale = 0f;
            foreach (var s in crew)
            {
                if (s.IsAliveState)
                {
                    finalAlive++;
                    sumHealth += s.Health;
                    sumHunger += s.Hunger;
                    sumThirst += s.Thirst;
                    sumMorale += s.Morale;
                }
            }

            result.SurvivorsAlive = finalAlive;
            result.TotalDeaths = config.CrewSize - finalAlive;
            result.FinalSurvivalRate = (float)finalAlive / config.CrewSize;
            result.AvgSurvivorHealth = finalAlive > 0 ? sumHealth / finalAlive : 0f;
            result.AvgSurvivorHunger = finalAlive > 0 ? sumHunger / finalAlive : 0f;
            result.AvgSurvivorThirst = finalAlive > 0 ? sumThirst / finalAlive : 0f;
            result.AvgSurvivorMorale = finalAlive > 0 ? sumMorale / finalAlive : 0f;

            result.TotalWaterInflow = totalIncomingWater;
            result.TotalWaterConsumedCrew = totalWaterConsumedCrew;
            result.TotalWaterCropTranspiration = totalCropTranspiration;
            result.TotalWaterFilterWaste = totalWaterWaste;
            result.FinalWaterStored = waterSys.TotalWater + inventory.CountById("clean_water");
            result.MaxWaterDiscrepancy = maxWaterDiscrepancy;

            result.TotalMeatProduced = totalMeatProduced;
            result.TotalCropsHarvested = totalCropsHarvested;
            result.TotalMealsServed = totalMealsServed;
            result.TotalFoodSpoiled = totalFoodSpoiled;
            result.TotalFuelBurned = totalFuelBurned;
            result.TotalBrownoutHours = totalBrownoutHours;

            // In baseline scenario, 100% survival and health > 60 is required
            if (config.ScenarioName == "Baseline")
            {
                if (result.FinalSurvivalRate < 1.0f)
                {
                    result.Success = false;
                    result.InvariantViolations.Add($"Baseline survival rate fell below 100%: {result.FinalSurvivalRate:P1} ({result.SurvivorsAlive}/{config.CrewSize})");
                }
                if (result.AvgSurvivorHealth < 60f)
                {
                    result.Success = false;
                    result.InvariantViolations.Add($"Baseline average health {result.AvgSurvivorHealth:F1} fell below safety threshold 60.0");
                }
            }

            return result;
        }
    }
}
