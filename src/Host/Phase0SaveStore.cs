// ============================================================================
// Save Store : Phase0SaveStore
// Core State : Ashfall.Core.Phase0EffectsSaveState
// Host Caller: Main.Phase0 / Phase0HostSession
// Purpose    : Phase 0 survivor behavioral quirks, specialized perks, and lingering trauma
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Phase-0 effects save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope and
    /// atomic write live in the service; this section keeps its historical
    /// strictness of NOT adopting pre-envelope bare-state files.
    /// </summary>
    public static class Phase0SaveStore
    {
        public const string FileName = "phase0_save.json";
        public const string SectionName = "phase0";

        private static readonly SaveStore<Phase0EffectsSaveState> s_store =
            SaveStoreHub.Checksummed<Phase0EffectsSaveState>(FileName, nameof(Phase0SaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(Phase0EffectsSaveState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static Phase0EffectsSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(Phase0EffectsSaveState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static Phase0EffectsSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(Phase0EffectsSaveState state) => s_store.TrySave(state);

        public static Phase0EffectsSaveState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(Phase0EffectsSaveState state) => s_store.CapturePersisted(state);
    }
}
