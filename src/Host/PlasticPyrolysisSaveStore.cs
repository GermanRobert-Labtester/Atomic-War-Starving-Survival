// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PlasticPyrolysisSaveStore
// Core State : Ashfall.Core.Shelter.PlasticPyrolysisState
// Host Caller: Main.Plans202_205
// Purpose    : Retort bay machine state, active batch, buffered outputs
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class PlasticPyrolysisSaveStore
    {
        public const string FileName = "plastic_pyrolysis_save.json";
        public const string SectionName = "plastic_pyrolysis";

        private static readonly SaveStore<PlasticPyrolysisState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PlasticPyrolysisSaveStore),
            SchemaVersionedEnvelope<PlasticPyrolysisState>.Encode,
            SchemaVersionedEnvelope<PlasticPyrolysisState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PlasticPyrolysisState state) => s_store.TrySave(state);
        public static PlasticPyrolysisState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PlasticPyrolysisState state) => s_store.CapturePersisted(state);
    }
}
