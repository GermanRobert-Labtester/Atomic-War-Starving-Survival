// ============================================================================
// Save Store : HoldfastSaveStore
// Core State : Ashfall.Core.HoldfastSave
// Host Caller: Main.Holdfast, Main.SaveOrchestrator / HoldfastRuntimeSession
// Purpose    : Holdfast Season 1 survival progression, shelter structural state, and milestone records
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="HoldfastSave"/> as JSON under user://holdfast_s1_save.json —
    /// thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub,
    /// codec flavor). The save shape and all validation live in
    /// <see cref="HoldfastSaveCodec"/>; path resolution, atomic write, and
    /// error handling live in the service.
    /// </summary>
    public static class HoldfastSaveStore
    {
        public const string FileName = "holdfast_s1_save.json";
        public const string SectionName = "holdfast_s1";

        private static readonly SaveStore<HoldfastSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(HoldfastSaveStore),
            (save, json) => HoldfastSaveCodec.Encode(save, json),
            (raw, json) => HoldfastSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(HoldfastSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static HoldfastSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(HoldfastSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static HoldfastSave? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(HoldfastSave save, string pathOverride = null!) =>
            s_store.TrySave(save, pathOverride);

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static HoldfastSave? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);
    }
}
