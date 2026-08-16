using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Expedition panel.
    /// Shows expedition details, routes, outcomes, and expedition history.
    /// </summary>
    public partial class ExpeditionPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblExpeditionTitle;
        private VBoxContainer _expeditionDetails;
        private Label _lblRouteTitle;
        private VBoxContainer _routeInfo;
        private Label _lblHistoryTitle;
        private VBoxContainer _historyList;

        // Placeholder expedition data
        private readonly string[] _placeholderExpeditionDetails = {
            "Expedition: Scavenge Ruined City",
            "Status: Active",
            "Duration: 3 days",
            "Team: 4 survivors",
            "Expected return: Day 15"
        };

        private readonly string[] _placeholderRouteInfo = {
            "Route: Bunker → Ruined City",
            "Distance: 12 km",
            "Terrain: Wasteland, Urban ruins",
            "Hazards: Fallout zones, Raiders",
            "Estimated travel time: 6 hours"
        };

        private readonly string[] _placeholderHistory = {
            "[Day 8] Expedition completed: Found 15 food, 3 medicine",
            "[Day 3] Expedition departed: Scavenge Ruined City",
            "[Day 1] Expedition planned: Scavenge Ruined City"
        };

        // Real data from host session
        // private ExpeditionHostSession? _expeditionHost;

        public void Bind(object expedition) // placeholder for ExpeditionHostSession
        {
            // _expeditionHost = (ExpeditionHostSession)expedition;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_expeditionDetails == null || _routeInfo == null || _historyList == null) return;

            // Clear existing lists
            while (_expeditionDetails.GetChildCount() > 0)
                _expeditionDetails.RemoveChild(_expeditionDetails.GetChild(0));
            while (_routeInfo.GetChildCount() > 0)
                _routeInfo.RemoveChild(_routeInfo.GetChild(0));
            while (_historyList.GetChildCount() > 0)
                _historyList.RemoveChild(_historyList.GetChild(0));

            // Display placeholder expedition details
            foreach (string detail in _placeholderExpeditionDetails)
            {
                var label = new Label { Text = detail };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _expeditionDetails.AddChild(label);
            }

            // Display placeholder route info
            foreach (string route in _placeholderRouteInfo)
            {
                var label = new Label { Text = route };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _routeInfo.AddChild(label);
            }

            // Display placeholder history
            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _historyList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("EXPEDITION STATUS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Expedition details section
            _lblExpeditionTitle = AshfallUiHelpers.MakeSectionHeader("EXPEDITION DETAILS");
            vbox.AddChild(_lblExpeditionTitle);

            _expeditionDetails = new VBoxContainer();
            _expeditionDetails.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _expeditionDetails.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_expeditionDetails);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Route info section
            _lblRouteTitle = AshfallUiHelpers.MakeSectionHeader("ROUTE INFORMATION");
            vbox.AddChild(_lblRouteTitle);

            _routeInfo = new VBoxContainer();
            _routeInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _routeInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_routeInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // History section
            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("EXPEDITION HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _historyList = new VBoxContainer();
            _historyList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _historyList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_historyList);

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
