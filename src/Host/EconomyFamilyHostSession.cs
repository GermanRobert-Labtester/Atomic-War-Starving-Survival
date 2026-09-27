// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : EconomyFamilyHostSession
// Purpose      : PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 — wire the economy family
//                engines that no plan referenced: trade-route monopoly,
//                contraband quoting, chit purity assay, and black-market heat
//                attention. Each engine keeps its own concern; this session is
//                the composition root and persists the two stateful owners
//                under their own checksummed `economy_family` section.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class EconomyFamilySaveState
    {
        public int schema_version { get; set; } = 1;
        public TradeRouteMonopolySaveState? monopoly { get; set; }
        public BlackMarketHeatAttentionSaveState? heat { get; set; }
    }

    public static class EconomyFamilySaveStore
    {
        public const string FileName = "economy_family_save.json";
        public const string SectionName = "economy_family";
        private static readonly SaveStore<EconomyFamilySaveState> s_store =
            SaveStoreHub.Checksummed<EconomyFamilySaveState>(FileName, nameof(EconomyFamilySaveStore));
        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(EconomyFamilySaveState state) => s_store.CaptureBare(state);
        public static EconomyFamilySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(EconomyFamilySaveState state) => s_store.TrySave(state);
        public static EconomyFamilySaveState? TryLoad() => s_store.TryLoad();
    }

    public sealed class EconomyFamilyHostSession : HostSessionBase
    {
        public TradeRouteMonopolyEngine Monopoly { get; } = new();
        public BlackMarketHeatAttentionEngine Heat { get; } = new();
        public string LastEvent { get; private set; } = string.Empty;

        public EconomyFamilyHostSession()
        {

        }

        // ---- trade-route monopoly (stateful, persisted) ----
        public int GetMonopolyPremiumPermille(string routeId) => Monopoly.GetCurrentMonopolyPremiumPermille(routeId);
        public void RecordRouteDelivery(string routeId, string exclusiveGoodId, int units, int day)
        {
            Monopoly.RecordDelivery(routeId, exclusiveGoodId, units, day);
            LastEvent = $"Route delivery recorded: {units}x {exclusiveGoodId} on {routeId}.";
            RaiseStateChanged();
        }
        public void ProcessRouteRecovery(int day) { Monopoly.ProcessDailyRecovery(day); RaiseStateChanged(); }

        // ---- contraband quoting (pure) ----
        public BlackMarketTradeQuote QuoteContrabandBuy(string itemId, int qty, int baseValue, ContrabandClassification cls, int heatPermille)
            => BlackMarketContrabandEngine.QuoteBuy(itemId, qty, baseValue, cls, heatPermille);
        public BlackMarketTradeQuote QuoteContrabandSell(string itemId, int qty, int baseValue, ContrabandClassification cls, int heatPermille)
            => BlackMarketContrabandEngine.QuoteSell(itemId, qty, baseValue, cls, heatPermille);

        // ---- chit purity assay (pure) ----
        public ChitAssayResult EvaluateChitAssay(int amount, PurityTier tier, int merchantPerceptionPermille, uint seed, int nonce = 1)
            => ChitPurityAssayEngine.EvaluateAssay(amount, tier, merchantPerceptionPermille, seed, nonce);

        // ---- black-market heat attention (stateful, persisted) ----
        public int AddSyndicateHeat(string syndicateId, int delta, string reason, int day)
            => Heat.AddHeat(syndicateId, delta, reason, day);
        public BlackMarketAttentionBand EvaluateSyndicateBand(string syndicateId, int threshold = 100)
        {
            var record = Heat.GetOrCreateRecord(syndicateId, threshold);
            return Heat.EvaluateBand(record.CurrentHeat, record.HeatThreshold);
        }
        public void TickHeat(int day, int seed) { Heat.ProcessDailyTick(day, seed); RaiseStateChanged(); }

        public EconomyFamilySaveState CaptureState() => new EconomyFamilySaveState
        {
            monopoly = Monopoly.CaptureState(),
            heat = Heat.CaptureState()
        };

        public void RestoreState(EconomyFamilySaveState? state)
        {
            if (state == null) return;
            Monopoly.RestoreState(state.monopoly);
            Heat.RestoreState(state.heat);
            LastEvent = "Economy family state restored.";
            RaiseStateChanged();
        }
    }
}
