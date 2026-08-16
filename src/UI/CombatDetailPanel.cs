using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Combat Detail panel.
    /// Shows detailed combat information, battle tactics, casualties, and combat outcomes.
    /// </summary>
    public partial class CombatDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblBattleTitle;
        private VBoxContainer _battleInfo;
        private Label _lblTacticsTitle;
        private VBoxContainer _tacticsData;
        private Label _lblCasualtiesTitle;
        private VBoxContainer _casualtyData;
        private Label _lblOutcomesTitle;
        private VBoxContainer _outcomesData;

        // Placeholder combat detail data
        private readonly string[] _placeholderBattle = {
            "Battle: Raid on Supply Caravan (Day 7)",
            "Location: Sector 12, Highway 7",
            "Duration: 2 hours",
            "Participants: 8 survivors vs 12 raiders",
            "Outcome: Victory (our side)",
            "Strategic Value: +15 rations captured"
        };

        private readonly string[] _placeholderTactics = {
            "Ambush Position: Forest edge (north side)",
            "Fire Support: 2 survivors with rifles",
            "Flanking Route: 3 survivors (west approach)",
            "Retreat Plan: Main road to bunker (5 min)",
            "Communication: Hand signals + radios",
            "Morale Boost: Leadership presence (Elena)"
        };

        private readonly string[] _placeholderCasualties = {
            "Our Side: 2 scouts killed (perimeter patrol)",
            "Our Side: 1 medic wounded (leg injury)",
            "Enemy Side: 4 raiders killed",
            "Enemy Side: 3 raiders captured",
            "Enemy Side: 5 raiders retreated",
            "Prisoners: 3 (interrogation pending)"
        };

        private readonly string[] _placeholderOutcomes = {
            "Resources Gained: +15 rations, +2 medkits",
            "Intelligence: Raider base location identified",
            "Morale Impact: +5 community morale",
            "Reputation: +10 with Black Flotilla",
            "Learning: Improved ambush tactics",
            "Next Steps: Interrogate prisoners, reinforce perimeter"
        };

        // Real data from host session
        // private CombatHostSession? _combatHost;

        public void Bind(object combat) // placeholder for CombatHostSession
        {
            // _combatHost = (CombatHostSession)combat;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_battleInfo == null || _tacticsData == null || _casualtyData == null || _outcomesData == null) return;

            // Clear existing lists
            while (_battleInfo.GetChildCount() > 0)
                _battleInfo.RemoveChild(_battleInfo.GetChild(0));
            while (_tacticsData.GetChildCount() > 0)
                _tacticsData.RemoveChild(_tacticsData.GetChild(0));
            while (_casualtyData.GetChildCount() > 0)
                _casualtyData.RemoveChild(_casualtyData.GetChild(0));
            while (_outcomesData.GetChildCount() > 0)
                _outcomesData.RemoveChild(_outcomesData.GetChild(0));

            // Display placeholder battle info
            foreach (string data in _placeholderBattle)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _battleInfo.AddChild(label);
            }

            // Display placeholder tactics
            foreach (string data in _placeholderTactics)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _tacticsData.AddChild(label);
            }

            // Display placeholder casualties
            foreach (string data in _placeholderCasualties)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _casualtyData.AddChild(label);
            }

            // Display placeholder outcomes
            foreach (string data in _placeholderOutcomes)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _outcomesData.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("COMBAT DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Battle info section
            _lblBattleTitle = AshfallUiHelpers.MakeSectionHeader("BATTLE INFORMATION");
            vbox.AddChild(_lblBattleTitle);

            _battleInfo = new VBoxContainer();
            _battleInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _battleInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_battleInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Tactics section
            _lblTacticsTitle = AshfallUiHelpers.MakeSectionHeader("BATTLE TACTICS");
            vbox.AddChild(_lblTacticsTitle);

            _tacticsData = new VBoxContainer();
            _tacticsData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tacticsData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tacticsData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Casualties section
            _lblCasualtiesTitle = AshfallUiHelpers.MakeSectionHeader("CASUALTIES & LOSSES");
            vbox.AddChild(_lblCasualtiesTitle);

            _casualtyData = new VBoxContainer();
            _casualtyData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _casualtyData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_casualtyData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Outcomes section
            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("BATTLE OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _outcomesData = new VBoxContainer();
            _outcomesData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _outcomesData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_outcomesData);

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
