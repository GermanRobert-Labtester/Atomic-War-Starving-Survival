// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// B5–B8 Phase 6: persists <see cref="DeepWellState"/> as a versioned
    /// envelope under <c>user://deep_well_save.json</c> — thin façade over the
    /// Core SaveStore service (same shape as SanitationSaveStore).
    /// </summary>
    public static class DeepWellSaveStore
    {
        public const string FileName = "deep_well_save.json";
        public const string SectionName = "deep_well";

        private static readonly SaveStore<DeepWellState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DeepWellSaveStore),
            SchemaVersionedEnvelope<DeepWellState>.Encode,
            SchemaVersionedEnvelope<DeepWellState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(DeepWellState state) => s_store.TrySave(state);
        public static DeepWellState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DeepWellState state) => s_store.CapturePersisted(state);
    }
}
