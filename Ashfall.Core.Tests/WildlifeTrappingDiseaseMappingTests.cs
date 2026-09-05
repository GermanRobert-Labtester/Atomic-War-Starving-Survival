// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingDiseaseMappingTests
    {
        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = WildlifeTrappingCatalogLoader.Load(FindDataDir(), fileIO, json);
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void Catalog_ContainsExactly15Prey()
        {
            var catalog = LoadCatalog();
            Assert.Equal(15, catalog.Prey.Count);
        }

        public static IEnumerable<object[]> ExpectedPreyMappingTable()
        {
            // The authoritative 15-prey mapping table from wildlife_trapping_catalog.json:
            // speciesId, expectedRisk, explicitDiseaseId, expectedResolvedId, basis
            yield return new object[] { "rabbit", 0.10f, "", "", "fallback boundary (risk <= 0.1)" };
            yield return new object[] { "cotton_hare", 0.10f, "", "", "fallback boundary (risk <= 0.1)" };
            yield return new object[] { "deer", 0.15f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
            yield return new object[] { "boar", 0.20f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
            yield return new object[] { "fox", 0.20f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
            yield return new object[] { "rat", 0.35f, "disease_typhoid_waterborne", "disease_typhoid_waterborne", "explicit catalog override" };
            yield return new object[] { "pheasant", 0.15f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
            yield return new object[] { "ash_crow", 0.25f, "disease_blood_fever", "disease_blood_fever", "explicit catalog override" };
            yield return new object[] { "mirror_carp", 0.10f, "", "", "fallback boundary (risk <= 0.1)" };
            yield return new object[] { "ash_pike", 0.12f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
            yield return new object[] { "irradiated_squirrel", 0.30f, "disease_spore_blight", "disease_spore_blight", "explicit catalog override" };
            yield return new object[] { "contaminated_fowl", 0.40f, "disease_zoonotic_flu", "disease_zoonotic_flu", "explicit catalog override" };
            yield return new object[] { "rad_dog", 0.30f, "disease_zoonotic_flu", "disease_zoonotic_flu", "explicit catalog override" };
            yield return new object[] { "muskrat", 0.20f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
            yield return new object[] { "hedgehog", 0.15f, "", "disease_zoonotic_flu", "medium fallback (risk > 0.1)" };
        }

        [Theory]
        [MemberData(nameof(ExpectedPreyMappingTable))]
        public void AuthoritativeCatalogPrey_ResolvesExpectedDiseaseId(
            string speciesId, float expectedRisk, string explicitDiseaseId, string expectedResolvedId, string basis)
        {
            Assert.False(string.IsNullOrEmpty(basis));
            var catalog = LoadCatalog();
            Assert.True(catalog.Prey.ContainsKey(speciesId), $"Catalog must contain prey '{speciesId}'");

            var prey = catalog.Prey[speciesId];
            Assert.Equal(expectedRisk, prey.diseaseRisk, precision: 3);
            Assert.Equal(explicitDiseaseId, prey.diseaseId ?? string.Empty);

            string resolved = PreyDefinition.ResolveDiseaseId(prey);
            Assert.Equal(expectedResolvedId, resolved);
        }

        [Fact]
        public void ThresholdMicroTest_ExactBoundary_SeparatesNoneFromZoonoticFlu()
        {
            // Exact 0.10 -> empty (no disease)
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(0.10f, null));
            // Just above 0.10 -> disease_zoonotic_flu
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(0.10001f, null));
            // 0.0 -> empty
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(0.0f, null));
            // Negative -> empty
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(-0.5f, null));
            // Standard medium risk (0.15, 0.20, 0.50) without explicit ID -> fallback
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(0.15f, null));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(0.20f, null));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(0.50f, null));
        }

        [Fact]
        public void ExplicitCatalogDiseaseId_AlwaysWinsOverTierFallback()
        {
            // Explicit ID at low risk (e.g. 0.05) must still return explicit ID
            Assert.Equal("disease_custom_low", PreyDefinition.ResolveDiseaseId(0.05f, "disease_custom_low"));
            // Explicit ID at medium risk must return explicit ID, not zoonotic flu
            Assert.Equal("disease_custom_med", PreyDefinition.ResolveDiseaseId(0.20f, "disease_custom_med"));
            // All five catalog explicit IDs preserve their authored value exactly
            var catalog = LoadCatalog();

            Assert.Equal("disease_typhoid_waterborne", PreyDefinition.ResolveDiseaseId(catalog.Prey["rat"]));
            Assert.Equal("disease_blood_fever", PreyDefinition.ResolveDiseaseId(catalog.Prey["ash_crow"]));
            Assert.Equal("disease_spore_blight", PreyDefinition.ResolveDiseaseId(catalog.Prey["irradiated_squirrel"]));
            Assert.Equal("disease_zoonotic_flu", PreyDefinition.ResolveDiseaseId(catalog.Prey["contaminated_fowl"]));
            Assert.Equal("disease_zoonotic_flu", PreyDefinition.ResolveDiseaseId(catalog.Prey["rad_dog"]));
        }

        [Fact]
        public void LowRiskPrey_RabbitCottonHareMirrorCarp_NeverResolveFallbackDisease()
        {
            var catalog = LoadCatalog();
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(catalog.Prey["rabbit"]));
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(catalog.Prey["cotton_hare"]));
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(catalog.Prey["mirror_carp"]));
        }

        [Fact]
        public void FallbackPrey_DeerFoxPheasantAshPikeMuskratHedgehogBoar_ResolveZoonoticFlu()
        {
            var catalog = LoadCatalog();
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["deer"]));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["fox"]));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["pheasant"]));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["ash_pike"]));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["muskrat"]));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["hedgehog"]));
            Assert.Equal(PreyDefinition.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(catalog.Prey["boar"]));
        }
    }
}
