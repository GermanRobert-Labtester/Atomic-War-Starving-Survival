using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Map panel.
    /// Shows world map, exploration progress, discovered locations, and navigation.
    /// </summary>
    public partial class MapPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblMapTitle;
        private VBoxContainer _mapInfo;
        private Label _lblLocationsTitle;
        private VBoxContainer _locationsList;
        private Label _lblExploredTitle;
        private VBoxContainer _exploredList;

        // Placeholder map data
        private readonly string[] _placeholderMapInfo = {
            "Current Location: Bunker (Sector 7)",
            "Total Area Explored: 35%",
            "Known Locations: 12/45",
            "Discovered Routes: 4/15",
            "Danger Zones: 3 active"
        };

        private readonly string[] _placeholderLocations = {
            "Bunker (Sector 7) — Home base, fully mapped",
            "Ruined City (Sector 12) — Scavenged, partially mapped",
            "Radio Tower (Sector 4) — Explored, hostile",
            "Supply Depot (Sector 9) — Secured, stocked",
            "Medical Center (Sector 15) — Explored, abandoned",
            "Military Outpost (Sector 1) — Hostile, unexplored"
        };

        private readonly string[] _placeholderExplored = {
            "Sector 7 — 100% explored",
            "Sector 12 — 75% explored",
            "Sector 4 — 60% explored",
            "Sector 9 — 50% explored",
            "Sector 15 — 40% explored",
            "Sector 1 — 10% explored (dangerous)"
        };

        // Real data from host session
        // private MapHostSession? _mapHost;

        public void Bind(object map) // placeholder for MapHostSession
        {
            // _mapHost = (MapHostSession)map;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_mapInfo == null || _locationsList == null || _exploredList == null) return;

            // Clear existing lists
            while (_mapInfo.GetChildCount() > 0)
                _mapInfo.RemoveChild(_mapInfo.GetChild(0));
            while (_locationsList.GetChildCount() > 0)
                _locationsList.RemoveChild(_locationsList.GetChild(0));
            while (_exploredList.GetChildCount() > 0)
                _exploredList.RemoveChild(_exploredList.GetChild(0));

            // Display placeholder map info
            foreach (string info in _placeholderMapInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _mapInfo.AddChild(label);
            }

            // Display placeholder locations
            foreach (string location in _placeholderLocations)
            {
                var label = new Label { Text = location };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _locationsList.AddChild(label);
            }

            // Display placeholder exploration progress
            foreach (string explored in _placeholderExplored)
            {
                var label = new Label { Text = explored };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _exploredList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("WORLD MAP & EXPLORATION", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Map info section
            _lblMapTitle = AshfallUiHelpers.MakeSectionHeader("MAP OVERVIEW");
            vbox.AddChild(_lblMapTitle);

            _mapInfo = new VBoxContainer();
            _mapInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _mapInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_mapInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Locations section
            _lblLocationsTitle = AshfallUiHelpers.MakeSectionHeader("KNOWN LOCATIONS");
            vbox.AddChild(_lblLocationsTitle);

            _locationsList = new VBoxContainer();
            _locationsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _locationsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_locationsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Explored section
            _lblExploredTitle = AshfallUiHelpers.MakeSectionHeader("EXPLORATION PROGRESS");
            vbox.AddChild(_lblExploredTitle);

            _exploredList = new VBoxContainer();
            _exploredList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _exploredList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_exploredList);

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
