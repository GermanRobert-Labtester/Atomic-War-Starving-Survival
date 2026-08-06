using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GuardDogState
    {
        public string petId = "pet_guard_dog";
        public int meatRationsRequired = 2;
        public bool isMalnourished = false;
        public bool canFight = true;
        public int meatFedToday = 0;
        public int veggiesFedToday = 0;
    }

    public class Pet_GuardDog
    {
        public event Action<string> OnDogFed;          // shelterId
        public event Action<string> OnDogMalnourished; // shelterId
        public event Action<string> OnDogRecovered;    // shelterId

        private GuardDogState _state;
        private string _dogId;

        public Pet_GuardDog(string dogId, GuardDogState state = null)
        {
            _dogId = dogId ?? string.Empty;
            _state = state ?? new GuardDogState();
        }

        public string PetId => _state.petId;
        public string DogId => _dogId;

        public void Feed(string shelterId, string foodType, int quantity)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[Pet_GuardDog] Feed called with null/empty shelterId.");
                return;
            }

            if (string.IsNullOrEmpty(foodType) || quantity <= 0)
            {
                return;
            }

            if (foodType.ToLower().Contains("meat"))
            {
                _state.meatFedToday += quantity;
            }
            else if (foodType.ToLower().Contains("vegetable") || foodType.ToLower().Contains("fungi"))
            {
                _state.veggiesFedToday += quantity;
            }

            OnDogFed?.Invoke(shelterId);
        }

        public void TickDay(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[Pet_GuardDog] TickDay called with null/empty shelterId.");
                return;
            }

            bool wasMalnourished = _state.isMalnourished;

            // Check if properly fed with meat
            if (_state.meatFedToday >= _state.meatRationsRequired)
            {
                // Well fed - can fight
                _state.isMalnourished = false;
                _state.canFight = true;

                if (wasMalnourished)
                {
                    OnDogRecovered?.Invoke(shelterId);
                }
            }
            else if (_state.veggiesFedToday > 0 && _state.meatFedToday < _state.meatRationsRequired)
            {
                // Fed only vegetables/fungi - becomes malnourished
                _state.isMalnourished = true;
                _state.canFight = false;

                if (!wasMalnourished)
                {
                    OnDogMalnourished?.Invoke(shelterId);
                }
            }
            else if (_state.meatFedToday == 0 && _state.veggiesFedToday == 0)
            {
                // Not fed at all - becomes malnourished
                _state.isMalnourished = true;
                _state.canFight = false;

                if (!wasMalnourished)
                {
                    OnDogMalnourished?.Invoke(shelterId);
                }
            }

            // Reset daily counters
            _state.meatFedToday = 0;
            _state.veggiesFedToday = 0;
        }

        public bool CanFightInRaid() => _state.canFight && !_state.isMalnourished;
        public bool IsMalnourished() => _state.isMalnourished;

        public GuardDogState CaptureState()
        {
            return new GuardDogState
            {
                petId = _state.petId,
                meatRationsRequired = _state.meatRationsRequired,
                isMalnourished = _state.isMalnourished,
                canFight = _state.canFight,
                meatFedToday = _state.meatFedToday,
                veggiesFedToday = _state.veggiesFedToday
            };
        }

        public void RestoreState(GuardDogState state)
        {
            _state = state ?? new GuardDogState();
        }
    }
}
