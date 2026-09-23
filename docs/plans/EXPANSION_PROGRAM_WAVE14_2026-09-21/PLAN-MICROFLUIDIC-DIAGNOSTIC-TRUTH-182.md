# PLAN-MICROFLUIDIC-DIAGNOSTIC-TRUTH-182 — Lab-on-Chip Tests, Sensitivity & Errors

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-PHARMACEUTICAL-TRUTH-167, PLAN-METROLOGY-TRUTH-172.
**Non-goals:** no outbreak model (Plan 47), no medicine production (Plan 167), no
instrument standards model (Plan 172).

## 1. Outcome
`Medical/MicrofluidicDiagnosticEngine.cs` (**609 lines**) is reachable and
unaddressed: the diagnostics that tell the holdfast what is spreading. A test's
value depends on sensitivity/specificity and its error modes; without a stated
contract, diagnostics are either perfect truth or flavor text.

| Deliverable | Detail |
|---|---|
| Test model | tests with sensitivity/specificity per condition, consumable reagent cost, and turnaround time |
| Result truth | a positive/negative result is probabilistic per those values using a registered stream; false results are possible and recorded |
| Operator/skill | outcome shifts with operator skill and instrument condition (Plan 172's standards) |
| Actionable routing | a positive routes to Plan 47's response and Plan 167's treatment inputs; a false positive is visible as such after confirmation |
| Save truth | pending tests and results restore; a load never re-rolls a result |

## 2. Evidence
- `Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs` (609 lines; unaddressed — Wave 13 audit).
- Plan 172 supplies instrument standards/condition inputs.
- Plan 47 consumes positives for outbreak response.
- Plan 167 consumes confirmed positives for treatment.

## 3. Packages
- **MFT-182A** test model + sensitivity/specificity table.
- **MFT-182B** seeded result distribution test (paired runs, bounds).
- **MFT-182C** operator/condition shift tests.
- **MFT-182D** routing tests (response/treatment) + false-positive confirmation path.
- **MFT-182E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Result rates over a large paired sample match the table within tolerance.
- False results appear and are confirmable; routing consumes only confirmed positives.
- Pending/complete tests survive save/load unchanged.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Perfect diagnosis → sensitivity/specificity are the constraint; the distribution test proves it.
Nondeterminism → all variance draws from the registered stream.

---

## 6. Expanded census (2 files · 716 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MicrofluidicDiagnosticCatalogLoader.cs` | 107 | Loader | — | 0 | 0 | 0 |
| `MicrofluidicDiagnosticEngine.cs` | 609 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `microfluidic_diagnostic_catalog.json` | object[4 keys] |
| `geothermal_steam_vent_diagnostics.json` | array[7] |

**State surfaces:** `MicrofluidicDiagnosticEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 3 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MFT-182A` | no name match — resolve at claim time |
| `MFT-182B` | no name match — resolve at claim time |
| `MFT-182C` | no name match — resolve at claim time |
| `MFT-182D` | no name match — resolve at claim time |
| `MFT-182E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/MicrofluidicDiagnosticHostSession.cs`, `src/Main.Plans146_149.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs`, `Ashfall.Core.Tests/Medical/MicrofluidicDiagnosticEngineTests.cs`, `Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `geothermal_aquifer` |
| `geothermal_orc` |
| `microfluidic_diagnostic` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--microfluidic-diagnostic-selftest` |
| `--microfluidic-diagnostic-uitest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--salt-steam-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnSteamTrip` | `Assets/Ashfall.Core/BrineWaterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json` |

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

Host files (`src/`) whose names share a domain token: **6**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/MicrofluidicDiagnosticHostSession.cs` |
| `src/Host/MicrofluidicDiagnosticSaveStore.cs` |
| `src/UI/MicrofluidicDiagnosticPanel.cs` |
| `src/UI/PanelSceneLoader.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `microfluidic_diagnostic` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `medical_microfluidic_diagnostics` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

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
**Surface:** save sections 1 (laddered 0) · RNG streams 1 · host files 7 · catalogs 1 · test regions 0 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MICROFLUIDIC-DIAGNOSTIC-TRUTH-182
wave: 14
status: PROPOSED — foreman claim required
packages: MFT-182A, MFT-182B, MFT-182C, MFT-182D, MFT-182E
claim paths:
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/Host/MicrofluidicDiagnosticHostSession.cs  # §19 candidate host surface
  - src/Host/MicrofluidicDiagnosticSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --microfluidic-diagnostic-selftest
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
