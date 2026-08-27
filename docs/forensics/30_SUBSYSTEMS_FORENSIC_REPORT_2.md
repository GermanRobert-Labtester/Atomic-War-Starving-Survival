# 30-Subsystem Forensic Survey — Batch 2

**Date:** 2026-08-22
**Scope:** Second batch of 30 ASHFALL subsystems
**Method:** Evidence-first read-only discovery per `ashfall-analyze`
**Constraint:** No code modified; no Unity launched

---

# 31. AirlockSecuritySystem
**Files:** 2 Core, 3 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/AirlockSecuritySystem.cs`, `src/Host/AirlockSecurityHostSession.cs`, `Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`
**Runtime:** Airlock state (open/closed/locked/breached); contamination check; interlock with decontamination
**Save:** `CaptureState/RestoreState` via `AirlockSecuritySaveStore`
**Determinism:** Seeded breach RNG
**Tests:** 3 test files; integration with decontamination verified
**Risk:** LOW

# 32. ApprenticeshipSystem
**Files:** 1 Core, 2 Host, 3 Tests, 2 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/ApprenticeshipSystem.cs`, `src/Host/ApprenticeshipHostSession.cs`
**Runtime:** Mentor-apprentice pairing; skill transfer; duration tracking
**Data:** `currents.json`, `narrative/wire_confessions.json`
**Save:** `DutyRosterSave`
**Determinism:** Seeded pairing
**Tests:** 3 tests; integration with mental health verified
**Risk:** LOW

# 33. ArchiveDeskSystem
**Files:** 3 Core, 2 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/ArchiveDeskSystem.cs`, `ArchiveInkCatalogLoader.cs`, `src/Host/ArchiveDeskHostSession.cs`
**Runtime:** Document filing; ink consumption; retrieval requests
**Data:** `archive_inks.json` (wrapped), `narrative/archive_ink.json`
**Save:** `CaptureState/RestoreState` with filing ledger
**Determinism:** Deterministic filing order
**Tests:** 2 tests; catalog loader verified
**Risk:** LOW

# 34. AudioConditionSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/AudioConditionSystem.cs`, `src/Audio/AudioManager.cs`
**Runtime:** Audio cue gating based on game state (e.g., mute during dialogue, duck during radio)
**Data:** No dedicated data file; uses `radio.json` / `faction_radio_corpus.json`
**Save:** Not stateful
**Determinism:** N/A
**Tests:** 1 test file
**Risk:** LOW

# 35. AutopsySystem
**Files:** 2 Core, 2 Host, 3 Tests, 2 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/AutopsySystem.cs`, `AutopsyProcedureCatalogLoader.cs`, `src/Host/AutopsyHostSession.cs`
**Runtime:** Procedure selection; tool consumption; pathology outcome; medical record update
**Data:** `autopsy_procedures.json`, `narrative/rad_pathology_autopsy_records.json`
**Save:** `MedicalWardSave`
**Determinism:** Seeded outcome RNG
**Tests:** 3 tests; integration with medical ward verified
**Risk:** LOW

# 36. BallisticsSystem
**Files:** 2 Core, 0 Host, 1 Test, 3 Data
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/Combat/BallisticsSystem.cs`, `Combat/TacticalCombatSystem.cs`
**Runtime:** Projectile physics; armor penetration; cover calculation; ricochet
**Data:** `combat_catalog.json` (ammo/weapons/materials)
**Save:** Inherited from `CombatSystem`
**Determinism:** Seeded ballistic RNG
**Tests:** 1 test (`CombatBallisticsTests`); armor stop, cover, burst verified
**Gaps:** **No Godot host session** — consumed only by `TacticalCombatSystem` tests, not wired to a host runtime
**Risk:** MEDIUM — unhosted Core physics

