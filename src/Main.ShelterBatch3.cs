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
        // ── 8 Batch-3 Host Sessions (Phase 13) ──
        private SumpFloodingHostSession _sumpFlooding = null!;
        private DecontaminationHostSession _decontamination = null!;
        private KitchenNutritionHostSession _kitchenNutrition = null!;
        private EquipmentConditionHostSession _equipmentCondition = null!;
        private LibraryStudyHostSession _libraryStudy = null!;
        private ArchiveDeskHostSession _archiveDesk = null!;
        private ContractorRosterHostSession _contractorRoster = null!;
        private MentalHealthCrisisHostSession _mentalHealthCrisis = null!;
        private ChemicalDependencyHostSession _chemicalDependency = null!;

        // Phantom / Traveling / ShelterAssignment (created inside MentalHealth/Assignment)
        private PhantomMemoryPanel _phantomMemoryPanel = null!;
        private TravelingCaravanPanel _travelingCaravanPanel = null!;
        private TravelingCaravanHostSession _travelingCaravan = null!;
        private ShelterAssignmentHostSession _shelterAssignment = null!;

        private SumpFloodingPanel _sumpFloodingPanel = null!;
        private DecontaminationPanel _decontaminationPanel = null!;
        private KitchenNutritionPanel _kitchenNutritionPanel = null!;
        private EquipmentConditionPanel _equipmentConditionPanel = null!;
        private LibraryStudyPanel _libraryStudyPanel = null!;
        private ArchiveDeskPanel _archiveDeskPanel = null!;
        private ContractorRosterPanel _contractorRosterPanel = null!;
        private MentalHealthCrisisPanel _mentalHealthCrisisPanel = null!;
        private ChemicalDependencyPanel _chemicalDependencyPanel = null!;

        private bool _sumpFloodingDirty;
        private bool _decontaminationDirty;
        private bool _kitchenNutritionDirty;
        private bool _equipmentConditionDirty;
        private bool _libraryStudyDirty;
        private bool _archiveDeskDirty;
        private bool _contractorRosterDirty;
        private bool _mentalHealthCrisisDirty;

        private void SetupSumpFlooding()
        {
            var sfState = SumpFloodingSaveStore.TryLoad() ?? new SumpFloodingState();
            var sfWeather = _world.Weather;
            var sfPower = _powerGrid.System;
            var sfDeepFreeze = new YearOfAshDeepFreezeSystem();
            var sfSys = new SumpFloodingSystem(new SeededRng(1986), sfWeather, sfPower, sfDeepFreeze, new GodotLog());
            sfSys.RestoreState(sfState);
            _sumpFlooding = new SumpFloodingHostSession(sfSys, sfWeather, sfPower, sfDeepFreeze);
            _sumpFlooding.StateChanged += () => _sumpFlooding.MarkDirty();
            _sumpFloodingPanel = new SumpFloodingPanel();
            _sumpFloodingPanel.Bind(_sumpFlooding);
            _sumpFloodingPanel.Visible = false;
            AddChild(_sumpFloodingPanel);
        }

        private void SetupDecontamination()
        {
            var dcState = DecontaminationSaveStore.TryLoad() ?? new DecontaminationState();
            var dcInv = _inventory.Inventory;
            var dcRad = _survivors.Radiation;
            var dcAirlock = _airlockSecurity.System;
            var dcStarting = _startingLevel.System;
            var dcSys = new DecontaminationSystem(new SeededRng(1986), dcRad, dcInv, dcAirlock, dcStarting, new GodotLog());
            dcSys.RestoreState(dcState);
            _decontamination = new DecontaminationHostSession(dcSys, dcRad, dcInv, dcAirlock, dcStarting);
            _decontamination.StateChanged += () => _decontamination.MarkDirty();
            _decontaminationPanel = new DecontaminationPanel();
            _decontaminationPanel.Bind(_decontamination);
            _decontaminationPanel.Visible = false;
            AddChild(_decontaminationPanel);
        }

        private void SetupKitchenNutrition()
        {
            var knState = KitchenNutritionSaveStore.TryLoad() ?? new KitchenNutritionState();
            var knInv = _inventory.Inventory;
            var knNeeds = _survivors.Needs;
            var knSys = new KitchenNutritionSystem(new SeededRng(1986), knInv, knNeeds, new GodotLog());
            knSys.RestoreState(knState);
            _kitchenNutrition = new KitchenNutritionHostSession(knSys, knInv, knNeeds);
            _kitchenNutrition.StateChanged += () => _kitchenNutrition.MarkDirty();
            _kitchenNutritionPanel = new KitchenNutritionPanel();
            _kitchenNutritionPanel.Bind(_kitchenNutrition);
            _kitchenNutritionPanel.Visible = false;
            AddChild(_kitchenNutritionPanel);
        }

        private void SetupEquipmentCondition()
        {
            var ecState = EquipmentConditionSaveStore.TryLoad() ?? new EquipmentConditionState();
            var ecInv = _inventory.Inventory;
            var ecCrafting = _crafting.Engine;
            var ecSys = new EquipmentConditionSystem(new SeededRng(1986), ecInv, ecCrafting, new GodotLog());
            ecSys.RestoreState(ecState);
            _equipmentCondition = new EquipmentConditionHostSession(ecSys, ecInv, ecCrafting);
            _equipmentCondition.StateChanged += () => _equipmentCondition.MarkDirty();
            _equipmentConditionPanel = new EquipmentConditionPanel();
            _equipmentConditionPanel.Bind(_equipmentCondition);
            _equipmentConditionPanel.Visible = false;
            AddChild(_equipmentConditionPanel);
        }

        private void SetupLibraryStudy(ResearchSystem sharedResearch)
        {
            var lsState = LibraryStudySaveStore.TryLoad() ?? new LibraryStudyState();
            var lsSkills = new SkillProgressionSystem();
            var lsResearch = sharedResearch;
            var lsJournal = _journal;
            var lsSys = new LibraryStudySystem(lsSkills, lsResearch, lsJournal, _expandedShelterRoster, new GodotLog());
            lsSys.RestoreState(lsState);
            _libraryStudy = new LibraryStudyHostSession(lsSys, lsSkills, lsResearch, lsJournal, _expandedShelterRoster);
            _libraryStudy.LoadCatalog(_dataDir);
            _libraryStudy.StateChanged += () => _libraryStudy.MarkDirty();
            _libraryStudyPanel = new LibraryStudyPanel();
            _libraryStudyPanel.Bind(_libraryStudy);
            _libraryStudyPanel.Visible = false;
            AddChild(_libraryStudyPanel);
        }

        private void SetupArchiveDesk()
        {
            var adState = ArchiveDeskSaveStore.TryLoad() ?? new ArchiveDeskState();
            var adJournal = _journal;
            var adKnowledge = new KnowledgeBase();
            var adInv = _inventory.Inventory;
            var adSys = new ArchiveDeskSystem(adJournal, adKnowledge, adInv, _expandedShelterRoster, new GodotLog());
            adSys.RestoreState(adState);
            _archiveDesk = new ArchiveDeskHostSession(adSys, adJournal, adKnowledge, adInv, _expandedShelterRoster);
            _archiveDesk.LoadInkCatalog(_dataDir);
            _archiveDesk.StateChanged += () => _archiveDesk.MarkDirty();
            _archiveDeskPanel = new ArchiveDeskPanel();
            _archiveDeskPanel.Bind(_archiveDesk);
            _archiveDeskPanel.Visible = false;
            AddChild(_archiveDeskPanel);
        }

        private void SetupContractorRoster()
        {
            var crState = ContractorRosterSaveStore.TryLoad() ?? new ContractorRosterState();
            var crInv = _inventory.Inventory;
            var crExpedition = _expeditions.Engine;
            var crSys = new ContractorRosterSystem(new SeededRng(1986), crInv, _expandedShelterRoster, crExpedition, new GodotLog());
            crSys.RestoreState(crState);
            _contractorRoster = new ContractorRosterHostSession(crSys, crInv, _expandedShelterRoster, crExpedition);
            _contractorRoster.StateChanged += () => _contractorRoster.MarkDirty();
            _contractorRosterPanel = new ContractorRosterPanel();
            _contractorRosterPanel.Bind(_contractorRoster);
            _contractorRosterPanel.Visible = false;
            AddChild(_contractorRosterPanel);
        }

        private void SetupMentalHealthCrisis()
        {
            var mhState = MentalHealthCrisisSaveStore.TryLoad() ?? new MentalHealthState();
            var mhNeeds = _survivors.Needs;
            var mhMedical = _medicalWard;
            _chemicalDependency = new ChemicalDependencyHostSession();
            var mhSys = new MentalHealthCrisisSystem(new SeededRng(1986), mhNeeds, mhMedical, _chemicalDependency.System, _expandedShelterRoster, new GodotLog());
            mhSys.RestoreState(mhState);
            _mentalHealthCrisis = new MentalHealthCrisisHostSession(mhSys, mhNeeds, mhMedical, _chemicalDependency.System, _expandedShelterRoster);
            _mentalHealthCrisis.StateChanged += () => _mentalHealthCrisis.MarkDirty();
            _mentalHealthCrisisPanel = new MentalHealthCrisisPanel();
            _mentalHealthCrisisPanel.Bind(_mentalHealthCrisis);
            _mentalHealthCrisisPanel.Visible = false;
            AddChild(_mentalHealthCrisisPanel);

            _chemicalDependency.StateChanged += () => _chemicalDependency.MarkDirty();
            var depLoaded = ChemicalDependencySaveStore.TryLoad();
            if (depLoaded != null) _chemicalDependency.RestoreSave(depLoaded);
            _chemicalDependencyPanel = new ChemicalDependencyPanel();
            _chemicalDependencyPanel.Bind(_chemicalDependency);
            _chemicalDependencyPanel.Visible = false;
            AddChild(_chemicalDependencyPanel);

            if (_phantomMemory == null) SetupPhantom();
            _phantomMemoryPanel = new PhantomMemoryPanel();
            if (_phantomMemory != null) _phantomMemoryPanel.Bind(_phantomMemory);
            _phantomMemoryPanel.Visible = false;
            AddChild(_phantomMemoryPanel);

            _travelingCaravan = TravelingCaravanHostSession.Create(_dataDir);
            _travelingCaravanPanel = new TravelingCaravanPanel();
            _travelingCaravanPanel.Bind(_travelingCaravan);
            _travelingCaravanPanel.Visible = false;
            AddChild(_travelingCaravanPanel);
        }

        private void SetupShelterAssignment()
        {
            _shelterAssignment = ShelterAssignmentHostSession.CreateDefault(new SeededRng(1986));
            if (!_shelterAssignment.TryLoad())
            {
            }
            _shelterThermal.SetAssignments(_shelterAssignment.System);
            SetupPhase0();
            _phase0.BindShelterAssignment(_shelterAssignment.System);
        }

        private void SaveSumpFlooding() => _sumpFlooding?.Save();
        private void SaveDecontamination() => _decontamination?.Save();
        private void SaveKitchenNutrition() => _kitchenNutrition?.Save();
        private void SaveEquipmentCondition() => _equipmentCondition?.Save();
        private void SaveLibraryStudy() => _libraryStudy?.Save();
        private void SaveArchiveDesk() => _archiveDesk?.Save();
        private void SaveContractorRoster() => _contractorRoster?.Save();
        private void SaveMentalHealthCrisis() => _mentalHealthCrisis?.Save();
        private void SaveChemicalDependency() => _chemicalDependency?.Save();
        private void SaveShelterAssignment()
        {
            if (_shelterAssignment != null) _shelterAssignment.TrySave();
        }
    }
}
