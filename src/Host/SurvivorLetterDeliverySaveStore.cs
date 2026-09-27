// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SurvivorLetterDeliverySaveStore
// ----------------------------------------------------------------------------
// Thin façade over the Core SaveStore service for the dead-letter delivery
// ledger. Same checksummed canonical envelope and atomic write behaviour as
// every other orphan-seal section; no bespoke persistence logic here.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class SurvivorLetterDeliverySaveStore
    {
        public const string FileName = "survivor_letter_delivery_save.json";
        public const string SectionName = "survivor_letter_delivery";

        private static readonly SaveStore<SurvivorLetterDeliverySaveState> s_store =
            SaveStoreHub.Checksummed<SurvivorLetterDeliverySaveState>(FileName, nameof(SurvivorLetterDeliverySaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists() => s_store.Exists();

        public static bool TrySave(SurvivorLetterDeliverySaveState state) => s_store.TrySave(state);

        public static SurvivorLetterDeliverySaveState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(SurvivorLetterDeliverySaveState state) => s_store.CapturePersisted(state);

        public static SurvivorLetterDeliverySaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
