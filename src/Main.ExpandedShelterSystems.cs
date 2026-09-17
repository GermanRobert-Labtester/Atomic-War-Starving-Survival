// SPDX-License-Identifier: MIT
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

        /// <summary>
        /// Lazily create the shared research engine, restore persisted research
        /// progress, and load the authoritative research_knowledge.json catalog
        /// (Plan 34: JSON is the sole authored research authority). All research
        /// consumers — crafting/workshop, autopsy, library study, research and
        /// atlas panels — must obtain the engine through this method so the
        /// campaign has exactly one research instance.
        /// </summary>
        private ResearchSystem EnsureSharedResearch()
        {
            if (_sharedResearch != null) return _sharedResearch;
            var saved = ResearchSaveStore.TryLoad();
            var engine = new ResearchSystem(log: new GodotLog(), state: saved);
            if (!string.IsNullOrEmpty(_dataDir))
            {
                var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
                int count = ResearchKnowledgeCatalogLoader.LoadAndRegister(engine, _dataDir, fileIO, new SystemTextJsonSerializer());
                if (count == 0)
                    GD.PushWarning($"[Ashfall Godot] research_knowledge.json loaded 0 nodes from {_dataDir} — research content unavailable");
            }
            engine.OnResearchCompleted += OnSharedResearchCompleted;
            _sharedResearch = engine;
            return engine;
        }

        private void OnSharedResearchCompleted(ResearchKnowledgeDef def)
        {
            if (def == null) return;
            SetupJournal();
            string bt = !string.IsNullOrEmpty(def.breakthroughItem) ? $" Breakthrough: {def.breakthroughItem}." : string.Empty;
            _journal?.TryAddRawEntry($"research_{def.id}", $"Research completed: {def.displayName}.{bt}", null!, _simDay);
            _journalDirty = true;
        }



        // Batch 4 BUG-14 follow-up: a SINGLE shared duty roster passed to
        // the eight systems above that consult the roster (apprenticeship,
        // library study, archive desk, contractor roster, mental health).
        // Previously each system held a fresh `new DutyRosterSystem()`, so
        // cross-system busy checks (mentor_busy / caregiver_busy) observed
        // an empty per-instance roster and never blocked.
        private DutyRosterSystem _expandedShelterRoster = new DutyRosterSystem();

        // Promoted from SetupExpandedShelterSystems local — Apprenticeship needs
        // the core SurvivorRelationsSystem instance for its constructor.
        private SurvivorRelationsSystem _survivorRelationsCore = null!;

        // ── 22 UI Panels ──


        // ── Dirty Flags ──



        private void SetupExpandedShelterSystems()
        {
            SetupSurvivors();
            SetupDutyRoster();
            // All shelter work consumers share the persisted campaign duty
            // authority; the former empty helper roster could disagree with
            // quarantine, care, fitness and the player-visible shift chart.
            _expandedShelterRoster = _dutyRoster.Roster;
            SetupInventory();
            SetupPowerGrid();
            SetupJournal();
            SetupCrafting();
            SetupExpeditions();
            SetupMedical();
            SetupMedicalWard();
            SetupStartingLevel();
            SetupWorld();

            _sharedResearch = EnsureSharedResearch();

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
            SetupWeatherHardening();
            SetupGeothermalAquifer();
            SetupShelterSchedule();
            SetupAutopsy(_sharedResearch);
            SetupWaystation();
            SetupSumpFlooding();
            WireWaterTreatmentSumpBridge();
            WireWildlifeDiseaseBridge();
            WireVinylRadioBridge();
            WireAutopsyBridge();
            SetupDecontamination();
            SetupPlans78To81();
            SetupPlans110To113();
            SetupPlans130To133();
            SetupPlans146To149();
            SetupKitchenNutrition();
            SetupGrainProcessing();
            SetupCryogenicAirSeparation();
            SetupHeliograph();
            SetupEquipmentCondition();
            SetupLibraryStudy(_sharedResearch);
            SetupArchiveDesk();
            SetupContractorRoster();
            SetupMentalHealthCrisis();
            SetupShelterAssignment();   // last — post-wiring to Thermal + Phase0
            SetupShelterDecor();        // uses the final assignment map + inventory catalog
        }

        /// <summary>Binds the Plan 72 electrostatic scrubber console to the ventilation session.</summary>
        private void BindElectrostaticScrubberPanel()
        {
            if (_ventilationHost == null) return;
            if (_electrostaticScrubberPanel == null)
            {
                _electrostaticScrubberPanel = new ElectrostaticScrubberPanel { Visible = false };
                _electrostaticScrubberPanel.OnClose += () => _electrostaticScrubberPanel.Visible = false;
                AddChild(_electrostaticScrubberPanel);
            }
            _electrostaticScrubberPanel.Bind(_ventilationHost);
            _electrostaticScrubberPanel.Visible = false;
        }

        private void WireWaterTreatmentSumpBridge()
        {
            if (_sumpFlooding == null || _waterTreatment == null) return;
            // Plan 70: bind the sludge-plant consumable inventory (flocculant /
            // filter cloth) and the canonical greywater routing target.
            _sumpFlooding.System.BindServices(_inventory?.Inventory, _waterTreatment.System, _ventilation);
            // Plan 72: bind stage services (seeded arc RNG, real power draw,
            // installation components, radioactive-drum inventory, arc-fire handoff).
            _ventilation?.BindStageServices(
                _campaignDay?.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 14),
                _powerGrid?.System,
                _inventory?.Inventory,
                _stageFireHazard);
            BindElectrostaticScrubberPanel();
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
            // Guard: WildlifeTrappingHostSession.ApplyDisease handles authoritative disease routing.
            // Only wire legacy fallback bridge if ApplyDisease delegate is not configured.
            if (_wildlifeTrapping.ApplyDisease != null) return;
            _wildlifeTrapping.System.OnButcheryCompleted += (siteId, butcherId, species, isToxic) =>
            {
                if (string.IsNullOrEmpty(butcherId)) return;
                var def = _survivors?.Roster?.FindDefinition(butcherId);
                if (def != null && def.traitIds != null && def.traitIds.Contains("skill_sanitization_expert"))
                    return;
                _disease.Engine.TryExpose(new DiseaseExposureContext
                {
                    SurvivorId = butcherId,
                    DiseaseId = DiseaseIds.ZoonoticFlu,
                    SourceId = "wildlife_butchery",
                    Day = _simDay,
                    ProbabilityModifier = 1.0f
                });
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
                    _vinylMorale.System.CancelBroadcastBrownout();
                    return;
                }
                // The vinyl relay is presentation-only: the Core event has
                // already resolved the morale/radio consequence. This cue
                // must never feed back into simulation state.
                _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVinylBroadcast);
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
                    {
                        _disease.Engine.TryExpose(new DiseaseExposureContext
                        {
                            SurvivorId = c.assignedMedicId,
                            DiseaseId = DiseaseIds.ZoonoticFlu,
                            SourceId = "autopsy_pathogen",
                            Day = _simDay,
                            ProbabilityModifier = 1.0f
                        });
                    }
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
            SaveWeatherHardening();
            SaveGeothermalAquifer();
            SaveShelterSchedule();
            SaveAutopsy();
            SaveWaystation();
            SaveSumpFlooding();
            SaveDecontamination();
            PersistPlans78To81();
            PersistPlans110To113();
            PersistPlans130To133();
            PersistPlans146To149();
            SaveKitchenNutrition();
            SaveGrainProcessing();
            SaveCryogenicAirSeparation();
            SaveHeliograph();
            SaveEquipmentCondition();
            SaveLibraryStudy();
            SaveResearch();
            SaveArchiveDesk();
            SaveContractorRoster();
            SaveMentalHealthCrisis();
            SaveChemicalDependency();
            SaveShelterAssignment();
            SaveShelterDecor();
            SaveFactionBranch();
        }

        /// <summary>Capture research progress into the campaign envelope (Plan 34: research state must round-trip).</summary>
        private void SaveResearch()
        {
            if (_sharedResearch != null)
                CaptureSection("research", ResearchSaveStore.TryCapturePersisted(_sharedResearch.CaptureState()));
        }













        /// <summary>
        /// Presentation lighting phase from the campaign hour. Drives the placeholder
        /// dawn/day/dusk/night backdrop variants on shelter, map, and expedition views.
        /// </summary>
        private static string LightingPhaseForHour(int hour)
        {
            int h = ((hour % 24) + 24) % 24;
            if (h >= 5 && h < 9) return "dawn";
            if (h >= 9 && h < 17) return "day";
            if (h >= 17 && h < 21) return "dusk";
            return "night";
        }

        private void TickAllExpandedShelterSystems(int day)
        {
            // Plan 189 intake bridge: piezometer advisory must land before the
            // water plant ticks so a blocked source refuses intake same-day.
            TickPiezometerAdvisoryBridge(day);

            // C2[6] 23A: water treatment spans two real loads. Extraction follows
            // room_water_pump; processing follows room_water_filtration (previously an
            // unread critical catalog row). Allocation-aware: a brownout that still
            // serves both buses keeps the plant at full throughput; one bus served is
            // partial; neither pauses treatment (the passive path remains).
            bool pumpServed = _powerGrid?.System == null
                || _powerGrid.System.IsRoomServed("room_water_pump");
            bool filtrationServed = _powerGrid?.System == null
                || _powerGrid.System.IsRoomServed("room_water_filtration");
            float waterPower = !pumpServed ? 0f : (filtrationServed ? 1f : 0.5f);
            _waterTreatment?.TickDay(day, waterPower);
            _airlockSecurity?.TickDay(day);
            _survivorRelations?.TickDay(day);
            _regionalTreaty?.TickDay(day);
            _vinylMorale?.TickDay(day);
            RefreshTrappingDensity();
            _wildlifeTrapping?.TickDay(day);
            _excavation?.TickDay();
            // DEBT-185: dormancy decay for the shared skill progression must run
            // on the campaign day owner; practice systems record against the
            // same instance and reactivate dormant skills.
            TickSharedSkillProgression(day);
            _apprenticeship?.TickDay(day);
            _caregiving?.TickDay(day);
            // C2[6] 23A: heating circulation is an electrical load. The generator's
            // waste heat only reaches the radiators while the circulation pump has
            // served room_heating power; no pump power = heat generated but not
            // delivered. Derived from the canonical base generator (external fuel-free
            // sources produce no combustion waste heat).
            if (_shelterThermal != null && _powerGrid?.System != null)
            {
                bool heatingPowered = _powerGrid.System.IsRoomServed("room_heating");
                float baseGeneratorKw = _powerGrid.System.BaseGenerationWatts
                    * _powerGrid.System.GeneratorOutputFactor / 1000f;
                _shelterThermal.System.SetGeneratorWasteHeat(baseGeneratorKw, heatingPowered);
            }
            _shelterThermal?.TickDay(day);
            _weatherHardening?.TickDay(day);
            _geothermalAquifer?.TickDay(day);
            // Plan 188: feed the campaign hour so the schedule derives Night from
            // its authored windows (the schedule remains the only phase owner).
            // The same hour drives the placeholder lighting backdrop variants.
            if (_campaignDay?.Calendar != null)
            {
                int hour = _campaignDay.Calendar.AsSimClock().HourOfDay;
                _shelterSchedule?.TickHour(hour);
                string lightingPhase = LightingPhaseForHour(hour);
                _shelterPanel?.SetLightingPhase(lightingPhase);
                _mapPanel?.SetLightingPhase(lightingPhase);
                _expeditionPanel?.SetLightingPhase(lightingPhase);
            }
            _shelterSchedule?.TickDay(day);
            _autopsy?.TickDay(day);
            // Plan 72 §3 ordering: advance ventilation/air filtration — hosts
            // pass weather truth; Core owns the intake conversion and stage math.
            if (_ventilation != null)
            {
                var ventWeather = _world.Weather.Current;
                // C2[6] 23A: mechanical air handling is a real load. When
                // room_air_filtration is shed the fans stop and only residual
                // passive draft removes air (see VentilationSystem.TickDay).
                bool mechanicalPower = _powerGrid?.System == null
                    || _powerGrid.System.IsRoomServed("room_air_filtration");
                _ventilation.TickDay(
                    day,
                    Ashfall.Core.ElectrostaticFiltrationCatalogLoader.WeatherIntakeParticulateKg(
                        ventWeather, _ventilation.State.mainDuctOpen),
                    Ashfall.Core.ElectrostaticFiltrationCatalogLoader.IsHotAshLoad(ventWeather),
                    mechanicalPower);
            }
            _waystation?.TickDaily(iceRoadOpen: true);
            _sumpFlooding?.TickDay(day);
            TickDeepWell(day); // B5–B8 Phase 6: deep-well raw-water intake (before consumers read the pools)
            TickWaterCondenser(day); // B5–B8 expansion: weather-indexed condensate intake
            _decontamination?.TickDay(day);
            TickPlans78To81(day);
            TickPlans110To113(day);
            TickPlans130To133(day);
            TickPlans146To149(day);
            _kitchenNutrition?.TickDay(day);
            TickPlans94To97(day);
            _equipmentCondition?.TickDay(day);
            _libraryStudy?.TickDay(day);
            _archiveDesk?.TickDay(day);
            _contractorRoster?.TickDay(day);
            _mentalHealthCrisis?.TickDay(day);
            TickSleepNarrative(day);
            _crafting?.TickDay(day);
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
                case "low_background_metrology":
                    if (_lowBackgroundPanel != null) { _lowBackgroundPanel.Visible = true; _lowBackgroundPanel.RefreshView(); }
                    break;
                case "insar_mapping":
                    if (_inSarPanel != null) { _inSarPanel.Visible = true; _inSarPanel.RefreshView(); }
                    break;
                case "hydraulic_extrusion":
                    if (_hydraulicExtrusionPanel != null) { _hydraulicExtrusionPanel.Visible = true; _hydraulicExtrusionPanel.RefreshView(); }
                    break;
                case "runflat_tire":
                    if (_runFlatTirePanel != null) { _runFlatTirePanel.Visible = true; _runFlatTirePanel.RefreshView(); }
                    break;
                case "sofc_power":
                    OpenSofcPowerPanel();
                    break;
                case "sound_ranging":
                    OpenSoundRangingPanel();
                    break;
                case "cvd_diamond":
                    OpenCvdDiamondPanel();
                    break;
                case "amphibious_draisine":
                    OpenAmphibiousDraisinePanel();
                    break;
                case "sanitation":
                    OpenSanitationPanel();
                    break;
                case "black_market":
                    OpenBlackMarketPanel();
                    break;
                case "sky_defense_battery":
                    OpenSkyDefenseBatteryPanel();
                    break;
                case "dynamic_quests":
                    OpenDynamicQuestlinePanel();
                    break;
                case "vehicle_garage":
                    OpenVehicleGaragePanel();
                    break;
                case "companion_kennel":
                    OpenKennelPanel();
                    break;
                case "beliefs_panel":
                    OpenBeliefsPanel();
                    break;
                case "anomaly_watch":
                    OpenAnomalyWatchPanel();
                    break;
                case "cybernetics":
                    OpenCyberneticsPanel();
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
                    SetupJournal();
                    DiscoverBureaucraticDocuments("archive_desk");
                    // Real archive-desk inspection of the bunker records drawer.
                    DiscoverFringeCultRecords("government_bunker");
                    DiscoverPaperPrintingRecords("government_bunker");
                    DiscoverBoneHornRecords("government_bunker");
                    DiscoverAbyssalAnomalyRecords("government_bunker");
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
                case "shelter_barter":
                    OpenShelterBarterPanel();
                    break;
                case "black_projects_archive":
                    OpenBlackProjectsArchivePanel();
                    break;
                case "shelter_decor":
                    SetupShelterDecor();
                    if (_shelterDecorPanel != null) { _shelterDecorPanel.Visible = true; _shelterDecorPanel.RefreshView(); }
                    break;
                case "medical_ward":
                    SetupJournal();
                    DiscoverBureaucraticDocuments("medical_office");
                    SetupMedicalWard();
                    if (_medicalWardPanel != null) { _medicalWardPanel.Visible = true; _medicalWardPanel.RefreshView(); }
                    break;
                case "plans_94_97":
                    SetupGrainProcessing();
                    SetupCryogenicAirSeparation();
                    SetupHeliograph();
                    SetupPlans94To97Panel();
                    if (_plans94To97Panel != null) { _plans94To97Panel.Visible = true; _plans94To97Panel.RefreshView(); }
                    break;
                case "plans_130_133":
                    OpenPlans130To133Panel();
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
            RemovePanel(_powerGridPanel); _powerGridPanel = null!;
            RemovePanel(_medicalWardPanel); _medicalWardPanel = null!;
            RemovePanel(_shelterDecorPanel); _shelterDecorPanel = null!;
            _plans94To97Panel?.Unbind();
            RemovePanel(_plans94To97Panel); _plans94To97Panel = null;
            _shelterBarterPanel?.Unbind();
            RemovePanel(_shelterBarterPanel); _shelterBarterPanel = null;
            ResetPlans130To133Panel();

            // Dispose / null host sessions
            _waterTreatment?.Dispose(); _waterTreatment = null!;
            _airlockSecurity?.Dispose(); _airlockSecurity = null!;
            _shelterThermal?.Dispose(); _shelterThermal = null!;
            _weatherHardening?.Dispose(); _weatherHardening = null!;
            _geothermalAquifer?.Dispose(); _geothermalAquifer = null!;
            _geothermalAquiferDirty = false;
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
            _powderMetallurgy?.Dispose(); _powderMetallurgy = null;
            _nvisCommunications?.Dispose(); _nvisCommunications = null;
            _lyophilization?.Dispose(); _lyophilization = null;
            _draisineRerailing?.Dispose(); _draisineRerailing = null;
            _grainProcessing?.Dispose(); _grainProcessing = null;
            _cryogenicAirSeparation?.Dispose(); _cryogenicAirSeparation = null;
            _heliograph?.Dispose(); _heliograph = null;
            _chemicalDependency?.Dispose(); _chemicalDependency = null!;
            _shelterAssignment?.Dispose(); _shelterAssignment = null!;
            _shelterDecor?.Dispose(); _shelterDecor = null!;
            _travelingCaravan?.Dispose(); _travelingCaravan = null!;
            _powerGrid?.Dispose(); _powerGrid = null!;
            _geothermalOrc?.Dispose(); _geothermalOrc = null!;
            _ballisticsWorkbench?.Dispose(); _ballisticsWorkbench = null!;
            _aeroponics?.Dispose(); _aeroponics = null!;
            _pneumaticDispatch?.Dispose(); _pneumaticDispatch = null!;
            _medicalWardSession?.Dispose(); _medicalWardSession = null!;
            _medicalWard = null!;
            _factionBranch?.Dispose(); _factionBranch = null!;
            _counterIntelligence?.Dispose(); _counterIntelligence = null!;
            _factionBranchDirty = false;
            _counterIntelligenceDirty = false;
            _expandedShelterRoster = new DutyRosterSystem();

            _airlockSecurityDirty = false;
            _shelterThermalDirty = false;
            _weatherHardeningDirty = false;
            _geothermalAquiferDirty = false;
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
            _powerGridDirty = false;
            _geothermalOrcDirty = false;
            _ballisticsWorkbenchDirty = false;
            _aeroponicsDirty = false;
            _pneumaticDispatchDirty = false;
            _medicalWardDirty = false;

            // Lifecycle reset is intentionally persistence-free. The expanded
            // shelter group owns the existing section captures, but it does
            // not delete slot projections, campaign envelopes, backups, or
            // global user:// saves. Destructive new-game cleanup remains in
            // the separately scoped DeleteGlobalSavesOnDisk path.
        }
    }
}
