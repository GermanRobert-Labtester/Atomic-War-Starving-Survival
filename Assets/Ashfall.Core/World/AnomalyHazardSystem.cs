// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 176 — Anomaly Hazard System (dynamic world hazard layer).
// Authority split (invariant: one authority per concern):
//   • FalloutSystem     — nuclear fallout clouds (existing moving-rad authority)
//   • WeatherSystem     — weather truth + surface wind vector (Plan 205)
//   • RadiationSystem   — per-survivor dose accumulation (sole dose writer)
//   • THIS SYSTEM       — authored anomaly/storm-front hazard zones: spawn,
//     deterministic movement, approach warnings, detection classification,
//     gated loot-site resolution, bounded wildlife modifier, overlap rules.
// It NEVER mutates survivor health, never computes dose, never invents loot
// (loot comes from canonical table_loot_* tables through the host/expedition
// loot authority), and never moves wildlife (ecology consumes the modifier).
//
// Determinism contract: identical hazard state + identical wind inputs +
// identical day-keyed forked RNG → identical movement/spawn outcomes.
// Positions are quantized to 0.01 km after every move so save/load is exact.
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    public enum HazardDetection
    {
        Undetected = 0,   // outside warning radius: nothing is known
        Signature = 1,    // felt/seen directly (inside radius) or warning band: presence known, unidentified
        Classified = 2    // detector capability met the authored threshold: full identity
    }

    public enum HazardWarningProfile
    {
        Late = 0,      // warnings fire at 60% of warning radius, lower confidence
        Standard = 1,  // warnings fire at 85% of warning radius
        Early = 2      // warnings fire at the full warning radius, highest confidence
    }

    [Serializable]
    public sealed class AnomalyHazardInstance
    {
        public string hazard_id { get; set; } = string.Empty;      // hazard_anomaly_<n>_<anomalyId>
        public string anomaly_id { get; set; } = string.Empty;     // authored definition id
        public float position_x { get; set; }
        public float position_y { get; set; }
        public float bearing_deg { get; set; }                     // storm_front travel bearing (0=N, 90=E)
        public float radius_km { get; set; }
        public float radiation_rate { get; set; }                  // authored center rate (bp-free, rads/hr)
        public string movement_profile { get; set; } = "static";
        public float movement_speed_kph { get; set; }
        public float wind_response { get; set; }
        public float intensity { get; set; } = 1f;                 // decays linearly with age
        public int age_days { get; set; }
        public int duration_days { get; set; }
        public int spawn_day { get; set; }
        public string warning_profile { get; set; } = "standard";
        public float warning_radius_km { get; set; }
        public float detection_threshold { get; set; }
        public string loot_table_id { get; set; } = string.Empty;
        public int wildlife_modifier_bp { get; set; }
        public bool active { get; set; } = true;
        public List<string> fired_warning_keys { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class AnomalyLootSiteState
    {
        public string site_id { get; set; } = string.Empty;        // hazard_anomaly_1_anomaly_x_site_1
        public string hazard_id { get; set; } = string.Empty;
        public string loot_table_id { get; set; } = string.Empty;  // canonical table reference
        public bool resolved { get; set; }
        public int resolved_day { get; set; }
    }

    [Serializable]
    public sealed class AnomalyHazardSystemState
    {
        public string system_id { get; set; } = "anomaly_hazard";
        public int schema_version { get; set; } = 1;
        public int hazard_counter { get; set; }
        public int last_tick_day { get; set; }
        public List<AnomalyHazardInstance> hazards { get; set; } = new List<AnomalyHazardInstance>();
        public List<AnomalyLootSiteState> loot_sites { get; set; } = new List<AnomalyLootSiteState>();
    }

    /// <summary>Spawn outcome (domain result, no exceptions).</summary>
    public sealed class AnomalySpawnResult
    {
        public bool Success;
        public string ReasonCode = string.Empty;                    // unknown_anomaly | hazard_cap_reached | invalid_position
        public AnomalyHazardInstance? Hazard;

        public static AnomalySpawnResult Fail(string reason) => new AnomalySpawnResult { Success = false, ReasonCode = reason };
    }

    /// <summary>Loot-site resolution outcome. NEVER carries items — the canonical
    /// loot table referenced by <see cref="AnomalyLootSiteState.loot_table_id"/>
    /// is consumed through the existing expedition/scavenging loot authority.</summary>
    public sealed class AnomalyLootResolution
    {
        public bool Success;
        public string ReasonCode = string.Empty;                   // unknown_loot_site | loot_already_resolved | hazard_expired
        public string HazardId = string.Empty;
        public string LootTableId = string.Empty;
    }

    /// <summary>Typed storm-approach warning payload (§4.12).</summary>
    public sealed class AnomalyApproachWarning
    {
        public string HazardId = string.Empty;
        public string AnomalyId = string.Empty;
        public string TargetId = string.Empty;                     // usually the shelter zone id
        public float DistanceKm;
        public float EtaDays;                                      // distance / daily advance
        public string DirectionCardinal = string.Empty;            // hazard travel bearing as cardinal text
        public string IntensityBand = string.Empty;                // light | moderate | severe
        public float Confidence;                                   // authored from warning profile
    }

    /// <summary>
    /// Deterministic moving-hazard authority for authored anomaly zones and
    /// radiation storm fronts. One canonical layer — alongside (not duplicating)
    /// <see cref="FalloutSystem"/> — that adds authored special zones and reward
    /// context on top of the shared weather/radiation truth.
    /// </summary>
    public sealed class AnomalyHazardSystem
    {
        public const string SystemId = "anomaly_hazard";

        /// <summary>Concurrent-hazard cap (§4.5): prevents infinite accumulation.</summary>
        public const int MaxConcurrentHazards = 4;

        /// <summary>Overlap rule (§4.6): radiation rates are ADDITIVE across overlapping
        /// hazards up to this cap; non-radiation environmental effect tags remain a
        /// separate union and are never capped away.</summary>
        public const float MaxCombinedRadiationRate = 600f;

        /// <summary>Authored world bounds (km); movement clamps to them (§4.4).</summary>
        public const float WorldMinKm = 0f;
        public const float WorldMaxKm = 512f;

        /// <summary>Storm-front bearing wander per tick, seeded by the host's day fork (§4.4).</summary>
        public const float WanderMaxDegrees = 15f;

        public const float ExpireIntensityFloor = 0.05f;

        public const float ConfidenceEarly = 0.9f;
        public const float ConfidenceStandard = 0.7f;
        public const float ConfidenceLate = 0.5f;
        public const float ConfidenceSignature = 0.4f;
        public const float ConfidenceContact = 1.0f;

        /// <summary>Plan 176 §4.8 — authored device capabilities (rads/hr each
        /// canonical device class can resolve). The host maps inventory items
        /// onto these; Core owns the threshold comparison.</summary>
        public const float GeigerCapabilityRadsPerHour = 60f;
        public const float DosimeterCapabilityRadsPerHour = 15f;

        private AnomalyDefinitionCatalog _catalog = new AnomalyDefinitionCatalog();
        private AnomalyHazardSystemState _state = new AnomalyHazardSystemState();

        public event Action<AnomalyHazardInstance>? OnHazardSpawned;
        public event Action<AnomalyHazardInstance>? OnHazardExpired;
        public event Action<AnomalyApproachWarning>? OnStormApproaching;
        public event Action<AnomalyHazardInstance>? OnHazardContact; // hazard began overlapping a target

        public AnomalyHazardSystemState State => _state;
        public AnomalyDefinitionCatalog Catalog => _catalog;

        public AnomalyHazardSystem(AnomalyDefinitionCatalog? catalog = null)
        {
            if (catalog != null) _catalog = catalog;
        }

        // ── Catalog ─────────────────────────────────────────────────────

        public void BindCatalog(AnomalyDefinitionCatalog catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        public AnomalyDefinition? Definition(string anomalyId) => _catalog.Find(anomalyId);

        // ── Spawn (§4.5) ────────────────────────────────────────────────

        /// <summary>
        /// Spawn one active hazard instance from an authored definition. The HOST
        /// owns the spawn decision (world seed, campaign day, region tags, prior
        /// strike history) and passes a forked RNG when it rolls a spawn; Core
        /// only validates, enforces the cap, and stamps deterministic identity.
        /// </summary>
        public AnomalySpawnResult TrySpawn(string anomalyId, float x, float y, int day,
            float bearingDeg = -1f)
        {
            var def = _catalog.Find(anomalyId);
            if (def == null) return AnomalySpawnResult.Fail("unknown_anomaly");
            if (x < WorldMinKm || x > WorldMaxKm || y < WorldMinKm || y > WorldMaxKm || float.IsNaN(x) || float.IsNaN(y))
                return AnomalySpawnResult.Fail("invalid_position");

            int activeCount = 0;
            foreach (var h in _state.hazards)
                if (h != null && h.active) activeCount++;
            if (activeCount >= MaxConcurrentHazards) return AnomalySpawnResult.Fail("hazard_cap_reached");

            _state.hazard_counter++;
            var hazard = new AnomalyHazardInstance
            {
                hazard_id = $"hazard_anomaly_{_state.hazard_counter}_{def.anomaly_id}",
                anomaly_id = def.anomaly_id,
                position_x = Quantize(x),
                position_y = Quantize(y),
                bearing_deg = NormalizeBearing(bearingDeg < 0f ? 0f : bearingDeg),
                radius_km = def.radius_km,
                radiation_rate = def.radiation_rate,
                movement_profile = def.movement_profile,
                movement_speed_kph = def.movement_speed_kph,
                wind_response = def.wind_response,
                intensity = 1f,
                age_days = 0,
                duration_days = def.duration_days,
                spawn_day = day,
                warning_profile = def.warning_profile,
                warning_radius_km = def.warning_radius_km,
                detection_threshold = def.detection_threshold,
                loot_table_id = def.loot_table_id,
                wildlife_modifier_bp = def.wildlife_modifier_bp,
                active = true
            };
            _state.hazards.Add(hazard);

            // Gated loot sites are registered at spawn from the authored count.
            for (int i = 1; i <= def.loot_site_count; i++)
            {
                _state.loot_sites.Add(new AnomalyLootSiteState
                {
                    site_id = $"{hazard.hazard_id}_site_{i}",
                    hazard_id = hazard.hazard_id,
                    loot_table_id = def.loot_table_id,
                    resolved = false
                });
            }

            OnHazardSpawned?.Invoke(hazard);
            return new AnomalySpawnResult { Success = true, ReasonCode = "spawned", Hazard = hazard };
        }

        // ── Daily tick: movement + aging (§4.4) ────────────────────────

        /// <summary>
        /// Advance all hazards one campaign day. Movement is deterministic:
        /// storm fronts travel their (wandered) bearing; wind_drift hazards ride
        /// the weather wind vector scaled by their wind_response. Positions are
        /// clamped to world bounds and quantized. <paramref name="variationRng"/>
        /// is the host's day-keyed fork (only consumed by storm-front wander);
        /// passing null removes wander — never rerolls anything.
        /// </summary>
        public void TickDay(int day, float windDirDeg, float windSpeedKph, ISeededRng? variationRng)
        {
            // Stable ordering by hazard id — never dictionary order (§1.4).
            var ordered = new List<AnomalyHazardInstance>(_state.hazards);
            ordered.Sort((a, b) => string.CompareOrdinal(a.hazard_id, b.hazard_id));

            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                if (h == null || !h.active) continue;

                switch (h.movement_profile)
                {
                    case "storm_front":
                    {
                        double wander = variationRng != null
                            ? (variationRng.NextDouble() * 2.0 - 1.0) * WanderMaxDegrees
                            : 0.0;
                        h.bearing_deg = NormalizeBearing(h.bearing_deg + (float)wander);
                        Advance(h, h.bearing_deg, h.movement_speed_kph * 24f);
                        break;
                    }
                    case "wind_drift":
                        float driftKm = Math.Max(0f, windSpeedKph) * Math.Max(0f, h.wind_response) * 24f;
                        Advance(h, windDirDeg, driftKm);
                        break;
                    default:
                        break; // static
                }

                h.age_days++;
                h.intensity = Math.Max(ExpireIntensityFloor, 1f - (float)h.age_days / Math.Max(1, h.duration_days));
                if (h.age_days >= h.duration_days)
                {
                    h.active = false;
                    OnHazardExpired?.Invoke(h);
                }
            }

            _state.last_tick_day = day;
        }

        private static void Advance(AnomalyHazardInstance h, float dirDeg, float distanceKm)
        {
            if (distanceKm <= 0f) return;
            // Same convention as FalloutSystem.CalculateWindDispersal:
            // 0° = North (+Y), 90° = East (+X).
            double rad = (90.0 - dirDeg) * (Math.PI / 180.0);
            float dx = (float)(Math.Cos(rad) * distanceKm);
            float dy = (float)(Math.Sin(rad) * distanceKm);
            h.position_x = Quantize(Math.Clamp(h.position_x + dx, WorldMinKm, WorldMaxKm));
            h.position_y = Quantize(Math.Clamp(h.position_y + dy, WorldMinKm, WorldMaxKm));
        }

        private static float Quantize(float v) => (float)Math.Round(v * 100f) / 100f;
        private static float NormalizeBearing(float deg)
        {
            float b = deg % 360f;
            if (b < 0f) b += 360f;
            return (float)Math.Round(b * 10f) / 10f;
        }

        // ── Queries (§4.6, §4.7, §4.11) ────────────────────────────────

        /// <summary>
        /// Additive radiation rate at a point from all active hazards, linear
        /// falloff inside each radius, scaled by hazard intensity, CAPPED at
        /// <see cref="MaxCombinedRadiationRate"/>. Dose ownership stays with
        /// RadiationSystem — this value is only ever consumed, never self-applied.
        /// </summary>
        public float GetRadiationRate(float x, float y)
        {
            float total = 0f;
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                float dist = Distance(h.position_x, h.position_y, x, y);
                if (dist > h.radius_km) continue;
                float falloff = 1f - dist / Math.Max(0.01f, h.radius_km);
                total += h.radiation_rate * falloff * h.intensity;
            }
            return Math.Min(total, MaxCombinedRadiationRate);
        }

        /// <summary>Union of environmental effect tags of hazards covering the point (§4.6).</summary>
        public List<string> GetEnvironmentalEffectTags(float x, float y)
        {
            var tags = new List<string>();
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                if (Distance(h.position_x, h.position_y, x, y) > h.radius_km) continue;
                var def = _catalog.Find(h.anomaly_id);
                if (def?.environmental_effect_tags == null) continue;
                for (int t = 0; t < def.environmental_effect_tags.Count; t++)
                {
                    string tag = def.environmental_effect_tags[t];
                    if (!string.IsNullOrEmpty(tag) && !tags.Contains(tag)) tags.Add(tag);
                }
            }
            tags.Sort(StringComparer.Ordinal);
            return tags;
        }

        /// <summary>Bounded wildlife modifier at a point (§4.11): sum of overlapping
        /// authored modifiers, clamped to [-1, 1]. Positive = attraction;
        /// negative = avoidance. Ecology consumes this; it owns the behavior.</summary>
        public float GetWildlifeModifier(float x, float y)
        {
            float sumBp = 0f;
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                if (Distance(h.position_x, h.position_y, x, y) <= h.radius_km)
                    sumBp += h.wildlife_modifier_bp;
            }
            return Math.Clamp(sumBp / 1000f, -1f, 1f);
        }

        public IReadOnlyList<AnomalyHazardInstance> GetOverlappingHazardIds(float x, float y)
        {
            var list = new List<AnomalyHazardInstance>();
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                if (Distance(h.position_x, h.position_y, x, y) <= h.radius_km) list.Add(h);
            }
            return list;
        }

        // ── Detection (§4.8) ───────────────────────────────────────────

        /// <summary>
        /// Detection classification at a point given the operating detector
        /// capability (rads/hr the device can resolve; 0 = no working device).
        /// No powered device → reduced information, never hidden truth: hazards
        /// inside their radius are always Signature (felt directly), and the
        /// radiation RATE is still consumable by RadiationSystem regardless.
        /// </summary>
        public HazardDetection ClassifyDetection(float x, float y, float detectorCapability)
        {
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                float dist = Distance(h.position_x, h.position_y, x, y);
                if (dist > h.warning_radius_km) continue;
                if (detectorCapability >= h.detection_threshold) return HazardDetection.Classified;
                // Warning band with an under-powered device: presence is known
                // (visual band, sound, fallout signs) but unidentified — reduced
                // information, never hidden truth.
                return HazardDetection.Signature;
            }
            return HazardDetection.Undetected;
        }

        public float GetDetectionConfidence(float x, float y, float detectorCapability)
        {
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                float dist = Distance(h.position_x, h.position_y, x, y);
                if (dist > h.warning_radius_km) continue;
                if (dist <= h.radius_km) return ConfidenceContact;
                if (detectorCapability >= h.detection_threshold) return ConfidenceContact * 0.95f;
                return ConfidenceSignature;
            }
            return 0f;
        }

        // ── Approach warnings (§4.12) ──────────────────────────────────

        /// <summary>
        /// Evaluate storm-approach warnings toward one target (the shelter).
        /// Moving hazards inside their effective warning band fire a typed
        /// warning ONCE per (hazard, target) episode; static zones never fire
        /// approach warnings (they are fixed terrain features, not approaching).
        /// Does NOT force lockdown — canonical emergency policy stays authoritative.
        /// </summary>
        public void EvaluateApproach(string targetId, float targetX, float targetY)
        {
            var ordered = OrderedActive();
            for (int i = 0; i < ordered.Count; i++)
            {
                var h = ordered[i];
                if (h.movement_profile == "static") continue;

                float dist = Distance(h.position_x, h.position_y, targetX, targetY);
                float triggerFraction = h.warning_profile switch
                {
                    "early" => 1.0f,
                    "late" => 0.6f,
                    _ => 0.85f
                };
                float triggerRadius = h.warning_radius_km * triggerFraction;
                if (dist > triggerRadius) continue;

                string key = $"{h.hazard_id}|{targetId}";
                if (h.fired_warning_keys.Contains(key)) continue;
                h.fired_warning_keys.Add(key);

                // ETA from remaining distance and this hazard's daily advance.
                float dailyAdvance = h.movement_profile == "storm_front"
                    ? h.movement_speed_kph * 24f
                    : Math.Max(0f, WindDailyAdvanceHintKm); // set by host before evaluate when drift-based
                float eta = dailyAdvance > 0.01f ? dist / dailyAdvance : 0f;

                var warning = new AnomalyApproachWarning
                {
                    HazardId = h.hazard_id,
                    AnomalyId = h.anomaly_id,
                    TargetId = targetId,
                    DistanceKm = dist,
                    EtaDays = eta,
                    DirectionCardinal = Cardinal(h.bearing_deg),
                    IntensityBand = h.radiation_rate >= 100f ? "severe" : h.radiation_rate >= 40f ? "moderate" : "light",
                    Confidence = h.warning_profile switch
                    {
                        "early" => ConfidenceEarly,
                        "late" => ConfidenceLate,
                        _ => ConfidenceStandard
                    }
                };
                OnStormApproaching?.Invoke(warning);
            }
        }

        /// <summary>Host-set hint: recent daily wind advance (km) so drift-hazard
        /// ETA is honest. Storm fronts use their own authored speed.</summary>
        public float WindDailyAdvanceHintKm { get; set; }

        private static string Cardinal(float deg)
        {
            int oct = (int)Math.Floor(((deg % 360f) + 22.5f) / 45f) % 8;
            return oct switch
            {
                0 => "N", 1 => "NE", 2 => "E", 3 => "SE",
                4 => "S", 5 => "SW", 6 => "W", _ => "NW"
            };
        }

        // ── Loot gating (§4.9) ─────────────────────────────────────────

        /// <summary>
        /// Resolve a gated anomaly loot site exactly once. Resolution only flips
        /// the site state and names the canonical table; the CALLER loots through
        /// the existing loot-table authority. A second resolve fails with
        /// <c>loot_already_resolved</c> — no duplicate loot after restore (§1.7).
        /// </summary>
        public AnomalyLootResolution TryResolveLootSite(string siteId, int day)
        {
            var site = _state.loot_sites.Find(s =>
                s != null && string.Equals(s.site_id, siteId, StringComparison.Ordinal));
            if (site == null)
                return new AnomalyLootResolution { Success = false, ReasonCode = "unknown_loot_site" };
            if (site.resolved)
                return new AnomalyLootResolution
                {
                    Success = false, ReasonCode = "loot_already_resolved",
                    HazardId = site.hazard_id, LootTableId = site.loot_table_id
                };

            site.resolved = true;
            site.resolved_day = day;
            return new AnomalyLootResolution
            {
                Success = true, ReasonCode = "resolved",
                HazardId = site.hazard_id, LootTableId = site.loot_table_id
            };
        }

        public IReadOnlyList<AnomalyLootSiteState> LootSites => _state.loot_sites;

        // ── Save (§4.14) ───────────────────────────────────────────────

        public AnomalyHazardSystemState CaptureState()
        {
            var copy = new AnomalyHazardSystemState
            {
                hazard_counter = _state.hazard_counter,
                last_tick_day = _state.last_tick_day,
                hazards = new List<AnomalyHazardInstance>(_state.hazards.Count),
                loot_sites = new List<AnomalyLootSiteState>(_state.loot_sites.Count)
            };
            foreach (var h in _state.hazards)
            {
                if (h == null) continue;
                copy.hazards.Add(new AnomalyHazardInstance
                {
                    hazard_id = h.hazard_id, anomaly_id = h.anomaly_id,
                    position_x = h.position_x, position_y = h.position_y,
                    bearing_deg = h.bearing_deg, radius_km = h.radius_km,
                    radiation_rate = h.radiation_rate,
                    movement_profile = h.movement_profile,
                    movement_speed_kph = h.movement_speed_kph,
                    wind_response = h.wind_response, intensity = h.intensity,
                    age_days = h.age_days, duration_days = h.duration_days,
                    spawn_day = h.spawn_day, warning_profile = h.warning_profile,
                    warning_radius_km = h.warning_radius_km,
                    detection_threshold = h.detection_threshold,
                    loot_table_id = h.loot_table_id,
                    wildlife_modifier_bp = h.wildlife_modifier_bp,
                    active = h.active,
                    fired_warning_keys = new List<string>(h.fired_warning_keys)
                });
            }
            foreach (var s in _state.loot_sites)
            {
                if (s == null) continue;
                copy.loot_sites.Add(new AnomalyLootSiteState
                {
                    site_id = s.site_id, hazard_id = s.hazard_id,
                    loot_table_id = s.loot_table_id, resolved = s.resolved,
                    resolved_day = s.resolved_day
                });
            }
            return copy;
        }

        public void RestoreState(AnomalyHazardSystemState? state)
        {
            if (state == null) return;
            _state = new AnomalyHazardSystemState
            {
                hazard_counter = state.hazard_counter,
                last_tick_day = state.last_tick_day,
                hazards = new List<AnomalyHazardInstance>(state.hazards?.Count ?? 0),
                loot_sites = new List<AnomalyLootSiteState>(state.loot_sites?.Count ?? 0)
            };
            if (state.hazards != null)
                foreach (var h in state.hazards)
                    if (h != null)
                        _state.hazards.Add(new AnomalyHazardInstance
                        {
                            hazard_id = h.hazard_id, anomaly_id = h.anomaly_id,
                            position_x = h.position_x, position_y = h.position_y,
                            bearing_deg = h.bearing_deg, radius_km = h.radius_km,
                            radiation_rate = h.radiation_rate,
                            movement_profile = h.movement_profile,
                            movement_speed_kph = h.movement_speed_kph,
                            wind_response = h.wind_response, intensity = h.intensity,
                            age_days = h.age_days, duration_days = h.duration_days,
                            spawn_day = h.spawn_day, warning_profile = h.warning_profile,
                            warning_radius_km = h.warning_radius_km,
                            detection_threshold = h.detection_threshold,
                            loot_table_id = h.loot_table_id,
                            wildlife_modifier_bp = h.wildlife_modifier_bp,
                            active = h.active,
                            fired_warning_keys = new List<string>(h.fired_warning_keys ?? new List<string>())
                        });
            if (state.loot_sites != null)
                foreach (var s in state.loot_sites)
                    if (s != null)
                        _state.loot_sites.Add(new AnomalyLootSiteState
                        {
                            site_id = s.site_id, hazard_id = s.hazard_id,
                            loot_table_id = s.loot_table_id, resolved = s.resolved,
                            resolved_day = s.resolved_day
                        });
        }

        // ── Internals ──────────────────────────────────────────────────

        private List<AnomalyHazardInstance> OrderedActive()
        {
            var list = new List<AnomalyHazardInstance>();
            foreach (var h in _state.hazards)
                if (h != null && h.active) list.Add(h);
            list.Sort((a, b) => string.CompareOrdinal(a.hazard_id, b.hazard_id));
            return list;
        }

        private static float Distance(float ax, float ay, float bx, float by)
        {
            float dx = ax - bx, dy = ay - by;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
