// ============================================================================
// Save Store : ChemicalReconSaveStore
// Core State : Ashfall.Core.Expeditions.ChemicalReconState
// Host Caller: Main.Plans78_81 / ChemicalReconHostSession
// Purpose    : Plans 78-81 — chemical hazard observations, discovered hazard
//              knowledge, collected sample references, detector battery, and
//              safe-corridor capability.
// ============================================================================
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Chemical recon save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// legacy <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service. Old saves (absent file) default to no recon data.
    /// </summary>
    public static class ChemicalReconSaveStore
    {
        public const string FileName = "chemical_recon_save.json";
        public const string SectionName = "chemical_recon";

        private static readonly SaveStore<ChemicalReconState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ChemicalReconSaveStore),
            SchemaVersionedEnvelope<ChemicalReconState>.Encode,
            SchemaVersionedEnvelope<ChemicalReconState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ChemicalReconState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ChemicalReconState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ChemicalReconState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ChemicalReconState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ChemicalReconState state) => s_store.TrySave(state);

        public static ChemicalReconState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ChemicalReconState state) => s_store.CapturePersisted(state);
    }
}
