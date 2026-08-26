using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    // ── Vector-Block Nun — Subterranean Tanning & Leatherwork ────────────────────

    public sealed class BarkTanningVatLog
    {
        [JsonPropertyName("id")]               public string Id               { get; init; } = string.Empty;
        [JsonPropertyName("tanning_vat_id")]   public string TanningVatId     { get; init; } = string.Empty;
        [JsonPropertyName("bark_species")]     public string BarkSpecies      { get; init; } = string.Empty;
        [JsonPropertyName("liquor_strength_baume")] public float LiquorStrengthBaume { get; init; }
        [JsonPropertyName("log_text")]         public string LogText          { get; init; } = string.Empty;
    }

    public sealed class BrainTanningHideReport
    {
        [JsonPropertyName("id")]                      public string Id                   { get; init; } = string.Empty;
        [JsonPropertyName("hide_source_animal")]      public string HideSourceAnimal     { get; init; } = string.Empty;
        [JsonPropertyName("brain_emulsion_batch_id")] public string BrainEmulsionBatchId { get; init; } = string.Empty;
        [JsonPropertyName("smoke_cycle_count")]       public int    SmokeCycleCount      { get; init; }
        [JsonPropertyName("log_text")]                public string LogText              { get; init; } = string.Empty;
    }

    public sealed class CurryingBurnishingAssay
    {
        [JsonPropertyName("id")]                 public string Id               { get; init; } = string.Empty;
        [JsonPropertyName("tanned_hide_lot_id")] public string TannedHideLotId { get; init; } = string.Empty;
        [JsonPropertyName("fat_liquor_type")]    public string FatLiquorType    { get; init; } = string.Empty;
        [JsonPropertyName("burnishing_tool")]    public string BurnishingTool   { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]           public string LogText          { get; init; } = string.Empty;
    }

    public sealed class AwlStitchJournal
    {
        [JsonPropertyName("id")]                 public string Id              { get; init; } = string.Empty;
        [JsonPropertyName("leather_panel_id")]   public string LeatherPanelId  { get; init; } = string.Empty;
        [JsonPropertyName("thread_material")]    public string ThreadMaterial   { get; init; } = string.Empty;
        [JsonPropertyName("stitch_length_mm")]   public float  StitchLengthMm  { get; init; }
        [JsonPropertyName("log_text")]           public string LogText          { get; init; } = string.Empty;
    }

    /// <summary>
    /// Loads and queries all four Vector-Block Nun narrative datasets:
    /// bark tanning vat logs, brain tanning hide reports,
    /// currying/burnishing assays, and awl saddle-stitch journals.
    /// </summary>
    public sealed class TanningLeatherworkCatalog
    {
        public IReadOnlyList<BarkTanningVatLog>      VatLogs        { get; }
        public IReadOnlyList<BrainTanningHideReport> HideReports    { get; }
        public IReadOnlyList<CurryingBurnishingAssay> CurryingAssays { get; }
        public IReadOnlyList<AwlStitchJournal>       StitchJournals { get; }

        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        private TanningLeatherworkCatalog(
            IReadOnlyList<BarkTanningVatLog>      vatLogs,
            IReadOnlyList<BrainTanningHideReport> hideReports,
            IReadOnlyList<CurryingBurnishingAssay> curryingAssays,
            IReadOnlyList<AwlStitchJournal>       stitchJournals)
        {
            VatLogs        = vatLogs;
            HideReports    = hideReports;
            CurryingAssays = curryingAssays;
            StitchJournals = stitchJournals;
        }

        public static TanningLeatherworkCatalog LoadFromDirectory(string directoryPath) =>
            new(
                Load<BarkTanningVatLog>      (directoryPath, "bark_tanning_vat_logs.json"),
                Load<BrainTanningHideReport> (directoryPath, "brain_tanning_hide_reports.json"),
                Load<CurryingBurnishingAssay>(directoryPath, "currying_burnishing_assays.json"),
                Load<AwlStitchJournal>       (directoryPath, "awl_saddle_stitch_journals.json")
            );

        private static IReadOnlyList<T> Load<T>(string dir, string file)
        {
            var json = File.ReadAllText(Path.Combine(dir, file));
            return CatalogLocator.LoadWrappedList<T>(json, _opts);
        }

        public IEnumerable<BarkTanningVatLog>      GetVatLogsByBarkSpecies(string species)   { foreach (var e in VatLogs)        if (string.Equals(e.BarkSpecies,   species,     StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<BrainTanningHideReport> GetBrainTanReportsByAnimal(string animal) { foreach (var e in HideReports)    if (string.Equals(e.HideSourceAnimal, animal,   StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<CurryingBurnishingAssay> GetCurryingAssaysByFatLiquor(string fat) { foreach (var e in CurryingAssays) if (string.Equals(e.FatLiquorType,  fat,         StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<AwlStitchJournal>       GetStitchJournalsByThread(string thread)  { foreach (var e in StitchJournals) if (string.Equals(e.ThreadMaterial, thread,      StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<BrainTanningHideReport> GetSmokeCycleReports(int minCycles)        { foreach (var e in HideReports)    if (e.SmokeCycleCount >= minCycles)                                                  yield return e; }
    }
}
