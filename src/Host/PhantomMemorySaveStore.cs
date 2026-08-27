// ============================================================================
// Save Store : PhantomMemorySaveStore
// Core State : Ashfall.Core.PhantomMemoryEngineState
// Host Caller: Main.Phase0 / PhantomMemoryHostSession
// Purpose    : Phase 0 phantom memory engine, trauma flashbacks, and psychological echoes
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Phantom Memory (Antigravity #41) save persistence — thin façade over
    /// the Core SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed
    /// envelope and atomic write live in the service; this section keeps its
    /// historical strictness of NOT adopting pre-envelope bare-state files.
    /// </summary>
    public static class PhantomMemorySaveStore
    {
        public const string FileName = "phantom_memory_save.json";
        public const string SectionName = "phantom_memory";

        private static readonly SaveStore<PhantomMemoryEngineState> s_store =
            SaveStoreHub.Checksummed<PhantomMemoryEngineState>(FileName, nameof(PhantomMemorySaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(PhantomMemoryEngineState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static PhantomMemoryEngineState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(PhantomMemoryEngineState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static PhantomMemoryEngineState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(PhantomMemoryEngineState state) => s_store.TrySave(state);

        public static PhantomMemoryEngineState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(PhantomMemoryEngineState state) => s_store.CapturePersisted(state);
    }
}
