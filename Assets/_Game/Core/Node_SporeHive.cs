using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SporeHiveState
    {
        public string nodeId = "node_spore_hive";
        public string displayName = "Spore Hive";
        public float toxicGasLevel = 80f;
        public int podCount = 5;
        public int podsOpened = 0;
        public int lootPerPod = 3;
        public float sporeReleaseChance = 0.40f;
    }

    /// <summary>
    /// Prompt #655: Node — Spore Hive.
    /// A ruined skyscraper overtaken by fungal biomass. Extreme ToxicGas fills the area.
    /// "Pods" containing pre-war humans hold pristine loot, but opening them risks
    /// lethal Spore release.
    /// </summary>
    public class Node_SporeHive
    {
        private SporeHiveState _state = new SporeHiveState();

        // -- Events --
        public event Action<SporeHiveState> OnNodeEntered;
        public event Action<SporeHiveState, int> OnPodOpened;
        public event Action<SporeHiveState, string> OnSporeReleased;
        public event Action<SporeHiveState> OnAllPodsExhausted;

        public SporeHiveState State => _state;

        /// <summary>Toxic gas level on entry — always hazardous.</summary>
        public float ToxicGasLevel => _state.toxicGasLevel;

        public int PodsRemaining => _state.podCount - _state.podsOpened;

        /// <summary>
        /// Called when the player enters the node. Fires the entry event
        /// with the current toxic gas level.
        /// </summary>
        public void EnterNode()
        {
            OnNodeEntered?.Invoke(_state);
        }

        /// <summary>
        /// Attempts to open the next available pod. Returns a result tuple:
        /// loot (number of items gained) and sporeAfflictionId (null if no
        /// spores were released, or the affliction id if they were).
        /// </summary>
        public (int loot, string sporeAfflictionId) TryOpenPod(System.Random rng)
        {
            if (PodsRemaining <= 0)
            {
                return (0, null);
            }

            _state.podsOpened++;
            int loot = _state.lootPerPod;

            OnPodOpened?.Invoke(_state, loot);

            // Check for spore release
            string afflictionId = null;
            if (rng != null && rng.NextDouble() < _state.sporeReleaseChance)
            {
                afflictionId = "spore_infection";
                OnSporeReleased?.Invoke(_state, afflictionId);
            }

            if (PodsRemaining <= 0)
            {
                OnAllPodsExhausted?.Invoke(_state);
            }

            return (loot, afflictionId);
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SporeHiveState GetState() => _state;

        public void RestoreState(SporeHiveState state)
        {
            _state = state ?? new SporeHiveState();
        }
    }
}
