# PLAN-GEOTHERMAL-PLANT-TRUTH-191 — Heat Source, Working Fluid & Baseload Output

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ENERGY-NUCLEAR-48, PLAN-DEEP-STRATA-83, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-GEOTHERMAL-PLANT-TRUTH-191_APPENDIX-A_SCAFFOLD.md`](PLAN-GEOTHERMAL-PLANT-TRUTH-191_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no grid balance rewrite (Plan 48), no geological strata model
(Plan 83), no new plant catalog.

## 1. Outcome
`Shelter/GeothermalOrcSystem.cs` (**556 lines**) is reachable and unaddressed:
a geothermal heat source driving an organic-Rankine-cycle generator. It is the
one baseload option that depends on a **drilled well** (Plan 83) and a working
fluid that degrades — neither relationship stated, so the plant is either
free infinite power or inert.

| Deliverable | Detail |
|---|---|
| Source model | heat availability derives from the deep well's state (Plan 83); no plant without a source |
| Output curve | generation depends on source temperature and fluid condition; a degraded fluid lowers output, never silently |
| Fluid upkeep | working fluid is an inventory item consumed/lost at documented rates; conservation via Plan 93 |
| Grid interface | output enters Plan 48's grid balance; the plant never feeds a private load |
| Save truth | plant state and fluid volume restore; output resumes at the same condition |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs` (556 lines; unaddressed — Wave 13/15 audit).
- Plan 83 owns deep strata and wells; Plan 48 the grid.
- Plan 119 supplies plant decay; Plan 93 verifies fluid transfers.
- `SaveSectionRegistry`: `deep_well` section already tracks the well side.

## 3. Packages
- **GOT-191A** source model + dependency on Plan 83 state.
- **GOT-191B** output curve + degraded-fluid fixture.
- **GOT-191C** fluid upkeep + conservation check.
- **GOT-191D** grid interface test with Plan 48.
- **GOT-191E** save round-trip; output resumes at stored condition.

## 4. Acceptance & verification
- No well → no output; degraded fluid lowers output per curve.
- Output appears in Plan 48's balance exactly once.
- Fluid consumption balances; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Infinite power → source dependency and output curve are fixtures.
Fluid as a hidden resource → upkeep is visible in the plant view and inventory.

---

## 6. Expanded census (6 files · 1,097 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Loader 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `GeothermalAquiferHeadlessDemo.cs` | 119 | Demo | — | 0 | 0 | 1 |
| `GeothermalAquiferState.cs` | 27 | DTO/Type | — | 0 | 0 | 0 |
| `GeothermalAquiferSystem.cs` | 303 | System | — | 0 | 0 | 2 |
| `GeothermalCatalog.cs` | 58 | Catalog | — | 0 | 0 | 0 |
| `GeothermalCatalogLoader.cs` | 34 | Loader | — | 0 | 0 | 0 |
| `GeothermalOrcSystem.cs` | 556 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `geothermal_drilling_depths.json` | object[2 keys] |
| `geothermal_strata_catalog.json` | object[2 keys] |
| `geothermal_borehole_logs.json` | array[7] |
| `geothermal_steam_vent_diagnostics.json` | array[7] |
| `geothermal_steam_well_logs.json` | array[8] |

**State surfaces:** `GeothermalAquiferHeadlessDemo.cs`, `GeothermalAquiferSystem.cs`, `GeothermalOrcSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 8 name references across the test tree |
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

Domain files: 6. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DEEP-STRATA-83` | 6 |
| `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` | 6 |
| `PLAN-AQUIFER-MONITORING-TRUTH-164` | 3 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 3 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `GOT-191A` | `GeothermalAquiferState.cs` |
| `GOT-191B` | no name match — resolve at claim time |
| `GOT-191C` | no name match — resolve at claim time |
| `GOT-191D` | no name match — resolve at claim time |
| `GOT-191E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **3**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `GeothermalAquiferHeadlessDemo` | `GeothermalAquiferSystem` |
| `GeothermalAquiferState` | `GeothermalAquiferSystem` |
| `GeothermalAquiferSystem` | `GeothermalAquiferState` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `GeothermalAquiferSystem` | 2 |
| `GeothermalAquiferState` | 1 |
| `GeothermalAquiferHeadlessDemo` | 0 |
| `GeothermalCatalog` | 0 |
| `GeothermalCatalogLoader` | 0 |
| `GeothermalOrcSystem` | 0 |

**Class split:** hub 2 · sink 0 · source 1 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **4** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/GeothermalAquiferHostSession.cs`, `src/Host/GeothermalAquiferSaveStore.cs`, `src/Host/Plans74To77HostSessions.cs`, `src/Main.ShelterInfrastructure.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Plans74To77SystemsTests.cs`, `Ashfall.Core.Tests/Shelter/GeothermalAquiferIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/GeothermalAquiferSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `geothermal_aquifer` |
| `geothermal_orc` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/geothermal_drilling_depths.json` |
| `Assets/StreamingAssets/Data/geothermal_strata_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_borehole_logs.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_well_logs.json` |

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

Host files (`src/`) whose names share a domain token: **12**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/GeothermalAquiferHostSession.cs` |
| `src/Host/GeothermalAquiferSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/UI/AquiferTreatyConcessionPanel.cs` |
| `src/UI/GeothermalAquiferPanel.cs` |
| `src/UI/GeothermalSteamTurbinePanel.cs` |
| `src/UI/PanelSceneLoader.cs` |
| `src/UI/SceneBindingHeadlessProbe.cs` |
| `src/YearOfAsh/GeothermalHeatingWidget.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `geothermal_aquifer` | no |
| `geothermal_orc` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `geothermal_drilling_depths.json` | UNRESOLVED |
| `narrative/geothermal_borehole_logs.json` | CODEX_ONLY |
| `narrative/geothermal_steam_vent_diagnostics.json` | CODEX_ONLY |
| `narrative/geothermal_steam_well_logs.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 12 · catalogs 9 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-GEOTHERMAL-PLANT-TRUTH-191
wave: 15
status: PROPOSED — foreman claim required
packages: GOT-191A, GOT-191B, GOT-191C, GOT-191D, GOT-191E
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferHostSession.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/geothermal_drilling_depths.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/geothermal_strata_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --ice-road-tick-demo
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
