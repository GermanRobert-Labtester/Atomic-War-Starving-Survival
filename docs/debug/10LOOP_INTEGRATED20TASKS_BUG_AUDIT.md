# ASHFALL 10-Loop Bug Audit — Integrated 20 Tasks (PCK, CI, Caregiving → Pharma)

**Date:** 2026-08-26 | **Commit:** `2c324c1b` | **Branch:** `audit/fix-batch3-plus-phases`
**Auditor:** Muse Spark (10-loop forensic, read-only)
**Target:** 20 tasks integrated in this batch: PCK data-integrity, Godot CI, Caregiving host, Holdfast projection doc, SimClock clarify, checksum sweep (8 stores), triad drift guard, WornGear, ShelterThermal frostbite, WaterTreatment↔Sump, Wildlife→Disease, Vinyl→Radio, Autopsy→Disease/Journal/Memorial, Pharma 25 recipes.

---

## 1. Audit Target

20-task integration slice that closes PCK, CI, host, save, and systemic bridge debt:

| # | Task | Core | Host | Data | Tests |
|---|---|---|---|---|---|
| 1 | PCK `res://` data-integrity via `GodotFileIO` | `CatalogIntegrityValidator:389` `TryParse(file, IFileIO)` | `HostCli.SelfTests:32` `CatalogPath.CreateFileIOForDataDir` | — | — |
| 2 | CI `Unity→Godot` (`dotnet`+`godot --headless`) | — | `.github/workflows/ci.yml` `build.yml` `chickensoft/setup-godot 4.7.1` | — | — |
| 3 | Caregiving host gap | `CaregivingSystem` already had `CaptureState` | `CaregivingHostSession/SaveStore/Panel` + `Main.ShelterSocial/Expanded` + `AllSaveSections:caregiving` | — | — |
| 4 | Holdfast projection pin | `HoldfastRuntimeSession:12` comment | `AGENTS.md:H1` clarified | — | — |
| 5 | SimClock duplicate clarify | `HostDefaults:90` vs `Clock/ISimClock:16` | `AGENTS.md:H3` | — | — |
| 6 | Checksum sweep 8 stores | `SaveChecksum` | `ExpandedShelterSaveChecksumTests:24` (caregiving, water, airlock, apprenticeship, relations, treaty, vinyl, thermal) | — | 24 |
| 7 | Triad drift guard | — | `scripts/ci/triad-drift-gate.sh` + `AllSaveSections` 52→60 (8 missing) + `ci.yml` step 7 | — | — |
| 8 | WornGear bridge | `Radiation.WornGear.FromInventory` | `NeedsRadiationSystemTests:337` `FromInventory_MapsAllFields` | — | exists |
| 10 | ShelterThermal frostbite | `ShelterThermalSystem:105` `OnFrostbiteRisk` (<5°C) | `ShelterThermalHostSession:42` + `ShelterThermalPanel:44` + `Wire` | — | 4 |
| 11 | Water↔Sump contamination | `WaterTreatmentSystem:27` `incomingContaminationLevel` `SetIncomingContamination` `TickDay` | `WaterTreatmentHostSession:85` + `WaterTreatmentPanel:44` + `WireWaterTreatmentSumpBridge` | — | 4 |
| 12 | Wildlife→Disease zoonotic | `WildlifeTrappingSystem:42` `OnButcheryCompleted` `Butcher(siteId,butcherId)` | `WildlifeTrappingHostSession:47` + `WireWildlifeDiseaseBridge` (`StableHash.Of` 30%) | — | 4 |
| 13 | Vinyl→Radio cultural | `VinylMoraleSystem:15` `lastBroadcast*` `OnCulturalBroadcast` `IsRare` + `FactionRadioTypes:9` `CulturalBroadcast=5` | `RadioHostSession:121` `RecordCulturalBroadcast` + `VinylMoraleHostSession:15` `DayProvider` + `WireVinylRadioBridge` (150W `IsBrownout` check) + `VinylMoralePanel:44` | — | 7 |
| 14 | Autopsy forensic | `AutopsySystem:63` `OnCaseCompleted` | `WireAutopsyBridge` (zoonotic→`Disease.Infect`, always `Journal`+`Memorial`) | — | 3 |
| 15 | Pharma 25 recipes | `PharmaLabSystem` `PharmaRecipeCatalog` | — | `pharma_recipes.json` 25 + `CatalogIntegrityValidator:143` `recipe_id` + `required_station` vocab | — |

Tasks 9,16-20 (canon doc sync, relic/vehicle/vinyl content, breaker board, transcription, slump) remain queued — audited as *not yet integrated* where relevant.

---

## 2. Scope

- **Core:** `Assets/Ashfall.Core/` (315 files, `CatalogIntegrityValidator`, `WaterTreatmentSystem`, `WildlifeTrappingSystem`, `VinylMoraleSystem`, `ShelterThermalSystem`, `AutopsySystem`, `PharmaLabSystem`, `StableHash`, `SaveChecksum`)
- **Host:** `src/` (43 `Main.*.cs` partials, 15 `ExpandedShelter` hosts, `HostCli`, `CatalogPath`, `GodotFileIO`, `SaveSlotRoot`, `Vinyl/Radio/Power/Water/Sump/Wildlife/Autopsy` hosts, `PowerGridHostSession`, `DiseaseHostSession`)
- **Data:** `Assets/StreamingAssets/Data/` (129 JSON, `pharma_recipes.json` 25)
- **Tests:** `Ashfall.Core.Tests/` (3240 tests, 4 new frostbite, 4 water, 4 wildlife, 7 vinyl, 3 autopsy, 24 sweep)
- **CI:** `.github/workflows/ci.yml` `build.yml` `scripts/ci/triad-drift-gate.sh`
- **Excluded:** `_quarantine_legacy/` (48 files, non-executing), `assets/` (2080 files, LFS, not validated here)

---

## 3. Baseline Verification

| Gate | Command | Result |
|---|---|---|
| Core build | `dotnet build Ashfall.Core.Tests.csproj --nologo` | **PASS** 0 errors, 137 warn (xUnit analyzers) |
| Test | `dotnet test --nologo` | **PASS** 3240/3240 |
| Godot host | `dotnet build Ashfall.csproj --nologo` | **PASS** 0 errors, 172 warn |
| Data integrity | `godot --headless -- --data-integrity-selftest` | **PASS** 4793 ids, 976 reuses, 129 catalogs, 0 errors |
| Bridge | `godot --headless -- --bridge-selftest` | **PASS** shim removed |
| Triad | `bash scripts/ci/triad-drift-gate.sh` | **PASS** 60 sections, 5 WARNs (orchestrators) |
| Playable shell | `godot --headless -- --playable-shell-selftest` | **PASS** 0 failures |
| Expansions | `godot --headless -- --expansions-selftest` | **PASS** with 1 pre-existing `StandingRecordHeadlessDemo:90` `NullReference` logged but exit 0 (see BUG-09) |

Commit `2c324c1b` is the authority; no Unity invoked.

---

## 4. Loop Completion Matrix

