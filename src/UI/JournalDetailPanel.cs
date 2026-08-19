using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Journal Detail panel.
    /// Shows detailed journal entries, personal notes, and narrative progression.
    /// </summary>
    public partial class JournalDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblEntryTitle;
        private VBoxContainer _entryContent;
        private Label _lblNotesTitle;
        private VBoxContainer _personalNotes;
        private Label _lblNarrativeTitle;
        private VBoxContainer _narrativeProgress;

        private readonly string[] _placeholderEntry = {
            "Day 25 — The Ledger Continues",
            "Today we discovered a supply cache in Sector 12. +15 rations, +3 medicine. The wasteland still holds secrets for those brave enough to search.",
            "Marcus treated Yuki's minor wound from perimeter patrol. No serious injuries today.",
            "Sofia negotiated a trade with the Black Flotilla. 5 food for 2 medicine. Good deal.",
            "Radiation levels stable. Shelter holding up. We survive another day."
        };

        private readonly string[] _placeholderNotes = {
            "Elena — Leadership is heavy but necessary. She makes tough calls.",
            "Marcus — Best medic we have. Haunted but reliable.",
            "Yuki — Quiet scout. Knows the wasteland better than anyone.",
            "David — Engineer par excellence. Builds what we need.",
            "Sofia — Trader with a conscience. Fair but calculating."
        };

        private readonly string[] _placeholderNarrative = {
            "Chapter 1: The Exchange — Nuclear detonations across the globe",
            "Chapter 2: Ashfall — Surviving the initial fallout and radiation",
            "Chapter 3: The Bunker — Establishing shelter and community",
            "Chapter 4: First Contact — Encountering other survivors",
            "Chapter 5: The Long Winter — Nuclear winter conditions setting in",
            "Chapter 6: Scavenging — Venturing out for supplies and knowledge",
            "Chapter 7: The Ledger — Documenting everything, everything matters",
            "Chapter 8: The Radio — Listening for hope in the static",
            "Chapter 9: The Expedition — Risking the wasteland for resources",
            "Chapter 10: The Choice — What survives, what's sacrificed"
        };

        public void Bind(object journal)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_entryContent == null || _personalNotes == null || _narrativeProgress == null) return;

            AshfallUiHelpers.EmptyChildren(_entryContent);
            AshfallUiHelpers.EmptyChildren(_personalNotes);
            AshfallUiHelpers.EmptyChildren(_narrativeProgress);

            foreach (string entry in _placeholderEntry)
            {
                var label = new Label { Text = entry };
                label.CustomMinimumSize = new Vector2(400, 40);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _entryContent.AddChild(label);
            }

            foreach (string note in _placeholderNotes)
            {
                var label = new Label { Text = note };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                _personalNotes.AddChild(label);
            }

            foreach (string narrative in _placeholderNarrative)
            {
                var label = new Label { Text = narrative };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _narrativeProgress.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("JOURNAL DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEntryTitle = AshfallUiHelpers.MakeSectionHeader("JOURNAL ENTRY");
            vbox.AddChild(_lblEntryTitle);

            _entryContent = new VBoxContainer();
            _entryContent.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _entryContent.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_entryContent);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNotesTitle = AshfallUiHelpers.MakeSectionHeader("PERSONAL NOTES");
            vbox.AddChild(_lblNotesTitle);

            _personalNotes = new VBoxContainer();
            _personalNotes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _personalNotes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_personalNotes);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNarrativeTitle = AshfallUiHelpers.MakeSectionHeader("NARRATIVE PROGRESSION");
            vbox.AddChild(_lblNarrativeTitle);

            _narrativeProgress = new VBoxContainer();
            _narrativeProgress.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _narrativeProgress.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_narrativeProgress);

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
