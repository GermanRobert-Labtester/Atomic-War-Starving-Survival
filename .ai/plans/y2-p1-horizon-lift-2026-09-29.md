# Feature / Task Plan: Year Two — P1 — Horizon Lift

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P1); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Days 361–720 have an honest catalog-driven climate; thermal/radon/ice road receive changing inputs; Days ≤360 unchanged.
- **Non-Goals:** No chapter/ending changes; no calendar rewrite beyond DEC-Y2-03; climate stays on the absolute calendar (not per-storyline).
- **Start gate:** P0 accepted

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs
- Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs (additive)
- Assets/Ashfall.Core/YearOfAsh/YearTwoClimateCatalog.cs (new)
- Assets/StreamingAssets/Data/year_two_climate.json (new)
- src/YearOfAsh/YearOfAshHostSession.cs
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs (INT, only if DEC-Y2-03 delegation)
- Ashfall.Core.Tests/YearOfAsh/YearTwoHorizonTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P1.1–P1.8 per umbrella §8 (golden 1→360 captured BEFORE editing).

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P1 all met
- [ ] `bin/run-scoped-tests` on YearTwoHorizonTests + Ashfall.Core.Tests/YearOfAsh/; `--year-of-ash-save-selftest`; host build 0 errors.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
