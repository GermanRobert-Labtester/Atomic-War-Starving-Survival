# Feature / Task Plan: Year Two — P7 — Year Two Chronicle & endings

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P7); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Nine-permutation Day-720 (profile end-day) Chronicle with Standing paragraphs and branch-ending modifiers; final seal once; legacy traits from generations.
- **Non-Goals:** No Chapter Three; no tone-lock breaks; no depiction of harm to children.
- **Start gate:** P3, P4, P6 accepted

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/Ashfall.Core/Endgame/YearTwoOutcomeEvaluator.cs (new)
- Assets/Ashfall.Core/Endgame/EndgameSystem.cs, UnifiedEndingResolver.cs (additive)
- Assets/StreamingAssets/Data/year_two_endings.json (new)
- Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs (additive)
- Ashfall.Core.Tests/Endgame/YearTwoOutcomeTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P7.1–P7.4 per umbrella §8.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P7 all met
- [ ] `bin/run-scoped-tests` on YearTwoOutcomeTests, CampaignOutcomeEvaluatorTests, Plan19EndingContinuityTests; `--unified-ending-selftest`, `--epilogue-chronicle-selftest`, `--campaign-legacy-selftest`.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
