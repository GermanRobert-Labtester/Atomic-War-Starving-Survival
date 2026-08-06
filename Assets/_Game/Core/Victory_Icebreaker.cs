using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class IcebreakerState
    {
        public string victoryId = "victory_icebreaker";
        public bool isActive = false;
        public bool submarineContacted = false;
        public int explosivesRequired = 100;
        public int explosivesDelivered = 0;
        public int distanceToSubNodes = 30;
        public int extractionDayLimit = 0;
        public bool isExtracted = false;
    }

    /// <summary>
    /// Prompt #559: Endgame: The Icebreaker (Submarine).
    /// Via HamRadio, contact a nuclear submarine 30 nodes away. Drag 100 Explosives
    /// across the map to blast through the ice sheet and board the sub before a faction army arrives.
    /// </summary>
    public class Victory_Icebreaker
    {
        private IcebreakerState _state = new IcebreakerState();

        public event Action<IcebreakerState> OnSubmarineContacted;
        public event Action<IcebreakerState, int> OnExplosivesDelivered;
        public event Action<IcebreakerState> OnIcebreakerExtracted;
        public event Action<IcebreakerState, string> OnExtractionFailed;

        public IcebreakerState State => _state;

        public bool ContactSubmarine(bool hasHamRadio)
        {
            if (!hasHamRadio || _state.submarineContacted) return false;

            _state.submarineContacted = true;
            _state.isActive = true;
            OnSubmarineContacted?.Invoke(_state);
            return true;
        }

        public int DeliverExplosives(int count)
        {
            if (!_state.isActive || !_state.submarineContacted) return _state.explosivesDelivered;

            _state.explosivesDelivered = Math.Min(_state.explosivesDelivered + count, _state.explosivesRequired);
            OnExplosivesDelivered?.Invoke(_state, _state.explosivesDelivered);
            return _state.explosivesDelivered;
        }

        public bool CheckExtraction(int currentDay, int explosivesDelivered, bool factionArmyArrived)
        {
            if (!_state.isActive) return false;

            _state.explosivesDelivered = explosivesDelivered;

            if (factionArmyArrived && _state.explosivesDelivered < _state.explosivesRequired)
            {
                OnExtractionFailed?.Invoke(_state, "faction_army_arrived_before_delivery");
                _state.isActive = false;
                return false;
            }

            if (_state.explosivesDelivered >= _state.explosivesRequired && !factionArmyArrived)
            {
                _state.isExtracted = true;
                OnIcebreakerExtracted?.Invoke(_state);
                return true;
            }

            if (_state.extractionDayLimit > 0 && currentDay > _state.extractionDayLimit)
            {
                OnExtractionFailed?.Invoke(_state, "extraction_day_limit_exceeded");
                _state.isActive = false;
                return false;
            }

            return false;
        }

        public bool IsVictoryAchieved()
        {
            return _state.isExtracted;
        }
    }
}
