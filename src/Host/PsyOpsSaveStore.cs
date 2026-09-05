// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PsyOpsSaveStore
// Core State : Ashfall.Core.Radio.PsyOpsSaveState
// Host Caller: Main.PsyOps
// Purpose    : Flagship XI Plan 157 — broadcast campaigns, jamming, counter-
//              propaganda, ideological-pressure ledger, campaign fatigue
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public static class PsyOpsSaveStore
    {
        public const string FileName = "psyops_save.json";
        public const string SectionName = "psyops";

        private static readonly global::Ashfall.Core.Save.SaveStore<PsyOpsSaveState> s_store =
            SaveStoreHub.FromCodec(
                FileName,
                nameof(PsyOpsSaveStore),
                (state, json) => PsyOpsSaveCodec.Encode(state, json),
                (json, serializer) =>
                    PsyOpsSaveCodec.TryDecode(json, serializer, out var decoded) ? decoded : null);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PsyOpsSaveState state) => s_store.TrySave(state);
        public static PsyOpsSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PsyOpsSaveState state) => s_store.CapturePersisted(state);
    }
}
