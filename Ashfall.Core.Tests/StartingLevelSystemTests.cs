using System;
using System.Linq;
using Xunit;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core.Tests
{
    public class StartingLevelSystemTests
    {
        [Fact]
        public void InitialHoldfastState_Has5Rooms_CorrectMaterials()
        {
            var system = new StartingLevelSystem();
            Assert.Equal(1, system.State.day);
            Assert.Equal("loc_bunker_holdfast", system.State.locationId);
            Assert.Equal(5, system.State.rooms.Count);

            var corridor = system.State.rooms.FirstOrDefault(r => r.roomId == "room_bunker_corridor");
            Assert.NotNull(corridor);
            Assert.Equal("Concrete", corridor.material);
            Assert.Equal(0.80f, corridor.attenuation, 2);

            var filter = system.State.rooms.FirstOrDefault(r => r.roomId == "room_filtration_stack");
            Assert.NotNull(filter);
            Assert.Equal("Lead", filter.material);
            Assert.Equal(0.99f, filter.attenuation, 2);

            var bunks = system.State.rooms.FirstOrDefault(r => r.roomId == "room_bunks_living");
            Assert.NotNull(bunks);
            Assert.Equal("Wood", bunks.material);
            Assert.Equal(0.10f, bunks.attenuation, 2);
        }

        [Fact]
        public void InspectRoom_MarksRoomAsInspected_AndLogsDirective()
        {
            var system = new StartingLevelSystem();
            var bunks = system.State.rooms.FirstOrDefault(r => r.roomId == "room_bunks_living");
            Assert.NotNull(bunks);
            Assert.False(bunks.isInspected);

            system.InspectRoom("room_bunks_living");
            Assert.True(bunks.isInspected);
            Assert.Contains(system.State.journalDirectives, d => d.Contains("Survivor Bunk Quarters"));
        }

        [Fact]
        public void ResolveMorningRationTriage_SetsPolicyAndMarksResolved()
        {
            var system = new StartingLevelSystem();
            Assert.False(system.State.morningTriageResolved);

            system.ResolveMorningRationTriage(RationPolicy.Half);
            Assert.True(system.State.morningTriageResolved);
            Assert.Equal(RationPolicy.Half, system.State.rationPolicy);
            Assert.Contains(system.State.journalDirectives, d => d.Contains("[MORNING TRIAGE]"));
        }

        [Fact]
        public void ResolveMiddayMaintenance_FortifyBunks_UpgradesShielding()
        {
            var system = new StartingLevelSystem();
            system.ResolveMiddayMaintenance(MaintenanceDirective.FortifyBunksLead);

            Assert.True(system.State.middayMaintenanceResolved);
            Assert.Equal(MaintenanceDirective.FortifyBunksLead, system.State.maintenanceDirective);

            var bunks = system.State.rooms.FirstOrDefault(r => r.roomId == "room_bunks_living");
            Assert.NotNull(bunks);
            Assert.Equal("Lead", bunks.material);
            Assert.Equal(0.99f, bunks.attenuation, 2);
        }

        [Fact]
        public void ResolveEveningRadio_SetsProtocol()
        {
            var system = new StartingLevelSystem();
            system.ResolveEveningRadio(RadioProtocol.MaintainSilence);

            Assert.True(system.State.eveningRadioResolved);
            Assert.Equal(RadioProtocol.MaintainSilence, system.State.radioProtocol);
            Assert.Contains(system.State.journalDirectives, d => d.Contains("[EVENING RADIO]"));
        }

        [Fact]
        public void TickDay_AdvancesDay_AndResetsDailyResolutionFlags()
        {
            var system = new StartingLevelSystem();
            system.ResolveMorningRationTriage(RationPolicy.Standard);
            system.ResolveMiddayMaintenance(MaintenanceDirective.ServiceFilterStack);
            system.ResolveEveningRadio(RadioProtocol.AcknowledgeHydroBarons);

            Assert.True(system.State.morningTriageResolved);
            Assert.True(system.State.middayMaintenanceResolved);
            Assert.True(system.State.eveningRadioResolved);

            system.TickDay();

            Assert.Equal(2, system.State.day);
            Assert.Equal(2, system.State.daysSurvived);
            Assert.False(system.State.morningTriageResolved);
            Assert.False(system.State.middayMaintenanceResolved);
            Assert.False(system.State.eveningRadioResolved);
        }

        [Fact]
        public void SaveLoadRoundTrip_PreservesAllHoldfastState()
        {
            var system = new StartingLevelSystem();
            system.InspectRoom("room_radio_tuner");
            system.ResolveMorningRationTriage(RationPolicy.Irradiated);
            system.ResolveMiddayMaintenance(MaintenanceDirective.FortifyBunksLead);
            system.ResolveEveningRadio(RadioProtocol.BroadcastBeacon);
            system.TickDay();

            var save = system.CaptureState();
            Assert.NotNull(save);
            Assert.Equal(2, save.day);
            Assert.Equal(RationPolicy.Irradiated, save.rationPolicy);

            var restored = new StartingLevelSystem();
            restored.RestoreState(save);

            Assert.Equal(2, restored.State.day);
            Assert.Equal(RationPolicy.Irradiated, restored.State.rationPolicy);
            Assert.Equal(MaintenanceDirective.FortifyBunksLead, restored.State.maintenanceDirective);
            Assert.Equal(RadioProtocol.BroadcastBeacon, restored.State.radioProtocol);

            var bunks = restored.State.rooms.FirstOrDefault(r => r.roomId == "room_bunks_living");
            Assert.NotNull(bunks);
            Assert.Equal("Lead", bunks.material);
            Assert.Equal(0.99f, bunks.attenuation, 2);

            var radio = restored.State.rooms.FirstOrDefault(r => r.roomId == "room_radio_tuner");
            Assert.NotNull(radio);
            Assert.True(radio.isInspected);
        }
    }
}
