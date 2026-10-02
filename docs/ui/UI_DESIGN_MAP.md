# ASHFALL UI Design Map — Programmatic

**Generated:** 2026-10-02 (from source; regenerate with `python3 scripts/ci/generate-ui-design-map.py`)

**Method:** static parse of `src/UI/*.cs`, `src/Main.PlayerSurfaces.cs`, `src/UI/GameDashboardPanel.cs`, `Ashfall.Core.UI.Theme`
**Canvas:** fixed 1920x1080 — full-rect modal shells

## 1. Surface inventory

- **240** UI panels in `src/UI/` — **112** dashboard shells, **116** bindable, **22** scene-backed, **119** with status rails
- Layout patterns: `shell:split-body` × 39, `shell:single-column` × 73, `scene-backed` × 22, `code-built` × 106
- Distinct shell minimum sizes: 720×480, 900×560, 900×580, 900×600, 900×620, 920×600, 950×600, 950×620, 1000×640, 1000×650, 1000×660, 1040×680, 1050×680, 1060×680, 1080×680, 1100×680, 1100×700, 1100×720, 1120×720, 1160×700, 1180×700, 1280×720

## 2. Design tokens (Ashfall.Core.UI.Theme)

### Colors

| Token | Hex |
|---|---|
| `Ink` | `#090B0C` |
| `Warm` | `#D3AA62` |
| `Hot` | `#F4C875` |
| `Pale` | `#E6E0D2` |
| `Muted` | `#938F84` |
| `Dim` | `#8E8F82` |
| `Exclusive` | `#C4785A` |
| `Critical` | `#FF5252` |
| `Surface` | `#0E1114` |
| `SurfaceCard` | `#14191E` |
| `SelectedBg` | `#282319` |
| `HoverBg` | `#1D2228` |
| `Success` | `#5CD670` |
| `Warning` | `#C97B3A` |
| `Radiation` | `#D9A026` |
| `RadiationAcute` | `#E65C2B` |
| `Info` | `#6EA3A8` |
| `Cyan` | `#6EA3A8` |
| `Entropy` | `#C97B3A` |
| `Lethe` | `#6EA3A8` |
| `Ozone` | `#DDE8E8` |
| `LetheAmber` | `#D4A35A` |
| `LetheRed` | `#D94040` |

### Spacing scale

| Token | px |
|---|---:|
| `SpacingXs` | 4 |
| `SpacingSm` | 8 |
| `SpacingMd` | 12 |
| `SpacingLg` | 16 |
| `SpacingXl` | 24 |

### Typography scale

| Token | px |
|---|---:|
| `FontSizeH1` | 30 |
| `FontSizeH2` | 24 |
| `FontSizeH3` | 19 |
| `FontSizeBody` | 15 |
| `FontSizeSmall` | 12 |
| `FontSizeMono` | 13 |
| `FontSizeLabel` | 12 |

## 3. Layout system

Every designed surface is an `AshfallDashboardShell` full-rect modal:
title bar → optional `AshfallStatusRail` (metric cards) → content stack (split-body or single-column) → header close button. Fixed canvas 1920×1080.

### Component library usage

| Component | Total instances | Panels using it |
|---|---:|---:|
| `datagrid` | 2051 | 34 |
| `metric_card` | 1644 | 122 |
| `vbox` | 489 | 156 |
| `separators` | 428 | 120 |
| `hbox` | 249 | 141 |
| `buttons` | 213 | 63 |
| `sidebar` | 199 | 32 |
| `scroll` | 124 | 84 |
| `option_buttons` | 53 | 26 |
| `panel_cards` | 39 | 23 |

## 4. Information architecture

Dashboard navigation groups in declared order (label → route):

### Primary

