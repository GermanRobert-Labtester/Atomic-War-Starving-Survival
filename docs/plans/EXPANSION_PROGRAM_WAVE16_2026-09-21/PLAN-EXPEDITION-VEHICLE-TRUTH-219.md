# PLAN-EXPEDITION-VEHICLE-TRUTH-219 — Field Rigs: Loadout, Range & Breakdown

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no travel resolution (Plan 30), no customization slots (Plan 154),
no armor grades (CF-P6 seam — do not touch).

## 1. Outcome
`ExpeditionVehicleSystem.cs` (**349 lines**, Core root) is reachable and
unaddressed: the rigs that carry expeditions. Plan 154 owns bench
customization; Plan 30 owns leg travel. The **field-side** side — fuel/range,
load capacity for an expedition, route suitability (Plan 95 edges), and
breakdown recovery — is unowned, so a rig is either abstract capacity or a
silent liability.

| Deliverable | Detail |
|---|---|
| Rig model | capacity (cargo, passengers), fuel consumption per leg class, and route suitability by vehicle class |
| Range truth | range derives from fuel + route resistance; an under-fuelled leg is refused with a visible reason, never stranded silently |
| Breakdown | condition (Plan 119) below threshold causes breakdown events with documented field-repair options |
| Loadout linkage | fitted parts (Plan 154) modify capacity/consumption as typed rows, not new fields |
| Save truth | rig state and position-in-transit restore; no teleport or re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` (349 lines; unaddressed — Wave 16 audit).
- Plan 154 supplies fitted-part effects; Plan 30 consumes range/suitability.
- Plan 95 provides route classes for suitability.
- Plan 93 verifies cargo transfers at both ends.

## 3. Packages
- **EVT-219A** rig model + suitability table.
- **EVT-219B** range/refusal tests per route class.
- **EVT-219C** breakdown + field-repair fixtures.
- **EVT-219D** loadout effect rows integration test.
- **EVT-219E** save round-trip incl. in-transit state.

## 4. Acceptance & verification
- Under-range legs are refused before departure; consumption matches the table.
- Breakdowns produce documented repair options; save/load preserves rig state.
- `bash scripts/run_test.sh` on the expedition test region.

## 5. Risks
Silent stranding → pre-departure refusal is a fixture.
Capacity inflation → fitted-part rows are the only modifiers; conservation checks cargo.

---

## 6. Expanded census (14 files · 4,762 lines)

Scope: `Assets/Ashfall.Core/Vehicles/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Demo 1 · Loader 1 · Support 5 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ArmoredCrawlerExpeditionSystem.cs` | 402 | System | — | 0 | 0 | 2 |
| `ExpeditionAggregate.cs` | 116 | Support | — | 0 | 0 | 1 |
| `ExpeditionCatalogLoader.cs` | 130 | Loader | — | 0 | 0 | 0 |
| `ExpeditionEncounterBridge.cs` | 355 | Support | — | 0 | 0 | 0 |
| `ExpeditionHeadlessDemo.cs` | 120 | Demo | — | 0 | 0 | 5 |
| `ExpeditionLootReferenceResolver.cs` | 93 | Support | — | 0 | 0 | 0 |
| `ExpeditionLootValidator.cs` | 115 | Support | — | 0 | 0 | 0 |
| `ExpeditionNavalSystem.cs` | 280 | System | — | 0 | 0 | 0 |
| `ExpeditionSystem.cs` | 1580 | System | — | 0 | 0 | 5 |
| `ExpeditionTravelStretch.cs` | 54 | Support | — | 0 | 0 | 0 |
| `VehicleArmorGradeCatalog.cs` | 155 | Catalog | — | 0 | 0 | 0 |
| `VehicleGarageCatalog.cs` | 62 | Catalog | — | 0 | 0 | 0 |
| `VehicleGarageSystem.cs` | 865 | System | — | 0 | 0 | 2 |
| `VehicleCustomizationSystem.cs` | 435 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `anomalous_expedition_encounters.json` | object[2 keys] |
| `vehicle_modifications.json` | object[2 keys] |
| `vehicles.json` | object[3 keys] |
| `expeditions.json` | object[2 keys] |
| `vehicle_armor_grades.json` | object[3 keys] |
| `vehicle_modules.json` | object[2 keys] |

**State surfaces:** `ArmoredCrawlerExpeditionSystem.cs`, `ExpeditionAggregate.cs`, `ExpeditionHeadlessDemo.cs`, `ExpeditionSystem.cs`, `VehicleGarageSystem.cs`, `VehicleCustomizationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Vehicles/` |
| Test references | 80 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 14. Other plans referencing their names: **11**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-TRANSPORT-EXPEDITION-30` | 14 |
| `PLAN-EXPEDITION-FAMILY-TRUTH-269` | 13 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-MARITIME-DEEPWATER-27` | 1 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `EVT-219A` | no name match — resolve at claim time |
| `EVT-219B` | no name match — resolve at claim time |
| `EVT-219C` | no name match — resolve at claim time |
| `EVT-219D` | no name match — resolve at claim time |
| `EVT-219E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 15; intra-domain edges: **8**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ExpeditionEncounterBridge` | `ExpeditionSystem` |
| `ExpeditionHeadlessDemo` | `ExpeditionSystem` |
| `ExpeditionLootValidator` | `ExpeditionCatalogLoader` |
| `ExpeditionLootValidator` | `ExpeditionLootReferenceResolver` |
| `ExpeditionSystem` | `ExpeditionVehicleSystem` |
| `VehicleGarageSystem` | `ExpeditionVehicleSystem` |
| `VehicleGarageSystem` | `VehicleArmorGradeCatalog` |
| `VehicleGarageSystem` | `VehicleGarageCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ExpeditionSystem` | 2 |
| `ExpeditionVehicleSystem` | 2 |
| `ExpeditionCatalogLoader` | 1 |
| `ExpeditionLootReferenceResolver` | 1 |
| `VehicleArmorGradeCatalog` | 1 |
| `VehicleGarageCatalog` | 1 |
| `ArmoredCrawlerExpeditionSystem` | 0 |
| `ExpeditionAggregate` | 0 |
| `ExpeditionEncounterBridge` | 0 |
| `ExpeditionHeadlessDemo` | 0 |

**Class split:** hub 1 · sink 5 · source 4 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 15. Host files: **36** · Test files: **60** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 36 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ContractorRosterHostSession.cs`, `src/Host/DutyRosterHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 60 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/ContractorRosterSystemTests.cs`, `Ashfall.Core.Tests/CraftingCommandTests.cs`, `Ashfall.Core.Tests/Data/RuntimeJsonBootstrapParityTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `armored_crawlers` |
| `encounter_choice` |
| `expedition` |
| `expedition_stealth` |
| `travel_encounters` |
| `vehicle_garage` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--ice-road-tick-demo` |
| `--patrol-encounter-selftest` |
| `--travel-encounter-selftest` |
| `--vehicle-garage-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **20**.

| Event | First declaration |
|---|---|
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterSelected` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterTriggered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnLootAdded` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/armored_crawler_modules.json` |
| `Assets/StreamingAssets/Data/narrative/armored_cockroach_hive_logs.json` |
| `Assets/StreamingAssets/Data/narrative/armored_locomotive_manifests.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_briefs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_planning_briefs_batch_1.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/naval_vessels.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **21**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/ExpeditionHostSession.cs` |
| `src/Host/ExpeditionSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.ExpeditionPlaytest.cs` |
| `src/Host/HostCli.VehicleGarage.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/TravelEncounterSaveStore.cs` |
| `src/Host/VehicleGarageSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `armored_crawlers` | no |
| `encounter_choice` | no |
| `expedition` | no |
| `expedition_stealth` | no |
| `travel_encounters` | no |
| `vehicle_garage` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `expedition` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **14**
(CODEX_ONLY 9, GAMEPLAY_CONSUMED 1, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/armored_locomotive_manifests.json` | CODEX_ONLY |
| `narrative/expedition_briefs_expansion.json` | CODEX_ONLY |
| `narrative/expedition_field_reports.json` | CODEX_ONLY |
| `narrative/expedition_field_reports_batch_2.json` | CODEX_ONLY |
| `narrative/expedition_planning_briefs_batch_1.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/scavenger_expedition_route_notes.json` | CODEX_ONLY |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 6 (laddered 0) · RNG streams 1 · host files 13 · catalogs 22 · test regions 0 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-EXPEDITION-VEHICLE-TRUTH-219
wave: 16
status: PROPOSED — foreman claim required
packages: EVT-219A, EVT-219B, EVT-219C, EVT-219D, EVT-219E
claim paths:
  - src/Host/ArmoredCrawlerSaveStore.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/EncounterChoiceSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpeditionHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/armored_crawler_modules.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --expedition-encounter-bridge-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
