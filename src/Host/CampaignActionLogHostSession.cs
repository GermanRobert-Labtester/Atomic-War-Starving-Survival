// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CampaignActionLogSaveStore
// Core Type  : Ashfall.Core.PlayerCommand.CampaignActionLog
// Host Caller: Main.CampaignActionLog
// Purpose    : Deterministic campaign action log. The Core type is the sole
//              authority over sequence assignment, ordering, capture, and
//              restore; the host owns only which player commands are recorded
//              and their persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class CampaignActionLogSaveStore
    {
        public const string FileName = "campaign_action_log_save.json";
        public const string SectionName = "campaign_action_log";

        private static readonly SaveStore<CampaignActionLogSave> s_store =
            SaveStoreHub.Checksummed<CampaignActionLogSave>(FileName, nameof(CampaignActionLogSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(CampaignActionLogSave state) => s_store.CaptureBare(state);
        public static CampaignActionLogSave? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(CampaignActionLogSave state) => s_store.TrySave(state);
        public static CampaignActionLogSave? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core campaign action log.</summary>
    public sealed class CampaignActionLogHostSession : HostSessionBase
    {
        private readonly CampaignActionLog _log = new CampaignActionLog();

        public static CampaignActionLogHostSession Create() => new CampaignActionLogHostSession();

        public string LastEvent { get; private set; } = string.Empty;

        public IReadOnlyList<CampaignActionLogEntry> Entries => _log.Entries;
        public int EntryCount => _log.Entries.Count;

        /// <summary>
        /// Records one successful significant player command. Sequence
        /// assignment and ordering are decided by the Core log.
        /// </summary>
        public long Record(
            int day,
            string commandCode,
            string actorId,
            string targetId,
            string resultCode)
        {
            var entry = new CampaignActionLogEntry
            {
                Day = day,
                CommandCode = commandCode ?? string.Empty,
                ActorId = actorId ?? string.Empty,
                TargetId = targetId ?? string.Empty,
                ResultCode = resultCode ?? string.Empty
            };
            long sequence = _log.Append(entry);
            LastEvent = $"Recorded action {sequence}: {entry.CommandCode} on day {day}.";
            RaiseStateChanged();
            return sequence;
        }

        public IReadOnlyList<CampaignActionLogEntry> EntriesForDay(int day) =>
            _log.Entries.Where(e => e.Day == day).ToList();

        public IReadOnlyList<CampaignActionLogEntry> EntriesForActor(string actorId) =>
            _log.Entries.Where(e => string.Equals(e.ActorId, actorId, StringComparison.Ordinal)).ToList();

        public int CountCommand(string commandCode) =>
            _log.Entries.Count(e => string.Equals(e.CommandCode, commandCode, StringComparison.Ordinal));

        public void ClearForNewCampaign()
        {
            _log.Clear();
            LastEvent = "Campaign action log cleared for a new campaign.";
            RaiseStateChanged();
        }

        public CampaignActionLogSave CaptureState() => _log.Capture();

        public void RestoreState(CampaignActionLogSave? save)
        {
            _log.Restore(save ?? new CampaignActionLogSave());
            LastEvent = $"Restored campaign action log ({_log.Entries.Count} entr(ies)).";
            RaiseStateChanged();
        }

        public bool TrySave() => CampaignActionLogSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var save = CampaignActionLogSaveStore.TryLoad();
            if (save == null) return false;
            RestoreState(save);
            return true;
        }
    }
}
