using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RebelModeratesState
    {
        public string id = "npc_rebel_moderates";
        public string displayName = "Rebel Moderates";
        public bool isPassive = true;
        public bool radioBackupCalled = false;
        public int backupWaveCount = 3;
        public int requiredMedsForIntel = 2;
    }

    /// <summary>
    /// Prompt #331: Faction: Rebel Moderates (Sub-Faction).
    /// Exhausted fighters who offer to trade IntelNodes for MedicalSupplies.
    /// Passive and highly defensive. If attacked, they call radio backup, starting a wave defense.
    /// </summary>
    public class NPC_RebelModerates
    {
        private RebelModeratesState _state = new RebelModeratesState();

        public event Action<RebelModeratesState> OnIntelTraded;
        public event Action<RebelModeratesState, int> OnRadioBackupCalled;

        public RebelModeratesState State => _state;

        public bool TradeIntelForMeds(ref int playerMedsCount, out string intelNodeId)
        {
            intelNodeId = null;
            if (playerMedsCount >= _state.requiredMedsForIntel)
            {
                playerMedsCount -= _state.requiredMedsForIntel;
                intelNodeId = "intel_rebel_tactical_map";
                OnIntelTraded?.Invoke(_state);
                return true;
            }
            return false;
        }

        public void TriggerRadioBackup()
        {
            _state.isPassive = false;
            _state.radioBackupCalled = true;
            OnRadioBackupCalled?.Invoke(_state, _state.backupWaveCount);
        }
    }
}
