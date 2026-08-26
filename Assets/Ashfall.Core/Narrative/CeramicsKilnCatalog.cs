using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    // ── Vector-Block Samekh — Subterranean Ceramics & Kiln Work ─────────────────

    public sealed class ClayWedgingFormingLog
    {
        [JsonPropertyName("id")]                  public string Id                { get; init; } = string.Empty;
        [JsonPropertyName("clay_bed_source")]      public string ClayBedSource     { get; init; } = string.Empty;
        [JsonPropertyName("wedging_cycle_count")]  public int    WedgingCycleCount { get; init; }
        [JsonPropertyName("forming_method")]       public string FormingMethod     { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]             public string LogText           { get; init; } = string.Empty;
    }

    public sealed class BisqueFiringRecord
    {
        [JsonPropertyName("id")]                    public string Id                  { get; init; } = string.Empty;
        [JsonPropertyName("kiln_chamber_id")]       public string KilnChamberId       { get; init; } = string.Empty;
        [JsonPropertyName("peak_temp_celsius")]     public float  PeakTempCelsius     { get; init; }
        [JsonPropertyName("firing_duration_hours")] public float  FiringDurationHours { get; init; }
        [JsonPropertyName("log_text")]              public string LogText              { get; init; } = string.Empty;
    }

    public sealed class SlipGlazeFormulationNote
    {
        [JsonPropertyName("id")]               public string Id             { get; init; } = string.Empty;
        [JsonPropertyName("base_clay_type")]   public string BaseClayType   { get; init; } = string.Empty;
        [JsonPropertyName("flux_material")]    public string FluxMaterial    { get; init; } = string.Empty;
        [JsonPropertyName("colorant_source")]  public string ColorantSource  { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]         public string LogText         { get; init; } = string.Empty;
    }

    public sealed class KilnDrawTrialAssay
    {
        [JsonPropertyName("id")]                      public string Id                  { get; init; } = string.Empty;
        [JsonPropertyName("kiln_chamber_id")]         public string KilnChamberId       { get; init; } = string.Empty;
        [JsonPropertyName("draw_trial_piece_type")]   public string DrawTrialPieceType  { get; init; } = string.Empty;
        [JsonPropertyName("surface_result")]          public string SurfaceResult        { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]                public string LogText              { get; init; } = string.Empty;
    }

    /// <summary>
    /// Loads and queries all four Vector-Block Samekh narrative datasets:
    /// clay wedging/forming logs, bisque firing records,
    /// slip/glaze formulation notes, and kiln draw trial assays.
    /// </summary>
    public sealed class CeramicsKilnCatalog
    {
        public IReadOnlyList<ClayWedgingFormingLog>    WedgingLogs    { get; }
        public IReadOnlyList<BisqueFiringRecord>       FiringRecords  { get; }
        public IReadOnlyList<SlipGlazeFormulationNote> GlazeNotes     { get; }
        public IReadOnlyList<KilnDrawTrialAssay>       DrawTrials     { get; }

        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        private CeramicsKilnCatalog(
            IReadOnlyList<ClayWedgingFormingLog>    wedgingLogs,
            IReadOnlyList<BisqueFiringRecord>       firingRecords,
            IReadOnlyList<SlipGlazeFormulationNote> glazeNotes,
            IReadOnlyList<KilnDrawTrialAssay>       drawTrials)
        {
            WedgingLogs   = wedgingLogs;
            FiringRecords = firingRecords;
            GlazeNotes    = glazeNotes;
            DrawTrials    = drawTrials;
        }

        public static CeramicsKilnCatalog LoadFromDirectory(string directoryPath) =>
            new(
                Load<ClayWedgingFormingLog>   (directoryPath, "clay_wedging_forming_logs.json"),
                Load<BisqueFiringRecord>      (directoryPath, "bisque_firing_records.json"),
                Load<SlipGlazeFormulationNote>(directoryPath, "slip_glaze_formulation_notes.json"),
                Load<KilnDrawTrialAssay>      (directoryPath, "kiln_draw_trial_assays.json")
            );

        private static IReadOnlyList<T> Load<T>(string dir, string file)
        {
            var json = File.ReadAllText(Path.Combine(dir, file));
            return CatalogLocator.LoadWrappedList<T>(json, _opts);
        }

        public IEnumerable<ClayWedgingFormingLog>    GetWedgingLogsByFormingMethod(string method)         { foreach (var e in WedgingLogs)   if (string.Equals(e.FormingMethod,  method,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<BisqueFiringRecord>       GetFiringRecordsByKiln(string kilnId)                { foreach (var e in FiringRecords) if (string.Equals(e.KilnChamberId, kilnId,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<SlipGlazeFormulationNote> GetGlazeNotesByFlux(string flux)                     { foreach (var e in GlazeNotes)    if (string.Equals(e.FluxMaterial,  flux,    StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<KilnDrawTrialAssay>       GetDrawTrialsByKiln(string kilnId)                   { foreach (var e in DrawTrials)    if (string.Equals(e.KilnChamberId, kilnId,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<BisqueFiringRecord>       GetHighTemperatureFirings(float minTempCelsius)       { foreach (var e in FiringRecords) if (e.PeakTempCelsius >= minTempCelsius)                                         yield return e; }
    }
}