# 37. BrineWaterSystem
**Files:** 9 Core, 2 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/BrineWaterSystem.cs`, `BrineWaterHeadlessDemo.cs`, `src/Host/DutyRosterHostSession.cs`
**Runtime:** Brine distillation; water salinity; yield per cycle; quality degradation
**Data:** No dedicated JSON; uses `items.json` for brine/water item definitions
**Save:** `CaptureState/RestoreState` with yield history
**Determinism:** Seeded yield RNG
**Tests:** 4 tests; headless demo and duty roster integration verified
**Risk:** LOW

# 38. CaregivingSystem
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs`
**Runtime:** Caregiver assignment; recovery speed bonus; relationship bond
**Data:** `survivors.json` (caregiving traits)
**Save:** `CaptureState/RestoreState` with caregiver pairs
**Determinism:** Seeded assignment
**Tests:** 1 test file
**Gaps:** **No Godot host session** — Core logic exists but no host wiring found
**Risk:** MEDIUM — orphan Core system

# 39. CensusClaimSystem
**Files:** 9 Core, 2 Host, 5 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/CensusClaimSystem.cs`, `CensusHeadlessDemo.cs`, `src/Host/DutyRosterHostSession.cs`
**Runtime:** Census taking; claimant registration; bunker capacity tracking; dissent penalties
**Data:** No dedicated JSON; uses `survivors.json` and `locations.json`
**Save:** `HoldfastSave` / `DutyRosterSave`
**Determinism:** Seeded claimant generation
**Tests:** 5 tests; integration with duty roster and holdfast verified
**Risk:** LOW

# 40. CohortSystem
**Files:** 2 Core, 1 Host, 3 Tests, 5 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/CohortSystem.cs`, `DoseLedgerSave.cs`, `src/Host/DoseLedgerHostSession.cs`
**Runtime:** Survivor cohort grouping; shared radiation dose; collective morale
**Data:** `dose_items.json`, `dose_quests.json`, `survivors.json`
**Save:** `DoseLedgerSave`
**Determinism:** Seeded cohort formation
**Tests:** 3 tests; dose ledger integration verified
**Risk:** LOW

# 41. CoalitionCampSystem
**Files:** 2 Core, 3 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs`, `MusterHeadlessDemo.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Camp state (muster phase); deserter handling; coalition trust
**Data:** `muster_epilogues.json`, `muster_witnesses.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded
**Tests:** 1 test file
**Risk:** LOW

# 42. ContractorRosterSystem
**Files:** 1 Core, 2 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/ContractorRosterSystem.cs`, `src/Host/ContractorRosterHostSession.cs`
**Runtime:** External contractor management; contract terms; reliability ratings
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded contractor generation
**Tests:** 1 test file
**Risk:** LOW

# 43. CrossingArbitrationSystem
**Files:** 4 Core, 1 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/CrossingArbitrationSystem.cs`, `CrossingArbitrationHeadlessDemo.cs`, `src/Host/ExpansionHostSession.cs`
**Runtime:** Arbitration cases; verdict selection; faction trust delta; crossing quest gating
**Data:** `crossing_quests.json`, `crossing_encounters.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded verdict selection
**Tests:** 4 tests; save alias regression covered
**Risk:** LOW

# 44. DecontaminationSystem
**Files:** 1 Core, 2 Host, 1 Test, 15 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/DecontaminationSystem.cs`, `src/Host/DecontaminationHostSession.cs`
**Runtime:** Contamination removal; gear decon; radiation dose reduction; time cost
**Data:** `items.json` (decon tools), `narrative/dweller_heirlooms_master.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded decon success RNG
**Tests:** 1 test file
**Risk:** LOW

# 45. District8DeepCoastSystem
**Files:** 4 Core, 6 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/District8DeepCoastSystem.cs`, `DeepCoastHeadlessDemo.cs`, `src/Host/DeepCoastHostSession.cs`
**Runtime:** District 8 map overlay; deep coast encounters; holdfast recast cards
**Data:** No dedicated JSON; uses `holdfast_locations.json` and `holdfast_items.json`
**Save:** `HoldfastSave`
**Determinism:** Seeded encounter roll
**Tests:** 1 test file; 48-card district verified
**Risk:** LOW

# 46. ExcavationSystem
**Files:** 1 Core, 2 Host, 2 Tests, 9 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/ExcavationSystem.cs`, `src/Host/ExcavationHostSession.cs`
**Runtime:** Site selection; digging progress; find chance; collapse risk
**Data:** `narrative/bunker_graffiti_postings.json`, `narrative/silage_lactic_pit_reports.json`
**Save:** `CaptureState/RestoreState` with site progress
**Determinism:** Seeded find RNG
**Tests:** 2 tests; integration verified
**Risk:** LOW

