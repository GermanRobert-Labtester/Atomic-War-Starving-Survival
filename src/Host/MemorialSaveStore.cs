// ============================================================================
// Save Store : MemorialSaveStore
// Core State : Ashfall.Core.Memorial.MemorialSave
// Host Caller: Main.Campaign / MemorialHostSession
// Purpose    : Fallen survivor memorial wall, cause of death records, and shelter grief tallies
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists MemorialState under user://memorial_save.json — façade over
    /// the Core SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor).
    /// MemorialSave is a self-checksummed type (the checksum is a field of the
    /// state itself), so encode/decode stamp and verify it directly; path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class MemorialSaveStore
    {
        public const string FileName = "memorial_save.json";
        public const string SectionName = "memorial";

        private static readonly SaveStore<MemorialSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MemorialSaveStore),
            EncodeSave,
            DecodeSave);

        public static string SavePath => s_store.SavePath;

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MemorialSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MemorialSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MemorialSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MemorialSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(MemorialSave save) => s_store.TrySave(save);

        public static MemorialSave? TryLoad() => s_store.TryLoad();

        private static string EncodeSave(MemorialSave save, IJsonSerializer json)
        {
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        private static MemorialSave? DecodeSave(string raw, IJsonSerializer json)
        {
            var save = json.Deserialize<MemorialSave>(raw);
            if (save == null) return null;
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("MemorialSave: empty checksum");
            if (!string.Equals(save.Checksum, SaveChecksum.Compute(save), StringComparison.Ordinal))
                throw new InvalidOperationException("MemorialSave: checksum mismatch");
            return save;
        }
    }
}
