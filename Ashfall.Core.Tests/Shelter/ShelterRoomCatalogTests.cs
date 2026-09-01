using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class ShelterRoomCatalogTests
    {
        private static string GetDataPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void DefaultCatalog_Contains22RoomsAnd12Rules()
        {
            var catalog = ShelterRoomCatalogLoader.GetDefaultCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(1, catalog.schema_version);
            Assert.True(catalog.rooms.Count >= 20, $"Expected at least 20 rooms, got {catalog.rooms.Count}");
            Assert.Equal(12, catalog.assignment_rules.Count);
        }

        [Fact]
        public void LoadFromFile_ParsesCorrectly()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);
            Assert.NotNull(catalog);
            Assert.True(catalog.rooms.Count >= 20);
            Assert.Equal(12, catalog.assignment_rules.Count);

            var corridor = catalog.rooms.FirstOrDefault(r => r.id == "room_bunker_corridor");
            Assert.NotNull(corridor);
            Assert.Equal(0, corridor.capacity);
            Assert.Equal("Corridor", corridor.function);

            var kitchen = catalog.rooms.FirstOrDefault(r => r.id == "room_kitchen");
            Assert.NotNull(kitchen);
            Assert.Equal("Kitchen", kitchen.function);
            Assert.Equal(2, kitchen.capacity);
        }

        [Fact]
        public void AllRoomIds_AreUniqueAndValidPrefix()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);

            var ids = catalog.rooms.Select(r => r.id).ToList();
            var distinctIds = ids.Distinct().ToList();
            Assert.Equal(ids.Count, distinctIds.Count);

            foreach (var id in ids)
            {
                Assert.StartsWith("room_", id);
            }
        }

        [Fact]
        public void AllRuleIds_AreUniqueAndValidPrefix()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);

            var ids = catalog.assignment_rules.Select(r => r.id).ToList();
            var distinctIds = ids.Distinct().ToList();
            Assert.Equal(ids.Count, distinctIds.Count);

            foreach (var id in ids)
            {
                Assert.StartsWith("rule_", id);
                Assert.False(string.IsNullOrWhiteSpace(id));
            }
        }

        [Fact]
        public void AssignmentRules_TargetValidFunctions()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);

            var roomFunctions = catalog.rooms.Select(r => r.function).Distinct().ToHashSet();

            foreach (var rule in catalog.assignment_rules)
            {
                Assert.Contains(rule.target_room_function, roomFunctions);
                Assert.True(rule.bonus_magnitude > 0);
            }
        }

        [Fact]
        public void ShelterAssignmentSystem_LoadsFromCatalogRooms()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);

            var rooms = catalog.rooms.Select(r => new ShelterRoom(r.id, r.display_name, r.capacity, r.required_skill_id, r.workstation_id)).ToList();
            var rng = new SeededRng(42);
            var system = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);

            Assert.Equal(catalog.rooms.Count, system.Rooms.Count);

            // Assign a survivor to the kitchen
            var result = system.Assign("chef_elena", "room_kitchen", day: 1);
            Assert.True(result.Succeeded);
            Assert.Equal(1, system.GetRoomOccupancy("room_kitchen"));
        }

        [Fact]
        public void DormitoryVariants_ReflectCapacityAndCostTradeoffs()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);

            var crowded = catalog.rooms.First(r => r.id == "room_bunks_crowded");
            var standard = catalog.rooms.First(r => r.id == "room_bunks");
            var privateQ = catalog.rooms.First(r => r.id == "room_quarters_private");

            Assert.True(crowded.capacity > standard.capacity);
            Assert.True(standard.capacity > privateQ.capacity);
        }

        [Fact]
        public void WorkshopVariants_TargetDistinctDisciplines()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);

            var general = catalog.rooms.First(r => r.id == "room_workshop");
            var heavy = catalog.rooms.First(r => r.id == "room_workshop_heavy");
            var precision = catalog.rooms.First(r => r.id == "room_workshop_precision");

            Assert.Contains("repair", general.tags);
            Assert.Contains("heavy_industrial", heavy.tags);
            Assert.Contains("precision", precision.tags);
        }

        [Fact]
        public void SaveRoundTrip_WithCatalogRooms_PreservesState()
        {
            string dataDir = GetDataPath();
            var catalog = ShelterRoomCatalogLoader.Load(dataDir);
            var rooms = catalog.rooms.Select(r => new ShelterRoom(r.id, r.display_name, r.capacity, r.required_skill_id, r.workstation_id)).ToList();

            var sys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(100));
            sys.Assign("survivor_1", "room_bunks", day: 2);
            sys.Assign("survivor_2", "room_kitchen", day: 2);

            var state = sys.CaptureState();
            Assert.Equal(2, state.Assignments.Count);

            var newSys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(101));
            newSys.RestoreState(state);

            Assert.Equal(2, newSys.GetAssignments().Count);
            Assert.True(newSys.AreInSameRoom("survivor_1", "survivor_1") == false);
            Assert.Equal("room_bunks", newSys.GetAssignmentForSurvivor("survivor_1")?.RoomId);
            Assert.Equal("room_kitchen", newSys.GetAssignmentForSurvivor("survivor_2")?.RoomId);
        }
    }
}
