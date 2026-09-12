// SPDX-License-Identifier: MIT
using Ashfall.Core.Save;
using Ashfall.Core.SkyDefense;

namespace AtomicWar.GodotApp
{
    /// <summary>Host façade for the sky_defense_battery save section (flagship Task 7).</summary>
    public static class SkyDefenseBatterySaveStore
    {
        public const string FileName = "sky_defense_battery_save.json";
        public const string SectionName = "sky_defense_battery";

        private static readonly SaveStore<SkyDefenseBatterySave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SkyDefenseBatterySaveStore),
            SchemaVersionedEnvelope<SkyDefenseBatterySave>.Encode,
            SchemaVersionedEnvelope<SkyDefenseBatterySave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SkyDefenseBatterySave state) => s_store.TrySave(state);
        public static SkyDefenseBatterySave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SkyDefenseBatterySave state) => s_store.CapturePersisted(state);
    }
}
