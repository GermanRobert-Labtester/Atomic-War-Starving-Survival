// SPDX-License-Identifier: MIT
using Ashfall.Core.Flags;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="CampaignConsequenceSaveState"/> as checksummed JSON —
    /// thin façade over the Core SaveStore&lt;T&gt; service. The ledger holds
    /// campaign consequence flags, counters, and history that gate quest
    /// prerequisites and moral-choice outcomes. Persisted across saves,
    /// cleared on new campaigns via the lifecycle registry.
    /// </summary>
    public static class ConsequenceLedgerSaveStore
    {
        public const string FileName = "consequence_ledger_save.json";
        public const string SectionName = "consequence_ledger";

        private static readonly SaveStore<CampaignConsequenceSaveState> s_store =
            SaveStoreHub.Checksummed<CampaignConsequenceSaveState>(
                FileName,
                nameof(ConsequenceLedgerSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture state to JSON for the campaign envelope without writing to disk.</summary>
        public static string TryCapture(CampaignConsequenceSaveState state) =>
            s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static CampaignConsequenceSaveState? TryRestore(string json) =>
            s_store.RestoreBare(json);

        /// <summary>Writes through the checksummed envelope. Returns false on failure.</summary>
        public static bool TrySave(CampaignConsequenceSaveState state) =>
            s_store.TrySave(state);

        /// <summary>Reads and validates through the envelope. Returns null when absent or corrupt.</summary>
        public static CampaignConsequenceSaveState? TryLoad() =>
            s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(CampaignConsequenceSaveState state) =>
            s_store.CapturePersisted(state);
    }
}
