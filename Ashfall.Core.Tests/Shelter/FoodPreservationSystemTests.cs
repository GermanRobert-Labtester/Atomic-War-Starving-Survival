// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class FoodPreservationSystemTests
    {
        private static FoodPreservationCatalog CreateMockCatalog()
        {
            var catalog = new FoodPreservationCatalog
            {
                preservation_tiers = new List<PreservationTierDef>
                {
                    new PreservationTierDef
                    {
                        id = "preservation_ambient",
                        display_name = "Ambient",
                        shelf_life_days = 5,
                        shelf_life_multiplier = 1.0f
                    },
                    new PreservationTierDef
                    {
                        id = "preservation_root_cellar",
                        display_name = "Root Cellar",
                        shelf_life_days = 15,
                        shelf_life_multiplier = 3.0f
                    },
                    new PreservationTierDef
                    {
                        id = "preservation_salt_cured",
                        display_name = "Salt Cured",
                        shelf_life_days = 30,
                        shelf_life_multiplier = 6.0f
                    },
                    new PreservationTierDef
                    {
                        id = "preservation_cryogenic",
                        display_name = "Cryogenic",
                        shelf_life_days = 100,
                        shelf_life_multiplier = 20.0f,
                        power_draw_watts = 150
                    }
                },
                curing_recipes = new List<CuringRecipeDef>
                {
                    new CuringRecipeDef
                    {
                        id = "recipe_cure_salt_rations",
                        display_name = "Salt Curing",
                        target_tier = "preservation_salt_cured",
                        input_item_id = "raw_meat",
                        input_quantity = 2,
                        preservative_item_id = "chemicals",
                        preservative_quantity = 1,
                        output_item_id = "dried_rations",
                        output_quantity = 2,
                        work_ticks_required = 2
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        [Fact]
        public void AddCohort_InitializesCorrectly()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            var res = system.AddCohort("raw_meat", 10, "preservation_ambient", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(10, system.GetTotalFood("raw_meat"));
            Assert.Equal(0, system.GetSpoiledFood("raw_meat"));
        }

        [Fact]
        public void TickDay_DecaysFreshness_BasedOnTier()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            system.AddCohort("ambient_meat", 5, "preservation_ambient", 1);
            system.AddCohort("cellar_meat", 5, "preservation_root_cellar", 1);

            system.TickDay(2);

            var ambient = system.State.Cohorts.Find(c => c.FoodItemId == "ambient_meat");
            var cellar = system.State.Cohorts.Find(c => c.FoodItemId == "cellar_meat");

            Assert.NotNull(ambient);
            Assert.NotNull(cellar);

            // Ambient shelf life = 5 days -> 20% loss per day -> 80% left
            Assert.Equal(80f, ambient.FreshnessPercent, 1);
            // Cellar shelf life = 15 days -> ~6.67% loss per day -> ~93.3% left
            Assert.True(cellar.FreshnessPercent > ambient.FreshnessPercent);
        }

        [Fact]
        public void TickDay_CryogenicOutage_AcceleratesDecayAfterBuffer()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            system.AddCohort("frozen_meat", 10, "preservation_cryogenic", 1);

            // Turn off power
            system.SetPowerStatus(false);

            // Day 1 outage (buffer day): standard slow decay (1% per day)
            system.TickDay(2);
            var cohort = system.State.Cohorts.Find(c => c.FoodItemId == "frozen_meat");
            Assert.NotNull(cohort);
            Assert.Equal(99f, cohort.FreshnessPercent, 1);

            // Day 2 outage: buffer expired, decay accelerates to ambient (20% per day)
            system.TickDay(3);
            Assert.Equal(79f, cohort.FreshnessPercent, 1);
        }

        [Fact]
        public void TickDay_SpoilsCohort_WhenFreshnessReachesZero()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            system.AddCohort("raw_meat", 5, "preservation_ambient", 1);

            bool spoiledEventFired = false;
            system.OnFoodSpoiled += c => spoiledEventFired = true;

            // Ambient has 5 days shelf life
            for (int day = 2; day <= 6; day++)
            {
                system.TickDay(day);
            }

            Assert.True(spoiledEventFired);
            Assert.Equal(0, system.GetTotalFood("raw_meat"));
            Assert.Equal(5, system.GetSpoiledFood("raw_meat"));
        }

        [Fact]
        public void StartCuringJob_ConsumesInventoryAndProducesFood()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            inv.AddById("raw_meat", 5);
            inv.AddById("chemicals", 3);

            var startRes = system.StartCuringJob("recipe_cure_salt_rations", "survivor_cook", 1);
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);
            Assert.Equal(3, inv.CountById("raw_meat"));
            Assert.Equal(2, inv.CountById("chemicals"));

            // Work ticks required = 2
            system.TickDay(2);
            Assert.Single(system.State.ActiveJobs);

            system.TickDay(3);
            Assert.Empty(system.State.ActiveJobs);
            Assert.Equal(2, inv.CountById("dried_rations"));
            Assert.Equal(2, system.GetTotalFood("dried_rations"));
        }

        [Fact]
        public void ConsumeFood_FIFOOrder_PrioritizesLowestFreshness()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            system.AddCohort("dried_rations", 5, "preservation_ambient", 1);
            system.TickDay(2); // Freshness drops to 80%

            system.AddCohort("dried_rations", 5, "preservation_ambient", 2); // Freshness is 100%

            // Consume 4 units - should pull from the older (80%) cohort
            int consumed = system.ConsumeFood("dried_rations", 4, out int spoiled);
            Assert.Equal(4, consumed);
            Assert.Equal(0, spoiled);

            // Older cohort should have 1 remaining, newer should still have 5
            var older = system.State.Cohorts.Find(c => c.DayStored == 1);
            var newer = system.State.Cohorts.Find(c => c.DayStored == 2);
            Assert.NotNull(older);
            Assert.NotNull(newer);
            Assert.Equal(1, older.Quantity);
            Assert.Equal(5, newer.Quantity);
        }

        [Fact]
        public void DiscardSpoiled_RemovesOnlySpoiledCohorts()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            system.AddCohort("raw_meat", 5, "preservation_ambient", 1);
            for (int day = 2; day <= 6; day++)
                system.TickDay(day); // Spoils

            system.AddCohort("fresh_meat", 10, "preservation_root_cellar", 6);

            Assert.Equal(5, system.GetSpoiledFood());
            Assert.Equal(10, system.GetTotalFood());

            int discarded = system.DiscardSpoiled();
            Assert.Equal(5, discarded);
            Assert.Equal(0, system.GetSpoiledFood());
            Assert.Equal(10, system.GetTotalFood());
        }

        [Fact]
        public void SaveRestoreState_RoundtripsIdenticalState()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new FoodPreservationSystem(rng, inv, catalog);

            system.AddCohort("raw_meat", 8, "preservation_root_cellar", 1);
            system.SetPowerStatus(false);
            system.TickDay(2);

            var state = system.CaptureState();

            var system2 = new FoodPreservationSystem(rng, inv, catalog);
            system2.RestoreState(state);

            Assert.False(system2.IsPowerOnline);
            Assert.Equal(1, system2.UnpoweredDays);
            Assert.Equal(8, system2.GetTotalFood("raw_meat"));
        }
    }
}
