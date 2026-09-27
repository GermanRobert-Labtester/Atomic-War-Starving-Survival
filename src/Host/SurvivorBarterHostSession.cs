// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SurvivorBarterSaveStore
// Core State : Ashfall.Core.Economy.SurvivorBarterSaveState
// Host Caller: Main.SurvivorBarter
// Purpose    : Survivor barter host session & persistence. The Core system
//              remains the sole offer/trade/reputation/favor authority; the host
//              composes the authored rule catalog and the campaign day tick.
//              Item custody stays with Inventory; the system only records the
//              trade, reputation, and obligation facts.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class SurvivorBarterSaveStore
    {
        public const string FileName = "survivor_barter_save.json";
        public const string SectionName = "survivor_barter";

        private static readonly SaveStore<SurvivorBarterSaveState> s_store =
            SaveStoreHub.Checksummed<SurvivorBarterSaveState>(FileName, nameof(SurvivorBarterSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SurvivorBarterSaveState state) => s_store.CaptureBare(state);
        public static SurvivorBarterSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SurvivorBarterSaveState state) => s_store.TrySave(state);
        public static SurvivorBarterSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="SurvivorBarterSystem"/>.</summary>
    public sealed class SurvivorBarterHostSession : HostSessionBase
    {
        private readonly SurvivorBarterSystem _system;

        public SurvivorBarterSystem System => _system;
        public bool CatalogReady { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public SurvivorBarterHostSession(SurvivorBarterSaveState? state = null)
        {
            _system = new SurvivorBarterSystem();
            if (state != null) _system.RestoreState(state);
        }

        public static SurvivorBarterHostSession Create(SurvivorBarterSaveState? state = null) =>
            new SurvivorBarterHostSession(state);

        public bool LoadCatalog(string dataDirectory)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory)) return false;
            string path = Path.Combine(dataDirectory, "barter_rules.json");
            if (!File.Exists(path)) return false;
            try
            {
                _system.LoadCatalog(File.ReadAllText(path));
                CatalogReady = true;
                return true;
            }
            catch (Exception)
            {
                CatalogReady = false;
                return false;
            }
        }

        public string SetActiveRule(string ruleId)
        {
            return LoadRulesByName(ruleId);
        }

        private string LoadRulesByName(string ruleId)
        {
            if (string.IsNullOrWhiteSpace(ruleId)) return "Empty rule id.";
            bool ok = _system.SetRule(ruleId);
            LastEvent = ok ? $"Active barter rule: {ruleId}." : $"Unknown barter rule: {ruleId}.";
            RaiseStateChanged();
            return LastEvent;
        }

        public BarterOffer? CreateOffer(
            string offererId, string targetId, int currentDay,
            IEnumerable<string>? offeredItems = null, IEnumerable<string>? requestedItems = null) =>
            _system.CreateOffer(offererId, targetId, offeredItems, requestedItems, null, null, currentDay);

        public CompletedBarterTrade? AcceptOffer(string offerId, int currentDay)
        {
            var trade = _system.AcceptOffer(offerId, currentDay);
            if (trade != null) { LastEvent = $"Barter {offerId} accepted."; RaiseStateChanged(); }
            return trade;
        }

        public bool RejectOffer(string offerId)
        {
            bool ok = _system.RejectOffer(offerId);
            if (ok) { LastEvent = $"Barter {offerId} rejected."; RaiseStateChanged(); }
            return ok;
        }

        public bool RaiseDispute(string tradeId, string complainantId)
        {
            bool ok = _system.RaiseDispute(tradeId, complainantId);
            if (ok) { LastEvent = $"Dispute raised on {tradeId}."; RaiseStateChanged(); }
            return ok;
        }

        public TradeReputation GetReputation(string survivorA, string survivorB) =>
            _system.GetReputation(survivorA, survivorB);

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            LastEvent = "Barter obligations ticked.";
            RaiseStateChanged();
        }

        public int ActiveOfferCount => _system.CaptureState().Offers.Count;
        public int CompletedTradeCount => _system.CaptureState().CompletedTrades.Count;

        public SurvivorBarterSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(SurvivorBarterSaveState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored survivor barter state.";
            RaiseStateChanged();
        }

        public bool TrySave() => SurvivorBarterSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = SurvivorBarterSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
