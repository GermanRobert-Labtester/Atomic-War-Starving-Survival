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

        // Single shared ResearchSystem consulted by autopsy + library study
        // (previously a local var in SetupExpandedShelterSystems, which made it
        // unreachable for the Research panel bind). Created fresh by
        // SetupExpandedShelterSystems; lazily by the research panel route.
        private ResearchSystem _sharedResearch = null!;



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

            _sharedResearch = new ResearchSystem(log: new GodotLog());

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
            SetupAutopsy(_sharedResearch);
            SetupWaystation();
            SetupSumpFlooding();
            WireWaterTreatmentSumpBridge();
            WireWildlifeDiseaseBridge();
            WireVinylRadioBridge();
            WireAutopsyBridge();
            SetupDecontamination();
            SetupKitchenNutrition();
            SetupEquipmentCondition();
            SetupLibraryStudy(_sharedResearch);
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
                var def = _survivors?.Roster?.FindDefinition(butcherId);
                if (def != null && def.traitIds != null && def.traitIds.Contains("skill_sanitization_expert"))
                    return;
                int seed = StableHash.Of(butcherId) ^ _simDay;
                var rng = new SeededRng(seed);
                if (rng.NextDouble() < 0.30)
                {
                    _disease.Engine.Infect(butcherId, DiseaseIds.ZoonoticFlu, _simDay);
                }
            };
        }

        private void WireVinylRadioBridge()
        {
            if (_vinylMorale == null || _radio == null || _powerGrid == null) return;
            _vinylMorale.System.OnCulturalBroadcast += (record, day) =>
            {
                // 150W transmitter load — if brownout, cancel broadcast and cut signal
                if (_powerGrid.System.IsBrownout)
                {
                    _vinylMorale.System.Stop();
                    return;
                }
                _radio.RecordCulturalBroadcast(record.record_id, record.genre, record.display_name, day, _vinylMorale.System.State.lastBroadcastSignalStrength);
            };
        }

        private void WireAutopsyBridge()
        {
            if (_autopsy == null) return;
            _autopsy.System.OnCaseCompleted += c =>
            {
                string finding = c.finding ?? string.Empty;
                // Future-proof: keyword-based forensic routing — add new findings without changing host wiring structure
                if (finding.IndexOf("zoonotic", StringComparison.OrdinalIgnoreCase) >= 0 || finding.IndexOf("influenza", StringComparison.OrdinalIgnoreCase) >= 0 || finding.IndexOf("spore", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (_disease != null && !string.IsNullOrEmpty(c.assignedMedicId))
                        _disease.Engine.Infect(c.assignedMedicId, DiseaseIds.ZoonoticFlu, _simDay);
                }
                // Always journal the forensic result for memorial/continuity
                _journal?.TryAddRawEntry("autopsy_completed", $"Autopsy {c.caseId} ({c.specimenId}): {finding}", null!, _simDay);
                // Memorialize if system available — use Memorialize with minimal input
                if (_memorial != null)
                {
                    try
                    {
                        _memorial.Memorialize(new Ashfall.Core.Memorial.MemorialInput
                        {
                            SurvivorId = c.specimenId,
                            Cause = finding,
                            Day = _simDay,
                            BirthDay = 0,
                            Epitaph = $"Forensic finding: {finding}"
                        });
                    }
                    catch (Exception ex)
                    {
                        // Memorial integration is optional; log warning without blocking autopsy flow.
                        GD.PushWarning($"[Ashfall Godot] Autopsy memorialization failed for {c.specimenId}: {ex.Message}");
                    }
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

        private void ResetExpandedShelterSessions()
        {
            // Remove instantiated panels from scene tree
            void RemovePanel(Control? panel)
            {
                if (panel != null && panel.IsInsideTree())
                    RemoveChild(panel);
            }

            RemovePanel(_waterTreatmentPanel); _waterTreatmentPanel = null!;
            RemovePanel(_airlockSecurityPanel); _airlockSecurityPanel = null!;
            RemovePanel(_shelterThermalPanel); _shelterThermalPanel = null!;
            RemovePanel(_shelterSchedulePanel); _shelterSchedulePanel = null!;
            RemovePanel(_autopsyReportPanel); _autopsyReportPanel = null!;
            RemovePanel(_waystationPanel); _waystationPanel = null!;
            RemovePanel(_survivorRelationsPanel); _survivorRelationsPanel = null!;
            RemovePanel(_regionalTreatyPanel); _regionalTreatyPanel = null!;
            RemovePanel(_vinylMoralePanel); _vinylMoralePanel = null!;
            RemovePanel(_wildlifeTrappingPanel); _wildlifeTrappingPanel = null!;
            RemovePanel(_excavationPanel); _excavationPanel = null!;
            RemovePanel(_apprenticeshipPanel); _apprenticeshipPanel = null!;
            RemovePanel(_caregivingPanel); _caregivingPanel = null!;
            RemovePanel(_sumpFloodingPanel); _sumpFloodingPanel = null!;
            RemovePanel(_decontaminationPanel); _decontaminationPanel = null!;
            RemovePanel(_kitchenNutritionPanel); _kitchenNutritionPanel = null!;
            RemovePanel(_equipmentConditionPanel); _equipmentConditionPanel = null!;
            RemovePanel(_libraryStudyPanel); _libraryStudyPanel = null!;
            RemovePanel(_archiveDeskPanel); _archiveDeskPanel = null!;
            RemovePanel(_contractorRosterPanel); _contractorRosterPanel = null!;
            RemovePanel(_mentalHealthCrisisPanel); _mentalHealthCrisisPanel = null!;
            RemovePanel(_chemicalDependencyPanel); _chemicalDependencyPanel = null!;
            RemovePanel(_phantomMemoryPanel); _phantomMemoryPanel = null!;
            RemovePanel(_travelingCaravanPanel); _travelingCaravanPanel = null!;

            // Dispose / null host sessions
            _waterTreatment?.Dispose(); _waterTreatment = null!;
            _airlockSecurity?.Dispose(); _airlockSecurity = null!;
            _shelterThermal?.Dispose(); _shelterThermal = null!;
            _shelterSchedule?.Dispose(); _shelterSchedule = null!;
            _autopsy?.Dispose(); _autopsy = null!;
            _waystation?.Dispose(); _waystation = null!;
            _survivorRelations?.Dispose(); _survivorRelations = null!;
            _survivorRelationsCore = null!;
            _regionalTreaty?.Dispose(); _regionalTreaty = null!;
            _vinylMorale?.Dispose(); _vinylMorale = null!;
            _wildlifeTrapping?.Dispose(); _wildlifeTrapping = null!;
            _excavation?.Dispose(); _excavation = null!;
            _apprenticeship?.Dispose(); _apprenticeship = null!;
            _caregiving?.Dispose(); _caregiving = null!;
            _sumpFlooding?.Dispose(); _sumpFlooding = null!;
            _decontamination?.Dispose(); _decontamination = null!;
            _kitchenNutrition?.Dispose(); _kitchenNutrition = null!;
            _equipmentCondition?.Dispose(); _equipmentCondition = null!;
            _libraryStudy?.Dispose(); _libraryStudy = null!;
            _archiveDesk?.Dispose(); _archiveDesk = null!;
            _contractorRoster?.Dispose(); _contractorRoster = null!;
            _mentalHealthCrisis?.Dispose(); _mentalHealthCrisis = null!;
            _chemicalDependency?.Dispose(); _chemicalDependency = null!;
            _shelterAssignment?.Dispose(); _shelterAssignment = null!;
            _travelingCaravan?.Dispose(); _travelingCaravan = null!;

            _airlockSecurityDirty = false;
            _shelterThermalDirty = false;
            _shelterScheduleDirty = false;
            _autopsyDirty = false;
            _waystationDirty = false;
            _survivorRelationsDirty = false;
            _regionalTreatyDirty = false;
            _vinylMoraleDirty = false;
            _wildlifeTrappingDirty = false;
            _excavationDirty = false;
            _apprenticeshipDirty = false;
            _caregivingDirty = false;
            _sumpFloodingDirty = false;
            _decontaminationDirty = false;
            _kitchenNutritionDirty = false;
            _equipmentConditionDirty = false;
            _libraryStudyDirty = false;
            _archiveDeskDirty = false;
            _contractorRosterDirty = false;
            _mentalHealthCrisisDirty = false;

            // Delete slot and global save files for expanded shelter systems
            // (section file names come from the single registry authority)
            foreach (var file in Ashfall.Core.Save.SaveSectionRegistry.SectionFileNames.Values)
            {
                string p = SaveSlotRoot.Resolve(file);
                if (System.IO.File.Exists(p))
                    System.IO.File.Delete(p);
                string globalP = System.IO.Path.Combine(ProjectSettings.GlobalizePath("user://"), file);
                if (System.IO.File.Exists(globalP))
                    System.IO.File.Delete(globalP);
            }
        }
    }
}
