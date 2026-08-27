// ============================================================================
// Save Store : AirlockSecuritySaveStore
// Core State : Ashfall.Core.AirlockSecurityState
// Host Caller: Main.ShelterInfrastructure / AirlockSecurityHostSession
// Purpose    : Airlock security protocols, decontamination cycles, and quarantine locks
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Airlock security save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class AirlockSecuritySaveStore
    {
        public const string FileName = "airlock_security_save.json";
        public const string SectionName = "airlock_security";

        private static readonly SaveStore<AirlockSecurityState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AirlockSecuritySaveStore),
            SchemaVersionedEnvelope<AirlockSecurityState>.Encode,
            SchemaVersionedEnvelope<AirlockSecurityState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(AirlockSecurityState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static AirlockSecurityState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(AirlockSecurityState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static AirlockSecurityState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(AirlockSecurityState state) => s_store.TrySave(state);

        public static AirlockSecurityState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(AirlockSecurityState state) => s_store.CapturePersisted(state);
    }
}
