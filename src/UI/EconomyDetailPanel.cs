// SPDX-License-Identifier: MIT
using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Economy Detail panel.
    /// Shows market resources, trade ledger, market state, and debt — bound
    /// to the live EconomyHostSession. Unbound renders an honest empty state.
    ///
    /// Plan 14A/14B presentation wave (Wave 8 B1): the embargo banner and the
    /// regional heat map render the Core read models ONLY — the panel never
    /// recomputes multipliers, price factors, or weather gating. All numbers
    /// arrive through <see cref="EconomyHostSession"/>.
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

        // Plan 14A/14B (B1) — embargo banner + regional heat map.
        private VBoxContainer _embargoBanner = null!;
        private VBoxContainer _heatMapContainer = null!;
        private VBoxContainer _heatMapDetail = null!;
        private string _selectedRegion = string.Empty;
        private string _selectedHeatItemId = string.Empty;

        private EconomyHostSession? _economy;
        private Func<WeatherKind>? _weatherProvider;

        public bool IsBound => _economy != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(EconomyHostSession? economy, Func<WeatherKind>? weatherProvider = null)
        {
            if (_economy != null)
                _economy.StateChanged -= RefreshView;
            _economy = economy;
            _weatherProvider = weatherProvider;
            if (_economy != null)
            {
                // Refresh discipline (plan §14B.8): economy transition events
                // only — market mutations, shocks, and the host's weather
                // bridge raise StateChanged; never per-frame polling.
                _economy.StateChanged += RefreshView;
            }
            RefreshView();
        }

        /// <summary>Lifecycle: drop subscriptions when the panel unbinds.</summary>
        public void Unbind()
        {
            if (_economy != null)
                _economy.StateChanged -= RefreshView;
            _economy = null;
            _weatherProvider = null;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_resourcesList == null || _tradeList == null || _marketList == null || _debtList == null || _embargoBanner == null || _heatMapContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_resourcesList);
            AshfallUiHelpers.EmptyChildren(_tradeList);
            AshfallUiHelpers.EmptyChildren(_marketList);
            AshfallUiHelpers.EmptyChildren(_debtList);
            AshfallUiHelpers.EmptyChildren(_embargoBanner);
            AshfallUiHelpers.EmptyChildren(_heatMapContainer);

            RenderedRowCount = 0;

            if (_economy == null)
            {
                _embargoBanner.AddChild(MakeDimLine("No economy session bound."));
                _resourcesList.AddChild(MakeDimLine("No economy session bound."));
                return;
            }

            RenderEmbargoBanner();
            RenderRegionalHeatMap();

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
                    var good = _economy.Catalog?.Find(d.itemId);
                    var quote = _economy.ExplainPrice(d.itemId);
                    string name = good?.displayName ?? d.itemId;
                    string why = FormatTopFactors(quote);
                    AddRow(_marketList,
                        $"{name} — {quote.finalPrice:0.0} · demand ×{d.multiplier:0.00}{why}",
                        Ashfall.Core.UI.Theme.Pale);
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

        /// <summary>
        /// Plan 14A (B1) — the embargo banner from
        /// <see cref="TradeEmbargoSystem.GetEmbargoSummary"/>: active weather,
        /// affected regions, active rule count, and the neutral state when
        /// clear. Read-only rendering — no embargo math in the panel.
        /// </summary>
        private void RenderEmbargoBanner()
        {
            if (_economy?.EmbargoSystem == null)
            {
                _embargoBanner.AddChild(MakeDimLine("EMBARGO RULES UNAVAILABLE — trade_embargoes.json not bound."));
                return;
            }

            WeatherKind weather = _weatherProvider?.Invoke() ?? WeatherKind.Clear;
            var summary = _economy.EmbargoSystem.GetEmbargoSummary(weather);
            RenderedRowCount++;

            if (!summary.embargoActive)
            {
                AddRow(_embargoBanner,
                    $"EMBARGO STATUS: CLEAR — no active rules for {weather}.",
                    Ashfall.Core.UI.Theme.Lethe);
                return;
            }

            AddRow(_embargoBanner,
                $"EMBARGO ACTIVE — {summary.weatherKind} · {summary.activeRuleCount} rule(s)" +
                (summary.allGoodsAffected ? " · ALL GOODS AFFECTED" : string.Empty),
                Ashfall.Core.UI.Theme.Hot);
            RenderedRowCount++;

            string regions = summary.affectedRegions.Count > 0
                ? string.Join(", ", summary.affectedRegions)
                : "none";
            AddRow(_embargoBanner, $"  Affected regions: {regions}", Ashfall.Core.UI.Theme.Warm);
            RenderedRowCount++;

            if (summary.affectedCategories.Count > 0)
            {
                AddRow(_embargoBanner,
                    "  Affected categories: " + string.Join(", ", summary.affectedCategories),
                    Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }

            if (summary.anyRouteBlocked)
            {
                AddRow(_embargoBanner,
                    "  Caravan routes: BLOCKED or SLOWED (see caravans panel).",
                    Ashfall.Core.UI.Theme.Critical);
                RenderedRowCount++;
            }
        }

        /// <summary>
        /// Plan 14B (B1) — the regional heat map: every authored
        /// region/good entry with its scarcity label as TEXT (CHEAP /
        /// NEUTRAL / EXPENSIVE — never color-only), a keyboard-focusable cell,
        /// and a decomposition drawn ONLY from the typed ExplainPrice factor
        /// rows evaluated at that region.
        /// </summary>
        private void RenderRegionalHeatMap()
        {
            if (_economy?.RegionalAtlas == null)
            {
                _heatMapContainer.AddChild(MakeDimLine("REGIONAL ATLAS UNAVAILABLE — regional_prices.json not bound."));
                return;
            }

            var entries = _economy.RegionalAtlas.Catalog.All()
                .OrderBy(e => e.Region, StringComparer.Ordinal)
                .ThenBy(e => e.ItemId, StringComparer.Ordinal)
                .ThenBy(e => e.Category, StringComparer.Ordinal)
                .ToList();
            if (entries.Count == 0)
            {
                _heatMapContainer.AddChild(MakeDimLine("No authored regional price entries."));
                return;
            }

            string currentRegion = string.Empty;
            HBoxContainer? row = null;
            foreach (var entry in entries)
            {
                if (!string.Equals(entry.Region, currentRegion, StringComparison.Ordinal))
                {
                    currentRegion = entry.Region;
                    AddRow(_heatMapContainer, "REGION: " + currentRegion.ToUpperInvariant(),
                        Ashfall.Core.UI.Theme.Pale);
                    row = new HBoxContainer();
                    row.AddThemeConstantOverride("separation", 4);
                    _heatMapContainer.AddChild(row);
                }

                string goodsId = entry.ItemId;
                string displayName = string.IsNullOrEmpty(goodsId)
                    ? entry.Category
                    : (_economy.Catalog?.Find(goodsId)?.displayName ?? goodsId);
                string band = ScarcityBandLabel(entry.ScarcityProfile);
                var cell = new Button
                {
                    Text = $"{displayName} · {band} (x{entry.BasePriceModifierPermille / 1000f:0.00})",
                    TooltipText = "Show the price decomposition for this region."
                };
                string region = entry.Region;
                cell.Pressed += () =>
                {
                    _selectedRegion = region;
                    _selectedHeatItemId = goodsId;
                    RefreshView();
                };
                row!.AddChild(cell);
                RenderedRowCount++;
            }

            _heatMapDetail = new VBoxContainer();
            _heatMapDetail.AddThemeConstantOverride("separation", 2);
            _heatMapContainer.AddChild(_heatMapDetail);
            RenderHeatMapDetail();
        }

        private void RenderHeatMapDetail()
        {
            _heatMapDetail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("REGIONAL DECOMPOSITION"));
            if (string.IsNullOrEmpty(_selectedRegion) || _economy == null)
            {
                _heatMapDetail.AddChild(MakeDimLine("Select a region cell to inspect its typed price factors."));
                return;
            }

            var quote = _economy.ExplainPrice(
                _selectedHeatItemId, MarketTransactionSide.Buy, _selectedRegion);
            AddRow(_heatMapDetail,
                $"{_selectedHeatItemId} @ {_selectedRegion} — final {quote.finalPrice:0.00} (base {quote.basePrice:0.00})",
                Ashfall.Core.UI.Theme.Warm);

            if (quote.factors == null || quote.factors.Count == 0)
            {
                _heatMapDetail.AddChild(MakeDimLine("  No active price factors."));
                return;
            }
            foreach (var factor in quote.factors
                .OrderByDescending(f => Math.Abs(f.delta))
                .ThenBy(f => (int)f.kind)
                .ThenBy(f => f.sourceId, StringComparer.Ordinal))
            {
                AddRow(_heatMapDetail, $"  {FormatFactor(factor)}", Ashfall.Core.UI.Theme.Dim);
            }
        }

        private static string ScarcityBandLabel(string scarcityProfile) => scarcityProfile switch
        {
            "local_surplus" => "CHEAP",
            "imported_scarce" => "EXPENSIVE",
            "balanced" => "NEUTRAL",
            _ => "NEUTRAL"
        };

        private static string FormatTopFactors(PriceExplanation explanation)
        {
            if (explanation.factors == null || explanation.factors.Count == 0)
                return " · price unavailable";

            var factors = explanation.factors
                .OrderByDescending(f => Math.Abs(f.delta))
                .ThenBy(f => (int)f.kind)
                .ThenBy(f => f.sourceId, StringComparer.Ordinal)
                .Take(2)
                .Select(FormatFactor)
                .ToArray();
            return factors.Length == 0 ? string.Empty : " · why: " + string.Join(", ", factors);
        }

        private static string FormatFactor(PriceFactorRecord factor)
        {
            string label = factor.kind switch
            {
                PriceFactorKind.Demand => "market demand",
                PriceFactorKind.FloorClamp => "price floor",
                PriceFactorKind.CeilingClamp => "price ceiling",
                PriceFactorKind.RegionalSupply => "regional supply",
                _ => "market pressure"
            };
            string direction = factor.delta >= 0f ? "+" : string.Empty;
            return $"{label} {direction}{factor.delta:0.0}";
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
            // Ticket #125: layout chrome owned by res://assets/ui/panels/EconomyDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
            // Sibling refresh code is unchanged.
            var binder = new SceneBinder(this, typeof(EconomyDetailPanel));
            binder.Require<VBoxContainer>("ResourcesList");
            binder.Require<VBoxContainer>("TradeList");
            binder.Require<VBoxContainer>("MarketList");
            binder.Require<VBoxContainer>("DebtList");
            binder.Require<Button>("CloseButton");
            _resourcesList = binder.Get<VBoxContainer>("ResourcesList");
            _tradeList = binder.Get<VBoxContainer>("TradeList");
            _marketList = binder.Get<VBoxContainer>("MarketList");
            _debtList = binder.Get<VBoxContainer>("DebtList");
            _contentVBox = binder.Get<VBoxContainer>("Content");

            // Plan 14A/14B (B1) — embargo banner + regional heat map sections,
            // inserted into the existing content column (scene-owned chrome
            // untouched; all content rendered per refresh in code).
            _embargoBanner = BuildInsertedSection("Sep1", "COMMODITY EMBARGOES");
            _heatMapContainer = BuildInsertedSection("DebtHeader", "REGIONAL PRICE HEAT MAP");
            _heatMapDetail = new VBoxContainer();

            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        /// <summary>Plan 14A/14B (B1) — create a titled section container and
        /// insert it before the named scene node (keeps the authored ordering:
        /// embargo above the ledgers, heat map above the debt block).</summary>
        private VBoxContainer BuildInsertedSection(string beforeNodeName, string headerText)
        {
            var section = new VBoxContainer();
            section.AddThemeConstantOverride("separation", 2);
            section.AddChild(AshfallUiHelpers.MakeSeparator());
            section.AddChild(AshfallUiHelpers.MakeSectionHeader(headerText));
            var list = new VBoxContainer();
            list.AddThemeConstantOverride("separation", 2);
            section.AddChild(list);
            _contentVBox.AddChild(section);
            var before = _contentVBox.GetNodeOrNull(beforeNodeName);
            if (before != null)
                _contentVBox.MoveChild(section, before.GetIndex());
            return list;
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
