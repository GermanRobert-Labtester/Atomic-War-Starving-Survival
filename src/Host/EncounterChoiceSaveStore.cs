using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// EncounterChoice resolver save persistence — backed by SaveEnvelopeHelper
    /// for atomic writing, checksum envelope integrity, and slot-root routing.
    /// </summary>
    public static class EncounterChoiceSaveStore
    {
        public const string FileName = "encounter_choice_save.json";
        public const string SectionName = "encounter_choice";

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => new FileSystemIO().FileExists(SavePath);

        public static string TryCaptureDirect(EncounterChoiceState state) =>
            SaveEnvelopeHelper.CaptureEnvelope(state);

        public static EncounterChoiceState? TryRestoreDirect(string json) =>
            SaveEnvelopeHelper.RestoreEnvelope<EncounterChoiceState>(json).State;

        public static string TryCapture(EncounterChoiceState state) =>
            SaveEnvelopeHelper.CaptureEnvelope(state);

        public static EncounterChoiceState? TryRestore(string json) =>
            SaveEnvelopeHelper.RestoreEnvelope<EncounterChoiceState>(json).State;

        public static bool TrySave(EncounterChoiceState state) =>
            SaveEnvelopeHelper.TrySaveAtomic(SavePath, state, createBackup: true);

        public static EncounterChoiceState? TryLoad() =>
            SaveEnvelopeHelper.TryLoad<EncounterChoiceState>(SavePath).State;
    }
}
