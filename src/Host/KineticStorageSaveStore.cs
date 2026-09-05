// ============================================================================
// Save Store : KineticStorageSaveStore
// Core State : Ashfall.Core.Shelter.KineticStorageState
// Host Caller: Main.Plans78_81 / KineticStorageHostSession
// Purpose    : Plans 78-81 — flywheel rotor RPM, stored energy, vacuum,
//              bearing thermal state, health, and containment state.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Kinetic storage save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// legacy <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service. Old saves (absent file) default to no flywheel installations.
    /// </summary>
    public static class KineticStorageSaveStore
    {
        public const string FileName = "kinetic_storage_save.json";
        public const string SectionName = "kinetic_storage";

        private static readonly SaveStore<KineticStorageState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(KineticStorageSaveStore),
            SchemaVersionedEnvelope<KineticStorageState>.Encode,
            SchemaVersionedEnvelope<KineticStorageState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(KineticStorageState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static KineticStorageState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(KineticStorageState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static KineticStorageState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(KineticStorageState state) => s_store.TrySave(state);

        public static KineticStorageState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(KineticStorageState state) => s_store.CapturePersisted(state);
    }
}
