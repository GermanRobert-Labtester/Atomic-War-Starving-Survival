// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : BionicsSaveStore
// Core State : Ashfall.Core.Medical.BionicsSystemState
// Host Caller: Main.Bionics
// Purpose    : Plan 177 — bionic implant instances (condition, integration,
//              power state, maintenance, complications). The limb/socket
//              truth stays in the amputation section; this store owns only
//              the implant components keyed to it.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class BionicsSaveStore
    {
        public const string FileName = "bionics_save.json";
        public const string SectionName = "bionics";

        private static readonly SaveStore<BionicsSystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(BionicsSaveStore),
            SchemaVersionedEnvelope<BionicsSystemState>.Encode,
            SchemaVersionedEnvelope<BionicsSystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(BionicsSystemState state) => s_store.TrySave(state);
        public static BionicsSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(BionicsSystemState state) => s_store.CapturePersisted(state);
    }
}
