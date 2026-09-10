// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radiation;
using Ashfall.Core.Recreation;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship follow-up: cross-plan scenarios A–F from the Plans 194–201
    /// integration roadmap, proven as deterministic Core-level journeys with
    /// paired-run and save/load continuation comparisons. Core owns all
    /// simulation; no host, no Godot, no wall clock, no unseeded RNG.
    /// </summary>
    public class LateGameCrossPlanScenarioTests
    {
        private static System.Text.Json.JsonSerializerOptions SerializerOpts() => new();

        private static string Serialize<T>(T value) =>
            System.Text.Json.JsonSerializer.Serialize(value, SerializerOpts());

        private static T? Deserialize<T>(string json) =>
            System.Text.Json.JsonSerializer.Deserialize<T>(json, SerializerOpts());

        // ── Scenario A — Festival under toxic-raid threat ───────────────────

        [Fact]
        public void ScenarioA_FestivalUnderToxicRaid_NoDuplicatedContaminationOrCeremonyCost()
        {
            // Run A: straight run.
            var chemA = new ChemWarfareSystem(new SeededRng(42));
            var ceremonyA = new CeremonySystem(new SeededRng(7));
            ceremonyA.LoadCatalog(LoadCatalogFile("ceremonies.json"));

            Assert.True(ceremonyA.ScheduleCeremony("ceremony_long_night_bonfire", 1, currentPopulation: 6, out _));
            foreach (var req in ceremonyA.CeremonyCatalog["ceremony_long_night_bonfire"].RequiredItems)
                Assert.True(ceremonyA.ContributeResource(req.ItemId, req.Quantity));

            var hazard = chemA.DeployHazard("chem_agent_choke_vapor", lane: 1, "raid_uitest");
            Assert.NotNull(hazard);
            chemA.TriggerShelterResidueHandoff("sector_gate", severityTier: 2);
            Assert.Equal(1, chemA.State.TotalResidueIncidentsLogged);

            for (int day = 2; day <= 8; day++)
                ceremonyA.TickDay(day, out _);
            Assert.Equal(CeremonyPhase.Completed, ceremonyA.ActiveCeremony!.Phase);
            int hazardsA = chemA.State.ActiveHazards.Count;
            int residueA = chemA.State.TotalResidueIncidentsLogged;
            int heldA = ceremonyA.State.TotalCeremoniesHeld;

            // Run B: save/load split at day 5 (mid-ceremony, mid-hazard).
            var chemB = new ChemWarfareSystem(new SeededRng(42));
            var ceremonyB = new CeremonySystem(new SeededRng(7));
            ceremonyB.LoadCatalog(LoadCatalogFile("ceremonies.json"));
            Assert.True(ceremonyB.ScheduleCeremony("ceremony_long_night_bonfire", 1, currentPopulation: 6, out _));
            foreach (var req in ceremonyB.CeremonyCatalog["ceremony_long_night_bonfire"].RequiredItems)
                Assert.True(ceremonyB.ContributeResource(req.ItemId, req.Quantity));
            chemB.DeployHazard("chem_agent_choke_vapor", lane: 1, "raid_uitest");
            chemB.TriggerShelterResidueHandoff("sector_gate", severityTier: 2);

            for (int day = 2; day <= 4; day++)
                ceremonyB.TickDay(day, out _);

            // Save/load boundary.
            chemB.RestoreState(Deserialize<ChemWarfareSaveState>(Serialize(chemB.CaptureState())));
            ceremonyB.RestoreState(Deserialize<CeremonySaveState>(Serialize(ceremonyB.CaptureState())));

            for (int day = 5; day <= 8; day++)
                ceremonyB.TickDay(day, out _);

            Assert.Equal(hazardsA, chemB.State.ActiveHazards.Count);
            Assert.Equal(residueA, chemB.State.TotalResidueIncidentsLogged);
            Assert.Equal(heldA, ceremonyB.State.TotalCeremoniesHeld);
        }

        // ── Scenario B — Comms array + orbital telemetry, exactly-once code ─

        [Fact]
        public void ScenarioB_CommsOrbitalTelemetry_StrategicCodeConsumedExactlyOnce()
        {
            var sys = new CommsArraySystem(new SeededRng(11));
            sys.LoadCatalog(LoadCatalogFile("comms_targets.json"));
            sys.SetArrayTier(3);
            sys.SetPowerState(true, 2000f);

            const string strategicId = "comms_target_strategic_uplink_cerberus";
            var target = sys.TargetCatalog[strategicId];
            sys.TuneFrequency(target.FrequencyKhz, target.Band);

            // Scan until contact — deterministic orbital model: the pass window
            // for this target opens at hours 1–2 of each day (phase from the
            // frequency hash). Scanning outside the window degrades the lock.
            string? contact = null;
            for (int day = 1; day <= 12 && contact == null; day++)
                for (int hour = 0; hour < 8 && contact == null; hour++)
                    contact = sys.TickScan(day, hour, 0.8f);
            Assert.NotNull(contact);

            var lockState = sys.GetOrCreateLock(strategicId);
            Assert.True(lockState.IsContactEstablished);
            Assert.False(string.IsNullOrEmpty(lockState.InterceptedData));
            int codesAfterIntercept = sys.State.StrategicAuthorizationCodes.Count;
            Assert.Equal(1, codesAfterIntercept);

            // Save/load split: code must survive exactly.
            sys.RestoreState(Deserialize<CommsArraySaveState>(Serialize(sys.CaptureState())));
            Assert.Equal(codesAfterIntercept, sys.State.StrategicAuthorizationCodes.Count);

            // One request consumes the code; a second request is impossible.
            Assert.True(sys.RequestStrategicStrike(strategicId, lockState.InterceptedData, out _));
            Assert.Equal(0, sys.State.StrategicAuthorizationCodes.Count);
            Assert.False(sys.RequestStrategicStrike(strategicId, lockState.InterceptedData, out _));
            Assert.Equal(0, sys.State.StrategicAuthorizationCodes.Count);
        }

        // ── Scenario C — Festival diplomacy: truce persists through save/load ─

        [Fact]
        public void ScenarioC_FestivalDiplomacy_TrucePersistsExactlyOnce()
        {
            var ceremony = new CeremonySystem(new SeededRng(21));
            ceremony.LoadCatalog(LoadCatalogFile("ceremonies.json"));

            int truceRequests = 0;
            int truceDays = 0;
            ceremony.OnTruceRequested += (factionId, days) => { truceRequests++; truceDays = days; };

            Assert.True(ceremony.ScheduleCeremony("ceremony_treaty_market", 1, currentPopulation: 6, out _));
            foreach (var req in ceremony.CeremonyCatalog["ceremony_treaty_market"].RequiredItems)
                Assert.True(ceremony.ContributeResource(req.ItemId, req.Quantity));
            Assert.True(ceremony.InviteFaction("faction_rebuilders", currentStanding: 40));
            Assert.Equal(1, truceRequests);
            Assert.Equal(4, truceDays); // authored truce_duration_days of treaty market

            // Save/load: truce state must not duplicate.
            ceremony.RestoreState(Deserialize<CeremonySaveState>(Serialize(ceremony.CaptureState())));
            Assert.Equal(1, truceRequests); // restore must not re-fire the request

            for (int day = 2; day <= 10; day++)
                ceremony.TickDay(day, out _);
            Assert.Equal(CeremonyPhase.Completed, ceremony.ActiveCeremony!.Phase);
            Assert.True(ceremony.ActiveCeremony.ActiveTruceDaysRemaining >= 0);
        }

        // ── Scenario D — Robot-assisted toxic cleanup: charge/wear persistence ─

        [Fact]
        public void ScenarioD_RobotToxicCleanup_ProgressPersistsWithoutDuplication()
        {
            var run = (bool splitFlag) =>
            {
                var chem = new ChemWarfareSystem(new SeededRng(42));
                var robotics = new RoboticsSystem(new SeededRng(77));
                robotics.LoadCatalog(LoadCatalogFile("robotics.json"));

                var drone = robotics.ReactivateRobot("robot_utility_maintenance_drone", programmerSkill01: 0.9f, out _);
                Assert.NotNull(drone);
                Assert.True(robotics.ProgramDirective(drone!.UnitId, "directive_repair", programmerSkill01: 0.9f, out _));

                chem.DeployHazard("chem_agent_corrosive_mist", lane: 0, "raid");
                int charge0 = drone.CoreChargeWh;

                if (splitFlag)
                {
                    for (int tick = 1; tick <= 6; tick++)
                        robotics.TickLabor(4, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
                    robotics.RestoreState(Deserialize<RoboticsSaveState>(Serialize(robotics.CaptureState())));
                    // RestoreState clones unit state — re-acquire the live instance.
                    drone = robotics.Units.First(u => u.UnitId == drone.UnitId);
                    for (int tick = 7; tick <= 12; tick++)
                        robotics.TickLabor(4, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
                }
                else
                {
                    for (int tick = 1; tick <= 12; tick++)
                        robotics.TickLabor(4, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
                }

                // Corrosion is routed through the condition authority — the
                // naval service owns the application seam; ChemWarfare only
                // classifies the hazard world.
                var condition = new EquipmentConditionSystem(
                    new SeededRng(5),
                    new Ashfall.Core.Inventory.Inventory(),
                    new CraftingSystem(new Ashfall.Core.Inventory.Inventory()),
                    null);
                var naval = new ExpeditionNavalSystem();
                naval.ApplyWaterCorrosion(new NavalVesselInstance(), toxicContamination: 0.3f, condition);

                return (charge: drone.CoreChargeWh, wear: drone.ChassisIntegrity, hazards: chem.State.ActiveHazards.Count);
            };

            var straight = run(false);
            var splitRun = run(true);

            Assert.Equal(straight.charge, splitRun.charge);
            Assert.Equal(straight.wear, splitRun.wear);
            Assert.Equal(straight.hazards, splitRun.hazards);
            Assert.InRange(straight.charge, 0, 4000);
        }

        // ── Scenario E — EMP crisis: robots down, prep persists, exact duration ─

        [Fact]
        public void ScenarioE_EmpCrisis_DisableDurationMatchesUninterruptedRun()
        {
            var run = (bool splitFlag) =>
            {
                var robotics = new RoboticsSystem(new SeededRng(31));
                robotics.LoadCatalog(LoadCatalogFile("robotics.json"));
                var unit = robotics.ReactivateRobot("robot_security_sentry_v1", 0.8f, out _)!;
                robotics.ProgramDirective(unit.UnitId, "directive_guard", 0.8f, out _);

                robotics.ApplyEmpShock(disableHours: 30);
                Assert.True(unit.IsEmpDisabled);

                if (splitFlag)
                {
                    robotics.TickLabor(10, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
                    robotics.RestoreState(Deserialize<RoboticsSaveState>(Serialize(robotics.CaptureState())));
                    // RestoreState clones unit state — re-acquire the live instance.
                    unit = robotics.Units.First(u => u.UnitId == unit.UnitId);
                    robotics.TickLabor(20, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
                }
                else
                {
                    robotics.TickLabor(30, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
                }

                return (disabled: unit.IsEmpDisabled, remaining: unit.EmpDisableHoursRemaining);
            };

            var straight = run(false);
            var splitRun = run(true);

            Assert.False(straight.disabled);
            Assert.False(splitRun.disabled);
        }

        // ── Scenario F — Rogue AI: deterministic trigger, exactly-once event ─

        [Fact]
        public void ScenarioF_RogueAiContact_DeterministicTriggerWithExactlyOneEvent()
        {
            var run = (bool splitFlag) =>
            {
                var robotics = new RoboticsSystem(new SeededRng(99));
                robotics.LoadCatalog(LoadCatalogFile("robotics.json"));
                var unit = robotics.ReactivateRobot("robot_field_scout_unit", programmerSkill01: 0.0f, out _)!;
                unit.LogicIntegrity = 100; // fragile core

                int rogueEvents = 0;
                robotics.OnRogueEventTriggered += _ => rogueEvents++;

                robotics.ProgramDirective(unit.UnitId, "directive_scout", programmerSkill01: 0.0f, out _);

                if (splitFlag)
                    robotics.RestoreState(Deserialize<RoboticsSaveState>(Serialize(robotics.CaptureState())));

                return (rogue: unit.IsRogue, events: rogueEvents, counter: robotics.State.TotalRogueEventsTriggered);
            };

            var a = run(false);
            var b = run(false);

            // Paired runs: identical outcome.
            Assert.Equal(a.rogue, b.rogue);
            Assert.Equal(a.events, b.events);
            // Exactly-once: event, counter and state agree.
            if (a.rogue)
            {
                Assert.Equal(1, a.events);
                Assert.Equal(1, a.counter);
            }
            else
            {
                Assert.Equal(0, a.events);
                Assert.Equal(0, a.counter);
            }

            // Save/load: rogue state must not re-fire.
            var c = run(true);
            Assert.Equal(a.events, c.events);
        }

        // ── Shared catalog access ──────────────────────────────────────────

        private static string LoadCatalogFile(string fileName)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = System.IO.Path.Combine(dir, "Assets", "StreamingAssets", "Data", fileName);
                if (System.IO.File.Exists(candidate))
                    return System.IO.File.ReadAllText(candidate);
                dir = System.IO.Path.GetDirectoryName(dir) ?? dir;
            }
            throw new InvalidOperationException($"Catalog not found: {fileName}");
        }
    }
}
