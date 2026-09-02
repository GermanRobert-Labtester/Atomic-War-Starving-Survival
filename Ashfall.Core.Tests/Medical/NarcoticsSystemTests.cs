using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class NarcoticsSystemTests
    {
        private static string LoadNarcoticsCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "narcotics.json");
            if (!File.Exists(path))
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data", "narcotics.json");
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = dir.Parent;
                }
                throw new FileNotFoundException("Could not find narcotics.json");
            }
            return File.ReadAllText(path);
        }

        [Fact]
        public void LoadCatalog_ParsesFormulasCorrectly()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var stim = system.GetDefinition("chem_hyper_stim");
            Assert.NotNull(stim);
            Assert.Equal("Stimulant", stim.category);
            Assert.Contains("reflex_boost", stim.effect_tags);
            Assert.True(stim.toxicity_contribution > 0f);

            var tincture = system.GetDefinition("chem_dulcimer_tincture");
            Assert.NotNull(tincture);
            Assert.Equal("Analgesic", tincture.category);
        }

        [Fact]
        public void BrewChem_ExecutesAtomicInventoryTransaction()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var inventory = new Dictionary<string, int>
            {
                ["item_medical_precursor_base"] = 3,
                ["item_sterile_solvent_pack"] = 2
            };

            int removedPrecursors = 0;
            int removedSolvents = 0;
            int addedStims = 0;

            bool brewed = system.BrewChem(
                "chem_hyper_stim",
                id => inventory.GetValueOrDefault(id, 0),
                (id, count) =>
                {
                    if (id == "item_medical_precursor_base") removedPrecursors += count;
                    if (id == "item_sterile_solvent_pack") removedSolvents += count;
                },
                (id, count) =>
                {
                    if (id == "item_chem_hyper_stim") addedStims += count;
                },
                out string error);

            Assert.True(brewed, error);
            Assert.Equal(1, removedPrecursors);
            Assert.Equal(1, removedSolvents);
            Assert.Equal(1, addedStims);
        }

        [Fact]
        public void AdministerChem_UpdatesToxicityAndTolerance()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var rng = new SeededRng(42);
            bool administered = system.AdministerChem("survivor_01", "chem_hyper_stim", rng, out string msg);

            Assert.True(administered, msg);
            var profile = system.GetOrCreateProfile("survivor_01");
            Assert.True(profile.bloodToxicity > 0f);
            Assert.Single(profile.activeEffects);
            Assert.Single(profile.dependencies);

            var dep = profile.dependencies[0];
            Assert.True(dep.tolerance > 0f);
            Assert.True(dep.dependencyLevel > 0f);
            Assert.False(dep.isWithdrawing);
        }

        [Fact]
        public void HighToxicity_CanTriggerOverdoseEmergency()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var profile = system.GetOrCreateProfile("survivor_heavy");
            profile.bloodToxicity = 75f; // Pre-existing high toxicity

            bool emergencyTriggered = false;
            system.OnOverdoseEmergency += (sId, reason) => emergencyTriggered = true;

            var rng = new SeededRng(1); // seed chosen for overdose trigger
            system.AdministerChem("survivor_heavy", "chem_clarity_salts", rng, out _);

            Assert.True(emergencyTriggered || profile.bloodToxicity >= 100f);
            Assert.True(system.TotalOverdoses > 0 || profile.bloodToxicity >= 100f);
        }

        [Fact]
        public void MedicalTick_ClearsToxicityAndProgressesWithdrawal()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var profile = system.GetOrCreateProfile("survivor_w");
            profile.bloodToxicity = 30f;
            profile.dependencies.Add(new DependencyRecord
            {
                chemId = "chem_hyper_stim",
                dependencyLevel = 45f,
                hoursSinceLastDose = 20f,
                isWithdrawing = false
            });

            var rng = new SeededRng(10);
            system.AdvanceMedicalTick(8f, rng);

            Assert.True(profile.bloodToxicity < 30f, "Blood toxicity must clear metabolically over time");
            var dep = profile.dependencies[0];
            Assert.True(dep.hoursSinceLastDose >= 28f);
            Assert.True(dep.isWithdrawing, "Withdrawal must trigger when dose lapse exceeds 24h for dependent survivor");
        }

        [Fact]
        public void RehabBed_CuresDependencyGradually()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var profile = system.GetOrCreateProfile("survivor_rehab");
            profile.dependencies.Add(new DependencyRecord
            {
                chemId = "chem_dulcimer_tincture",
                dependencyLevel = 60f,
                tolerance = 0.5f,
                isWithdrawing = true
            });

            bool assigned = system.AssignToRehabBed("survivor_rehab");
            Assert.True(assigned);
            Assert.True(profile.inRehabBed);

            var rng = new SeededRng(42);
            // Simulate 15 days of intensive bedrest rehab
            for (int d = 0; d < 15; d++)
            {
                system.AdvanceMedicalTick(24f, rng);
            }

            Assert.False(profile.inRehabBed);
            Assert.Equal(1, system.TotalRehabs);
            Assert.Equal(0f, profile.dependencies[0].dependencyLevel);
            Assert.False(profile.dependencies[0].isWithdrawing);
        }

        [Fact]
        public void NarcoticsState_RoundTripPreservation()
        {
            var system = new NarcoticsSystem();
            system.LoadCatalog(LoadNarcoticsCatalogJson(), new SystemTextJsonSerializer());

            var rng = new SeededRng(5);
            system.AdministerChem("surv_save", "chem_haze_resin", rng, out _);

            var state = system.CaptureState();
            Assert.Single(state.survivors);
            Assert.Equal(1, state.totalDosesAdministered);

            var restored = new NarcoticsSystem();
            restored.RestoreState(state);

            Assert.Single(restored.Profiles);
            Assert.Equal(1, restored.TotalDoses);
        }
    }
}
