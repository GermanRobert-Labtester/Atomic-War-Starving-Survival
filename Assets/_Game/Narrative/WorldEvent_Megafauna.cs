using System;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class MegafaunaState
    {
        public string eventId = "world_event_megafauna";
        public string displayName = "Megafauna Migration";
        public bool isActive = false;
        public string currentNodeId = "";
        public int daysToCrossMap = 7;
        public int daysRemaining = 0;
        public float meatYield = 200f;
        public bool requiresExplosives = true;
    }

    /// <summary>
    /// Prompt #650: World Event — Megafauna Migration.
    /// A herd of massive mutants moves across the map over a week, wiping Factions
    /// from nodes it passes through. Huntable for enormous meat yields, but only
    /// with Explosives.
    /// </summary>
    public class WorldEvent_Megafauna
    {
        private MegafaunaState _state = new MegafaunaState();

        // -- Events --
        public event Action<MegafaunaState> OnMegafaunaArrived;
        public event Action<MegafaunaState, string> OnNodeWiped;
        public event Action<MegafaunaState> OnMegafaunaDeparted;
        public event Action<MegafaunaState, float> OnHuntSuccessful;
        public event Action<MegafaunaState> OnHuntFailed;

        public MegafaunaState State => _state;

        /// <summary>
        /// Triggers the megafauna migration starting at the given node.
        /// </summary>
        public void Trigger(string startNodeId)
        {
            _state.isActive = true;
            _state.currentNodeId = startNodeId ?? "";
            _state.daysRemaining = _state.daysToCrossMap;

            OnMegafaunaArrived?.Invoke(_state);
        }

        /// <summary>
        /// Daily tick: the herd moves to the next node, wiping any faction there.
        /// Pass the next node id along the migration path.
        /// </summary>
        public void TickDay(string nextNodeId)
        {
            if (!_state.isActive) return;

            // Wipe the faction at the current node
            string wipedNode = _state.currentNodeId;
            OnNodeWiped?.Invoke(_state, wipedNode);

            _state.currentNodeId = nextNodeId ?? "";
            _state.daysRemaining = Math.Max(0, _state.daysRemaining - 1);

            if (_state.daysRemaining <= 0)
            {
                _state.isActive = false;
                OnMegafaunaDeparted?.Invoke(_state);
            }
        }

        /// <summary>
        /// Attempt to hunt the megafauna at the current node.
        /// Requires sufficient combat power and explosives.
        /// Returns meat yield on success, 0 on failure.
        /// </summary>
        public float Hunt(float combatPower, bool hasExplosives, System.Random rng)
        {
            if (!_state.isActive) return 0f;

            if (_state.requiresExplosives && !hasExplosives)
            {
                OnHuntFailed?.Invoke(_state);
                return 0f;
            }

            // Base success chance scales with combat power; explosives are mandatory
            float successChance = Math.Min(1f, combatPower / 100f);
            if (rng != null && rng.NextDouble() > successChance)
            {
                OnHuntFailed?.Invoke(_state);
                return 0f;
            }

            float yield_ = _state.meatYield;
            OnHuntSuccessful?.Invoke(_state, yield_);
            return yield_;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public MegafaunaState GetState() => _state;

        // ── Save / Load ────────────────────────────────────────────────


        public MegafaunaState CaptureState() => _state;



        public void RestoreState(MegafaunaState state)
        {
            _state = state ?? new MegafaunaState();
        }

}
}
