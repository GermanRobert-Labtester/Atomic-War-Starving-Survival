// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MigrationConsequenceSaveStore
// Core State : Ashfall.Core.Economy.MigrationConsequenceSaveState
// Host Caller: Main.MigrationConsequence (Setup / Save)
// Purpose    : XP-08-F6 — exactly-once seasonal migration consequence ledger.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class MigrationConsequenceSaveStore
    {
        public const string FileName = "migration_consequence_save.json";
        public const string SectionName = "migration_consequence";

        private static readonly SaveStore<MigrationConsequenceSaveState> s_store =
            SaveStoreHub.Checksummed<MigrationConsequenceSaveState>(FileName, nameof(MigrationConsequenceSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(MigrationConsequenceSaveState state) => s_store.TrySave(state);
        public static MigrationConsequenceSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(MigrationConsequenceSaveState state) => s_store.CaptureBare(state);
        public static MigrationConsequenceSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
    }
}
