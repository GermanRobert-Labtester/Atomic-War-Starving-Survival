using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class MilVsRebelState
    {
        public string id = "skirmish_mil_vs_rebel";
        public string locationId;
        public float rebelTrustLossOnHelpMil = 100f;
        public float militaryTrustLossOnHelpRebel = 100f;
        /// <summary>AP / M855A1 / battle-rifle exclusives — non-craftable military stock.</summary>
        public List<string> milAmmoReward = new List<string>
        {
            "ammo_556x45_ap",
            "ammo_556x45_m855a1",
            "ammo_762x51_ap"
        };
        /// <summary>Rebel battle-rifle / AP exclusives — non-craftable.</summary>
        public List<string> rebelAmmoReward = new List<string>
        {
            "ammo_762x39_ap",
            "ammo_545x39_ap",
            "ammo_300blk_ap"
        };
        public bool playerIntervenedForMilitary = false;
        public bool playerIntervenedForRebels = false;
    }

    /// <summary>
    /// Prompt #341: Skirmish: Military vs. Rebels.
    /// Classic firefight. Intervening to assist Military rewards high-tier exclusive
    /// ammunition (AP / M855A1 / battle rifle), but destroys all Rebel trust.
    /// Helping rebels awards rebel-exclusive battle-rifle AP instead.
    /// </summary>
    public class Skirmish_Mil_vs_Rebel
    {
        private MilVsRebelState _state = new MilVsRebelState();

        public event Action<MilVsRebelState, List<string>, float> OnMilitaryAssisted;
        public event Action<MilVsRebelState, List<string>, float> OnRebelAssisted;

        public MilVsRebelState State => _state;

        public Skirmish_Mil_vs_Rebel(string locationId)
        {
            _state.locationId = locationId;
            // Ensure rewards stay on the exclusive catalog even if save wiped them.
            if (_state.milAmmoReward == null || _state.milAmmoReward.Count == 0)
                _state.milAmmoReward = Item_AmmoTypes.DefaultMilitaryInterventionRewards();
            if (_state.rebelAmmoReward == null || _state.rebelAmmoReward.Count == 0)
                _state.rebelAmmoReward = Item_AmmoTypes.DefaultRebelInterventionRewards();
        }

        public List<string> InterveneForMilitary(out float rebelTrustDelta)
        {
            _state.playerIntervenedForMilitary = true;
            rebelTrustDelta = -_state.rebelTrustLossOnHelpMil;
            var rewards = _state.milAmmoReward ?? Item_AmmoTypes.DefaultMilitaryInterventionRewards();
            OnMilitaryAssisted?.Invoke(_state, rewards, rebelTrustDelta);
            return rewards;
        }

        public List<string> InterveneForRebels(out float militaryTrustDelta)
        {
            _state.playerIntervenedForRebels = true;
            militaryTrustDelta = -_state.militaryTrustLossOnHelpRebel;
            var rewards = _state.rebelAmmoReward ?? Item_AmmoTypes.DefaultRebelInterventionRewards();
            OnRebelAssisted?.Invoke(_state, rewards, militaryTrustDelta);
            return rewards;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public MilVsRebelState CaptureState() => _state;

        public void RestoreState(MilVsRebelState saved)
        {
            if (saved == null) return;
            _state = saved;
            if (_state.milAmmoReward == null || _state.milAmmoReward.Count == 0)
                _state.milAmmoReward = Item_AmmoTypes.DefaultMilitaryInterventionRewards();
            if (_state.rebelAmmoReward == null || _state.rebelAmmoReward.Count == 0)
                _state.rebelAmmoReward = Item_AmmoTypes.DefaultRebelInterventionRewards();
        }
    }
}
