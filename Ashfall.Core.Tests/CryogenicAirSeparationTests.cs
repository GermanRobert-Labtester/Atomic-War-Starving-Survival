using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CryogenicAirSeparationTests
    {
        [Fact]
        public void PoweredCycle_GrantsCanonicalGasProductsAtomically()
        {
            var inventory = new Inventory.Inventory();
            float power = 1000f;
            var system = new CryogenicAirSeparationSystem(inventory, new SeededRng(21), () => power);
            system.ConfigureProducts(new[]
            {
                new CryogenicGasProduct { product_id = "gas_oxygen", units_per_cycle = 2 },
                new CryogenicGasProduct { product_id = "gas_nitrogen", units_per_cycle = 3 }
            });

            Assert.True(system.SetRunning(true));
            system.TickDay(1);

            Assert.Equal(2, inventory.CountById("gas_oxygen"));
            Assert.Equal(3, inventory.CountById("gas_nitrogen"));
            Assert.Equal(1, system.State.cycles_completed);
            Assert.Equal(CryogenicPlantBand.Running, system.Band);
        }

        [Fact]
        public void BrownoutBlocksCycleWithoutGrantingProducts()
        {
            var inventory = new Inventory.Inventory();
            float power = 100f;
            var system = new CryogenicAirSeparationSystem(inventory, new SeededRng(22), () => power);
            system.ConfigureProducts(new[]
            {
                new CryogenicGasProduct { product_id = "gas_oxygen", units_per_cycle = 2 }
            });
            Assert.True(system.SetRunning(true));

            system.TickDay(1);

            Assert.Equal(0, inventory.CountById("gas_oxygen"));
            Assert.Equal(1, system.State.cycles_blocked);
            Assert.Equal(CryogenicPlantBand.Ready, system.Band);
        }

        [Fact]
        public void ProductStorageFailureDoesNotCommitWearOrCycle()
        {
            var inventory = new Inventory.Inventory { Capacity = 1 };
            inventory.AddById("existing_item", 1);
            float power = 1000f;
            var system = new CryogenicAirSeparationSystem(inventory, new SeededRng(23), () => power);
            system.ConfigureProducts(new[]
            {
                new CryogenicGasProduct { product_id = "gas_oxygen", units_per_cycle = 2 }
            });
            Assert.True(system.SetRunning(true));

            system.TickDay(1);

            Assert.Equal(0, system.State.cycles_completed);
            Assert.Equal(0, inventory.CountById("gas_oxygen"));
            Assert.Equal(100f, system.State.plant_integrity);
            Assert.Equal(1, system.State.cycles_blocked);
        }

        [Fact]
        public void LowConditionFaultIsSeededAndSaveSafe()
        {
            var inventory = new Inventory.Inventory();
            float power = 1000f;
            var first = new CryogenicAirSeparationSystem(inventory, new SeededRng(24), () => power);
            first.ConfigureProducts(new[]
            {
                new CryogenicGasProduct { product_id = "gas_oxygen", units_per_cycle = 1 }
            });
            first.State.plant_integrity = 1f;
            first.State.filter_condition = 1f;
            Assert.True(first.SetRunning(true));
            first.TickDay(1);
            var saved = first.CaptureState();

            var second = new CryogenicAirSeparationSystem(inventory, new SeededRng(24), () => power);
            second.ConfigureProducts(new[]
            {
                new CryogenicGasProduct { product_id = "gas_oxygen", units_per_cycle = 1 }
            });
            second.RestoreState(saved);
            Assert.Equal(saved.plant_integrity, second.State.plant_integrity);
            Assert.Equal(saved.failure_events, second.State.failure_events);
            Assert.Equal(saved.last_tick_day, second.State.last_tick_day);
        }
    }
}
