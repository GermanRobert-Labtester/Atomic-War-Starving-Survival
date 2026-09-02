// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans198_201_LateGameSystemsIntegrationTests
    {
        [Fact]
        public void ScenarioA_ToxicRaidDuringFestivalPreparation_EmitsResidueHandoffWithoutCorruptingCeremony()
        {
            var rng = new SeededRng(198201);
            var chem = new ChemWarfareSystem(rng);
            var ceremony = new CeremonySystem(rng);

            // Load minimal fixtures
            ceremony.LoadCatalog(@"{
  ""schema_version"": 1,
  ""ceremonies"": [{
    ""id"": ""ceremony_founding_day"",
    ""display_name"": ""Founding Day"",
    ""preparation_days"": 3,
    ""required_room_id"": ""room_common_mess_hall"",
    ""min_population"": 4,
    ""required_items"": [{ ""item_id"": ""canned_food"", ""quantity"": 10 }],
    ""morale_boost"": 25.0,
    ""stress_relief"": 20.0,
    ""truce_duration_days"": 0,
    ""truce_eligible"": false,
    ""disaster_pool"": [],
    ""description"": ""test""
  }]
}");

            chem.LoadCatalog(@"{
  ""schema_version"": 1,
  ""agents"": [{
    ""id"": ""chem_agent_irritant_prewar"",
    ""display_name"": ""Tear Gas"",
    ""hazard_class"": ""irritant"",
    ""base_density_permille"": 600,
    ""persistence_ticks"": 5,
    ""filter_wear_permille"": 50,
    ""exposure_severity"": 1,
    ""visual_profile_id"": ""fog"",
    ""description"": ""test""
  }]
}");

            // 1. Schedule ceremony
            bool scheduled = ceremony.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);
            Assert.True(scheduled);
            Assert.Equal(CeremonyPhase.Preparing, ceremony.ActiveCeremony!.Phase);

            // 2. Toxic raid begins
            var hazard = chem.DeployHazard("chem_agent_irritant_prewar", 1, "raider_mortar");
            Assert.NotNull(hazard);

            // 3. Combat ticks
            chem.TickCombat(WeatherKind.Clear, 0, 1);
            Assert.Equal(4, hazard.RemainingTicks);

            // 4. Breach residue handoff to shelter sanitation
            string? loggedSector = null;
            int loggedSeverity = 0;
            chem.OnShelterResidueCreated += (sec, sev) =>
            {
                loggedSector = sec;
                loggedSeverity = sev;
            };

            chem.TriggerShelterResidueHandoff("sector_airlock_gate", 2);
            Assert.Equal("sector_airlock_gate", loggedSector);
            Assert.Equal(2, loggedSeverity);

            // 5. Ceremony preparation remains coherent and unaffected
            ceremony.ContributeResource("canned_food", 10);
            ceremony.TickDay(11, out _);
            Assert.Equal(2, ceremony.ActiveCeremony.PreparationDaysRemaining);
            Assert.Equal(10, ceremony.ActiveCeremony.CommittedItems["canned_food"]);
        }

        [Fact]
        public void ScenarioB_CommsArraySatelliteLock_UnlocksStrategicOrbitalStrikeAuthorization()
        {
            var rng = new SeededRng(199001);
            var comms = new CommsArraySystem(rng);

            comms.LoadCatalog(@"{
  ""schema_version"": 1,
  ""targets"": [{
    ""id"": ""comms_target_strategic_uplink_cerberus"",
    ""display_name"": ""Platform Cerberus"",
    ""target_type"": ""strategic_uplink"",
    ""min_array_tier"": 3,
    ""frequency_khz"": 448900,
    ""band"": ""UHF"",
    ""required_power_watts"": 1200,
    ""description"": ""test"",
    ""has_satellite_window"": false,
    ""is_strategic"": true,
    ""revealed_faction_id"": """"
  }]
}");

            comms.SetArrayTier(3);
            comms.SetPowerState(true, 1500f);
            comms.TuneFrequency(448900, "UHF");

            // Scan until contact established and auth code generated
            for (int i = 0; i < 8; i++)
            {
                comms.TickScan(1, 12, 1.0f);
            }

            Assert.Single(comms.State.StrategicAuthorizationCodes);
            string code = comms.State.StrategicAuthorizationCodes[0];

            // Request strategic orbital strike
            bool strikeAuthorized = comms.RequestStrategicStrike("comms_target_strategic_uplink_cerberus", code, out string err);
            Assert.True(strikeAuthorized);
            Assert.True(string.IsNullOrEmpty(err));
            Assert.Empty(comms.State.StrategicAuthorizationCodes); // Code consumed atomically
        }

        [Fact]
        public void ScenarioC_FestivalDiplomacy_BrokersTruceAndOpensDistantCaravan()
        {
            var rng = new SeededRng(200001);
            var ceremony = new CeremonySystem(rng);
            var comms = new CommsArraySystem(rng);

            ceremony.LoadCatalog(@"{
  ""schema_version"": 1,
  ""ceremonies"": [{
    ""id"": ""ceremony_treaty_market"",
    ""display_name"": ""Treaty Fair"",
    ""preparation_days"": 1,
    ""required_room_id"": ""room_common_mess_hall"",
    ""min_population"": 4,
    ""required_items"": [{ ""item_id"": ""canned_food"", ""quantity"": 5 }],
    ""morale_boost"": 20.0,
    ""stress_relief"": 15.0,
    ""truce_duration_days"": 4,
    ""truce_eligible"": true,
    ""disaster_pool"": [],
    ""description"": ""test""
  }]
}");

            comms.LoadCatalog(@"{
  ""schema_version"": 1,
  ""targets"": [{
    ""id"": ""comms_target_consortium_caravan"",
    ""display_name"": ""Merchant Caravan"",
    ""target_type"": ""merchant_consortium"",
    ""min_array_tier"": 1,
    ""frequency_khz"": 144300,
    ""band"": ""VHF"",
    ""required_power_watts"": 450,
    ""description"": ""test"",
    ""has_satellite_window"": false,
    ""is_strategic"": false,
    ""revealed_faction_id"": ""faction_supply_corps""
  }]
}");

            // 1. Schedule treaty market & invite faction
            ceremony.ScheduleCeremony("ceremony_treaty_market", 15, 6, out _);
            bool accepted = ceremony.InviteFaction("faction_supply_corps", 20); // friendly standing
            Assert.True(accepted);
            Assert.Equal(4, ceremony.ActiveCeremony!.ActiveTruceDaysRemaining);

            // 2. Tune comms array to merchant consortium
            comms.SetPowerState(true, 600f);
            comms.TuneFrequency(144300, "VHF");
            for (int i = 0; i < 8; i++) comms.TickScan(15, 10, 0.7f);

            Assert.Contains("comms_target_consortium_caravan", comms.State.DecodedTargetIds);
        }

        [Fact]
        public void ScenarioD_MaintenanceRobot_AssistsHazardCleanupWithoutBiologicalFatigue()
        {
            var rng = new SeededRng(201001);
            var robotics = new RoboticsSystem(rng);

            robotics.LoadCatalog(@"{
  ""schema_version"": 1,
  ""robots"": [{
    ""id"": ""robot_utility_maintenance_drone"",
    ""display_name"": ""Scrub Drone"",
    ""role"": ""maintenance"",
    ""armor_rating"": 0.15,
    ""max_chassis_integrity"": 1000,
    ""max_core_charge_wh"": 4000,
    ""labor_drain_w"": 200,
    ""charging_rate_w"": 400,
    ""emp_disable_hours"": 12,
    ""reactivation_materials"": [],
    ""compatible_tasks"": [ ""chemical_cleanup"" ],
    ""description"": ""test""
  }]
}");

            var drone = robotics.ReactivateRobot("robot_utility_maintenance_drone", 0.8f, out _);
            Assert.NotNull(drone);

            robotics.ProgramDirective(drone.UnitId, "directive_clean_hazards", 0.8f, out _);
            Assert.Equal("directive_clean_hazards", drone.AssignedDirective);

            // Run 8-hour cleanup shift
            robotics.TickLabor(8, isDockedToGrid: false, gridPowerAvailableWatts: 0f);

            // Charge drained: 200W * 8h = 1600Wh. Initial charge was 2000Wh -> 400Wh left.
            Assert.Equal(400, drone.CoreChargeWh);
            Assert.True(drone.ChassisIntegrity < 1000);
            Assert.False(drone.IsRogue);
        }

        [Fact]
        public void ScenarioE_EmpShockDisablesRobotsAndCommsArraySimultaneously()
        {
            var rng = new SeededRng(201002);
            var robotics = new RoboticsSystem(rng);
            var comms = new CommsArraySystem(rng);

            robotics.LoadCatalog(@"{
  ""schema_version"": 1,
  ""robots"": [{
    ""id"": ""robot_security_sentry_v1"",
    ""display_name"": ""Sentry"",
    ""role"": ""security"",
    ""armor_rating"": 0.45,
    ""max_chassis_integrity"": 1000,
    ""max_core_charge_wh"": 6000,
    ""labor_drain_w"": 350,
    ""charging_rate_w"": 750,
    ""emp_disable_hours"": 24,
    ""reactivation_materials"": [],
    ""compatible_tasks"": [],
    ""description"": ""test""
  }]
}");

            var bot = robotics.ReactivateRobot("robot_security_sentry_v1", 0.8f, out _);
            comms.SetPowerState(true, 1000f);

            // EMP Event occurs
            robotics.ApplyEmpShock(24);
            comms.SetPowerState(false, 0f); // Grid brownout / EMP surge

            Assert.True(bot!.IsEmpDisabled);
            Assert.False(comms.State.IsPowered);

            // Comms cannot scan while unpowered
            string? scan = comms.TickScan(1, 12, 0.5f);
            Assert.Null(scan);

            // Robot cannot be given directives while EMP disabled
            bool programmed = robotics.ProgramDirective(bot.UnitId, "directive_patrol", 0.8f, out string err);
            Assert.False(programmed);
            Assert.Contains("EMP-disabled", err);
        }

        [Fact]
        public void CrossSystem_SaveLoadSplitReplay_RestoresAllFourStatesFaithfully()
        {
            var rng = new SeededRng(198201);
            var chem = new ChemWarfareSystem(rng);
            var comms = new CommsArraySystem(rng);
            var ceremony = new CeremonySystem(rng);
            var robotics = new RoboticsSystem(rng);

            chem.LoadCatalog(@"{ ""schema_version"": 1, ""agents"": [{ ""id"": ""chem_agent_irritant_prewar"", ""display_name"": ""Tear"", ""hazard_class"": ""irritant"", ""base_density_permille"": 500, ""persistence_ticks"": 8, ""filter_wear_permille"": 40, ""exposure_severity"": 1, ""visual_profile_id"": ""fog"", ""description"": ""t"" }] }");
            comms.LoadCatalog(@"{ ""schema_version"": 1, ""targets"": [{ ""id"": ""comms_target_weather_beacon_alpha"", ""display_name"": ""Beacon"", ""target_type"": ""automated_beacon"", ""min_array_tier"": 1, ""frequency_khz"": 14220, ""band"": ""HF"", ""required_power_watts"": 150, ""description"": ""t"", ""has_satellite_window"": false, ""is_strategic"": false, ""revealed_faction_id"": """" }] }");
            ceremony.LoadCatalog(@"{ ""schema_version"": 1, ""ceremonies"": [{ ""id"": ""ceremony_founding_day"", ""display_name"": ""Founding"", ""preparation_days"": 3, ""required_room_id"": ""room_common_mess_hall"", ""min_population"": 4, ""required_items"": [{ ""item_id"": ""canned_food"", ""quantity"": 10 }], ""morale_boost"": 25.0, ""stress_relief"": 20.0, ""truce_duration_days"": 0, ""truce_eligible"": false, ""disaster_pool"": [], ""description"": ""t"" }] }");
            robotics.LoadCatalog(@"{ ""schema_version"": 1, ""robots"": [{ ""id"": ""robot_security_sentry_v1"", ""display_name"": ""Sentry"", ""role"": ""security"", ""armor_rating"": 0.45, ""max_chassis_integrity"": 1000, ""max_core_charge_wh"": 6000, ""labor_drain_w"": 350, ""charging_rate_w"": 750, ""emp_disable_hours"": 24, ""reactivation_materials"": [], ""compatible_tasks"": [], ""description"": ""t"" }] }");

            // Mutate all 4 systems
            chem.DeployHazard("chem_agent_irritant_prewar", 0, "test_turret");
            comms.SetArrayTier(2);
            comms.SetPowerState(true, 1000f);
            comms.TuneFrequency(14220, "HF");
            comms.TickScan(1, 12, 0.5f);
            ceremony.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);
            ceremony.ContributeResource("canned_food", 6);
            var bot = robotics.ReactivateRobot("robot_security_sentry_v1", 0.7f, out _);
            robotics.ProgramDirective(bot!.UnitId, "directive_guard", 0.7f, out _);

            // Capture
            var chemSave = chem.CaptureState();
            var commsSave = comms.CaptureState();
            var ceremonySave = ceremony.CaptureState();
            var roboticsSave = robotics.CaptureState();

            // Fresh instances
            var chem2 = new ChemWarfareSystem(new SeededRng(999));
            var comms2 = new CommsArraySystem(new SeededRng(999));
            var ceremony2 = new CeremonySystem(new SeededRng(999));
            var robotics2 = new RoboticsSystem(new SeededRng(999));

            // Restore
            chem2.RestoreState(chemSave);
            comms2.RestoreState(commsSave);
            ceremony2.RestoreState(ceremonySave);
            robotics2.RestoreState(roboticsSave);

            // Verify parity
            Assert.Single(chem2.State.ActiveHazards);
            Assert.Equal("chem_agent_irritant_prewar", chem2.State.ActiveHazards[0].AgentId);

            Assert.Equal(2, comms2.State.ArrayTier);
            Assert.Single(comms2.State.Locks);

            Assert.NotNull(ceremony2.ActiveCeremony);
            Assert.Equal(6, ceremony2.ActiveCeremony.CommittedItems["canned_food"]);

            Assert.Single(robotics2.Units);
            Assert.Equal("directive_guard", robotics2.Units[0].AssignedDirective);
        }
    }
}
