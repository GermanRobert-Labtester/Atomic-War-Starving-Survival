// ============================================================================
// Save Store : SilentFoundrySaveStore
// Core State : Ashfall.Core.Foundry.SilentFoundryState
// Host Caller: Main.Economy / SilentFoundryHostSession
// Purpose    : Silent Foundry automated forge queues, heat cycles, and alloy fabrication
// ============================================================================
using Ashfall.Core.Foundry;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Silent Foundry save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope and
    /// atomic write live in the service; this section keeps its historical
    /// strictness of NOT adopting pre-envelope bare-state files.
    /// </summary>
    public static class SilentFoundrySaveStore
    {
        public const string FileName = "silent_foundry_save.json";
        public const string SectionName = "silent_foundry";

        private static readonly SaveStore<SilentFoundryState> s_store =
            SaveStoreHub.Checksummed<SilentFoundryState>(FileName, nameof(SilentFoundrySaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(SilentFoundryState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static SilentFoundryState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(SilentFoundryState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static SilentFoundryState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(SilentFoundryState state) => s_store.TrySave(state);

        public static SilentFoundryState? TryLoad() => s_store.TryLoad();
    }
}
