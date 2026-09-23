// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Ecology;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class WorkingBeastExpeditionTests
    {
        private static CompanionSpeciesProfile GoatProfile() => new CompanionSpeciesProfile
        {
            species_id = "species_feral_goat",
            display_name = "Feral Goat",
            role_tags = { "pack", "morale" },
            base_food_per_day = 3,
            fallback_food_item_ids = { "crop_ash_grain", "item_grain_flour" },
            max_health = 70,
            trainability = 5,
            bond_rate = 6,
            guard_rating = 5,
            pack_capacity_kg = 25,
            morale_support_bp = 120,
            disease_resistance = 6
        };

        private static CompanionAnimalSystem CreateCompanionSystemWithGoat(
            string survivorId,
            string companionId = "domestic_goat_1",
            int trainingLevel = 0)
        {
            var system = new CompanionAnimalSystem(new[] { GoatProfile() });
            var regRes = system.RegisterCompanion(companionId, "species_feral_goat", 1);
            Assert.True(regRes.Success);

            var assignRes = system.Assign(companionId, survivorId, CompanionRole.Pack, _ => true);
            Assert.True(assignRes.Success);

            if (trainingLevel > 0)
            {
                var comp = system.Companion(companionId)!;
                comp.training_level = trainingLevel;
            }

            return system;
        }

        private static ExpeditionDefinition CreateDefinition()
        {
            return new ExpeditionDefinition
            {
                id = "exp_quarry",
                displayName = "Limestone Quarry",
                distanceTicks = 10,
                baseStaminaDrainPerHour = 2f,
                dangerLevel = 1
            };
        }

        [Fact]
        public void Default_expedition_has_baseline_foot_capacity()
        {
            var expSys = new ExpeditionSystem();
            var def = CreateDefinition();

            Assert.True(expSys.Start(def, "surv_solo", 1));
            Assert.True(expSys.Active.ContainsKey("surv_solo"));
            var active = expSys.Active["surv_solo"];
            Assert.Equal(40f, active.maxLootCapacityKg);
        }

        [Fact]
        public void Pack_companion_augments_expedition_loot_capacity_by_training_tier()
        {
            var expSys = new ExpeditionSystem();
            // Untrained goat: 25kg * 0.5 = 12.5kg bonus
            var companionSysUntrained = CreateCompanionSystemWithGoat("surv_untrained", "goat_un", trainingLevel: 0);
            expSys.SetPackCapacityBonusQuery(companionSysUntrained.GetPackCapacityBonusForSurvivor);

            var def = CreateDefinition();
            Assert.True(expSys.Start(def, "surv_untrained", 1));
            Assert.Equal(52.5f, expSys.Active["surv_untrained"].maxLootCapacityKg);

            // Expert goat: 25kg * 1.0 = 25kg bonus
            var expSys2 = new ExpeditionSystem();
            var companionSysExpert = CreateCompanionSystemWithGoat("surv_expert", "goat_ex", trainingLevel: 4);
            expSys2.SetPackCapacityBonusQuery(companionSysExpert.GetPackCapacityBonusForSurvivor);

            Assert.True(expSys2.Start(def, "surv_expert", 1));
            Assert.Equal(65f, expSys2.Active["surv_expert"].maxLootCapacityKg);
        }

        [Fact]
        public void PreviewStart_reflects_pack_animal_cargo_bonus()
        {
            var expSys = new ExpeditionSystem();
            // Expert goat: +25kg
            var companionSys = CreateCompanionSystemWithGoat("surv_handler", trainingLevel: 4);

            expSys.SetPackCapacityBonusQuery(companionSys.GetPackCapacityBonusForSurvivor);

            var def = CreateDefinition();
            var preview = expSys.PreviewStart(def, "surv_handler", 1);

            Assert.True(preview.IsAvailable);
            Assert.True(preview.ProjectedDeltas.ContainsKey("pack_capacity_bonus_kg"));
            Assert.Equal(25.0, preview.ProjectedDeltas["pack_capacity_bonus_kg"]);
            Assert.Equal(65.0, preview.ProjectedDeltas["cargo_capacity_kg"]);
        }

        [Fact]
        public void Pack_companion_enables_carrying_loot_beyond_foot_capacity()
        {
            var expSys = new ExpeditionSystem();
            // Expert goat: 40kg + 25kg = 65kg capacity
            var companionSys = CreateCompanionSystemWithGoat("surv_hauler", trainingLevel: 4);
            expSys.SetPackCapacityBonusQuery(companionSys.GetPackCapacityBonusForSurvivor);

            var def = CreateDefinition();
            Assert.True(expSys.Start(def, "surv_hauler", 1));
            var exp = expSys.Active["surv_hauler"];

            // Attempt to grant 55kg of salvage (exceeds 40kg foot limit, within 65kg pack limit)
            var status = expSys.TryGrantLoot("surv_hauler", "item_iron_beam", 55f, 1);
            Assert.Equal(ExpeditionSystem.LootGrantStatus.Granted, status);
            Assert.Equal(55f, exp.currentWeightKg);

            // Adding another 15kg would reach 70kg > 65kg -> capacity rejection
            var rejectStatus = expSys.TryGrantLoot("surv_hauler", "item_scrap", 15f, 1);
            Assert.Equal(ExpeditionSystem.LootGrantStatus.RejectedCapacity, rejectStatus);
            Assert.Equal(55f, exp.currentWeightKg);
        }

        [Fact]
        public void Dead_or_unassigned_companion_grants_zero_bonus()
        {
            var expSys = new ExpeditionSystem();
            var companionSys = CreateCompanionSystemWithGoat("surv_handler", "domestic_dead", trainingLevel: 4);

            // Kill companion
            var comp = companionSys.Companion("domestic_dead")!;
            comp.alive = false;

            expSys.SetPackCapacityBonusQuery(companionSys.GetPackCapacityBonusForSurvivor);

            var def = CreateDefinition();
            Assert.True(expSys.Start(def, "surv_handler", 1));
            var active = expSys.Active["surv_handler"];
            Assert.Equal(40f, active.maxLootCapacityKg);
        }
    }
}
