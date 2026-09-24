# ASHFALL UI Design Map — Programmatic

**Generated:** 2026-09-24 (from source; regenerate with `python3 scripts/ci/generate-ui-design-map.py`)

**Method:** static parse of `src/UI/*.cs`, `src/Main.PlayerSurfaces.cs`, `src/UI/GameDashboardPanel.cs`, `Ashfall.Core.UI.Theme`
**Canvas:** fixed 1920x1080 — full-rect modal shells

## 1. Surface inventory

- **241** UI panels in `src/UI/` — **110** dashboard shells, **114** bindable, **22** scene-backed, **118** with status rails
- Layout patterns: `shell:split-body` × 38, `shell:single-column` × 72, `scene-backed` × 22, `code-built` × 109
- Distinct shell minimum sizes: 720×480, 900×560, 900×580, 900×600, 920×600, 950×600, 950×620, 1000×640, 1000×650, 1000×660, 1040×680, 1050×680, 1060×680, 1100×640, 1100×680, 1100×700, 1100×720, 1160×700, 1180×700, 1280×720

## 2. Design tokens (Ashfall.Core.UI.Theme)

### Colors

| Token | Hex |
|---|---|
| `Ink` | `#090B0C` |
| `Warm` | `#D3AA62` |
| `Hot` | `#F4C875` |
| `Pale` | `#C7DCD0` |
| `Muted` | `#938F84` |
| `Dim` | `#66675F` |
| `Exclusive` | `#C4785A` |
| `Critical` | `#E63333` |
| `Surface` | `#050709` |
| `SurfaceCard` | `#090B0D` |
| `SelectedBg` | `#282319` |
| `HoverBg` | `#1D2228` |
| `Success` | `#5CD670` |
| `Warning` | `#FF6B35` |
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
| `FontSizeLabel` | 11 |

## 3. Layout system

Every designed surface is an `AshfallDashboardShell` full-rect modal:
title bar → optional `AshfallStatusRail` (metric cards) → content stack (split-body or single-column) → header close button. Fixed canvas 1920×1080.

### Component library usage

| Component | Total instances | Panels using it |
|---|---:|---:|
| `datagrid` | 2014 | 33 |
| `metric_card` | 1587 | 118 |
| `vbox` | 473 | 151 |
| `separators` | 413 | 118 |
| `buttons` | 229 | 65 |
| `hbox` | 223 | 133 |
| `sidebar` | 199 | 32 |
| `scroll` | 117 | 78 |
| `option_buttons` | 43 | 20 |
| `panel_cards` | 39 | 23 |

## 4. Information architecture

Dashboard navigation groups in declared order (label → route):

### SYSTEMS

```
STATUS -> status
SURVIVORS -> survivors
SURVIVAL -> survival_detail
INVENTORY -> inventory
CRAFTING -> crafting
MEDICAL -> medical
AFFLICTIONS -> afflictions
EXPEDITIONS -> expeditions
WEATHER -> weather
WEATHER DETAIL -> weather_detail
RADIO -> radio
MAP -> map
SHELTER -> shelter
ATMOSPHERE -> shelter_atmosphere
TRADE -> trade
ECONOMY -> economy_detail
RESEARCH -> research
GREENHOUSE -> greenhouse
FACTIONS -> factions
COMMUNIQUÉS -> faction_communique_board
MUSTER -> muster
VERDICT -> verdict
MARITIME -> maritime
DUTY ROSTER -> duty_roster
QUESTS -> quests
EVENTS -> event_detail
NARRATIVE ARCS -> narrative_arc
JOURNAL -> journal_detail
BLACK PROJECTS -> black_projects_archive
RADIATION -> radiation_detail
RAD HISTORY -> radiation_history
DOSE ATLAS -> dose_geography
ACHIEVEMENTS -> achievements
HELP -> help
GUIDANCE -> guidance
```

### EXPANSION SURFACES

```
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
```

- Panel-registry configured ids: **135**
- Expanded-surface ids: **61**
- `OpenExpandedPanel` cases: **61**

## 5. Measured design invariants

