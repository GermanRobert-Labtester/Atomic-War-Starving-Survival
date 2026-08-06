using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MethaneState
    {
        public string hazardId;
        public float breachChance = 0.15f;
        public bool isGasPresent;
        public bool isDetonated;
    }

    public class MethaneSystem
    {
        private readonly MethaneState _state;

        public MethaneState State => _state;

        public event Action<string> OnMethaneBreached;   // hazardId
        public event Action<string, string> OnIgnition;    // hazardId, ignitionSource
        public event Action<string> OnRoomDestroyed;      // hazardId

        public MethaneSystem(string hazardId)
        {
            _state = new MethaneState
            {
                hazardId = hazardId,
                breachChance = 0.15f,
                isGasPresent = false,
                isDetonated = false
            };
        }

        /// <summary>
        /// Attempt excavation below Level 3. Returns true if a methane pocket is breached.
        /// </summary>
        public bool TryExcavate(int bunkerLevel, Random rng)
        {
            if (bunkerLevel <= 3)
                return false;

            if (rng.NextDouble() < _state.breachChance)
            {
                _state.isGasPresent = true;
                OnMethaneBreached?.Invoke(_state.hazardId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ignite gas with a flame/spark source (Torch, Heater, Firearm). Returns true if detonation occurs.
        /// </summary>
        public bool TryIgnite(string ignitionSource)
        {
            if (!_state.isGasPresent || _state.isDetonated)
                return false;

            _state.isDetonated = true;
            OnIgnition?.Invoke(_state.hazardId, ignitionSource);
            OnRoomDestroyed?.Invoke(_state.hazardId);
            return true;
        }

        public bool IsRoomDestroyed()
        {
            return _state.isDetonated;
        }
    }
}
