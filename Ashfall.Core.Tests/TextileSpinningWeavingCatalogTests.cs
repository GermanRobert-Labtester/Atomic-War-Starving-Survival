// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TextileSpinningWeavingCatalogTests : CatalogTestBase
    {
        private static string DataDir => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "Assets", "StreamingAssets", "Data", "narrative");

        private static TextileSpinningWeavingCatalog Load() =>
            TextileSpinningWeavingCatalog.LoadFromDirectory(DataDir);

        [Fact]
        public void DraftingLogs_StructuralContract_IsComplete()
        {
            var catalog = Load();
            AssertCounts(("DraftingLogs", catalog.DraftingLogs.Count, 8));
            AssertStringPropertiesPopulated(
                "DraftingLogs", catalog.DraftingLogs, e => e.Id,
                ("id", e => e.Id),
                ("spindle_unit_id", e => e.SpindleUnitId),
                ("fibre_stock_type", e => e.FibreStockType),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "DraftingLogs", catalog.DraftingLogs, e => e.Id,
                ("draft_ratio_target", e => (double)e.DraftRatioTarget));
        }

        [Fact]
        public void DraftingLogs_QueryByFibre_ReturnsResults()
        {
            Assert.NotEmpty(Load().GetDraftingLogsByFibre("nettle_bast"));
        }

        [Fact]
        public void DraftingLogs_QueryByFibre_CaseInsensitive()
        {
            var catalog = Load();
            var lower = catalog.GetDraftingLogsByFibre("hemp_tow");
            var upper = catalog.GetDraftingLogsByFibre("HEMP_TOW");
            Assert.Equal(
                new List<string>(System.Linq.Enumerable.Select(lower, e => e.Id)),
                new List<string>(System.Linq.Enumerable.Select(upper, e => e.Id)));
        }

        [Fact]
        public void WarpTallies_StructuralContract_IsComplete()
        {
            var catalog = Load();
            AssertCounts(("WarpTallies", catalog.WarpTallies.Count, 8));
            AssertStringPropertiesPopulated(
                "WarpTallies", catalog.WarpTallies, e => e.Id,
                ("id", e => e.Id),
                ("loom_frame_id", e => e.LoomFrameId),
                ("warp_fibre_type", e => e.WarpFibreType),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "WarpTallies", catalog.WarpTallies, e => e.Id,
                ("weft_thread_count", e => e.WeftThreadCount));
        }

        [Fact]
        public void WarpTallies_QueryByFrame_ReturnsResults()
        {
            Assert.NotEmpty(Load().GetWarpTalliesByFrame("il_peg_frame_01"));
        }

        [Fact]
        public void WarpTallies_QueryByFrame_CaseInsensitive()
        {
            var catalog = Load();
            var lower = catalog.GetWarpTalliesByFrame("bl_cord_frame_01");
            var upper = catalog.GetWarpTalliesByFrame("BL_CORD_FRAME_01");
            Assert.Equal(
                System.Linq.Enumerable.Count(lower),
                System.Linq.Enumerable.Count(upper));
        }

        [Fact]
        public void HeddleReports_StructuralContract_IsComplete()
        {
            var catalog = Load();
            AssertCounts(("HeddleReports", catalog.HeddleReports.Count, 7));
            AssertStringPropertiesPopulated(
                "HeddleReports", catalog.HeddleReports, e => e.Id,
                ("id", e => e.Id),
                ("treadle_unit_id", e => e.TreadleUnitId),
                ("tie_up_pattern", e => e.TieUpPattern),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "HeddleReports", catalog.HeddleReports, e => e.Id,
                ("heddle_count", e => e.HeddleCount));
        }

        [Fact]
        public void HeddleReports_QueryByPattern_ReturnsResults()
        {
            Assert.NotEmpty(Load().GetHeddleReportsByPattern("plain_weave"));
        }

        [Fact]
        public void HeddleReports_PlainWeaveCount_IsTwo()
        {
            var results = System.Linq.Enumerable.ToList(
                Load().GetHeddleReportsByPattern("plain_weave"));
            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void NapAssays_StructuralContract_IsComplete()
        {
            var catalog = Load();
            AssertCounts(("NapAssays", catalog.NapAssays.Count, 7));
            AssertStringPropertiesPopulated(
                "NapAssays", catalog.NapAssays, e => e.Id,
                ("id", e => e.Id),
                ("fulling_trough_id", e => e.FullingTroughId),
                ("cloth_substrate_type", e => e.ClothSubstrateType),
                ("log_text", e => e.LogText));
        }

        [Fact]
        public void NapAssays_QueryBySubstrate_ReturnsResults()
        {
            Assert.NotEmpty(Load().GetNapAssaysBySubstrate("hemp_plain_weave"));
        }

        [Fact]
        public void NapAssays_TeaselFulledQuery_ReturnsFourEntries()
        {
            var results = System.Linq.Enumerable.ToList(Load().GetTeaselFulledAssays());
            Assert.Equal(4, results.Count);
        }

        [Fact]
        public void NapAssays_LienenNoNapTool_HasNoneValue()
        {
            var catalog = Load();
            var linen = System.Linq.Enumerable.First(
                catalog.GetNapAssaysBySubstrate("linen_plain_weave"));
            Assert.Equal("none", linen.NapRaisingTool);
        }

        [Fact]
        public void AllEntries_TotalCount_IsThirty()
        {
            var catalog = Load();
            var total = catalog.DraftingLogs.Count
                      + catalog.WarpTallies.Count
                      + catalog.HeddleReports.Count
                      + catalog.NapAssays.Count;
            Assert.Equal(30, total);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var catalog = Load();
            var seen = new HashSet<string>();
            void Check(string id) => Assert.True(seen.Add(id), $"Duplicate id: {id}");

            foreach (var entry in catalog.DraftingLogs) Check(entry.Id);
            foreach (var entry in catalog.WarpTallies) Check(entry.Id);
            foreach (var entry in catalog.HeddleReports) Check(entry.Id);
            foreach (var entry in catalog.NapAssays) Check(entry.Id);
        }

        [Fact]
        public void AllEntries_LogTextsAreAtLeastTwentyChars()
        {
            var catalog = Load();
            void Check(string id, string text) =>
                Assert.True(text.Length >= 20, $"{id}: log_text too short");

            foreach (var entry in catalog.DraftingLogs) Check(entry.Id, entry.LogText);
            foreach (var entry in catalog.WarpTallies) Check(entry.Id, entry.LogText);
            foreach (var entry in catalog.HeddleReports) Check(entry.Id, entry.LogText);
            foreach (var entry in catalog.NapAssays) Check(entry.Id, entry.LogText);
        }
    }
}