```
INVENTORY -> inventory
GREENHOUSE -> greenhouse
SURVIVORS -> survivors
MAP -> map
JOURNAL -> journal
SAVE / LOAD -> save
SETTINGS -> settings
STATUS -> status
SURVIVAL -> survival_detail
CRAFTING -> crafting
MEDICAL -> medical
AFFLICTIONS -> afflictions
EXPEDITIONS -> expeditions
WEATHER -> weather
WEATHER DETAIL -> weather_detail
RADIO -> radio
SHELTER -> shelter
ATMOSPHERE -> shelter_atmosphere
TRADE -> trade
ECONOMY -> economy_detail
RESEARCH -> research
FACTIONS -> factions
COMMUNIQUÉS -> faction_communique_board
MUSTER -> muster
VERDICT -> verdict
MARITIME -> maritime
DUTY ROSTER -> duty_roster
ROMANCE / FAMILY -> romance_family_board
COLONY OPERATIONS -> colony_operations
MEDIATION DESK -> ideological_mediation_desk
QUESTS -> quests
EVENTS -> event_detail
NARRATIVE ARCS -> narrative_arc
JOURNAL DETAIL -> journal_detail
BLACK PROJECTS -> black_projects_archive
RADIATION -> radiation_detail
RAD HISTORY -> radiation_history
DOSE ATLAS -> dose_geography
ACHIEVEMENTS -> achievements
HELP -> help
GUIDANCE -> guidance
POLITICS -> politics
PRISONERS -> prisoners
FORCED LABOR -> forced_labor
NARCOTICS LAB -> narcotics
MUTATIONS -> mutation_tree
NURSERY -> nursery
AVIATION -> aviation
STEALTH OPS -> stealth
FALLOUT RADAR -> fallout_detail
FARMING -> farming
GEOTHERMAL ORC -> geothermal_orc
BALLISTICS BENCH -> ballistics_workbench
AEROPONICS -> aeroponics
PNEUMATIC DISPATCH -> pneumatic_dispatch
DEFENSE GRID -> defense_grid
PSYCH WATCH -> psychology_arcs
BESTIARY -> bestiary
INTRIGUE -> hidden_agenda
REPUTATION -> shelter_reputation
PROPAGANDA -> propaganda
RUMORS -> rumors
SECURITY -> shelter_security
VISITORS -> visitor_integration
PERSONAL EFFECTS -> personal_belongings
PERSONAL QUESTS -> personal_quests
TIME CAPSULE -> time_capsule
WILLS & LEGACY -> death_legacy
SOCIAL BONDS -> relationship_decay
MEMORIAL CENOTAPH -> iron_cenotaph_memorial
BIOGAS DIGESTER -> biogas_digester
FUNGAL FERMENTER -> fungal_protein_fermenter
GEOTHERMAL TURBINE -> geothermal_turbine
DIESEL GENERATOR -> heavy_marine_diesel_gen
CUPOLA FURNACE -> induction_cupola_furnace
PLASMA SMELTING -> plasma_smelting
LOGISTICS AIRLOCK -> logistics_airlock
LONG WALK -> long_walk_expedition
PROSTHETICS LATHE -> mechanical_prosthetics_lathe
PRINTING PRESS -> printing_press
DEBT LEDGER -> subterranean_debt_ledger
WAR KENNEL -> war_dog_kennel
ULTRASONIC AIRLOCK -> ultrasonic_decontam_airlock
TRAUMA BONDS -> trauma_bonding_cohort
CONDUCT VOUCH -> crossing_safe_conduct_vouch
CARTOGRAPHY -> cartography_gis
DRUM ARCHIVE -> magnetic_drum_archive
SHRAPNEL AEGIS -> surface_shrapnel_aegis
INSURGENCY -> clandestine_insurgency
FUNGI BEDS -> fungi_cultivation
PLASTIC PYROLYSIS -> plastic_pyrolysis
CARGO AIRDROP -> cargo_airdrop
EB-PVD COATER -> ebpvd_coating
MICROFLUIDIC LAB -> microfluidic_diagnostic
MINE FLAIL -> mine_clearing_flail
RAIL GRINDING -> rail_grinding
RADIO INTEL -> radio_intelligence
SHELTER SOCIAL -> shelter_social
SUBTERRANEAN OPS -> subterranean_operations
SCRUBBER STAGE -> electrostatic_scrubber
GEOTHERMAL AQUIFER -> geothermal_aquifer
RADON MIGRATION -> basal_radon_migration
SEISMOGRAPH -> borehole_seismograph
CRYO VAULT -> cryo_permafrost_core
BREACHING OPS -> vault_door_breaching
```

