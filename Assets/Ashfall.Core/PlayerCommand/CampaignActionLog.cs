// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.PlayerCommand
{
    /// <summary>
    /// Deterministic campaign action log.
    /// Every successful significant player command appends one entry.
    /// Ordering is stable by sequence number; entries are serializable
    /// and survive save/load as part of the campaign envelope.
    /// </summary>
    [Serializable]
    public sealed class CampaignActionLog
    {
        private long _nextSequence = 1;
        public List<CampaignActionLogEntry> Entries { get; } = new List<CampaignActionLogEntry>();

        /// <summary>Append a new entry and return its monotonic sequence number.</summary>
        public long Append(CampaignActionLogEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            entry.Sequence = _nextSequence++;
            Entries.Add(entry);
            return entry.Sequence;
        }

        /// <summary>Clear all entries (e.g. new campaign).</summary>
        public void Clear()
        {
            _nextSequence = 1;
            Entries.Clear();
        }

        /// <summary>Restore from persisted state.</summary>
        public void Restore(CampaignActionLogSave save)
        {
            if (save == null) return;
            Entries.Clear();
            if (save.Entries != null)
            {
                foreach (var e in save.Entries)
                    Entries.Add(e);
            }
            _nextSequence = save.NextSequence;
            if (_nextSequence <= 0 && Entries.Count > 0)
                _nextSequence = Entries[Entries.Count - 1].Sequence + 1;
        }

        public CampaignActionLogSave Capture()
        {
            return new CampaignActionLogSave
            {
                Entries = new List<CampaignActionLogEntry>(Entries),
                NextSequence = _nextSequence
            };
        }
    }

    /// <summary>One deterministic campaign action record.</summary>
    [Serializable]
    public sealed class CampaignActionLogEntry
    {
        public long Sequence { get; set; }
        public int Day { get; set; }
        public string CommandCode { get; set; } = string.Empty;
        public string ActorId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public string ResultCode { get; set; } = string.Empty;
        public Dictionary<string, double> Deltas { get; set; } = new Dictionary<string, double>();
    }

    /// <summary>Persisted envelope for the campaign action log.</summary>
    [Serializable]
    public sealed class CampaignActionLogSave
    {
        public List<CampaignActionLogEntry> Entries { get; set; } = new List<CampaignActionLogEntry>();
        public long NextSequence { get; set; } = 1;
    }
}
