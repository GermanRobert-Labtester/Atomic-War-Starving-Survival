// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class PlanE1_28CaptivePreservationTests
    {
        private static (ShelterPrisonerSystem prisoners, FoodPreservationSystem food, Inventory.Inventory inv) CreateFixture()
        {
            var inv = new Inventory.Inventory();
            inv.TryProduce("chemicals", 10);
            inv.TryProduce("raw_meat", 10);

            var captiveCatalog = new CaptiveInterrogationCatalog
            {
                captive_archetypes = new List<CaptiveArchetypeDef>
                {
                    new CaptiveArchetypeDef
                    {
                        id = "captive_raider_scout",
                        display_name = "Raider Scout",
                        faction_origin = "faction_iron_raiders",
                        base_resistance = 40f,
                        base_hostility = 60f,
                        penal_labor_efficiency = 1.2f,
                        potential_topics = new List<string> { "topic_arms_cache" }
                    }
                },
                interrogation_topics = new List<InterrogationTopicDef>
                {
                    new InterrogationTopicDef
                    {
                        id = "topic_arms_cache",
                        display_name = "Arms Cache",
                        intel_category = "tactical_intel",
                        intel_gain = 3,
                        resistance_cost = 20f
                    }
                }
            };
            captiveCatalog.Index();

            var foodCatalog = new FoodPreservationCatalog
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
                        id = "preservation_salt_cured",
                        display_name = "Salt Cured",
                        shelf_life_days = 30,
                        shelf_life_multiplier = 6.0f
                    }
                },
                curing_recipes = new List<CuringRecipeDef>
                {
                    new CuringRecipeDef
                    {
                        id = "cure_salted_meat",
                        display_name = "Salt Curing Meat",
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
            foodCatalog.Index();

            var rng = new SeededRng(2828);
            var prisoners = new ShelterPrisonerSystem(rng, inv, captiveCatalog);
            var food = new FoodPreservationSystem(rng, inv, foodCatalog);

            return (prisoners, food, inv);
        }

        [Fact]
        public void CaptiveDetention_Capture_AssignPenalLabor_ExecutesShift()
        {
            var (prisoners, _, _) = CreateFixture();

            var capResult = prisoners.CapturePrisoner("captive_raider_scout", "Captured Marauder", currentDay: 1);
            Assert.Equal(ActionResult.StatusKind.Success, capResult.Status);

            var p = prisoners.GetPrisoner("captive_2");
            Assert.NotNull(p);
            Assert.Equal(PrisonerStatus.Detained, p.Status);

            var assignRes = prisoners.AssignPenalLabor(p.PrisonerId, PenalShiftKind.SlurryPumping);
            Assert.Equal(ActionResult.StatusKind.Success, assignRes.Status);
            Assert.Equal(PrisonerStatus.PenalLabor, p.Status);

            prisoners.TickDay(2);
            Assert.True(p.Fatigue >= 20f);
            Assert.Equal(1, prisoners.State.TotalLaborShiftsCompleted);
        }

        [Fact]
        public void CaptiveInterrogation_BreaksResistance_ExtractsIntelTopic()
        {
            var (prisoners, _, _) = CreateFixture();
            prisoners.CapturePrisoner("captive_raider_scout", "Infiltrator", currentDay: 1);

            var p = prisoners.GetPrisoner("captive_2");
            Assert.NotNull(p);

            float initialResistance = p.Resistance;

            // Firm pressure approach
            var res = prisoners.Interrogate(p.PrisonerId, "topic_arms_cache", InterrogationApproach.FirmPressure, "interrogator_1");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.True(p.Resistance < initialResistance);
            Assert.Contains("topic_arms_cache", p.ExtractedTopics);
        }

        [Fact]
        public void FoodPreservation_CuringJob_ConsumesInputsAndProducesPreservedOutput()
        {
            var (_, food, inv) = CreateFixture();

            Assert.Equal(10, inv.CountById("raw_meat"));
            Assert.Equal(10, inv.CountById("chemicals"));

            var startRes = food.StartCuringJob("cure_salted_meat", "cook_1", currentDay: 1);
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);

            // Inputs consumed immediately upon job start
            Assert.Equal(8, inv.CountById("raw_meat"));
            Assert.Equal(9, inv.CountById("chemicals"));

            // Advance days until completion (2 days required)
            food.TickDay(2);
            food.TickDay(3);

            // Output produced into inventory
            Assert.Equal(2, inv.CountById("dried_rations"));
        }

        [Fact]
        public void CaptiveAndPreservation_CaptureAndRestore_PreservesIntegrity()
        {
            var (prisoners, food, _) = CreateFixture();
            prisoners.CapturePrisoner("captive_raider_scout", "Subject Alpha", 1);
            food.StartCuringJob("cure_salted_meat", "cook_1", 1);

            var pState = prisoners.CaptureState();
            var fState = food.CaptureState();

            Assert.Single(pState.Prisoners);
            Assert.Single(fState.ActiveJobs);

            var newInv = new Inventory.Inventory();
            var newCatalog = new CaptiveInterrogationCatalog();
            var newFoodCatalog = new FoodPreservationCatalog();
            var restoredPrisoners = new ShelterPrisonerSystem(new SeededRng(1), newInv, newCatalog);
            var restoredFood = new FoodPreservationSystem(new SeededRng(1), newInv, newFoodCatalog);

            restoredPrisoners.RestoreState(pState);
            restoredFood.RestoreState(fState);

            Assert.Equal(1, restoredPrisoners.PrisonerCount);
            Assert.Single(restoredFood.State.ActiveJobs);
        }
    }
}
