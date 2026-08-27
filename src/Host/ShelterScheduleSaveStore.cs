// ============================================================================
// Save Store : ShelterScheduleSaveStore
// Core State : Ashfall.Core.ShelterScheduleState
// Host Caller: Main.ShelterInfrastructure / ShelterScheduleHostSession
// Purpose    : Shelter daily routine schedules, curfew hours, and rationing shifts
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Shelter schedule save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class ShelterScheduleSaveStore
    {
        public const string FileName = "shelter_schedule_save.json";
        public const string SectionName = "shelter_schedule";

        private static readonly SaveStore<ShelterScheduleState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterScheduleSaveStore),
            SchemaVersionedEnvelope<ShelterScheduleState>.Encode,
            SchemaVersionedEnvelope<ShelterScheduleState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ShelterScheduleState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ShelterScheduleState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ShelterScheduleState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ShelterScheduleState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ShelterScheduleState state) => s_store.TrySave(state);

        public static ShelterScheduleState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ShelterScheduleState state) => s_store.CapturePersisted(state);
    }
}
