// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : FoodPreservationSaveStore
// Core State : Ashfall.Core.Shelter.FoodPreservationState
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope adapter for food preservation state.</summary>
    public static class FoodPreservationSaveStore
    {
        public const string FileName = "food_preservation_save.json";
        public const string SectionName = "food_preservation";

        private static readonly SaveStore<FoodPreservationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(FoodPreservationSaveStore),
            SchemaVersionedEnvelope<FoodPreservationState>.Encode,
            SchemaVersionedEnvelope<FoodPreservationState>.Decode);

        public static string TryCapturePersisted(FoodPreservationState state) => s_store.CapturePersisted(state);
        public static FoodPreservationState? TryLoad() => s_store.TryLoad();
    }
}
