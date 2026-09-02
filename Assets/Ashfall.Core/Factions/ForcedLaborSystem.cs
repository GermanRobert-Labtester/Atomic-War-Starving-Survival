using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
#pragma warning disable CS8618

namespace Ashfall.Core.Factions
{
    [Serializable]
    public class LaborCampDefinition
    {
        public string camp_id = string.Empty;
        public string name = string.Empty;
        public string labor_intensity = "Medium";
        public float base_productivity = 1.35f;
        public float guard_requirement_ratio = 0.20f;
        public float injury_risk_per_shift = 0.06f;
        public float health_stress_per_shift = 2.5f;
        public float hunger_drain_modifier = 1.25f;
        public float morale_harm_bystander = -1.0f;
        public float coercion_requirement = 25.0f;
        public float escape_opportunity = 0.12f;
        public float sabotage_opportunity = 0.15f;
        public float rebellion_pressure_gain = 1.8f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public class LaborCampsCatalog
    {
        public int schema_version = 1;
        public List<LaborCampDefinition> camps = new List<LaborCampDefinition>();
    }

    [Serializable]
    public class ForcedLaborerState
    {
        public string captiveId = string.Empty;
        public string campId = string.Empty;
        public int shiftsCompleted = 0;
        public float physicalStrain = 0f; // 0..100
        public float health = 100f; // 0..100
        public bool isRestrained = true;
        public float individualResentment = 20f; // 0..100
    }

    [Serializable]
    public class RebellionRiskBreakdown
    {
        public float populationPressure;
        public float crueltyFactor;
        public float guardDeficiency;
        public float resentmentFactor;
        public float totalRisk; // 0..1
    }

    [Serializable]
    public class ForcedLaborState
    {
        public string systemId = "forced_labor_system";
        public List<ForcedLaborerState> laborers = new List<ForcedLaborerState>();
        public float crueltyIndex = 0f; // 0..100
        public float resistancePressure = 0f; // 0..100
        public int guardCount = 2;
        public bool isRebellionActive = false;
        public int totalEscaped = 0;
        public int totalRebellions = 0;
        public int totalSabotages = 0;
    }

    public class ForcedLaborSystem
    {
        public const string SystemId = "forced_labor_system";

        private readonly Dictionary<string, LaborCampDefinition> _camps = new Dictionary<string, LaborCampDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, ForcedLaborerState> _laborers = new Dictionary<string, ForcedLaborerState>(StringComparer.Ordinal);

        private float _crueltyIndex = 0f;
        private float _resistancePressure = 0f;
        private int _guardCount = 2;
        private bool _isRebellionActive = false;
        private int _totalEscaped = 0;
        private int _totalRebellions = 0;
        private int _totalSabotages = 0;

        public event Action<string, float>? OnLaborOutputGenerated;
        public event Action<string, string>? OnLaborerInjured;
        public event Action<string, string>? OnSabotageCommitted;
        public event Action<string, bool>? OnEscapeAttempted;
        public event Action<string>? OnRebellionTriggered;
        public event Action<bool, string>? OnRebellionResolved;

        public IReadOnlyCollection<ForcedLaborerState> Laborers => _laborers.Values;
        public float CrueltyIndex => _crueltyIndex;
        public float ResistancePressure => _resistancePressure;
        public int GuardCount => _guardCount;
        public bool IsRebellionActive => _isRebellionActive;
        public int TotalEscaped => _totalEscaped;
        public int TotalRebellions => _totalRebellions;
        public int TotalSabotages => _totalSabotages;

        public void LoadCatalog(string jsonText, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(jsonText) || serializer == null) return;
            try
            {
                var catalog = serializer.Deserialize<LaborCampsCatalog>(jsonText);
                if (catalog?.camps != null)
                {
                    _camps.Clear();
                    foreach (var c in catalog.camps)
                    {
                        if (!string.IsNullOrEmpty(c.camp_id))
                            _camps[c.camp_id] = c;
                    }
                }
            }
            catch
            {
                // Graceful fallback
            }
        }

        public LaborCampDefinition? GetCamp(string campId)
        {
            return _camps.TryGetValue(campId, out var c) ? c : null;
        }

        public void SetGuardCount(int guards)
        {
            _guardCount = Math.Max(0, guards);
        }

