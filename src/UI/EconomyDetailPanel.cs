using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Economy Detail panel.
    /// Shows market resources, trade ledger, market state, and debt — bound
    /// to the live EconomyHostSession. Unbound renders an honest empty state.
    /// </summary>
    public partial class EconomyDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblResourcesTitle;
        private VBoxContainer _resourcesList;
        private Label _lblTradeTitle;
        private VBoxContainer _tradeList;
        private Label _lblMarketTitle;
        private VBoxContainer _marketList;
        private Label _lblDebtTitle;
        private VBoxContainer _debtList;

        private EconomyHostSession? _economy;

        public bool IsBound => _economy != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(EconomyHostSession? economy)
        {
            _economy = economy;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_resourcesList == null || _tradeList == null || _marketList == null || _debtList == null) return;

            AshfallUiHelpers.EmptyChildren(_resourcesList);
            AshfallUiHelpers.EmptyChildren(_tradeList);
            AshfallUiHelpers.EmptyChildren(_marketList);
            AshfallUiHelpers.EmptyChildren(_debtList);

            RenderedRowCount = 0;

            if (_economy == null)
            {
                _resourcesList.AddChild(MakeDimLine("No economy session bound."));
                return;
            }

            // ── Resources: catalog goods count ──
            if (_economy.Catalog != null)
            {
                int goods = _economy.Catalog.Count;
                AddRow(_resourcesList, $"Catalog goods: {goods}", Ashfall.Core.UI.Theme.Pale);
                RenderedRowCount++;
            }

            // ── Trade ledger: recent entries ──
            if (_economy.Market != null)
            {
                var state = _economy.Market.State;
                AddRow(_tradeList, $"Market day: {state.day} · tick {state.tickCount}", Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;

                int shown = 0;
                foreach (var entry in state.ledger.OrderByDescending(l => l.day).Take(10))
                {
                    AddRow(_tradeList, $"[Day {entry.day}] {entry.quantity}× {entry.itemId} @ {entry.unitPrice:0.0} → {entry.counterparty}",
                        Ashfall.Core.UI.Theme.Warm);
                    shown++;
                    RenderedRowCount++;
                }
                if (shown == 0)
                    _tradeList.AddChild(MakeDimLine("No trade ledger entries."));

                // ── Market demand ──
                int demandShown = 0;
                foreach (var d in state.demand.Take(10))
                {
                    AddRow(_marketList, $"{d.itemId} — demand ×{d.multiplier:0.00}", Ashfall.Core.UI.Theme.Pale);
                    demandShown++;
                    RenderedRowCount++;
                }
                if (demandShown == 0)
                    _marketList.AddChild(MakeDimLine("No active demand modifiers."));
            }
            else
            {
                _tradeList.AddChild(MakeDimLine("No market system bound."));
            }

            // ── Debt: not modeled in Core MarketSystem ──
            _debtList.AddChild(MakeDimLine("Debt tracking not modeled in the market system."));
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
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

            _lblResourcesTitle = AshfallUiHelpers.MakeSectionHeader("CATALOG RESOURCES");
            vbox.AddChild(_lblResourcesTitle);
            _resourcesList = new VBoxContainer();
            _resourcesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _resourcesList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_resourcesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTradeTitle = AshfallUiHelpers.MakeSectionHeader("TRADE LEDGER");
            vbox.AddChild(_lblTradeTitle);
            _tradeList = new VBoxContainer();
            _tradeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tradeList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_tradeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMarketTitle = AshfallUiHelpers.MakeSectionHeader("MARKET DEMAND");
            vbox.AddChild(_lblMarketTitle);
            _marketList = new VBoxContainer();
            _marketList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _marketList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_marketList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblDebtTitle = AshfallUiHelpers.MakeSectionHeader("DEBT & CREDIT");
            vbox.AddChild(_lblDebtTitle);
            _debtList = new VBoxContainer();
            _debtList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _debtList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_debtList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
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
