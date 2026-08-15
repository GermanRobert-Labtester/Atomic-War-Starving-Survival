using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using Ashfall.Core.Journal;

namespace AtomicWar.Journal
{
    /// <summary>
    /// Diegetic journal book (Godot): playthrough log + codex tabs
    /// (Items / People / Places / Events) fed by a JournalCodex builder.
    /// No modal popups — new discoveries ping the footer strip; the player
    /// opens with [J], switches tabs with [1]-[5], closes with [Esc]/[J].
    /// Also renders StatusLine/DetailSummary text mirrors for the collapsed
    /// HUD strip (docs/ui/JOURNAL_UI_PLAN.md §9).
    /// </summary>
    public partial class JournalBookUI : Control
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

        public IReadOnlyList<JournalEntry> Entries => _entries;

        public event Action? OnOpened;
        public event Action? OnClosed;
        public event Action<JournalEntry>? OnEntryPushed;
        public event Action<int>? OnTabChanged;

        private JournalSystem? _journal;
        private Func<JournalTab, IReadOnlyList<JournalCodexRow>>? _codexProvider;
        private Func<int, bool>? _unreadProvider;
        private Func<int>? _dayProvider;

        private readonly List<JournalEntry> _entries = new List<JournalEntry>();

        // Godot nodes
        private PanelContainer _panel = null!;
        private Label _headerLabel = null!;
        private readonly List<Button> _tabButtons = new List<Button>();
        private ScrollContainer _scroll = null!;
        private RichTextLabel _content = null!;
        private Label _footerLabel = null!;

        // Palette — uses Theme.cs tokens for consistency.
        private static readonly Color ColAmber = new Color(Ashfall.Core.UI.Theme.Hot.r, Ashfall.Core.UI.Theme.Hot.g, Ashfall.Core.UI.Theme.Hot.b, Ashfall.Core.UI.Theme.Hot.a);
        private static readonly Color ColTeal = new Color(Ashfall.Core.UI.Theme.Lethe.r, Ashfall.Core.UI.Theme.Lethe.g, Ashfall.Core.UI.Theme.Lethe.b, Ashfall.Core.UI.Theme.Lethe.a);
        private static readonly Color ColBody = new Color(Ashfall.Core.UI.Theme.Pale.r, Ashfall.Core.UI.Theme.Pale.g, Ashfall.Core.UI.Theme.Pale.b, Ashfall.Core.UI.Theme.Pale.a);
        private static readonly Color ColMeta = new Color(Ashfall.Core.UI.Theme.Muted.r, Ashfall.Core.UI.Theme.Muted.g, Ashfall.Core.UI.Theme.Muted.b, Ashfall.Core.UI.Theme.Muted.a);
        private static readonly Color ColLocked = new Color(Ashfall.Core.UI.Theme.Dim.r, Ashfall.Core.UI.Theme.Dim.g, Ashfall.Core.UI.Theme.Dim.b, Ashfall.Core.UI.Theme.Dim.a);
        private static readonly Color ColRust = new Color(Ashfall.Core.UI.Theme.Entropy.r, Ashfall.Core.UI.Theme.Entropy.g, Ashfall.Core.UI.Theme.Entropy.b, Ashfall.Core.UI.Theme.Entropy.a);

        /// <summary>Bind the journal + codex row builder (called once during HUD wiring).</summary>
        public void Bind(
            JournalSystem journal,
            Func<JournalTab, IReadOnlyList<JournalCodexRow>> codexProvider,
            Func<int, bool>? unreadProvider = null,
            Func<int>? dayProvider = null)
        {
            _journal = journal;
            _codexProvider = codexProvider;
            _unreadProvider = unreadProvider;
            _dayProvider = dayProvider;
            if (_journal != null)
            {
                _journal.OnEntryAdded -= HandleEntryAdded;
                _journal.OnEntryAdded += HandleEntryAdded;
                _journal.OnTabChanged -= HandleSystemTabChanged;
                _journal.OnTabChanged += HandleSystemTabChanged;
                _journal.OnCodexUnlocked -= HandleCodexUnlocked;
                _journal.OnCodexUnlocked += HandleCodexUnlocked;
            }
            Refresh();
        }

