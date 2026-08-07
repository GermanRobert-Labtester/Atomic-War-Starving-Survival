using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MassGraveState
    {
        public string anomalyId = "map_anomaly_mass_grave";
        public string displayName = "The Mass Grave Trench";
        public float travelMoraleDrop = 20f;
        public float karmaPenaltyOnRob = 30f;
        public float sanityDropOnRob = 25f;
        public List<string> graveRobLoot = new List<string> { "prewar_jewelry", "gold_teeth_bag", "pocket_watch" };
    }

    /// <summary>
    /// Prompt #458: Anomaly: The Mass Grave Trench.
    /// Horrific mass grave trench serving as a massive disease vector.
    /// Moving through drops Morale by 20. Players can choose to rob corpses for high-value loot at the cost of Karma and Sanity.
    /// </summary>
    public class MapAnomaly_MassGrave
    {
        private MassGraveState _state = new MassGraveState();

        public event Action<MassGraveState, string, float> OnMassGraveTraversedMoraleDropped;
        public event Action<MassGraveState, string, float, float> OnCorpsesRobbedKarmaSanityPenalized;

        public MassGraveState State => _state;

        public void TraverseGraveNode(string partyId, ref float partyMorale)
        {
            partyMorale = Mathf.Max(0f, partyMorale - _state.travelMoraleDrop);
            OnMassGraveTraversedMoraleDropped?.Invoke(_state, partyId, _state.travelMoraleDrop);
        }

        public List<string> RobCorpses(string survivorId, ref float globalKarma, ref float survivorSanity)
        {
            globalKarma -= _state.karmaPenaltyOnRob;
            survivorSanity = Mathf.Max(0f, survivorSanity - _state.sanityDropOnRob);

            OnCorpsesRobbedKarmaSanityPenalized?.Invoke(_state, survivorId, _state.karmaPenaltyOnRob, _state.sanityDropOnRob);
            return new List<string>(_state.graveRobLoot);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MassGraveState CaptureState()
        {
            return new MassGraveState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                travelMoraleDrop = _state.travelMoraleDrop,
                karmaPenaltyOnRob = _state.karmaPenaltyOnRob,
                sanityDropOnRob = _state.sanityDropOnRob,
                graveRobLoot = _state.graveRobLoot != null ? new System.Collections.Generic.List<string>(_state.graveRobLoot) : new System.Collections.Generic.List<string>(),
            };
        }

        public void RestoreState(MassGraveState saved)
        {
            if (saved == null)
            {
                _state = new MassGraveState();
                return;
            }
            _state = new MassGraveState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                travelMoraleDrop = saved.travelMoraleDrop,
                karmaPenaltyOnRob = saved.karmaPenaltyOnRob,
                sanityDropOnRob = saved.sanityDropOnRob,
                graveRobLoot = saved.graveRobLoot != null ? new System.Collections.Generic.List<string>(saved.graveRobLoot) : new System.Collections.Generic.List<string>(),
            };
        }
    }
}
