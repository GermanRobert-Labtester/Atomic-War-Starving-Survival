// ============================================================================
// Save Store : DoseLedgerSaveStore
// Core State : Ashfall.Core.DoseLedgerSave
// Host Caller: Main.Holdfast, Main.Phase0 / DoseLedgerHostSession
// Purpose    : Cumulative radiation dose ledger, threshold brackets, and survivor exposure logs
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="DoseLedgerSave"/> as JSON under
    /// user://dose_ledger_save.json — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// validation live in <see cref="DoseLedgerSaveCodec"/>; path resolution,
    /// atomic write, and error handling live in the service.
    /// </summary>
    public static class DoseLedgerSaveStore
    {
        public const string FileName = "dose_ledger_save.json";
        public const string SectionName = "dose_ledger";

        private static readonly SaveStore<DoseLedgerSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DoseLedgerSaveStore),
            (save, json) => DoseLedgerSaveCodec.Encode(save, json),
            (raw, json) => DoseLedgerSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(DoseLedgerSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static DoseLedgerSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(DoseLedgerSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static DoseLedgerSave? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(DoseLedgerSave save, string pathOverride = null!) =>
            s_store.TrySave(save, pathOverride);

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static DoseLedgerSave? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);
    }
}
