// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CandleMakingWaxCatalogTests : CatalogTestBase
    {
        private static string DataDir => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "Assets", "StreamingAssets", "Data", "narrative");

        private static CandleMakingWaxCatalog Load() =>
            CandleMakingWaxCatalog.LoadFromDirectory(DataDir);

        [Fact]
        public void Catalog_StructuralContract_IsComplete()
        {
            var catalog = Load();

            AssertCounts(
                ("TallowLogs", catalog.TallowLogs.Count, 8),
                ("WaxRecords", catalog.WaxRecords.Count, 8),
                ("WickReports", catalog.WickReports.Count, 7),
                ("CandleAssays", catalog.CandleAssays.Count, 7));

            AssertStringPropertiesPopulated(
                "TallowLogs", catalog.TallowLogs, e => e.Id,
                ("id", e => e.Id),
                ("fat_source_animal", e => e.FatSourceAnimal),
                ("rendering_vat_id", e => e.RenderingVatId),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "TallowLogs", catalog.TallowLogs, e => e.Id,
                ("yield_grams", e => (double)e.YieldGrams));

            AssertStringPropertiesPopulated(
                "WaxRecords", catalog.WaxRecords, e => e.Id,
                ("id", e => e.Id),
                ("clarification_method", e => e.ClarificationMethod),
                ("clarity_grade", e => e.ClarityGrade),
                ("log_text", e => e.LogText));

            AssertStringPropertiesPopulated(
                "WickReports", catalog.WickReports, e => e.Id,
                ("id", e => e.Id),
                ("wick_fibre_type", e => e.WickFibreType),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "WickReports", catalog.WickReports, e => e.Id,
                ("braid_ply_count", e => e.BraidPlyCount));

            AssertStringPropertiesPopulated(
                "CandleAssays", catalog.CandleAssays, e => e.Id,
                ("id", e => e.Id),
                ("candle_method", e => e.CandleMethod),
                ("wax_blend_type", e => e.WaxBlendType),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "CandleAssays", catalog.CandleAssays, e => e.Id,
                ("burn_duration_hours", e => (double)e.BurnDurationHours));
        }

        [Fact] public void GetTallowByAnimal_Dog_NotEmpty() =>
            Assert.NotEmpty(Load().GetTallowLogsByAnimal("dog"));

        [Fact] public void GetClarificationByMethod_Float_NotEmpty() =>
            Assert.NotEmpty(Load().GetClarificationRecordsByMethod("hot_water_float"));

        [Fact] public void GetWickByFibre_Cotton_NotEmpty() =>
            Assert.NotEmpty(Load().GetWickReportsByFibre("cotton_rag_strip"));

        [Fact] public void GetCandleByMethod_Dipping_NotEmpty() =>
            Assert.NotEmpty(Load().GetCandleAssaysByMethod("dipping"));

        [Fact] public void GetLongBurningCandles_3h_NotEmpty() =>
            Assert.NotEmpty(Load().GetLongBurningCandles(3f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var catalog = Load();
            Assert.Equal(30,
                catalog.TallowLogs.Count + catalog.WaxRecords.Count +
                catalog.WickReports.Count + catalog.CandleAssays.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var catalog = Load();
            foreach (var entry in catalog.TallowLogs) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.WaxRecords) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.WickReports) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.CandleAssays) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var catalog = Load();
            foreach (var entry in catalog.TallowLogs) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.WaxRecords) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.WickReports) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.CandleAssays) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
        }
    }
}
