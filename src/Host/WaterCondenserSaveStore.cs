// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// B5–B8 expansion: persists <see cref="AtmosphericCondenserState"/> as a
    /// versioned envelope under <c>user://water_condenser_save.json</c> — same
    /// shape as DeepWellSaveStore.
    /// </summary>
    public static class WaterCondenserSaveStore
    {
        public const string FileName = "water_condenser_save.json";
        public const string SectionName = "water_condenser";

        private static readonly SaveStore<AtmosphericCondenserState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WaterCondenserSaveStore),
            SchemaVersionedEnvelope<AtmosphericCondenserState>.Encode,
            SchemaVersionedEnvelope<AtmosphericCondenserState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(AtmosphericCondenserState state) => s_store.TrySave(state);
        public static AtmosphericCondenserState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AtmosphericCondenserState state) => s_store.CapturePersisted(state);
    }
}
