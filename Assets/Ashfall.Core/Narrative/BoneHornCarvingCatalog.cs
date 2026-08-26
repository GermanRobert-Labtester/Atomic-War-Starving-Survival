using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    // ── Vector-Block Tsadi — Subterranean Bone & Horn Carving ───────────────────

    public sealed class BoneDegreasingPrepLog
    {
        [JsonPropertyName("id")]                   public string Id                { get; init; } = string.Empty;
        [JsonPropertyName("bone_source_animal")]   public string BoneSourceAnimal  { get; init; } = string.Empty;
        [JsonPropertyName("degreasing_method")]    public string DegreasingMethod  { get; init; } = string.Empty;
        [JsonPropertyName("prep_duration_days")]   public int    PrepDurationDays  { get; init; }
        [JsonPropertyName("log_text")]             public string LogText            { get; init; } = string.Empty;
    }

    public sealed class AntlerHornSawingRecord
    {
        [JsonPropertyName("id")]               public string Id             { get; init; } = string.Empty;
        [JsonPropertyName("material_type")]    public string MaterialType   { get; init; } = string.Empty;
        [JsonPropertyName("saw_tool_id")]      public string SawToolId      { get; init; } = string.Empty;
        [JsonPropertyName("blank_shape_cut")]  public string BlankShapeCut  { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]         public string LogText         { get; init; } = string.Empty;
    }

    public sealed class ScrapingPolishingReport
    {
        [JsonPropertyName("id")]               public string Id             { get; init; } = string.Empty;
        [JsonPropertyName("blank_material")]   public string BlankMaterial  { get; init; } = string.Empty;
        [JsonPropertyName("abrasive_used")]    public string AbrasiveUsed   { get; init; } = string.Empty;
        [JsonPropertyName("surface_finish")]   public string SurfaceFinish  { get; init; } = string.Empty;
        [JsonPropertyName("log_text")]         public string LogText         { get; init; } = string.Empty;
    }

    public sealed class NeedleAwlHookAssay
    {
        [JsonPropertyName("id")]                    public string Id                { get; init; } = string.Empty;
        [JsonPropertyName("tool_type")]             public string ToolType          { get; init; } = string.Empty;
        [JsonPropertyName("bone_blank_id")]         public string BoneBlankId       { get; init; } = string.Empty;
        [JsonPropertyName("point_angle_degrees")]   public float  PointAngleDegrees { get; init; }
        [JsonPropertyName("log_text")]              public string LogText            { get; init; } = string.Empty;
    }

    /// <summary>
    /// Loads and queries all four Vector-Block Tsadi narrative datasets:
    /// bone degreasing/prep logs, antler/horn sawing records,
    /// scraping/polishing reports, and needle/awl/hook assays.
    /// </summary>
    public sealed class BoneHornCarvingCatalog
    {
        public IReadOnlyList<BoneDegreasingPrepLog>  DegreasingLogs  { get; }
        public IReadOnlyList<AntlerHornSawingRecord> SawingRecords   { get; }
        public IReadOnlyList<ScrapingPolishingReport> PolishingReports { get; }
        public IReadOnlyList<NeedleAwlHookAssay>     ToolAssays      { get; }

        private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        private BoneHornCarvingCatalog(
            IReadOnlyList<BoneDegreasingPrepLog>  degreasingLogs,
            IReadOnlyList<AntlerHornSawingRecord> sawingRecords,
            IReadOnlyList<ScrapingPolishingReport> polishingReports,
            IReadOnlyList<NeedleAwlHookAssay>     toolAssays)
        {
            DegreasingLogs   = degreasingLogs;
            SawingRecords    = sawingRecords;
            PolishingReports = polishingReports;
            ToolAssays       = toolAssays;
        }

        public static BoneHornCarvingCatalog LoadFromDirectory(string directoryPath) =>
            new(
                Load<BoneDegreasingPrepLog>  (directoryPath, "bone_degreasing_prep_logs.json"),
                Load<AntlerHornSawingRecord> (directoryPath, "antler_horn_sawing_records.json"),
                Load<ScrapingPolishingReport>(directoryPath, "scraping_polishing_reports.json"),
                Load<NeedleAwlHookAssay>     (directoryPath, "needle_awl_hook_assays.json")
            );

        private static IReadOnlyList<T> Load<T>(string dir, string file)
        {
            var json = File.ReadAllText(Path.Combine(dir, file));
            return CatalogLocator.LoadWrappedList<T>(json, _opts);
        }

        public IEnumerable<BoneDegreasingPrepLog>  GetDegreasingLogsByAnimal(string animal)    { foreach (var e in DegreasingLogs)   if (string.Equals(e.BoneSourceAnimal, animal,    StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<AntlerHornSawingRecord> GetSawingRecordsByMaterial(string material) { foreach (var e in SawingRecords)    if (string.Equals(e.MaterialType,     material,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<ScrapingPolishingReport> GetPolishingReportsByAbrasive(string abr)  { foreach (var e in PolishingReports) if (string.Equals(e.AbrasiveUsed,    abr,        StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<NeedleAwlHookAssay>     GetToolAssaysByType(string toolType)        { foreach (var e in ToolAssays)       if (string.Equals(e.ToolType,         toolType,  StringComparison.OrdinalIgnoreCase)) yield return e; }
        public IEnumerable<NeedleAwlHookAssay>     GetSharpToolAssays(float maxPointAngle)     { foreach (var e in ToolAssays)       if (e.PointAngleDegrees <= maxPointAngle)                                             yield return e; }
    }
}
