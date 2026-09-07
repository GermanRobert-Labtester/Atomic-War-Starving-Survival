using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class NuclearCorePowerGridPublishTests
    {
        private static PowerGridSystem MakeGrid()
        {
            return new PowerGridSystem(
                new PowerGridState
                {
                    GenerationWatts = 800f,
                    FuelUnits = 100f,
                    BatteryCapacityWh = 4000f,
                    BatteryReserveWh = 0f
                },
                new[]
                {
                    new PowerGridRoom(
                        "room_test",
                        "Test Room",
                        100f,
                        PowerGridRoomPriority.Standard)
                },
                new SeededRng(98));
        }

        private static NuclearCoreLifecycleSystem MakeNuclear(
            Inventory.Inventory inventory,
            params NuclearCoreDefinition[] definitions)
        {
            return new NuclearCoreLifecycleSystem(
                inventory,
                new NuclearCoreCatalog(definitions),
                new SeededRng(98));
        }

        [Fact]
        public void ExternalContribution_IsIdempotent_AndZeroRemovesIt()
        {
            var grid = MakeGrid();

            Assert.True(grid.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId, 100f));
            Assert.Equal(900f, grid.GenerationWatts, 2);

            Assert.True(grid.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId, 100f));
            Assert.Single(grid.GenerationContributions);
            Assert.Equal(900f, grid.GenerationWatts, 2);

            Assert.True(grid.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId, 0f));
            Assert.Empty(grid.GenerationContributions);
            Assert.Equal(800f, grid.GenerationWatts, 2);
        }

        [Fact]
        public void ExternalContribution_IsFuelFree()
        {
            var withoutNuclear = MakeGrid();
            var withNuclear = MakeGrid();
            withNuclear.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId, 100f);

            var baseTick = withoutNuclear.TickDay(1, new SeededRng(98));
            var nuclearTick = withNuclear.TickDay(1, new SeededRng(98));

            Assert.Equal(baseTick.FuelConsumed, nuclearTick.FuelConsumed, 3);
        }

        [Fact]
        public void NuclearOutput_PublishesInstalledPassiveCores_AndScramRemovesOnlyThatOutput()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("item_scram_rtg", 1);
            var nuclear = MakeNuclear(
                inventory,
                new NuclearCoreDefinition
                {
                    id = "core_rtg_test",
                    powerClass = "RTG",
                    baseElectricalOutput = 100f,
                    emergencyShutdownItemId = "item_scram_rtg"
                },
                new NuclearCoreDefinition
                {
                    id = "core_sealed_test",
                    powerClass = "SealedCell",
                    baseElectricalOutput = 250f,
                    emergencyShutdownItemId = "item_scram_sealed"
                });
            var grid = MakeGrid();

            Assert.True(nuclear.TryInstallCore("rtg_1", "core_rtg_test"));
            Assert.True(nuclear.TryInstallCore("sealed_1", "core_sealed_test"));
            grid.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId,
                nuclear.GetTotalGenerationWatts());
            Assert.Equal(1150f, grid.GenerationWatts, 2);

            Assert.True(nuclear.TryEmergencyScram("rtg_1"));
            grid.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId,
                nuclear.GetTotalGenerationWatts());
            Assert.Equal(1050f, grid.GenerationWatts, 2);
        }

        [Fact]
        public void NuclearState_RestoreThenRepublish_RehydratesGridProjection()
        {
            var sourceInventory = new Inventory.Inventory();
            var source = MakeNuclear(
                sourceInventory,
                new NuclearCoreDefinition
                {
                    id = "core_rtg_test",
                    powerClass = "RTG",
                    baseElectricalOutput = 100f,
                    emergencyShutdownItemId = "item_scram"
                });
            Assert.True(source.TryInstallCore("rtg_1", "core_rtg_test"));
            var save = source.CaptureState();

            var restored = MakeNuclear(
                new Inventory.Inventory(),
                new NuclearCoreDefinition
                {
                    id = "core_rtg_test",
                    powerClass = "RTG",
                    baseElectricalOutput = 100f,
                    emergencyShutdownItemId = "item_scram"
                });
            restored.RestoreState(save);

            var grid = MakeGrid();
            Assert.Empty(grid.GenerationContributions);
            grid.SetGenerationContribution(
                NuclearCoreLifecycleSystem.PowerSourceId,
                restored.GetTotalGenerationWatts());

            Assert.Equal(900f, grid.GenerationWatts, 2);
            Assert.Equal(100f, grid.Snapshot().GenerationContributions[
                NuclearCoreLifecycleSystem.PowerSourceId], 2);
        }
    }
}
