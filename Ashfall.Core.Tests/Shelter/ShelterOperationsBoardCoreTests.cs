// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using PlayerInventory = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterOperationsBoardCoreTests
    {
        private static string ResolveConstructionCatalog()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found))
                return Path.Combine(found, "shelter_construction.json");
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return Path.Combine(found, "shelter_construction.json");
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private static ShelterExpansionSystem CreateSystem()
        {
            var system = new ShelterExpansionSystem();
            system.LoadCatalog(File.ReadAllText(ResolveConstructionCatalog()));
            return system;
        }

        private static PlayerInventory CreateInventory(int scrapMetal, int scrapWood)
        {
            var inventory = new PlayerInventory { MaxWeight = 1000f };
            inventory.TryProduce("scrap_metal", scrapMetal);
            inventory.TryProduce("scrap_wood", scrapWood);
            return inventory;
        }

        [Fact]
        public void Construction_BillsAtomicallyAndRejectsUnsafeStability()
        {
            var system = CreateSystem();
            var inventory = CreateInventory(10, 9);

            var insufficient = system.TryStartRoomConstruction(
                "bp_deep_bunkhouse", 1, 0, 1, 1, inventory);

            Assert.False(insufficient.Succeeded);
            Assert.Equal(ConstructionStartCode.InsufficientResources, insufficient.Code);
            Assert.Empty(system.GetAllProjects());
            Assert.Equal(10, inventory.CountById("scrap_metal"));
            Assert.Equal(9, inventory.CountById("scrap_wood"));

            inventory.TryProduce("scrap_wood", 1);
            system.AdjustStability(-57);
            var unsafeResult = system.TryStartRoomConstruction(
                "bp_deep_bunkhouse", 1, 0, 1, 1, inventory);

            Assert.False(unsafeResult.Succeeded);
            Assert.Equal(ConstructionStartCode.UnsafeStability, unsafeResult.Code);
            Assert.Empty(system.GetAllProjects());
            Assert.Equal(10, inventory.CountById("scrap_metal"));
            Assert.Equal(10, inventory.CountById("scrap_wood"));

            system.AdjustStability(57);
            var started = system.TryStartRoomConstruction(
                "bp_deep_bunkhouse", 1, 0, 1, 1, inventory);

            Assert.True(started.Succeeded);
            Assert.NotNull(started.Project);
            Assert.Equal(ConstructionStartCode.Started, started.Code);
            Assert.Equal(0, inventory.CountById("scrap_metal"));
            Assert.Equal(0, inventory.CountById("scrap_wood"));
        }

        [Fact]
        public void DepthExcavation_IsUniqueAndUnlocksOneLevelWhenComplete()
        {
            var system = CreateSystem();
            var inventory = new PlayerInventory { MaxWeight = 1000f };
            inventory.TryProduce("scrap_metal", 50);
            inventory.TryProduce("concrete_rubble", 30);
            inventory.TryProduce("wooden_plank", 12);

            var first = system.TryStartDepthExcavation(2, inventory);
            Assert.True(first.Succeeded);
            Assert.NotNull(first.Project);
            var duplicate = system.TryStartDepthExcavation(3, inventory);
            Assert.False(duplicate.Succeeded);
            Assert.Equal(ConstructionStartCode.DuplicateProject, duplicate.Code);

            Assert.True(system.ProgressProject(first.Project!.ProjectId, 14, 3));
            Assert.Equal(2, system.MaxDepthUnlocked);
            Assert.Equal(25, inventory.CountById("scrap_metal"));
            Assert.Equal(15, inventory.CountById("concrete_rubble"));
            Assert.Equal(6, inventory.CountById("wooden_plank"));
        }

        [Fact]
        public void Crews_AreExclusiveBoundedAndProgressWithFatigueAndPractice()
        {
            var system = CreateSystem();
            var inventory = CreateInventory(10, 10);
            var started = system.TryStartRoomConstruction(
                "bp_deep_bunkhouse", 1, 0, 1, 1, inventory);
            string projectId = started.Project!.ProjectId;
            system.IsCrewSurvivorEligible = survivorId => survivorId != "unfit";
            var practice = new List<string>();
            var fatigue = new List<float>();
            system.ResolveCrewSkillBonus = (survivorId, skillId) =>
            {
                Assert.Equal("skill_crafting", skillId);
                return 0.25f;
            };
            system.RecordCrewSkillPractice = (survivorId, skillId, xp, day) =>
                practice.Add($"{survivorId}:{skillId}:{xp}:{day}");
            system.ApplyCrewFatigue = (survivorId, amount) => fatigue.Add(amount);

            Assert.False(system.TryAssignCrew(projectId, "unfit"));
            Assert.True(system.TryAssignCrew(projectId, "worker_a"));
            Assert.True(system.TryAssignCrew(projectId, "worker_b"));
            Assert.True(system.TryAssignCrew(projectId, "worker_c"));
            Assert.False(system.TryAssignCrew(projectId, "worker_d"));
            Assert.False(system.TryAssignCrew("missing", "worker_d"));
            Assert.False(system.TryAssignCrew(projectId, "worker_a"));

            Assert.Equal(0, system.ProgressAssignedProjects(2));
            Assert.Equal(3, practice.Count);
            Assert.All(practice, entry => Assert.Contains(":skill_crafting:1:2", entry));
            Assert.Equal(3, fatigue.Count);
            Assert.All(fatigue, amount => Assert.Equal(2f, amount));

            var savedProject = system.GetProject(projectId)!;
            Assert.Equal(new[] { "worker_a", "worker_b", "worker_c" }, savedProject.CrewSurvivorIds);
            var state = system.CaptureState();
            var restored = CreateSystem();
            restored.RestoreState(state);
            Assert.Equal(savedProject.CrewSurvivorIds, restored.GetProject(projectId)!.CrewSurvivorIds);
        }

        [Fact]
        public void CompletedBlueprintCapacityBonus_ReprojectsWithoutStacking()
        {
            var system = CreateSystem();
            var project = system.StartRoomConstruction("bp_deep_bunkhouse", 1, 0, 1, 1)!;
            Assert.True(system.ProgressProject(project.ProjectId, project.LaborRequiredDays, 2));
            var bonuses = system.GetCompletedCapacityBonuses();

            var assignment = new ShelterAssignmentSystem(
                new ShelterAssignmentState(),
                new[] { new ShelterRoom("room_bunks", "Bunks", 4) },
                new SeededRng(42));
            assignment.ApplyCapacityBonuses(bonuses);
            Assert.Equal(8, assignment.GetRoomCapacity("room_bunks"));
            assignment.ApplyCapacityBonuses(bonuses);
            Assert.Equal(8, assignment.GetRoomCapacity("room_bunks"));
        }
    }
}
