// ============================================================================
// Save Store : ThirdonarySaveStore
// Core State : Ashfall.Core.Thirdonary.ThirdonarySaveEnvelope
// Host Caller: Main.Quests / ThirdonaryHostSession
// Purpose    : Thirdonary expansion quest narrative trees and faction cipher state
// ============================================================================
using Ashfall.Core.Thirdonary;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists ThirdonarySaveEnvelope as JSON under
    /// user://thirdonary_quest_save.json — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// validation live in <see cref="ThirdonarySaveCodec"/>; path resolution,
    /// atomic write, and error handling live in the service. This section
    /// keeps its void Save call surface with a path-overridable load.
    /// </summary>
    public static class ThirdonarySaveStore
    {
        public const string FileName = "thirdonary_quest_save.json";
        public const string SectionName = "thirdonary";

        private static readonly SaveStore<ThirdonarySaveEnvelope> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ThirdonarySaveStore),
            (envelope, json) => ThirdonarySaveCodec.Encode(envelope, json),
            (raw, json) => ThirdonarySaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ThirdonarySaveEnvelope envelope) => s_store.CaptureBare(envelope);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ThirdonarySaveEnvelope? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Writes through the codec (checksum stamped). Void surface preserved.</summary>
        public static void Save(ThirdonarySaveEnvelope envelope)
        {
            s_store.TrySave(envelope);
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static ThirdonarySaveEnvelope? TryLoad(string pathOverride = null!)
        {
            return s_store.TryLoad(pathOverride);
        }
    }
}
