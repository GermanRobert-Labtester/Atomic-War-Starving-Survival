using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.YearOfAsh;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Factions & Diplomacy panel.
    /// Manages wasteland faction relations, trust metrics, trade privileges,
    /// Scavenger Guild claims, Crossing arbitration, and diplomatic communiques.
    /// </summary>
    public partial class FactionsPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnFactionDetailRequested;
        public event Action? OnMusterPanelRequested;
        public event Action? OnFoundryPanelRequested;
        /// <summary>Player chose to pay the warlord tribute in full (amount = current ask).</summary>
        public event Action<int>? OnWarlordTributePay;
        /// <summary>Player refused the warlord tribute this week.</summary>
        public event Action? OnWarlordTributeRefuse;

        private VBoxContainer _overviewContainer = null!;
        private VBoxContainer _factionsContainer = null!;
        private VBoxContainer _relationsContainer = null!;
        private VBoxContainer _eventsContainer = null!;
        private Label _statusSummary = null!;

        private HoldfastFactionsCatalog? _factions;
        private HoldfastTradeSession? _trade;
        private MusterHostSession? _muster;
        private ExpansionHostSession? _expansions;
        private YearOfAshHostSession? _yearOfAsh;

        public bool IsBound => _factions != null || _muster != null || _expansions != null;

        /// <summary>True after RefreshView when the Silent Foundry Guild card rendered.</summary>
        public bool HasGuildCard { get; private set; }

        /// <summary>Last authored collector line shown in the warlord card (presentation-local).</summary>
        private string _collectorNote = string.Empty;

        public void Bind(
            HoldfastFactionsCatalog? factions,
            HoldfastTradeSession? trade = null,
            MusterHostSession? muster = null,
            ExpansionHostSession? expansions = null,
            YearOfAshHostSession? yearOfAsh = null)
        {
            _factions = factions;
            _trade = trade;
            _muster = muster;
            _expansions = expansions;
            _yearOfAsh = yearOfAsh;

            if (_muster != null)
                _muster.StateChanged += RefreshView;
            if (_expansions != null)
                _expansions.StateChanged += RefreshView;
            if (_yearOfAsh?.Warlord != null)
            {
                _yearOfAsh.Warlord.OnStateChanged += RefreshView;
                _yearOfAsh.Warlord.OnTributeSettled += (paidFull, day) =>
                    _collectorNote = _yearOfAsh.CollectorLine(paidFull ? "paid" : "short", day);
                _yearOfAsh.Warlord.OnTributeDemanded += (_, _, day) =>
                    _collectorNote = _yearOfAsh.CollectorLine("demand", day);
            }

            RefreshView();
        }

        public void RefreshView()
        {
            if (_overviewContainer == null || _factionsContainer == null ||
                _relationsContainer == null || _eventsContainer == null)
                return;

            AshfallUiHelpers.EmptyChildren(_overviewContainer);
            AshfallUiHelpers.EmptyChildren(_factionsContainer);
            AshfallUiHelpers.EmptyChildren(_relationsContainer);
            AshfallUiHelpers.EmptyChildren(_eventsContainer);

            // ── 1. Diplomatic Summary ──
            int totalFactions = _factions?.Count ?? 5;
            float guildTrust = _muster?.ScavengerGuild?.Trust ?? 50.0f;
            int guildClaims = _muster?.ScavengerGuild?.State?.claimedSiteIds?.Count ?? 0;
            int blacklistedCount = _muster?.ScavengerGuild?.State?.blacklistedShelterIds?.Count ?? 0;

            var ovCard = AshfallUiHelpers.MakeCardFrame("WASTELAND DIPLOMATIC & TRADE NETWORK", "REGISTRY STATUS");
            var ovBox = ovCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Known Major Factions", $"{totalFactions} Sovereign Organizations", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Scavenger Guild Trust", $"{guildTrust:F1} / 100", AshfallUiHelpers.ToColor(guildTrust >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Guild Claimed Sites", $"{guildClaims} Active Mining / Scrap Claims", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Sanctions & Blacklists", blacklistedCount > 0 ? $"{blacklistedCount} Active Hostile Enforcements" : "Zero Sanctions Imposed", AshfallUiHelpers.ToColor(blacklistedCount > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));

            if (_muster != null)
            {
                var btnMuster = AshfallUiHelpers.MakeButton("OPEN SECTOR MUSTER // CURRENTS & ESCALATION", () =>
                {
                    OnMusterPanelRequested?.Invoke();
                });
                ovBox.AddChild(btnMuster);
            }

            _overviewContainer.AddChild(ovCard);

            // ── 2. Known Factions List ──
            var factionEntries = new List<HoldfastFactionEntry>();
            if (_factions != null && _factions.Count > 0)
            {
                foreach (var f in _factions)
                {
                    if (f != null && !string.IsNullOrEmpty(f.Id))
                        factionEntries.Add(f);
                }
            }

            // If empty, supply canonical core factions
            if (factionEntries.Count == 0)
            {
                factionEntries.Add(new HoldfastFactionEntry(
                    "faction_black_flotilla", "The Black Flotilla", "Maritime Traders / Neutral", "The Flooded Coast",
                    true, 45f, new[] { "clean_water", "medicine", "electronics" }, new[] { "fuel", "fish_rations", "filter_spares" },
                    "\"The sea did not burn. It only poisoned. We sail what remains.\"", "Open Water Barter Agreement"));

                factionEntries.Add(new HoldfastFactionEntry(
                    "faction_scavenger_guild", "The Scavenger Guild", "Industrial Scrappers / Pragmatic", "loc_scavenger_guildhall",
                    true, guildTrust, new[] { "dosimeters", "scrap_metal", "tools" }, new[] { "mechanical_parts", "lead_sheeting" },
                    "\"Every ruin has an owner. Violate the two-color ledger at your peril.\"", "Brannick Sten's Claim Accord"));

                factionEntries.Add(new HoldfastFactionEntry(
                    "faction_ledger_keepers", "The Ledger Keepers", "Archivists & Chroniclers / Neutral", "The High Vaults",
                    true, 60f, new[] { "cassette_tapes", "books", "schematics" }, new[] { "purified_water", "anti_rad_pills" },
                    "\"The war took the cities. We will not let it take the memory.\"", "Mutual Archival Exchange"));

                factionEntries.Add(new HoldfastFactionEntry(
                    "faction_iron_covenant", "The Iron Covenant", "Militant Enclave / Wary", "Sector 01 Outpost",
                    true, 30f, new[] { "ammunition", "armor_plates", "fuel" }, new[] { "weapons", "reinforced_concrete" },
                    "\"Order is forged under pressure. Civilians stay outside the gate.\"", "Armistice Checkpoint"));

                factionEntries.Add(new HoldfastFactionEntry(
                    "faction_green_thread", "The Green Thread", "Agrarian Collectivists / Cautious Allies", "The Allotments",
                    true, 55f, new[] { "seeds", "potassium_iodide", "fertilizer" }, new[] { "fresh_produce", "herbal_poultices" },
                    "\"The soil will breathe again if we shield the roots from fallout.\"", "Seed Sharing Protocol"));
            }

            foreach (var f in factionEntries)
            {
                var card = AshfallUiHelpers.MakeCardFrame(f.DisplayName, f.Alignment.ToUpperInvariant());
                var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                var headerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var emblem = AshfallUiHelpers.MakeFactionEmblem(f.Id, 44);
                headerRow.AddChild(emblem);

                var quoteBox = AshfallUiHelpers.MakeVBox(2);
                quoteBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                var quoteLbl = AshfallUiHelpers.MakeSmall(f.SignatureQuote);
                quoteLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                quoteBox.AddChild(quoteLbl);

                var regionLbl = AshfallUiHelpers.MakeLabel($"Base: {f.HomeRegion} · Stance: {f.AccessRule}", Ashfall.Core.UI.Theme.FontSizeLabel, Ashfall.Core.UI.Theme.Muted);
                quoteBox.AddChild(regionLbl);
                headerRow.AddChild(quoteBox);
                cardBox.AddChild(headerRow);

                cardBox.AddChild(AshfallUiHelpers.MakeSeparator());

                // Trade profile
                string wantsText = f.Wants != null && f.Wants.Length > 0 ? string.Join(", ", f.Wants) : "None registered";
                string offersText = f.Offers != null && f.Offers.Length > 0 ? string.Join(", ", f.Offers) : "None registered";

                cardBox.AddChild(AshfallUiHelpers.MakeDataRow("Demand (Wants)", wantsText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                cardBox.AddChild(AshfallUiHelpers.MakeDataRow("Supply (Offers)", offersText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                cardBox.AddChild(AshfallUiHelpers.MakeDataRow("Standing / Trust", $"{f.Trust:F1} / 100", AshfallUiHelpers.ToColor(f.Trust >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));

                var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                string factionId = f.Id;
                var btnInspect = AshfallUiHelpers.MakeButton($"DIPLOMATIC DOSSIER // [{f.DisplayName}]", () =>
                {
                    OnFactionDetailRequested?.Invoke(factionId);
                });
                btnInspect.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                btnRow.AddChild(btnInspect);
                cardBox.AddChild(btnRow);

                _factionsContainer.AddChild(card);
            }

            // ── 2b. Treaty Systems — The Silent Foundry Guild (Exp 10) ──
            var foundrySys = _expansions?.SilentFoundry;
            var foundryFaction = _expansions?.FoundryData?.Faction;
            HasGuildCard = foundrySys != null && foundryFaction != null;
            if (foundrySys != null && foundryFaction != null)
            {
                var guildCard = AshfallUiHelpers.MakeCardFrame(
                    foundryFaction.display_name, "ACCORD SYSTEMS // THE WORKS");
                var guildBox = guildCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                var guildHeader = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var guildEmblem = AshfallUiHelpers.MakeFactionEmblem(foundryFaction.faction_id, 44);
                guildHeader.AddChild(guildEmblem);
                var guildIdentity = AshfallUiHelpers.MakeSmall(foundryFaction.identity, autowrap: true);
                guildIdentity.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                guildIdentity.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                guildHeader.AddChild(guildIdentity);
                guildBox.AddChild(guildHeader);
                guildBox.AddChild(AshfallUiHelpers.MakeSeparator());

                float standing = foundrySys.GuildStanding;
                guildBox.AddChild(AshfallUiHelpers.MakeDataRow("Foundry Standing", $"{standing:F0} / 100", AshfallUiHelpers.ToColor(standing >= 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
                guildBox.AddChild(AshfallUiHelpers.MakeDataRow("Foundry", foundrySys.IsUnlocked ? $"OPEN · heat {foundrySys.HeatStage} · casts {foundrySys.TotalProductionCount}" : "SEALED — blueprint catalogued", AshfallUiHelpers.ToColor(foundrySys.IsUnlocked ? Ashfall.Core.UI.Theme.Pale : Ashfall.Core.UI.Theme.Dim)));

                if (foundryFaction.internal_divisions != null && foundryFaction.internal_divisions.Length > 0)
                    guildBox.AddChild(AshfallUiHelpers.MakeDataRow("Internal Divisions", string.Join(", ", foundryFaction.internal_divisions).Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));

                foreach (var rel in foundryFaction.relationships)
                {
                    if (rel == null || string.IsNullOrEmpty(rel.faction_id)) continue;
                    guildBox.AddChild(AshfallUiHelpers.MakeDataRow(
                        "↔ " + rel.faction_id.Replace('_', ' '),
                        rel.stance.Replace('_', ' ') + " — " + rel.notes, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
                }

                var btnFoundry = AshfallUiHelpers.MakeButton("OPEN THE FOUNDRY FLOOR", () =>
                {
                    // The host routes this through the standard panel-open path.
                    OnFoundryPanelRequested?.Invoke();
                });
                guildBox.AddChild(btnFoundry);

                _factionsContainer.AddChild(guildCard);
            }

            // ── 3. Strategic Standing & Legal Accords ──
            var relCard = AshfallUiHelpers.MakeCardFrame("STRATEGIC TREATIES & LEDGER DEBT", "TREATY STATUS");
            var relBox = relCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            relBox.AddChild(AshfallUiHelpers.MakeDataRow("Scavenger Guild Claim Ledger", "Two-color boundary system active. Stripping marked sites causes immediate blacklist.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            relBox.AddChild(AshfallUiHelpers.MakeDataRow("Nobody's Crossing Accord", "Vouch access required for passage across the northern ice road gate.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            relBox.AddChild(AshfallUiHelpers.MakeDataRow("Ledger Keepers Archive", "Knowledge reciprocity active. Relic blueprints grant credit value.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            _relationsContainer.AddChild(relCard);

            // ── 3b. Adaptive Warlord Doctrine (Year of Ash, proposed model) ──
            if (_yearOfAsh?.Warlord != null)
            {
                var w = _yearOfAsh.Warlord;
                var wl = w.Catalog.Warlord;
                var wCard = AshfallUiHelpers.MakeCardFrame("WARLORD DOCTRINE — SECTOR 4", "ADAPTIVE STRATEGY (identity: " + wl.faction_id + ")");
                var wBox = wCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                string doctrine = w.Doctrine != null ? w.Doctrine.display_name : w.DoctrineId;
                wBox.AddChild(AshfallUiHelpers.MakeDataRow("Current Doctrine", doctrine, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                wBox.AddChild(AshfallUiHelpers.MakeDataRow("Supply", w.Supply + " / " + w.SupplyNeed, AshfallUiHelpers.ToColor(w.Supply < w.SupplyNeed ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));

                // Tribute ledger (player-visible, from Core state).
                int ask = Math.Max(1, (int)(wl.tribute_base_amount * w.TributeMultiplier));
                string tributeState = w.State.consecutiveShortWeeks > 0
                    ? $"ask {ask}× {wl.tribute_currency_item} — {w.State.consecutiveShortWeeks} short week(s), collector is keeping notes"
                    : $"ask {ask}× {wl.tribute_currency_item} — ledger current";
                wBox.AddChild(AshfallUiHelpers.MakeDataRow("Tribute", tributeState,
                    AshfallUiHelpers.ToColor(w.State.consecutiveShortWeeks > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm)));
                wBox.AddChild(AshfallUiHelpers.MakeDataRow("Paid to Date", w.State.totalWeeksPaid + " of " + w.State.totalWeeksAsked + " asks", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                wBox.AddChild(AshfallUiHelpers.MakeDataRow("Operations", w.TotalOperations + " · " + w.State.casualties + " casualties", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));

                // Collector note (authored prose, deterministic by day).
                if (!string.IsNullOrEmpty(_collectorNote))
                    wBox.AddChild(AshfallUiHelpers.MakeSmall(_collectorNote, autowrap: true));

                // Payment loop: pay the current ask in full, or refuse it.
                if (_yearOfAsh != null)
                {
                    var payRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    payRow.AddThemeConstantOverride("h_separation", (int)Ashfall.Core.UI.Theme.SpacingSm);
                    var btnPay = AshfallUiHelpers.MakeButton($"PAY TRIBUTE ({ask}× {wl.tribute_currency_item})", () => OnWarlordTributePay?.Invoke(ask));
                    btnPay.CustomMinimumSize = new Vector2(300, 34);
                    payRow.AddChild(btnPay);
                    var btnRefuse = AshfallUiHelpers.MakeButton("REFUSE THIS WEEK", () => OnWarlordTributeRefuse?.Invoke());
                    btnRefuse.CustomMinimumSize = new Vector2(180, 34);
                    payRow.AddChild(btnRefuse);
                    wBox.AddChild(payRow);
                }

                if (w.State.territory != null)
                {
                    for (int i = 0; i < w.State.territory.Count; i++)
                    {
                        var rec = w.State.territory[i];
                        if (rec == null) continue;
                        string stateName = ((Ashfall.Core.Warlords.WarlordTerritoryState)rec.state).ToString();
                        float danger = w.TravelDangerModifier(rec.locationId);
                        wBox.AddChild(AshfallUiHelpers.MakeDataRow(
                            rec.locationId,
                            stateName + (danger > 0f ? " · travel danger +" + (danger * 100f).ToString("F0") + "%" : ""),
                            AshfallUiHelpers.ToColor(rec.state == (int)Ashfall.Core.Warlords.WarlordTerritoryState.Controlled
                                ? Ashfall.Core.UI.Theme.Hot
                                : (rec.state == (int)Ashfall.Core.Warlords.WarlordTerritoryState.Contested ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim))));
                    }
                }
                _relationsContainer.AddChild(wCard);
            }

            // ── 4. Diplomatic Events & Radio Intercepts ──
            var evCard = AshfallUiHelpers.MakeCardFrame("RECENT DIPLOMATIC COMMUNIQUES", "RADIO INTERCEPTS");
            var evBox = evCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            evBox.AddChild(AshfallUiHelpers.MakeDataRow("[Day 04] Black Flotilla", "Coastal barge dispatch confirmed trade route into Sector 12.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            evBox.AddChild(AshfallUiHelpers.MakeDataRow("[Day 03] Scavenger Guild", "Brannick Sten renewed boundary markers near Denial Cut Substation.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            evBox.AddChild(AshfallUiHelpers.MakeDataRow("[Day 02] Ledger Keepers", "Emissary courier delivered technical index of surviving infrastructure.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            evBox.AddChild(AshfallUiHelpers.MakeDataRow("[Day 01] Green Thread", "Agrarian collective requested potassium iodide exchange for hydroponic seeds.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _eventsContainer.AddChild(evCard);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
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

            var title = AshfallUiHelpers.MakeTitle("FACTIONS & WASTELAND DIPLOMACY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(title);

            _statusSummary = AshfallUiHelpers.MakeMetadata("Monitor geopolitical standings, faction trust, trade specialization, claim boundaries, and diplomatic treaties.");
            _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
            _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(_statusSummary);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_overviewContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var factionsTitle = AshfallUiHelpers.MakeSectionHeader("KNOWN FACTION PROTOCOLS & ALLIANCES");
            rootBox.AddChild(factionsTitle);

            _factionsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_factionsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var relTitle = AshfallUiHelpers.MakeSectionHeader("TREATIES, STANDING & DEBT OBLIGATIONS");
            rootBox.AddChild(relTitle);

            _relationsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_relationsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var evTitle = AshfallUiHelpers.MakeSectionHeader("RECENT FACTION COMMUNIQUES & DISPATCHES");
            rootBox.AddChild(evTitle);

            _eventsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_eventsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE DIPLOMACY [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            rootBox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close factions panel");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
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

        public override void _ExitTree()
        {
            if (_muster != null)
                _muster.StateChanged -= RefreshView;
            if (_expansions != null)
                _expansions.StateChanged -= RefreshView;
            if (_yearOfAsh?.Warlord != null)
                _yearOfAsh.Warlord.OnStateChanged -= RefreshView;
            base._ExitTree();
        }
    }
}
