using System;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_IvoFennState
    {
        public string id = "npc_ivo_fenn";
        public string displayName = "Ivo Fenn";
        public bool isActive;
        /// <summary>Records filed in the records room.</summary>
        public int recordsFiled;
        /// <summary>True once Ivo has refused to summarise the Charter.</summary>
        public bool refusedCharterSummary;
        /// <summary>True once Ivo has produced the original Charter file.</summary>
        public bool charterProduced;
    }

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Ivo Fenn, records clerk.
    /// Stationary. Files everything. Will not destroy a record.
    /// Will not summarise the Charter — he produces files, not summaries.
    /// If Overlay plates the records room, Ivo files the plate's receipt
    /// and not the plate.
    /// </summary>
    public class NPC_IvoFenn
    {
        private NPC_IvoFennState _state = new NPC_IvoFennState();

        public event Action<NPC_IvoFennState, int> OnRecordFiled;
        public event Action<NPC_IvoFennState> OnCharterProduced;

        public NPC_IvoFennState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>File a record in the records room. Returns the running count.</summary>
        public int FileRecord()
        {
            _state.recordsFiled++;
            OnRecordFiled?.Invoke(_state, _state.recordsFiled);
            return _state.recordsFiled;
        }

        /// <summary>
        /// Refuse to summarise the Charter. Ivo produces files,
        /// not summaries. One-time event.
        /// </summary>
        public bool RefuseCharterSummary()
        {
            if (_state.refusedCharterSummary) return false;
            _state.refusedCharterSummary = true;
            return true;
        }

        /// <summary>
        /// Produce the original Charter file (three dry pages).
        /// One-time event — once produced, it stays produced.
        /// </summary>
        public bool ProduceCharter()
        {
            if (_state.charterProduced) return false;
            _state.charterProduced = true;
            OnCharterProduced?.Invoke(_state);
            return true;
        }

        public NPC_IvoFennState CaptureState() => _state;
        public void RestoreState(NPC_IvoFennState saved) { _state = saved ?? new NPC_IvoFennState(); }
    }
}
