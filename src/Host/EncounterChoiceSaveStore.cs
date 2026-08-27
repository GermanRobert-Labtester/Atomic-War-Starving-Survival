using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// EncounterChoice resolver save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub) for atomic writing,
    /// checksum envelope integrity, backup rotation, and slot-root routing.
    /// Capture/restore keep this section's envelope-capture semantics.
    /// </summary>
    public static class EncounterChoiceSaveStore
    {
        public const string FileName = "encounter_choice_save.json";
        public const string SectionName = "encounter_choice";

        private static readonly SaveStore<EncounterChoiceState> s_store =
            SaveStoreHub.Checksummed<EncounterChoiceState>(FileName, nameof(EncounterChoiceSaveStore), createBackup: true);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(EncounterChoiceState state) =>
            s_store.CaptureEnvelope(state);

        public static EncounterChoiceState? TryRestoreDirect(string json) =>
            s_store.RestoreEnvelope(json);

        public static string TryCapture(EncounterChoiceState state) =>
            s_store.CaptureEnvelope(state);

        public static EncounterChoiceState? TryRestore(string json) =>
            s_store.RestoreEnvelope(json);

        public static bool TrySave(EncounterChoiceState state) =>
            s_store.TrySave(state);

        public static EncounterChoiceState? TryLoad() =>
            s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(EncounterChoiceState state) => s_store.CapturePersisted(state);
    }
}
