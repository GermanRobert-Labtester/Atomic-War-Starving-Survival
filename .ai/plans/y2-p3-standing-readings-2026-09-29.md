# Feature / Task Plan: Year Two — P3 — Standing (A/B/C/D) & quarterly Readings

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P3); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Pure Standing derivation, Standing Sheet page, four dated readings in four voices; Standing D (The Late Call) resolves into A/B/C.
- **Non-Goals:** Never rewrites verdict flags; no new save section; no stat penalties from lease tiers (relay/corridor read model only).
- **Start gate:** P2 accepted

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/Ashfall.Core/Endgame/YearTwoStanding.cs (new)
- Assets/Ashfall.Core/Verdict/QuarterlyReadingLedger.cs (new), VerdictSave.cs (additive)
- Verdict host session (name from P0)
- Assets/StreamingAssets/Data/year_two_chapter.json (sentence tables), year_two_radio.json (new)
- src/UI/ChroniclePanel.cs (Standing Sheet page)
- Ashfall.Core.Tests/Verdict/YearTwoStandingTests.cs, QuarterlyReadingLedgerTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P3.1–P3.7 per umbrella §8, plus Standing D Approach readings and evidence-gate waiver wiring.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P3 all met
- [ ] `bin/run-scoped-tests` on YearTwoStandingTests, QuarterlyReadingLedgerTests, Ashfall.Core.Tests/Verdict/; `--verdict-selftest`; save round-trip.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
