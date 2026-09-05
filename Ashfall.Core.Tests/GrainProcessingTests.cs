using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class GrainProcessingTests
    {
        private static GrainProcessingSystem Create(out Inventory.Inventory inventory)
        {
            inventory = new Inventory.Inventory();
            var system = new GrainProcessingSystem(inventory);
            system.RegisterRecipe(new GrainProcessingRecipe
            {
                recipe_id = "recipe_ash_grain_flour",
                input_item_id = "crop_ash_grain",
                input_quantity = 2,
                output_item_id = "grain_flour",
                output_quantity = 3,
                processing_hours = 8f
            });
            Assert.True(system.RegisterSilo("silo_01"));
            return system;
        }

        [Fact]
        public void Milling_ConsumesInputAtomically_AndGrantsKitchenConsumableOutput()
        {
            var system = Create(out var inventory);
            inventory.AddById("crop_ash_grain", 2);

            Assert.True(system.StartMilling("recipe_ash_grain_flour", "silo_01").IsSuccess);
            Assert.Equal(0, inventory.CountById("crop_ash_grain"));
            system.TickDay(1);

            Assert.Equal(3, inventory.CountById("grain_flour"));
            Assert.Equal(1, system.State.total_batches_completed);
            Assert.Empty(system.State.active_jobs);
        }

        [Fact]
        public void Milling_InsufficientInputDoesNotCreateJob()
        {
            var system = Create(out var inventory);
            Assert.False(system.StartMilling("recipe_ash_grain_flour", "silo_01").IsSuccess);
            Assert.Empty(system.State.active_jobs);
            Assert.Equal(0, inventory.CountById("crop_ash_grain"));
        }

        [Fact]
        public void SiloTreatment_UsesAtomicInventoryTransaction()
        {
            var system = Create(out var inventory);
            inventory.AddById("crop_ash_grain", 2);
            inventory.AddById("silo_treatment", 1);
            Assert.True(system.StartMilling("recipe_ash_grain_flour", "silo_01").IsSuccess);

            system.GetSilo("silo_01")!.pest_pressure = 55f;
            Assert.True(system.TreatSilo("silo_01", "silo_treatment", 1, 40f).IsSuccess);
            Assert.Equal(15f, system.GetSilo("silo_01")!.pest_pressure);
            Assert.Equal(0, inventory.CountById("silo_treatment"));
        }

        [Fact]
        public void CriticalSiloBlocksNewMilling()
        {
            var system = Create(out var inventory);
            inventory.AddById("crop_ash_grain", 2);
            system.GetSilo("silo_01")!.pest_pressure = 80f;

            Assert.False(system.StartMilling("recipe_ash_grain_flour", "silo_01").IsSuccess);
            Assert.Equal(2, inventory.CountById("crop_ash_grain"));
        }

        [Fact]
        public void OutputCapacityFailureKeepsJobRetryable()
        {
            var system = Create(out var inventory);
            inventory.Capacity = 3;
            inventory.AddById("silo_blocker", 1);
            inventory.AddById("silo_blocker_two", 1);
            inventory.AddById("crop_ash_grain", 2);
            inventory.Capacity = 2;
            Assert.True(system.StartMilling("recipe_ash_grain_flour", "silo_01").IsSuccess);

            system.TickDay(1);
            Assert.Single(system.State.active_jobs);
            Assert.True(system.State.active_jobs[0].is_blocked);
            Assert.Equal(0, inventory.CountById("grain_flour"));
        }

        [Fact]
        public void SaveRoundTripDoesNotRollOrLoseSiloState()
        {
            var system = Create(out var inventory);
            system.GetSilo("silo_01")!.integrity = 48f;
            system.GetSilo("silo_01")!.pest_pressure = 22f;
            system.TickDay(3);
            var saved = system.CaptureState();

            var restored = new GrainProcessingSystem(inventory);
            restored.RegisterRecipe(new GrainProcessingRecipe
            {
                recipe_id = "recipe_ash_grain_flour",
                input_item_id = "crop_ash_grain",
                input_quantity = 2,
                output_item_id = "grain_flour",
                output_quantity = 3,
                processing_hours = 8f
            });
            restored.RestoreState(saved);

            var silo = restored.GetSilo("silo_01")!;
            Assert.Equal(48f, silo.integrity);
            Assert.Equal(26.5f, silo.pest_pressure, 3);
            Assert.Equal(3, restored.State.last_tick_day);
        }
    }
}
