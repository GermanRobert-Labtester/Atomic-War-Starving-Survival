using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SiblingFeudState
    {
        public string eventId = "event_sibling_feud";
        public float jealousyAffinityDrain = -0.1f;
    }

    public class Event_SiblingFeud
    {
        public event Action<string, string> OnJealousyTriggered;
        public event Action<string, string, float> OnAffinityReduced;

        private readonly SiblingFeudState _state;

        public Event_SiblingFeud()
        {
            _state = new SiblingFeudState();
        }

        public void OnSkillXPGained(string teenId, string siblingId)
        {
            OnJealousyTriggered?.Invoke(teenId, siblingId);
            OnAffinityReduced?.Invoke(teenId, siblingId, _state.jealousyAffinityDrain);
        }

        public float GetAffinityPenalty()
        {
            return _state.jealousyAffinityDrain;
        }

        public static bool AreSiblings(string idA, string idB, Dictionary<string, List<string>> familyTree)
        {
            if (string.IsNullOrEmpty(idA) || string.IsNullOrEmpty(idB))
                return false;
            if (idA == idB)
                return false;

            foreach (var kvp in familyTree)
            {
                List<string> children = kvp.Value;
                if (children.Contains(idA) && children.Contains(idB))
                    return true;
            }

            return false;
        }

        public SiblingFeudState CaptureState() => _state;

        public void RestoreState(SiblingFeudState state)
        {
            if (state == null) return;
            _state.eventId = state.eventId;
            _state.jealousyAffinityDrain = state.jealousyAffinityDrain;
        }
    }
}
