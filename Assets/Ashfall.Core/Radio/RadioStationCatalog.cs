// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Authoritative catalog of the 6 canonical radio stations in ASHFALL.
    /// Manages station identities, default frequencies, persona descriptions,
    /// and dynamic station state transitions (Normal, Degraded, Jammed, Silent).
    /// Loaded from authoritative radio_stations.json in StreamingAssets/Data.
    /// </summary>
    public sealed class RadioStationCatalog
    {
        public const string StationCivilDefense = "station_civil_defense";
        public const string StationGarrisonOverlord = "station_garrison_overlord";
        public const string StationVitrifiedCrater = "station_vitrified_crater";
        public const string StationOpenClassroom = "station_open_classroom";
        public const string StationNumbersSigint = "station_numbers_sigint";
        public const string StationAutomatedRelay = "station_automated_relay";

        private readonly Dictionary<string, RadioStationDefinition> _stations =
            new Dictionary<string, RadioStationDefinition>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, RadioStationState> _stateOverrides =
            new Dictionary<string, RadioStationState>(StringComparer.OrdinalIgnoreCase);

        public RadioStationCatalog()
        {
        }

        public IReadOnlyCollection<RadioStationDefinition> AllStations => _stations.Values;

        public void Clear()
        {
            _stations.Clear();
            _stateOverrides.Clear();
        }

        public int LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return 0;
            try
            {
                return RadioStationCatalogLoader.LoadFromJsonString(this, json);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<json>", "RadioStationCatalog", ex_CATDIAG);
                return 0;
            }
        }

        public int LoadFromDataDirectory(string dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) return 0;
            string path = Path.Combine(dataDir, RadioStationCatalogLoader.StationsFileName);
            if (!File.Exists(path)) return 0;
            try
            {
                return RadioStationCatalogLoader.LoadAndRegister(this, dataDir);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "RadioStationCatalog", ex_CATDIAG);
                return 0;
            }
        }

        public void Register(RadioStationDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.StationId)) return;
            _stations[def.StationId] = def;
        }

        public RadioStationDefinition? GetStation(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return null;
            return _stations.TryGetValue(stationId, out var def) ? def : null;
        }

        public RadioStationDefinition? FindStationAtFrequency(float frequencyMhz, float toleranceMhz = 0.5f)
        {
            RadioStationDefinition? best = null;
            float minDiff = float.MaxValue;
            foreach (var s in _stations.Values)
            {
                float diff = Math.Abs(s.FrequencyMhz - frequencyMhz);
                if (diff <= toleranceMhz && diff < minDiff)
                {
                    minDiff = diff;
                    best = s;
                }
            }
            return best;
        }

        public RadioStationState GetStationState(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return RadioStationState.Normal;
            if (_stateOverrides.TryGetValue(stationId, out var ov))
                return ov;
            if (_stations.TryGetValue(stationId, out var def))
                return def.DefaultState;
            return RadioStationState.Normal;
        }

        public void SetStationState(string stationId, RadioStationState state)
        {
            if (string.IsNullOrEmpty(stationId)) return;
            _stateOverrides[stationId] = state;
        }

        public void ResetOverrides()
        {
            _stateOverrides.Clear();
        }

        public Dictionary<string, RadioStationState> ExportOverrides()
        {
            return new Dictionary<string, RadioStationState>(_stateOverrides, StringComparer.OrdinalIgnoreCase);
        }

        public void ImportOverrides(IDictionary<string, RadioStationState>? overrides)
        {
            _stateOverrides.Clear();
            if (overrides == null) return;
            foreach (var kvp in overrides)
            {
                if (!string.IsNullOrEmpty(kvp.Key))
                    _stateOverrides[kvp.Key] = kvp.Value;
            }
        }

        public RadioProgramSlot? GetCurrentSlot(string stationId, int campaignDay, int hour)
        {
            var station = GetStation(stationId);
            return station?.GetCurrentSlot(campaignDay, hour);
        }

        public RadioProgramSlot? GetNextSlot(string stationId, int campaignDay, int hour)
        {
            var station = GetStation(stationId);
            return station?.GetNextSlot(campaignDay, hour);
        }

        public RadioSignalStrength ComputeSignalStrength(string stationId, RadioReceptionFactors? factors)
        {
            var station = GetStation(stationId);
            float baseStrength = station != null ? 0.9f : 0.2f;
            if (station != null && station.FrequencyMhz < 30.0f)
            {
                // Shortwave base attenuation
                baseStrength = 0.85f;
            }
            return RadioSignalStrength.Evaluate(baseStrength, factors);
        }
    }
}
