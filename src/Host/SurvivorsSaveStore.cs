// ============================================================================
// Save Store : SurvivorsSaveStore
// Core State : Ashfall.Core.SurvivorsSaveState
// Host Caller: Main.Survivors / SurvivorsHostSession
// Purpose    : Survivor roster profiles, vital needs, injuries, traits, and morale
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Survivors (needs + radiation) save persistence — thin façade over the
    /// Core SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed
    /// envelope and atomic write live in the service; this section keeps its
    /// historical strictness of NOT adopting pre-envelope bare-state files.
    /// </summary>
    public static class SurvivorsSaveStore
    {
        public const string FileName = "survivors_save.json";
        public const string SectionName = "survivors";

        private static readonly SaveStore<SurvivorsSaveState> s_store =
            SaveStoreHub.Checksummed<SurvivorsSaveState>(FileName, nameof(SurvivorsSaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(SurvivorsSaveState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static SurvivorsSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(SurvivorsSaveState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static SurvivorsSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(SurvivorsSaveState state) => s_store.TrySave(state);

        public static SurvivorsSaveState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(SurvivorsSaveState state) => s_store.CapturePersisted(state);
    }
}
