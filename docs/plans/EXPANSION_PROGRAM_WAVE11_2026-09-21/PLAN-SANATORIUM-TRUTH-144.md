# PLAN-SANATORIUM-TRUTH-144 — Admission, Therapy Plans, Progress & Discharge

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-ACUTE-TRAUMA-CARE-124, PLAN-DUTY-ROSTER-TRUTH-101.
**Implementation scaffold:** [`PLAN-SANATORIUM-TRUTH-144_APPENDIX-A_SCAFFOLD.md`](PLAN-SANATORIUM-TRUTH-144_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-MENTAL-HEALTH-THERAPY-64` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no psychological model (Plan 64 owns it), no acute medical ward
(Plan 124), no new therapy lore beyond the existing catalog.

## 1. Outcome
`Sanatorium/` holds `PsychologicalSanatoriumSystem.cs` and
`PsychologicalTherapyCatalog.cs`: a care facility on top of Plan 64's model. No
plan states admission criteria, how a therapy plan is chosen and progressed,
what a discharge means, or how a resident's absence interacts with duty
coverage — so the facility can become a second psychological authority.

| Deliverable | Detail |
|---|---|
| Admission | criteria read from Plan 64's state + acute flags from Plan 124; admission is an event with a day |
| Therapy plan | plan rows from the catalog matched to condition; progress accrues on the canonical clock per documented per-day effect |
| Progress truth | progress modifies Plan 64's model through its owner; the sanatorium stores no private severity score |
| Discharge | `improved / stable / unimproved / terminated` states with consequences routed to their owners |
| Coverage interaction | a resident is unavailable for duty/roster coverage (Plan 101 sees them as unavailable), never doubly counted |

## 2. Evidence
- `Assets/Ashfall.Core/Sanatorium/`: the two files above (verified).
- Plan 64 owns the psychological model; this is a facility worklist over it.
- Plan 101's coverage model must treat residents as unavailable — the integration test lives there.
- Plan 124 may refer cases to the sanatorium; the admission criteria table is the boundary.

## 3. Packages
- **SNT-144A** admission criteria table + event.
- **SNT-144B** therapy plan matching + per-day progress test.
- **SNT-144C** progress writes through Plan 64's owner (no private score proof).
- **SNT-144D** discharge states + consequence routing tests.
- **SNT-144E** coverage integration test with Plan 101 (no double count).

## 4. Acceptance & verification
- Admission requires its criteria; a non-qualifying case is refused typed.
- Progress is observable in Plan 64's values, not a parallel number.
- A resident is unavailable in coverage and returns on discharge.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Sanatorium/` (create if absent).

## 5. Risks
Second psych authority → the no-private-score test is the guard.
Indefinite residence → discharge states are terminal; a resident cannot linger without a plan row.

---

## 6. Expanded census (2 files · 603 lines)

Scope: `Assets/Ashfall.Core/Sanatorium/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PsychologicalSanatoriumSystem.cs` | 453 | System | **yes** | 0 | 0 | 2 |
| `PsychologicalTherapyCatalog.cs` | 150 | Catalog | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `PsychologicalSanatoriumSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Sanatorium/` (create if absent) |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MENTAL-HEALTH-THERAPY-64` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SNT-144A` | no name match — resolve at claim time |
| `SNT-144B` | no name match — resolve at claim time |
| `SNT-144C` | no name match — resolve at claim time |
| `SNT-144D` | no name match — resolve at claim time |
| `SNT-144E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.FlagshipInstitutions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `psychological_arcs` |
| `psychological_sanatorium` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--real-main-journey-selftest` |

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
| `Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json` |
| `Assets/StreamingAssets/Data/psychological_therapies.json` |
| `Assets/StreamingAssets/Data/psychological_trauma.json` |

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

Host files (`src/`) whose names share a domain token: **1**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/PsychologicalSanatoriumSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `psychological_arcs` | no |
| `psychological_sanatorium` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(CODEX_ONLY 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/dweller_psychological_journals.json` | CODEX_ONLY |
| `psychological_therapies.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 1 · catalogs 5 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SANATORIUM-TRUTH-144
wave: 11
status: PROPOSED — foreman claim required
packages: SNT-144A, SNT-144B, SNT-144C, SNT-144D, SNT-144E
claim paths:
  - src/Host/PsychologicalSanatoriumSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/psychological_therapies.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --real-main-journey-selftest
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
