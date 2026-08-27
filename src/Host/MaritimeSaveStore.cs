// ============================================================================
// Save Store : MaritimeSaveStore
// Core State : Ashfall.Core.MaritimeHostSave
// Host Caller: Main.Maritime / DeepCoastHostSession, MaritimeHostSession
// Purpose    : Black Flotilla / Deep Coast maritime sorties, dive salvage, and hull condition
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Maritime (Expansion 09) save persistence — façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). The Core
    /// MaritimeHostSave is a self-checksummed envelope; encode/decode stamp
    /// and verify it directly while path resolution, atomic write, and error
    /// handling live in the service.
    /// </summary>
    public static class MaritimeSaveStore
    {
        public const string FileName = "maritime_save.json";
        public const string SectionName = "maritime";

        private static readonly SaveStore<MaritimeHostSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MaritimeSaveStore),
            EncodeSave,
            DecodeSave);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MaritimeHostSave save) => s_store.CaptureBare(save);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MaritimeHostSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MaritimeHostSave save) => s_store.CaptureBare(save);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MaritimeHostSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(MaritimeHostSave save) => s_store.TrySave(save);

        public static MaritimeHostSave? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(MaritimeHostSave save) => s_store.CapturePersisted(save);

        private static string EncodeSave(MaritimeHostSave save, IJsonSerializer json)
        {
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        private static MaritimeHostSave? DecodeSave(string raw, IJsonSerializer json)
        {
            var save = json.Deserialize<MaritimeHostSave>(raw);
            if (save == null) return null;
            // The checksummed envelope is the only Maritime format; an empty
            // checksum means a malformed new-format save, not "legacy".
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("checksum field missing (corrupt save).");
            if (!string.Equals(save.Checksum, SaveChecksum.Compute(save), StringComparison.Ordinal))
                throw new InvalidOperationException("checksum mismatch (corrupt or foreign save).");
            return save;
        }
    }
}
