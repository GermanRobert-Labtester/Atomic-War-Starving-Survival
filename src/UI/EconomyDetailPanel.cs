using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Economy Detail panel.
    /// Shows detailed economic data, trade history, resource flows, and market analysis.
    /// </summary>
    public partial class EconomyDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblResourcesTitle;
        private VBoxContainer _resourceFlows;
        private Label _lblTradeTitle;
        private VBoxContainer _tradeHistory;
        private Label _lblMarketTitle;
        private VBoxContainer _marketData;
        private Label _lblDebtTitle;
        private VBoxContainer _debtData;

        // Placeholder economy detail data
        private readonly string[] _placeholderResources = {
            "Food Inflow: +15 units/day (scavenging + trade)",
            "Food Outflow: -12 units/day (consumption)",
            "Water Inflow: +8 units/day (filtration + rain)",
            "Water Outflow: -10 units/day (consumption + hygiene)",
            "Fuel Inflow: +2 units/day (scavenging)",
            "Fuel Outflow: -3 units/day (heating + generators)"
        };

        private readonly string[] _placeholderTrade = {
            "[Day 24] Black Flotilla — Traded 5 food for 2 medicine",
            "[Day 22] Ledger Keepers — Exchanged knowledge for iodine",
            "[Day 20] Green Thread — Bartered materials for seeds",
            "[Day 18] Ashen Hand — Refused trade (hostile stance)",
            "[Day 15] Iron Covenant — Military supplies for protection"
        };

        private readonly string[] _placeholderMarket = {
            "Food Price: 3 units (stable)",
            "Medicine Price: 8 units (rising - demand high)",
            "Fuel Price: 5 units (volatile - supply low)",
            "Materials Price: 2 units (stable)",
            "Labor Value: 1 unit/hour (baseline)",
            "Inflation Rate: +2% per week (mild)"
        };

        private readonly string[] _placeholderDebt = {
            "Total Debt: 15 units (to Black Flotilla)",
            "Interest Rate: 5% per month",
            "Next Payment: Day 30",
            "Collateral: 10 rations (seized if default)",
            "Credit Rating: Good (85/100)",
            "Loan Options: Available from Green Thread"
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
            if (_resourceFlows == null || _tradeHistory == null || _marketData == null || _debtData == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_resourceFlows);
            AshfallUiHelpers.EmptyChildren(_tradeHistory);
            AshfallUiHelpers.EmptyChildren(_marketData);
            AshfallUiHelpers.EmptyChildren(_debtData);

            // Display placeholder resource flows
            foreach (string data in _placeholderResources)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _resourceFlows.AddChild(label);
            }

            // Display placeholder trade history
            foreach (string trade in _placeholderTrade)
            {
                var label = new Label { Text = trade };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _tradeHistory.AddChild(label);
            }

            // Display placeholder market data
            foreach (string data in _placeholderMarket)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _marketData.AddChild(label);
            }

            // Display placeholder debt data
            foreach (string data in _placeholderDebt)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _debtData.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("ECONOMY DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Resource flows section
            _lblResourcesTitle = AshfallUiHelpers.MakeSectionHeader("RESOURCE FLOWS");
            vbox.AddChild(_lblResourcesTitle);

            _resourceFlows = new VBoxContainer();
            _resourceFlows.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _resourceFlows.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_resourceFlows);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Trade history section
            _lblTradeTitle = AshfallUiHelpers.MakeSectionHeader("TRADE HISTORY");
            vbox.AddChild(_lblTradeTitle);

            _tradeHistory = new VBoxContainer();
            _tradeHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tradeHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tradeHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Market data section
            _lblMarketTitle = AshfallUiHelpers.MakeSectionHeader("MARKET ANALYSIS");
            vbox.AddChild(_lblMarketTitle);

            _marketData = new VBoxContainer();
            _marketData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _marketData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_marketData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Debt data section
            _lblDebtTitle = AshfallUiHelpers.MakeSectionHeader("DEBT & CREDIT");
            vbox.AddChild(_lblDebtTitle);

            _debtData = new VBoxContainer();
            _debtData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _debtData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_debtData);

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