- Panel-registry configured ids: **143**
- Expanded-surface ids: **64**
- `OpenExpandedPanel` cases: **65**

## 5. Measured design invariants

| Rule | Holds | Evidence |
|---|---|---|
| Every dashboard shell declares a min size ≥ 720×480 | ✅ | 112 shell panels |
| Every bindable panel exposes Unbind (lifecycle symmetry) | ❌ | 116 bindable panels; missing: DutyRosterDetailPanel, EpiloguePanel, EventsLogPanel, FactionDetailPanel, MaritimeAtlasPanel, OnboardingHintPanel, OpeningProtocolModal, WeatherSondePanel |
| Every expanded panel id has an open path (switch case or PanelRegistry binding) | ✅ | 64 ids, 65 switch cases, 143 registry bindings |
| Every surface with status-rail cards obtains the rail via shell.SetStatusRail() | ✅ | 119 surfaces with rails |

## 6. Shell surface design specs

Per-surface spec derived from source (binding target, layout, metrics, actions):

| Panel | Shell title | Binds | Layout | Min size | Rail cards | Scroll | Options | Buttons | Routes | LOC |
|---|---|---|---|---|---|---:|---:|---:|---|---:|
| `AirlockSecurityPanel` | Airlock Security // Sentry & Biometrics | `AirlockSecurityHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | airlock_security | 144 |
| `AmphibiousDraisinePanel` | Amphibious Draisine // Flooded-Corridor Mobility | `AmphibiousDraisineHostSession` | `shell:single-column` | 950×620 | 4 | 0 | 0 | 0 | amphibious_draisine | 161 |
| `AmputationTriagePanel` | TRIAGE TABLE // AMPUTATION & PROSTHETICS | `AmputationSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 328 |
| `AnomalyWatchPanel` | Anomaly Watch // Moving Hazards | `—` | `shell:single-column` | 900×560 | 4 | 0 | 0 | 0 | — | 136 |
| `ApprenticeshipPanel` | Apprenticeship // Skill Mentorship | `ApprenticeshipHostSession` | `shell:split-body` | 1000×650 | 2 | 1 | 2 | 0 | apprenticeship | 234 |
| `ArchaeologyExcavationPanel` | BEFORE // PRE-WAR ARCHIVES & DIG SITES | `ArchaeologySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 227 |
| `ArchiveDeskPanel` | SYS: ARCHIVAL SCRIPT & TRANSCRIPTION // ARCHIVE DESK | `ArchiveDeskHostSession` | `shell:split-body` | 1040×680 | 6 | 3 | 1 | 1 | archive_desk | 412 |
| `AutopsyReportPanel` | Clinical Autopsy // Forensic Pathology | `AutopsyHostSession` | `shell:single-column` | 1000×650 | 2 | 0 | 0 | 0 | autopsy_report | 120 |
| `AviationUI` | Aviation // Airfield & Reconnaissance | `AviationSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | aviation | 127 |
| `BeliefsPanel` | Beliefs // Doctrinal Climate | `—` | `shell:single-column` | 900×560 | 4 | 0 | 0 | 0 | — | 138 |
| `BestiaryPanel` | Wasteland Bestiary // Field Ledger | `—` | `shell:single-column` | 1000×660 | 4 | 0 | 0 | 0 | bestiary | 282 |
| `BioFermentationPanel` | FERMENTATION REACTOR // BIOLOGICAL PROCESS | `BioFermentationEngine` | `shell:single-column` | 1000×640 | 0 | 1 | 2 | 0 | — | 393 |
| `BlackMarketPanel` | The Quiet Counter // Underworld Trade | `BlackMarketHostSession` | `shell:split-body` | 920×600 | 5 | 1 | 0 | 4 | — | 449 |
| `CaregivingPanel` | Caregiving // Bedside Tending | `CaregivingHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | caregiving | 130 |
| `CargoAirdropPanel` | AIRDROP WATCH // CARGO RECOVERY | `CargoAirdropHostSession` | `shell:single-column` | 1000×650 | 0 | 1 | 0 | 0 | cargo_airdrop | 235 |
| `CeremonyFestivalPanel` | COMMON HEARTH // WASTELAND FESTIVALS | `CeremonySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 294 |
| `ChemUI` | Pharmacy // Chemical Engineering & Narcotics | `NarcoticsSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | narcotics, pharma_lab | 126 |
| `ChemWarfareDefensePanel` | ATMOSPHERE WATCH // TOXIC HAZARD MONITOR | `ChemWarfareSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 233 |
| `ChemicalDependencyPanel` | SYS: CHEMICAL DEPENDENCY & DETOX MATRIX v2.4 | `ChemicalDependencyHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | chemical_dependency | 420 |
| `ChemicalLabPanel` | CHEMICAL SYNTHESIS LAB // INDUSTRIAL REAGENTS | `—` | `shell:single-column` | 1060×680 | 5 | 2 | 0 | 0 | — | 477 |
| `ChemicalReconPanel` | Chemical Reconnaissance // CBRN Survey | `ChemicalReconHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 0 | 0 | — | 398 |
| `ChroniclePanel` | CAMPAIGN CHRONICLE // EPILOGUE ARCHIVE | `—` | `shell:split-body` | 1100×700 | 3 | 1 | 0 | 0 | chronicle, epilogue | 284 |
| `CombatHudOverlay` | Combat HUD Overlay // Live Encounter Mini-Monitor | `CombatHostSession` | `shell:split-body` | 1280×720 | 8 | 0 | 0 | 0 | — | 506 |
| `CommsArrayTransceiverPanel` | LONG EAR // COMMUNICATIONS ARRAY | `CommsArraySystem` | `shell:single-column` | 1050×680 | 5 | 1 | 0 | 0 | — | 259 |
| `ContractorRosterPanel` | SYS: CONTRACTOR GUILD & MERCENARY ROSTER // CONTRACT MATRIX | `ContractorRosterHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | contractor_roster | 331 |
| `CvdDiamondPanel` | Synthetic Diamond Tooling // CVD Growth | `CvdDiamondHostSession` | `shell:single-column` | 900×600 | 4 | 0 | 0 | 0 | cvd_diamond | 135 |
| `CyberneticsPanel` | Cybernetics // Prosthetic Care | `—` | `shell:single-column` | 900×580 | 4 | 0 | 0 | 0 | — | 155 |
| `DeconAirlockPanel` | Decontamination Airlock // Baseline Interlock | `DecontaminationHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 0 | 0 | — | 401 |
| `DecontaminationPanel` | SYS: AIRLOCK DECONTAMINATION // SCRUBBER MATRIX | `DecontaminationHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | decontamination | 348 |
| `DefenseGridPanel` | Defense Grid // Wire & Pit | `DefenseHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 2 | 0 | defense_grid | 491 |
| `DesperationCrisisPanel` | SANCTUARY CRISIS // DESPERATION & TABOO MONITOR | `DesperationSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 2 | — | 161 |
| `DifficultySettingsPanel` | Difficulty // Campaign Parameters | `—` | `shell:split-body` | 900×620 | 2 | 0 | 0 | 0 | — | 386 |
| `DoseGeographyPanel` | DOSE GEOGRAPHY — EXPOSURE MAP | `—` | `shell:split-body` | 720×480 | 8 | 0 | 0 | 0 | — | 519 |
| `DutyRosterPanel` | Duty Roster // Shift Coverage & Sick-List | `—` | `shell:split-body` | 1100×720 | 6 | 0 | 0 | 4 | duty_roster, duty_roster_detail | 757 |
| `DynamicQuestlinePanel` | EMERGENCY QUESTS // DYNAMIC OPERATIONS | `DynamicQuestlineSystem` | `shell:single-column` | 1100×680 | 5 | 1 | 0 | 0 | dynamic_quests | 201 |
| `EbPvdCoatingPanel` | EB-PVD COATER // THERMAL BARRIER DEPOSITION | `EbPvdCoatingHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | ebpvd_coating | 345 |
| `EquipmentConditionPanel` | SYS: ARMORY WORKBENCH & CONDITION // DURABILITY MATRIX | `EquipmentConditionHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | equipment_condition | 315 |
| `ExcavationPanel` | Subterranean Excavation // Deep Strata | `ExcavationHostSession` | `shell:single-column` | 1000×650 | 1 | 0 | 0 | 0 | excavation | 107 |
| `FactionsNarrativePanel` | Factions Narrative // Trust Tree & Diplomacy | `IFactionStanceProvider` | `shell:single-column` | 1100×720 | 6 | 0 | 0 | 0 | event_detail, journal | 501 |
| `FalloutPlumePanel` | ATMOSPHERIC HAZARD // FALLOUT TRACKING & DISPERSAL | `FalloutSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 1 | — | 145 |
| `FarmingPanel` | Advanced Agriculture // Bench & Compost | `AgricultureHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 1 | 0 | — | 482 |
| `FungiCultivationBedPanel` | DARK BEDS // SUBTERRANEAN FUNGI CULTIVATION | `FungiCultivationSystem` | `shell:single-column` | 1000×650 | 0 | 1 | 2 | 0 | — | 315 |
| `GeodeticSurveyPanel` | Geodetic Survey // Triangulation Network | `GeodeticSurveyHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 0 | 0 | — | 399 |
| `HiddenAgendaPanel` | Survivor Intrigue // Hidden Agendas & Betrayals | `HiddenAgendaHostSession` | `shell:split-body` | 1000×650 | 4 | 1 | 0 | 1 | hidden_agenda | 321 |
| `HydraulicExtrusionPanel` | Heavy Fabrication // Hydraulic Extrusion | `HydraulicExtrusionHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 2 | 3 | hydraulic_extrusion | 297 |
| `InSarMappingPanel` | Deformation Intelligence // InSAR Mapping | `InSarMappingHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 2 | 2 | insar_mapping | 277 |
| `JournalPanel` | JOURNAL & NARRATIVE | `JournalHostSession` | `shell:split-body` | 720×480 | 0 | 1 | 0 | 0 | journal | 498 |
| `JusticeTribunalPanel` | THE RECORD // SHELTER TRIBUNAL | `JusticeSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 1 | 0 | — | 224 |
| `KennelPanel` | Kennel // Companion Animals | `—` | `shell:single-column` | 900×560 | 4 | 0 | 0 | 0 | — | 126 |
| `KineticStoragePanel` | Kinetic Storage // Flywheel Control | `KineticStorageHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | — | 390 |
| `KitchenNutritionPanel` | SYS: KITCHEN GALLEY & NUTRITION // DIETARY MATRIX | `KitchenNutritionHostSession` | `shell:split-body` | 1040×680 | 6 | 4 | 0 | 0 | kitchen_nutrition | 656 |
| `LaborUI` | Forced Labor // Captivity & Work Details | `ForcedLaborSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | forced_labor | 132 |
| `LibraryStudyPanel` | SYS: COHORT LIBRARY & TECH STUDY // STUDY MATRIX | `LibraryStudyHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | library_study | 348 |
| `LowBackgroundLeadPanel` | Assay Bench // Low-Background Metrology | `LowBackgroundMetrologyHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 1 | 4 | low_background_metrology | 270 |
| `MaritimeAtlasPanel` | Maritime Atlas // Deep Coast Dive Coordinates | `MaritimeHostSession` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 467 |
| `MedicalWardPanel` | SYS: TRAUMA WARD & SURGICAL TRIAGE BAY-03 | `MedicalWardHostSession` | `shell:split-body` | 1060×680 | 5 | 3 | 0 | 0 | medical_ward | 480 |
| `MentalHealthCrisisPanel` | SYS: PSYCHIATRIC WARD & CRISIS INTERVENTION // TRIAGE MATRIX | `MentalHealthCrisisHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | mental_health_crisis | 302 |
| `MercenaryBountyBoardPanel` | WARLORD CONTRACTS // MERCENARY BOUNTY BOARD | `MercenarySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 237 |
| `MicrofluidicDiagnosticPanel` | MICROFLUIDIC DIAGNOSTICS // RAPID IMMUNOCHIP ANALYZER | `MicrofluidicDiagnosticHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | microfluidic_diagnostic | 394 |
| `MineFlailPanel` | DEMINING FLAIL MODULE // VEHICLE BREACHING CONTROL | `MineClearingFlailHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | mine_flail | 314 |
| `MusterAtlasPanel` | The Muster // Sector Currents & Coalition Camps | `MusterHostSession` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 546 |
| `MutationTreePanel` | Genetics // Mutation Trees & Instability | `MutationSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | mutation_tree | 112 |
| `NightWatchPanel` | The Watch // Patrol Readiness | `NightWatchHostSession` | `shell:single-column` | 1080×680 | 4 | 1 | 1 | 0 | night_watch | 350 |
| `NurseryPanel` | Nursery // Childhood & Schoolhouse | `GenerationalSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 2 | century_seed, nursery | 166 |
| `PersonalBelongingsPanel` | Personal Effects // Keepsakes, Favorites & Gifts | `PersonalBelongingsHostSession` | `shell:split-body` | 1000×650 | 3 | 1 | 5 | 5 | — | 315 |
| `PersonalQuestPanel` | Survivor Character Arcs // Personal Quests | `PersonalQuestHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | — | 228 |
| `PhantomMemoryPanel` | SYS: PHANTOM MEMORY & RELIC TRIGGERS // PSYCH MATRIX | `—` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | phantom_memory, standing_record | 342 |
| `GeothermalOrcPanel` | B74 // GEOTHERMAL ORC LOOP | `GeothermalOrcHostSession` | `shell:single-column` | 720×480 | 0 | 0 | 0 | 0 | — | 387 |
| `PlasticPyrolysisPanel` | RETORT BAY // WASTE PLASTIC RECLAMATION | `PlasticPyrolysisHostSession` | `shell:single-column` | 1000×650 | 0 | 1 | 0 | 0 | plastic_pyrolysis | 274 |
| `PoliticsUI` | Council Chamber // Settlement Politics & Law | `PoliticsSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | politics | 127 |
| `PrisonerPanel` | Detention // Prisoner Management & Intel | `PrisonerSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 3 | prisoners | 190 |
| `PropagandaPanel` | Information Warfare // Propaganda & Influence | `PropagandaHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 0 | propaganda | 227 |
| `PsychologyArcPanel` | Psychological Arcs // Watch | `—` | `shell:single-column` | 1100×700 | 4 | 0 | 0 | 0 | — | 332 |
| `QuestsAtlasPanel` | Quests Atlas // Storyline & Protocol Progression | `—` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 431 |
| `RailGrindingPanel` | RAIL GRINDING & REPROFILING // STRATEGIC RAIL CORRIDOR CONTROL | `RailGrindingHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | rail_grinding | 328 |
| `RailwayTerminalPanel` | THE LINE // RAILWAY TERMINAL | `RailwaySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 248 |
| `RecruitmentPanel` | Survivor Recruitment & Defection Desk // Operations | `RecruitmentHostSession` | `shell:single-column` | 1000×660 | 4 | 0 | 0 | 0 | recruitment | 261 |
| `RegionalTreatyPanel` | Regional Treaty // Diplomatic Accords | `RegionalTreatyHostSession` | `shell:single-column` | 1000×650 | 2 | 0 | 0 | 0 | regional_treaty | 161 |
| `RelationshipDecayPanel` | Survivor Social Ecology // Bonds & Social Drift | `RelationshipDecayHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | relationship_decay | 192 |
| `ResearchAtlasPanel` | Research Atlas // Knowledge Nodes · Breakthroughs · R&D Queue | `ResearchHostSession` | `shell:split-body` | 1280×720 | 6 | 1 | 0 | 1 | — | 497 |
| `RoboticsWorkshopPanel` | AWAKENED STEEL // ROBOTICS WORKSHOP | `RoboticsSystem` | `shell:single-column` | 1050×680 | 4 | 1 | 1 | 0 | — | 295 |
| `RumorBoardPanel` | Intelligence & Rumors // Wasteland Whispers & Intercepts | `RumorNetworkHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 1 | rumors | 241 |
| `RunFlatTirePanel` | Vehicle Shop // Run-Flat Wheel Set | `RunFlatTireHostSession` | `shell:single-column` | 1000×650 | 5 | 0 | 3 | 4 | runflat_tire | 249 |
| `SanitationPanel` | Waste & Sanitation // Shelter Hygiene | `SanitationHostSession` | `shell:single-column` | 900×580 | 4 | 0 | 0 | 0 | sanitation | 142 |
| `ShelterAtmospherePanel` | Shelter Atmosphere & Ambiance // Environmental Character | `ShelterAtmosphereHostSession` | `shell:single-column` | 950×600 | 4 | 0 | 0 | 2 | shelter_atmosphere | 233 |
| `ShelterBarterPanel` | SHELTER AIRLOCK BARTER // CARAVAN TRADING ROUTE | `—` | `shell:split-body` | 1100×700 | 5 | 3 | 0 | 0 | — | 1169 |
| `ShelterDecorPanel` | Shelter Interior // Memorial Wall | `ShelterDecorHostSession` | `shell:single-column` | 1160×700 | 4 | 2 | 1 | 0 | — | 451 |
| `ShelterOperationsPanel` | Shelter Operations // Works, Crews & Outposts | `ShelterOperationsHostSession` | `shell:single-column` | 1120×720 | 4 | 1 | 1 | 0 | shelter_operations | 399 |
| `ShelterReputationPanel` | Shelter Reputation // External Perception & Notoriety | `ShelterReputationHostSession` | `shell:split-body` | 1000×650 | 4 | 1 | 0 | 0 | shelter_reputation | 329 |
| `ShelterSchedulePanel` | Shelter Schedule // Shift Assignment | `ShelterScheduleHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | shelter_schedule | 129 |
| `ShelterSecurityPanel` | Shelter Security // Access Control & Lockdown Protocols | `ShelterSecurityHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 5 | shelter_security | 305 |
| `ShelterThermalPanel` | Shelter Thermal // Central Heating | `ShelterThermalHostSession` | `shell:single-column` | 1000×650 | 5 | 0 | 0 | 1 | shelter_thermal | 240 |
| `SilentFoundryPanel` | The Silent Foundry // Cupola & Casting Bay | `—` | `shell:split-body` | 1100×720 | 8 | 0 | 0 | 0 | silent_foundry | 594 |
| `SkillMatrixPanel` | Survivor Skill Matrix // Progression & Decay | `—` | `shell:single-column` | 1100×720 | 6 | 0 | 0 | 0 | — | 504 |
| `SkyDefenseBatteryPanel` | SKY DEFENSE // COUNTER-BATTERY | `—` | `shell:single-column` | 1180×700 | 6 | 1 | 4 | 0 | sky_defense_battery | 636 |
| `SolidOxideFuelCellPanel` | Solid-Oxide Fuel Cell // Quiet Baseload | `SofcPowerHostSession` | `shell:single-column` | 900×600 | 4 | 0 | 0 | 0 | sofc_power | 148 |
| `SoundRangingPanel` | Acoustic Sound-Ranging // Siege Warning | `SoundRangingHostSession` | `shell:single-column` | 950×620 | 4 | 0 | 0 | 0 | sound_ranging | 156 |
| `StandingRecordAtlasPanel` | Standing Record Atlas // Ground Layouts · Memory Strata · Site Mutations | `StandingRecordHostSession` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 525 |
| `StealthReadoutPanel` | Stealth // Camouflage & Detection Readout | `StealthSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | stealth | 114 |
| `SumpFloodingPanel` | SYS: SUBLEVEL SUMP & FLOOD CONTROL // MATRIX | `SumpFloodingHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | sump_flooding | 367 |
| `SurvivorDeathLegacyPanel` | Survivor Memorial & Wills // Death Records & Estates | `SurvivorDeathLegacyHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | death_legacy | 215 |
| `SurvivorDowntimePanel` | COMMON HOURS // HOBBIES & DOWNTIME | `SurvivorDowntimeSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 234 |
| `SurvivorRelationsPanel` | Survivor Relations // Social Dynamics | `SurvivorRelationsHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | survivor_relations | 198 |
| `TimeCapsulePanel` | Shelter Heritage // Time Capsules & Legacy Messages | `TimeCapsuleHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | time_capsule | 297 |
| `TravelingCaravanPanel` | SYS: TRAVELING CARAVANS & REGIONAL TRADE // ROUTE RADAR | `—` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | traveling_caravan | 466 |
| `VehicleGaragePanel` | VEHICLE GARAGE // OVERLAND MAINTENANCE | `—` | `shell:single-column` | 1180×700 | 4 | 1 | 3 | 0 | vehicle_garage | 636 |
| `VinylMoralePanel` | Common Room // Vinyl Turntable | `VinylMoraleHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 1 | 2 | vinyl_morale | 207 |
| `VisitorIntegrationPanel` | Visitor Integration // Temporary Residency & Processing | `VisitorIntegrationHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 8 | visitor_integration | 297 |
| `WaterTreatmentPanel` | Water Treatment // Filtration & Decon | `WaterTreatmentHostSession` | `shell:single-column` | 1000×650 | 5 | 0 | 0 | 0 | water_treatment | 298 |
| `WaystationNetworkPanel` | Waystation A // Forward Outpost | `WaystationHostSession` | `shell:split-body` | 1000×650 | 3 | 0 | 0 | 2 | waystation_network | 186 |
| `WildlifeTrappingPanel` | Wildlife Trapping // Snare Network | `WildlifeTrappingHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 1 | 6 | wildlife_trapping | 525 |
| `WinterFreezePanel` | DEEP WINTER // FREEZE WATCH | `YearOfAshDeepFreezeSystem` | `shell:single-column` | 950×620 | 4 | 1 | 0 | 0 | — | 194 |

