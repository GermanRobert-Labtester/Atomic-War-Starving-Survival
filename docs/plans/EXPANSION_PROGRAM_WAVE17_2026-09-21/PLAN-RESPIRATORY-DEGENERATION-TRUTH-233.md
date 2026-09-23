# PLAN-RESPIRATORY-DEGENERATION-TRUTH-233 — Dust Lungs: Exposure, Progression & Care

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ACUTE-TRAUMA-CARE-124, PLAN-MUTATION-HEREDITY-81, PLAN-MICROFLUIDIC-DIAGNOSTIC-TRUTH-182, PLAN-SURGICAL-WARD-TRUTH-213.
**Non-goals:** no chronic-dose model (Plan 81), no diagnostics engine (Plan 182),
no ward facility (Plan 213).

## 1. Outcome
`Medical/RespiratoryDegenerationSystem.cs` (**287 lines**) is reachable and
unaddressed: progressive lung conditions from dust, ash, and work exposure.
Plans 81/124/182/213 cover dose, acute care, tests, and the ward — the
**progressive occupational condition** with its exposure source, work
restrictions, and palliation has no owner.

| Deliverable | Detail |
|---|---|
| Exposure model | condition accrues from documented work/shelter exposures (dusty trades Plan 45/68, filtration state Plan 40); no hidden roll |
| Progression | staged progression per a day-based curve; stage changes have visible symptoms |
| Work coupling | a survivor's work capacity drops per stage through the existing capacity owners; no private productivity penalty |
| Care paths | diagnosis via Plan 182, treatment/palliation via Plan 213/144 owners |
| Save truth | stage and exposure restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` (287 lines; unaddressed — Wave 17 audit).
- Plan 45/68 supply work exposures; Plan 40 filtration state.
- Plan 182 diagnoses; Plan 213/144 treat.
- Plan 119 could cover equipment (masks) that reduces exposure — noted.

## 3. Packages
- **RDT-233A** exposure table + curve.
- **RDT-233B** stage progression + symptom-visibility fixtures.
- **RDT-233C** work-capacity routing (no private penalty).
- **RDT-233D** care path tests (diagnosis/treatment hand-offs).
- **RDT-233E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Progression matches the curve for scripted exposure; stages are visible with symptoms.
- Capacity changes appear in the existing owners; care paths work through 182/213.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Invisible disease → stage symptoms and warnings are requirements.
Overlap with 81 → dose stays there; this is occupational exposure.

---

## 6. Expanded census (2 files · 424 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `RespiratoryAfflictionHandler.cs` | 137 | Support | — | 0 | 0 | 0 |
| `RespiratoryDegenerationSystem.cs` | 287 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `RespiratoryDegenerationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 15 name references across the test tree |
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

Domain method: plan-body artifact list.
Governed artifacts: 4. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 2 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |
| `PLAN-MUTATION-HEREDITY-81` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` |
| `Medical/RespiratoryDegenerationSystem.cs` |
| `RespiratoryAfflictionHandler.cs` |
| `RespiratoryDegenerationSystem.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `RDT-233A` | no name match — resolve at claim time |
| `RDT-233B` | no name match — resolve at claim time |
| `RDT-233C` | no name match — resolve at claim time |
| `RDT-233D` | no name match — resolve at claim time |
| `RDT-233E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **7** · Test files: **9** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/HostCli.PanelTests.cs`, `src/Host/PanelBindLifecycleSelfTest.cs`, `src/Host/Phase0HostSession.cs`, `src/Main.Medical.cs`, `src/UI/AfflictionsPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 9 | `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs`, `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`, `Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs`, `Ashfall.Core.Tests/Medical/MedicalPipelinePhase2Tests.cs`, `Ashfall.Core.Tests/Medical/MedicalPipelineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `crafting` |
| `medical` |
| `medical_pipeline` |
| `medical_ward` |
| `nuclear_core_lifecycle` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--duty-roster-loop-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--medical-selftest` |
| `--medical-ward-save-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--playable-loop-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-hazard-loop-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnCraftingPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnMedicalProcessingCompleted` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnRespiratoryDegradationIncreased` | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |

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

Host files (`src/`) whose names share a domain token: **0**
(0 of them panels/HUD).

| Host file |
|---|
| — | no host filename shares a token with this domain |

**Verdict:** no host file shares a token with this domain — the surface may be driven through a generic panel, or may not be surfaced at all. Verify before claiming a route.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 0 · catalogs 1 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RESPIRATORY-DEGENERATION-TRUTH-233
wave: 17
status: PROPOSED — foreman claim required
packages: RDT-233A, RDT-233B, RDT-233C, RDT-233D, RDT-233E
claim paths:
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --duty-roster-loop-selftest
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
