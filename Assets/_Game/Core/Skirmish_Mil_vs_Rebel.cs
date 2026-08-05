using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MilVsRebelState
    {
        public string id = "skirmish_mil_vs_rebel";
        public string locationId;
        public float rebelTrustLossOnHelpMil = 100f;
        public List<string> milAmmoReward = new List<string> { "high_tier_military_ammo_box", "ap_rifle_rounds" };
        public bool playerIntervenedForMilitary = false;
    }

    /// <summary>
    /// Prompt #341: Skirmish: Military vs. Rebels.
    /// Classic firefight. Intervening to assist Military rewards high-tier Ammunition,
    /// but destroys all Rebel trust.
    /// </summary>
    public class Skirmish_Mil_vs_Rebel
    {
        private MilVsRebelState _state = new MilVsRebelState();

        public event Action<MilVsRebelState, List<string>, float> OnMilitaryAssisted;

        public MilVsRebelState State => _state;

        public Skirmish_Mil_vs_Rebel(string locationId)
        {
            _state.locationId = locationId;
        }

        public List<string> InterveneForMilitary(out float rebelTrustDelta)
        {
            _state.playerIntervenedForMilitary = true;
            rebelTrustDelta = -_state.rebelTrustLossOnHelpMil;

            OnMilitaryAssisted?.Invoke(_state, _state.milAmmoReward, rebelTrustDelta);
            return _state.milAmmoReward;
        }
    }
}
