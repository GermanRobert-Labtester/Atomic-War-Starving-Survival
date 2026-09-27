// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SpiritualRitualSaveStore
// Core State : Ashfall.Core.Spiritual.SpiritualRitualSaveState
// Host Caller: Main.SpiritualRitual (SetupSpiritualRitual / SaveSpiritualRitual)
// Purpose    : EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED — ritual cooldown
//              ledger. Bounded: last-performed day per authored ritual id.
//              It is NOT a piety meter and NOT a second ritual registry.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Spiritual;

namespace AtomicWar.GodotApp
{
    public static class SpiritualRitualSaveStore
    {
        public const string FileName = "spiritual_ritual_save.json";
        public const string SectionName = "spiritual_ritual";

        private static readonly SaveStore<SpiritualRitualSaveState> s_store =
            SaveStoreHub.Checksummed<SpiritualRitualSaveState>(FileName, nameof(SpiritualRitualSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(SpiritualRitualSaveState state) => s_store.TrySave(state);
        public static SpiritualRitualSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SpiritualRitualSaveState state) => s_store.CaptureBare(state);
        public static SpiritualRitualSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
    }
}
