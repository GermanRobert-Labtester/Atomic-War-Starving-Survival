// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Journeys
{
    /// <summary>
    /// Journey J43/J44: Protective Equipment Degradation & Write-back Journey.
    /// Simulates a 7-day wasteland campaign proving that protective gear degrades under real exposure,
    /// wear updates the canonical inventory, protection scales down dynamically, multiple crew members
    /// do not multiply gear wear per tick, and saves accurately capture degraded condition.
    /// </summary>
    public class ProtectiveGearDegradationJourneyTests
    {
        [Fact]
        public void JourneyJ43_44_ProtectiveEquipment_MultiDayExposure_DegradesAndPersists()
        {
            // ── Day 1: Scavengers find and equip fresh gear ──
            var inv = new InventoryContainer();
            var mask = new ItemDefinition
            {
                id = "gas_mask",
                displayName = "Gas Mask",
                type = ItemType.Protective,
                isEquipable = true,
                equipSlot = EquipSlot.Face,
                radProtection = 30f,
                durability = 100f
            };
            var suit = new ItemDefinition
            {
                id = "hazmat_suit",
                displayName = "Hazmat Suit",
                type = ItemType.Protective,
                isEquipable = true,
                equipSlot = EquipSlot.Body,
                radProtection = 80f,
                durability = 100f
            };

            inv.Add(mask, 1);
            inv.Add(suit, 1);
            Assert.True(inv.Equip(mask));
            Assert.True(inv.Equip(suit));

            // Combined initial protection = 30 + 80 = 110 mSv/hr
            Assert.Equal(110f, inv.GetEquippedProtection(), 2);

            var resolver = new ExposureEnvironmentResolver();
            var radSystem = new RadiationSystem(exposureContext: s =>
            {
                var env = resolver.Resolve(s.Id);
                var buffer = new List<WornGear>();
                inv.FillWornGear(buffer);
                return env.ToExposureContext(buffer);
            });

            var mikhail = new SurvivorRadState { Id = "survivor_gunner_mikhail", IsAlive = true };
            var elena = new SurvivorRadState { Id = "survivor_scout_elena", IsAlive = true };
            radSystem.Register(mikhail);
            radSystem.Register(elena);

            // ── Day 1-2: Mikhail goes to ShelterPerimeter (40 mSv/hr) for 24 hours ──
            // Elena stays in ShelterInterior (0 mSv/hr)
            resolver.SetSurvivorLocation("survivor_gunner_mikhail", SurvivorExposureLocation.ShelterPerimeter);
            resolver.SetSurvivorLocation("survivor_scout_elena", SurvivorExposureLocation.ShelterInterior);

            for (int h = 0; h < 24; h++)
                radSystem.Tick(1f);

            // Mikhail was exposed, Elena was indoors.
            // Exactly 24 hours of wear applied:
            // mask: 100 - 24 * 1.0 = 76
            // suit: 100 - 24 * 0.5 = 88
            var maskEquipped = inv.GetEquipped(EquipSlot.Face);
            var suitEquipped = inv.GetEquipped(EquipSlot.Body);
            Assert.NotNull(maskEquipped);
            Assert.NotNull(suitEquipped);
            Assert.Equal(76f, maskEquipped!.CurrentDurability, 2);
            Assert.Equal(88f, suitEquipped!.CurrentDurability, 2);

            // Protection has degraded: 30 * (76/100) + 80 * (88/100) = 22.8 + 70.4 = 93.2
            float expectedProtectionDay2 = 30f * 0.76f + 80f * 0.88f;
            Assert.Equal(expectedProtectionDay2, inv.GetEquippedProtection(), 2);

            // ── Day 3: Both survivors go outdoors in 50 mSv/hr storm for 12 hours ──
            resolver.SetSurvivorLocation("survivor_gunner_mikhail", SurvivorExposureLocation.WastelandOutdoors);
            resolver.SetSurvivorLocation("survivor_scout_elena", SurvivorExposureLocation.WastelandOutdoors);

            for (int h = 0; h < 12; h++)
                radSystem.Tick(1f);

            // Crucial: 2 survivors sharing gear for 12 hours must apply exactly 12 hours of wear (NO DUPLICATE WEAR)
            // mask: 76 - 12 * 1.0 = 64
            // suit: 88 - 12 * 0.5 = 82
            Assert.Equal(64f, maskEquipped.CurrentDurability, 2);
            Assert.Equal(82f, suitEquipped.CurrentDurability, 2);

            // ── Day 4: Mid-journey save and reload ──
            var save = inv.CaptureState();
            var restoredInv = new InventoryContainer();
            var itemMap = new Dictionary<string, ItemDefinition>
            {
                ["gas_mask"] = mask,
                ["hazmat_suit"] = suit
            };
            restoredInv.RestoreState(save, id => itemMap.TryGetValue(id, out var d) ? d : null);

            var reloadedMask = restoredInv.GetEquipped(EquipSlot.Face);
            var reloadedSuit = restoredInv.GetEquipped(EquipSlot.Body);
            Assert.NotNull(reloadedMask);
            Assert.NotNull(reloadedSuit);
            Assert.Equal(64f, reloadedMask!.CurrentDurability, 2);
            Assert.Equal(82f, reloadedSuit!.CurrentDurability, 2);

            // ── Day 5-7: Long wasteland march (64 hours outdoors) ──
            var radSystem2 = new RadiationSystem(exposureContext: s =>
            {
                var env = resolver.Resolve(s.Id);
                var buffer = new List<WornGear>();
                restoredInv.FillWornGear(buffer);
                return env.ToExposureContext(buffer);
            });
            radSystem2.Register(mikhail);
            radSystem2.Register(elena);

            for (int h = 0; h < 64; h++)
                radSystem2.Tick(1f);

            // Mask had 64 durability: 64 - 64 * 1.0 = 0 (breaks)
            // Suit had 82 durability: 82 - 64 * 0.5 = 50
            Assert.Equal(0f, reloadedMask.CurrentDurability, 2);
            Assert.Equal(50f, reloadedSuit.CurrentDurability, 2);

            // When mask is 0 durability, total protection is purely suit at 50% = 80 * 0.5 = 40
            Assert.Equal(40f, restoredInv.GetEquippedProtection(), 2);
        }
    }
}
