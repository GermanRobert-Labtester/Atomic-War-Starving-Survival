// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ZealotrySaveStore
// Core State : Ashfall.Core.Survivors.ZealotrySystemState
// Host Caller: Main.Zealotry
// Purpose    : Plan 175 — fictional ideological pressure state (belief,
//              conviction, fervor, dissent, shrines, escalation ladder)
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class ZealotrySaveStore
    {
        public const string FileName = "zealotry_save.json";
        public const string SectionName = "zealotry";

        private static readonly SaveStore<ZealotrySystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ZealotrySaveStore),
            SchemaVersionedEnvelope<ZealotrySystemState>.Encode,
            SchemaVersionedEnvelope<ZealotrySystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ZealotrySystemState state) => s_store.TrySave(state);
        public static ZealotrySystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ZealotrySystemState state) => s_store.CapturePersisted(state);
    }
}
