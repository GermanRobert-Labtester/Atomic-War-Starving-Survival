// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<string, LaborCampDefinition> _camps = new Dictionary<string, LaborCampDefinition>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, ForcedLaborerState> _laborers = new Dictionary<string, ForcedLaborerState>(StringComparer.OrdinalIgnoreCase);

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

        public IReadOnlyCollection<ForcedLaborerState> Laborers =>
            _laborers.Values.Select(CloneLaborer).ToList();
        public float CrueltyIndex => _crueltyIndex;
        public float ResistancePressure => _resistancePressure;
        public int GuardCount => _guardCount;
        public bool IsRebellionActive => _isRebellionActive;
        public int TotalEscaped => _totalEscaped;
        public int TotalRebellions => _totalRebellions;
        public int TotalSabotages => _totalSabotages;

        public void LoadCatalog(string jsonText, IJsonSerializer serializer)
        {
            _camps.Clear();
            if (string.IsNullOrWhiteSpace(jsonText) || serializer == null) return;
            try
            {
                var catalog = serializer.Deserialize<LaborCampsCatalog>(jsonText);
                if (catalog?.camps == null) return;
                foreach (var camp in catalog.camps)
                {
                    if (camp == null || string.IsNullOrWhiteSpace(camp.camp_id)) continue;
                    _camps[camp.camp_id.Trim()] = CloneCamp(camp);
                }
            }
            catch (Exception)
            {
                // A malformed reload must not leave the previous catalog active.
                _camps.Clear();
            }
        }

        public LaborCampDefinition? GetCamp(string campId)
        {
            if (string.IsNullOrWhiteSpace(campId)) return null;
            return _camps.TryGetValue(campId.Trim(), out var camp) ? CloneCamp(camp) : null;
        }

        public void SetGuardCount(int guards)
        {
            _guardCount = Math.Max(0, guards);
        }

        public bool AssignLaborer(string captiveId, string campId, bool restrained, out string failureReason)
        {
            if (string.IsNullOrWhiteSpace(captiveId))
            {
                failureReason = "Captive ID required";
                return false;
            }
            captiveId = captiveId.Trim();
            campId = campId?.Trim() ?? string.Empty;
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
            return !string.IsNullOrWhiteSpace(captiveId) && _laborers.Remove(captiveId.Trim());
        }

        public bool EmancipateLaborer(string captiveId)
        {
            if (!string.IsNullOrWhiteSpace(captiveId) && _laborers.Remove(captiveId.Trim()))
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

            float strain = SanitizeRange(laborer.physicalStrain, 0f, 100f);
            float health = SanitizeRange(laborer.health, 0f, 100f);
            float baseProductivity = SanitizeFinite(camp.base_productivity);
            float requiredRatio = Math.Max(0.1f, SanitizeFinite(camp.guard_requirement_ratio));
            float safeGuardRatio = Math.Max(0f, SanitizeFinite(guardRatio));
            float strainPenalty = Math.Clamp(strain / 100f, 0f, 0.6f);
            float baseProd = Math.Max(0f, baseProductivity * (1f - strainPenalty));
            float oversight = Math.Clamp(safeGuardRatio / requiredRatio, 0.3f, 1.2f);
            float healthMult = Math.Clamp(health / 100f, 0.2f, 1f);
            float result = baseProd * oversight * healthMult;
            return IsFinite(result) ? Math.Max(0.1f, result) : 0.1f;
        }

        public RebellionRiskBreakdown CalculateRebellionRisk()
        {
            var risk = new RebellionRiskBreakdown();
            _crueltyIndex = SanitizeRange(_crueltyIndex, 0f, 100f);
            _resistancePressure = SanitizeRange(_resistancePressure, 0f, 100f);
            if (_laborers.Count == 0) return risk;

            // Population pressure
            risk.populationPressure = Math.Clamp(_laborers.Count * 0.05f, 0f, 0.4f);

            // Cruelty factor
            risk.crueltyFactor = (_crueltyIndex / 100f) * 0.35f;

            // Guard deficiency
            float requiredGuards = 0f;
            foreach (var l in _laborers.Values)
            {
                if (l == null) continue;
                if (_camps.TryGetValue(l.campId, out var c))
                    requiredGuards += Math.Max(0f, SanitizeFinite(c.guard_requirement_ratio));
                else
                    requiredGuards += 0.25f;
            }

            if (_guardCount < requiredGuards)
            {
                risk.guardDeficiency = Math.Min(0.5f, (requiredGuards - _guardCount) * 0.15f);
            }

            // Resentment
            risk.resentmentFactor = (_resistancePressure / 100f) * 0.35f;

            risk.totalRisk = SanitizeRange(
                risk.populationPressure + risk.crueltyFactor + risk.guardDeficiency + risk.resentmentFactor,
                0.02f,
                0.95f);
            return risk;
        }

        public void AdvanceDailyShift(ISeededRng rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (_laborers.Count == 0)
            {
                _resistancePressure = SanitizeRange(_resistancePressure - 2f, 0f, 100f);
                _crueltyIndex = SanitizeRange(_crueltyIndex - 0.5f, 0f, 100f);
                return;
            }

            float guardRatio = (float)_guardCount / Math.Max(1, _laborers.Count);
            float totalShiftOutput = 0f;
            var toRemove = new List<string>();

            foreach (var laborer in _laborers.Values)
            {
                if (laborer == null || !_camps.TryGetValue(laborer.campId, out var camp)) continue;

                // Productivity
                float prod = CalculateProductivity(camp, laborer, guardRatio);
                totalShiftOutput += prod;
                if (laborer.shiftsCompleted < int.MaxValue) laborer.shiftsCompleted++;

                // Strain & Health decay
                laborer.physicalStrain = SanitizeRange(
                    laborer.physicalStrain + (SanitizeFinite(camp.health_stress_per_shift) * 2f),
                    0f,
                    100f);
                laborer.health = SanitizeRange(
                    laborer.health - SanitizeFinite(camp.health_stress_per_shift),
                    0f,
                    100f);
                laborer.individualResentment = SanitizeRange(
                    laborer.individualResentment + SanitizeFinite(camp.rebellion_pressure_gain),
                    0f,
                    100f);

                // Injury roll
                if (rng.NextDouble() < SanitizeProbability(camp.injury_risk_per_shift))
                {
                    laborer.health = Math.Max(0f, laborer.health - 25f);
                    OnLaborerInjured?.Invoke(laborer.captiveId, $"Severe crush trauma at {camp.name}");
                }

                // Sabotage roll
                if (rng.NextDouble() < SanitizeProbability(camp.sabotage_opportunity)
                    * (laborer.individualResentment / 100f))
                {
                    if (_totalSabotages < int.MaxValue) _totalSabotages++;
                    _crueltyIndex = SanitizeRange(_crueltyIndex + 2f, 0f, 100f);
                    OnSabotageCommitted?.Invoke(laborer.captiveId, $"Sabotaged tools and severed safety cables at {camp.name}");
                }

                // Escape roll
                float escapeChance = SanitizeProbability(camp.escape_opportunity)
                    * (laborer.isRestrained ? 0.4f : 1f);
                if (guardRatio < camp.guard_requirement_ratio) escapeChance *= 1.8f;

                if (rng.NextDouble() < SanitizeProbability(escapeChance * 0.25f))
                {
                    toRemove.Add(laborer.captiveId);
                    if (_totalEscaped < int.MaxValue) _totalEscaped++;
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
            _resistancePressure = SanitizeRange(
                _resistancePressure + (_laborers.Count * 1.2f),
                0f,
                100f);
            _crueltyIndex = SanitizeRange(
                _crueltyIndex + (_laborers.Count * 0.4f),
                0f,
                100f);

            // Evaluate rebellion
            var risk = CalculateRebellionRisk();
            if (!_isRebellionActive && risk.totalRisk > 0.40f)
            {
                if (rng.NextDouble() < SanitizeProbability(risk.totalRisk * 0.35f))
                {
                    _isRebellionActive = true;
                    if (_totalRebellions < int.MaxValue) _totalRebellions++;
                    OnRebellionTriggered?.Invoke("Captive laborers have overpowered guards and barricaded the excavation galleries!");
                }
            }
        }

        public bool SuppressRebellion(bool lethalForce, ISeededRng rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (!_isRebellionActive) return false;

            double roll = rng.NextDouble();
            bool success = lethalForce ? (roll < 0.85) : (roll < 0.55);

            if (success)
            {
                _isRebellionActive = false;
                _resistancePressure = SanitizeRange(_resistancePressure - 50f, 10f, 100f);
                if (lethalForce)
                {
                    _crueltyIndex = SanitizeRange(_crueltyIndex + 15f, 0f, 100f);
                    // Casualties: remove 1-2 laborers
                    int casualtyRoll = Math.Clamp(rng.Next(1, 3), 1, 2);
                    int casualtyCount = Math.Min(_laborers.Count, casualtyRoll);
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
                _crueltyIndex = SanitizeRange(_crueltyIndex + 10f, 0f, 100f);
                OnRebellionResolved?.Invoke(false, "Suppression failed! Rebels maintain control of the worksite.");
                return false;
            }
        }

        public ForcedLaborState CaptureState()
        {
            var state = new ForcedLaborState
            {
                systemId = SystemId,
                crueltyIndex = SanitizeRange(_crueltyIndex, 0f, 100f),
                resistancePressure = SanitizeRange(_resistancePressure, 0f, 100f),
                guardCount = Math.Max(0, _guardCount),
                isRebellionActive = _isRebellionActive,
                totalEscaped = Math.Max(0, _totalEscaped),
                totalRebellions = Math.Max(0, _totalRebellions),
                totalSabotages = Math.Max(0, _totalSabotages)
            };
            foreach (var laborer in _laborers.Values)
            {
                if (laborer != null) state.laborers.Add(CloneLaborer(laborer));
            }
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

            _crueltyIndex = SanitizeRange(state.crueltyIndex, 0f, 100f);
            _resistancePressure = SanitizeRange(state.resistancePressure, 0f, 100f);
            _guardCount = Math.Max(0, state.guardCount);
            _isRebellionActive = state.isRebellionActive;
            _totalEscaped = Math.Max(0, state.totalEscaped);
            _totalRebellions = Math.Max(0, state.totalRebellions);
            _totalSabotages = Math.Max(0, state.totalSabotages);
            if (state.laborers == null) return;

            foreach (var laborer in state.laborers)
            {
                if (laborer == null || string.IsNullOrWhiteSpace(laborer.captiveId)
                    || string.IsNullOrWhiteSpace(laborer.campId)) continue;
                var clone = CloneLaborer(laborer);
                _laborers[clone.captiveId] = clone;
            }
        }

        private static ForcedLaborerState CloneLaborer(ForcedLaborerState source)
        {
            return new ForcedLaborerState
            {
                captiveId = source.captiveId?.Trim() ?? string.Empty,
                campId = source.campId?.Trim() ?? string.Empty,
                shiftsCompleted = Math.Max(0, source.shiftsCompleted),
                physicalStrain = SanitizeRange(source.physicalStrain, 0f, 100f),
                health = SanitizeRange(source.health, 0f, 100f),
                isRestrained = source.isRestrained,
                individualResentment = SanitizeRange(source.individualResentment, 0f, 100f)
            };
        }

        private static LaborCampDefinition CloneCamp(LaborCampDefinition source)
        {
            return new LaborCampDefinition
            {
                camp_id = source.camp_id?.Trim() ?? string.Empty,
                name = source.name ?? string.Empty,
                labor_intensity = source.labor_intensity ?? string.Empty,
                base_productivity = SanitizeFinite(source.base_productivity),
                guard_requirement_ratio = SanitizeRange(source.guard_requirement_ratio, 0f, 100f),
                injury_risk_per_shift = SanitizeProbability(source.injury_risk_per_shift),
                health_stress_per_shift = SanitizeRange(source.health_stress_per_shift, 0f, 100f),
                hunger_drain_modifier = SanitizeRange(source.hunger_drain_modifier, 0f, 100f),
                morale_harm_bystander = SanitizeFinite(source.morale_harm_bystander),
                coercion_requirement = SanitizeRange(source.coercion_requirement, 0f, 100f),
                escape_opportunity = SanitizeProbability(source.escape_opportunity),
                sabotage_opportunity = SanitizeProbability(source.sabotage_opportunity),
                rebellion_pressure_gain = SanitizeRange(source.rebellion_pressure_gain, 0f, 100f),
                tags = source.tags == null
                    ? new List<string>()
                    : source.tags.Where(tag => !string.IsNullOrWhiteSpace(tag)).Distinct(StringComparer.Ordinal).ToList()
            };
        }

        private static bool IsFinite(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value);

        private static float SanitizeFinite(float value) =>
            IsFinite(value) ? value : 0f;

        private static float SanitizeRange(float value, float min, float max)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) return min;
            return Math.Clamp(value, min, max);
        }

        private static float SanitizeProbability(float value) =>
            SanitizeRange(value, 0f, 1f);
    }
}