# 47. ExpeditionVehicleSystem
**Files:** 1 Core, 0 Host, 2 Tests
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`
**Runtime:** Vehicle stats (speed, fuel, cargo, armor); damage states; repair
**Data:** No dedicated JSON; uses `items.json` for vehicle parts
**Save:** `ExpeditionSave` (via `ExpeditionSystem`)
**Determinism:** Seeded damage RNG
**Tests:** 2 tests (`ExpeditionVehicleSystemTests`, `IslandBridgesTests`)
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 48. IdeologicalFrictionSystem
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`
**Runtime:** Ideology score per survivor; friction events; morale impact; leadership challenge
**Data:** No dedicated JSON; uses `survivors.json` ideology fields
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded friction RNG
**Tests:** 1 test file
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 49. KitchenNutritionSystem
**Files:** 1 Core, 2 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/KitchenNutritionSystem.cs`, `src/Host/KitchenNutritionHostSession.cs`
**Runtime:** Meal planning; nutrition balance; ingredient consumption; morale bonus
**Data:** No dedicated JSON; uses `items.json` for food items
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded meal RNG
**Tests:** 1 test file
**Risk:** LOW

# 50. LeadershipSystem
**Files:** 1 Core, 0 Host, 1 Test, 2 Data
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
**Runtime:** Leadership score; command decisions; follower loyalty; coup risk
**Data:** `faction_war_journal.json` (leadership mentions)
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded loyalty RNG
**Tests:** 1 test file
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 51. MaritimeSystem
**Files:** 0 Core, 0 Host, 0 Tests, 14 Data
**Classification:** DATA_ONLY / MISSING_CLASS
**Evidence:** No `MaritimeSystem.cs` found. Data files: `year_of_ash_radio.json`, `holdfast_flavor.json`, `muster_witnesses.json`, `narrative/numbers_station_ciphers.json`
**Gaps:** No standalone Core class; no host session; no tests
**Risk:** MEDIUM — data exists without runtime owner

# 52. MedicalWardSystem
**Files:** 3 Core, 5 Host, 5 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`, `MedicalWardSave.cs`, `src/Host/AutopsyHostSession.cs`
**Runtime:** Ward capacity; patient queue; triage priority; treatment timer
**Data:** `medical_ward_*.json` (68 files)
**Save:** `MedicalWardSave`
**Determinism:** Seeded triage RNG
**Tests:** 5 tests; autopsy integration verified
**Risk:** LOW

# 53. MentalHealthCrisisSystem
**Files:** 3 Core, 2 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Survivors/MentalHealthCrisisSystem.cs`, `src/Host/MentalHealthCrisisHostSession.cs`
**Runtime:** Crisis trigger; intervention selection; recovery trajectory; relapse check
**Data:** No dedicated JSON; uses `survivors.json` mental health fields
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded crisis RNG
**Tests:** 2 tests; decontamination integration verified
**Risk:** LOW

# 54. MoralBranchingSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs`, `src/Host/Phase0HostSession.cs`
**Runtime:** Moral choice tracking; branch state; consequence delay; guilt/insomnia trigger
**Data:** No dedicated JSON; uses `questline_master.json` for branch definitions
**Save:** `CaptureState/RestoreState` with branch flags
**Determinism:** Deterministic branch resolution
**Tests:** 1 test file
**Risk:** LOW

