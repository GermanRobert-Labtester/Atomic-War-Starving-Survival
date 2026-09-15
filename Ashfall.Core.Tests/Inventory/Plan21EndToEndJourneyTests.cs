// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Plan21ProtectiveWear
{
    /// <summary>
    /// C2 / Plan 21C (§42) — end-to-end protective-wear journey at Core level:
    /// fresh mask → hot-zone estimate predicts safety → real exposure wears the
    /// canonical item → exactly-once failure → post-failure dose rises because
    /// protection actually drops → replacement crafting (the authored repair
    /// path) restores readiness → the estimate reflects the repaired state.
    /// </summary>
    public sealed class Plan21EndToEndJourneyTests
    {
        private static ItemDefinition Mask(float durability = 24f, float degradeRate = 1f)
        {
            return new ItemDefinition
            {
                id = "item_journey_mask",
                displayName = "Journey Mask",
                isEquipable = true,
                equipSlot = EquipSlot.Face,
                radProtection = 10f,
                durability = durability,
                degradeRate = degradeRate
            };
        }

        private static ExpeditionDefinition Site(int ticks = 9)
        {
            return new ExpeditionDefinition
            {
                id = "loc_journey_hot",
                displayName = "Journey Hot Site",
                distanceTicks = ticks,
                dangerLevel = 2
            };
        }

        [Fact]
        public void Dispatch_Wear_Failure_Replace_Redispatch()
        {
            var mask = Mask(durability: 24f, degradeRate: 1f);
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            inv.Add(mask, 2); // one equipped + one spare in storage
            Assert.True(inv.Equip(mask));
            Assert.Equal(1, inv.Count(mask)); // spare remains

            // 1–2. Fresh mask (24 h life) vs a 21 h trip (9 out + 3 loot + 9 in):
            // the estimate predicts survival.
            var site = Site();
            var worn = new List<WornGear>();
            inv.FillWornGear(worn);
            var freshInputs = new ExpeditionProtectiveInputs
            {
                LocationRadRatePerHour = 30f,
                WorkingProtection = RadiationSystem.ComputeGearProtection(worn),
                WeakestGearDegradeRate = mask.GetEffectiveDegradeRate(),
                WeakestGearDurability = inv.Equipped[0].CurrentDurability
            };
            var freshEstimate = ExpeditionSystem.Estimate(site, ExpeditionStance.Stealth, protective: freshInputs);
            Assert.Equal(21f, freshEstimate.projectedTripHours, 3);
            Assert.False(freshEstimate.predictsMidRouteFailure);
            Assert.Equal(24f, freshEstimate.protectiveLifeHours, 3);

            // 3–4. Real exposure wears the canonical item; failure fires once.
            int failures = 0;
            inv.OnProtectiveGearFailed += (_, _) => failures++;
            var system = new RadiationSystem(exposureContext: _ => new ExposureContext
            {
                ZoneRadLevel = 30f,
                WornGear = worn
            });
            var survivor = new SurvivorRadState { Id = "journey" };
            system.Register(survivor);

            for (int hour = 0; hour < 25; hour++) // mask dies at hour 24
                system.Tick(1f);
            Assert.Equal(1, failures); // exactly-once failure transition
            Assert.Equal(0f, inv.Equipped[0].CurrentDurability, 3);

            // 5. Post-failure dose rate rises: protection actually dropped.
            // Hourly dose before failure: 30 − 10 = 20; after: 30 − 0 = 30.
            float before = survivor.LifetimeRadiationExposure;
            system.Tick(1f);
            float hourlyAfter = survivor.LifetimeRadiationExposure - before;
            Assert.Equal(30f, hourlyAfter, 2);

            // 6–7. Replacement crafting (the authored repair path): the spare
            // mask is equipped; condition is fresh again.
            Assert.True(inv.Equip(mask)); // storage unit → equipped slot
            Assert.Equal(1, failures); // no new failure event from equipping
            var wornAfter = new List<WornGear>();
            inv.FillWornGear(wornAfter);
            Assert.True(wornAfter[0].EffectiveProtection() > 0f);

            // 8–9. The estimate now reflects the repaired (replaced) value.
            var repairedInputs = new ExpeditionProtectiveInputs
            {
                LocationRadRatePerHour = 30f,
                WorkingProtection = RadiationSystem.ComputeGearProtection(wornAfter),
                WeakestGearDegradeRate = mask.GetEffectiveDegradeRate(),
                WeakestGearDurability = inv.Equipped[0].CurrentDurability
            };
            var repairedEstimate = ExpeditionSystem.Estimate(site, ExpeditionStance.Stealth, protective: repairedInputs);
            Assert.True(repairedEstimate.partyProtection > 0f);
            Assert.Equal(24f, repairedEstimate.protectiveLifeHours, 3);
            Assert.Equal(freshEstimate.protectiveLifeHours, repairedEstimate.protectiveLifeHours, 3);
        }

        [Fact]
        public void RouteShortening_ReducesProjectedDoseAndWear()
        {
            // Plan §42 variant: predicted mid-route failure → shorter route →
            // projection improves.
            var longTrip = ExpeditionSystem.Estimate(Site(15), ExpeditionStance.Stealth,
                protective: Inputs(degradeRate: 1f, durability: 20f)); // 33 h trip vs 20 h life
            var shortTrip = ExpeditionSystem.Estimate(Site(6), ExpeditionStance.Stealth,
                protective: Inputs(degradeRate: 1f, durability: 20f)); // 15 h trip vs 20 h life

            Assert.True(longTrip.predictsMidRouteFailure);   // life 20 < 33 h
            Assert.False(shortTrip.predictsMidRouteFailure); // life 20 ≥ 15 h
            Assert.True(shortTrip.projectedDoseTotal < longTrip.projectedDoseTotal);
            Assert.True(shortTrip.projectedGearWear < longTrip.projectedGearWear);
        }

        private static ExpeditionProtectiveInputs Inputs(
            float degradeRate = 1f, float durability = 40f)
        {
            return new ExpeditionProtectiveInputs
            {
                LocationRadRatePerHour = 30f,
                WorkingProtection = 10f,
                WeakestGearDegradeRate = degradeRate,
                WeakestGearDurability = durability
            };
        }
    }
}
