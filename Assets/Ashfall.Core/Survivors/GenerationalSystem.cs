// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 178: Childhood & Generational Rearing
// Pure deterministic simulation: child phases, schoolhouse education, trauma
// absorption, starvation stunting, and one-time adult transition.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Random;

namespace Ashfall.Core.Survivors
{
    public enum DevelopmentPhase
    {
        Infant = 0,
        YoungChild = 1,
        OlderChild = 2,
        Adolescent = 3,
        AdultTransitioned = 4
    }

    public enum EducationLevel
    {
        None = 0,
        Basic = 1,
        Competent = 2,
        Advanced = 3
    }

    [Serializable]
    public sealed class FormativeEvent
    {
        public string eventId { get; set; } = string.Empty;
        public string survivorId { get; set; } = string.Empty;
        public string eventTypeId { get; set; } = string.Empty;
        public int campaignDay { get; set; } = 1;
        public float severity { get; set; } = 1.0f;
        public string sourceEntityId { get; set; } = string.Empty;
        public bool resolved { get; set; } = false;
    }

    [Serializable]
    public sealed class ChildDevelopment
    {
        public string survivorId { get; set; } = string.Empty;
        public int birthDay { get; set; } = 1;
        public DevelopmentPhase developmentPhase { get; set; } = DevelopmentPhase.YoungChild;
        public float developmentProgress { get; set; } = 0.0f;
        public float educationXp { get; set; } = 0.0f;
        public string educationFocusId { get; set; } = "practical_survival";
        public float nutritionScore { get; set; } = 100.0f;
        public float safetyScore { get; set; } = 100.0f;
        public float traumaLoad { get; set; } = 0.0f;
        public float stuntingSeverity { get; set; } = 0.0f;
        public List<string> formativeEventIds { get; set; } = new List<string>();
        public List<string> acquiredDevelopmentTraitIds { get; set; } = new List<string>();
        public int lastGrowthTickDay { get; set; } = 0;
        public bool adulthoodProcessed { get; set; } = false;
        public string? assignedTeacherId { get; set; } = null;
        public string? assignedGuardianId { get; set; } = null;
        public int consecutiveStarvationDays { get; set; } = 0;
    }

