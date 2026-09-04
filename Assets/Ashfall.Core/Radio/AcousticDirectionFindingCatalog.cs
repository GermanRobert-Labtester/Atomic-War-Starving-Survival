// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// One listening-array profile for long-baseline acoustic early warning.
    /// Normalized sensor classes and balance values only — no propagation
    /// physics, frequency specifications, or targeting data.
    /// </summary>
    [Serializable]
    public sealed class AcousticArrayProfile
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string sensor_class = string.Empty;
        public string baseline_class = "medium"; // short | medium | long
        public float noise_tolerance; // 0..1 normalized
        public List<string> signal_bands = new List<string>();
        public float base_detection_range_km;
        public float confidence_gain; // per observation, normalized
        public string warning_window_class = "window_medium";
        public float power_demand_w;
        public float maintenance_wear;
        public List<string> install_item_ids = new List<string>();
        public List<string> repair_item_ids = new List<string>();
        public string dampening_item_id = string.Empty;
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id)) { error = "Array id cannot be empty."; return false; }
            if (noise_tolerance < 0f || noise_tolerance > 1f)
            { error = $"Array '{id}' noise_tolerance must be within [0, 1]."; return false; }
            if (signal_bands == null || signal_bands.Count == 0)
            { error = $"Array '{id}' must listen to at least one signal band."; return false; }
            if (base_detection_range_km < 0) { error = $"Array '{id}' range cannot be negative."; return false; }
            if (confidence_gain < 0) { error = $"Array '{id}' confidence gain cannot be negative."; return false; }
            if (power_demand_w < 0 || maintenance_wear < 0)
            { error = $"Array '{id}' power/wear cannot be negative."; return false; }
            if (install_item_ids == null || install_item_ids.Count == 0)
            { error = $"Array '{id}' must bill at least one install item."; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class AcousticSignalBandDef
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public bool is_hostile;
    }

    [Serializable]
    public sealed class AcousticWarningWindowDef
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public int tier = 1;
    }

    [Serializable]
    public sealed class AcousticDirectionFindingCatalogDto
    {
        public int schema_version = 1;
        public List<AcousticArrayProfile> arrays = new List<AcousticArrayProfile>();
        public List<AcousticSignalBandDef> signal_bands = new List<AcousticSignalBandDef>();
        public List<AcousticWarningWindowDef> warning_windows = new List<AcousticWarningWindowDef>();
    }

    public sealed class AcousticDirectionFindingCatalog
    {
        private readonly Dictionary<string, AcousticArrayProfile> _arrays = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, AcousticSignalBandDef> _bands = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, AcousticWarningWindowDef> _windows = new(StringComparer.OrdinalIgnoreCase);

        public AcousticDirectionFindingCatalog(
            IEnumerable<AcousticArrayProfile>? arrays,
            IEnumerable<AcousticSignalBandDef>? bands,
            IEnumerable<AcousticWarningWindowDef>? windows)
        {
            foreach (var a in arrays ?? Enumerable.Empty<AcousticArrayProfile>())
                if (a != null && !string.IsNullOrWhiteSpace(a.id)) _arrays[a.id] = a;
            foreach (var b in bands ?? Enumerable.Empty<AcousticSignalBandDef>())
                if (b != null && !string.IsNullOrWhiteSpace(b.id)) _bands[b.id] = b;
            foreach (var w in windows ?? Enumerable.Empty<AcousticWarningWindowDef>())
                if (w != null && !string.IsNullOrWhiteSpace(w.id)) _windows[w.id] = w;
        }

        public IReadOnlyDictionary<string, AcousticArrayProfile> Arrays => _arrays;
        public IReadOnlyDictionary<string, AcousticSignalBandDef> Bands => _bands;
        public IReadOnlyDictionary<string, AcousticWarningWindowDef> WarningWindows => _windows;

        public AcousticArrayProfile? GetArray(string arrayId) =>
            string.IsNullOrEmpty(arrayId) ? null : _arrays.TryGetValue(arrayId, out var a) ? a : null;

        public bool IsBandHostile(string bandId) =>
            !string.IsNullOrEmpty(bandId) && _bands.TryGetValue(bandId, out var b) && b.is_hostile;

        public AcousticWarningWindowDef? GetWindow(string windowId) =>
            string.IsNullOrEmpty(windowId) ? null : _windows.TryGetValue(windowId, out var w) ? w : null;
    }

    public static class AcousticDirectionFindingCatalogLoader
    {
        public const string DefaultFileName = "acoustic_triangulation_catalog.json";

        public static AcousticDirectionFindingCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null) return null;
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string raw = fileIO.ReadAllText(path);
            AcousticDirectionFindingCatalogDto? dto;
            try
            {
                dto = json.Deserialize<AcousticDirectionFindingCatalogDto>(raw);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "acoustic_triangulation_catalog", ex);
                return null;
            }
            if (dto?.arrays == null) return null;

            return new AcousticDirectionFindingCatalog(dto.arrays, dto.signal_bands, dto.warning_windows);
        }
    }
}
