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

> **Editorial polish (prose pass, non-contractual):** the Prologue, §1b and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.
>
> **Second prose pass (2026-09-29, non-contractual):** the Prologue gains one *second layer*
> paragraph and a new **§1b Texture, Mystery & Voice** section, matching the corpus standard used by
> the family plans. Same rule as above: narrative texture only. §6's register is unchanged — the
> new fragments are texture, not new recorded questions (the Open Mystery Index counts still hold).

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

**The second layer.** Nine paragraphs, each one a description, none of them a judgement — the
Chronicle is the game declining to do the one thing endgames are supposed to do. The *final seal
once* rule is what gives that restraint its weight: the paragraph is not written for a player who
might reload it. It is written for a run that will never be editable again, which is the only
condition under which writing becomes an act rather than a menu. And the legacy traits are the
program's strangest invention — a fact about one shelter that will be quietly true inside a
different shelter's arithmetic, with neither campaign ever learning the other exists. That is the quietest mercy this design has:
a fact that helps without ever being thanked.

---

## 1. Goal & Outcome

> *Design intent: write the last page as a description, not a score — and seal it exactly once.*

- **Goal:** Nine-permutation Day-720 (profile end-day) Chronicle with Standing paragraphs and branch-ending modifiers; final seal once; legacy traits from generations.
- **Non-Goals:** No Chapter Three; no tone-lock breaks; no depiction of harm to children.
- **Start gate:** P3, P4, P6 accepted

---

## 1b. Texture, Mystery & Voice

*(Second prose pass. Narrative texture and writing guidance only — no authority, no claimed path,
no acceptance criterion. §6's register is unchanged; the fragments below are content candidates
and deliberate silences, not new recorded questions.)*

**What the player is never told.**

- Who writes the Chronicle. The game writes it; no scribe is modelled, and the prose must never imply one is watching over the last page's shoulder.
- Whether nine is complete (P7-OM-1). The permutation is authored and stops. Whether the ninth is missing something is not asserted.
- What a legacy trait looks like from the receiving side. It arrives as arithmetic, not as a story. Somewhere, a number is different and nobody knows why.
- What the final seal is made of (P7-OM-5). The program seals once and does not define the object sealed.

**Voice — sample fragments** (content candidates for `year_two_endings.json` paragraph rows).

> "The second year is described here in nine paragraphs. None of them is a verdict. All of them are true."

> "Standing: read. Profile: read. What follows is not a score; it is a portrait drawn from arithmetic."

> "Sealed. Once. There is no second seal and no appeal, which is what makes the first one mean something."

> "Legacy trait recorded: the apprentice finished the pump. Somewhere else, a pump is easier to keep. Neither game knows about the other."

**Design texture beats.**

- **Description, not verdict.** Every paragraph must survive being read aloud in a quiet room without anyone flinching.
- **Seal exactly once.** One irreversible act, performed one time — that is the whole of the ceremony and its only ornament.
- **Tone-lock is structural.** `No tone-lock breaks` and `no depiction of harm to children` shape every sentence, not merely its subject matter.
- **The last page does not raise its voice.** Resist epilogue gestures, sequel winks, and the urge to explain the year. The refusal to explain is the ending's dignity.

**Third pass — the last page (texture only; §6's register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §6's register is unchanged; the fragments
below are content candidates for `year_two_endings.json` paragraph rows and deliberate silences,
not new recorded questions.)*

**The shape of the polish.** The last page does not raise its voice. Every fragment here must
survive being read aloud in a quiet room without anyone flinching — the flinch belongs to the
waiting, not the paragraph. The prose's whole discipline is *description at the volume of an
archive*: nine paragraphs, one seal, and a legacy trait that is felt as luck before it is found as
arithmetic.

**What the chronicle leaves lying around.**

> "Paragraph nine, printed. It is the same length as the others. Nothing about the ending is
> permitted to be louder than the year."

> "Seal, one impression. The page is heavy with the absence of a second one."

> "Legacy trait, received: a number is different and nobody knows why. The receiving game calls
> that luck, and it is not wrong."

**Held silences (texture, not register rows).**

- What the ninth permutation is for. The permutation is authored and stops (P7-OM-1); whether
  something is missing is not asserted and must not be. Texture only.
- Who reads a legacy trait aloud. It arrives as arithmetic (§1b); the arithmetic is never announced
  and the different number is felt before it is found.

---

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
