// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class WeatherHardeningSystemTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        [Fact]
        public void Catalog_LoadsSuccessfullyFromDataDir()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = WeatherHardeningCatalogLoader.Load(GetDataDir(), files, json);

            Assert.NotNull(catalog);
            Assert.NotEmpty(catalog!.Upgrades);
            Assert.Contains(catalog.Upgrades, u => u.UpgradeId == "upgrade_intake_defrost_element");
        }

        [Fact]
        public void InstallUpgrade_ConsumesMaterialsAndRecordsUpgrade()
        {
            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("copper_wire", 10);
            inventory.AddById("rubber", 5);

            var system = new WeatherHardeningSystem(null, new SeededRng(42), null, inventory: inventory);
            var def = new WeatherHardeningUpgradeDef
            {
                UpgradeId = "upgrade_trace_heating_cable",
                DisplayName = "Trace Heating Cable",
                TargetType = "pipe_network",
                MaterialCosts = new List<MaterialCost>
                {
                    new MaterialCost { ItemId = "copper_wire", Amount = 4 },
                    new MaterialCost { ItemId = "rubber", Amount = 2 }
                },
                FreezeRiskReduction = 0.35f,
                ThermalRetention = 0.10f
            };
            system.RegisterUpgrade(def);

            var result = system.InstallUpgrade("upgrade_trace_heating_cable", "zone_hydroponics");
            Assert.True(result.IsSuccess);
            Assert.True(system.IsUpgradeInstalled("upgrade_trace_heating_cable"));
            Assert.Equal(6, inventory.CountById("copper_wire"));
            Assert.Equal(3, inventory.CountById("rubber"));
            Assert.True(system.GetInstalledFreezeProtection("zone_hydroponics") > 0f);
        }

        [Fact]
        public void InstallUpgrade_WithoutMaterials_IsBlocked()
        {
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var system = new WeatherHardeningSystem(null, new SeededRng(42), null, inventory: inventory);
            var def = new WeatherHardeningUpgradeDef
            {
                UpgradeId = "upgrade_trace_heating_cable",
                DisplayName = "Trace Heating Cable",
                MaterialCosts = new List<MaterialCost>
                {
                    new MaterialCost { ItemId = "copper_wire", Amount = 4 }
                }
            };
            system.RegisterUpgrade(def);

            var result = system.InstallUpgrade("upgrade_trace_heating_cable", "zone_hydroponics");
            Assert.False(result.IsSuccess);
            Assert.Equal("insufficient_materials", result.FailureCode);
        }

        [Fact]
        public void TickDay_AccumulatesIntakeIce()
        {
            var system = new WeatherHardeningSystem(null, new SeededRng(42));
            float initialIce = system.GlobalIntakeIce;

            system.TickDay(1);
            Assert.True(system.GlobalIntakeIce >= initialIce);
        }

        [Fact]
        public void SaveRoundTrip_PreservesInstalledUpgradesAndIntakeIce()
        {
            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("copper_wire", 10);
            var system = new WeatherHardeningSystem(null, new SeededRng(42), null, inventory: inventory);
            var def = new WeatherHardeningUpgradeDef
            {
                UpgradeId = "upgrade_trace_heating_cable",
                DisplayName = "Trace Heating Cable",
                MaterialCosts = new List<MaterialCost>
                {
                    new MaterialCost { ItemId = "copper_wire", Amount = 2 }
                }
            };
            system.RegisterUpgrade(def);
            system.InstallUpgrade("upgrade_trace_heating_cable", "zone_bunker");
            system.TickDay(1);

            var captured = system.CaptureState();
            Assert.Single(captured.installedUpgrades);

            var restoredSystem = new WeatherHardeningSystem(null, new SeededRng(42));
            restoredSystem.RegisterUpgrade(def);
            restoredSystem.RestoreState(captured);

            Assert.True(restoredSystem.IsUpgradeInstalled("upgrade_trace_heating_cable"));
            Assert.Equal(captured.globalIntakeIce, restoredSystem.GlobalIntakeIce);
            Assert.Equal(1, restoredSystem.State.lastProcessedDay);
        }

        [Fact]
        public void TickDay_FreezeBurst_RegistersThermalPipeBurst()
        {
            var rng = new SeededRng(7);
            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = -18f });
            var thermal = new ShelterThermalSystem(rng, needs, starting, deepFreeze);
            thermal.AddRoom("room_hydro", "Hydroponics", 40f);
            thermal.AddPipe("pipe_hydro_main", "room_hydro", "room_hydro");

            var hardening = new WeatherHardeningSystem(
                new WeatherHardeningState
                {
                    lastProcessedDay = 0,
                    zones = new List<ZoneHardeningState>
                    {
                        new ZoneHardeningState { zoneId = "room_hydro", pipeFreezeProgress = 99.95f }
                    }
                },
                rng,
                thermal: thermal);

            bool burst = false;
            hardening.OnPipeBurst += (_, _, _) => burst = true;
            hardening.TickDay(1);

            Assert.True(burst);
            var pipe = thermal.State.pipes.Find(p => p.pipeId == "pipe_hydro_main");
            Assert.NotNull(pipe);
            Assert.True(pipe!.hasBurst);
            Assert.Equal(1, pipe.burstDay);
        }

        [Fact]
        public void TickDay_InstalledRetention_SetsThermalInsulationModifier()
        {
            var rng = new SeededRng(9);
            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = 12f });
            var thermal = new ShelterThermalSystem(rng, needs, starting, deepFreeze);
            thermal.AddRoom("room_bunker", "Bunker", 50f, insulationFactor: 1f);

            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("copper_wire", 10);
            var hardening = new WeatherHardeningSystem(null, rng, thermal: thermal, inventory: inventory);
            hardening.RegisterUpgrade(new WeatherHardeningUpgradeDef
            {
                UpgradeId = "upgrade_retention_blanket",
                DisplayName = "Retention Blanket",
                MaterialCosts = new List<MaterialCost> { new MaterialCost { ItemId = "copper_wire", Amount = 1 } },
                ThermalRetention = 0.40f
            });
            Assert.True(hardening.InstallUpgrade("upgrade_retention_blanket", "room_bunker").IsSuccess);

            float before = thermal.GetEffectiveInsulationFactor("room_bunker");
            hardening.TickDay(1);
            float after = thermal.GetEffectiveInsulationFactor("room_bunker");
            Assert.True(after > before);
        }
    }
}
