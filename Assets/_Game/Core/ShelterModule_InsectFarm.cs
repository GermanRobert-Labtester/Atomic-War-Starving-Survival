using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class InsectFarmState
    {
        public string moduleId = "shelter_module_insect_farm";
        public float proteinPerDay = 5f;
        public float noiseFromChirping = 0.6f;
        public float requiresHumidity = 0.8f;
        public float requiresHeat = 0.7f;
        public float totalProteinHarvested = 0f;
        public bool isActive = false;
    }

    public class ShelterModule_InsectFarm
    {
        public event Action<string, float> OnProteinHarvested;    // shelterId, amount
        public event Action<string> OnChirpingDisturbed;          // survivorId

        private InsectFarmState _state;

        public ShelterModule_InsectFarm(InsectFarmState state = null)
        {
            _state = state ?? new InsectFarmState();
        }

        public string ModuleId => _state.moduleId;

        public void TickDay(string shelterId, float humidity, float heat, List<string> vulnerableSurvivors)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[ShelterModule_InsectFarm] TickDay called with null/empty shelterId.");
                return;
            }

            // Check environmental conditions
            bool conditionsMet = humidity >= _state.requiresHumidity && heat >= _state.requiresHeat;

            if (conditionsMet)
            {
                _state.isActive = true;

                // Produce protein from crickets
                _state.totalProteinHarvested += _state.proteinPerDay;
                OnProteinHarvested?.Invoke(shelterId, _state.proteinPerDay);

                // Chirping disturbs vulnerable survivors (insomniacs, paranoids)
                if (vulnerableSurvivors != null && vulnerableSurvivors.Count > 0)
                {
                    foreach (string survivorId in vulnerableSurvivors)
                    {
                        if (!string.IsNullOrEmpty(survivorId))
                        {
                            OnChirpingDisturbed?.Invoke(survivorId);
                        }
                    }
                }
            }
            else
            {
                _state.isActive = false;
            }
        }

        public bool IsActive() => _state.isActive;
        public float GetTotalProteinHarvested() => _state.totalProteinHarvested;
        public float GetNoiseLevel() => _state.noiseFromChirping;

        public InsectFarmState CaptureState()
        {
            return new InsectFarmState
            {
                moduleId = _state.moduleId,
                proteinPerDay = _state.proteinPerDay,
                noiseFromChirping = _state.noiseFromChirping,
                requiresHumidity = _state.requiresHumidity,
                requiresHeat = _state.requiresHeat,
                totalProteinHarvested = _state.totalProteinHarvested,
                isActive = _state.isActive
            };
        }

        public void RestoreState(InsectFarmState state)
        {
            _state = state ?? new InsectFarmState();
        }
    }
}
