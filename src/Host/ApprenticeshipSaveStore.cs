// ============================================================================
// Save Store : ApprenticeshipSaveStore
// Core State : Ashfall.Core.ApprenticeshipState
// Host Caller: Main.ShelterSocial / ApprenticeshipHostSession
// Purpose    : Apprenticeship mentor-student assignments and craft training progression
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Apprenticeship save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class ApprenticeshipSaveStore
    {
        public const string FileName = "apprenticeship_save.json";
        public const string SectionName = "apprenticeship";

        private static readonly SaveStore<ApprenticeshipState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ApprenticeshipSaveStore),
            SchemaVersionedEnvelope<ApprenticeshipState>.Encode,
            SchemaVersionedEnvelope<ApprenticeshipState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ApprenticeshipState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ApprenticeshipState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ApprenticeshipState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ApprenticeshipState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ApprenticeshipState state) => s_store.TrySave(state);

        public static ApprenticeshipState? TryLoad() => s_store.TryLoad();
    }
}
