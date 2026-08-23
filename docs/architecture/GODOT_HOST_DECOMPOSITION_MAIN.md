# Main.cs Decomposition Plan — GAP-ARCH-01

**Date:** 2026-08-23
**Status:** PROPOSED (not implemented — requires separate approved task)
**Method:** Read-only forensic decomposition (ashfall-decompose-godot)

---

## 1. Current State

**8,431 lines across 22 partial files** in `src/Main*.cs`.

| File | Lines | Domain | Setup | Save | Flush |
|------|------:|--------|:-----:|:----:|:-----:|
| Main.cs | 831 | Core orchestration, fields, dispatch | — | — | — |
| Main.SaveOrchestrator.cs | 234 | SaveAll coordination | — | 1 | — |
| Main.ExpandedShelterSystems.cs | 525 | 21 shelter systems | 1 | 1 | — |
| Main.Holdfast.cs | 935 | Holdfast + Duty Roster + Ice Road | 4 | 4 | 3 |
| Main.UiTests.cs | 1,178 | 31 headless self-tests | — | — | — |
| Main.UiPanels.cs | 680 | Panel creation + wiring | — | — | — |
| Main.GameFlow.cs | 544 | Game flow state machine | — | — | — |
| Main.YearOfAsh.cs | 427 | Year of Ash | 1 | 1 | 1 |
| Main.World.cs | 405 | World/Crafting/Greenhouse/Power | 7 | 7 | 2 |
| Main.Muster.cs | 345 | Muster | 1 | 1 | — |
| Main.Phase0.cs | 324 | Phase0 + Dose + Phantom | 3 | 3 | 2 |
| Main.Expeditions.cs | 262 | Expeditions/Combat/Wasteland | 5 | 4 | 2 |
| Main.Economy.cs | 247 | Economy/Caravans/Foundry | 3 | 3 | 2 |
| Main.Narrative.cs | 196 | Narrative/Journal/Radio | 4 | 3 | 2 |
| Main.Verdict.cs | 195 | Verdict | 1 | 1 | 1 |
| Main.Medical.cs | 195 | Medical/Disease/Ward | 3 | 3 | 1 |
| Main.Maritime.cs | 166 | Maritime/DeepCoast | 2 | 1 | 1 |
| Main.Survivors.cs | 138 | Survivors/UtilityAI | 2 | 1 | — |
| Main.PanelLifecycle.cs | 130 | Panel open/close | — | — | — |
| Main.Inventory.cs | 123 | Inventory | 1 | 1 | — |
| Main.UiHandlers.cs | 351 | UI event handlers | — | — | — |

**Field declarations:** 141 `private` fields in Main.cs (lines 40-272), plus ~45 in ExpandedShelterSystems.cs.

**Triad totals:** 41 Setup / 34 Save + SaveAll / 18 Flush.

**Cross-domain dependencies:** 25+ Setup methods reference fields from other partial files.

---

## 2. Problems Identified

### P1 — Main.cs field bloat (831 lines, 141 fields)
Main.cs declares all 141 fields in one block, even though each field belongs to a specific domain partial. This makes Main.cs a navigation bottleneck — you must scroll through 270+ lines of field declarations to reach `_Ready()`.

### P2 — Main.Holdfast.cs is the largest partial (935 lines)
Contains 4 unrelated subsystems: Holdfast runtime, Duty Roster, Expansion Hub, and Ice Road. Each has its own Setup/Save/Flush triad but they share one file.

### P3 — Main.ExpandedShelterSystems.cs mega-method (525 lines)
`SetupExpandedShelterSystems()` constructs all 21 systems in one 250-line method. `SaveAllExpandedShelterSystems()` is similarly monolithic. A bug in one system's setup risks the entire block.

### P4 — Main.UiTests.cs is not orchestration (1,178 lines)
31 self-test methods live in the Main partial class but are not part of the game's runtime orchestration. They're headless integration tests invoked via CLI flags.

### P5 — Main.UiPanels.cs mixes creation and lifecycle (680 lines)
Panel instantiation, AddChild wiring, and visibility toggling are interleaved with panel open/close handlers.

---

## 3. Proposed Extraction Plan

### Phase 1 — Move fields to domain partials (reduces Main.cs by ~230 lines)

Move each field declaration from Main.cs into the partial file that uses it.

