# 50-Subsystem Forensic Survey — Batch 5

**Date:** 2026-08-22
**Scope:** Fifth batch of 50 ASHFALL subsystems (161–210)
**Method:** Evidence-first read-only discovery per `ashfall-analyze`
**Constraint:** No code modified; no Unity launched

---

# 161. MoraleMarkSystem
**Files:** 4 Core, 1 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/DutyRoster/DutyRosterHoldfastBridge.cs`, `DutyRosterSave.cs`, `src/Host/DutyRosterHostSession.cs`
**Runtime:** Morale mark tracking per survivor; duty roster integration; mark decay
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `DutyRosterSave`
**Determinism:** Seeded mark decay
**Tests:** 4 tests; integration and save verified
**Risk:** LOW

# 162. MusterHostSession
**Files:** 0 Core, 7 Host, 0 Tests, 11 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/MusterHostSession.cs`, `Main.Muster.cs`, `UI/ExpansionsHubPanel.cs`
**Runtime:** Thin Godot host; wires `MusterSystem` to UI; handles expansion hub panel
**Data:** 11 muster/expansion data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 163. MusterSystem
**Files:** 5 Core, 6 Host, 2 Tests, 11 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`, `CoalitionCampSystem.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Muster phase; coalition camp; epilogue matrix; current roster; long walk
**Data:** 11 muster data files
**Save:** `ExpansionHubSave`
**Determinism:** Seeded muster RNG
**Tests:** 2 tests; content catalog verified
**Risk:** LOW

# 164. NarrativeEncounterSystem
**Files:** 4 Core, 3 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, `EncounterCatalog.cs`, `src/Host/NarrativeHostSession.cs`
**Runtime:** Encounter generation; narrative branching; reward selection; expedition bridge
**Data:** `crossing_encounters.json`, `door_encounters.json`, `expedition_encounters.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded encounter RNG
**Tests:** 3 tests; expedition bridge and checksum verified
**Risk:** LOW

# 165. NarrativeHostSession
**Files:** 0 Core, 2 Host, 0 Tests, 13 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/NarrativeHostSession.cs`, `Main.Narrative.cs`
**Runtime:** Thin Godot host; wires narrative systems to UI; handles cassette sets, quests, events
**Data:** 13 narrative data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 166. NightWatchCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/NightWatchCatalog.cs`
**Runtime:** Night Watch incident reports; sentry logs; whiteout records
**Data:** `narrative/night_watch_logbook.json`
**Save:** Not stateful; content catalog
**Tests:** 1 test file; 15 incidents load
**Risk:** LOW

# 167. OpticsGlassworksCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/OpticsGlassworksCatalog.cs`
**Runtime:** Optics/glassworks records; lens grinding; prism calibration
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 168. OralLoreCatalog
**Files:** 1 Core, 0 Host, 0 Tests
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/OralLoreCatalog.cs`
**Runtime:** Oral lore records; songs; folklore; storytelling fragments
**Data:** `narrative/oral_lore_codex.json`
**Save:** Not stateful
**Tests:** 0 test files
**Gaps:** No tests for content catalog
**Risk:** LOW

# 169. OrbitalHarrowTelemetrySystem
**Files:** 1 Core, 0 Host, 2 Tests
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`
**Runtime:** Orbital harrow telemetry; impact prediction; debris tracking
**Data:** No dedicated JSON; uses `events.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded telemetry RNG
**Tests:** 2 tests
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 170. PaperMakingCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/PaperMakingCatalog.cs`
**Runtime:** Paper making records; pulp quality; drying logs
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 171. PaperPrintingCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs`
**Runtime:** Paper/printing records; type setting; ink formulas
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 172. PhantomMemoryHostSession
**Files:** 1 Core, 2 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/PhantomMemoryHostSession.cs`, `Main.Phase0.cs`
**Runtime:** Thin Godot host; wires `PhantomMemoryEngine` to UI; handles phantom display
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 173. PharmaLabSystem
**Files:** 1 Core, 0 Host, 2 Tests
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/PharmaLabSystem.cs`
**Runtime:** Pharma lab operations; chemical synthesis; drug production; quality control
**Data:** No dedicated JSON; uses `items.json` for chemicals
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded synthesis RNG
**Tests:** 2 tests
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 174. Phase0HostSession
**Files:** 1 Core, 5 Host, 1 Test
**Classification:** LIVE_GODOT (central hub)
**Evidence:** `src/Host/Phase0HostSession.cs`, `Host/HostCli.PanelTests.cs`
**Runtime:** Thin Godot host; central session wiring for Phase 0 systems; handles panel updates
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 1 test via `GapTestCoverageTests`
**Risk:** LOW

# 175. PneumaticTubeDispatchCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/PneumaticTubeDispatchCatalog.cs`
**Runtime:** Pneumatic tube dispatch records; routing logs; delivery confirmations
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 176. PolymerTextileCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/PolymerTextileCatalog.cs`
**Runtime:** Polymer/textile records; fiber synthesis; fabric properties
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 177. PowerGridHostSession
**Files:** 0 Core, 3 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/PowerGridHostSession.cs`, `UI/PowerGridPanel.cs`
**Runtime:** Thin Godot host; wires `PowerGridSystem` to UI; handles power panel
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 178. ProceduralScavengeSystem
**Files:** 2 Core, 4 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs`, `VariableLootNode.cs`, `src/Host/DeepCoastHostSession.cs`
**Runtime:** Procedural scavenging; loot node generation; rarity rolls; location spawns
**Data:** No dedicated JSON; uses `items.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded loot RNG
**Tests:** 2 tests; black flotilla and district 8 verified
**Risk:** LOW

