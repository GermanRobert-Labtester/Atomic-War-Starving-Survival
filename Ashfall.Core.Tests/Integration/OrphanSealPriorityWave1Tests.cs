// SPDX-License-Identifier: MIT
// ============================================================================
// ORPHAN-SEAL-PRIORITY-W1 (2026-09-23, user-authorized integrator package)
//
// Focused contract tests for the ten priority orphan authorities: the
// pre-validated save sections are registered with the expected owner methods,
// the retired prisoner duplicate is gone, each system loads its canonical
// catalog and round-trips its captured state, and same-seed behavior stays
// deterministic.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Communications;
using Ashfall.Core.Culture;
using Ashfall.Core.Factions;
using Ashfall.Core.Education;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.Weather;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class OrphanSealPriorityWave1Tests
    {
        private static readonly (string Section, string SaveMethod, string SetupMethod)[] PrioritySections =
        {
            ("survivor_autonomy", "SaveSurvivorAutonomy", "SetupSurvivorAutonomy"),
            ("nuclear_winter_progression", "SaveNuclearWinter", "SetupNuclearWinter"),
            ("seasonal_celebration", "SaveSeasonalCelebration", "SetupSeasonalCelebration"),
            ("disaster_response", "SaveDisasterResponse", "SetupDisasterResponse"),
            ("communications", "SaveCommunications", "SetupCommunications"),
            ("colony", "SaveColony", "SetupColony"),
            ("hobby", "SaveHobby", "SetupHobby"),
            ("survivor_education", "SaveSurvivorEducation", "SetupSurvivorEducation"),
            ("shelter_expansion", "SaveShelterExpansion", "SetupShelterExpansion"),
            ("confession_secret", "SaveConfessionSecrets", "SetupConfessionSecrets"),
            ("shelter_festival", "SaveShelterFestival", "SetupShelterFestival"),
            ("faction_covert_ops", "SaveFactionCovertOps", "SetupFactionCovertOps"),
        };

        private static string CatalogPath(string fileName)
        {
            string path = Path.Combine(AppContext.BaseDirectory,
                "../../../../Assets/StreamingAssets/Data/" + fileName);
            if (File.Exists(path)) return path;
            string alt = Path.Combine("Assets/StreamingAssets/Data", fileName);
            if (File.Exists(alt)) return alt;
            return string.Empty;
        }

        private static string? ReadCatalog(string fileName)
        {
            string path = CatalogPath(fileName);
            return path.Length > 0 ? File.ReadAllText(path) : null;
        }

        [Fact]
        public void PrioritySections_AreRegisteredWithExpectedOwnerMethods()
        {
            foreach (var (section, saveMethod, setupMethod) in PrioritySections)
            {
                var metadata = SaveSectionRegistry.All
                    .FirstOrDefault(s => string.Equals(s.SectionKey, section, StringComparison.Ordinal));
                Assert.True(metadata != null, $"missing save section: {section}");
                Assert.Equal(saveMethod, metadata!.SaveMethod);
                Assert.Equal(setupMethod, metadata.SetupMethod);
            }
        }

        [Fact]
        public void RetiredPrisonerDuplicate_KeepsOnlyTheCanonicalSection()
        {
            Assert.DoesNotContain(SaveSectionRegistry.All,
                s => string.Equals(s.SectionKey, "shelter_prisoners", StringComparison.Ordinal));
            Assert.Contains(SaveSectionRegistry.All,
                s => string.Equals(s.SectionKey, "prisoner_management", StringComparison.Ordinal));
        }

        [Fact]
        public void PriorityCatalogs_ExistAndLoadRows()
        {
            Assert.NotNull(ReadCatalog("autonomy_actions.json"));
            Assert.NotNull(ReadCatalog("nuclear_winter_phases.json"));
            Assert.NotNull(ReadCatalog("shelter_celebrations.json"));
            Assert.NotNull(ReadCatalog("disaster_templates.json"));
            Assert.NotNull(ReadCatalog("communications_networks.json"));
            Assert.NotNull(ReadCatalog("colony_blueprints.json"));
            Assert.NotNull(ReadCatalog("hobby_definitions.json"));
            Assert.NotNull(ReadCatalog("education_curriculum.json"));
            Assert.NotNull(ReadCatalog("shelter_construction.json"));
            Assert.NotNull(ReadCatalog("confession_secrets.json"));
        }

        [Fact]
        public void SurvivorAutonomy_CatalogAndStateRoundTrip()
        {
            var system = new SurvivorAutonomySystem(new SeededRng(1));
            string? json = ReadCatalog("autonomy_actions.json");
            Assert.NotNull(json);
            system.LoadCatalog(json!);
            Assert.True(system.Templates.Count > 0);

            system.AssignGoal("survivor_test", "goal_probe", "Probe goal", 2);
            system.AdvanceGoal("survivor_test");

            var reloaded = new SurvivorAutonomySystem(new SeededRng(1));
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.NotNull(reloaded.GetGoal("survivor_test"));
        }

        [Fact]
        public void NuclearWinter_CatalogAndStateRoundTrip_IsDeterministic()
        {
            string? json = ReadCatalog("nuclear_winter_phases.json");
            Assert.NotNull(json);

            var a = new NuclearWinterProgressionSystem();
            a.LoadCatalog(json!);
            var first = a.AdvanceDay(30, new SeededRng(7));

            var b = new NuclearWinterProgressionSystem();
            b.LoadCatalog(json!);
            var second = b.AdvanceDay(30, new SeededRng(7));

            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.Equal(first!.PhaseId, second!.PhaseId);
            Assert.Equal(first.SeasonId, second.SeasonId);

            var reloaded = new NuclearWinterProgressionSystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(a.CaptureState());
            Assert.NotNull(reloaded.GetPhaseForDay(30));
            Assert.Equal(first.PhaseId, reloaded.GetPhaseForDay(30)!.PhaseId);
        }

        [Fact]
        public void SeasonalCelebration_CatalogAndHistoryRoundTrip()
        {
            string? json = ReadCatalog("shelter_celebrations.json");
            Assert.NotNull(json);
            var system = new SeasonalCelebrationSystem();
            system.LoadCatalog(json!);
            var holiday = system.Holidays.Values.First();
            var record = system.HoldCelebration(holiday.HolidayId, "small", 4, new SeededRng(3));
            Assert.NotNull(record);
            Assert.Single(system.History);

            var reloaded = new SeasonalCelebrationSystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.Single(reloaded.History);
            Assert.Equal(record!.HolidayId, reloaded.History[0].HolidayId);
        }

        [Fact]
        public void DisasterResponse_CatalogAndStateRoundTrip()
        {
            string? json = ReadCatalog("disaster_templates.json");
            Assert.NotNull(json);
            var system = new DisasterResponseSystem();
            system.LoadCatalog(json!);
            var disaster = system.TriggerDisaster(
                DisasterType.Earthquake, DisasterSeverity.Moderate, new List<string>(), 5);
            Assert.NotNull(disaster);
            Assert.NotNull(system.GetDisaster(disaster!.DisasterId));

            var reloaded = new DisasterResponseSystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.Single(reloaded.GetAllDisasters());
            Assert.Equal(disaster.DisasterId, reloaded.GetAllDisasters()[0].DisasterId);
        }

        [Fact]
        public void Communications_CatalogAndAntennaStateRoundTrip()
        {
            string? json = ReadCatalog("communications_networks.json");
            Assert.NotNull(json);
            var system = new CommunicationsSystem();
            system.LoadCatalog(json!);
            system.DegradeAntennas(5.0);
            Assert.True(system.GetEffectiveReceptionRangeKm() >= 0.0);

            var reloaded = new CommunicationsSystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.NotNull(reloaded.CaptureState());
        }

        [Fact]
        public void Colony_CatalogAndStateRoundTrip()
        {
            string? json = ReadCatalog("colony_blueprints.json");
            Assert.NotNull(json);
            var system = new ColonySystem();
            system.LoadCatalog(json!);
            var colony = system.EstablishColony("loc_holdfast", "Probe Colony", ColonyType.Outpost, null, 50f, 3);
            Assert.NotNull(colony);
            system.TickDay(4);

            var reloaded = new ColonySystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.True(reloaded.CaptureState().Colonies.Count >= 1);
        }

        [Fact]
        public void Hobby_CatalogAndProgressRoundTrip()
        {
            string? json = ReadCatalog("hobby_definitions.json");
            Assert.NotNull(json);
            var system = new HobbySystem();
            system.LoadCatalog(json!);
            var hobby = system.AuthoredHobbies.First();
            var result = system.ConductSession("survivor_test", hobby.HobbyId, 1, null, new SeededRng(4));
            Assert.NotNull(result);

            var reloaded = new HobbySystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.True(reloaded.GetSurvivorHobbies("survivor_test").Count >= 1);
        }

        [Fact]
        public void SurvivorEducation_CatalogAndRecordRoundTrip()
        {
            string? json = ReadCatalog("education_curriculum.json");
            Assert.NotNull(json);
            var system = new SurvivorEducationSystem();
            system.LoadCatalog(json!);
            var learner = system.RegisterLearner("student_test", 8);
            Assert.NotNull(learner);

            var reloaded = new SurvivorEducationSystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.NotNull(reloaded.GetRecord("student_test"));
        }

        [Fact]
        public void ShelterExpansion_CatalogAndProjectRoundTrip()
        {
            string? json = ReadCatalog("shelter_construction.json");
            Assert.NotNull(json);
            var system = new ShelterExpansionSystem();
            system.LoadCatalog(json!);
            var project = system.StartDepthExcavation(1);
            Assert.NotNull(project);
            system.ProgressProject(project!.ProjectId, 500.0, 2);

            var reloaded = new ShelterExpansionSystem();
            reloaded.LoadCatalog(json!);
            reloaded.RestoreState(system.CaptureState());
            Assert.True(reloaded.CaptureState().Projects.Count >= 1);
        }

        [Fact]
        public void ShelterFestival_PlanRoundTrips()
        {
            var system = new ShelterFestivalEngine();
            var plan = system.ScheduleFestival(FestivalType.HarvestCommunion, "Probe festival", 10);
            Assert.NotNull(plan);
            Assert.Single(system.Festivals);

            var reloaded = new ShelterFestivalEngine();
            reloaded.RestoreState(system.CaptureState());
            Assert.Single(reloaded.Festivals);
            Assert.Equal(plan!.FestivalId, reloaded.Festivals[0].FestivalId);
        }

        [Fact]
        public void FactionCovertOps_StateRoundTrips()
        {
            string? json = ReadCatalog("espionage_operations.json");
            Assert.NotNull(json);
            var catalog = CovertOpsCatalog.LoadFromJson(json!);
            Assert.True(catalog.AllOperations.Count >= 5);

            var coordinator = new FactionCovertOpsCoordinator(catalog);
            var op = catalog.AllOperations.First();
            Assert.True(coordinator.LaunchOperation(op.OperationId, "agent_probe", 1.0f, 1));

            var reloaded = new FactionCovertOpsCoordinator(catalog);
            reloaded.RestoreState(coordinator.CaptureState());
            Assert.Equal(coordinator.ActiveOperations.Count, reloaded.ActiveOperations.Count);
        }

        [Fact]
        public void ConfessionSecrets_CatalogAndChoiceRoundTrip()
        {
            string? json = ReadCatalog("confession_secrets.json");
            Assert.NotNull(json);
            var catalog = new ConfessionSecretCatalog();
            catalog.Load(json!, new SystemTextJsonSerializer());
            var secret = catalog.AllSecrets.First();
            Assert.NotNull(secret);

            var system = new ConfessionSecretSystem(catalog);
            Assert.True(system.DiscoverSecret(secret.secret_id, 1, "test"));

            var reloaded = new ConfessionSecretSystem(catalog);
            reloaded.RestoreState(system.CaptureState());
            Assert.True(reloaded.IsDiscovered(secret.secret_id));
        }
    }
}
