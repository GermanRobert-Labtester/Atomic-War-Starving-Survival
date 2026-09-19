// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : HiddenAgendaSaveStore
// Core State : Ashfall.Core.Survivors.HiddenAgendaState
// Host Caller: Main.HiddenAgenda / HiddenAgendaHostSession
// Purpose    : Plan 132 — survivor hidden agendas, clues, and betrayal arcs
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HiddenAgendaSaveStore
    {
        public const string FileName = "hidden_agenda_save.json";
        public const string SectionName = "hidden_agenda";

        private static readonly SaveStore<HiddenAgendaState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(HiddenAgendaSaveStore),
            SchemaVersionedEnvelope<HiddenAgendaState>.Encode,
            SchemaVersionedEnvelope<HiddenAgendaState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(HiddenAgendaState state) => s_store.TrySave(state);
        public static HiddenAgendaState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(HiddenAgendaState state) => s_store.CapturePersisted(state);
    }
}
