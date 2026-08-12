using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class MutantHiveState
    {
        public string node_id = "node_mutant_hive";
        public bool is_discovered = false;
        public bool webbing_active = true;
        public float speed_multiplier = 0.5f;
        public int cocoons_total = 0;
        public int cocoons_opened = 0;
        public float swarm_spawn_chance = 0.5f;
        public List<string> cocoon_ids = new List<string>();
        public List<string> looted_cocoon_ids = new List<string>();
    }

    /// <summary>
    /// Prompt #851: Mutant Hive — Subway station overtaken by insectoid
    /// mutations. Floor webbing halves speed. Pristine loot in cocoons;
    /// cutting 50% spawns a Swarm encounter.
    /// </summary>
    public sealed class Node_MutantHive
    {
        private MutantHiveState _state;
        private readonly System.Random _rng;

        public event Action<string> OnDiscovered;                      // node_id
        public event Action OnExpeditionEntered;
        public event Action<float> OnWebbingSlowed;                    // multiplier
        public event Action<string, bool> OnCocoonCut;                  // cocoon_id, swarm_spawned
        public event Action OnSwarmEncounter;
        public event Action<string, string> OnLootRetrieved;            // cocoon_id, loot_id

        public string NodeId => _state.node_id;

        public Node_MutantHive() : this(SeededRandom.Create(SeededRandom.WorldSeed, "node_mutanthive")) { }

        public Node_MutantHive(System.Random rng)
        {
            _state = new MutantHiveState();
            _rng = rng ?? SeededRandom.Create(SeededRandom.WorldSeed, "node_mutanthive");
        }

        /// <summary>
        /// Marks the mutant hive as discovered at the given node.
        /// </summary>
        public void Discover(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[Node_MutantHive] node_id is null or empty.");
                return;
            }

            _state.node_id = node_id;
            _state.is_discovered = true;

            OnDiscovered?.Invoke(node_id);
            GameLog.Log($"[Node_MutantHive] Discovered at node '{node_id}'.");
        }

        /// <summary>
        /// Begins an expedition into the hive. Webbing slows movement to
        /// half speed.
        /// </summary>
        public void EnterExpedition()
        {
            _state.webbing_active = true;
            OnExpeditionEntered?.Invoke();
            OnWebbingSlowed?.Invoke(_state.speed_multiplier);
            GameLog.Log($"[Node_MutantHive] Expedition entered. Speed multiplier: {_state.speed_multiplier:F1}.");
        }

        /// <summary>
        /// Cuts open a cocoon. Has a 50% chance to spawn a swarm encounter.
        /// The survivor must fight the swarm to keep the loot.
        /// </summary>
        public bool CutCocoon(string cocoon_id, float combat_skill)
        {
            if (string.IsNullOrEmpty(cocoon_id))
            {
                Debug.LogError("[Node_MutantHive] cocoon_id is null or empty.");
                return false;
            }

            if (_state.looted_cocoon_ids.Contains(cocoon_id))
            {
                Debug.LogWarning($"[Node_MutantHive] Cocoon '{cocoon_id}' already opened.");
                return false;
            }

            _state.cocoons_opened++;
            _state.looted_cocoon_ids.Add(cocoon_id);

            bool swarm_spawned = CheckSwarmSpawn();
            OnCocoonCut?.Invoke(cocoon_id, swarm_spawned);

            if (swarm_spawned)
            {
                OnSwarmEncounter?.Invoke();
                GameLog.Log($"[Node_MutantHive] Swarm spawned from cocoon '{cocoon_id}'!");
            }

            return !swarm_spawned || combat_skill >= 0.5f;
        }

        /// <summary>
        /// Rolls the swarm spawn chance (default 50%).
        /// </summary>
        public bool CheckSwarmSpawn()
        {
            return (float)_rng.NextDouble() < _state.swarm_spawn_chance;
        }

        /// <summary>
        /// Retrieves the loot id from an opened cocoon. Returns null if the
        /// cocoon has not been opened.
        /// </summary>
        public string GetLootFromCocoon(string cocoon_id)
        {
            if (string.IsNullOrEmpty(cocoon_id))
                return null;

            if (!_state.looted_cocoon_ids.Contains(cocoon_id))
                return null;

            // Loot id derived from cocoon id — pristine pre-war loot
            string loot_id = $"loot_pristine_{cocoon_id}";
            OnLootRetrieved?.Invoke(cocoon_id, loot_id);
            return loot_id;
        }

        public MutantHiveState CaptureState()
        {
            return new MutantHiveState
            {
                node_id = _state.node_id,
                is_discovered = _state.is_discovered,
                webbing_active = _state.webbing_active,
                speed_multiplier = _state.speed_multiplier,
                cocoons_total = _state.cocoons_total,
                cocoons_opened = _state.cocoons_opened,
                swarm_spawn_chance = _state.swarm_spawn_chance,
                cocoon_ids = new List<string>(_state.cocoon_ids),
                looted_cocoon_ids = new List<string>(_state.looted_cocoon_ids)
            };
        }

        public void RestoreState(MutantHiveState saved)
        {
            _state = saved ?? new MutantHiveState();
        }
    }
}
