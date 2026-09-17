// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Plan22FoodAuthority
{
    public sealed class Plan22OneFoodAuthorityTests
    {
        [Fact]
        public void Consume_CannedFood_AppliesAuthoredHungerValue_NotHardcoded30()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var cannedFood = new ItemDefinition
            {
                id = "canned_food",
                displayName = "Canned Food",
                type = ItemType.Food,
                hungerRestore = 40f,
                weight = 0.5f
            };
            inv.Add(cannedFood, 1);
            Assert.Equal(1, inv.Count(cannedFood));

            float appliedHungerDelta = 0f;
            bool ok = inv.Consume(
                cannedFood,
                applyNeed: (type, delta) =>
                {
                    if (type == ItemType.Food)
                        appliedHungerDelta = delta;
                    return true;
                });

            Assert.True(ok);
            Assert.Equal(0, inv.Count(cannedFood));
            Assert.Equal(-40f, appliedHungerDelta);
            Assert.NotEqual(-30f, appliedHungerDelta); // Proves INV-22.3: not hardcoded 30
        }

        [Fact]
        public void Consume_CleanWater_AppliesAuthoredThirstValue()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var water = new ItemDefinition
            {
                id = "clean_water",
                displayName = "Clean Water",
                type = ItemType.Water,
                thirstRestore = 50f,
                weight = 0.8f
            };
            inv.Add(water, 1);

            float appliedThirstDelta = 0f;
            bool ok = inv.Consume(
                water,
                applyNeed: (type, delta) =>
                {
                    if (type == ItemType.Water)
                        appliedThirstDelta = delta;
                    return true;
                });

            Assert.True(ok);
            Assert.Equal(0, inv.Count(water));
            Assert.Equal(-50f, appliedThirstDelta);
        }

        [Fact]
        public void Consume_IrradiatedWater_AppliesContaminationAndThirst()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var dirtyWater = new ItemDefinition
            {
                id = "irradiated_water",
                displayName = "Irradiated Water",
                type = ItemType.IrradiatedWater,
                thirstRestore = 40f,
                contamination = 0.6f,
                weight = 0.8f
            };
            inv.Add(dirtyWater, 1);

            float appliedThirst = 0f;
            float appliedDose = 0f;
            bool ok = inv.Consume(
                dirtyWater,
                applyNeed: (type, delta) =>
                {
                    if (type == ItemType.Water) appliedThirst = delta;
                    return true;
                },
                applyContamination: dose => appliedDose = dose);

            Assert.True(ok);
            Assert.Equal(0, inv.Count(dirtyWater));
            Assert.Equal(-40f, appliedThirst);
            // ContaminationDosePerUnit is 50f; 0.6f * 50f = 30f dose
            Assert.Equal(30f, appliedDose, precision: 3);
        }

        [Fact]
        public void Consume_IodinePills_InvokesApplyIodine()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var iodine = new ItemDefinition
            {
                id = "iodine_pills",
                displayName = "Iodine Pills",
                type = ItemType.Iodine,
                weight = 0.05f
            };
            inv.Add(iodine, 1);

            bool iodineCalled = false;
            bool ok = inv.Consume(
                iodine,
                applyIodine: () => iodineCalled = true);

            Assert.True(ok);
            Assert.Equal(0, inv.Count(iodine));
            Assert.True(iodineCalled);
        }

        [Fact]
        public void Consume_AntiRad_AppliesRadCleanseScaled()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var antiRad = new ItemDefinition
            {
                id = "anti_rad",
                displayName = "Anti-Rad",
                type = ItemType.AntiRad,
                radCleanse = 50f,
                weight = 0.1f
            };
            inv.Add(antiRad, 1);

            float cleansed = 0f;
            bool ok = inv.Consume(
                antiRad,
                applyRadCleanse: r => cleansed = r,
                therapeuticScale: 0.8f);

            Assert.True(ok);
            Assert.Equal(0, inv.Count(antiRad));
            Assert.Equal(40f, cleansed, precision: 3); // 50 * 0.8 = 40
        }

        [Fact]
        public void Consume_FailedNeedCallback_RollsBackInventoryCount()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var food = new ItemDefinition
            {
                id = "canned_food",
                displayName = "Canned Food",
                type = ItemType.Food,
                hungerRestore = 40f,
                weight = 0.5f
            };
            inv.Add(food, 1);

            bool ok = inv.Consume(
                food,
                applyNeed: (type, delta) => false); // Refuse need modification

            Assert.False(ok);
            Assert.Equal(1, inv.Count(food)); // Rolled back atomically
        }

        [Fact]
        public void Consume_TherapeuticScale_ClampedZeroToOne()
        {
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var med = new ItemDefinition
            {
                id = "bandage",
                displayName = "Bandage",
                type = ItemType.Medical,
                healthEffect = 20f,
                weight = 0.1f
            };
            inv.Add(med, 2);

            float healthAppliedOver = 0f;
            inv.Consume(med, (t, d) => { healthAppliedOver = d; return true; }, therapeuticScale: 2.5f);
            Assert.Equal(20f, healthAppliedOver); // Clamped to 1.0f -> 20 * 1 = 20

            float healthAppliedNegative = 0f;
            inv.Consume(med, (t, d) => { healthAppliedNegative = d; return true; }, therapeuticScale: -1f);
            Assert.Equal(0f, healthAppliedNegative); // Clamped to 0.0f -> 20 * 0 = 0
        }
    }
}
