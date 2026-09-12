// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : FluidLogisticsSaveStore
// Core State : Ashfall.Core.Shelter.FluidLogisticsState
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope adapter for fluid topology state.</summary>
    public static class FluidLogisticsSaveStore
    {
        public const string FileName = "fluid_logistics_save.json";
        public const string SectionName = "fluid_logistics";

        private static readonly SaveStore<FluidLogisticsState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(FluidLogisticsSaveStore),
            SchemaVersionedEnvelope<FluidLogisticsState>.Encode,
            SchemaVersionedEnvelope<FluidLogisticsState>.Decode);

        public static string TryCapturePersisted(FluidLogisticsState state) => s_store.CapturePersisted(state);
        public static FluidLogisticsState? TryLoad() => s_store.TryLoad();
    }
}
