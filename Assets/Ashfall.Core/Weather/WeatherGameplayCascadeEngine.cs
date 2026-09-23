// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Weather
{
    public enum CascadeTargetSystem
    {
        Shelter,
        Expedition,
        Faction,
        Economy,
        MentalHealth,
        Location
    }

    public enum CascadeEffectType
    {
        Damage,
        Delay,
        PriceChange,
        BehaviorChange,
        Accessibility,
        ThermalLoad,
        FiltrationStress,
        PowerOutput,
        HazardSurge
    }

    [Serializable]
    public sealed class WeatherEffectDef
    {
        public string id { get; set; } = string.Empty;
        public string weather_kind { get; set; } = string.Empty;
        public string target_system { get; set; } = string.Empty;
        public string effect_type { get; set; } = string.Empty;
        public float magnitude { get; set; }
        public int duration_days { get; set; } = 1;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class WeatherEffect
    {
        public string effectId { get; set; } = string.Empty;
        public CascadeTargetSystem targetSystem { get; set; }
        public CascadeEffectType effectType { get; set; }
        public float magnitude { get; set; }
        public int durationDays { get; set; }
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class WeatherEvent
    {
        public string id { get; set; } = string.Empty;
        public WeatherKind weatherKind { get; set; }
        public float severity { get; set; } // 0..100
        public int startDay { get; set; }
        public int durationDays { get; set; }
        public List<string> affectedRegions { get; set; } = new List<string>();
        public List<WeatherEffect> effects { get; set; } = new List<WeatherEffect>();
    }

    [Serializable]
    public sealed class WeatherCascadeState
    {
        public int schema_version { get; set; } = 1;
        public List<WeatherEvent> activeEvents { get; set; } = new List<WeatherEvent>();
        public List<WeatherEvent> eventHistory { get; set; } = new List<WeatherEvent>();
        public List<WeatherEffect> activeEffects { get; set; } = new List<WeatherEffect>();
    }

    /// <summary>
    /// Pure domain engine that translates atmospheric weather events into concrete
    /// operational pressures across shelter integrity, expedition risk, faction behavior,
    /// economic price shocks, mental health morale, and location accessibility (C2[27] / Plan 135 / DEC-139).
    /// </summary>
    public sealed class WeatherGameplayCascadeEngine
    {
        public const string DefaultCatalogFileName = "weather_gameplay_effects.json";

        /// <summary>State schema version. A save carrying any other version is
        /// rejected rather than partially restored (Plan 135 Task 1 step 5).</summary>
        public const int StateSchemaVersion = 1;

        private readonly List<WeatherEffectDef> _catalogTemplates = new List<WeatherEffectDef>();
        private WeatherCascadeState _state;

        // Integration Seams
        public Action<WeatherEvent, IReadOnlyList<WeatherEffect>>? OnWeatherCascadeEvaluatedSeam { get; set; }
        public Action<string, float>? OnShelterDamageSurgedSeam { get; set; }
        public Action<string, float>? OnExpeditionDelayForecastedSeam { get; set; }
        public Action<string, float, int>? OnMarketShockTriggeredSeam { get; set; }
        public Action<string, float>? OnMentalHealthStressSurgedSeam { get; set; }
        public Action<string, bool>? OnLocationAccessibilityChangedSeam { get; set; }

        public WeatherGameplayCascadeEngine(WeatherCascadeState? state = null)
        {
            _state = state ?? new WeatherCascadeState();
        }

        public WeatherCascadeState State => _state;
        public IReadOnlyList<WeatherEffectDef> Templates => _catalogTemplates;

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var list = CatalogLocator.LoadWrappedList<WeatherEffectDef>(json, SystemTextJsonSerializer.Options);
                if (list != null && list.Count > 0)
                {
                    _catalogTemplates.Clear();
                    _catalogTemplates.AddRange(list);
                }
            }
            catch
            {
                // Fallback: templates empty
            }
        }

        /// <summary>
        /// Plan 135 / C2[27] strict path: replaces the authored templates with
        /// rows that have already been validated by
        /// <c>WeatherCascadeCatalogLoader</c>. Unlike <see cref="LoadCatalog"/>
        /// this is caller-policed: the caller has proof the rows resolve to real
        /// weather kinds, target systems, and effect types, so no lenient
        /// vocabulary fallback can hide an authored typo inside a live cascade.
        /// Returns the number of templates loaded (0 leaves the engine unbound,
        /// which the host treats as an error rather than generating effects).
        /// </summary>
        public int BindValidatedTemplates(IReadOnlyList<WeatherEffectDef>? templates)
        {
            _catalogTemplates.Clear();
            if (templates == null) return 0;
            int loaded = 0;
            foreach (var t in templates)
            {
                if (t == null || string.IsNullOrWhiteSpace(t.id)) continue;
                _catalogTemplates.Add(t);
                loaded++;
            }
            return loaded;
        }

        public static WeatherGameplayCascadeEngine LoadFromDirectory(string dataDir, IFileIO fileIO)
        {
            var engine = new WeatherGameplayCascadeEngine();
            if (fileIO != null && !string.IsNullOrEmpty(dataDir))
            {
                string path = fileIO.Combine(dataDir, DefaultCatalogFileName);
                if (fileIO.FileExists(path))
                {
                    engine.LoadCatalog(fileIO.ReadAllText(path));
                }
            }
            return engine;
        }

        /// <summary>
        /// Evaluates cascade effects for a weather occurrence, creating a registered WeatherEvent
        /// and firing relevant domain seams.
        /// </summary>
        public WeatherEvent EvaluateWeatherCascade(
            WeatherKind kind,
            float severity,
            int currentDay,
            IReadOnlyList<string>? regions = null,
            int fortificationLevel = 0,
            ISeededRng? rng = null)
        {
            severity = Math.Clamp(severity, 0f, 100f);
            var affectedRegions = regions != null && regions.Count > 0
                ? regions.ToList()
                : new List<string> { "holdfast_core_sector" };

            var matchingTemplates = _catalogTemplates
                .Where(t => string.Equals(t.weather_kind, kind.ToString(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            var effects = new List<WeatherEffect>();
            int maxDuration = 1;

            if (matchingTemplates.Count > 0)
            {
                foreach (var template in matchingTemplates)
                {
                    var target = ParseTarget(template.target_system);
                    var type = ParseEffectType(template.effect_type);
                    float effectiveMagnitude = template.magnitude;

                    // Fortification mitigation: shelter damage is eliminated or reduced if fortified
                    if (target == CascadeTargetSystem.Shelter && type == CascadeEffectType.Damage)
                    {
                        if (fortificationLevel >= 2)
                        {
                            effectiveMagnitude = 0f;
                        }
                        else if (fortificationLevel == 1)
                        {
                            effectiveMagnitude *= 0.5f;
                        }
                    }

                    int duration = Math.Max(1, template.duration_days);
                    if (duration > maxDuration) maxDuration = duration;

                    effects.Add(new WeatherEffect
                    {
                        effectId = template.id,
                        targetSystem = target,
                        effectType = type,
                        magnitude = effectiveMagnitude,
                        durationDays = duration,
                        description = template.description
                    });
                }
            }
            else
            {
                // Fallback algorithmic generation when templates unauthored
                effects.AddRange(GenerateDefaultEffects(kind, severity, fortificationLevel));
                maxDuration = effects.Count > 0 ? effects.Max(e => e.durationDays) : 1;
            }

            var weatherEvent = new WeatherEvent
            {
                id = $"weather_event_{kind.ToString().ToLowerInvariant()}_{currentDay}_{_state.eventHistory.Count + 1}",
                weatherKind = kind,
                severity = severity,
                startDay = currentDay,
                durationDays = maxDuration,
                affectedRegions = affectedRegions,
                effects = effects
            };

            // Register into active state
            _state.activeEvents.Add(weatherEvent);
            foreach (var eff in effects)
            {
                _state.activeEffects.Add(eff);
            }

            // Fire Domain Seams
            OnWeatherCascadeEvaluatedSeam?.Invoke(weatherEvent, effects);

            foreach (var eff in effects)
            {
                switch (eff.targetSystem)
                {
                    case CascadeTargetSystem.Shelter:
                        if (eff.effectType == CascadeEffectType.Damage && eff.magnitude > 0f)
                        {
                            OnShelterDamageSurgedSeam?.Invoke("weather_structural_stress", eff.magnitude);
                        }
                        break;
                    case CascadeTargetSystem.Expedition:
                        if (eff.effectType == CascadeEffectType.Delay)
                        {
                            foreach (var r in affectedRegions)
                            {
                                OnExpeditionDelayForecastedSeam?.Invoke(r, eff.magnitude);
                            }
                        }
                        break;
                    case CascadeTargetSystem.Economy:
                        if (eff.effectType == CascadeEffectType.PriceChange)
                        {
                            OnMarketShockTriggeredSeam?.Invoke("fuel_and_supplies", eff.magnitude, eff.durationDays);
                        }
                        break;
                    case CascadeTargetSystem.MentalHealth:
                        OnMentalHealthStressSurgedSeam?.Invoke(eff.description, eff.magnitude);
                        break;
                    case CascadeTargetSystem.Location:
                        if (eff.effectType == CascadeEffectType.Accessibility)
                        {
                            bool isAccessible = eff.magnitude > 0f;
                            foreach (var r in affectedRegions)
                            {
                                OnLocationAccessibilityChangedSeam?.Invoke(r, isAccessible);
                            }
                        }
                        break;
                }
            }

            return weatherEvent;
        }

        /// <summary>
        /// Advances the simulation clock by 1 day, decrementing active effect durations
        /// and archiving expired events to history.
        /// </summary>
        public void TickDay(int currentDay)
        {
            var remainingEvents = new List<WeatherEvent>();
            foreach (var ev in _state.activeEvents)
            {
                if (currentDay >= ev.startDay + ev.durationDays)
                {
                    _state.eventHistory.Add(ev);
                }
                else
                {
                    remainingEvents.Add(ev);
                }
            }
            _state.activeEvents = remainingEvents;

            // Recalculate active effects
            _state.activeEffects = remainingEvents.SelectMany(e => e.effects).ToList();
        }

        public WeatherCascadeState CaptureState()
        {
            return new WeatherCascadeState
            {
                schema_version = _state.schema_version,
                activeEvents = _state.activeEvents.Select(CloneEvent).ToList(),
                eventHistory = _state.eventHistory.Select(CloneEvent).ToList(),
                activeEffects = _state.activeEffects.Select(CloneEffect).ToList()
            };
        }

        public void RestoreState(WeatherCascadeState? state)
        {
            if (state == null)
            {
                _state = new WeatherCascadeState();
                return;
            }

            // Schema gate: an unknown version keeps the current state rather
            // than restoring rows the engine cannot interpret. The host reports
            // the rejection through the journal/CLI instead of silently
            // half-applying a foreign shape.
            if (state.schema_version != StateSchemaVersion)
            {
                throw new InvalidOperationException(
                    $"WeatherCascadeState schema_version '{state.schema_version}' is not supported "
                    + $"(expected {StateSchemaVersion}).");
            }

            _state = new WeatherCascadeState
            {
                schema_version = state.schema_version,
                activeEvents = state.activeEvents != null ? state.activeEvents.Select(CloneEvent).ToList() : new List<WeatherEvent>(),
                eventHistory = state.eventHistory != null ? state.eventHistory.Select(CloneEvent).ToList() : new List<WeatherEvent>(),
                activeEffects = state.activeEffects != null ? state.activeEffects.Select(CloneEffect).ToList() : new List<WeatherEffect>()
            };
        }

        private static List<WeatherEffect> GenerateDefaultEffects(WeatherKind kind, float severity, int fortificationLevel)
        {
            var list = new List<WeatherEffect>();
            bool severe = severity >= 60f;

            if (severe && (kind == WeatherKind.Blizzard || kind == WeatherKind.BlackRain || kind == WeatherKind.FalloutStorm || kind == WeatherKind.IceStorm || kind == WeatherKind.GlassStorm))
            {
                float dmg = fortificationLevel >= 2 ? 0f : (fortificationLevel == 1 ? 12.5f : 25f);
                if (dmg > 0f)
                {
                    list.Add(new WeatherEffect
                    {
                        effectId = "default_storm_damage",
                        targetSystem = CascadeTargetSystem.Shelter,
                        effectType = CascadeEffectType.Damage,
                        magnitude = dmg,
                        durationDays = 2,
                        description = "Storm buffeting stresses exterior shelter bulkheads."
                    });
                }

                list.Add(new WeatherEffect
                {
                    effectId = "default_expedition_delay",
                    targetSystem = CascadeTargetSystem.Expedition,
                    effectType = CascadeEffectType.Delay,
                    magnitude = 0.5f,
                    durationDays = 2,
                    description = "Severe atmospheric storm slows overland travel."
                });

                list.Add(new WeatherEffect
                {
                    effectId = "default_economic_shock",
                    targetSystem = CascadeTargetSystem.Economy,
                    effectType = CascadeEffectType.PriceChange,
                    magnitude = 1.5f,
                    durationDays = 3,
                    description = "Storm scarcity triggers localized price spikes."
                });

                list.Add(new WeatherEffect
                {
                    effectId = "default_storm_depression",
                    targetSystem = CascadeTargetSystem.MentalHealth,
                    effectType = CascadeEffectType.BehaviorChange,
                    magnitude = -10f,
                    durationDays = 2,
                    description = "Confinement and storm acoustics dampen survivor morale."
                });
            }
            else if (kind == WeatherKind.FalseSpring || kind == WeatherKind.Clear)
            {
                list.Add(new WeatherEffect
                {
                    effectId = "default_morale_lift",
                    targetSystem = CascadeTargetSystem.MentalHealth,
                    effectType = CascadeEffectType.BehaviorChange,
                    magnitude = 15f,
                    durationDays = 2,
                    description = "Clear skies and moderate temperature raise survivor hope."
                });
            }

            return list;
        }

        private static CascadeTargetSystem ParseTarget(string target)
        {
            if (string.Equals(target, "shelter", StringComparison.OrdinalIgnoreCase)) return CascadeTargetSystem.Shelter;
            if (string.Equals(target, "expedition", StringComparison.OrdinalIgnoreCase)) return CascadeTargetSystem.Expedition;
            if (string.Equals(target, "faction", StringComparison.OrdinalIgnoreCase)) return CascadeTargetSystem.Faction;
            if (string.Equals(target, "economy", StringComparison.OrdinalIgnoreCase)) return CascadeTargetSystem.Economy;
            if (string.Equals(target, "mental_health", StringComparison.OrdinalIgnoreCase)) return CascadeTargetSystem.MentalHealth;
            if (string.Equals(target, "location", StringComparison.OrdinalIgnoreCase)) return CascadeTargetSystem.Location;
            return CascadeTargetSystem.Shelter;
        }

        private static CascadeEffectType ParseEffectType(string type)
        {
            if (string.Equals(type, "damage", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.Damage;
            if (string.Equals(type, "delay", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.Delay;
            if (string.Equals(type, "price_change", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.PriceChange;
            if (string.Equals(type, "behavior_change", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.BehaviorChange;
            if (string.Equals(type, "accessibility", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.Accessibility;
            if (string.Equals(type, "thermal_load", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.ThermalLoad;
            if (string.Equals(type, "filtration_stress", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.FiltrationStress;
            if (string.Equals(type, "power_output", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.PowerOutput;
            if (string.Equals(type, "hazard_surge", StringComparison.OrdinalIgnoreCase)) return CascadeEffectType.HazardSurge;
            return CascadeEffectType.BehaviorChange;
        }

        private static WeatherEvent CloneEvent(WeatherEvent ev)
        {
            return new WeatherEvent
            {
                id = ev.id,
                weatherKind = ev.weatherKind,
                severity = ev.severity,
                startDay = ev.startDay,
                durationDays = ev.durationDays,
                affectedRegions = ev.affectedRegions != null ? new List<string>(ev.affectedRegions) : new List<string>(),
                effects = ev.effects != null ? ev.effects.Select(CloneEffect).ToList() : new List<WeatherEffect>()
            };
        }

        private static WeatherEffect CloneEffect(WeatherEffect eff)
        {
            return new WeatherEffect
            {
                effectId = eff.effectId,
                targetSystem = eff.targetSystem,
                effectType = eff.effectType,
                magnitude = eff.magnitude,
                durationDays = eff.durationDays,
                description = eff.description
            };
        }
    }
}
