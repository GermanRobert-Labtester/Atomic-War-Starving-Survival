using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DiseaseCatalogExpansionTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static DiseaseCatalog LoadCatalog()
        {
            return DiseaseCatalogLoader.Load(
                DataDir(),
                new FileSystemIO(),
                new SystemTextJsonSerializer());
        }

        [Fact]
        public void Catalog_ReconcilesToTwenty_AndPreservesTheLiveSixteenPrefix()
        {
            var catalog = LoadCatalog();

            Assert.False(catalog.HasErrors, string.Join("; ", catalog.Errors));
            Assert.Equal(20, catalog.Count);

            var preservedPrefix = new[]
            {
                "disease_cholera",
                "disease_zoonotic_flu",
                "disease_blood_fever",
                "disease_spore_blight",
                "disease_acute_radiation_syndrome",
                "disease_fungal_respiratory",
                "disease_typhoid_waterborne",
                "disease_wellspring_cramps",
                "disease_silt_jaundice",
                "disease_condemned_air_cough",
                "disease_dry_bunker_hiss",
                "disease_septic_rust_wound_fever",
                "disease_reused_needle_fever",
                "disease_deep_excavation_mold_lung",
                "disease_silo_lung",
                "disease_prion_tremor"
            };

            Assert.Equal(
                preservedPrefix,
                catalog.Diseases.Take(preservedPrefix.Length).Select(d => d.id).ToArray());
            Assert.Equal(
                new[]
                {
                    "disease_dysentery",
                    "disease_meningococcal_fever",
                    "disease_bloodborne_hepatitis",
                    "disease_spore_wound_dermatitis"
                },
                catalog.Diseases.Skip(preservedPrefix.Length).Select(d => d.id).ToArray());
            Assert.Equal(20, catalog.Diseases.Select(d => d.id).Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void NewEntries_UseSupportedVectors_AndResolvableInventoryReferences()
        {
            var catalog = LoadCatalog();
            var items = new HashSet<string>(
                ItemCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer())
                    .Select(item => item.id),
                StringComparer.Ordinal);
            var supportedVectors = new HashSet<string>(StringComparer.Ordinal)
            {
                DiseaseVectorNames.Water,
                DiseaseVectorNames.Air,
                DiseaseVectorNames.Blood,
                DiseaseVectorNames.Spore
            };

            foreach (var disease in catalog.Diseases)
            {
                Assert.Contains(disease.vector, supportedVectors);
                Assert.Contains(disease.countermeasure_item_id, items);
                Assert.NotEmpty(disease.guidance);
                Assert.NotEmpty(disease.source_note);
                Assert.NotEmpty(disease.treatments);
                Assert.NotEmpty(disease.phases);
            }
        }

        [Fact]
        public void NewEntries_RegisterThroughTheExistingDiseaseRuntimePath()
        {
            var catalog = LoadCatalog();
            var system = new DiseaseSystem(rng: new SeededRng(112));
            system.BindCatalog(catalog);

            foreach (string diseaseId in new[]
            {
                "disease_dysentery",
                "disease_meningococcal_fever",
                "disease_bloodborne_hepatitis",
                "disease_spore_wound_dermatitis"
            })
            {
                string survivorId = "plan112_" + diseaseId;
                Assert.NotNull(system.GetDefinition(diseaseId));
                Assert.NotEmpty(system.GetTransmissionVector(diseaseId));

                system.Infect(survivorId, diseaseId, 1);

                Assert.True(system.IsInfected(survivorId, diseaseId));
            }
        }
    }
}