        public bool AssignLaborer(string captiveId, string campId, bool restrained, out string failureReason)
        {
            if (string.IsNullOrEmpty(captiveId))
            {
                failureReason = "Captive ID required";
                return false;
            }
            if (!_camps.TryGetValue(campId, out var camp))
            {
                failureReason = "Labor camp assignment not found";
                return false;
            }

            if (_laborers.TryGetValue(captiveId, out var existing))
            {
                existing.campId = campId;
                existing.isRestrained = restrained;
            }
            else
            {
                _laborers[captiveId] = new ForcedLaborerState
                {
                    captiveId = captiveId,
                    campId = campId,
                    shiftsCompleted = 0,
                    physicalStrain = 0f,
                    health = 100f,
                    isRestrained = restrained,
                    individualResentment = 25f
                };
            }

            // Coercive assignment increases cruelty
            _crueltyIndex = Math.Min(100f, _crueltyIndex + 1.5f);
            failureReason = string.Empty;
            return true;
        }

        public bool UnassignLaborer(string captiveId)
        {
            return _laborers.Remove(captiveId);
        }

        public bool EmancipateLaborer(string captiveId)
        {
            if (_laborers.Remove(captiveId))
            {
                _crueltyIndex = Math.Max(0f, _crueltyIndex - 4.0f);
                _resistancePressure = Math.Max(0f, _resistancePressure - 5.0f);
                return true;
            }
            return false;
        }

        public float CalculateProductivity(LaborCampDefinition camp, ForcedLaborerState laborer, float guardRatio)
        {
            if (camp == null || laborer == null) return 0f;

            // Exhaustion penalty
            float strainPenalty = Math.Clamp(laborer.physicalStrain / 100f, 0f, 0.6f);
            float baseProd = camp.base_productivity * (1f - strainPenalty);

            // Guard oversight modifier
            float requiredRatio = Math.Max(0.1f, camp.guard_requirement_ratio);
            float oversight = Math.Clamp(guardRatio / requiredRatio, 0.3f, 1.2f);

            // Health penalty
            float healthMult = Math.Clamp(laborer.health / 100f, 0.2f, 1.0f);

            return Math.Max(0.1f, baseProd * oversight * healthMult);
        }

        public RebellionRiskBreakdown CalculateRebellionRisk()
        {
            var risk = new RebellionRiskBreakdown();
            if (_laborers.Count == 0) return risk;

            // Population pressure
            risk.populationPressure = Math.Clamp(_laborers.Count * 0.05f, 0f, 0.4f);

            // Cruelty factor
            risk.crueltyFactor = (_crueltyIndex / 100f) * 0.35f;

            // Guard deficiency
            float requiredGuards = 0f;
            foreach (var l in _laborers.Values)
            {
                if (_camps.TryGetValue(l.campId, out var c))
                    requiredGuards += c.guard_requirement_ratio;
                else
                    requiredGuards += 0.25f;
            }

            if (_guardCount < requiredGuards)
            {
                risk.guardDeficiency = Math.Min(0.5f, (requiredGuards - _guardCount) * 0.15f);
            }

            // Resentment
            risk.resentmentFactor = (_resistancePressure / 100f) * 0.35f;

            risk.totalRisk = Math.Clamp(risk.populationPressure + risk.crueltyFactor + risk.guardDeficiency + risk.resentmentFactor, 0.02f, 0.95f);
            return risk;
        }

