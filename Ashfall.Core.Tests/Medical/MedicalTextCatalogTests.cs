// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class MedicalTextCatalogTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void MedicalTextCatalog_LoadsRealFileCorrectly()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();

            var catalog = MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            Assert.NotNull(catalog);
            Assert.Equal(83, catalog.Count);
            Assert.Equal(83, catalog.AllConditions.Count);

            // Verify alphabetical sort by ID
            for (int i = 1; i < catalog.AllConditions.Count; i++)
            {
                Assert.True(string.CompareOrdinal(catalog.AllConditions[i - 1].id, catalog.AllConditions[i].id) < 0,
                    $"Conditions not sorted: {catalog.AllConditions[i - 1].id} vs {catalog.AllConditions[i].id}");
            }

            foreach (var c in catalog.AllConditions)
            {
                Assert.False(string.IsNullOrWhiteSpace(c.id), "Condition missing id");
                Assert.False(string.IsNullOrWhiteSpace(c.category), $"Condition {c.id} missing category");
                Assert.False(string.IsNullOrWhiteSpace(c.display_name), $"Condition {c.id} missing display_name");
                Assert.False(string.IsNullOrWhiteSpace(c.diagnosis_text), $"Condition {c.id} missing diagnosis_text");
            }
        }

        [Fact]
        public void MedicalTextCatalog_TryGetConditionText_RetrievesExpectedEntry()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            var rad = catalog.TryGetConditionText("medical_radiation_exposure");
            Assert.NotNull(rad);
            Assert.Equal("Radiation Exposure", rad.display_name);
            Assert.Equal("illness", rad.category);
            Assert.NotEmpty(rad.symptom_descriptions);
            Assert.NotEmpty(rad.treatment_steps);

            var fracture = catalog.TryGetConditionText("medical_fracture");
            Assert.NotNull(fracture);
            Assert.Equal("Fracture", fracture.display_name);
            Assert.Equal("injury", fracture.category);

            var asthma = catalog.TryGetConditionText("medical_asthma");
            Assert.NotNull(asthma);
            Assert.Equal("Asthma", asthma.display_name);
            Assert.Equal("chronic_disease", asthma.category);
        }

        [Fact]
        public void MedicalTextCatalog_MissingOrUnknownId_ReturnsNullGracefully()
        {
            var catalog = new MedicalTextCatalog();
            Assert.Null(catalog.TryGetConditionText("nonexistent_condition"));
            Assert.Null(catalog.TryGetConditionText(null));
            Assert.Null(catalog.TryGetConditionText(""));
            Assert.Equal(string.Empty, catalog.GetSafeDiagnosisSummary("unknown_id"));
            Assert.Equal(string.Empty, catalog.GetSymptomProse("unknown_id"));
            Assert.Equal(string.Empty, catalog.GetComplicationWarning("unknown_id"));
            Assert.Equal(string.Empty, catalog.GetRecoveryProse("unknown_id"));
        }

        [Fact]
        public void MedicalTextCatalog_DeterministicSymptomSelection_StableAcrossCalls()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            int testSeed = 42;
            string s1 = catalog.GetSymptomProse("medical_radiation_exposure", testSeed);
            string s2 = catalog.GetSymptomProse("medical_radiation_exposure", testSeed);
            string s3 = catalog.GetSymptomProse("medical_radiation_exposure", testSeed);

            Assert.False(string.IsNullOrEmpty(s1));
            Assert.Equal(s1, s2);
            Assert.Equal(s1, s3);
        }

        [Fact]
        public void MedicalTextCatalog_AccuracyAudit_NoDebunkedClaims()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            // Audit 1: Bone healing myth ("stronger at the break point") must NOT exist
            var fracture = catalog.TryGetConditionText("medical_fracture");
            Assert.NotNull(fracture);
            foreach (var effect in fracture.long_term_effects)
            {
                Assert.DoesNotContain("stronger at the break point", effect, StringComparison.OrdinalIgnoreCase);
            }

            // Audit 2: "radiation heating from the inside" language must NOT exist
            var rad = catalog.TryGetConditionText("medical_radiation_exposure");
            Assert.NotNull(rad);
            foreach (var pain in rad.pain_descriptions)
            {
                Assert.DoesNotContain("burning from the inside", pain, StringComparison.OrdinalIgnoreCase);
            }

            // Audit 3: No condition in the catalog should claim stinging indicates efficacy
            foreach (var c in catalog.AllConditions)
            {
                string corpus = string.Join(" ", c.pain_descriptions.Concat(c.treatment_steps).Concat(c.recovery_descriptions));
                Assert.DoesNotContain("means it is working", corpus, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void MedicalConditionResolver_ResolvesRuntimeAfflictions()
        {
            Assert.Equal("medical_asthma", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.RespiratoryDegenerationId));
            Assert.Equal("medical_radiation_exposure", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.RadiationSicknessId));
            Assert.Equal("medical_addiction", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.ChemicalDependencyId));
            Assert.Equal("medical_laceration", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.HealthDeficitId));

            Assert.Equal("medical_ptsd", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.CombatTraumaId));
            Assert.Equal("medical_panic_attack", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.SomaticFlashbackId));
            Assert.Equal("medical_insomnia", MedicalConditionResolver.ResolveToMedicalTextId(MedicalTreatmentCatalog.GuiltInsomniaId));

            Assert.Equal("medical_radiation_exposure", MedicalConditionResolver.ResolveToMedicalTextId("disease_acute_radiation_syndrome"));
            Assert.Equal("medical_wound_infection", MedicalConditionResolver.ResolveToMedicalTextId("disease_septic_rust_wound_fever"));
            Assert.Equal("medical_infection", MedicalConditionResolver.ResolveToMedicalTextId("disease_spore_wound_dermatitis"));
            Assert.Equal("medical_dehydration_severe", MedicalConditionResolver.ResolveToMedicalTextId("disease_cholera"));

            Assert.Equal("medical_laceration", MedicalConditionResolver.ResolveToMedicalTextId("medical_laceration"));
            Assert.Null(MedicalConditionResolver.ResolveToMedicalTextId("unknown_unmapped_condition_id"));
            Assert.Null(MedicalConditionResolver.ResolveToMedicalTextId(null));
        }

        [Fact]
        public void MedicalConditionResolver_GetClinicalProse_DeterministicForSurvivor()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            var prose1 = MedicalConditionResolver.GetClinicalProse(catalog, MedicalTreatmentCatalog.RadiationSicknessId, "survivor_sarah_chen");
            var prose2 = MedicalConditionResolver.GetClinicalProse(catalog, MedicalTreatmentCatalog.RadiationSicknessId, "survivor_sarah_chen");

            Assert.NotNull(prose1);
            Assert.NotNull(prose2);
            Assert.Equal("Radiation Exposure", prose1.DisplayName);
            Assert.Equal(prose1.DiagnosisSummary, prose2.DiagnosisSummary);
            Assert.Equal(prose1.SymptomLine, prose2.SymptomLine);
            Assert.Equal(prose1.ComplicationWarning, prose2.ComplicationWarning);
        }

        [Fact]
        public void DwellerMedicalCatalog_LoadsCombinedCorpus_76CasesWithoutCollisions()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();

            var catalog = DwellerMedicalCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            Assert.Equal(76, catalog.Count);
            Assert.Equal(76, catalog.AllCases.Count);

            var case1 = catalog.GetById("med_01_acute_rad_blast_exposure");
            Assert.NotNull(case1);
            Assert.Equal("Pvt. Victor S.", case1.patient_name);

            // Verify entry from expansion file exists and doc_type is loaded
            var expCase = catalog.GetById("med_intake_01_conscript");
            Assert.NotNull(expCase);
            Assert.False(string.IsNullOrEmpty(expCase.doc_type));

            // Verify unique IDs across all 76
            var uniqueIds = new HashSet<string>(catalog.AllCases.Select(c => c.case_id), StringComparer.OrdinalIgnoreCase);
            Assert.Equal(76, uniqueIds.Count);
        }

        [Fact]
        public void MedicalTextCatalog_EmptyOrCorruptedJson_FailsGracefully()
        {
            var serializer = new SystemTextJsonSerializer();
            var catalog = new MedicalTextCatalog();

            // Null/empty string should not throw
            catalog.Load(string.Empty, serializer);
            Assert.Equal(0, catalog.Count);

            catalog.Load("   ", serializer);
            Assert.Equal(0, catalog.Count);
        }

        [Fact]
        public void MedicalTextCatalog_DeduplicatesDuplicateIds()
        {
            string jsonWithDuplicate = @"{
                ""schema_version"": 1,
                ""collection_id"": ""medical_texts"",
                ""conditions"": [
                    {
                        ""id"": ""medical_test_dupe"",
                        ""category"": ""injury"",
                        ""display_name"": ""First"",
                        ""diagnosis_text"": ""Original diagnosis""
                    },
                    {
                        ""id"": ""medical_test_dupe"",
                        ""category"": ""injury"",
                        ""display_name"": ""Second"",
                        ""diagnosis_text"": ""Duplicate diagnosis""
                    }
                ]
            }";

            var serializer = new SystemTextJsonSerializer();
            var catalog = new MedicalTextCatalog();
            catalog.Load(jsonWithDuplicate, serializer);

            Assert.Equal(1, catalog.Count);
            var entry = catalog.TryGetConditionText("medical_test_dupe");
            Assert.NotNull(entry);
            Assert.Equal("First", entry.display_name);
            Assert.Equal("Original diagnosis", entry.diagnosis_text);
        }
    }
}
