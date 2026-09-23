// SPDX-License-Identifier: MIT
// Expansion 27 — The Thread : GarmentLayeringThermalEngine focused tests
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Textiles;

namespace Ashfall.Core.Tests.Textiles
{
    public sealed class GarmentLayeringThermalEngineTests
    {
        // ── 1. Full layering bonus applies when all four slots are filled ──
        [Fact]
        public void EvaluateThermalInsulation_FullLayeringBonus_WhenAllSlotsFilled()
        {
            var garments = new List<WornGarmentState>
            {
                new WornGarmentState { Layer = GarmentLayer.Base,         InsulationPermille = 100, DurabilityPermille = 1000 },
                new WornGarmentState { Layer = GarmentLayer.Mid,          InsulationPermille = 200, DurabilityPermille = 1000 },
                new WornGarmentState { Layer = GarmentLayer.Outer,        InsulationPermille = 150, DurabilityPermille = 1000 },
                new WornGarmentState { Layer = GarmentLayer.WeatherShell, InsulationPermille = 80,  DurabilityPermille = 1000,
                                        IsWaterproof = true }
            };

            var result = GarmentLayeringThermalEngine.EvaluateThermalInsulation(garments, 0);

            Assert.True(result.IsFullyLayered, "Should detect full layering");
            Assert.True(result.HasWaterproofShell, "Should detect waterproof WeatherShell");
            // Total raw = 100+200+150+80 = 530; + 150 bonus = 680
            Assert.True(result.EffectiveWarmthPermille >= 650,
                $"Expected ≥650 warmth, got {result.EffectiveWarmthPermille}");
        }

        // ── 2. Dirt accumulation causes hygiene penalty ──
        [Fact]
        public void EvaluateThermalInsulation_HygienePenalty_WhenHighlyDirty()
        {
            var garments = new List<WornGarmentState>
            {
                new WornGarmentState { Layer = GarmentLayer.Base, InsulationPermille = 300,
                                        DurabilityPermille = 1000, DirtPermille = 900 }
            };

            var result = GarmentLayeringThermalEngine.EvaluateThermalInsulation(garments, 0);

            Assert.True(result.HygienePenaltyPermille < 0,
                "Heavy dirt should produce a hygiene penalty");
        }

        // ── 3. Shelter warmth stacks with garment insulation ──
        [Fact]
        public void EvaluateThermalInsulation_ShelterStacks_WithGarmentInsulation()
        {
            var garments = new List<WornGarmentState>
            {
                new WornGarmentState { Layer = GarmentLayer.Mid, InsulationPermille = 200, DurabilityPermille = 1000 }
            };

            var noShelter  = GarmentLayeringThermalEngine.EvaluateThermalInsulation(garments, 0);
            var withShelter = GarmentLayeringThermalEngine.EvaluateThermalInsulation(garments, 400);

            Assert.True(withShelter.EffectiveWarmthPermille > noShelter.EffectiveWarmthPermille,
                "Shelter warmth should stack with garment insulation");
        }

        // ── 4. Laundry restores dirt proportional to wash quality ──
        [Fact]
        public void CalculateLaundryHygieneRestoration_RemovesDirt_ProportionalToWashQuality()
        {
            int poorWashRestore = GarmentLayeringThermalEngine.CalculateLaundryHygieneRestoration(800, 200);
            int goodWashRestore = GarmentLayeringThermalEngine.CalculateLaundryHygieneRestoration(800, 900);

            Assert.True(goodWashRestore > poorWashRestore,
                "Better wash quality should remove more dirt");
            Assert.True(poorWashRestore >= 0 && goodWashRestore >= 0,
                "Restoration must be non-negative");
        }

        // ── 5. Heavy activity causes greater daily durability loss and dirt accumulation ──
        [Fact]
        public void AdvanceDailyGarmentWear_HeavyActivity_CausesMostWearAndDirt()
        {
            var restGarment  = new WornGarmentState { Layer = GarmentLayer.Outer, DurabilityPermille = 1000 };
            var heavyGarment = new WornGarmentState { Layer = GarmentLayer.Outer, DurabilityPermille = 1000 };

            GarmentLayeringThermalEngine.AdvanceDailyGarmentWear(restGarment, ActivityLevel.Rest);
            GarmentLayeringThermalEngine.AdvanceDailyGarmentWear(heavyGarment, ActivityLevel.Heavy);

            Assert.True(heavyGarment.DurabilityPermille < restGarment.DurabilityPermille,
                "Heavy activity should cause more durability loss than rest");
            Assert.True(heavyGarment.DirtPermille > restGarment.DirtPermille,
                "Heavy activity should accumulate more dirt than rest");
        }
    }
}
