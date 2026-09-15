// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RopeMakingCordageCatalogTests : CatalogTestBase
    {
        private static string DataDir => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "Assets", "StreamingAssets", "Data", "narrative");

        private static RopeMakingCordageCatalog Load() =>
            RopeMakingCordageCatalog.LoadFromDirectory(DataDir);

        [Fact]
        public void Catalog_StructuralContract_IsComplete()
        {
            var catalog = Load();

            AssertCounts(
                ("HecklingLogs", catalog.HecklingLogs.Count, 8),
                ("StrandReports", catalog.StrandReports.Count, 8),
                ("ClosingLogs", catalog.ClosingLogs.Count, 7),
                ("BreakAssays", catalog.BreakAssays.Count, 7));

            AssertStringPropertiesPopulated(
                "HecklingLogs", catalog.HecklingLogs, e => e.Id,
                ("id", e => e.Id),
                ("fibre_source_plant", e => e.FibreSourcePlant),
                ("heckling_comb_id", e => e.HecklingCombId),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "HecklingLogs", catalog.HecklingLogs, e => e.Id,
                ("retting_days", e => e.RettingDays));

            AssertStringPropertiesPopulated(
                "StrandReports", catalog.StrandReports, e => e.Id,
                ("id", e => e.Id),
                ("fibre_type", e => e.FibreType),
                ("twist_direction", e => e.TwistDirection),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "StrandReports", catalog.StrandReports, e => e.Id,
                ("strand_count_per_yarn", e => e.StrandCountPerYarn));

            AssertStringPropertiesPopulated(
                "ClosingLogs", catalog.ClosingLogs, e => e.Id,
                ("id", e => e.Id),
                ("closing_tool", e => e.ClosingTool),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "ClosingLogs", catalog.ClosingLogs, e => e.Id,
                ("rope_diameter_mm", e => (double)e.RopeDiameterMm));

            AssertStringPropertiesPopulated(
                "BreakAssays", catalog.BreakAssays, e => e.Id,
                ("id", e => e.Id),
                ("failure_mode", e => e.FailureMode),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "BreakAssays", catalog.BreakAssays, e => e.Id,
                ("test_load_kg", e => (double)e.TestLoadKg));
        }

        [Fact] public void GetHecklingByPlant_Nettle_NotEmpty() =>
            Assert.NotEmpty(Load().GetHecklingLogsByPlant("nettle"));

        [Fact] public void GetStrandByFibre_Hemp_NotEmpty() =>
            Assert.NotEmpty(Load().GetStrandReportsByFibre("hemp_line"));

        [Fact] public void GetClosingByTool_TopHook_NotEmpty() =>
            Assert.NotEmpty(Load().GetClosingLogsByTool("top_hook_iron"));

        [Fact] public void GetBreakByFailureMode_StrandParting_NotEmpty() =>
            Assert.NotEmpty(Load().GetBreakAssaysByFailureMode("strand_parting"));

        [Fact] public void GetRopesAbove50kg_NotEmpty() =>
            Assert.NotEmpty(Load().GetRopesAboveTestLoad(50f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var catalog = Load();
            Assert.Equal(30,
                catalog.HecklingLogs.Count + catalog.StrandReports.Count +
                catalog.ClosingLogs.Count + catalog.BreakAssays.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var catalog = Load();
            foreach (var entry in catalog.HecklingLogs) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.StrandReports) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.ClosingLogs) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.BreakAssays) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var catalog = Load();
            foreach (var entry in catalog.HecklingLogs) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.StrandReports) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.ClosingLogs) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.BreakAssays) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
        }
    }
}
