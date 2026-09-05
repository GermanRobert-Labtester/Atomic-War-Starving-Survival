// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PathogenStrainSaveStore
// Core State : Ashfall.Core.Disease.PathogenStrainSaveState
// Host Caller: Main.PathogenStrains
// Purpose    : Flagship XI Plan 155 — fictional strain layer cure projects and
//              unlocked cures (mutation results persist in the disease section)
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Disease;

namespace AtomicWar.GodotApp
{
    public static class PathogenStrainSaveStore
    {
        public const string FileName = "pathogen_strains_save.json";
        public const string SectionName = "pathogen_strains";

        private static readonly global::Ashfall.Core.Save.SaveStore<PathogenStrainSaveState> s_store =
            SaveStoreHub.FromCodec(
                FileName,
                nameof(PathogenStrainSaveStore),
                (state, json) => PathogenStrainSaveCodec.Encode(state, json),
                (json, serializer) =>
                    PathogenStrainSaveCodec.TryDecode(json, serializer, out var decoded) ? decoded : null);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PathogenStrainSaveState state) => s_store.TrySave(state);
        public static PathogenStrainSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PathogenStrainSaveState state) => s_store.CapturePersisted(state);
    }
}
