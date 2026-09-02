// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DesperationSaveStore
// Core State : Ashfall.Core.Survivors.DesperationState
// Host Caller: Main.Plans186_189
// Purpose    : Starvation crisis desperation acts and cannibalism history
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class DesperationSaveStore
    {
        public const string FileName = "desperation_save.json";
        public const string SectionName = "desperation";

        private static readonly SaveStore<DesperationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DesperationSaveStore),
            SchemaVersionedEnvelope<DesperationState>.Encode,
            SchemaVersionedEnvelope<DesperationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(DesperationState state) => s_store.TrySave(state);
        public static DesperationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DesperationState state) => s_store.CapturePersisted(state);
    }
}
