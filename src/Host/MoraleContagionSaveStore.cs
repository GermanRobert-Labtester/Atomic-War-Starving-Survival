// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MoraleContagionSaveStore
// Core State : Ashfall.Core.Survivors.MoraleContagionSaveState
// Host Caller: Main.MoraleContagion
// Purpose    : Flagship XI Plan 154 — morale contagion channels, isolation
//              markers, schism ledger, and HopeBeacon installation state
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class MoraleContagionSaveStore
    {
        public const string FileName = "morale_contagion_save.json";
        public const string SectionName = "morale_contagion";

        private static readonly global::Ashfall.Core.Save.SaveStore<MoraleContagionSaveState> s_store =
            SaveStoreHub.FromCodec(
                FileName,
                nameof(MoraleContagionSaveStore),
                (state, json) => MoraleContagionSaveCodec.Encode(state, json),
                (json, serializer) =>
                    MoraleContagionSaveCodec.TryDecode(json, serializer, out var decoded) ? decoded : null);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(MoraleContagionSaveState state) => s_store.TrySave(state);
        public static MoraleContagionSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(MoraleContagionSaveState state) => s_store.CapturePersisted(state);
    }
}
