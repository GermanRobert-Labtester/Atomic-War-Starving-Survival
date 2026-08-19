using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Economy overlay panel.
    /// Shows resources, trade status, supply chains, and economic indicators.
    /// </summary>
    public partial class EconomyOverlayPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblResourcesTitle;
        private VBoxContainer _resourceList;
        private Label _lblTradeTitle;
        private VBoxContainer _tradeLog;
        private Label _lblEconomyTitle;
        private VBoxContainer _economyStats;

        // Placeholder economy data
        private readonly string[] _placeholderResources = {
            "Food: 45 units (5 days)",
            "Water: 30 units (3 days)",
            "Fuel: 12 units (2 days)",
            "Medicine: 8 units",
            "Materials: 25 units"
        };

        private readonly string[] _placeholderTradeLog = {
            "[Day 12] Traded 5 food for 2 medicine",
            "[Day 11] Received supply drop: 10 water",
            "[Day 10] Caravan arrived: 8 materials",
            "[Day 9] Sold 3 fuel for 1 iodine"
        };

        private readonly string[] _placeholderEconomyStats = {
            "Daily consumption: -12 units",
            "Storage capacity: 60/100",
            "Trade routes: 2 active",
            "Economic status: Stable"
        };

        // Real data from host session
        // private EconomyHostSession? _economyHost;

        public void Bind(object economy) // placeholder for EconomyHostSession
        {
            // _economyHost = (EconomyHostSession)economy;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_resourceList == null || _tradeLog == null || _economyStats == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_resourceList);
            AshfallUiHelpers.EmptyChildren(_tradeLog);
            AshfallUiHelpers.EmptyChildren(_economyStats);

            // Display placeholder resources
            foreach (string resource in _placeholderResources)
            {
                var label = new Label { Text = resource };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _resourceList.AddChild(label);
            }

            // Display placeholder trade log
            foreach (string trade in _placeholderTradeLog)
            {
                var label = new Label { Text = trade };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _tradeLog.AddChild(label);
            }

            // Display placeholder economy stats
            foreach (string stat in _placeholderEconomyStats)
            {
                var label = new Label { Text = stat };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _economyStats.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("ECONOMY & RESOURCES", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Resources section
            _lblResourcesTitle = AshfallUiHelpers.MakeSectionHeader("RESOURCE STOCK");
            vbox.AddChild(_lblResourcesTitle);

            _resourceList = new VBoxContainer();
            _resourceList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _resourceList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_resourceList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Trade log section
            _lblTradeTitle = AshfallUiHelpers.MakeSectionHeader("RECENT TRADES");
            vbox.AddChild(_lblTradeTitle);

            _tradeLog = new VBoxContainer();
            _tradeLog.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tradeLog.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tradeLog);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Economy stats section
            _lblEconomyTitle = AshfallUiHelpers.MakeSectionHeader("ECONOMIC STATUS");
            vbox.AddChild(_lblEconomyTitle);

            _economyStats = new VBoxContainer();
            _economyStats.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _economyStats.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_economyStats);

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
