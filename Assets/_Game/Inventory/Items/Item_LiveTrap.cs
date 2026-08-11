using System;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class LiveTrapState
    {
        public string itemId = "item_live_trap";
        public float plagueChance = 0.5f;
        public bool isSet = false;
        public int ratsCapturedToday = 0;
    }

    public class Item_LiveTrap
    {
        public event Action<string, int> OnRatsCaptured;          // survivorId, count
        public event Action<string, string> OnDiseaseContracted;  // survivorId, diseaseId
        public event Action<string, int> OnMeatObtained;          // survivorId, count

        private LiveTrapState _state;
        private string _trapId;

        public Item_LiveTrap(string trapId, LiveTrapState state = null)
        {
            _trapId = trapId ?? string.Empty;
            _state = state ?? new LiveTrapState();
        }

        public string ItemId => _state.itemId;
        public string TrapId => _trapId;

        public int SetTrap(string survivorId, System.Random rng)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Item_LiveTrap] SetTrap called with null/empty survivorId.");
                return 0;
            }

            // Random number of rats captured (0-3)
            int ratsCaptured = rng.Next(0, 4);
            _state.ratsCapturedToday = ratsCaptured;
            _state.isSet = true;

            if (ratsCaptured > 0)
            {
                OnRatsCaptured?.Invoke(survivorId, ratsCaptured);
            }

            return ratsCaptured;
        }

        public (int meat, bool diseaseContracted, string diseaseId) Butcher(string survivorId, int ratCount, System.Random rng)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Item_LiveTrap] Butcher called with null/empty survivorId.");
                return (0, false, string.Empty);
            }

            if (ratCount <= 0)
            {
                return (0, false, string.Empty);
            }

            // Each rat yields 1 meat
            int meatObtained = ratCount;
            OnMeatObtained?.Invoke(survivorId, meatObtained);

            // 50% chance to contract disease (plague or hantavirus)
            bool diseaseContracted = false;
            string diseaseId = string.Empty;

            if (rng.NextDouble() < _state.plagueChance)
            {
                diseaseContracted = true;
                // 50/50 between plague and hantavirus
                diseaseId = rng.NextDouble() < 0.5 ? "disease_plague" : "disease_hantavirus";
                OnDiseaseContracted?.Invoke(survivorId, diseaseId);
            }

            _state.isSet = false;
            _state.ratsCapturedToday = 0;

            return (meatObtained, diseaseContracted, diseaseId);
        }

        public bool IsSet() => _state.isSet;
        public int GetRatsCapturedToday() => _state.ratsCapturedToday;

        public LiveTrapState CaptureState()
        {
            return new LiveTrapState
            {
                itemId = _state.itemId,
                plagueChance = _state.plagueChance,
                isSet = _state.isSet,
                ratsCapturedToday = _state.ratsCapturedToday
            };
        }

        public void RestoreState(LiveTrapState state)
        {
            _state = state ?? new LiveTrapState();
        }
    }
}
