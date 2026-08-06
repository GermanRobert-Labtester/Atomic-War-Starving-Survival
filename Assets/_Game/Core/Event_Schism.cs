using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SchismState
    {
        public string eventId = "event_schism";
        public bool isActive;
        public List<string> factionASurvivors = new List<string>();
        public List<string> factionBSurvivors = new List<string>();
        public string zealotId;
        public string preacherId;
    }

    public class Event_Schism
    {
        public event Action OnSchismStarted;
        public event Action<string, bool> OnSurvivorPickedSide;
        public event Action OnSchismResolved;

        private readonly SchismState _state;
        private readonly List<string> _factionA;
        private readonly List<string> _factionB;

        public Event_Schism()
        {
            _state = new SchismState();
            _factionA = _state.factionASurvivors;
            _factionB = _state.factionBSurvivors;
        }

        public Event_Schism(SchismState state)
        {
            _state = state ?? new SchismState();
            _factionA = _state.factionASurvivors ?? new List<string>();
            _factionB = _state.factionBSurvivors ?? new List<string>();
            _state.factionASurvivors = _factionA;
            _state.factionBSurvivors = _factionB;
        }

        /// <summary>
        /// Splits the bunker into two factions led by the zealot and the preacher.
        /// Other survivors have not yet picked a side.
        /// </summary>
        public void TriggerSchism(string zealotId, string preacherId, List<string> allSurvivorIds)
        {
            _factionA.Clear();
            _factionB.Clear();

            _factionA.Add(zealotId);
            _factionB.Add(preacherId);

            _state.zealotId = zealotId;
            _state.preacherId = preacherId;
            _state.isActive = true;

            OnSchismStarted?.Invoke();
        }

        /// <summary>
        /// A survivor picks a side. isFactionA = true joins the zealot's faction.
        /// </summary>
        public void PickSide(string survivorId, bool isFactionA)
        {
            if (!_state.isActive) return;

            // Remove from both lists first to prevent duplicates
            _factionA.Remove(survivorId);
            _factionB.Remove(survivorId);

            if (isFactionA)
            {
                _factionA.Add(survivorId);
            }
            else
            {
                _factionB.Add(survivorId);
            }

            OnSurvivorPickedSide?.Invoke(survivorId, isFactionA);
        }

        /// <summary>
        /// Returns true if the healer refuses to treat the patient because they are
        /// in opposing factions during an active schism.
        /// </summary>
        public bool CheckHealingBlocked(string healerId, string patientId)
        {
            if (!_state.isActive) return false;

            bool healerInA = _factionA.Contains(healerId);
            bool healerInB = _factionB.Contains(healerId);
            bool patientInA = _factionA.Contains(patientId);
            bool patientInB = _factionB.Contains(patientId);

            // Blocked if they are in different factions
            if (healerInA && patientInB) return true;
            if (healerInB && patientInA) return true;

            return false;
        }

        /// <summary>
        /// If one faction's leader (zealot or preacher) is removed, the schism ends.
        /// </summary>
        public void ResolveSchism()
        {
            _state.isActive = false;
            _factionA.Clear();
            _factionB.Clear();
            _state.zealotId = null;
            _state.preacherId = null;

            OnSchismResolved?.Invoke();
        }

        public SchismState CaptureState()
        {
            _state.isActive = _state.isActive;
            // Lists are already in sync since _factionA/_factionB reference _state lists
            return _state;
        }

        public void RestoreState(SchismState state)
        {
            if (state == null) return;
            _state.isActive = state.isActive;
            _state.zealotId = state.zealotId;
            _state.preacherId = state.preacherId;

            _factionA.Clear();
            _factionB.Clear();

            if (state.factionASurvivors != null) _factionA.AddRange(state.factionASurvivors);
            if (state.factionBSurvivors != null) _factionB.AddRange(state.factionBSurvivors);

            _state.factionASurvivors = _factionA;
            _state.factionBSurvivors = _factionB;
        }
    }
}
