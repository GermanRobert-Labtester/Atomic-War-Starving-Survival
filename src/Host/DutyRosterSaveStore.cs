// ============================================================================
// Save Store : DutyRosterSaveStore
// Core State : Ashfall.Core.DutyRosterSave
// Host Caller: Main.DutyRoster, Main.Holdfast / DutyRosterHostSession
// Purpose    : Duty roster shift allocations, work assignments, and fatigue modifiers
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="DutyRosterSave"/> as JSON under
    /// user://duty_roster_save.json — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). The save
    /// shape and all validation live in <see cref="DutyRosterSaveCodec"/>;
    /// path resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class DutyRosterSaveStore
    {
        public const string FileName = "duty_roster_save.json";
        public const string SectionName = "duty_roster";

        private static readonly SaveStore<DutyRosterSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DutyRosterSaveStore),
            (save, json) => DutyRosterSaveCodec.Encode(save, json),
            (raw, json) => DutyRosterSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(DutyRosterSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static DutyRosterSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(DutyRosterSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static DutyRosterSave? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(DutyRosterSave save, string pathOverride = null!) =>
            s_store.TrySave(save, pathOverride);

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static DutyRosterSave? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);
    }
}
