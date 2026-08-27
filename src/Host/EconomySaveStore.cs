// ============================================================================
// Save Store : EconomySaveStore
// Core State : Ashfall.Core.Economy.MarketState
// Host Caller: Main.Economy / EconomyHostSession
// Purpose    : Wasteland market commodity prices, price shocks, supply-demand, and barter balances
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side integrity envelope: the sim contract (MarketState) stays core
    /// versioned state; the checksum lives here so tampered saves are refused.
    /// The checksum hashes the STATE directly (not the envelope) and the
    /// fields serialize Checksum-first — both preserved by the delegates below.
    /// </summary>
    public class EconomySaveEnvelope
    {
        public string Checksum = string.Empty;
        public MarketState State;
    }

    /// <summary>
    /// Economy (market port) save persistence — façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor): path
    /// resolution, atomic write, and error handling live in the service;
    /// envelope shape, state-hash, and the guarded legacy bare migration stay
    /// in Economy-specific delegates.
    /// </summary>
    public static class EconomySaveStore
    {
        public const string FileName = "economy_save.json";
        public const string SectionName = "economy";

        private static readonly SaveStore<MarketState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(EconomySaveStore),
            EncodeState,
            DecodeState);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MarketState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MarketState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MarketState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MarketState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(MarketState state) => s_store.TrySave(state);

        public static bool TrySave(MarketState state, string path) => s_store.TrySave(state, path);

        public static MarketState? TryLoad() => s_store.TryLoad();

        public static MarketState? TryLoad(string path) => s_store.TryLoad(path);

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(MarketState state) => s_store.CapturePersisted(state);

        private static string EncodeState(MarketState state, IJsonSerializer json)
        {
            var envelope = new EconomySaveEnvelope
            {
                Checksum = SaveChecksum.Compute(state),
                State = state
            };
            return json.Serialize(envelope);
        }

        private static MarketState? DecodeState(string raw, IJsonSerializer json)
        {
            var envelope = json.Deserialize<EconomySaveEnvelope>(raw);
            if (envelope != null && envelope.State != null)
            {
                if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                // Tamper gate: recompute over the state; mismatch refuses the save.
                if (!string.Equals(SaveChecksum.Compute(envelope.State), envelope.Checksum,
                        StringComparison.Ordinal))
                    return null;
                return envelope.State;
            }
            // Legacy migration: a bare MarketState (pre-checksum store shape)
            // has no envelope; accept it so an upgrade never silently loses
            // the economy. Legacy saves carry no checksum by definition.
            var legacy = json.Deserialize<MarketState>(raw);
            if (legacy != null && !string.IsNullOrEmpty(legacy.systemId))
                return legacy;
            return null;
        }
    }
}