        public override void _Ready()
        {
            BuildVisualTree();
            Refresh();
        }

        public override void _ExitTree()
        {
            if (_journal != null)
            {
                _journal.OnEntryAdded -= HandleEntryAdded;
                _journal.OnTabChanged -= HandleSystemTabChanged;
                _journal.OnCodexUnlocked -= HandleCodexUnlocked;
            }
        }

        private void HandleEntryAdded(JournalEntry entry) => Push(entry);
        private void HandleCodexUnlocked(string key) => Refresh();

        private void HandleSystemTabChanged(int tab)
        {
            if (ActiveTab != tab)
                SwitchTab(tab);
        }

        // -----------------------------------------------------------------
        // Visual tree
        // -----------------------------------------------------------------

        private void BuildVisualTree()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            _panel = new PanelContainer();
            _panel.SetAnchorsPreset(LayoutPreset.FullRect);
            var bg = new StyleBoxFlat
            {
                BgColor = new Color(Ashfall.Core.UI.Theme.Ink.r, Ashfall.Core.UI.Theme.Ink.g, Ashfall.Core.UI.Theme.Ink.b, 0.97f),
                BorderColor = new Color(Ashfall.Core.UI.Theme.Line.r, Ashfall.Core.UI.Theme.Line.g, Ashfall.Core.UI.Theme.Line.b, 0.85f)
            };
            bg.SetBorderWidthAll(2);
            bg.SetCornerRadiusAll(3);
            bg.ContentMarginLeft = 22;
            bg.ContentMarginRight = 22;
            bg.ContentMarginTop = 16;
            bg.ContentMarginBottom = 14;
            _panel.AddThemeStyleboxOverride("panel", bg);
            AddChild(_panel);

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _panel.AddChild(vbox);

