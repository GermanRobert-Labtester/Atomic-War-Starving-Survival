// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 35 & Plan 43 Integration Tests:
// - Plan 35: Goods Must Arrive: The Production-to-Provisioning Chain & No-Silent-Loss Gate
// - Plan 43: Governing Together: Leadership, Policy & Consent
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Governance;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Production
{
    public sealed class Plan35_43ProductionGovernanceIntegrationTests : CatalogTestBase
    {
        private const string TestPoliciesJson = @"{
          ""schema_version"": 1,
          ""policies"": [
            {
              ""id"": ""policy_ration_triage"",
              ""scope"": ""rations"",
              ""proposer_rules"": ""leader_only"",
              ""decision_method"": ""executive"",
              ""default_option_id"": ""ration_standard"",
              ""options"": [
                { ""id"": ""ration_standard"", ""label"": ""Standard"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""ration_policy"", ""effect_payload"": ""Standard"" },
                { ""id"": ""ration_half"", ""label"": ""Half"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""ration_policy"", ""effect_payload"": ""Half"" }
              ]
            },
            {
              ""id"": ""policy_curfew"",
              ""scope"": ""curfew"",
              ""proposer_rules"": ""open_council"",
              ""decision_method"": ""consensus"",
              ""default_option_id"": ""curfew_disabled"",
              ""options"": [
                { ""id"": ""curfew_disabled"", ""label"": ""Disabled"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""schedule_flag"", ""effect_payload"": ""curfew_disabled"" },
                { ""id"": ""curfew_enabled"", ""label"": ""Enabled"", ""attention_cost"": 1, ""reversal_cost"": 1, ""effect_type"": ""schedule_flag"", ""effect_payload"": ""curfew_enabled"" }
              ]
            }
          ]
        }";

        [Fact]
        public void Plan35_ProductionDeliveryChain_MultiProducer_DeliversToInventorySink_WithoutSilentLoss()
        {
            var rng = new SeededRng(1337);
            var inventory = new InventoryContainer { MaxWeight = 500f };
            var needs = new NeedsSystem();

            var survivor = new SurvivorNeedsState { Id = "worker_bob", Hunger = 65f, Morale = 50f };
            needs.Register(survivor);

            // 1. Greenhouse Harvest -> Inventory Sink
            var greenhouse = new GreenhouseSystem(2026);
            greenhouse.EnsurePlots(1);
            bool planted = greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, currentDay: 1, out _);
            Assert.True(planted);
            greenhouse.Water(0, 50f, tainted: false);

            for (int day = 2; day <= 10; day++)
            {
                greenhouse.Water(0, 20f, tainted: false);
                greenhouse.TickDay(day, 8f, 0f);
            }

            var harvest = greenhouse.Harvest(0);
            Assert.True(harvest.success);
            Assert.True(harvest.amount > 0);

            var mushroomItem = new ItemDefinition { id = harvest.yieldItemId, displayName = "Harvested Mushroom", weight = 0.2f };
            inventory.Add(mushroomItem, harvest.amount);
            Assert.Equal(harvest.amount, inventory.Count(mushroomItem));

            // 2. Kitchen Meal Prep & Serving -> Pantry Portions -> Survivor Needs Modification
            var kitchen = new KitchenNutritionSystem(rng, inventory, needs);
            var cannedMeat = new ItemDefinition { id = "canned_meat", displayName = "Canned Meat", weight = 0.5f };
            inventory.Add(cannedMeat, 4);

            var prepInputs = new Dictionary<string, int> { { "canned_meat", 2 } };
            var startRes = kitchen.StartPrepJob("stew", "worker_bob", prepInputs);
            Assert.True(startRes.IsSuccess);
            Assert.Equal(2, inventory.Count(cannedMeat)); // 2 consumed

            kitchen.TickDay(1);
            Assert.NotEmpty(kitchen.State.pantry);
            Assert.Equal("stew", kitchen.State.pantry[0].itemId);

            float initialHunger = survivor.Hunger;
            var serveRes = kitchen.ServeMeal("worker_bob", "stew");
            Assert.True(serveRes.IsSuccess);
            Assert.True(survivor.Hunger < initialHunger, "Consuming meal must alleviate hunger");

            // 3. Crafting Delivery -> Finished Item In Inventory
            var crafting = new CraftingSystem(inventory);
            var scrapItem = new ItemDefinition { id = "scrap_metal", displayName = "Scrap Metal", weight = 1.0f };
            var bandItem = new ItemDefinition { id = "reinforced_band", displayName = "Reinforced Band", weight = 0.8f };
            inventory.Add(scrapItem, 3);

            var recipe = new Recipe
            {
                id = "craft_band",
                recipeName = "Reinforced Band",
                ingredients = new List<Ingredient> { new Ingredient { item = scrapItem, amount = 2 } },
                result = bandItem,
                resultAmount = 1,
                craftingTimeHours = 1f
            };

            bool craftStarted = crafting.StartCraft(recipe, null!);
            Assert.True(craftStarted);
            Assert.Equal(1, inventory.Count(scrapItem)); // 2 consumed

            crafting.Tick(2f); // complete craft
            Assert.Equal(1, inventory.Count(bandItem)); // produced into sink
        }

        [Fact]
        public void Plan35_ProductionDeliveryChain_ConstrainedInventory_RefundsAndFiresExplicitRefusal()
        {
            var inventory = new InventoryContainer();
            var itemInput = new ItemDefinition { id = "copper_wire", displayName = "Copper Wire", weight = 1f };
            var itemOutput = new ItemDefinition { id = "heavy_radiator", displayName = "Heavy Radiator", weight = 50f };

            inventory.MaxWeight = 100f;
            inventory.Add(itemInput, 5);

            var crafting = new CraftingSystem(inventory);
            var recipe = new Recipe
            {
                id = "craft_radiator",
                recipeName = "Heavy Radiator",
                ingredients = new List<Ingredient> { new Ingredient { item = itemInput, amount = 2 } },
                result = itemOutput,
                resultAmount = 1,
                craftingTimeHours = 1f
            };

            bool overflowFired = false;
            crafting.OnCraftResultOverflow += (r, resId, amt) =>
            {
                overflowFired = true;
                Assert.Equal("heavy_radiator", resId);
                Assert.Equal(1, amt);
            };

            bool craftStarted = crafting.StartCraft(recipe, null!);
            Assert.True(craftStarted);
            Assert.Equal(3, inventory.Count(itemInput)); // 2 reserved

            // Constrain capacity so output cannot fit
            inventory.MaxWeight = 5f;

            crafting.Tick(2f); // Finish craft

            // Verifications:
            // 1. Explicit overflow event emitted
            Assert.True(overflowFired, "Overflow notification must fire on sink refusal");
            // 2. Ingredients fully refunded (back to 5)
            Assert.Equal(5, inventory.Count(itemInput));
            // 3. No phantom output created
            Assert.Equal(0, inventory.Count(itemOutput));

            // 4. Persistence roundtrip test
            var state = inventory.CaptureState();
            var restored = new InventoryContainer();
            restored.RestoreState(state, id => id == "copper_wire" ? itemInput : null);
            Assert.Equal(5, restored.Count(itemInput));
        }

        [Fact]
        public void Plan43_PolicySystem_CatalogLoading_ExecutiveVsConsensus_AndLeadershipEnforcement()
        {
            var catalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();
            catalog.Load(TestPoliciesJson, serializer);

            var leadership = new LeadershipSystem
            {
                GetAliveSurvivorIds = () => new List<string> { "surv_commander", "surv_citizen" }
            };
            leadership.DesignateLeader("surv_commander");

            var policySys = new PolicySystem(catalog)
            {
                Leadership = leadership
            };

            // 1. Defaults verified
            Assert.Equal("ration_standard", policySys.GetActiveOption("rations"));
            Assert.Equal("curfew_disabled", policySys.GetActiveOption("curfew"));

            // 2. Non-leader attempting executive policy is blocked
            var failResult = policySys.SetPolicy("rations", "ration_half", "surv_citizen", day: 5);
            Assert.False(failResult.IsSuccess);
            Assert.Equal("leader_only_policy", failResult.FailureCode);

            // 3. Leader successfully enacts executive policy
            var successResult = policySys.SetPolicy("rations", "ration_half", "surv_commander", day: 5, "rations conservation");
            Assert.True(successResult.IsSuccess);
            Assert.Equal("ration_half", policySys.GetActiveOption("rations"));
            Assert.Single(policySys.History);
            Assert.Equal("surv_commander", policySys.History[0].proposer_id);

            // 4. Open council consensus policy can be enacted by citizen
            var councilResult = policySys.SetPolicy("curfew", "curfew_enabled", "surv_citizen", day: 6);
            Assert.True(councilResult.IsSuccess);
            Assert.Equal("curfew_enabled", policySys.GetActiveOption("curfew"));

            // 5. Re-enacting already active policy is rejected
            var repeatResult = policySys.SetPolicy("curfew", "curfew_enabled", "surv_commander", day: 7);
            Assert.False(repeatResult.IsSuccess);
            Assert.Equal("already_active", repeatResult.FailureCode);
        }

        [Fact]
        public void Plan43_DutyRoster_CrewConsent_BlocksRefusedRoles_AndPreservesPolicyState()
        {
            var roster = new DutyRosterSystem();
            roster.Unlock(10);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 10);
            roster.WriteName("survivor_sentry", "Sentry", "scout", DutyRosterIds.ScriptPencil, 10, true);

            // Attach crew consent rule: refuses NightWatch due to exhaustion
            roster.EvaluateCrewConsent = (survivorId, role) =>
            {
                if (survivorId == "survivor_sentry" && role == DutyRosterIds.RoleNightWatch)
                    return CrewConsentVerdict.Refused("acute_fatigue");
                return CrewConsentVerdict.Accepted();
            };

            // Preview consent
            var preview = roster.PreviewCrewConsent("survivor_sentry", DutyRosterIds.RoleNightWatch);
            Assert.False(preview.Consented);
            Assert.Equal("acute_fatigue", preview.RefusalReason);

            // Assignment rejected
            var assignResult = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_sentry");
            Assert.False(assignResult.IsSuccess);
            Assert.Equal("acute_fatigue", assignResult.FailureCode);
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleNightWatch));

            // Consented assignment succeeds
            var messResult = roster.AssignWithResult(DutyRosterIds.RoleMess, "survivor_sentry");
            Assert.True(messResult.IsSuccess);
            Assert.Equal("survivor_sentry", roster.GetAssignment(DutyRosterIds.RoleMess));

            // Policy System Save/Restore roundtrip
            var catalog = new PolicyCatalog();
            var serializer = new SystemTextJsonSerializer();
            catalog.Load(TestPoliciesJson, serializer);

            var policySys = new PolicySystem(catalog);
            policySys.SetPolicy("curfew", "curfew_enabled", "surv_chief", day: 12, "perimeter breach alert");

            var state = policySys.CaptureState();
            var restoredSys = new PolicySystem(catalog);
            restoredSys.RestoreState(state);

            Assert.Equal("curfew_enabled", restoredSys.GetActiveOption("curfew"));
            Assert.Single(restoredSys.History);
            Assert.Equal("surv_chief", restoredSys.History[0].proposer_id);
            Assert.Equal(12, restoredSys.History[0].day);
            Assert.Equal("perimeter breach alert", restoredSys.History[0].reason);
        }
    }
}