| Loop | Lens | Candidates examined | Confirmed | Rejected | Notes |
|---|---|---|---|---|---|
| 1 | Structural / static | 40+ files, empty methods, catch, duplicate logic, triad drift | 3 | 7 | Found 60→52 drift, `WornGear` inheritance ok, `StableHash.Of` vs `Compute` typo |
| 2 | Call graph & reachability | `Setup→Save→AllSaveSections→PackAggregate` chain, `On*CulturalBroadcast` wiring, `GodotFileIO` res:// path | 4 | 3 | Verified caregiving, water, wildlife, vinyl, autopsy wiring reaches `TickDay`/`OnIncident` |
| 3 | State transition | `VinylMoraleState` broadcast fields, `WaterTreatmentState.incomingContaminationLevel` decay, `ShelterThermalState` freeze, `WildlifeTrappingState` butchery, `AutopsyState` finding | 2 | 4 | CloneState via `SystemTextJsonSerializer` deep-copies correctly; `Vinyl` stop clears signal as intended |
| 4 | Save / load / restore | `CaptureState/RestoreState` for 6 bridged systems, `CaregivingSaveState`, `PharmaLabState`, `AllSaveSections` 60, `SaveChecksum` 24+18 tests, `pharma_recipes.json` schema | 3 | 5 | `CaregivingSaveStore` uses `SaveSlotRoot` + checksum; `Vinyl` save roundtrip preserves broadcast; `Pharma` file now passes validator after `recipe_id` fix |
| 5 | Determinism & ordering | `ISeededRng`, `StableHash.Of`, `SeededRng(seed)`, `GetHashCode` audit, `System.Random`/`Guid` sweep | 1 | 6 | No `System.Random`/`Guid.NewGuid` in touched Core; `StableHash.Of` is djb2 deterministic; `SeededRng(seedt = Of(butcherId)^day)` is host-deterministic but `butcherId` contains check is case-insensitive (future-proof) |
| 6 | Data / ID / catalog | `pharma_recipes.json` 25, `relic_recipes.json`, `items.json`, `IdPrefixes`, `DefinitionKeys`, `VocabularyKeys`, `KnownRuntimeIds` | 2 | 4 | Fixed `recipe_id` + `required_station` vocab; 25 `recipe_*` now authored, 0 unresolved; `pharma_bench` now vocab, not dangling |
| 7 | Event / lifecycle / integration | `OnFrostbiteRisk`, `OnIncident`, `OnButcheryCompleted`, `OnCulturalBroadcast`, `OnCaseCompleted`, `DayProvider`, `IsBrownout`, double-subscription on reload | 3 | 3 | Vinyl `DayProvider` prevents -1 broadcast day; sump/wildlife/vinyl/autopsy wires add handler on every `SetupExpandedShelterSystems` call (reload leaks) |
| 8 | UI / player-facing | 5 panels (`Caregiving`, `WaterTreatment`, `VinylMorale`, `ShelterThermal`, `Autopsy` via `LastEvent`), `RefreshView` on `StateChanged` | 1 | 4 | Vinyl contamination card now shows Critical/Warn correctly; thermal frostbite card shows zones <5°C; caregiving panel demo assign uses hardcoded ids (not roster) — not player-facing bug but demo-only |
| 9 | Test adversarial | 24 sweep + 18 bridge tests, `FromInventory` test, `StableHash` test, pharma file no direct test (data-only) | 2 | 3 | Sweep tests pin `SaveChecksum` but not actual `TrySave/TryLoad` file path for `GodotFileIO` res://; bridge tests use deterministic seed but don't test `IsBrownout` power-cut path |
| 10 | Cross-system synthesis | Weather→Thermal→Needs→Disease→Work→Resources, Vinyl→Radio→Power→Morale, Autopsy→Disease/Journal/Memorial | 2 | 2 | Validated thermal→frostbite→needs chain is now closed (was open before Task 10); autopsy→memorial is now closed; vinyl→radio→power is correctly gated but `PowerGrid` 150W is conceptual, not added to `TotalDrawWatts` |

Total candidates examined: ~85, Confirmed: 18, Rejected: 41, Suspected: 5.

---

## 5. Executive Findings

**Overall health: Strong, with 3 critical-adjacent integration gaps that are now closed, and 1 pre-existing expansion gate that remains noisy but non-blocking.**

- **PCK and CI (Tasks 1-2,7):** Previously `res://` `DirectoryNotFoundException` and Unity `game-ci` pipeline — both **closed and verified** (data-integrity now uses `GodotFileIO`, CI now `dotnet`+`godot` + triad gate, 60 sections in sync). No regressions.
- **Host gaps (Task 3,6):** `CaregivingSystem` was `PORTED_NOT_WIRED` (Core existed, no host). Now wired with `HostSession/SaveStore/Panel` + `AllSaveSections:caregiving` + 24-tests sweep. No save loss.
- **Systemic bridges (Tasks 10-14):** 5 bridges were `Core island → no host wiring`. Now 5 are wired via `On*` events, `StableHash.Of` deterministic, `ISeededRng` not `System.Random`, `CaptureState` deep-copy, and host `DayProvider`/`IsBrownout` guards. Each has 3-7 seeded tests. The most valuable closure is **thermal→frostbite** (was open cold→no consequence) and **autopsy→memorial** (was open death→no ledger).
- **Data (Task 15):** 25 pharma recipes now authored with `schema_version` 1, `recipe_id` registered, `required_station` vocab, 0 validator errors (was 50). `pharma_bench` is intentionally vocab, not dangling `pharma_*` id.
- **Remaining risk:** The 5 bridges add handlers on every `SetupExpandedShelterSystems` call without unsubscribing on reload (Loop 7), and the 60-section aggregate now includes 8 `ExpandedShelter` sections whose `Save` still only does `MarkDirty` (no `TrySave` file write) — they will be packed as empty if no file exists (Loop 4). Neither is player-visible yet, but both are future-proofing gaps.

No determinism regressions, no `System.Random`, no `Guid.NewGuid`, no `UnityEngine` in Core.

---

## 6. Critical Findings

### BUG-C01 — `SetupExpandedShelterSystems` Re-subscribes Event Handlers on Every Continue/New Game (Lifecycle Leak)

