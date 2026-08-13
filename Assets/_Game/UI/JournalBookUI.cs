using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Events;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Diegetic journal book: playthrough log + immersive tutorial pages +
    /// codex tabs (Items / People / Places / Events) fed by a JournalCodex
    /// builder. No modal popups — new discoveries ping the strip; player
    /// opens with [J], switches tabs with [1]-[5].
    /// </summary>
    public class JournalBookUI : MonoBehaviour
    {
        public const int MaxVisibleCollapsed = 4;
        public const int MaxVisibleOpen = JournalSystem.MaxEntries;

        public bool IsOpen { get; private set; }
        public bool HasUnread { get; private set; }
        /// <summary>True after a new entry until acknowledged (open or MarkRead).</summary>
        public bool NotificationPing { get; private set; }
        public int NotificationPingCount { get; private set; }
        public int EntryCount { get; private set; }
        public string LatestText { get; private set; } = string.Empty;
        public string LatestAuthor { get; private set; } = string.Empty;
        public string LatestTimestamp { get; private set; } = string.Empty;
        public string StatusLine { get; private set; } = "JOURNAL: —";
        public string DetailSummary { get; private set; } = "No entries yet.";

        /// <summary>Active tab index; 0 = Log. Mirrors JournalSystem.ActiveTab.</summary>
        public int ActiveTab { get; private set; }

        private readonly List<JournalEntry> _entries = new List<JournalEntry>();

        /// <summary>Optional codex row builder; null hides the non-Log tabs.</summary>
        private Func<JournalTab, IReadOnlyList<JournalCodexRow>> _codexProvider;

        /// <summary>Optional per-tab unread lookup (mirrors JournalSystem.HasUnreadForTab).</summary>
        private Func<int, bool> _unreadProvider;

        public IReadOnlyList<JournalEntry> Entries => _entries;

        public event Action OnOpened;
        public event Action OnClosed;
        public event Action<JournalEntry> OnEntryPushed;
        public event Action<int> OnTabChanged;

        /// <summary>Bind the codex row builder (called once during HUD wiring).</summary>
        public void SetCodexProvider(Func<JournalTab, IReadOnlyList<JournalCodexRow>> provider)
        {
            _codexProvider = provider;
            Refresh();
        }

        /// <summary>Bind the per-tab unread lookup (called once during HUD wiring).</summary>
        public void SetUnreadProvider(Func<int, bool> provider)
        {
            _unreadProvider = provider;
            Refresh();
        }

        /// <summary>Switch the visible tab (0..TabCount-1). Raises OnTabChanged.</summary>
        public void SwitchTab(int tab)
        {
            int clamped = tab < 0 ? 0 : (tab >= JournalSystem.TabCount ? JournalSystem.TabCount - 1 : tab);
            if (clamped == ActiveTab) return;
            ActiveTab = clamped;
            OnTabChanged?.Invoke(clamped);
            Refresh();
        }

        /// <summary>
        /// Push a new journal entry (from JournalSystem.OnEntryAdded).
        /// Triggers notification ping.
        /// </summary>
        public void Push(JournalEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.Text)) return;
            _entries.Insert(0, entry);
            while (_entries.Count > JournalSystem.MaxEntries)
                _entries.RemoveAt(_entries.Count - 1);
            HasUnread = true;
            NotificationPing = true;
            NotificationPingCount++;
            OnEntryPushed?.Invoke(entry);
            Refresh();
        }

        /// <summary>Replace book contents (save restore / full sync).</summary>
        public void SetEntries(IReadOnlyList<JournalEntry> entries)
        {
            _entries.Clear();
            if (entries != null)
            {
                for (int i = 0; i < entries.Count && i < JournalSystem.MaxEntries; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.Text)) continue;
                    _entries.Add(e);
                }
            }
            Refresh();
        }

        public void ApplyUiState(bool isOpen, bool hasUnread, bool notificationPing = false, int activeTab = 0)
        {
            IsOpen = isOpen;
            HasUnread = hasUnread;
            NotificationPing = notificationPing;
            ActiveTab = activeTab < 0 ? 0 : (activeTab >= JournalSystem.TabCount ? JournalSystem.TabCount - 1 : activeTab);
            if (isOpen)
            {
                HasUnread = false;
                NotificationPing = false;
            }
            Refresh();
        }

        public void Clear()
        {
            _entries.Clear();
            HasUnread = false;
            NotificationPing = false;
            NotificationPingCount = 0;
            Refresh();
        }

        public void Open()
        {
            IsOpen = true;
            HasUnread = false;
            NotificationPing = false;
            OnOpened?.Invoke();
            Refresh();
        }

        public void Close()
        {
            IsOpen = false;
            OnClosed?.Invoke();
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public void MarkRead()
        {
            HasUnread = false;
            NotificationPing = false;
            Refresh();
        }

        public void AcknowledgePing()
        {
            NotificationPing = false;
            Refresh();
        }

        public void Refresh()
        {
            EntryCount = _entries.Count;
            if (_entries.Count == 0 && _codexProvider == null)
            {
                LatestText = string.Empty;
                LatestAuthor = string.Empty;
                LatestTimestamp = string.Empty;
                StatusLine = IsOpen
                    ? "JOURNAL [OPEN]  empty  [J] close"
                    : "JOURNAL  empty  [J]";
                if (NotificationPing) StatusLine = "JOURNAL · PING  empty  [J]";
                DetailSummary = IsOpen
                    ? "JOURNAL [J]\nNo pages yet. Survivors write when they learn something."
                    : "No journal entries.";
                return;
            }

            if (IsOpen && ActiveTab != 0 && _codexProvider != null)
            {
                RefreshCodexTab();
                return;
            }

            if (_entries.Count == 0)
            {
                LatestText = string.Empty;
                LatestAuthor = string.Empty;
                LatestTimestamp = string.Empty;
                StatusLine = IsOpen
                    ? "JOURNAL [OPEN]  empty  [J] close"
                    : "JOURNAL  empty  [J]";
                if (NotificationPing) StatusLine = "JOURNAL · PING  empty  [J]";
                DetailSummary = IsOpen
                    ? "JOURNAL [J]\nNo pages yet. Survivors write when they learn something."
                    : "No journal entries.";
                return;
            }

            var top = _entries[0];
            LatestText = top.Text ?? string.Empty;
            LatestAuthor = top.AuthorName ?? string.Empty;
            LatestTimestamp = !string.IsNullOrEmpty(top.Timestamp)
                ? top.Timestamp
                : $"Day {top.Day}";

            string unread = HasUnread ? " · NEW" : string.Empty;
            string ping = NotificationPing ? " · PING" : string.Empty;
            string openMark = IsOpen ? " [OPEN]" : "";
            string shortBody = LatestText.Length > 48
                ? LatestText.Substring(0, 45) + "…"
                : LatestText;
            StatusLine = $"JOURNAL{openMark}{unread}{ping}  {LatestTimestamp} · {LatestAuthor}  {shortBody}";
            if (!IsOpen)
                StatusLine += "  [J]";

            int max = IsOpen ? MaxVisibleOpen : MaxVisibleCollapsed;
            var sb = new StringBuilder();
            sb.AppendLine(StatusLine);
            if (IsOpen)
                sb.AppendLine($"--- journal (newest first) — tab {ActiveTab + 1}/{JournalSystem.TabCount} [1]-[5] ---");
            int shown = 0;
            for (int i = 0; i < _entries.Count && shown < max; i++)
            {
                var e = _entries[i];
                string when = !string.IsNullOrEmpty(e.Timestamp) ? e.Timestamp : $"Day {e.Day}";
                string who = !string.IsNullOrEmpty(e.AuthorName) ? e.AuthorName : "—";
                sb.AppendLine($"  · {when} — {who}");
                sb.AppendLine($"    {e.Text}");
                shown++;
            }
            if (!IsOpen && _entries.Count > MaxVisibleCollapsed)
                sb.AppendLine($"  … +{_entries.Count - MaxVisibleCollapsed} older  [J] open book");
            if (IsOpen)
                sb.AppendLine("[J] close book");
            DetailSummary = sb.ToString().TrimEnd();
        }

        /// <summary>Render the active codex tab (Items/People/Places/Events).</summary>
        private void RefreshCodexTab()
        {
            var rows = _codexProvider != null ? _codexProvider((JournalTab)ActiveTab) : null;
            bool tabUnread = _unreadProvider != null && _unreadProvider(ActiveTab);
            var sb = new StringBuilder();
            string unreadMark = tabUnread ? " · NEW" : string.Empty;
            sb.AppendLine($"JOURNAL  tab {ActiveTab + 1}/{JournalSystem.TabCount}  {(JournalTab)ActiveTab}{unreadMark}  [1]-[5]");
            sb.AppendLine($"--- {(JournalTab)ActiveTab} ---");
            if (rows == null || rows.Count == 0)
            {
                sb.AppendLine("  No pages here yet.");
                DetailSummary = sb.ToString().TrimEnd();
                StatusLine = $"JOURNAL [{(JournalTab)ActiveTab}]  empty  [1]-[5]";
                return;
            }
            int shown = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row.IsLocked)
                {
                    sb.AppendLine($"  · [---] {row.DisplayName}");
                    if (shown < MaxVisibleOpen)
                        sb.AppendLine($"    {row.Body}");
                }
                else
                {
                    sb.AppendLine($"  · {row.DisplayName}" + (string.IsNullOrEmpty(row.Meta) ? string.Empty : $"  ({row.Meta})"));
                    sb.AppendLine($"    {row.Body}");
                }
                shown++;
                if (shown >= MaxVisibleOpen) break;
            }
            if (rows.Count > MaxVisibleOpen)
                sb.AppendLine($"  … +{rows.Count - MaxVisibleOpen} more");
            DetailSummary = sb.ToString().TrimEnd();
            StatusLine = $"JOURNAL [{(JournalTab)ActiveTab}]  {rows.Count} entries{unreadMark}  [1]-[5]";
        }
    }
}
