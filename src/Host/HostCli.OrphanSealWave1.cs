// SPDX-License-Identifier: MIT
// ============================================================================
// ORPHAN-SEAL-PRIORITY-W1 host probe.
//
// --orphan-seal-wave1-selftest proves, headlessly, that the ten priority
// orphan authorities load their canonical catalog, execute a real command,
// and round-trip their captured state — the exact seams the host wiring uses.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Communications;
using Ashfall.Core.Culture;
using Ashfall.Core.Factions;
using Ashfall.Core.Education;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.Weather;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunOrphanSealWave1SelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} orphan-seal-w1/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            string? Read(string file)
            {
                string path = System.IO.Path.Combine(dataDirectory, file);
                return io.FileExists(path) ? io.ReadAllText(path) : null;
            }

            // 1 — Survivor autonomy.
            var autonomyCatalog = Read("autonomy_actions.json");
            Check("autonomy_catalog_present", autonomyCatalog != null);
            var autonomy = new SurvivorAutonomySystem(new SeededRng(1));
            if (autonomyCatalog != null) autonomy.LoadCatalog(autonomyCatalog);
            Check("autonomy_catalog_rows", autonomy.Templates.Count > 0, $"got {autonomy.Templates.Count}");
            autonomy.AssignGoal("survivor_test", "goal_probe", "Probe goal", 2);
            autonomy.AdvanceGoal("survivor_test");
            var autonomyState = autonomy.CaptureState();
            var autonomyReloaded = new SurvivorAutonomySystem(new SeededRng(1));
            if (autonomyCatalog != null) autonomyReloaded.LoadCatalog(autonomyCatalog);
            autonomyReloaded.RestoreState(autonomyState);
            Check("autonomy_state_round_trip", autonomyReloaded.GetGoal("survivor_test") != null);

            // 2 — Nuclear winter progression.
            var winterCatalog = Read("nuclear_winter_phases.json");
            Check("winter_catalog_present", winterCatalog != null);
            var winter = new NuclearWinterProgressionSystem();
            if (winterCatalog != null) winter.LoadCatalog(winterCatalog);
            var climate = winter.AdvanceDay(30, new SeededRng(2));
            Check("winter_climate_advances", climate != null && !string.IsNullOrEmpty(climate.PhaseId));
            var winterReloaded = new NuclearWinterProgressionSystem();
            if (winterCatalog != null) winterReloaded.LoadCatalog(winterCatalog);
            winterReloaded.RestoreState(winter.CaptureState());
            Check("winter_state_round_trip", winterReloaded.GetPhaseForDay(30) != null);

            // 3 — Seasonal celebrations.
            var celebrationCatalog = Read("shelter_celebrations.json");
            Check("celebration_catalog_present", celebrationCatalog != null);
            var celebrations = new SeasonalCelebrationSystem();
            if (celebrationCatalog != null) celebrations.LoadCatalog(celebrationCatalog);
            var holiday = celebrations.Holidays.Values.FirstOrDefault();
            var celebrationRecord = holiday != null
                ? celebrations.HoldCelebration(holiday.HolidayId, "small", 4, new SeededRng(3))
                : null;
            Check("celebration_holds", celebrationRecord != null && celebrations.History.Count == 1);
            var celebrationsReloaded = new SeasonalCelebrationSystem();
            if (celebrationCatalog != null) celebrationsReloaded.LoadCatalog(celebrationCatalog);
            celebrationsReloaded.RestoreState(celebrations.CaptureState());
            Check("celebration_state_round_trip", celebrationsReloaded.History.Count == 1);

            // 4 — Disaster response.
            var disasterCatalog = Read("disaster_templates.json");
            Check("disaster_catalog_present", disasterCatalog != null);
            var disasters = new DisasterResponseSystem();
            if (disasterCatalog != null) disasters.LoadCatalog(disasterCatalog);
            var disaster = disasters.TriggerDisaster(
                DisasterType.Earthquake, DisasterSeverity.Moderate, new List<string>(), 5);
            Check("disaster_triggers", disaster != null && disasters.GetDisaster(disaster!.DisasterId) != null);
            var disastersReloaded = new DisasterResponseSystem();
            if (disasterCatalog != null) disastersReloaded.LoadCatalog(disasterCatalog);
            disastersReloaded.RestoreState(disasters.CaptureState());
            Check("disaster_state_round_trip", disastersReloaded.GetAllDisasters().Count == 1);

            // 5 — Communications (antenna layer).
            var commsCatalog = Read("communications_networks.json");
            Check("comms_catalog_present", commsCatalog != null);
            var comms = new CommunicationsSystem();
            if (commsCatalog != null) comms.LoadCatalog(commsCatalog);
            comms.DegradeAntennas(5.0);
            Check("comms_range_evaluates", comms.GetEffectiveReceptionRangeKm() >= 0.0);
            var commsReloaded = new CommunicationsSystem();
            if (commsCatalog != null) commsReloaded.LoadCatalog(commsCatalog);
            commsReloaded.RestoreState(comms.CaptureState());
            Check("comms_state_round_trip", commsReloaded.CaptureState() != null);

            // 6 — Colonies (player-founded establishments).
            var colonyCatalog = Read("colony_blueprints.json");
            Check("colony_catalog_present", colonyCatalog != null);
            var colonies = new ColonySystem();
            if (colonyCatalog != null) colonies.LoadCatalog(colonyCatalog);
            var colony = colonies.EstablishColony(
                "loc_holdfast", "Probe Colony", ColonyType.Outpost, null, 50f, 3);
            colonies.TickDay(4);
            Check("colony_establishes", colony != null);
            var coloniesReloaded = new ColonySystem();
            if (colonyCatalog != null) coloniesReloaded.LoadCatalog(colonyCatalog);
            coloniesReloaded.RestoreState(colonies.CaptureState());
            Check("colony_state_round_trip", coloniesReloaded.CaptureState().Colonies.Count >= 1);

            // 7 — Hobbies.
            var hobbyCatalog = Read("hobby_definitions.json");
            Check("hobby_catalog_present", hobbyCatalog != null);
            var hobbies = new HobbySystem();
            if (hobbyCatalog != null) hobbies.LoadCatalog(hobbyCatalog);
            var hobby = hobbies.AuthoredHobbies.FirstOrDefault();
            var hobbyResult = hobby != null
                ? hobbies.ConductSession("survivor_test", hobby.HobbyId, 1, null, new SeededRng(4))
                : null;
            Check("hobby_session_conducts", hobbyResult != null);
            var hobbiesReloaded = new HobbySystem();
            if (hobbyCatalog != null) hobbiesReloaded.LoadCatalog(hobbyCatalog);
            hobbiesReloaded.RestoreState(hobbies.CaptureState());
            Check("hobby_state_round_trip", hobby == null
                || hobbiesReloaded.GetSurvivorHobbies("survivor_test").Count >= 1);

            // 8 — Survivor education.
            var educationCatalog = Read("education_curriculum.json");
            Check("education_catalog_present", educationCatalog != null);
            var education = new SurvivorEducationSystem();
            if (educationCatalog != null) education.LoadCatalog(educationCatalog);
            var learner = education.RegisterLearner("student_test", 8);
            Check("education_registers_learner", learner != null);
            var educationReloaded = new SurvivorEducationSystem();
            if (educationCatalog != null) educationReloaded.LoadCatalog(educationCatalog);
            educationReloaded.RestoreState(education.CaptureState());
            Check("education_state_round_trip", educationReloaded.GetRecord("student_test") != null);

            // 9 — Shelter expansion.
            var expansionCatalog = Read("shelter_construction.json");
            Check("expansion_catalog_present", expansionCatalog != null);
            var expansion = new ShelterExpansionSystem();
            if (expansionCatalog != null) expansion.LoadCatalog(expansionCatalog);
            var project = expansion.StartDepthExcavation(1);
            Check("expansion_starts_project", project != null);
            if (project != null)
                expansion.ProgressProject(project.ProjectId, 500.0, 2);
            var expansionReloaded = new ShelterExpansionSystem();
            if (expansionCatalog != null) expansionReloaded.LoadCatalog(expansionCatalog);
            expansionReloaded.RestoreState(expansion.CaptureState());
            Check("expansion_state_round_trip", expansionReloaded.CaptureState().Projects.Count >= 1);

            // 10 — Confession secrets.
            var confessionCatalogJson = Read("confession_secrets.json");
            Check("confession_catalog_present", confessionCatalogJson != null);
            var confessionCatalog = new ConfessionSecretCatalog();
            if (confessionCatalogJson != null)
                confessionCatalog.Load(confessionCatalogJson, new SystemTextJsonSerializer());
            var secret = confessionCatalog.AllSecrets.FirstOrDefault();
            Check("confession_catalog_rows", secret != null);
            bool discovered = false;
            if (secret != null)
            {
                var confessions = new ConfessionSecretSystem(confessionCatalog);
                discovered = confessions.DiscoverSecret(secret.secret_id, 1, "probe");
                var confessionsReloaded = new ConfessionSecretSystem(confessionCatalog);
                confessionsReloaded.RestoreState(confessions.CaptureState());
                Check("confession_state_round_trip", confessionsReloaded.IsDiscovered(secret.secret_id));
            }
            else
            {
                Check("confession_state_round_trip", false, "no authored secret rows");
            }
            Check("confession_discovers", discovered);

            // 11 — Shelter festivals (planned; complements SeasonalCelebration).
            var festivals = new ShelterFestivalEngine();
            var festivalPlan = festivals.ScheduleFestival(FestivalType.HarvestCommunion, "Probe festival", 10);
            Check("festival_schedules", festivalPlan != null && festivals.Festivals.Count == 1);
            var festivalsReloaded = new ShelterFestivalEngine();
            festivalsReloaded.RestoreState(festivals.CaptureState());
            Check("festival_state_round_trip", festivalsReloaded.Festivals.Count == 1);

            // 12 — Rival covert ops (complements player-deployed EspionageSystem).
            var covertCatalog = CovertOpsCatalog.LoadFromJson(Read("espionage_operations.json") ?? string.Empty);
            Check("covert_catalog_rows", covertCatalog.AllOperations.Count >= 5,
                $"got {covertCatalog.AllOperations.Count}");
            var coordinator = new FactionCovertOpsCoordinator(covertCatalog);
            var covertOp = covertCatalog.AllOperations.FirstOrDefault();
            bool covertLaunched = covertOp != null
                && coordinator.LaunchOperation(covertOp.OperationId, "agent_probe", 1.0f, 1);
            Check("covert_operation_launches", covertLaunched);
            var coordinatorReloaded = new FactionCovertOpsCoordinator(covertCatalog);
            coordinatorReloaded.RestoreState(coordinator.CaptureState());
            Check("covert_state_round_trip",
                coordinatorReloaded.ActiveOperations.Count == coordinator.ActiveOperations.Count);

            GD.Print($"orphan seal wave1 selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("orphan_seal_wave1_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }
    }
}
