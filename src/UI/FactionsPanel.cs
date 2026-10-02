// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class FactionsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string>? OnFactionDetailRequested;
        public event Action? OnMusterPanelRequested;
        public event Action? OnFoundryPanelRequested;
        public event Action? OnCultureCodexRequested;
        public event Action? OnOpenStanceMatrixRequested;
        public event Action? OnOpenNarrativesRequested;
        /// <summary>Player chose to pay the warlord tribute in full (amount = current ask).</summary>
        public event Action<int>? OnWarlordTributePay;
        /// <summary>Player refused the warlord tribute this week.</summary>
        public event Action? OnWarlordTributeRefuse;
        /// <summary>Player contested this week's ask (recorded, no currency handed over).</summary>
        public event Action? OnWarlordTributeContest;
        /// <summary>Player handed over everything the collector asked for and more.</summary>
        public event Action? OnWarlordTributeSubmit;
        /// <summary>Truthful projection of whether this week's ask already has a response.</summary>
        public Func<string>? WarlordResponseStateProvider { get; set; }
        /// <summary>Player committed allegiance to a specific faction branch.</summary>
        public event Action<string>? OnCommitBranchRequested;

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
        private Ashfall.Core.Factions.FactionBranchCoordinator? _branchCoordinator;
        private InformantNetworkHostSession? _informantNetwork;
        private Ashfall.Core.MoralChoice.MoralChoiceSystem? _moralChoice;

        public bool IsBound =>
            _factions != null || _trade != null || _muster != null || _expansions != null ||
            _yearOfAsh != null || _branchCoordinator != null || _informantNetwork != null ||
            _moralChoice != null;

        /// <summary>True after RefreshView when the Silent Foundry Guild card rendered.</summary>
        public bool HasGuildCard { get; private set; }

        /// <summary>Last authored collector line shown in the warlord card (presentation-local).</summary>
        private string _collectorNote = string.Empty;

        public void Bind(
            HoldfastFactionsCatalog? factions,
            HoldfastTradeSession? trade = null,
            MusterHostSession? muster = null,
            ExpansionHostSession? expansions = null,
            YearOfAshHostSession? yearOfAsh = null,
            Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
            Ashfall.Core.MoralChoice.MoralChoiceSystem? moralChoice = null,
            InformantNetworkHostSession? informantNetwork = null)
        {
            Unbind(clearPresentation: false);
            _informantNetwork = informantNetwork;
            _factions = factions;
            _trade = trade;
            _muster = muster;
            _expansions = expansions;
            _yearOfAsh = yearOfAsh;
            _branchCoordinator = branchCoordinator;
            _moralChoice = moralChoice;

            if (_muster != null)
                _muster.StateChanged += RefreshView;
            if (_expansions != null)
                _expansions.StateChanged += RefreshView;
            if (_branchCoordinator != null)
                _branchCoordinator.OnStateChanged += RefreshView;
            if (_informantNetwork != null)
                _informantNetwork.StateChanged += RefreshView;
            if (_yearOfAsh?.Warlord != null)
            {
                _yearOfAsh.Warlord.OnStateChanged += RefreshView;
                _yearOfAsh.Warlord.OnTributeSettled += HandleWarlordTributeSettled;
                _yearOfAsh.Warlord.OnTributeDemanded += HandleWarlordTributeDemanded;
            }

            RefreshView();
        }

        private void HandleWarlordTributeSettled(bool paidFull, int day)
        {
            if (_yearOfAsh != null)
                _collectorNote = _yearOfAsh.CollectorLine(paidFull ? "paid" : "short", day);
        }

        private void HandleWarlordTributeDemanded(int amount, string itemId, int day)
        {
            if (_yearOfAsh != null)
                _collectorNote = _yearOfAsh.CollectorLine("demand", day);
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

            var ovCard = AshfallUiHelpers.MakeCardFrame(
                AshfallUiText.Tr("ui.factions.overview.title", "WASTELAND DIPLOMATIC & TRADE NETWORK"),
                AshfallUiText.Tr("ui.factions.overview.kicker", "REGISTRY STATUS"));
            var ovBox = ovCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            ovBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.known_major", "Known Major Factions"), $"{totalFactions} Sovereign Organizations", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.guild_trust", "Scavenger Guild Trust"), $"{guildTrust:F1} / 100", AshfallUiHelpers.ToColor(guildTrust >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.guild_claims", "Guild Claimed Sites"), $"{guildClaims} Active Mining / Scrap Claims", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.sanctions", "Sanctions & Blacklists"), blacklistedCount > 0 ? $"{blacklistedCount} Active Hostile Enforcements" : "Zero Sanctions Imposed", AshfallUiHelpers.ToColor(blacklistedCount > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));

            if (_muster != null)
            {
                var btnMuster = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.btn_muster", "OPEN SECTOR MUSTER // CURRENTS & ESCALATION"), () =>
                {
                    OnMusterPanelRequested?.Invoke();
                });
                ovBox.AddChild(btnMuster);
            }

            var btnCulture = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.btn_culture", "OPEN FACTION CULTURE CODEX // EVERYDAY CUSTOMS"), () =>
            {
                OnCultureCodexRequested?.Invoke();
            });
            ovBox.AddChild(btnCulture);
            var btnStanceMatrix = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.btn_stance", "FACTION STANCE MATRIX // RELATIONS GRID"), () =>
            {
                OnOpenStanceMatrixRequested?.Invoke();
            });
            ovBox.AddChild(btnStanceMatrix);
            var btnNarratives = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.btn_narratives", "FACTION NARRATIVES // DISPATCHES & ARCS"), () =>
            {
                OnOpenNarrativesRequested?.Invoke();
            });
            ovBox.AddChild(btnNarratives);

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
                var quoteLbl = AshfallUiHelpers.MakeSmall(f.SignatureQuote, autowrap: true);
                quoteLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                quoteLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                quoteBox.AddChild(quoteLbl);

                var regionLbl = AshfallUiHelpers.MakeLabel(
                    AshfallUiText.TrFormat("ui.factions.base_stance", "Base: {0} · Stance: {1}", f.HomeRegion, f.AccessRule),
                    Ashfall.Core.UI.Theme.FontSizeLabel, Ashfall.Core.UI.Theme.Muted);
                quoteBox.AddChild(regionLbl);
                headerRow.AddChild(quoteBox);
                cardBox.AddChild(headerRow);

                cardBox.AddChild(AshfallUiHelpers.MakeSeparator());

                // Trade profile
                string wantsText = f.Wants != null && f.Wants.Length > 0 ? string.Join(", ", f.Wants) : AshfallUiText.Tr("ui.factions.none_registered", "None registered");
                string offersText = f.Offers != null && f.Offers.Length > 0 ? string.Join(", ", f.Offers) : AshfallUiText.Tr("ui.factions.none_registered", "None registered");

                cardBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.demand", "Demand (Wants)"), wantsText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                cardBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.supply", "Supply (Offers)"), offersText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                cardBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.standing_trust", "Standing / Trust"), $"{f.Trust:F1} / 100", AshfallUiHelpers.ToColor(f.Trust >= 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));

                var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                string factionId = f.Id;
                var btnInspect = AshfallUiHelpers.MakeButton(AshfallUiText.TrFormat("ui.factions.dossier", "DIPLOMATIC DOSSIER // [{0}]", f.DisplayName), () =>
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
                    foundryFaction.display_name, AshfallUiText.Tr("ui.factions.foundry.kicker", "ACCORD SYSTEMS // THE WORKS"));
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
                guildBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.foundry.standing", "Foundry Standing"), $"{standing:F0} / 100", AshfallUiHelpers.ToColor(standing >= 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Critical)));
                guildBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.foundry.state", "Foundry"), foundrySys.IsUnlocked ? $"OPEN · heat {foundrySys.HeatStage} · casts {foundrySys.TotalProductionCount}" : "SEALED — blueprint catalogued", AshfallUiHelpers.ToColor(foundrySys.IsUnlocked ? Ashfall.Core.UI.Theme.Pale : Ashfall.Core.UI.Theme.Dim)));

                if (foundryFaction.internal_divisions != null && foundryFaction.internal_divisions.Length > 0)
                    guildBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.foundry.divisions", "Internal Divisions"), string.Join(", ", foundryFaction.internal_divisions).Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));

                foreach (var rel in foundryFaction.relationships)
                {
                    if (rel == null || string.IsNullOrEmpty(rel.faction_id)) continue;
                    guildBox.AddChild(AshfallUiHelpers.MakeDataRow(
                        "↔ " + rel.faction_id.Replace('_', ' '),
                        rel.stance.Replace('_', ' ') + " — " + rel.notes, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
                }

                var btnFoundry = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.btn_foundry", "OPEN THE FOUNDRY FLOOR"), () =>
                {
                    // The host routes this through the standard panel-open path.
                    OnFoundryPanelRequested?.Invoke();
                });
                guildBox.AddChild(btnFoundry);

                _factionsContainer.AddChild(guildCard);
            }

            // ── 3. Strategic Standing & Legal Accords ──
            var relCard = AshfallUiHelpers.MakeCardFrame(
                AshfallUiText.Tr("ui.factions.treaties.title", "STRATEGIC TREATIES & LEDGER DEBT"),
                AshfallUiText.Tr("ui.factions.treaties.kicker", "TREATY STATUS"));
            var relBox = relCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            relBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.treaty.guild", "Scavenger Guild Claim Ledger"), AshfallUiText.Tr("ui.factions.treaty.guild_detail", "Two-color boundary system active. Stripping marked sites causes immediate blacklist."), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            relBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.treaty.crossing", "Nobody's Crossing Accord"), AshfallUiText.Tr("ui.factions.treaty.crossing_detail", "Vouch access required for passage across the northern ice road gate."), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            relBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.treaty.archive", "Ledger Keepers Archive"), AshfallUiText.Tr("ui.factions.treaty.archive_detail", "Knowledge reciprocity active. Relic blueprints grant credit value."), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            _relationsContainer.AddChild(relCard);

            // ── 3b. Adaptive Warlord Doctrine (Year of Ash, proposed model) ──
            if (_yearOfAsh?.Warlord != null)
            {
                var w = _yearOfAsh.Warlord;
                var wl = w.Catalog.Warlord;
                var wCard = AshfallUiHelpers.MakeCardFrame(AshfallUiText.Tr("ui.factions.warlord.title", "WARLORD DOCTRINE — SECTOR 4"), AshfallUiText.TrFormat("ui.factions.warlord.kicker", "ADAPTIVE STRATEGY (identity: {0})", wl.faction_id));
                var wBox = wCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                string doctrine = w.Doctrine != null ? w.Doctrine.display_name : w.DoctrineId;
                wBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.warlord.doctrine", "Current Doctrine"), doctrine, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                wBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.warlord.supply", "Supply"), w.Supply + " / " + w.SupplyNeed, AshfallUiHelpers.ToColor(w.Supply < w.SupplyNeed ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));

                // Tribute ledger (player-visible, from Core state).
                int ask = Math.Max(1, (int)(wl.tribute_base_amount * w.TributeMultiplier));
                string tributeState = w.State.consecutiveShortWeeks > 0
                    ? AshfallUiText.TrFormat("ui.factions.warlord.tribute_short", "ask {0}× {1} — {2} short week(s), collector is keeping notes", ask, wl.tribute_currency_item, w.State.consecutiveShortWeeks)
                    : AshfallUiText.TrFormat("ui.factions.warlord.tribute_current", "ask {0}× {1} — ledger current", ask, wl.tribute_currency_item);
                wBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.warlord.tribute", "Tribute"), tributeState,
                    AshfallUiHelpers.ToColor(w.State.consecutiveShortWeeks > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm)));
                wBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.warlord.paid", "Paid to Date"), w.State.totalWeeksPaid + " of " + w.State.totalWeeksAsked + " asks", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                wBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.warlord.ops", "Operations"), w.TotalOperations + " · " + w.State.casualties + " casualties", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));

                // Collector note (authored prose, deterministic by day).
                if (!string.IsNullOrEmpty(_collectorNote))
                    wBox.AddChild(AshfallUiHelpers.MakeSmall(_collectorNote, autowrap: true));

                // Payment loop: pay the current ask in full, refuse it, contest it, or
                // hand it all over. Once the week has a response the buttons are
                // disabled — the response surface is idempotent, and the panel says so.
                if (_yearOfAsh != null)
                {
                    bool alreadyResponded = false;
                    try { alreadyResponded = (WarlordResponseStateProvider?.Invoke() ?? "awaiting response") != "awaiting response"; }
                    catch { alreadyResponded = false; }

                    var payRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    payRow.AddThemeConstantOverride("h_separation", (int)Ashfall.Core.UI.Theme.SpacingSm);
                    var btnPay = AshfallUiHelpers.MakeButton(AshfallUiText.TrFormat("ui.factions.warlord.pay", "PAY TRIBUTE ({0}× {1})", ask, wl.tribute_currency_item), () => OnWarlordTributePay?.Invoke(ask));
                    btnPay.CustomMinimumSize = new Vector2(300, 34);
                    btnPay.Disabled = alreadyResponded;
                    payRow.AddChild(btnPay);
                    var btnContest = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.warlord.contest", "CONTEST"), () => OnWarlordTributeContest?.Invoke());
                    btnContest.CustomMinimumSize = new Vector2(110, 34);
                    btnContest.Disabled = alreadyResponded;
                    payRow.AddChild(btnContest);
                    var btnSubmit = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.warlord.submit", "SUBMIT ALL"), () => OnWarlordTributeSubmit?.Invoke());
                    btnSubmit.CustomMinimumSize = new Vector2(130, 34);
                    btnSubmit.Disabled = alreadyResponded;
                    payRow.AddChild(btnSubmit);
                    var btnRefuse = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.warlord.refuse", "REFUSE THIS WEEK"), () => OnWarlordTributeRefuse?.Invoke());
                    btnRefuse.CustomMinimumSize = new Vector2(180, 34);
                    btnRefuse.Disabled = alreadyResponded;
                    payRow.AddChild(btnRefuse);
                    wBox.AddChild(payRow);

                    if (alreadyResponded)
                    {
                        try
                        {
                            wBox.AddChild(AshfallUiHelpers.MakeBody(
                                AshfallUiText.TrFormat("ui.factions.warlord.response_logged", "This week's answer is already logged: {0}. The collector will be back next week.", WarlordResponseStateProvider?.Invoke() ?? "responded")));
                        }
                        catch { /* projection failure must never break the panel */ }
                    }
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

            // ── 3b. The Weight of Choices: Faction Progression & Branch Storylines ──
            if (_branchCoordinator != null)
            {
                var branchCard = AshfallUiHelpers.MakeCardFrame(
                    AshfallUiText.Tr("ui.factions.branch.title", "THE WEIGHT OF CHOICES // FACTION PROGRESSION"),
                    AshfallUiText.Tr("ui.factions.branch.kicker", "STRATEGIC ALLEGIANCE"));
                var branchBox = branchCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                string activeFaction = _branchCoordinator.ActiveFactionKind.ToString();
                string activeBranch = _branchCoordinator.ActiveBranchId?.Replace('_', ' ') ?? AshfallUiText.Tr("ui.factions.branch.unaligned", "Unaligned (Prospective Paths Open)");
                string ponrText = _branchCoordinator.IsPonrLocked ? AshfallUiText.Tr("ui.factions.branch.ponr_locked", "LOCKED (Point of No Return Reached)") : AshfallUiText.Tr("ui.factions.branch.ponr_open", "Open (Pre-PoNR)");
                string endingText = _branchCoordinator.ResolvedEndingId?.Replace('_', ' ') ?? AshfallUiText.Tr("ui.factions.branch.unresolved", "Unresolved");

                branchBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.branch.active", "Active Allegiance"), activeFaction, AshfallUiHelpers.ToColor(_branchCoordinator.IsCommitted ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale)));
                branchBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.branch.current", "Current Branch"), activeBranch.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
                branchBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.branch.ponr", "PoNR Status"), ponrText, AshfallUiHelpers.ToColor(_branchCoordinator.IsPonrLocked ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale)));
                if (_branchCoordinator.ResolvedEndingId != null)
                {
                    branchBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.branch.ending", "Resolved Ending"), endingText.Replace('_', ' '), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                }

                branchBox.AddChild(AshfallUiHelpers.MakeSeparator());

                // Standing summaries
                var standings = _branchCoordinator.GetFactionStandingSummaries();
                foreach (var s in standings)
                {
                    string statusDesc = s.IsJoined ? AshfallUiText.Tr("ui.factions.standing.joined", "Joined / Allied") : (s.IsOpposed ? AshfallUiText.Tr("ui.factions.standing.opposed", "Opposed") : (s.IsHostile ? AshfallUiText.Tr("ui.factions.standing.hostile", "Hostile") : (s.IsAllied ? AshfallUiText.Tr("ui.factions.standing.allied", "Allied") : AshfallUiText.Tr("ui.factions.standing.neutral", "Neutral"))));
                    branchBox.AddChild(AshfallUiHelpers.MakeDataRow(
                        s.DisplayName,
                        AshfallUiText.TrFormat("ui.factions.standing.summary", "Standing: {0} | Alignment: {1} ({2})", s.Standing.ToString("+0;-0;0"), s.Alignment.ToString("+0;-0;0"), statusDesc),
                        AshfallUiHelpers.ToColor(s.IsHostile ? Ashfall.Core.UI.Theme.Critical : (s.IsJoined || s.IsAllied ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale))));
                }

                branchBox.AddChild(AshfallUiHelpers.MakeSeparator());
                if (_informantNetwork != null)
                {
                    branchBox.AddChild(AshfallUiHelpers.MakeSmall(AshfallUiText.Tr("ui.factions.informant.header", "INFORMANT NETWORK // FIELD TRADECRAFT")));
                    foreach (var informant in _informantNetwork.System.State.informants)
                    {
                        branchBox.AddChild(AshfallUiHelpers.MakeDataRow(
                            $"{informant.InformantId} ({informant.Archetype})",
                            informant.IsCompromised
                                ? AshfallUiText.TrFormat("ui.factions.informant.burned", "BURNED · suspicion {0}/1000", informant.SuspicionPermille)
                                : AshfallUiText.TrFormat("ui.factions.informant.readout", "loyalty {0}/1000 · suspicion {1}/1000 · yield {2}/1000", informant.LoyaltyPermille, informant.SuspicionPermille, informant.IntelligenceYieldPermille),
                            AshfallUiHelpers.ToColor(informant.IsCompromised
                                ? Ashfall.Core.UI.Theme.Critical
                                : (informant.SuspicionPermille >= 500 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale))));
                    }
                    branchBox.AddChild(AshfallUiHelpers.MakeSmall(_informantNetwork.Readout()));
                    branchBox.AddChild(AshfallUiHelpers.MakeSeparator());
                }
                branchBox.AddChild(AshfallUiHelpers.MakeSmall(AshfallUiText.Tr("ui.factions.branch.paths", "BRANCH PATH AVAILABILITY & CONSEQUENCES:")));

                var options = _branchCoordinator.GetBranchOptions(_moralChoice);
                int rendered = 0;
                foreach (var opt in options)
                {
                    if (rendered >= 6 && !_branchCoordinator.IsCommitted) break;
                    if (_branchCoordinator.IsCommitted && !opt.IsCommitted) continue;

                    string statusTag = opt.IsCommitted ? "[COMMITTED]" : (opt.IsAvailable ? "[AVAILABLE]" : $"[LOCKED: {opt.LockoutReason}]");
                    var optRow = AshfallUiHelpers.MakeDataRow(
                        $"{opt.DisplayName} ({opt.FactionKind})",
                        statusTag,
                        AshfallUiHelpers.ToColor(opt.IsCommitted ? Ashfall.Core.UI.Theme.Hot : (opt.IsAvailable ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));
                    branchBox.AddChild(optRow);

                    if (opt.IsCommitted || opt.IsAvailable)
                    {
                        var desc = AshfallUiHelpers.MakeSmall(AshfallUiText.TrFormat("ui.factions.branch.consequence", "Consequence: {0} · Trigger: {1}", opt.ConsequencesSummary, opt.PonrTrigger));
                        branchBox.AddChild(desc);
                    }

                    if (opt.IsAvailable && !_branchCoordinator.IsCommitted)
                    {
                        string branchId = opt.BranchId;
                        var commitBtn = AshfallUiHelpers.MakeButton(AshfallUiText.TrFormat("ui.factions.branch.commit", "COMMIT ALLEGIANCE // [{0}]", opt.DisplayName.ToUpperInvariant()), () =>
                        {
                            OnCommitBranchRequested?.Invoke(branchId);
                        });
                        commitBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                        branchBox.AddChild(commitBtn);

                        var warn = AshfallUiHelpers.MakeSmall(AshfallUiText.Tr("ui.factions.branch.warning", "WARNING: Committing allegiance permanently locks out competing factions. The door will close."));
                        warn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warning));
                        branchBox.AddChild(warn);
                    }
                    rendered++;
                }

                _relationsContainer.AddChild(branchCard);
            }

            // ── 4. Diplomatic Events & Radio Intercepts ──
            var evCard = AshfallUiHelpers.MakeCardFrame(
                AshfallUiText.Tr("ui.factions.events.title", "RECENT DIPLOMATIC COMMUNIQUES"),
                AshfallUiText.Tr("ui.factions.events.kicker", "RADIO INTERCEPTS"));
            var evBox = evCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            var catalog = _yearOfAsh?.WarRunner?.Catalog;
            int currentDay = _yearOfAsh?.Timeline?.CurrentDay ?? 0;
            var visible = catalog?.Communiques?
                .Where(c => c.day <= currentDay)
                .OrderByDescending(c => c.day)
                .Take(4)
                .ToList();

            if (visible != null && visible.Count > 0)
            {
                foreach (var c in visible)
                {
                    evBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.TrFormat("ui.factions.events.day", "[Day {0:D2}] {1}", c.day, c.factionId), c.title, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                }
            }
            else
            {
                evBox.AddChild(AshfallUiHelpers.MakeDataRow(AshfallUiText.Tr("ui.factions.events.status_label", "STATUS"), AshfallUiText.Tr("ui.factions.events.none", "No diplomatic communiqués intercepted on local frequencies."), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            }
            _eventsContainer.AddChild(evCard);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            AddChild(AshfallUiHelpers.MakeBackdropOverlay());

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

            var title = AshfallUiHelpers.MakeTitle(AshfallUiText.Tr("ui.factions.title", "FACTIONS & WASTELAND DIPLOMACY"), Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(title);

            _statusSummary = AshfallUiHelpers.MakeMetadata(AshfallUiText.Tr("ui.factions.summary", "Monitor geopolitical standings, faction trust, trade specialization, claim boundaries, and diplomatic treaties."));
            _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
            _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(_statusSummary);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_overviewContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var factionsTitle = AshfallUiHelpers.MakeSectionHeader(AshfallUiText.Tr("ui.factions.section.factions", "KNOWN FACTION PROTOCOLS & ALLIANCES"));
            rootBox.AddChild(factionsTitle);

            _factionsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_factionsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var relTitle = AshfallUiHelpers.MakeSectionHeader(AshfallUiText.Tr("ui.factions.section.relations", "TREATIES, STANDING & DEBT OBLIGATIONS"));
            rootBox.AddChild(relTitle);

            _relationsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_relationsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var evTitle = AshfallUiHelpers.MakeSectionHeader(AshfallUiText.Tr("ui.factions.section.events", "RECENT FACTION COMMUNIQUES & DISPATCHES"));
            rootBox.AddChild(evTitle);

            _eventsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_eventsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.factions.close", "CLOSE DIPLOMACY [Esc]"), () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            rootBox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall(AshfallUiText.Tr("ui.factions.esc_hint", "[Esc] to close factions panel"));
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

            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
        public void Unbind() => Unbind(clearPresentation: true);

        private void Unbind(bool clearPresentation)
        {
            if (_muster != null)
                _muster.StateChanged -= RefreshView;
            if (_expansions != null)
                _expansions.StateChanged -= RefreshView;
            if (_branchCoordinator != null)
                _branchCoordinator.OnStateChanged -= RefreshView;
            if (_informantNetwork != null)
                _informantNetwork.StateChanged -= RefreshView;
            if (_yearOfAsh?.Warlord != null)
            {
                _yearOfAsh.Warlord.OnStateChanged -= RefreshView;
                _yearOfAsh.Warlord.OnTributeSettled -= HandleWarlordTributeSettled;
                _yearOfAsh.Warlord.OnTributeDemanded -= HandleWarlordTributeDemanded;
            }

            _factions = null;
            _trade = null;
            _muster = null;
            _expansions = null;
            _yearOfAsh = null;
            _branchCoordinator = null;
            _informantNetwork = null;
            _moralChoice = null;
            _collectorNote = string.Empty;
            HasGuildCard = false;

            if (clearPresentation)
            {
                AshfallUiHelpers.EmptyChildren(_overviewContainer);
                AshfallUiHelpers.EmptyChildren(_factionsContainer);
                AshfallUiHelpers.EmptyChildren(_relationsContainer);
                AshfallUiHelpers.EmptyChildren(_eventsContainer);
            }
        }

        public override void _Notification(int what)
        {
            if (what == NotificationPredelete)
                Unbind(clearPresentation: false);
            base._Notification(what);
        }

        public override void _ExitTree()
        {
            Unbind(clearPresentation: true);
            base._ExitTree();
        }
    }
}