| Rule | Holds | Evidence |
|---|---|---|
| Every dashboard shell declares a min size ≥ 720×480 | ✅ | 110 shell panels |
| Every bindable panel exposes Unbind (lifecycle symmetry) | ❌ | 114 bindable panels; missing: DutyRosterDetailPanel, EpiloguePanel, EventsLogPanel, FactionDetailPanel, MaritimeAtlasPanel, OnboardingHintPanel, OpeningProtocolModal, WeatherSondePanel |
| Every expanded panel id has an open path (switch case or PanelRegistry binding) | ✅ | 61 ids, 61 switch cases, 135 registry bindings |
| Every surface with status-rail cards obtains the rail via shell.SetStatusRail() | ✅ | 118 surfaces with rails |

## 6. Shell surface design specs

Per-surface spec derived from source (binding target, layout, metrics, actions):

| Panel | Shell title | Binds | Layout | Min size | Rail cards | Scroll | Options | Buttons | Routes | LOC |
|---|---|---|---|---|---|---:|---:|---:|---|---:|
| `AirlockSecurityPanel` | Airlock Security // Sentry & Biometrics | `AirlockSecurityHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | airlock_security | 142 |
| `AmphibiousDraisinePanel` | Amphibious Draisine // Flooded-Corridor Mobility | `AmphibiousDraisineHostSession` | `shell:single-column` | 950×620 | 4 | 0 | 0 | 0 | amphibious_draisine | 159 |
| `AmputationTriagePanel` | TRIAGE TABLE // AMPUTATION & PROSTHETICS | `AmputationSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 274 |
| `AnomalyWatchPanel` | Anomaly Watch // Moving Hazards | `—` | `shell:single-column` | 900×560 | 4 | 0 | 0 | 0 | — | 136 |
| `ApprenticeshipPanel` | Apprenticeship // Skill Mentorship | `ApprenticeshipHostSession` | `shell:single-column` | 1000×650 | 2 | 0 | 0 | 0 | apprenticeship | 113 |
| `ArchaeologyExcavationPanel` | BEFORE // PRE-WAR ARCHIVES & DIG SITES | `ArchaeologySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 222 |
| `ArchiveDeskPanel` | SYS: ARCHIVAL SCRIPT & TRANSCRIPTION // ARCHIVE DESK | `ArchiveDeskHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | archive_desk | 303 |
| `AutopsyReportPanel` | Clinical Autopsy // Forensic Pathology | `AutopsyHostSession` | `shell:single-column` | 1000×650 | 2 | 0 | 0 | 0 | autopsy_report | 117 |
| `AviationUI` | Aviation // Airfield & Reconnaissance | `AviationSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | aviation | 127 |
| `BeliefsPanel` | Beliefs // Doctrinal Climate | `—` | `shell:single-column` | 900×560 | 4 | 0 | 0 | 0 | — | 138 |
| `BestiaryPanel` | Wasteland Bestiary // Field Ledger | `—` | `shell:single-column` | 1000×660 | 4 | 0 | 0 | 0 | bestiary | 282 |
| `BioFermentationPanel` | FERMENTATION REACTOR // BIOLOGICAL PROCESS | `BioFermentationEngine` | `shell:single-column` | 1000×640 | 0 | 1 | 2 | 0 | — | 332 |
| `BlackMarketPanel` | The Quiet Counter // Underworld Trade | `BlackMarketHostSession` | `shell:split-body` | 920×600 | 5 | 1 | 0 | 4 | — | 448 |
| `CaregivingPanel` | Caregiving // Bedside Tending | `CaregivingHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | caregiving | 128 |
| `CargoAirdropPanel` | AIRDROP WATCH // CARGO RECOVERY | `CargoAirdropHostSession` | `shell:single-column` | 1000×650 | 0 | 1 | 0 | 0 | cargo_airdrop | 230 |
| `CeremonyFestivalPanel` | COMMON HEARTH // WASTELAND FESTIVALS | `CeremonySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 289 |
| `ChemUI` | Pharmacy // Chemical Engineering & Narcotics | `NarcoticsSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | narcotics, pharma_lab | 126 |
| `ChemWarfareDefensePanel` | ATMOSPHERE WATCH // TOXIC HAZARD MONITOR | `ChemWarfareSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 228 |
| `ChemicalDependencyPanel` | SYS: CHEMICAL DEPENDENCY & DETOX MATRIX v2.4 | `ChemicalDependencyHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | chemical_dependency | 413 |
| `ChemicalLabPanel` | CHEMICAL SYNTHESIS LAB // INDUSTRIAL REAGENTS | `—` | `shell:single-column` | 1060×680 | 5 | 2 | 0 | 0 | — | 477 |
| `ChemicalReconPanel` | Chemical Reconnaissance // CBRN Survey | `ChemicalReconHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 0 | 0 | — | 398 |
| `ChroniclePanel` | CAMPAIGN CHRONICLE // EPILOGUE ARCHIVE | `—` | `shell:split-body` | 1100×700 | 2 | 1 | 0 | 0 | chronicle, epilogue | 218 |
| `CombatHudOverlay` | Combat HUD Overlay // Live Encounter Mini-Monitor | `CombatHostSession` | `shell:split-body` | 1280×720 | 8 | 0 | 0 | 0 | — | 453 |
| `CommsArrayTransceiverPanel` | LONG EAR // COMMUNICATIONS ARRAY | `CommsArraySystem` | `shell:single-column` | 1050×680 | 5 | 1 | 0 | 0 | — | 254 |
| `ContractorRosterPanel` | SYS: CONTRACTOR GUILD & MERCENARY ROSTER // CONTRACT MATRIX | `ContractorRosterHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | contractor_roster | 331 |
| `CvdDiamondPanel` | Synthetic Diamond Tooling // CVD Growth | `CvdDiamondHostSession` | `shell:single-column` | 900×600 | 4 | 0 | 0 | 0 | cvd_diamond | 133 |
| `CyberneticsPanel` | Cybernetics // Prosthetic Care | `—` | `shell:single-column` | 900×580 | 4 | 0 | 0 | 0 | — | 155 |
| `DeconAirlockPanel` | Decontamination Airlock // Baseline Interlock | `DecontaminationHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 0 | 0 | — | 401 |
| `DecontaminationPanel` | SYS: AIRLOCK DECONTAMINATION // SCRUBBER MATRIX | `DecontaminationHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | decontamination | 348 |
| `DefenseGridPanel` | Defense Grid // Wire & Pit | `DefenseHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 2 | 0 | defense_grid | 475 |
| `DesperationCrisisPanel` | SANCTUARY CRISIS // DESPERATION & TABOO MONITOR | `DesperationSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 2 | — | 161 |
| `DoseGeographyPanel` | DOSE GEOGRAPHY — EXPOSURE MAP | `—` | `shell:split-body` | 720×480 | 6 | 0 | 0 | 0 | — | 433 |
| `DutyRosterPanel` | Duty Roster // Shift Coverage & Sick-List | `—` | `shell:split-body` | 1100×720 | 6 | 0 | 0 | 4 | duty_roster, duty_roster_detail | 724 |
| `DynamicQuestlinePanel` | EMERGENCY QUESTS // DYNAMIC OPERATIONS | `DynamicQuestlineSystem` | `shell:single-column` | 1100×680 | 5 | 1 | 0 | 0 | dynamic_quests | 199 |
| `EbPvdCoatingPanel` | EB-PVD COATER // THERMAL BARRIER DEPOSITION | `EbPvdCoatingHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | ebpvd_coating | 352 |
| `EquipmentConditionPanel` | SYS: ARMORY WORKBENCH & CONDITION // DURABILITY MATRIX | `EquipmentConditionHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | equipment_condition | 315 |
| `ExcavationPanel` | Subterranean Excavation // Deep Strata | `ExcavationHostSession` | `shell:single-column` | 1000×650 | 1 | 0 | 0 | 0 | excavation | 107 |
| `ExpeditionRadarPanel` | Sortie Radar // Wasteland Movement Network | `—` | `shell:split-body` | 1100×640 | 6 | 0 | 0 | 0 | — | 603 |
| `FactionsNarrativePanel` | Factions Narrative // Trust Tree & Diplomacy | `IFactionStanceProvider` | `shell:single-column` | 1100×720 | 6 | 0 | 0 | 0 | event_detail, journal | 501 |
| `FalloutPlumePanel` | ATMOSPHERIC HAZARD // FALLOUT TRACKING & DISPERSAL | `FalloutSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 1 | — | 145 |
| `FarmingPanel` | Advanced Agriculture // Bench & Compost | `AgricultureHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 1 | 0 | — | 482 |
| `FungiCultivationBedPanel` | DARK BEDS // SUBTERRANEAN FUNGI CULTIVATION | `FungiCultivationSystem` | `shell:single-column` | 1000×650 | 0 | 1 | 2 | 0 | — | 310 |
| `GeodeticSurveyPanel` | Geodetic Survey // Triangulation Network | `GeodeticSurveyHostSession` | `shell:single-column` | 1100×720 | 5 | 0 | 0 | 0 | — | 399 |
| `GreenhousePanel` | The Glass Orchard // Sub-surface Hydroponics | `GreenhouseHostSession` | `shell:single-column` | 1100×720 | 9 | 0 | 0 | 0 | greenhouse | 806 |
| `HiddenAgendaPanel` | Survivor Intrigue // Hidden Agendas & Betrayals | `HiddenAgendaHostSession` | `shell:split-body` | 1000×650 | 4 | 1 | 0 | 1 | hidden_agenda | 321 |
| `HydraulicExtrusionPanel` | Heavy Fabrication // Hydraulic Extrusion | `HydraulicExtrusionHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 2 | 3 | hydraulic_extrusion | 297 |
| `InSarMappingPanel` | Deformation Intelligence // InSAR Mapping | `InSarMappingHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 2 | 2 | insar_mapping | 275 |
| `JournalPanel` | JOURNAL & NARRATIVE | `JournalHostSession` | `shell:split-body` | 720×480 | 0 | 1 | 0 | 0 | journal | 487 |
| `JusticeTribunalPanel` | THE RECORD // SHELTER TRIBUNAL | `JusticeSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 1 | 0 | — | 219 |
| `KennelPanel` | Kennel // Companion Animals | `—` | `shell:single-column` | 900×560 | 4 | 0 | 0 | 0 | — | 126 |
| `KineticStoragePanel` | Kinetic Storage // Flywheel Control | `KineticStorageHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | — | 390 |
| `KitchenNutritionPanel` | SYS: KITCHEN GALLEY & NUTRITION // DIETARY MATRIX | `KitchenNutritionHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | kitchen_nutrition | 395 |
| `LaborUI` | Forced Labor // Captivity & Work Details | `ForcedLaborSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | forced_labor | 132 |
| `LibraryStudyPanel` | SYS: COHORT LIBRARY & TECH STUDY // STUDY MATRIX | `LibraryStudyHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | library_study | 348 |
| `LowBackgroundLeadPanel` | Assay Bench // Low-Background Metrology | `LowBackgroundMetrologyHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 1 | 4 | low_background_metrology | 270 |
| `MaritimeAtlasPanel` | Maritime Atlas // Deep Coast Dive Coordinates | `MaritimeHostSession` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 467 |
| `MedicalWardPanel` | SYS: TRAUMA WARD & SURGICAL TRIAGE BAY-03 | `MedicalWardHostSession` | `shell:split-body` | 1060×680 | 5 | 3 | 0 | 0 | medical_ward | 476 |
| `MentalHealthCrisisPanel` | SYS: PSYCHIATRIC WARD & CRISIS INTERVENTION // TRIAGE MATRIX | `MentalHealthCrisisHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | mental_health_crisis | 302 |
| `MercenaryBountyBoardPanel` | WARLORD CONTRACTS // MERCENARY BOUNTY BOARD | `MercenarySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 232 |
| `MicrofluidicDiagnosticPanel` | MICROFLUIDIC DIAGNOSTICS // RAPID IMMUNOCHIP ANALYZER | `MicrofluidicDiagnosticHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | microfluidic_diagnostic | 401 |
| `MineFlailPanel` | DEMINING FLAIL MODULE // VEHICLE BREACHING CONTROL | `MineClearingFlailHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | mine_flail | 321 |
| `MusterAtlasPanel` | The Muster // Sector Currents & Coalition Camps | `MusterHostSession` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 537 |
| `MutationTreePanel` | Genetics // Mutation Trees & Instability | `MutationSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | mutation_tree | 112 |
| `NurseryPanel` | Nursery // Childhood & Schoolhouse | `GenerationalSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 2 | century_seed, nursery | 166 |
| `PersonalBelongingsPanel` | Personal Effects // Keepsakes, Favorites & Gifts | `PersonalBelongingsHostSession` | `shell:split-body` | 1000×650 | 3 | 1 | 5 | 5 | — | 315 |
| `PersonalQuestPanel` | Survivor Character Arcs // Personal Quests | `PersonalQuestHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | personal_quests | 227 |
| `PhantomMemoryPanel` | SYS: PHANTOM MEMORY & RELIC TRIGGERS // PSYCH MATRIX | `—` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | phantom_memory, standing_record | 335 |
| `GeothermalOrcPanel` | B74 // GEOTHERMAL ORC LOOP | `GeothermalOrcHostSession` | `shell:single-column` | 720×480 | 0 | 0 | 0 | 0 | — | 367 |
| `PlasticPyrolysisPanel` | RETORT BAY // WASTE PLASTIC RECLAMATION | `PlasticPyrolysisHostSession` | `shell:single-column` | 1000×650 | 0 | 1 | 0 | 0 | plastic_pyrolysis | 269 |
| `PoliticsUI` | Council Chamber // Settlement Politics & Law | `PoliticsSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | politics | 127 |
| `PrisonerPanel` | Detention // Prisoner Management & Intel | `PrisonerSystem` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 3 | prisoners | 190 |
| `PropagandaPanel` | Information Warfare // Propaganda & Influence | `PropagandaHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 0 | propaganda | 227 |
| `PsychologyArcPanel` | Psychological Arcs // Watch | `—` | `shell:single-column` | 1100×700 | 4 | 0 | 0 | 0 | — | 332 |
| `QuestsAtlasPanel` | Quests Atlas // Storyline & Protocol Progression | `—` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 431 |
| `RailGrindingPanel` | RAIL GRINDING & REPROFILING // STRATEGIC RAIL CORRIDOR CONTROL | `RailGrindingHostSession` | `shell:single-column` | 1100×720 | 4 | 0 | 0 | 0 | rail_grinding | 335 |
| `RailwayTerminalPanel` | THE LINE // RAILWAY TERMINAL | `RailwaySystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 243 |
| `RegionalTreatyPanel` | Regional Treaty // Diplomatic Accords | `RegionalTreatyHostSession` | `shell:single-column` | 1000×650 | 2 | 0 | 0 | 0 | regional_treaty | 161 |
| `RelationshipDecayPanel` | Survivor Social Ecology // Bonds & Social Drift | `RelationshipDecayHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | relationship_decay | 191 |
| `ResearchAtlasPanel` | Research Atlas // Knowledge Nodes · Breakthroughs · R&D Queue | `ResearchHostSession` | `shell:split-body` | 1280×720 | 6 | 1 | 0 | 1 | — | 492 |
| `RoboticsWorkshopPanel` | AWAKENED STEEL // ROBOTICS WORKSHOP | `RoboticsSystem` | `shell:single-column` | 1050×680 | 4 | 1 | 1 | 0 | — | 290 |
| `RumorBoardPanel` | Intelligence & Rumors // Wasteland Whispers & Intercepts | `RumorNetworkHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 1 | rumors | 241 |
| `RunFlatTirePanel` | Vehicle Shop // Run-Flat Wheel Set | `RunFlatTireHostSession` | `shell:single-column` | 1000×650 | 5 | 0 | 3 | 4 | runflat_tire | 249 |
| `SanitationPanel` | Waste & Sanitation // Shelter Hygiene | `SanitationHostSession` | `shell:single-column` | 900×580 | 4 | 0 | 0 | 0 | — | 142 |
| `ShelterAtmospherePanel` | Shelter Atmosphere & Ambiance // Environmental Character | `ShelterAtmosphereHostSession` | `shell:single-column` | 950×600 | 4 | 0 | 0 | 2 | shelter_atmosphere | 221 |
| `ShelterBarterPanel` | SHELTER AIRLOCK BARTER // CARAVAN TRADING ROUTE | `—` | `shell:split-body` | 1100×700 | 5 | 3 | 0 | 0 | — | 1142 |
| `ShelterDecorPanel` | Shelter Interior // Memorial Wall | `ShelterDecorHostSession` | `shell:single-column` | 1160×700 | 4 | 2 | 1 | 0 | — | 444 |
| `ShelterReputationPanel` | Shelter Reputation // External Perception & Notoriety | `ShelterReputationHostSession` | `shell:split-body` | 1000×650 | 4 | 1 | 0 | 0 | shelter_reputation | 329 |
| `ShelterSchedulePanel` | Shelter Schedule // Shift Assignment | `ShelterScheduleHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | shelter_schedule | 127 |
| `ShelterSecurityPanel` | Shelter Security // Access Control & Lockdown Protocols | `ShelterSecurityHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 5 | shelter_security | 304 |
| `ShelterThermalPanel` | Shelter Thermal // Central Heating | `ShelterThermalHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 0 | 0 | shelter_thermal | 126 |
| `SilentFoundryPanel` | The Silent Foundry // Cupola & Casting Bay | `—` | `shell:split-body` | 1100×720 | 8 | 0 | 0 | 0 | silent_foundry | 590 |
| `SkillMatrixPanel` | Survivor Skill Matrix // Progression & Decay | `—` | `shell:single-column` | 1100×720 | 6 | 0 | 0 | 0 | — | 500 |
| `SkyDefenseBatteryPanel` | SKY DEFENSE // COUNTER-BATTERY | `—` | `shell:single-column` | 1180×700 | 6 | 1 | 4 | 0 | sky_defense_battery | 636 |
| `SolidOxideFuelCellPanel` | Solid-Oxide Fuel Cell // Quiet Baseload | `SofcPowerHostSession` | `shell:single-column` | 900×600 | 4 | 0 | 0 | 0 | sofc_power | 146 |
| `SoundRangingPanel` | Acoustic Sound-Ranging // Siege Warning | `SoundRangingHostSession` | `shell:single-column` | 950×620 | 4 | 0 | 0 | 0 | sound_ranging | 154 |
| `StandingRecordAtlasPanel` | Standing Record Atlas // Ground Layouts · Memory Strata · Site Mutations | `StandingRecordHostSession` | `shell:split-body` | 1280×720 | 6 | 0 | 0 | 0 | — | 516 |
| `StealthReadoutPanel` | Stealth // Camouflage & Detection Readout | `StealthSystem` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | stealth | 114 |
| `SumpFloodingPanel` | SYS: SUBLEVEL SUMP & FLOOD CONTROL // MATRIX | `SumpFloodingHostSession` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | sump_flooding | 367 |
| `SurvivorDeathLegacyPanel` | Survivor Memorial & Wills // Death Records & Estates | `SurvivorDeathLegacyHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | death_legacy | 214 |
| `SurvivorDowntimePanel` | COMMON HOURS // HOBBIES & DOWNTIME | `SurvivorDowntimeSystem` | `shell:single-column` | 1000×650 | 4 | 1 | 0 | 0 | — | 229 |
| `SurvivorRelationsPanel` | Survivor Relations // Social Dynamics | `SurvivorRelationsHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 0 | 0 | survivor_relations | 198 |
| `TimeCapsulePanel` | Shelter Heritage // Time Capsules & Legacy Messages | `TimeCapsuleHostSession` | `shell:split-body` | 1000×650 | 3 | 2 | 0 | 0 | time_capsule | 215 |
| `TravelingCaravanPanel` | SYS: TRAVELING CARAVANS & REGIONAL TRADE // ROUTE RADAR | `—` | `shell:split-body` | 1040×680 | 5 | 3 | 0 | 0 | traveling_caravan | 466 |
| `VehicleGaragePanel` | VEHICLE GARAGE // OVERLAND MAINTENANCE | `—` | `shell:single-column` | 1180×700 | 4 | 1 | 3 | 0 | vehicle_garage | 636 |
| `VinylMoralePanel` | Common Room // Vinyl Turntable | `VinylMoraleHostSession` | `shell:single-column` | 1000×650 | 4 | 0 | 1 | 2 | vinyl_morale | 207 |
| `VisitorIntegrationPanel` | Visitor Integration // Temporary Residency & Processing | `VisitorIntegrationHostSession` | `shell:split-body` | 1000×650 | 4 | 2 | 0 | 8 | visitor_integration | 293 |
| `WaterTreatmentPanel` | Water Treatment // Filtration & Decon | `WaterTreatmentHostSession` | `shell:single-column` | 1000×650 | 5 | 0 | 0 | 0 | water_treatment | 143 |
| `WaystationNetworkPanel` | Waystation A // Forward Outpost | `WaystationHostSession` | `shell:split-body` | 1000×650 | 3 | 0 | 0 | 2 | waystation_network | 186 |
| `WildlifeTrappingPanel` | Wildlife Trapping // Snare Network | `WildlifeTrappingHostSession` | `shell:single-column` | 1000×650 | 3 | 0 | 1 | 6 | wildlife_trapping | 525 |
| `WinterFreezePanel` | DEEP WINTER // FREEZE WATCH | `YearOfAshDeepFreezeSystem` | `shell:single-column` | 950×620 | 4 | 1 | 0 | 0 | — | 189 |

