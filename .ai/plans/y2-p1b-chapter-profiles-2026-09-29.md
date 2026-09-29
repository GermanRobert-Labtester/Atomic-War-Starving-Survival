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

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

---

## 0. Prologue — The Branches

> *"A story with one ending is a story somebody controlled. A story with branches is a story
> somebody lived."*

Year One already ends. It ends in several ways, decided by verdict and standing and the accumulated
weight of a hundred small choices. What it has never had is a *profile* — a way for the ending to
know which story it is the ending of.

Chapter Profiles give the ending that knowledge: per-storyline Reckoning days, Reading day and
close rules, a Year One ending source, a Standing modifier. And the default profile — `profile_base_v1`
— reproduces today's game **exactly**, bit for bit, forever. That is not a compatibility note. It
is the promise that makes the whole system ethically playable.

**Tone & register.** Structural, careful, almost reverent about consistency. The vocabulary is the
narrative machine: *profile, branch, reckoning, reading, ending source, modifier, legacy*. Prose
should sound like someone who edits a book without changing the words on any page the reader has
already turned.

**Mystery & texture.** `ReckoningClock` is a **boundary adapter** (DEC-Y2-13) — it shifts timing
without rewriting authored verdict day gates. A gate that was authored for Day 300 and is now read
on Day 420 has not been moved. It has been *reinterpreted*. That distinction is the card's deepest
idea and it should never be collapsed into "we changed the dates".

---

## 1. Goal & Outcome

> *Design intent: let the ending know which story it is ending — and never once disturb an ending
> that already worked.*

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

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P1B-OM-1 | Which storyline is `profile_base_v1`? | It is the default and it reproduces today exactly. Whether it is *a* story or *the* story is not asserted. | Never — a rule, not a gap. |
| P1B-OM-2 | What does a Reckoning day mean to the people in the shelter? | `ReckoningClock` is a boundary adapter. It shifts timing and authors no fiction of observance. | P8 content waves, if a Reading is ever broadcast. |
| P1B-OM-3 | How many endings did Year One have before this? | The card adds *sources* and *modifiers*, never endings. The earlier count is not recorded here. | Never — texture by omission. |
| P1B-OM-4 | Does a branch remember the branch not taken? | Profiles are resolved, not tracked. No branch history is stored and none should be. | Never — the artefact is the atmosphere. |
| P1B-OM-5 | Why is the Standing modifier per storyline? | The card authors the modifier. Whether a story *deserves* its own reputation is not discussed. | Never — locked by scope. |
