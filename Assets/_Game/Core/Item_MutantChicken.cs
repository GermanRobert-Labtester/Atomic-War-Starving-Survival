using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MutantChickenState
    {
        public string itemId = "item_mutant_chicken";
        public bool isFed = false;
        public bool isFeral = false;
        public int eggProductionPerDay = 1;
        public int eggsLaidToday = 0;
    }

    public class Item_MutantChicken
    {
        /// <summary>
        /// MISC-005: seeded stream so this system's rolls replay identically. The
        /// call sites below previously used wall-clock UnityEngine.Random, which made
        /// the same save produce different outcomes on each load.
        /// </summary>
        private static System.Random _fallbackRng;
    private static System.Random FallbackRng =>
        _fallbackRng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("item_mutantchicken");

        public event Action<string> OnEggLaid;           // shelterId
        public event Action<string> OnChickenWentFeral;   // shelterId
        public event Action<string, string> OnSurvivorAttacked; // chickenId, survivorId

        private MutantChickenState _state;
        private string _chickenId;
        private int _totalEggs;

        public Item_MutantChicken(string chickenId, MutantChickenState state = null)
        {
            _chickenId = chickenId ?? string.Empty;
            _state = state ?? new MutantChickenState();
            _totalEggs = 0;
        }

        public string ItemId => _state.itemId;
        public string ChickenId => _chickenId;

        public void Feed(string shelterId, bool hasFungi)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[Item_MutantChicken] Feed called with null/empty shelterId.");
                return;
            }

            if (hasFungi)
            {
                _state.isFed = true;
                if (_state.isFeral)
                {
                    // Fed chicken calms down from feral state
                    _state.isFeral = false;
                }
            }
        }

        public void TickDay(string shelterId, List<string> survivorIds = null)
        {
            if (string.IsNullOrEmpty(shelterId))
            {
                Debug.LogWarning("[Item_MutantChicken] TickDay called with null/empty shelterId.");
                return;
            }

            if (!_state.isFed)
            {
                // Not fed — goes feral and attacks a random survivor
                if (!_state.isFeral)
                {
                    _state.isFeral = true;
                    OnChickenWentFeral?.Invoke(shelterId);
                }

                // Attack a survivor if any are present
                if (survivorIds != null && survivorIds.Count > 0)
                {
                    int targetIndex = FallbackRng.Next(0, survivorIds.Count);
                    OnSurvivorAttacked?.Invoke(_chickenId, survivorIds[targetIndex]);
                }
            }
            else
            {
                // Fed — lays eggs
                _totalEggs += _state.eggProductionPerDay;
                _state.eggsLaidToday = _state.eggProductionPerDay;

                for (int i = 0; i < _state.eggProductionPerDay; i++)
                {
                    OnEggLaid?.Invoke(shelterId);
                }

                // Reset fed status for next day
                _state.isFed = false;
            }
        }

        public bool IsFeral() => _state.isFeral;

        public int GetEggCount() => _totalEggs;

        public MutantChickenState CaptureState()
        {
            return new MutantChickenState
            {
                itemId = _state.itemId,
                isFed = _state.isFed,
                isFeral = _state.isFeral,
                eggProductionPerDay = _state.eggProductionPerDay,
                eggsLaidToday = _state.eggsLaidToday
            };
        }

        public void RestoreState(MutantChickenState state)
        {
            _state = state ?? new MutantChickenState();
        }
    }
}