    [Serializable]
    public sealed class DevelopmentTraitDef
    {
        public string trait_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string polarity { get; set; } = "Positive";
        public Dictionary<string, float> stat_modifiers { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        public Dictionary<string, float> skill_modifiers { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        public Dictionary<string, float> morale_modifiers { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        public Dictionary<string, float> work_modifiers { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        public float weight { get; set; } = 1.0f;
        public float min_trauma { get; set; } = 0.0f;
        public float max_trauma { get; set; } = 100.0f;
        public float min_education { get; set; } = 0.0f;
        public string exclusive_group { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DevelopmentTraitsCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<DevelopmentTraitDef> traits { get; set; } = new List<DevelopmentTraitDef>();
    }

    [Serializable]
    public sealed class GenerationalState
    {
        public int schema_version { get; set; } = 1;
        public List<ChildDevelopment> children { get; set; } = new List<ChildDevelopment>();
        public List<FormativeEvent> formativeEvents { get; set; } = new List<FormativeEvent>();
        public int totalAdulthoodTransitions { get; set; } = 0;
        public bool schoolhouseActive { get; set; } = true;
    }

    public sealed class GenerationalSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem? _needs;
        private readonly ILog _log;

        private readonly Dictionary<string, DevelopmentTraitDef> _traits =
            new Dictionary<string, DevelopmentTraitDef>(StringComparer.Ordinal);

        private GenerationalState _state = new GenerationalState();

        public event Action<string, DevelopmentPhase, List<string>>? OnAdulthoodReached;
        public event Action<string, string, float>? OnFormativeEventRecorded;
        public event Action<string, DevelopmentPhase>? OnPhaseAdvanced;

        public GenerationalState State => _state;

        public GenerationalSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            NeedsSystem? needs = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _needs = needs;
            _log = log ?? new NullLog();
        }

        public void RegisterTrait(DevelopmentTraitDef trait)
        {
            if (trait == null || string.IsNullOrWhiteSpace(trait.trait_id)) return;
            _traits[trait.trait_id] = trait;
        }

        public ChildDevelopment EnsureChild(string survivorId, int birthDay, DevelopmentPhase initialPhase = DevelopmentPhase.YoungChild)
        {
            var existing = _state.children.FirstOrDefault(c => c.survivorId == survivorId);
            if (existing != null) return existing;

            var child = new ChildDevelopment
            {
                survivorId = survivorId,
                birthDay = birthDay,
                developmentPhase = initialPhase,
                developmentProgress = (float)initialPhase * 25.0f,
                lastGrowthTickDay = 0
            };
            _state.children.Add(child);
            return child;
        }

        public ChildDevelopment? GetChild(string survivorId)
        {
            return _state.children.FirstOrDefault(c => c.survivorId == survivorId);
        }

        public bool AssignGuardian(string childId, string guardianId)
        {
            var child = GetChild(childId);
            if (child == null) return false;
            child.assignedGuardianId = guardianId;
            return true;
        }

        public bool AssignTeacher(string childId, string teacherId, string focusId)
        {
            var child = GetChild(childId);
            if (child == null) return false;
            child.assignedTeacherId = teacherId;
            child.educationFocusId = focusId;
            return true;
        }

        public void RecordFormativeEvent(string childId, string eventTypeId, float severity, string sourceId, int day)
        {
            var child = GetChild(childId);
            if (child == null || child.adulthoodProcessed) return;

            string eventId = $"formative_{childId}_{day}_{_state.formativeEvents.Count + 1}";
            var ev = new FormativeEvent
            {
                eventId = eventId,
                survivorId = childId,
                eventTypeId = eventTypeId,
                campaignDay = day,
                severity = severity,
                sourceEntityId = sourceId,
                resolved = false
            };
            _state.formativeEvents.Add(ev);
            child.formativeEventIds.Add(eventId);

            // Age-sensitivity multiplier: younger children absorb trauma more profoundly
            float sensitivity = child.developmentPhase switch
            {
                DevelopmentPhase.Infant => 1.5f,
                DevelopmentPhase.YoungChild => 1.3f,
                DevelopmentPhase.OlderChild => 1.1f,
                DevelopmentPhase.Adolescent => 0.9f,
                _ => 1.0f
            };

            // Guardian buffering reduces acute trauma shock
            if (!string.IsNullOrEmpty(child.assignedGuardianId))
            {
                sensitivity *= 0.75f;
            }

            child.traumaLoad = Math.Clamp(child.traumaLoad + (severity * sensitivity * 10.0f), 0.0f, 100.0f);
            OnFormativeEventRecorded?.Invoke(childId, eventTypeId, severity);
        }

        public void GrowthTick(int currentDay)
        {
            foreach (var child in _state.children)
            {
                if (child.adulthoodProcessed) continue;
                if (child.lastGrowthTickDay == currentDay) continue; // Idempotent per day

                child.lastGrowthTickDay = currentDay;

                // 1. Evaluate Needs & Starvation Stunting
                if (_needs != null)
                {
                    var survivorNeeds = _needs.Get(child.survivorId);
                    if (survivorNeeds != null)
                    {
                        if (survivorNeeds.Hunger >= 80.0f)
                        {
                            child.consecutiveStarvationDays++;
                            if (child.consecutiveStarvationDays >= 3)
                            {
                                child.stuntingSeverity = Math.Min(100.0f, child.stuntingSeverity + 10.0f);
                            }
                        }
                        else
                        {
                            child.consecutiveStarvationDays = 0;
                        }

                        child.nutritionScore = Math.Max(0.0f, 100.0f - (survivorNeeds.Hunger * 0.5f));
                        child.safetyScore = Math.Max(0.0f, survivorNeeds.Warmth * 0.5f + (100.0f - child.traumaLoad) * 0.5f);
                    }
                }

                // 2. Schoolhouse Education Progress
                if (_state.schoolhouseActive && !string.IsNullOrEmpty(child.assignedTeacherId))
                {
                    float dailyXp = 5.0f;
                    // Tools bonus: primer or slate adds education XP
                    if (_inventory.CountById("school_primer") > 0 || _inventory.CountById("slate_and_chalk") > 0)
                    {
                        dailyXp += 3.0f;
                    }
                    child.educationXp += dailyXp;
                }

                // 3. Development Progress Advance
                // Base growth + nutrition bonus - severe stunting penalty
                float dailyGrowth = 2.5f;
                if (child.nutritionScore > 70.0f) dailyGrowth += 0.5f;
                if (child.stuntingSeverity > 40.0f) dailyGrowth = Math.Max(1.0f, dailyGrowth - 1.0f);

                child.developmentProgress = Math.Min(100.0f, child.developmentProgress + dailyGrowth);

                // 4. Phase Transitions
                var oldPhase = child.developmentPhase;
                if (child.developmentProgress >= 100.0f)
                {
                    child.developmentPhase = DevelopmentPhase.AdultTransitioned;
                    ProcessAdulthood(child);
                }
                else if (child.developmentProgress >= 75.0f && child.developmentPhase < DevelopmentPhase.Adolescent)
                {
                    child.developmentPhase = DevelopmentPhase.Adolescent;
                }
                else if (child.developmentProgress >= 50.0f && child.developmentPhase < DevelopmentPhase.OlderChild)
                {
                    child.developmentPhase = DevelopmentPhase.OlderChild;
                }
                else if (child.developmentProgress >= 25.0f && child.developmentPhase < DevelopmentPhase.YoungChild)
                {
                    child.developmentPhase = DevelopmentPhase.YoungChild;
                }

                if (oldPhase != child.developmentPhase && child.developmentPhase != DevelopmentPhase.AdultTransitioned)
                {
                    OnPhaseAdvanced?.Invoke(child.survivorId, child.developmentPhase);
                }
            }
        }

        private void ProcessAdulthood(ChildDevelopment child)
        {
            if (child.adulthoodProcessed) return;
            child.adulthoodProcessed = true;
            _state.totalAdulthoodTransitions++;

            // Deterministic Trait Roll based on trauma, education, and stunting
            var acquired = new List<string>();
            var eligibleTraits = _traits.Values
                .Where(t => child.traumaLoad >= t.min_trauma && child.traumaLoad <= t.max_trauma)
                .Where(t => child.educationXp >= t.min_education)
                .OrderBy(t => t.trait_id, StringComparer.Ordinal)
                .ToList();

            if (child.stuntingSeverity >= 30.0f)
            {
                if (_traits.ContainsKey("development_trait_malnourished_growth"))
                {
                    acquired.Add("development_trait_malnourished_growth");
                }
            }

            var usedGroups = new HashSet<string>(StringComparer.Ordinal);
            foreach (var acq in acquired)
            {
                if (_traits.TryGetValue(acq, out var def) && !string.IsNullOrEmpty(def.exclusive_group))
                    usedGroups.Add(def.exclusive_group);
            }

            foreach (var trait in eligibleTraits)
            {
                if (acquired.Contains(trait.trait_id)) continue;
                if (!string.IsNullOrEmpty(trait.exclusive_group) && usedGroups.Contains(trait.exclusive_group)) continue;

                // Deterministic probability roll
                float roll = (float)_rng.NextDouble();
                if (roll <= (trait.weight * 0.75f))
                {
                    acquired.Add(trait.trait_id);
                    if (!string.IsNullOrEmpty(trait.exclusive_group))
                        usedGroups.Add(trait.exclusive_group);

                    if (acquired.Count >= 2) break; // Maximum 2 initial traits
                }
            }

            // Guarantee at least 1 trait if none qualified
            if (acquired.Count == 0 && eligibleTraits.Count > 0)
            {
                var fallback = eligibleTraits[0];
                acquired.Add(fallback.trait_id);
            }

            child.acquiredDevelopmentTraitIds = acquired;
            OnAdulthoodReached?.Invoke(child.survivorId, child.developmentPhase, acquired);
        }

        public float GetNeedsCaloricMultiplier(string survivorId)
        {
            var child = GetChild(survivorId);
            if (child == null || child.adulthoodProcessed) return 1.0f;

            return child.developmentPhase switch
            {
                DevelopmentPhase.Infant => 0.4f,
                DevelopmentPhase.YoungChild => 0.6f,
                DevelopmentPhase.OlderChild => 0.8f,
                DevelopmentPhase.Adolescent => 1.2f, // Growing teenagers burn elevated calories
                _ => 1.0f
            };
        }

        public float CalculateSettlementChildhoodMoraleBonus()
        {
            int safeCount = 0;
            foreach (var c in _state.children)
            {
                if (c.adulthoodProcessed) continue;
                if (c.safetyScore >= 60.0f && c.nutritionScore >= 60.0f && c.traumaLoad < 40.0f)
                {
                    safeCount++;
                }
            }
            // Bounded morale bonus: +3 per safe child, capped at +15
            return Math.Min(15.0f, safeCount * 3.0f);
        }

        public GenerationalState CaptureState() => _state;

        public void RestoreState(GenerationalState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
