// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class GprSurveyTarget
    {
        public string target_id = string.Empty;
        public string anomaly_profile_id = string.Empty;
    }

    [Serializable]
    public sealed class SubsurfaceObservation
    {
        public string observation_id = string.Empty;
        public string target_id = string.Empty;
        public string anomaly_class = "unknown_reflector";
        public string terrain_id = string.Empty;
        public string mode_id = string.Empty;
        public float confidence;
        public float depth_band;
        public float coverage;
        public int day;
    }

    [Serializable]
    public sealed class BuriedAnomalyLead
    {
        public string lead_id = string.Empty;
        public string target_id = string.Empty;
        public string anomaly_class = string.Empty;
        public float confidence;
        public bool verified;
    }

    [Serializable]
    public sealed class GprActiveSurveyState
    {
        public string target_id = string.Empty;
        public string anomaly_profile_id = string.Empty;
        public string terrain_id = string.Empty;
        public string mode_id = string.Empty;
        public int progress_ticks;
        public int day;
    }

    [Serializable]
    public sealed class GroundPenetratingRadarState
    {
        public int schema_version = 1;
        public ulong rng_state;
        public string equipment_id = string.Empty;
        public float calibration = 1f;
        public float condition = 1f;
        public int next_observation_number = 1;
        public GprActiveSurveyState? active_survey;
        public List<SubsurfaceObservation> observations = new List<SubsurfaceObservation>();
        public List<BuriedAnomalyLead> leads = new List<BuriedAnomalyLead>();
    }

    public sealed class GprSurveyResult
    {
        public bool Success { get; set; }
        public string FailureCode { get; set; } = string.Empty;
        public SubsurfaceObservation? Observation { get; set; }
    }

    public static class GprFailureCodes
    {
        public const string EquipmentUnavailable = "gpr.equipment_unavailable";
        public const string PowerUnavailable = "gpr.power_unavailable";
        public const string ModeUnknown = "gpr.mode_unknown";
        public const string TerrainUnsupported = "gpr.terrain_unsupported";
        public const string AnomalyUnknown = "gpr.anomaly_unknown";
        public const string SurveyActive = "gpr.survey_active";
        public const string LeadAlreadyKnown = "gpr.lead_already_known";
    }

    /// <summary>Uncertain, batch-oriented survey intelligence. It creates leads, never loot or excavation results.</summary>
    public sealed class GroundPenetratingRadarEngine
    {
        private readonly ISeededRng _rng;
        private GroundPenetratingRadarCatalog _catalog;
        private GroundPenetratingRadarState _state = new GroundPenetratingRadarState();
        private IPlayerInventoryPort? _inventory;

        public GroundPenetratingRadarEngine(ISeededRng? rng = null, GroundPenetratingRadarCatalog? catalog = null)
        {
            _rng = rng ?? new SeededRng(121);
            _catalog = catalog ?? new GroundPenetratingRadarCatalog(new GroundPenetratingRadarCatalogDto
            {
                equipment = new List<GprEquipmentProfile> { new GprEquipmentProfile { gpr_id = "gpr_cart_mk1", battery_item_id = "battery_pack" } },
                modes = new List<GprSurveyModeProfile>
                {
                    new GprSurveyModeProfile { mode_id = "gpr_deep_scan", penetration_rating = 0.85f, resolution_rating = 0.35f, power_cost = 2, scan_time_ticks = 3 },
                    new GprSurveyModeProfile { mode_id = "gpr_detail_scan", penetration_rating = 0.35f, resolution_rating = 0.90f, power_cost = 3, scan_time_ticks = 4 }
                },
                terrains = new List<GprTerrainProfile> { new GprTerrainProfile { terrain_id = "dry_soil", attenuation = 0.1f } },
                anomalies = new List<GprAnomalyProfile> { new GprAnomalyProfile { anomaly_id = "gpr_buried_structure", anomaly_class = "buried_structure", depth_band = 30f, signal_strength = 0.8f } }
            });
        }

        public GroundPenetratingRadarCatalog Catalog => _catalog;
        public GroundPenetratingRadarState State => _state;
        public void BindCatalog(GroundPenetratingRadarCatalog catalog) => _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        public void BindInventory(IPlayerInventoryPort inventory) => _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));

        public ActionResult Equip(string equipmentId)
        {
            var equipment = _catalog.FindEquipment(equipmentId);
            if (equipment == null) return ActionResult.Blocked(GprFailureCodes.EquipmentUnavailable, "gpr.equipment_unknown");
            _state.equipment_id = equipmentId;
            _state.condition = Math.Clamp(_state.condition <= 0f ? equipment.starting_condition : _state.condition, 0f, 1f);
            _state.calibration = Math.Clamp(_state.calibration <= 0f ? equipment.starting_calibration : _state.calibration, 0f, 1f);
            return ActionResult.Success("gpr.equipment_equipped");
        }

        public ActionResult BeginSurvey(string targetId, string anomalyProfileId, string terrainId, string modeId, int day = 0)
        {
            if (_catalog.FindEquipment(_state.equipment_id) == null) return ActionResult.Blocked(GprFailureCodes.EquipmentUnavailable, "gpr.equipment_unavailable");
            var mode = _catalog.FindMode(modeId);
            if (mode == null) return ActionResult.Blocked(GprFailureCodes.ModeUnknown, "gpr.mode_unknown");
            if (_catalog.FindTerrain(terrainId) == null) return ActionResult.Blocked(GprFailureCodes.TerrainUnsupported, "gpr.terrain_unsupported");
            if (_catalog.FindAnomaly(anomalyProfileId) == null) return ActionResult.Blocked(GprFailureCodes.AnomalyUnknown, "gpr.anomaly_unknown");
            if (_state.active_survey != null) return ActionResult.Blocked(GprFailureCodes.SurveyActive, "gpr.survey_active");
            var equipment = _catalog.FindEquipment(_state.equipment_id)!;
            if (_inventory == null || !_inventory.TryConsume(equipment.battery_item_id, mode.power_cost))
                return ActionResult.Blocked(GprFailureCodes.PowerUnavailable, "gpr.power_unavailable");
            _state.active_survey = new GprActiveSurveyState { target_id = targetId ?? string.Empty, anomaly_profile_id = anomalyProfileId, terrain_id = terrainId, mode_id = modeId, day = day };
            return ActionResult.Success("gpr.survey_started");
        }

        public GprSurveyResult Tick(float operatorSkill = 0.5f)
        {
            var active = _state.active_survey;
            if (active == null) return new GprSurveyResult { FailureCode = "gpr.idle" };
            var mode = _catalog.FindMode(active.mode_id);
            var terrain = _catalog.FindTerrain(active.terrain_id);
            var anomaly = _catalog.FindAnomaly(active.anomaly_profile_id);
            if (mode == null || terrain == null || anomaly == null) return new GprSurveyResult { FailureCode = GprFailureCodes.AnomalyUnknown };
            active.progress_ticks++;
            if (active.progress_ticks < mode.scan_time_ticks) return new GprSurveyResult { Success = true };

            var prior = FindObservation(active.target_id);
            float attenuation = Math.Clamp(1f - terrain.attenuation, 0f, 1f);
            float signal = anomaly.signal_strength * mode.penetration_rating * attenuation * Math.Clamp(_state.condition, 0.1f, 1f) * _state.calibration;
            float noise = 0.08f + (1f - Math.Clamp(_state.condition, 0.1f, 1f)) * 0.12f;
            float measured = Math.Clamp(signal - noise + (_rng.NextFloat() - 0.5f) * noise, 0f, 1f);
            float confidence = Math.Clamp(measured * (0.45f + mode.resolution_rating * 0.55f) * (0.8f + Math.Clamp(operatorSkill, 0f, 1f) * 0.2f), 0f, 1f);
            if (prior != null) confidence = Math.Clamp(prior.confidence + confidence * 0.45f, 0f, 1f);
            var observation = new SubsurfaceObservation
            {
                observation_id = $"gpr_obs_{_state.next_observation_number++}", target_id = active.target_id,
                anomaly_class = confidence >= 0.75f ? anomaly.anomaly_class : "unknown_reflector",
                terrain_id = active.terrain_id, mode_id = active.mode_id, confidence = confidence,
                depth_band = anomaly.depth_band * (1f + (1f - confidence) * 0.3f), coverage = mode.resolution_rating, day = active.day
            };
            _state.observations.Add(observation);
            _state.active_survey = null;
            return new GprSurveyResult { Success = true, Observation = observation };
        }

        public bool TryCreateLead(string targetId, out BuriedAnomalyLead? lead)
        {
            lead = null;
            var observation = FindObservation(targetId);
            if (observation == null || observation.confidence < 0.55f) return false;
            foreach (var existing in _state.leads)
                if (existing.target_id == targetId) { lead = existing; return false; }
            lead = new BuriedAnomalyLead { lead_id = $"gpr_lead_{_state.leads.Count + 1}", target_id = targetId, anomaly_class = observation.anomaly_class, confidence = observation.confidence };
            _state.leads.Add(lead);
            return true;
        }

        public SubsurfaceObservation? FindObservation(string targetId)
        {
            for (int i = _state.observations.Count - 1; i >= 0; i--)
                if (_state.observations[i].target_id == targetId) return _state.observations[i];
            return null;
        }

        public GroundPenetratingRadarState CaptureState()
        {
            var copy = new GroundPenetratingRadarState
            {
                schema_version = _state.schema_version, rng_state = _rng is SeededRng seeded ? seeded.PeekState() : 0UL,
                equipment_id = _state.equipment_id, calibration = _state.calibration,
                condition = _state.condition, next_observation_number = _state.next_observation_number,
                observations = new List<SubsurfaceObservation>(), leads = new List<BuriedAnomalyLead>()
            };
            if (_state.active_survey != null) copy.active_survey = new GprActiveSurveyState
            {
                target_id = _state.active_survey.target_id, anomaly_profile_id = _state.active_survey.anomaly_profile_id,
                terrain_id = _state.active_survey.terrain_id, mode_id = _state.active_survey.mode_id,
                progress_ticks = _state.active_survey.progress_ticks, day = _state.active_survey.day
            };
            foreach (var observation in _state.observations) copy.observations.Add(new SubsurfaceObservation
            {
                observation_id = observation.observation_id, target_id = observation.target_id, anomaly_class = observation.anomaly_class,
                terrain_id = observation.terrain_id, mode_id = observation.mode_id, confidence = observation.confidence,
                depth_band = observation.depth_band, coverage = observation.coverage, day = observation.day
            });
            foreach (var existing in _state.leads) copy.leads.Add(new BuriedAnomalyLead
            {
                lead_id = existing.lead_id, target_id = existing.target_id, anomaly_class = existing.anomaly_class,
                confidence = existing.confidence, verified = existing.verified
            });
            return copy;
        }

        public void RestoreState(GroundPenetratingRadarState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state = state;
            _state.observations ??= new List<SubsurfaceObservation>();
            _state.leads ??= new List<BuriedAnomalyLead>();
            _state.calibration = Math.Clamp(_state.calibration, 0f, 1f);
            _state.condition = Math.Clamp(_state.condition, 0f, 1f);
            if (_rng is SeededRng seeded && _state.rng_state != 0UL)
                seeded.SeekState(_state.rng_state);
        }
    }
}
