// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the black market (Plan 211). Loads the
    /// inventory catalog, binds the canonical market (the ONLY price source)
    /// and the canonical bounty authority, refreshes daily-persistent stock
    /// from the economy RNG stream, and persists state. No rules here.
    /// </summary>
    public sealed class BlackMarketHostSession
    : HostSessionBase
    {
        public BlackMarketSystem System { get; }

        /// <summary>
        /// Canonical faction bounty authority composed for underworld
        /// escalation; its state persists via the black_market section's
        /// additive snapshot (§171.18) until a dedicated store exists.
        /// </summary>
        public FactionBountySystem Bounties { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public BlackMarketHostSession(BlackMarketSystem system)
            : this(system, new FactionBountySystem())
        {
        }

        public BlackMarketHostSession(BlackMarketSystem system, FactionBountySystem bounties)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            Bounties = bounties ?? throw new ArgumentNullException(nameof(bounties));
            System.BindFactionBountySystem(Bounties);
            System.OnContactDiscovered += id =>
            {
                LastEvent = $"Underworld contact made: {id}";
                RaiseStateChanged();
            };
            System.OnStockRefreshed += (id, day, lines) =>
            {
                LastEvent = $"Syndicate stock refreshed ({lines} lines, day {day})";
                RaiseStateChanged();
            };
            System.OnDebtIssued += debt =>
            {
                LastEvent = $"Loan taken from {debt.syndicateId}: {debt.principalUnits:0} due day {debt.dueDay}";
                RaiseStateChanged();
            };
            System.OnDebtRepaid += (debt, amount) =>
            {
                LastEvent = $"Repaid {amount:0} to {debt.syndicateId}";
                RaiseStateChanged();
            };
            System.OnDebtOverdue += debt =>
            {
                LastEvent = $"DEBT OVERDUE: {debt.syndicateId} (day {debt.dueDay} passed)";
                RaiseStateChanged();
            };
            System.OnBountyPlaced += debt =>
            {
                LastEvent = $"A bounty is out. The {debt.syndicateId} remember.";
                RaiseStateChanged();
            };
            System.OnStateChanged += _ => RaiseStateChanged();
        }

        /// <summary>
        /// Compose the full session: catalog + canonical market + canonical
        /// bounty authority + persisted state. The market is REQUIRED — the
        /// black market never computes its own item values.
        /// </summary>
        public static BlackMarketHostSession Create(string dataDir, MarketSystem market)
        {
            if (market == null) throw new ArgumentNullException(nameof(market));
            var system = new BlackMarketSystem();
            var session = new BlackMarketHostSession(system);

            var load = BlackMarketInventoryCatalogLoader.Load(
                dataDir ?? string.Empty, new FileSystemIO(), new SystemTextJsonSerializer());
            if (!load.HasErrors && load.Entries.Count > 0)
            {
                system.BindCatalog(BlackMarketInventoryCatalogLoader.ToCatalog(load));
                system.BindMarket(market);
            }
            else if (load.HasErrors)
            {
                session.LastEvent = "Black-market catalog rejected: " + load.Errors[0];
            }

            var saved = BlackMarketSaveStore.TryLoad();
            if (saved != null)
            {
                system.RestoreState(saved);   // bounty snapshot rebinds into Bounties
                system.BindMarket(market);
                session.LastEvent = "Black market restored from save.";
            }
            return session;
        }

        /// <summary>
        /// Advance one day: refresh discovered syndicates' stock snapshots
        /// from the black_market_stock RNG stream, then run the debt/heat
        /// tick. Called by the `underworld_market` day owner (phase 4, after
        /// the debt-ledger tick). Same-day reopen never rerolls — the Core
        /// refresh guard handles that.
        /// </summary>
        public void TickDay(int day, ISeededRng stockRng)
        {
            foreach (var contactId in System.DiscoveredContacts)
                System.EnsureStockSnapshot(contactId, day, stockRng);
            System.TickDaily(day);
        }

        public string StatusLine()
        {
            var contacts = System.DiscoveredContacts;
            int activeDebts = 0;
            foreach (var d in System.State.debts)
                if (d != null && d.status == UnderworldDebtRecord.StatusActive) activeDebts++;
            return contacts.Count == 0
                ? "Black market: no known contacts."
                : $"Black market: {contacts.Count} contacts · {activeDebts} active debts · last: {LastEvent}";
        }

        // ── Save / Load ──────────────────────────────────────────────

        public BlackMarketState CaptureSave() => System.CaptureState();
        public void RestoreSave(BlackMarketState state) => System.RestoreState(state);
    }
}
