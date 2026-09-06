// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterPrisonerSaveStore
// Core State : Ashfall.Core.Shelter.ShelterPrisonerState
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope adapter for shelter prisoner state.</summary>
    public static class ShelterPrisonerSaveStore
    {
        public const string FileName = "shelter_prisoners_save.json";
        public const string SectionName = "shelter_prisoners";

        private static readonly SaveStore<ShelterPrisonerState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterPrisonerSaveStore),
            SchemaVersionedEnvelope<ShelterPrisonerState>.Encode,
            SchemaVersionedEnvelope<ShelterPrisonerState>.Decode);

        public static string TryCapturePersisted(ShelterPrisonerState state) => s_store.CapturePersisted(state);
        public static ShelterPrisonerState? TryLoad() => s_store.TryLoad();
    }
}
