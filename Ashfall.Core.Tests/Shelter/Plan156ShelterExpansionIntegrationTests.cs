// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core Tests : Plan 156 — Shelter Expansion & Physical Renovation
// Subsystem          : ShelterExpansionSystem / Grid Construction & Renovation Tests
// Authority          : Next-steps-plans/Plan_156_Shelter_Expansion_Physical_Renovation.md
//                      UNBLOCK-PROGRAM-WAVE32-BATCH5-PLANS (DEC-152)
// ============================================================================
using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Plan156Shelter
{
    public sealed class Plan156ShelterExpansionIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void Catalog_Loads_BlueprintsAndUpgradesSuccessfully()
        {
            var system = new ShelterExpansionSystem();
            string path = ResolveDataPath("shelter_construction.json");
            Assert.True(File.Exists(path), $"shelter_construction.json missing at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            var project = system.StartRoomConstruction("bp_hydroponic_bay", 1, 0, 1, currentDay: 5);
            Assert.NotNull(project);
            Assert.Equal("bp_hydroponic_bay", project.BlueprintId);
            Assert.Equal(8.0, project.LaborRequiredDays);
        }

        [Fact]
        public void NewRoomConstruction_ProgressionAndStabilityCost()
        {
            var system = new ShelterExpansionSystem();
            double initialStability = system.StabilityRating;

            var proj = system.StartRoomConstruction("bp_deep_bunkhouse", 2, 0, 1, currentDay: 10);
            Assert.NotNull(proj);

            // Progress labor by 3.0 days (not completed yet)
            bool completed = system.ProgressProject(proj.ProjectId, 3.0, currentDay: 13);
            Assert.False(completed);

            // Progress remainder of labor (3.0 more days -> total 6.0 >= 6.0)
            completed = system.ProgressProject(proj.ProjectId, 3.0, currentDay: 16);
            Assert.True(completed);

            var completedProj = system.GetProject(proj.ProjectId)!;
            Assert.Equal(ProjectStatus.Completed, completedProj.Status);
            Assert.Equal(16, completedProj.CompletionDay);

            // The room is now created and constructed
            var newRoom = system.GetRoom("room_2_0_d1");
            Assert.NotNull(newRoom);
            Assert.True(newRoom.IsConstructed);
            Assert.Equal(100.0, newRoom.Condition);

            // Stability rating dropped by blueprint stability cost (4.0)
            Assert.True(system.StabilityRating < initialStability);
            Assert.Equal(1, system.TotalRoomsConstructed);
        }

        [Fact]
        public void OccupiedCell_PreventsOverlappingConstruction()
        {
            var system = new ShelterExpansionSystem();

            // Command hub is already at (0, 0, 1)
            var duplicateProj = system.StartRoomConstruction("bp_hydroponic_bay", 0, 0, 1, currentDay: 1);
            Assert.Null(duplicateProj);

            // Valid unoccupied cell
            var validProj = system.StartRoomConstruction("bp_hydroponic_bay", 0, 1, 1, currentDay: 1);
            Assert.NotNull(validProj);

            // Cell now occupied by active project
            var conflictingProj = system.StartRoomConstruction("bp_deep_bunkhouse", 0, 1, 1, currentDay: 2);
            Assert.Null(conflictingProj);
        }

        [Fact]
        public void Renovation_RestoresRoomConditionFromDamage()
        {
            var system = new ShelterExpansionSystem();
            // Damage command hub
            system.DegradeRoomCondition("room_command_hub", 35.0);
            var room = system.GetRoom("room_command_hub")!;
            Assert.Equal(65.0, room.Condition);

            // Start renovation
            var renovProj = system.StartRenovation("room_command_hub", currentDay: 20);
            Assert.NotNull(renovProj);
            Assert.Equal(ConstructionProjectType.Renovation, renovProj.ProjectType);

            // Complete renovation
            system.ProgressProject(renovProj.ProjectId, renovProj.LaborRequiredDays, currentDay: 22);

            var restoredRoom = system.GetRoom("room_command_hub")!;
            Assert.Equal(100.0, restoredRoom.Condition);
        }

        [Fact]
        public void Upgrade_InstallsAndRestoresStructuralStability()
        {
            var system = new ShelterExpansionSystem();
            // Lower stability to simulate heavy excavation
            system.AdjustStability(-30.0);
            double lowStability = system.StabilityRating;

            // Start structural strut upgrade on command hub
            var upgProj = system.StartUpgrade("room_command_hub", "upg_structural_pillar", currentDay: 30);
            Assert.NotNull(upgProj);

            // Complete upgrade
            system.ProgressProject(upgProj.ProjectId, upgProj.LaborRequiredDays, currentDay: 34);

            var room = system.GetRoom("room_command_hub")!;
            Assert.Contains("upg_structural_pillar", room.Upgrades);

            // Stability restored by 10.0
            Assert.Equal(lowStability + 10.0, system.StabilityRating);
        }

        [Fact]
        public void DepthExcavationAndSaveRestore_PreservesFullState()
        {
            var system = new ShelterExpansionSystem();
            Assert.Equal(1, system.MaxDepthUnlocked);

            // Excavate depth shaft
            var shaftProj = system.StartDepthExcavation(currentDay: 40);
            system.ProgressProject(shaftProj.ProjectId, shaftProj.LaborRequiredDays, currentDay: 52);

            Assert.Equal(2, system.MaxDepthUnlocked);

            // Capture state
            var state = system.CaptureState();
            Assert.Equal(1, state.SchemaVersion);
            Assert.Equal(2, state.MaxDepthUnlocked);

            // Restore in fresh instance
            var freshSystem = new ShelterExpansionSystem();
            freshSystem.RestoreState(state);

            Assert.Equal(2, freshSystem.MaxDepthUnlocked);
            Assert.Equal(system.StabilityRating, freshSystem.StabilityRating);
            var restoredShaft = freshSystem.GetProject(shaftProj.ProjectId);
            Assert.NotNull(restoredShaft);
            Assert.Equal(ProjectStatus.Completed, restoredShaft.Status);
        }

        [Fact]
        public void ProjectIds_AreDeterministic_AcrossIdenticalRuns()
        {
            string path = ResolveDataPath("shelter_construction.json");
            string json = File.ReadAllText(path);

            var a = new ShelterExpansionSystem();
            a.LoadCatalog(json);
            var b = new ShelterExpansionSystem();
            b.LoadCatalog(json);

            var pa = a.StartRoomConstruction("bp_hydroponic_bay", 1, 0, 1, currentDay: 5);
            var pb = b.StartRoomConstruction("bp_hydroponic_bay", 1, 0, 1, currentDay: 5);
            Assert.NotNull(pa);
            Assert.NotNull(pb);
            Assert.Equal(pa!.ProjectId, pb!.ProjectId);

            // Deterministic ids stay unique within one shelter.
            var p2 = a.StartRoomConstruction("bp_deep_bunkhouse", 2, 0, 1, currentDay: 5);
            Assert.NotNull(p2);
            Assert.NotEqual(pa.ProjectId, p2!.ProjectId);

            // Renovation ids share the same deterministic counter namespace.
            // Use mirrored fresh shelters so both sides hold identical state.
            var a2 = new ShelterExpansionSystem();
            a2.LoadCatalog(json);
            var b2 = new ShelterExpansionSystem();
            b2.LoadCatalog(json);
            a2.DegradeRoomCondition("room_command_hub", 35.0);
            b2.DegradeRoomCondition("room_command_hub", 35.0);
            var ra = a2.StartRenovation("room_command_hub", currentDay: 20);
            var rb = b2.StartRenovation("room_command_hub", currentDay: 20);
            Assert.NotNull(ra);
            Assert.NotNull(rb);
            Assert.Equal(ra!.ProjectId, rb!.ProjectId);
        }
    }
}
