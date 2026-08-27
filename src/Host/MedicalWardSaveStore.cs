// ============================================================================
// Save Store : MedicalWardSaveStore
// Core State : Ashfall.Core.Medical.MedicalWardSave
// Host Caller: Main.Medical / MedicalWardHostSession
// Purpose    : Medical ward bed occupancy, hospital triage, and critical care status
// ============================================================================
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists MedicalWardSave under user://medical_ward_save.json — thin
    /// façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub,
    /// codec flavor). Shape and validation live in
    /// <see cref="MedicalWardSaveCodec"/>; path resolution, atomic write, and
    /// error handling live in the service.
    /// </summary>
    public static class MedicalWardSaveStore
    {
        public const string FileName = "medical_ward_save.json";
        public const string SectionName = "medical_ward";

        private static readonly SaveStore<MedicalWardSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MedicalWardSaveStore),
            (save, json) => MedicalWardSaveCodec.EncodeToString(save, json),
            (raw, json) => MedicalWardSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MedicalWardSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MedicalWardSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MedicalWardSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MedicalWardSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(MedicalWardSave save) => s_store.TrySave(save);

        public static MedicalWardSave? TryLoad() => s_store.TryLoad();
    }
}
