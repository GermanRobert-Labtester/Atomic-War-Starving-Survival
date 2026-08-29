// ============================================================================
// Save Store : SurvivorFateSaveStore
// Core State : Ashfall.Core.Survivors.SurvivorFateSave (wraps SurvivorFateSaveState)
// Host Caller: Main.SurvivorFate
// Purpose    : Unified survivor-death ledger — one immutable fate record per
//              deceased survivor (cause, day, source), idempotency authority
//              for the death cascade, and the memorial/archive record that
//              outlives a terminal campaign.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists the survivor-fate ledger under user://survivor_fate_save.json —
    /// façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub, codec
    /// flavor). SurvivorFateSave is a self-checksummed type (the checksum is a
    /// field of the state itself), so encode/decode stamp and verify it
    /// directly; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class SurvivorFateSaveStore
    {
        public const string FileName = "survivor_fate_save.json";
        public const string SectionName = "survivor_fate";

        private static readonly SaveStore<SurvivorFateSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SurvivorFateSaveStore),
            EncodeSave,
            DecodeSave);

        public static string SavePath => s_store.SavePath;

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(SurvivorFateSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static SurvivorFateSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static bool TrySave(SurvivorFateSave save) => s_store.TrySave(save);

        public static SurvivorFateSave? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(SurvivorFateSave save) => s_store.CapturePersisted(save);

        private static string EncodeSave(SurvivorFateSave save, IJsonSerializer json)
        {
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        private static SurvivorFateSave? DecodeSave(string raw, IJsonSerializer json)
        {
            var save = json.Deserialize<SurvivorFateSave>(raw);
            if (save == null) return null;
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("SurvivorFateSave: empty checksum");
            if (!string.Equals(save.Checksum, SaveChecksum.Compute(save), StringComparison.Ordinal))
                throw new InvalidOperationException("SurvivorFateSave: checksum mismatch");
            return save;
        }
    }
}
