using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Faction Detail panel.
    /// Shows comprehensive diplomatic dossier, ideological alignment, trade commodities,
    /// treaty provisions, trust ratings, and intelligence logs for a selected faction.
    /// </summary>
    public partial class FactionDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _infoContainer = null!;
        private VBoxContainer _diplomacyContainer = null!;
        private VBoxContainer _tradeContainer = null!;
        private VBoxContainer _eventsContainer = null!;
        private Label _titleLabel = null!;

        public void Bind(
            HoldfastFactionEntry faction,
            HoldfastTradeSession? trade = null,
            MusterHostSession? muster = null,
            ExpansionHostSession? expansions = null)
        {
            if (faction == null) return;

            if (_titleLabel != null)
                _titleLabel.Text = $"DIPLOMATIC DOSSIER // {faction.DisplayName.ToUpperInvariant()}";

            if (_infoContainer == null || _diplomacyContainer == null ||
                _tradeContainer == null || _eventsContainer == null)
                return;

            AshfallUiHelpers.EmptyChildren(_infoContainer);
            AshfallUiHelpers.EmptyChildren(_diplomacyContainer);
            AshfallUiHelpers.EmptyChildren(_tradeContainer);
            AshfallUiHelpers.EmptyChildren(_eventsContainer);

            // ── 1. Faction Profile Card ──
            var infoCard = AshfallUiHelpers.MakeCardFrame("FACTION PROFILE & DOCTRINE", faction.Id);
            var infoBox = infoCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            var headerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var emblem = AshfallUiHelpers.MakeFactionEmblem(faction.Id, 56);
            headerRow.AddChild(emblem);

            var quoteBox = AshfallUiHelpers.MakeVBox(4);
            quoteBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var quoteLbl = AshfallUiHelpers.MakeBody(faction.SignatureQuote);
            quoteLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            quoteBox.AddChild(quoteLbl);

            headerRow.AddChild(quoteBox);
            infoBox.AddChild(headerRow);

            infoBox.AddChild(AshfallUiHelpers.MakeSeparator());

            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Alignment & Doctrine", faction.Alignment, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Base of Operations", string.IsNullOrEmpty(faction.HomeRegion) ? "Mobile / Wasteland Grid" : faction.HomeRegion, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Operational Status", faction.IsActive ? "ACTIVE SOVEREIGN POWER" : "DORMANT CADRE", AshfallUiHelpers.ToColor(faction.IsActive ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));
            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Access Treaty", faction.AccessRule, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _infoContainer.AddChild(infoCard);

            // ── 2. Diplomatic Standing & Trust ──
            var dipCard = AshfallUiHelpers.MakeCardFrame("DIPLOMATIC STANDING & TRUST METRICS", "SECURITY RATING");
            var dipBox = dipCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            float trustVal = faction.Trust;
            if (faction.Id == "faction_scavenger_guild" && muster?.ScavengerGuild != null)
                trustVal = muster.ScavengerGuild.Trust;

            string stance = trustVal >= 75 ? "Strategic Allied Partner" : (trustVal >= 50 ? "Neutral Barter Accord" : (trustVal >= 25 ? "Cautious Non-Aggression" : "Hostile / Sanctioned"));

            dipBox.AddChild(AshfallUiHelpers.MakeDataRow("Trust Rating", $"{trustVal:F1} / 100", AshfallUiHelpers.ToColor(trustVal >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
            dipBox.AddChild(AshfallUiHelpers.MakeDataRow("Diplomatic Stance", stance, AshfallUiHelpers.ToColor(trustVal >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
            dipBox.AddChild(AshfallUiHelpers.MakeDataRow("Border Clearance", "Holdfast Cohort Cleared for Transit", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            dipBox.AddChild(AshfallUiHelpers.MakeDataRow("Extradition Accord", "Standard Wasteland Neutrality", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            _diplomacyContainer.AddChild(dipCard);

            // ── 3. Trade & Resource Exchange ──
            var tradeCard = AshfallUiHelpers.MakeCardFrame("COMMERCE, SUPPLY & COMMODITY SPECIALIZATION", "MARKET EXCHANGE");
            var tradeBox = tradeCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            string wants = faction.Wants != null && faction.Wants.Length > 0 ? string.Join(", ", faction.Wants).Replace('_', ' ') : "Raw Materials, Clean Water, Medical Supplies";
            string offers = faction.Offers != null && faction.Offers.Length > 0 ? string.Join(", ", faction.Offers).Replace('_', ' ') : "Fuel, Radiation Shielding, Mechanical Parts";

            tradeBox.AddChild(AshfallUiHelpers.MakeDataRow("High Demand (Wants)", wants, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            tradeBox.AddChild(AshfallUiHelpers.MakeDataRow("Surplus Supply (Offers)", offers, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            tradeBox.AddChild(AshfallUiHelpers.MakeDataRow("Tariff Rating", "Standard Ledger Value (0% Mark-up)", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            tradeBox.AddChild(AshfallUiHelpers.MakeDataRow("Payment Medium", "Barter Currency / Ledger Credit Chits", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _tradeContainer.AddChild(tradeCard);

            // ── 4. Intelligence & Historical Logs ──
            var eventCard = AshfallUiHelpers.MakeCardFrame("INTELLIGENCE ARCHIVE & RECENT DISPATCHES", "TRANSMISSIONS");
            var eventBox = eventCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            eventBox.AddChild(AshfallUiHelpers.MakeDataRow("Historical Note", $"{faction.DisplayName} established territory in the early months following the initial nuclear exchange.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            eventBox.AddChild(AshfallUiHelpers.MakeDataRow("Recent Action", "Emissary contact confirmed standing treaties remain in effect for the current season.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            eventBox.AddChild(AshfallUiHelpers.MakeDataRow("Threat Advisory", "Maintain diplomatic protocols and avoid unverified scavenging in marked sectors.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            _eventsContainer.AddChild(eventCard);
        }

        public void Bind(object faction)
        {
            if (faction is HoldfastFactionEntry entry)
                Bind(entry);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.03f, 0.04f, 0.05f, 0.96f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
            AddChild(scroll);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            center.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(center);

            var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(760, 0);
            center.AddChild(rootBox);

            _titleLabel = AshfallUiHelpers.MakeTitle("FACTION DIPLOMATIC DOSSIER", Ashfall.Core.UI.Theme.FontSizeH1);
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(_titleLabel);

            var sub = AshfallUiHelpers.MakeMetadata("Comprehensive diplomatic standing, treaty protocols, and trade specialization profile.");
            sub.HorizontalAlignment = HorizontalAlignment.Center;
            sub.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(sub);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _infoContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_infoContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _diplomacyContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_diplomacyContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _tradeContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_tradeContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _eventsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_eventsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO FACTIONS [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            rootBox.AddChild(btnClose);
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
