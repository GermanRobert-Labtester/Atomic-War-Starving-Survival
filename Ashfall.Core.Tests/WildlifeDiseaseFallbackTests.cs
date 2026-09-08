// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 5: Disease Tier Fallback Coverage.
    /// Proves that every prey without an explicit diseaseId follows the exact fallback tier,
    /// that every explicit mapping bypasses fallback, that the 0.10 boundary is inclusive
    /// on the low-risk side, and that ResolveDiseaseId() runs exactly once per butchery.
    /// </summary>
    public class WildlifeDiseaseFallbackTests
    {
        [Fact]
        public void Task5_01_Catalog_ContainsExactly15Prey_AllUniqueAndValid()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            Assert.Equal(15, catalog.Prey.Count);

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var kv in catalog.Prey)
            {
                var prey = kv.Value;
                Assert.False(string.IsNullOrWhiteSpace(prey.speciesId), "Prey speciesId cannot be empty or whitespace");
                Assert.True(seenIds.Add(prey.speciesId), $"Duplicate speciesId '{prey.speciesId}' in catalog");
                Assert.True(float.IsFinite(prey.diseaseRisk), $"Prey '{prey.speciesId}' has non-finite diseaseRisk: {prey.diseaseRisk}");
                Assert.InRange(prey.diseaseRisk, 0.0f, 1.0f);
            }
        }

        public static IEnumerable<object[]> NamedFallbackCases()
        {
            yield return new object[] { "rabbit", 0.10f, "" };
            yield return new object[] { "cotton_hare", 0.10f, "" };
            yield return new object[] { "deer", 0.15f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
            yield return new object[] { "fox", 0.20f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
            yield return new object[] { "pheasant", 0.15f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
            yield return new object[] { "mirror_carp", 0.10f, "" };
            yield return new object[] { "ash_pike", 0.12f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
            yield return new object[] { "muskrat", 0.20f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
            yield return new object[] { "hedgehog", 0.15f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
            yield return new object[] { "boar", 0.20f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId };
        }

        [Theory]
        [MemberData(nameof(NamedFallbackCases))]
        public void Task5_02_NamedFallbackPrey_FollowExactTier(string speciesId, float expectedRisk, string expectedDiseaseId)
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            Assert.True(catalog.Prey.TryGetValue(speciesId, out var prey), $"Catalog must contain prey '{speciesId}'");

            Assert.Equal(expectedRisk, prey.diseaseRisk, precision: 3);
            Assert.True(string.IsNullOrEmpty(prey.diseaseId), $"Prey '{speciesId}' unexpectedly has explicit diseaseId: '{prey.diseaseId}'");

            string resolved = PreyDefinition.ResolveDiseaseId(prey);
            Assert.Equal(expectedDiseaseId, resolved);
        }

        [Theory]
        [InlineData(0.00f, "")]
        [InlineData(0.05f, "")]
        [InlineData(0.10f, "")]
        [InlineData(0.1001f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId)]
        [InlineData(0.12f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId)]
        [InlineData(0.15f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId)]
        [InlineData(0.50f, WildlifeTrappingCatalogTestFixture.FallbackDiseaseId)]
        public void Task5_03_ExactBoundary_InclusiveAtPointOne(float risk, string expectedDiseaseId)
        {
            string resolved = PreyDefinition.ResolveDiseaseId(risk, null);
            Assert.Equal(expectedDiseaseId, resolved);
        }

        [Fact]
        public void Task5_04_ExplicitOverrides_AlwaysBeatTierFallback()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();

            // 1. rat -> disease_typhoid_waterborne (proves override differs from zoonotic flu)
            Assert.True(catalog.Prey.TryGetValue("rat", out var rat));
            Assert.Equal("disease_typhoid_waterborne", rat.diseaseId);
            Assert.Equal("disease_typhoid_waterborne", PreyDefinition.ResolveDiseaseId(rat));

            // 2. ash_crow -> disease_blood_fever (proves override differs from zoonotic flu)
            Assert.True(catalog.Prey.TryGetValue("ash_crow", out var ashCrow));
            Assert.Equal("disease_blood_fever", ashCrow.diseaseId);
            Assert.Equal("disease_blood_fever", PreyDefinition.ResolveDiseaseId(ashCrow));

            // 3. irradiated_squirrel -> disease_spore_blight (proves override differs from zoonotic flu)
            Assert.True(catalog.Prey.TryGetValue("irradiated_squirrel", out var squirrel));
            Assert.Equal("disease_spore_blight", squirrel.diseaseId);
            Assert.Equal("disease_spore_blight", PreyDefinition.ResolveDiseaseId(squirrel));

            // 4. contaminated_fowl -> disease_zoonotic_flu
            Assert.True(catalog.Prey.TryGetValue("contaminated_fowl", out var fowl));
            Assert.Equal(WildlifeTrappingCatalogTestFixture.FallbackDiseaseId, fowl.diseaseId);
            Assert.Equal(WildlifeTrappingCatalogTestFixture.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(fowl));

            // 5. rad_dog -> disease_zoonotic_flu
            Assert.True(catalog.Prey.TryGetValue("rad_dog", out var dog));
            Assert.Equal(WildlifeTrappingCatalogTestFixture.FallbackDiseaseId, dog.diseaseId);
            Assert.Equal(WildlifeTrappingCatalogTestFixture.FallbackDiseaseId, PreyDefinition.ResolveDiseaseId(dog));
        }

        [Fact]
        public void Task5_05_WholeCatalogSweep_All15PreyMatchIndependentOracle()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            Assert.Equal(15, catalog.Prey.Count);

            foreach (var expected in WildlifeTrappingCatalogTestFixture.Authoritative15Prey)
            {
                Assert.True(catalog.Prey.TryGetValue(expected.SpeciesId, out var prey),
                    $"Catalog missing expected prey '{expected.SpeciesId}'");

                string expectedOracle = WildlifeTrappingCatalogTestFixture.ExpectedDisease(prey);
                string actualProduction = PreyDefinition.ResolveDiseaseId(prey);

                Assert.True(expectedOracle == actualProduction,
                    $"Prey '{expected.SpeciesId}': diseaseRisk={prey.diseaseRisk}, " +
                    $"explicitId='{prey.diseaseId}', expected='{expectedOracle}', actual='{actualProduction}'");

                Assert.Equal(expected.ExpectedResolvedDiseaseId, actualProduction);
            }
        }

        [Fact]
        public void Task5_06_ResolveDiseaseId_RunsExactlyOncePerButchery()
        {
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(system);

            var session = new TestWildlifeTrappingSession(system);
            session.Catalog = catalog;

            int resolverCalls = 0;
            session.DiseaseResolver = prey =>
            {
                resolverCalls++;
                return PreyDefinition.ResolveDiseaseId(prey);
            };

            string appliedDisease = string.Empty;
            session.ApplyDisease = (survivor, diseaseId, day) =>
            {
                appliedDisease = diseaseId;
            };

            var guaranteedPrey = new PreyDefinition
            {
                speciesId = "guaranteed_deer",
                displayName = "Guaranteed Deer",
                diseaseRisk = 1.0f,
                diseaseId = ""
            };
            system.RegisterPreyDefinition(guaranteedPrey);

            // Deploy and force a catch without pre-resolved diseaseId on site
            system.SetTrap("site_test", "bait_scrap_meat", "dweller_butcher", "snare");
            var site = system.State.trapSites.Find(s => s.siteId == "site_test");
            Assert.NotNull(site);
            site!.hasCatch = true;
            site.catchSpecies = "guaranteed_deer";
            site.diseaseId = string.Empty; // simulate unauthored/fallback site state

            // Execute butchery
            var res = session.Butcher("site_test", "dweller_butcher");
            Assert.True(res.IsSuccess);

            // Resolver must have been called exactly once
            Assert.Equal(1, resolverCalls);
            Assert.Equal(WildlifeTrappingCatalogTestFixture.FallbackDiseaseId, appliedDisease);

            // Repeated calls/queries on the session or state do NOT re-invoke resolver
            Assert.Equal(1, resolverCalls);
        }

        [Fact]
        public void Task5_07_DefensiveDiseaseCases()
        {
            // Explicit ID + low risk still returns explicit ID
            Assert.Equal("disease_custom_low", PreyDefinition.ResolveDiseaseId(0.05f, "disease_custom_low"));

            // Explicit ID + high risk still returns explicit ID
            Assert.Equal("disease_custom_high", PreyDefinition.ResolveDiseaseId(0.80f, "disease_custom_high"));

            // Null prey returns empty
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId((PreyDefinition?)null));

            // Negative risk without explicit ID returns empty
            Assert.Equal(string.Empty, PreyDefinition.ResolveDiseaseId(-0.1f, null));
        }
    }
}
