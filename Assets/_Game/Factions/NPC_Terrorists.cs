using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class TerroristState
    {
        public string id = "npc_terrorists";
        public string displayName = "The Terrorists (Nihilists)";
        public float hp = 60f;
        public bool isSuicideVestEquipped = true;
        public bool hasDetonated = false;
        public float detonationAoeDamage = 80f;
        public bool allLootDestroyed = false;
    }

    /// <summary>
    /// Prompt #334: Faction: The Terrorists (Nihilists).
    /// Hostile to all factions. Equipped with Suicide Vests. Reaching 0 HP triggers a detonation
    /// that deals massive AoE damage and destroys all loot in the encounter.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_Terrorists
    {
        private TerroristState _state = new TerroristState();

        public event Action<TerroristState, float> OnSuicideVestDetonated;

        public TerroristState State => _state;

        public float TakeDamage(float amount)
        {
            if (_state.hp <= 0f) return 0f;

            _state.hp -= amount;
            if (_state.hp <= 0f && _state.isSuicideVestEquipped && !_state.hasDetonated)
            {
                _state.hp = 0f;
                _state.hasDetonated = true;
                _state.allLootDestroyed = true;

                OnSuicideVestDetonated?.Invoke(_state, _state.detonationAoeDamage);
                return _state.detonationAoeDamage;
            }

            return 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TerroristState CaptureState() => _state;

        public void RestoreState(TerroristState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
