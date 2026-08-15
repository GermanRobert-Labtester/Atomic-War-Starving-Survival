using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_TheOfficeState
    {
        public string id = "faction_the_office";
        public string displayName = "The Office";
        public bool isActive;
        public float trust;
        public bool accessGranted = true;
    }

    /// <summary>
    /// Currents-style trust float. Not a Sector 4 Power. Not in faction_lore.json.
    /// </summary>
    public class NPC_TheOffice
    {
        private NPC_TheOfficeState _state = new NPC_TheOfficeState();

        public event Action<NPC_TheOfficeState> OnStateChanged;

        public NPC_TheOfficeState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void AdjustTrust(float delta)
        {
            _state.trust = Mathf.Clamp(_state.trust + delta, -100f, 100f);
            OnStateChanged?.Invoke(_state);
        }

        public NPC_TheOfficeState CaptureState() => _state;
        public void RestoreState(NPC_TheOfficeState saved) { _state = saved ?? new NPC_TheOfficeState(); }
    }
}
