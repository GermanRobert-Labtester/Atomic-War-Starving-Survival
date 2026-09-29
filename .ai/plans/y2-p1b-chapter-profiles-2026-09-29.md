# Feature / Task Plan: Year Two — P1B — Storyline Chapter Profiles & branch-aware Year One ending

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P1B); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Per-storyline Reckoning days, Reading day/close rule, Year One ending source and Standing modifier; default profile reproduces today exactly.
- **Non-Goals:** No rewrite of authored verdict day gates (ReckoningClock adapter only); no change to saves already in progress (legacy profile); no new save section.
- **Start gate:** P1 accepted

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/StreamingAssets/Data/chapter_profiles.json (new)
- Assets/Ashfall.Core/Endgame/ChapterProfileCatalog.cs, ChapterProfileResolver.cs (new)
- Assets/Ashfall.Core/Verdict/ReckoningSystem.cs, ReckoningClock.cs (new)
- Assets/Ashfall.Core/Endgame/EndgameSystem.cs, UnifiedEndingResolver.cs (additive)
- src/Main.Endgame.cs, src/Main.UnifiedEnding.cs (INT co-sign)
- Verdict host session (name from P0)
- Assets/Ashfall.Core/CatalogIntegrityValidator.cs (INT)
- Ashfall.Core.Tests/Endgame/ChapterProfileTests.cs, Ashfall.Core.Tests/Verdict/ReckoningClockTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P1B.1–P1B.6 per umbrella §8 (legacy-parity golden BEFORE editing ReckoningSystem/EvaluateEnding).

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P1B all met
- [ ] `bin/run-scoped-tests` on ChapterProfileTests, ReckoningClockTests, Ashfall.Core.Tests/Verdict/, EndgameSystemTests, Plan19EndingContinuityTests, Plan145UnifiedEndingIntegrationTests; `--verdict-selftest`, `--unified-ending-selftest`; save round-trip; host build.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
