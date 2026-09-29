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

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

---

## 0. Prologue — Nine

> *"Nine endings, and none of them is an ending. That is what a year after a war is like."*

The Year Two Chronicle is a **permutation**, not a branch: nine possible paragraphs produced from
Standing × profile × the accumulated shape of two years, and every one of them is a *description*
rather than a verdict. It is the game's last act of writing and its first act of not judging.

**Final seal once.** One seal, at the end, and never a second. The phrase is procedural and
emotional at the same time: this is the one irreversible thing the program does, and it does it
exactly one time.

**Legacy traits from generations.** What P4 raised, P7 inherits. The apprentices and acting
successors of Year Two do not vanish at Day 720; they become *traits* on a campaign that will be
read by somebody else's game.

**Tone & register.** Archival, elegiac, deliberately unspectacular. The vocabulary is the
chronicle: *permutation, paragraph, standing, modifier, seal, legacy*. Prose should sound like the
last page of a book that knows it is the last page and refuses to raise its voice.

**Mystery & texture.** `No tone-lock breaks` and `no depiction of harm to children` are the two
Non-Goals that shape every paragraph. §6 keeps open what the ninth permutation is *for*.

---

## 1. Goal & Outcome

> *Design intent: write the last page as a description, not a score — and seal it exactly once.*

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

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P7-OM-1 | Why nine? | The permutation is authored as nine and stops. Whether nine is complete is not asserted. | Never — texture by omission. |
| P7-OM-2 | Who reads the Chronicle? | It is produced at the profile end-day and sealed once. No reader is modelled and none should be. | Never — a rule, not a gap. |
| P7-OM-3 | What is a legacy trait *for*? | `CampaignLegacySystem` takes an additive field. What another campaign does with it is outside this program. | The next campaign, if one is ever authored. |
| P7-OM-4 | Does the Chronicle agree with the Standing readings? | P3's four voices are never reconciled and P7 takes Standing paragraphs as input. Disagreement is permitted by design. | Never — the artefact is the instrument. |
| P7-OM-5 | What does the final seal seal? | The program seals once and does not define the object sealed. | Never — a rule, not a gap. |
