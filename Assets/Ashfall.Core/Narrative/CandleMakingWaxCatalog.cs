using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    // ── Vector-Block Pe — Subterranean Candle Making & Wax Rendering ────────────

    public sealed class TallowRenderingVatLog
    {
        [JsonPropertyName("id")]                public string Id              { get; init; } = string.Empty;
        [JsonPropertyName("fat_source_animal")] public string FatSourceAnimal { get; init; } = string.Empty;
        [JsonPropertyName("rendering_vat_id")]  public string RenderingVatId  { get; init; } = string.Empty;
        [JsonPropertyName("yield_grams")]       public float  YieldGrams      { get; init; }
        [JsonPropertyName("log_text")]          public string LogText          { get; init; } = string.Empty;
    }

    public sealed class BeeswaxClarificationRecord
    {
        [JsonPropertyName("id")]                    public string Id                  { get; init; } = string.Empty;
        [JsonPropertyName("wax_lot_source")]        public string WaxLotSource        { get; init; } = string.Empty;
        [JsonPropertyName("clarification_method")] public string ClarificationMethod  { get; init; } = string.Empty;
        [JsonPropertyName("clarity_grade")]         public string ClarityGrade         { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]              public string LogText              { get; init; } = string.Empty;
    }

    public sealed class WickBraidingPrimingReport
    {
        [JsonPropertyName("id")]                public string Id              { get; init; } = string.Empty;
        [JsonPropertyName("wick_fibre_type")]   public string WickFibreType   { get; init; } = string.Empty;
        [JsonPropertyName("braid_ply_count")]   public int    BraidPlyCount   { get; init; }
        [JsonPropertyName("priming_wax_type")]  public string PrimingWaxType  { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]          public string LogText          { get; init; } = string.Empty;
    }

    public sealed class CandleDipMouldAssay
    {
        [JsonPropertyName("id")]                    public string Id                  { get; init; } = string.Empty;
        [JsonPropertyName("candle_method")]         public string CandleMethod         { get; init; } = string.Empty;
        [JsonPropertyName("wax_blend_type")]        public string WaxBlendType         { get; init; } = string.Empty;
        [JsonPropertyName("burn_duration_hours")]   public float  BurnDurationHours    { get; init; }
        [JsonPropertyName("log_text")]              public string LogText              { get; init; } = string.Empty;
    }

    /// <summary>
    /// Loads and queries all four Vector-Block Pe narrative datasets:
    /// tallow rendering logs, beeswax clarification records,
    /// wick braiding/priming reports, and candle dip/mould assays.
    /// </summary>
    public sealed class CandleMakingWaxCatalog
    {
        public IReadOnlyList<TallowRenderingVatLog>     TallowLogs     { get; }
        public IReadOnlyList<BeeswaxClarificationRecord> WaxRecords    { get; }
        public IReadOnlyList<WickBraidingPrimingReport> WickReports    { get; }
        public IReadOnlyList<CandleDipMouldAssay>       CandleAssays   { get; }

        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        private CandleMakingWaxCatalog(
            IReadOnlyList<TallowRenderingVatLog>     tallowLogs,
            IReadOnlyList<BeeswaxClarificationRecord> waxRecords,
            IReadOnlyList<WickBraidingPrimingReport> wickReports,
            IReadOnlyList<CandleDipMouldAssay>       candleAssays)
        {
            TallowLogs   = tallowLogs;
            WaxRecords   = waxRecords;
            WickReports  = wickReports;
            CandleAssays = candleAssays;
        }

        public static CandleMakingWaxCatalog LoadFromDirectory(string directoryPath) =>
            new(
                Load<TallowRenderingVatLog>     (directoryPath, "tallow_rendering_vat_logs.json"),
                Load<BeeswaxClarificationRecord>(directoryPath, "beeswax_clarification_records.json"),
                Load<WickBraidingPrimingReport> (directoryPath, "wick_braiding_priming_reports.json"),
                Load<CandleDipMouldAssay>       (directoryPath, "candle_dip_mould_assays.json")
            );

        private static IReadOnlyList<T> Load<T>(string dir, string file)
        {
            var json = File.ReadAllText(Path.Combine(dir, file));
            return JsonSerializer.Deserialize<List<T>>(json, _opts)
                   ?? throw new InvalidOperationException($"Failed to deserialize {file}");
        }

        public IEnumerable<TallowRenderingVatLog>     GetTallowLogsByAnimal(string animal)          { foreach (var e in TallowLogs)   if (string.Equals(e.FatSourceAnimal,      animal,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<BeeswaxClarificationRecord> GetClarificationRecordsByMethod(string method){ foreach (var e in WaxRecords)    if (string.Equals(e.ClarificationMethod, method,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<WickBraidingPrimingReport> GetWickReportsByFibre(string fibre)           { foreach (var e in WickReports)  if (string.Equals(e.WickFibreType,        fibre,   StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<CandleDipMouldAssay>       GetCandleAssaysByMethod(string method)        { foreach (var e in CandleAssays) if (string.Equals(e.CandleMethod,          method,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<CandleDipMouldAssay>       GetLongBurningCandles(float minHours)         { foreach (var e in CandleAssays) if (e.BurnDurationHours >= minHours)                                                     yield return e; }
    }
}
