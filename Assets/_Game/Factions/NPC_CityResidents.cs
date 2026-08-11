using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class CityResidentsState
    {
        public string id = "npc_city_residents";
        public string displayName = "City Residents";
        public bool isPassive = true;
        public float empathMoraleDropOnLooting = 25f;
    }

    /// <summary>
    /// Prompt #345: NPC Encounter: City Residents.
    /// Huddled masses in apartment nodes. Passive. Looting their containers triggers begging and crying,
    /// causing a heavy Morale drop (-25) for Empath characters, but zero penalty for Sociopaths.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_CityResidents
    {
        private CityResidentsState _state = new CityResidentsState();

        public event Action<CityResidentsState, float> OnBeggingGuiltTriggered;

        public CityResidentsState State => _state;

        public float LootContainer(bool isEmpath, bool isSociopath)
        {
            if (isSociopath) return 0f;
            float moraleDrop = isEmpath ? _state.empathMoraleDropOnLooting : 10f;

            OnBeggingGuiltTriggered?.Invoke(_state, moraleDrop);
            return moraleDrop;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CityResidentsState CaptureState() => _state;

        public void RestoreState(CityResidentsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
