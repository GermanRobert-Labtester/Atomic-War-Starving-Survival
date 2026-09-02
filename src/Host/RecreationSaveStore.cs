// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RecreationSaveStore
// Core State : Ashfall.Core.Recreation.RecreationState
// Host Caller: Main.Plans194_197
// Purpose    : Survivor downtime, hobbies, skill progression & recreation sessions
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Recreation;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RecreationSaveStore
    {
        public const string FileName = "recreation_save.json";
        public const string SectionName = "recreation";

        private static readonly SaveStore<RecreationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RecreationSaveStore),
            SchemaVersionedEnvelope<RecreationState>.Encode,
            SchemaVersionedEnvelope<RecreationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RecreationState state) => s_store.TrySave(state);
        public static RecreationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RecreationState state) => s_store.CapturePersisted(state);
    }
}
