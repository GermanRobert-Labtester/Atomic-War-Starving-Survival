// SPDX-License-Identifier: MIT
// Plan 63 / B4 — Disease Quarantine Policy Loop & Expansion Depth Tests (B4-001 through B4-020).
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseQuarantineCoordinatorTests
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

        private static DiseaseCatalog LoadCatalog()
            => DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static (MedicalWardSystem Ward, DiseaseSystem Disease, DutyRosterSystem Roster, DiseaseQuarantineCoordinator Coord, Dictionary<string, int> Inventory)
            CreateTestSetup(int seed = 12345, int isolationBeds = 2)
        {
            var beds = new List<MedicalBed>();
            for (int i = 1; i <= isolationBeds; i++)
            {
                beds.Add(new MedicalBed($"bed_iso_{i}", $"Isolation {i}", MedicalBedCategory.Isolation, isolation: true));
            }
            beds.Add(new MedicalBed("bed_gen_1", "General 1", MedicalBedCategory.General, isolation: false));
            beds.Add(new MedicalBed("bed_gen_2", "General 2", MedicalBedCategory.General, isolation: false));

            var procedures = new List<MedicalProcedureDef>();
            var ward = new MedicalWardSystem(new MedicalWardState(), beds, procedures);

            var catalog = LoadCatalog();
            var diseaseSys = new DiseaseSystem(new DiseaseSystemState(), new SeededRng(seed));
            diseaseSys.BindCatalog(catalog);

            var roster = new DutyRosterSystem(100);
            roster.Unlock(1);

            var inventory = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                { "clean_water", 100 },
                { "canned_food", 100 },
                { "medical_kit", 100 },
                { "antibiotics", 100 }
            };

            Func<string, int, bool> tryConsume = (item, qty) =>
            {
                if (inventory.TryGetValue(item, out int count) && count >= qty)
                {
                    inventory[item] = count - qty;
                    return true;
                }
                return false;
            };

            diseaseSys.TryConsumeItem = tryConsume;

            var coord = new DiseaseQuarantineCoordinator(ward, diseaseSys, roster, tryConsume, () => ContainmentCapability.None);
            return (ward, diseaseSys, roster, coord, inventory);
        }

        [Fact]
        public void B4_001_DiseaseIdsUnique()
        {
            var catalog = LoadCatalog();
            Assert.False(catalog.HasErrors, string.Join("; ", catalog.Errors));
            Assert.True(catalog.Count >= 16, $"Expected >= 16 diseases, found {catalog.Count}");

            var ids = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < catalog.Diseases.Count; i++)
            {
                var d = catalog.Diseases[i];
                Assert.NotNull(d);
                Assert.StartsWith("disease_", d.id);
                Assert.True(ids.Add(d.id), $"Duplicate disease ID found: {d.id}");
            }
        }

        [Fact]
        public void B4_002_PhaseDefinitionsValid()
        {
            var catalog = LoadCatalog();
            for (int i = 0; i < catalog.Diseases.Count; i++)
            {
                var d = catalog.Diseases[i];
                Assert.NotNull(d.phases);
                if (d.phases.Count > 0)
                {
                    for (int j = 0; j < d.phases.Count; j++)
                    {
                        var phase = d.phases[j];
                        Assert.NotNull(phase);
                        Assert.True(phase.duration_days > 0, $"{d.id} phase {j} has non-positive duration");
                        Assert.True(phase.contagiousness >= 0f, $"{d.id} phase {j} has negative contagiousness");
                    }
                }
            }
        }

        [Fact]
        public void B4_003_TypedInfectionAndExposureApi()
        {
            var (_, diseaseSys, _, _, _) = CreateTestSetup();

            var ctx = new DiseaseExposureContext
            {
                SurvivorId = "surv_alpha",
                DiseaseId = DiseaseIds.Cholera,
                Day = 1,
                ProbabilityModifier = 100.0f // guaranteed
            };

            var res = diseaseSys.TryExpose(ctx);
            Assert.True(res.Infected);
            Assert.Equal("infected", res.Reason);
            Assert.True(diseaseSys.IsInfected("surv_alpha", DiseaseIds.Cholera));

            // Exposure while already infected returns blocked
            var res2 = diseaseSys.TryExpose(ctx);
            Assert.False(res2.Infected);
            Assert.Equal("already_infected", res2.Reason);

            // Exposure with active immunity returns blocked
            diseaseSys.SetImmunity("surv_beta", DiseaseIds.Cholera, 10, strength: 1.0f);
            var ctxImmune = new DiseaseExposureContext
            {
                SurvivorId = "surv_beta",
                DiseaseId = DiseaseIds.Cholera,
                Day = 2,
                ProbabilityModifier = 100.0f
            };
            var resImmune = diseaseSys.TryExpose(ctxImmune);
            Assert.False(resImmune.Infected);
            Assert.Equal("immune", resImmune.Reason);
        }

        [Fact]
        public void B4_004_WildlifeProbabilityDataDriven()
        {
            var catalog = LoadCatalog();
            var src = catalog.GetExposureSource("wildlife_butchery");
            Assert.NotNull(src);
            Assert.Equal(DiseaseIds.ZoonoticFlu, src.disease_id);
            Assert.True(src.base_probability > 0f && src.base_probability <= 1.0f);
            Assert.Equal(0.30f, src.base_probability, precision: 2);
        }

        [Fact]
        public void B4_005_AutopsyProbabilityDataDriven()
        {
            var catalog = LoadCatalog();
            var src = catalog.GetExposureSource("autopsy_pathogen");
            Assert.NotNull(src);
            Assert.Equal(DiseaseIds.ZoonoticFlu, src.disease_id);
            Assert.True(src.base_probability > 0f && src.base_probability <= 1.0f);
            Assert.Equal(0.25f, src.base_probability, precision: 2);
        }

        [Fact]
        public void B4_006_FixedSeedDiseaseArcExactDeterminism()
        {
            var (_, diseaseA, _, _, _) = CreateTestSetup(seed: 9999);
            var (_, diseaseB, _, _, _) = CreateTestSetup(seed: 9999);

            var roster = new List<string> { "s1", "s2", "s3", "s4", "s5" };

            diseaseA.Infect("s1", DiseaseIds.Cholera, 1);
            diseaseB.Infect("s1", DiseaseIds.Cholera, 1);

            for (int day = 1; day <= 20; day++)
            {
                diseaseA.TickDaily(day, roster);
                diseaseB.TickDaily(day, roster);
            }

            var snapA = diseaseA.GetSnapshot();
            var snapB = diseaseB.GetSnapshot();

            Assert.Equal(snapA.total_infected, snapB.total_infected);
            Assert.Equal(snapA.total_recovered, snapB.total_recovered);
            Assert.Equal(snapA.total_deaths, snapB.total_deaths);
        }

        [Fact]
        public void B4_007_IsolationLowersTransmission()
        {
            // Two identical runs with 1 index patient and a large susceptible cohort.
            // Setup A: patient is isolated via coordinator.
            // Setup B: patient is free and unisolated.
            var (wardA, diseaseA, _, coordA, _) = CreateTestSetup(seed: 4242);
            var (_, diseaseB, _, _, _) = CreateTestSetup(seed: 4242);

            var candidates = new List<string>();
            for (int i = 0; i < 20; i++) candidates.Add($"target_{i}");

            diseaseA.Infect("index_pt", DiseaseIds.ZoonoticFlu, 1);
            diseaseB.Infect("index_pt", DiseaseIds.ZoonoticFlu, 1);

            // Isolate in A
            var admit = coordA.ExecuteAssignIsolation("index_pt", 1);
            Assert.True(admit.Success);

            for (int day = 1; day <= 10; day++)
            {
                coordA.TickDaily(day);
                diseaseA.TickDaily(day, candidates);
                diseaseB.TickDaily(day, candidates);
            }

            // Unisolated B spreads significantly more than isolated A
            Assert.True(diseaseA.TotalInfectionsHistory <= diseaseB.TotalInfectionsHistory,
                $"Isolated history {diseaseA.TotalInfectionsHistory} should be <= unisolated {diseaseB.TotalInfectionsHistory}");
        }

        [Fact]
        public void B4_008_ContainmentKnowledgeGivesBoundedImprovement()
        {
            var none = ContainmentCapability.FromResearch(null);
            Assert.False(none.HasPathogenContainment);
            Assert.Equal(0f, none.EfficacyBonus);

            var withResearch = ContainmentCapability.FromResearch(k => k == "knowledge_pathogen_containment");
            Assert.True(withResearch.HasPathogenContainment);
            Assert.True(withResearch.EfficacyBonus > 0f && withResearch.EfficacyBonus <= 0.15f);
            Assert.True(withResearch.CareEfficiencyBonus > 0f && withResearch.CareEfficiencyBonus <= 0.25f);
        }

        [Fact]
        public void B4_009_ResearchStateNotSerializedIntoDisease()
        {
            var (_, diseaseSys, _, _, _) = CreateTestSetup();
            var state = diseaseSys.CaptureState();

            // Reflection check: DiseaseSystemState must not have research fields
            var fields = typeof(DiseaseSystemState).GetFields();
            foreach (var f in fields)
            {
                Assert.DoesNotContain("research", f.Name, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("knowledge", f.Name, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void B4_010_NoBedGivesHonestBlock()
        {
            var (ward, _, _, coord, _) = CreateTestSetup(isolationBeds: 1);

            var preview1 = coord.PreviewAssignIsolation("patient_1");
            Assert.True(preview1.CanExecute);
            var exec1 = coord.ExecuteAssignIsolation("patient_1", 1);
            Assert.True(exec1.Success);

            // Second patient cannot be admitted — no isolation beds left
            var preview2 = coord.PreviewAssignIsolation("patient_2");
            Assert.False(preview2.CanExecute);
            Assert.Equal("no_isolation_beds_available", preview2.Reason);

            var exec2 = coord.ExecuteAssignIsolation("patient_2", 1);
            Assert.False(exec2.Success);
            Assert.Equal("no_isolation_beds_available", exec2.Reason);
        }

        [Fact]
        public void B4_011_IsolationRemovesIncompatibleDutyPairing()
        {
            var (_, _, roster, coord, _) = CreateTestSetup();

            roster.WriteName("worker_bob", "Bob", "Guard", DutyRosterSystem.ScriptPencil, 1, true);
            roster.Assign(DutyRosterIds.RoleNightWatch, "worker_bob");
            Assert.Equal(DutyRosterIds.RoleNightWatch, roster.GetRoleOf("worker_bob"));

            // Executing isolation clears the duty role
            var exec = coord.ExecuteAssignIsolation("worker_bob", 1);
            Assert.True(exec.Success);

            Assert.Null(roster.GetRoleOf("worker_bob"));
            Assert.True(roster.IsSurvivorReservedExternally?.Invoke("worker_bob") == true);
        }

        [Fact]
        public void B4_012_CareResourcesChargedOnce()
        {
            var (_, _, _, coord, inventory) = CreateTestSetup(isolationBeds: 2);

            coord.ExecuteAssignIsolation("patient_1", 1);
            coord.ExecuteAssignIsolation("patient_2", 1);

            int waterBefore = inventory["clean_water"];
            int foodBefore = inventory["canned_food"];
            int medBefore = inventory["medical_kit"];

            coord.TickDaily(1);

            Assert.Equal(waterBefore - 2, inventory["clean_water"]);
            Assert.Equal(foodBefore - 2, inventory["canned_food"]);
            Assert.Equal(medBefore - 2, inventory["medical_kit"]);
        }

        [Fact]
        public void B4_013_NoUniversalInstantCure()
        {
            var (_, diseaseSys, _, _, _) = CreateTestSetup();
            diseaseSys.Infect("pt_cure", DiseaseIds.Cholera, 1);

            // Supportive/symptomatic treatment does not remove infection
            var res = diseaseSys.TryTreat("pt_cure", DiseaseIds.Cholera, "clean_water", 1);
            Assert.True(res.Accepted);
            Assert.False(res.Cured);
            Assert.True(diseaseSys.IsInfected("pt_cure", DiseaseIds.Cholera));

            // Cumulative lethality reduction is capped at 0.90
            var patientState = diseaseSys.GetDiseaseState(DiseaseIds.Cholera)?.infected[0];
            Assert.NotNull(patientState);
            Assert.True(patientState.lethality_reduction <= DiseaseSystem.MaxLethalityReduction);
        }

        [Fact]
        public void B4_014_ImmunitySurvivesSaveReload()
        {
            var (_, diseaseA, _, _, _) = CreateTestSetup();
            diseaseA.SetImmunity("surv_immune", DiseaseIds.Cholera, untilDay: 45, strength: 0.95f);

            var saved = diseaseA.CaptureState();

            var diseaseB = new DiseaseSystem();
            diseaseB.BindCatalog(LoadCatalog());
            diseaseB.RestoreState(saved);

            Assert.True(diseaseB.HasImmunity("surv_immune", DiseaseIds.Cholera, 20));
            Assert.False(diseaseB.HasImmunity("surv_immune", DiseaseIds.Cholera, 50));
            var imm = diseaseB.GetImmunity("surv_immune", DiseaseIds.Cholera);
            Assert.NotNull(imm);
            Assert.Equal(45, imm.immunity_until_day);
            Assert.Equal(0.95f, imm.strength);
        }

        [Fact]
        public void B4_015_TerminalOutcomeCreatesOneFateRecord()
        {
            var (_, diseaseSys, _, _, _) = CreateTestSetup();
            int deathCount = 0;
            diseaseSys.OnOutcomeResolved += (s, d, recovered) =>
            {
                if (!recovered) deathCount++;
            };

            // Force 100% lethality
            diseaseSys.EffectiveLethalityModifier = (s, d) => 1.0f;
            diseaseSys.Infect("doomed", DiseaseIds.Cholera, 1);

            for (int day = 1; day <= 15; day++)
            {
                diseaseSys.TickDaily(day, new[] { "doomed" });
            }

            Assert.Equal(1, deathCount);
        }

        [Fact]
        public void B4_016_RestoreDoesNotReInfectOrReKill()
        {
            var (_, diseaseA, _, _, _) = CreateTestSetup();
            diseaseA.Infect("surv_saved", DiseaseIds.Cholera, 1);

            var saved = diseaseA.CaptureState();

            var diseaseB = new DiseaseSystem();
            diseaseB.BindCatalog(LoadCatalog());

            int infectionEvents = 0;
            int outcomeEvents = 0;
            diseaseB.OnInfection += (s, d) => infectionEvents++;
            diseaseB.OnOutcomeResolved += (s, d, rec) => outcomeEvents++;

            diseaseB.RestoreState(saved);

            Assert.Equal(0, infectionEvents);
            Assert.Equal(0, outcomeEvents);
        }

        [Fact]
        public void B4_017_UnknownDiseaseIdPreserved()
        {
            var (_, diseaseSys, _, _, _) = CreateTestSetup();
            var state = diseaseSys.CaptureState();

            state.diseases.Add(new DiseaseEntryState
            {
                disease_id = "disease_fictional_alien_fever",
                vector_type = "air",
                infections_total = 3
            });

            var restored = new DiseaseSystem();
            restored.BindCatalog(LoadCatalog());
            restored.RestoreState(state);

            var custom = restored.GetDiseaseState("disease_fictional_alien_fever");
            Assert.NotNull(custom);
            Assert.Equal(3, custom.infections_total);
        }

        [Fact]
        public void B4_018_QuarantineOnOffTradeoffMeasurable()
        {
            // Seeded 30-day simulation comparing Quarantine OFF vs Quarantine ON
            const int seed = 54321;
            var (wardA, diseaseA, _, coordA, invA) = CreateTestSetup(seed: seed, isolationBeds: 5);
            var (wardB, diseaseB, _, _, _) = CreateTestSetup(seed: seed, isolationBeds: 0);

            var cohort = new List<string>();
            for (int i = 0; i < 15; i++) cohort.Add($"cohort_{i}");

            // Seed initial infection
            diseaseA.Infect("cohort_0", DiseaseIds.ZoonoticFlu, 1);
            diseaseB.Infect("cohort_0", DiseaseIds.ZoonoticFlu, 1);

            int quarantinedBedDays = 0;

            for (int day = 1; day <= 30; day++)
            {
                // In A, isolate symptomatic patients if beds exist
                var snapA = diseaseA.GetSnapshot();
                for (int p = 0; p < snapA.patients.Count; p++)
                {
                    var pt = snapA.patients[p];
                    if (!pt.quarantined && coordA.IsIsolated(pt.survivor_id) == false)
                    {
                        coordA.ExecuteAssignIsolation(pt.survivor_id, day);
                    }
                }

                quarantinedBedDays += wardA.State.Admissions.FindAll(a => a.Status == MedicalAdmissionStatus.Active).Count;

                coordA.TickDaily(day);
                diseaseA.TickDaily(day, cohort);
                diseaseB.TickDaily(day, cohort);
            }

            // Quarantine on: incurs real resource burden & bed days
            Assert.True(quarantinedBedDays > 0, "Quarantine ON must incur bed days");
            Assert.True(invA["clean_water"] < 100, "Quarantine ON must consume clean water");

            // Secondary infections or deaths in A should be <= unisolated B
            Assert.True(diseaseA.TotalInfectionsHistory <= diseaseB.TotalInfectionsHistory,
                $"Secondary infections in A ({diseaseA.TotalInfectionsHistory}) should be <= B ({diseaseB.TotalInfectionsHistory})");
        }

        [Fact]
        public void B4_019_MedicalUiHonest()
        {
            var (_, diseaseSys, _, _, _) = CreateTestSetup();
            diseaseSys.Infect("ui_patient", DiseaseIds.Cholera, 1);

            var snap = diseaseSys.GetSnapshot();
            Assert.Equal(1, snap.total_infected);
            var p = snap.patients[0];
            Assert.Equal("ui_patient", p.survivor_id);
            Assert.Equal("Cholera", p.disease_name);
            Assert.Equal(DiseaseStageNames.Incubating, p.current_stage);
            Assert.Equal("incubating", p.stage_token);
        }

        [Fact]
        public void B4_020_FullQuarantinePolicyLoop()
        {
            var (ward, diseaseSys, roster, coord, inv) = CreateTestSetup(isolationBeds: 2);

            roster.WriteName("hero_patient", "Hero", "Scout", DutyRosterSystem.ScriptPencil, 1, true);
            roster.Assign(DutyRosterIds.RoleHatchOpener, "hero_patient");

            // 1. Exposure & Infection
            var exp = diseaseSys.TryInfect("hero_patient", DiseaseIds.Cholera, 1);
            Assert.True(exp.Infected);

            // 2. Preview isolation
            var preview = coord.PreviewAssignIsolation("hero_patient");
            Assert.True(preview.CanExecute);
            Assert.Equal(DutyRosterIds.RoleHatchOpener, preview.ConflictingRole);

            // 3. Execute isolation
            var assign = coord.ExecuteAssignIsolation("hero_patient", 1);
            Assert.True(assign.Success);
            Assert.True(coord.IsIsolated("hero_patient"));
            Assert.Null(roster.GetRoleOf("hero_patient")); // Duty unassigned

            // 4. Daily care drain
            int waterBefore = inv["clean_water"];
            coord.TickDaily(1);
            Assert.Equal(waterBefore - 1, inv["clean_water"]);

            // 5. Treat curatively
            var treat = diseaseSys.TryTreat("hero_patient", DiseaseIds.Cholera, "antibiotics", 1);
            Assert.True(treat.Accepted);
            Assert.True(treat.Cured);

            // 6. Immunity granted on recovery
            Assert.True(diseaseSys.HasImmunity("hero_patient", DiseaseIds.Cholera, 5));

            // 7. Release from isolation
            var release = coord.ExecuteReleaseIsolation("hero_patient", 2);
            Assert.True(release.Success);
            Assert.False(coord.IsIsolated("hero_patient"));

            // 8. Re-assignable to duty
            Assert.False(roster.IsSurvivorReservedExternally?.Invoke("hero_patient") == true);
            var reassigned = roster.Assign(DutyRosterIds.RoleHatchOpener, "hero_patient");
            Assert.True(reassigned);
        }
    }
}
