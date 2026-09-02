// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CommsArraySaveStore
// Core State : Ashfall.Core.World.CommsArraySaveState
// Host Caller: Main.Plans198_201
// Purpose    : Long-range communications array, telemetry locks & strategic codes
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class CommsArraySaveStore
    {
        public const string FileName = "comms_array_save.json";
        public const string SectionName = "comms_array";

        private static readonly SaveStore<CommsArraySaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CommsArraySaveStore),
            SchemaVersionedEnvelope<CommsArraySaveState>.Encode,
            SchemaVersionedEnvelope<CommsArraySaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CommsArraySaveState state) => s_store.TrySave(state);
        public static CommsArraySaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CommsArraySaveState state) => s_store.CapturePersisted(state);
    }
}
