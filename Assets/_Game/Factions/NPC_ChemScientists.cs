using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class ChemScientistsState
    {
        public string id = "npc_chem_scientists";
        public string displayName = "The Chem-Weapon Scientists";
        public bool isMustardGasProductionActive = true;
        public int militaryGuardCount = 3;
        public bool isSabotagedOrExecuted = false;
        public bool researchStolen = false;
        public float rebelTrustGainOnExecution = 30f;
        public float chemistryXpGainOnSteal = 250f;
    }

    /// <summary>
    /// Prompt #326: NPC Encounter: The Chem-Weapon Scientists.
    /// Spawns at Military Bases or Hospitals. Non-combatants producing Mustard Gas.
    /// Player can execute them to halt production (+Rebel Faction Trust) or steal research (+AdvancedChemistry XP).
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_ChemScientists
    {
        private ChemScientistsState _state = new ChemScientistsState();

        public event Action<ChemScientistsState, float> OnExecutedForRebelTrust;
        public event Action<ChemScientistsState, float> OnResearchStolenForChemistryXp;

        public ChemScientistsState State => _state;

        public bool CanTriggerAtLocation(string locationId)
        {
            return locationId == "military_base" || locationId == "general_hospital";
        }

        public float ExecuteScientistsToStopWeapon()
        {
            if (_state.isSabotagedOrExecuted) return 0f;
            _state.isMustardGasProductionActive = false;
            _state.isSabotagedOrExecuted = true;

            OnExecutedForRebelTrust?.Invoke(_state, _state.rebelTrustGainOnExecution);
            return _state.rebelTrustGainOnExecution;
        }

        public float StealResearch()
        {
            if (_state.researchStolen) return 0f;
            _state.researchStolen = true;

            OnResearchStolenForChemistryXp?.Invoke(_state, _state.chemistryXpGainOnSteal);
            return _state.chemistryXpGainOnSteal;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ChemScientistsState CaptureState() => _state;

        public void RestoreState(ChemScientistsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
