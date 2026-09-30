// SPDX-License-Identifier: MIT
using System;
#pragma warning disable CS8618
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp.Economy
{
    /// <summary>
    /// Thin Godot market panel: renders the goods catalog with item icons
    /// resolved through AssetRegistry (fallback texture when an icon is
    /// missing — AssetRegistry logs each missing id ONCE, so no spam), plus
    /// current price and demand. Presentation only; zero rules.
    /// </summary>
    public partial class EconomyMarketPanel : PanelContainer
    {
        private EconomyHostSession _session;
        private VBoxContainer _goodsList;
        private Label _lblSummary;
        private Label? _commodityTrends;
        private bool _fallbackObserved;

        // Optional guild-stance binding: when present, the summary strip shows
        // the Silent Foundry's real trade access (derived from the durable ledger).
        private Ashfall.Core.Economy.IFactionStanceProvider? _stanceProvider;
        private string _stanceFactionId = string.Empty;

        /// <summary>
        /// Plan 56 phase 4 — the region this market view is evaluated from.
        /// Drives the per-row provenance tag ("locally made" / "imported" /
        /// "general supply") via RegionalSupplyRouter. Defaults to the
        /// shelter market ("settlement").
        /// </summary>
        public string CurrentRegion { get; set; } = "settlement";

        /// <summary>
        /// Plan 215 overlay completion — the lawful player route for switching
        /// the shelter-wide rationing protocol. Returns a feedback line; the
        /// canonical ResourceRationingSystem validates and applies.
        /// </summary>
        public Func<string, string>? RationingProtocolCommand { get; set; }

        private Label _rationingSummary = null!;
        private OptionButton _rationingProtocolChoice = null!;
        private Button _rationingApplyButton = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(420, 300);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "THE MARKET — SUPPLY AND DEMAND",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", 13);
            rootVbox.AddChild(title);

            _lblSummary = new Label { Text = "..." };
            rootVbox.AddChild(_lblSummary);

            // Plan 212 — daily commodity ticker. Shows the canonical category
            // indices with arrow + word (never color-only). The economy
            // updates daily; the ticker reflects that fixed canonical state.
            _commodityTrends = new Label
            {
                Text = "COMMODITY TRENDS\n  —",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _commodityTrends.AddThemeFontSizeOverride("font_size", 11);
            rootVbox.AddChild(_commodityTrends);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 220)
            };
            rootVbox.AddChild(scroll);

            // Plan 215 — rationing overlay readout: the current policy, per-
            // resource tiers, and active crises from the canonical owner, plus
            // ONE explicit protocol command. A refresh never mutates policy.
            rootVbox.AddChild(MakeSection("RATIONING POLICY"));
            var protocolRow = new HBoxContainer();
            protocolRow.AddThemeConstantOverride("separation", 8);
            _rationingProtocolChoice = new OptionButton { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _rationingProtocolChoice.TooltipText = "Shelter-wide rationing protocol to apply.";
            protocolRow.AddChild(_rationingProtocolChoice);
            _rationingApplyButton = new Button { Text = "APPLY", CustomMinimumSize = new Vector2(0, Ashfall.Core.UI.Theme.MinInteractiveHeight) };
            _rationingApplyButton.TooltipText = "Apply the selected rationing protocol (validated by the canonical owner).";
            _rationingApplyButton.Pressed += OnRationingApplyPressed;
            protocolRow.AddChild(_rationingApplyButton);
            rootVbox.AddChild(protocolRow);
            _rationingSummary = new Label
            {
                Text = "RATIONING\n  —",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _rationingSummary.AddThemeFontSizeOverride("font_size", 11);
            rootVbox.AddChild(_rationingSummary);

            _goodsList = new VBoxContainer();
            scroll.AddChild(_goodsList);
        }

        public void BindSession(EconomyHostSession session)
        {
            _session = session;
            if (_session != null)
                _session.StateChanged += RefreshView;
            // First paint: deferred so snapshot fixtures (which bind before
            // _Ready) still get a populated list on the captured frame.
            CallDeferred(nameof(RefreshView));
        }

        /// <summary>
        /// Bind the foundry stance surface so the live market strip shows the
        /// Silent Foundry's trade access (Trade / Rob / HostileRaid) and trust.
        /// The provider is the existing FactionStanceEngine; no new authority.
        /// </summary>
        public void BindStance(Ashfall.Core.Economy.IFactionStanceProvider provider, string factionId)
        {
            _stanceProvider = provider;
            _stanceFactionId = factionId ?? string.Empty;
            RefreshView();
        }

        public void UnbindSession()
        {
            if (_session == null) return;
            _session.StateChanged -= RefreshView;
            _session = null!;
        }

        public override void _ExitTree()
        {
            UnbindSession();
            base._ExitTree();
        }

        public void RefreshView()
        {
            // Snapshot fixtures bind before _Ready — the row list does not
            // exist yet; the deferred first refresh (BindSession) covers it.
            if (_goodsList == null || _session == null || _session.Catalog == null) return;

            AshfallUiHelpers.EmptyChildren(_goodsList);_lblSummary.Text =
                $"Day {_session.Market.Day} · ledger {_session.Market.State.ledger.Count} lines · " +
                $"supplies {( _session.Market.IsSuppliesShort() ? "SHORT" : "normal")}";

            if (_stanceProvider != null && !string.IsNullOrEmpty(_stanceFactionId))
            {
                var stance = _stanceProvider.GetStance(_stanceFactionId);
                float trust = _stanceProvider.GetEffectiveTrust(_stanceFactionId);
                string access = stance switch
                {
                    Ashfall.Core.Economy.TradeStance.Trade => "open",
                    Ashfall.Core.Economy.TradeStance.ShareIntel => "open (intel)",
                    Ashfall.Core.Economy.TradeStance.Refuse => "REFUSED",
                    Ashfall.Core.Economy.TradeStance.Rob => "BLOCKED — ROBBERY RISK",
                    Ashfall.Core.Economy.TradeStance.HostileRaid => "BLOCKED — HOSTILE",
                    _ => stance.ToString()
                };
                _lblSummary.Text += $" · FOUNDRY GUILD stall {access} · trust {trust:F0}";
            }

            RefreshCommodityTrends();
            RefreshRationing();

            foreach (var good in _session.Catalog.All())
            {
                var row = new HBoxContainer();
                row.AddThemeConstantOverride("separation", 8);

                var icon = new TextureRect
                {
                    CustomMinimumSize = new Vector2(32, 32),
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
                };
                var asset = AssetRegistry.GetItem(good.id);
                if (asset.Texture != null)
                {
                    icon.Texture = asset.Texture;
                }
                else
                {
                    _fallbackObserved = true; // fallback path exercised (missing icon)
                }
                row.AddChild(icon);

                var label = new Label
                {
                    Text = $"{good.displayName} — {_session.Market.GetPrice(good.id):0.00} " +
                           $"(demand {_session.Market.GetDemandMultiplier(good.id):0.00})",
                    CustomMinimumSize = new Vector2(300, 0)
                };
                label.AddThemeFontSizeOverride("font_size", 11);
                row.AddChild(label);

                // Plan 56 phase 4 — provenance tag: text label, never color-only
                // (Plan 14 accessibility). Empty for unannotated goods.
                var provenance = RegionalSupplyRouter.ProvenanceLabel(
                    _session.Catalog, CurrentRegion, good.id);
                if (!string.IsNullOrEmpty(provenance))
                {
                    var tag = new Label
                    {
                        Text = "[" + provenance + "]",
                        CustomMinimumSize = new Vector2(120, 0)
                    };
                    tag.AddThemeFontSizeOverride("font_size", 10);
                    row.AddChild(tag);
                }

                _goodsList.AddChild(row);
            }
        }

        /// <summary>
        /// Plan 215 — rationing overlay readout. Distinguishes policy from
        /// stock: the multiplier line is policy; stock levels live with the
        /// inventory owner and are never re-computed here.
        /// </summary>
        private void OnRationingApplyPressed()
        {
            if (RationingProtocolCommand == null || _rationingProtocolChoice.Selected < 0
                || _rationingProtocolChoice.ItemCount == 0)
            {
                return;
            }
            string protocolId = _rationingProtocolChoice.GetItemMetadata(_rationingProtocolChoice.Selected).AsString();
            if (string.IsNullOrEmpty(protocolId)) return;
            string feedback = RationingProtocolCommand(protocolId);
            if (!string.IsNullOrEmpty(feedback) && _rationingSummary != null)
            {
                _rationingSummary.Text = feedback;
            }
            RefreshView();
        }

        private void RefreshRationing()
        {
            if (_rationingSummary == null || _session == null) return;
            var rationing = _session.Rationing;

            var previous = _rationingProtocolChoice.Selected >= 0
                ? _rationingProtocolChoice.GetItemMetadata(_rationingProtocolChoice.Selected).AsString()
                : null;
            _rationingProtocolChoice.Clear();
            foreach (var proto in _session.RationingProtocols)
            {
                if (proto == null || string.IsNullOrWhiteSpace(proto.Id)) continue;
                _rationingProtocolChoice.AddItem(string.IsNullOrEmpty(proto.Name) ? proto.Id : proto.Name);
                _rationingProtocolChoice.SetItemMetadata(_rationingProtocolChoice.ItemCount - 1, proto.Id);
            }
            _rationingProtocolChoice.Selected = _rationingProtocolChoice.ItemCount > 0 ? 0 : -1;
            if (!string.IsNullOrEmpty(previous))
            {
                for (int i = 0; i < _rationingProtocolChoice.ItemCount; i++)
                {
                    if (_rationingProtocolChoice.GetItemMetadata(i).AsString() == previous)
                    {
                        _rationingProtocolChoice.Select(i);
                        break;
                    }
                }
            }
            _rationingApplyButton.Disabled = RationingProtocolCommand == null || _rationingProtocolChoice.ItemCount == 0;

            var lines = new System.Collections.Generic.List<string>();
            var active = rationing.ActiveProtocol;
            lines.Add($"  policy: {(active != null ? $"{active.Name} ({rationing.ActiveProtocolId})" : rationing.ActiveProtocolId + " (legacy, definition not loaded)")}");
            foreach (var target in rationing.RationTargets)
            {
                if (target == null || string.IsNullOrWhiteSpace(target.ResourceId)) continue;
                lines.Add($"  {target.ResourceId}: {target.Tier} (×{target.BaseMultiplier:0.00})");
            }
            if (rationing.TargetCount == 0)
            {
                lines.Add("  no ration targets set — full allocations");
            }
            int activeCrises = rationing.ActiveCrisesCount;
            lines.Add($"  crises: {(activeCrises == 0 ? "none active" : $"{activeCrises} active")}");
            _rationingSummary.Text = "RATIONING\n" + string.Join("\n", lines);
        }

        private static Control MakeSection(string title)
        {
            var label = new Label { Text = title };
            label.AddThemeFontSizeOverride("font_size", 12);
            return label;
        }

        /// <summary>
        /// Plan 212 — commodity trend strip: category index with an arrow and
        /// a word (accessibility: never color-only). Categories with no
        /// tracked index read as stable. Canonical values only — no
        /// presentation-side re-computation.
        /// </summary>
        private void RefreshCommodityTrends()
        {
            if (_commodityTrends == null || _session == null) return;
            var market = _session.Market;
            var lines = new System.Collections.Generic.List<string>();
            foreach (var category in Ashfall.Core.Economy.GoodCategories.Known)
            {
                var baseline = market.FindCommodityBaseline(category);
                if (baseline == null) continue;
                float current = market.GetCategoryMultiplier(category);
                float target = baseline.base_multiplier_permille / 1000f;
                string arrow = current > target * 1.03f ? "▲" : current < target * 0.97f ? "▼" : "—";
                string word = arrow == "▲" ? "rising" : arrow == "▼" ? "falling" : "stable";
                lines.Add($"  {arrow} {word}: {category} ×{current:0.00}");
            }
            var active = market.ActiveShocks;
            if (active.Count > 0)
            {
                foreach (var shock in active)
                    lines.Add($"  ! {shock.categoryId} {(shock.isShortage ? "SHORTAGE" : "CRASH")} "
                        + $"(x{shock.severityBp / 10000f:0.00}) until day {shock.expiryDay}");
            }
            _commodityTrends.Text = "COMMODITY TRENDS\n" + (lines.Count == 0 ? "  — all stable" : string.Join("\n", lines));
        }
    }
}
