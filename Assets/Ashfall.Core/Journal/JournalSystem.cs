using System;
using System.Collections.Generic;

namespace Ashfall.Core.Journal
{
    /// <summary>
    /// Auto-generated survivor journal: playthrough log + immersive tutorial.
    /// Discoveries land once via <see cref="KnowledgeBase"/>; text is trait-voiced.
    /// EventRunner calls into this when world state first trips a discovery.
    /// </summary>
    public class JournalSystem
    {
        public const int MaxEntries = 64;

        /// <summary>Number of journal tabs (Log, Items, People, Places, Events).</summary>
        public const int TabCount = 5;

        public event Action<JournalEntry> OnEntryAdded;
        /// <summary>Fired when a new entry should ping the player (diegetic, not a modal).</summary>
        public event Action<JournalEntry> OnNotificationPing;
        /// <summary>Fired when the active tab changes (UI mirrors; save captures).</summary>
        public event Action<int> OnTabChanged;
        /// <summary>Fired when a codex unlock key is discovered for the first time.</summary>
        public event Action<string> OnCodexUnlocked;

        private readonly List<JournalEntry> _entries = new List<JournalEntry>();
        private readonly KnowledgeBase _knowledge = new KnowledgeBase();
        private int _seq;
        private Func<JournalEntry> _entryFactory;
        private Action<JournalEntry> _entryRecycler;

        /// <summary>Active tab index (0 = Log). Clamped to [0, TabCount).</summary>
        public int ActiveTab { get; private set; }

        /// <summary>Entry count at the moment each tab was last viewed (-1 = never).</summary>
        private readonly int[] _lastSeenIndexPerTab = new int[TabCount];

        /// <summary>Codex unlocks at the moment each tab was last viewed (-1 = never).</summary>
        private readonly int[] _lastSeenCodexPerTab = new int[TabCount];

        /// <summary>Total first-time codex unlocks (item/location/survivor/event keys).</summary>
        public int CodexUnlockCount { get; private set; }

        public JournalSystem()
        {
            for (int i = 0; i < TabCount; i++)
            {
                _lastSeenIndexPerTab[i] = -1;
                _lastSeenCodexPerTab[i] = -1;
            }
        }

        /// <summary>Entry count the last time <paramref name="tab"/> was viewed, or -1.</summary>
        public int GetLastSeenIndex(int tab)
        {
            return tab >= 0 && tab < TabCount ? _lastSeenIndexPerTab[tab] : -1;
        }

        /// <summary>Codex unlock count the last time <paramref name="tab"/> was viewed, or -1.</summary>
        public int GetLastSeenCodexIndex(int tab)
        {
            return tab >= 0 && tab < TabCount ? _lastSeenCodexPerTab[tab] : -1;
        }

        /// <summary>True when new content landed after the tab was last viewed.</summary>
        public bool HasUnreadForTab(int tab)
        {
            if (tab <= 0) return HasUnread; // Log tab mirrors global unread
            int lastSeen = GetLastSeenIndex(tab);
            if (lastSeen >= 0 && _entries.Count > lastSeen) return true;
            int lastSeenCodex = GetLastSeenCodexIndex(tab);
            return lastSeenCodex >= 0 && CodexUnlockCount > lastSeenCodex;
        }

        /// <summary>Switch the active tab (clamped); raises OnTabChanged on change.</summary>
        public void SwitchTab(int tab)
        {
            int clamped = tab < 0 ? 0 : (tab >= TabCount ? TabCount - 1 : tab);
            if (clamped == ActiveTab) return;
            ActiveTab = clamped;
            _lastSeenIndexPerTab[clamped] = _entries.Count;
            _lastSeenCodexPerTab[clamped] = CodexUnlockCount;
            OnTabChanged?.Invoke(clamped);
        }

        /// <summary>Record that the UI showed <paramref name="tab"/> (unread reset for it).</summary>
        public void MarkTabViewed(int tab)
        {
            if (tab < 0 || tab >= TabCount) return;
            _lastSeenIndexPerTab[tab] = _entries.Count;
            _lastSeenCodexPerTab[tab] = CodexUnlockCount;
            if (tab == 0)
            {
                HasUnread = false;
                NotificationPing = false;
            }
        }

        // -----------------------------------------------------------------
        // Codex unlocks (docs/ui/JOURNAL_UI_PLAN.md §5, §7)
        // -----------------------------------------------------------------

        public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
        public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
        public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
        public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
        public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);

