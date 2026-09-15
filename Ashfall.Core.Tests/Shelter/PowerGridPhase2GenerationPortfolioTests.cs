// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// B5–B8 Phase 2 (Plan 65) generation portfolio:
    /// - solar grid-tie chain: canonical inverter item → connect → host
    ///   publishes kW×1000 under the stable <c>solar_concentrator</c> source
    ///   id; research gates the item via the recipe chain, never a free buff;
    /// - battery bank build chain: canonical reconditioned battery → bounded
    ///   capacity additions, reserve untouched, additive save field with
    ///   legacy-save defaults.
    /// </summary>
    public class PowerGridPhase2GenerationPortfolioTests
    {
        // ─── Solar grid-tie ────────────────────────────────────────────────

        private static SolarConcentratorEngine MakeSolarEngine(Inventory.Inventory? inv = null)
        {
            var catalog = new SolarConcentratorCatalog();
            catalog.concentrators.Add(new SolarConcentratorDef
            {
                concentrator_id = "solar_dish_medium",
                max_thermal_kw = 10.0f,
                stirling_output_kw = 1.5f
            });
            var engine = new SolarConcentratorEngine(inv ?? new Inventory.Inventory(), new SeededRng(2101), () => 0.9f);
            engine.LoadCatalog(catalog);
            return engine;
        }

        [Fact]
        public void GridTie_DefaultsDisconnected_LegacySavesFeedNothing()
        {
            var engine = MakeSolarEngine();
            engine.State.stirlingAttached = true;
            engine.State.currentElectricalKw = 1.2f; // real output
            Assert.False(engine.State.gridTieConnected);
            Assert.Equal(0f, engine.GridFeedWatts); // legacy behavior: no grid feed
        }

        [Fact]
        public void GridTie_MissingInverter_Blocked_MutatesNothing()
        {
            var engine = MakeSolarEngine();
            engine.State.stirlingAttached = true;

            var res = engine.ConnectGridTie();

            Assert.True(res.IsFailure);
            Assert.False(engine.State.gridTieConnected);
            Assert.Equal(0f, engine.GridFeedWatts);
        }

        [Fact]
        public void GridTie_RequiresStirlingGenerator()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(SolarConcentratorEngine.ItemGridTieInverter, 1);
            var engine = MakeSolarEngine(inv);
            engine.State.stirlingAttached = false;

            var res = engine.ConnectGridTie();

            Assert.True(res.IsFailure);
            Assert.False(engine.State.gridTieConnected);
            Assert.Equal(1, inv.CountById(SolarConcentratorEngine.ItemGridTieInverter)); // not consumed
        }

        [Fact]
        public void GridTie_ConsumesInverterExactlyOnce_AndFeedsGrid()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(SolarConcentratorEngine.ItemGridTieInverter, 1);
            var engine = MakeSolarEngine(inv);
            engine.State.stirlingAttached = true;
            engine.State.currentElectricalKw = 1.2f;

            var res = engine.ConnectGridTie();

            Assert.False(res.IsFailure);
            Assert.True(engine.State.gridTieConnected);
            Assert.Equal(0, inv.CountById(SolarConcentratorEngine.ItemGridTieInverter));
            Assert.Equal(1200f, engine.GridFeedWatts, 1);

            // Second connect is blocked — the inverter is installed state, not repeatable.
            var again = engine.ConnectGridTie();
            Assert.True(again.IsFailure);
            Assert.Equal(1200f, engine.GridFeedWatts, 1);
        }

        [Fact]
        public void GridTie_SaveRoundTrip_PreservesConnection()
        {
            var inv = new Inventory.Inventory();
            inv.AddById(SolarConcentratorEngine.ItemGridTieInverter, 1);
            var engine = MakeSolarEngine(inv);
            engine.State.stirlingAttached = true;
            engine.State.currentElectricalKw = 0.8f;
            Assert.False(engine.ConnectGridTie().IsFailure);

            var restored = new SolarConcentratorEngine(new Inventory.Inventory(), new SeededRng(2102));
            restored.RestoreState(engine.CaptureState());

            Assert.True(restored.State.gridTieConnected);
            Assert.Equal(800f, restored.GridFeedWatts, 1); // restored output feeds again, no replayed consumption
        }

        [Fact]
        public void GridTie_LegacySaveWithoutField_RestoresDisconnected()
        {
            // Old saves predate gridTieConnected — they must restore as
            // disconnected (no free grid feed after migration).
            const string legacy = "{\"concentratorId\":\"solar_dish_medium\",\"isDeployed\":true," +
                "\"mirrorReflectivity\":1.0,\"surfaceCondition\":1.0,\"alignmentQuality\":0.85," +
                "\"trackingMode\":1,\"stirlingAttached\":true,\"currentThermalKw\":3.2," +
                "\"currentElectricalKw\":0.7,\"daysCleaned\":2,\"lastProcessedDay\":40}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<SolarConcentratorState>(legacy);
            Assert.NotNull(state);
            Assert.False(state!.gridTieConnected);

            var engine = MakeSolarEngine();
            engine.RestoreState(state);
            Assert.Equal(0f, engine.GridFeedWatts);
        }

        // ─── Battery bank build chain ──────────────────────────────────────

        private static PowerGridSystem MakeGrid(float batteryReserveWh = 2000f, float batteryCapacityWh = 4000f)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "filtration_off")
            };
            var state = new PowerGridState
            {
                GenerationWatts = 800f,
                FuelUnits = 100f,
                BatteryCapacityWh = batteryCapacityWh,
                BatteryReserveWh = batteryReserveWh
            };
            return new PowerGridSystem(state, rooms, new SeededRng(2103));
        }

        [Fact]
        public void BatteryBank_Install_AddsCapacity_NeverReserve()
        {
            var grid = MakeGrid(batteryReserveWh: 1234.5f, batteryCapacityWh: 4000f);
            Assert.True(grid.TryInstallBatteryBank(out var reason), reason);
            Assert.Equal(1, grid.InstalledBatteryBankCount);
            Assert.Equal(5000f, grid.State.BatteryCapacityWh, 1);
            Assert.Equal(1234.5f, grid.State.BatteryReserveWh, 1); // stored energy untouched
        }

        [Fact]
        public void BatteryBank_Install_BoundedAtMax()
        {
            var grid = MakeGrid();
            for (int i = 0; i < PowerGridSystem.MaxInstalledBatteryBanks; i++)
                Assert.True(grid.TryInstallBatteryBank(out _), $"bank {i + 1}");

            Assert.False(grid.TryInstallBatteryBank(out var reason));
            Assert.Equal("battery_bank_slots_full", reason);
            Assert.Equal(4000f + PowerGridSystem.BatteryBankCapacityWh * PowerGridSystem.MaxInstalledBatteryBanks,
                grid.State.BatteryCapacityWh, 1);
        }

        [Fact]
        public void BatteryBank_Install_FiresTypedEvent()
        {
            var grid = MakeGrid();
            PowerGridEvent? evt = null;
            grid.OnPowerChanged += e => evt = e;
            Assert.True(grid.TryInstallBatteryBank(out _));
            Assert.NotNull(evt);
            Assert.Equal(PowerGridEventKind.BatteryBankInstalled, evt!.Kind);
            Assert.Equal(PowerGridSystem.BatteryBankItemId, evt.RoomId);
        }

        [Fact]
        public void BatteryBank_SaveRoundTrip_ExactParity()
        {
            var grid = MakeGrid(batteryReserveWh: 999.5f, batteryCapacityWh: 4000f);
            Assert.True(grid.TryInstallBatteryBank(out _));
            Assert.True(grid.TryInstallBatteryBank(out _));
            grid.AddFuel(12.5f);

            var restored = MakeGrid(batteryReserveWh: 0f, batteryCapacityWh: 0f);
            restored.RestoreState(grid.CaptureState());

            Assert.Equal(2, restored.InstalledBatteryBankCount);
            Assert.Equal(6000f, restored.State.BatteryCapacityWh, 1);
            Assert.Equal(999.5f, restored.State.BatteryReserveWh, 1);
            Assert.Equal(112.5f, restored.State.FuelUnits, 1);
        }

        [Fact]
        public void BatteryBank_LegacySaveWithoutField_RestoresDefaults()
        {
            // Old Phase-0 fixture shape (pre battery-bank field): must restore
            // with zero banks and the stored capacity exactly — no free energy.
            const string legacy = "{\"SimDay\":47,\"GenerationWatts\":813.5,\"FuelUnits\":62.25," +
                "\"BatteryReserveWh\":1234.5,\"BatteryCapacityWh\":4000," +
                "\"ClosedBreakers\":[\"room_workshop\"],\"TrippedRooms\":[\"room_foundry\"]," +
                "\"Priorities\":[{\"RoomId\":\"room_greenhouse\",\"Priority\":1}]," +
                "\"LastSurgeDay\":31,\"InstalledCoatedPartItemIds\":[]}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<PowerGridState>(legacy);
            Assert.NotNull(state);

            var grid = MakeGrid();
            grid.RestoreState(state!);
            Assert.Equal(0, grid.InstalledBatteryBankCount);
            Assert.Equal(4000f, grid.State.BatteryCapacityWh, 1);
            Assert.Equal(1234.5f, grid.State.BatteryReserveWh, 1); // no free energy granted
        }
    }
}
