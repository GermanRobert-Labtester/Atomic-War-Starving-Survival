// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TanningLeatherworkCatalogTests : CatalogTestBase
    {
        private static string DataDir => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "Assets", "StreamingAssets", "Data", "narrative");

        private static TanningLeatherworkCatalog Load() =>
            TanningLeatherworkCatalog.LoadFromDirectory(DataDir);

        [Fact]
        public void Catalog_StructuralContract_IsComplete()
        {
            var catalog = Load();

            AssertCounts(
                ("VatLogs", catalog.VatLogs.Count, 8),
                ("HideReports", catalog.HideReports.Count, 8),
                ("CurryingAssays", catalog.CurryingAssays.Count, 7),
                ("StitchJournals", catalog.StitchJournals.Count, 7));

            AssertStringPropertiesPopulated(
                "VatLogs", catalog.VatLogs, e => e.Id,
                ("id", e => e.Id),
                ("bark_species", e => e.BarkSpecies),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "VatLogs", catalog.VatLogs, e => e.Id,
                ("liquor_strength_baume", e => (double)e.LiquorStrengthBaume));

            AssertStringPropertiesPopulated(
                "HideReports", catalog.HideReports, e => e.Id,
                ("id", e => e.Id),
                ("hide_source_animal", e => e.HideSourceAnimal),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "HideReports", catalog.HideReports, e => e.Id,
                ("smoke_cycle_count", e => e.SmokeCycleCount));

            AssertStringPropertiesPopulated(
                "CurryingAssays", catalog.CurryingAssays, e => e.Id,
                ("id", e => e.Id),
                ("fat_liquor_type", e => e.FatLiquorType),
                ("log_text", e => e.LogText));

            AssertStringPropertiesPopulated(
                "StitchJournals", catalog.StitchJournals, e => e.Id,
                ("id", e => e.Id),
                ("thread_material", e => e.ThreadMaterial),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "StitchJournals", catalog.StitchJournals, e => e.Id,
                ("stitch_length_mm", e => (double)e.StitchLengthMm));
        }

        [Fact] public void GetVatLogsByBarkSpecies_ReturnsResults() =>
            Assert.NotEmpty(Load().GetVatLogsByBarkSpecies("oak"));

        [Fact] public void GetBrainTanByAnimal_ReturnsResults() =>
            Assert.NotEmpty(Load().GetBrainTanReportsByAnimal("dog"));

        [Fact] public void GetCurryingByFatLiquor_ReturnsResults() =>
            Assert.NotEmpty(Load().GetCurryingAssaysByFatLiquor("tallow"));

        [Fact] public void GetStitchByThread_ReturnsResults() =>
            Assert.NotEmpty(Load().GetStitchJournalsByThread("sinew"));

        [Fact] public void GetSmokeCycleReports_MinTwo_NotEmpty() =>
            Assert.NotEmpty(Load().GetSmokeCycleReports(2));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var catalog = Load();
            Assert.Equal(30,
                catalog.VatLogs.Count + catalog.HideReports.Count +
                catalog.CurryingAssays.Count + catalog.StitchJournals.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var catalog = Load();
            foreach (var entry in catalog.VatLogs) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.HideReports) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.CurryingAssays) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.StitchJournals) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var catalog = Load();
            foreach (var entry in catalog.VatLogs) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.HideReports) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.CurryingAssays) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.StitchJournals) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
        }
    }
}
