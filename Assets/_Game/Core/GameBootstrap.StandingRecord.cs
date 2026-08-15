using System.Collections.Generic;
using Ashfall.Core;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: THE STANDING RECORD — Unity host wiring.
    /// Location layouts (rooms), location memory (strata recasts), site encounters
    /// (room-keyed with Overlay access), and the record quest catalog.
    /// Spec: docs/expansions/expansion_03_the_standing_record_plan.md §5, §7.1.
    /// </summary>
    public partial class GameBootstrap
    {
        public LocationLayoutSystem LocationLayoutSystem { get; private set; }
        public LocationMemorySystem LocationMemorySystem { get; private set; }
        public SiteEncounterSystem SiteEncounterSystem { get; private set; }
        public StandingRecordCatalog RecordCatalog { get; private set; }

        private void BootStandingRecord()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            LocationLayoutSystem = new LocationLayoutSystem(files, json, GameLogAdapter.Instance);
            LocationLayoutSystem.Load(UnityEngine.Application.streamingAssetsPath + "/Data");

            LocationMemorySystem = new LocationMemorySystem(files, json, GameLogAdapter.Instance);
            LocationMemorySystem.Load(UnityEngine.Application.streamingAssetsPath + "/Data");

            SiteEncounterSystem = new SiteEncounterSystem(_worldSeed + 1808);

            RecordCatalog = new StandingRecordCatalogLoader(files, json, GameLogAdapter.Instance)
                .Load(UnityEngine.Application.streamingAssetsPath + "/Data");

            _registry.RegisterEventDriven("location_layout_system");
            _registry.RegisterEventDriven("location_memory_system");
            _registry.RegisterEventDriven("site_encounter_system");

            if (SaveSystem != null)
                SaveSystem.SetWorldFlag(LocationLayoutSystem.FlagExpUnlocked,
                    LocationLayoutSystem.IsUnlocked);

            GameLog.Log("[GameBootstrap] Standing Record booted: " + LocationLayoutSystem.LayoutCount
                + " layouts · " + LocationMemorySystem.StratumCount + " strata · "
                + (RecordCatalog?.Quests?.Count ?? 0) + " quests.");
        }

        private sealed class GameLogAdapter : ILog
        {
            public static readonly GameLogAdapter Instance = new GameLogAdapter();
            public void Info(string message) => GameLog.Log(message);
            public void Warn(string message) => GameLog.Log("[warn] " + message);
            public void Error(string message) => GameLog.Log("[error] " + message);
        }
    }
}