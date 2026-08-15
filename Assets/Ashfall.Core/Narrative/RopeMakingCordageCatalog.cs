using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    // ── Vector-Block Ayin — Subterranean Rope Making & Cordage ──────────────────

    public sealed class FibreHecklingPrepLog
    {
        [JsonPropertyName("id")]                 public string Id              { get; init; } = string.Empty;
        [JsonPropertyName("fibre_source_plant")] public string FibreSourcePlant { get; init; } = string.Empty;
        [JsonPropertyName("retting_days")]       public int    RettingDays     { get; init; }
        [JsonPropertyName("heckling_comb_id")]   public string HecklingCombId  { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]           public string LogText          { get; init; } = string.Empty;
    }

    public sealed class StrandTwistingLayReport
    {
        [JsonPropertyName("id")]                    public string Id                { get; init; } = string.Empty;
        [JsonPropertyName("fibre_type")]            public string FibreType         { get; init; } = string.Empty;
        [JsonPropertyName("twist_direction")]       public string TwistDirection    { get; init; } = string.Empty;
        [JsonPropertyName("strand_count_per_yarn")] public int    StrandCountPerYarn { get; init; }
        [JsonPropertyName("log_text")]              public string LogText            { get; init; } = string.Empty;
    }

    public sealed class ThreeStrandRopeClosingLog
    {
        [JsonPropertyName("id")]               public string Id            { get; init; } = string.Empty;
        [JsonPropertyName("strand_yarn_id")]   public string StrandYarnId  { get; init; } = string.Empty;
        [JsonPropertyName("rope_diameter_mm")] public float  RopeDiameterMm { get; init; }
        [JsonPropertyName("closing_tool")]     public string ClosingTool   { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]         public string LogText        { get; init; } = string.Empty;
    }

    public sealed class RopeBreakLoadAssay
    {
        [JsonPropertyName("id")]            public string Id           { get; init; } = string.Empty;
        [JsonPropertyName("rope_lot_id")]   public string RopeLotId    { get; init; } = string.Empty;
        [JsonPropertyName("test_load_kg")]  public float  TestLoadKg   { get; init; }
        [JsonPropertyName("failure_mode")]  public string FailureMode  { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]      public string LogText      { get; init; } = string.Empty;
    }

    /// <summary>
    /// Loads and queries all four Vector-Block Ayin narrative datasets:
    /// fibre heckling prep logs, strand twisting reports,
    /// rope closing logs, and break load assays.
    /// </summary>
    public sealed class RopeMakingCordageCatalog
    {
        public IReadOnlyList<FibreHecklingPrepLog>    HecklingLogs   { get; }
        public IReadOnlyList<StrandTwistingLayReport> StrandReports  { get; }
        public IReadOnlyList<ThreeStrandRopeClosingLog> ClosingLogs  { get; }
        public IReadOnlyList<RopeBreakLoadAssay>      BreakAssays    { get; }

        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        private RopeMakingCordageCatalog(
            IReadOnlyList<FibreHecklingPrepLog>    hecklingLogs,
            IReadOnlyList<StrandTwistingLayReport> strandReports,
            IReadOnlyList<ThreeStrandRopeClosingLog> closingLogs,
            IReadOnlyList<RopeBreakLoadAssay>      breakAssays)
        {
            HecklingLogs  = hecklingLogs;
            StrandReports = strandReports;
            ClosingLogs   = closingLogs;
            BreakAssays   = breakAssays;
        }

        public static RopeMakingCordageCatalog LoadFromDirectory(string directoryPath) =>
            new(
                Load<FibreHecklingPrepLog>    (directoryPath, "fibre_heckling_prep_logs.json"),
                Load<StrandTwistingLayReport> (directoryPath, "strand_twisting_lay_reports.json"),
                Load<ThreeStrandRopeClosingLog>(directoryPath, "three_strand_rope_closing_logs.json"),
                Load<RopeBreakLoadAssay>      (directoryPath, "rope_break_load_assays.json")
            );

        private static IReadOnlyList<T> Load<T>(string dir, string file)
        {
            var json = File.ReadAllText(Path.Combine(dir, file));
            return JsonSerializer.Deserialize<List<T>>(json, _opts)
                   ?? throw new InvalidOperationException($"Failed to deserialize {file}");
        }

        public IEnumerable<FibreHecklingPrepLog>    GetHecklingLogsByPlant(string plant)      { foreach (var e in HecklingLogs)  if (string.Equals(e.FibreSourcePlant, plant,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<StrandTwistingLayReport> GetStrandReportsByFibre(string fibre)     { foreach (var e in StrandReports) if (string.Equals(e.FibreType,         fibre,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<ThreeStrandRopeClosingLog> GetClosingLogsByTool(string tool)       { foreach (var e in ClosingLogs)   if (string.Equals(e.ClosingTool,        tool,   StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<RopeBreakLoadAssay>      GetBreakAssaysByFailureMode(string mode)  { foreach (var e in BreakAssays)   if (string.Equals(e.FailureMode,        mode,   StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<RopeBreakLoadAssay>      GetRopesAboveTestLoad(float minKg)        { foreach (var e in BreakAssays)   if (e.TestLoadKg >= minKg)                                                           yield return e; }
    }
}
