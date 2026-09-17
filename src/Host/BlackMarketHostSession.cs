// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;

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

        private BlackMarketSettlementService? _settlement;
        private Func<int>? _dayProvider;

        /// <summary>Exactly one result event per player command attempt.</summary>
        public event Action<BlackMarketActionResult>? ActionCompleted;

        public bool ActionsAvailable => _settlement != null && _dayProvider != null;
        public long WalletValue => _settlement?.WalletValue ?? 0L;

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
        /// Bind the existing wallet and inventory authorities. The host owns
        /// no currency or goods state; the coordinator is stateless.
        /// </summary>
        public void BindSettlementOwners(HoldfastTradeSession wallet,
            Ashfall.Core.Inventory.Inventory inventory, ItemCatalog items, Func<int> dayProvider)
        {
            _settlement = new BlackMarketSettlementService(System, wallet, inventory, items);
            _dayProvider = dayProvider ?? throw new ArgumentNullException(nameof(dayProvider));
        }

        public int InventoryCountForEntry(string entryId)
        {
            var entry = System.Catalog.FindEntry(entryId);
            return entry == null ? 0 : (_settlement?.InventoryCount(entry.item_id) ?? 0);
        }

        public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity) =>
            _settlement?.PreviewBuy(syndicateId, entryId, quantity, CurrentDay()) ?? UnboundPreview("buy");

        public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity) =>
            _settlement?.PreviewSell(syndicateId, entryId, quantity, CurrentDay()) ?? UnboundPreview("sell");

        public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int durationDays) =>
            _settlement?.PreviewLoan(syndicateId, units, CurrentDay(), durationDays) ?? UnboundPreview("take_loan");

        public BlackMarketActionPreview PreviewRepay(string debtId, long units) =>
            _settlement?.PreviewRepay(debtId, units, CurrentDay()) ?? UnboundPreview("repay");

        public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity) =>
            Publish(_settlement?.Buy(syndicateId, entryId, quantity, CurrentDay()) ?? UnboundResult("buy"));

        public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity) =>
            Publish(_settlement?.Sell(syndicateId, entryId, quantity, CurrentDay()) ?? UnboundResult("sell"));

        public BlackMarketActionResult TakeLoan(string syndicateId, long units, int durationDays) =>
            Publish(_settlement?.TakeLoan(syndicateId, units, CurrentDay(), durationDays) ?? UnboundResult("take_loan"));

        public BlackMarketActionResult Repay(string debtId, long units) =>
            Publish(_settlement?.Repay(debtId, units, CurrentDay()) ?? UnboundResult("repay"));

        private int CurrentDay() => _dayProvider?.Invoke() ?? 0;

        private BlackMarketActionResult Publish(BlackMarketActionResult result)
        {
            LastEvent = result.Message;
            ActionCompleted?.Invoke(result);
            return result;
        }

        private static BlackMarketActionPreview UnboundPreview(string actionId) =>
            new BlackMarketActionPreview(false, actionId, string.Empty, string.Empty, string.Empty,
                string.Empty, 0, 0, 0, "settlement_unbound", "Trade settlement is not available.");

        private static BlackMarketActionResult UnboundResult(string actionId) =>
            new BlackMarketActionResult(false, actionId, string.Empty, string.Empty, string.Empty,
                string.Empty, 0, 0, 0, 0, 0, "settlement_unbound", "Trade settlement is not available.");

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
