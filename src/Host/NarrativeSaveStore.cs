// ============================================================================
// Save Store : NarrativeSaveStore
// Core State : Ashfall.Core.NarrativeEncounterState
// Host Caller: Main.Narrative / NarrativeHostSession
// Purpose    : Narrative encounter branches, dialogue state machine, and narrative flags
// ============================================================================
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Narrative (encounter port) save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope,
    /// atomic write, and legacy bare-state loading live in the service.
    /// </summary>
    public static class NarrativeSaveStore
    {
        public const string FileName = "narrative_save.json";
        public const string SectionName = "narrative";

        private static readonly SaveStore<NarrativeEncounterState> s_store =
            SaveStoreHub.Checksummed<NarrativeEncounterState>(FileName, nameof(NarrativeSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(NarrativeEncounterState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static NarrativeEncounterState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(NarrativeEncounterState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static NarrativeEncounterState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(NarrativeEncounterState state) => s_store.TrySave(state);

        public static NarrativeEncounterState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(NarrativeEncounterState state) => s_store.CapturePersisted(state);
    }
}
