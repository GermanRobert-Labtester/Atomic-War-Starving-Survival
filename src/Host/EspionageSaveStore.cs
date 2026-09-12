// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : EspionageSaveStore
// Core State : Ashfall.Core.Factions.EspionageState
// ============================================================================
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope adapter for espionage state.</summary>
    public static class EspionageSaveStore
    {
        public const string FileName = "espionage_save.json";
        public const string SectionName = "espionage";

        private static readonly SaveStore<EspionageState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(EspionageSaveStore),
            SchemaVersionedEnvelope<EspionageState>.Encode,
            SchemaVersionedEnvelope<EspionageState>.Decode);

        public static string TryCapturePersisted(EspionageState state) => s_store.CapturePersisted(state);
        public static EspionageState? TryLoad() => s_store.TryLoad();
    }
}
