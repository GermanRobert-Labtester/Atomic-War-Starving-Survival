// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AnomalyHazardSaveStore
// Core State : Ashfall.Core.World.AnomalyHazardSystemState
// Host Caller: Main.Anomaly
// Purpose    : Plan 176 — authored anomaly/storm-front hazard zones:
//              positions, movement state, warning keys, loot-site resolution
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class AnomalyHazardSaveStore
    {
        public const string FileName = "anomaly_hazard_save.json";
        public const string SectionName = "anomaly_hazard";

        private static readonly SaveStore<AnomalyHazardSystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AnomalyHazardSaveStore),
            SchemaVersionedEnvelope<AnomalyHazardSystemState>.Encode,
            SchemaVersionedEnvelope<AnomalyHazardSystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(AnomalyHazardSystemState state) => s_store.TrySave(state);
        public static AnomalyHazardSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AnomalyHazardSystemState state) => s_store.CapturePersisted(state);
    }
}
