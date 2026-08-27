// ============================================================================
// Save Store : RegionalTreatySaveStore
// Core State : Ashfall.Core.RegionalTreatyState
// Host Caller: Main.ShelterSocial / RegionalTreatyHostSession
// Purpose    : Regional faction pacts, non-aggression treaties, and border agreements
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Regional treaty save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class RegionalTreatySaveStore
    {
        public const string FileName = "regional_treaty_save.json";
        public const string SectionName = "regional_treaty";

        private static readonly SaveStore<RegionalTreatyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RegionalTreatySaveStore),
            SchemaVersionedEnvelope<RegionalTreatyState>.Encode,
            SchemaVersionedEnvelope<RegionalTreatyState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(RegionalTreatyState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static RegionalTreatyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(RegionalTreatyState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static RegionalTreatyState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(RegionalTreatyState state) => s_store.TrySave(state);

        public static RegionalTreatyState? TryLoad() => s_store.TryLoad();
    }
}
