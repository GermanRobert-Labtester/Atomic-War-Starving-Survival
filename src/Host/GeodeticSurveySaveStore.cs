// ============================================================================
// Save Store : GeodeticSurveySaveStore
// Core State : Ashfall.Core.World.GeodeticSurveyState
// Host Caller: Main.Plans78_81 / GeodeticSurveyHostSession
// Purpose    : Plans 78-81 — survey monuments, observations, resolved
//              triangles, network accuracy, and unlocked shortcut knowledge.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Geodetic survey save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// legacy <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service. Old saves (absent file) default to an empty survey network.
    /// </summary>
    public static class GeodeticSurveySaveStore
    {
        public const string FileName = "geodetic_survey_save.json";
        public const string SectionName = "geodetic_survey";

        private static readonly SaveStore<GeodeticSurveyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(GeodeticSurveySaveStore),
            SchemaVersionedEnvelope<GeodeticSurveyState>.Encode,
            SchemaVersionedEnvelope<GeodeticSurveyState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(GeodeticSurveyState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static GeodeticSurveyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(GeodeticSurveyState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static GeodeticSurveyState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(GeodeticSurveyState state) => s_store.TrySave(state);

        public static GeodeticSurveyState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(GeodeticSurveyState state) => s_store.CapturePersisted(state);
    }
}
