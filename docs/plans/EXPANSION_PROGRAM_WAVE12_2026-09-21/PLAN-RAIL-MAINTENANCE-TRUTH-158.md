# PLAN-RAIL-MAINTENANCE-TRUTH-158 — Track Wear, Closures & Repair Crews

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-MAINTENANCE-DECAY-TRUTH-119, PLAN-SPATIAL-SIM-AUTHORITY-95.
**Implementation scaffold:** [`PLAN-RAIL-MAINTENANCE-TRUTH-158_APPENDIX-A_SCAFFOLD.md`](PLAN-RAIL-MAINTENANCE-TRUTH-158_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-MAINTENANCE-DECAY-TRUTH-119` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no rail content, no travel resolution (Plan 30), no generic decay
model (Plan 119 owns the shared contract).

## 1. Outcome
`Rail/RailTrackMaintenanceEngine.cs` is host-unreachable (Plan 1 Appendix A) and
the only rail-maintenance authority. Rail is the one transport mode where
neglect has a geographic consequence — a worn segment closes a corridor, not
just an item. No contract states wear rates, closure thresholds, or how crews
restore a line.

| Deliverable | Detail |
|---|---|
| Segment model | rail segments from Plan 95's route edges with condition per segment |
| Wear rules | wear accrues from traffic and weather (Plan 28) per the Plan 119 contract, not a private rate table |
| Closure threshold | at/below threshold a segment is closed to traffic; the closure is visible and blocks the route leg |
| Repair crews | crew + material consumption restores condition per documented action; conservation via Plan 93 |
| Recovery path | a closed corridor reopens at its current (repaired) condition, not fully healed |

## 2. Evidence
- Plan 1 Appendix A: `RailTrackMaintenanceEngine` host-unreachable; Appendices H/K size and surface it.
- Plan 119 supplies the shared decay contract this plan instantiates for rail.
- Plan 95 owns route edges segments attach to.
- Plan 30 owns travel that consumes closure state.

## 3. Packages
- **RMT-158A** segment model + attachment check.
- **RMT-158B** wear rules via Plan 119's contract (no private table).
- **RMT-158C** closure threshold + route-block test.
- **RMT-158D** repair crew path + material conservation.
- **RMT-158E** recovery-at-current-condition test.

## 4. Acceptance & verification
- Wear follows the shared contract; a difficulty multiplier change affects it identically to other classes.
- A closed segment blocks through-travel and shows in the route view; reopening uses current condition.
- Repair consumes materials and balances in the conservation wrapper.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Rail/` (create if absent).

## 5. Risks
Private decay table → forbidden by contract; the difficulty binding test catches divergence.
Closure deadlock → closure has a visible state and at least one repair path; the fixture proves it.

---

## 6. Expanded census (1 files · 243 lines)

Scope: `Assets/Ashfall.Core/Rail/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `RailTrackMaintenanceEngine.cs` | 243 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `rail_grinding_catalog.json` | object[4 keys] |
| `rail_logistics_catalog.json` | object[2 keys] |
| `rail_network.json` | object[4 keys] |
| `railway_interlock_catalog.json` | object[6 keys] |
| `rerailing_equipment_catalog.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Rail/` |
| Test references | 1 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RMT-158A` | no name match — resolve at claim time |
| `RMT-158B` | no name match — resolve at claim time |
| `RMT-158C` | no name match — resolve at claim time |
| `RMT-158D` | no name match — resolve at claim time |
| `RMT-158E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 7. Host files: **2** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.Plans146_149.cs`, `src/Main.Plans190_193.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Expeditions/RailLogisticsPlan73Tests.cs`, `Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs`, `Ashfall.Core.Tests/Rail/RailTrackMaintenanceEngineTests.cs`, `Ashfall.Core.Tests/RailwayInterlockEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `equipment` |
| `equipment_condition` |
| `fluid_logistics` |
| `maintenance` |
| `piezometer_network` |
| `rail_grinding` |
| `railway` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--rail-grinding-selftest` |
| `--rail-grinding-uitest` |
| `--real-main-journey-selftest` |
| `--rumor-network-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnEquipmentChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnMaintenanceCompleted` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/piezometer_network_catalog.json` |
| `Assets/StreamingAssets/Data/pneumatic_network_catalog.json` |
| `Assets/StreamingAssets/Data/rail_grinding_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (106 files, 897 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Equipment` | 1 | 4 |
| `Excavation` | 1 | 5 |
| `Greenhouse` | 1 | 17 |
| `Rail` | 1 | 5 |
| `Shelter` | 87 | 754 |

**Verdict:** 897 cases sit under matching regions — run those first (`Audio`, `Combat`, `Equipment`, `Excavation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **218**
(25 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/FluidLogisticsHostSession.cs` |
| `src/Host/FluidLogisticsSaveStore.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **27**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `caravan_trade_network` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `excavation` | no |
| `excavation_hazards` | no |
| `expanded_shelter` | no |
| `fluid_logistics` | no |
| `maintenance` | no |
| `piezometer_network` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `acoustic_detection` |
| `anomaly_hazard` |
| `route_engineering_rail_grinding` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **127**
(CODEX_ONLY 114, GAMEPLAY_CONSUMED 4, OPTIONAL 1, UNRESOLVED 8).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `excavation_sites.json` | UNRESOLVED |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/artesian_well_contamination_logs.json` | CODEX_ONLY |
| `narrative/bark_tanning_vat_logs.json` | CODEX_ONLY |

**Verdict:** 8 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 27 (laddered 0) · RNG streams 4 · host files 16 · catalogs 22 · test regions 7 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RAIL-MAINTENANCE-TRUTH-158
wave: 12
status: PROPOSED — foreman claim required
packages: RMT-158A, RMT-158B, RMT-158C, RMT-158D, RMT-158E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/breaching_equipment_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/excavation_hazard_mitigation.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --plans-122-125-balance-soak
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
