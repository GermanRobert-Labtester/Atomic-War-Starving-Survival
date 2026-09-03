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

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(420, 300);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
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

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 220)
            };
            rootVbox.AddChild(scroll);

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
    }
}