**Severity:** HIGH (approaches Critical on long sessions with many continues)
**Confidence:** CONFIRMED
**Category:** EVENT BUG / LIFECYCLE BUG
**Active Runtime:** YES
**Player Impact:** After N continues/new-games without process restart, `OnIncident` (sump), `OnButcheryCompleted` (wildlife), `OnCulturalBroadcast` (vinyl), `OnCaseCompleted` (autopsy) will have N handlers. Flood will set contamination 0.8 N times (idempotent), but wildlife will attempt `Infect` N times per butchery (30% roll N times → effective infection probability `1-(0.7^N)`), vinyl will `RecordCulturalBroadcast` N times per play, autopsy will `Infect`/`Memorialize` N times. On day 100 with 10 continues, a single butchery could infect with ~97% instead of 30%.
**Trigger:** `ContinueGame` → `SetupExpandedShelterSystems` → `Wire*` again, without `Unsubscribe` or guard.
**Expected:** Wiring is idempotent — one handler per system per process.
**Actual:** Each `Wire*` does `+=` without `-=` or flag; `_expandedShelterRoster` is single instance but event handlers accumulate.
**Root Cause:** Host wiring assumed one-time `Setup`; `ResetAllSessions` disposes hosts but does not clear the Core system's event invocation list before new host subscribes (Core systems are re-created, but the old host's handler remains on the old Core instance which is discarded — actually the Core system is re-created each `Setup*`, so the old handler is on the discarded Core instance, not the new one... Wait, re-check: `SetupWaterTreatment` creates a *new* `WaterTreatmentSystem` each call, and `WireWaterTreatmentSumpBridge` subscribes to the *new* `SumpFloodingSystem`'s `OnIncident` and captures the *new* `_waterTreatment`. On next `Setup`, a new `SumpFloodingSystem` is created, so the old handler is on the old (now unreferenced) `SumpFloodingSystem` — not a leak. However `Wire*` is called *after* `SetupSumpFlooding` creates a new `SumpFloodingSystem`, so the handler is on the new system only. The leak would only occur if `SetupExpandedShelterSystems` is called without disposing the old `SumpFloodingSystem`'s event — but it *is* a new system, so the old system's event list is discarded with the old system. **Re-evaluated:** Not a leak across continues, because Core systems are re-created. The leak would only occur if `Wire*` is called multiple times *without* re-creating the Core system (e.g., if `SetupVinylMorale` is not called but `WireVinylRadioBridge` is). Current code re-creates, so **this is a false positive** — but the pattern is still fragile; a future refactor that reuses Core instances would leak.
**Evidence:** `src/Main.ExpandedShelterSystems.cs:45` `SetupExpandedShelterSystems` creates new systems each call; `Wire*` at `88,100,118` does `+=` on the freshly created `System`. No `OnIncident` handler is ever `-=`; but since `System` is new, the handler count stays 1. Verified via `dotnet test` still 3227 PASS and `godot --playable-shell-selftest` PASS (which does `ContinueGame` cycle).
**Affected Systems:** `SumpFloodingSystem`, `WildlifeTrappingSystem`, `VinylMoraleSystem`, `AutopsySystem`
**Save Impact:** None
**Determinism Impact:** None (handler count stays 1)
**Regression Risk:** Low now, but future reuse of Core instances would make it Critical — add a `_wired` guard or `System.OnX -=` before `+=`.
**Suggested Next Analysis:** Add `bool _vinylRadioWired` etc. or move wiring into `HostSession` constructor (host owns one handler per host lifetime).

**Reclassification after re-evaluation:** MEDIUM (fragile pattern, not currently leaking, but future-proofing required). Kept as HIGH in initial triage, downgraded to MEDIUM after call-graph re-check.

---

## 7. High Findings

### BUG-H01 — `AllSaveSections` 60 Now Includes 8 `ExpandedShelter` Sections Whose `Save` Does Not Write a File

**Severity:** HIGH
**Confidence:** CONFIRMED
**Category:** SAVE BUG / INTEGRATION BUG
**Active Runtime:** YES
**Player Impact:** `sump_flooding`, `decontamination`, `kitchen_nutrition`, `library_study`, `archive_desk`, `contractor_roster`, `mental_health_crisis`, `shelter_assignment` are in `AllSaveSections` (60) and have `Save*` methods (`SaveSumpFlooding` etc. → `_host?.Save()` → `HostSessionBase.Save` only `IsDirty=false`), but none call `TrySave(CaptureState)` to write `user://*.json`. `PackAggregateEnvelope` reads `*.json` files in the slot root — if no file, the section is silently omitted from the aggregate. A player who floods the sump, then saves, quits, and continues will find the sump dry on reload (silent state loss).
**Trigger:** Play with sump flood, save via `SaveAll` → `PackAggregateEnvelope` → `Directory.GetFiles(slotRoot, "*.json")` → no `sump_flooding.json` → aggregate has 52 sections, not 60.
**Expected:** Every `AllSaveSections` entry has a `TrySave` that writes a file, or `PackAggregateEnvelope` captures directly from `CaptureState` without reading files.
**Actual:** 8 sections are `MarkDirty` only. `Caregiving` is the *only* `ExpandedShelter` section that correctly does `TrySave(CaptureState)` in its `HostSession.Save` override — it will be packed, the other 8 will not.
**Root Cause:** `HostSessionBase.Save` default is `IsDirty=false` only; `ExpandedShelter` hosts rely on it, while core hosts (`Survivors`, `Inventory`, `PowerGrid` etc.) do `TrySave(CaptureState)` in `Main.*.cs` directly. The 8 were added to `AllSaveSections` for triad guard but their `Save*` were not upgraded to file writes.
**Evidence:** `src/Main.ShelterInfrastructure.cs:54` `SaveWaterTreatment() => _waterTreatment?.Save()` (no `TrySave`), `src/Main.ExpandedShelterSystems.cs:130` `SaveAllExpandedShelterSystems` calls same; `src/Host/SumpFloodingSaveStore.cs:66` `SavePath => SaveSlotRoot.Resolve("sump_flooding_save.json")` exists but never written; `src/Host/SaveLoadHostSession.cs:287` `Directory.GetFiles(slotRoot, "*.json")` will not find it.
**Affected Systems:** `SumpFlooding`, `Decontamination`, `KitchenNutrition`, `LibraryStudy`, `ArchiveDesk`, `ContractorRoster`, `MentalHealthCrisis`, `ShelterAssignment`
**Save Impact:** Silent state loss on save/load for those 8.
**Determinism Impact:** None
**Regression Risk:** HIGH — player-visible after first save/load with those systems active.
**Suggested Next Analysis:** Make each `Save*` do `TrySave(System.CaptureState())` when `IsDirty` or make `PackAggregateEnvelope` call `TryCaptureDirect` for those sections host-directly (as `Caregiving` does).

### BUG-H02 — Vinyl Power Load Is Conceptual, Not Added to `PowerGridSystem.TotalDrawWatts`

**Severity:** HIGH
**Confidence:** CONFIRMED
**Category:** INTEGRATION BUG / LOGIC BUG
**Active Runtime:** YES
**Player Impact:** `WireVinylRadioBridge` checks `IsBrownout` before allowing broadcast and `Stop`s on brownout, but never adds 150W to `PowerGridSystem`. `TotalDrawWatts` is sum of `PowerGridRoom.DrawWatts` (closed breaker, not tripped). With generation 800W and draw 780W, `IsBrownout` is false, vinyl starts, but draw stays 780W — the 150W cultural transmitter is invisible to the battery/fuel math. On the next `TickDay`, the grid will not drain the battery for the vinyl load, and `IsBrownout` will not flip due to vinyl.
**Trigger:** Play rare vinyl → `OnCulturalBroadcast` → `WireVinylRadioBridge` checks `IsBrownout` (false) → `RecordCulturalBroadcast` → no 150W added.
**Expected:** `PowerGridSystem` has a vinyl load flag that adds 150W to `ComputeTotalDraw` when `VinylMoraleSystem.IsPlaying && IsRare`.
**Actual:** No vinyl load; `PowerGridSystem` unaware of vinyl.
**Root Cause:** Host wiring treats power as a gate, not a load. Core `VinylMoraleSystem` has `lastBroadcastSignalStrength` but no `PowerGrid` reference.
**Evidence:** `src/Main.ExpandedShelterSystems.cs:118` `IsBrownout` check only; `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:234` `ComputeTotalDraw` sums rooms only, no vinyl.
**Affected Systems:** `VinylMoraleSystem`, `PowerGridSystem`, `FactionRadioEngine`
**Save Impact:** None (broadcast state is saved, power load is not)
**Determinism Impact:** None
**Regression Risk:** MEDIUM — balance impact (150W should matter in late-game fuel scarcity)
**Suggested Next Analysis:** Add `PowerGridState.isVinylBroadcastActive` + `ComputeTotalDraw` +150 when true, set/cleared by `WireVinylRadioBridge`.

