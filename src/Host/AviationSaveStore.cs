// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AviationSaveStore
// Core State : Ashfall.Core.Expeditions.AviationState
// Host Caller: Main.Plans182_185
// Purpose    : Aviation airframes, flight plans, aerial mapping, and crash rescue
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class AviationSaveStore
    {
        public const string FileName = "aviation_save.json";
        public const string SectionName = "aviation";

        private static readonly SaveStore<AviationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AviationSaveStore),
            SchemaVersionedEnvelope<AviationState>.Encode,
            SchemaVersionedEnvelope<AviationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(AviationState state) => s_store.TrySave(state);
        public static AviationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AviationState state) => s_store.CapturePersisted(state);
    }
}
