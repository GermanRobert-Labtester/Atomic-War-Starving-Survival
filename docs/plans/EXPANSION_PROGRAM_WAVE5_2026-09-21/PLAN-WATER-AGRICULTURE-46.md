# PLAN-WATER-AGRICULTURE-46 — Watersheds, Irrigation, Soil & Closed-Loop Farming

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-FOOD-CUISINE-39, PLAN-SHELTER-ARCHITECTURE-40,
PLAN-VERTICAL-BODY-INDUSTRY-05.
**Expanded appendix:** [`PLAN-WATER-AGRICULTURE-46_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-WATER-AGRICULTURE-46_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's water & agriculture
systems (3 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real hydrology models, no second water store
(`WaterTreatmentSystem`/`FluidInfrastructure` remain canonical), no GMO claims.

---

## 1. Outcome

Water is the game's second pressure and its systems are authored but scattered:
`WaterTreatmentSystem` (with intake advisory bridge), `DeepWellSystem`,
`BrineWaterSystem`, `FluidInfrastructure`, `SumpFloodingSystem`, aquifer
piezometer engines, `AtmosphericCondenserSystem`, plus farming:
`GreenhouseSystem`, `AgricultureSystem`, `CropStrainCatalog`,
`FungiCultivationSystem`, `ApicultureSystem`, `OilseedPressingEngine`,
`SoilReclamationProfileEngine`, `Aquaponics`, and the host-unreachable
`WaterQualityProfileEngine`/`WaterSourceSystem` projections.

Player loop: **find water → treat it → move it → irrigate → rebuild soil →
close the loop → survive drought or flood**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Sources | deep well, brine, condenser, snow/ice, aquifer | drill, tap, harvest | yield, contamination, depth cost |
| Treatment | `WaterTreatmentSystem`, quality profile | filter, boil, dose | potability, advisories, disease |
| Network | `FluidInfrastructure`, pressure nodes | lay pipes, valve, store | pressure, loss, freeze risk |
| Irrigation | greenhouse/farms, `SoilReclamationProfileEngine` | schedule, drip, reclaim | yield, salinity, soil health |
| Closed loops | aquaponics, compost, fungi, wastewater | cycle nutrients | feed/food/fertilizer yield |
| Resilience | sump, drainage, reservoirs | drain, store, ration | flood recovery, drought buffer |
| Pollination | `ApicultureSystem` | hives | crop yield modifier |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `WaterTreatmentSystem`, `DeepWellSystem`, `BrineWaterSystem`, `AtmosphericCondenserSystem`, `SumpFloodingSystem`, `FluidInfrastructure`, `Water/WaterQualityProfileEngine.cs` (orphan), `Water/WaterSourceSystem.cs` (orphan), `Farming/` (6 files incl. orphans `OilseedPressingEngine`, `SoilReclamationProfileEngine`) |
| Data | `water_sources.json`, `fluid_infrastructure.json`, `hydroponic_crops.json`, `crop_strains.json`, `geothermal_strata_catalog.json`, `sanitation_facilities.json` |
| Sealed prior | `DEBT-189` intake advisory bridge (8/8), Plan 168 water delivery (5/5), Plan 196 food type/temp, Plan 91 greenhouse items, Plan 210 sanitation |
| Contracts | piezometer advisory before WT tick; sump greywater named source; no `WaterSourceSystem` as a store (debt rule) |

---

## 3. Packages

### WA-46A — Source portfolio
- Deep well (depth/hazard), brine (treatment cost), condenser (power/weather),
  ice/snow melt (seasonal), aquifer (piezometer yield) as a single source
  table with yields and quality.
- **Acceptance:** one source authority; alerts before depletion; no parallel
  store; hazard from `SubterraneanSubsidenceEngine` where applicable.
- **Verify:** deep-well/brine focused suites.

### WA-46B — Treatment and quality
- Contamination classes (biological, chemical, radiological), treatment
  efficacy per method, advisory bridge live, and dose consequences routed to
  `RadiationSystem`/`DiseaseSystem`.
- **Acceptance:** potability explainable; treatment consumes filter/energy;
  disease exposure uses the authored `foul_water_draw` path.
- **Verify:** water-treatment/sump bridge suites.

### WA-46C — Network, pressure and freezing
- `FluidInfrastructure` pressure nodes, leakage, valves, storage tanks; cold
  snaps freeze exposed pipes (thermal link) and burst them (maintenance link).
- **Acceptance:** losses explainable; freeze prevention is a real decision;
  repairs use materials.
- **Verify:** fluid + thermal focused suites.

### WA-46D — Irrigation, soil and salinity
- Irrigation schedules per crop; salinity/depletion from brine/effluent use;
  `SoilReclamationProfileEngine` reclaims over time with compost/fallow;
  blight links to ecology.
- **Acceptance:** yield = f(water, soil, light, skill) visible; reclamation
  reversible; no infinite fertility.
- **Verify:** farming suites + balance sim.

### WA-46E — Closed-loop systems
- Aquaponics (fish+plants), fungi (waste→protein), compost (waste→fertilizer),
  wastewater return: each loop has input/output ratios and failure modes.
- **Acceptance:** loops net-positive only when maintained; failures are
  recoverable; every output is a real item.
- **Verify:** aquaponics/fungi focused suites.

### WA-46F — Drought and flood resilience
- Reservoir storage, rationing policy, sump/drainage capacity, flood-proofing;
  drought reduces yields while flood risks stores and disease.
- **Acceptance:** buffers are sized and visible; a bad season is survivable
  with preparation; no random unfair wipe.
- **Verify:** sump + weather focused suites.

### WA-46G — Content volumes
- +6 water sources, +10 crop strains, +6 loop recipes, +6 reclamation profiles,
  +4 contingency events; fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Water becomes trivially solved | multiple sources with distinct costs; contamination and freezing persist |
| Loop balance breaks economy | authored ratios + balance sim before tuning |
| Overlap with Plan 40 fluids | one owner (`FluidInfrastructure`); Plan 40 adds rooms, this adds sources |
| Drought feels unfair | forecast visibility + storage decisions + difficulty presets |

## 5. Verification

```bash
godot --headless --path . -- --deep-well-selftest
godot --headless --path . -- --aquaponics-selftest
godot --headless --path . -- --agriculture-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Farming/
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan168WaterDeliveryTests.cs
```

---

## 6. Expanded census (4 files · 1,099 lines)

Scope: `Assets/Ashfall.Core/Water/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `OilseedPressingEngine.cs` | 246 | System | — | 0 | 0 | 0 |
| `SoilReclamationProfileEngine.cs` | 216 | System | **yes** | 0 | 0 | 0 |
| `WaterQualityProfileEngine.cs` | 203 | System | **yes** | 0 | 0 | 0 |
| `WaterSourceSystem.cs` | 434 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `water_sources.json` | object[3 keys] |
| `boiler_feedwater_deaerator_audits.json` | array[7] |
| `deckle_mould_watermark_audits.json` | array[8] |
| `steam_trap_water_hammer_logs.json` | array[7] |
| `sweet_water_glycerin_assays.json` | array[7] |
| `water_clock_orifice_silt_records.json` | array[7] |

**State surfaces:** `WaterSourceSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Water/` |
| Test references | 5 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 4. Other plans referencing them: **6**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 4 |
| `EVIDENCE` | 4 |
| `PLAN-FOOD-CUISINE-39` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-PRESERVATION-TRUTH-118` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `WA-46A` | `WaterSourceSystem.cs` |
| `WA-46B` | `WaterQualityProfileEngine.cs` |
| `WA-46C` | no name match — resolve at claim time |
| `WA-46D` | no name match — resolve at claim time |
| `WA-46E` | no name match — resolve at claim time |
| `WA-46F` | no name match — resolve at claim time |
| `WA-46G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **0** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Emergency/Plan194EmergencyAlertIntegrationTests.cs`, `Ashfall.Core.Tests/Farming/OilseedPressingTests.cs`, `Ashfall.Core.Tests/Farming/SoilReclamationProfileEngineTests.cs`, `Ashfall.Core.Tests/Water/Plan189WaterSourceIntegrationTests.cs`, `Ashfall.Core.Tests/Water/WaterQualityProfileEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **0**; isolated: **4**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `agriculture` |
| `water_condenser` |
| `water_treatment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--agriculture-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnWaterStateChanged` | `Assets/Ashfall.Core/BrineWaterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json` |
| `Assets/StreamingAssets/Data/narrative/sweet_water_glycerin_assays.json` |
| `Assets/StreamingAssets/Data/narrative/water_clock_orifice_silt_records.json` |
| `Assets/StreamingAssets/Data/narrative/water_quality_test_reports_batch_2.json` |
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

Host files (`src/`) whose names share a domain token: **9**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/WaterCondenserHostSession.cs` |
| `src/Host/WaterCondenserSaveStore.cs` |
| `src/Host/WaterTreatmentHostSession.cs` |
| `src/Host/WaterTreatmentSaveStore.cs` |
| `src/Main.WaterCondenser.cs` |
| `src/UI/WaterTreatmentPanel.cs` |
| `src/UI/WaterTreatmentPanelContent.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `water_condenser` | no |
| `water_treatment` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 4).

| Catalog | Classification |
|---|---|
| `narrative/steam_trap_water_hammer_logs.json` | CODEX_ONLY |
| `narrative/sweet_water_glycerin_assays.json` | CODEX_ONLY |
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

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 3 (laddered 0) · RNG streams 3 · host files 12 · catalogs 10 · test regions 1 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WATER-AGRICULTURE-46
wave: —
status: PROPOSED — foreman claim required
packages: WA-46A, WA-46B, WA-46C, WA-46D, WA-46E, WA-46F, WA-46G
claim paths:
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/WaterCondenserHostSession.cs  # §19 candidate host surface
  - src/Host/WaterCondenserSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Water/
  - godot --headless --path . -- --agriculture-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
