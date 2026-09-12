// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Excavation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CampaignContinuityFlagship54_57Tests
    {
        private sealed class CampaignContext
        {
            public SeededRng Rng { get; }
            public NeedsSystem Needs { get; }
            public StartingLevelSystem StartingLevel { get; }
            public YearOfAshDeepFreezeSystem DeepFreeze { get; }
            public ShelterThermalSystem Thermal { get; }
            public InventoryContainer Inventory { get; }
            public ExcavationHazardSystem Hazard { get; }
            public SeismicDynamicsSystem Seismic { get; }
            public ShelterBarterSystem Barter { get; }
            public SkillProgressionSystem Skills { get; }
            public DutyRosterSystem Roster { get; }
            public SurvivorRelationsSystem Relations { get; }
            public ShelterAssignmentSystem Assignments { get; }
            public ApprenticeshipSystem Apprenticeship { get; }

            public CampaignContext(int seed)
            {
                Rng = new SeededRng(seed);
                Needs = new NeedsSystem();
                StartingLevel = new StartingLevelSystem();
                DeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = -8f });
                Thermal = new ShelterThermalSystem(Rng, Needs, StartingLevel, DeepFreeze);

                Inventory = new InventoryContainer();
                Hazard = new ExcavationHazardSystem(Inventory, Rng);
                Seismic = new SeismicDynamicsSystem(Rng, Thermal, Hazard, Inventory);
                Barter = new ShelterBarterSystem(Rng, Inventory, Thermal);

                Skills = new SkillProgressionSystem();
                Roster = new DutyRosterSystem();
                Relations = new SurvivorRelationsSystem(Rng);

                var rooms = new List<ShelterRoom>
                {
                    new ShelterRoom("room_workshop", "Workshop", 4),
                    new ShelterRoom("room_airlock", "Airlock", 4),
                    new ShelterRoom("bunker_core", "Core", 4)
                };
                var assignState = new ShelterAssignmentState();
                Assignments = new ShelterAssignmentSystem(assignState, rooms, Rng);

                Apprenticeship = new ApprenticeshipSystem(Rng, Skills, Roster, Relations, Assignments, Inventory);

                // Initial Shelter Setup
                Thermal.AddRoom("bunker_core", "Bunker Core", 120f, hasRadiator: true);
                Thermal.AddRoom("room_workshop", "Shelter Workshop", 80f, hasRadiator: true);
                Thermal.AddRoom("room_airlock", "Outer Airlock", 60f, hasRadiator: false);
                Thermal.AddPipe("pipe_core_to_workshop", "bunker_core", "room_workshop");

                // Stock initial inventory
                Inventory.TryProduce("item_scrap_metal", 60);
                Inventory.TryProduce("item_fuel", 40);

                // Initial survivors & assignments
                Skills.RecordAction(new SimpleSkillActor("survivor_mentor_1"), "engineering", 55f, 0);
                Assignments.Assign("survivor_mentor_1", "room_workshop", null, 0);
                Assignments.Assign("survivor_apprentice_1", "room_workshop", null, 0);

                // Register will
                Apprenticeship.RegisterWill(new SurvivorWill
                {
                    testatorSurvivorId = "survivor_mentor_1",
                    primaryBeneficiaryId = "survivor_apprentice_1",
                    bequeathedItemIds = new List<string> { "item_blowtorch" }
                });

                // Start mentorship pair (500 XP target so active until mentor death on day 20)
                Apprenticeship.StartPair("survivor_mentor_1", "survivor_apprentice_1", "engineering", 500f, "mentorship_generator_maintenance");
            }

            public void SimulateDay(int day)
            {
                // Day 5: Retrofit insulation in bunker_core
                if (day == 5)
                {
                    Thermal.RetrofitInsulation("bunker_core", "insul_scrap_panels", Inventory);
                }

                // Day 7: Execute trade with scrap salvagers at airlock for blowtorch
                if (day == 7 && Barter.State.caravans.TryGetValue("caravan_scrap_salvagers", out var cState) && cState.isAtAirlock)
                {
                    var offer = new Dictionary<string, int> { { "item_scrap_metal", 30 } };
                    var req = new Dictionary<string, int> { { "item_blowtorch", 1 } };
                    Barter.ExecuteTrade("caravan_scrap_salvagers", offer, req);
                }

                // Day 10: Surface kinetic shockwave hits
                if (day == 10)
                {
                    Seismic.InjectKineticShock(75f, "bunker_core");
                }

                // Day 15: Start transcription of technical manual
                if (day == 15)
                {
                    Apprenticeship.StartTranscription("survivor_mentor_1", "mentorship_generator_maintenance", Inventory);
                }

                // Day 20: Mentor passes away from natural causes / fallout
                if (day == 20)
                {
                    Apprenticeship.NotifyMentorDeath("survivor_mentor_1");
                    Apprenticeship.ExecuteWill("survivor_mentor_1", Inventory, new HashSet<string> { "survivor_apprentice_1" });
                }

                // Sequential pipeline execution order (PLANS_54_57_AUTHORITY_MAP.md §2)
                Seismic.TickDay(day);
                Thermal.TickDay(day);
                Apprenticeship.TickDay(day);
                Barter.TickDay(day);
            }
        }

        [Fact]
        public void Continuity_30DayDeterministicReplay_ProducesIdenticalOutputs()
        {
            var runA = new CampaignContext(1337);
            for (int day = 1; day <= 30; day++)
                runA.SimulateDay(day);

            var runB = new CampaignContext(1337);
            for (int day = 1; day <= 30; day++)
                runB.SimulateDay(day);

            // 1. Thermal check
            Assert.Equal(runA.Thermal.State.rooms[0].currentTempC, runB.Thermal.State.rooms[0].currentTempC);
            Assert.Equal(runA.Thermal.State.rooms[1].currentTempC, runB.Thermal.State.rooms[1].currentTempC);
            Assert.Equal(runA.Thermal.State.roomInsulationLevels["bunker_core"], runB.Thermal.State.roomInsulationLevels["bunker_core"]);

            // 2. Seismic check
            Assert.Equal(runA.Seismic.State.faults["fault_sub_strata_rift"].currentTension, runB.Seismic.State.faults["fault_sub_strata_rift"].currentTension);
            Assert.Equal(runA.Seismic.State.recentQuakes.Count, runB.Seismic.State.recentQuakes.Count);

            // 3. Barter check
            Assert.Equal(runA.Barter.State.completedTradesCount, runB.Barter.State.completedTradesCount);

            // 4. Apprenticeship & Will check
            Assert.Equal(runA.Apprenticeship.State.executedWills.Count, runB.Apprenticeship.State.executedWills.Count);
            Assert.True(runA.Apprenticeship.State.legacyTraitsGranted.ContainsKey("survivor_apprentice_1"));
            Assert.Equal(runA.Apprenticeship.State.legacyTraitsGranted["survivor_apprentice_1"][0], runB.Apprenticeship.State.legacyTraitsGranted["survivor_apprentice_1"][0]);

            // 5. Inventory check
            Assert.Equal(runA.Inventory.CountById("item_blowtorch"), runB.Inventory.CountById("item_blowtorch"));
            Assert.Equal(runA.Inventory.CountById("item_scrap_metal"), runB.Inventory.CountById("item_scrap_metal"));
        }

        [Fact]
        public void Continuity_Day15SaveRestoreSplit_MatchesContinuousRun()
        {
            // Continuous run to Day 30
            var continuous = new CampaignContext(2026);
            for (int day = 1; day <= 30; day++)
                continuous.SimulateDay(day);

            // Split run: Day 1-15, Save, Restore, Day 16-30
            var split = new CampaignContext(2026);
            for (int day = 1; day <= 15; day++)
                split.SimulateDay(day);

            // Capture state at Day 15
            var thermalSave = split.Thermal.CaptureState();
            var seismicSave = split.Seismic.CaptureState();
            var barterSave = split.Barter.CaptureState();
            var apprenticeSave = split.Apprenticeship.CaptureState();

            // Create fresh context and restore
            var restored = new CampaignContext(2026);
            restored.Thermal.RestoreState(thermalSave);
            restored.Seismic.RestoreState(seismicSave);
            restored.Barter.RestoreState(barterSave);
            restored.Apprenticeship.RestoreState(apprenticeSave);

            // Fast-forward restored context to Day 30
            for (int day = 16; day <= 30; day++)
                restored.SimulateDay(day);

            // Verify parity at Day 30
            Assert.Equal(continuous.Thermal.State.rooms[0].currentTempC, restored.Thermal.State.rooms[0].currentTempC, precision: 1);
            Assert.Equal(continuous.Thermal.State.rooms[1].currentTempC, restored.Thermal.State.rooms[1].currentTempC, precision: 1);
            Assert.Equal(continuous.Seismic.State.faults["fault_sub_strata_rift"].currentTension, restored.Seismic.State.faults["fault_sub_strata_rift"].currentTension, precision: 1);
            Assert.Equal(continuous.Barter.State.completedTradesCount, restored.Barter.State.completedTradesCount);
            Assert.Equal(continuous.Apprenticeship.State.executedWills.Count, restored.Apprenticeship.State.executedWills.Count);
        }
    }
}