| Target file | Fields to move | Count |
|---|---|---|
| Main.YearOfAsh.cs | `_yearOfAsh`, `_doorModal`, `_questlineModal`, `_factionWarMap`, `_radioTerminal`, `_geothermalWidget`, `_radonWidget`, `_yearOfAshPanel` | 8 |
| Main.Phase0.cs | `_phantomMemory`, `_phase0`, `_doseLedger`, `_doseSurface` | 4 |
| Main.Muster.cs | `_muster`, `_currentsRoster`, `_approachModal`, `_campWidget`, `_witnessPanel` | 5 |
| Main.Verdict.cs | `_verdict`, `_verdictReadoutLabel`, `_verdictPanel` | 3 |
| Main.Maritime.cs | `_maritime`, `_deepCoast` | 2 |
| Main.Expeditions.cs | `_expeditions`, `_combat`, `_wastelandMap`, `_encounterChoice` | 4 |
| Main.World.cs | `_campaignDay`, `_dailyBriefing`, `_dailyBriefingModal`, `_medicalWard`, `_memorial`, `_powerGrid`, `_greenhouse`, `_startingLevel` | 8 |
| Main.UiPanels.cs | All 40+ panel fields (`_radiationDetailPanel`, `_eventsLogPanel`, etc.) | ~45 |
| Main.Holdfast.cs | Holdfast/DutyRoster/IceRoad fields | ~10 |
| Main.Economy.cs | Economy/Caravan/Foundry fields | ~6 |
| Main.Medical.cs | Medical/Disease fields | ~4 |
| Main.Narrative.cs | Narrative/Journal/Radio fields | ~5 |
| Main.Survivors.cs | Survivor fields | ~3 |
| Main.Inventory.cs | Inventory fields | ~2 |

**Risk:** Low. Field moves are compile-time verified. No runtime behavior changes.
**Rollback:** `git checkout` the affected files.
**Checkpoint:** `dotnet build Ashfall.csproj` must succeed after each batch of moves.

### Phase 2 — Split Main.Holdfast.cs (935 → ~4 files)

| New file | Content | Est. lines |
|---|---|---:|
| Main.Holdfast.cs | Holdfast runtime only (Setup, Save, Flush) | ~300 |
| Main.DutyRoster.cs | Duty Roster triad | ~200 |
| Main.ExpansionHub.cs | Expansion Hub triad | ~200 |
| Main.IceRoad.cs | Ice Road tick demo | ~100 |

**Dependency hazard:** `_shelterAssignment` is created in `SetupExpandedShelterSystems()` but used by `SetupPhase0()` via `BindShelterAssignment()`. The split must preserve the call order: `SetupExpandedShelterSystems()` runs before `SetupPhase0()` in the shelter→Phase0 binding path.

**Risk:** Medium. Cross-file field references need careful ordering.
**Checkpoint:** `dotnet build` + `--holdfast-briefing` + `--duty-roster-save-selftest`.

### Phase 3 — Extract self-tests from Main.UiTests.cs (1,178 lines)

Move the 31 self-test methods into domain-specific test files:

| New file | Methods moved | Count |
|---|---|---:|
| HostTests/YearOfAshTests.cs | `RunYearOfAshSaveSelfTest` | 1 |
| HostTests/DutyRosterTests.cs | `RunDutyRosterSaveSelfTest` | 1 |
| HostTests/ExpansionHubTests.cs | `RunExpansionHubSaveSelfTest` | 1 |
| HostTests/ExpeditionTests.cs | `RunExpeditionSelfTest`, `RunExpeditionEncounterBridgeSelfTest` | 2 |
| HostTests/MedicalTests.cs | `RunMedicalSelfTest` | 1 |
| HostTests/EconomyTests.cs | `RunEconomySelfTest`, `RunCaravanSelfTest` | 2 |
| HostTests/Phase0Tests.cs | `RunPhase0SelfTest` | 1 |
| HostTests/WorldTests.cs | `RunWorldSelfTest`, `RunShelterHazardLoopSelfTest`, `RunShelterOperationsSelfTest` | 3 |
| HostTests/UiTests.cs | `RunUiLayoutSelfTest`, `RunUiSnapshotSelfTest`, `RunJournalWeatherPanelSelfTest` | 3 |
| HostTests/RemainingTests.cs | All other self-tests | ~17 |

**Alternative:** Keep `Main.UiTests.cs` but rename to `HostTests/AllSelfTests.cs` to make it clear these are test methods, not orchestration.