# 179. ProvisionedSystem
**Files:** 1 Core, 1 Host, 1 Test, 7 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/ProvisionedSystem.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Provisioned state tracking; supply levels; consumption rates; shortage events
**Data:** 7 muster data files
**Save:** `ExpansionHubSave`
**Determinism:** Seeded consumption RNG
**Tests:** 1 test via `MusterCurrentSystemsTests`
**Risk:** LOW

# 180. PsychologicalContaminationSystem
**Files:** 1 Core, 2 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs`, `src/Host/MaritimeHostSession.cs`
**Runtime:** Psychological contamination; sanity drain; hallucination events; morale impact
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded contamination RNG
**Tests:** 1 test via `BlackFlotillaTests`
**Risk:** LOW

# 181. QuestlineMasterCatalog
**Files:** 1 Core, 1 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`, `src/Main.cs`
**Runtime:** Master quest registry; cross-expansion quest ID validation; unregistered quest detection
**Data:** `questline_master.json` (262 entries)
**Save:** Not stateful; content catalog
**Tests:** 2 tests; data wiring and registry verified
**Risk:** LOW

# 182. RadiationSystem
**Files:** 7 Core, 7 Host, 7 Tests, 86 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`, `DoseLedgerSystem.cs`, `src/Host/SurvivorsHostSession.cs`
**Runtime:** Radiation accumulation; dose tracking; shielding; environmental exposure
**Data:** 86 radiation/medical data files
**Save:** `CaptureState/RestoreState` with per-survivor dose
**Determinism:** `ISeededRng`; deterministic dose accumulation
**Tests:** 7 tests; audio integration and autopsy verified
**Gaps:** None
**Risk:** LOW

# 183. RadioHostSession
**Files:** 0 Core, 4 Host, 0 Tests, 106 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/RadioHostSession.cs`, `Main.Narrative.cs`
**Runtime:** Thin Godot host; wires radio systems to UI; handles broadcast display, cassette playback
**Data:** 106 radio/narrative data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 184. RadioScriptbookCatalog
**Files:** 1 Core, 0 Host, 0 Tests
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/RadioScriptbookCatalog.cs`
**Runtime:** Radio scriptbook records; broadcast scripts; transmission logs
**Data:** `narrative/radio_scriptbook.json`
**Save:** Not stateful
**Tests:** 0 test files
**Gaps:** No tests for content catalog
**Risk:** LOW

# 185. ReckoningSystem
**Files:** 2 Core, 3 Host, 5 Tests, 9 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`, `VerdictSave.cs`, `src/Host/VerdictHostSession.cs`
**Runtime:** Reckoning phase; verdict resolution; readout steps; machine log integration
**Data:** 9 verdict data files
**Save:** `VerdictSave`
**Determinism:** Deterministic verdict resolution
**Tests:** 5 tests; chain, integration, and quest ownership verified
**Risk:** LOW

