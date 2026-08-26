using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.World;
using Ashfall.Core.Crafting;
using Ashfall.Core.Journal;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── 12 Expanded Shelter Host Sessions ──
        private WaterTreatmentHostSession _waterTreatment = null!;
        private AirlockSecurityHostSession _airlockSecurity = null!;
        private SurvivorRelationsHostSession _survivorRelations = null!;
        private RegionalTreatyHostSession _regionalTreaty = null!;
        private VinylMoraleHostSession _vinylMorale = null!;
        private WildlifeTrappingHostSession _wildlifeTrapping = null!;
        private ExcavationHostSession _excavation = null!;
        private ApprenticeshipHostSession _apprenticeship = null!;
        private ShelterThermalHostSession _shelterThermal = null!;
        private ShelterScheduleHostSession _shelterSchedule = null!;
        private AutopsyHostSession _autopsy = null!;
        private WaystationHostSession _waystation = null!;



        // Batch 4 BUG-14 follow-up: a SINGLE shared duty roster passed to
        // the eight systems above that consult the roster (apprenticeship,
        // library study, archive desk, contractor roster, mental health).
        // Previously each system held a fresh `new DutyRosterSystem()`, so
        // cross-system busy checks (mentor_busy / caregiver_busy) observed
        // an empty per-instance roster and never blocked.
        private readonly DutyRosterSystem _expandedShelterRoster = new DutyRosterSystem();

        // Promoted from SetupExpandedShelterSystems local — Apprenticeship needs
        // the core SurvivorRelationsSystem instance for its constructor.
        private SurvivorRelationsSystem _survivorRelationsCore = null!;

        // ── 22 UI Panels ──
        private WaterTreatmentPanel _waterTreatmentPanel = null!;
        private AirlockSecurityPanel _airlockSecurityPanel = null!;
        private SurvivorRelationsPanel _survivorRelationsPanel = null!;
        private RegionalTreatyPanel _regionalTreatyPanel = null!;
        private VinylMoralePanel _vinylMoralePanel = null!;
        private WildlifeTrappingPanel _wildlifeTrappingPanel = null!;
        private ExcavationPanel _excavationPanel = null!;
        private ApprenticeshipPanel _apprenticeshipPanel = null!;
        private ShelterThermalPanel _shelterThermalPanel = null!;
        private ShelterSchedulePanel _shelterSchedulePanel = null!;
        private AutopsyReportPanel _autopsyReportPanel = null!;
        private WaystationNetworkPanel _waystationPanel = null!;


        // ── Dirty Flags ──
        private bool _airlockSecurityDirty;
        private bool _survivorRelationsDirty;
        private bool _regionalTreatyDirty;
        private bool _vinylMoraleDirty;
        private bool _wildlifeTrappingDirty;
        private bool _excavationDirty;
        private bool _apprenticeshipDirty;
        private bool _shelterThermalDirty;
        private bool _shelterScheduleDirty;
        private bool _autopsyDirty;
        private bool _waystationDirty;



        private void SetupExpandedShelterSystems()
        {
            SetupSurvivors();
            SetupInventory();
            SetupPowerGrid();
            SetupJournal();
            SetupCrafting();
            SetupExpeditions();
            SetupMedical();
            SetupMedicalWard();
            SetupStartingLevel();
            SetupWorld();

            var sharedResearch = new ResearchSystem(log: new GodotLog());

            SetupWaterTreatment();
            SetupAirlockSecurity();
            SetupSurvivorRelations();
            SetupRegionalTreaty();
            SetupVinylMorale();
            SetupWildlifeTrapping();
            SetupExcavation();
            SetupApprenticeship();
            SetupShelterThermal();
            SetupShelterSchedule();
            SetupAutopsy(sharedResearch);
            SetupWaystation();
            SetupSumpFlooding();
            SetupDecontamination();
            SetupKitchenNutrition();
            SetupEquipmentCondition();
            SetupLibraryStudy(sharedResearch);
            SetupArchiveDesk();
            SetupContractorRoster();
            SetupMentalHealthCrisis();
            SetupShelterAssignment();   // last — post-wiring to Thermal + Phase0
        }

        private void SetupWaterTreatment()
        {
            var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
            var wtSys = new WaterTreatmentSystem(new GodotLog());
            wtSys.RestoreState(wtState);
            _waterTreatment = new WaterTreatmentHostSession(wtSys);
            _waterTreatment.StateChanged += () => _waterTreatment.MarkDirty();
            _waterTreatmentPanel = new WaterTreatmentPanel();
            _waterTreatmentPanel.Bind(_waterTreatment);
            _waterTreatmentPanel.Visible = false;
            AddChild(_waterTreatmentPanel);
        }

        private void SetupAirlockSecurity()
        {
            var asState = AirlockSecuritySaveStore.TryLoad() ?? new AirlockSecurityState();
            var asSys = new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());
            asSys.RestoreState(asState);
            _airlockSecurity = new AirlockSecurityHostSession(asSys);
            _airlockSecurity.StateChanged += () => _airlockSecurity.MarkDirty();
            _airlockSecurityPanel = new AirlockSecurityPanel();
            _airlockSecurityPanel.Bind(_airlockSecurity);
            _airlockSecurityPanel.Visible = false;
            AddChild(_airlockSecurityPanel);
        }

        private void SetupSurvivorRelations()
        {
            var srState = SurvivorRelationsSaveStore.TryLoad() ?? new SurvivorRelationsState();
            var srSys = new SurvivorRelationsSystem(new SeededRng(1986), new GodotLog());
            _survivorRelationsCore = srSys;
            srSys.RestoreState(srState);
            _survivorRelations = new SurvivorRelationsHostSession(srSys);
            _survivorRelations.StateChanged += () => _survivorRelations.MarkDirty();
            _survivorRelationsPanel = new SurvivorRelationsPanel();
            _survivorRelationsPanel.Bind(_survivorRelations);
            _survivorRelationsPanel.Visible = false;
            AddChild(_survivorRelationsPanel);
        }

        private void SetupRegionalTreaty()
        {
            var rtState = RegionalTreatySaveStore.TryLoad() ?? new RegionalTreatyState();
            var rtSys = new RegionalTreatySystem(new GodotLog());
            rtSys.RestoreState(rtState);
            _regionalTreaty = new RegionalTreatyHostSession(rtSys);
            _regionalTreaty.StateChanged += () => _regionalTreaty.MarkDirty();
            _regionalTreatyPanel = new RegionalTreatyPanel();
            _regionalTreatyPanel.Bind(_regionalTreaty);
            _regionalTreatyPanel.Visible = false;
            AddChild(_regionalTreatyPanel);
        }

        private void SetupVinylMorale()
        {
            var vmState = VinylMoraleSaveStore.TryLoad() ?? new VinylMoraleState();
            var vmSys = new VinylMoraleSystem(new GodotLog());
            vmSys.RestoreState(vmState);
            _vinylMorale = new VinylMoraleHostSession(vmSys);
            _vinylMorale.StateChanged += () => _vinylMorale.MarkDirty();
            _vinylMoralePanel = new VinylMoralePanel();
            _vinylMoralePanel.Bind(_vinylMorale);
            _vinylMoralePanel.Visible = false;
            AddChild(_vinylMoralePanel);
        }

        private void SetupWildlifeTrapping()
        {
            var wtrapState = WildlifeTrappingSaveStore.TryLoad() ?? new WildlifeTrappingState();
            var wtrapSys = new WildlifeTrappingSystem(new SeededRng(1986), new GodotLog());
            wtrapSys.RestoreState(wtrapState);
            _wildlifeTrapping = new WildlifeTrappingHostSession(wtrapSys);
            _wildlifeTrapping.StateChanged += () => _wildlifeTrapping.MarkDirty();
            _wildlifeTrappingPanel = new WildlifeTrappingPanel();
            _wildlifeTrappingPanel.Bind(_wildlifeTrapping);
            _wildlifeTrappingPanel.Visible = false;
            AddChild(_wildlifeTrappingPanel);
        }

        private void SetupExcavation()
        {
            var exState = ExcavationSaveStore.TryLoad() ?? new ExcavationState();
            var exSys = new ExcavationSystem(new SeededRng(1986), new GodotLog());
            exSys.RestoreState(exState);
            _excavation = new ExcavationHostSession(exSys);
            _excavation.StateChanged += () => _excavation.MarkDirty();
            _excavationPanel = new ExcavationPanel();
            _excavationPanel.Bind(_excavation);
            _excavationPanel.Visible = false;
            AddChild(_excavationPanel);
        }

        private void SetupApprenticeship()
        {
            var appState = ApprenticeshipSaveStore.TryLoad() ?? new ApprenticeshipState();
            var appSkills = new SkillProgressionSystem();
            var appSys = new ApprenticeshipSystem(new SeededRng(1986), appSkills, _expandedShelterRoster, _survivorRelationsCore, new GodotLog());
            appSys.RestoreState(appState);
            _apprenticeship = new ApprenticeshipHostSession(appSys);
            _apprenticeship.StateChanged += () => _apprenticeship.MarkDirty();
            _apprenticeshipPanel = new ApprenticeshipPanel();
            _apprenticeshipPanel.Bind(_apprenticeship);
            _apprenticeshipPanel.Visible = false;
            AddChild(_apprenticeshipPanel);
        }

        private void SetupShelterThermal()
        {
            var stState = ShelterThermalSaveStore.TryLoad() ?? new ShelterThermalState();
            var stNeeds = _survivors.Needs;
            var stStarting = _startingLevel.System;
            var stDeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
            var stSys = new ShelterThermalSystem(new SeededRng(1986), stNeeds, stStarting, stDeepFreeze, new GodotLog());
            stSys.RestoreState(stState);
            _shelterThermal = new ShelterThermalHostSession(stSys);
            _shelterThermal.StateChanged += () => _shelterThermal.MarkDirty();
            _shelterThermalPanel = new ShelterThermalPanel();
            _shelterThermalPanel.Bind(_shelterThermal);
            _shelterThermalPanel.Visible = false;
            AddChild(_shelterThermalPanel);
        }

        private void SetupShelterSchedule()
        {
            var ssState = ShelterScheduleSaveStore.TryLoad() ?? new ShelterScheduleState();
            var ssPower = _powerGrid.System;
            var ssSys = new ShelterScheduleSystem(ssPower, new GodotLog());
            ssSys.RestoreState(ssState);
            _shelterSchedule = new ShelterScheduleHostSession(ssSys);
            _shelterSchedule.LoadCatalog(_dataDir);
            _shelterSchedule.StateChanged += () => _shelterSchedule.MarkDirty();
            _shelterSchedulePanel = new ShelterSchedulePanel();
            _shelterSchedulePanel.Bind(_shelterSchedule);
            _shelterSchedulePanel.Visible = false;
            AddChild(_shelterSchedulePanel);
        }

        private void SetupAutopsy(ResearchSystem sharedResearch)
        {
            var auState = AutopsySaveStore.TryLoad() ?? new AutopsyState();
            var auInv = _inventory.Inventory;
            var auRad = _survivors.Radiation;
            var auStarting = _startingLevel.System;
            var auVent = new VentilationSystem(auStarting);
            var auRes = sharedResearch;
            var auMedical = _medicalWard;
            var auSys = new AutopsySystem(new SeededRng(1986), auInv, auRad, auVent, auRes, auMedical, new GodotLog());
            auSys.RestoreState(auState);
            _autopsy = new AutopsyHostSession(auSys);
            _autopsy.LoadCatalog(_dataDir);
            _autopsy.StateChanged += () => _autopsy.MarkDirty();
            _autopsyReportPanel = new AutopsyReportPanel();
            _autopsyReportPanel.Bind(_autopsy);
            _autopsyReportPanel.Visible = false;
            AddChild(_autopsyReportPanel);
        }

        private void SetupWaystation()
        {
            var wsState = WaystationSaveStore.TryLoad() ?? new WaystationSystemState();
            var wsSys = new WaystationSystem();
            wsSys.RestoreState(wsState);
            _waystation = new WaystationHostSession(wsSys);
            _waystation.StateChanged += () => _waystation.MarkDirty();
            _waystationPanel = new WaystationNetworkPanel();
            _waystationPanel.Bind(_waystation);
            _waystationPanel.Visible = false;
            AddChild(_waystationPanel);
        }

        private void SaveAllExpandedShelterSystems()
        {
            SaveWaterTreatment();
            SaveAirlockSecurity();
            SaveSurvivorRelations();
            SaveRegionalTreaty();
            SaveVinylMorale();
            SaveWildlifeTrapping();
            SaveExcavation();
            SaveApprenticeship();
            SaveShelterThermal();
            SaveShelterSchedule();
            SaveAutopsy();
            SaveWaystation();
            SaveSumpFlooding();
            SaveDecontamination();
            SaveKitchenNutrition();
            SaveEquipmentCondition();
            SaveLibraryStudy();
            SaveArchiveDesk();
            SaveContractorRoster();
            SaveMentalHealthCrisis();
            SaveChemicalDependency();
            SaveShelterAssignment();
        }

        private void SaveWaterTreatment()
        {
            _waterTreatment?.Save();
        }

        private void SaveAirlockSecurity()
        {
            _airlockSecurity?.Save();
        }

        private void SaveSurvivorRelations()
        {
            _survivorRelations?.Save();
        }

        private void SaveRegionalTreaty()
        {
            _regionalTreaty?.Save();
        }

        private void SaveVinylMorale()
        {
            _vinylMorale?.Save();
        }

        private void SaveWildlifeTrapping()
        {
            _wildlifeTrapping?.Save();
        }

        private void SaveExcavation()
        {
            _excavation?.Save();
        }

        private void SaveApprenticeship()
        {
            _apprenticeship?.Save();
        }

        private void SaveShelterThermal()
        {
            _shelterThermal?.Save();
        }

        private void SaveShelterSchedule()
        {
            _shelterSchedule?.Save();
        }

        private void SaveAutopsy()
        {
            _autopsy?.Save();
        }

        private void SaveWaystation()
        {
            _waystation?.Save();
        }

        private void TickAllExpandedShelterSystems(int day)
        {
            _waterTreatment?.TickDay(day);
            _airlockSecurity?.TickDay(day);
            _survivorRelations?.TickDay(day);
            _regionalTreaty?.TickDay(day);
            _vinylMorale?.TickDay(day);
            _wildlifeTrapping?.TickDay(day);
            _excavation?.TickDay();
            _apprenticeship?.TickDay(day);
            _shelterThermal?.TickDay(day);
            _shelterSchedule?.TickDay(day);
            _autopsy?.TickDay(day);
            _waystation?.TickDaily(iceRoadOpen: true);
            _sumpFlooding?.TickDay(day);
            _decontamination?.TickDay(day);
            _kitchenNutrition?.TickDay(day);
            _equipmentCondition?.TickDay(day);
            _libraryStudy?.TickDay(day);
            _archiveDesk?.TickDay(day);
            _contractorRoster?.TickDay(day);
            _mentalHealthCrisis?.TickDay(day);
        }

        public void OpenExpandedPanel(string panelKey)
        {
            switch (panelKey)
            {
                case "water_treatment":
                    if (_waterTreatmentPanel != null) { _waterTreatmentPanel.Visible = true; _waterTreatmentPanel.RefreshView(); }
                    break;
                case "airlock_security":
                    if (_airlockSecurityPanel != null) { _airlockSecurityPanel.Visible = true; _airlockSecurityPanel.RefreshView(); }
                    break;
                case "survivor_relations":
                    if (_survivorRelationsPanel != null) { _survivorRelationsPanel.Visible = true; _survivorRelationsPanel.RefreshView(); }
                    break;
                case "regional_treaty":
                    if (_regionalTreatyPanel != null) { _regionalTreatyPanel.Visible = true; _regionalTreatyPanel.RefreshView(); }
                    break;
                case "vinyl_morale":
                    if (_vinylMoralePanel != null) { _vinylMoralePanel.Visible = true; _vinylMoralePanel.RefreshView(); }
                    break;
                case "wildlife_trapping":
                    if (_wildlifeTrappingPanel != null) { _wildlifeTrappingPanel.Visible = true; _wildlifeTrappingPanel.RefreshView(); }
                    break;
                case "excavation":
                    if (_excavationPanel != null) { _excavationPanel.Visible = true; _excavationPanel.RefreshView(); }
                    break;
                case "apprenticeship":
                    if (_apprenticeshipPanel != null) { _apprenticeshipPanel.Visible = true; _apprenticeshipPanel.RefreshView(); }
                    break;
                case "shelter_thermal":
                    if (_shelterThermalPanel != null) { _shelterThermalPanel.Visible = true; _shelterThermalPanel.RefreshView(); }
                    break;
                case "shelter_schedule":
                    if (_shelterSchedulePanel != null) { _shelterSchedulePanel.Visible = true; _shelterSchedulePanel.RefreshView(); }
                    break;
                case "autopsy_report":
                    if (_autopsyReportPanel != null) { _autopsyReportPanel.Visible = true; _autopsyReportPanel.RefreshView(); }
                    break;
                case "waystation_network":
                    if (_waystationPanel != null) { _waystationPanel.Visible = true; _waystationPanel.RefreshView(); }
                    break;
                case "chemical_dependency":
                    if (_chemicalDependencyPanel != null) { _chemicalDependencyPanel.Visible = true; _chemicalDependencyPanel.RefreshView(); }
                    break;
                case "sump_flooding":
                    if (_sumpFloodingPanel != null) { _sumpFloodingPanel.Visible = true; _sumpFloodingPanel.RefreshView(); }
                    break;
                case "decontamination":
                    if (_decontaminationPanel != null) { _decontaminationPanel.Visible = true; _decontaminationPanel.RefreshView(); }
                    break;
                case "kitchen_nutrition":
                    if (_kitchenNutritionPanel != null) { _kitchenNutritionPanel.Visible = true; _kitchenNutritionPanel.RefreshView(); }
                    break;
                case "equipment_condition":
                    if (_equipmentConditionPanel != null) { _equipmentConditionPanel.Visible = true; _equipmentConditionPanel.RefreshView(); }
                    break;
                case "library_study":
                    if (_libraryStudyPanel != null) { _libraryStudyPanel.Visible = true; _libraryStudyPanel.RefreshView(); }
                    break;
                case "archive_desk":
                    if (_archiveDeskPanel != null) { _archiveDeskPanel.Visible = true; _archiveDeskPanel.RefreshView(); }
                    break;
                case "contractor_roster":
                    if (_contractorRosterPanel != null) { _contractorRosterPanel.Visible = true; _contractorRosterPanel.RefreshView(); }
                    break;
                case "mental_health_crisis":
                    if (_mentalHealthCrisisPanel != null) { _mentalHealthCrisisPanel.Visible = true; _mentalHealthCrisisPanel.RefreshView(); }
                    break;
                case "phantom_memory":
                    if (_phantomMemoryPanel != null) { _phantomMemoryPanel.Visible = true; _phantomMemoryPanel.RefreshView(); }
                    break;
                case "traveling_caravan":
                    if (_travelingCaravanPanel != null) { _travelingCaravanPanel.Visible = true; _travelingCaravanPanel.RefreshView(); }
                    break;
                case "medical_ward":
                    SetupMedicalWard();
                    if (_medicalWardPanel != null) { _medicalWardPanel.Visible = true; _medicalWardPanel.RefreshView(); }
                    break;
                case "journal":
                    SetupJournal();
                    if (_journalPanel != null) { _journalPanel.Bind(_journal); _journalPanel.Visible = true; _journalPanel.RefreshView(); }
                    break;
                case "weather":
                    SetupWorld();
                    if (_weatherPanel != null) { _weatherPanel.Bind(_world); _weatherPanel.Visible = true; _weatherPanel.RefreshView(); }
                    break;
            }
        }
    }
}
