using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public sealed class TravelEncounterSystem
    {
        private readonly TravelEncounterCatalog _catalog;
        private readonly Dictionary<string, int> _chainStages = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _encounterAvailableDay = new(StringComparer.OrdinalIgnoreCase);

        public TravelEncounterCatalog Catalog => _catalog;

        public event Action<string, string>? OnChoiceResolved;
        public event Action<string, int>? OnChainStageAdvanced;

        public TravelEncounterSystem(TravelEncounterCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public int GetChainStage(string chainId)
        {
            if (string.IsNullOrEmpty(chainId)) return 0;
            return _chainStages.TryGetValue(chainId, out int stage) ? stage : 0;
        }

        public void SetChainStage(string chainId, int stage)
        {
            if (string.IsNullOrEmpty(chainId)) return;
            _chainStages[chainId] = stage;
            OnChainStageAdvanced?.Invoke(chainId, stage);
        }

        public bool IsEncounterEligible(TravelEncounterDefinition encounter, string region, float dangerLevel, string currentSeason, int currentDay)
        {
            if (encounter == null) return false;

            // Cooldown check
            if (_encounterAvailableDay.TryGetValue(encounter.Id, out int nextDay) && currentDay < nextDay)
            {
                return false;
            }

            // Danger level range
            if (dangerLevel < encounter.MinDangerLevel || dangerLevel > encounter.MaxDangerLevel)
            {
                return false;
            }

            // Region filter
            if (encounter.RegionTags != null && encounter.RegionTags.Count > 0 && !string.IsNullOrEmpty(region))
            {
                if (!encounter.RegionTags.Exists(r => string.Equals(r, region, StringComparison.OrdinalIgnoreCase) || string.Equals(r, "all", StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }

            // Season filter
            if (encounter.SeasonTags != null && encounter.SeasonTags.Count > 0 && !string.IsNullOrEmpty(currentSeason))
            {
                if (!encounter.SeasonTags.Exists(s => string.Equals(s, currentSeason, StringComparison.OrdinalIgnoreCase) || string.Equals(s, "all", StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }

            // Chain progression filter
            if (!string.IsNullOrEmpty(encounter.ChainId))
            {
                int currentStage = GetChainStage(encounter.ChainId);
                if (encounter.PrereqChainStage != currentStage)
                {
                    return false;
                }
            }

            return true;
        }

        public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance)
        {
            if (encounter == null) return 0f;
            float weight = encounter.BaseWeight;

            if (!string.IsNullOrEmpty(stance) && encounter.StanceWeights != null && encounter.StanceWeights.TryGetValue(stance, out float multiplier))
            {
                weight *= multiplier;
            }

            return Math.Max(0.01f, weight);
        }

        public TravelEncounterDefinition? SelectEncounter(string region, float dangerLevel, string stance, string currentSeason, int currentDay, ISeededRng rng)
        {
            var eligible = new List<TravelEncounterDefinition>();
            var weights = new List<float>();
            float totalWeight = 0f;

            foreach (var encounter in _catalog.Encounters)
            {
                if (IsEncounterEligible(encounter, region, dangerLevel, currentSeason, currentDay))
                {
                    float w = GetEffectiveWeight(encounter, stance);
                    eligible.Add(encounter);
                    weights.Add(w);
                    totalWeight += w;
                }
            }

            if (eligible.Count == 0 || totalWeight <= 0f)
            {
                return null;
            }

            float roll = (float)(rng.NextDouble() * totalWeight);
            float accum = 0f;

            for (int i = 0; i < eligible.Count; i++)
            {
                accum += weights[i];
                if (roll <= accum || i == eligible.Count - 1)
                {
                    return eligible[i];
                }
            }

            return eligible[0];
        }

        public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out int moraleDelta, out int guiltDelta, out string unlockedFieldGuideId)
        {
            moraleDelta = 0;
            guiltDelta = 0;
            unlockedFieldGuideId = string.Empty;

            if (!_catalog.TryGetEncounter(encounterId, out var encounter))
            {
                return false;
            }

            TravelEncounterChoice? selectedChoice = null;
            foreach (var c in encounter.Choices)
            {
                if (string.Equals(c.ChoiceId, choiceId, StringComparison.OrdinalIgnoreCase))
                {
                    selectedChoice = c;
                    break;
                }
            }

            if (selectedChoice == null)
            {
                return false;
            }

            moraleDelta = selectedChoice.MoraleDelta;
            guiltDelta = selectedChoice.GuiltDelta;
            unlockedFieldGuideId = selectedChoice.UnlocksFieldGuideId;

            // Set cooldown for this encounter
            _encounterAvailableDay[encounterId] = currentDay + 5;

            // Advance chain if applicable
            if (!string.IsNullOrEmpty(encounter.ChainId) && selectedChoice.AdvancesChainStage > 0)
            {
                SetChainStage(encounter.ChainId, selectedChoice.AdvancesChainStage);
            }

            OnChoiceResolved?.Invoke(encounterId, choiceId);
            return true;
        }

        public TravelEncounterState CaptureState()
        {
            return new TravelEncounterState
            {
                ChainStages = new Dictionary<string, int>(_chainStages, StringComparer.OrdinalIgnoreCase),
                EncounterAvailableDay = new Dictionary<string, int>(_encounterAvailableDay, StringComparer.OrdinalIgnoreCase)
            };
        }

        public void RestoreState(TravelEncounterState? state)
        {
            _chainStages.Clear();
            _encounterAvailableDay.Clear();
            if (state == null) return;

            if (state.ChainStages != null)
            {
                foreach (var kvp in state.ChainStages)
                {
                    _chainStages[kvp.Key] = kvp.Value;
                }
            }

            if (state.EncounterAvailableDay != null)
            {
                foreach (var kvp in state.EncounterAvailableDay)
                {
                    _encounterAvailableDay[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
