// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RadioStationSaveStore
// Core State : Ashfall.Core.Radio.RadioStationStateSave
// Host Caller: Main.RadioStation
// Purpose    : Radio station frequency tuning, signal lock, and triangulation
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Radio;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RadioStationSaveStore
    {
        public const string FileName = "radio_station_save.json";
        public const string SectionName = "radio_station";

        private static readonly SaveStore<RadioStationStateSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RadioStationSaveStore),
            SchemaVersionedEnvelope<RadioStationStateSave>.Encode,
            SchemaVersionedEnvelope<RadioStationStateSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RadioStationStateSave state) => s_store.TrySave(state);
        public static RadioStationStateSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RadioStationStateSave state) => s_store.CapturePersisted(state);
    }
}
