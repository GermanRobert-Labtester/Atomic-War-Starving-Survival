// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RailwaySaveStore
// Core State : Ashfall.Core.Expeditions.RailwayState
// Host Caller: Main.Plans190_193
// Purpose    : Rail network, track repair, and armored train operations
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RailwaySaveStore
    {
        public const string FileName = "railway_save.json";
        public const string SectionName = "railway";

        private static readonly SaveStore<RailwayState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RailwaySaveStore),
            SchemaVersionedEnvelope<RailwayState>.Encode,
            SchemaVersionedEnvelope<RailwayState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RailwayState state) => s_store.TrySave(state);
        public static RailwayState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RailwayState state) => s_store.CapturePersisted(state);
    }
}
