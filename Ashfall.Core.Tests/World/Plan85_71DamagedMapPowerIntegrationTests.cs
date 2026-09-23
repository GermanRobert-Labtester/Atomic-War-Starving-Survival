// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan85_71DamagedMapPowerIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void DamagedMapAndPowerGridCatalogs_LoadCleanly_WithoutSchemaDrift()
        {
            // Plan 85: Damaged Map Zones (12 zones, 32 fragments)
            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(errors);
            Assert.Equal(12, zones.Count);

            var fragmentCount = zones.Sum(z => z.Fragments.Count);
            Assert.Equal(32, fragmentCount);

            // Plan 71: Power Grid Rooms (18 rooms with calibrated draw watts and priorities)
            var ok = ShelterPowerGridCatalogLoader.TryLoad(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer(),
                out var powerCatalog, out var powerError);
            Assert.True(ok, powerError);
            Assert.NotNull(powerCatalog);
            Assert.Equal(18, powerCatalog!.Rooms.Count);

            Assert.Equal(1, powerCatalog.SchemaVersion);
            Assert.Equal(800f, powerCatalog.GenerationWattsDefault);
            Assert.Equal(4000f, powerCatalog.BatteryCapacityWhDefault);
            Assert.Equal(100f, powerCatalog.FuelUnitsDefault);

            // Cross-reference: Metro service ring installation node
            var metroZone = zones.FirstOrDefault(z => z.ZoneId == "metro_service_ring");
            Assert.NotNull(metroZone);
            Assert.Equal("loc_electrical_maintenance_exchange", DamagedMapSystem.ResolveRevealNodeId(metroZone!.InstallationId));
            Assert.Contains(PowerGridSystem.BatteryBankItemId, metroZone.RevealedItems);
        }

        [Fact]
        public void PowerGridSystem_BrownoutLoadShedding_TiersFollowPriority()
        {
            var ok = ShelterPowerGridCatalogLoader.TryLoad(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer(),
                out var powerCatalog, out _);
            Assert.True(ok);

            var rooms = powerCatalog!.Rooms.Select(r => new PowerGridRoom(
                r.Id,
                r.DisplayName,
                r.DrawWatts,
                r.DefaultPriority switch
                {
                    "critical" => PowerGridRoomPriority.Critical,
                    "standard" => PowerGridRoomPriority.Standard,
                    _ => PowerGridRoomPriority.Low
                },
                r.FailureEffectId)).ToList();

            var state = new PowerGridState
            {
                GenerationWatts = 800f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 4000f,
                FuelUnits = 100f
            };

            var gridSystem = new PowerGridSystem(state, rooms, new SeededRng(1985));
            Assert.Equal(18, gridSystem.Rooms.Count);
            Assert.Equal(2910f, gridSystem.TotalDrawWatts);

            // Test emergency brownout load-shed preset
            var shedRooms = gridSystem.ApplyBrownoutShedPreset();
            Assert.NotEmpty(shedRooms);

            // Critical rooms (e.g. air filtration, clinic, cryo vault) must NEVER be demoted
            Assert.Equal(PowerGridRoomPriority.Critical, gridSystem.EffectivePriority("room_air_filtration"));
            Assert.Equal(PowerGridRoomPriority.Critical, gridSystem.EffectivePriority("room_clinic"));
            Assert.Equal(PowerGridRoomPriority.Critical, gridSystem.EffectivePriority("room_cryo_vault"));

            // Restore catalog defaults
            int restoredCount = gridSystem.ApplyCatalogDefaultPriorities();
            Assert.True(restoredCount > 0);
            Assert.Equal(PowerGridRoomPriority.Standard, gridSystem.EffectivePriority("room_greenhouse"));

            // Add external generation source (e.g. TRIGA reactor or solar array)
            gridSystem.SetGenerationContribution("source_triga_aux", 2500f);
            Assert.True(gridSystem.GenerationWatts >= 3300f);
            Assert.True(gridSystem.NetWatts > 0f);
            Assert.False(gridSystem.IsBrownout);
        }

        [Fact]
        public void DamagedMapSystem_FragmentDiscoveryToInstallationReveal()
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var mapSystem = new WastelandMapSystem(new WastelandMapState(), nodes, routes);

            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(errors);

            var damagedMapSystem = new DamagedMapSystem(zones, mapSystem);

            var metroZone = zones.First(z => z.ZoneId == "metro_service_ring");
            string nodeId = DamagedMapSystem.ResolveRevealNodeId(metroZone.InstallationId)!;

            // Destination must be initially locked
            Assert.True(damagedMapSystem.IsDestinationLocked(nodeId));
            Assert.False(mapSystem.IsDiscovered(nodeId));

            // Track events
            DamagedMapZone? completedZone = null;
            string? revealedNode = null;
            damagedMapSystem.OnZoneCompleted += z => completedZone = z;
            damagedMapSystem.OnInstallationRevealed += (z, id) => revealedNode = id;

            // Register fragments 1 and 2
            Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_1"));
            Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_2"));
            Assert.Equal(2, damagedMapSystem.RegisteredCount("metro_service_ring"));
            Assert.False(damagedMapSystem.IsZoneComplete("metro_service_ring"));
            Assert.True(damagedMapSystem.IsDestinationLocked(nodeId));

            // Register fragment 3 -> triggers completion and reveal
            Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_3"));
            Assert.Equal(3, damagedMapSystem.RegisteredCount("metro_service_ring"));
            Assert.True(damagedMapSystem.IsZoneComplete("metro_service_ring"));
            Assert.NotNull(completedZone);
            Assert.Equal("metro_service_ring", completedZone!.ZoneId);
            Assert.Equal(nodeId, revealedNode);

            // Node is now discovered and unlocked on the world map
            Assert.True(mapSystem.IsDiscovered(nodeId));
            Assert.False(mapSystem.IsLocked(nodeId));
            Assert.False(damagedMapSystem.IsDestinationLocked(nodeId));
        }

        [Fact]
        public void DamagedMapElectricalSalvage_DirectlySupportsShelterPowerGrid_CrossLinkage()
        {
            // 1. Discover and assemble the Metro Service Ring damaged map
            var (nodes, routes) = WastelandMapCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var mapSystem = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var damagedMapSystem = new DamagedMapSystem(zones, mapSystem);

            var metroZone = zones.First(z => z.ZoneId == "metro_service_ring");
            foreach (var frag in metroZone.Fragments)
            {
                damagedMapSystem.RegisterFragment(frag.fragment_id);
            }
            Assert.True(damagedMapSystem.IsZoneComplete("metro_service_ring"));
            Assert.Contains(PowerGridSystem.BatteryBankItemId, metroZone.RevealedItems);

            // 2. Power Grid consumes the salvaged reconditioned battery to install bank upgrade
            var ok = ShelterPowerGridCatalogLoader.TryLoad(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer(),
                out var powerCatalog, out _);
            Assert.True(ok);

            var rooms = powerCatalog!.Rooms.Select(r => new PowerGridRoom(
                r.Id, r.DisplayName, r.DrawWatts, PowerGridRoomPriority.Standard, r.FailureEffectId)).ToList();

            var gridState = new PowerGridState
            {
                GenerationWatts = 800f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 4000f,
                FuelUnits = 100f
            };
            var gridSystem = new PowerGridSystem(gridState, rooms, new SeededRng(777));

            Assert.Equal(0, gridSystem.InstalledBatteryBankCount);
            Assert.Equal(4000f, gridSystem.BatteryCapacityWh);

            // Install battery bank 1 using the item discovered from the revealed installation
            bool install1 = gridSystem.TryInstallBatteryBank(out string reason1);
            Assert.True(install1, reason1);
            Assert.Equal(1, gridSystem.InstalledBatteryBankCount);
            Assert.Equal(5000f, gridSystem.BatteryCapacityWh);

            // Install battery bank 2
            bool install2 = gridSystem.TryInstallBatteryBank(out string reason2);
            Assert.True(install2, reason2);
            Assert.Equal(2, gridSystem.InstalledBatteryBankCount);
            Assert.Equal(6000f, gridSystem.BatteryCapacityWh);

            // 3. Save/load round-trip across both map and power grid authorities
            var capturedMap = mapSystem.CaptureState();
            var capturedGrid = gridState; // persistent state DTO

            var restoredMap = new WastelandMapSystem(capturedMap, nodes, routes);
            var restoredDamagedMap = new DamagedMapSystem(zones, restoredMap);
            var restoredGrid = new PowerGridSystem(capturedGrid, rooms, new SeededRng(888));

            string exchangeNodeId = DamagedMapSystem.ResolveRevealNodeId(metroZone.InstallationId)!;
            Assert.True(restoredDamagedMap.IsZoneComplete("metro_service_ring"));
            Assert.True(restoredMap.IsDiscovered(exchangeNodeId));
            Assert.False(restoredDamagedMap.IsDestinationLocked(exchangeNodeId));

            Assert.Equal(2, restoredGrid.InstalledBatteryBankCount);
            Assert.Equal(6000f, restoredGrid.BatteryCapacityWh);
        }
    }
}
