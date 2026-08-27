// ============================================================================
// Save Store : ExpansionQuestSaveStore
// Core State : Ashfall.Core.ExpansionQuestSaveEnvelope
// Host Caller: Main.Quests / ExpansionQuestHostSession
// Purpose    : Expansion quest graph runtime states, objective progression, and quest flags
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="ExpansionQuestSaveEnvelope"/> as JSON under
    /// user://expansion_quest_save.json — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// validation live in <see cref="ExpansionQuestSaveCodec"/>; path
    /// resolution, atomic write, and error handling live in the service. This
    /// section keeps its void Save call surface with a path-overridable load.
    /// </summary>
    public static class ExpansionQuestSaveStore
    {
        public const string FileName = "expansion_quest_save.json";
        public const string SectionName = "expansion_quest";

        private static readonly SaveStore<ExpansionQuestSaveEnvelope> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ExpansionQuestSaveStore),
            (envelope, json) => ExpansionQuestSaveCodec.Encode(envelope, json),
            (raw, json) => ExpansionQuestSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ExpansionQuestSaveEnvelope envelope) => s_store.CaptureBare(envelope);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ExpansionQuestSaveEnvelope? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ExpansionQuestSaveEnvelope envelope) => s_store.CaptureBare(envelope);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ExpansionQuestSaveEnvelope? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Writes through the codec (checksum stamped). Void surface preserved.</summary>
        public static void Save(ExpansionQuestSaveEnvelope envelope)
        {
            s_store.TrySave(envelope);
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static ExpansionQuestSaveEnvelope TryLoad(string pathOverride = null!)
        {
            return s_store.TryLoad(pathOverride)!;
        }

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ExpansionQuestSaveEnvelope envelope) => s_store.CapturePersisted(envelope);
    }
}
