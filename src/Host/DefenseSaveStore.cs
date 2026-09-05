// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DefenseSaveStore
// Core State : Ashfall.Core.Defense.DefenseSystemState
// Host Caller: Main.Plans162_165
// Purpose    : Trap installations (armed/sprung/broken), reset/repair history,
//              and the bounded structured raid log (Plan 163).
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class DefenseSaveStore
    {
        public const string FileName = "settlement_defenses_save.json";
        public const string SectionName = "settlement_defenses";

        private static readonly SaveStore<DefenseSystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DefenseSaveStore),
            SchemaVersionedEnvelope<DefenseSystemState>.Encode,
            SchemaVersionedEnvelope<DefenseSystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(DefenseSystemState state) => s_store.TrySave(state);
        public static DefenseSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DefenseSystemState state) => s_store.CapturePersisted(state);
    }
}
