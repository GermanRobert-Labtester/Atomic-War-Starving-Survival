using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DwellerMedicalCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void DwellerMedical_LoadsAll40CanonicalCaseRecords()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "dweller_medical_casebook.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new DwellerMedicalCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(40, catalog.AllCases.Count);

            // Test first case (ARS Victor Day 2)
            var first = catalog.GetById("med_01_acute_rad_blast_exposure");
            Assert.NotNull(first);
            Assert.Equal("Dr. Irina Vel", first.attending_physician);
            Assert.Equal("Pvt. Victor S.", first.patient_name);
            Assert.Equal(4200.0f, first.dose_estimate_msv);
            Assert.Contains("Prussian Blue", first.intervention);

            // Test Vel deathbed case (Day 620)
            var velCase = catalog.GetById("med_20_vel_final_bedside_chart");
            Assert.NotNull(velCase);
            Assert.Equal("Sonya (Apprentice Surgeon)", velCase.attending_physician);
            Assert.Equal("Dr. Irina Vel", velCase.patient_name);
            Assert.Contains("thirty-one names", velCase.doctor_margin_note);

            // Test final case (Day 3650)
            var final = catalog.GetById("med_40_the_final_casebook_inscription");
            Assert.NotNull(final);
            Assert.Equal(3650, final.recorded_day);
            Assert.Contains("Sun Meadow Hospital", final.intervention);

            // Test physician query
            var velCases = catalog.GetByPhysician("Vel");
            var sonyaCases = catalog.GetByPhysician("Sonya");
            Assert.True(velCases.Count >= 18);
            Assert.True(sonyaCases.Count >= 20);

            // Test category search
            var oncology = catalog.GetByCategory("Oncology");
            Assert.True(oncology.Count >= 3);
        }

        [Fact]
        public void DwellerMedical_AllEntriesHaveValidInterventionsAndNotes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "dweller_medical_casebook.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new DwellerMedicalCatalog();
            catalog.Load(json, serializer);

            foreach (var c in catalog.AllCases)
            {
                Assert.False(string.IsNullOrWhiteSpace(c.case_id), "Missing case_id");
                Assert.True(c.recorded_day > 0, $"Invalid recorded_day on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.attending_physician), $"Missing physician on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.patient_id), $"Missing patient_id on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.patient_name), $"Missing patient_name on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.category), $"Missing category on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.symptoms), $"Missing symptoms on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.intervention), $"Missing intervention on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.outcome), $"Missing outcome on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.doctor_margin_note), $"Missing margin note on {c.case_id}");
                Assert.True(c.doctor_margin_note.Length > 30, $"Margin note too brief on {c.case_id}");
                Assert.NotNull(c.tags);
                Assert.True(c.tags.Length > 0, $"Tags empty on {c.case_id}");
            }
        }
    }
}
