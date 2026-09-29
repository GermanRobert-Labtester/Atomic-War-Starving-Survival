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

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

---

## 0. Prologue — Horizon Lift

> *"The year was never designed to have a second winter. We are going to have to invent one
> honestly."*

Day 360 is a wall in the data. Every climate curve, thermal input, radon reading and ice road in
the game was authored to arrive somewhere inside a single year, and then to stop. The Horizon Lift
is the unglamorous act of teaching those curves that there is more calendar than they were told.

The word in the goal is **honest**. Not extended, not extrapolated, not fudged — *catalog-driven*,
so that Days 361–720 have weather that was authored rather than defaulted. Thermal, radon and ice
road receive changing inputs; Days ≤ 360 do not change by a single frame.

**Tone & register.** Meteorological, patient, exact. The vocabulary is the almanac: *season, curve,
input, thermal, radon, freeze, thaw*. Prose should sound like a weather office that has been handed
a longer year and refuses to invent anything it cannot source.

**Mystery & texture.** P1.1's instruction — **golden 1→360 captured BEFORE editing** — is the
card's spine and its ethic. The past is fixed first, in evidence, and only then is the future
authored. `year_two_climate.json` is a file about what has not happened yet, written by people who
have agreed not to disturb what has.

---

## 1. Goal & Outcome

> *Design intent: a second winter that was authored, not defaulted. And not one changed frame in
> the first year.*

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

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P1-OM-1 | What was the second winter going to be before this card? | Nothing. The data stopped at Day 360 and would have defaulted. The card authors it and declines to narrate the absence. | Never — a rule, not a gap. |
| P1-OM-2 | Why does climate stay on the absolute calendar and not per-storyline? | Non-Goals locks it. Whether a storyline *deserves* its own weather is not discussed and must not be. | Never — locked by scope. |
| P1-OM-3 | What does the horizon look like from inside the shelter? | The card lifts inputs, not viewpoints. No fiction of observation is authored here. | P8 content waves, if a radio line ever mentions a season. |
| P1-OM-4 | Is a lifted horizon the same as a longer year? | `CampaignCalendar` is touched only under DEC-Y2-03 delegation. Whether time itself changed is deliberately unresolved. | Never — the artefact is the atmosphere. |
