// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Greenhouse
{
    /// <summary>
    /// B5–B8 Phase 4 (Plan 64) greenhouse loop closure:
    /// - nutrient band: canonical item_hydroponic_nutrients application raises
    ///   a decaying per-plot band; a fed crop lowers blight risk (bounded,
    ///   never negative); zero-nutrient plots keep the exact legacy chance;
    /// - visible blight-risk decomposition: read-only profile matching the
    ///   TickDay roll inputs, consuming no RNG;
    /// - microclimate winter light pressure: deep-winter windows reduce
    ///   effective light; the researched capability compensates only with a
    ///   powered greenhouse room (research = permission, power = input);
    /// - save compatibility: additive nutrientLevel field (legacy restores 0).
    /// </summary>
    public class GreenhousePhase4LoopClosureTests
    {
        private static GreenhouseSystem MakePlanted(float soilContamination = 50f)
        {
            var gh = new GreenhouseSystem(seed: 4);
            gh.EnsurePlots(2);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 1, out _);
            gh.Plots[0].water = 50f; // watered
            gh.Plots[0].soilContamination = soilContamination;
            return gh;
        }

        // ─── A. Nutrient band ──────────────────────────────────────────────

        [Fact]
        public void ApplyNutrients_RaisesBand_TowardSaturation()
        {
            var gh = MakePlanted();
            Assert.True(gh.ApplyNutrients(0, out var consumed));
            Assert.Equal(GreenhouseSystem.NutrientItemId, consumed);
            Assert.Equal(GreenhouseSystem.NutrientApplicationLevel, gh.Plots[0].nutrientLevel, 3);

            gh.ApplyNutrients(0, out _);
            Assert.Equal(1f, gh.Plots[0].nutrientLevel, 3); // capped, never exceeds
        }

        [Fact]
        public void ApplyNutrients_Blocked_OnFallowAndFailedPlots()
        {
            var gh = new GreenhouseSystem(seed: 4);
            gh.EnsurePlots(2);
            Assert.False(gh.ApplyNutrients(0, out _)); // fallow

            gh.EnsurePlots(1);
            gh.Plots[0].seedItemId = GreenhouseExpansionCatalog.Items.SeedTuber;
            gh.Plots[0].stage = (int)GreenhouseStage.Failed;
            Assert.False(gh.ApplyNutrients(0, out _));
        }

        [Fact]
        public void Nutrients_LowerBlightRisk_NeverNegative()
        {
            // Low-pressure plot: contamination 10 → tiny legacy chance; full
            // nutrient band subtracts but clamps at 0.
            var gh = MakePlanted(soilContamination: 10f);
            var before = gh.GetBlightRiskProfile(0, hasWater: true);
            Assert.Equal(0f, gh.Plots[0].nutrientLevel, 3);

            gh.ApplyNutrients(0, out _);
            gh.ApplyNutrients(0, out _); // saturate
            var after = gh.GetBlightRiskProfile(0, hasWater: true);

            Assert.Equal(0f, before.NutrientReduction, 5); // legacy parity at zero nutrients
            Assert.Equal(before.FinalChancePerDay, before.FinalChancePerDay, 5);
            Assert.True(after.FinalChancePerDay <= before.FinalChancePerDay,
                "prevention may only lower risk");
            Assert.True(after.FinalChancePerDay >= 0f, "risk never negative");
            Assert.Equal(GreenhouseSystem.NutrientBlightRiskReduction, after.NutrientReduction, 5);
        }

        [Fact]
        public void NutrientBand_DecaysDaily_DosingIsRecurring()
        {
            var gh = MakePlanted();
            gh.ApplyNutrients(0, out _);
            float level = gh.Plots[0].nutrientLevel;

            gh.TickDay(1, 6f, 0f);

            Assert.True(gh.Plots[0].nutrientLevel < level, "band decays; dosing recurs");
        }

        [Fact]
        public void BlightRiskProfile_IsReadOnly_ConsumingNoRng()
        {
            var gh = MakePlanted();
            long rollsBefore = gh.State.blightRollCount;

            gh.GetBlightRiskProfile(0, hasWater: true);
            gh.GetBlightRiskProfile(0, hasWater: false);
            gh.GetBlightRiskProfile(1, hasWater: true);

            Assert.Equal(rollsBefore, gh.State.blightRollCount); // no RNG consumed
        }

        [Fact]
        public void BlightRiskProfile_FinalChance_MatchesLegacyFormula()
        {
            var gh = MakePlanted(soilContamination: 80f);
            var profile = gh.GetBlightRiskProfile(0, hasWater: false);

            var def = GreenhouseExpansionCatalog.CropCatalog.Get(GreenhouseExpansionCatalog.Items.SeedTuber)!;
            float expected = GreenhouseSystem.BaseBlightChancePerDay
                             * (1f - def.BlightResistance)
                             * Math.Clamp(80f / GreenhouseSystem.MaxContamination, 0f, 1f)
                             * 2.5f; // drought stress
            Assert.Equal(expected, profile.FinalChancePerDay, 5);
            Assert.Equal(2.5f, profile.DroughtStress, 3);
        }

        // ─── B. Microclimate winter light pressure ─────────────────────────

        [Fact]
        public void WinterLight_NonWinter_PassesThroughUnchanged()
        {
            Assert.Equal(1000, AgricultureSystem.WinterAdjustedLightPermille(1000, "window_first_thaw", false, true));
            Assert.Equal(0, AgricultureSystem.WinterAdjustedLightPermille(0, "window_dry_ash", false, false));
        }

        [Fact]
        public void WinterLight_Powered_WithoutCapability_IsPenalized()
        {
            Assert.Equal(AgricultureSystem.DeepWinterLightPermille,
                AgricultureSystem.WinterAdjustedLightPermille(1000, "window_deep_freeze", false, true));
            Assert.Equal(AgricultureSystem.DeepWinterLightPermille,
                AgricultureSystem.WinterAdjustedLightPermille(1000, "window_long_winter", false, true));
        }

        [Fact]
        public void WinterLight_WithCapability_AndPower_IsFullyCompensated()
        {
            Assert.Equal(1000, AgricultureSystem.WinterAdjustedLightPermille(1000, "window_deep_freeze", true, true));
        }

        [Fact]
        public void WinterLight_WithoutPower_IsZero_RegardlessOfCapability()
        {
            // Research never substitutes for the physical power input (§15.3).
            Assert.Equal(0, AgricultureSystem.WinterAdjustedLightPermille(0, "window_deep_freeze", true, false));
            Assert.Equal(0, AgricultureSystem.WinterAdjustedLightPermille(0, "window_long_winter", false, false));
        }

        // ─── C. Save compatibility ─────────────────────────────────────────

        [Fact]
        public void NutrientLevel_SaveRoundTrips()
        {
            var gh = MakePlanted();
            gh.ApplyNutrients(0, out _);

            var restored = new GreenhouseSystem(seed: 4);
            restored.RestoreState(gh.CaptureState());

            Assert.Equal(gh.Plots[0].nutrientLevel, restored.Plots[0].nutrientLevel, 3);
        }

        [Fact]
        public void LegacySave_WithoutNutrientLevel_RestoresZero()
        {
            const string legacy = "{\"saveId\":\"greenhouse\",\"plots\":[{\"plotIndex\":0," +
                "\"seedItemId\":\"item_seed_tuber\",\"stage\":2,\"growth\":40.5,\"water\":55.0," +
                "\"soilContamination\":12.0,\"blight\":0.0,\"plantedDay\":30}]," +
                "\"preWarWheatUnlocked\":false,\"totalHarvests\":3,\"blightRollCount\":120}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<GreenhouseState>(legacy);
            Assert.NotNull(state);

            var gh = new GreenhouseSystem(seed: 4);
            gh.RestoreState(state!);
            Assert.Equal(0f, gh.Plots[0].nutrientLevel, 3); // no free prevention
            Assert.Equal(40.5f, gh.Plots[0].growth, 2);     // legacy state intact
        }

        // ─── D. Deterministic tick-level integration (Core) ─────────────

        [Fact]
        public void SaturatedNutrients_FiveDayCycle_BlightChanceClampsToZero()
        {
            // Low-pressure plot (resistant crop, low contamination, watered):
            // legacy chance 0.06 × 0.30 × 0.1 × 1.0 = 0.0018 < reduction 0.04
            // → saturated band clamps the daily chance to exactly zero while
            // the band lasts. Deterministic: the outbreak roll can never hit,
            // but the roll stream still advances (never skipped).
            var gh = MakePlanted(soilContamination: 10f);
            gh.Plots[0].water = GreenhouseSystem.MaxWater; // 100 units: no drought across 5 days (tuber 12/day)
            gh.ApplyNutrients(0, out _);
            gh.ApplyNutrients(0, out _); // saturate

            long rollsBefore = gh.State.blightRollCount;
            for (int day = 1; day <= 5; day++)
                gh.TickDay(day, 6f, 0f);

            Assert.Equal(rollsBefore + 5, gh.State.blightRollCount);
            Assert.Equal(0f, gh.Plots[0].blight, 5); // no blight accumulated
        }
        // ─── E. Crop rotation (§27 expansion) ──────────────────────────

        [Fact]
        public void CropRotation_SameCropStreak_RaisesRisk_OtherCropResets()
        {
            var gh = MakePlanted(soilContamination: 50f);
            gh.Clear(0);

            // The bed already grew tuber (MakePlanted) — replanting tuber is a
            // repeat: the ledger continues.
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 1, out _);
            Assert.Equal(1, gh.Plots[0].sameCropStreak);

            // Same crop again (clear → plant): streak 2.
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 2, out _);
            Assert.Equal(2, gh.Plots[0].sameCropStreak);

            // Rotating to a different crop resets the bed.
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedGrain, 3, out _);
            Assert.Equal(0, gh.Plots[0].sameCropStreak);
        }

        [Fact]
        public void CropRotation_Pressure_IsVisible_InRiskProfile_AndClamped()
        {
            var gh = MakePlanted(soilContamination: 50f);
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 1, out _);
            gh.Plots[0].sameCropStreak = GreenhouseSystem.MaxRotationStreakCount; // ceiling

            var profile = gh.GetBlightRiskProfile(0, hasWater: true);
            Assert.Equal(GreenhouseSystem.RotationBlightStepPerStreak * GreenhouseSystem.MaxRotationStreakCount,
                profile.RotationPressure, 5);
            Assert.InRange(profile.FinalChancePerDay, 0f, 1f); // always clamped
            Assert.True(profile.FinalChancePerDay >= profile.ContaminationPressure * 0.018f,
                "rotation pressure only raises the risk over the non-rotation baseline");

            // Zero streak → no pressure (legacy parity).
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedGrain, 2, out _);
            var clean = gh.GetBlightRiskProfile(0, hasWater: true);
            Assert.Equal(0f, clean.RotationPressure, 5);
        }

        [Fact]
        public void CropRotation_StreakSurvivesHarvest_ButNotDifferentCrop()
        {
            var gh = MakePlanted(soilContamination: 0f);
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 1, out _);
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 2, out _); // repeat → streak 1
            gh.Plots[0].stage = (int)GreenhouseStage.Mature;

            var harvest = gh.Harvest(0);
            Assert.True(harvest.success);
            Assert.Equal(2, gh.Plots[0].sameCropStreak); // the soil remembers

            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 3, out _); // same again → 3
            Assert.Equal(3, gh.Plots[0].sameCropStreak);
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedGrain, 4, out _); // rotation → 0
            Assert.Equal(0, gh.Plots[0].sameCropStreak);
        }

        [Fact]
        public void CropRotation_SaveRoundTrips_AndLegacyRestoresZero()
        {
            var gh = MakePlanted();
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 1, out _);
            gh.Clear(0);
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 2, out _); // repeat → 2

            var restored = new GreenhouseSystem(seed: 4);
            restored.RestoreState(gh.CaptureState());
            Assert.Equal(2, restored.Plots[0].sameCropStreak);

            const string legacy = "{\"saveId\":\"greenhouse\",\"plots\":[{\"plotIndex\":0," +
                "\"seedItemId\":\"item_seed_tuber\",\"stage\":2,\"growth\":40.5,\"water\":55.0," +
                "\"soilContamination\":12.0,\"blight\":0.0,\"plantedDay\":30}]," +
                "\"preWarWheatUnlocked\":false,\"totalHarvests\":3,\"blightRollCount\":120}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<GreenhouseState>(legacy);
            var legacyGh = new GreenhouseSystem(seed: 4);
            legacyGh.RestoreState(state!);
            Assert.Equal(0, legacyGh.Plots[0].sameCropStreak); // no free pressure, no free benefit
        }
    }
}
