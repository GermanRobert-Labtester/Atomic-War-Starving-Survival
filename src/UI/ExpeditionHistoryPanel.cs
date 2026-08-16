using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Expedition History panel.
    /// Shows detailed expedition history, outcomes, and lessons learned.
    /// </summary>
    public partial class ExpeditionHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _expeditionHistory;
        private Label _lblOutcomesTitle;
        private VBoxContainer _expeditionOutcomes;
        private Label _lblLessonsTitle;
        private VBoxContainer _lessonsLearned;

        private readonly string[] _placeholderHistory = {
            "[Day 7] Raid on Supply Caravan — Victory, 2 casualties",
            "[Day 12] Ambush in Sector 4 — Retreat, 1 casualty",
            "[Day 18] Bunker Defense — Victory, 0 casualties",
            "[Day 22] Skirmish at Radio Tower — Inconclusive, 3 casualties",
            "[Day 25] Supply Run to Sector 12 — Successful, 0 casualties"
        };

        private readonly string[] _placeholderOutcomes = {
            "Total Expeditions: 5",
            "Victories: 2 (40%)",
            "Retreats: 1 (20%)",
            "Inconclusive: 1 (20%)",
            "Successful: 1 (20%)",
            "Total Resources Gained: +35 rations, +5 medicine",
            "Total Casualties: 6 (2 killed, 4 wounded)"
        };

        private readonly string[] _placeholderLessons = {
            "Day 7: Ambush tactics effective — Replicate for future raids",
            "Day 12: Retreat planning necessary — Pre-planned escape routes",
            "Day 18: Defensive positioning strong — Improve perimeter",
            "Day 22: Communication critical — Hand signals + radios",
            "Day 25: Supply runs successful — Continue regular expeditions",
            "Overall: Improved tactics, better preparedness"
        };

        public void Bind(object expeditionHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_expeditionHistory == null || _expeditionOutcomes == null || _lessonsLearned == null) return;

            while (_expeditionHistory.GetChildCount() > 0) _expeditionHistory.RemoveChild(_expeditionHistory.GetChild(0));
            while (_expeditionOutcomes.GetChildCount() > 0) _expeditionOutcomes.RemoveChild(_expeditionOutcomes.GetChild(0));
            while (_lessonsLearned.GetChildCount() > 0) _lessonsLearned.RemoveChild(_lessonsLearned.GetChild(0));

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _expeditionHistory.AddChild(label);
            }

            foreach (string outcome in _placeholderOutcomes)
            {
                var label = new Label { Text = outcome };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _expeditionOutcomes.AddChild(label);
            }

            foreach (string lesson in _placeholderLessons)
            {
                var label = new Label { Text = lesson };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _lessonsLearned.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("EXPEDITION HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("EXPEDITION HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _expeditionHistory = new VBoxContainer();
            _expeditionHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _expeditionHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_expeditionHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("EXPEDITION OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _expeditionOutcomes = new VBoxContainer();
            _expeditionOutcomes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _expeditionOutcomes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_expeditionOutcomes);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblLessonsTitle = AshfallUiHelpers.MakeSectionHeader("LESSONS LEARNED");
            vbox.AddChild(_lblLessonsTitle);

            _lessonsLearned = new VBoxContainer();
            _lessonsLearned.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _lessonsLearned.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_lessonsLearned);

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
