# Plan 24 — Survivor Fitness, Needs, Labor, and Medical Journey: CLOSEOUT

**Status:** CLOSED — both signature items resolved 2026-09-18
**Wave:** 8 (implementation unblocker), 2026-09-17; signatures 2026-09-18
**Package:** user-authorized integrator package (`C1_planintegration[5]`),
implemented in dependency order (24A → 24B → 24C) across Tasks A1–A4.

> The implementation, data, tests, and generated truth are complete and
> verified for every non-decision item. The two acceptance criteria that
> required a product/architecture signature (ward staffing;
> affliction-specific recovery ramp) were resolved on 2026-09-18 under the
> user's completion-first program authorization: ward staffing was
> implemented and sealed per option (b), and the recovery ramp was closed
> per option (ii). The snapshot rebaseline remains environment-blocked
> (renderer-capable session required) and is the only recorded residual.

## Original open gates and their terminal states

| # | Open gate (Plan 24 package) | State | Evidence |
|---|---|---|---|
| 1 | Broad needs-source migration (nine families) | **CLOSED-WITH-EVIDENCE** | Task A1: seven families attributed through the shared seam (`grief.shelter_loss`, `thermal.room_warmth`, `hygiene.self_care_withdrawn`, `kitchen.meal_quality`/`unsafe`, `ration.confrontation`/`theft`, `leadership.crisis_aura`/`morale`, `contagion.isolation`/`pressure`); missed-meal / friction-morale / overwork rows documented absent (not fabricated). `Plan24NeedsSourceMigrationTests` 9/9 + full family-suite regression |
| 2 | Worker-hours + skill-to-yield contract | **CLOSED-WITH-EVIDENCE** | Task A2: `DutyHourLedger` (derived), measured overwork (`duty_hours_overwork`) with data-authored magnitudes through the seam, `WorkerProductivityContract` (one seam, §24B.23), kitchen + workshop wiring; greenhouse/foundry-output/medical producer rows documented with named blockers. `Plan24DutyHourTests` 10/10 |
| 3 | Ward staffing | **CLOSED-WITH-EVIDENCE (sealed 2026-09-18)** | Option (b) implemented: data-authored `ward` duty role (`skill_paramedic`, hazard class `medical`), `DutyRosterIds.RoleWard`, `MedicalWardSystem.StaffingPreflight` gate, host binding in `Main.Medical.cs`. `MedicalWardSystemTests` 14/14, `Plan24FitnessForDutyTests` 11/11, `Plan24DutyRosterFitnessTests` 7/7. Ledger: `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED |
| 4 | Assignment UI | **CLOSED-WITH-EVIDENCE** | DutyRosterPanel: candidate assignment, vacate, impaired-warning confirm/cancel dialog (Core contract's own reason ids, commit-time revalidation, exactly one confirmed mutation); panel lifecycle PASS, a11y 5/5 |
| 5 | Overwork consequences | **CLOSED-WITH-EVIDENCE** | Data-authored `overwork` block; daily-boundary routing; no-double-application + removal tests |
| 6 | Grief-to-needs named source | **CLOSED-WITH-EVIDENCE** | Task A3: `grief.bond_loss` derived from the persisted relationship ledger (onset-stamped, linear decay, reload-safe); pure rate functions tested |
| 7 | Mourning action | **CLOSED-WITH-EVIDENCE** | `MemorialSystem.Mourn` once-per-death (`MournedDay`), attributed recovery + journal line, cenotaph route; exactly-once + rejection tests |
| 8 | Ration re-split + grievance journey | **CLOSED-WITH-EVIDENCE** | Task A3 journey test through the real fate cascade → re-split → grievance |
| 9 | Affliction-specific recovery ramp | **CLOSED (option ii signed 2026-09-18)** | Uniform data-authored discharge window declared complete per the user's program authorization (option ii, zero-code). Admissions carry no cause field; no ramp authority was fabricated and none is owed. Affliction-specific ramps, if ever wanted, are new scoped packages — not Plan 24 debt |
| 10 | Full save/load journey tests | **CLOSED-WITH-EVIDENCE** | `Plan24JourneyParityTests` 3/3: treatment journey continuous==interrupted; death/grief journey continuous==interrupted-after-cascade; field-by-field + fingerprint equality through the production save path |
| 11 | 30-day balance simulation | **CLOSED-WITH-EVIDENCE** | Same file: 30-day invariant ledger, per-day impossible-state exclusions (dead never assigned, need ranges), same-seed equality, **day-15 mid-reload suffix equality** |
| 12 | Snapshot review | **CLOSED (2026-09-25, real renderer session)** | Executed per L-P24R on the first renderer-capable host (AMD Radeon Graphics / radeonsi, direct rendering, canonical 1920×1080). All 32 snapshot targets regenerated at canonical size; diff re-run 32/32 MATCH; malformed `shelter_decor_default.png` golden replaced; atlas detail text wrap and horizontal-scroll artifacts fixed before capture. |
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
  selftest in this wave (`ERROR: 1 resources still in use at exit`).
  **Classified 2026-09-17 (D3):** the a11y smoke-specimen harness leaks a fixed
  8 ObjectDB / 2 CanvasItem RIDs / 1 font RID / 1 resource at exit (identical
  across three runs); the production lifecycle path
  `--panel-bind-lifecycle-selftest` emits zero. Documented as known-benign
  Tier 3 harness noise in `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` §4,
  with the distinguisher (growth across cycles or appearance on a lifecycle
  path = real leak).
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

## Signature queue (foreman) — RESOLVED 2026-09-18

1. **Ward staffing** — **SIGNED (option b)** and implemented; sealed as
   `DEBT-PLAN24-MEDICAL-WARD-STAFFING` (RETIRED) in `KNOWN_DEBT.md`.
2. **Affliction recovery ramp** — **SIGNED (option ii)**: the uniform
   data-authored discharge window is declared complete; no per-affliction
   ramp authority exists or is owed.

Both signatures were granted by the user (foreman) on 2026-09-18 under the
completion-first integration program authorization. Plan 24 is CLOSED; the
environment-blocked snapshot rebaseline (item 12) is the only recorded
residual and requires no further decision.

## Addendum: Procedural Unblock of Snapshot Rebaseline Residual (L-P24R, 2026-09-23)

Under integrator package `UNBLOCK-RESIDUALS-PLANS-24-31` (user-authorized 2026-09-23), the residual execution protocol is formally ratified per `UNBLOCK-04` §5.11 / `DEC-304`:
1. Plan 24 remains **CLOSED**; no further architecture, domain logic, data schema, or save store decisions are required.
2. The snapshot rebaseline execution protocol is established for the first session equipped with a real display/GPU:
   - Read the intended render changes inventory above (Survivor detail need contributors, Duty roster hours row & confirmation dialog, Cenotaph status & vigil button).
   - Render each affected panel at canonical 1920x1080 resolution using the repository's snapshot harness.
   - Verify any diffs against the intended render changes inventory. Rebaseline golden assets only for intended diffs.
   - Update this document's item 12 to CLOSED once executed on a renderer-capable host.
3. Headless sessions are strictly prohibited from fabricating or bypassing golden snapshots.

**L-P24R EXECUTED 2026-09-25** under the UI/UX audit package: real display/GPU present
(`:0`, AMD Radeon Graphics, accelerated). Snapshot targets were aligned to the
canonical 1920×1080 (protocol item 2), the full set captured and Diff-verified
(32/32 MATCH after regeneration), and goldens rebaselined for intended diffs only
(all 32 diffs attributable to landed UI waves since the 2026-09-05 goldens, the
canonical-size alignment, spacing token normalization, and two atlas-render fixes;
`ui-layout-selftest` PASS, `ui-accessibility-selftest` 5/5 PASS under the same
session). Item 12 above is therefore CLOSED.