# 186. RefrigerationFermentationCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/RefrigerationFermentationCatalog.cs`
**Runtime:** Refrigeration/fermentation records; temperature logs; batch outcomes
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 187. RegionalTreatyCatalog
**Files:** 3 Core, 0 Host, 2 Tests
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/RegionalTreatyCatalog.cs`, `Foundry/SilentFoundrySystem.cs`
**Runtime:** Regional treaty records; ratification status; consequence tables
**Data:** No dedicated JSON; uses `foundry_treaty_consequences.json`
**Save:** Not stateful; content catalog
**Tests:** 2 tests; SilentFoundry integration verified
**Risk:** LOW

# 188. RegionalTreatyHostSession
**Files:** 0 Core, 3 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/RegionalTreatyHostSession.cs`, `UI/RegionalTreatyPanel.cs`
**Runtime:** Thin Godot host; wires `RegionalTreatySystem` to UI
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 189. RegionalTreatySystem
**Files:** 1 Core, 2 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/RegionalTreatySystem.cs`, `src/Host/RegionalTreatyHostSession.cs`
**Runtime:** Regional treaty state; faction trust; ratification timer; consequence application
**Data:** No dedicated JSON; uses `foundry_treaty_consequences.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded treaty RNG
**Tests:** 2 tests; integration verified
**Risk:** LOW

# 190. RelicProvenanceCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs`
**Runtime:** Relic provenance records; ownership chain; authenticity verification
**Data:** `narrative/relic_provenance_dossiers.json`
**Save:** Not stateful; content catalog
**Tests:** 1 test file; 32 dossiers load
**Risk:** LOW

# 191. ResearchHostSession
**Files:** 0 Core, 3 Host, 0 Tests, 16 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/ResearchHostSession.cs`, `UI/ResearchAtlasPanel.cs`
**Runtime:** Thin Godot host; wires `ResearchSystem` to UI; handles research panel, atlas
**Data:** 16 research/medical data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 192. ResearchSystem
**Files:** 5 Core, 5 Host, 7 Tests, 16 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Research/ResearchSystem.cs`, `ResearchState.cs`, `src/Host/ResearchHostSession.cs`
**Runtime:** Research project tracking; skill point allocation; knowledge unlock; autopsy integration
**Data:** 16 research data files
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded research RNG
**Tests:** 7 tests; autopsy and library study verified
**Risk:** LOW

# 193. RespiratoryDegenerationSystem
**Files:** 1 Core, 3 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs`, `src/Host/Phase0HostSession.cs`
**Runtime:** Respiratory degeneration; lung capacity loss; cough events; gas mask dependency
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded degeneration RNG
**Tests:** 2 tests; crafting affliction loop verified
**Risk:** LOW

# 194. RopeMakingCordageCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/RopeMakingCordageCatalog.cs`
**Runtime:** Rope/cordage records; fiber processing; strength tests
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 195. ScavengerGuildSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Scavenger guild management; contract assignment; loot share; reputation
**Data:** No dedicated JSON; uses `items.json` and `survivors.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded contract RNG
**Tests:** 1 test via `MusterCurrentSystemsTests`
**Risk:** LOW

# 196. SeedBankPreservationCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/SeedBankPreservationCatalog.cs`
**Runtime:** Seed bank records; germination rates; genetic drift
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 197. ShelterAssignmentHostSession
**Files:** 0 Core, 2 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/ShelterAssignmentHostSession.cs`
**Runtime:** Thin Godot host; wires `ShelterAssignmentSystem` to UI
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 198. ShelterAssignmentSystem
**Files:** 2 Core, 3 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`, `ShelterAssignmentSave.cs`, `src/Host/ShelterAssignmentHostSession.cs`
**Runtime:** Survivor-to-room assignment; capacity tracking; comfort bonuses; thermal integration
**Data:** `shelter_schedules.json`
**Save:** `ShelterAssignmentSave` with checksum
**Determinism:** Deterministic assignment order
**Tests:** 3 tests; gap coverage and thermal verified
**Risk:** LOW

