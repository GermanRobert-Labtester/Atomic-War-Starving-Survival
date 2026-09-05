// SPDX-License-Identifier: MIT
// ASHFALL campaign endgame & epilogue save store facade (Plan 84 / Task B25).

using Ashfall.Core.Endgame;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Campaign endgame persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class EndgameSaveStore
    {
        public const string FileName = "endgame_save.json";
        public const string SectionName = "endgame";

        private static readonly SaveStore<EndgameSaveState> s_store =
            SaveStoreHub.Checksummed<EndgameSaveState>(
                FileName,
                nameof(EndgameSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(EndgameSaveState state) => s_store.CaptureBare(state);
        public static EndgameSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(EndgameSaveState state) => s_store.CaptureBare(state);
        public static EndgameSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(EndgameSaveState state) => s_store.TrySave(state);
        public static EndgameSaveState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(EndgameSaveState state) => s_store.CapturePersisted(state);
    }
}
