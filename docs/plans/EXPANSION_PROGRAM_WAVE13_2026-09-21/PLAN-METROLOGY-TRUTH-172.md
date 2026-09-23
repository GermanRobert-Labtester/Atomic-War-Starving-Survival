# PLAN-METROLOGY-TRUTH-172 — Calibration Standards, Tolerances & Quality Assurance

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-MAINTENANCE-DECAY-TRUTH-119, PLAN-CRAFT-QUALITY-TRUTH-112.
**Implementation scaffold:** [`PLAN-METROLOGY-TRUTH-172_APPENDIX-A_SCAFFOLD.md`](PLAN-METROLOGY-TRUTH-172_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CRAFT-QUALITY-TRUTH-112` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new workshop content, no quality-tier model (Plan 112), no
machine decay table (Plan 119).

## 1. Outcome
`Shelter/PrecisionMetrologySystem.cs` (671 lines) is reachable and unaddressed.
Precision work (optics, instruments, gauges) depends on **measurement
standards**: what a workshop can verify, the tolerance it can hold, and how
calibration degrades. Without a contract, precision is a flat unlock instead of
a capability the holdfast maintains.

| Deliverable | Detail |
|---|---|
| Standard set | the reference standards the holdfast holds, each with a tolerance grade and a source (salvage/craft) |
| Capability rule | a workshop can produce at the tolerance its standards support; below-standard work is possible and marked lower, never silently equal |
| Calibration | standards and instruments drift per Plan 119's contract; recalibration consumes documented inputs |
| QA link | produced goods' tolerance maps into Plan 112's quality tiers through a stated table, not a parallel tier |
| Save truth | standards, grades, and drift restore; a load never re-calibrates |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs` (671 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 112 owns produced-goods tiers; this plan maps tolerance to them.
- Plan 119 supplies drift and the recalibration cost seam.
- Plan 45 owns the industry systems whose outputs use the capability.

## 3. Packages
- **PMT-172A** standard set + tolerance grade table.
- **PMT-172B** capability rule + below-standard marking test.
- **PMT-172C** drift and recalibration path (consumes inputs; conservation check).
- **PMT-172D** tolerance→tier mapping table + fixture per tier boundary.
- **PMT-172E** save round-trip; no re-calibration on load.

## 4. Acceptance & verification
- Production capability follows the held standards; removing a standard lowers capability in the fixture.
- Drift progresses per the shared contract; recalibration restores it and consumes inputs.
- Tier mapping produces the documented tier at each boundary.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Tier duplication → mapping table only; tiers stay in Plan 112.
Silent capability → standards and grades are visible in the workshop view.

---

## 6. Expanded census (3 files · 1,099 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PrecisionBroachingCatalog.cs` | 81 | Catalog | — | 0 | 0 | 0 |
| `PrecisionMetrologySystem.cs` | 671 | System | **yes** | 0 | 0 | 2 |
| `PrecisionOpticsEngine.cs` | 347 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `metrology_standards_catalog.json` | object[5 keys] |
| `precision_optics_catalog.json` | object[2 keys] |
| `precision_broaching_catalog.json` | object[5 keys] |

**State surfaces:** `PrecisionMetrologySystem.cs`, `PrecisionOpticsEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 4 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-PRECISION-OPTICS-TRUTH-220` | 3 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PMT-172A` | no name match — resolve at claim time |
| `PMT-172B` | no name match — resolve at claim time |
| `PMT-172C` | no name match — resolve at claim time |
| `PMT-172D` | no name match — resolve at claim time |
| `PMT-172E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.PlansB86_B89.cs`, `src/Host/PrecisionOpticsHostSession.cs`, `src/Main.Plans110_113.cs`, `src/Main.PlansB86_B89.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs`, `Ashfall.Core.Tests/Shelter/Plans142145CatalogLoadTests.cs`, `Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

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

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 3 (laddered 0) · RNG streams 3 · host files 9 · catalogs 4 · test regions 1 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-METROLOGY-TRUTH-172
wave: 13
status: PROPOSED — foreman claim required
packages: PMT-172A, PMT-172B, PMT-172C, PMT-172D, PMT-172E
claim paths:
  - src/Host/LowBackgroundMetrologyHostSession.cs  # §19 candidate host surface
  - src/Host/LowBackgroundMetrologySaveStore.cs  # §19 candidate host surface
  - src/Host/PrecisionMetrologySaveStore.cs  # §19 candidate host surface
  - src/Host/PrecisionOpticsHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/metrology_standards_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/precision_broaching_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Optics/
  - godot --headless --path . -- --precision-metrology-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
