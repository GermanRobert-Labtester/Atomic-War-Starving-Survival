using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public class ForcedLaborSystemTests
    {
        private static string LoadLaborCampsCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "labor_camps.json");
            if (!File.Exists(path))
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data", "labor_camps.json");
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = dir.Parent;
                }
                throw new FileNotFoundException("Could not find labor_camps.json");
            }
            return File.ReadAllText(path);
        }

        [Fact]
        public void LoadCatalog_ParsesCampsCorrectly()
        {
            var system = new ForcedLaborSystem();
            system.LoadCatalog(LoadLaborCampsCatalogJson(), new SystemTextJsonSerializer());

            var sump = system.GetCamp("camp_sump_drainage");
            Assert.NotNull(sump);
            Assert.Equal("Medium", sump.labor_intensity);
            Assert.Equal(0.20f, sump.guard_requirement_ratio);

            var lead = system.GetCamp("camp_lead_slag_hauling");
            Assert.NotNull(lead);
            Assert.Equal("Extreme", lead.labor_intensity);
            Assert.True(lead.base_productivity > sump.base_productivity);
        }

        [Fact]
        public void AssignAndEmancipate_ModulatesCrueltyIndex()
        {
            var system = new ForcedLaborSystem();
            system.LoadCatalog(LoadLaborCampsCatalogJson(), new SystemTextJsonSerializer());

            Assert.Equal(0f, system.CrueltyIndex);

            bool assigned = system.AssignLaborer("captive_01", "camp_sump_drainage", true, out string reason);
            Assert.True(assigned, reason);
            Assert.Single(system.Laborers);
            Assert.True(system.CrueltyIndex > 0f);

            float crueltyBefore = system.CrueltyIndex;
            bool freed = system.EmancipateLaborer("captive_01");
            Assert.True(freed);
            Assert.Empty(system.Laborers);
            Assert.True(system.CrueltyIndex < crueltyBefore, "Emancipation must reduce CrueltyIndex");
        }

        [Fact]
        public void CalculateProductivity_AccountsForStrainAndGuards()
        {
            var system = new ForcedLaborSystem();
            system.LoadCatalog(LoadLaborCampsCatalogJson(), new SystemTextJsonSerializer());
            var camp = system.GetCamp("camp_trench_fortification")!;

            var freshWorker = new ForcedLaborerState { physicalStrain = 0f, health = 100f };
            var exhaustedWorker = new ForcedLaborerState { physicalStrain = 80f, health = 50f };

            float prodFresh = system.CalculateProductivity(camp, freshWorker, 0.30f);
            float prodExhausted = system.CalculateProductivity(camp, exhaustedWorker, 0.30f);
            float prodLowGuards = system.CalculateProductivity(camp, freshWorker, 0.10f);

            Assert.True(prodFresh > prodExhausted, "Exhausted worker must yield lower productivity");
            Assert.True(prodFresh > prodLowGuards, "Low guard ratio must reduce labor oversight and output");
        }

        [Fact]
        public void AdvanceDailyShift_AccumulatesStrainAndOutput()
        {
            var system = new ForcedLaborSystem();
            system.LoadCatalog(LoadLaborCampsCatalogJson(), new SystemTextJsonSerializer());
            system.SetGuardCount(2);

            system.AssignLaborer("c1", "camp_sump_drainage", true, out _);
            system.AssignLaborer("c2", "camp_sump_drainage", true, out _);

            float recordedOutput = 0f;
            system.OnLaborOutputGenerated += (item, amount) => recordedOutput += amount;

            var rng = new SeededRng(100);
            system.AdvanceDailyShift(rng);

            Assert.True(recordedOutput > 0f);
            Assert.True(system.ResistancePressure > 0f);
            Assert.True(system.CrueltyIndex > 0f);

            foreach (var l in system.Laborers)
            {
                Assert.Equal(1, l.shiftsCompleted);
                Assert.True(l.physicalStrain > 0f);
                Assert.True(l.health < 100f);
            }
        }

        [Fact]
        public void RebellionTrigger_And_SuppressionResolution()
        {
            var system = new ForcedLaborSystem();
            system.LoadCatalog(LoadLaborCampsCatalogJson(), new SystemTextJsonSerializer());
            system.SetGuardCount(0); // Zero guards -> extreme rebellion risk

            // Add multiple extreme laborers
            for (int i = 0; i < 6; i++)
            {
                system.AssignLaborer($"slave_{i}", "camp_lead_slag_hauling", false, out _);
            }

            var rng = new SeededRng(999);
            // Tick multiple days to build up pressure and trigger rebellion
            for (int day = 0; day < 5; day++)
            {
                if (system.IsRebellionActive) break;
                system.AdvanceDailyShift(rng);
            }

            Assert.True(system.IsRebellionActive, "Extreme conditions with 0 guards must trigger rebellion");
            Assert.True(system.TotalRebellions > 0);

            // Suppress with lethal force
            bool suppressed = system.SuppressRebellion(true, rng);
            Assert.True(suppressed);
            Assert.False(system.IsRebellionActive);
            Assert.True(system.ResistancePressure < 60f);
        }

        [Fact]
        public void ForcedLaborState_RoundTripPreservation()
        {
            var system = new ForcedLaborSystem();
            system.LoadCatalog(LoadLaborCampsCatalogJson(), new SystemTextJsonSerializer());
            system.AssignLaborer("c_save", "camp_scrap_demolition", true, out _);

            var state = system.CaptureState();
            Assert.Single(state.laborers);
            Assert.True(state.crueltyIndex > 0f);

            var restored = new ForcedLaborSystem();
            restored.RestoreState(state);

            Assert.Single(restored.Laborers);
            Assert.Equal(state.crueltyIndex, restored.CrueltyIndex);
        }
    }
}
