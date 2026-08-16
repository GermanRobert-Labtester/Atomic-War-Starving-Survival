using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Journal panel.
    /// Shows day logs, personal notes, narrative progression, and story entries.
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

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("JOURNAL & NARRATIVE", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Day logs section
            _lblLogsTitle = AshfallUiHelpers.MakeSectionHeader("DAY LOGS");
            vbox.AddChild(_lblLogsTitle);

            _logEntries = new VBoxContainer();
            _logEntries.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _logEntries.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_logEntries);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Character notes section
            _lblNotesTitle = AshfallUiHelpers.MakeSectionHeader("PERSONAL NOTES");
            vbox.AddChild(_lblNotesTitle);

            _notesList = new VBoxContainer();
            _notesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _notesList.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_notesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Story chapters section
            _lblStoryTitle = AshfallUiHelpers.MakeSectionHeader("STORY PROGRESSION");
            vbox.AddChild(_lblStoryTitle);

            _storyEntries = new VBoxContainer();
            _storyEntries.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _storyEntries.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_storyEntries);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
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
