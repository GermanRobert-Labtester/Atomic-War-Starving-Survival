// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AdvancedIndustrialSaveStore
// Core State : FischerTropschSynthesisState + CarbonCompositeState +
//              UvCoronaDetectionState + GroundPenetratingRadarState
// Host Caller: Main.AdvancedIndustrial
// Purpose    : Plans 118-121 - synthetic lubricant, UV corona detection,
//              carbon composites, and ground-penetrating radar.
// ============================================================================

using System;
using Ashfall.Core.Radio;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    [Serializable]
    public sealed class AdvancedIndustrialSaveState
    {
        public int schema_version = 1;
        public int day;
        public FischerTropschSynthesisState synthesis = new FischerTropschSynthesisState();
        public CarbonCompositeState composites = new CarbonCompositeState();
        public UvCoronaDetectionState uv = new UvCoronaDetectionState();
        public GroundPenetratingRadarState gpr = new GroundPenetratingRadarState();
    }

    public static class AdvancedIndustrialSaveStore
    {
        public const string FileName = "advanced_industrial_save.json";
        public const string SectionName = "advanced_industrial";

        private static readonly SaveStore<AdvancedIndustrialSaveState> s_store =
            SaveStoreHub.Checksummed<AdvancedIndustrialSaveState>(FileName, nameof(AdvancedIndustrialSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(AdvancedIndustrialSaveState state) => s_store.CaptureBare(state);
        public static AdvancedIndustrialSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(AdvancedIndustrialSaveState state) => s_store.TrySave(state);
        public static AdvancedIndustrialSaveState? TryLoad() => s_store.TryLoad();
    }
}
