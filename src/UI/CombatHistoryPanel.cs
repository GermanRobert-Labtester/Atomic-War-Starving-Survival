using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Combat History panel.
    /// Shows detailed combat history, battle outcomes, and tactical analysis.
    /// </summary>
    public partial class CombatHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _combatHistory;
        private Label _lblOutcomesTitle;
        private VBoxContainer _battleOutcomes;
        private Label _lblTacticsTitle;
        private VBoxContainer _tacticalAnalysis;

        private readonly string[] _placeholderHistory = {
            "[Day 7] Raid on Supply Caravan — Victory, 2 casualties",
            "[Day 12] Ambush in Sector 4 — Retreat, 1 casualty",
            "[Day 18] Bunker Defense — Victory, 0 casualties",
            "[Day 22] Skirmish at Radio Tower — Inconclusive, 3 casualties",
            "[Day 25] Supply Run to Sector 12 — Successful, 0 casualties"
        };

        private readonly string[] _placeholderOutcomes = {
            "Total Engagements: 5",
            "Victories: 2 (40%)",
            "Retreats: 1 (20%)",
            "Inconclusive: 1 (20%)",
            "Successful: 1 (20%)",
            "Total Casualties: 6 (2 killed, 4 wounded)",
            "Resources Gained: +35 rations, +5 medicine",
            "Intel Gathered: 3 enemy positions mapped"
        };

        private readonly string[] _placeholderTactics = {
            "Ambush Tactics: Effective (Day 7, Day 12)",
            "Defensive Positioning: Strong (Day 18)",
            "Retreat Planning: Necessary (Day 12)",
            "Communication: Hand signals + radios",
            "Medical Response: Rapid (Marcus on standby)",
            "Extraction Routes: Pre-planned for all expeditions",
            "Lessons Learned: Improved perimeter defense"
        };

        public void Bind(object combatHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_combatHistory == null || _battleOutcomes == null || _tacticalAnalysis == null) return;

            while (_combatHistory.GetChildCount() > 0) _combatHistory.RemoveChild(_combatHistory.GetChild(0));
            while (_battleOutcomes.GetChildCount() > 0) _battleOutcomes.RemoveChild(_battleOutcomes.GetChild(0));
            while (_tacticalAnalysis.GetChildCount() > 0) _tacticalAnalysis.RemoveChild(_tacticalAnalysis.GetChild(0));

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _combatHistory.AddChild(label);
            }

            foreach (string outcome in _placeholderOutcomes)
            {
                var label = new Label { Text = outcome };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _battleOutcomes.AddChild(label);
            }

            foreach (string tactic in _placeholderTactics)
            {
                var label = new Label { Text = tactic };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _tacticalAnalysis.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("COMBAT HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("BATTLE LOG");
            vbox.AddChild(_lblHistoryTitle);

            _combatHistory = new VBoxContainer();
            _combatHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _combatHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_combatHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("BATTLE OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _battleOutcomes = new VBoxContainer();
            _battleOutcomes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _battleOutcomes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_battleOutcomes);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTacticsTitle = AshfallUiHelpers.MakeSectionHeader("TACTICAL ANALYSIS");
            vbox.AddChild(_lblTacticsTitle);

            _tacticalAnalysis = new VBoxContainer();
            _tacticalAnalysis.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tacticalAnalysis.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tacticalAnalysis);

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
