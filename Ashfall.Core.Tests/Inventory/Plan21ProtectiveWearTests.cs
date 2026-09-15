// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Plan21ProtectiveWear
{
    /// <summary>
    /// C2 / Plan 21A — protective-wear authority contract. The repaired chain
    /// (exposure → data rate → sink → canonical EquippedItem.CurrentDurability →
    /// EffectiveProtection decay) is pinned here, plus the new 21A remainder:
    /// exactly-once failure transition (P2) and the Core remaining-life estimate
    /// (P3). WornGear stays a read projection: only the sink mutates authority.
    /// </summary>
    public sealed class Plan21ProtectiveWearTests
    {
        private static ItemDefinition Mask(float durability = 40f, float degradeRate = 1f)
        {
            return new ItemDefinition
            {
                id = "item_plan21_mask",
                displayName = "Test Mask",
                isEquipable = true,
                equipSlot = EquipSlot.Face,
                radProtection = 10f,
                durability = durability,
                degradeRate = degradeRate
            };
        }

        private static InventoryContainer EquippedInventory(ItemDefinition item)
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            inv.Add(item, 1);
            Assert.True(inv.Equip(item));
            return inv;
        }

        // ── Repaired-chain pins (should pass; guard against regression) ──

        [Fact]
        public void Exposure_WearsCanonicalEquippedItem_ThroughSink()
        {
            var mask = Mask(durability: 40f, degradeRate: 1f);
            var inv = EquippedInventory(mask);
            float before = inv.Equipped[0].CurrentDurability;

            var worn = new List<WornGear>();
            inv.FillWornGear(worn);
            var system = new RadiationSystem(exposureContext: _ => new ExposureContext
            {
                ZoneRadLevel = 30f,
                WornGear = worn
            });
            var state = new SurvivorRadState { Id = "s" };
            system.Register(state);
            system.Tick(5f);

            Assert.Equal(before - 5f, inv.Equipped[0].CurrentDurability, 3);
        }

        [Fact]
        public void ProjectionDirectFieldWrite_HasNoAuthority()
        {
            var inv = EquippedInventory(Mask());
            var worn = new List<WornGear>();
            inv.FillWornGear(worn);

            worn[0].CurrentDurability = 0f; // direct projection write is forbidden

            Assert.Equal(40f, inv.Equipped[0].CurrentDurability, 3);
        }

        [Fact]
        public void ProjectionDegrade_WritesThroughSink_ToAuthority()
        {
            var inv = EquippedInventory(Mask());
            var worn = new List<WornGear>();
            inv.FillWornGear(worn);

            worn[0].Degrade(3f); // routed through ConditionSink

            Assert.Equal(37f, inv.Equipped[0].CurrentDurability, 3);
        }

        [Fact]
        public void ZeroDurability_EffectiveProtection_Zero()
        {
            var inv = EquippedInventory(Mask());
            inv.Equipped[0].CurrentDurability = 0f;
            var worn = new List<WornGear>();
            inv.FillWornGear(worn);

            Assert.Equal(0f, worn[0].EffectiveProtection(), 3);
        }

        [Fact]
        public void AuthoredDegradeRate_OverridesFamilyDefault()
        {
            // D1 (recorded decision): authored degradeRate wins; the Core
            // family defaults are documented domain fallbacks only.
            var authored = Mask(durability: 50f, degradeRate: 2f);
            Assert.Equal(2f, authored.GetEffectiveDegradeRate(), 3);

            var fallback = Mask(durability: 50f, degradeRate: 0f);
            Assert.Equal(1f, fallback.GetEffectiveDegradeRate(), 3); // Face default
        }

        [Fact]
        public void SaveRoundTrip_PreservesWornDurability()
        {
            var inv = EquippedInventory(Mask());
            inv.RecordWear(inv.Equipped[0], 17.5f, "radiation");

            var state = inv.CaptureState();

            var restored = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            restored.RestoreState(state, id => id == "item_plan21_mask" ? Mask() : null);

            Assert.Equal(inv.Equipped[0].CurrentDurability, restored.Equipped[0].CurrentDurability, 3);
        }

        [Fact]
        public void HazmatMultiplier_HooksScaleGearWear()
        {
            var inv = EquippedInventory(Mask(durability: 100f, degradeRate: 1f));
            float before = inv.Equipped[0].CurrentDurability;
            var worn = new List<WornGear>();
            inv.FillWornGear(worn);

            var system = new RadiationSystem(
                exposureContext: _ => new ExposureContext { ZoneRadLevel = 30f, WornGear = worn },
                hazmatDegradeMultiplier: _ => 5f); // e.g. black-rain melt
            var state = new SurvivorRadState { Id = "s" };
            system.Register(state);
            system.Tick(2f);

            Assert.Equal(before - 10f, inv.Equipped[0].CurrentDurability, 3);
        }

        // ── 21A remainder: failure semantics (P2) ─────────────────────

        [Fact]
        public void RecordWear_ToZero_FiresFailureExactlyOnce()
        {
            var inv = EquippedInventory(Mask(durability: 10f, degradeRate: 1f));
            int failures = 0;
            string? causeSeen = null;
            inv.OnProtectiveGearFailed += (item, cause) => { failures++; causeSeen = cause; };

            inv.RecordWear(inv.Equipped[0], 10f, "radiation"); // → exactly 0
            Assert.Equal(1, failures);
            Assert.Equal("radiation", causeSeen);

            inv.RecordWear(inv.Equipped[0], 5f, "radiation"); // already dead — no repeat
            Assert.Equal(1, failures);
        }

        [Fact]
        public void DegradeEquippedGear_GoesThroughRecordWear_AndFiresFailureOnce()
        {
            var inv = EquippedInventory(Mask(durability: 4f, degradeRate: 2f));
            int failures = 0;
            inv.OnProtectiveGearFailed += (_, _) => failures++;

            inv.DegradeEquippedGear(3f); // 2 rate × 3 h = 6 ≥ 4 durability → dead

            Assert.Equal(0f, inv.Equipped[0].CurrentDurability, 3);
            Assert.Equal(1, failures); // one mutation API, one transition event
        }

        // ── 21A remainder: remaining-life estimate (P3) ────────────────

        [Fact]
        public void EstimateProtectiveHoursRemaining_UsesDataRate()
        {
            var inv = EquippedInventory(Mask(durability: 20f, degradeRate: 2f));
            // 20 durability / (2 per hour) = 10 hours at multiplier 1.
            Assert.True(inv.TryEstimateWeakestProtectiveLife(1f, out var life));
            Assert.Equal("item_plan21_mask", life.ItemId);
            Assert.Equal(10f, life.HoursRemaining, 3);
        }

        [Fact]
        public void EstimateProtectiveHoursRemaining_ScalesWithExposureMultiplier()
        {
            var inv = EquippedInventory(Mask(durability: 20f, degradeRate: 2f));
            Assert.True(inv.TryEstimateWeakestProtectiveLife(5f, out var life)); // black-rain melt
            Assert.Equal(2f, life.HoursRemaining, 3); // 20 / (2 × 5)
        }

        [Fact]
        public void EstimateProtectiveHoursRemaining_NoProtectiveGear_ReturnsFalse()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            Assert.False(inv.TryEstimateWeakestProtectiveLife(1f, out _));
        }

        [Fact]
        public void EstimateReflects_WeakestItemNotStrongest()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var weak = Mask(durability: 6f, degradeRate: 1f);
            weak.id = "item_plan21_weak_mask";
            var strong = Mask(durability: 90f, degradeRate: 1f);
            strong.id = "item_plan21_strong_mask";
            strong.equipSlot = EquipSlot.Body;
            inv.Add(weak, 1);
            inv.Add(strong, 1);
            Assert.True(inv.Equip(weak));
            Assert.True(inv.Equip(strong));

            Assert.True(inv.TryEstimateWeakestProtectiveLife(1f, out var life));
            Assert.Equal("item_plan21_weak_mask", life.ItemId);
            Assert.Equal(6f, life.HoursRemaining, 3);
        }
    }

    /// <summary>
    /// Source gates for the host wiring the Core contract: the weather melt
    /// multiplier must be bound (P7), the worn-gear projection buffer reused
    /// (P4), and any remaining-life display must consume the Core estimate
    /// with no panel-side arithmetic (P3, plan §3.8).
    /// </summary>
    public sealed class Plan21ProtectiveWearSourceGateTests
    {
        private static string RepoRoot
        {
            get
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8; i++)
                {
                    if (File.Exists(Path.Combine(dir, "Ashfall.csproj"))) return dir;
                    dir = Path.GetDirectoryName(dir)!;
                }
                throw new InvalidOperationException("repository root not found");
            }
        }

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void HostBinds_WeatherHazmatMeltMultiplier()
        {
            string survivors = Read("src/Main.Survivors.cs");
            Assert.Contains("HazmatDegradeMultiplier", survivors, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsSession_ReusesWornGearBuffer()
        {
            string session = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("_wornGearBuffer", session, StringComparison.Ordinal);
        }

        [Fact]
        public void RadiationDetailPanel_RemainingLifeLine_UsesCoreEstimate()
        {
            // Layering: panel → session helper → Core estimate. The Core
            // function must live in the session (one arithmetic path), never
            // in the panel (plan §3.8).
            string panel = Read("src/UI/RadiationDetailPanel.cs");
            Assert.Contains("GetWeakestProtectiveLife", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("GetEffectiveDegradeRate", panel, StringComparison.Ordinal);

            string session = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("TryEstimateWeakestProtectiveLife", session, StringComparison.Ordinal);
        }
    }
}
