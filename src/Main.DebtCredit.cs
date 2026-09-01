using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan IV — ledger debt consequences & trade credit, host wiring.
    /// The bridge maps dispatcher requests onto the canonical live systems
    /// (YearOfAsh faction war, Muster iron raiders, the shared shelter
    /// inventory); the coordinator turns insufficient-funds trade refusals
    /// into explicit, catalog-driven credit offers. Main owns the composition;
    /// every rule stays in Ashfall.Core.
    /// </summary>
    public partial class Main : Control
    {
        private DebtConsequenceHostBridge? _debtBridge;
        private TradeCreditCoordinator? _tradeCredit;
        private bool _debtBridgeDirty;
        private bool _debtIntegrationBuilding;

        /// <summary>The debtor who signs shelter credit: the player survivor.</summary>
        private string DebtDebtorId => _holdfastRuntime?.PlayerSurvivorId ?? "survivor_dr_sarah_chen";

        private int DebtCampaignDay() => _core != null ? _core.Clock.Day : _simDay;

        /// <summary>
        /// Idempotent composition of the debt-consequence integration. The
        /// expansion session owns the ledger/catalog/dispatcher/embargo ledger;
        /// this wires the host authorities exactly once and binds the embargo
        /// query shared by trade and credit. Reentrant-safe: the inner call
        /// made while the authority graph is being built returns immediately.
        /// </summary>
        private void EnsureDebtConsequenceIntegration()
        {
            if (_debtBridge != null || _debtIntegrationBuilding)
            {
                _holdfastTerminal?.BindCredit(_tradeCredit);
                return;
            }
            if (_expansions == null || _expansions.DebtDispatcher == null) return;

            _debtIntegrationBuilding = true;
            try
            {
                SetupYearOfAsh();
                SetupMuster();
                SetupInventory();
                SetupHoldfastRuntime();

                var dispatcher = _expansions.DebtDispatcher;
                dispatcher.SetDayProvider(DebtCampaignDay);

                _debtBridge = new DebtConsequenceHostBridge(
                    dispatcher,
                    _yearOfAsh.FactionWar,
                    _expansions.Embargoes,
                    DebtCampaignDay,
                    new GodotLog(),
                    ironRaiders: _muster.IronRaiders,
                    tryRemoveItems: TryRemoveShelterItems,
                    countItem: CountShelterItem,
                    selectLaborSurvivor: () => DebtDebtorId);
                _debtBridge.OnStateChanged += () =>
                {
                    _debtBridgeDirty = true;
                    _expansionHubDirty = true;
                };

                // One embargo query, two consumers: the terminal's trade session and
                // the credit coordinator. Credit can never bypass what trade cannot.
                _holdfastRuntime.Trade.EmbargoQuery =
                    factionId => _expansions.Embargoes.IsEmbargoed(factionId, DebtCampaignDay());

                _tradeCredit = new TradeCreditCoordinator(
                    _expansions.Ledger,
                    _expansions.DebtCatalog!,
                    _expansions.Embargoes,
                    DebtCampaignDay,
                    GrantPrincipal,
                    DebtDebtorId,
                    factionWar: _yearOfAsh.FactionWar,
                    revokeItems: RevokePrincipal,
                    log: new GodotLog());
            }
            finally
            {
                _debtIntegrationBuilding = false;
            }
            _holdfastTerminal?.BindCredit(_tradeCredit);
        }

        // ── Shelter inventory authority delegates ─────────────────────

        private bool GrantPrincipal(string itemId, int quantity)
        {
            if (_inventory == null) return false;
            string canonical = ItemAliases.ToCanonical(itemId);
            var def = _inventory.Catalog.Get(canonical);
            return def != null
                ? _inventory.Inventory.Add(def, quantity)
                : _inventory.Inventory.AddById(canonical, quantity);
        }

        private void RevokePrincipal(string itemId, int quantity)
        {
            if (_inventory == null) return;
            _inventory.Inventory.RemoveById(ItemAliases.ToCanonical(itemId), quantity);
        }

        private bool TryRemoveShelterItems(string itemId, int quantity)
        {
            if (_inventory == null) return false;
            return _inventory.Inventory.RemoveById(ItemAliases.ToCanonical(itemId), quantity);
        }

        private int CountShelterItem(string itemId)
        {
            if (_inventory == null) return 0;
            return _inventory.Inventory.CountById(ItemAliases.ToCanonical(itemId));
        }

        /// <summary>Phase-4 day owner: debt ages in the real campaign and the
        /// bounded labor windows close. Forfeits dispatch through the bridge into
        /// the canonical faction/raid/inventory systems.</summary>
        private sealed class DebtLedgerDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DebtLedgerDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupExpansions();
                if (_m._expansions == null || _m._expansions.DebtDispatcher == null) return;
                _m.EnsureDebtConsequenceIntegration();
                _m._expansions.Ledger.TickDaily(day);
                _m._debtBridge?.TickDaily(day);
                events.Add(new DayStateChangeEvent("debt_ledger_ticked", "debt_ledger", null, null, day));
            }
        }
    }
}
