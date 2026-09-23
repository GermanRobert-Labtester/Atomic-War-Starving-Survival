# PLAN-PSYOPS-TRUTH-210 — Influence Broadcasts, Targeting & Counter-Effects

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PROPAGANDA-TRUTH-150, PLAN-RADIO-STATION-TRUTH-209, PLAN-ESPIONAGE-SYSTEM-TRUTH-161, PLAN-MORALE-UNREST-TRUTH-129.
**Non-goals:** no propaganda action set (Plan 150), no station operations
(Plan 209), no operations lifecycle (Plan 161).

## 1. Outcome
`Radio/PsyOpsSystem.cs` (**418 lines**) is reachable and unaddressed: influence
broadcasts aimed at listeners beyond the walls. Propaganda (Plan 150) shapes
the holdfast; espionage (Plan 161) runs field operations; the station
(Plan 209) transmits. PsyOps is the **targeted external** influence layer —
and without a contract it is either a free win or flavor text.

| Deliverable | Detail |
|---|---|
| Message model | influence messages with a target audience class and a subject faction/group |
| Delivery | requires a transmitting station (Plan 209) at the documented reach; a message without a station is impossible |
| Effect routing | effects write to Plan 29's standing and Plan 129's marks where they apply locally; no private belief score |
| Counter-effects | exposure to contradictory facts (Plans 120/126) or enemy broadcasts reduces effect; documented |
| Detection | the target may detect the source (Plan 161's operations); detection carries consequences |
| Save truth | active messages and detected state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Radio/PsyOpsSystem.cs` (418 lines; unaddressed — Wave 16 audit).
- Plans 150/209/161 are the three adjacent owners; boundaries stated per plan.
- Plan 29 owns standing effects; Plan 129 collective marks.
- Plan 120/126 supply contradiction inputs.

## 3. Packages
- **PST-210A** message model + audience table.
- **PST-210B** delivery-via-station test (no station → no message).
- **PST-210C** effect routing tests (no private score).
- **PST-210D** counter-effect + detection fixtures.
- **PST-210E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Every message delivers only through a station with reach; effects appear in named owners.
- Contradiction reduces effect per the table; detection routes to Plan 161.
- Save/load preserves active/detected state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/`.

## 5. Risks
Free influence → station dependency + counter-effects are the constraints.
Duplicate belief score → routing-only writes; the proof test enforces it.

---

## 6. Expanded census (3 files · 653 lines)

Scope: `Assets/Ashfall.Core/Radio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Save 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PsyOpsCatalog.cs` | 69 | Catalog | — | 0 | 0 | 0 |
| `PsyOpsSave.cs` | 166 | Save | — | 0 | 0 | 0 |
| `PsyOpsSystem.cs` | 418 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `PsyOpsSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radio/` |
| Test references | 6 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-RADIO-FAMILY-TRUTH-266` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PST-210A` | no name match — resolve at claim time |
| `PST-210B` | no name match — resolve at claim time |
| `PST-210C` | no name match — resolve at claim time |
| `PST-210D` | no name match — resolve at claim time |
| `PST-210E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/PsyOpsHostSession.cs`, `src/Main.CampaignOwners.cs`, `src/Main.PsyOps.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs`, `Ashfall.Core.Tests/Plan167EspionageConsequenceRoutingTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `PsyOpsCatalog` | `PsyOpsSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `psyops` |

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

Catalog JSON files whose names share a domain token: **0**.

| Catalog |
|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog filename shares a token with this domain — the authority is likely code-defined or its data lives in a broader catalog. Not a conclusion; check the owning loader.

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
| `src/Main.PsyOps.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `psyops` | no |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 1 · catalogs 0 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PSYOPS-TRUTH-210
wave: 16
status: PROPOSED — foreman claim required
packages: PST-210A, PST-210B, PST-210C, PST-210D, PST-210E
claim paths:
  - src/Main.PsyOps.cs  # §19 candidate host surface
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
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