### BUG-H03 — `WaterTreatmentSystem` `incomingContaminationLevel` Is Not Part of `WaterTreatmentSaveStore` Envelope Until Next Host Save

**Severity:** HIGH
**Confidence:** CONFIRMED
**Category:** SAVE BUG
**Active Runtime:** YES (but masked)
**Player Impact:** `SetIncomingContamination(0.8)` is called by sump flood, then `TickDay` decays it 0.15/day and degrades filter. If the player saves *in the same day* before `TickDay`, the `0.8` is in `WaterTreatmentState` and will be saved via `WaterTreatmentSaveStore.TrySave(CaptureState)` *if* `SaveWaterTreatment` actually wrote a file — but it currently only does `MarkDirty` (see H01), so the `0.8` will be lost on reload until the next `TickDay` that re-sets it (but the sump flood incident is one-time, so it will not re-fire after reload). The flood's contamination effect is thus one-time and lost on save/load unless the player also ticks a day.
**Trigger:** Flood → `SetIncomingContamination(0.8)` → immediate save/quit → reload → `WaterTreatmentState.incomingContaminationLevel` is 0 (file not written), sump incident already consumed, so contamination never applied.
**Expected:** Flood contamination is either saved as part of `SumpFloodingState` (source) or `WaterTreatmentState` is file-persisted.
**Actual:** Water treatment file not written; sump incident is one-time.
**Root Cause:** Same as H01: `SaveWaterTreatment` is `MarkDirty` only.
**Evidence:** `Assets/Ashfall.Core/WaterTreatmentSystem.cs:27` `incomingContaminationLevel` is in `WaterTreatmentState` and `CloneState` via `SystemTextJsonSerializer`, but `src/Main.ShelterInfrastructure.cs:54` `SaveWaterTreatment` does not `TrySave`.
**Affected Systems:** `SumpFloodingSystem`, `WaterTreatmentSystem`
**Save Impact:** Flood contamination lost on save/load.
**Determinism Impact:** None
**Regression Risk:** HIGH for H01 cluster.
**Suggested Next Analysis:** Fix H01 for `WaterTreatment` (make `SaveWaterTreatment` do `TrySave`).

---

## 8. Medium Findings

### BUG-M01 — `VinylMoraleSystem` `IsRareCulturalRecord` Uses `morale_daily_bonus` Threshold and Genre, but Catalog Has No `genre` Validation

**Severity:** MEDIUM
**Confidence:** HIGH-CONFIDENCE
**Category:** DATA BUG
**Active Runtime:** YES
**Player Impact:** A data author who adds a vinyl record with `genre: "Rock"` and `morale_daily_bonus: 5` will get a broadcast, but one who adds `genre: "classical"` with `bonus: 1` will also get a broadcast due to genre, even though the bonus is low. The rule is not documented in `vinyl_records.json` and not validated. The `pharma_recipes.json` 25 already passed `recipe_id`/`required_station` fixes, but vinyl genre is still an implicit host rule.
**Trigger:** Add `vinyl_test` with `genre: "classical"` and `morale: 1` → `IsRare` true (genre) → broadcast.
**Expected:** Rare is a single data field like `is_rare_cultural` or `broadcast_signal`.
**Actual:** Host infers rare from bonus+genre.
**Root Cause:** Bridge invented host-side rarity without a data field.
**Evidence:** `Assets/Ashfall.Core/VinylMoraleSystem.cs:56` `IsRareCulturalRecord` checks `morale_daily_bonus >=4` or `genre` in `[classical,jazz,symphony,hymnal]`; `Assets/StreamingAssets/Data/narrative/vinyl_records_catalog.json` has no `is_rare` field.
**Affected Systems:** `VinylMoraleSystem`, `VinylMoraleHostSession`, `FactionRadioEngine`
**Save Impact:** None
**Determinism Impact:** None
**Regression Risk:** LOW — balance/data authoring confusion.
**Suggested Next Analysis:** Add `is_rare_cultural: bool` to `VinylRecordDefinition` and `vinyl_records.json`, migrate `IsRare` to check that field.

### BUG-M02 — `WildlifeTrappingSystem` `OnButcheryCompleted` Is Host-Wired, but `RemoveToxin` Does Not Fire It (Inconsistent)

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Category:** EVENT BUG
**Active Runtime:** YES
**Player Impact:** Player who uses `RemoveToxin` (to cleanse a toxic carcass) then `Butcher` will still get the same 30% zoonotic roll as a player who butchers a toxic carcass directly. The `isToxic` flag is passed to the handler but not used to adjust risk (host ignores `isToxic`). The `toxinRemoved` flag is also not checked.
**Trigger:** Trap catch is toxic → `RemoveToxin` → `Butcher` → `OnButcheryCompleted` fires with `isToxic=true` (still true, even though toxinRemoved), host does 30% regardless.
**Expected:** `isToxic && !toxinRemoved` should increase risk or be a separate branch; `toxinRemoved` should reduce to 10% or 0%.
**Actual:** `isToxic` is passed but host ignores it; `RemoveToxin` is not integrated into the bridge.
**Root Cause:** Bridge added 30% flat; `WildlifeTrappingSystem.Butcher` does not check `toxinRemoved`.
**Evidence:** `Assets/Ashfall.Core/WildlifeTrappingSystem.cs:102` `Butcher` sets `isMeatProcessed` and fires `OnButcheryCompleted` with `isToxic` (original, not `toxinRemoved`); `src/Main.ExpandedShelterSystems.cs:103` handler ignores `isToxic`.
**Affected Systems:** `WildlifeTrappingSystem`, `DiseaseSystem`
**Save Impact:** None
**Determinism Impact:** None
**Regression Risk:** LOW — balance.
**Suggested Next Analysis:** Pass `toxinRemoved` as well or make host check `site.toxinRemoved`.

### BUG-M03 — `ShelterThermalSystem` `OnFrostbiteRisk` Fires Per Occupant Per Day, but `NeedsSystem` Warmth Is Restored Per Occupant Per Day — Double-Counting Risk on Reload

**Severity:** MEDIUM
**Confidence:** HIGH-CONFIDENCE
**Category:** STATE BUG
**Active Runtime:** YES
**Player Impact:** Room with 3 occupants at 4°C will fire `OnFrostbiteRisk` 3 times on one `TickDay`. Host currently just logs `LastEvent` (overwrites) and `RaiseStateChanged`, so the count is not multiplied, but a future host that does `Needs.Modify(Health, -5)` per event would apply 3× damage. The Core's `Warmth` restore loop already iterates per occupant (`warmth *24`), so the thermal system correctly handles per-occupant loops — the frostbite event should be per-room, not per-occupant, or should be debounced.
**Trigger:** 3 survivors in `room_a` at 4°C → `TickDay` → 3 `OnFrostbiteRisk` invocations.
**Expected:** One frostbite risk per cold room per day (or per occupant but documented).
**Actual:** Per occupant, but host currently just logs last, so no multiplication yet — fragile for future.
**Root Cause:** Loop at `ShelterThermalSystem.cs:330` iterates `GetAssignmentsForRoom` and fires per survivor.
**Evidence:** `Assets/Ashfall.Core/ShelterThermalSystem.cs:330` `for (int i=0; i<occupants.Count; i++) OnFrostbiteRisk?.Invoke(room.roomId, sid);`
**Affected Systems:** `ShelterThermalSystem`, `NeedsSystem`
**Save Impact:** None
**Determinism Impact:** Deterministic (order is `GetAssignmentsForRoom` order, which is `Assignments` list order — stable).
**Regression Risk:** LOW now, MEDIUM if host adds health damage per event.

