using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AshGoatState
    {
        public string itemId = "item_ash_goat";
        public float noiseLevel = 0.9f;
        public float smellLevel = 0.9f;
        public float milkPerDay = 2f;
        public bool isFed = false;
        public float totalMilkProduced = 0f;
    }

    public class Item_AshGoat
    {
        /// <summary>
        /// MISC-005: seeded stream so this system's rolls replay identically. The
        /// call sites below previously used wall-clock UnityEngine.Random, which made
        /// the same save produce different outcomes on each load.
        /// </summary>
        private static readonly System.Random FallbackRng =
            AtomicWar._Game.Utilities.SeededRandom.CreateFixed("item_ashgoat");

        public event Action<string, float> OnMilkProduced;      // shelterId, amount
        public event Action<string, float> OnNoiseGenerated;    // shelterId, noise
        public event Action<string> OnPredatorAttracted;        // shelterId

        private AshGoatState _state;
        private string _goatId;

        public Item_AshGoat(string goatId, AshGoatState state = null)
        {
            _goatId = goatId ?? string.Empty;
            _state = state ?? new AshGoatState();
        }

        public string ItemId => _state.itemId;
        public string GoatId => _goatId;

        public void Feed(string shelterId, string wasteType)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[Item_AshGoat] Feed called with null/empty shelterId.");
                return;
            }

            // Goat eats anything - waste, spoiled food, etc.
            if (!string.IsNullOrEmpty(wasteType))
            {
                _state.isFed = true;
            }
        }

        public void TickDay(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[Item_AshGoat] TickDay called with null/empty shelterId.");
                return;
            }

            if (_state.isFed)
            {
                // Produce milk
                _state.totalMilkProduced += _state.milkPerDay;
                OnMilkProduced?.Invoke(shelterId, _state.milkPerDay);

                // Generate noise and smell (attracts predators)
                OnNoiseGenerated?.Invoke(shelterId, _state.noiseLevel);

                // High chance to attract predators due to noise/smell
                float predatorChance = (_state.noiseLevel + _state.smellLevel) * 0.5f;
                if (FallbackRng.NextDouble() < predatorChance)
                {
                    OnPredatorAttracted?.Invoke(shelterId);
                }

                // Reset fed status for next day
                _state.isFed = false;
            }
        }

        public float GetTotalMilkProduced() => _state.totalMilkProduced;
        public float GetNoiseLevel() => _state.noiseLevel;
        public float GetSmellLevel() => _state.smellLevel;

        public AshGoatState CaptureState()
        {
            return new AshGoatState
            {
                itemId = _state.itemId,
                noiseLevel = _state.noiseLevel,
                smellLevel = _state.smellLevel,
                milkPerDay = _state.milkPerDay,
                isFed = _state.isFed,
                totalMilkProduced = _state.totalMilkProduced
            };
        }

        public void RestoreState(AshGoatState state)
        {
            _state = state ?? new AshGoatState();
        }
    }
}
