using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Auto-generated survivor journal: playthrough log + immersive tutorial.
    /// Discoveries land once via <see cref="KnowledgeBase"/>; text is trait-voiced.
    /// EventRunner calls into this when world state first trips a discovery.
    /// </summary>
    public class JournalSystem
    {
        public const int MaxEntries = 64;

        private readonly List<JournalEntry> _entries = new List<JournalEntry>();
        private readonly KnowledgeBase _knowledge = new KnowledgeBase();
        private int _seq;

        public KnowledgeBase Knowledge => _knowledge;
        /// <summary>Newest-first log.</summary>
        public IReadOnlyList<JournalEntry> Entries => _entries;
        public int EntryCount => _entries.Count;
        public string LatestText =>
            _entries.Count > 0 ? (_entries[0].Text ?? string.Empty) : string.Empty;

        /// <summary>Unread badge / notification state for the journal book UI.</summary>
        public bool HasUnread { get; set; }
        /// <summary>True after a new entry until the UI acknowledges the ping.</summary>
        public bool NotificationPing { get; private set; }
        public int NotificationPingCount { get; private set; }
        public bool HudIsOpen { get; set; }

        public event Action<JournalEntry> OnEntryAdded;
        /// <summary>Fired when a new entry should ping the player (diegetic, not a modal).</summary>
        public event Action<JournalEntry> OnNotificationPing;

        /// <summary>
        /// Record a discovery if new. Returns the entry, or null if already known / invalid.
        /// </summary>
        public JournalEntry TryDiscover(
            string knowledgeKey,
            Survivor author,
            int day,
            float hour = -1f)
        {
            if (string.IsNullOrEmpty(knowledgeKey)) return null;
            if (!_knowledge.Discover(knowledgeKey)) return null;

            var bias = author != null ? author.RiskBias : RiskBiasTrait.Realist;
            string name = author != null && !string.IsNullOrEmpty(author.DisplayName)
                ? author.DisplayName
                : (author != null && !string.IsNullOrEmpty(author.Id) ? author.Id : "Unknown");
            string authorId = author?.Id ?? string.Empty;

            var entry = new JournalEntry
            {
                Id = $"journal_{++_seq}_{knowledgeKey}",
                Text = JournalVoice.ComposeFullText(knowledgeKey, bias, day),
                Timestamp = JournalVoice.FormatTimestamp(day, hour),
                AuthorName = name,
                AuthorId = authorId,
                KnowledgeKey = knowledgeKey,
                Day = day > 0 ? day : 1,
                Hour = hour
            };

            _entries.Insert(0, entry);
            while (_entries.Count > MaxEntries)
                _entries.RemoveAt(_entries.Count - 1);

            HasUnread = true;
            NotificationPing = true;
            NotificationPingCount++;
            OnEntryAdded?.Invoke(entry);
            OnNotificationPing?.Invoke(entry);
            return entry;
        }

        public void AcknowledgePing()
        {
            NotificationPing = false;
        }

        public void MarkRead()
        {
            HasUnread = false;
            NotificationPing = false;
        }

        public void Clear()
        {
            _entries.Clear();
            _knowledge.Clear();
            _seq = 0;
            HasUnread = false;
            NotificationPing = false;
            NotificationPingCount = 0;
            HudIsOpen = false;
        }

        public JournalSave CaptureState()
        {
            return new JournalSave
            {
                Entries = _entries.ToArray(),
                Knowledge = _knowledge.CaptureState(),
                NextSeq = _seq,
                HasUnread = HasUnread,
                NotificationPing = NotificationPing,
                NotificationPingCount = NotificationPingCount,
                HudIsOpen = HudIsOpen
            };
        }

        public void RestoreState(JournalSave save)
        {
            Clear();
            if (save == null) return;
            _seq = Math.Max(0, save.NextSeq);
            HasUnread = save.HasUnread;
            NotificationPing = save.NotificationPing;
            NotificationPingCount = Math.Max(0, save.NotificationPingCount);
            HudIsOpen = save.HudIsOpen;
            _knowledge.RestoreState(save.Knowledge);
            if (save.Entries == null) return;
            for (int i = 0; i < save.Entries.Length && i < MaxEntries; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.Text)) continue;
                _entries.Add(e);
            }
        }
    }

    [Serializable]
    public class JournalSave
    {
        public JournalEntry[] Entries;
        public KnowledgeBaseSave Knowledge;
        public int NextSeq;
        public bool HasUnread;
        public bool NotificationPing;
        public int NotificationPingCount;
        public bool HudIsOpen;
    }
}