## 7. Non-shell bindable surfaces

23 bindable panels render without the dashboard shell (scene-backed forms, sub-views, read-only rows):

| Panel | Binds | Pattern | Routes | LOC |
|---|---|---|---|---:|
| `BrineExtractionPanel` | `SilentFoundryHostSession` | `code-built` | — | 237 |
| `CombatDetailPanel` | `CombatHostSession` | `scene-backed` | combat, combat_detail | 162 |
| `CombatHistoryPanel` | `CombatHostSession` | `code-built` | combat, combat_detail | 211 |
| `CombatPanel` | `CombatHostSession` | `code-built` | combat, combat_detail | 740 |
| `DutyRosterDetailPanel` | `DutyRosterHostSession` | `scene-backed` | duty_roster, duty_roster_detail | 165 |
| `EmergencyResponseHud` | `CrisisPresentationSnapshot` | `scene-backed` | — | 330 |
| `EpiloguePanel` | `CampaignOutcomeSnapshot` | `code-built` | chronicle, epilogue | 252 |
| `EventsLogPanel` | `object` | `code-built` | event_detail, journal | 205 |
| `FactionDetailPanel` | `object` | `scene-backed` | — | 156 |
| `FactionMatrixPanel` | `IFactionStanceProvider` | `code-built` | — | 482 |
| `FeedbackPanel` | `IFeedbackService` | `code-built` | — | 262 |
| `GeothermalAquiferPanel` | `GeothermalAquiferHostSession` | `code-built` | — | 173 |
| `GreenhousePanel` | `GreenhouseHostSession` | `code-built` | greenhouse | 806 |
| `InventoryPanel` | `InventoryHostSession` | `code-built` | inventory, inventory_detail | 341 |
| `OnboardingHintPanel` | `OnboardingJourney` | `code-built` | help | 498 |
| `OpeningProtocolModal` | `StartingLevelHostSession` | `code-built` | protocol | 277 |
| `PowerGridPanel` | `PowerGridHostSession` | `code-built` | power_grid | 316 |
| `RadioPanel` | `RadioHostSession` | `code-built` | radio | 741 |
| `ReconTelemetryPanel` | `ReconTelemetryHostSession` | `code-built` | — | 160 |
| `SaveLoadPanel` | `SaveLoadHostSession` | `code-built` | — | 411 |
| `WeatherHistoryPanel` | `WeatherSystem` | `code-built` | — | 207 |
| `WeatherPanel` | `WeatherHostSession` | `code-built` | map, weather | 511 |
| `WeatherSondePanel` | `WeatherHostSession` | `code-built` | — | 202 |

## 8. Machine-readable output

Full token/route/panel JSON: `docs/ui/ui_design_map.json` (240 panels, 1 nav groups).

