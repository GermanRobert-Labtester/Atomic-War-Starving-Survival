using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Subterranean Cartography & 3D Cavity GIS (MAP-02).
    /// 3D LiDAR point cloud voxel mapping, underground cavern network GIS, and radiation isobars.
    /// </summary>
    public partial class SubterraneanCartographyPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _voxelLabel = null!;
        private Label _cavityLabel = null!;
        private Label _voidLabel = null!;
        private Label _isobarLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _scanButton = null!;
        private Button _isobarButton = null!;
        private Button _waypointButton = null!;
        private Button _hazardButton = null!;
        private Button _closeButton = null!;

        public bool IsBound { get; private set; } = true;
        public int SimDay { get; set; } = 1;

        public override void _Ready()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var bg = new ColorRect { Color = AshfallUiHelpers.ToColor(DesignTheme.Ink) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            root.AddThemeConstantOverride("separation", 10);
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER EXPLORATION // 3D CAVITY GIS CARTOGRAPHY (MAP-02)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: MAPPING - SECTOR 04 POINT CLOUD / 42.8 KM TUNNEL SURVEYED]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _voxelLabel = AshfallUiHelpers.MakeBody("Point Cloud Density: 4.8M Voxels Processed");
            grid.AddChild(_voxelLabel);

            _cavityLabel = AshfallUiHelpers.MakeBody("Surveyed Cavities: 18 Stable Chambers");
            grid.AddChild(_cavityLabel);

            _voidLabel = AshfallUiHelpers.MakeBody("Unmapped Voids: 6 Anomalous Pockets");
            grid.AddChild(_voidLabel);

            _isobarLabel = AshfallUiHelpers.MakeBody("Radiation Isobar Gradient: 0.45 R/h Boundary Line");
            grid.AddChild(_isobarLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _scanButton = new Button { Text = "[EXECUTE 3D LIDAR SCAN]" };
            _scanButton.Pressed += () => ShowFeedback("LiDAR sweep completed. 240,000 new point cloud voxels integrated.");
            consoleBox.AddChild(_scanButton);

            _isobarButton = new Button { Text = "[PROJECT RAD ISOBAR OVERLAY]" };
            _isobarButton.Pressed += () => ShowFeedback("Radiation isobar layer rendered across depth slices -40m to -250m.");
            consoleBox.AddChild(_isobarButton);

            _waypointButton = new Button { Text = "[PLOT EXTRACTION WAYPOINT]" };
            _waypointButton.Pressed += () => ShowFeedback("Extraction Waypoint Foxtrot synchronized with surface team.");
            consoleBox.AddChild(_waypointButton);

            _hazardButton = new Button { Text = "[FLAG COLLAPSE HAZARD]" };
            _hazardButton.Pressed += () => ShowFeedback("Sector 09 marked with structural collapse hazard warning.");
            consoleBox.AddChild(_hazardButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("GIS Subterranean Grid synchronized with expedition navigation beacon.");
            _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            root.AddChild(_feedbackLabel);
        }

        private void ShowFeedback(string msg)
        {
            if (_feedbackLabel != null)
            {
                _feedbackLabel.Text = msg;
                _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            }
        }

        public void Open()
        {
            Visible = true;
        }

        public void RefreshView() { }
        public void Unbind() { }
    }
}
