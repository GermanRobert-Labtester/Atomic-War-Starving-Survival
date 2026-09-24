// SPDX-License-Identifier: MIT
// ============================================================================
// ORPHAN-SEAL-PRIORITY-W1 (2026-09-23, user-authorized integrator package)
//
// Host composition for the ten priority orphan authorities. Setup binds each
// Core system to its canonical catalog and pre-validated save section; the
// day tick advances only systems with a legitimate daily owner; every
// mutating command is exposed through a host session so no panel or CLI probe
// holds gameplay state. Observability runs through the journal seam plus the
// --orphan-seal-wave1-selftest probe.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Communications;
using Ashfall.Core.Culture;
using Ashfall.Core.Education;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.Weather;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorAutonomyHostSession? _survivorAutonomy;
        private NuclearWinterHostSession? _nuclearWinter;
        private SeasonalCelebrationHostSession? _seasonalCelebration;
        private DisasterResponseHostSession? _disasterResponse;
        private CommunicationsHostSession? _communications;
        private ColonyHostSession? _colony;
        private HobbyHostSession? _hobby;
        private SurvivorEducationHostSession? _survivorEducation;
        private ShelterExpansionHostSession? _shelterExpansion;
        private ConfessionSecretHostSession? _confessionSecrets;
        private ShelterFestivalHostSession? _shelterFestival;
        private FactionCovertOpsHostSession? _covertOps;

        private string _lastWinterPhaseId = string.Empty;
        private bool _orphanSealWave1Dirty;

        public SurvivorAutonomyHostSession? SurvivorAutonomy => _survivorAutonomy;
        public NuclearWinterHostSession? NuclearWinter => _nuclearWinter;
        public SeasonalCelebrationHostSession? SeasonalCelebration => _seasonalCelebration;
        public CommunicationsHostSession? Communications => _communications;
        public ColonyHostSession? Colony => _colony;
        public SurvivorEducationHostSession? SurvivorEducation => _survivorEducation;

        private static string? TryReadWave1Catalog(string fileName)
        {
            try
            {
                string path = CatalogPath.ResolveCatalog(fileName);
                var io = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
                if (!io.FileExists(path)) return null;
                return io.ReadAllText(path);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[OrphanSealWave1] catalog {fileName} unreadable: {ex.Message}");
                return null;
            }
        }

        private void SetupOrphanSealWave1()
        {
            SetupSurvivorAutonomy();
            SetupNuclearWinter();
            SetupSeasonalCelebration();
            SetupDisasterResponse();
            SetupCommunications();
            SetupColony();
            SetupHobby();
            SetupSurvivorEducation();
            SetupShelterExpansion();
            SetupConfessionSecrets();
            SetupShelterFestival();
            SetupFactionCovertOps();
        }

        private void SetupSurvivorAutonomy()
        {
            if (_survivorAutonomy != null) return;
            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Social, 0, 40)
                : new SeededRng(1400);
            var system = new SurvivorAutonomySystem(rng);
            var catalog = TryReadWave1Catalog("autonomy_actions.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = SurvivorAutonomySaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _survivorAutonomy = new SurvivorAutonomyHostSession(system);
            _survivorAutonomy.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupNuclearWinter()
        {
            if (_nuclearWinter != null) return;
            var system = new NuclearWinterProgressionSystem();
            var catalog = TryReadWave1Catalog("nuclear_winter_phases.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = NuclearWinterSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _nuclearWinter = new NuclearWinterHostSession(system);
            _nuclearWinter.StateChanged += () => _orphanSealWave1Dirty = true;
            _lastWinterPhaseId = system.GetPhaseForDay(Math.Max(1, _simDay))?.PhaseId ?? string.Empty;
        }

        private void SetupSeasonalCelebration()
        {
            if (_seasonalCelebration != null) return;
            var system = new SeasonalCelebrationSystem();
            var catalog = TryReadWave1Catalog("shelter_celebrations.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = SeasonalCelebrationSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _seasonalCelebration = new SeasonalCelebrationHostSession(system);
            _seasonalCelebration.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupDisasterResponse()
        {
            if (_disasterResponse != null) return;
            var system = new DisasterResponseSystem();
            var catalog = TryReadWave1Catalog("disaster_templates.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = DisasterResponseSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _disasterResponse = new DisasterResponseHostSession(system);
            _disasterResponse.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupCommunications()
        {
            if (_communications != null) return;
            var system = new CommunicationsSystem();
            var catalog = TryReadWave1Catalog("communications_networks.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = CommunicationsSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _communications = new CommunicationsHostSession(system);
            _communications.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupColony()
        {
            if (_colony != null) return;
            var system = new ColonySystem();
            var catalog = TryReadWave1Catalog("colony_blueprints.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = ColonySaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _colony = new ColonyHostSession(system);
            _colony.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupHobby()
        {
            if (_hobby != null) return;
            var system = new HobbySystem();
            var catalog = TryReadWave1Catalog("hobby_definitions.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = HobbySaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _hobby = new HobbyHostSession(system);
            _hobby.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupSurvivorEducation()
        {
            if (_survivorEducation != null) return;
            var system = new SurvivorEducationSystem();
            var catalog = TryReadWave1Catalog("education_curriculum.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = SurvivorEducationSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _survivorEducation = new SurvivorEducationHostSession(system);
            _survivorEducation.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupShelterExpansion()
        {
            if (_shelterExpansion != null) return;
            SetupSurvivors();
            SetupShelterAssignment();
            var system = new ShelterExpansionSystem();
            var catalog = TryReadWave1Catalog("shelter_construction.json");
            if (catalog != null) system.LoadCatalog(catalog);
            var saved = ShelterExpansionSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);

            system.IsCrewSurvivorEligible = survivorId =>
            {
                var survivor = _survivors?.Find(survivorId);
                return survivor != null && survivor.IsAlive && survivor.Health > 0f
                    && EvaluateSurvivorFitness(survivorId).Level != FitnessLevel.Incapacitated;
            };
            system.ResolveCrewSkillBonus = (survivorId, _) =>
                EnsureSharedSkillProgression().GetDisciplineSkillBonus(survivorId, "crafting");
            system.ApplyCrewFatigue = (survivorId, amount) =>
            {
                var survivor = _survivors?.Find(survivorId);
                if (survivor != null) _survivors!.Needs.Modify(survivor, NeedKind.Fatigue, amount);
            };
            system.ApplySurvivorMoraleDelta = (survivorId, delta) =>
            {
                var survivor = _survivors?.Find(survivorId);
                if (survivor != null) _survivors!.Needs.Modify(survivor, NeedKind.Morale, delta);
            };
            system.OnProjectStartedSeam += _ => _orphanSealWave1Dirty = true;
            system.OnProjectCompletedSeam += project =>
            {
                _shelterAssignment?.System.ApplyCapacityBonuses(system.GetCompletedCapacityBonuses());
                _journal?.TryAddRawEntry(
                    "shelter_construction_completed",
                    $"Shelter project {project.ProjectId} was completed.",
                    null!, Math.Max(1, _simDay));
                _orphanSealWave1Dirty = true;
            };

            _shelterExpansion = new ShelterExpansionHostSession(system);
            _shelterExpansion.StateChanged += () => _orphanSealWave1Dirty = true;
            _shelterAssignment?.System.ApplyCapacityBonuses(system.GetCompletedCapacityBonuses());
        }

        private void SetupConfessionSecrets()
        {
            if (_confessionSecrets != null) return;
            var catalog = new ConfessionSecretCatalog();
            var catalogJson = TryReadWave1Catalog("confession_secrets.json");
            if (catalogJson != null)
                catalog.Load(catalogJson, new SystemTextJsonSerializer());
            var system = new ConfessionSecretSystem(catalog, new GodotLog());
            var saved = ConfessionSecretSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _confessionSecrets = new ConfessionSecretHostSession(system);
            _confessionSecrets.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupShelterFestival()
        {
            if (_shelterFestival != null) return;
            var system = new ShelterFestivalEngine();
            var saved = ShelterFestivalSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _shelterFestival = new ShelterFestivalHostSession(system);
            _shelterFestival.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void SetupFactionCovertOps()
        {
            if (_covertOps != null) return;
            var catalogJson = TryReadWave1Catalog("espionage_operations.json");
            var catalog = CovertOpsCatalog.LoadFromJson(catalogJson ?? string.Empty);
            var system = new FactionCovertOpsCoordinator(catalog);
            var saved = FactionCovertOpsSaveStore.TryLoad();
            if (saved != null && !string.IsNullOrEmpty(saved.json))
                system.RestoreState(saved.json);
            _covertOps = new FactionCovertOpsHostSession(system);
            _covertOps.StateChanged += () => _orphanSealWave1Dirty = true;
        }

        private void PersistOrphanSealWave1()
        {
            SaveSurvivorAutonomy();
            SaveNuclearWinter();
            SaveSeasonalCelebration();
            SaveDisasterResponse();
            SaveCommunications();
            SaveColony();
            SaveHobby();
            SaveSurvivorEducation();
            SaveShelterExpansion();
            SaveConfessionSecrets();
            SaveShelterFestival();
            SaveFactionCovertOps();
            _orphanSealWave1Dirty = false;
        }

        private void SaveSurvivorAutonomy()
            => CaptureWave1IfPresent("survivor_autonomy",
                _survivorAutonomy?.System.CaptureState(), SurvivorAutonomySaveStore.TryCapturePersisted);

        private void SaveNuclearWinter()
            => CaptureWave1IfPresent("nuclear_winter_progression",
                _nuclearWinter?.System.CaptureState(), NuclearWinterSaveStore.TryCapturePersisted);

        private void SaveSeasonalCelebration()
            => CaptureWave1IfPresent("seasonal_celebration",
                _seasonalCelebration?.System.CaptureState(), SeasonalCelebrationSaveStore.TryCapturePersisted);

        private void SaveDisasterResponse()
            => CaptureWave1IfPresent("disaster_response",
                _disasterResponse?.System.CaptureState(), DisasterResponseSaveStore.TryCapturePersisted);

        private void SaveCommunications()
            => CaptureWave1IfPresent("communications",
                _communications?.System.CaptureState(), CommunicationsSaveStore.TryCapturePersisted);

        private void SaveColony()
            => CaptureWave1IfPresent("colony",
                _colony?.System.CaptureState(), ColonySaveStore.TryCapturePersisted);

        private void SaveHobby()
            => CaptureWave1IfPresent("hobby",
                _hobby?.System.CaptureState(), HobbySaveStore.TryCapturePersisted);

        private void SaveSurvivorEducation()
            => CaptureWave1IfPresent("survivor_education",
                _survivorEducation?.System.CaptureState(), SurvivorEducationSaveStore.TryCapturePersisted);

        private void SaveShelterExpansion()
            => CaptureWave1IfPresent("shelter_expansion",
                _shelterExpansion?.System.CaptureState(), ShelterExpansionSaveStore.TryCapturePersisted);

        private void SaveConfessionSecrets()
            => CaptureWave1IfPresent("confession_secret",
                _confessionSecrets?.System.CaptureState(), ConfessionSecretSaveStore.TryCapturePersisted);

        private void SaveShelterFestival()
            => CaptureWave1IfPresent("shelter_festival",
                _shelterFestival?.System.CaptureState(), ShelterFestivalSaveStore.TryCapturePersisted);

        private void SaveFactionCovertOps()
        {
            if (_covertOps == null) return;
            CaptureSection("faction_covert_ops", FactionCovertOpsSaveStore.TryCapturePersisted(
                new CovertOpsPersistedPayload { json = _covertOps.System.CaptureState() }));
        }

        private void CaptureWave1IfPresent<T>(string section, T? state, Func<T, string> capture)
            where T : class
        {
            if (state != null) CaptureSection(section, capture(state));
        }

        private void TickOrphanSealWave1(int day)
        {
            if (day <= 0) return;
            SetupSurvivorAutonomy();
            SetupNuclearWinter();
            SetupSeasonalCelebration();
            SetupDisasterResponse();
            SetupCommunications();
            SetupColony();

            TickNuclearWinter(day);
            TickDisasterResponse(day);
            TickCommunications(day);
            _colony!.TickDay(day);
            TickSurvivorAutonomy(day);
            TickShelterOperationsCrews(day);
            TickSeasonalCelebration(day);
            _shelterFestival?.TickDay(day);
            TickFactionCovertOps(day);
        }

        private void TickFactionCovertOps(int day)
        {
            if (_covertOps == null) return;
            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Social, day, 42)
                : new SeededRng(4200 + day);
            _covertOps.AdvanceDay(rng, day);
            _orphanSealWave1Dirty = true;
        }

        private void TickNuclearWinter(int day)
        {
            if (_nuclearWinter == null) return;
            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Weather, day, 40)
                : new SeededRng(4000 + day);
            var climate = _nuclearWinter.System.AdvanceDay(day, rng);
            if (climate != null && !string.IsNullOrEmpty(climate.PhaseId)
                && !string.Equals(climate.PhaseId, _lastWinterPhaseId, StringComparison.Ordinal))
            {
                _lastWinterPhaseId = climate.PhaseId;
                _journal?.TryAddRawEntry(
                    "nuclear_winter_phase",
                    $"The sky settled into {climate.PhaseDisplayName} ({climate.SeasonDisplayName}); the shelter adjusts its fuel and planting plans.",
                    null!, day);
                _orphanSealWave1Dirty = true;
            }
        }

        private void TickDisasterResponse(int day)
        {
            if (_disasterResponse == null) return;
            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Shelter, day, 41)
                : new SeededRng(4100 + day);
            foreach (var disaster in _disasterResponse.Disasters)
            {
                if (disaster == null) continue;
                bool resolved = _disasterResponse.TickDisaster(disaster.DisasterId, 1.0, day, rng);
                if (resolved)
                {
                    _journal?.TryAddRawEntry(
                        "disaster_resolved",
                        $"Response teams closed out {disaster.Type} affecting {disaster.AffectedRoomIds?.Count ?? 0} room(s).",
                        null!, day);
                    _orphanSealWave1Dirty = true;
                }
            }
        }

        private void TickCommunications(int day)
        {
            _communications?.DegradeAntennas(0.2);
        }

        private void TickSurvivorAutonomy(int day)
        {
            if (_survivorAutonomy == null || _survivors == null) return;
            var roster = _survivors.RosterState;
            if (roster == null || roster.Count == 0) return;
            for (int i = 0; i < roster.Count; i++)
            {
                var survivor = roster[i];
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                var action = _survivorAutonomy.EvaluateDaily(
                    survivor.Id, day, survivor.Morale, survivor.Fatigue);
                if (action != null)
                {
                    _orphanSealWave1Dirty = true;
                    _journal?.TryAddRawEntry(
                        "survivor_autonomy",
                        action.description.Length > 0
                            ? action.description
                            : $"{survivor.Id} followed a personal priority: {action.title}.",
                        null!, day);
                }
            }
        }

        private void TickSeasonalCelebration(int day)
        {
            if (_seasonalCelebration == null) return;
            var holiday = _seasonalCelebration.CheckHoliday(day);
            if (holiday == null) return;
            if (_seasonalCelebration.System.IsHolidayOccurrenceResolved(holiday.HolidayId, day))
                return;
            _journal?.TryAddRawEntry(
                "seasonal_holiday",
                $"Today is {holiday.Name}. The shelter decides whether to mark it or let it pass quietly.",
                null!, day);
            _orphanSealWave1Dirty = true;
        }

        private void TickShelterOperationsCrews(int day)
        {
            if (_shelterExpansion == null) return;
            var rng = _campaignDay?.Rng?.Fork(CampaignStreamIds.Shelter, day, 61)
                ?? new SeededRng(6100 + day);
            var skills = EnsureSharedSkillProgression();
            _shelterExpansion.System.RecordCrewSkillPractice = (survivorId, skillId, xp, currentDay) =>
            {
                var definition = skills.GetSkill(skillId);
                if (definition == null) return;
                skills.RecordAction(
                    new SimpleSkillActor(survivorId),
                    definition.disciplineId,
                    xp,
                    currentDay,
                    rng);
            };
            int completed = _shelterExpansion.TickCrews(day);
            if (completed > 0)
                _shelterAssignment?.System.ApplyCapacityBonuses(
                    _shelterExpansion.System.GetCompletedCapacityBonuses());
        }

        private void ResetOrphanSealWave1()
        {
            _survivorAutonomy?.Dispose(); _survivorAutonomy = null;
            _nuclearWinter?.Dispose(); _nuclearWinter = null;
            _seasonalCelebration?.Dispose(); _seasonalCelebration = null;
            _disasterResponse?.Dispose(); _disasterResponse = null;
            _communications?.Dispose(); _communications = null;
            _colony?.Dispose(); _colony = null;
            _hobby?.Dispose(); _hobby = null;
            _survivorEducation?.Dispose(); _survivorEducation = null;
            _shelterExpansion?.Dispose(); _shelterExpansion = null;
            _confessionSecrets?.Dispose(); _confessionSecrets = null;
            _shelterFestival?.Dispose(); _shelterFestival = null;
            _covertOps?.Dispose(); _covertOps = null;
            _orphanSealWave1Dirty = false;
            _lastWinterPhaseId = string.Empty;
        }
    }
}
