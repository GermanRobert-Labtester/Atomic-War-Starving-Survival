// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 201: Shelter Sanitation & Waste Management System — Integration Tests
// Verifies sanitation facility catalog loading, waste accumulation scaling,
// facility processing, room cleaning duties, hygiene band derivation, and
// save/restore state persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan201SanitationIntegrationTests : CatalogTestBase
    {
        private static SanitationFacilityCatalog LoadCatalog()
        {
            var load = SanitationFacilityCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            return SanitationFacilityCatalogLoader.ToCatalog(load);
        }

        [Fact]
        public void LoadCatalog_LoadsAllAuthoredSanitationFacilities()
        {
            string path = Path.Combine(DataDirectory, "sanitation_facilities.json");
            Assert.True(File.Exists(path), $"sanitation_facilities.json must exist at {path}");

            var catalog = LoadCatalog();
            Assert.True(catalog.Count >= 7, $"Expected at least 7 facilities, got {catalog.Count}");

            var latrine = catalog.Find("sanitation_basic_latrine");
            Assert.NotNull(latrine);
            Assert.Equal("latrine", latrine.facility_type);
            Assert.Equal(40, latrine.capacity);
            Assert.Equal(6, latrine.processing_rate);
            Assert.Contains("organic", latrine.waste_types);

            var incinerator = catalog.Find("sanitation_incinerator");
            Assert.NotNull(incinerator);
            Assert.Equal("incinerator", incinerator.facility_type);
            Assert.Equal(6, incinerator.power_draw);
        }

        [Fact]
        public void RoomAccumulation_GeneratesOrganicWasteFromPopulation()
        {
            var catalog = LoadCatalog();
            var system = new SanitationSystem();
            system.BindFacilityCatalog(catalog);

            system.EnsureRoom("bunkhouse_north", RoomWasteRole.Residential);
            system.EnsureRoom("bunkhouse_south", RoomWasteRole.Residential);

            system.TickDaily(day: 1, population: 10);

            var north = system.FindRoom("bunkhouse_north");
            var south = system.FindRoom("bunkhouse_south");
            Assert.NotNull(north);
            Assert.NotNull(south);

            // 10 survivors distributed across 2 residential rooms = 5 units each
            Assert.Equal(5f, north.organic, 2);
            Assert.Equal(5f, south.organic, 2);
        }

        [Fact]
        public void FacilityProcessing_ReducesWasteWhenPoweredAndStaffed()
        {
            var catalog = LoadCatalog();
            var system = new SanitationSystem();
            system.BindFacilityCatalog(catalog);

            system.EnsureRoom("communal_latrine_room", RoomWasteRole.Residential);
            system.InstallFacility("sanitation_basic_latrine", "communal_latrine_room");

            // Tick day 1: 6 pop generates 6 organic waste, latrine processes 6 units
            system.TickDaily(day: 1, population: 6);

            var room = system.FindRoom("communal_latrine_room");
            Assert.NotNull(room);
            // 6 generated - 6 processed = 0 remaining
            Assert.Equal(0f, room.organic, 2);
        }

        [Fact]
        public void ApplyCleaning_RemovesWasteAndReportsCleaningResult()
        {
            var catalog = LoadCatalog();
            var system = new SanitationSystem();
            system.BindFacilityCatalog(catalog);

            system.EnsureRoom("workshop", RoomWasteRole.Industrial);
            // Simulate accumulation: 20 chemical waste
            system.TickDaily(day: 1, population: 0);
            var room = system.FindRoom("workshop")!;
            room.chemical = 20f;

            var result = system.ApplyCleaning("workshop", workers: 2, skill01: 0.6f, priority: CleaningPriority.High, day: 1);

            Assert.True(result.Accepted);
            Assert.True(result.ChemicalRemoved > 0f);
            Assert.True(room.chemical < 20f);
        }

        [Fact]
        public void GetShelterHygieneBand_ReflectsShelterContaminationLevels()
        {
            var catalog = LoadCatalog();
            var system = new SanitationSystem();
            system.BindFacilityCatalog(catalog);

            system.EnsureRoom("hab_a", RoomWasteRole.Residential);
            var room = system.FindRoom("hab_a")!;

            // Clean state
            room.organic = 0f;
            Assert.Equal(HygieneBand.Excellent, system.GetShelterHygieneBand());

            // Heavy accumulation
            room.organic = 45f;
            var degradedBand = system.GetShelterHygieneBand();
            Assert.True(degradedBand >= HygieneBand.Poor);
        }

        [Fact]
        public void SaveRestoreState_PreservesRoomsFacilitiesAndWasteLevels()
        {
            var catalog = LoadCatalog();
            var system = new SanitationSystem();
            system.BindFacilityCatalog(catalog);

            system.EnsureRoom("medical_ward", RoomWasteRole.Medical);
            system.InstallFacility("sanitation_air_scrubber", "medical_ward");
            var room = system.FindRoom("medical_ward")!;
            room.chemical = 12.5f;

            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.Single(captured.rooms);
            Assert.Single(captured.facilities);

            var restoredSystem = new SanitationSystem();
            restoredSystem.BindFacilityCatalog(catalog);
            restoredSystem.RestoreState(captured);

            var restoredRoom = restoredSystem.FindRoom("medical_ward");
            Assert.NotNull(restoredRoom);
            Assert.Equal(12.5f, restoredRoom.chemical, 2);
            var restoredFacilities = restoredSystem.CaptureState().facilities.Where(f => f.roomId == "medical_ward").ToList();
            Assert.Single(restoredFacilities);
        }
    }
}
