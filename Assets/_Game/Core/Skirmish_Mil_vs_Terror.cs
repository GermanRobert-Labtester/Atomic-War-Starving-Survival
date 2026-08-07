using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MilVsTerrorState
    {
        public string id = "skirmish_mil_vs_terror";
        public string locationId;
        public float strayBulletChance = 0.15f; // Flat 15% chance every turn
        public float strayBulletDamage = 25f;
        public int combatTurnsElapsed = 0;
        public bool hasGainedExplodedModifier = false;
    }

    /// <summary>
    /// Prompt #340: Skirmish: Military vs. Terrorists.
    /// Military has armor, Terrorists have explosives. Over time, fighting grants the node the Exploded modifier.
    /// Intervening exposes player to flat 15% stray bullet damage chance per turn.
    /// </summary>
    public class Skirmish_Mil_vs_Terror
    {
        private MilVsTerrorState _state = new MilVsTerrorState();

        public event Action<MilVsTerrorState, float> OnStrayBulletHitPlayer;
        public event Action<MilVsTerrorState, string> OnNodeBecameExploded;

        public MilVsTerrorState State => _state;

        public Skirmish_Mil_vs_Terror(string locationId)
        {
            _state.locationId = locationId;
        }

        public float CheckTurnCrossfire(System.Random rng)
        {
            _state.combatTurnsElapsed++;
            if (rng.NextDouble() < _state.strayBulletChance)
            {
                OnStrayBulletHitPlayer?.Invoke(_state, _state.strayBulletDamage);
                return _state.strayBulletDamage;
            }
            return 0f;
        }

        public bool AdvanceSkirmishDuration(int hoursElapsed, FixedNodeState nodeState)
        {
            if (hoursElapsed >= 2 && !_state.hasGainedExplodedModifier)
            {
                _state.hasGainedExplodedModifier = true;
                if (nodeState != null)
                {
                    nodeState.ruinModifier = LocationStateModifier.Exploded.ToString();
                }
                OnNodeBecameExploded?.Invoke(_state, _state.locationId);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MilVsTerrorState CaptureState() => _state;

        public void RestoreState(MilVsTerrorState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
