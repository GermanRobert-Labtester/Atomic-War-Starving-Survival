// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Sole loader authority for radio_stations.json.
    /// Invariant: Pure C#, zero engine dependencies.
    /// Strict validation: duplicate IDs, invalid frequency, schedule bounds, missing file.
    /// </summary>
    public static class RadioStationCatalogLoader
    {
        public const string StationsFileName = "radio_stations.json";
        public const float MinFrequencyMhz = 0.1f;
        public const float MaxFrequencyMhz = 1000.0f;

        public static int LoadAndRegister(
            RadioStationCatalog catalog,
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? serializer = null)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            if (string.IsNullOrWhiteSpace(dataDir))
                throw new ArgumentException("Data directory must be non-empty", nameof(dataDir));

            string path = Path.Combine(dataDir, StationsFileName);
            fileIO ??= new FileSystemIO();
            if (!fileIO.FileExists(path))
            {
                throw new FileNotFoundException($"Authoritative radio stations catalog not found: {path}");
            }

            string json = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidDataException($"Authoritative radio stations file is empty: {path}");
            }

            return LoadFromJsonString(catalog, json, path);
        }

        public static int LoadFromJsonString(RadioStationCatalog catalog, string json, string sourcePath = "<json>")
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            if (string.IsNullOrWhiteSpace(json)) return 0;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("schema_version", out var schemaProp) || schemaProp.GetInt32() < 1)
            {
                throw new InvalidDataException($"Missing or invalid schema_version in {sourcePath}");
            }

            JsonElement arr;
            if (root.TryGetProperty("stations", out arr) && arr.ValueKind == JsonValueKind.Array)
            {
                // Supported
            }
            else if (root.TryGetProperty("radio_stations", out arr) && arr.ValueKind == JsonValueKind.Array)
            {
                // Supported
            }
            else
            {
                throw new InvalidDataException($"No 'stations' or 'radio_stations' array found in {sourcePath}");
            }

            var registeredIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int count = 0;

            foreach (var elem in arr.EnumerateArray())
            {
                var def = JsonSerializer.Deserialize<RadioStationDefinition>(elem.GetRawText(), SystemTextJsonSerializer.Options);
                if (def == null) continue;

                if (string.IsNullOrWhiteSpace(def.StationId))
                {
                    throw new InvalidDataException($"Radio station missing station_id in {sourcePath}");
                }

                if (registeredIds.Contains(def.StationId))
                {
                    throw new InvalidDataException($"Duplicate radio station ID detected: '{def.StationId}' in {sourcePath}");
                }

                if (def.FrequencyMhz < MinFrequencyMhz || def.FrequencyMhz > MaxFrequencyMhz)
                {
                    throw new InvalidDataException($"Radio station '{def.StationId}' has invalid frequency {def.FrequencyMhz} MHz (allowed: {MinFrequencyMhz}..{MaxFrequencyMhz}) in {sourcePath}");
                }

                if (string.IsNullOrWhiteSpace(def.OwnerFactionId))
                {
                    throw new InvalidDataException($"Radio station '{def.StationId}' missing owner_faction_id in {sourcePath}");
                }

                // Schedule sanity check
                if (def.Schedule != null)
                {
                    foreach (var slot in def.Schedule)
                    {
                        if (slot.StartHour < 0 || slot.StartHour > 23 || slot.EndHour < 0 || slot.EndHour > 23)
                        {
                            throw new InvalidDataException($"Station '{def.StationId}' slot '{slot.SlotId}' has invalid hours ({slot.StartHour}..{slot.EndHour}) in {sourcePath}");
                        }
                    }
                }

                catalog.Register(def);
                registeredIds.Add(def.StationId);
                count++;
            }

            return count;
        }
    }
}
