# PLAN-AQUIFER-MONITORING-TRUTH-164 — Piezometry, Drawdown & Well Sustainability

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WATER-AGRICULTURE-46, PLAN-DEEP-STRATA-83, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-AQUIFER-MONITORING-TRUTH-164_APPENDIX-A_SCAFFOLD.md`](PLAN-AQUIFER-MONITORING-TRUTH-164_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-FLUID-LOGISTICS-TRUTH-179` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new well content, no geology simulation (Plan 83), no water
distribution (Plan 46 owns it).

## 1. Outcome
`Shelter/AquiferPiezometerEngine.cs` (829 lines) is reachable and unaddressed:
it reads groundwater level against extraction. Without a stated contract it is
either decoration or an invisible cap; done right it makes water a **stock**
with a visible trend and a sustainability rule.

| Deliverable | Detail |
|---|---|
| Aquifer state | level per well/aquifer with recharge (seeded, weather-linked via Plan 28) and drawdown from extraction |
| Piezometer truth | the engine's reading equals the stored level; display reads it, panels do not estimate |
| Sustainability rule | extraction beyond recharge lowers level; documented thresholds produce visible consequences (yield drop, dry well) |
| Deep coupling | deeper wells from Plan 83 draw from deeper strata with separate levels; no shared pool |
| Save truth | levels restore; a load mid-drawdown continues the same trend |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs` (829 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 46 owns water sources/distribution; this plan supplies their stock truth.
- Plan 83 owns deep strata; deep-well extraction attaches there.
- `SaveSectionRegistry`: `deep_well` section exists (built deep-well pump state, yield ledger per its description) — this plan extends that seam.

## 3. Packages
- **AQM-164A** aquifer state model + recharge/drawdown table.
- **AQM-164B** piezometer reading fidelity test (engine value = stored value).
- **AQM-164C** sustainability thresholds + yield-drop/dry-well fixtures.
- **AQM-164D** deep-strata separation test (two wells, two levels).
- **AQM-164E** save round-trip mid-drawdown.

## 4. Acceptance & verification
- Level equals the engine reading at every checkpoint; no panel estimation.
- Over-extraction produces the documented consequence at threshold; recharge restores per rule.
- Deep and shallow wells never share a level.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Invisible cap → thresholds and trends are surfaced through existing water panels.
Weather coupling drift → recharge reads Plan 28 state; no private weather copy.

---

## 6. Expanded census (4 files · 1,278 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Demo 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AquiferPiezometerEngine.cs` | 829 | System | **yes** | 1 | 0 | 2 |
| `GeothermalAquiferHeadlessDemo.cs` | 119 | Demo | — | 0 | 0 | 1 |
| `GeothermalAquiferState.cs` | 27 | DTO/Type | — | 0 | 0 | 0 |
| `GeothermalAquiferSystem.cs` | 303 | System | — | 0 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `piezometer_network_catalog.json` | object[11 keys] |

**State surfaces:** `AquiferPiezometerEngine.cs`, `GeothermalAquiferHeadlessDemo.cs`, `GeothermalAquiferSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 4 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

Domain files: 4. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` | 4 |
| `PLAN-DEEP-STRATA-83` | 3 |
| `PLAN-GEOTHERMAL-PLANT-TRUTH-191` | 3 |
| `PLAN-ENERGY-NUCLEAR-48` | 2 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AQM-164A` | `GeothermalAquiferState.cs`, `AquiferPiezometerEngine.cs`, `GeothermalAquiferHeadlessDemo.cs` |
| `AQM-164B` | `AquiferPiezometerEngine.cs` |
| `AQM-164C` | no name match — resolve at claim time |
| `AQM-164D` | no name match — resolve at claim time |
| `AQM-164E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **5** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/GeothermalAquiferHostSession.cs`, `src/Host/GeothermalAquiferSaveStore.cs`, `src/Host/PiezometerHostSession.cs`, `src/Main.Piezometer.cs`, `src/Main.ShelterInfrastructure.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Shelter/GeothermalAquiferIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/GeothermalAquiferSystemTests.cs`, `Ashfall.Core.Tests/Water/Plan189IntakeAdvisoryBridgeTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **3**; isolated: **1**.

| From | → To |
|---|---|
| `GeothermalAquiferHeadlessDemo` | `GeothermalAquiferSystem` |
| `GeothermalAquiferState` | `GeothermalAquiferSystem` |
| `GeothermalAquiferSystem` | `GeothermalAquiferState` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `geothermal_aquifer` |
| `geothermal_orc` |
| `piezometer_network` |

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

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/geothermal_drilling_depths.json` |
| `Assets/StreamingAssets/Data/geothermal_strata_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_borehole_logs.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_well_logs.json` |
| `Assets/StreamingAssets/Data/piezometer_network_catalog.json` |

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
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/GeothermalAquiferHostSession.cs` |
| `src/Host/GeothermalAquiferSaveStore.cs` |
| `src/Host/PiezometerHostSession.cs` |
| `src/Host/PiezometerSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.Piezometer.cs` |
| `src/UI/AquiferTreatyConcessionPanel.cs` |
| `src/UI/GeothermalAquiferPanel.cs` |
| `src/UI/GeothermalSteamTurbinePanel.cs` |
| `src/UI/SceneBindingHeadlessProbe.cs` |
| `src/YearOfAsh/GeothermalHeatingWidget.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `geothermal_aquifer` | no |
| `geothermal_orc` | no |
| `piezometer_network` | no |

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
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 12 · catalogs 10 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AQUIFER-MONITORING-TRUTH-164
wave: 13
status: PROPOSED — foreman claim required
packages: AQM-164A, AQM-164B, AQM-164C, AQM-164D, AQM-164E
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferHostSession.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferSaveStore.cs  # §19 candidate host surface
  - src/Host/PiezometerHostSession.cs  # §19 candidate host surface
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
