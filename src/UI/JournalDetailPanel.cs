using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Journal;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Journal Detail panel.
    /// Shows journal entries, codex unlocks, and tab state — bound to the
    /// live JournalHostSession. Unbound renders an honest empty state.
    /// </summary>
    public partial class JournalDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblEntriesTitle;
        private VBoxContainer _entriesList;
        private Label _lblCodexTitle;
        private VBoxContainer _codexList;
        private Label _lblTabsTitle;
        private VBoxContainer _tabsList;

        private JournalSystem? _journal;

        public bool IsBound => _journal != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(JournalSystem? journal)
        {
            _journal = journal;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_entriesList == null || _codexList == null || _tabsList == null) return;

            AshfallUiHelpers.EmptyChildren(_entriesList);
            AshfallUiHelpers.EmptyChildren(_codexList);
            AshfallUiHelpers.EmptyChildren(_tabsList);

            RenderedRowCount = 0;

            if (_journal == null)
            {
                _entriesList.AddChild(MakeDimLine("No journal session bound."));
                return;
            }

            var sys = _journal;

            // ── Recent entries ──
            foreach (var entry in sys.Entries.Take(15))
            {
                if (entry == null) continue;
                AddRow(_entriesList, $"[Day {entry.Day}] {entry.KnowledgeKey} — {entry.AuthorName}",
                    Ashfall.Core.UI.Theme.Pale);
                RenderedRowCount++;
            }
            if (sys.EntryCount == 0)
                _entriesList.AddChild(MakeDimLine("No journal entries yet."));

            // ── Codex unlocks ──
            AddRow(_codexList, $"Codex unlocks: {sys.CodexUnlockCount}", Ashfall.Core.UI.Theme.Lethe);
            AddRow(_codexList, $"Has unread: {sys.HasUnread}", sys.HasUnread ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 2;

            // ── Tab state ──
            AddRow(_tabsList, $"Active tab: {sys.ActiveTab} / {JournalSystem.TabCount - 1}", Ashfall.Core.UI.Theme.Pale);
            for (int t = 0; t < JournalSystem.TabCount; t++)
            {
                AddRow(_tabsList, $"Tab {t}: {(sys.HasUnreadForTab(t) ? "unread" : "read")}",
                    sys.HasUnreadForTab(t) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount++;
            }
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        public override void _Ready()
        {
            // Ticket #125: layout chrome owned by res://assets/ui/panels/JournalDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
            // Sibling refresh code is unchanged.
            var binder = new SceneBinder(this, typeof(JournalDetailPanel));
            binder.Require<VBoxContainer>("EntriesList");
            binder.Require<VBoxContainer>("CodexList");
            binder.Require<VBoxContainer>("TabsList");
            binder.Require<Button>("CloseButton");
            _entriesList = binder.Get<VBoxContainer>("EntriesList");
            _codexList = binder.Get<VBoxContainer>("CodexList");
            _tabsList = binder.Get<VBoxContainer>("TabsList");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
