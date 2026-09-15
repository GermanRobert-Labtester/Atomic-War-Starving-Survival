// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Plan21ProtectiveWear
{
    /// <summary>
    /// C2 / Plan 21B §28 — 200-day protective-gear soak (user-authorized
    /// dedicated window). Deterministic Core harness: constant hot-zone
    /// exposure, a finite authored spare pool, immediate replacement from
    /// storage, and a day-100 save/load swap. Tracks gear lifespan, failure
    /// exactly-once, dose transition, condition bounds, and migration
    /// stability. No production data is modified.
    /// </summary>
    public sealed class Plan21LongCampaignSoakTests
    {
        private const int Days = 200;
        private const int HoursPerDay = 24;
        private const float ZoneRad = 30f;

        private static ItemDefinition MaskDef()
        {
            // Real authored values from items.json (gas_mask).
            return new ItemDefinition
            {
                id = "gas_mask",
                displayName = "Gas Mask",
                isEquipable = true,
                equipSlot = EquipSlot.Face,
                radProtection = 30f,
                durability = 100f,
                degradeRate = 1.0f
            };
        }

        private sealed class SoakState
        {
            public InventoryContainer Inv = null!;
            public RadiationSystem System = null!;
            public SurvivorRadState Survivor = null!;
            public List<WornGear> Worn = null!;
            public int Failures;
            public float MinDurabilitySeen = float.MaxValue;
        }

        private static SoakState StartCampaign(int sparePool)
        {
            var mask = MaskDef();
            var state = new SoakState
            {
                Inv = new InventoryContainer { Capacity = 99, MaxWeight = 999f },
                Survivor = new SurvivorRadState { Id = "soak" },
                Worn = new List<WornGear>()
            };
            state.Inv.Add(mask, sparePool);
            state.System = new RadiationSystem(exposureContext: _ => new ExposureContext
            {
                ZoneRadLevel = ZoneRad,
                WornGear = state.Worn
            });
            state.System.Register(state.Survivor);
            state.Inv.OnProtectiveGearFailed += (_, _) => state.Failures++;
            state.Inv.Equip(mask);
            state.Inv.FillWornGear(state.Worn);
            return state;
        }

        /// <summary>One day: 24 hourly ticks with immediate replacement from
        /// storage when the equipped mask dies. Returns whether a replacement
        /// was possible when needed.</summary>
        private static bool TickDay(SoakState s, ItemDefinition maskDef)
        {
            bool replacementAlwaysPossible = true;
            for (int hour = 0; hour < HoursPerDay; hour++)
            {
                s.System.Tick(1f);
                var equipped = s.Inv.Equipped.Count > 0 ? s.Inv.Equipped[0] : null;
                if (equipped != null)
                {
                    if (equipped.CurrentDurability < s.MinDurabilitySeen)
                        s.MinDurabilitySeen = equipped.CurrentDurability;
                    if (equipped.CurrentDurability <= 0f)
                    {
                        // Replacement cycle: broken item to storage, spare out.
                        if (s.Inv.Unequip(EquipSlot.Face) != null && s.Inv.Equip(maskDef))
                            s.Inv.FillWornGear(s.Worn);
                        else
                            replacementAlwaysPossible = false;
                    }
                }
            }
            return replacementAlwaysPossible;
        }

        [Fact]
        public void Soak200Days_Lifespan_ReplacementCount_NoUnderflow()
        {
            var s = StartCampaign(sparePool: 60);
            var maskDef = MaskDef();

            for (int day = 1; day <= Days; day++)
                TickDay(s, maskDef);

            // Each mask lasts exactly durability/rate = 100 h of exposure;
            // 4800 exposure hours → 48 masks consumed, pool not exhausted.
            Assert.Equal(48, s.Failures);
            Assert.True(s.Inv.Equipped[0].CurrentDurability > 0f);
            Assert.True(s.MinDurabilitySeen >= 0f); // no underflow

            // Protection decays continuously with durability fraction, so the
            // survivor accrues dose throughout; the acute scale saturates long
            // before the pool runs dry (no healing after failure).
            Assert.Equal(100f, s.Survivor.RadiationDose, 3);
            Assert.True(s.Survivor.LifetimeRadiationExposure > 0f);
        }

        [Fact]
        public void Soak200Days_SaveLoadAtDay100_ContinuousParity()
        {
            // The migration guarantee (plan §28): a campaign interrupted by a
            // day-100 save/load must be indistinguishable from an uninterrupted
            // run — same failure count, same lifetime dose trajectory.
            var continuous = StartCampaign(sparePool: 60);
            var swapped = StartCampaign(sparePool: 60);
            var maskDef = MaskDef();

            for (int day = 1; day <= 100; day++)
            {
                TickDay(continuous, maskDef);
                TickDay(swapped, maskDef);
            }

            // Swap through the real codec state.
            float durabilityAtSwap = swapped.Inv.Equipped[0].CurrentDurability;
            var captured = swapped.Inv.CaptureState();
            var restored = new InventoryContainer { Capacity = 99, MaxWeight = 999f };
            restored.RestoreState(captured, id => id == "gas_mask" ? MaskDef() : null);
            Assert.Equal(durabilityAtSwap, restored.Equipped[0].CurrentDurability, 2);
            swapped.Inv = restored;
            restored.OnProtectiveGearFailed += (_, _) => swapped.Failures++;
            swapped.Inv.FillWornGear(swapped.Worn);

            for (int day = 101; day <= Days; day++)
            {
                TickDay(continuous, maskDef);
                TickDay(swapped, maskDef);
            }

            Assert.Equal(continuous.Failures, swapped.Failures);
            Assert.Equal(48, continuous.Failures);
            Assert.Equal(continuous.Survivor.LifetimeRadiationExposure,
                swapped.Survivor.LifetimeRadiationExposure, 2);
            Assert.Equal(continuous.Survivor.RadiationDose,
                swapped.Survivor.RadiationDose, 2);
        }

        [Fact]
        public void Soak_IsDeterministic_PairedRunsIdentical()
        {
            string Fingerprint()
            {
                var s = StartCampaign(sparePool: 60);
                var maskDef = MaskDef();
                for (int day = 1; day <= Days; day++)
                    TickDay(s, maskDef);
                return $"failures={s.Failures};life={s.Survivor.LifetimeRadiationExposure:F2};dose={s.Survivor.RadiationDose:F2}";
            }

            Assert.Equal(Fingerprint(), Fingerprint());
        }
    }
}
