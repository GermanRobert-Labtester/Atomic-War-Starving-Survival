# PLAN-FLUID-LOGISTICS-TRUTH-179 — Pipes, Tanks & Flow Balance

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WATER-AGRICULTURE-46, PLAN-AQUIFER-MONITORING-TRUTH-164, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no water-quality model (Plan 46), no aquifer stock (Plan 164), no
new pipe catalog.

## 1. Outcome
`Shelter/FluidLogisticsSystem.cs` (**695 lines**) is reachable and unaddressed:
the connective tissue between sources (Plan 164), storage, and consumers. Flow
systems fail silently — a break leaks until a tank empties — unless the
contract states balance, pressure, and leak discovery.

| Deliverable | Detail |
|---|---|
| Network model | nodes (sources, tanks, consumers) and edges (pipes) from built state; no independent map |
| Flow balance | inflow = outflow + storage change + loss; a balance test over a scripted day |
| Pressure/priority | documented service priority (drinking > sanitation > production) during shortage; visible to the player |
| Leaks and breaks | condition per edge via Plan 119's contract; a broken edge loses fluid until repaired and is discoverable |
| Save truth | volumes and edge conditions restore; a load never redistributes fluid |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs` (695 lines; unaddressed — Wave 13 audit).
- Plan 164 owns source stock feeding the network; Plan 46 owns quality.
- Plan 119 supplies edge condition decay.
- Plan 93 verifies fluid items where fluid is carried as inventory.

## 3. Packages
- **FLT-179A** network model + node/edge table.
- **FLT-179B** flow balance test + deliberate leak fixture.
- **FLT-179C** priority table + shortage fixture.
- **FLT-179D** condition/break path + repair consumption.
- **FLT-179E** save round-trip; no redistribution on load.

## 4. Acceptance & verification
- Balance closes within tolerance over a scripted day; a leak shows as loss with a source edge.
- Priority order holds under shortage in the fixture.
- Save/load preserves volumes exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Silent leaks → leak loss appears in the balance report and edge condition.
Second map → nodes/edges reference built state; the attachment check enforces it.

---

## 6. Expanded census (4 files · 1,506 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FluidDeliveryApplicator.cs` | 137 | Support | — | 0 | 0 | 0 |
| `FluidLogisticsSystem.cs` | 695 | System | **yes** | 0 | 0 | 2 |
| `FluidWaterTreatmentBridge.cs` | 46 | Support | — | 0 | 0 | 0 |
| `PneumaticDispatchSystem.cs` | 628 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `fluid_infrastructure.json` | object[4 keys] |
| `microfluidic_diagnostic_catalog.json` | object[4 keys] |
| `pneumatic_network_catalog.json` | object[5 keys] |
| `pneumatic_carrier_capsule_logs.json` | array[8] |
| `pneumatic_cylinder_leather_assays.json` | array[7] |
| `pneumatic_tube_diverter_audits.json` | array[8] |

**State surfaces:** `FluidLogisticsSystem.cs`, `PneumaticDispatchSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 6 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 2 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FLT-179A` | no name match — resolve at claim time |
| `FLT-179B` | no name match — resolve at claim time |
| `FLT-179C` | no name match — resolve at claim time |
| `FLT-179D` | no name match — resolve at claim time |
| `FLT-179E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/FluidLogisticsHostSession.cs`, `src/Host/Plans74To77HostSessions.cs`, `src/Main.Piezometer.cs`, `src/Main.Plans166_169.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs`, `Ashfall.Core.Tests/Plan168FluidLogisticsTests.cs`, `Ashfall.Core.Tests/Plan168WaterDeliveryTests.cs`, `Ashfall.Core.Tests/Plans74To77SystemsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **3**; isolated: **1**.

| From | → To |
|---|---|
| `FluidDeliveryApplicator` | `FluidLogisticsSystem` |
| `FluidDeliveryApplicator` | `FluidWaterTreatmentBridge` |
| `FluidWaterTreatmentBridge` | `FluidLogisticsSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `fluid_logistics` |
| `pneumatic_dispatch` |
| `water_condenser` |
| `water_treatment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnTreatmentCompleted` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnWaterStateChanged` | `Assets/Ashfall.Core/BrineWaterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/fluid_infrastructure.json` |
| `Assets/StreamingAssets/Data/narrative/pneumatic_carrier_capsule_logs.json` |
| `Assets/StreamingAssets/Data/narrative/pneumatic_cylinder_leather_assays.json` |
| `Assets/StreamingAssets/Data/narrative/pneumatic_tube_diverter_audits.json` |
| `Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json` |
| `Assets/StreamingAssets/Data/narrative/sweet_water_glycerin_assays.json` |
| `Assets/StreamingAssets/Data/narrative/timber_creosote_treatment_logs.json` |
| `Assets/StreamingAssets/Data/narrative/water_clock_orifice_silt_records.json` |
| `Assets/StreamingAssets/Data/narrative/water_quality_test_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/pneumatic_network_catalog.json` |
| `Assets/StreamingAssets/Data/rail_logistics_catalog.json` |
| `Assets/StreamingAssets/Data/water_sources.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (5 files, 37 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Water` | 5 | 37 |

**Verdict:** 37 cases sit under matching regions — run those first (`Water`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **12**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/FluidLogisticsHostSession.cs` |
| `src/Host/FluidLogisticsSaveStore.cs` |
| `src/Host/HoldfastDispatchLog.cs` |
| `src/Host/WaterCondenserHostSession.cs` |
| `src/Host/WaterCondenserSaveStore.cs` |
| `src/Host/WaterTreatmentHostSession.cs` |
| `src/Host/WaterTreatmentSaveStore.cs` |
| `src/Main.WaterCondenser.cs` |
| `src/Settings/KeyBindingApplicator.cs` |
| `src/UI/HeavyLogisticsAirlockPanel.cs` |
| `src/UI/WaterTreatmentPanel.cs` |
| `src/UI/WaterTreatmentPanelContent.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `fluid_logistics` | no |
| `pneumatic_dispatch` | no |
| `water_condenser` | no |
| `water_treatment` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 8).

| Catalog | Classification |
|---|---|
| `narrative/pneumatic_carrier_capsule_logs.json` | CODEX_ONLY |
| `narrative/pneumatic_cylinder_leather_assays.json` | CODEX_ONLY |
| `narrative/pneumatic_tube_diverter_audits.json` | CODEX_ONLY |
| `narrative/steam_trap_water_hammer_logs.json` | CODEX_ONLY |
| `narrative/sweet_water_glycerin_assays.json` | CODEX_ONLY |
| `narrative/timber_creosote_treatment_logs.json` | CODEX_ONLY |
| `narrative/water_clock_orifice_silt_records.json` | CODEX_ONLY |
| `narrative/water_quality_test_reports_batch_2.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

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
**Surface:** save sections 4 (laddered 0) · RNG streams 0 · host files 12 · catalogs 20 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FLUID-LOGISTICS-TRUTH-179
wave: 14
status: PROPOSED — foreman claim required
packages: FLT-179A, FLT-179B, FLT-179C, FLT-179D, FLT-179E
claim paths:
  - src/Host/FluidLogisticsHostSession.cs  # §19 candidate host surface
  - src/Host/FluidLogisticsSaveStore.cs  # §19 candidate host surface
  - src/Host/HoldfastDispatchLog.cs  # §19 candidate host surface
  - src/Host/WaterCondenserHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/fluid_infrastructure.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/pneumatic_carrier_capsule_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Water/
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
