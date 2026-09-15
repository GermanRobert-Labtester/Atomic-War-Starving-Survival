// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 174 Phase 2 — host wiring contract tests.
// Proves: the save-section registry row, the CompanionSaveStore codec, the
// Core system wiring through a full save/load round trip, the guard/pack
// routing seams (bounded, host-routed), and the old-save baseline.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Ashfall.Core.Save;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Plan174Ecology
{
    public sealed class Plan174CompanionHostWiringTests
    {
        private static CompanionSpeciesProfile Hound() => new CompanionSpeciesProfile
        {
            species_id = "species_ash_hound",
            display_name = "Ash Hound",
            role_tags = { "guard", "morale" },
            base_food_per_day = 2,
            fallback_food_item_ids = { "raw_meat" },
            max_health = 80,
            trainability = 7,
            bond_rate = 9,
            guard_rating = 45,
            morale_support_bp = 250
        };

        // ── save registry contract ─────────────────────────────────────

        [Fact]
        public void SaveSectionRegistry_HasCompanionRow_WithMatchingMetadata()
        {
            var row = Assert.Single(SaveSectionRegistry.All, r => r.SectionKey == "companion_animals");
            Assert.Equal("SaveCompanionAnimals", row.SaveMethod);
            Assert.Equal("SetupCompanionAnimals", row.SetupMethod);
            Assert.Equal("hunting", row.Owner);
        }

        // ── pack seam (host-routed, bounded) ───────────────────────────

        [Fact]
        public void PackCapacityBonusForSurvivor_RoutesOnlyPackRoleCompanions()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "crop_ash_grain", 400 } };
            var goat = new CompanionSpeciesProfile
            {
                species_id = "species_feral_goat",
                display_name = "Feral Goat",
                role_tags = { "pack" },
                base_food_per_day = 3,
                fallback_food_item_ids = { "crop_ash_grain" },
                max_health = 70,
                pack_capacity_kg = 25
            };
            var system = new CompanionAnimalSystem(new[] { goat });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);

            system.RegisterCompanion("domestic_1", "species_feral_goat", 1);
            var c = system.Companion("domestic_1")!;
            c.health = 70;
            c.training_level = 4; // Expert → full training factor

            // Guard role → no pack bonus.
            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            Assert.Equal(0f, system.GetPackCapacityBonusForSurvivor("survivor_a"), 3);

            // Pack role → bounded bonus, never above authored capacity.
            system.Assign("domestic_1", "survivor_a", CompanionRole.Pack, _ => true);
            float bonus = system.GetPackCapacityBonusForSurvivor("survivor_a");
            Assert.InRange(bonus, 0f, goat.pack_capacity_kg);
        }

        // ── guard seam (DefenseSystem night detection) ────────────────

        [Fact]
        public void GuardModifierTotal_NormalizesForRaidDetection()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            system.TickDay(2);

            float total = system.GetGuardModifierTotal();
            Assert.InRange(total, 0f, Hound().guard_rating);
        }

        // ── veterinary handoff ────────────────────────────────────────

        [Fact]
        public void TreatSickness_ConsumesCanonicalItem_AndClearsTreatableStates()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "item_antibiotics", 2 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 1);
            var c = system.Companion("domestic_1")!;
            c.sickness = (int)CompanionSicknessState.Infection;

            Assert.False(system.TreatSickness("domestic_1", "item_missing_kit").Success);
            var treated = system.TreatSickness("domestic_1", "item_antibiotics");
            Assert.True(treated.Success);
            Assert.Equal(1, counts["item_antibiotics"]);
            Assert.Equal((int)CompanionSicknessState.Healthy, c.sickness);

            // Malnutrition is a feeding problem, not a kit problem.
            c.sickness = (int)CompanionSicknessState.Malnutrition;
            Assert.Equal("needs_feeding_not_treatment", system.TreatSickness("domestic_1", "item_antibiotics").ReasonCode);
        }

        // ── persistence round trip ────────────────────────────────────

        [Fact]
        public void SaveCodec_RoundTripsThroughEnvelope()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "raw_meat", 400 } };
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.BindFoodPort(id => counts.TryGetValue(id, out var n) ? n : 0, (id, a) => counts[id] -= a);
            system.RegisterCompanion("domestic_1", "species_ash_hound", 3, "Sable");
            system.Assign("domestic_1", "survivor_a", CompanionRole.Guard, _ => true);
            for (int day = 3; day <= 9; day++) system.TickDay(day);

            var state = system.CaptureState();
            var json = new SystemTextJsonSerializer();
            string persisted = SchemaVersionedEnvelope<CompanionSystemState>.Encode(state, json);
            var decoded = SchemaVersionedEnvelope<CompanionSystemState>.Decode(persisted, json);

            Assert.NotNull(decoded);
            Assert.Equal(state.companions.Count, decoded!.companions.Count);
            Assert.Equal(state.companions[0].companion_id, decoded.companions[0].companion_id);
            Assert.Equal(state.companions[0].bond, decoded.companions[0].bond);
            Assert.Equal(state.companions[0].training_level, decoded.companions[0].training_level);
        }

        [Fact]
        public void OldSaveBaseline_NoCompanionsSection_YieldsEmptySystem()
        {
            // Old saves never carried a companion section: restore(null) → clean,
            // adoptable-from-wildlife baseline with no fabricated animals (§10).
            var system = new CompanionAnimalSystem(new[] { Hound() });
            system.RestoreState(null);
            Assert.Empty(system.State.companions);
            Assert.Equal(0f, system.GetGuardModifierTotal(), 3);
            Assert.Equal(0f, system.GetPackCapacityBonusForSurvivor("anyone"), 3);
        }
    }
}