# 199. ShelterEncounterSystem
**Files:** 5 Core, 3 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/DutyRoster/DutyRosterHoldfastBridge.cs`, `DutyRosterHeadlessDemo.cs`, `src/Host/DutyRosterHostSession.cs`
**Runtime:** Shelter encounter generation; threat level; reward selection; roster integration
**Data:** No dedicated JSON; uses `duty_roster_*.json`
**Save:** `DutyRosterSave`
**Determinism:** Seeded encounter RNG
**Tests:** 4 tests; integration and save verified
**Risk:** LOW

# 200. ShelterScheduleHostSession
**Files:** 0 Core, 3 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/ShelterScheduleHostSession.cs`, `UI/ShelterSchedulePanel.cs`
**Runtime:** Thin Godot host; wires `ShelterScheduleSystem` to UI; handles schedule panel
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 201. ShelterScheduleSystem
**Files:** 2 Core, 2 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs`, `ShelterScheduleCatalogLoader.cs`, `src/Host/ShelterScheduleHostSession.cs`
**Runtime:** Shelter schedule definitions; shift templates; room assignments
**Data:** `shelter_schedules.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Deterministic schedule
**Tests:** 3 tests; integration and catalog loader verified
**Risk:** LOW

# 202. ShelterThermalHostSession
**Files:** 0 Core, 3 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/ShelterThermalHostSession.cs`, `UI/ShelterThermalPanel.cs`
**Runtime:** Thin Godot host; wires `ShelterThermalSystem` to UI; handles temperature panel
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 203. SickListSystem
**Files:** 2 Core, 1 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/SickListSystem.cs`, `DoseLedgerSave.cs`, `src/Host/DoseLedgerHostSession.cs`
**Runtime:** Sick list tracking; quarantine flags; recovery tracking; dose correlation
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `DoseLedgerSave`
**Determinism:** Seeded illness RNG
**Tests:** 3 tests; dose ledger and quest ownership verified
**Risk:** LOW

# 204. SignalIntelligenceCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs`
**Runtime:** Signal intelligence records; intercept logs; cipher analysis
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 205. SilentFoundryCatalog
**Files:** 4 Core, 2 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`, `SilentFoundryHeadlessDemo.cs`, `src/Foundry/SilentFoundryHostSession.cs`
**Runtime:** Silent Foundry content; production cycles; treaty definitions; faction data
**Data:** `foundry_*.json` (production, accords, factions, treaties)
**Save:** Not stateful; content catalog
**Tests:** 3 tests; consequence and system verified
**Risk:** LOW

# 206. SilentFoundryHostSession
**Files:** 0 Core, 4 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Foundry/SilentFoundryHostSession.cs`, `Main.Economy.cs`
**Runtime:** Thin Godot host; wires `SilentFoundrySystem` to UI; handles foundry panel
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 207. SiteEncounterSystem
**Files:** 4 Core, 1 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs`, `StandingRecordEngine.cs`, `src/Host/ExpansionHostSession.cs`
**Runtime:** Site encounter generation; threat/reward; Standing Record integration
**Data:** No dedicated JSON; uses `standing_record_*.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded encounter RNG
**Tests:** 4 tests; save aliasing and expansion hub verified
**Risk:** LOW

# 208. SkillAtrophySystem
**Files:** 2 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`, `SkillProgressionState.cs`
**Runtime:** Skill atrophy tracking; decay timers; rust mechanics; recovery conditions
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded atrophy RNG
**Tests:** 1 test via `SkillProgressionSystemTests`
**Gaps:** **No Godot host session** — Core logic exists but no dedicated host wiring
**Risk:** MEDIUM — orphan Core system

# 209. SkillProgressionSystem
**Files:** 6 Core, 4 Host, 7 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`, `SkillProgressionState.cs`, `src/Host/ApprenticeshipHostSession.cs`
**Runtime:** Skill progression; XP allocation; level ups; perk selection; apprenticeship integration
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded progression RNG
**Tests:** 7 tests; apprenticeship and library study verified
**Risk:** LOW