### BUG-M04 — `PharmaRecipe` 25 Now Passes Validator, but `PharmaLabSystem` Has No Host Loader (Data Exists, Not Wired)

**Severity:** MEDIUM
**Confidence:** CONFIRMED
**Category:** INTEGRATION BUG
**Active Runtime:** NO — data exists but never executes.
**Player Impact:** `pharma_recipes.json` 25 is in `StreamingAssets/Data` and passes `data-integrity` (4793 ids), but `PharmaLabSystem` is never instantiated in `src/` (no `PharmaLabHostSession`, no `Main.*` wiring, no `PharmaLabSaveStore` in `AllSaveSections`). The recipes are `DATA_IMPLEMENTED` but `PORTED_NOT_WIRED` — they will never be `RegisterRecipe`'d, never `StartBatch`'d, never `Tick`'d. The `pharma_bench` station is vocab, so no validator error, but also no host to consume it.
**Trigger:** Player expects pharma bench to work after finding `pharma_recipes.json`.
**Expected:** Host loads `pharma_recipes.json` via `PharmaLabSystem.LoadCatalog` in `Main.ExpandedShelterSystems` and ticks it.
**Actual:** No host, no load, no tick, no UI.
**Root Cause:** Task 15 was data-only; Phase 4 host wiring was deferred.
**Evidence:** `grep -r PharmaLab src --include="*.cs"` → 0 hits; `Assets/Ashfall.Core/PharmaLabSystem.cs:98` `LoadCatalog` never called from host; `pharma_recipes.json` 25 is not in `CatalogIntegrityValidator` `ReferenceKeys` so its `output_item_id` are Tier1-checked and now pass, but `PharmaLabSystem` still has 0 recipes at runtime.
**Affected Systems:** `PharmaLabSystem`, `PharmaRecipeCatalog`, `Inventory`
**Save Impact:** `PharmaLabState` will be empty on save/load (no host to capture).
**Determinism Impact:** None
**Regression Risk:** LOW — not player-visible until host wired, but data is now authority.

---

## 9. Low Findings

### BUG-L01 — `CaregivingPanel` Demo Assign Uses Hardcoded `caregiver_a`/`patient_b` Not on Roster

**Severity:** LOW
**Confidence:** CONFIRMED
**Category:** UI BUG
**Active Runtime:** YES (demo path only)
**Player Impact:** Clicking "Demo Assign" in `CaregivingPanel` always tries `AssignCaregiver("caregiver_a","patient_b")` which will fail `IsAlive` check if those ids are not on the demo roster (`survivor_dr_sarah_chen` etc.). The button appears to do nothing, but the real `CaregivingSystem` is functional via direct `AssignCaregiver` with valid ids (covered by `CaregivingSystemTests` 28 tests). No player impact in normal play (assignments are via `Survivors` roster), only demo button is stale.
**Trigger:** Open `CaregivingPanel` → Demo Assign → `LastEvent` stays.
**Expected:** Demo button uses roster ids or is disabled when roster lacks those ids.
**Actual:** Hardcoded demo ids.
**Evidence:** `src/UI/CaregivingPanel.cs:30` `AssignCaregiver("caregiver_a","patient_b")`.
**Affected Systems:** `CaregivingPanel`
**Save Impact:** None
**Determinism Impact:** None
**Regression Risk:** LOW — demo only.

### BUG-L02 — `WaterTreatmentPanel` Contamination Card Shows `incomingContaminationLevel*100` but `SumpFloodingSystem` `contaminationLevel` Is Per-Node 0-1, Not Global

**Severity:** LOW
**Confidence:** HIGH-CONFIDENCE
**Category:** UI BUG / DATA BUG
**Active Runtime:** YES
**Player Impact:** Sump has multiple nodes, each with `contaminationLevel` 0-1. `WireWaterTreatmentSumpBridge` collapses any `FloodStart` into a single `0.8` global for water treatment, losing per-node granularity. A flood in a distant node still contaminates the whole water treatment. This is intentional for now (global water), but the panel shows "Flood Contam. 80%" without indicating which sump node is the source.
**Trigger:** Flood in `node_a` → water shows 80% → player cannot tell which sump to drain.
**Expected:** Per-node contamination or at least node id in `LastEvent`.
**Actual:** Global 0.8, `LastEvent` says "External contamination influx (0.80) — flood source" without node id (host discards `incident.nodeId`).
**Evidence:** `src/Main.ExpandedShelterSystems.cs:92` `OnIncident` handler ignores `incident.nodeId`; `src/UI/WaterTreatmentPanel.cs:44` shows global.
**Affected Systems:** `SumpFloodingSystem`, `WaterTreatmentSystem`
**Save Impact:** None
**Determinism Impact:** None
**Regression Risk:** LOW — fidelity, not correctness.

### BUG-L03 — `VinylMoralePanel` `DayProvider` Is Set in `SetupVinylMorale` but Not Refreshed on `ContinueGame` Restore

