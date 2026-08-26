using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TextileSpinningWeavingCatalogTests
    : CatalogTestBase{
        private static string DataDir =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                         "..", "..", "..", "..",
                         "Assets", "StreamingAssets", "Data", "narrative");

        private static TextileSpinningWeavingCatalog Load() =>
            TextileSpinningWeavingCatalog.LoadFromDirectory(DataDir);

        // ── Batch 1: Drop-Spindle & Flyer-Wheel Fibre Drafting Logs ──────────────

        [Fact]
        public void DraftingLogs_LoadsEightEntries()
        {
            var catalog = Load();
            Assert.Equal(8, catalog.DraftingLogs.Count);
        }

        [Fact]
        public void DraftingLogs_AllIdsPopulated()
        {
            foreach (var e in Load().DraftingLogs)
                Assert.False(string.IsNullOrWhiteSpace(e.Id), $"Empty id in drafting log");
        }

        [Fact]
        public void DraftingLogs_AllSpindleUnitIdsPopulated()
        {
            foreach (var e in Load().DraftingLogs)
                Assert.False(string.IsNullOrWhiteSpace(e.SpindleUnitId),
                    $"{e.Id}: missing spindle_unit_id");
        }

        [Fact]
        public void DraftingLogs_AllFibreStockTypesPopulated()
        {
            foreach (var e in Load().DraftingLogs)
                Assert.False(string.IsNullOrWhiteSpace(e.FibreStockType),
                    $"{e.Id}: missing fibre_stock_type");
        }

        [Fact]
        public void DraftingLogs_AllDraftRatiosPositive()
        {
            foreach (var e in Load().DraftingLogs)
                Assert.True(e.DraftRatioTarget > 0f,
                    $"{e.Id}: draft_ratio_target must be positive");
        }

        [Fact]
        public void DraftingLogs_AllLogTextsPopulated()
        {
            foreach (var e in Load().DraftingLogs)
                Assert.False(string.IsNullOrWhiteSpace(e.LogText),
                    $"{e.Id}: missing log_text");
        }

        [Fact]
        public void DraftingLogs_QueryByFibre_ReturnsResults()
        {
            var results = Load().GetDraftingLogsByFibre("nettle_bast");
            Assert.NotEmpty(results);
        }

        [Fact]
        public void DraftingLogs_QueryByFibre_CaseInsensitive()
        {
            var catalog = Load();
            var lower   = catalog.GetDraftingLogsByFibre("hemp_tow");
            var upper   = catalog.GetDraftingLogsByFibre("HEMP_TOW");
            Assert.Equal(new System.Collections.Generic.List<string>(
                             System.Linq.Enumerable.Select(lower, e => e.Id)),
                         new System.Collections.Generic.List<string>(
                             System.Linq.Enumerable.Select(upper, e => e.Id)));
        }

        // ── Batch 2: Inkle & Backstrap Loom Warp/Weft Tally Sheets ──────────────

        [Fact]
        public void WarpTallies_LoadsEightEntries()
        {
            var catalog = Load();
            Assert.Equal(8, catalog.WarpTallies.Count);
        }

        [Fact]
        public void WarpTallies_AllIdsPopulated()
        {
            foreach (var e in Load().WarpTallies)
                Assert.False(string.IsNullOrWhiteSpace(e.Id), $"Empty id in warp tally");
        }

        [Fact]
        public void WarpTallies_AllLoomFrameIdsPopulated()
        {
            foreach (var e in Load().WarpTallies)
                Assert.False(string.IsNullOrWhiteSpace(e.LoomFrameId),
                    $"{e.Id}: missing loom_frame_id");
        }

        [Fact]
        public void WarpTallies_AllWarpFibreTypesPopulated()
        {
            foreach (var e in Load().WarpTallies)
                Assert.False(string.IsNullOrWhiteSpace(e.WarpFibreType),
                    $"{e.Id}: missing warp_fibre_type");
        }

        [Fact]
        public void WarpTallies_AllWeftCountsPositive()
        {
            foreach (var e in Load().WarpTallies)
                Assert.True(e.WeftThreadCount > 0,
                    $"{e.Id}: weft_thread_count must be positive");
        }

        [Fact]
        public void WarpTallies_AllLogTextsPopulated()
        {
            foreach (var e in Load().WarpTallies)
                Assert.False(string.IsNullOrWhiteSpace(e.LogText),
                    $"{e.Id}: missing log_text");
        }

        [Fact]
        public void WarpTallies_QueryByFrame_ReturnsResults()
        {
            var results = Load().GetWarpTalliesByFrame("il_peg_frame_01");
            Assert.NotEmpty(results);
        }

        [Fact]
        public void WarpTallies_QueryByFrame_CaseInsensitive()
        {
            var catalog = Load();
            var lower   = catalog.GetWarpTalliesByFrame("bl_cord_frame_01");
            var upper   = catalog.GetWarpTalliesByFrame("BL_CORD_FRAME_01");
            Assert.Equal(System.Linq.Enumerable.Count(lower),
                         System.Linq.Enumerable.Count(upper));
        }

        // ── Batch 3: Treadle Loom Heddle Threading & Tie-Up Reports ─────────────

        [Fact]
        public void HeddleReports_LoadsSevenEntries()
        {
            var catalog = Load();
            Assert.Equal(7, catalog.HeddleReports.Count);
        }

        [Fact]
        public void HeddleReports_AllIdsPopulated()
        {
            foreach (var e in Load().HeddleReports)
                Assert.False(string.IsNullOrWhiteSpace(e.Id), $"Empty id in heddle report");
        }

        [Fact]
        public void HeddleReports_AllTreadleUnitIdsPopulated()
        {
            foreach (var e in Load().HeddleReports)
                Assert.False(string.IsNullOrWhiteSpace(e.TreadleUnitId),
                    $"{e.Id}: missing treadle_unit_id");
        }

        [Fact]
        public void HeddleReports_AllHeddleCountsPositive()
        {
            foreach (var e in Load().HeddleReports)
                Assert.True(e.HeddleCount > 0,
                    $"{e.Id}: heddle_count must be positive");
        }

        [Fact]
        public void HeddleReports_AllTieUpPatternsPopulated()
        {
            foreach (var e in Load().HeddleReports)
                Assert.False(string.IsNullOrWhiteSpace(e.TieUpPattern),
                    $"{e.Id}: missing tie_up_pattern");
        }

        [Fact]
        public void HeddleReports_AllLogTextsPopulated()
        {
            foreach (var e in Load().HeddleReports)
                Assert.False(string.IsNullOrWhiteSpace(e.LogText),
                    $"{e.Id}: missing log_text");
        }

        [Fact]
        public void HeddleReports_QueryByPattern_ReturnsResults()
        {
            var results = Load().GetHeddleReportsByPattern("plain_weave");
            Assert.NotEmpty(results);
        }

        [Fact]
        public void HeddleReports_PlainWeaveCount_IsTwo()
        {
            var results = System.Linq.Enumerable.ToList(
                Load().GetHeddleReportsByPattern("plain_weave"));
            Assert.Equal(2, results.Count);
        }

        // ── Batch 4: Fulling Trough & Nap-Raising Surface-Finish Assays ──────────

        [Fact]
        public void NapAssays_LoadsSevenEntries()
        {
            var catalog = Load();
            Assert.Equal(7, catalog.NapAssays.Count);
        }

        [Fact]
        public void NapAssays_AllIdsPopulated()
        {
            foreach (var e in Load().NapAssays)
                Assert.False(string.IsNullOrWhiteSpace(e.Id), $"Empty id in nap assay");
        }

        [Fact]
        public void NapAssays_AllFullingTroughIdsPopulated()
        {
            foreach (var e in Load().NapAssays)
                Assert.False(string.IsNullOrWhiteSpace(e.FullingTroughId),
                    $"{e.Id}: missing fulling_trough_id");
        }

        [Fact]
        public void NapAssays_AllClothSubstratesPopulated()
        {
            foreach (var e in Load().NapAssays)
                Assert.False(string.IsNullOrWhiteSpace(e.ClothSubstrateType),
                    $"{e.Id}: missing cloth_substrate_type");
        }

        [Fact]
        public void NapAssays_AllLogTextsPopulated()
        {
            foreach (var e in Load().NapAssays)
                Assert.False(string.IsNullOrWhiteSpace(e.LogText),
                    $"{e.Id}: missing log_text");
        }

        [Fact]
        public void NapAssays_QueryBySubstrate_ReturnsResults()
        {
            var results = Load().GetNapAssaysBySubstrate("hemp_plain_weave");
            Assert.NotEmpty(results);
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

        // ── Cross-Batch Integrity ─────────────────────────────────────────────────

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
            var seen = new System.Collections.Generic.HashSet<string>();
            void Check(string id) => Assert.True(seen.Add(id), $"Duplicate id: {id}");

            foreach (var e in catalog.DraftingLogs)  Check(e.Id);
            foreach (var e in catalog.WarpTallies)   Check(e.Id);
            foreach (var e in catalog.HeddleReports) Check(e.Id);
            foreach (var e in catalog.NapAssays)     Check(e.Id);
        }

        [Fact]
        public void AllEntries_LogTextsAreAtLeastTwentyChars()
        {
            var catalog = Load();
            void Check(string id, string text) =>
                Assert.True(text.Length >= 20, $"{id}: log_text too short");

            foreach (var e in catalog.DraftingLogs)  Check(e.Id, e.LogText);
            foreach (var e in catalog.WarpTallies)   Check(e.Id, e.LogText);
            foreach (var e in catalog.HeddleReports) Check(e.Id, e.LogText);
            foreach (var e in catalog.NapAssays)     Check(e.Id, e.LogText);
        }
    }
}
