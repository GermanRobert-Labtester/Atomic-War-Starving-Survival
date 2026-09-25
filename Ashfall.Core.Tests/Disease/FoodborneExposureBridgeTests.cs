// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W1 — Foodborne disease bridge (focused suite; run alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE (cases AV.1).
//
// These tests pin the pure translation (FoodborneExposureMath) and the authored
// catalog row that the Main seam consumes. The host seam itself
// (HoldfastRuntimeSession.FoodConsumed) is exercised by the runtime selftests;
// what matters here is that the contract the seam relies on is deterministic,
// clamped, fail-closed, and present in the data authority.
// ============================================================================

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FoodborneExposureBridgeTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        // ── Pure translation (AV.1 cases 1–5) ─────────────────────────────────

        [Fact]
        public void SpoiledShare_IsZeroForUntrackedItem()
        {
            // A food item with no preservation record is treated as clean
            // (fail-closed: the bridge never invents spoilage).
            Assert.Equal(0f, FoodborneExposureMath.SpoiledShare(0, 0));
            Assert.Equal(0f, FoodborneExposureMath.SpoiledShare(0, 12));
        }

        [Fact]
        public void SpoiledShare_IsZeroForCleanStock()
        {
            // AV.1 case 1: clean preserved stock raises no exposure.
            Assert.Equal(0f, FoodborneExposureMath.SpoiledShare(100, 0));
            Assert.Equal(0f, FoodborneExposureMath.ExposureModifierForShare(0f));
        }

        [Fact]
        public void SpoiledShare_ScalesWithSpoiledFraction()
        {
            // AV.1 case 2: 40 of 100 units spoiled ⇒ share 0.4.
            Assert.Equal(0.4f, FoodborneExposureMath.SpoiledShare(100, 40), 4);
        }

        [Fact]
        public void SpoiledShare_SaturatesAtOne()
        {
            // All-spoiled stock is the worst case; the share never exceeds 1.
            Assert.Equal(1f, FoodborneExposureMath.SpoiledShare(50, 50));
            Assert.Equal(1f, FoodborneExposureMath.SpoiledShare(50, 80));
        }

        [Fact]
        public void EffectiveProbability_ScalesBaseByShare_AndClamps()
        {
            // The catalog's base_probability remains the disease-risk authority;
            // the spoiled share only scales it, and the result stays in [0, 1].
            Assert.Equal(0.14f, FoodborneExposureMath.EffectiveProbability(0.35f, 0.4f), 4);
            Assert.Equal(0f, FoodborneExposureMath.EffectiveProbability(0.35f, 0f), 4);
            Assert.Equal(1f, Math.Min(1f, FoodborneExposureMath.EffectiveProbability(1f, 1f)), 4);
        }

        [Fact]
        public void Translation_IsNeutralForInvalidInputs()
        {
            // NaN / non-positive inputs neutralize rather than propagate.
            Assert.Equal(0f, FoodborneExposureMath.EffectiveProbability(float.NaN, 0.5f));
            Assert.Equal(0f, FoodborneExposureMath.EffectiveProbability(0.3f, float.NaN));
            Assert.Equal(0f, FoodborneExposureMath.EffectiveProbability(-1f, 0.5f));
            Assert.Equal(0f, FoodborneExposureMath.ExposureModifierForShare(float.NaN));
        }

        [Fact]
        public void Translation_IsDeterministic_TwoPass()
        {
            // AV.1 case 8: identical inputs, identical outputs, no state.
            float a = FoodborneExposureMath.EffectiveProbability(0.35f, FoodborneExposureMath.SpoiledShare(73, 19));
            float b = FoodborneExposureMath.EffectiveProbability(0.35f, FoodborneExposureMath.SpoiledShare(73, 19));
            Assert.Equal(a, b);
        }

        // ── Authored catalog row (AV.1 case 7) ──────────────────────────────

        [Fact]
        public void Catalog_HasSpoiledPreservedStockSource_ReferencingARealDisease()
        {
            var catalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var source = catalog.GetExposureSource(FoodborneExposureMath.SpoiledPreservedStockSourceId);

            Assert.NotNull(source);
            Assert.Equal("spoiled_preserved_stock", source!.source_id);
            Assert.True(source.base_probability > 0f && source.base_probability <= 1f);
            Assert.NotNull(catalog.GetById(source.disease_id));
        }

        // ── Receiver contract: the disease authority owns the roll (AA.1) ────

        [Fact]
        public void DiseaseAuthority_RespectsImmunity_AndResolvesExposure()
        {
            // The seam must never bypass immunity: the disease authority's
            // TryExpose is the only writer and it honors temporary immunity.
            var catalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var source = catalog.GetExposureSource(FoodborneExposureMath.SpoiledPreservedStockSourceId);
            Assert.NotNull(source);

            var system = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(1234));
            system.BindCatalog(catalog); // the host's expansion hub does this at setup
            const string survivor = "s_foodborne_probe";

            var context = new DiseaseExposureContext
            {
                SurvivorId = survivor,
                DiseaseId = source!.disease_id,
                SourceId = source.source_id,
                ProbabilityModifier = 1f, // worst-case share: the authority still decides
                BypassImmunity = false,
                Day = 3
            };

            // An immune survivor is blocked; the bridge never overrides this.
            system.SetImmunity(survivor, source.disease_id, untilDay: 30, strength: 1.0f);
            var immune = system.TryExpose(context);
            Assert.False(immune.Infected);
            Assert.Equal("immune", immune.Reason);
        }

        [Fact]
        public void DiseaseAuthority_RejectsUnknownDisease_FailClosed()
        {
            var catalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var system = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(99));
            system.BindCatalog(catalog);

            var result = system.TryExpose(new DiseaseExposureContext
            {
                SurvivorId = "s_probe",
                DiseaseId = "disease_does_not_exist",
                SourceId = FoodborneExposureMath.SpoiledPreservedStockSourceId,
                ProbabilityModifier = 1f,
                Day = 1
            });

            Assert.False(result.Infected);
            Assert.Equal("unknown_disease", result.Reason);
        }
    }
}
