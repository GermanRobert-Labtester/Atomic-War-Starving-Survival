// ============================================================================
// Save Store : HostEventSaveStore
// Core State : AtomicWar.GodotApp.Host.HostEventState
// Host Caller: Main.Narrative / HostEventAdapter
// Purpose    : Single campaign codec/projection for host-level dynamic event progress
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using AtomicWar.GodotApp.Host;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists the adapter-owned <see cref="HostEventState"/> as JSON under
    /// <c>user://host_event_save.json</c> — the compatibility projection and
    /// campaign payload use the same Core SaveStore&lt;T&gt; codec via SaveStoreHub.
    /// Checksummed envelope, atomic write, and legacy bare-state loading live in the service.
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

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(HostEventState state) => s_store.CapturePersisted(state);
    }
}
