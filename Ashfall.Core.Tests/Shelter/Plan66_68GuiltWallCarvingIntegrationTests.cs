// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan66_68GuiltWallCarvingIntegrationTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            throw new DirectoryNotFoundException("StreamingAssets/Data directory could not be located.");
        }

        [Fact]
        public void GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle()
        {
            var dataDir = GetDataDir();
            var catalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);

            // Plan 66 catalog integrity: exactly 40 authored items
            Assert.Equal(40, catalog.Count);
            Assert.Equal(40, catalog.Items.Count);

            foreach (var item in catalog.Items)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.ChoicePattern));
                Assert.False(string.IsNullOrWhiteSpace(item.Title));
                Assert.False(string.IsNullOrWhiteSpace(item.Description));
                Assert.InRange(item.Severity, 0.1f, 1.0f);
            }

            // Description formatting verification
            var orderDeathDef = catalog.GetByPattern("order_survivor_to_death");
            Assert.NotNull(orderDeathDef);
            var formatted = orderDeathDef!.FormatDescription("Elena Vance");
            Assert.Contains("Elena Vance", formatted);

            // System integration
            var insomniaSystem = new GuiltInsomniaSystem();
            string criticalSurvivorId = null;
            insomniaSystem.OnGuiltInsomniaCritical += sId => criticalSurvivorId = sId;

            // Record moderate guilt
            bool foundFuel = catalog.TryGetSeverity("burn_critical_fuel_for_comfort", out float fuelSeverity);
            Assert.True(foundFuel);
            insomniaSystem.RecordGuilt("surv_marcus", "burn_critical_fuel_for_comfort", fuelSeverity, currentDay: 1);

            Assert.Equal(1, insomniaSystem.GetGuiltSourceCount("surv_marcus"));
            Assert.Equal(fuelSeverity, insomniaSystem.GetInsomniaSeverity("surv_marcus"), 3);
            Assert.Null(criticalSurvivorId);

            // Record high-severity guilt pushing survivor past critical threshold
            bool foundDeath = catalog.TryGetSeverity("order_survivor_to_death", out float deathSeverity);
            Assert.True(foundDeath);
            Assert.True(deathSeverity >= GuiltInsomniaSystem.HighSeverityThreshold);

            insomniaSystem.RecordGuilt("surv_marcus", "order_survivor_to_death", deathSeverity, currentDay: 2);
            Assert.Equal("surv_marcus", criticalSurvivorId);
            Assert.Equal(2, insomniaSystem.GetGuiltSourceCount("surv_marcus"));
            Assert.Equal(1.0f, insomniaSystem.GetInsomniaSeverity("surv_marcus"), 3);

            // Sleep quality penalty
            float sleepMultiplier = insomniaSystem.GetSleepQualityMultiplier("surv_marcus");
            Assert.True(sleepMultiplier < 1.0f);

            // Sedative compensation
            bool sedativeApplied = insomniaSystem.ApplySedative("surv_marcus");
            Assert.True(sedativeApplied);
            Assert.True(insomniaSystem.GetInsomniaSeverity("surv_marcus") < 1.0f);

            // Save / restore round-trip
            var saveState = insomniaSystem.CaptureState();
            Assert.NotNull(saveState);
            Assert.Single(saveState.survivors);

            var restoredSystem = new GuiltInsomniaSystem();
            restoredSystem.RestoreState(saveState);

            Assert.Equal(2, restoredSystem.GetGuiltSourceCount("surv_marcus"));
            Assert.Equal(insomniaSystem.GetInsomniaSeverity("surv_marcus"), restoredSystem.GetInsomniaSeverity("surv_marcus"), 3);

            // Dialogue resolution removes newest guilt record
            bool dialogueOk = restoredSystem.ResolveGuiltThroughDialogue("surv_marcus");
            Assert.True(dialogueOk);
            Assert.Equal(1, restoredSystem.GetGuiltSourceCount("surv_marcus"));
        }

        [Fact]
        public void WallCarvingCatalog_And_MoraleBandSelection_FullContract()
        {
            var dataDir = GetDataDir();
            var catalog = WallCarvingCatalog.LoadFromDirectory(dataDir);

            // Plan 68 catalog integrity: exactly 3 bands, 20 templates per band = 60 total
            Assert.Equal(3, catalog.Bands.Count);
            Assert.Equal(60, catalog.TotalTemplateCount);

            var highBand = catalog.GetBandForMorale(75f);
            Assert.NotNull(highBand);
            Assert.Equal("high", highBand!.MoraleBand);
            Assert.Equal(20, highBand.Templates.Count);
            Assert.Equal(0.3f, highBand.CarvingChance, 2);

            var medBand = catalog.GetBandForMorale(45f);
            Assert.NotNull(medBand);
            Assert.Equal("medium", medBand!.MoraleBand);
            Assert.Equal(20, medBand.Templates.Count);
            Assert.Equal(0.2f, medBand.CarvingChance, 2);

            var lowBand = catalog.GetBandForMorale(15f);
            Assert.NotNull(lowBand);
            Assert.Equal("low", lowBand!.MoraleBand);
            Assert.Equal(20, lowBand.Templates.Count);
            Assert.Equal(0.15f, lowBand.CarvingChance, 2);

            // Boundary and clamping tests
            Assert.Equal("high", catalog.GetBandForMorale(100f)!.MoraleBand);
            Assert.Equal("high", catalog.GetBandForMorale(125f)!.MoraleBand);
            Assert.Equal("high", catalog.GetBandForMorale(60f)!.MoraleBand);
            Assert.Equal("medium", catalog.GetBandForMorale(59f)!.MoraleBand);
            Assert.Equal("medium", catalog.GetBandForMorale(30f)!.MoraleBand);
            Assert.Equal("low", catalog.GetBandForMorale(29f)!.MoraleBand);
            Assert.Equal("low", catalog.GetBandForMorale(0f)!.MoraleBand);
            Assert.Equal("low", catalog.GetBandForMorale(-20f)!.MoraleBand);

            // Deterministic template picking
            int selectedIndex = 3;
            var pickedTemplate = catalog.GetRandomTemplate(80f, max => selectedIndex % max);
            Assert.Equal(highBand.Templates[selectedIndex], pickedTemplate);
        }

        [Fact]
        public void PsychologicalStress_And_CulturalTrace_SystemCoupling()
        {
            var dataDir = GetDataDir();
            var guiltCatalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
            var carvingCatalog = WallCarvingCatalog.LoadFromDirectory(dataDir);
            var insomniaSystem = new GuiltInsomniaSystem();

            // Initial shelter state: steady morale (75f) -> high morale wall carving
            float shelterMorale = 75f;
            var initialBand = carvingCatalog.GetBandForMorale(shelterMorale);
            Assert.NotNull(initialBand);
            Assert.Equal("high", initialBand!.MoraleBand);
            var hopefulTemplate = carvingCatalog.GetRandomTemplate(shelterMorale, _ => 0);
            Assert.False(string.IsNullOrWhiteSpace(hopefulTemplate));

            // Ruthless decisions induce guilt across multiple dwellers
            string[] survivors = { "surv_leader", "surv_medic", "surv_scout" };
            string[] patterns = { "order_survivor_to_death", "triage_by_utility", "leave_wounded_behind" };

            float cumulativeInsomnia = 0f;
            for (int i = 0; i < survivors.Length; i++)
            {
                if (guiltCatalog.TryGetSeverity(patterns[i], out float sev))
                {
                    insomniaSystem.RecordGuilt(survivors[i], patterns[i], sev, currentDay: 5);
                    cumulativeInsomnia += insomniaSystem.GetInsomniaSeverity(survivors[i]);
                }
            }

            Assert.True(cumulativeInsomnia >= 2.0f);

            // Morale drops as psychological consequences spread through the shelter
            shelterMorale -= (cumulativeInsomnia * 25f);
            shelterMorale = Math.Max(5f, shelterMorale);

            // Wall carving culture reflects community despair
            var depressedBand = carvingCatalog.GetBandForMorale(shelterMorale);
            Assert.NotNull(depressedBand);
            Assert.Equal("low", depressedBand!.MoraleBand);

            var bleakTemplate = carvingCatalog.GetRandomTemplate(shelterMorale, _ => 0);
            Assert.False(string.IsNullOrWhiteSpace(bleakTemplate));
            Assert.NotEqual(hopefulTemplate, bleakTemplate);

            // Therapy and reconciliation lift community out of critical despair
            for (int i = 0; i < survivors.Length; i++)
            {
                insomniaSystem.ApplyTherapyRelief(survivors[i], 0.80f);
            }

            // Morale recovers toward medium band
            shelterMorale += 35f;
            var recoveredBand = carvingCatalog.GetBandForMorale(shelterMorale);
            Assert.NotNull(recoveredBand);
            Assert.Equal("medium", recoveredBand!.MoraleBand);
        }
    }
}
