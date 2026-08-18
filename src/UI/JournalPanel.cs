using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Journal panel.
    /// Shows day logs, personal notes, narrative progression, and story entries.
    /// Wrapped in the ASHFALL Dashboard Shell so a sidebar lets the user jump
    /// between Day Logs / Personal Notes / Story Progression sections. The
    /// shell preserves the existing modal-style chrome (warm amber, dim
    /// labels, 9-slice frame) — Stitch's sidebar / memorial layout is
    /// deliberately treated as inspiration, not contract.
    /// </summary>
    public partial class JournalPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblLogsTitle;
        private VBoxContainer _logEntries;
        private Label _lblNotesTitle;
        private VBoxContainer _notesList;
        private Label _lblStoryTitle;
        private VBoxContainer _storyEntries;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;

        // Placeholder journal data
        private readonly string[] _placeholderLogs = {
            "[Day 1] The exchange is over. We survived the initial blast. Bunker is intact.",
            "[Day 3] First day outside. Radiation levels elevated but manageable.",
            "[Day 5] Found a survivor group at the perimeter. Tented them in the east wing.",
            "[Day 8] Radio contact established. Unknown frequency, but they're listening.",
            "[Day 12] Water supply critical. Need to find a clean source within 5km.",
            "[Day 15] Medical supplies running low. Need to scavenge or trade."
        };

        private readonly string[] _placeholderNotes = {
            "Elena — Leader. Decisive but carries the weight of command.",
            "Marcus — Medic. Skilled but haunted by what he's seen.",
            "Yuki — Scout. Quiet, observant, knows the wasteland.",
            "David — Engineer. Pragmatic, builds what's needed, when it's needed.",
            "Sofia — Trader. Calculating, but not unkind. Sees opportunity in everything."
        };

        private readonly string[] _placeholderStory = {
            "Chapter 1: The Exchange — Nuclear detonations across the globe.",
            "Chapter 2: Ashfall — Surviving the initial fallout and radiation.",
            "Chapter 3: The Bunker — Establishing shelter and community.",
            "Chapter 4: First Contact — Encountering other survivors.",
            "Chapter 5: The Long Winter — Nuclear winter conditions setting in.",
            "Chapter 6: Scavenging — venturing out for supplies and knowledge.",
            "Chapter 7: The Ledger — Documenting everything, everything matters.",
            "Chapter 8: The Radio — Listening for hope in the static.",
            "Chapter 9: The Expedition — Risking the wasteland for resources.",
            "Chapter 10: The Choice — What survives, what's sacrificed."
        };

        // Real data from host session
        // private JournalHostSession? _journalHost;

        public void Bind(object journal) // placeholder for JournalHostSession
        {
            // _journalHost = (JournalHostSession)journal;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_logEntries == null || _notesList == null || _storyEntries == null) return;

            // Clear existing entries
            while (_logEntries.GetChildCount() > 0)
                _logEntries.RemoveChild(_logEntries.GetChild(0));
            while (_notesList.GetChildCount() > 0)
                _notesList.RemoveChild(_notesList.GetChild(0));
            while (_storyEntries.GetChildCount() > 0)
                _storyEntries.RemoveChild(_storyEntries.GetChild(0));

            // Display placeholder day logs
            foreach (string log in _placeholderLogs)
            {
                var label = new Label { Text = log };
                label.CustomMinimumSize = new Vector2(450, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _logEntries.AddChild(label);
            }

            // Display placeholder character notes
            foreach (string note in _placeholderNotes)
            {
                var label = new Label { Text = note };
                label.CustomMinimumSize = new Vector2(450, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                _notesList.AddChild(label);
            }

            // Display placeholder story chapters
            foreach (string chapter in _placeholderStory)
            {
                var label = new Label { Text = chapter };
                label.CustomMinimumSize = new Vector2(450, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _storyEntries.AddChild(label);
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            // Dashboard shell — sidebar lets the user jump between Day Logs /
            // Personal Notes / Story Progression. No status rail here — the
            // journal is a narrative surface, not a metrics dashboard.
            _shell = new AshfallDashboardShell(
                "JOURNAL & NARRATIVE", 820, 600);
            center.AddChild(_shell);
            _sidebar = _shell.SetSidebar(new[]
            {
                new AshfallSidebar.Item { Id = "logs",    Label = "Day Logs",      Hint = "CHRONICLE" },
                new AshfallSidebar.Item { Id = "notes",   Label = "Personal Notes",Hint = "COHORT NOTATIONS" },
                new AshfallSidebar.Item { Id = "story",   Label = "Story",         Hint = "PROGRESSION" },
            }, "CHAPTERS", "logs");
            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

            // Content slot — single scrollable VBox that hosts the three
            // named sub-sections. Sidebar selection scrolls the matching
            // sub-section into view.
            var scrollRoot = new ScrollContainer();
            scrollRoot.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollRoot.SizeFlagsVertical = SizeFlags.ExpandFill;
            var scrollMargin = new MarginContainer();
            scrollMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingLg);
            scrollMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingLg);
            scrollMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            scrollRoot.AddChild(scrollMargin);
            _shell.SetContent(scrollRoot);

            var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingLg);
            vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollMargin.AddChild(vbox);

            // Day logs section
            _lblLogsTitle = AshfallUiHelpers.MakeSectionHeader("DAY LOGS");
            vbox.AddChild(_lblLogsTitle);

            _logEntries = new VBoxContainer();
            _logEntries.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _logEntries.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_logEntries);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Character notes section
            _lblNotesTitle = AshfallUiHelpers.MakeSectionHeader("PERSONAL NOTES");
            vbox.AddChild(_lblNotesTitle);

            _notesList = new VBoxContainer();
            _notesList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _notesList.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_notesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Story chapters section
            _lblStoryTitle = AshfallUiHelpers.MakeSectionHeader("STORY PROGRESSION");
            vbox.AddChild(_lblStoryTitle);

            _storyEntries = new VBoxContainer();
            _storyEntries.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _storyEntries.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_storyEntries);

            if (_sidebar != null)
            {
                _sidebar.OnSelected += id =>
                {
                    if (id == "logs" && _lblLogsTitle != null)
                        ScrollToChild(scrollRoot, _lblLogsTitle);
                    else if (id == "notes" && _lblNotesTitle != null)
                        ScrollToChild(scrollRoot, _lblNotesTitle);
                    else if (id == "story" && _lblStoryTitle != null)
                        ScrollToChild(scrollRoot, _lblStoryTitle);
                };
            }

            // Populate placeholder entries (SnapshotHarness doesn't bind a
            // host session, so we render the in-file fixtures so the screen
            // remains inspectable).
            RefreshView();
        }

        private static void ScrollToChild(ScrollContainer scroll, Control child)
        {
            if (scroll == null || child == null) return;
            try
            {
                float targetOffset = 0f;
                Node walker = child;
                while (walker != null && walker != scroll)
                {
                    if (walker is Control w && walker != scroll)
                        targetOffset += w.Position.Y;
                    walker = walker.GetParent();
                }
                if (targetOffset > 0)
                    scroll.ScrollVertical = (int)Math.Max(0, targetOffset - 8);
            }
            catch
            {
                // best-effort
            }
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
