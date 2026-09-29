# Feature / Task Plan: Year Two — P0 — Premise audit & decision packet

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P0); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Re-verify F1–F21 at path:line; census 360-day constants and verdict day-gate consumers; locate host owners; draft per-storyline profile table; restate open decisions for one-line confirm.
- **Non-Goals:** No source/data/ledger edits. No code. No tests.
- **Start gate:** None (predecessor of all)

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- docs/plans/year_two/Y2_PREMISE_EVIDENCE.md (new)
- docs/plans/year_two/Y2_DECISION_PACKET.md (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P0.1 Re-verify F1–F21 (CONFIRMED/STALE/CHANGED).
2. P0.2 Census 360-day constants; classify runtime clamp / inert default / content window.
3. P0.3 Name: Verdict host session, role owner, expedition dispatch host, registration owner, F16b flag ids, four-outpost retro-binding locations, verdict day-gate consumers.
4. P0.4 Draft storyline table (family + first distinct branches) for P1B signature.
5. P0.5 Write both docs; foreman confirms DEC-Y2-01,-04…-08,-10,-11 defaults.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P0 all met
- [ ] static only; `git diff --check` on the two docs (no tests).
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
