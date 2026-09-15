// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SanitationSaveStore
// Core State : Ashfall.Core.Shelter.SanitationState
// Host Caller: Main.Sanitation
// Purpose    : Plan 210 — room waste, hygiene inputs, compost queue, spills
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class SanitationSaveStore
    {
        public const string FileName = "sanitation_save.json";
        public const string SectionName = "sanitation";

        private static readonly SaveStore<SanitationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SanitationSaveStore),
            SchemaVersionedEnvelope<SanitationState>.Encode,
            SchemaVersionedEnvelope<SanitationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SanitationState state) => s_store.TrySave(state);
        public static SanitationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SanitationState state) => s_store.CapturePersisted(state);
    }
}