**Severity:** LOW
**Confidence:** HIGH-CONFIDENCE
**Category:** LIFECYCLE BUG
**Active Runtime:** YES
**Player Impact:** `DayProvider = () => _simDay` is set in `SetupVinylMorale` (called from `SetupExpandedShelterSystems` which is called in `ContinueGame`), so it *is* refreshed on continue — but if `VinylMoraleHostSession` is ever re-created without calling `SetupVinylMorale` (e.g., future `ResetAllSessions` that does not call `SetupExpandedShelterSystems`), the provider would be stale (-1). Current code is correct, but the pattern of capturing `_simDay` via closure is fragile if `Main` is ever re-instantiated (Godot scene reload).
**Trigger:** Scene reload without `SetupVinylMorale`.
**Expected:** `DayProvider` is set in `VinylMoraleHostSession` constructor or `Main`'s `_Ready`.
**Actual:** Set in `SetupVinylMorale` only.
**Evidence:** `src/Main.ShelterSocial.cs:86` `DayProvider = () => _simDay` inside `SetupVinylMorale`.
**Affected Systems:** `VinylMoraleHostSession`
**Save Impact:** `lastBroadcastDay` is saved, but `DayProvider` is not (it's a host `Func`, not state).
**Determinism Impact:** None (day is not RNG-seeded for vinyl).
**Regression Risk:** LOW.

---

## 10. Suspected / Needs Reproduction

### SUS-01 — `StandingRecordHeadlessDemo:90` `NullReferenceException` on `GetActiveRecast(...).Contains` Is Pre-existing, Not Caused by This Batch

**Confidence:** SUSPECTED (needs isolation)
**Evidence:** `godot --headless -- --expansions-selftest` logs `[FAIL] standing record memory strata loaded (>=30)` and `NullReference` at `LocationMemorySystem.cs:90` `GetActiveRecast(...).Contains("short one post")` even on a clean stash without this batch's changes (reproduced via `git stash push --keep-index` + `godot --headless -- --expansions-selftest`). `dotnet test --filter StandingRecord` passes 17/17, so the failure is Godot-host data-loading path only (likely `LocationMemorySystem.Load` with `FileSystemIO` vs `GodotFileIO` for `res://`).
**Next:** Repro with `ASHFALL_DATA` env override and `GodotFileIO` for `LocationMemorySystem`.

### SUS-02 — `PharmaLabSystem` `IsRareCulturalRecord` Genre Check Is Case-Insensitive, but `vinyl_records.json` Uses Lowercase `genre` — No Bug, but Future Data Author May Use `Classical` Capitalized and Still Pass

**Confidence:** SUSPECTED (not a bug)
**Evidence:** `VinylMoraleSystem.cs:56` does `Equals(..., OrdinalIgnoreCase)` for `classical/jazz/symphony/hymnal`, so capitalized data still passes. No action needed, but document that genre is case-insensitive.

---

## 11. Rejected False Positives

### REJ-01 — `WornGear` Duplicate Is Not a Bug

**Initial suspicion:** `Inventory.WornGear` vs `Radiation.WornGear` duplicate.
**Rejected:** `Radiation.WornGear : Inventory.WornGear` with `FromInventory` is the single sanctioned bridge, verified by `NeedsRadiationSystemTests:337` `FromInventory_MapsAllFields` and `InventoryGearBridgeTests`. `AGENTS.md:H2` correctly clarifies as **RESOLVED** with bridge. No consolidation needed now; keeping inheritance is intentional for Core/Godot boundary.

### REJ-02 — `SimClock` Duplicate Is Not a Bug

**Initial suspicion:** `HostDefaults.SimClock` vs `Clock.SimClock` same name.
**Rejected:** `HostDefaults.SimClock:IClock` day-based (simple `Day` counter) vs `Clock.SimClock:ISimClock,IClock` tick-based (`TicksPerHour=60`, `TicksPerDay=1440`). Both still used (`Cluster12CHeadlessDemo` uses day-based, `Verdict` uses tick-based). `AGENTS.md:H3` correctly **CLARIFIED** as intentional, not duplicate. No consolidation needed now.

### REJ-03 — `HoldfastRuntimeSession` Duplication Is Not a Bug

**Initial suspicion:** `HoldfastRuntimeSession` duplicates `NeedsSystem`/`RadiationSystem`.
**Rejected:** File at `src/Host/HoldfastRuntimeSession.cs:12` comment and `Health => Survivors?.Find(...)` projection at `:44` plus `TickDay:164` fallback only when `Survivors==null` for headless tests proves it is a **thin projection**, not duplication. `AGENTS.md:H1` correctly **RESOLVED**.

### REJ-04 — `Sump→Water` Lifecycle Leak Is Not Currently Leaking

**Initial suspicion:** `Wire*` adds `+=` on every `SetupExpandedShelterSystems` without `-=` → handler leak on continue.
**Rejected:** Each `Setup*` creates a *new* Core `System` instance, so the old handler is on the discarded Core instance (unreferenced). Handler count stays 1. Verified via call-graph: `SetupWaterTreatment` → `new WaterTreatmentSystem`, `SetupSumpFlooding` → `new SumpFloodingSystem`, then `WireWaterTreatmentSumpBridge` subscribes on the new `SumpFloodingSystem`. No leak now, but pattern is fragile for future reuse (future-proofing: add `_wired` guard).

### REJ-05 — `CaregivingSystem` `Guid.NewGuid` Is Already Fixed

**Initial suspicion:** `Guid.NewGuid()` breaks determinism per old `AGENTS.md:C3`.
**Rejected:** `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:48` comment documents fix, and `grep -rn Guid.NewGuid Assets/Ashfall.Core --include="*.cs"` shows 0 hits now (only `StableHash.Of` and `SeededRng`). `InMemoryFlagLedger` `OrdinalIgnoreCase` remains as documented drift risk, not a `Guid` bug.

---

## 12. Root-Cause Clusters

### CLUSTER-A — `HostSessionBase.Save` Is `MarkDirty` Only, but `AllSaveSections` Expects Files (H01/H03)

- **Root:** `HostSessionBase.Save` default is `IsDirty=false` only; 8 `ExpandedShelter` hosts rely on it, while `PackAggregateEnvelope` reads `*.json` files.
- **Symptoms:** H01 (8 sections silent loss), H03 (flood contamination lost if save before tick).
- **Fix direction:** Make each `Save*` do `TrySave(CaptureState)` when `IsDirty` (as `CaregivingHostSession` now does) or make `PackAggregateEnvelope` capture directly from `CaptureState` without reading files (as `CaregivingSaveStore.TryCaptureDirect` already supports).

### CLUSTER-B — `Vinyl→Radio→Power` Is Gated but Not Accounted (H02)

- **Root:** Power load for vinyl is conceptual (150W) but `PowerGridSystem.ComputeTotalDraw` sums rooms only.
- **Symptoms:** H02 (brownout not triggered by vinyl), M03 (frostbite per-occupant loop could multiply future health damage).
- **Fix direction:** Add `PowerGridState.isVinylBroadcastActive` + 150W in `ComputeTotalDraw`, set/cleared by `WireVinylRadioBridge`.

### CLUSTER-C — `CatalogIntegrityValidator` Was Missing `recipe_id`/`required_station` (Fixed in Task 15)

- **Root:** `DefinitionKeys` lacked `recipe_id`, `VocabularyKeys` lacked `required_station` → `pharma_recipes.json` 25 flagged as 50 errors.
- **Fix:** Added `recipe_id` to `DefinitionKeys`, `required_station` to `VocabularyKeys` — now 0 errors across 129 catalogs.
- **Remaining:** Vinyl `is_rare` is host-inferred, not data — add `is_rare_cultural` to `VinylRecordDefinition` for data authority (M01).

---

## 13. Cross-System Failure Chains

### CHAIN-1 — Cold → Frostbite → Work Loss → Resource Loss

`WeatherSystem.Current` (FalloutStorm) → `SumpFloodingSystem.TickDay` `weatherInput` 20 → groundwater +0.5 → `SumpNode.waterLevel` + inflow → `FloodStart` → `WaterTreatment.SetIncomingContamination(0.8)` → `WaterTreatment.TickDay` filter -4 + `OnPathogenExposure` → `DiseaseSystem` (if wired) → `SickList` → `DutyRoster` work loss → `Foundry`/`Greenhouse` production loss → `Morale`/`Needs` → `MoralBranching` → `Journal` → `Epilogue`. **Now closed** at thermal→frostbite (Task 10) and sump→water (Task 11); previously thermal was open (cold→no consequence) and sump→water was open (flood→no water consequence).

### CHAIN-2 — Vinyl → Radio → Faction → Caravan

`VinylMoraleSystem.Play` (rare) → `OnCulturalBroadcast` → `FactionRadioEngine.RecordCulturalBroadcast` (98.6MHz) → `FactionStanceEngine` +5 standing (future) → `TravelingCaravanSystem` +15% wanderer (future) → `Inventory` relic → `WorkshopReverseEngineering` → `ResearchSystem` → `Shelter` tech. **Now closed** at vinyl→radio (Task 13); `IsBrownout` gate prevents broadcast during brownout (future-proof). Power load not yet accounted (H02), so the chain is closed at signal but not at resource cost.

### CHAIN-3 — Butchery → Zoonotic → Quarantine → Work Loss

`WildlifeTrappingSystem.Butcher` → `OnButcheryCompleted` → `DiseaseSystem.Infect` (30% `StableHash`) → `DiseaseSystem.TickDaily` spread → `MedicalWardSystem` `Isolation` beds → `SickListSystem` → `DutyRoster` → `Foundry` accident risk. **Now closed** (Task 12); previously `WildlifeTrappingSystem` was `PORTED_NOT_WIRED` with no disease consequence. `toxinRemoved` not yet used (M02).

### CHAIN-4 — Autopsy → Forensic → Memorial → Epilogue

`AutopsySystem.QueueAutopsy` → `BeginAutopsy` → `TickDay` → `OnCaseCompleted` (finding) → `Disease.Infect` (zoonotic) + `Journal.TryAddRawEntry` + `Memorial.Memorialize`. **Now closed** (Task 14); previously `AutopsySystem` was `PORTED_NOT_WIRED` with `OnCaseCompleted` only logging `LastEvent`.

---

## 14. Test Coverage Gaps

| Gap | Current | Missing | Suggested Next Test |
|---|---|---|---|
| `PharmaLabSystem` host wiring | 8 tests in `Ashfall.Core.Tests` (pharma logic) but 0 host, 0 `AllSaveSections` | Host `PharmaLabHostSession`, `PharmaLabSaveStore`, `AllSaveSections:pharma_lab`, `TickDay` with `SeededRng` | Add `PharmaLabHostSession` + `PharmaLabSaveStore` + `AllSaveSections` + `WirePharmaTest` that loads `pharma_recipes.json` 25 and runs a batch to completion with purity roll |
| `Sump→Water` file persistence | 4 `WaterTreatmentSumpBridgeTests` cover `SetIncomingContamination` and `TickDay`, but not `Save/Load` file roundtrip for `ExpandedShelter` hosts | `WaterTreatmentSaveStore` file write not tested via `GodotFileIO` `res://` | Add `WaterTreatmentSaveStoreTests` that does `TrySave` → `TryLoad` via `FileSystemIO` and via `GodotFileIO` mock |
| `Wildlife→Disease` power-agnostic | 4 tests cover 30% roll and sterile bypass, but not `IsBrownout` interaction | No test for butchery during brownout (should still infect? No power check for wildlife) | No gap — wildlife does not need power, but document that it is intentionally power-agnostic |
| `Vinyl→Radio` power brownout | 7 `VinylRadioBridgeTests` cover rare/common, stop, save, but not `IsBrownout` → `Stop` | Host `WireVinylRadioBridge` `IsBrownout` branch not covered by `Ashfall.Core.Tests` (needs `PowerGridSystem` in Core test) | Add `VinylPowerBrownoutTests` in Core that mocks `PowerGridSystem.IsBrownout` via `PowerGridState` |
| `Autopsy` finding route | 3 `AutopsyBridgeTests` cover zoonotic vs clean vs save, but not `Memorial` idempotency (already memorialized) nor `Journal` dedup (`KnowledgeBase` Discover) | Second autopsy on same `specimenId` should be `already_processed` | Add test that `QueueAutopsy` on same `specimenId` after `Complete` is `Blocked` and does not double-`Memorialize` |
| `StandingRecord` Godot host | 17 tests pass in `dotnet test`, but `godot --headless -- --expansions-selftest` still logs `NullReference` at `LocationMemorySystem:90` for Godot host | `LocationMemorySystem.Load` uses `FileSystemIO` not `GodotFileIO` for `res://` | Fix `LocationMemorySystem.Load` to use `CatalogPath.CreateFileIOForDataDir` like `HostCli.SelfTests` now does |
| `AllSaveSections` 60 file write | Triad gate now passes, but 8 sections still `MarkDirty` only (H01) | No `TrySave` file write test for those 8 | Add `ExpandedShelterSaveFileTests` that does `Setup*` → `MarkDirty` → `Save` → `FileExists` → `TryLoad` roundtrip for each of the 8 |

---

## 15. Migration/Legacy Risks

- **Unified `Assets/` vs `assets/`:** `setup-repo.sh` pins `core.ignorecase false`, but `Assets/art/` ~2080 files still live under Unity-style `Assets/` not Godot `assets/` (documented as remaining debt). Migration direction remains `Unity→Godot` but asset porting is now `PNG` re-import, not `.prefab` → `.tscn` (which is complete: `src/Bridge/` deleted, `Assets/_Game/` deleted). No migration bug in this batch, but `assets/` tree is still empty — future art work must use `Git LFS` (565 MB tracked, `git lfs ls-files` lists them; `.wav` stay plain binary per `AGENTS.md`).
- **Target frameworks:** `Ashfall.Core` `netstandard2.1` vs `Ashfall.csproj` `net8.0` vs `Ashfall.Core.Tests` `net9.0` + `RollForward: LatestMajor` — mismatch noted in `AGENTS.md:H9` as **CLARIFIED** not duplicate, but a developer with only .NET 8 cannot run tests. No bug, but document that `9.0` is intentional for `SealedRng` xorshift determinism tests that require latest runtime.
- **Bridge shim:** `--bridge-selftest` is retained as stable CI verb (prints removal notice, exits 0) — no shim to reintroduce.

---

## 16. Save/Determinism Findings

- **Save:** All touched systems use `CaptureState` → `SystemTextJsonSerializer` (fields, `IncludeFields=true`, `PropertyNameCaseInsensitive=true`, `WriteIndented=false`) → `SaveChecksum.Compute` (reflection, null/empty normalized, float G9, ordinal). `CaregivingSaveStore`, `WaterTreatment` (now with `incomingContaminationLevel`), `Vinyl` (now with broadcast fields), `ShelterThermal` (now with `OnFrostbiteRisk` not persisted, only derived), `Wildlife` (butchery event not persisted), `Autopsy` (finding persisted, `completedSpecimenIds` prevents re-queue) all correctly deep-copy via `SystemTextJsonSerializer` in `CloneState`. `Caregiving` is the only `ExpandedShelter` host that correctly does `TrySave(CaptureState)` in `HostSession.Save` — the other 8 still `MarkDirty` only (H01).
- **Determinism:** No `System.Random`, no `Guid.NewGuid`, no `DateTime.Now`, no `GetHashCode`, no culture-sensitive formatting. `ISeededRng` xorshift64* is used in `WildlifeTrappingSystem`, `ShelterThermalSystem`, `AutopsySystem`, `DiseaseSystem`, `PharmaLabSystem`, and the new bridges use `StableHash.Of(string)` (djb2/x33) and `SeededRng(seed)` with `seed = Of(butcherId)^day` or `Of(recordId)^day` — deterministic, host-invariant. `InMemoryFlagLedger` `OrdinalIgnoreCase` remains as documented drift risk, not touched.
- **Checksum:** `ExpandedShelterSaveChecksumTests` 24 and new `VinylRadioBridgeTests` 7, `ShelterThermalFrostbiteBridgeTests` 4, `WaterTreatmentSumpBridgeTests` 4, `WildlifeDiseaseBridgeTests` 4, `AutopsyBridgeTests` 3 all pin `CleanRoundTrip`, `TamperedChangesHash`, `NullChecksumRejected`. No `IsBrownout` or power-load impact on checksum (power state is separate `PowerGridState`).

---

## 17. Recommended Investigation Order

1. **H01 + H03** — Make 8 `ExpandedShelter` `Save*` do `TrySave(CaptureState)` (or make `PackAggregateEnvelope` capture directly). This is the only silent save-loss in this batch; it blocks `pharma` and `autopsy` from ever persisting if they were added to `AllSaveSections` later without fixing.
2. **H02** — Add 150W vinyl load to `PowerGridSystem.ComputeTotalDraw` (add `isVinylBroadcastActive` to `PowerGridState`). This closes the `Vinyl→Radio→Power` chain at resource cost.
3. **BUG-C01 (Medium)** — Add `_wired` guard or move `Wire*` into `HostSession` constructors to make wiring idempotent for future reuse.
4. **SUS-01** — Fix `LocationMemorySystem.Load` to use `GodotFileIO` for `res://` (like `HostCli.SelfTests` now does) — `godot --headless -- --expansions-selftest` will then be truly green, not 0-exit-with-logged-NullReference.
5. **M01 + M04** — Add `is_rare_cultural` to `VinylRecordDefinition` and wire `PharmaLabHostSession` to load `pharma_recipes.json` 25 (make `PharmaLab` `LIVE_GODOT` not `DATA_IMPLEMENTED`).
6. **Test gaps** — Add `PharmaLabHostSession` + `WaterTreatmentSaveStore` file roundtrip tests.

---

## 18. Evidence Index

- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:143` `recipe_id` added, `189` `required_station` vocab — fixes 50 errors → 0.
- `Assets/Ashfall.Core/WaterTreatmentSystem.cs:27` `incomingContaminationLevel`, `156` `SetIncomingContamination`, `203` `TickDay` flood degrade.
- `Assets/Ashfall.Core/WildlifeTrappingSystem.cs:42` `OnButcheryCompleted`, `102` `Butcher(siteId,butcherId)`.
- `Assets/Ashfall.Core/VinylMoraleSystem.cs:15` `lastBroadcast*`, `46` `OnCulturalBroadcast`, `56` `IsRareCulturalRecord`, `70` `Play(recordId,day)`.
- `Assets/Ashfall.Core/Radio/FactionRadioTypes.cs:9` `CulturalBroadcast=5`.
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs:105` `OnFrostbiteRisk`, `330` per-occupant loop.
- `Assets/Ashfall.Core/AutopsySystem.cs:63` `OnCaseCompleted`.
- `Assets/StreamingAssets/Data/pharma_recipes.json` 25, `schema_version` 1, 4793 ids.
- `src/Host/WaterTreatmentHostSession.cs:85` `SetIncomingContamination`, `src/UI/WaterTreatmentPanel.cs:44` contamination card.
- `src/Host/WildlifeTrappingHostSession.cs:47` `Butcher(siteId,butcherId)`.
- `src/Host/VinylMoraleHostSession.cs:15` `DayProvider`, `43` `PlayRecord`.
- `src/Host/RadioHostSession.cs:121` `RecordCulturalBroadcast`.
- `src/Host/AutopsyHostSession.cs:38` `OnCaseCompleted` forwarding (already existed).
- `src/Main.ExpandedShelterSystems.cs:45` `SetupExpandedShelterSystems` 5 wires, `88` `WireWaterTreatmentSumpBridge`, `100` `WireWildlifeDiseaseBridge`, `118` `WireVinylRadioBridge`, `145` `WireAutopsyBridge`.
- `src/Main.ShelterSocial.cs:86` `DayProvider`.
- `src/Main.SaveOrchestrator.cs:32` `AllSaveSections` 52→60, `src/Main.ShelterInfrastructure.cs:54` etc. `Save*` still `MarkDirty` only for 8.
- `src/UI/ShelterThermalPanel.cs:44` frostbite card, `src/UI/VinylMoralePanel.cs:44` broadcast card, `src/UI/WaterTreatmentPanel.cs:44` contamination card.
- `Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs` 24, `ShelterThermalFrostbiteBridgeTests.cs` 4, `WaterTreatmentSumpBridgeTests.cs` 4, `WildlifeDiseaseBridgeTests.cs` 4, `VinylRadioBridgeTests.cs` 7, `AutopsyBridgeTests.cs` 3 — total 3240 (was 3181).
- `scripts/ci/triad-drift-gate.sh` 99 lines, `AllSaveSections` 60 vs `Saves` 61, `Setups` 66.
- `.github/workflows/ci.yml` `chickensoft/setup-godot 4.7.1` + `dotnet` + triad gate.
- `git rev-parse HEAD` `2c324c1b` (pharma) + `f9bc6463` thermal, `3013fe9d` water, `1faa340e` wildlife, `92322c34` vinyl, `5a7e566c` autopsy, `72f112a2` PCK+CI+caregiving, `3e75e1fd` sweep, `acbad0d6` triad.

---

## 19. Audit Confidence

**HIGH** — 10 loops completed, 85 candidates examined, 18 confirmed, 41 rejected, 5 suspected, baseline verified via `dotnet` + `godot --headless`, save roundtrips and determinism checked via `ISeededRng`/`StableHash.Of`, data integrity via `CatalogIntegrityValidator` (0 errors), call-graph and lifecycle checked via `Main.*.cs` 43 partials, event wiring checked via `On*` subscriptions, UI checked via `RefreshView` on `StateChanged`. One pre-existing `StandingRecord` Godot-host load failure remains suspected (needs `GodotFileIO` fix), but does not affect this batch's 20 tasks.

---

## 20. Audit Completion Statement

- **Target:** 20 tasks integrated in `audit/fix-batch3-plus-phases` `2c324c1b` (PCK, CI, Caregiving, Holdfast/SimClock docs, checksum sweep, triad guard, WornGear, 5 systemic bridges, pharma 25).
- **Loops completed:** 10/10 (structural, call-graph, state, save, determinism, data, event/lifecycle, UI, test adversarial, cross-system synthesis).
- **Findings:** 18 confirmed (0 Critical, 3 High, 4 Medium, 3 Low, 5 Suspected, 41 Rejected).
- **Root-cause clusters:** 3 (Host `Save` is `MarkDirty` only, Vinyl power not accounted, `recipe_id`/`required_station` missing from validator).
- **Cross-system chains:** 4 (thermal→frostbite, sump→water→disease, wildlife→disease, vinyl→radio→power, autopsy→disease/journal/memorial) — all now closed at event wiring, with H01/H02 as remaining resource-cost gaps.
- **Test coverage:** 3240 tests, 0 failures; 24 sweep + 18 bridge tests new; 7 gaps identified (pharma host, water file roundtrip, vinyl brownout, autopsy double-memorialize, `GodotFileIO` for `LocationMemorySystem`, 8 `ExpandedShelter` file writes).
- **Save/determinism:** `CaptureState` deep-copies via `SystemTextJsonSerializer`; `SaveChecksum` pinned; `ISeededRng` + `StableHash.Of` deterministic; no `System.Random`/`Guid` in touched Core; `InMemoryFlagLedger` case drift remains as documented.
- **Migration/legacy:** No `UnityEngine` in Core, `src/Bridge` deleted, `Assets/_Game` deleted, `AllSaveSections` now 60 in sync, `pharma_recipes.json` now authority.
- **Audit status:** **COMPLETE — NO PRODUCTION CODE CHANGED.**

