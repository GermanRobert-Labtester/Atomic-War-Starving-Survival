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
        private ShelterDecorHostSession _shelterDecor = null!;
        private ShelterDecorPanel _shelterDecorPanel = null!;

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
            if (_sumpFlooding != null) return;
            SetupCampaignDay();
            var sfState = SumpFloodingSaveStore.TryLoad() ?? new SumpFloodingState();
            var sfWeather = _world.Weather;
            var sfPower = _powerGrid.System;
            var sfDeepFreeze = new YearOfAshDeepFreezeSystem();
            var sfSys = new SumpFloodingSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 10), sfWeather, sfPower, sfDeepFreeze, new GodotLog());
            // Plan 70: parameterize drainage ingress/silt/pump-load from the
            // sump_drainage_catalog.json authority (strata stay optional —
            // nodes without a bound stratum keep the legacy inflow model).
            var sumpStrata = Ashfall.Core.SumpDrainageCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            sfSys.ApplyStratumCatalog(sumpStrata);
            sfSys.RestoreState(sfState);
            _sumpFlooding = new SumpFloodingHostSession(sfSys, sfWeather, sfPower, sfDeepFreeze);
            if (_sumpFloodingPanel != null && _sumpFloodingPanel.IsInsideTree())
                RemoveChild(_sumpFloodingPanel);
            _sumpFloodingPanel = new SumpFloodingPanel();
            _sumpFloodingPanel.Bind(_sumpFlooding);
            _sumpFloodingPanel.Visible = false;
            AddChild(_sumpFloodingPanel);
            // Plan 70: bind the sludge-plant console to the same session
            // (replaces the hardcoded stub created in the bulk panel pass).
            if (_slurryDewateringSumpPanel != null && _slurryDewateringSumpPanel.IsInsideTree())
                RemoveChild(_slurryDewateringSumpPanel);
            _slurryDewateringSumpPanel = new SlurryDewateringSumpPanel();
            _slurryDewateringSumpPanel.Bind(_sumpFlooding);
            _slurryDewateringSumpPanel.Visible = false;
            AddChild(_slurryDewateringSumpPanel);
        }

        private void SetupDecontamination()
        {
            if (_decontamination != null) return;
            SetupCampaignDay();
            var dcState = DecontaminationSaveStore.TryLoad() ?? new DecontaminationState();
            var dcInv = _inventory.Inventory;
            var dcRad = _survivors.Radiation;
            var dcAirlock = _airlockSecurity.System;
            var dcStarting = _startingLevel.System;
            var dcSys = new DecontaminationSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 11), dcRad, dcInv, dcAirlock, dcStarting, new GodotLog());
            dcSys.RestoreState(dcState);
            _decontamination = new DecontaminationHostSession(dcSys, dcRad, dcInv, dcAirlock, dcStarting);
            if (_decontaminationPanel != null && _decontaminationPanel.IsInsideTree())
                RemoveChild(_decontaminationPanel);
            _decontaminationPanel = new DecontaminationPanel();
            _decontaminationPanel.Bind(_decontamination);
            _decontaminationPanel.Visible = false;
            AddChild(_decontaminationPanel);
        }

        private void SetupKitchenNutrition()
        {
            if (_kitchenNutrition != null) return;
            SetupCampaignDay();
            var knState = KitchenNutritionSaveStore.TryLoad() ?? new KitchenNutritionState();
            var knInv = _inventory.Inventory;
            var knNeeds = _survivors.Needs;
            var knSys = new KitchenNutritionSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 12), knInv, knNeeds, new GodotLog());
            knSys.RestoreState(knState);
            _kitchenNutrition = new KitchenNutritionHostSession(knSys, knInv, knNeeds);
            if (_kitchenNutritionPanel != null && _kitchenNutritionPanel.IsInsideTree())
                RemoveChild(_kitchenNutritionPanel);
            _kitchenNutritionPanel = new KitchenNutritionPanel();
            _kitchenNutritionPanel.Bind(_kitchenNutrition);
            _kitchenNutritionPanel.Visible = false;
            AddChild(_kitchenNutritionPanel);
        }

        private void SetupEquipmentCondition()
        {
            if (_equipmentCondition != null) return;
            SetupCampaignDay();
            var ecState = EquipmentConditionSaveStore.TryLoad() ?? new EquipmentConditionState();
            var ecInv = _inventory.Inventory;
            var ecCrafting = _crafting.Engine;
            var ecSys = new EquipmentConditionSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 13), ecInv, ecCrafting, new GodotLog());
            ecSys.RestoreState(ecState);
            _equipmentCondition = new EquipmentConditionHostSession(ecSys, ecInv, ecCrafting);
            // Combat projects its default weapon loadout from this authority
            // and writes engagement wear back here (WeaponEquipmentBridge).
            if (_combat != null)
                _combat.Equipment = ecSys;
            if (_equipmentConditionPanel != null && _equipmentConditionPanel.IsInsideTree())
                RemoveChild(_equipmentConditionPanel);
            _equipmentConditionPanel = new EquipmentConditionPanel();
            _equipmentConditionPanel.Bind(_equipmentCondition);
            _equipmentConditionPanel.Visible = false;
            AddChild(_equipmentConditionPanel);
        }

        private void SetupLibraryStudy(ResearchSystem sharedResearch)
        {
            if (_libraryStudy != null) return;
            var lsState = LibraryStudySaveStore.TryLoad() ?? new LibraryStudyState();
            var lsSkills = EnsureSharedSkillProgression();
            var lsResearch = sharedResearch;
            var lsJournal = _journal;
            var lsSys = new LibraryStudySystem(lsSkills, lsResearch, lsJournal, _expandedShelterRoster, new GodotLog());
            lsSys.RestoreState(lsState);
            _libraryStudy = new LibraryStudyHostSession(lsSys, lsSkills, lsResearch, lsJournal, _expandedShelterRoster);
            _libraryStudy.LoadCatalog(_dataDir);
            if (_libraryStudyPanel != null && _libraryStudyPanel.IsInsideTree())
                RemoveChild(_libraryStudyPanel);
            _libraryStudyPanel = new LibraryStudyPanel();
            _libraryStudyPanel.Bind(_libraryStudy);
            _libraryStudyPanel.Visible = false;
            AddChild(_libraryStudyPanel);
        }

        private void SetupArchiveDesk()
        {
            if (_archiveDesk != null) return;
            var adState = ArchiveDeskSaveStore.TryLoad() ?? new ArchiveDeskState();
            var adJournal = _journal;
            var adKnowledge = new KnowledgeBase();
            var adInv = _inventory.Inventory;
            var adSys = new ArchiveDeskSystem(adJournal, adKnowledge, adInv, _expandedShelterRoster, new GodotLog());
            adSys.RestoreState(adState);
            _archiveDesk = new ArchiveDeskHostSession(adSys, adJournal, adKnowledge, adInv, _expandedShelterRoster);
            _archiveDesk.LoadInkCatalog(_dataDir);
            if (_archiveDeskPanel != null && _archiveDeskPanel.IsInsideTree())
                RemoveChild(_archiveDeskPanel);
            _archiveDeskPanel = new ArchiveDeskPanel();
            _archiveDeskPanel.Bind(_archiveDesk);
            _archiveDeskPanel.Visible = false;
            AddChild(_archiveDeskPanel);
        }

        private void SetupContractorRoster()
        {
            if (_contractorRoster != null) return;
            SetupCampaignDay();
            var crState = ContractorRosterSaveStore.TryLoad() ?? new ContractorRosterState();
            var crInv = _inventory.Inventory;
            var crExpedition = _expeditions.Engine;
            var crSys = new ContractorRosterSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 14), crInv, _expandedShelterRoster, crExpedition, new GodotLog());
            crSys.RestoreState(crState);
            _contractorRoster = new ContractorRosterHostSession(crSys, crInv, _expandedShelterRoster, crExpedition);
            if (_contractorRosterPanel != null && _contractorRosterPanel.IsInsideTree())
                RemoveChild(_contractorRosterPanel);
            _contractorRosterPanel = new ContractorRosterPanel();
            _contractorRosterPanel.Bind(_contractorRoster);
            _contractorRosterPanel.Visible = false;
            AddChild(_contractorRosterPanel);
        }

        private void SetupMentalHealthCrisis()
        {
            if (_mentalHealthCrisis != null) return;
            SetupCampaignDay();
            var mhState = MentalHealthCrisisSaveStore.TryLoad() ?? new MentalHealthState();
            var mhNeeds = _survivors.Needs;
            var mhMedical = _medicalWard;
            // Task #133: one shared chem-dep authority — the MedicalHostSession
            // engine (already loaded + merged from both save sections). The
            // chemical_dependency save section is written from this same shared
            // engine by SaveChemicalDependency, keeping it in sync.
            SetupMedical();
            _chemicalDependency = new ChemicalDependencyHostSession(_medical.Engine);
            // Task #133 P1b: share the pipeline when already bound; otherwise
            // EnsureMedicalPipeline backfills this reference once it runs.
            _chemicalDependency.Pipeline = _medical.Pipeline;
            var mhSys = new MentalHealthCrisisSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Psychology, 0, 15), mhNeeds, mhMedical, _chemicalDependency.System, _expandedShelterRoster, new GodotLog());
            mhSys.RestoreState(mhState);
            _mentalHealthCrisis = new MentalHealthCrisisHostSession(mhSys, mhNeeds, mhMedical, _chemicalDependency.System, _expandedShelterRoster);
            if (_mentalHealthCrisisPanel != null && _mentalHealthCrisisPanel.IsInsideTree())
                RemoveChild(_mentalHealthCrisisPanel);
            _mentalHealthCrisisPanel = new MentalHealthCrisisPanel();
            _mentalHealthCrisisPanel.Bind(_mentalHealthCrisis);
            _mentalHealthCrisisPanel.Visible = false;
            AddChild(_mentalHealthCrisisPanel);

            // Task #133: no separate restore here — the shared engine was already
            // loaded and merged by MedicalHostSession.Create. Re-restoring the
            // legacy section would wipe canonical medical-ledger rows.
            if (_chemicalDependencyPanel != null && _chemicalDependencyPanel.IsInsideTree())
                RemoveChild(_chemicalDependencyPanel);
            _chemicalDependencyPanel = new ChemicalDependencyPanel();
            _chemicalDependencyPanel.Bind(_chemicalDependency);
            _chemicalDependencyPanel.Visible = false;
            AddChild(_chemicalDependencyPanel);

            if (_phantomMemory == null) SetupPhantom();
            if (_phantomMemoryPanel != null && _phantomMemoryPanel.IsInsideTree())
                RemoveChild(_phantomMemoryPanel);
            _phantomMemoryPanel = new PhantomMemoryPanel();
            if (_phantomMemory != null) _phantomMemoryPanel.Bind(_phantomMemory, _inventory);
            _phantomMemoryPanel.Visible = false;
            AddChild(_phantomMemoryPanel);

            _travelingCaravan = TravelingCaravanHostSession.Create(_dataDir);
            if (_travelingCaravanPanel != null && _travelingCaravanPanel.IsInsideTree())
                RemoveChild(_travelingCaravanPanel);
            _travelingCaravanPanel = new TravelingCaravanPanel();
            _travelingCaravanPanel.Bind(_travelingCaravan);
            _travelingCaravanPanel.Visible = false;
            AddChild(_travelingCaravanPanel);
        }

        private void SetupShelterAssignment()
        {
            if (_shelterAssignment != null) return;
            SetupCampaignDay();
            _shelterAssignment = ShelterAssignmentHostSession.CreateDefault(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng);
            if (!_shelterAssignment.TryLoad())
            {
            }
            _shelterThermal.SetAssignments(_shelterAssignment.System);
            SetupPhase0();
            _phase0.BindShelterAssignment(_shelterAssignment.System);
        }

        /// <summary>
        /// Plan 12C final host triad. Placements restore from the campaign
        /// envelope; item modifiers are rebuilt from the authoritative live
        /// item catalog, never copied into a save. Existing memorial entries
        /// are reconciled once so saves authored before this panel gain their
        /// wall records deterministically.
        /// </summary>
        private void SetupShelterDecor()
        {
            if (_shelterDecor != null) return;

            SetupSurvivors();
            SetupInventory();
            SetupShelterAssignment();
            SetupMemorial();

            var system = new ShelterDecorSystem();
            var saved = ShelterDecorSaveStore.TryLoad();
            if (saved != null)
                system.RestoreState(saved.Capture());

            _shelterDecor = new ShelterDecorHostSession(
                system,
                _shelterAssignment.System,
                _survivors.Needs,
                _inventory);
            _shelterDecor.SetCurrentDay(_simDay);
            _shelterDecor.LoadCatalogModifiers();

            // This is intentionally idempotent: only legacy memorial entries
            // without a plaque become new placements, and a duplicate survivor
            // key is never mounted twice.
            foreach (var entry in _memorial.Entries)
            {
                if (!_shelterDecor.TryMountMemorialPlaque(entry, out var reason))
                    GD.PushWarning("[Ashfall Godot] Memorial plaque reconcile skipped: " + reason);
            }

            if (_shelterDecorPanel != null && _shelterDecorPanel.IsInsideTree())
                RemoveChild(_shelterDecorPanel);
            _shelterDecorPanel = new ShelterDecorPanel();
            _shelterDecorPanel.Bind(_shelterDecor);
            _shelterDecorPanel.Visible = false;
            AddChild(_shelterDecorPanel);
        }

        private void SaveSumpFlooding()
        {
            if (_sumpFlooding != null)
                CaptureSection("sump_flooding", SumpFloodingSaveStore.TryCapturePersisted(_sumpFlooding.System.CaptureState()));
        }
        private void SaveDecontamination()
        {
            if (_decontamination != null)
                CaptureSection("decontamination", DecontaminationSaveStore.TryCapturePersisted(_decontamination.System.CaptureState()));
        }
        private void SaveKitchenNutrition()
        {
            if (_kitchenNutrition != null)
                CaptureSection("kitchen_nutrition", KitchenNutritionSaveStore.TryCapturePersisted(_kitchenNutrition.System.CaptureState()));
        }
        private void SaveEquipmentCondition()
        {
            if (_equipmentCondition != null)
                CaptureSection("equipment_condition", EquipmentConditionSaveStore.TryCapturePersisted(_equipmentCondition.System.CaptureState()));
        }
        private void SaveLibraryStudy()
        {
            if (_libraryStudy != null)
                CaptureSection("library_study", LibraryStudySaveStore.TryCapturePersisted(_libraryStudy.System.CaptureState()));
        }
        private void SaveArchiveDesk()
        {
            if (_archiveDesk != null)
                CaptureSection("archive_desk", ArchiveDeskSaveStore.TryCapturePersisted(_archiveDesk.System.CaptureState()));
        }
        private void SaveContractorRoster()
        {
            if (_contractorRoster != null)
                CaptureSection("contractor_roster", ContractorRosterSaveStore.TryCapturePersisted(_contractorRoster.System.CaptureState()));
        }
        private void SaveMentalHealthCrisis()
        {
            if (_mentalHealthCrisis != null)
                CaptureSection("mental_health_crisis", MentalHealthCrisisSaveStore.TryCapturePersisted(_mentalHealthCrisis.System.CaptureState()));
        }
        private void SaveChemicalDependency()
        {
            if (_chemicalDependency != null)
                CaptureSection("chemical_dependency", ChemicalDependencySaveStore.TryCapturePersisted(_chemicalDependency.System.CaptureState()));
        }
        private void SaveShelterAssignment()
        {
            if (_shelterAssignment == null) return;

            var save = new ShelterAssignmentSave
            {
                simDay = 0,
                Rooms = new List<ShelterRoomSave>(),
                State = _shelterAssignment.System.CaptureState()
            };
            foreach (var room in _shelterAssignment.System.Rooms)
            {
                save.Rooms.Add(new ShelterRoomSave
                {
                    RoomId = room.RoomId,
                    DisplayName = room.DisplayName,
                    Capacity = room.Capacity,
                    RequiredSkillId = room.RequiredSkillId,
                    WorkstationId = room.WorkstationId
                });
            }

            if (CaptureSection("shelter_assignment", ShelterAssignmentSaveStore.TryCapturePersisted(save)))
                _shelterAssignment.ClearDirty();
        }

        private void SaveShelterDecor()
        {
            if (_shelterDecor == null) return;
            if (CaptureSection("shelter_decor", ShelterDecorSaveStore.TryCapturePersisted(_shelterDecor.System.State)))
                _shelterDecor.ClearDirty();
        }
    }
}
