// SPDX-License-Identifier: MIT
// ============================================================================
// Campaign action log host composition. The Core CampaignActionLog is the sole
// authority over sequence assignment, ordering, capture, and restore. The host
// owns only which successful significant player commands are recorded and the
// log's persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.PlayerCommand;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CampaignActionLogHostSession? _actionLog;
        private bool _actionLogDirty;

        public CampaignActionLogHostSession? CampaignActionLogSession => _actionLog;

        public void SetupCampaignActionLog()
        {
            if (_actionLog != null) return;
            _actionLog = CampaignActionLogHostSession.Create();
            var saved = CampaignActionLogSaveStore.TryLoad();
            if (saved != null) _actionLog.RestoreState(saved);
            _actionLog.StateChanged += () => _actionLogDirty = true;
        }

        /// <summary>
        /// Records one successful significant player command. Sequence numbers
        /// and ordering are assigned by the Core log so replays stay stable.
        /// </summary>
        public long RecordCampaignAction(int day, string commandCode, string actorId, string targetId, string resultCode)
        {
            SetupCampaignActionLog();
            long sequence = _actionLog!.Record(day, commandCode, actorId, targetId, resultCode);
            _actionLogDirty = true;
            return sequence;
        }

        public int ClearCampaignActionLogForNewCampaign()
        {
            SetupCampaignActionLog();
            _actionLog!.ClearForNewCampaign();
            _actionLogDirty = true;
            return _actionLog.EntryCount;
        }

        public IReadOnlyList<CampaignActionLogEntry> GetCampaignActionLogEntriesForDay(int day)
        {
            SetupCampaignActionLog();
            return _actionLog!.EntriesForDay(day);
        }

        public (int Entries, long NextSequence, string LastCommand) GetCampaignActionLogReadout()
        {
            SetupCampaignActionLog();
            var entries = _actionLog!.Entries;
            return (entries.Count,
                    _actionLog.EntryCount > 0 ? entries[entries.Count - 1].Sequence : 0,
                    _actionLog.EntryCount > 0 ? entries[entries.Count - 1].CommandCode : string.Empty);
        }

        public void SaveCampaignActionLog()
        {
            if (_actionLog == null) return;
            var state = _actionLog.CaptureState();
            if (CaptureSection(CampaignActionLogSaveStore.SectionName, CampaignActionLogSaveStore.TryCapturePersisted(state)))
                _actionLogDirty = false;
        }

        public void ResetCampaignActionLog()
        {
            _actionLog = null;
            _actionLogDirty = false;
        }
    }
}
