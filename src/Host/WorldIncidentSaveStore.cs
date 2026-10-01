// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope persistence for WorldIncidentSystem state.</summary>
    public static class WorldIncidentSaveStore
    {
        public const string FileName = "world_incidents_save.json";
        public const string SectionName = "world_incidents";

        private static readonly SaveStore<WorldIncidentState> s_store =
            SaveStoreHub.Checksummed<WorldIncidentState>(FileName, nameof(WorldIncidentSaveStore));

        public static string TryCapturePersisted(WorldIncidentState state) => s_store.CapturePersisted(state);
        public static string TryCaptureDirect(WorldIncidentState state) => s_store.CaptureBare(state);
        public static WorldIncidentState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static WorldIncidentState? TryLoad() => s_store.TryLoad();
    }
}
