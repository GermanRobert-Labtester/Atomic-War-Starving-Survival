using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SlaughterPetState
    {
        public string actionId = "action_slaughter_pet";
        public int foodYield = 50;
        public bool traumaIsPermanent = true;
        public bool hasBeenUsed = false;
    }

    public class Action_SlaughterPet
    {
        public event Action<string, string, int> OnPetSlaughtered;    // survivorId, petType, foodYield
        public event Action<string, string> OnTraumaInflicted;          // survivorId, petName

        private SlaughterPetState _state;

        public Action_SlaughterPet(SlaughterPetState state = null)
        {
            _state = state ?? new SlaughterPetState();
        }

        public string ActionId => _state.actionId;

        public int Slaughter(string survivorId, string petType, string petName, List<string> allSurvivorIds)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Action_SlaughterPet] Slaughter called with null/empty survivorId.");
                return 0;
            }

            if (string.IsNullOrEmpty(petType))
            {
                Debug.LogWarning("[Action_SlaughterPet] Slaughter called with null/empty petType.");
                return 0;
            }

            // Yield massive food from the pet
            int foodGained = _state.foodYield;
            _state.hasBeenUsed = true;

            OnPetSlaughtered?.Invoke(survivorId, petType, foodGained);

            // Inflict permanent trauma on all survivors who loved the animal
            if (allSurvivorIds != null && allSurvivorIds.Count > 0)
            {
                foreach (string survivor in allSurvivorIds)
                {
                    if (!string.IsNullOrEmpty(survivor))
                    {
                        OnTraumaInflicted?.Invoke(survivor, petName ?? "the pet");
                    }
                }
            }

            return foodGained;
        }

        public bool HasBeenUsed() => _state.hasBeenUsed;
        public int GetFoodYield() => _state.foodYield;
        public bool IsTraumaPermanent() => _state.traumaIsPermanent;

        public SlaughterPetState CaptureState()
        {
            return new SlaughterPetState
            {
                actionId = _state.actionId,
                foodYield = _state.foodYield,
                traumaIsPermanent = _state.traumaIsPermanent,
                hasBeenUsed = _state.hasBeenUsed
            };
        }

        public void RestoreState(SlaughterPetState state)
        {
            _state = state ?? new SlaughterPetState();
        }
    }
}
