# PLAN-INSTITUTIONS-TRUTH-141 — Office & Berth Assignment Ledger with Availability Rules

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-POLITICS-69, PLAN-LABOUR-PROFESSIONS-68, PLAN-DUTY-ROSTER-TRUTH-101.
**Implementation scaffold:** [`PLAN-INSTITUTIONS-TRUTH-141_APPENDIX-A_SCAFFOLD.md`](PLAN-INSTITUTIONS-TRUTH-141_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SURVIVOR-ROSTER-TRUTH-244` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no political system (Plan 69 owns policy/surfaces), no roster
(Plan 101 owns shifts), no second survivor model.

## 1. Outcome
`Institutions/` holds two types: `IInstitutionAvailability.cs` and
`InstitutionAssignmentLedger.cs`. The ledger is the natural single owner for
"who holds which office/berth and why", but nothing states the availability
predicate contract, the assignment lifecycle, or the vacating rules — so
assignments can silently double-book or persist after a holder's death.

| Deliverable | Detail |
|---|---|
| Availability contract | `IInstitutionAvailability` predicates documented per institution (who is eligible, when, under which conditions) |
| Assignment lifecycle | vacant → nominee → holder → vacated with the event that moves each edge; one holder per berth enforced |
| Vacating rules | death, injury, removal, resignation, and reassignment each have a documented path; no phantom holders |
| Ledger truth | the ledger is the only place a berth's holder is recorded; panels read it, they do not cache it |
| Persistence | assignments restore with their section; a load never re-nominates or duplicates |

## 2. Evidence
- `Assets/Ashfall.Core/Institutions/`: `IInstitutionAvailability.cs`, `InstitutionAssignmentLedger.cs` (verified; the whole directory).
- Plan 101 owns shift posts (duty roster); institution berths are distinct long-lived offices — the boundary is stated.
- Plan 68 owns professions; eligibility predicates may read profession values, not own them.
- Plan 69 owns collective politics; the ledger supplies holder facts to it.

## 3. Packages
- **INT-141A** availability predicate table per institution.
- **INT-141B** lifecycle + one-holder-per-berth enforcement tests.
- **INT-141C** vacating path tests (death/injury/removal/resign/reassign).
- **INT-141D** ledger-only rule + panel read check.
- **INT-141E** persistence round-trip + no re-nomination on load.

## 4. Acceptance & verification
- Double-booking a berth fails typed; every vacate path leaves the berth vacant, not ghosted.
- A dead holder is never presented; a load does not re-nominate.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Institutions/` (create if absent).

## 5. Risks
Overlap with roster → posts stay in Plan 101; berths are offices, and the boundary is tested.
Eligibility drift → predicates are data-visible and tested per institution.

---

## 6. Expanded census (2 files · 97 lines)

Scope: `Assets/Ashfall.Core/Institutions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `IInstitutionAvailability.cs` | 40 | Support | **yes** | 0 | 0 | 0 |
| `InstitutionAssignmentLedger.cs` | 57 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Institutions/` (create if absent) |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `INT-141A` | `IInstitutionAvailability.cs`, `InstitutionAssignmentLedger.cs` |
| `INT-141B` | no name match — resolve at claim time |
| `INT-141C` | no name match — resolve at claim time |
| `INT-141D` | `InstitutionAssignmentLedger.cs` |
| `INT-141E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.FlagshipInstitutions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`, `Ashfall.Core.Tests/DiplomaticSummitTests.cs`, `Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs`, `Ashfall.Core.Tests/SkyDefenseBatteryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **15** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `cryo_vault` |
| `cultural_archives` |
| `diplomatic_summits` |
| `dose_ledger` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `perimeter_defense` |
| `psychological_arcs` |
| `psychological_sanatorium` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--defense-selftest` |
| `--dose-ledger-selftest` |
| `--ledger-debt-selftest` |
| `--real-main-journey-selftest` |
| `--sky-defense-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnAssignmentChanged` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnCulturalBroadcast` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |

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

Host files (`src/`) whose names share a domain token: **7**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Main.FlagshipInstitutions.cs` |
| `src/UI/CaravanBarterLedgerPanel.cs` |
| `src/UI/DoseLedgerPanel.cs` |
| `src/UI/SubterraneanDebtLedgerPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `shelter_assignment` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

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
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |

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
**Surface:** save sections 2 (laddered 1) · RNG streams 0 · host files 7 · catalogs 4 · test regions 0 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INSTITUTIONS-TRUTH-141
wave: 11
status: PROPOSED — foreman claim required
packages: INT-141A, INT-141B, INT-141C, INT-141D, INT-141E
claim paths:
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - src/Host/ShelterAssignmentHostSession.cs  # §19 candidate host surface
  - src/Main.FlagshipInstitutions.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/ledger_debt_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --defense-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
