# PLAN-SKY-DEFENSE-TRUTH-135 — Battery Readiness, Ordnance Accounting & Interception Rules

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BASE-DEFENSE-RAIDS-61, PLAN-CRISIS-DISASTER-RESPONSE-80, PLAN-INVENTORY-CONSERVATION-93.
**Implementation scaffold:** [`PLAN-SKY-DEFENSE-TRUTH-135_APPENDIX-A_SCAFFOLD.md`](PLAN-SKY-DEFENSE-TRUTH-135_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-BASE-DEFENSE-RAIDS-61` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new combat model (Plan 61), no flight simulation, no
inventory store (Plan 93); the battery does not become a second munitions pile.

## 1. Outcome
`SkyDefense/` holds `SkyDefenseBatterySystem.cs` and
`SkyDefenseOrdnanceCatalog.cs`. The system has no stated contract for readiness
(fuel/power/crew), ordnance accounting (rounds consumed per engagement, tracked
by the inventory owner), interception rules (what a battery can and cannot hit),
or how an alarm reaches it.

| Deliverable | Detail |
|---|---|
| Readiness model | inputs (power from the shelter grid, crew from the roster, condition of the mount) resolved into a readiness state with a single owner |
| Ordnance accounting | each engagement consumes rounds through the existing inventory seam; `SkyDefenseOrdnanceCatalog` defines the rounds; no local ammo counter |
| Interception rules | deterministic hit/effect rules per threat class; seeded variance only where the design says so, drawn from a registered stream |
| Alarm integration | alert state comes from the emergency owner (Plan 80); the battery reacts, it does not own the alarm |
| Save truth | battery state and remaining rounds restore; a load never re-rolls an engagement |

## 2. Evidence
- `Assets/Ashfall.Core/SkyDefense/`: `SkyDefenseBatterySystem.cs`, `SkyDefenseOrdnanceCatalog.cs` (verified).
- Plan 61 owns raid resolution; this plan supplies aerial-threat engagement as one defense path.
- Plan 93's conservation wrapper measures round deltas, which this plan must balance.
- Plan 80 owns alarms; the battery consumes alert state.

## 3. Packages
- **SDT-135A** readiness model + owner table.
- **SDT-135B** ordnance accounting through the inventory seam + conservation test.
- **SDT-135C** interception rules per threat class + seeded variance test.
- **SDT-135D** alarm input wiring from Plan 80 (no local alarm state).
- **SDT-135E** save round-trip + no-re-roll-on-load test.

## 4. Acceptance & verification
- Readiness changes with power/crew/condition exactly per the model; no hidden inputs.
- Rounds balance in the conservation wrapper across a scripted engagement set.
- Save/load mid-engagement set preserves state; no outcome changes on load.
- `bash scripts/run_test.sh Ashfall.Core.Tests/SkyDefense/` (create if absent).

## 5. Risks
Munitions duplication → rounds live in inventory; a test asserts the battery holds no count.
Readiness as authority → it reads grid/roster owners; the owner table is the contract.

---

## 6. Expanded census (2 files · 560 lines)

Scope: `Assets/Ashfall.Core/SkyDefense/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SkyDefenseBatterySystem.cs` | 459 | System | **yes** | 0 | 0 | 2 |
| `SkyDefenseOrdnanceCatalog.cs` | 101 | Catalog | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `sky_defense_ordnance.json` | object[2 keys] |

**State surfaces:** `SkyDefenseBatterySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/SkyDefense/` (create if absent) |
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
| `PLAN-BASE-DEFENSE-RAIDS-61` | 1 |
| `PLAN-AUTONOMOUS-MACHINES-79` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SDT-135A` | no name match — resolve at claim time |
| `SDT-135B` | `SkyDefenseOrdnanceCatalog.cs` |
| `SDT-135C` | no name match — resolve at claim time |
| `SDT-135D` | no name match — resolve at claim time |
| `SDT-135E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **4** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.SkyDefense.cs`, `src/Main.FlagshipInstitutions.cs`, `src/Main.SkyDefense.cs`, `src/UI/SkyDefenseBatteryPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/SkyDefenseBatteryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `perimeter_defense` |
| `sky_defense_battery` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--defense-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |
| `--sky-defense-selftest` |

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

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/sky_defense_ordnance.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (4 files, 36 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Defense` | 4 | 36 |

**Verdict:** 36 cases sit under matching regions — run those first (`Defense`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **9**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/DefenseHostSession.cs` |
| `src/Host/DefenseSaveStore.cs` |
| `src/Host/HostCli.SkyDefense.cs` |
| `src/Host/PerimeterDefenseSaveStore.cs` |
| `src/Host/SkyDefenseBatterySaveStore.cs` |
| `src/Main.SkyDefense.cs` |
| `src/UI/ChemWarfareDefensePanel.cs` |
| `src/UI/DefenseGridPanel.cs` |
| `src/UI/SkyDefenseBatteryPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `perimeter_defense` | no |
| `sky_defense_battery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `defense_capture` |
| `defense_damage` |
| `defense_targeting` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `sky_defense_ordnance.json` | UNRESOLVED |

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
**Surface:** save sections 2 (laddered 0) · RNG streams 3 · host files 12 · catalogs 2 · test regions 1 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SKY-DEFENSE-TRUTH-135
wave: 11
status: PROPOSED — foreman claim required
packages: SDT-135A, SDT-135B, SDT-135C, SDT-135D, SDT-135E
claim paths:
  - src/Host/DefenseHostSession.cs  # §19 candidate host surface
  - src/Host/DefenseSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.SkyDefense.cs  # §19 candidate host surface
  - src/Host/PerimeterDefenseSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/sky_defense_ordnance.json  # §17 catalog (verify schema + consumer)
  - sky_defense_ordnance.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Defense/
  - godot --headless --path . -- --defense-selftest
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