## 7. Non-shell bindable surfaces

23 bindable panels render without the dashboard shell (scene-backed forms, sub-views, read-only rows):

| Panel | Binds | Pattern | Routes | LOC |
|---|---|---|---|---:|
| `BrineExtractionPanel` | `SilentFoundryHostSession` | `code-built` | — | 236 |
| `CombatDetailPanel` | `CombatHostSession` | `scene-backed` | combat, combat_detail | 160 |
| `CombatHistoryPanel` | `CombatHostSession` | `code-built` | combat, combat_detail | 198 |
| `CombatPanel` | `CombatHostSession` | `code-built` | combat, combat_detail | 475 |
| `DutyRosterDetailPanel` | `DutyRosterHostSession` | `scene-backed` | duty_roster, duty_roster_detail | 165 |
| `EmergencyResponseHud` | `CrisisPresentationSnapshot` | `scene-backed` | — | 282 |
| `EpiloguePanel` | `CampaignOutcomeSnapshot` | `code-built` | chronicle, epilogue | 252 |
| `EventsLogPanel` | `object` | `code-built` | event_detail, journal | 200 |
| `FactionDetailPanel` | `object` | `scene-backed` | — | 156 |
| `FactionMatrixPanel` | `IFactionStanceProvider` | `code-built` | — | 482 |
| `FeedbackPanel` | `IFeedbackService` | `code-built` | — | 235 |
| `GeothermalAquiferPanel` | `GeothermalAquiferHostSession` | `code-built` | — | 173 |
| `InventoryPanel` | `InventoryHostSession` | `code-built` | inventory, inventory_detail | 283 |
| `OnboardingHintPanel` | `OnboardingJourney` | `code-built` | help | 426 |
| `OpeningProtocolModal` | `StartingLevelHostSession` | `code-built` | protocol | 239 |
| `PowerGridPanel` | `PowerGridHostSession` | `code-built` | power_grid | 304 |
| `RadioPanel` | `RadioHostSession` | `code-built` | radio | 701 |
| `ReconTelemetryPanel` | `ReconTelemetryHostSession` | `code-built` | — | 160 |
| `SaveLoadPanel` | `SaveLoadHostSession` | `code-built` | — | 403 |
| `SurvivorsPanel` | `SurvivorsHostSession` | `code-built` | status, survivor_detail, survivors | 333 |
| `WeatherHistoryPanel` | `WeatherSystem` | `code-built` | — | 207 |
| `WeatherPanel` | `WeatherHostSession` | `code-built` | map, weather | 486 |
| `WeatherSondePanel` | `WeatherHostSession` | `code-built` | — | 202 |

## 8. Machine-readable output

Full token/route/panel JSON: `docs/ui/ui_design_map.json` (241 panels, 2 nav groups).

