// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SeismicDynamicsSaveStore
// Core State : Ashfall.Core.Shelter.SeismicDynamicsSaveState
// Host Caller: Main.PlansB68_B69
// Purpose    : Plan B68 — fault tension, slips, geophone coverage, dampener
//              integrity, quake history
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class SeismicDynamicsSaveStore
    {
        public const string FileName = "seismic_dynamics_save.json";
        public const string SectionName = "seismic_dynamics";

        private static readonly SaveStore<SeismicDynamicsSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SeismicDynamicsSaveStore),
            SchemaVersionedEnvelope<SeismicDynamicsSaveState>.Encode,
            SchemaVersionedEnvelope<SeismicDynamicsSaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SeismicDynamicsSaveState state) => s_store.TrySave(state);
        public static SeismicDynamicsSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SeismicDynamicsSaveState state) => s_store.CapturePersisted(state);
    }
}
