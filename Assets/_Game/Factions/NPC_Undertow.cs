using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_UndertowState
    {
        public string id = "faction_undertow";
        public string displayName = "The Undertow";
        public bool isActive;
        /// <summary>True once the Kittiwake chart has been copied and circulated.</summary>
        public bool chartDistributed;
        /// <summary>Base accident rate on Drown expeditions; rises after the chart circulates.</summary>
        public float salvageAccidentRisk = 0.1f;
        /// <summary>Rescue price multiplier: agreed after you are already in the water.</summary>
        public float rescueFeeMultiplier = 1f;
        /// <summary>Times the player's crew has been rescued (each one is a data point).</summary>
        public int rescuesPerformed;
    }

    /// <summary>
    /// Lore bible 05_FACTIONS §8 — The Undertow (dangerous, deniable Current).
    /// Wreckers. They salvage the accidents that happen in the Drown, and the
    /// accidents happen at a rate that is difficult to explain by water alone.
    /// They do not present as a faction, ever. They present as helpful
    /// strangers who arrive very quickly. They have never attacked anyone.
    /// </summary>
    public class NPC_Undertow
    {
        private NPC_UndertowState _state = new NPC_UndertowState();

        public event Action<NPC_UndertowState> OnChartCirculated;
        public event Action<NPC_UndertowState, float> OnRescueOffered;

        public NPC_UndertowState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>
        /// The Kittiwake chart: copy it and distribute it and the Drown becomes
        /// navigable for everyone — which ends the Undertow's business model
        /// permanently. They do not attack. Expeditions simply start having
        /// accidents, and every single time, someone is lucky enough to be close.
        /// </summary>
        public void ChartDistributed()
        {
            if (_state.chartDistributed) return;
            _state.chartDistributed = true;
            _state.salvageAccidentRisk = 0.5f;
            OnChartCirculated?.Invoke(_state);
        }

        /// <summary>
        /// A rescue offer: real, delivered, at a price agreed after the fact.
        /// Returns the fee multiplier for this rescue.
        /// </summary>
        public float OfferRescue()
        {
            _state.rescuesPerformed++;
            float fee = _state.rescueFeeMultiplier * (1f + 0.2f * _state.rescuesPerformed);
            OnRescueOffered?.Invoke(_state, fee);
            return fee;
        }

        public NPC_UndertowState CaptureState() => _state;
        public void RestoreState(NPC_UndertowState saved) { _state = saved ?? new NPC_UndertowState(); }
    }
}
