// SPDX-License-Identifier: MIT
// ============================================================================
// Catalog    : ShelterPowerGridCatalog (+Loader)
// Data       : Assets/StreamingAssets/Data/power_grid.json
// System     : PowerGridSystem (shelter electrical authority)
// Purpose    : Runtime parsing of the authoritative power-grid room catalog.
//              Replaces the host's hardcoded room snapshot (Plan G2 of the
//              SHELTER_GRID_CATALOG_SEAL wave) so catalog edits reach the
//              simulation. A missing or unusable file falls back to embedded
//              defaults so the host never fails to boot over a catalog.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    /// <summary>One authored power-grid room definition (snake_case JSON contract).</summary>
    [Serializable]
    public sealed class ShelterPowerGridRoomDef
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("display_name")] public string DisplayName { get; set; } = string.Empty;
        [JsonPropertyName("draw_watts")] public float DrawWatts { get; set; }
        [JsonPropertyName("default_priority")] public string DefaultPriority { get; set; } = "standard";
        [JsonPropertyName("failure_effect_id")] public string FailureEffectId { get; set; } = string.Empty;
    }

    /// <summary>Root document for power_grid.json.</summary>
    [Serializable]
    public sealed class ShelterPowerGridCatalogDef
    {
        [JsonPropertyName("schema_version")] public int SchemaVersion { get; set; }
        [JsonPropertyName("generation_watts_default")] public float GenerationWattsDefault { get; set; }
        [JsonPropertyName("battery_capacity_wh_default")] public float BatteryCapacityWhDefault { get; set; }
        [JsonPropertyName("fuel_units_default")] public float FuelUnitsDefault { get; set; }
        [JsonPropertyName("rooms")] public List<ShelterPowerGridRoomDef> Rooms { get; set; } = new List<ShelterPowerGridRoomDef>();
    }

    /// <summary>
    /// Loads the authoritative shelter power-grid catalog. Behavior policy:
    /// missing file → embedded fallback (boot must never fail over a catalog);
    /// malformed content → strict error via <see cref="TryLoad"/> for tests and
    /// fallback-plus-diagnostic via <see cref="LoadOrDefault"/> for the host.
    /// </summary>
    public static class ShelterPowerGridCatalogLoader
    {
        public const string FileName = "power_grid.json";
        public const int SupportedSchemaVersion = 1;

        /// <summary>Parse + validate strictly. Returns false with a specific error message.</summary>
        public static bool TryLoad(string dataDir, IFileIO files, IJsonSerializer serializer,
            out ShelterPowerGridCatalogDef? catalog, out string error)
        {
            catalog = null;
            error = string.Empty;
            if (files == null) { error = $"{FileName}: no file IO adapter."; return false; }
            if (serializer == null) { error = $"{FileName}: no JSON serializer adapter."; return false; }
            if (string.IsNullOrWhiteSpace(dataDir)) { error = $"{FileName}: no data directory."; return false; }

            string path = files.Combine(dataDir, FileName);
            if (!files.FileExists(path)) { error = $"{FileName}: file not found at '{path}'."; return false; }

            string text;
            try { text = files.ReadAllText(path); }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "FileRead", ex);
                error = $"{FileName}: read failed ({ex.Message}).";
                return false;
            }
            if (string.IsNullOrWhiteSpace(text)) { error = $"{FileName}: file is empty."; return false; }

            try
            {
                catalog = JsonSerializer.Deserialize<ShelterPowerGridCatalogDef>(text, SystemTextJsonSerializer.Options);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "ShelterPowerGridCatalogDef", ex);
                error = $"{FileName}: malformed JSON ({ex.Message}).";
                catalog = null;
                return false;
            }
            if (catalog == null) { error = $"{FileName}: deserialized to null."; return false; }
            return Validate(catalog, out error);
        }

        /// <summary>
        /// Host-facing load: strict parse, falling back to embedded defaults with a
        /// diagnostic when the file is missing or unusable.
        /// </summary>
        public static ShelterPowerGridCatalogDef LoadOrDefault(string dataDir, IFileIO files, IJsonSerializer serializer)
        {
            if (TryLoad(dataDir, files, serializer, out var catalog, out var error))
                return catalog!;
            if (!string.IsNullOrEmpty(error) && !error.Contains("file not found", StringComparison.Ordinal))
                CatalogDiagnostics.Warn(FileName, "ShelterPowerGridCatalogDef", new InvalidOperationException(error));
            return FallbackDefault();
        }

        /// <summary>Structural validation: schema version, unique IDs, sane numerics, known priorities.</summary>
        public static bool Validate(ShelterPowerGridCatalogDef catalog, out string error)
        {
            error = string.Empty;
            if (catalog == null) { error = $"{FileName}: catalog is null."; return false; }
            if (catalog.SchemaVersion != SupportedSchemaVersion)
            { error = $"{FileName}: unsupported schema_version {catalog.SchemaVersion} (expected {SupportedSchemaVersion})."; return false; }
            if (catalog.GenerationWattsDefault < 0f || catalog.BatteryCapacityWhDefault < 0f || catalog.FuelUnitsDefault < 0f)
            { error = $"{FileName}: negative default generation/battery/fuel values."; return false; }
            if (catalog.Rooms == null || catalog.Rooms.Count == 0)
            { error = $"{FileName}: no rooms defined."; return false; }

            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var room in catalog.Rooms)
            {
                if (room == null || string.IsNullOrWhiteSpace(room.Id))
                { error = $"{FileName}: room with empty id."; return false; }
                if (!ids.Add(room.Id))
                { error = $"{FileName}: duplicate room id '{room.Id}'."; return false; }
                if (room.DrawWatts < 0f)
                { error = $"{FileName}: room '{room.Id}' has negative draw_watts."; return false; }
                if (string.IsNullOrWhiteSpace(room.DisplayName))
                { error = $"{FileName}: room '{room.Id}' has empty display_name."; return false; }
                if (MapPriority(room.Id, room.DefaultPriority) == null)
                { error = $"{FileName}: room '{room.Id}' has unknown default_priority '{room.DefaultPriority}'."; return false; }
            }
            return true;
        }

        /// <summary>Map a catalog priority string to the simulation enum; null when unknown.</summary>
        public static PowerGridRoomPriority? MapPriority(string roomId, string priority)
        {
            switch (priority?.Trim().ToLowerInvariant())
            {
                case "critical": return PowerGridRoomPriority.Critical;
                case "standard": return PowerGridRoomPriority.Standard;
                case "low": return PowerGridRoomPriority.Low;
                case "disabled": return PowerGridRoomPriority.Disabled;
                default:
                    CatalogDiagnostics.Warn(FileName, $"default_priority:{roomId}",
                        new ArgumentException($"Unknown priority '{priority}'."));
                    return null;
            }
        }

        /// <summary>
        /// Embedded fallback matching the previously hardcoded host snapshot plus the
        /// canonical rooms it was missing. Used only when the catalog file is absent.
        /// </summary>
        public static ShelterPowerGridCatalogDef FallbackDefault() => new ShelterPowerGridCatalogDef
        {
            SchemaVersion = SupportedSchemaVersion,
            GenerationWattsDefault = 800f,
            BatteryCapacityWhDefault = 4000f,
            FuelUnitsDefault = 100f,
            Rooms = new List<ShelterPowerGridRoomDef>
            {
                new ShelterPowerGridRoomDef { Id = "room_air_filtration", DisplayName = "Air Filtration", DrawWatts = 180f, DefaultPriority = "critical", FailureEffectId = "fx_filtration_off" },
                new ShelterPowerGridRoomDef { Id = "room_clinic", DisplayName = "Clinic", DrawWatts = 120f, DefaultPriority = "critical", FailureEffectId = "fx_clinic_off" },
                new ShelterPowerGridRoomDef { Id = "room_water_pump", DisplayName = "Water Pump", DrawWatts = 100f, DefaultPriority = "critical", FailureEffectId = "fx_water_pressure_drop" },
                new ShelterPowerGridRoomDef { Id = "room_greenhouse", DisplayName = "Greenhouse", DrawWatts = 160f, DefaultPriority = "standard", FailureEffectId = "fx_grow_lights_off" },
                new ShelterPowerGridRoomDef { Id = "room_foundry", DisplayName = "Silent Foundry", DrawWatts = 220f, DefaultPriority = "low", FailureEffectId = "fx_foundry_standstill" },
                new ShelterPowerGridRoomDef { Id = "room_lighting_main", DisplayName = "Main Lighting", DrawWatts = 80f, DefaultPriority = "low", FailureEffectId = "fx_lighting_dim" },
                new ShelterPowerGridRoomDef { Id = "room_workshop", DisplayName = "Workshop", DrawWatts = 300f, DefaultPriority = "low", FailureEffectId = "fx_workshop_offline" }
            }
        };
    }
}
