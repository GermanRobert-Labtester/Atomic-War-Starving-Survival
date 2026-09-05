// SPDX-License-Identifier: MIT
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Armored crawler expedition persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class ArmoredCrawlerSaveStore
    {
        public const string FileName = "armored_crawlers_save.json";
        public const string SectionName = "armored_crawlers";

        private static readonly SaveStore<ArmoredCrawlerExpeditionSave> s_store =
            SaveStoreHub.Checksummed<ArmoredCrawlerExpeditionSave>(
                FileName,
                nameof(ArmoredCrawlerSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(ArmoredCrawlerExpeditionSave state) => s_store.CaptureBare(state);
        public static ArmoredCrawlerExpeditionSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(ArmoredCrawlerExpeditionSave state) => s_store.CaptureBare(state);
        public static ArmoredCrawlerExpeditionSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ArmoredCrawlerExpeditionSave state) => s_store.TrySave(state);
        public static ArmoredCrawlerExpeditionSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(ArmoredCrawlerExpeditionSave state) => s_store.CapturePersisted(state);
    }
}
