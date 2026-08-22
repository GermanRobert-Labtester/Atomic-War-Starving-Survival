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

        // ── 8 Batch-3 Host Sessions (Phase 13 wiring) ──
        private SumpFloodingHostSession _sumpFlooding = null!;
        private DecontaminationHostSession _decontamination = null!;
        private KitchenNutritionHostSession _kitchenNutrition = null!;
        private EquipmentConditionHostSession _equipmentCondition = null!;
        private LibraryStudyHostSession _libraryStudy = null!;
        private ArchiveDeskHostSession _archiveDesk = null!;
        private ContractorRosterHostSession _contractorRoster = null!;
        private MentalHealthCrisisHostSession _mentalHealthCrisis = null!;

        // ── 12 UI Panels ──
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
        private bool _waterTreatmentDirty;
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

        // ── 8 Batch-3 Dirty Flags (Phase 13) ──
        private bool _sumpFloodingDirty;
        private bool _decontaminationDirty;
        private bool _kitchenNutritionDirty;
        private bool _equipmentConditionDirty;
        private bool _libraryStudyDirty;
        private bool _archiveDeskDirty;
        private bool _contractorRosterDirty;
        private bool _mentalHealthCrisisDirty;

        private void SetupExpandedShelterSystems()
        {
            // 1. Water Treatment
            var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
            var wtSys = new WaterTreatmentSystem(new GodotLog());
            wtSys.RestoreState(wtState);
            _waterTreatment = new WaterTreatmentHostSession(wtSys);
            _waterTreatment.StateChanged += () => _waterTreatmentDirty = true;
            _waterTreatmentPanel = new WaterTreatmentPanel();
            _waterTreatmentPanel.Bind(_waterTreatment);
            _waterTreatmentPanel.Visible = false;
            AddChild(_waterTreatmentPanel);

            // 2. Airlock Security
            var asState = AirlockSecuritySaveStore.TryLoad() ?? new AirlockSecurityState();
            var asSys = new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());
            asSys.RestoreState(asState);
            _airlockSecurity = new AirlockSecurityHostSession(asSys);
            _airlockSecurity.StateChanged += () => _airlockSecurityDirty = true;
            _airlockSecurityPanel = new AirlockSecurityPanel();
            _airlockSecurityPanel.Bind(_airlockSecurity);
            _airlockSecurityPanel.Visible = false;
            AddChild(_airlockSecurityPanel);

            // 3. Survivor Relations
            var srState = SurvivorRelationsSaveStore.TryLoad() ?? new SurvivorRelationsState();
            var srSys = new SurvivorRelationsSystem(new SeededRng(1986), new GodotLog());
            srSys.RestoreState(srState);
            _survivorRelations = new SurvivorRelationsHostSession(srSys);
            _survivorRelations.StateChanged += () => _survivorRelationsDirty = true;
            _survivorRelationsPanel = new SurvivorRelationsPanel();
            _survivorRelationsPanel.Bind(_survivorRelations);
            _survivorRelationsPanel.Visible = false;
            AddChild(_survivorRelationsPanel);

            // 4. Regional Treaty
            var rtState = RegionalTreatySaveStore.TryLoad() ?? new RegionalTreatyState();
            var rtSys = new RegionalTreatySystem(new GodotLog());
            rtSys.RestoreState(rtState);
            _regionalTreaty = new RegionalTreatyHostSession(rtSys);
            _regionalTreaty.StateChanged += () => _regionalTreatyDirty = true;
            _regionalTreatyPanel = new RegionalTreatyPanel();
            _regionalTreatyPanel.Bind(_regionalTreaty);
            _regionalTreatyPanel.Visible = false;
            AddChild(_regionalTreatyPanel);

            // 5. Vinyl Morale
            var vmState = VinylMoraleSaveStore.TryLoad() ?? new VinylMoraleState();
            var vmSys = new VinylMoraleSystem(new GodotLog());
            vmSys.RestoreState(vmState);
            _vinylMorale = new VinylMoraleHostSession(vmSys);
            _vinylMorale.StateChanged += () => _vinylMoraleDirty = true;
            _vinylMoralePanel = new VinylMoralePanel();
            _vinylMoralePanel.Bind(_vinylMorale);
            _vinylMoralePanel.Visible = false;
            AddChild(_vinylMoralePanel);

            // 6. Wildlife Trapping
            var wtrapState = WildlifeTrappingSaveStore.TryLoad() ?? new WildlifeTrappingState();
            var wtrapSys = new WildlifeTrappingSystem(new SeededRng(1986), new GodotLog());
            wtrapSys.RestoreState(wtrapState);
            _wildlifeTrapping = new WildlifeTrappingHostSession(wtrapSys);
            _wildlifeTrapping.StateChanged += () => _wildlifeTrappingDirty = true;
            _wildlifeTrappingPanel = new WildlifeTrappingPanel();
            _wildlifeTrappingPanel.Bind(_wildlifeTrapping);
            _wildlifeTrappingPanel.Visible = false;
            AddChild(_wildlifeTrappingPanel);

            // 7. Excavation
            var exState = ExcavationSaveStore.TryLoad() ?? new ExcavationState();
            var exSys = new ExcavationSystem(new SeededRng(1986), new GodotLog());
            exSys.RestoreState(exState);
            _excavation = new ExcavationHostSession(exSys);
            _excavation.StateChanged += () => _excavationDirty = true;
            _excavationPanel = new ExcavationPanel();
            _excavationPanel.Bind(_excavation);
            _excavationPanel.Visible = false;
            AddChild(_excavationPanel);

            // 8. Apprenticeship
            var appState = ApprenticeshipSaveStore.TryLoad() ?? new ApprenticeshipState();
            var appSkills = new SkillProgressionSystem();
            var appRoster = new DutyRosterSystem();
            var appSys = new ApprenticeshipSystem(new SeededRng(1986), appSkills, appRoster, srSys, new GodotLog());
            appSys.RestoreState(appState);
            _apprenticeship = new ApprenticeshipHostSession(appSys);
            _apprenticeship.StateChanged += () => _apprenticeshipDirty = true;
            _apprenticeshipPanel = new ApprenticeshipPanel();
            _apprenticeshipPanel.Bind(_apprenticeship);
            _apprenticeshipPanel.Visible = false;
            AddChild(_apprenticeshipPanel);

            // 9. Shelter Thermal
            var stState = ShelterThermalSaveStore.TryLoad() ?? new ShelterThermalState();
            var stNeeds = new NeedsSystem();
            var stStarting = new StartingLevelSystem();
            var stDeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
            var stSys = new ShelterThermalSystem(new SeededRng(1986), stNeeds, stStarting, stDeepFreeze, new GodotLog());
            stSys.RestoreState(stState);
            _shelterThermal = new ShelterThermalHostSession(stSys);
            _shelterThermal.StateChanged += () => _shelterThermalDirty = true;
            _shelterThermalPanel = new ShelterThermalPanel();
            _shelterThermalPanel.Bind(_shelterThermal);
            _shelterThermalPanel.Visible = false;
            AddChild(_shelterThermalPanel);

            // 10. Shelter Schedule
            var ssState = ShelterScheduleSaveStore.TryLoad() ?? new ShelterScheduleState();
            var ssPowerState = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var ssRooms = new List<PowerGridRoom> { new PowerGridRoom("room_main", "Main Vault", 100f) };
            var ssPower = new PowerGridSystem(ssPowerState, ssRooms, new SeededRng(1986));
            var ssSys = new ShelterScheduleSystem(ssPower, new GodotLog());
            ssSys.RestoreState(ssState);
            _shelterSchedule = new ShelterScheduleHostSession(ssSys);
            _shelterSchedule.StateChanged += () => _shelterScheduleDirty = true;
            _shelterSchedulePanel = new ShelterSchedulePanel();
            _shelterSchedulePanel.Bind(_shelterSchedule);
            _shelterSchedulePanel.Visible = false;
            AddChild(_shelterSchedulePanel);

            // 11. Autopsy
            var auState = AutopsySaveStore.TryLoad() ?? new AutopsyState();
            var auInv = new Ashfall.Core.Inventory.Inventory();
            var auRad = new RadiationSystem(seed: 1986);
            var auStarting = new StartingLevelSystem();
            var auVent = new VentilationSystem(auStarting);
            var auRes = new ResearchSystem();
            var auWardState = new MedicalWardState();
            var auBed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
            var auProc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            var auMedical = new MedicalWardSystem(auWardState, new[] { auBed }, new[] { auProc });
            var auSys = new AutopsySystem(new SeededRng(1986), auInv, auRad, auVent, auRes, auMedical, new GodotLog());
            auSys.RestoreState(auState);
            _autopsy = new AutopsyHostSession(auSys);
            _autopsy.StateChanged += () => _autopsyDirty = true;
            _autopsyReportPanel = new AutopsyReportPanel();
            _autopsyReportPanel.Bind(_autopsy);
            _autopsyReportPanel.Visible = false;
            AddChild(_autopsyReportPanel);

            // 12. Waystation
            var wsState = WaystationSaveStore.TryLoad() ?? new WaystationSystemState();
            var wsSys = new WaystationSystem();
            wsSys.RestoreState(wsState);
            _waystation = new WaystationHostSession(wsSys);
            _waystation.StateChanged += () => _waystationDirty = true;
            _waystationPanel = new WaystationNetworkPanel();
            _waystationPanel.Bind(_waystation);
            _waystationPanel.Visible = false;
            AddChild(_waystationPanel);

            // 13. Sump Flooding
            var sfState = SumpFloodingSaveStore.TryLoad() ?? new SumpFloodingState();
            var sfWeather = new WeatherSystem();
            sfWeather.BindProfile(new SeasonProfileDef { id = "default" }, 1986);
            var sfPowerState = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var sfRooms = new List<PowerGridRoom> { new PowerGridRoom("sump_a", "Lower Level", 100f) };
            var sfPower = new PowerGridSystem(sfPowerState, sfRooms, new SeededRng(1986));
            var sfDeepFreeze = new YearOfAshDeepFreezeSystem();
            var sfSys = new SumpFloodingSystem(new SeededRng(1986), sfWeather, sfPower, sfDeepFreeze, new GodotLog());
            sfSys.RestoreState(sfState);
            _sumpFlooding = new SumpFloodingHostSession(sfSys, sfWeather, sfPower, sfDeepFreeze);
            _sumpFlooding.StateChanged += () => _sumpFloodingDirty = true;

            // 14. Decontamination
            var dcState = DecontaminationSaveStore.TryLoad() ?? new DecontaminationState();
            var dcInv = new Ashfall.Core.Inventory.Inventory();
            var dcRad = new RadiationSystem(seed: 1986);
            var dcAirlock = new AirlockSecuritySystem(new SeededRng(1986));
            var dcStarting = new StartingLevelSystem();
            var dcSys = new DecontaminationSystem(new SeededRng(1986), dcRad, dcInv, dcAirlock, dcStarting, new GodotLog());
            dcSys.RestoreState(dcState);
            _decontamination = new DecontaminationHostSession(dcSys, dcRad, dcInv, dcAirlock, dcStarting);
            _decontamination.StateChanged += () => _decontaminationDirty = true;

            // 15. Kitchen Nutrition
            var knState = KitchenNutritionSaveStore.TryLoad() ?? new KitchenNutritionState();
            var knInv = new Ashfall.Core.Inventory.Inventory();
            var knNeeds = new NeedsSystem();
            var knSys = new KitchenNutritionSystem(new SeededRng(1986), knInv, knNeeds, new GodotLog());
            knSys.RestoreState(knState);
            _kitchenNutrition = new KitchenNutritionHostSession(knSys, knInv, knNeeds);
            _kitchenNutrition.StateChanged += () => _kitchenNutritionDirty = true;

            // 16. Equipment Condition
            var ecState = EquipmentConditionSaveStore.TryLoad() ?? new EquipmentConditionState();
            var ecInv = new Ashfall.Core.Inventory.Inventory();
            var ecCrafting = new CraftingSystem(ecInv);
            var ecSys = new EquipmentConditionSystem(new SeededRng(1986), ecInv, ecCrafting, new GodotLog());
            ecSys.RestoreState(ecState);
            _equipmentCondition = new EquipmentConditionHostSession(ecSys, ecInv, ecCrafting);
            _equipmentCondition.StateChanged += () => _equipmentConditionDirty = true;

            // 17. Library Study
            var lsState = LibraryStudySaveStore.TryLoad() ?? new LibraryStudyState();
            var lsSkills = new SkillProgressionSystem();
            var lsResearch = new ResearchSystem();
            var lsJournal = new JournalSystem();
            var lsRoster = new DutyRosterSystem();
            var lsSys = new LibraryStudySystem(lsSkills, lsResearch, lsJournal, lsRoster, new GodotLog());
            lsSys.RestoreState(lsState);
            _libraryStudy = new LibraryStudyHostSession(lsSys, lsSkills, lsResearch, lsJournal, lsRoster);
            _libraryStudy.StateChanged += () => _libraryStudyDirty = true;

            // 18. Archive Desk
            var adState = ArchiveDeskSaveStore.TryLoad() ?? new ArchiveDeskState();
            var adJournal = new JournalSystem();
            var adKnowledge = new KnowledgeBase();
            var adInv = new Ashfall.Core.Inventory.Inventory();
            var adRoster = new DutyRosterSystem();
            var adSys = new ArchiveDeskSystem(adJournal, adKnowledge, adInv, adRoster, new GodotLog());
            adSys.RestoreState(adState);
            _archiveDesk = new ArchiveDeskHostSession(adSys, adJournal, adKnowledge, adInv, adRoster);
            _archiveDesk.StateChanged += () => _archiveDeskDirty = true;

            // 19. Contractor Roster
            var crState = ContractorRosterSaveStore.TryLoad() ?? new ContractorRosterState();
            var crInv = new Ashfall.Core.Inventory.Inventory();
            var crRoster = new DutyRosterSystem();
            var crExpedition = new ExpeditionSystem();
            var crSys = new ContractorRosterSystem(new SeededRng(1986), crInv, crRoster, crExpedition, new GodotLog());
            crSys.RestoreState(crState);
            _contractorRoster = new ContractorRosterHostSession(crSys, crInv, crRoster, crExpedition);
            _contractorRoster.StateChanged += () => _contractorRosterDirty = true;

            // 20. Mental Health Crisis
            var mhState = MentalHealthCrisisSaveStore.TryLoad() ?? new MentalHealthState();
            var mhNeeds = new NeedsSystem();
            var mhWardState = new MedicalWardState();
            var mhBed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.Psychiatric);
            var mhProc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            var mhMedical = new MedicalWardSystem(mhWardState, new[] { mhBed }, new[] { mhProc });
            var mhDependency = new ChemicalDependencySystem();
            var mhRoster = new DutyRosterSystem();
            var mhSys = new MentalHealthCrisisSystem(new SeededRng(1986), mhNeeds, mhMedical, mhDependency, mhRoster, new GodotLog());
            mhSys.RestoreState(mhState);
            _mentalHealthCrisis = new MentalHealthCrisisHostSession(mhSys, mhNeeds, mhMedical, mhDependency, mhRoster);
            _mentalHealthCrisis.StateChanged += () => _mentalHealthCrisisDirty = true;
        }

        private void SaveAllExpandedShelterSystems()
        {
            if (_waterTreatment != null)
            {
                WaterTreatmentSaveStore.TrySave(_waterTreatment.System.CaptureState());
                _waterTreatmentDirty = false;
            }
            if (_airlockSecurity != null)
            {
                AirlockSecuritySaveStore.TrySave(_airlockSecurity.System.CaptureState());
                _airlockSecurityDirty = false;
            }
            if (_survivorRelations != null)
            {
                SurvivorRelationsSaveStore.TrySave(_survivorRelations.System.CaptureState());
                _survivorRelationsDirty = false;
            }
            if (_regionalTreaty != null)
            {
                RegionalTreatySaveStore.TrySave(_regionalTreaty.System.CaptureState());
                _regionalTreatyDirty = false;
            }
            if (_vinylMorale != null)
            {
                VinylMoraleSaveStore.TrySave(_vinylMorale.System.CaptureState());
                _vinylMoraleDirty = false;
            }
            if (_wildlifeTrapping != null)
            {
                WildlifeTrappingSaveStore.TrySave(_wildlifeTrapping.System.CaptureState());
                _wildlifeTrappingDirty = false;
            }
            if (_excavation != null)
            {
                ExcavationSaveStore.TrySave(_excavation.System.CaptureState());
                _excavationDirty = false;
            }
            if (_apprenticeship != null)
            {
                ApprenticeshipSaveStore.TrySave(_apprenticeship.System.CaptureState());
                _apprenticeshipDirty = false;
            }
            if (_shelterThermal != null)
            {
                ShelterThermalSaveStore.TrySave(_shelterThermal.System.CaptureState());
                _shelterThermalDirty = false;
            }
            if (_shelterSchedule != null)
            {
                ShelterScheduleSaveStore.TrySave(_shelterSchedule.System.CaptureState());
                _shelterScheduleDirty = false;
            }
            if (_autopsy != null)
            {
                AutopsySaveStore.TrySave(_autopsy.System.CaptureState());
                _autopsyDirty = false;
            }
            if (_waystation != null)
            {
                WaystationSaveStore.TrySave(_waystation.System.CaptureState());
                _waystationDirty = false;
            }
            if (_sumpFlooding != null)
            {
                SumpFloodingSaveStore.TrySave(_sumpFlooding.System.CaptureState());
                _sumpFloodingDirty = false;
            }
            if (_decontamination != null)
            {
                DecontaminationSaveStore.TrySave(_decontamination.System.CaptureState());
                _decontaminationDirty = false;
            }
            if (_kitchenNutrition != null)
            {
                KitchenNutritionSaveStore.TrySave(_kitchenNutrition.System.CaptureState());
                _kitchenNutritionDirty = false;
            }
            if (_equipmentCondition != null)
            {
                EquipmentConditionSaveStore.TrySave(_equipmentCondition.System.CaptureState());
                _equipmentConditionDirty = false;
            }
            if (_libraryStudy != null)
            {
                LibraryStudySaveStore.TrySave(_libraryStudy.System.CaptureState());
                _libraryStudyDirty = false;
            }
            if (_archiveDesk != null)
            {
                ArchiveDeskSaveStore.TrySave(_archiveDesk.System.CaptureState());
                _archiveDeskDirty = false;
            }
            if (_contractorRoster != null)
            {
                ContractorRosterSaveStore.TrySave(_contractorRoster.System.CaptureState());
                _contractorRosterDirty = false;
            }
            if (_mentalHealthCrisis != null)
            {
                MentalHealthCrisisSaveStore.TrySave(_mentalHealthCrisis.System.CaptureState());
                _mentalHealthCrisisDirty = false;
            }
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
            }
        }
    }
}
