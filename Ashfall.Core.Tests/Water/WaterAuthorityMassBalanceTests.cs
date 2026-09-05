// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Production;
using Xunit;

namespace Ashfall.Core.Tests.Water
{
    /// <summary>
    /// REM-008 / R14 — Single physical water authority and mass balance tests.
    ///
    /// Verifies:
    ///   1. DrawWater packages fluid into inventory items via IOutputSink and conserves mass.
    ///   2. DrawWater when inventory is full refuses delivery and leaves reservoir intact.
    ///   3. PourWater unpackages inventory water into bulk reservoir and conserves mass.
    ///   4. PourWater when insufficient inventory refuses and leaves reservoir intact.
    ///   5. ConsumeRation draws from both plant reservoir and packaged bottles, prioritizing clean water.
    ///   6. ConsumeRation with Irradiated policy draws irradiated water first with exposure callbacks.
    ///   7. Save/reload mid-transfer preserves mass and in-flight processing state.
    ///   8. Mass balance property test across 200 simulated days closes exactly.
    /// </summary>
    public sealed class WaterAuthorityMassBalanceTests
    {
        private static ItemDefinition LookupItem(string id) =>
            new ItemDefinition { id = id, displayName = id, stackMax = 99, weight = 0.5f };

        [Fact]
        public void DrawWater_PackagesFluidIntoInventory_ConservesMass()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 30f);
            var inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };

            var result = sys.DrawWater(WaterType.Clean, 8, inventory);

            Assert.True(result.IsSuccess);
            Assert.Equal(22f, sys.CleanWater);
            Assert.Equal(8, inventory.CountById("clean_water"));
            Assert.Equal(30f, sys.CleanWater + inventory.CountById("clean_water"));
        }

        [Fact]
        public void DrawWater_WhenInventoryFull_RefusesAndReservoirIsUnchanged()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 20f);
            var inventory = new Inventory.Inventory { Capacity = 1, MaxWeight = 100f };
            inventory.TryProduce("scrap_metal", 1);

            var result = sys.DrawWater(WaterType.Clean, 5, inventory);

            Assert.False(result.IsSuccess);
            Assert.Equal(20f, sys.CleanWater);
            Assert.Equal(0, inventory.CountById("clean_water"));
        }

        [Fact]
        public void PourWater_UnpackagesInventoryIntoReservoir_ConservesMass()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 10f);
            var inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inventory.TryProduce("clean_water", 5);

            var result = sys.PourWater(WaterType.Clean, 4, inventory);

            Assert.True(result.IsSuccess);
            Assert.Equal(14f, sys.CleanWater);
            Assert.Equal(1, inventory.CountById("clean_water"));
            Assert.Equal(15f, sys.CleanWater + inventory.CountById("clean_water"));
        }

        [Fact]
        public void PourWater_WhenInsufficientInventory_RefusesAndReservoirIsUnchanged()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 10f);
            var inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inventory.TryProduce("clean_water", 2);

            var result = sys.PourWater(WaterType.Clean, 5, inventory);

            Assert.False(result.IsSuccess);
            Assert.Equal(10f, sys.CleanWater);
            Assert.Equal(2, inventory.CountById("clean_water"));
        }

        [Fact]
        public void ConsumeRation_DrawsFromBothPlantAndInventory_PrioritizingClean()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 3f);
            sys.AddWater(WaterType.Raw, 10f);

            var inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inventory.TryProduce("clean_water", 4);
            inventory.TryProduce("dirty_water", 2);

            // Needed: 5L. Should take 3L clean from plant + 2 clean bottles from inventory.
            var result = sys.ConsumeRation(5f, inventory);

            Assert.True(result.IsSuccess);
            Assert.Equal(0f, sys.CleanWater);
            Assert.Equal(2, inventory.CountById("clean_water"));
            Assert.Equal(10f, sys.RawWater);
            Assert.Equal(2, inventory.CountById("dirty_water"));
            Assert.True(result.Deltas.ContainsKey("consumed"));
            Assert.Equal(5.0, result.Deltas["consumed"], precision: 3);
        }

        [Fact]
        public void ConsumeRation_IrradiatedPolicy_ConsumesIrradiatedWithExposure()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 5f);
            sys.AddWater(WaterType.Irradiated, 2f);

            var inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inventory.TryProduce("irradiated_water", 3);

            float radDose = 0f;
            sys.OnRadiationExposure += d => radDose += d;

            // Needed: 4L irradiated. Takes 2L from plant + 2 from inventory.
            var result = sys.ConsumeRation(4f, inventory, forceIrradiated: true);

            Assert.True(result.IsSuccess);
            Assert.Equal(0f, sys.IrradiatedWater);
            Assert.Equal(1, inventory.CountById("irradiated_water"));
            Assert.Equal(5f, sys.CleanWater); // clean water preserved
            Assert.True(radDose > 0f);
        }

        [Fact]
        public void SaveAndReload_MidTransfer_PreservesMassAndState()
        {
            var sys1 = new WaterTreatmentSystem();
            sys1.AddWater(WaterType.Raw, 50f);
            sys1.AddWater(WaterType.Clean, 15f);
            sys1.State.charcoalSupply = 50f;

            var inv1 = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };

            // Start treatment of 20L raw water
            var startRes = sys1.StartTreatment(TreatmentMode.CharcoalFiltration, 20f);
            Assert.True(startRes.IsSuccess);
            Assert.True(sys1.IsProcessing);

            // Draw 5L clean water into inventory
            var drawRes = sys1.DrawWater(WaterType.Clean, 5, inv1);
            Assert.True(drawRes.IsSuccess);

            // Capture state
            var savedPlant = sys1.CaptureState();
            var savedInv = inv1.CaptureState();

            // Restore into fresh instances
            var sys2 = new WaterTreatmentSystem();
            sys2.RestoreState(savedPlant);

            var inv2 = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv2.RestoreState(savedInv, LookupItem);

            Assert.True(sys2.IsProcessing);
            Assert.Equal(10f, sys2.CleanWater);
            Assert.Equal(30f, sys2.RawWater);
            Assert.Equal(5, inv2.CountById("clean_water"));

            // Advance day to complete treatment
            sys2.TickDay(1);
            Assert.False(sys2.IsProcessing);

            // 20L * 0.85 = 17L clean output, 3L waste
            Assert.Equal(27f, sys2.CleanWater);
            var completedJob = sys2.State.completedJobs.Last();
            Assert.Equal(17f, completedJob.cleanOutput);
            Assert.Equal(3f, completedJob.wasteAmount);

            // Total mass check:
            // Input was: 50L raw + 15L clean = 65L.
            // Current is: 30L raw + 27L clean + 5L packaged + 3L waste = 65L.
            float total = sys2.RawWater + sys2.CleanWater + inv2.CountById("clean_water") + completedJob.wasteAmount;
            Assert.Equal(65f, total, precision: 3);
        }

        [Fact]
        public void MassBalancePropertyTest_Across200SimulatedDays()
        {
            var rng = new SeededRng(1984);
            var sys = new WaterTreatmentSystem();
            var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 500f };

            // Initial stocks
            sys.AddWater(WaterType.Clean, 100f);
            sys.AddWater(WaterType.Raw, 50f);
            sys.AddWater(WaterType.Brackish, 30f);
            sys.AddWater(WaterType.Irradiated, 20f);
            sys.State.charcoalSupply = 500f;
            sys.State.distillationFuel = 500f;

            inv.TryProduce("clean_water", 20);
            inv.TryProduce("dirty_water", 10);
            inv.TryProduce("irradiated_water", 5);

            double initialWater = sys.TotalWater + inv.CountById("clean_water") + inv.CountById("dirty_water") + inv.CountById("irradiated_water");
            double totalIncoming = 0.0;
            double totalWaste = 0.0;
            double totalConsumed = 0.0;

            for (int day = 1; day <= 200; day++)
            {
                // 1. Stochastic incoming water (e.g. rainfall, well seep, scavenger finds)
                if (day % 3 == 0)
                {
                    float rainLiters = rng.Next(5, 15);
                    sys.AddWater(WaterType.Raw, rainLiters);
                    totalIncoming += rainLiters;
                }
                if (day % 7 == 0)
                {
                    int foundBottles = rng.Next(1, 4);
                    inv.TryProduce("clean_water", foundBottles);
                    totalIncoming += foundBottles;
                }

                // 2. Start treatment if idle and supplies available
                if (!sys.IsProcessing && sys.RawWater >= 10f && sys.State.charcoalSupply >= 5f)
                {
                    sys.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);
                }
                else if (!sys.IsProcessing && sys.BrackishWater >= 10f && sys.State.distillationFuel >= 5f)
                {
                    sys.StartTreatment(TreatmentMode.Distillation, 10f);
                }

                // 3. Player conversions (draw or pour)
                if (sys.CleanWater >= 10f && inv.CountById("clean_water") < 30)
                {
                    int drawAmount = rng.Next(1, 5);
                    sys.DrawWater(WaterType.Clean, drawAmount, inv, day);
                }
                else if (inv.CountById("clean_water") > 35)
                {
                    int pourAmount = rng.Next(1, 4);
                    sys.PourWater(WaterType.Clean, pourAmount, inv);
                }

                // 4. Daily crew ration consumption
                float crewNeed = rng.Next(3, 6);
                var rationRes = sys.ConsumeRation(crewNeed, inv);
                if (rationRes.Deltas != null && rationRes.Deltas.TryGetValue("consumed", out var consumedVal))
                {
                    totalConsumed += consumedVal;
                }

                // 5. Check if a treatment job completed during this tick
                int completedJobsBefore = sys.State.completedJobs.Count;
                sys.TickDay(day);
                int completedJobsAfter = sys.State.completedJobs.Count;

                if (completedJobsAfter > completedJobsBefore)
                {
                    for (int j = completedJobsBefore; j < completedJobsAfter; j++)
                    {
                        totalWaste += sys.State.completedJobs[j].wasteAmount;
                    }
                }

                // 6. Conservation of Mass Invariant Verification
                double inFlightWater = sys.IsProcessing ? sys.State.processingTarget : 0.0;
                double currentWater = sys.TotalWater + inFlightWater +
                                      inv.CountById("clean_water") +
                                      inv.CountById("dirty_water") +
                                      inv.CountById("irradiated_water");

                double expectedWater = initialWater + totalIncoming;
                double accountedWater = currentWater + totalWaste + totalConsumed;

                double discrepancy = Math.Abs(expectedWater - accountedWater);
                Assert.True(discrepancy < 0.01,
                    $"[Day {day}] Water mass balance violation: expected {expectedWater:F3}, accounted {accountedWater:F3} (discrepancy: {discrepancy:F4})");
            }
        }

        [Fact]
        public void DrawWater_PartialDelivery_ConservesExactMassDrawn()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 20f);
            var inventory = new Inventory.Inventory { Capacity = 10, MaxWeight = 1.5f };
            inventory.TryProduce("scrap_metal", 1); // 1.0f weight occupied, 0.5f free

            // Requests 5L, but inventory can only accept 1 unit (0.5f weight free, 0.5f per bottle)
            var result = sys.DrawWater(WaterType.Clean, 5, inventory);

            Assert.True(result.IsSuccess);
            Assert.Equal(19f, sys.CleanWater);
            Assert.Equal(1, inventory.CountById("clean_water"));
            Assert.Equal(20f, sys.CleanWater + inventory.CountById("clean_water"));
        }

        [Fact]
        public void ConsumeRation_WhenBulkCleanDepleted_DrawsFromPackagedClean_ThenRaw()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Clean, 0f);
            sys.AddWater(WaterType.Raw, 5f);
            var inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inventory.TryProduce("clean_water", 2);

            // Needed: 3L. 0 in bulk clean -> takes 2 clean from inventory -> 1 remaining taken from bulk raw
            var result = sys.ConsumeRation(3f, inventory);

            Assert.True(result.IsSuccess);
            Assert.Equal(0, inventory.CountById("clean_water"));
            Assert.Equal(4f, sys.RawWater);
            Assert.Equal(3.0, result.Deltas["consumed"], precision: 3);
            Assert.True(result.Deltas["contamination_exposure"] > 0);
        }
    }
}
