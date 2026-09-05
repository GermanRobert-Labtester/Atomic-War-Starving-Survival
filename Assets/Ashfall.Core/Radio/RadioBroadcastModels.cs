// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Content genre / classification for unified radio broadcasts.
    /// </summary>
    public enum BroadcastGenre
    {
        CivilianNews = 0,
        MilitaryEdict = 1,
        EmergencyAlert = 2,
        NumbersStation = 3,
        SurvivorTestimony = 4,
        ReligiousLiturgy = 5,
        TradeMarket = 6,
        Educational = 7,
        InfrastructureLogistics = 8,
        VerdictCensus = 9,
        FactionWar = 10,
        AutomatedLoop = 11,
        DistressSignal = 12,
        AtmosphericMystery = 13
    }

    /// <summary>
    /// Source reliability tier for diegetic broadcasts.
    /// </summary>
    public enum SourceReliability
    {
        Official = 0,
        Partisan = 1,
        Anonymous = 2,
        Automated = 3,
        Unknown = 4
    }

    /// <summary>
    /// Alert / priority tier for radio transmission interruption.
    /// </summary>
    public enum BroadcastPriority
    {
        Routine = 0,
        Important = 1,
        Urgent = 2,
        Emergency = 3
    }

    /// <summary>
    /// Operational state of a radio station broadcaster.
    /// </summary>
    public enum RadioStationState
    {
        Normal = 0,
        Degraded = 1,
        Jammed = 2,
        Captured = 3,
        Silent = 4,
        EmergencyOnly = 5
    }

    /// <summary>
    /// Scheduled program time-slot for a radio station.
    /// </summary>
    [Serializable]
    public sealed class RadioProgramSlot
    {
        [JsonPropertyName("slot_id")]
        public string SlotId { get; set; } = string.Empty;

        [JsonPropertyName("start_hour")]
        public int StartHour { get; set; } = 0;

        [JsonPropertyName("end_hour")]
        public int EndHour { get; set; } = 23;

        [JsonPropertyName("program_type")]
        public string ProgramType { get; set; } = string.Empty;

        [JsonPropertyName("broadcast_pool_id")]
        public string BroadcastPoolId { get; set; } = string.Empty;

        [JsonPropertyName("min_state")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RadioStationState MinStationState { get; set; } = RadioStationState.Normal;

        [JsonPropertyName("weight")]
        public int Weight { get; set; } = 10;

        [JsonPropertyName("day_min")]
        public int DayMin { get; set; } = 1;

        [JsonPropertyName("day_max")]
        public int DayMax { get; set; } = 9999;

        public bool MatchesTime(int campaignDay, int hour)
        {
            if (campaignDay < DayMin || campaignDay > DayMax)
                return false;

            int h = ((hour % 24) + 24) % 24;
            if (StartHour <= EndHour)
                return h >= StartHour && h <= EndHour;
            // Wraps midnight (e.g. 22..4)
            return h >= StartHour || h <= EndHour;
        }
    }

    /// <summary>
    /// Reception factors used to calculate effective signal strength and explain degradations.
    /// </summary>
    public sealed class RadioReceptionFactors
    {
        public float DistanceKm { get; set; } = 0f;
        public float WeatherAttenuation01 { get; set; } = 0f;
        public bool IsBrownout { get; set; }
        public float ReceiverCondition01 { get; set; } = 1.0f;
        public bool IsJammed { get; set; }
        public bool HasAntennaArray { get; set; }
        public bool HasAmplifier { get; set; }
    }

    /// <summary>
    /// Evaluated radio signal strength with typed degradation reasons.
    /// </summary>
    [Serializable]
    public sealed class RadioSignalStrength
    {
        public float RawStrength01 { get; set; } = 1.0f;
        public float EffectiveStrength01 { get; set; } = 1.0f;
        public string QualityBand { get; set; } = "Optimal"; // Optimal, Good, Degraded, Critical, Unreadable
        public List<string> Reasons { get; set; } = new List<string>();

        public static RadioSignalStrength Evaluate(float baseStrength01, RadioReceptionFactors? factors)
        {
            factors ??= new RadioReceptionFactors();
            var reasons = new List<string>();
            float eff = Math.Clamp(baseStrength01, 0f, 1f);

            if (factors.DistanceKm > 50f)
            {
                float loss = Math.Min(0.4f, (factors.DistanceKm - 50f) * 0.005f);
                eff = Math.Max(0f, eff - loss);
                reasons.Add("distance_loss");
            }

            if (factors.WeatherAttenuation01 > 0.1f)
            {
                float loss = factors.WeatherAttenuation01 * 0.35f;
                eff = Math.Max(0f, eff - loss);
                reasons.Add("weather_attenuation");
            }

            if (factors.IsBrownout)
            {
                eff *= 0.3f;
                reasons.Add("power_brownout");
            }

            if (factors.ReceiverCondition01 < 0.8f)
            {
                float penalty = (1f - factors.ReceiverCondition01) * 0.4f;
                eff = Math.Max(0f, eff - penalty);
                reasons.Add("receiver_damage");
            }

            if (factors.IsJammed)
            {
                eff = Math.Min(eff, 0.15f);
                reasons.Add("jamming");
            }

            if (factors.HasAntennaArray)
            {
                eff = Math.Min(1.0f, eff + 0.15f);
                reasons.Add("antenna_bonus");
            }

            if (factors.HasAmplifier)
            {
                eff = Math.Min(1.0f, eff + 0.10f);
                reasons.Add("amplifier_bonus");
            }

            eff = Math.Clamp(eff, 0f, 1f);

            string band;
            if (eff >= 0.80f) band = "Optimal";
            else if (eff >= 0.55f) band = "Good";
            else if (eff >= 0.35f) band = "Degraded";
            else if (eff >= 0.15f) band = "Critical";
            else band = "Unreadable";

            return new RadioSignalStrength
            {
                RawStrength01 = baseStrength01,
                EffectiveStrength01 = eff,
                QualityBand = band,
                Reasons = reasons
            };
        }
    }

    /// <summary>
    /// Canonical station definition in ASHFALL airwaves.
    /// </summary>
    [Serializable]
    public sealed class RadioStationDefinition
    {
        private string _stationId = string.Empty;

        [JsonPropertyName("station_id")]
        public string StationId
        {
            get => _stationId;
            set => _stationId = value;
        }

        [JsonPropertyName("id")]
        public string Id
        {
            get => _stationId;
            set { if (string.IsNullOrEmpty(_stationId)) _stationId = value; }
        }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("frequency_mhz")]
        public float FrequencyMhz { get; set; } = 88.5f;

        [JsonPropertyName("owner_faction_id")]
        public string OwnerFactionId { get; set; } = string.Empty;

        [JsonPropertyName("persona_voice")]
        public string PersonaVoice { get; set; } = string.Empty;

        [JsonPropertyName("reliability")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceReliability Reliability { get; set; } = SourceReliability.Official;

        [JsonPropertyName("default_state")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RadioStationState DefaultState { get; set; } = RadioStationState.Normal;

        [JsonPropertyName("silence_text")]
        public string SilenceText { get; set; } = "STATIC... [ Carrier hum steady. No voice detected. ]";

        [JsonPropertyName("jammed_text")]
        public string JammedText { get; set; } = "STATIC... [ Severe RF interference / heterodyne squeal. ]";

        [JsonPropertyName("signal_profile_id")]
        public string SignalProfileId { get; set; } = string.Empty;

        [JsonPropertyName("equipment_requirements")]
        public List<string> EquipmentRequirements { get; set; } = new List<string>();

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("schedule")]
        public List<RadioProgramSlot> Schedule { get; set; } = new List<RadioProgramSlot>();

        public RadioProgramSlot? GetCurrentSlot(int campaignDay, int hour)
        {
            if (Schedule == null || Schedule.Count == 0) return null;
            RadioProgramSlot? best = null;
            foreach (var slot in Schedule)
            {
                if (!slot.MatchesTime(campaignDay, hour)) continue;
                if (best == null)
                {
                    best = slot;
                }
                else if (slot.Weight > best.Weight)
                {
                    best = slot;
                }
                else if (slot.Weight == best.Weight)
                {
                    if (slot.StartHour < best.StartHour)
                    {
                        best = slot;
                    }
                    else if (slot.StartHour == best.StartHour &&
                             string.Compare(slot.SlotId, best.SlotId, StringComparison.Ordinal) < 0)
                    {
                        best = slot;
                    }
                }
            }
            return best;
        }

        public RadioProgramSlot? GetNextSlot(int campaignDay, int hour)
        {
            if (Schedule == null || Schedule.Count == 0) return null;
            var current = GetCurrentSlot(campaignDay, hour);
            for (int offset = 1; offset <= 24; offset++)
            {
                int targetHour = (hour + offset) % 24;
                int targetDay = campaignDay + ((hour + offset) / 24);
                var slot = GetCurrentSlot(targetDay, targetHour);
                if (slot != null && (current == null || slot.SlotId != current.SlotId))
                {
                    return slot;
                }
            }
            return current;
        }
    }

    /// <summary>
    /// Unified broadcast record combining data from radio.json, year_of_ash_radio.json,
    /// verdict_radio.json, and faction_war_radio.json.
    /// </summary>
    [Serializable]
    public sealed class UnifiedRadioBroadcast
    {
        public string BroadcastId { get; set; } = string.Empty;
        public float FrequencyMhz { get; set; } = 88.5f;
        public int DayMin { get; set; } = 1;
        public int DayMax { get; set; } = 9999;
        public int DayTrigger { get; set; } = 1;
        public string StationId { get; set; } = string.Empty;
        public string SourceName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
        public SourceReliability Reliability { get; set; } = SourceReliability.Official;
        public BroadcastPriority Priority { get; set; } = BroadcastPriority.Routine;
        public int SignalStrength { get; set; } = 5; // S-units 1..9
        public bool IsEmergency { get; set; }
        public bool IsOneShot { get; set; }
        public string AudioCue { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public string DownstreamConsequence { get; set; } = string.Empty;
    }

    /// <summary>
    /// Appointment program format definition.
    /// </summary>
    [Serializable]
    public sealed class AppointmentProgramDefinition
    {
        public string ProgramId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string StationId { get; set; } = string.Empty;
        public float FrequencyMhz { get; set; } = 88.5f;
        public int CadenceDays { get; set; } = 1;
        public int DayOffset { get; set; } = 0;
        public int CadenceWindow { get; set; } = 0;
        public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
        public BroadcastPriority Priority { get; set; } = BroadcastPriority.Important;
    }
}
