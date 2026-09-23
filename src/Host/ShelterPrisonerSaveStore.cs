// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterPrisonerSaveStore
// Core State : Ashfall.Core.Shelter.ShelterPrisonerState
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Checksummed campaign-envelope adapter for the retired Plan 63 shelter
    /// prisoner state. ORPHAN-SEAL-W1 (2026-09-23): PrisonerSystem (Plan 179)
    /// is the single captive authority; this store is read-only migration input
    /// for <c>Main.MigrateLegacyShelterPrisoners</c> and its section is no
    /// longer registered or written (hence the private SectionName const — the
    /// registry gate must see no live section for it).
    /// </summary>
    public static class ShelterPrisonerSaveStore
    {
        public const string FileName = "shelter_prisoners_save.json";
        private const string SectionName = "shelter_prisoners";

        private static readonly SaveStore<ShelterPrisonerState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterPrisonerSaveStore),
            SchemaVersionedEnvelope<ShelterPrisonerState>.Encode,
            SchemaVersionedEnvelope<ShelterPrisonerState>.Decode);

        public static string TryCapturePersisted(ShelterPrisonerState state) => s_store.CapturePersisted(state);
        public static ShelterPrisonerState? TryLoad() => s_store.TryLoad();
    }
}
