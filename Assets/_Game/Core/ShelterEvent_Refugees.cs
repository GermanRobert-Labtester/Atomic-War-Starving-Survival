using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RefugeeWaveState
    {
        public string eventId = "shelter_event_refugees";
        public int totalRefugeeCount = 15;
        public int maxAdmitCapacity = 2;
        public List<string> admittedRefugeeIds = new List<string>();
        public List<string> turnedAwayRefugeeIds = new List<string>();
    }

    /// <summary>
    /// Prompt #416: Event: Refugee Wave.
    /// A nearby settlement collapses, sending 15 starving refugees to the hatch.
    /// The shelter can only support 2, forcing a mass triage choice on who is admitted and who is turned away.
    /// </summary>
    public class ShelterEvent_Refugees
    {
        private RefugeeWaveState _state = new RefugeeWaveState();

        public event Action<RefugeeWaveState, List<string>, List<string>> OnRefugeeTriageResolved;

        public RefugeeWaveState State => _state;

        public bool PerformTriage(List<string> selectedToAdmit, List<string> allRefugees)
        {
            if (selectedToAdmit == null || selectedToAdmit.Count > _state.maxAdmitCapacity)
                return false;

            _state.admittedRefugeeIds = new List<string>(selectedToAdmit);
            _state.turnedAwayRefugeeIds = new List<string>();

            foreach (var r in allRefugees)
            {
                if (!selectedToAdmit.Contains(r))
                {
                    _state.turnedAwayRefugeeIds.Add(r);
                }
            }

            OnRefugeeTriageResolved?.Invoke(_state, _state.admittedRefugeeIds, _state.turnedAwayRefugeeIds);
            return true;
        }
    }
}
