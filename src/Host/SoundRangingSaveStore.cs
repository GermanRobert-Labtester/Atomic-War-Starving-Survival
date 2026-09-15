// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SoundRangingSaveStore
// Core State : SoundRangingThreatEngine.SoundRangingStationState
// Host Caller: Main.Plans122to125
// Purpose    : Plan 123 — array calibration, node status, observation history, active threat estimate
// ============================================================================
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class SoundRangingSaveStore
    {
        public const string FileName = "sound_ranging_save.json";
        public const string SectionName = "sound_ranging";

        private static readonly SaveStore<SoundRangingThreatEngine.SoundRangingStationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SoundRangingSaveStore),
            SchemaVersionedEnvelope<SoundRangingThreatEngine.SoundRangingStationState>.Encode,
            SchemaVersionedEnvelope<SoundRangingThreatEngine.SoundRangingStationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SoundRangingThreatEngine.SoundRangingStationState state) => s_store.TrySave(state);
        public static SoundRangingThreatEngine.SoundRangingStationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SoundRangingThreatEngine.SoundRangingStationState state) => s_store.CapturePersisted(state);
    }
}
