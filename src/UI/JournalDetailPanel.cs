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
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("JOURNAL DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEntriesTitle = AshfallUiHelpers.MakeSectionHeader("RECENT ENTRIES");
            vbox.AddChild(_lblEntriesTitle);
            _entriesList = new VBoxContainer();
            _entriesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _entriesList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_entriesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblCodexTitle = AshfallUiHelpers.MakeSectionHeader("CODEX UNLOCKS");
            vbox.AddChild(_lblCodexTitle);
            _codexList = new VBoxContainer();
            _codexList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _codexList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_codexList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTabsTitle = AshfallUiHelpers.MakeSectionHeader("TAB STATE");
            vbox.AddChild(_lblTabsTitle);
            _tabsList = new VBoxContainer();
            _tabsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tabsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_tabsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
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
