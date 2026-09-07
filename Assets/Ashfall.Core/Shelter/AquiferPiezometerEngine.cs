// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 115 — Wasteland Underground Water-Table Piezometer & Hydrogeology
// Network.
//
// AUTHORITY MAP (Flagship Plans 114-117, Wave 2):
//   * WaterTreatmentSystem (Ashfall.Core) remains the ONLY water-stock and
//     treatment authority. This engine NEVER grants, moves or purifies
//     water. It produces intelligence: drawdown forecasts, contamination
//     forecasts, monitoring confidence and isolation state.
//   * The host (or a test bridge) forwards advisories into
//     WaterTreatmentSystem.RegisterContaminationAdvisory, which stays
//     decoupled from these DTOs on purpose (primitive seam).
//   * No wall clock: campaign day comes from the host DayProvider.
//   * No System.Random: all stochastic outcomes flow through ISeededRng.
//   * Bounded history only: per-node trend windows are capped; no
//     unbounded time-series bloats saves.
//   * Real-world drilling/borehole engineering is deliberately abstracted
//     to normalized game ratings (classes and 0..1 indices).
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Shelter
{
    // ─────────────────────────────────────────────────────────────────
    // Catalog DTOs (data authority: Assets/StreamingAssets/Data/
    // piezometer_network_catalog.json)
    // ─────────────────────────────────────────────────────────────────

    /// <summary>One monitored geological stratum — a piezometer zone.</summary>
    [Serializable]
    public sealed class PiezometerStrataDef
    {
        public string strata_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        /// <summary>negligible | low | moderate | high</summary>
        public string baseline_recharge_class { get; set; } = "moderate";
        /// <summary>small | moderate | large</summary>
        public string storage_capacity_class { get; set; } = "moderate";
        /// <summary>0..1 — how easily fracture events open contamination paths.</summary>
        public float fracture_susceptibility { get; set; } = 0.3f;
        /// <summary>slow | moderate | fast — abstract contaminant transport speed.</summary>
        public string contamination_transport_class { get; set; } = "moderate";
        /// <summary>Construction cost per sensor node installed in this stratum.</summary>
        public Dictionary<string, int> sensor_install_cost { get; set; } = new Dictionary<string, int>();
        /// <summary>0..1 — best-case reading confidence for a healthy node.</summary>
        public float monitoring_accuracy { get; set; } = 0.7f;
        public string maintenance_profile_id { get; set; } = string.Empty;
        /// <summary>Abstract water sources (wells/intakes) this stratum feeds.</summary>
        public List<string> monitored_source_ids { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>Catalog-level maintenance profile referenced by strata.</summary>
    [Serializable]
    public sealed class PiezometerMaintenanceProfile
    {
        public string maintenance_profile_id { get; set; } = string.Empty;
        public Dictionary<string, int> required_items { get; set; } = new Dictionary<string, int>();
        /// <summary>Clogging accumulation per day at 0..1 scale, before modifiers.</summary>
        public float fouling_per_day { get; set; } = 0.01f;
        /// <summary>Calibration drift per day at 0..1 scale, before modifiers.</summary>
        public float calibration_drift_per_day { get; set; } = 0.008f;
    }

    /// <summary>Data-driven forecast tuning — no hard-coded real-world claims.</summary>
    [Serializable]
    public sealed class PiezometerForecastTuning
    {
        /// <summary>Contamination signal (0..1) at or above which a warning is issued.</summary>
        public float warning_signal_threshold { get; set; } = 0.45f;
        /// <summary>Drawdown head index (0..1 used = low) below which drawdown is critical.</summary>
        public float critical_head_index { get; set; } = 0.25f;
        /// <summary>Head index above which drawdown is considered stable.</summary>
        public float stable_head_index { get; set; } = 0.55f;
        /// <summary>Minimum monitoring confidence required before any warning is issued.</summary>
        public float minimum_warning_confidence { get; set; } = 0.3f;
        /// <summary>Daily demand (0..1 normalized) considered heavy pumping.</summary>
        public float heavy_demand_threshold { get; set; } = 0.7f;
    }

    [Serializable]
    public sealed class AquiferPiezometerCatalog
    {
        public int schema_version { get; set; } = 1;
        public string network_id { get; set; } = "aquifer_network_primary";
        public string display_name { get; set; } = "Deep Monitoring Network";
        /// <summary>Item consumed when installing the network.</summary>
        public string sensor_item_id { get; set; } = "item_groundwater_sensor";
        /// <summary>Item consumed per isolation action.</summary>
        public string isolation_module_item_id { get; set; } = "item_aquifer_isolation_module";
        /// <summary>Item consumed per node maintenance action.</summary>
        public string maintenance_kit_item_id { get; set; } = "item_well_maintenance_kit";
        public List<PiezometerStrataDef> strata { get; set; } = new List<PiezometerStrataDef>();
        public List<PiezometerMaintenanceProfile> maintenance_profiles { get; set; } = new List<PiezometerMaintenanceProfile>();
        public PiezometerForecastTuning forecast { get; set; } = new PiezometerForecastTuning();
        /// <summary>Bounded per-node trend window (readings kept in state).</summary>
        public int trend_window { get; set; } = 8;
        public List<string> tags { get; set; } = new List<string>();
    }

    // ─────────────────────────────────────────────────────────────────
    // State DTOs
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class PiezometerNodeState
    {
        public string node_id { get; set; } = string.Empty;
        public string strata_id { get; set; } = string.Empty;
        /// <summary>0..100 physical condition of the monitoring installation.</summary>
        public float condition { get; set; } = 100f;
        /// <summary>0..1 calibration quality; drifts daily, restored by maintenance.</summary>
        public float calibration { get; set; } = 1f;
        /// <summary>0..1 water-head abstraction (1 = full, 0 = dry).</summary>
        public float head_index { get; set; } = 0.8f;
        /// <summary>0..1 abstract water quality at the node.</summary>
        public float water_quality_index { get; set; } = 1f;
        /// <summary>0..1 rising contamination signal detected at the node.</summary>
        public float contamination_signal { get; set; } = 0f;
        /// <summary>0..1 fouling (silt/biological) accumulated.</summary>
        public float clogging_state { get; set; } = 0f;
        public bool online { get; set; } = true;
        public int last_read_day { get; set; }
        /// <summary>Bounded recent head readings (oldest first), capped at catalog trend window.</summary>
        public List<float> head_trend { get; set; } = new List<float>();
    }

    /// <summary>An active or resolved early-warning record. Persisted so a
    /// warning never re-rolls or re-fires after save/load.</summary>
    [Serializable]
    public sealed class AquiferWarningRecord
    {
        public string warning_id { get; set; } = string.Empty;
        public string strata_id { get; set; } = string.Empty;
        public int issued_day { get; set; }
        /// <summary>Forecast arrival window in campaign days (min/max, data-driven).</summary>
        public int arrival_window_min_days { get; set; }
        public int arrival_window_max_days { get; set; }
        public float confidence_at_issue { get; set; }
        public bool resolved { get; set; }
    }

    [Serializable]
    public sealed class HydrogeologyNetworkState
    {
        public int schema_version { get; set; } = 1;
        public string network_id { get; set; } = string.Empty;
        public bool constructed { get; set; }
        public List<PiezometerNodeState> nodes { get; set; } = new List<PiezometerNodeState>();
        /// <summary>0..100 aggregate aquifer health abstraction.</summary>
        public float aquifer_health { get; set; } = 80f;
        /// <summary>stable | declining | critical</summary>
        public string drawdown_state { get; set; } = "stable";
        /// <summary>0..1 aggregate contamination risk across monitored strata.</summary>
        public float contamination_risk { get; set; } = 0f;
        /// <summary>0..1 furthest contamination front progress across strata.</summary>
        public float contamination_front_progress { get; set; } = 0f;
        /// <summary>negligible | low | moderate | high — aggregate recharge state.</summary>
        public string recharge_state { get; set; } = "moderate";
        /// <summary>0..1 cumulative fracture state (events push it up, decay pulls down).</summary>
        public float fracture_state { get; set; } = 0f;
        /// <summary>0..1 aggregate confidence in network readings.</summary>
        public float monitoring_confidence { get; set; } = 0.6f;
        /// <summary>Strata zone ids excluded from intake by isolation actions.</summary>
        public List<string> active_isolation_zone_ids { get; set; } = new List<string>();
        public List<AquiferWarningRecord> warnings { get; set; } = new List<AquiferWarningRecord>();
        public int last_sample_day { get; set; }
        public int days_monitored { get; set; }
        public int next_warning_number { get; set; } = 1;
    }

    // ─────────────────────────────────────────────────────────────────
    // Result DTOs
    // ─────────────────────────────────────────────────────────────────

    /// <summary>Deterministic contamination forecast snapshot for one stratum.</summary>
    [Serializable]
    public sealed class AquiferContaminationForecast
    {
        /// <summary>none | elevated | severe</summary>
        public string risk_level { get; set; } = "none";
        /// <summary>Estimated arrival window in campaign days; -1 when unknown.</summary>
        public int estimated_arrival_window_min { get; set; } = -1;
        public int estimated_arrival_window_max { get; set; } = -1;
        public List<string> affected_source_ids { get; set; } = new List<string>();
        /// <summary>0..1 confidence derived from node health/calibration/accuracy.</summary>
        public float confidence { get; set; }
        /// <summary>e.g. isolate_zone, treat_intake, maintain_nodes</summary>
        public List<string> recommended_action_tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// The advisory bridge payload. Deliberately primitive: the water
    /// treatment authority consumes facts, not piezometer object graphs.
    /// </summary>
    [Serializable]
    public sealed class HydrogeologyAdvisory
    {
        public string advisory_id { get; set; } = string.Empty;
        public int day { get; set; }
        /// <summary>none | watch | warning</summary>
        public string advisory_level { get; set; } = "none";
        /// <summary>Drawdown: stable | declining | critical</summary>
        public string drawdown_state { get; set; } = "stable";
        public float monitoring_confidence { get; set; }
        public List<string> contaminated_source_ids { get; set; } = new List<string>();
        public List<string> isolated_zone_ids { get; set; } = new List<string>();
        public List<string> recommended_action_tags { get; set; } = new List<string>();
    }

    /// <summary>Typed failure codes for the Godot host to format.</summary>
    public static class AquiferPiezometerFailures
    {
        public const string NotConstructed = "piez.not_constructed";
        public const string AlreadyConstructed = "piez.already_constructed";
        public const string MaterialsMissing = "piez.materials_missing";
        public const string UnknownNode = "piez.unknown_node";
        public const string UnknownZone = "piez.unknown_zone";
        public const string ZoneAlreadyIsolated = "piez.zone_already_isolated";
        public const string ZoneNotIsolated = "piez.zone_not_isolated";
        public const string MaintenanceNotNeeded = "piez.maintenance_not_needed";
    }

    // ─────────────────────────────────────────────────────────────────
    // Engine
    // ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Plan 115 hydrogeology monitoring engine. Intelligence-only:
    /// forecasts, warnings, confidence and isolation state. Water stock,
    /// treatment chemistry and drinkability remain owned by
    /// <see cref="WaterTreatmentSystem"/>.
    /// </summary>
    public sealed class AquiferPiezometerEngine
    {
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private AquiferPiezometerCatalog _catalog = new AquiferPiezometerCatalog();
        private HydrogeologyNetworkState _state = new HydrogeologyNetworkState();

        // Inventory ports (host-bound; deterministic).
        private Func<string, int> _getCount = _ => 0;
        private Action<string, int> _consume = (_, _) => { };

        /// <summary>Host-injected campaign day provider (deterministic).</summary>
        public Func<int>? DayProvider { get; set; }
        /// <summary>Host-injected normalized pump demand 0..1 (host-owned water usage).</summary>
        public Func<float>? PumpDemandProvider { get; set; }
        /// <summary>Host-injected seasonal recharge modifier 0..1 (host-owned season).</summary>
        public Func<float>? SeasonalRechargeModifierProvider { get; set; }
        /// <summary>Hydrogeologist skill 0..1 — interpretation confidence + maintenance.</summary>
        public Func<float>? HydrogeologistSkillProvider { get; set; }
        /// <summary>Well-driller skill 0..1 — maintenance effectiveness, isolation success.</summary>
        public Func<float>? WellDrillerSkillProvider { get; set; }

        private int CurrentDay() => DayProvider?.Invoke() ?? 0;
        private float PumpDemand() => Math.Clamp(PumpDemandProvider?.Invoke() ?? 0.4f, 0f, 1f);
        private float SeasonalModifier() => Math.Clamp(SeasonalRechargeModifierProvider?.Invoke() ?? 0.5f, 0f, 1f);
        private float HydrogeologistSkill() => Math.Clamp(HydrogeologistSkillProvider?.Invoke() ?? 0f, 0f, 1f);
        private float WellDrillerSkill() => Math.Clamp(WellDrillerSkillProvider?.Invoke() ?? 0f, 0f, 1f);

        public HydrogeologyNetworkState State => _state;
        public AquiferPiezometerCatalog Catalog => _catalog;
        public bool IsConstructed => _state.constructed;

        /// <summary>Raised once per newly issued early warning (never re-fired on load).</summary>
        public event Action<AquiferWarningRecord>? OnWarningIssued;
        public event Action<string>? OnEventRaised;

        public AquiferPiezometerEngine(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(2115);
            _log = log ?? NullLog.Instance;
        }

        public void BindCatalog(AquiferPiezometerCatalog catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        public void BindInventory(Func<string, int> getCount, Action<string, int> consume)
        {
            _getCount = getCount ?? _getCount;
            _consume = consume ?? _consume;
        }

        private PiezometerMaintenanceProfile? FindProfile(string? profileId)
        {
            foreach (var p in _catalog.maintenance_profiles)
                if (p.maintenance_profile_id == profileId) return p;
            return _catalog.maintenance_profiles.Count > 0 ? _catalog.maintenance_profiles[0] : null;
        }

        private PiezometerStrataDef? FindStrata(string strataId)
        {
            foreach (var s in _catalog.strata)
                if (s.strata_id == strataId) return s;
            return null;
        }

        private static float TransportSpeedFactor(string transportClass) => transportClass switch
        {
            "slow" => 0.6f,
            "moderate" => 1.0f,
            "fast" => 1.6f,
            _ => 1.0f
        };

        private static float RechargeClassFactor(string rechargeClass) => rechargeClass switch
        {
            "negligible" => 0.15f,
            "low" => 0.5f,
            "moderate" => 1.0f,
            "high" => 1.5f,
            _ => 1.0f
        };

        private static float StorageClassFactor(string storageClass) => storageClass switch
        {
            "small" => 0.6f,
            "moderate" => 1.0f,
            "large" => 1.4f,
            _ => 1.0f
        };

        // ── Construction ────────────────────────────────────────────────

        /// <summary>
        /// Installs the monitoring network: one sensor node per catalog
        /// stratum. Old saves default to an unbuilt network (invariant 14).
        /// </summary>
        public ActionResult ConstructNetwork()
        {
            if (_state.constructed)
                return ActionResult.Blocked(AquiferPiezometerFailures.AlreadyConstructed, "piez.already_constructed");
            if (_catalog.strata.Count == 0)
                return ActionResult.Failed(AquiferPiezometerFailures.MaterialsMissing, "piez.no_strata");

            // Aggregate install cost across strata (sensors per zone).
            var totals = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var s in _catalog.strata)
                foreach (var cost in s.sensor_install_cost)
                    totals[cost.Key] = (totals.TryGetValue(cost.Key, out var v) ? v : 0) + cost.Value;

            foreach (var cost in totals)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(AquiferPiezometerFailures.MaterialsMissing, "piez.missing_install_material");
            }
            foreach (var cost in totals)
                _consume(cost.Key, cost.Value);

            _state = new HydrogeologyNetworkState
            {
                schema_version = 1,
                network_id = _catalog.network_id,
                constructed = true,
                last_sample_day = CurrentDay()
            };
            foreach (var s in _catalog.strata)
            {
                _state.nodes.Add(new PiezometerNodeState
                {
                    node_id = $"node_{s.strata_id}",
                    strata_id = s.strata_id,
                    last_read_day = CurrentDay()
                });
            }

            Raise("piez.network_constructed");
            _log.Info("[Piezometer] Hydrogeology monitoring network installed.");
            RecomputeConfidence();
            return ActionResult.Success("piez.network_constructed");
        }

        // ── Daily simulation ────────────────────────────────────────────

        /// <summary>
        /// Daily network update: head/drawdown, contamination front
        /// progression, fouling/calibration drift, confidence recompute.
        /// Daily/periodic cadence only — no per-frame simulation.
        /// </summary>
        public void TickDay(int day)
        {
            if (!_state.constructed) return;

            _state.days_monitored++;
            _state.last_sample_day = day;

            float demand = PumpDemand();
            float seasonal = SeasonalModifier();

            // ── Drawdown: demand vs recharge per node ──────────────────
            float worstHead = 1f;
            float headSum = 0f;
            foreach (var node in _state.nodes)
            {
                var strata = FindStrata(node.strata_id);
                float recharge = RechargeClassFactor(strata?.baseline_recharge_class ?? "moderate")
                                 * (0.5f + seasonal);
                float storage = StorageClassFactor(strata?.storage_capacity_class ?? "moderate");

                // Deterministic micro-variation through the seeded rng.
                float noise = (_rng.NextFloat() - 0.5f) * 0.02f;
                float headDelta = ((recharge * 0.05f) - (demand * 0.045f)) / Math.Max(0.2f, storage) + noise;
                node.head_index = Math.Clamp(node.head_index + headDelta, 0f, 1f);
                node.head_trend.Add(node.head_index);
                if (node.head_trend.Count > _catalog.trend_window)
                    node.head_trend.RemoveAt(0);

                node.last_read_day = day;
                worstHead = Math.Min(worstHead, node.head_index);
                headSum += node.head_index;
            }

            _state.drawdown_state = worstHead <= _catalog.forecast.critical_head_index
                ? "critical"
                : worstHead <= _catalog.forecast.stable_head_index ? "declining" : "stable";

            // Recharge state summary (aggregate, catalog classes + season).
            float aggRecharge = 0f;
            foreach (var s in _catalog.strata)
                aggRecharge += RechargeClassFactor(s.baseline_recharge_class);
            aggRecharge = _catalog.strata.Count > 0 ? aggRecharge / _catalog.strata.Count : 0f;
            aggRecharge *= (0.5f + seasonal);
            _state.recharge_state = aggRecharge switch
            {
                >= 1.3f => "high",
                >= 0.7f => "moderate",
                >= 0.3f => "low",
                _ => "negligible"
            };

            // ── Fracture state: decays slowly toward zero ──────────────
            _state.fracture_state = Math.Clamp(_state.fracture_state - 0.005f, 0f, 1f);

            // ── Contamination front progression ────────────────────────
            float worstSignal = 0f;
            float frontProgress = 0f;
            float qualitySum = 0f;
            foreach (var node in _state.nodes)
            {
                var strata = FindStrata(node.strata_id);
                float transport = TransportSpeedFactor(strata?.contamination_transport_class ?? "moderate");
                float fractureFactor = 1f + _state.fracture_state * (strata?.fracture_susceptibility ?? 0f);
                float pressure = _state.contamination_risk * transport * fractureFactor;
                // Deterministic micro-progression through the seeded rng.
                float step = Math.Max(0f, (0.004f + pressure * 0.01f) * (0.75f + _rng.NextFloat() * 0.5f));
                node.contamination_signal = Math.Clamp(node.contamination_signal + step, 0f, 1f);
                node.water_quality_index = Math.Clamp(1f - node.contamination_signal * 0.8f, 0f, 1f);

                worstSignal = Math.Max(worstSignal, node.contamination_signal);
                frontProgress = Math.Max(frontProgress, node.contamination_signal);
                qualitySum += node.water_quality_index;

                // ── Fouling / calibration drift / wear ─────────────────
                var profile = FindProfile(strata?.maintenance_profile_id);
                float skillCare = 1f - 0.3f * HydrogeologistSkill();
                node.clogging_state = Math.Clamp(node.clogging_state + (profile?.fouling_per_day ?? 0.01f) * skillCare, 0f, 1f);
                node.calibration = Math.Clamp(node.calibration - (profile?.calibration_drift_per_day ?? 0.008f) * skillCare, 0f, 1f);
                node.condition = Math.Clamp(node.condition - 0.05f, 0f, 100f);
                if (node.condition < 15f || node.clogging_state >= 0.95f)
                    node.online = false;
            }

            int nodeCount = Math.Max(1, _state.nodes.Count);
            _state.aquifer_health = Math.Clamp((headSum / nodeCount) * 60f + (qualitySum / nodeCount) * 40f, 0f, 100f);
            _state.contamination_risk = worstSignal;
            _state.contamination_front_progress = frontProgress;

            RecomputeConfidence();
            CheckEarlyWarnings(day);
        }

        /// <summary>
        /// Host-reported surface contamination event (fallout, severe blast
        /// fracture, contaminated surface water). Deterministic: same event
        /// history produces the same front acceleration.
        /// </summary>
        public void ReportSurfaceContaminationEvent(float severity01)
        {
            if (!_state.constructed) return;
            float severity = Math.Clamp(severity01, 0f, 1f);
            _state.contamination_risk = Math.Clamp(_state.contamination_risk + severity * 0.25f, 0f, 1f);
            foreach (var node in _state.nodes)
            {
                var strata = FindStrata(node.strata_id);
                float transport = TransportSpeedFactor(strata?.contamination_transport_class ?? "moderate");
                node.contamination_signal = Math.Clamp(node.contamination_signal + severity * 0.12f * transport, 0f, 1f);
            }
            Raise("piez.surface_contamination_event");
        }

        /// <summary>Host-reported severe ground fracture event (blast, quake).</summary>
        public void ReportFractureEvent(float severity01)
        {
            if (!_state.constructed) return;
            float severity = Math.Clamp(severity01, 0f, 1f);
            _state.fracture_state = Math.Clamp(_state.fracture_state + severity * 0.3f, 0f, 1f);
            Raise("piez.fracture_event");
        }

        // ── Forecasts & advisories ──────────────────────────────────────

        private float NodeConfidence(PiezometerNodeState node)
        {
            var strata = FindStrata(node.strata_id);
            float accuracy = strata?.monitoring_accuracy ?? 0.5f;
            float health = Math.Clamp(node.condition / 100f, 0f, 1f);
            float clean = 1f - node.clogging_state;
            float cal = node.calibration;
            float online = node.online ? 1f : 0.2f;
            // Specialist interpretation applied exactly once, here.
            float skill = 1f + 0.15f * HydrogeologistSkill();
            return Math.Clamp(accuracy * (0.4f + 0.3f * health + 0.15f * clean + 0.15f * cal) * online * skill, 0f, 1f);
        }

        private void RecomputeConfidence()
        {
            if (_state.nodes.Count == 0)
            {
                _state.monitoring_confidence = 0f;
                return;
            }
            float sum = 0f;
            foreach (var n in _state.nodes) sum += NodeConfidence(n);
            _state.monitoring_confidence = Math.Clamp(sum / _state.nodes.Count, 0f, 1f);
        }

        /// <summary>Deterministic forecast snapshot for a stratum (or aggregate when null).</summary>
        public AquiferContaminationForecast ForecastContamination(string? strataId = null)
        {
            var affected = new List<string>();
            float signal = 0f;
            float confidence = 0f;
            int sampled = 0;

            foreach (var node in _state.nodes)
            {
                if (strataId != null && node.strata_id != strataId) continue;
                var strata = FindStrata(node.strata_id);
                bool zoneIsolated = strata != null
                    && _state.active_isolation_zone_ids.Contains(IsolationZoneId(node.strata_id));
                if (strata != null && !zoneIsolated)
                {
                    foreach (var src in strata.monitored_source_ids)
                        if (!affected.Contains(src)) affected.Add(src);
                }
                signal = Math.Max(signal, node.contamination_signal);
                confidence += NodeConfidence(node);
                sampled++;
            }
            confidence = sampled > 0 ? confidence / sampled : 0f;

            var forecast = new AquiferContaminationForecast
            {
                confidence = confidence,
                affected_source_ids = affected
            };

            var tuning = _catalog.forecast;
            if (signal < tuning.warning_signal_threshold || confidence < tuning.minimum_warning_confidence)
            {
                forecast.risk_level = signal < tuning.warning_signal_threshold * 0.5f ? "none" : "elevated";
                forecast.recommended_action_tags.Add("maintain_nodes");
                return forecast;
            }

            forecast.risk_level = signal >= tuning.warning_signal_threshold * 1.5f ? "severe" : "elevated";

            // Data-driven arrival window: remaining distance over abstract front speed.
            float remaining = Math.Max(0.02f, 1f - signal);
            var slowest = 10;
            var fastest = 4;
            foreach (var node in _state.nodes)
            {
                if (strataId != null && node.strata_id != strataId) continue;
                var strata = FindStrata(node.strata_id);
                float transport = TransportSpeedFactor(strata?.contamination_transport_class ?? "moderate");
                int max = (int)MathF.Ceiling(remaining * 20f / transport);
                int min = (int)MathF.Ceiling(remaining * 12f / transport);
                slowest = Math.Min(slowest, max);
                fastest = Math.Min(fastest, min);
            }
            if (sampled > 0)
            {
                forecast.estimated_arrival_window_min = fastest;
                forecast.estimated_arrival_window_max = slowest;
            }
            forecast.recommended_action_tags.Add("isolate_zone");
            forecast.recommended_action_tags.Add("treat_intake");
            return forecast;
        }

        /// <summary>
        /// Builds the current advisory payload for the water authority.
        /// Pure projection of committed state — safe to call repeatedly.
        /// </summary>
        public HydrogeologyAdvisory BuildAdvisory()
        {
            var forecast = ForecastContamination();
            var level = "none";
            if (_state.contamination_risk >= _catalog.forecast.warning_signal_threshold * 1.5f) level = "warning";
            else if (_state.contamination_risk >= _catalog.forecast.warning_signal_threshold * 0.5f) level = "watch";

            var advisory = new HydrogeologyAdvisory
            {
                advisory_id = $"advisory_{_state.network_id}_{CurrentDay()}",
                day = CurrentDay(),
                advisory_level = level,
                drawdown_state = _state.drawdown_state,
                monitoring_confidence = _state.monitoring_confidence,
                contaminated_source_ids = forecast.risk_level == "none"
                    ? new List<string>()
                    : forecast.affected_source_ids,
                isolated_zone_ids = new List<string>(_state.active_isolation_zone_ids),
                recommended_action_tags = forecast.recommended_action_tags
            };
            if (_state.drawdown_state != "stable" && !advisory.recommended_action_tags.Contains("reduce_pumping"))
                advisory.recommended_action_tags.Add("reduce_pumping");
            return advisory;
        }

        private void CheckEarlyWarnings(int day)
        {
            var tuning = _catalog.forecast;
            foreach (var node in _state.nodes)
            {
                if (!node.online) continue;
                if (node.contamination_signal < tuning.warning_signal_threshold) continue;
                if (_state.monitoring_confidence < tuning.minimum_warning_confidence) continue;

                string warningId = $"warn_{node.strata_id}_{_state.next_warning_number}";
                bool alreadyIssued = _state.warnings.Exists(w => w.strata_id == node.strata_id && !w.resolved);
                if (alreadyIssued) continue;

                var strata = FindStrata(node.strata_id);
                float transport = TransportSpeedFactor(strata?.contamination_transport_class ?? "moderate");
                float remaining = Math.Max(0.02f, 1f - node.contamination_signal);
                var record = new AquiferWarningRecord
                {
                    warning_id = warningId,
                    strata_id = node.strata_id,
                    issued_day = day,
                    arrival_window_min_days = (int)MathF.Ceiling(remaining * 12f / transport),
                    arrival_window_max_days = (int)MathF.Ceiling(remaining * 20f / transport),
                    confidence_at_issue = _state.monitoring_confidence
                };
                _state.next_warning_number++;
                _state.warnings.Add(record);
                OnWarningIssued?.Invoke(record);
                Raise("piez.warning_issued");
                _log.Warn($"[Piezometer] Contamination warning for {node.strata_id} (confidence {_state.monitoring_confidence:P0}).");
            }
        }

        // ── Isolation ───────────────────────────────────────────────────

        private static string IsolationZoneId(string strataId) => strataId;

        /// <summary>All source ids not excluded by an active isolation.</summary>
        public List<string> AvailableSourceIds()
        {
            var available = new List<string>();
            foreach (var strata in _catalog.strata)
            {
                if (_state.active_isolation_zone_ids.Contains(IsolationZoneId(strata.strata_id))) continue;
                foreach (var src in strata.monitored_source_ids)
                    if (!available.Contains(src)) available.Add(src);
            }
            return available;
        }

        public bool IsSourceIsolated(string sourceId)
        {
            foreach (var strata in _catalog.strata)
            {
                if (!strata.monitored_source_ids.Contains(sourceId)) continue;
                if (_state.active_isolation_zone_ids.Contains(IsolationZoneId(strata.strata_id)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Abstract facility isolation of one stratum zone: compatible
        /// equipment + specialist check + resource cost. Excludes the
        /// zone's sources from intake advisories (the water authority
        /// still owns any supply consequence).
        /// </summary>
        public ActionResult IsolateAquiferZone(string zoneId)
        {
            if (!_state.constructed)
                return ActionResult.Failed(AquiferPiezometerFailures.NotConstructed, "piez.not_constructed");
            var strata = FindStrata(zoneId);
            if (strata == null)
                return ActionResult.Failed(AquiferPiezometerFailures.UnknownZone, "piez.unknown_zone");
            string zone = IsolationZoneId(zoneId);
            if (_state.active_isolation_zone_ids.Contains(zone))
                return ActionResult.Blocked(AquiferPiezometerFailures.ZoneAlreadyIsolated, "piez.zone_already_isolated");

            if (_getCount(_catalog.isolation_module_item_id) < 1)
                return ActionResult.Blocked(AquiferPiezometerFailures.MaterialsMissing, "piez.missing_isolation_module");

            // Driller skill gates success deterministically (no rng reroll loops).
            if (WellDrillerSkill() < 0.25f)
                return ActionResult.Blocked(AquiferPiezometerFailures.MaterialsMissing, "piez.no_specialist");

            _consume(_catalog.isolation_module_item_id, 1);
            _state.active_isolation_zone_ids.Add(zone);

            // Resolve any open warning for the zone — the threat is contained.
            foreach (var w in _state.warnings)
                if (w.strata_id == zoneId && !w.resolved) w.resolved = true;

            Raise("piez.zone_isolated");
            return ActionResult.Success("piez.zone_isolated");
        }

        /// <summary>Reconnects an isolated zone (module is not refunded).</summary>
        public ActionResult ReconnectAquiferZone(string zoneId)
        {
            string zone = IsolationZoneId(zoneId);
            if (!_state.active_isolation_zone_ids.Contains(zone))
                return ActionResult.Blocked(AquiferPiezometerFailures.ZoneNotIsolated, "piez.zone_not_isolated");
            _state.active_isolation_zone_ids.Remove(zone);
            Raise("piez.zone_reconnected");
            return ActionResult.Success("piez.zone_reconnected");
        }

        // ── Maintenance ─────────────────────────────────────────────────

        /// <summary>Services one node: clears fouling, restores calibration and condition.</summary>
        public ActionResult MaintainNode(string nodeId)
        {
            if (!_state.constructed)
                return ActionResult.Failed(AquiferPiezometerFailures.NotConstructed, "piez.not_constructed");
            var node = _state.nodes.Find(n => n.node_id == nodeId);
            if (node == null)
                return ActionResult.Failed(AquiferPiezometerFailures.UnknownNode, "piez.unknown_node");

            var strata = FindStrata(node.strata_id);
            var profile = FindProfile(strata?.maintenance_profile_id);
            var costs = profile?.required_items ?? new Dictionary<string, int>();
            foreach (var cost in costs)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(AquiferPiezometerFailures.MaterialsMissing, "piez.missing_maintenance_material");
            }
            foreach (var cost in costs)
                _consume(cost.Key, cost.Value);

            float driller = WellDrillerSkill();
            node.clogging_state = Math.Max(0f, node.clogging_state - (0.8f + 0.2f * driller));
            node.calibration = Math.Min(1f, node.calibration + 0.5f + 0.3f * driller);
            node.condition = Math.Min(100f, node.condition + 40f + 20f * driller);
            node.online = node.condition >= 15f;
            RecomputeConfidence();
            Raise("piez.node_maintained");
            return ActionResult.Success("piez.node_maintained");
        }

        // ── Persistence ─────────────────────────────────────────────────

        public HydrogeologyNetworkState CaptureState()
        {
            var clone = new HydrogeologyNetworkState
            {
                schema_version = _state.schema_version,
                network_id = _state.network_id,
                constructed = _state.constructed,
                nodes = new List<PiezometerNodeState>(_state.nodes),
                aquifer_health = _state.aquifer_health,
                drawdown_state = _state.drawdown_state,
                contamination_risk = _state.contamination_risk,
                contamination_front_progress = _state.contamination_front_progress,
                recharge_state = _state.recharge_state,
                fracture_state = _state.fracture_state,
                monitoring_confidence = _state.monitoring_confidence,
                active_isolation_zone_ids = new List<string>(_state.active_isolation_zone_ids),
                warnings = new List<AquiferWarningRecord>(_state.warnings),
                last_sample_day = _state.last_sample_day,
                days_monitored = _state.days_monitored,
                next_warning_number = _state.next_warning_number
            };
            // Defensive deep copy of node trend windows.
            clone.nodes = _state.nodes.Select(n => new PiezometerNodeState
            {
                node_id = n.node_id,
                strata_id = n.strata_id,
                condition = n.condition,
                calibration = n.calibration,
                head_index = n.head_index,
                water_quality_index = n.water_quality_index,
                contamination_signal = n.contamination_signal,
                clogging_state = n.clogging_state,
                online = n.online,
                last_read_day = n.last_read_day,
                head_trend = new List<float>(n.head_trend)
            }).ToList();
            return clone;
        }

        public void RestoreState(HydrogeologyNetworkState? state)
        {
            if (state == null) return; // Old saves: unbuilt/empty network default.
            _state = state;
            if (_state.nodes == null) _state.nodes = new List<PiezometerNodeState>();
            if (_state.active_isolation_zone_ids == null) _state.active_isolation_zone_ids = new List<string>();
            if (_state.warnings == null) _state.warnings = new List<AquiferWarningRecord>();
            if (_state.schema_version < 1 || _state.schema_version > 1)
                _state.schema_version = 1;
            // Bounded window enforcement on restore (catalog may have changed).
            foreach (var n in _state.nodes)
            {
                if (n.head_trend == null) n.head_trend = new List<float>();
                while (n.head_trend.Count > _catalog.trend_window)
                    n.head_trend.RemoveAt(0);
            }
        }

        private void Raise(string eventId) => OnEventRaised?.Invoke(eventId);
    }
}
