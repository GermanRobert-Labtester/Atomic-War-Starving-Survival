// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Weather
{
    public enum ClimateTrend
    {
        Cooling,
        Stable,
        Warming
    }

    [Serializable]
    public sealed class WinterPhaseDef
    {
        [JsonPropertyName("phase_id")]
        public string PhaseId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("start_day")]
        public int StartDay { get; set; } = 1;

        [JsonPropertyName("end_day")]
        public int EndDay { get; set; } = 90;

        [JsonPropertyName("severity_modifier")]
        public float SeverityModifier { get; set; } = 1.0f;

        [JsonPropertyName("temperature_base_celsius")]
        public float TemperatureBaseCelsius { get; set; } = -10.0f;

        [JsonPropertyName("storm_frequency_modifier")]
        public float StormFrequencyModifier { get; set; } = 1.0f;

        [JsonPropertyName("radiation_modifier")]
        public float RadiationModifier { get; set; } = 1.0f;

        [JsonPropertyName("daylight_penalty_hours")]
        public float DaylightPenaltyHours { get; set; } = 1.0f;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SeasonalCycleDef
    {
        [JsonPropertyName("season_id")]
        public string SeasonId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("duration_days")]
        public int DurationDays { get; set; } = 30;

        [JsonPropertyName("temperature_offset_celsius")]
        public float TemperatureOffsetCelsius { get; set; } = 0.0f;

        [JsonPropertyName("storm_chance_multiplier")]
        public float StormChanceMultiplier { get; set; } = 1.0f;

        [JsonPropertyName("base_daylight_hours")]
        public float BaseDaylightHours { get; set; } = 10.0f;

        [JsonPropertyName("morale_daily_impact")]
        public float MoraleDailyImpact { get; set; } = 0.0f;
    }

    [Serializable]
    public sealed class ClimateState
    {
        public int Day { get; set; }
        public string PhaseId { get; set; } = string.Empty;
        public string PhaseDisplayName { get; set; } = string.Empty;
        public string SeasonId { get; set; } = string.Empty;
        public string SeasonDisplayName { get; set; } = string.Empty;
        public float GlobalTemperatureCelsius { get; set; }
        public float StormSeverityMultiplier { get; set; }
        public float RadiationModifier { get; set; }
        public float EffectiveDaylightHours { get; set; }
        public float DailyMoraleImpact { get; set; }
        public float HeatingFuelCostMultiplier { get; set; }
        public float ExpeditionHazardMultiplier { get; set; }
        public float CropGrowthRateMultiplier { get; set; }
        public ClimateTrend Trend { get; set; }
    }

    [Serializable]
    public sealed class NuclearWinterCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("phases")]
        public List<WinterPhaseDef> Phases { get; set; } = new List<WinterPhaseDef>();

        [JsonPropertyName("seasons")]
        public List<SeasonalCycleDef> Seasons { get; set; } = new List<SeasonalCycleDef>();
    }

    [Serializable]
    public sealed class NuclearWinterSaveState
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("current_day")]
        public int CurrentDay { get; set; } = 1;

        [JsonPropertyName("last_phase_id")]
        public string LastPhaseId { get; set; } = string.Empty;

        [JsonPropertyName("last_season_id")]
        public string LastSeasonId { get; set; } = string.Empty;

        [JsonPropertyName("climate_anomaly_count")]
        public int ClimateAnomalyCount { get; set; }

        [JsonPropertyName("recorded_events")]
        public List<string> RecordedEvents { get; set; } = new List<string>();
    }

    /// <summary>
    /// Plan 164 / DEC-159: Nuclear Winter Progression System.
    /// Governs multi-phase climate deterioration, cyclical post-nuclear seasons,
    /// dynamic temperature declines, daylight deprivation, and systemic shelter/expedition modifiers.
    /// </summary>
    public sealed class NuclearWinterProgressionSystem
    {
        private readonly List<WinterPhaseDef> _phases = new List<WinterPhaseDef>();
        private readonly List<SeasonalCycleDef> _seasons = new List<SeasonalCycleDef>();
        private readonly List<string> _recordedEvents = new List<string>();

        private int _currentDay = 1;
        private string _lastPhaseId = string.Empty;
        private string _lastSeasonId = string.Empty;
        private int _anomalyCount;

        public IReadOnlyList<WinterPhaseDef> Phases => _phases;
        public IReadOnlyList<SeasonalCycleDef> Seasons => _seasons;
        public IReadOnlyList<string> RecordedEvents => _recordedEvents;
        public int CurrentDay => _currentDay;
        public int AnomalyCount => _anomalyCount;

        public Action<WinterPhaseDef, WinterPhaseDef>? OnPhaseTransitionedSeam { get; set; }
        public Action<SeasonalCycleDef, SeasonalCycleDef>? OnSeasonTransitionedSeam { get; set; }
        public Action<ClimateState, string>? OnSevereClimateAnomalySeam { get; set; }

        public NuclearWinterProgressionSystem()
        {
            LoadFallbackCatalog();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<NuclearWinterCatalogData>(json, options);
                if (data != null)
                {
                    if (data.Phases != null && data.Phases.Count > 0)
                    {
                        _phases.Clear();
                        _phases.AddRange(data.Phases.OrderBy(p => p.StartDay));
                    }

                    if (data.Seasons != null && data.Seasons.Count > 0)
                    {
                        _seasons.Clear();
                        _seasons.AddRange(data.Seasons);
                    }
                }
            }
            catch (JsonException)
            {
                // Catalog parsing failed, fallback retained
            }
        }

        public WinterPhaseDef GetPhaseForDay(int day)
        {
            if (_phases.Count == 0)
                LoadFallbackCatalog();

            var phase = _phases.FirstOrDefault(p => day >= p.StartDay && day <= p.EndDay);
            return phase ?? _phases.Last();
        }

        public SeasonalCycleDef GetSeasonForDay(int day)
        {
            if (_seasons.Count == 0)
                LoadFallbackCatalog();

            int totalCycleDays = _seasons.Sum(s => Math.Max(1, s.DurationDays));
            if (totalCycleDays <= 0) totalCycleDays = 120;

            int dayInCycle = (Math.Max(1, day) - 1) % totalCycleDays;
            int accumulatedDays = 0;

            foreach (var season in _seasons)
            {
                accumulatedDays += Math.Max(1, season.DurationDays);
                if (dayInCycle < accumulatedDays)
                    return season;
            }

            return _seasons[0];
        }

        public ClimateState EvaluateClimate(int day, ISeededRng? rng = null)
        {
            var phase = GetPhaseForDay(day);
            var season = GetSeasonForDay(day);

            float baseTemp = phase.TemperatureBaseCelsius + season.TemperatureOffsetCelsius;
            float stormMult = phase.StormFrequencyModifier * season.StormChanceMultiplier;
            float radMod = phase.RadiationModifier;
            float daylight = Math.Max(2.0f, season.BaseDaylightHours - phase.DaylightPenaltyHours);
            float dailyMorale = season.MoraleDailyImpact - (phase.SeverityModifier > 1.2f ? (phase.SeverityModifier - 1.0f) * 1.5f : 0f);

            // Deterministic daily jitter if RNG supplied
            if (rng != null)
            {
                float tempJitter = (float)(rng.NextDouble() * 4.0 - 2.0); // +/- 2 degrees
                baseTemp += tempJitter;
            }

            // Trend evaluation
            ClimateTrend trend;
            if (phase.PhaseId.Contains("escalating") || phase.PhaseId.Contains("peak"))
                trend = ClimateTrend.Cooling;
            else if (phase.PhaseId.Contains("late") || phase.PhaseId.Contains("stabilizing"))
                trend = ClimateTrend.Warming;
            else
                trend = ClimateTrend.Stable;

            // Derived systemic multipliers
            float heatingMultiplier = Math.Max(1.0f, 1.0f + (Math.Abs(Math.Min(0f, baseTemp)) / 15.0f));
            float expeditionHazardMultiplier = Math.Max(1.0f, stormMult * (phase.SeverityModifier));
            float cropGrowthMultiplier = Math.Max(0.05f, (daylight / 12.0f) * (1.0f / phase.SeverityModifier));

            return new ClimateState
            {
                Day = day,
                PhaseId = phase.PhaseId,
                PhaseDisplayName = phase.DisplayName,
                SeasonId = season.SeasonId,
                SeasonDisplayName = season.DisplayName,
                GlobalTemperatureCelsius = (float)Math.Round(baseTemp, 1),
                StormSeverityMultiplier = (float)Math.Round(stormMult, 2),
                RadiationModifier = (float)Math.Round(radMod, 2),
                EffectiveDaylightHours = (float)Math.Round(daylight, 1),
                DailyMoraleImpact = (float)Math.Round(dailyMorale, 2),
                HeatingFuelCostMultiplier = (float)Math.Round(heatingMultiplier, 2),
                ExpeditionHazardMultiplier = (float)Math.Round(expeditionHazardMultiplier, 2),
                CropGrowthRateMultiplier = (float)Math.Round(cropGrowthMultiplier, 2),
                Trend = trend
            };
        }

        public ClimateState AdvanceDay(int day, ISeededRng? rng = null)
        {
            _currentDay = day;
            var state = EvaluateClimate(day, rng);

            // Phase transition detection
            if (!string.IsNullOrEmpty(_lastPhaseId) && _lastPhaseId != state.PhaseId)
            {
                var prevPhase = _phases.FirstOrDefault(p => p.PhaseId == _lastPhaseId) ?? GetPhaseForDay(day - 1);
                var curPhase = GetPhaseForDay(day);
                _recordedEvents.Add($"Day {day}: Nuclear winter entered phase '{curPhase.DisplayName}'");
                OnPhaseTransitionedSeam?.Invoke(prevPhase, curPhase);
            }
            _lastPhaseId = state.PhaseId;

            // Season transition detection
            if (!string.IsNullOrEmpty(_lastSeasonId) && _lastSeasonId != state.SeasonId)
            {
                var prevSeason = _seasons.FirstOrDefault(s => s.SeasonId == _lastSeasonId) ?? GetSeasonForDay(day - 1);
                var curSeason = GetSeasonForDay(day);
                _recordedEvents.Add($"Day {day}: Season transitioned to '{curSeason.DisplayName}'");
                OnSeasonTransitionedSeam?.Invoke(prevSeason, curSeason);
            }
            _lastSeasonId = state.SeasonId;

            // Check for severe climate anomaly (deterministic roll)
            if (rng != null && state.StormSeverityMultiplier > 1.5f)
            {
                if (rng.NextDouble() < 0.15) // 15% chance during severe seasons
                {
                    _anomalyCount++;
                    string anomalyDesc = state.GlobalTemperatureCelsius < -20.0f
                        ? "Atmospheric Deep Freeze Cold Snap"
                        : "Stratospheric Fallout Ashstorm";
                    _recordedEvents.Add($"Day {day}: Anomaly occurred: {anomalyDesc}");
                    OnSevereClimateAnomalySeam?.Invoke(state, anomalyDesc);
                }
            }

            return state;
        }

        public float CalculateHeatingDemand(float baseDemand, int day)
        {
            var state = EvaluateClimate(day);
            return (float)Math.Round(baseDemand * state.HeatingFuelCostMultiplier, 2);
        }

        public float CalculateExpeditionRisk(float baseRisk, int day)
        {
            var state = EvaluateClimate(day);
            return (float)Math.Round(baseRisk * state.ExpeditionHazardMultiplier, 2);
        }

        public float CalculateCropYieldMultiplier(int day, bool hasGreenhouse)
        {
            var state = EvaluateClimate(day);
            if (hasGreenhouse)
            {
                // Greenhouse protects against sub-zero temperature kill, providing steady base yield
                return Math.Max(0.75f, state.CropGrowthRateMultiplier * 1.5f);
            }
            return state.CropGrowthRateMultiplier;
        }

        public NuclearWinterSaveState CaptureState()
        {
            return new NuclearWinterSaveState
            {
                SchemaVersion = 1,
                CurrentDay = _currentDay,
                LastPhaseId = _lastPhaseId,
                LastSeasonId = _lastSeasonId,
                ClimateAnomalyCount = _anomalyCount,
                RecordedEvents = new List<string>(_recordedEvents)
            };
        }

        public void RestoreState(NuclearWinterSaveState? state)
        {
            if (state == null)
                return;

            _currentDay = state.CurrentDay > 0 ? state.CurrentDay : 1;
            _lastPhaseId = state.LastPhaseId ?? string.Empty;
            _lastSeasonId = state.LastSeasonId ?? string.Empty;
            _anomalyCount = state.ClimateAnomalyCount;
            _recordedEvents.Clear();
            if (state.RecordedEvents != null)
                _recordedEvents.AddRange(state.RecordedEvents);
        }

        private void LoadFallbackCatalog()
        {
            _phases.Clear();
            _phases.Add(new WinterPhaseDef
            {
                PhaseId = "phase_initial",
                DisplayName = "Initial Winter",
                StartDay = 1,
                EndDay = 90,
                SeverityModifier = 1.0f,
                TemperatureBaseCelsius = -10.0f,
                StormFrequencyModifier = 1.0f,
                RadiationModifier = 1.4f,
                DaylightPenaltyHours = 1.5f
            });
            _phases.Add(new WinterPhaseDef
            {
                PhaseId = "phase_escalating",
                DisplayName = "Escalating Freeze",
                StartDay = 91,
                EndDay = 180,
                SeverityModifier = 1.3f,
                TemperatureBaseCelsius = -18.0f,
                StormFrequencyModifier = 1.4f,
                RadiationModifier = 1.8f,
                DaylightPenaltyHours = 2.5f
            });
            _phases.Add(new WinterPhaseDef
            {
                PhaseId = "phase_peak",
                DisplayName = "Peak Depths",
                StartDay = 181,
                EndDay = 270,
                SeverityModifier = 1.7f,
                TemperatureBaseCelsius = -28.0f,
                StormFrequencyModifier = 1.8f,
                RadiationModifier = 1.6f,
                DaylightPenaltyHours = 3.5f
            });
            _phases.Add(new WinterPhaseDef
            {
                PhaseId = "phase_late",
                DisplayName = "Late Settling",
                StartDay = 271,
                EndDay = 360,
                SeverityModifier = 1.2f,
                TemperatureBaseCelsius = -15.0f,
                StormFrequencyModifier = 1.2f,
                RadiationModifier = 1.1f,
                DaylightPenaltyHours = 2.0f
            });
            _phases.Add(new WinterPhaseDef
            {
                PhaseId = "phase_stabilizing",
                DisplayName = "New Normal",
                StartDay = 361,
                EndDay = 99999,
                SeverityModifier = 1.0f,
                TemperatureBaseCelsius = -8.0f,
                StormFrequencyModifier = 0.9f,
                RadiationModifier = 0.8f,
                DaylightPenaltyHours = 1.0f
            });

            _seasons.Clear();
            _seasons.Add(new SeasonalCycleDef
            {
                SeasonId = "season_deep_winter",
                DisplayName = "Deep Winter",
                DurationDays = 30,
                TemperatureOffsetCelsius = -10.0f,
                StormChanceMultiplier = 1.5f,
                BaseDaylightHours = 6.5f,
                MoraleDailyImpact = -1.5f
            });
            _seasons.Add(new SeasonalCycleDef
            {
                SeasonId = "season_pale_spring",
                DisplayName = "Pale Spring",
                DurationDays = 30,
                TemperatureOffsetCelsius = -2.0f,
                StormChanceMultiplier = 1.1f,
                BaseDaylightHours = 10.0f,
                MoraleDailyImpact = 0.0f
            });
            _seasons.Add(new SeasonalCycleDef
            {
                SeasonId = "season_ash_summer",
                DisplayName = "Ash Summer",
                DurationDays = 30,
                TemperatureOffsetCelsius = 6.0f,
                StormChanceMultiplier = 0.7f,
                BaseDaylightHours = 14.0f,
                MoraleDailyImpact = 1.0f
            });
            _seasons.Add(new SeasonalCycleDef
            {
                SeasonId = "season_black_autumn",
                DisplayName = "Black Autumn",
                DurationDays = 30,
                TemperatureOffsetCelsius = -3.0f,
                StormChanceMultiplier = 1.2f,
                BaseDaylightHours = 9.5f,
                MoraleDailyImpact = -0.5f
            });
        }
    }
}
