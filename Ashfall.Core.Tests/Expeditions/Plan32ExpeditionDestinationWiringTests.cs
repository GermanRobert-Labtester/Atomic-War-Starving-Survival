using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class Plan32ExpeditionDestinationWiringTests : IDisposable
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _serializer;

        public Plan32ExpeditionDestinationWiringTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            }
            _fileIO = new FileSystemIO();
            _serializer = new SystemTextJsonSerializer();
            ExpeditionDefinitionRegistry.Clear();
        }

        public void Dispose()
        {
            ExpeditionDefinitionRegistry.Clear();
        }

        private sealed class TestExpeditionDto
        {
            public string id { get; set; } = string.Empty;
            public string displayName { get; set; } = string.Empty;
            public int distanceTicks { get; set; } = 8;
            public float travelHours { get; set; }
            public int dangerLevel { get; set; } = 1;
            public float encounterChancePerTick { get; set; } = 0.12f;
            public float baseStaminaDrainPerHour { get; set; } = 2.0f;
            public List<string>? lootCategories { get; set; }
        }

        private List<ExpeditionDefinition> LoadPrimaryExpeditions()
        {
            string primaryPath = _fileIO.Combine(_dataDir, "expeditions.json");
            string raw = _fileIO.ReadAllText(primaryPath);
            var dtos = CatalogLocator.LoadWrappedList<TestExpeditionDto>(raw, SystemTextJsonSerializer.Options);
            var list = new List<ExpeditionDefinition>();
            foreach (var dto in dtos)
            {
                if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                int ticks = dto.distanceTicks > 0
                    ? dto.distanceTicks
                    : (dto.travelHours > 0f ? (int)Math.Round(dto.travelHours * 2f) : 8);
                float encounterChance = dto.encounterChancePerTick > 0f
                    ? dto.encounterChancePerTick
                    : Math.Clamp(0.10f + dto.dangerLevel * 0.02f, 0.05f, 0.50f);
                float drain = dto.baseStaminaDrainPerHour > 0f
                    ? dto.baseStaminaDrainPerHour
                    : Math.Clamp(1.5f + dto.dangerLevel * 0.25f, 1.0f, 5.0f);

                var def = new ExpeditionDefinition
                {
                    id = dto.id,
                    displayName = !string.IsNullOrEmpty(dto.displayName) ? dto.displayName : dto.id,
                    distanceTicks = ticks > 0 ? ticks : 8,
                    dangerLevel = dto.dangerLevel > 0 ? dto.dangerLevel : 1,
                    encounterChancePerTick = encounterChance,
                    baseStaminaDrainPerHour = drain,
                    lootCategories = dto.lootCategories != null ? new List<string>(dto.lootCategories) : new List<string>()
                };
                list.Add(def);
                ExpeditionDefinitionRegistry.Register(def);
            }
            return list;
        }

        [Fact]
        public void ExpeditionsCatalog_LoadsExactly50Destinations()
        {
            var defs = LoadPrimaryExpeditions();
            Assert.Equal(53, defs.Count);
        }

        [Fact]
        public void Expeditions_All50HaveUniqueCanonicalIdsAndValidBounds()
        {
            var defs = LoadPrimaryExpeditions();
            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var def in defs)
            {
                Assert.False(string.IsNullOrEmpty(def.id), "Expedition ID must not be empty.");
                Assert.True(seenIds.Add(def.id), $"Duplicate expedition ID detected: {def.id}");
                Assert.False(string.IsNullOrEmpty(def.displayName), $"Display name missing for {def.id}");
                Assert.True(def.distanceTicks >= 1, $"distanceTicks must be >= 1 for {def.id}, was {def.distanceTicks}");
                Assert.True(def.dangerLevel >= 1 && def.dangerLevel <= 10, $"dangerLevel must be 1..10 for {def.id}, was {def.dangerLevel}");
                Assert.True(def.encounterChancePerTick >= 0.05f && def.encounterChancePerTick <= 0.50f,
                    $"encounterChancePerTick must be 0.05..0.50 for {def.id}, was {def.encounterChancePerTick}");
                Assert.True(def.baseStaminaDrainPerHour >= 1.0f && def.baseStaminaDrainPerHour <= 5.0f,
                    $"baseStaminaDrainPerHour must be 1.0..5.0 for {def.id}, was {def.baseStaminaDrainPerHour}");
                Assert.NotNull(def.lootCategories);
                Assert.True(def.lootCategories.Count > 0, $"lootCategories must have entries for {def.id}");
            }
        }

        [Fact]
        public void Expeditions_OriginalTwoRecordsArePreserved()
        {
            LoadPrimaryExpeditions();
            var allotments = ExpeditionDefinitionRegistry.Get("loc_the_allotments");
            var substation = ExpeditionDefinitionRegistry.Get("loc_denial_cut_substation");

            Assert.NotNull(allotments);
            Assert.Equal("The Works Allotment Commune", allotments.displayName);
            Assert.Equal(5, allotments.distanceTicks);
            Assert.Equal(2, allotments.dangerLevel);

            Assert.NotNull(substation);
            Assert.Equal("The Denial Cut Substation", substation.displayName);
            Assert.Equal(8, substation.distanceTicks);
            Assert.Equal(4, substation.dangerLevel);
        }

        [Fact]
        public void Expeditions_TierDistributionMatchesPlan()
        {
            var defs = LoadPrimaryExpeditions();
            int scavenge = 0;
            int standard = 0;
            int hazardous = 0;
            int deep = 0;

            foreach (var def in defs)
            {
                if (def.dangerLevel <= 3) scavenge++;
                else if (def.dangerLevel <= 5) standard++;
                else if (def.dangerLevel <= 7) hazardous++;
                else deep++;
            }

            Assert.Equal(16, scavenge);
            Assert.Equal(18, standard);
            Assert.Equal(13, hazardous);
            Assert.Equal(6, deep);
        }

        [Theory]
        [InlineData("suburban_house", 2, 2)]                         // Scavenge
        [InlineData("rural_gas_station", 3, 3)]                      // Scavenge
        [InlineData("checkpoint_kilo_armory", 6, 4)]                 // Standard
        [InlineData("loc_motel_verity", 6, 5)]                       // Standard
        [InlineData("abandoned_hospital", 4, 6)]                     // Hazardous
        [InlineData("loc_ordnance_shoulder", 8, 7)]                  // Hazardous
        [InlineData("government_bunker", 8, 8)]                      // Deep
        [InlineData("location_the_dead_hand_core", 18, 10)]          // Deep
        public void RepresentativeDestinations_CanDispatchAndComplete(string destId, int expectedDistance, int expectedDanger)
        {
            LoadPrimaryExpeditions();
            var def = ExpeditionDefinitionRegistry.Get(destId);
            Assert.NotNull(def);
            Assert.Equal(expectedDistance, def.distanceTicks);
            Assert.Equal(expectedDanger, def.dangerLevel);

            var sys = new ExpeditionSystem();
            bool completed = false;
            sys.OnExpeditionCompleted += s => completed = true;
            var rng = new SeededRng(42);
            Assert.True(sys.Start(def, "sv_scout", 1));

            // Outbound travel (0.5 hours per tick)
            for (int i = 0; i < def.distanceTicks; i++)
            {
                sys.TickHours(0.5f, rng);
            }

            var exp = sys.Active["sv_scout"];
            Assert.Equal((int)ExpeditionPhase.Looting, exp.phase);

            // Looting ticks (3 ticks auto-retreat)
            for (int i = 0; i < 3; i++)
            {
                sys.TickHours(0.5f, rng);
            }

            exp = sys.Active["sv_scout"];
            Assert.Equal((int)ExpeditionPhase.Inbound, exp.phase);

            // Inbound travel (0.5 hours per tick)
            for (int i = 0; i < def.distanceTicks; i++)
            {
                sys.TickHours(0.5f, rng);
            }

            Assert.Equal(0, sys.ActiveCount);
            Assert.True(completed, "Expedition should have triggered OnExpeditionCompleted");
        }

        [Fact]
        public void MidExpeditionSaveAndRestore_MaintainsDestinationIntegrity()
        {
            LoadPrimaryExpeditions();
            var def = ExpeditionDefinitionRegistry.Get("location_arcology_sector_4");
            Assert.NotNull(def);

            var sys = new ExpeditionSystem();
            var rng = new SeededRng(99);
            sys.Start(def, "sv_veteran", 1);

            // Advance 4 ticks
            for (int i = 0; i < 4; i++)
            {
                sys.TickHours(0.5f, rng);
            }

            var state = sys.CaptureState();
            Assert.Single(state);
            Assert.Equal("location_arcology_sector_4", state[0].locationId);
            Assert.Equal(4, state[0].travelTicksCompleted);

            // Restore in a fresh system
            var restoredSys = new ExpeditionSystem();
            restoredSys.RestoreState(state);
            Assert.Equal(1, restoredSys.ActiveCount);

            var restoredExp = restoredSys.Active["sv_veteran"];
            Assert.Equal("location_arcology_sector_4", restoredExp.locationId);
            Assert.Equal(4, restoredExp.travelTicksCompleted);
            Assert.Equal(16, restoredExp.distanceTicks);
        }
    }
}
