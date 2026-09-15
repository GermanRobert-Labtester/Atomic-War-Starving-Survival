// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 210 follow-up — the live power-grid feed for sanitation
    /// facilities. The host supplies the canonical
    /// <see cref="PowerGridSystem.IsRoomPowered"/> query; unset provider
    /// (legacy saves/tests) keeps the stored facility.powered, byte-identical.
    /// </summary>
    public sealed class Plan210SanitationPowerFeedTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            throw new DirectoryNotFoundException("StreamingAssets data dir not found: " + candidate);
        }

        private static SanitationSystem CreateSystem()
        {
            var load = SanitationFacilityCatalogLoader.Load(
                GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            var system = new SanitationSystem();
            system.BindFacilityCatalog(SanitationFacilityCatalogLoader.ToCatalog(load));

            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);
            room.organic = 30f;
            Assert.True(system.InstallFacility("sanitation_wash_station", "dorm") != null);
            return system;
        }

        [Fact]
        public void ProviderUnset_LegacyBehavior_PreservesStoredPoweredFlag()
        {
            var system = CreateSystem();
            var facility = system.State.facilities.Single(f => f.roomId == "dorm");
            facility.powered = false;   // authored/stored state governs

            system.TickDaily(1, population: 0);
            Assert.Equal(30f, system.EnsureRoom("dorm", RoomWasteRole.Residential).organic, 4);
            Assert.False(facility.powered);   // not overwritten by any provider
        }

        [Fact]
        public void PoweredRoom_ContinuesProcessing()
        {
            var system = CreateSystem();
            system.RoomPowerProvider = roomId => roomId == "dorm";   // grid says dorm is powered
            var facility = system.State.facilities.Single(f => f.roomId == "dorm");
            Assert.True(facility.powered);   // feed flips the stale stored flag back on

            system.TickDaily(1, population: 0);
            Assert.Equal(22f, system.EnsureRoom("dorm", RoomWasteRole.Residential).organic, 4);
            Assert.True(system.State.facilities.Single(f => f.roomId == "dorm").powered);
        }

        [Fact]
        public void UnpoweredRoom_StallsFacility_AndFeedPersistsAcrossTicks()
        {
            var system = CreateSystem();
            system.RoomPowerProvider = roomId => false;   // brownout
            var facility = system.State.facilities.Single(f => f.roomId == "dorm");

            system.TickDaily(1, population: 0);
            Assert.False(facility.powered);
            Assert.Equal(30f, system.EnsureRoom("dorm", RoomWasteRole.Residential).organic, 4);

            // Idempotence: still stalled on the next tick while the grid is down.
            system.TickDaily(2, population: 0);
            Assert.False(facility.powered);
            Assert.Equal(30f, system.EnsureRoom("dorm", RoomWasteRole.Residential).organic, 4);
        }
    }
}
