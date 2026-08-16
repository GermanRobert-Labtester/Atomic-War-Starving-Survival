using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Trade Detail panel.
    /// Shows detailed trade information, trade history, market prices, and trade negotiations.
    /// </summary>
    public partial class TradeDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblTradeInfoTitle;
        private VBoxContainer _tradeInfo;
        private Label _lblHistoryTitle;
        private VBoxContainer _tradeHistory;
        private Label _lblMarketTitle;
        private VBoxContainer _marketPrices;
        private Label _lblNegotiationTitle;
        private VBoxContainer _negotiationData;

        private readonly string[] _placeholderTradeInfo = {
            "Current Trade: Food for Medicine",
            "Partner: Black Flotilla",
            "Status: Active Negotiation",
            "Offer: 5 food for 2 medicine",
            "Counter: 6 food for 3 medicine",
            "Deadline: Day 30"
        };

        private readonly string[] _placeholderHistory = {
            "[Day 24] Traded 5 food for 2 medicine with Black Flotilla",
            "[Day 22] Exchanged knowledge for iodine with Ledger Keepers",
            "[Day 20] Bartered materials for seeds with Green Thread",
            "[Day 18] Refused trade with Ashen Hand (hostile)",
            "[Day 15] Military supplies for protection from Iron Covenant"
        };

        private readonly string[] _placeholderMarket = {
            "Food Price: 3 units (stable)",
            "Medicine Price: 8 units (rising)",
            "Fuel Price: 5 units (volatile)",
            "Materials Price: 2 units (stable)",
            "Labor Value: 1 unit/hour",
            "Inflation Rate: +2% weekly"
        };

        private readonly string[] _placeholderNegotiation = {
            "Current Offer: 5 food for 2 medicine",
            "Partner Counter: 6 food for 3 medicine",
            "My Counter: 4 food for 2 medicine",
            "Best Possible: 5 food for 3 medicine",
            "Breakpoint: 7 food for 1 medicine",
            "Recommendation: Accept 5 food for 2 medicine"
        };

        public void Bind(object tradeDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_tradeInfo == null || _tradeHistory == null || _marketPrices == null || _negotiationData == null) return;

            while (_tradeInfo.GetChildCount() > 0) _tradeInfo.RemoveChild(_tradeInfo.GetChild(0));
            while (_tradeHistory.GetChildCount() > 0) _tradeHistory.RemoveChild(_tradeHistory.GetChild(0));
            while (_marketPrices.GetChildCount() > 0) _marketPrices.RemoveChild(_marketPrices.GetChild(0));
            while (_negotiationData.GetChildCount() > 0) _negotiationData.RemoveChild(_negotiationData.GetChild(0));

            foreach (string info in _placeholderTradeInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _tradeInfo.AddChild(label);
            }

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _tradeHistory.AddChild(label);
            }

            foreach (string price in _placeholderMarket)
            {
                var label = new Label { Text = price };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _marketPrices.AddChild(label);
            }

            foreach (string negotiation in _placeholderNegotiation)
            {
                var label = new Label { Text = negotiation };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _negotiationData.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("TRADE DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTradeInfoTitle = AshfallUiHelpers.MakeSectionHeader("TRADE INFORMATION");
            vbox.AddChild(_lblTradeInfoTitle);

            _tradeInfo = new VBoxContainer();
            _tradeInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tradeInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tradeInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("TRADE HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _tradeHistory = new VBoxContainer();
            _tradeHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tradeHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tradeHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMarketTitle = AshfallUiHelpers.MakeSectionHeader("MARKET PRICES");
            vbox.AddChild(_lblMarketTitle);

            _marketPrices = new VBoxContainer();
            _marketPrices.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _marketPrices.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_marketPrices);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNegotiationTitle = AshfallUiHelpers.MakeSectionHeader("TRADE NEGOTIATION");
            vbox.AddChild(_lblNegotiationTitle);

            _negotiationData = new VBoxContainer();
            _negotiationData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _negotiationData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_negotiationData);

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
