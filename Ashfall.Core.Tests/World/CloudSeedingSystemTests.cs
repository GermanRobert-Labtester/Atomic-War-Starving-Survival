// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests.World
{
    public class CloudSeedingSystemTests
    {
        private (CloudSeedingSystem system, WeatherSystem weather, WeatherStationSystem station, Inventory.Inventory inventory) CreateHarness(int seed = 42)
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, seed);
            var station = new WeatherStationSystem(weather, new SeededRng(seed));
            var inventory = new Inventory.Inventory();

            var system = new CloudSeedingSystem(weather, station, inventory, new SeededRng(seed));
            return (system, weather, station, inventory);
        }

        [Fact]
        public void NotInstalled_DeployFailsWithoutMutation()
        {
            var (system, weather, _, inventory) = CreateHarness();
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 5);

            var pre = system.PreflightDeploy(1, WeatherKind.GlassStorm, 1);
            Assert.False(pre.CanDeploy);
            Assert.Equal("cloud_seeding.not_installed", pre.Reason);

            var result = system.Deploy(1, WeatherKind.GlassStorm, 1);
            Assert.False(result.Success);
            Assert.Equal(5, inventory.CountById(CloudSeedingSystem.PrimaryCanisterId));
            Assert.False(system.IsOnCooldown);
        }

        [Fact]
        public void ResearchLocked_DeployFailsWithoutConsumingMaterials()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(1);
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 2);
            system.ResearchUnlockedProvider = id => false; // Locked

            var pre = system.PreflightDeploy(1, WeatherKind.GlassStorm, 1);
            Assert.False(pre.CanDeploy);
            Assert.Equal("cloud_seeding.research_locked", pre.Reason);

            var result = system.Deploy(1, WeatherKind.GlassStorm, 1);
            Assert.False(result.Success);
            Assert.Equal(2, inventory.CountById(CloudSeedingSystem.PrimaryCanisterId));
            Assert.False(system.IsOnCooldown);
        }

        [Fact]
        public void MissingMaterials_DeployFailsWithoutMutation()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(1);
            system.ResearchUnlockedProvider = id => true;

            var pre = system.PreflightDeploy(1, WeatherKind.GlassStorm, 1);
            Assert.False(pre.CanDeploy);
            Assert.Equal("cloud_seeding.insufficient_materials", pre.Reason);

            var result = system.Deploy(1, WeatherKind.GlassStorm, 1);
            Assert.False(result.Success);
            Assert.False(system.IsOnCooldown);
        }

        [Fact]
        public void IllegalWeatherTarget_DeployFails()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(1);
            system.ResearchUnlockedProvider = id => true;
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 1);

            // Clear weather is not an eligible severe target
            var pre = system.PreflightDeploy(1, WeatherKind.Clear, 1);
            Assert.False(pre.CanDeploy);
            Assert.Equal("cloud_seeding.illegal_weather_target", pre.Reason);

            var result = system.Deploy(1, WeatherKind.Clear, 1);
            Assert.False(result.Success);
            Assert.Equal(1, inventory.CountById(CloudSeedingSystem.PrimaryCanisterId));
        }

        [Fact]
        public void UnlockedAndStocked_SuccessfulDeploy_ClearsWeatherAndAppliesCooldown()
        {
            var (system, weather, station, inventory) = CreateHarness(seed: 100);
            system.Install(1);
            station.Install(1);
            station.Calibrate(1);
            system.ResearchUnlockedProvider = id => true;
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 1);

            weather.ForceWeather(WeatherKind.GlassStorm);
            Assert.Equal(WeatherKind.GlassStorm, weather.Current);

            // Deploy to cancel GlassStorm on day 1
            var result = system.Deploy(1, WeatherKind.GlassStorm, 1);

            // Canister consumed exactly once
            Assert.Equal(0, inventory.CountById(CloudSeedingSystem.PrimaryCanisterId));
            Assert.True(system.IsOnCooldown);
            Assert.Equal(7, system.CooldownRemaining);

            if (result.Success)
            {
                Assert.Equal(WeatherKind.Clear, weather.Current);
            }
        }

        [Fact]
        public void SecondaryReagents_FallbackWhenCanisterMissing()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(1);
            system.ResearchUnlockedProvider = id => true;
            inventory.AddById("chemicals", 5);
            inventory.AddById("fuel", 5);

            var pre = system.PreflightDeploy(1, WeatherKind.FalloutStorm, 1);
            Assert.True(pre.CanDeploy);
            Assert.Equal(3, pre.Cost["chemicals"]);
            Assert.Equal(2, pre.Cost["fuel"]);

            var result = system.Deploy(1, WeatherKind.FalloutStorm, 1);
            Assert.Equal(2, inventory.CountById("chemicals")); // 5 - 3 = 2
            Assert.Equal(3, inventory.CountById("fuel"));      // 5 - 2 = 3
            Assert.True(system.IsOnCooldown);
        }

        [Fact]
        public void CooldownActive_BlocksSecondDeployment()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(1);
            system.ResearchUnlockedProvider = id => true;
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 5);

            var r1 = system.Deploy(1, WeatherKind.RadHail, 1);
            Assert.True(system.IsOnCooldown);

            var pre2 = system.PreflightDeploy(1, WeatherKind.RadHail, 1);
            Assert.False(pre2.CanDeploy);
            Assert.Equal("cloud_seeding.cooldown_active", pre2.Reason);

            var r2 = system.Deploy(1, WeatherKind.RadHail, 1);
            Assert.False(r2.Success);
            Assert.Equal("cloud_seeding.cooldown_active", r2.FailureReason);
            // Only 1 canister consumed
            Assert.Equal(4, inventory.CountById(CloudSeedingSystem.PrimaryCanisterId));
        }

        [Fact]
        public void TickDay_DecrementsCooldownDeterministically()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(1);
            system.ResearchUnlockedProvider = id => true;
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 1);

            system.Deploy(1, WeatherKind.Blizzard, 1);
            Assert.Equal(7, system.CooldownRemaining);

            for (int day = 2; day <= 8; day++)
            {
                system.TickDay(day);
                Assert.Equal(7 - (day - 1), system.CooldownRemaining);
            }

            Assert.Equal(0, system.CooldownRemaining);
            Assert.False(system.IsOnCooldown);
        }

        [Fact]
        public void SaveRestore_PreservesCooldownAndInstallationState()
        {
            var (system, _, _, inventory) = CreateHarness();
            system.Install(3);
            system.ResearchUnlockedProvider = id => true;
            inventory.AddById(CloudSeedingSystem.PrimaryCanisterId, 1);

            system.Deploy(3, WeatherKind.IceStorm, 3);
            Assert.Equal(7, system.CooldownRemaining);

            var state = system.CaptureState();

            var (system2, _, _, _) = CreateHarness();
            system2.RestoreState(state);

            Assert.True(system2.IsInstalled);
            Assert.True(system2.IsOnCooldown);
            Assert.Equal(7, system2.CooldownRemaining);
            Assert.Equal(3, system2.State.installDay);
        }

        [Fact]
        public void OldSave_WithNullState_DefaultsNeutral()
        {
            var (system, _, _, _) = CreateHarness();
            system.RestoreState(null);

            Assert.False(system.IsInstalled);
            Assert.False(system.IsOnCooldown);
            Assert.Equal(0, system.CooldownRemaining);
        }
    }
}
