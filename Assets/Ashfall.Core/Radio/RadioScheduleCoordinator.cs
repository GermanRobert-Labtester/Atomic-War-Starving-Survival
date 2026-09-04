// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Evaluated scheduled transmission result returned to the player/host.
    /// </summary>
    public sealed class ScheduledBroadcastResult
    {
        public bool HasTransmission { get; set; }
        public float FrequencyMhz { get; set; }
        public string StationId { get; set; } = string.Empty;
        public string StationName { get; set; } = string.Empty;
        public string SourceName { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
        public SourceReliability Reliability { get; set; } = SourceReliability.Official;
        public BroadcastPriority Priority { get; set; } = BroadcastPriority.Routine;
        public int SignalStrength { get; set; } = 5; // S-units 1..9
        public float VuStrength { get; set; } = 0.5f; // 0..1
        public string AudioCue { get; set; } = string.Empty;
        public bool IsEmergency { get; set; }
        public bool IsSilence { get; set; }
        public bool IsJammed { get; set; }
        public string BroadcastId { get; set; } = string.Empty;

        public static ScheduledBroadcastResult StaticDeadAir(float freqMhz, string silenceMsg = "STATIC... [ No carrier detected on frequency. ] ...STATIC")
        {
            return new ScheduledBroadcastResult
            {
                HasTransmission = false,
                FrequencyMhz = freqMhz,
                StationId = string.Empty,
                StationName = "DEAD AIR / STATIC",
                SourceName = "Unattended Spectrum",
                Headline = "Dead Air",
                Message = silenceMsg,
                Genre = BroadcastGenre.AutomatedLoop,
                Reliability = SourceReliability.Automated,
                Priority = BroadcastPriority.Routine,
                SignalStrength = 1,
                VuStrength = 0.05f,
                IsSilence = true
            };
        }
    }

    /// <summary>
    /// Authoritative scheduling coordinator for ASHFALL airwaves.
    /// Invariant: Pure C#, deterministic via ISeededRng and day integer math, zero engine references.
    /// </summary>
    public sealed class RadioScheduleCoordinator
    {
        private readonly RadioBroadcastCatalog _catalog;
        private readonly RadioStationCatalog _stations;
        private readonly List<AppointmentProgramDefinition> _appointmentPrograms = new List<AppointmentProgramDefinition>();

        // Dynamic world-state alert injectors
        private string? _severeWeatherAlert;
        private string? _orbitalHarrowAlert;
        private string? _diseaseOutbreakAlert;
        private string? _routeDisruptionAlert;
        private string? _foundryStrikeAlert;
        private string? _treatyAlert;

        public RadioScheduleCoordinator(RadioBroadcastCatalog catalog, RadioStationCatalog stations)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _stations = stations ?? throw new ArgumentNullException(nameof(stations));
            RegisterAppointmentPrograms();
        }

        public RadioBroadcastCatalog Catalog => _catalog;
        public RadioStationCatalog Stations => _stations;
        public IReadOnlyList<AppointmentProgramDefinition> AppointmentPrograms => _appointmentPrograms;

        private void RegisterAppointmentPrograms()
        {
            _appointmentPrograms.Add(new AppointmentProgramDefinition
            {
                ProgramId = "prog_morning_weather",
                Title = "Morning Meteorological & Fallout Forecast",
                StationId = RadioStationCatalog.StationCivilDefense,
                FrequencyMhz = 88.50f,
                CadenceDays = 1,
                CadenceWindow = 0,
                Genre = BroadcastGenre.CivilianNews,
                Priority = BroadcastPriority.Important
            });

            _appointmentPrograms.Add(new AppointmentProgramDefinition
            {
                ProgramId = "prog_lost_and_found",
                Title = "Missing Persons & Survivor Message Roll",
                StationId = RadioStationCatalog.StationCivilDefense,
                FrequencyMhz = 88.50f,
                CadenceDays = 3,
                CadenceWindow = 1,
                Genre = BroadcastGenre.SurvivorTestimony,
                Priority = BroadcastPriority.Important
            });

            _appointmentPrograms.Add(new AppointmentProgramDefinition
            {
                ProgramId = "prog_market_caravan",
                Title = "Regional Market & Caravan Exchange Bulletin",
                StationId = RadioStationCatalog.StationOpenClassroom,
                FrequencyMhz = 101.50f,
                CadenceDays = 4,
                CadenceWindow = 2,
                Genre = BroadcastGenre.TradeMarket,
                Priority = BroadcastPriority.Important
            });

            _appointmentPrograms.Add(new AppointmentProgramDefinition
            {
                ProgramId = "prog_route_conditions",
                Title = "Waystation & Road Condition Service",
                StationId = RadioStationCatalog.StationOpenClassroom,
                FrequencyMhz = 142.50f,
                CadenceDays = 2,
                CadenceWindow = 0,
                Genre = BroadcastGenre.InfrastructureLogistics,
                Priority = BroadcastPriority.Important
            });

            _appointmentPrograms.Add(new AppointmentProgramDefinition
            {
                ProgramId = "prog_public_health",
                Title = "Public Health & Outbreak Advisory",
                StationId = RadioStationCatalog.StationCivilDefense,
                FrequencyMhz = 88.50f,
                CadenceDays = 7,
                CadenceWindow = 0,
                Genre = BroadcastGenre.EmergencyAlert,
                Priority = BroadcastPriority.Urgent
            });

            _appointmentPrograms.Add(new AppointmentProgramDefinition
            {
                ProgramId = "prog_industrial_foundry",
                Title = "Foundry & Labor Dispatch",
                StationId = RadioStationCatalog.StationGarrisonOverlord,
                FrequencyMhz = 88.40f,
                CadenceDays = 14,
                CadenceWindow = 0,
                Genre = BroadcastGenre.MilitaryEdict,
                Priority = BroadcastPriority.Important
            });
        }

        // ── World Alert Injection API ───────────────────────────────────────────

        public void InjectWeatherAlert(string? alertMessage) => _severeWeatherAlert = alertMessage;
        public void InjectOrbitalAlert(string? alertMessage) => _orbitalHarrowAlert = alertMessage;
        public void InjectDiseaseAlert(string? alertMessage) => _diseaseOutbreakAlert = alertMessage;
        public void InjectRouteAlert(string? alertMessage) => _routeDisruptionAlert = alertMessage;
        public void InjectFoundryAlert(string? alertMessage) => _foundryStrikeAlert = alertMessage;
        /// <summary>Plan VIII · Task 21.6 — canonical broadcast surfacing for typed
        /// treaty transitions (ratified/broken/expired). Rendered as a Regional
        /// Compact Wire bulletin on the market/classroom civilian stations.</summary>
        public void InjectTreatyAlert(string? alertMessage) => _treatyAlert = alertMessage;

        public void ClearDynamicAlerts()
        {
            _severeWeatherAlert = null;
            _orbitalHarrowAlert = null;
            _diseaseOutbreakAlert = null;
            _routeDisruptionAlert = null;
            _foundryStrikeAlert = null;
            _treatyAlert = null;
        }

        // ── Schedule Resolution ─────────────────────────────────────────────────

        public ScheduledBroadcastResult Resolve(float frequencyMhz, int day, ISeededRng rng, float toleranceMhz = 0.5f)
        {
            var station = _stations.FindStationAtFrequency(frequencyMhz, toleranceMhz);
            if (station == null)
            {
                return ScheduledBroadcastResult.StaticDeadAir(frequencyMhz);
            }

            var stationState = _stations.GetStationState(station.StationId);

            // 1. Station State Checks (Silent / Jammed)
            if (stationState == RadioStationState.Silent)
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = false,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = station.DisplayName,
                    Headline = "Station Carrier Silent",
                    Message = station.SilenceText,
                    Genre = BroadcastGenre.AutomatedLoop,
                    Reliability = station.Reliability,
                    Priority = BroadcastPriority.Routine,
                    SignalStrength = 1,
                    VuStrength = 0.05f,
                    IsSilence = true
                };
            }

            if (stationState == RadioStationState.Jammed)
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = "Jammed Carrier",
                    Headline = "Signal Jammed",
                    Message = station.JammedText,
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Unknown,
                    Priority = BroadcastPriority.Urgent,
                    SignalStrength = 2,
                    VuStrength = 0.2f,
                    IsJammed = true
                };
            }

            // 2. High-Priority Dynamic World Alerts (Emergency Tier)
            if (!string.IsNullOrEmpty(_severeWeatherAlert) &&
                (station.StationId == RadioStationCatalog.StationCivilDefense || station.StationId == RadioStationCatalog.StationAutomatedRelay))
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = "Civil Meteorological Bureau",
                    Headline = "EMERGENCY SEVERE WEATHER ALERT",
                    Message = _severeWeatherAlert,
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Emergency,
                    IsEmergency = true,
                    SignalStrength = 8,
                    VuStrength = 0.9f,
                    BroadcastId = "alert_dynamic_severe_weather"
                };
            }

            if (!string.IsNullOrEmpty(_orbitalHarrowAlert) &&
                (station.StationId == RadioStationCatalog.StationAutomatedRelay || station.StationId == RadioStationCatalog.StationCivilDefense))
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = "Orbital Defense Telemetry Array",
                    Headline = "FLASH: ORBITAL HARROW KINETIC REENTRY",
                    Message = _orbitalHarrowAlert,
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Automated,
                    Priority = BroadcastPriority.Emergency,
                    IsEmergency = true,
                    SignalStrength = 8,
                    VuStrength = 0.85f,
                    BroadcastId = "alert_dynamic_orbital_harrow"
                };
            }

            if (!string.IsNullOrEmpty(_diseaseOutbreakAlert) &&
                station.StationId == RadioStationCatalog.StationCivilDefense)
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = "Bureau of Public Health",
                    Headline = "PUBLIC HEALTH VECTOR OUTBREAK ADVISORY",
                    Message = _diseaseOutbreakAlert,
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Urgent,
                    IsEmergency = true,
                    SignalStrength = 7,
                    VuStrength = 0.8f,
                    BroadcastId = "alert_dynamic_disease_outbreak"
                };
            }

            if (!string.IsNullOrEmpty(_routeDisruptionAlert) &&
                (station.StationId == RadioStationCatalog.StationOpenClassroom || station.StationId == RadioStationCatalog.StationGarrisonOverlord))
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = "Waystation Scout / Lineman",
                    Headline = "CRITICAL ROUTE DISRUPTION BULLETIN",
                    Message = _routeDisruptionAlert,
                    Genre = BroadcastGenre.InfrastructureLogistics,
                    Reliability = SourceReliability.Anonymous,
                    Priority = BroadcastPriority.Urgent,
                    SignalStrength = 6,
                    VuStrength = 0.7f,
                    BroadcastId = "alert_dynamic_route_disruption"
                };
            }

            if (!string.IsNullOrEmpty(_treatyAlert) &&
                (station.StationId == RadioStationCatalog.StationOpenClassroom || station.StationId == RadioStationCatalog.StationAutomatedRelay))
            {
                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = "Regional Compact Wire",
                    Headline = "DIPLOMATIC ACCORD BULLETIN",
                    Message = _treatyAlert,
                    Genre = BroadcastGenre.CivilianNews,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 6,
                    VuStrength = 0.7f,
                    BroadcastId = "alert_dynamic_treaty_bulletin"
                };
            }

            // 3. Eligible Authored Broadcasts from Catalog
            var eligible = _catalog.GetEligibleBroadcasts(frequencyMhz, day, toleranceMhz);
            if (eligible.Count > 0)
            {
                // Sort by priority descending, then pick deterministically
                eligible.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                var topPriority = eligible[0].Priority;
                var topCandidates = eligible.FindAll(x => x.Priority == topPriority);

                int pickIdx = topCandidates.Count == 1 ? 0 : (rng != null ? rng.Next(0, topCandidates.Count) : (int)((uint)day * 2654435761u % (uint)topCandidates.Count));
                var picked = topCandidates[pickIdx];

                float offset = Math.Abs(station.FrequencyMhz - frequencyMhz);
                float vu = Math.Clamp(1.0f - (offset / toleranceMhz) * 0.7f, 0.1f, 1.0f);

                return new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    FrequencyMhz = frequencyMhz,
                    StationId = station.StationId,
                    StationName = station.DisplayName,
                    SourceName = string.IsNullOrEmpty(picked.SourceName) ? station.DisplayName : picked.SourceName,
                    Headline = string.IsNullOrEmpty(picked.Title) ? picked.Genre.ToString() : picked.Title,
                    Message = picked.Message,
                    Genre = picked.Genre,
                    Reliability = picked.Reliability,
                    Priority = picked.Priority,
                    SignalStrength = picked.SignalStrength,
                    VuStrength = vu,
                    AudioCue = picked.AudioCue,
                    IsEmergency = picked.IsEmergency,
                    BroadcastId = picked.BroadcastId
                };
            }

            // 4. Fallback Routine Broadcaster Carrier
            return new ScheduledBroadcastResult
            {
                HasTransmission = true,
                FrequencyMhz = frequencyMhz,
                StationId = station.StationId,
                StationName = station.DisplayName,
                SourceName = station.DisplayName,
                Headline = "Standard Station Carrier",
                Message = $"[{station.DisplayName}] Carrier active. Voice modulation idle. Monitoring frequency {station.FrequencyMhz:0.00} MHz.",
                Genre = BroadcastGenre.AutomatedLoop,
                Reliability = station.Reliability,
                Priority = BroadcastPriority.Routine,
                SignalStrength = 5,
                VuStrength = 0.5f
            };
        }
    }
}
