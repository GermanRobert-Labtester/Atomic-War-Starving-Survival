// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 196 food-type gate + storage-temperature seam against FoodPreservationSystem.
    /// KitchenNutrition is intentionally out of scope (prepared meals only).
    /// </summary>
    public sealed class Plan196FoodTypeTempSeamTests
    {
        private static FoodPreservationCatalog CreateStrictCatalog()
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
                        allowed_food_types = new List<string> { "raw_meat", "dried_rations", "vegetable" }
                    },
                    new PreservationTierDef
                    {
                        id = "preservation_salt_cured",
                        display_name = "Salt Cured",
                        shelf_life_days = 30,
                        allowed_food_types = new List<string> { "raw_meat", "dried_rations" }
                    },
                    new PreservationTierDef
                    {
                        id = "preservation_fermented",
                        display_name = "Fermented",
                        shelf_life_days = 25,
                        allowed_food_types = new List<string> { "vegetable", "tuber" }
                    }
                },
                food_type_by_item_id = new Dictionary<string, string>
                {
                    { "raw_meat", "raw_meat" },
                    { "cooked_meat", "raw_meat" },
                    { "dried_rations", "dried_rations" },
                    { "crop_leafy_green", "vegetable" },
                    { "crop_hardy_tuber", "tuber" }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static FoodPreservationSystem CreateSystem(FoodPreservationCatalog? catalog = null)
        {
            return new FoodPreservationSystem(
                new SeededRng(196),
                new Inventory.Inventory(),
                catalog ?? CreateStrictCatalog());
        }

        [Fact]
        public void AddCohort_AllowsMappedFoodType_OnTierAllowList()
        {
            var system = CreateSystem();
            var res = system.AddCohort("cooked_meat", 2, "preservation_ambient", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(2, system.GetTotalFood("cooked_meat"));
        }

        [Fact]
        public void AddCohort_BlocksFoodType_NotOnTierAllowList()
        {
            var system = CreateSystem();
            var res = system.AddCohort("crop_hardy_tuber", 3, "preservation_salt_cured", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("food_type_not_allowed", res.FailureCode);
            Assert.Equal(0, system.GetTotalFood("crop_hardy_tuber"));
        }

        [Fact]
        public void AddCohort_EmptyAllowList_RemainsUnrestricted()
        {
            var catalog = new FoodPreservationCatalog
            {
                preservation_tiers = new List<PreservationTierDef>
                {
                    new PreservationTierDef
                    {
                        id = "preservation_ambient",
                        shelf_life_days = 5
                        // allowed_food_types left empty
                    }
                }
            };
            catalog.Index();
            var system = CreateSystem(catalog);

            var res = system.AddCohort("ambient_meat", 4, "preservation_ambient", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(4, system.GetTotalFood("ambient_meat"));
        }

        [Fact]
        public void TickDay_DefaultStorageTemp_PreservesPriorAmbientDecay()
        {
            var catalog = new FoodPreservationCatalog
            {
                preservation_tiers = new List<PreservationTierDef>
                {
                    new PreservationTierDef { id = "preservation_ambient", shelf_life_days = 5 }
                }
            };
            catalog.Index();
            var system = CreateSystem(catalog);
            system.AddCohort("raw_meat", 5, "preservation_ambient", 1);

            system.TickDay(2);

            var cohort = system.State.Cohorts[0];
            // Default 10°C → factor 1.0 → 20% loss → 80% remaining (pre-196 contract).
            Assert.Equal(FoodPreservationSystem.DefaultStorageTemperatureC, system.StorageTemperatureC);
            Assert.Equal(80f, cohort.FreshnessPercent, 1);
        }

        [Fact]
        public void TickDay_ColdStorage_SlowsDecay()
        {
            var catalog = new FoodPreservationCatalog
            {
                preservation_tiers = new List<PreservationTierDef>
                {
                    new PreservationTierDef { id = "preservation_ambient", shelf_life_days = 5 }
                }
            };
            catalog.Index();
            var system = CreateSystem(catalog);
            system.AddCohort("raw_meat", 5, "preservation_ambient", 1);
            system.SetStorageTemperatureC(2f);

            system.TickDay(2);

            var cohort = system.State.Cohorts[0];
            // shelfDays = 5 * 1.5 = 7.5 → ~13.33% loss → ~86.67% remaining
            Assert.True(cohort.FreshnessPercent > 85f && cohort.FreshnessPercent < 88f);
        }

        [Fact]
        public void TickDay_WarmStorage_AcceleratesDecay()
        {
            var catalog = new FoodPreservationCatalog
            {
                preservation_tiers = new List<PreservationTierDef>
                {
                    new PreservationTierDef { id = "preservation_ambient", shelf_life_days = 5 }
                }
            };
            catalog.Index();
            var system = CreateSystem(catalog);
            system.AddCohort("raw_meat", 5, "preservation_ambient", 1);
            system.SetStorageTemperatureC(22f);

            system.TickDay(2);

            var cohort = system.State.Cohorts[0];
            // shelfDays = 5 * 0.5 = 2.5 → 40% loss → 60% remaining
            Assert.Equal(60f, cohort.FreshnessPercent, 1);
        }

        [Fact]
        public void ResolveFoodType_UsesMapThenIdentity()
        {
            var catalog = CreateStrictCatalog();
            Assert.Equal("raw_meat", catalog.ResolveFoodType("cooked_meat"));
            Assert.Equal("unknown_snack", catalog.ResolveFoodType("unknown_snack"));
        }

        [Fact]
        public void AuthoredCatalog_LoadsFoodTypeMap_AndGatesSaltCured()
        {
            var dataDir = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
                    "Assets", "StreamingAssets", "Data"));
            if (!File.Exists(Path.Combine(dataDir, "food_preservation.json")))
            {
                // Fallback: walk up from cwd (test host may differ).
                dataDir = Path.GetFullPath("Assets/StreamingAssets/Data");
            }

            Assert.True(File.Exists(Path.Combine(dataDir, "food_preservation.json")),
                $"missing food_preservation.json under {dataDir}");

            var catalog = FoodPreservationCatalogLoader.Load(dataDir, new FileSystemIO());
            Assert.True(catalog.food_type_by_item_id.Count > 0);
            Assert.Equal("vegetable", catalog.ResolveFoodType("crop_leafy_green"));

            var system = CreateSystem(catalog);
            var blocked = system.AddCohort("crop_leafy_green", 1, "preservation_salt_cured", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);

            var allowed = system.AddCohort("raw_meat", 1, "preservation_salt_cured", 1);
            Assert.Equal(ActionResult.StatusKind.Success, allowed.Status);
        }
    }
}
