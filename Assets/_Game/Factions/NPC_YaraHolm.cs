using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_YaraHolmState
    {
        public string id = "npc_yara_holm";
        public string displayName = "Yara Holm";
        public bool isActive;
        public bool accessGranted = true;
        public bool withdrewPermanently;
        public int darkLampRequests;
        public bool blastRefused = true;
    }

    /// <summary>
    /// Cutter. Will not guide onto dark ice. Will not blast.
    /// Dark and lit are moral words.
    /// </summary>
    public class NPC_YaraHolm
    {
        private NPC_YaraHolmState _state = new NPC_YaraHolmState();

        public event Action<NPC_YaraHolmState> OnAccessWithdrawn;
        public event Action<NPC_YaraHolmState> OnStateChanged;

        public NPC_YaraHolmState State => _state;
        public bool AccessGranted => _state.accessGranted && !_state.withdrewPermanently;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>Relight-for-a-trap / blast: Cutters withdraw. Same shape as Ivy's exception.</summary>
        public bool RequestDarkException()
        {
            _state.darkLampRequests++;
            if (_state.darkLampRequests >= 1 && _state.accessGranted)
                Withdraw(permanent: false);
            return false;
        }

        public void RecordBlast()
        {
            _state.blastRefused = false;
            Withdraw(permanent: true);
        }

        public void Withdraw(bool permanent)
        {
            if (!_state.accessGranted && _state.withdrewPermanently) return;
            _state.accessGranted = false;
            _state.withdrewPermanently = permanent;
            OnAccessWithdrawn?.Invoke(_state);
            OnStateChanged?.Invoke(_state);
        }

        public NPC_YaraHolmState CaptureState() => _state;
        public void RestoreState(NPC_YaraHolmState saved) { _state = saved ?? new NPC_YaraHolmState(); }
    }
}
