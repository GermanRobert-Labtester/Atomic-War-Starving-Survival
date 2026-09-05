// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Acceptance tests for Task 43 & Task 44 (Protective Equipment Degradation & Write-back).
    /// Enforces that protective gear wears during exposure, rates differ across categories,
    /// non-protective items have documented zero rates, wear writes back to canonical EquippedItem,
    /// no duplicate wear occurs with multiple survivors per tick, and degraded state round-trips losslessly.
    /// </summary>
    public class ProtectiveGearDegradationTests
    {
        private static ItemDefinition ProtectiveDef(
            string id,
            EquipSlot slot,
            float radProtection = 30f,
            float durability = 100f,
            float authoredRate = 0f)
        {
            return new ItemDefinition
            {
                id = id,
                displayName = id,
                type = ItemType.Protective,
                isEquipable = true,
                equipSlot = slot,
                radProtection = radProtection,
                durability = durability,
                degradeRate = authoredRate
            };
        }

        [Fact]
        public void ProtectiveGear_FaceMask_DegradesAtAuthoredOrDerivedRate()
        {
            // Task 43 AC 1 & 2: Gas mask derives 1.0 durability/hr for Face filters
            var mask = ProtectiveDef("gas_mask", EquipSlot.Face, radProtection: 30f, durability: 100f);
            Assert.Equal(1.0f, mask.GetEffectiveDegradeRate());

            var inv = new InventoryContainer();
            inv.Add(mask, 1);
            Assert.True(inv.Equip(mask));

            var buffer = new List<WornGear>();
            inv.FillWornGear(buffer);
            Assert.Single(buffer);
            Assert.Equal(1.0f, buffer[0].DegradeRate);
        }

        [Fact]
        public void ProtectiveGear_BodySuit_DegradesAtAuthoredOrDerivedRate()
        {
            // Task 43 AC 1 & 2: Hazmat suit derives 0.5 durability/hr for Body suits
            var suit = ProtectiveDef("hazmat_suit", EquipSlot.Body, radProtection: 80f, durability: 100f);
            Assert.Equal(0.5f, suit.GetEffectiveDegradeRate());

            // Different gear degrades at different rates: mask wears 2x faster than heavy suit
            var mask = ProtectiveDef("gas_mask", EquipSlot.Face, radProtection: 30f, durability: 100f);
            Assert.True(mask.GetEffectiveDegradeRate() > suit.GetEffectiveDegradeRate());
        }

        [Fact]
        public void ProtectiveGear_ExplicitAuthoredRate_OverridesDerivedDefault()
        {
            // Explicit rate in item JSON or catalog takes precedence
            var customMask = ProtectiveDef("heavy_respirator", EquipSlot.Face, radProtection: 50f, durability: 100f, authoredRate: 2.5f);
            Assert.Equal(2.5f, customMask.GetEffectiveDegradeRate());
        }

        [Fact]
        public void NonProtectiveItem_ZeroRateIsIntentionalAndDocumented()
        {
            // Task 43 AC 3: Non-protective items have rate 0.0 (intentional and documented)
            var food = new ItemDefinition { id = "canned_food", type = ItemType.Food, durability = 0f };
            var weapon = new ItemDefinition { id = "pipe_rifle", type = ItemType.Tool, isEquipable = true, equipSlot = EquipSlot.Weapon, radProtection = 0f, durability = 100f };
            var scrap = new ItemDefinition { id = "scrap_metal", type = ItemType.Material, durability = 0f };

            Assert.Equal(0f, food.GetEffectiveDegradeRate());
            Assert.Equal(0f, weapon.GetEffectiveDegradeRate());
            Assert.Equal(0f, scrap.GetEffectiveDegradeRate());
        }

        [Fact]
        public void EquippedItem_CurrentDurabilityDecreases_WritesBackToCanonicalInventory()
        {
            // Task 44 AC 1: Radiation gear degradation mutates canonical EquippedItem state
            var inv = new InventoryContainer();
            var mask = ProtectiveDef("gas_mask", EquipSlot.Face, radProtection: 30f, durability: 100f);
            inv.Add(mask, 1);
            inv.Equip(mask);

            var radSystem = new RadiationSystem(exposureContext: _ =>
            {
                var buffer = new List<WornGear>();
                inv.FillWornGear(buffer);
                return new ExposureContext
                {
                    ZoneRadLevel = 40f,
                    WornGear = buffer
                };
            });

            var survivor = new SurvivorRadState { Id = "survivor_1", IsAlive = true };
            radSystem.Register(survivor);

            // 10 hours of active exposure at 40 mSv/hr
            radSystem.Tick(10f);

            var equipped = inv.GetEquipped(EquipSlot.Face);
            Assert.NotNull(equipped);
            // Gas mask degrades at 1.0/hr -> 10 hours = -10 durability
            Assert.Equal(90f, equipped!.CurrentDurability, 2);
            // Effective protection drops from 30f to 30 * 0.9 = 27f
            Assert.Equal(27f, inv.GetEquippedProtection(), 2);
        }

        [Fact]
        public void MultipleSurvivors_NoDuplicateWearPerTick()
        {
            // Task 44 AC 3: No duplicate wear application per tick when multiple survivors share exposure context
            var inv = new InventoryContainer();
            var mask = ProtectiveDef("gas_mask", EquipSlot.Face, radProtection: 30f, durability: 100f);
            inv.Add(mask, 1);
            inv.Equip(mask);

            var radSystem = new RadiationSystem(exposureContext: _ =>
            {
                var buffer = new List<WornGear>();
                inv.FillWornGear(buffer);
                return new ExposureContext
                {
                    ZoneRadLevel = 50f,
                    WornGear = buffer
                };
            });

            // 4 alive survivors simultaneously exposed
            radSystem.Register(new SurvivorRadState { Id = "s1", IsAlive = true });
            radSystem.Register(new SurvivorRadState { Id = "s2", IsAlive = true });
            radSystem.Register(new SurvivorRadState { Id = "s3", IsAlive = true });
            radSystem.Register(new SurvivorRadState { Id = "s4", IsAlive = true });

            // Tick 1 hour
            radSystem.Tick(1f);

            var equipped = inv.GetEquipped(EquipSlot.Face);
            Assert.NotNull(equipped);
            // EXACTLY 1.0 wear applied, NOT 4.0 wear!
            Assert.Equal(99f, equipped!.CurrentDurability, 3);

            // Tick 5 more hours
            radSystem.Tick(5f);
            // EXACTLY 5.0 additional wear applied
            Assert.Equal(94f, equipped.CurrentDurability, 3);
        }

        [Fact]
        public void ZeroAmbientRadiation_GearDoesNotDegrade()
        {
            // Gear only loses condition under radiation exposure
            var inv = new InventoryContainer();
            var mask = ProtectiveDef("gas_mask", EquipSlot.Face, radProtection: 30f, durability: 100f);
            inv.Add(mask, 1);
            inv.Equip(mask);

            var radSystem = new RadiationSystem(exposureContext: _ =>
            {
                var buffer = new List<WornGear>();
                inv.FillWornGear(buffer);
                return new ExposureContext
                {
                    ZoneRadLevel = 0f, // Zero radiation (clean shelter)
                    WornGear = buffer
                };
            });

            radSystem.Register(new SurvivorRadState { Id = "s1", IsAlive = true });
            radSystem.Tick(24f);

            var equipped = inv.GetEquipped(EquipSlot.Face);
            Assert.NotNull(equipped);
            Assert.Equal(100f, equipped!.CurrentDurability);
        }

        [Fact]
        public void OneHundredHourExposure_DurabilityDegradesAndReloadPreservesState()
        {
            // Task 44 AC 2 & Step 5: 100-hour exposure test with save/load round-trip
            var inv = new InventoryContainer();
            var mask = ProtectiveDef("gas_mask", EquipSlot.Face, radProtection: 30f, durability: 100f); // rate 1.0/hr
            var suit = ProtectiveDef("hazmat_suit", EquipSlot.Body, radProtection: 80f, durability: 100f); // rate 0.5/hr

            inv.Add(mask, 1);
            inv.Add(suit, 1);
            inv.Equip(mask);
            inv.Equip(suit);

            var radSystem = new RadiationSystem(exposureContext: _ =>
            {
                var buffer = new List<WornGear>();
                inv.FillWornGear(buffer);
                return new ExposureContext
                {
                    ZoneRadLevel = 50f,
                    WornGear = buffer
                };
            });
            radSystem.Register(new SurvivorRadState { Id = "s1", IsAlive = true });

            // 1. Advance 40 hours
            for (int h = 0; h < 40; h++)
                radSystem.Tick(1f);

            var maskEquipped = inv.GetEquipped(EquipSlot.Face);
            var suitEquipped = inv.GetEquipped(EquipSlot.Body);
            Assert.NotNull(maskEquipped);
            Assert.NotNull(suitEquipped);
            Assert.Equal(60f, maskEquipped!.CurrentDurability, 2); // 100 - 40 * 1.0 = 60
            Assert.Equal(80f, suitEquipped!.CurrentDurability, 2); // 100 - 40 * 0.5 = 80

            // 2. Save state
            var saveState = inv.CaptureState();
            Assert.Equal(2, saveState.equipped.Count);

            // 3. Restore state into a brand new inventory
            var itemMap = new Dictionary<string, ItemDefinition>
            {
                { "gas_mask", mask },
                { "hazmat_suit", suit }
            };
            var reloadedInv = new InventoryContainer();
            reloadedInv.RestoreState(saveState, id => itemMap.TryGetValue(id, out var def) ? def : null);

            var reloadedMask = reloadedInv.GetEquipped(EquipSlot.Face);
            var reloadedSuit = reloadedInv.GetEquipped(EquipSlot.Body);
            Assert.NotNull(reloadedMask);
            Assert.NotNull(reloadedSuit);
            // Task 44 AC 2: Reload preserves degraded value
            Assert.Equal(60f, reloadedMask!.CurrentDurability, 2);
            Assert.Equal(80f, reloadedSuit!.CurrentDurability, 2);

            // 4. Continue exposure with reloaded inventory for 60 more hours (100 hours total)
            var radSystem2 = new RadiationSystem(exposureContext: _ =>
            {
                var buffer = new List<WornGear>();
                reloadedInv.FillWornGear(buffer);
                return new ExposureContext
                {
                    ZoneRadLevel = 50f,
                    WornGear = buffer
                };
            });
            radSystem2.Register(new SurvivorRadState { Id = "s1", IsAlive = true });

            for (int h = 0; h < 60; h++)
                radSystem2.Tick(1f);

            // After 100 total hours:
            // Mask had 60 durability left - 60 hours * 1.0 = 0 durability (completely broken)
            Assert.Equal(0f, reloadedMask.CurrentDurability, 2);
            // Suit had 80 durability left - 60 hours * 0.5 = 50 durability
            Assert.Equal(50f, reloadedSuit.CurrentDurability, 2);

            // When broken (durability 0), mask provides 0 protection
            var bufferBroken = new List<WornGear>();
            reloadedInv.FillWornGear(bufferBroken);
            var maskWorn = bufferBroken.Find(w => w.SourceItem?.id == "gas_mask");
            Assert.NotNull(maskWorn);
            Assert.Equal(0f, maskWorn!.EffectiveProtection());

            // 5. Prove 0 durability persists across save/load
            var finalSave = reloadedInv.CaptureState();
            var finalInv = new InventoryContainer();
            finalInv.RestoreState(finalSave, id => itemMap.TryGetValue(id, out var def) ? def : null);
            var finalMask = finalInv.GetEquipped(EquipSlot.Face);
            var finalSuit = finalInv.GetEquipped(EquipSlot.Body);
            Assert.Equal(0f, finalMask!.CurrentDurability);
            Assert.Equal(50f, finalSuit!.CurrentDurability);
        }
    }
}
