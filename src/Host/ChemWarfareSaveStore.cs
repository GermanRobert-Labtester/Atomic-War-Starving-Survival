// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ChemWarfareSaveStore
// Core State : Ashfall.Core.Combat.ChemWarfareSaveState
// Host Caller: Main.Plans198_201
// Purpose    : Fictionalized CBRN hazard warfare, active gas zones & contamination
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ChemWarfareSaveStore
    {
        public const string FileName = "chem_warfare_save.json";
        public const string SectionName = "chem_warfare";

        private static readonly SaveStore<ChemWarfareSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ChemWarfareSaveStore),
            SchemaVersionedEnvelope<ChemWarfareSaveState>.Encode,
            SchemaVersionedEnvelope<ChemWarfareSaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ChemWarfareSaveState state) => s_store.TrySave(state);
        public static ChemWarfareSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ChemWarfareSaveState state) => s_store.CapturePersisted(state);
    }
}
