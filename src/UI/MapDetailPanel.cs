using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Map Detail panel.
    /// Shows detailed world map with exploration progress, discovered locations, and navigation.
    /// </summary>
    public partial class MapDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblMapInfoTitle;
        private VBoxContainer _mapInfo;
        private Label _lblLocationsTitle;
        private VBoxContainer _discoveredLocations;
        private Label _lblRoutesTitle;
        private VBoxContainer _discoveredRoutes;
        private Label _lblExploredTitle;
        private VBoxContainer _explorationProgress;

        private readonly string[] _placeholderMapInfo = {
            "Current Location: Bunker (Sector 7)",
            "Total Area Explored: 35%",
            "Known Locations: 12/45",
            "Discovered Routes: 4/15",
            "Danger Zones: 3 active",
            "Last Updated: Day 25"
        };

        private readonly string[] _placeholderLocations = {
            "Bunker (Sector 7) — Home base, fully mapped",
            "Ruined City (Sector 12) — Scavenged, partially mapped",
            "Radio Tower (Sector 4) — Explored, hostile",
            "Supply Depot (Sector 9) — Secured, stocked",
            "Medical Center (Sector 15) — Explored, abandoned",
            "Military Outpost (Sector 1) — Hostile, unexplored",
            "Water Source (Sector 8) — Discovered, unexplored",
            "Trading Post (Sector 11) — Discovered, neutral"
        };

        private readonly string[] _placeholderRoutes = {
            "Bunker → Sector 7 (Home) — Safe, mapped",
            "Bunker → Sector 12 (Ruined City) — Active route",
            "Bunker → Sector 4 (Radio Tower) — Dangerous",
            "Bunker → Sector 9 (Supply Depot) — Secured",
            "Sector 7 → Sector 8 (Water Source) — New route",
            "Sector 7 → Sector 11 (Trading Post) — Unexplored"
        };

        private readonly string[] _placeholderExploration = {
            "Sector 7 — 100% explored",
            "Sector 12 — 75% explored",
            "Sector 4 — 60% explored",
            "Sector 9 — 50% explored",
            "Sector 15 — 40% explored",
            "Sector 1 — 10% explored (dangerous)",
            "Sector 8 — 20% explored (new)",
            "Sector 11 — 5% explored (unexplored)"
        };

        public void Bind(object mapDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_mapInfo == null || _discoveredLocations == null || _discoveredRoutes == null || _explorationProgress == null) return;

            while (_mapInfo.GetChildCount() > 0) _mapInfo.RemoveChild(_mapInfo.GetChild(0));
            while (_discoveredLocations.GetChildCount() > 0) _discoveredLocations.RemoveChild(_discoveredLocations.GetChild(0));
            while (_discoveredRoutes.GetChildCount() > 0) _discoveredRoutes.RemoveChild(_discoveredRoutes.GetChild(0));
            while (_explorationProgress.GetChildCount() > 0) _explorationProgress.RemoveChild(_explorationProgress.GetChild(0));

            foreach (string info in _placeholderMapInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _mapInfo.AddChild(label);
            }

            foreach (string location in _placeholderLocations)
            {
                var label = new Label { Text = location };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _discoveredLocations.AddChild(label);
            }

            foreach (string route in _placeholderRoutes)
            {
                var label = new Label { Text = route };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _discoveredRoutes.AddChild(label);
            }

            foreach (string explored in _placeholderExploration)
            {
                var label = new Label { Text = explored };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _explorationProgress.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("MAP DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMapInfoTitle = AshfallUiHelpers.MakeSectionHeader("MAP INFORMATION");
            vbox.AddChild(_lblMapInfoTitle);

            _mapInfo = new VBoxContainer();
            _mapInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _mapInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_mapInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblLocationsTitle = AshfallUiHelpers.MakeSectionHeader("DISCOVERED LOCATIONS");
            vbox.AddChild(_lblLocationsTitle);

            _discoveredLocations = new VBoxContainer();
            _discoveredLocations.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _discoveredLocations.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_discoveredLocations);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRoutesTitle = AshfallUiHelpers.MakeSectionHeader("DISCOVERED ROUTES");
            vbox.AddChild(_lblRoutesTitle);

            _discoveredRoutes = new VBoxContainer();
            _discoveredRoutes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _discoveredRoutes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_discoveredRoutes);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblExploredTitle = AshfallUiHelpers.MakeSectionHeader("EXPLORATION PROGRESS");
            vbox.AddChild(_lblExploredTitle);

            _explorationProgress = new VBoxContainer();
            _explorationProgress.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _explorationProgress.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_explorationProgress);

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