            _headerLabel = new Label { Text = "BUNKER LEDGER" };
            _headerLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH2);
            _headerLabel.AddThemeColorOverride("font_color", ColAmber);
            vbox.AddChild(_headerLabel);

            var tabRow = new HBoxContainer();
            tabRow.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            vbox.AddChild(tabRow);

            var group = new ButtonGroup();
            string[] tabNames = { "LOG", "ITEMS", "PEOPLE", "PLACES", "EVENTS" };
            for (int i = 0; i < JournalSystem.TabCount; i++)
            {
                int tab = i;
                var btn = new Button
                {
                    Text = tabNames[i],
                    ToggleMode = true,
                    ButtonGroup = group,
                    CustomMinimumSize = new Vector2(96, 34)
                };
                btn.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                btn.Toggled += (bool on) => { if (on && _journal != null) _journal.SwitchTab(tab); };
                tabRow.AddChild(btn);
                _tabButtons.Add(btn);
            }

            _scroll = new ScrollContainer();
            _scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
            _scroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            vbox.AddChild(_scroll);

            _content = new RichTextLabel
            {
                BbcodeEnabled = true,
                FitContent = true,
                ScrollActive = false,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _scroll.AddChild(_content);

            _footerLabel = new Label { Text = string.Empty };
            _footerLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _footerLabel.AddThemeColorOverride("font_color", ColMeta);
            vbox.AddChild(_footerLabel);
        }

        // -----------------------------------------------------------------
        // State API (mirrors the Unity JournalBookUI surface)
        // -----------------------------------------------------------------

        /// <summary>Switch the visible tab (0..TabCount-1). Raises OnTabChanged.</summary>
        public void SwitchTab(int tab)
        {
            int clamped = tab < 0 ? 0 : (tab >= JournalSystem.TabCount ? JournalSystem.TabCount - 1 : tab);
            if (clamped == ActiveTab) return;
            ActiveTab = clamped;
            OnTabChanged?.Invoke(clamped);
            Refresh();
        }

        /// <summary>Push a new journal entry (from JournalSystem.OnEntryAdded).</summary>
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
            if (_journal != null)
            {
                _journal.HudIsOpen = true;
                _journal.MarkTabViewed(ActiveTab);
            }
            OnOpened?.Invoke();
            Refresh();
        }

        public void Close()
        {
            IsOpen = false;
            if (_journal != null)
                _journal.HudIsOpen = false;
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

        /// <summary>Raw rendered content of the active tab (rich view / smoke tests).</summary>
        public string ActiveTabContent => _content != null ? _content.Text : string.Empty;

        // -----------------------------------------------------------------
        // Rendering
        // -----------------------------------------------------------------

        public void Refresh()
        {
            EntryCount = _entries.Count;
            if (IsOpen && ActiveTab != 0 && _codexProvider != null)
            {
                RefreshCodexTab();
            }
            else if (_entries.Count == 0)
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
            }
            else
            {
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

                var sb = new StringBuilder();
                sb.AppendLine(StatusLine);
                if (IsOpen)
                    sb.AppendLine($"--- journal (newest first) — tab {ActiveTab + 1}/{JournalSystem.TabCount} [1]-[5] ---");
                int max = IsOpen ? MaxVisibleOpen : MaxVisibleCollapsed;
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

            RepaintNodes();
        }

        /// <summary>
        /// Text-mode mirror for the active codex tab (Unity RefreshCodexTab parity).
        /// Also drives StatusLine so the collapsed HUD strip can show codex state.
        /// </summary>
        private void RefreshCodexTab()
        {
            var tab = (JournalTab)ActiveTab;
            var rows = _codexProvider != null ? _codexProvider(tab) : null;
            bool tabUnread = _unreadProvider != null && _unreadProvider(ActiveTab);
            var sb = new StringBuilder();
            string unreadMark = tabUnread ? " · NEW" : string.Empty;
            sb.AppendLine($"JOURNAL  tab {ActiveTab + 1}/{JournalSystem.TabCount}  {tab}{unreadMark}  [1]-[5]");
            sb.AppendLine($"--- {tab} ---");
            if (rows == null || rows.Count == 0)
            {
                sb.AppendLine("  No pages here yet.");
                DetailSummary = sb.ToString().TrimEnd();
                StatusLine = $"JOURNAL [{tab}]  empty  [1]-[5]";
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
            StatusLine = $"JOURNAL [{tab}]  {rows.Count} entries{unreadMark}  [1]-[5]";
        }

        private void RepaintNodes()
        {
            if (_panel == null) return;

            Visible = IsOpen;
            int day = _dayProvider != null ? _dayProvider() : CurrentMaxDay();
            int unread = HasUnread ? 1 : 0;
            if (_journal != null && _journal.HasUnread) unread = 1;
            int today = CountTodayEntries(day);

            _headerLabel.Text =
                $"[J] BUNKER LEDGER    Day {day} · hand-annotated    " +
                (unread > 0 ? "unread: yes" : "no unread") +
                "    [Esc] close";

            for (int i = 0; i < _tabButtons.Count; i++)
            {
                bool tabUnread = _unreadProvider != null
                    ? _unreadProvider(i)
                    : (_journal != null && _journal.HasUnreadForTab(i));
                string[] tabNames = { "LOG", "ITEMS", "PEOPLE", "PLACES", "EVENTS" };
                _tabButtons[i].Text = tabNames[i] + (tabUnread ? " ·NEW" : "");
                _tabButtons[i].ButtonPressed = (i == ActiveTab);
            }

            _content.Text = BuildContentBbcode();
            _footerLabel.Text =
                $"[J] toggle  ·  {(HasUnread ? "new pages" : "nothing new")}  ·  +{today} today  ·  write page";
        }

        private int CurrentMaxDay()
        {
            int day = 1;
            for (int i = 0; i < _entries.Count; i++)
                if (_entries[i].Day > day) day = _entries[i].Day;
            return day;
        }

        private int CountTodayEntries(int day)
        {
            int n = 0;
            for (int i = 0; i < _entries.Count; i++)
                if (_entries[i].Day == day) n++;
            return n;
        }

        private string BuildContentBbcode()
        {
            var sb = new StringBuilder();
            if (IsOpen && ActiveTab != 0 && _codexProvider != null)
            {
                AppendCodexTab(sb);
                return sb.ToString();
            }

            sb.Append(Colored($"--- LOG ---", ColTeal)).Append('\n');
            if (_entries.Count == 0)
            {
                sb.Append(Colored("No pages yet. Survivors write when they learn something.", ColBody));
                return sb.ToString();
            }
            for (int i = 0; i < _entries.Count && i < MaxVisibleOpen; i++)
            {
                var e = _entries[i];
                string when = !string.IsNullOrEmpty(e.Timestamp) ? e.Timestamp : $"Day {e.Day}";
                string who = !string.IsNullOrEmpty(e.AuthorName) ? e.AuthorName : "—";
                sb.Append(Colored($"· {when} — {who}", ColAmber)).Append('\n');
                sb.Append(Colored(Escape(e.Text), ColBody)).Append('\n');
                string tag = DeriveTag(e.KnowledgeKey);
                if (!string.IsNullOrEmpty(tag))
                    sb.Append(Colored($"  [{tag}]", ColMeta)).Append('\n');
            }
            return sb.ToString();
        }

        private void AppendCodexTab(StringBuilder sb)
        {
            var tab = (JournalTab)ActiveTab;
            var rows = _codexProvider != null ? _codexProvider(tab) : null;
            bool tabUnread = _unreadProvider != null && _unreadProvider(ActiveTab);
            string unreadMark = tabUnread ? " · NEW" : string.Empty;
            sb.Append(Colored($"--- {tab} ---{unreadMark}", ColTeal)).Append('\n');
            if (rows == null || rows.Count == 0)
            {
                sb.Append(Colored("No pages here yet.", ColBody));
                return;
            }
            for (int i = 0; i < rows.Count && i < MaxVisibleOpen; i++)
            {
                var row = rows[i];
                if (row.IsLocked)
                {
                    sb.Append(Colored($"· [---] {Escape(row.DisplayName)}", ColLocked)).Append('\n');
                    sb.Append(Colored($"  {Escape(row.Body)}", ColLocked)).Append('\n');
                }
                else
                {
                    string meta = string.IsNullOrEmpty(row.Meta) ? string.Empty : $"  ({row.Meta})";
                    sb.Append(Colored($"· {Escape(row.DisplayName)}", ColAmber))
                        .Append(Colored(meta, ColMeta)).Append('\n');
                    sb.Append(Colored(Escape(row.Body), ColBody)).Append('\n');
                }
            }
            if (rows.Count > MaxVisibleOpen)
                sb.Append(Colored($"… +{rows.Count - MaxVisibleOpen} more", ColMeta));
        }

        /// <summary>Auto tag derived from knowledge-key namespace conventions.</summary>
        private static string DeriveTag(string? knowledgeKey)
        {
            if (string.IsNullOrEmpty(knowledgeKey)) return string.Empty;
            if (knowledgeKey.StartsWith("anchor_broadcast_")) return "broadcast";
            if (knowledgeKey.StartsWith("item_seen_")) return "discovery";
            if (knowledgeKey.StartsWith("location_visited_")) return "field note";
            if (knowledgeKey.StartsWith("survivor_met_")) return "survivor";
            if (knowledgeKey.StartsWith("event_fired_")) return "event";
            return "lore";
        }

        private static string Colored(string? text, Color color)
        {
            return $"[color=#{color.R8:X2}{color.G8:X2}{color.B8:X2}]{text ?? string.Empty}[/color]";
        }

        private static string Escape(string? text)
        {
            return (text ?? string.Empty).Replace("[", "[lb]");
        }
    }
}
