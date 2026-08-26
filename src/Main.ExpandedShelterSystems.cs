using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Disease;
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


        // ── Dirty Flags ──



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
            SetupCaregiving();
            SetupShelterThermal();
            SetupShelterSchedule();
            SetupAutopsy(sharedResearch);
            SetupWaystation();
            SetupSumpFlooding();
            WireWaterTreatmentSumpBridge();
            WireWildlifeDiseaseBridge();
            SetupDecontamination();
            SetupKitchenNutrition();
            SetupEquipmentCondition();
            SetupLibraryStudy(sharedResearch);
            SetupArchiveDesk();
            SetupContractorRoster();
            SetupMentalHealthCrisis();
            SetupShelterAssignment();   // last — post-wiring to Thermal + Phase0
        }

        private void WireWaterTreatmentSumpBridge()
        {
            if (_sumpFlooding == null || _waterTreatment == null) return;
            _sumpFlooding.System.OnIncident += incident =>
            {
                if (incident.kind == FloodIncidentKind.FloodStart || incident.kind == FloodIncidentKind.Contamination)
                {
                    _waterTreatment.SetIncomingContamination(0.8f);
                }
            };
        }

        private void WireWildlifeDiseaseBridge()
        {
            if (_wildlifeTrapping == null || _disease == null) return;
            _wildlifeTrapping.System.OnButcheryCompleted += (siteId, butcherId, species, isToxic) =>
            {
                if (string.IsNullOrEmpty(butcherId)) return;
                // Sterile technique trait — placeholder deterministic check: butcherId containing "sterile" has the trait
                // Real check would query SurvivorCatalog/Roster trait, but host keeps it simple and deterministic
                bool hasSterile = butcherId.IndexOf("sterile", StringComparison.OrdinalIgnoreCase) >= 0;
                if (hasSterile) return;
                int seed = StableHash.Of(butcherId) ^ _simDay;
                var rng = new SeededRng(seed);
                if (rng.NextDouble() < 0.30)
                {
                    _disease.Engine.Infect(butcherId, DiseaseIds.ZoonoticFlu, _simDay);
                }
            };
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
            SaveCaregiving();
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
            _caregiving?.TickDay(day);
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
                case "caregiving":
                    if (_caregivingPanel != null) { _caregivingPanel.Visible = true; _caregivingPanel.RefreshView(); }
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
