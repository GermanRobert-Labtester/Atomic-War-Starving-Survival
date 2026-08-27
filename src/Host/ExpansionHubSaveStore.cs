// ============================================================================
// Save Store : ExpansionHubSaveStore
// Core State : Ashfall.Core.ExpansionHubSave
// Host Caller: Main.ExpansionHub, Main.Holdfast / ExpansionHubHostSession
// Purpose    : Expansion module registry, activation flags, and cross-expansion telemetry
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="ExpansionHubSave"/> as JSON under
    /// user://expansion_hub_save.json — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// validation live in <see cref="ExpansionHubSaveCodec"/>; path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class ExpansionHubSaveStore
    {
        public const string FileName = "expansion_hub_save.json";
        public const string SectionName = "expansion_hub";

        private static readonly SaveStore<ExpansionHubSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ExpansionHubSaveStore),
            (save, json) => ExpansionHubSaveCodec.Encode(save, json),
            (raw, json) => ExpansionHubSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ExpansionHubSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ExpansionHubSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ExpansionHubSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ExpansionHubSave? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(ExpansionHubSave save, string pathOverride = null!) =>
            s_store.TrySave(save, pathOverride);

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static ExpansionHubSave? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ExpansionHubSave save) => s_store.CapturePersisted(save);
    }
}
