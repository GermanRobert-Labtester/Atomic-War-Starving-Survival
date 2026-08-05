using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SpecOpsState
    {
        public string id = "npc_spec_ops";
        public string displayName = "Special Ops Unit";
        public bool isNightVisionEquipped = true;
        public int flashbangCharges = 3;
        public float baseLethalityMultiplier = 2.5f;
        public bool canSurrender = false;
        public List<string> pristineGearLoot = new List<string> { "pristine_body_armor", "night_vision_goggles", "tactical_rifle" };
    }

    /// <summary>
    /// Prompt #324: NPC Encounter: Special Ops Unit.
    /// Highly lethal military combatants using Flashbangs (stuns player, drains Stamina)
    /// and NightVision (immune to darkness penalties). Never surrender. Drop pristine military gear.
    /// </summary>
    public class NPC_SpecOps
    {
        private SpecOpsState _state = new SpecOpsState();

        public event Action<SpecOpsState, float, float> OnFlashbangUsed; // state, stunDuration, staminaDrain

        public SpecOpsState State => _state;

        public bool TryUseFlashbang(out float stunDuration, out float staminaDrain)
        {
            stunDuration = 0f;
            staminaDrain = 0f;
            if (_state.flashbangCharges <= 0) return false;

            _state.flashbangCharges--;
            stunDuration = 3.0f;  // Stuns player for 3 seconds
            staminaDrain = 40.0f; // Drains 40 Stamina

            OnFlashbangUsed?.Invoke(_state, stunDuration, staminaDrain);
            return true;
        }

        public float GetAccuracyMultiplier(bool isDarknessActive)
        {
            // NightVision makes them immune to darkness penalties
            if (isDarknessActive && _state.isNightVisionEquipped)
                return 1.0f;
            return isDarknessActive ? 0.6f : 1.0f;
        }
    }
}
