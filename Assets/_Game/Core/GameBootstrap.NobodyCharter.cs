using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — host wiring. The social gate into the
    /// Crossing, the three Blocs' catalog, the Scalehouse Row locations, and
    /// the opening companions. Called from InitDeepLore after Currents +
    /// Holdfast. Stands alone when the sister packs are absent (reads their
    /// flags only if present).
    /// </summary>
    public partial class GameBootstrap
    {
        public VouchAccessSystem Vouch { get; private set; }
        public CrossingArbitrationSystem Arbitration { get; private set; }
        /// <summary>The single engine-agnostic ledger (§5.3) — Ashfall.Core, not a host twin.</summary>
        public LedgerDebtSystem Ledger { get; private set; }
        public NPC_OsranKell NPCOsranKell { get; private set; }
        public NPC_MattisCray NPCMattisCray { get; private set; }
        public NPC_DessaVane NPCDessaVane { get; private set; }
        public NPC_PerrinAshby NPCPerrinAshby { get; private set; }
        public NPC_IvoFenn NPCIvoFenn { get; private set; }
        public NPC_WynSabler NPCWynSabler { get; private set; }

        private void BootNobodyCharter()
        {
            Vouch = new VouchAccessSystem();
            Arbitration = new CrossingArbitrationSystem();
            Ledger = new LedgerDebtSystem();
            NPCOsranKell = new NPC_OsranKell();
            NPCMattisCray = new NPC_MattisCray();
            NPCDessaVane = new NPC_DessaVane();
            NPCPerrinAshby = new NPC_PerrinAshby();
            NPCIvoFenn = new NPC_IvoFenn();
            NPCWynSabler = new NPC_WynSabler();

            var osran = CharactersCatalogLoader.GetById(CrossingIds.Npcs.OsranKell);
            NPCOsranKell.Initialise(osran != null ? osran.display_name : "Osran Kell");
            var mattis = CharactersCatalogLoader.GetById(CrossingIds.Npcs.MattisCray);
            NPCMattisCray.Initialise(mattis != null ? mattis.display_name : "Mattis Cray");
            var dessa = CharactersCatalogLoader.GetById(CrossingIds.Npcs.DessaVane);
            NPCDessaVane.Initialise(dessa != null ? dessa.display_name : "Dessa Vane");
            var perrin = CharactersCatalogLoader.GetById(CrossingIds.Npcs.PerrinAshby);
            NPCPerrinAshby.Initialise(perrin != null ? perrin.display_name : "Perrin Ashby");
            var ivo = CharactersCatalogLoader.GetById(CrossingIds.Npcs.IvoFenn);
            NPCIvoFenn.Initialise(ivo != null ? ivo.display_name : "Ivo Fenn");
            var wyn = CharactersCatalogLoader.GetById(CrossingIds.Npcs.WynSabler);
            NPCWynSabler.Initialise(wyn != null ? wyn.display_name : "Wyn Sabler");

            InitialiseBackerPool();
            MergeCrossingLocations();
            if (GeneratedMap != null)
                CrossingMapSeeder.Attach(GeneratedMap, CrossingLocationsCatalogLoader.Load());
            MergeCrossingItems();
            LoadCrossingQuests();

            if (ExpeditionSystem != null)
                ExpeditionSystem.SetVouchAccessSystem(Vouch);

            WireNobodyCharterEvents();

            // Event-driven systems — vouch/NPC state only moves through
            // host action calls, not an unattended background tick.
            _registry.RegisterEventDriven("vouch_access_system");
            _registry.RegisterEventDriven("crossing_arbitration_system");
            _registry.RegisterEventDriven("ledger_debt_system");
            _registry.RegisterEventDriven("npc_osran_kell");
            _registry.RegisterEventDriven("npc_mattis_cray");
            _registry.RegisterEventDriven("npc_dessa_vane");
            _registry.RegisterEventDriven("npc_perrin_ashby");
            _registry.RegisterEventDriven("npc_ivo_fenn");
            _registry.RegisterEventDriven("npc_wyn_sabler");

            // The Ostrowski rumour is the pack's front door: it sets
            // exp_nobodys_charter_unlocked on the vouch card's min_day —
            // never at boot.
            _registry.RegisterDaily("nobody_charter_rumour", MaybeStartNobodyCharterRumour);

            GameLog.Log("[GameBootstrap] Nobody's Charter booted: VouchAccess, Arbitration, Ledger, Scale, Scalehouse Row, Osran, Mattis, Dessa, Perrin, Ivo, Wyn.");
        }

        private void MergeCrossingLocations()
        {
            if (_locationCatalog == null) return;
            int n = CrossingLocationsCatalogLoader.ApplyToCatalog(_locationCatalog);
            if (n > 0)
                GameLog.Log("[GameBootstrap] Crossing locations applied: " + n);
        }

        private void MergeCrossingItems()
        {
            if (_itemCatalog == null) return;
            var defs = CrossingItemsCatalogLoader.MaterialiseAll();
            int added = 0;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                if (_itemCatalog.GetById(d.id) != null) continue;
                _itemCatalog.items.Add(d);
                added++;
            }
            if (added > 0)
                GameLog.Log("[GameBootstrap] Crossing items merged: " + added);
        }

        private void LoadCrossingQuests()
        {
            var cards = CrossingQuestCatalogLoader.Load();
            CrossingQuests = cards != null ? cards : new List<AtomicWar._Game.Data.CrossingQuestEntry>();
            if (CrossingQuests.Count > 0)
                GameLog.Log("[GameBootstrap] Crossing quests registered: " + CrossingQuests.Count);
        }

        /// <summary>Cached quest cards from crossing_quests.json (see CrossingQuestCatalogLoader).</summary>
        public List<AtomicWar._Game.Data.CrossingQuestEntry> CrossingQuests { get; private set; }

        /// <summary>Look up a registered Crossing quest card by id (null-safe).</summary>
        public AtomicWar._Game.Data.CrossingQuestEntry GetCrossingQuest(string questId)
        {
            if (CrossingQuests == null || string.IsNullOrEmpty(questId)) return null;
            for (int i = 0; i < CrossingQuests.Count; i++)
            {
                var q = CrossingQuests[i];
                if (q != null && q.id == questId) return q;
            }
            return null;
        }

        private void InitialiseBackerPool()
        {
            // Named stallholders — the pool from which Standing rulings draw backers.
            // Not Utility AI agents; scripted micro-dispute participants only.
            var backers = new List<BackerDef>
            {
                new BackerDef { id = "backer_hal_the_riveter", displayName = "Hal (the Riveter)",
                    wants = "stall_territory", willNot = "vote_against_own_stall", principled = false },
                new BackerDef { id = "backer_marta_saltline", displayName = "Marta (Saltline)",
                    wants = "salt_trade_fairness", willNot = "side_with_weight_rigger", principled = true },
                new BackerDef { id = "backer_jorn_the_deaf", displayName = "Jorn (the Deaf)",
                    wants = "quiet_access", willNot = "back_a_liar", principled = true },
                new BackerDef { id = "backer_yelka_fence", displayName = "Yelka (the Fence)",
                    wants = "smuggling_access", willNot = "back_the_compact_publicly", principled = false },
                new BackerDef { id = "backer_old_petr", displayName = "Old Petr",
                    wants = "respect", willNot = "outlive_a_bad_ruling", principled = true },
                new BackerDef { id = "backer_suki_weaver", displayName = "Suki (the Weaver)",
                    wants = "cloth_prices_stable", willNot = "back_a_forfeit_she_cant_see", principled = false },
                new BackerDef { id = "backer_tomas_lean", displayName = "Tomas (the Lean)",
                    wants = "grain_access", willNot = "sign_twice", principled = false },
                new BackerDef { id = "backer_nia_watch", displayName = "Nia (Watch)",
                    wants = "gate_security", willNot = "back_a_violence", principled = true },
                new BackerDef { id = "backer_ren_carter", displayName = "Ren (the Carter)",
                    wants = "road_access", willNot = "back_an_unweighed_claim", principled = false },
                new BackerDef { id = "backer_deserter_contact", displayName = "The Deserter",
                    wants = "passage_guarantee", willNot = "be_named_in_public", principled = true },
                new BackerDef { id = "backer_greta_anne", displayName = "Greta-Anne",
                    wants = "annex_beds_open", willNot = "starve_out_refugees", principled = true },
                new BackerDef { id = "backer_pike_bones", displayName = "Pike (Bones)",
                    wants = "medical_trade", willNot = "back_an_infection_risk", principled = false },
            };
            Arbitration.LoadBackerPool(backers);
        }

        private void WireNobodyCharterEvents()
        {
            if (Vouch != null)
            {
                Vouch.OnVouchGranted += npcId =>
                {
                    SaveSystem?.SetWorldFlag(CrossingIds.Flags.VouchedClean, true);
                    GrantCrossingVouchRewardOnce();
                };
                Vouch.OnVouchBurned += () =>
                {
                    SaveSystem?.SetWorldFlag(CrossingIds.Flags.VouchedClean, false);
                    SaveSystem?.SetWorldFlag(CrossingIds.Flags.VouchBurned, true);
                };
                Vouch.OnAccessSoftened += () =>
                    SaveSystem?.SetWorldFlag(CrossingIds.Flags.AccessSoftened, true);
            }

            if (Arbitration != null)
            {
                Arbitration.OnRulingMade += ruling =>
                {
                    bool honest = ruling.shape == RulingShape.Honest;
                    SaveSystem?.SetWorldFlag(CrossingIds.Flags.StandingHonest, honest);
                    SaveSystem?.SetWorldFlag(CrossingIds.Flags.StandingRigged, !honest);
                    GameLog.Log("[Nobody's Charter] Standing ruling held: " + ruling.topic
                        + " (" + (honest ? "honest" : "rigged") + ", " + ruling.backers.Count + " backers)");
                };
                Arbitration.OnRulingOverturned += ruling =>
                    GameLog.Log("[Nobody's Charter] Standing ruling overturned: " + ruling.topic);
                Arbitration.OnBribeRefused += (backerId, topic) =>
                    GameLog.Log("[Nobody's Charter] " + backerId + " refuses to be bought on " + topic
                        + " — and says so where the board is. The ruling is marked.");
            }

            if (Ledger != null)
            {
                Ledger.OnContractSigned += contract =>
                    GameLog.Log("[Nobody's Charter] Contract signed: " + contract.debtorId
                        + " (principal: " + contract.principal + ", forfeit: " + contract.forfeit + ")");
                Ledger.OnContractPaid += contract =>
                    GameLog.Log("[Nobody's Charter] Contract paid: " + contract.debtorId);
                Ledger.OnContractRenegotiated += contract =>
                    GameLog.Log("[Nobody's Charter] Contract renegotiated: " + contract.debtorId
                        + " (term " + contract.termDays + " days, forfeit: " + contract.forfeit + ")");
                Ledger.OnForfeitTriggered += contract =>
                    GameLog.Log("[Nobody's Charter] Forfeit due: " + contract.forfeit
                        + " (debtor " + contract.debtorId + ")");
                Ledger.OnLedgerTampered += () =>
                    GameLog.Log("[Nobody's Charter] Ledger tamper attempt. Ivo's records do not lie.");
            }

            if (NPCMattisCray != null)
            {
                NPCMattisCray.OnBurned += _ =>
                    GameLog.Log("[Nobody's Charter] Mattis Cray will not vouch again this playthrough.");
            }
        }

        // ── Host-facing action API (quests / EventRunner choices call these) ──
        /// <summary>True when the player may currently pass the Crossing's viaduct gate.</summary>
        public bool GateAllowsCrossing() => Vouch != null && Vouch.HasAccess;

        /// <summary>
        /// Grant (or re-grant) access at the Crossing on a vouching NPC's word.
        /// When <paramref name="lastResort"/> is true and the vouching NPC is
        /// Mattis, his one-time last-resort vouch is consumed.
        /// </summary>
        public bool TryVouchAtCrossing(string npcId, bool lastResort = false)
        {
            if (Vouch == null) return false;
            bool granted = Vouch.GrantVouch(npcId, lastResort);
            if (granted && lastResort && NPCMattisCray != null && npcId == CrossingIds.Npcs.MattisCray)
                NPCMattisCray.GiveVouch();
            return granted;
        }

        /// <summary>
        /// The player betrayed their vouching trust; the gate re-closes. When
        /// the burned sponsor was Mattis Cray, his name is burned with it —
        /// he will not vouch again this playthrough (Sprint 0 coupling).
        /// </summary>
        public bool BurnCrossingVouch()
        {
            if (Vouch == null) return false;
            string sponsor = Vouch.VouchedBy;
            bool burned = Vouch.BurnVouch();
            if (burned && sponsor == CrossingIds.Npcs.MattisCray)
            {
                NPCMattisCray?.BurnMattis();
                GameLog.Log("[Nobody's Charter] Mattis Cray's name was burned at the gate. He will not vouch again.");
            }
            return burned;
        }

        /// <summary>
        /// After the opening arc the player's own name becomes sufficient.
        /// Requires a prior name on the ledger (granted or burned) — you
        /// cannot soften a gate that was never opened through a name.
        /// </summary>
        public bool SoftenCrossingAccess() => Vouch != null && Vouch.SoftenAccess();

        // ── Standing / Arbitration host API ─────────────────────────────

        /// <summary>Call a Standing on a topic. Returns false if already held/overturned.</summary>
        public bool CallCrossingStanding(string topic, int currentDay)
            => Arbitration?.CallStanding(topic, currentDay) ?? false;

        /// <summary>A backer declares support for the topic's ruling.</summary>
        public bool DeclareCrossingBacker(string topic, string backerId)
            => Arbitration?.DeclareBacker(topic, backerId) ?? false;

        /// <summary>Overturn a held ruling with 3+ counter-backers.</summary>
        public bool OverturnCrossingRuling(string topic, IReadOnlyList<string> counterBackerIds)
            => Arbitration?.OverturnRuling(topic, counterBackerIds) ?? false;

        /// <summary>
        /// Attempt to buy a backer's support on a pending Standing. A principled
        /// backer refuses publicly (a mark); a non-principled backer accepts and
        /// the ruling will hold Rigged, never Honest.
        /// </summary>
        public BribeResult TryBribeCrossingBacker(string topic, string backerId)
            => Arbitration?.TryBribeBacker(topic, backerId) ?? BribeResult.Invalid;

        /// <summary>True when the topic's ruling is currently held (honest, 3+ backers).</summary>
        public bool IsCrossingRulingHeld(string topic)
            => Arbitration?.IsRulingHeld(topic) ?? false;

        /// <summary>True when the topic's ruling has been overturned.</summary>
        public bool IsCrossingRulingOverturned(string topic)
            => Arbitration?.IsRulingOverturned(topic) ?? false;

        // ── Ledger / Debt host API (§5.3, Ashfall.Core single source) ───

        /// <summary>One reading of a contract at the Underwrite Hall. Creates/updates the draft.</summary>
        public bool PresentCrossingContract(string debtorId, float principal, int termDays, float rate, string forfeit)
            => Ledger?.PresentContract(debtorId, principal, termDays, rate, forfeit) ?? false;

        /// <summary>Sign. Requires the contract to have been read twice (§5.3).</summary>
        public bool SignCrossingContract(string debtorId, int currentDay)
            => Ledger?.SignContract(debtorId, currentDay) ?? false;

        /// <summary>Pay off a contract in full (also the honoured path after a forfeit is due).</summary>
        public bool PayCrossingContract(string debtorId, int currentDay)
            => Ledger?.PayContract(debtorId, currentDay) ?? false;

        /// <summary>
        /// Renegotiate terms. On signed ink this is only allowed at term end.
        /// A contested renegotiation requires a FRESH Standing on the given
        /// topic — held honestly AND called within
        /// LedgerDebtSystem.StandingFreshDays of the current day (bible §5.3:
        /// "requires a fresh Standing if contested"). The gate itself is
        /// enforced inside the core LedgerDebtSystem; this wrapper only
        /// composes it with the arbitration board.
        /// </summary>
        public bool RenegotiateCrossingContract(string debtorId, float newPrincipal, int newTermDays,
            float newRate, string newForfeit, bool contested = false, string standingTopic = null, int currentDay = -1)
        {
            return Ledger?.RenegotiateContract(debtorId, newPrincipal, newTermDays, newRate, newForfeit,
                contested, contested ? () => IsCrossingStandingFresh(standingTopic, currentDay) : null) ?? false;
        }

        /// <summary>
        /// Fresh Standing check composed for the core ledger gate: the topic's
        /// ruling is held honestly and was called within StandingFreshDays of
        /// currentDay. A stale or rigged Standing authorises nothing.
        /// </summary>
        private bool IsCrossingStandingFresh(string topic, int currentDay)
        {
            if (Arbitration == null || string.IsNullOrEmpty(topic)) return false;
            if (!Arbitration.IsRulingHeld(topic)) return false;
            var ruling = Arbitration.GetRuling(topic);
            if (ruling == null) return false;
            if (currentDay < 0) return false; // freshness needs the day — no day, no gate pass
            return currentDay - ruling.dayCalled <= LedgerDebtSystem.StandingFreshDays;
        }

        /// <summary>Daily tick — terms run down; forfeits come due at zero.</summary>
        public void TickCrossingLedger(int currentDay)
            => Ledger?.TickDaily(currentDay);

        /// <summary>One strike on the ledger per playthrough. Ivo's records do not lie.</summary>
        public bool TamperCrossingLedger()
            => Ledger?.TamperLedger() ?? false;

        // ── Sprint 0: the rumour + the vouch reward ───────────────────────

        /// <summary>
        /// Start the Ostrowski rumour (sets exp_nobodys_charter_unlocked).
        /// Idempotent; returns false if already unlocked. Never called at boot.
        /// </summary>
        public bool StartNobodyCharterRumour(int day)
        {
            if (SaveSystem != null && SaveSystem.GetWorldFlag(CrossingIds.Flags.ExpansionUnlocked)) return false;
            SaveSystem?.SetWorldFlag(CrossingIds.Flags.ExpansionUnlocked, true);
            GameLog.Log("[Nobody's Charter] Day " + day
                + ": Ostrowski will sell a sketch of the approach to the Crossing. He will not walk there himself. "
                + "\"I sold them a map once. That was the whole transaction. I'd like it to stay that way.\"");
            return true;
        }

        private void MaybeStartNobodyCharterRumour(int day)
        {
            if (SaveSystem != null && SaveSystem.GetWorldFlag(CrossingIds.Flags.ExpansionUnlocked)) return;
            var vouch = GetCrossingQuest(CrossingIds.Quests.TheVouch);
            if (vouch == null) return;
            if (day < vouch.min_day) return;
            StartNobodyCharterRumour(day);
        }

        /// <summary>
        /// "A Name at the Gate" reward: the vouch token plus the knowledge
        /// that names are the currency. Once per playthrough, on the first
        /// vouch granted, regardless of sponsor.
        /// </summary>
        private void GrantCrossingVouchRewardOnce()
        {
            if (SaveSystem != null && SaveSystem.GetWorldFlag(CrossingIds.Flags.VouchRewarded)) return;
            GiveCrossingItem(CrossingIds.Items.VouchToken, 1);
            SaveSystem?.SetWorldFlag(CrossingIds.Knowledge.TheVouch, true);
            SaveSystem?.SetWorldFlag(CrossingIds.Flags.VouchRewarded, true);
            GameLog.Log("[Nobody's Charter] Vouch entered on the crossing ledger. Vouch token granted.");
        }

        private void GiveCrossingItem(string itemId, int count)
        {
            if (Inventory == null || string.IsNullOrEmpty(itemId) || count <= 0) return;
            var def = _itemCatalog != null ? _itemCatalog.GetById(itemId) : null;
            if (def == null)
            {
                GameLog.LogWarning("[Nobody's Charter] GiveItem skipped — unknown id " + itemId);
                return;
            }
            Inventory.Add(def, count);
        }
    }
}