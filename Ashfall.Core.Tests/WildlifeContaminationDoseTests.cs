// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 6: Contamination Dose Scale Verification.
    /// Proves wildlife contamination doses use the same scale expected by RadiationSystem,
    /// remain sub-acute per single catch, and can accumulate into meaningful acute risk
    /// across repeated exposures.
    /// </summary>
    public class WildlifeContaminationDoseTests
    {
        [Fact]
        public void Task6_01_RadiationAuthority_AcuteThresholdIs80Rads()
        {
            // Section 14: Confirm from source that AcuteThreshold is 80f
            Assert.Equal(80.0f, RadiationSystem.AcuteThreshold);
        }

        [Fact]
        public void Task6_02_WholeCatalogSingleCatchSafety_AllPreyBelowAcuteThreshold()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            Assert.Equal(15, catalog.Prey.Count);

            foreach (var kv in catalog.Prey)
            {
                var prey = kv.Value;
                float dose = prey.contaminationDose;

                Assert.True(float.IsFinite(dose),
                    $"Prey '{prey.speciesId}' has non-finite contaminationDose: {dose}");
                Assert.True(dose >= 0.0f,
                    $"Prey '{prey.speciesId}' has negative contaminationDose: {dose}");
                Assert.True(dose < RadiationSystem.AcuteThreshold,
                    $"Prey '{prey.speciesId}' contaminationDose ({dose} rads) reaches or exceeds AcuteThreshold ({RadiationSystem.AcuteThreshold} rads)");
            }
        }

        [Fact]
        public void Task6_03_NamedBalanceAnchors_MatchAuthoredCatalog()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();

            // 1. rat: 4.0 rads
            Assert.True(catalog.Prey.TryGetValue("rat", out var rat));
            Assert.Equal(4.0f, rat.contaminationDose);

            // 2. ash_crow: 6.0 rads
            Assert.True(catalog.Prey.TryGetValue("ash_crow", out var ashCrow));
            Assert.Equal(6.0f, ashCrow.contaminationDose);

            // 3. irradiated_squirrel: 20.0 rads
            Assert.True(catalog.Prey.TryGetValue("irradiated_squirrel", out var squirrel));
            Assert.Equal(20.0f, squirrel.contaminationDose);

            // 4. contaminated_fowl: 12.0 rads
            Assert.True(catalog.Prey.TryGetValue("contaminated_fowl", out var fowl));
            Assert.Equal(12.0f, fowl.contaminationDose);

            // 5. rad_dog: 8.0 rads
            Assert.True(catalog.Prey.TryGetValue("rad_dog", out var dog));
            Assert.Equal(8.0f, dog.contaminationDose);

            // 6. fallback: 2.0 rads
            Assert.Equal(2.0f, PreyDefinition.FallbackContaminationDose);
            Assert.Equal(2.0f, WildlifeTrappingCatalogTestFixture.FallbackContaminationDose);
        }

        [Fact]
        public void Task6_04_CumulativeExposureProof_Four20RadExposuresReachAcuteSickness()
        {
            // Section 16.3: Use RadiationSystem directly, proving 4x 20 rads triggers AcuteRadiationSickness
            var radSys = new RadiationSystem(seed: 42);
            var survivor = new SurvivorRadState
            {
                Id = "survivor_trapper",
                RadiationDose = 0.0f,
                IsAlive = true
            };
            radSys.Register(survivor);

            const float singleDose = 20.0f; // irradiated_squirrel dose

            // Exposure 1: 20 rads
            radSys.Expose(survivor, singleDose, 1.0f);
            Assert.Equal(20.0f, survivor.RadiationDose);
            Assert.False(survivor.HasStatus(SurvivorStatus.AcuteRadiationSickness));

            // Exposure 2: 40 rads
            radSys.Expose(survivor, singleDose, 1.0f);
            Assert.Equal(40.0f, survivor.RadiationDose);
            Assert.False(survivor.HasStatus(SurvivorStatus.AcuteRadiationSickness));

            // Exposure 3: 60 rads
            radSys.Expose(survivor, singleDose, 1.0f);
            Assert.Equal(60.0f, survivor.RadiationDose);
            Assert.False(survivor.HasStatus(SurvivorStatus.AcuteRadiationSickness));

            // Exposure 4: 80 rads -> hits AcuteThreshold (80 rads)
            radSys.Expose(survivor, singleDose, 1.0f);
            Assert.Equal(80.0f, survivor.RadiationDose);
            Assert.True(survivor.HasStatus(SurvivorStatus.AcuteRadiationSickness));
        }

        [Fact]
        public void Task6_05_ExactDelegateValue_OnContaminationHit()
        {
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(system);

            var session = new TestWildlifeTrappingSession(system);
            session.Catalog = catalog;

            int calls = 0;
            float receivedDose = 0f;
            string receivedSurvivor = string.Empty;

            session.ApplyContamination = (survivor, dose) =>
            {
                calls++;
                receivedDose = dose;
                receivedSurvivor = survivor;
            };

            // Set up catch with known high-dose prey (irradiated_squirrel = 20 rads)
            system.SetTrap("site_alpha", "bait_berry_mash", "dweller_scavenger", "deadfall");
            var site = system.State.trapSites.Find(s => s.siteId == "site_alpha");
            Assert.NotNull(site);
            site!.hasCatch = true;
            site.catchSpecies = "irradiated_squirrel";
            site.contaminationDose = 20.0f; // explicit dose on site

            var res = session.Butcher("site_alpha", "dweller_scavenger");
            Assert.True(res.IsSuccess);

            // Delegate must be called exactly once with exact dose
            Assert.Equal(1, calls);
            Assert.Equal(20.0f, receivedDose);
            Assert.Equal("dweller_scavenger", receivedSurvivor);
        }

        [Fact]
        public void Task6_06_NoDelegateCall_OnContaminationMiss()
        {
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(system);

            var session = new TestWildlifeTrappingSession(system);
            session.Catalog = catalog;

            int calls = 0;
            session.ApplyContamination = (survivor, dose) => { calls++; };

            // Set up clean catch with 0 contamination dose and clean site
            system.SetTrap("site_beta", "bait_grain_lure", "dweller_clean", "snare");
            var site = system.State.trapSites.Find(s => s.siteId == "site_beta");
            Assert.NotNull(site);
            site!.hasCatch = true;
            site.contaminationDose = 0.0f;
            site.diseaseId = string.Empty;

            var cleanPrey = new PreyDefinition
            {
                speciesId = "clean_rabbit",
                displayName = "Clean Rabbit",
                contaminationRisk = 0.0f,
                contaminationDose = 0.0f
            };
            system.RegisterPreyDefinition(cleanPrey);
            site.catchSpecies = "clean_rabbit";

            var res = session.Butcher("site_beta", "dweller_clean");
            Assert.True(res.IsSuccess);

            Assert.Equal(0, calls);
        }

        [Fact]
        public void Task6_07_FallbackDosePath_AppliesFallbackWhenAuthoredDoseIsZero()
        {
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(system);

            var session = new TestWildlifeTrappingSession(system);
            session.Catalog = catalog;

            float receivedDose = 0f;
            int calls = 0;
            session.ApplyContamination = (survivor, dose) =>
            {
                calls++;
                receivedDose = dose;
            };

            // Prey with contaminationRisk = 1.0 (guaranteed roll) but no authored dose (0f)
            var unauthoredDosePrey = new PreyDefinition
            {
                speciesId = "unauthored_dose_beast",
                displayName = "Unauthored Beast",
                contaminationRisk = 1.0f,
                contaminationDose = 0.0f
            };
            system.RegisterPreyDefinition(unauthoredDosePrey);

            system.SetTrap("site_fallback", "bait_scrap_meat", "dweller_test", "snare");
            var site = system.State.trapSites.Find(s => s.siteId == "site_fallback");
            Assert.NotNull(site);
            site!.hasCatch = true;
            site.catchSpecies = "unauthored_dose_beast";
            site.contaminationDose = 0.0f; // site dose not pre-resolved

            var res = session.Butcher("site_fallback", "dweller_test");
            Assert.True(res.IsSuccess);

            Assert.Equal(1, calls);
            Assert.Equal(PreyDefinition.FallbackContaminationDose, receivedDose);
            Assert.Equal(2.0f, receivedDose);
        }

        [Fact]
        public void Task6_08_RadiationUnitBridge_OneContaminationDoseEqualsOneRadPerHour()
        {
            // Section 17: Bridge a trapping dose to RadiationSystem.Expose() directly.
            // 12 rads/hour for 1 hour increases accumulated dose by exactly 12 rads.
            var radSys = new RadiationSystem(seed: 99);
            var survivor = new SurvivorRadState { Id = "hunter_bridge", RadiationDose = 10.0f, IsAlive = true };
            radSys.Register(survivor);

            float caughtDose = 12.0f; // contaminated_fowl dose
            radSys.Expose(survivor, caughtDose, 1.0f);

            Assert.Equal(22.0f, survivor.RadiationDose);
            Assert.Equal(12.0f, survivor.LifetimeRadiationExposure);
        }
    }
}
