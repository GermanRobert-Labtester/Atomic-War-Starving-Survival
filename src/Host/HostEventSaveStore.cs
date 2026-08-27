// ============================================================================
// Save Store : HostEventSaveStore
// Core State : Ashfall.Core.HostEventState
// Host Caller: Main.Narrative / HostEventHostSession
// Purpose    : Host-level dynamic event triggers, queued incidents, and event history
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using AtomicWar.GodotApp.Host;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="HostEventState"/> as JSON under
    /// <c>user://host_event_save.json</c> — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope,
    /// atomic write, and legacy bare-state loading live in the service.
    /// </summary>
    public static class HostEventSaveStore
    {
        public const string FileName = "host_event_save.json";
        public const string SectionName = "host_event";

        private static readonly SaveStore<HostEventState> s_store =
            SaveStoreHub.Checksummed<HostEventState>(FileName, nameof(HostEventSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static string TryCapture(HostEventState state) => s_store.CaptureBare(state);

        public static HostEventState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(HostEventState state) => s_store.TrySave(state);

        public static HostEventState? TryLoad() => s_store.TryLoad();
    }
}