**Risk:** Low. Self-tests are standalone methods with no field dependencies on Main.
**Checkpoint:** `dotnet build` + `--bridge-selftest` + spot-check 3 self-test verbs.

### Phase 4 — Break up SetupExpandedShelterSystems (525 lines)

Split the 250-line mega-method into per-system setup methods:

```
SetupExpandedShelterSystems()  // orchestrator — calls the 21 sub-methods
├── SetupWaterTreatment()
├── SetupAirlockSecurity()
├── SetupSurvivorRelations()
├── SetupRegionalTreaty()
├── ... (one per system)
└── SetupShelterAssignment()   // last — other systems depend on it
```

Each sub-method returns its host session. The orchestrator wires cross-system dependencies (e.g., `_shelterThermal.SetAssignments(_shelterAssignment.System)`).

**Risk:** Medium. The 21 systems have implicit ordering dependencies (e.g., MentalHealthCrisis needs the roster, ShelterThermal needs assignments).
**Checkpoint:** `dotnet build` + `--shelter-operations-selftest` + `--shelter-hazard-loop-selftest`.

---

## 4. Triad Completeness Audit

Every domain partial should have a complete Setup→Save→Flush triad. Gaps:

| Domain | Setup | Save | Flush | Gap |
|---|:---:|:---:|:---:|---|
| Inventory | ✅ | ✅ | ❌ | No dirty flag — saves every time |
| Survivors | ✅ | ✅ | ❌ | No dirty flag |
| Muster | ✅ | ✅ | ❌ | No dirty flag |
| Maritime | ✅ | ✅ | ✅ | Complete |
| ExpandedShelter | ✅ | ✅ | ❌ | SaveAllExpandedShelterSystems saves all 21 every tick |

**Recommendation:** Add dirty flags to Inventory, Survivors, Muster, and ExpandedShelter to match the Flush pattern used by the other 14 domains. Low priority — functional, just wasteful.

---

## 5. Dependency Hazards

| Hazard | Description | Mitigation |
|---|---|---|
| `_shelterAssignment` cross-domain | Created in ExpandedShelter, consumed by Phase0 | Keep `SetupExpandedShelterSystems()` call before `SetupPhase0()` in GameFlow |
| `_journal` shared across 4+ domains | Narrative, Verdict, Maritime, YearOfAsh all write to it | Field stays in Main.Narrative.cs; other partials access via `_journal` |
| `_campaignDay` tick ordering | CampaignDayCoordinator.TickDay() must run after all system Setup | Keep in Main.World.cs; _Ready() ordering preserved |
| SaveAll orchestration | SaveAll() in SaveOrchestrator calls 34 Save methods | Must update call order if any Save method moves to a new file |
| **`TickSimDay()` mega-coupling** | Calls 19 Setup methods, touches fields from every domain (Main.Holdfast.cs:210) | Hardest method to decompose — must preserve all 19 lazy-init calls |
| **`ContinueGame()` ordering** | Calls 23 Setup methods in strict dependency order (Main.SaveOrchestrator.cs:137) | Any decomposition must preserve this exact ordering |
| **`_Notification` shutdown gap** | Only 11 of 34 stores saved on window close — 23 stores at risk of data loss | Independent bug — should be fixed by calling `SaveAll()` from `_Notification` |
| Lazy-init pattern | Every button handler calls `SetupXxx()` before using the field | Creates dense cross-domain call graph; decomposition must preserve idempotency |

---

## 6. Recommended Execution Order

0. **Phase 0** (shutdown gap fix) — Replace `_Notification` handler's partial save with `SaveAll()`. Fixes data loss on window close (23 stores currently unsaved). Trivial one-line fix, highest urgency.
1. **Phase 1** (field moves) — Lowest risk, highest navigation improvement. Do first.
2. **Phase 3** (extract self-tests) — Low risk, reduces Main.UiTests.cs from 1,178 to 0.
3. **Phase 2** (split Holdfast) — Medium risk, but Holdfast is the largest partial.
4. **Phase 4** (break mega-method) — Highest risk, most systems touched. Do last.

Each phase is an independent PR. Each phase has a clear rollback checkpoint.

---

## 7. What This Plan Does NOT Do

- Does not move gameplay logic (that belongs in Core, not in another Godot file).
- Does not change the `partial class Main` pattern — all files remain part of the same class.
- Does not introduce new abstractions or interfaces.
- Does not change save/load behavior or wire contracts.