        public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
        public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
        public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
        public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));

        private bool UnlockCodex(string key)
        {
            if (!_knowledge.Discover(key)) return false;
            CodexUnlockCount++;
            OnCodexUnlocked?.Invoke(key);
            return true;
        }

        /// <summary>
        /// Wire an object pool (GenericObjectPool) so evicted/cleared entries are
        /// recycled instead of collected, and new entries reuse pooled instances.
        /// Null factory falls back to `new JournalEntry()`; null recycler disables recycling.
        /// </summary>
        public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler)
        {
            _entryFactory = factory;
            _entryRecycler = recycler;
        }

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

        /// <summary>
        /// Record a discovery if new. Returns the entry, or null if already known / invalid.
        /// </summary>
        public JournalEntry TryDiscover(
            string knowledgeKey,
            ISurvivorAuthor author,
            int day,
            float hour = -1f)
        {
            if (string.IsNullOrEmpty(knowledgeKey)) return null;
            if (!_knowledge.Discover(knowledgeKey)) return null;

            var bias = author != null ? author.RiskBias : RiskBiasTrait.Realist;
            string text = JournalVoice.ComposeFullText(knowledgeKey, bias, day);
            return InsertEntry(knowledgeKey, text, author, day, hour);
        }

        /// <summary>
        /// Record a freeform narrative entry once per knowledge key (Prompt #19
        /// ghost-station diary fragments). Deduped via <see cref="KnowledgeBase"/>.
        /// </summary>
        public JournalEntry TryAddRawEntry(
            string knowledgeKey,
            string text,
            ISurvivorAuthor author,
            int day,
            float hour = -1f)
        {
            if (string.IsNullOrEmpty(knowledgeKey) || string.IsNullOrEmpty(text)) return null;
            if (!_knowledge.Discover(knowledgeKey)) return null;
            return InsertEntry(knowledgeKey, text, author, day, hour);
        }

        private JournalEntry InsertEntry(
            string knowledgeKey,
            string text,
            ISurvivorAuthor author,
            int day,
            float hour)
        {
            string name = author != null && !string.IsNullOrEmpty(author.DisplayName)
                ? author.DisplayName
                : (author != null && !string.IsNullOrEmpty(author.Id) ? author.Id : "Unknown");
            string authorId = author?.Id ?? string.Empty;

            var entry = _entryFactory != null ? _entryFactory() : new JournalEntry();
            entry.Id = $"journal_{++_seq}_{knowledgeKey}";
            entry.Text = text ?? string.Empty;
            entry.Timestamp = JournalVoice.FormatTimestamp(day, hour);
            entry.AuthorName = name;
            entry.AuthorId = authorId;
            entry.KnowledgeKey = knowledgeKey;
            entry.Day = day > 0 ? day : 1;
            entry.Hour = hour;

            _entries.Insert(0, entry);
            JournalEntry evicted = null;
            if (_entries.Count > MaxEntries)
            {
                evicted = _entries[_entries.Count - 1];
                _entries.RemoveAt(_entries.Count - 1);
            }

            HasUnread = true;
            NotificationPing = true;
            NotificationPingCount++;
            OnEntryAdded?.Invoke(entry);
            OnNotificationPing?.Invoke(entry);

            // Recycle only after subscribers ran: the journal book trims its
            // mirrored list inside OnEntryAdded and must drop the reference first.
            if (evicted != null)
                _entryRecycler?.Invoke(evicted);
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
            if (_entryRecycler != null)
            {
                for (int i = 0; i < _entries.Count; i++)
                    _entryRecycler(_entries[i]);
            }
            _entries.Clear();
            _knowledge.Clear();
            _seq = 0;
            HasUnread = false;
            NotificationPing = false;
            NotificationPingCount = 0;
            HudIsOpen = false;
            ActiveTab = 0;
            CodexUnlockCount = 0;
            for (int i = 0; i < TabCount; i++)
            {
                _lastSeenIndexPerTab[i] = -1;
                _lastSeenCodexPerTab[i] = -1;
            }
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
                HudIsOpen = HudIsOpen,
                ActiveTab = ActiveTab,
                LastSeenIndexPerTab = (int[])_lastSeenIndexPerTab.Clone(),
                LastSeenCodexPerTab = (int[])_lastSeenCodexPerTab.Clone(),
                CodexUnlockCount = CodexUnlockCount
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
            ActiveTab = Math.Max(0, Math.Min(save.ActiveTab, TabCount - 1));
            CodexUnlockCount = Math.Max(0, save.CodexUnlockCount);
            if (save.LastSeenIndexPerTab != null)
            {
                for (int i = 0; i < TabCount && i < save.LastSeenIndexPerTab.Length; i++)
                    _lastSeenIndexPerTab[i] = save.LastSeenIndexPerTab[i];
            }
            if (save.LastSeenCodexPerTab != null)
            {
                for (int i = 0; i < TabCount && i < save.LastSeenCodexPerTab.Length; i++)
                    _lastSeenCodexPerTab[i] = save.LastSeenCodexPerTab[i];
            }
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
        public int ActiveTab;
        public int[] LastSeenIndexPerTab;
        public int[] LastSeenCodexPerTab;
        public int CodexUnlockCount;
    }
}