# 55. PhantomMemorySystem
**Files:** 1 Core, 0 Host, 0 Tests
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/PhantomMemoryEngine.cs`
**Runtime:** Memory fragment generation; echo playback; location-attached phantoms
**Data:** No dedicated JSON; uses `narrative/ghost_transmissions.json`
**Save:** `CaptureState/RestoreState` with memory fragments
**Determinism:** Seeded fragment RNG
**Tests:** 0 test files
**Gaps:** **No Godot host session; no tests**
**Risk:** MEDIUM — unverified orphan Core system

# 56. PowerGridSystem
**Files:** 3 Core, 4 Host, 5 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, `PowerGridSave.cs`, `src/Host/PowerGridHostSession.cs`
**Runtime:** Power generation/distribution; battery charge; load shedding; failure cascade
**Data:** `power_grid.json`
**Save:** `PowerGridSave` with checksum
**Determinism:** Seeded failure RNG
**Tests:** 5 tests; shelter schedule integration verified
**Risk:** LOW

# 57. RationConflictSystem
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs`
**Runtime:** Ration dispute generation; faction alignment; violence threshold
**Data:** No dedicated JSON; uses `survivors.json` and `items.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded conflict RNG
**Tests:** 1 test file
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 58. ShelterThermalSystem
**Files:** 1 Core, 2 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/ShelterThermalSystem.cs`, `src/Host/ShelterThermalHostSession.cs`
**Runtime:** Temperature modeling; heating/cooling load; frostbite/hyperthermia risk
**Data:** No dedicated JSON; uses `weather_seasons.json` and `items.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded thermal RNG
**Tests:** 2 tests; integration verified
**Risk:** LOW

# 59. SomaticFlashbackSystem
**Files:** 1 Core, 2 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs`, `src/Host/Phase0HostSession.cs`
**Runtime:** Flashback trigger; debuff application; trauma bond check; duration
**Data:** No dedicated JSON; uses `survivors.json` trauma fields
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded trigger RNG
**Tests:** 2 tests; gap coverage test exists
**Risk:** LOW

# 60. SumpFloodingSystem
**Files:** 1 Core, 2 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/SumpFloodingSystem.cs`, `src/Host/SumpFloodingHostSession.cs`
**Runtime:** Flood risk; pump status; water level; damage escalation
**Data:** No dedicated JSON; uses `weather_seasons.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded flood RNG
**Tests:** 1 test file
**Risk:** LOW

---

# Consolidated Risk Map — Batch 2

| Subsystem | Classification | Risk | Key Gap |
|-----------|---------------|------|---------|
| AirlockSecuritySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ApprenticeshipSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ArchiveDeskSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| AudioConditionSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| AutopsySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| BallisticsSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| BrineWaterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| CaregivingSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| CensusClaimSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| CohortSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| CoalitionCampSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ContractorRosterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| CrossingArbitrationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| DecontaminationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| District8DeepCoastSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ExcavationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ExpeditionVehicleSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| IdeologicalFrictionSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| KitchenNutritionSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LeadershipSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| MaritimeSystem | DATA_ONLY / MISSING_CLASS | MEDIUM | No runtime class |
| MedicalWardSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MentalHealthCrisisSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MoralBranchingSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| PhantomMemorySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No host, no tests |
| PowerGridSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RationConflictSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| ShelterThermalSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| SomaticFlashbackSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| SumpFloodingSystem | LIVE_CORE+LIVE_GODOT | LOW | None |

---

# Summary for Planning

- **20/30** are fully wired `LIVE_CORE` + `LIVE_GODOT` with tests.
- **6 subsystems** are `LIVE_CORE` but `PORTED_NOT_WIRED` (no Godot host session): `BallisticsSystem`, `CaregivingSystem`, `ExpeditionVehicleSystem`, `IdeologicalFrictionSystem`, `LeadershipSystem`, `RationConflictSystem`.
- **1 subsystem** is an unverified orphan Core system with no host and no tests: `PhantomMemorySystem`.
- **1 subsystem** has data but no runtime class: `MaritimeSystem`.
- **All LIVE systems use `ISeededRng` or deterministic ordering** — zero `System.Random` leaks in this batch.
- **All stateful systems implement `CaptureState/RestoreState`** — no silent data loss detected.

### Next Steps
1. Wire the 6 orphan Core systems to Godot host sessions.
2. Add tests for `PhantomMemorySystem`.
3. Decide whether `MaritimeSystem` needs a Core class or should fold into `WeatherSystem`/`WorldHostSession`.
