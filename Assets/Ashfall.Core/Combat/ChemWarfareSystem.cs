// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 198 — Biological Weapons & Chemical Warfare System
// Subsystem    : Fictionalized CBRN Hazard Warfare & Contamination Mechanics
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Combat
{
    /// <summary>Abstract fictionalized toxic agent definition loaded from chemical_weapons.json.</summary>
    [Serializable]
    public sealed class ToxicAgentDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("hazard_class")]
        public string HazardClass { get; set; } = string.Empty;

        [JsonPropertyName("base_density_permille")]
        public int BaseDensityPermille { get; set; } = 500;

        [JsonPropertyName("persistence_ticks")]
        public int PersistenceTicks { get; set; } = 8;

        [JsonPropertyName("filter_wear_permille")]
        public int FilterWearPermille { get; set; } = 50;

        [JsonPropertyName("exposure_severity")]
        public int ExposureSeverity { get; set; } = 1;

        [JsonPropertyName("visual_profile_id")]
        public string VisualProfileId { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>Active tactical hazard zone in a combat lane or shelter breach.</summary>
    [Serializable]
    public sealed class ToxicHazardZoneState
    {
        public string HazardId { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;
        public int CombatLane { get; set; } = 1;
        public int DensityPermille { get; set; } = 500;
        public int RemainingTicks { get; set; } = 8;
        public string SourceId { get; set; } = string.Empty;
        public bool IsCleared { get; set; }

        public ToxicHazardZoneState Clone() => new ToxicHazardZoneState
        {
            HazardId = HazardId,
            AgentId = AgentId,
            CombatLane = CombatLane,
            DensityPermille = DensityPermille,
            RemainingTicks = RemainingTicks,
            SourceId = SourceId,
            IsCleared = IsCleared
        };
    }

    /// <summary>Persistent campaign state for chemical warfare and toxic hazards.</summary>
    [Serializable]
    public sealed class ChemWarfareSaveState
    {
        public string SystemId { get; set; } = ChemWarfareSystem.SystemId;
        public List<ToxicHazardZoneState> ActiveHazards { get; set; } = new List<ToxicHazardZoneState>();
        public int TotalHazardsDeployed { get; set; }
        public int TotalResidueIncidentsLogged { get; set; }
    }

    /// <summary>
    /// Engine-agnostic tactical CBRN hazard warfare system.
    /// Simulates fictionalized toxic hazard zones, wind-driven drift, respirator filter attrition,
    /// and shelter decontamination residue handoff with strict zero-unseeded-RNG determinism.
    /// </summary>
    public sealed class ChemWarfareSystem
    {
        public const string SystemId = "chem_warfare_system";

        private readonly Dictionary<string, ToxicAgentDefinition> _agentCatalog =
            new Dictionary<string, ToxicAgentDefinition>(StringComparer.Ordinal);

        private ChemWarfareSaveState _state = new ChemWarfareSaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<ToxicHazardZoneState>? OnHazardDeployed;
        public event Action<string, int, int>? OnToxicExposureResolved; // actorId, severityTier, lane
        public event Action<string, int>? OnShelterResidueCreated;       // sectorId, severityTier
        public event Action? OnStateChanged;

        public ChemWarfareSaveState State => _state;
        public IReadOnlyDictionary<string, ToxicAgentDefinition> AgentCatalog => _agentCatalog;

        public ChemWarfareSystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("agents", out var agentsEl) && agentsEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in agentsEl.EnumerateArray())
                {
                    var agent = JsonSerializer.Deserialize<ToxicAgentDefinition>(el.GetRawText());
                    if (agent != null && !string.IsNullOrEmpty(agent.Id))
                    {
                        _agentCatalog[agent.Id] = agent;
                    }
                }
            }
        }

        public ToxicHazardZoneState? DeployHazard(string agentId, int lane, string sourceId, int? customDensity = null)
        {
            if (string.IsNullOrEmpty(agentId)) return null;

            int density = 500;
            int ticks = 8;

            if (_agentCatalog.TryGetValue(agentId, out var def))
            {
                density = def.BaseDensityPermille;
                ticks = def.PersistenceTicks;
            }

            if (customDensity.HasValue)
            {
                density = Math.Clamp(customDensity.Value, 0, 1000);
            }

            lane = Math.Clamp(lane, 0, 2);

            _state.TotalHazardsDeployed++;
            string hazardId = $"hazard_{agentId}_{_state.TotalHazardsDeployed}";

            var hazard = new ToxicHazardZoneState
            {
                HazardId = hazardId,
                AgentId = agentId,
                CombatLane = lane,
                DensityPermille = density,
                RemainingTicks = ticks,
                SourceId = sourceId ?? string.Empty,
                IsCleared = false
            };

            _state.ActiveHazards.Add(hazard);
            OnHazardDeployed?.Invoke(hazard);
            OnStateChanged?.Invoke();
            return hazard;
        }

        public void TickCombat(WeatherKind weather, int windDirection = 0, int windStrengthTier = 1)
        {
            if (_state.ActiveHazards.Count == 0) return;

            windStrengthTier = Math.Clamp(windStrengthTier, 0, 3);

            for (int i = _state.ActiveHazards.Count - 1; i >= 0; i--)
            {
                var h = _state.ActiveHazards[i];
                if (h.IsCleared)
                {
                    _state.ActiveHazards.RemoveAt(i);
                    continue;
                }

                h.RemainingTicks--;

                // Density decay: base 50 permille + wind dispersion
                int decay = 50 + (windStrengthTier * 25);
                if (weather == WeatherKind.Rain || weather == WeatherKind.AcidSnow)
                {
                    decay += 40; // Precipitation scrubs volatile aerosols faster
                }
                else if (weather == WeatherKind.Blizzard)
                {
                    decay += 70;
                }

                h.DensityPermille = Math.Max(0, h.DensityPermille - decay);

                // Wind drift: high wind can push hazard across lanes
                if (windStrengthTier >= 2 && windDirection != 0)
                {
                    int targetLane = Math.Clamp(h.CombatLane + (windDirection > 0 ? 1 : -1), 0, 2);
                    h.CombatLane = targetLane;
                }

                if (h.RemainingTicks <= 0 || h.DensityPermille <= 0)
                {
                    h.IsCleared = true;
                    _state.ActiveHazards.RemoveAt(i);
                }
            }

            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Evaluates actor exposure in their current combat lane.
        /// Returns effective severity tier (0..4) and calculates filter wear to apply to equipment.
        /// </summary>
        public int EvaluateActorExposure(string actorId, int actorLane, float maskCondition01, out float filterWearApplied)
        {
            filterWearApplied = 0f;
            if (string.IsNullOrEmpty(actorId)) return 0;

            actorLane = Math.Clamp(actorLane, 0, 2);
            var hazard = _state.ActiveHazards.Find(h => !h.IsCleared && h.CombatLane == actorLane);
            if (hazard == null || hazard.DensityPermille <= 0) return 0;

            int baseSeverity = 1;
            int filterWearPermille = 50;
            if (_agentCatalog.TryGetValue(hazard.AgentId, out var def))
            {
                baseSeverity = def.ExposureSeverity;
                filterWearPermille = def.FilterWearPermille;
            }

            maskCondition01 = Math.Clamp(maskCondition01, 0f, 1f);

            // Filter wear is proportional to hazard density
            filterWearApplied = (filterWearPermille / 1000f) * (hazard.DensityPermille / 1000f);

            // Effective exposure severity depends on mask integrity
            int effectiveSeverity;
            if (maskCondition01 >= 0.70f)
            {
                effectiveSeverity = 0; // Filter fully absorbs
            }
            else if (maskCondition01 >= 0.30f)
            {
                effectiveSeverity = Math.Max(0, baseSeverity - 1); // Partial breakthrough
            }
            else
            {
                effectiveSeverity = baseSeverity; // Mask failure or absent
            }

            if (effectiveSeverity > 0)
            {
                OnToxicExposureResolved?.Invoke(actorId, effectiveSeverity, actorLane);
            }

            return effectiveSeverity;
        }

        public bool ClearHazard(string hazardId)
        {
            var h = _state.ActiveHazards.Find(x => x.HazardId == hazardId);
            if (h == null || h.IsCleared) return false;

            h.IsCleared = true;
            _state.ActiveHazards.Remove(h);
            OnStateChanged?.Invoke();
            return true;
        }

        public void TriggerShelterResidueHandoff(string sectorId, int severityTier)
        {
            if (string.IsNullOrEmpty(sectorId)) return;
            severityTier = Math.Clamp(severityTier, 1, 4);

            _state.TotalResidueIncidentsLogged++;
            OnShelterResidueCreated?.Invoke(sectorId, severityTier);
            OnStateChanged?.Invoke();
        }

        // ── Save / Restore ──────────────────────────────────────────────────

        public ChemWarfareSaveState CaptureState()
        {
            var copy = new ChemWarfareSaveState
            {
                SystemId = _state.SystemId,
                TotalHazardsDeployed = _state.TotalHazardsDeployed,
                TotalResidueIncidentsLogged = _state.TotalResidueIncidentsLogged,
                ActiveHazards = new List<ToxicHazardZoneState>(_state.ActiveHazards.Count)
            };

            foreach (var h in _state.ActiveHazards)
            {
                copy.ActiveHazards.Add(h.Clone());
            }

            return copy;
        }

        public void RestoreState(ChemWarfareSaveState? state)
        {
            if (state == null)
            {
                _state = new ChemWarfareSaveState();
                return;
            }

            _state = new ChemWarfareSaveState
            {
                SystemId = state.SystemId ?? SystemId,
                TotalHazardsDeployed = state.TotalHazardsDeployed,
                TotalResidueIncidentsLogged = state.TotalResidueIncidentsLogged,
                ActiveHazards = new List<ToxicHazardZoneState>()
            };

            if (state.ActiveHazards != null)
            {
                foreach (var h in state.ActiveHazards)
                {
                    if (h != null)
                    {
                        _state.ActiveHazards.Add(h.Clone());
                    }
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
