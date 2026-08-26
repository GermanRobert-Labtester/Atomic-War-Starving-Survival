using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Vector-Block Mem — Subterranean Textile Spinning & Weaving
    //  Four entry types:
    //    • DropSpindleDraftingLog     – drop-spindle and flyer-wheel fibre drafting
    //    • InkleLoomWarpTally         – inkle and backstrap loom warp/weft tallies
    //    • TreadleLoomHeddleReport    – treadle loom heddle threading and tie-up reports
    //    • FullingTroughNapAssay      – fulling trough and nap-raising surface-finish assays
    // ─────────────────────────────────────────────────────────────────────────────

    // ── Batch 1: Drop-Spindle & Flyer-Wheel Fibre Drafting Logs ─────────────────

    public sealed class DropSpindleDraftingLog
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("spindle_unit_id")]
        public string SpindleUnitId { get; init; } = string.Empty;

        [JsonPropertyName("fibre_stock_type")]
        public string FibreStockType { get; init; } = string.Empty;

        [JsonPropertyName("draft_ratio_target")]
        public float DraftRatioTarget { get; init; }

        [JsonPropertyName("log_text")]
        public string LogText { get; init; } = string.Empty;
    }

    // ── Batch 2: Inkle & Backstrap Loom Warp/Weft Tally Sheets ─────────────────

    public sealed class InkleLoomWarpTally
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("loom_frame_id")]
        public string LoomFrameId { get; init; } = string.Empty;

        [JsonPropertyName("warp_fibre_type")]
        public string WarpFibreType { get; init; } = string.Empty;

        [JsonPropertyName("weft_thread_count")]
        public int WeftThreadCount { get; init; }

        [JsonPropertyName("log_text")]
        public string LogText { get; init; } = string.Empty;
    }

    // ── Batch 3: Treadle Loom Heddle Threading & Tie-Up Reports ─────────────────

    public sealed class TreadleLoomHeddleReport
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("treadle_unit_id")]
        public string TreadleUnitId { get; init; } = string.Empty;

        [JsonPropertyName("heddle_count")]
        public int HeddleCount { get; init; }

        [JsonPropertyName("tie_up_pattern")]
        public string TieUpPattern { get; init; } = string.Empty;

        [JsonPropertyName("log_text")]
        public string LogText { get; init; } = string.Empty;
    }

    // ── Batch 4: Fulling Trough & Nap-Raising Surface-Finish Assays ─────────────

    public sealed class FullingTroughNapAssay
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("fulling_trough_id")]
        public string FullingTroughId { get; init; } = string.Empty;

        [JsonPropertyName("cloth_substrate_type")]
        public string ClothSubstrateType { get; init; } = string.Empty;

        [JsonPropertyName("nap_raising_tool")]
        public string NapRaisingTool { get; init; } = string.Empty;

        [JsonPropertyName("log_text")]
        public string LogText { get; init; } = string.Empty;
    }

    // ── Catalog ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Loads and queries all four Vector-Block Mem narrative datasets:
    /// fibre drafting logs, loom warp tallies, heddle threading reports,
    /// and fulling-trough surface-finish assays.
    /// </summary>
    public sealed class TextileSpinningWeavingCatalog
    {
        public IReadOnlyList<DropSpindleDraftingLog>  DraftingLogs  { get; }
        public IReadOnlyList<InkleLoomWarpTally>      WarpTallies   { get; }
        public IReadOnlyList<TreadleLoomHeddleReport> HeddleReports { get; }
        public IReadOnlyList<FullingTroughNapAssay>   NapAssays     { get; }

        private static readonly JsonSerializerOptions _opts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private TextileSpinningWeavingCatalog(
            IReadOnlyList<DropSpindleDraftingLog>  draftingLogs,
            IReadOnlyList<InkleLoomWarpTally>      warpTallies,
            IReadOnlyList<TreadleLoomHeddleReport> heddleReports,
            IReadOnlyList<FullingTroughNapAssay>   napAssays)
        {
            DraftingLogs  = draftingLogs;
            WarpTallies   = warpTallies;
            HeddleReports = heddleReports;
            NapAssays     = napAssays;
        }

        /// <summary>Loads all four batches from a common directory.</summary>
        public static TextileSpinningWeavingCatalog LoadFromDirectory(string directoryPath)
        {
            return new TextileSpinningWeavingCatalog(
                Load<DropSpindleDraftingLog> (directoryPath, "drop_spindle_fibre_drafting_logs.json"),
                Load<InkleLoomWarpTally>     (directoryPath, "inkle_loom_warp_tally_sheets.json"),
                Load<TreadleLoomHeddleReport>(directoryPath, "treadle_loom_heddle_reports.json"),
                Load<FullingTroughNapAssay>  (directoryPath, "fulling_trough_nap_assays.json")
            );
        }

        private static IReadOnlyList<T> Load<T>(string dir, string file)
        {
            var path = Path.Combine(dir, file);
            var json = File.ReadAllText(path);
            return CatalogLocator.LoadWrappedList<T>(json, _opts);
        }

        // ── Queries ──────────────────────────────────────────────────────────────

        /// <summary>Returns drafting logs for a specific fibre stock type.</summary>
        public IEnumerable<DropSpindleDraftingLog> GetDraftingLogsByFibre(string fibreType)
        {
            foreach (var e in DraftingLogs)
                if (string.Equals(e.FibreStockType, fibreType, StringComparison.OrdinalIgnoreCase))
                    yield return e;
        }

        /// <summary>Returns warp tallies for a given loom frame id.</summary>
        public IEnumerable<InkleLoomWarpTally> GetWarpTalliesByFrame(string frameId)
        {
            foreach (var e in WarpTallies)
                if (string.Equals(e.LoomFrameId, frameId, StringComparison.OrdinalIgnoreCase))
                    yield return e;
        }

        /// <summary>Returns heddle reports for a given tie-up pattern.</summary>
        public IEnumerable<TreadleLoomHeddleReport> GetHeddleReportsByPattern(string pattern)
        {
            foreach (var e in HeddleReports)
                if (string.Equals(e.TieUpPattern, pattern, StringComparison.OrdinalIgnoreCase))
                    yield return e;
        }

        /// <summary>Returns nap assays for a given cloth substrate type.</summary>
        public IEnumerable<FullingTroughNapAssay> GetNapAssaysBySubstrate(string substrate)
        {
            foreach (var e in NapAssays)
                if (string.Equals(e.ClothSubstrateType, substrate, StringComparison.OrdinalIgnoreCase))
                    yield return e;
        }

        /// <summary>Returns all nap assays that used a teasel head.</summary>
        public IEnumerable<FullingTroughNapAssay> GetTeaselFulledAssays()
        {
            foreach (var e in NapAssays)
                if (string.Equals(e.NapRaisingTool, "teasel_head", StringComparison.OrdinalIgnoreCase))
                    yield return e;
        }
    }
}
