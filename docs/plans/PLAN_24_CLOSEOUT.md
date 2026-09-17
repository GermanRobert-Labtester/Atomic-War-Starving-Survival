# Plan 24 — Survivor Fitness, Needs, Labor, and Medical Journey: CLOSEOUT

**Status:** CLOSED-WITH-DEFERRALS — PENDING TWO FOREMAN SIGNATURES
**Wave:** 8 (implementation unblocker), 2026-09-17
**Package:** user-authorized integrator package (`C1_planintegration[5]`),
implemented in dependency order (24A → 24B → 24C) across Tasks A1–A4.

> The implementation, data, tests, and generated truth are complete and
> verified for every non-decision item. Two acceptance criteria require a
> product/architecture signature (ward staffing; affliction-specific recovery
> ramp) and are presented in the implementation log's decision memos. Until
> signed, they are OPEN decision items — not silently deferred, not
> fabricated. On signature this document's label becomes CLOSED (option
> (c)/(iii)) or the signed options become new implementation packages.

## Original open gates and their terminal states

| # | Open gate (Plan 24 package) | State | Evidence |
|---|---|---|---|
| 1 | Broad needs-source migration (nine families) | **CLOSED-WITH-EVIDENCE** | Task A1: seven families attributed through the shared seam (`grief.shelter_loss`, `thermal.room_warmth`, `hygiene.self_care_withdrawn`, `kitchen.meal_quality`/`unsafe`, `ration.confrontation`/`theft`, `leadership.crisis_aura`/`morale`, `contagion.isolation`/`pressure`); missed-meal / friction-morale / overwork rows documented absent (not fabricated). `Plan24NeedsSourceMigrationTests` 9/9 + full family-suite regression |
| 2 | Worker-hours + skill-to-yield contract | **CLOSED-WITH-EVIDENCE** | Task A2: `DutyHourLedger` (derived), measured overwork (`duty_hours_overwork`) with data-authored magnitudes through the seam, `WorkerProductivityContract` (one seam, §24B.23), kitchen + workshop wiring; greenhouse/foundry-output/medical producer rows documented with named blockers. `Plan24DutyHourTests` 10/10 |
| 3 | Ward staffing | **OPEN — AWAITING SIGNATURE** | A2 implementation-selective memo (options a/b/c; recommendation b). No authority fabricated |
| 4 | Assignment UI | **CLOSED-WITH-EVIDENCE** | DutyRosterPanel: candidate assignment, vacate, impaired-warning confirm/cancel dialog (Core contract's own reason ids, commit-time revalidation, exactly one confirmed mutation); panel lifecycle PASS, a11y 5/5 |
| 5 | Overwork consequences | **CLOSED-WITH-EVIDENCE** | Data-authored `overwork` block; daily-boundary routing; no-double-application + removal tests |
| 6 | Grief-to-needs named source | **CLOSED-WITH-EVIDENCE** | Task A3: `grief.bond_loss` derived from the persisted relationship ledger (onset-stamped, linear decay, reload-safe); pure rate functions tested |
| 7 | Mourning action | **CLOSED-WITH-EVIDENCE** | `MemorialSystem.Mourn` once-per-death (`MournedDay`), attributed recovery + journal line, cenotaph route; exactly-once + rejection tests |
| 8 | Ration re-split + grievance journey | **CLOSED-WITH-EVIDENCE** | Task A3 journey test through the real fate cascade → re-split → grievance |
| 9 | Affliction-specific recovery ramp | **OPEN — AWAITING SIGNATURE** | A3 design note (options i/ii/iii; admissions carry no cause field; no dormant data). No ramp authority fabricated |
| 10 | Full save/load journey tests | **CLOSED-WITH-EVIDENCE** | `Plan24JourneyParityTests` 3/3: treatment journey continuous==interrupted; death/grief journey continuous==interrupted-after-cascade; field-by-field + fingerprint equality through the production save path |
| 11 | 30-day balance simulation | **CLOSED-WITH-EVIDENCE** | Same file: 30-day invariant ledger, per-day impossible-state exclusions (dead never assigned, need ranges), same-seed equality, **day-15 mid-reload suffix equality** |
| 12 | Snapshot review | **ENVIRONMENT-BLOCKED (documented)** | Headless renderer unavailable (SubViewport needs a real display); no snapshot files touched by this wave; the intended render changes are enumerated below for the first renderer-capable rebaseline |
| 13 | Needs characterization 14/15 baseline | **CLOSED-WITH-EVIDENCE** | Task A4: model proven (nine distinct emissions), assertion retargeted 7→9 with drift note; 15/15 |
| 14 | Worker identity contract (§24B.23) | **CLOSED-WITH-EVIDENCE** | One shared contract; the nine-family source ids + the labor-cluster source ids form the stable vocabulary; no per-producer setters |

## Intended render changes (snapshot rebaseline inventory)

- Survivor detail: "Top active need contributors" / "Recent need contributors"
  now populated by the migrated A1 sources and the derived `overwork.*` /
  `grief.bond_loss` rates.
- Duty roster: new Duty-Hours row (committed/recommended + OVERWORKED text),
  ASSIGNMENT section (candidate buttons, VACATE), impaired-warning
  CONFIRM/CANCEL dialog.
- Cenotaph: truthful memorial status line (recorded souls + vigil pending),
  live vigil button with result-text feedback.

No golden snapshots were regenerated without review; the snapshot gate
remains environment-dependent and must run on a renderer-capable session.

## Verification battery (Wave 8 final)

- **Focused/regression:** Survivors 243/243 · DutyRoster 28/28 · Medical
  359/359 · Memorial 69/69 · kitchen 12/12+6/6+2/2 · caregiving 28/28 ·
  apprenticeship 3/3 · skills 12/12 · duty integration 40/40 · briefing
  13/13 · needs characterization 15/15 · comprehensive save suite 1160/1160 ·
  save checksums 24/24 · determinism sweep 199/199 · determinism guard 2/2.
- **Plan 24 new suites:** `Plan24NeedsSourceMigrationTests` 9/9 ·
  `Plan24DutyHourTests` 10/10 · `Plan24RecoveryGriefTests` 6/6 ·
  `Plan24JourneyParityTests` 3/3.
- **Host gates:** build 0 errors/0 warnings · data-integrity PASS (332
  catalogs, 0 errors) · content-utilization gate PASS · panel-bind-lifecycle
  PASS · ui-accessibility 5/5 · compiler-warning baseline PASS · triad drift
  PASS.
- **Sanctioned aggregate:** `verify-fast.sh` — **ALL 47 GATES PASSED
  CLEANLY** (after regenerating the generated-truth chain for this wave's new
  files/data).
- **Save/replay:** zero new save sections; additive legacy-default fields only
  (`duty_roles.json` labor blocks are data; kitchen quality stamps; relationship
  `grief_since_day`; memorial `MournedDay`). Mid-journey and mid-simulation
  reload parity proven field-by-field.

## Known non-Plan-24 debt routed elsewhere

- **Godot shutdown resource/RID warning** — reproduced during the a11y
  selftest in this wave (`ERROR: 1 resources still in use at exit`) — routed
  to Task D3 with the reproduction evidence.
- **Greenhouse / foundry-output / medical producer rows** — named blockers
  (no worker identity; operator-free forging; staffing signature) recorded in
  the implementation log.
- **Water-outage hygiene decay (§24B.13)** — no current model; not fabricated.

## Files/authorities affected (authority map)

`docs/systems/SURVIVOR_STATE_AUTHORITY_MATRIX.md` carries the row-level
updates: needs sources → attributed seam; duty assignment → derived hours +
overwork; death/fate → grief/bond/mourning; every domain owner unchanged. The
full implementation ledger is `docs/plans/C1_planintegration[5]_
IMPLEMENTATION_LOG.md`.

## Signature queue (foreman)

1. **Ward staffing** — options (a) medical-ward staffing model / (b)
   data-authored ward duty role (recommended) / (c) defer.
2. **Affliction recovery ramp** — options (i) cause-tracked per-affliction
   discharge windows / (ii) declare the uniform data-authored window complete
   / (iii) defer (recommended if affliction-specific recovery is not a goal).
