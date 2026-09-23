# PLAN-PRECISION-OPTICS-TRUTH-220 — Lenses, Grinding Standards & Instrument Output

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-METROLOGY-TRUTH-172, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-INDUSTRY-AUTOMATION-45.
**Non-goals:** no standards set (Plan 172), no quality tiers (Plan 112), no
factory engines (Plan 45).

## 1. Outcome
`Shelter/PrecisionOpticsEngine.cs` (**347 lines**) is reachable and
unaddressed, and `Optics/PrecisionGlassworksOpticsEngine.cs` is host-unreachable
(Plan 1 Appendix A) — a sibling pair. Plan 172 owns measurement standards.
Lens/optics production is the workshop where standards turn into instruments;
without a contract it is either a flat unlock or a dice craft.

| Deliverable | Detail |
|---|---|
| Production model | lens/optic classes with grinding/finishing steps and achievable tolerance from Plan 172's standards |
| Output grades | optics carry Plan 112 tiers; a lower grade degrades the instrument's error band (Plan 168/189/204), never a silent equal |
| Rework | failed lenses are reworkable at a cost, with the failure visible |
| Sibling seam | the host-unreachable glassworks engine's package (Plan 1) consumes this contract; no duplicate production rules |
| Save truth | in-progress grinding and standards state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs` (347 lines; unaddressed — Wave 16 audit).
- Plan 1 Appendix A/H/K: `PrecisionGlassworksOpticsEngine` (Optics/) is host-unreachable — its seal package should bind to this contract.
- Plan 172 supplies tolerance capability; Plan 112 grades outputs.
- Plans 168/189/204 consume instrument error bands.

## 3. Packages
- **POT-220A** production model + step table.
- **POT-220B** tolerance→grade tests at boundaries.
- **POT-220C** rework path + visible failure.
- **POT-220D** Grade→error-band contract tests with Plan 168/189.
- **POT-220E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Grades follow capability; a lower-grade optic widens the instrument band measurably.
- Rework consumes inputs and can succeed; save/load preserves progress.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Flat unlock → tolerance dependency is a fixture.
Duplicate rules with the orphan engine → the seal binds to this contract; no second table.

---

## 6. Expanded census (4 files · 1,401 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PrecisionGlassworksOpticsEngine.cs` | 302 | System | **yes** | 0 | 0 | 0 |
| `PrecisionBroachingCatalog.cs` | 81 | Catalog | — | 0 | 0 | 0 |
| `PrecisionMetrologySystem.cs` | 671 | System | — | 0 | 0 | 2 |
| `PrecisionOpticsEngine.cs` | 347 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `precision_optics_catalog.json` | object[2 keys] |
| `precision_broaching_catalog.json` | object[5 keys] |

**State surfaces:** `PrecisionMetrologySystem.cs`, `PrecisionOpticsEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 5 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-METROLOGY-TRUTH-172` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `POT-220A` | no name match — resolve at claim time |
| `POT-220B` | no name match — resolve at claim time |
| `POT-220C` | no name match — resolve at claim time |
| `POT-220D` | no name match — resolve at claim time |
| `POT-220E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **4** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.PlansB86_B89.cs`, `src/Host/PrecisionOpticsHostSession.cs`, `src/Main.Plans110_113.cs`, `src/Main.PlansB86_B89.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/Optics/PrecisionGlassworksOpticsEngineTests.cs`, `Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs`, `Ashfall.Core.Tests/Shelter/Plans142145CatalogLoadTests.cs`, `Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **1**; isolated: **2**.

| From | → To |
|---|---|
| `PrecisionGlassworksOpticsEngine` | `PrecisionOpticsEngine` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `low_background_metrology` |
| `precision_metrology` |
| `precision_optics` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--precision-metrology-selftest` |

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

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/glassworks_recipes.json` |
| `Assets/StreamingAssets/Data/metrology_standards_catalog.json` |
| `Assets/StreamingAssets/Data/precision_broaching_catalog.json` |
| `Assets/StreamingAssets/Data/precision_optics_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 5 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Optics` | 1 | 5 |

**Verdict:** 5 cases sit under matching regions — run those first (`Optics`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **6**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/LowBackgroundMetrologyHostSession.cs` |
| `src/Host/LowBackgroundMetrologySaveStore.cs` |
| `src/Host/PrecisionMetrologySaveStore.cs` |
| `src/Host/PrecisionOpticsHostSession.cs` |
| `src/Host/PrecisionOpticsSaveStore.cs` |
| `src/Main.LowBackgroundMetrology.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `low_background_metrology` | no |
| `precision_metrology` | no |
| `precision_optics` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `low_background_metrology` |
| `metrology_calibration_drift` |
| `metrology_measurement_noise` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `precision_optics_catalog.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 3 (laddered 0) · RNG streams 3 · host files 9 · catalogs 5 · test regions 1 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PRECISION-OPTICS-TRUTH-220
wave: 16
status: PROPOSED — foreman claim required
packages: POT-220A, POT-220B, POT-220C, POT-220D, POT-220E
claim paths:
  - src/Host/LowBackgroundMetrologyHostSession.cs  # §19 candidate host surface
  - src/Host/LowBackgroundMetrologySaveStore.cs  # §19 candidate host surface
  - src/Host/PrecisionMetrologySaveStore.cs  # §19 candidate host surface
  - src/Host/PrecisionOpticsHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/glassworks_recipes.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/metrology_standards_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Optics/
  - godot --headless --path . -- --precision-metrology-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
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
