// SPDX-License-Identifier: MIT
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope persistence for EchoSystem state.</summary>
    public static class EchoSaveStore
    {
        public const string FileName = "echoes_save.json";
        public const string SectionName = "echoes";

        private static readonly SaveStore<EchoState> s_store =
            SaveStoreHub.Checksummed<EchoState>(FileName, nameof(EchoSaveStore));

        public static string TryCapturePersisted(EchoState state) => s_store.CapturePersisted(state);
        public static string TryCaptureDirect(EchoState state) => s_store.CaptureBare(state);
        public static EchoState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static EchoState? TryLoad() => s_store.TryLoad();
    }
}
