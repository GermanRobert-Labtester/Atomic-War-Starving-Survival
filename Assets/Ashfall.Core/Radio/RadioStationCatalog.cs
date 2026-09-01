// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Authoritative catalog of the 6 canonical radio stations in ASHFALL.
    /// Manages station identities, default frequencies, persona descriptions,
    /// and dynamic station state transitions (Normal, Degraded, Jammed, Silent).
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
            RegisterDefaults();
        }

        public IReadOnlyCollection<RadioStationDefinition> AllStations => _stations.Values;

        public void RegisterDefaults()
        {
            // 1. Civil Defense & Public Service
            Register(new RadioStationDefinition
            {
                StationId = StationCivilDefense,
                DisplayName = "Central Civil Defense Radio",
                FrequencyMhz = 88.50f,
                OwnerFactionId = "faction_civil_defense",
                PersonaVoice = "Formal, measured mid-century emergency announcer with periodic half-hour chimes.",
                Reliability = SourceReliability.Official,
                DefaultState = RadioStationState.Normal,
                SilenceText = "STATIC... [ Civil Defense carrier lost. Emergency tone unmonitored. ] ...STATIC",
                JammedText = "STATIC... [ Heavy ionospheric flutter overriding Civil Defense frequency. ]"
            });

            // 2. Iron Garrison Command / Overlord Actual
            Register(new RadioStationDefinition
            {
                StationId = StationGarrisonOverlord,
                DisplayName = "Iron Garrison / Overlord Actual",
                FrequencyMhz = 88.40f,
                OwnerFactionId = "military_remnants",
                PersonaVoice = "Barked tactical military orders, diesel generator hum, squelch tail clicks.",
                Reliability = SourceReliability.Partisan,
                DefaultState = RadioStationState.Normal,
                SilenceText = "STATIC... [ Garrison tactical frequency silent. No carrier active. ] ...STATIC",
                JammedText = "STATIC... [ Tactical frequency jammed with pulsed tactical noise. ]"
            });

            // 3. Voice of the Vitrified Crater / Ash Pulpit
            Register(new RadioStationDefinition
            {
                StationId = StationVitrifiedCrater,
                DisplayName = "Voice of the Vitrified Crater",
                FrequencyMhz = 104.20f,
                OwnerFactionId = "children_of_the_crater",
                PersonaVoice = "Deep resonant liturgical chanting recorded in sealed subterranean chambers.",
                Reliability = SourceReliability.Partisan,
                DefaultState = RadioStationState.Normal,
                SilenceText = "STATIC... [ The sermon has ended. Pure analog vacuum hiss fills the band. ]",
                JammedText = "STATIC... [ Static screams with electrical discharge from the fallout cloud. ]"
            });

            // 4. The Open Airwaves / Civilian Free Network
            Register(new RadioStationDefinition
            {
                StationId = StationOpenClassroom,
                DisplayName = "The Open Airwaves (Classroom & Lineman)",
                FrequencyMhz = 91.30f,
                OwnerFactionId = "faction_independent_survivors",
                PersonaVoice = "Warm, patient human voices; chalk taps, tool rattles, wind in antenna masts.",
                Reliability = SourceReliability.Anonymous,
                DefaultState = RadioStationState.Normal,
                SilenceText = "STATIC... [ The classroom is empty. Antenna line swaying in the ash wind. ]",
                JammedText = "STATIC... [ Weak pirate signal buried in thermal receiver noise. ]"
            });

            // 5. Unidentified Numbers Stations & Sigint
            Register(new RadioStationDefinition
            {
                StationId = StationNumbersSigint,
                DisplayName = "Clandestine Numbers Array",
                FrequencyMhz = 14.487f,
                OwnerFactionId = "faction_unknown_intelligence",
                PersonaVoice = "Synthetic female voice reading 5-figure phonetic number groups, music box chimes.",
                Reliability = SourceReliability.Automated,
                DefaultState = RadioStationState.Normal,
                SilenceText = "STATIC... [ Clandestine carrier dead. Transmitter room RTG depleted. ] ...STATIC",
                JammedText = "STATIC... [ Heterodyne whistle shifting across the shortwave band. ]"
            });

            // 6. Automated Meteorological & Emergency Relay
            Register(new RadioStationDefinition
            {
                StationId = StationAutomatedRelay,
                DisplayName = "Automated Emergency Beacon Array",
                FrequencyMhz = 142.85f,
                OwnerFactionId = "faction_automated_infrastructure",
                PersonaVoice = "Synthetic robotic teletype synthesizer, continuous navigational carrier pings.",
                Reliability = SourceReliability.Automated,
                DefaultState = RadioStationState.Normal,
                SilenceText = "STATIC... [ Automated repeater shut down. Battery bank drained to zero. ]",
                JammedText = "STATIC... [ Telemetry carrier mutilated by EMP surge. ]"
            });
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
    }
}
