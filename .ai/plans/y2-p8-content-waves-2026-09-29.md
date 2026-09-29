# Feature / Task Plan: Year Two — P8 — Content waves W1–W4 (one quarter each)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P8); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

## 1. Goal & Outcome
- **Goal:** Prose-led quests, radio, encounters, keeper follow-ups per quarter, integrated only through existing loaders.
- **Non-Goals:** No new mechanics; nothing outside `minDay ≥ profile chapter-open day` and ≤ chapter end; no explained idioms; no depicted harm to children.
- **Start gate:** W1: P1+P1B+P2+P3 · W2: P4c+P5 · W3: P6 · W4: P4b/c+P7

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/StreamingAssets/Data/year_two_quests.json, year_two_radio.json, narrative_encounters_year_two.json (new, wave by wave)
- keeper follow-up data path (from P0)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. Each wave is its own claim/handoff; content minimums per umbrella §5 P8 table.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P8 all met
- [ ] data-integrity gate; content-utilization scan; `NarrativeEncounterSystemTests`; new `--year-two-quarter-selftest <n>` (INT descriptor).
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated
