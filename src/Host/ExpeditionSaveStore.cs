// ============================================================================
// Save Store : ExpeditionSaveStore
// Core State : Ashfall.Core.Expeditions.ExpeditionAggregateState
// Host Caller: Main.Expeditions / ExpeditionHostSession
// Purpose    : Active expedition sorties, field loot, and the vehicle garage
// ============================================================================
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Expedition save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). The section
    /// payload is the expedition aggregate (active sorties + vehicle garage);
    /// the decode also accepts the two legacy shapes — the pre-aggregate
    /// <c>{ State: [ ... ] }</c> envelope and the older bare list — so every
    /// expedition save ever written still loads.
    /// </summary>
    public static class ExpeditionSaveStore
    {
        public const string FileName = "expedition_save.json";
        public const string SectionName = "expedition";

        private static readonly SaveStore<ExpeditionAggregateState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ExpeditionSaveStore),
            EncodeAggregate,
            DecodeAggregate);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the aggregate to JSON without writing to disk.</summary>
        public static string TryCapture(ExpeditionAggregateState aggregate) => s_store.CaptureBare(aggregate);

        /// <summary>Restore the aggregate from JSON without reading from disk.</summary>
        public static ExpeditionAggregateState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ExpeditionAggregateState aggregate) => s_store.TrySave(aggregate);

        public static ExpeditionAggregateState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ExpeditionAggregateState aggregate) => s_store.CapturePersisted(aggregate);

        private static string EncodeAggregate(ExpeditionAggregateState aggregate, IJsonSerializer json)
            => ExpeditionAggregateCodec.Encode(aggregate, json);

        private static ExpeditionAggregateState? DecodeAggregate(string raw, IJsonSerializer json)
            => ExpeditionAggregateCodec.Decode(raw, json);
    }
}
