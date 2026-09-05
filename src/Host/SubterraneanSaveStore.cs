// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SubterraneanSaveStore
// Core State : Ashfall.Core.Subterranean.SubterraneanSaveState
// Host Caller: Main.Subterranean
// Purpose    : Flagship XI Plan 156 — generated underground topology, node
//              structural/oxygen/flood/shoring state and discovery
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Subterranean;

namespace AtomicWar.GodotApp
{
    public static class SubterraneanSaveStore
    {
        public const string FileName = "subterranean_save.json";
        public const string SectionName = "subterranean";

        private static readonly global::Ashfall.Core.Save.SaveStore<SubterraneanSaveState> s_store =
            SaveStoreHub.FromCodec(
                FileName,
                nameof(SubterraneanSaveStore),
                (state, json) => SubterraneanSaveCodec.Encode(state, json),
                (json, serializer) =>
                    SubterraneanSaveCodec.TryDecode(json, serializer, out var decoded) ? decoded : null);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SubterraneanSaveState state) => s_store.TrySave(state);
        public static SubterraneanSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SubterraneanSaveState state) => s_store.CapturePersisted(state);
    }
}
