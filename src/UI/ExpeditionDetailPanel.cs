using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Expedition Detail panel.
    /// Shows detailed expedition information, team composition, route planning, and expedition outcomes.
    /// </summary>
    public partial class ExpeditionDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblExpeditionInfoTitle;
        private VBoxContainer _expeditionInfo;
        private Label _lblTeamTitle;
        private VBoxContainer _teamComposition;
        private Label _lblRouteTitle;
        private VBoxContainer _routePlanning;
        private Label _lblOutcomesTitle;
        private VBoxContainer _expeditionOutcomes;

        private readonly string[] _placeholderExpeditionInfo = {
            "Expedition: Scavenge Ruined City",
            "Status: Active",
            "Duration: 3 days",
            "Departure: Day 12, 06:00",
            "Expected Return: Day 15, 18:00",
            "Risk Level: Medium"
        };

        private readonly string[] _placeholderTeam = {
            "Yuki (Scout) — Leader, navigation",
            "David (Engineer) — Technical support",
            "Marcus (Medic) — Medical support",
            "Sofia (Trader) — Resource assessment",
            "2 Volunteers — General support"
        };

        private readonly string[] _placeholderRoute = {
            "Route: Bunker → Sector 12 (Ruined City)",
            "Distance: 12 km",
            "Terrain: Wasteland, Urban ruins",
            "Hazards: Fallout zones, Raider camps",
            "Estimated Travel Time: 6 hours each way",
            "Rest Stops: 2 planned (Sector 8, Sector 10)"
        };

        private readonly string[] _placeholderOutcomes = {
            "Resources Found: +15 rations, +3 medicine",
            "Intel Gathered: Raider base location mapped",
            "Casualties: 0 (Safe return)",
            "Experience Gained: +5 scouting, +3 engineering",
            "Morale Impact: +3 community morale",
            "Next Expedition: Planned for Day 20"
        };

        public void Bind(object expedition)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_expeditionInfo == null || _teamComposition == null || _routePlanning == null || _expeditionOutcomes == null) return;

            AshfallUiHelpers.EmptyChildren(_expeditionInfo);
            AshfallUiHelpers.EmptyChildren(_teamComposition);
            AshfallUiHelpers.EmptyChildren(_routePlanning);
            AshfallUiHelpers.EmptyChildren(_expeditionOutcomes);

            foreach (string info in _placeholderExpeditionInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _expeditionInfo.AddChild(label);
            }

            foreach (string team in _placeholderTeam)
            {
                var label = new Label { Text = team };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _teamComposition.AddChild(label);
            }

            foreach (string route in _placeholderRoute)
            {
                var label = new Label { Text = route };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _routePlanning.AddChild(label);
            }

            foreach (string outcome in _placeholderOutcomes)
            {
                var label = new Label { Text = outcome };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _expeditionOutcomes.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("EXPEDITION DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblExpeditionInfoTitle = AshfallUiHelpers.MakeSectionHeader("EXPEDITION INFORMATION");
            vbox.AddChild(_lblExpeditionInfoTitle);

            _expeditionInfo = new VBoxContainer();
            _expeditionInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _expeditionInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_expeditionInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTeamTitle = AshfallUiHelpers.MakeSectionHeader("TEAM COMPOSITION");
            vbox.AddChild(_lblTeamTitle);

            _teamComposition = new VBoxContainer();
            _teamComposition.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _teamComposition.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_teamComposition);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRouteTitle = AshfallUiHelpers.MakeSectionHeader("ROUTE PLANNING");
            vbox.AddChild(_lblRouteTitle);

            _routePlanning = new VBoxContainer();
            _routePlanning.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _routePlanning.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_routePlanning);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("EXPEDITION OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _expeditionOutcomes = new VBoxContainer();
            _expeditionOutcomes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _expeditionOutcomes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_expeditionOutcomes);

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
