// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : TraumaBondSaveStore
// Core State : Ashfall.Core.Survivors.TraumaBondSaveState
// Host Caller: Main.TraumaBond (SetupTraumaBond / SaveTraumaBond)
// Purpose    : Trauma bond authority — shared-hazard bond strengths, daily
//              decay, and the co-shift efficiency bonus.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class TraumaBondSaveStore
    {
        public const string FileName = "trauma_bond_save.json";
        public const string SectionName = "trauma_bond";

        private static readonly SaveStore<TraumaBondSaveState> s_store =
            SaveStoreHub.Checksummed<TraumaBondSaveState>(FileName, nameof(TraumaBondSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(TraumaBondSaveState state) => s_store.TrySave(state);
        public static TraumaBondSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(TraumaBondSaveState state) => s_store.CaptureBare(state);
        public static TraumaBondSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
    }
}
