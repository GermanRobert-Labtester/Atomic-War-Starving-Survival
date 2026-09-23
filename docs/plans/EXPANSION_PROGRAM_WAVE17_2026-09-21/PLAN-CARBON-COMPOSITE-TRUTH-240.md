# PLAN-CARBON-COMPOSITE-TRUTH-240 — Advanced Materials: Layup, Curing & Limits

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-METROLOGY-TRUTH-172, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-COATING-TECH-TRUTH-188.
**Non-goals:** no industry family (Plan 45), no standards (Plan 172), no
coating process (Plan 188).

## 1. Outcome
`Shelter/CarbonCompositeEngine.cs` (**237 lines**) is reachable and
unaddressed: producing advanced composite parts for high-stress uses. It is the
top of the materials ladder and the easiest place for a "wonder material" to
trivialize other tiers. Without a contract, it is either a flat upgrade or
inert.

| Deliverable | Detail |
|---|---|
| Process model | layup/curing steps with documented inputs (resin, fiber, heat from Plan 48) and a per-batch yield |
| Quality/tolerance | output grade from inputs + bench capability (Plan 172) mapped to Plan 112 tiers; a poor batch is marked |
| Limits | documented use classes (which parts may be composite) so the material does not replace existing tiers globally |
| Failure modes | curing defects, delamination under stress routed as condition loss (Plan 119) |
| Save truth | in-progress batches and cured stock restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core.Tests/Shelter/CarbonCompositeEngine.cs` (237 lines; unaddressed — Wave 17 audit).
- Plan 45 owns the industrial family; Plan 172 capability; Plan 112 grades.
- Plan 188's coating can finish composite parts — boundary stated.
- Plan 119 receives stress/condition effects.

## 3. Packages
- **CCT-240A** process model + input/yield table.
- **CCT-240B** grade mapping tests at boundaries.
- **CCT-240C** use-class limit table + tests (no global replacement).
- **CCT-240D** defect/delamination fixtures routed to Plan 119.
- **CCT-240E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Grades follow inputs and capability; a poor batch is visibly marked.
- Composite parts only appear in declared use classes; defects degrade condition per contract.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Wonder material → use-class limits are a fixture.
Invisible defects → delamination is a condition event, not a hidden stat.

---

## 6. Expanded census (2 files · 400 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CarbonCompositeCatalog.cs` | 163 | Catalog | — | 0 | 0 | 0 |
| `CarbonCompositeEngine.cs` | 237 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `carbon_composite_catalog.json` | object[4 keys] |
| `activated_carbon_adsorption_records.json` | array[7] |

**State surfaces:** `CarbonCompositeEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 2 name references across the test tree |
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
Governed artifacts: 6. Other plans referencing them: **1**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ENERGY-NUCLEAR-48` | 1 |

**Reading:** incoming edges are coordination risk.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Shelter/CarbonCompositeEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_recon` |
| `recon_telemetry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--carbon-composite-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `On12CActivated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnBurdenedCompassionActivated` | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/carbon_composite_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/activated_carbon_adsorption_records.json` |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(CODEX_ONLY 1).

| Catalog | Classification |
|---|---|
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 0 · catalogs 3 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CARBON-COMPOSITE-TRUTH-240
wave: 17
status: PROPOSED — foreman claim required
packages: CCT-240A, CCT-240B, CCT-240C, CCT-240D, CCT-240E
claim paths:
  - Assets/StreamingAssets/Data/carbon_composite_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/activated_carbon_adsorption_records.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
