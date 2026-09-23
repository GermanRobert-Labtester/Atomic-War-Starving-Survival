# PLAN-BALLISTICS-WORKBENCH-TRUTH-184 — Ammunition Assembly, Tolerances & Safety

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-COMBAT-DEPTH-62, PLAN-METROLOGY-TRUTH-172, PLAN-CRAFT-QUALITY-TRUTH-112.
**Non-goals:** no combat resolver (Plan 62), no quality tiers (Plan 112), no
standard set (Plan 172).

## 1. Outcome
`Combat/BallisticsWorkbenchSystem.cs` (**572 lines**) is reachable and
unaddressed: reloading and assembling ammunition. It is the one crafting bench
where quality has a **safety** consequence — a bad round can fail on the bench
or in the field. Nothing states tolerances, failure modes, or who consumes the
ammunition.

| Deliverable | Detail |
|---|---|
| Assembly model | rounds per batch with input casings/powder/primer; outputs are inventory items (Plan 93) |
| Tolerance/quality | workbench capability from Plan 172's standards determines achievable tier (Plan 112) |
| Failure modes | a bench failure (bad lot) is visible and consumes inputs; a field failure routes as a misfire input to Plan 62 |
| Safety rules | documented deviations (overcharge) produce explicit risk, not hidden bonus damage |
| Save truth | in-progress batches and lot quality restore; a load never re-rolls a batch |

## 2. Evidence
- `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs` (572 lines; unaddressed — Wave 13 audit).
- Plan 172 supplies tolerance capability; Plan 112 grades outputs.
- Plan 62 consumes ammunition supply and misfire outcomes.
- Plan 93 verifies input/output conservation.

## 3. Packages
- **BWT-184A** assembly model + input table.
- **BWT-184B** tolerance→tier tests at boundaries.
- **BWT-184C** bench failure fixture + input consumption.
- **BWT-184D** misfire input contract to Plan 62.
- **BWT-184E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Output tier follows workbench capability; below-capability lots are marked.
- A bench failure consumes inputs and yields no rounds; conservation balances.
- Misfire inputs appear in Plan 62's data; no hidden damage modifiers.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/`.

## 5. Risks
Hidden overcharge bonuses → deviations create explicit risk rows, never silent upside.
Tier duplication → mapping only; tiers stay in Plan 112.

---

## 6. Expanded census (3 files · 1,340 lines)

Scope: `Assets/Ashfall.Core/Combat/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BallisticShieldEngine.cs` | 437 | System | — | 0 | 0 | 2 |
| `BallisticsSystem.cs` | 331 | System | — | 0 | 0 | 0 |
| `BallisticsWorkbenchSystem.cs` | 572 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `ballistic_shield_catalog.json` | object[2 keys] |
| `ballistics_workbench_catalog.json` | object[2 keys] |

**State surfaces:** `BallisticShieldEngine.cs`, `BallisticsWorkbenchSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Combat/` |
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

Domain files: 3. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-COMBAT-DEPTH-62` | 3 |
| `PLAN-COMBAT-FAMILY-TRUTH-273` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `BWT-184A` | no name match — resolve at claim time |
| `BWT-184B` | no name match — resolve at claim time |
| `BWT-184C` | `BallisticsWorkbenchSystem.cs` |
| `BWT-184D` | no name match — resolve at claim time |
| `BWT-184E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/BallisticShieldHostSession.cs`, `src/Host/CombatHostSession.cs`, `src/Host/Plans74To77HostSessions.cs`, `src/Main.Plans110_113.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/CombatBallisticsTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/Plans74To77SystemsTests.cs` |
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

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `ballistic_shield` |
| `ballistics_workbench` |

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

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/ballistic_shield_catalog.json` |
| `Assets/StreamingAssets/Data/ballistics_workbench_catalog.json` |

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

Host files (`src/`) whose names share a domain token: **2**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BallisticShieldSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `ballistic_shield` | no |
| `ballistics_workbench` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `ballistic_shield_catalog.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 2 · catalogs 3 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BALLISTICS-WORKBENCH-TRUTH-184
wave: 14
status: PROPOSED — foreman claim required
packages: BWT-184A, BWT-184B, BWT-184C, BWT-184D, BWT-184E
claim paths:
  - src/Host/BallisticShieldHostSession.cs  # §19 candidate host surface
  - src/Host/BallisticShieldSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/ballistic_shield_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/ballistics_workbench_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
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