        public void AdvanceDailyShift(ISeededRng rng)
        {
            if (_laborers.Count == 0)
            {
                _resistancePressure = Math.Max(0f, _resistancePressure - 2.0f);
                _crueltyIndex = Math.Max(0f, _crueltyIndex - 0.5f);
                return;
            }

            float guardRatio = (float)_guardCount / Math.Max(1, _laborers.Count);
            float totalShiftOutput = 0f;
            var toRemove = new List<string>();

            foreach (var laborer in _laborers.Values)
            {
                if (!_camps.TryGetValue(laborer.campId, out var camp)) continue;

                // Productivity
                float prod = CalculateProductivity(camp, laborer, guardRatio);
                totalShiftOutput += prod;
                laborer.shiftsCompleted++;

                // Strain & Health decay
                laborer.physicalStrain = Math.Min(100f, laborer.physicalStrain + (camp.health_stress_per_shift * 2f));
                laborer.health = Math.Max(5f, laborer.health - camp.health_stress_per_shift);
                laborer.individualResentment = Math.Min(100f, laborer.individualResentment + camp.rebellion_pressure_gain);

                // Injury roll
                if (rng.NextDouble() < camp.injury_risk_per_shift)
                {
                    laborer.health = Math.Max(0f, laborer.health - 25f);
                    OnLaborerInjured?.Invoke(laborer.captiveId, $"Severe crush trauma at {camp.name}");
                }

                // Sabotage roll
                if (rng.NextDouble() < camp.sabotage_opportunity * (laborer.individualResentment / 100f))
                {
                    _totalSabotages++;
                    _crueltyIndex = Math.Min(100f, _crueltyIndex + 2.0f);
                    OnSabotageCommitted?.Invoke(laborer.captiveId, $"Sabotaged tools and severed safety cables at {camp.name}");
                }

                // Escape roll
                float escapeChance = camp.escape_opportunity * (laborer.isRestrained ? 0.4f : 1.0f);
                if (guardRatio < camp.guard_requirement_ratio) escapeChance *= 1.8f;

                if (rng.NextDouble() < escapeChance * 0.25f)
                {
                    toRemove.Add(laborer.captiveId);
                    _totalEscaped++;
                    OnEscapeAttempted?.Invoke(laborer.captiveId, true);
                }
            }

            // Clean up escapees
            foreach (var id in toRemove)
            {
                _laborers.Remove(id);
            }

            OnLaborOutputGenerated?.Invoke("shelter_scrap_materials", totalShiftOutput);

            // Accumulate global pressure
            _resistancePressure = Math.Min(100f, _resistancePressure + (_laborers.Count * 1.2f));
            _crueltyIndex = Math.Min(100f, _crueltyIndex + (_laborers.Count * 0.4f));

            // Evaluate rebellion
            var risk = CalculateRebellionRisk();
            if (!_isRebellionActive && risk.totalRisk > 0.40f)
            {
                if (rng.NextDouble() < risk.totalRisk * 0.35f)
                {
                    _isRebellionActive = true;
                    _totalRebellions++;
                    OnRebellionTriggered?.Invoke("Captive laborers have overpowered guards and barricaded the excavation galleries!");
                }
            }
        }

        public bool SuppressRebellion(bool lethalForce, ISeededRng rng)
        {
            if (!_isRebellionActive) return false;

            double roll = rng.NextDouble();
            bool success = lethalForce ? (roll < 0.85) : (roll < 0.55);

            if (success)
            {
                _isRebellionActive = false;
                _resistancePressure = Math.Max(10f, _resistancePressure - 50f);
                if (lethalForce)
                {
                    _crueltyIndex = Math.Min(100f, _crueltyIndex + 15f);
                    // Casualties: remove 1-2 laborers
                    int casualtyCount = Math.Min(_laborers.Count, rng.Next(1, 3));
                    var keys = new List<string>(_laborers.Keys);
                    for (int i = 0; i < casualtyCount && i < keys.Count; i++)
                    {
                        _laborers.Remove(keys[i]);
                    }
                }
                OnRebellionResolved?.Invoke(true, lethalForce ? "Lethal force suppressed the revolt with captive casualties." : "Guards restored order through non-lethal riot containment.");
                return true;
            }
            else
            {
                _resistancePressure = 100f;
                _crueltyIndex = Math.Min(100f, _crueltyIndex + 10f);
                OnRebellionResolved?.Invoke(false, "Suppression failed! Rebels maintain control of the worksite.");
                return false;
            }
        }

        public ForcedLaborState CaptureState()
        {
            var state = new ForcedLaborState
            {
                systemId = SystemId,
                crueltyIndex = _crueltyIndex,
                resistancePressure = _resistancePressure,
                guardCount = _guardCount,
                isRebellionActive = _isRebellionActive,
                totalEscaped = _totalEscaped,
                totalRebellions = _totalRebellions,
                totalSabotages = _totalSabotages
            };
            foreach (var kv in _laborers) state.laborers.Add(kv.Value);
            return state;
        }

        public void RestoreState(ForcedLaborState? state)
        {
            _laborers.Clear();
            _crueltyIndex = 0f;
            _resistancePressure = 0f;
            _guardCount = 2;
            _isRebellionActive = false;
            _totalEscaped = 0;
            _totalRebellions = 0;
            _totalSabotages = 0;

            if (state == null) return;

            _crueltyIndex = state.crueltyIndex;
            _resistancePressure = state.resistancePressure;
            _guardCount = state.guardCount;
            _isRebellionActive = state.isRebellionActive;
            _totalEscaped = state.totalEscaped;
            _totalRebellions = state.totalRebellions;
            _totalSabotages = state.totalSabotages;

            if (state.laborers != null)
            {
                foreach (var l in state.laborers)
                {
                    if (!string.IsNullOrEmpty(l.captiveId))
                        _laborers[l.captiveId] = l;
                }
            }
        }
    }
}
