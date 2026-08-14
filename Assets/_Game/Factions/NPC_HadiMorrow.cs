using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_HadiMorrowState
    {
        public string id = "npc_hadi_morrow";
        public string displayName = "Hadi Morrow";
        public bool isActive;
        public string status = "home"; // home / listed / hidden / sent / never_back
        public bool titleCorrected;
        public bool childSeptic;
        public float trust;
    }

    /// <summary>
    /// Veterinary assistant. The trade the rubric scored cheap and District 8
    /// cannot desalinate a child without. Will not call himself a doctor.
    /// Will not leave a septic child for a form without being ordered.
    /// </summary>
    public class NPC_HadiMorrow
    {
        private NPC_HadiMorrowState _state = new NPC_HadiMorrowState();

        public event Action<NPC_HadiMorrowState> OnStateChanged;

        public NPC_HadiMorrowState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void SetStatus(string status)
        {
            _state.status = status;
            OnStateChanged?.Invoke(_state);
        }

        public void CorrectTitle(bool corrected)
        {
            _state.titleCorrected = corrected;
            OnStateChanged?.Invoke(_state);
        }

        public void SetChildSeptic(bool septic)
        {
            _state.childSeptic = septic;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_HadiMorrowState CaptureState() => _state;
        public void RestoreState(NPC_HadiMorrowState saved) { _state = saved ?? new NPC_HadiMorrowState(); }
    }
}