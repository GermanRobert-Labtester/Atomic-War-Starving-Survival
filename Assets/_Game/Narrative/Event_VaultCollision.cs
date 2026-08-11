using System;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class VaultCollisionState
    {
        public string eventId;
        public float collisionChance = 0.05f;
        public bool hasCollided;
        public string neighborState = string.Empty; // "dead" | "starving" | "hostile"
    }

    public class VaultCollisionSystem
    {
        private readonly VaultCollisionState _state;
        private static readonly string[] NeighborStates = { "dead", "starving", "hostile" };

        public VaultCollisionState State => _state;

        public event Action<string, string> OnCollision;       // eventId, neighborState
        public event Action<string, string> OnLootOrThreat;    // eventId, outcome

        public VaultCollisionSystem(string eventId)
        {
            _state = new VaultCollisionState
            {
                eventId = eventId,
                collisionChance = 0.05f,
                hasCollided = false,
                neighborState = string.Empty
            };
        }

        /// <summary>
        /// Rare chance to break into another shelter when digging laterally.
        /// Returns true if a collision occurs.
        /// </summary>
        public bool TryDigLaterally(Random rng)
        {
            if (_state.hasCollided)
                return false;

            if (rng.NextDouble() < _state.collisionChance)
            {
                _state.hasCollided = true;
                _state.neighborState = NeighborStates[rng.Next(NeighborStates.Length)];
                OnCollision?.Invoke(_state.eventId, _state.neighborState);
                return true;
            }
            return false;
        }

        public string GetNeighborState()
        {
            return _state.neighborState;
        }

        /// <summary>
        /// Returns outcome description based on neighbor state.
        /// </summary>
        public string GetLootOrThreat()
        {
            string outcome;
            switch (_state.neighborState)
            {
                case "dead":     outcome = "free_loot"; break;
                case "starving": outcome = "begging"; break;
                case "hostile":  outcome = "turf_war"; break;
                default:         outcome = "none"; break;
            }
            OnLootOrThreat?.Invoke(_state.eventId, outcome);
            return outcome;
        }

        public VaultCollisionState CaptureState()
        {
            return new VaultCollisionState
            {
                eventId = _state.eventId,
                collisionChance = _state.collisionChance,
                hasCollided = _state.hasCollided,
                neighborState = _state.neighborState,
            };
        }

        public void RestoreState(VaultCollisionState saved)
        {
            if (saved == null) return;
            _state.eventId = saved.eventId;
            _state.collisionChance = saved.collisionChance;
            _state.hasCollided = saved.hasCollided;
            _state.neighborState = saved.neighborState;
        }
    }
}
