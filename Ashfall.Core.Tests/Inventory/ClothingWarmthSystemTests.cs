// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ClothingWarmthSystemTests
    {
        [Fact]
        public void Defaults_ProfilesRegisteredAcrossLayers()
        {
            var system = new ClothingWarmthSystem();

            Assert.True(system.Profiles.ContainsKey("item_ragged_coat"));
            Assert.True(system.Profiles.ContainsKey("item_winter_coat"));
            Assert.True(system.Profiles.ContainsKey("item_thermal_underwear"));
            Assert.True(system.Profiles.ContainsKey("item_fur_boots"));
            Assert.True(system.Profiles.ContainsKey("item_hazmat_cold_suit"));
            Assert.True(system.Profiles.ContainsKey("item_arctic_survival_suit"));

            var arctic = system.Profiles["item_arctic_survival_suit"];
            Assert.True(arctic.is_waterproof);
            Assert.Equal(ClothingLayer.Outer, arctic.layer);
            Assert.Equal(6000, arctic.cold_mitigation_bp); // 60% base
        }

        [Fact]
        public void EquipClothing_LayersItemsCorrectly_AndReplacesSameLayer()
        {
            var system = new ClothingWarmthSystem();

            // Equip ragged coat (Outer)
            system.EquipClothing("surv_alice", "item_ragged_coat");
            var equipped1 = system.GetEquipped("surv_alice");
            Assert.Single(equipped1);
            Assert.Equal("item_ragged_coat", equipped1[0].item_id);

            // Equip winter coat (Outer) -> should replace ragged coat
            system.EquipClothing("surv_alice", "item_winter_coat");
            var equipped2 = system.GetEquipped("surv_alice");
            Assert.Single(equipped2);
            Assert.Equal("item_winter_coat", equipped2[0].item_id);

            // Equip thermal underwear (Underwear) and fur boots (Accessory)
            system.EquipClothing("surv_alice", "item_thermal_underwear");
            system.EquipClothing("surv_alice", "item_fur_boots");
            var equipped3 = system.GetEquipped("surv_alice");
            Assert.Equal(3, equipped3.Count);
        }

        [Fact]
        public void CalculateColdLossReduction_AppliesConditionAndWetnessPenalties()
        {
            var system = new ClothingWarmthSystem();

            // Pristine winter coat: 2500 bp (25% reduction)
            system.EquipClothing("surv_bob", "item_winter_coat", condition: 1.0f);
            float pristineReduction = system.CalculateColdLossReduction("surv_bob");
            Assert.Equal(0.25f, pristineReduction, 2);

            // Degraded coat (condition 0.5f) -> 12.5% reduction
            system.EquipClothing("surv_bob", "item_winter_coat", condition: 0.5f);
            float degradedReduction = system.CalculateColdLossReduction("surv_bob");
            Assert.Equal(0.125f, degradedReduction, 2);

            // Soak clothing (wetness 1.0f): non-waterproof suffers 50% penalty
            system.EquipClothing("surv_bob", "item_winter_coat", condition: 1.0f);
            system.ApplyWetness("surv_bob", 1.0f);
            float soakedReduction = system.CalculateColdLossReduction("surv_bob");
            // 25% * (1 - 0.5) = 12.5%
            Assert.Equal(0.125f, soakedReduction, 2);
        }

        [Fact]
        public void NeedsSystem_Integration_ClothingReducesWarmthLossRate()
        {
            var clothingSystem = new ClothingWarmthSystem();

            var profile = new NeedsProfile
            {
                warmthLossPerHourInCold = 10f,
                warmthRestorePerHourNearHeat = 15f
            };

            // Setup two identical survivors in cold (not near heat)
            var needsSystem = new NeedsSystem(profile, isNearHeatSource: _ => false);
            needsSystem.ClothingWarmthReductionProvider = id => clothingSystem.CalculateColdLossReduction(id);

            var nakedSurvivor = new SurvivorNeedsState { Id = "surv_naked", Warmth = 100f };
            var bundledSurvivor = new SurvivorNeedsState { Id = "surv_bundled", Warmth = 100f };

            needsSystem.Register(nakedSurvivor);
            needsSystem.Register(bundledSurvivor);

            // Bundle up survivor with winter coat (25%) + thermal underwear (15%) + fur boots (12%) = 52% reduction
            clothingSystem.EquipClothing("surv_bundled", "item_winter_coat");
            clothingSystem.EquipClothing("surv_bundled", "item_thermal_underwear");
            clothingSystem.EquipClothing("surv_bundled", "item_fur_boots");

            // Tick 2 hours in freezing weather
            needsSystem.Tick(2f);

            float nakedWarmth = nakedSurvivor.Warmth;
            float bundledWarmth = bundledSurvivor.Warmth;

            // Naked survivor lost 10f * 2 = 20f -> Warmth is 80f
            Assert.Equal(80f, nakedWarmth, 1);

            // Bundled survivor had ~52% reduction, so lost only ~48% of 20f = 9.6f -> Warmth is ~90.4f
            Assert.True(bundledWarmth > nakedWarmth);
            Assert.Equal(90.4f, bundledWarmth, 1);
        }

        [Fact]
        public void WaterproofOuter_ResistsWetness_AndProtectsInsulation()
        {
            var system = new ClothingWarmthSystem();

            // Normal winter coat vs Hazmat cold suit (waterproof)
            system.EquipClothing("surv_regular", "item_winter_coat");
            system.EquipClothing("surv_hazmat", "item_hazmat_cold_suit");

            // Apply 0.5f wetness storm exposure
            system.ApplyWetness("surv_regular", 0.5f);
            system.ApplyWetness("surv_hazmat", 0.5f);

            // Regular coat took full 0.5f wetness
            Assert.Equal(0.5f, system.State.survivors["surv_regular"].wetness, 2);

            // Hazmat resisted 80% wetness -> took only 0.1f wetness
            Assert.Equal(0.1f, system.State.survivors["surv_hazmat"].wetness, 2);
        }

        [Fact]
        public void DryClothing_RestoresWarmthEfficiencyNearHeat()
        {
            var system = new ClothingWarmthSystem();

            system.EquipClothing("surv_charlie", "item_winter_coat");
            system.ApplyWetness("surv_charlie", 1.0f);

            float wetReduction = system.CalculateColdLossReduction("surv_charlie");
            Assert.Equal(0.125f, wetReduction, 2);

            // Dry clothing near heat source for 3 hours
            system.DryClothing("surv_charlie", 3f);
            Assert.Equal(0f, system.State.survivors["surv_charlie"].wetness, 2);

            float driedReduction = system.CalculateColdLossReduction("surv_charlie");
            Assert.Equal(0.25f, driedReduction, 2);
        }

        [Fact]
        public void SaveLoad_RoundTrip_PreservesEquippedAndConditions()
        {
            var sys1 = new ClothingWarmthSystem();
            sys1.EquipClothing("surv_dave", "item_hazmat_cold_suit", condition: 0.85f);
            sys1.EquipClothing("surv_dave", "item_thermal_balaclava", condition: 0.90f);
            sys1.ApplyWetness("surv_dave", 0.35f);

            var saved = sys1.CaptureState();

            var sys2 = new ClothingWarmthSystem();
            sys2.RestoreState(saved);

            var equipped = sys2.GetEquipped("surv_dave");
            Assert.Equal(2, equipped.Count);
            Assert.Equal("item_hazmat_cold_suit", equipped[0].item_id);
            Assert.Equal(0.85f, equipped[0].condition, 2);
            Assert.Equal(0.07f, sys2.State.survivors["surv_dave"].wetness, 2); // 0.35 * 0.2 due to waterproof
            Assert.True(sys2.CalculateColdLossReduction("surv_dave") > 0.40f);
        }
    }
}