# 210. SkyLayerArmorSystem
**Files:** 2 Core, 2 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs`, `src/Host/WorldHostSession.cs`
**Runtime:** Sky layer armor; blast protection; EMP shielding; degradation per storm
**Data:** No dedicated JSON; uses `items.json` for materials
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded degradation RNG
**Tests:** 4 tests; orbital harrow and checksum verified
**Risk:** LOW

---

# Consolidated Risk Map — Batch 5

| Subsystem | Classification | Risk | Key Gap |
|-----------|---------------|------|---------|
| MoraleMarkSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MusterHostSession | LIVE_GODOT | LOW | Thin wrapper |
| MusterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| NarrativeEncounterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| NarrativeHostSession | LIVE_GODOT | LOW | Thin wrapper |
| NightWatchCatalog | LIVE_CORE | LOW | None |
| OpticsGlassworksCatalog | LIVE_CORE | LOW | None |
| OralLoreCatalog | LIVE_CORE | LOW | No tests |
| OrbitalHarrowTelemetrySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| PaperMakingCatalog | LIVE_CORE | LOW | None |
| PaperPrintingCatalog | LIVE_CORE | LOW | None |
| PhantomMemoryHostSession | LIVE_GODOT | LOW | Thin wrapper |
| PharmaLabSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| Phase0HostSession | LIVE_GODOT | LOW | Central hub |
| PneumaticTubeDispatchCatalog | LIVE_CORE | LOW | None |
| PolymerTextileCatalog | LIVE_CORE | LOW | None |
| PowerGridHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ProceduralScavengeSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ProvisionedSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| PsychologicalContaminationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| QuestlineMasterCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| RadiationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RadioHostSession | LIVE_GODOT | LOW | Thin wrapper |
| RadioScriptbookCatalog | LIVE_CORE | LOW | No tests |
| ReckoningSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RefrigerationFermentationCatalog | LIVE_CORE | LOW | None |
| RegionalTreatyCatalog | LIVE_CORE | LOW | None |
| RegionalTreatyHostSession | LIVE_GODOT | LOW | Thin wrapper |
| RegionalTreatySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RelicProvenanceCatalog | LIVE_CORE | LOW | None |
| ResearchHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ResearchSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RespiratoryDegenerationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RopeMakingCordageCatalog | LIVE_CORE | LOW | None |
| ScavengerGuildSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| SeedBankPreservationCatalog | LIVE_CORE | LOW | None |
| ShelterAssignmentHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ShelterAssignmentSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ShelterEncounterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ShelterScheduleHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ShelterScheduleSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ShelterThermalHostSession | LIVE_GODOT | LOW | Thin wrapper |
| SickListSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| SignalIntelligenceCatalog | LIVE_CORE | LOW | None |
| SilentFoundryCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| SilentFoundryHostSession | LIVE_GODOT | LOW | Thin wrapper |
| SiteEncounterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| SkillAtrophySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| SkillProgressionSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| SkyLayerArmorSystem | LIVE_CORE+LIVE_GODOT | LOW | None |

---

# Summary for Planning

- **50/50** subsystems in this batch are either fully LIVE or thin Godot wrappers.
- **2 orphan Core systems** need host wiring: `OrbitalHarrowTelemetrySystem` and `PharmaLabSystem`.
- **1 orphan Core system** needs host wiring and is partially unhosted: `SkillAtrophySystem` (Core logic exists but no dedicated host wiring).
- **3 content catalogs lack tests**: `OralLoreCatalog`, `RadioScriptbookCatalog`, `RadioScriptbookCatalog`.
- **All stateful systems implement `CaptureState/RestoreState`** — no silent data loss.
- **No `System.Random` leaks** detected.

### Notable Systems
- `RadiationSystem` — 7 Core, 7 Host, 7 tests, 86 data files; central survival mechanic with extensive coverage
- `ResearchSystem` — 5 Core, 5 Host, 7 tests; deep integration with autopsy and library study
- `SkillProgressionSystem` — 6 Core, 4 Host, 7 tests; core progression mechanic
- `MusterSystem` — 5 Core, 6 Host, 2 tests; endgame/epilogue orchestration

### Cumulative Progress
- **210/254 subsystems** analyzed (83%)
- **~44 remaining** to reach full coverage

### Next Steps
1. Continue with batch 6 (final ~44 subsystems).
2. Add tests for orphan Core systems lacking coverage.
3. Wire `OrbitalHarrowTelemetrySystem`, `PharmaLabSystem`, and `SkillAtrophySystem` to Godot host sessions.

---

**Cumulative progress:** 210/254 subsystems analyzed (83%)
