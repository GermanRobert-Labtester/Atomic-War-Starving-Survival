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

## 0. Prologue — The Late Call

> *"Four readings, four voices, one quarter. They do not agree with each other. That is the entire
> reason there are four."*

Standing is not a score. It is a **description** — the shelter's own account of what it has become,
derived purely from the resolved verdict and read back four times a year by four different voices
that are under no obligation to concur.

A, B and C are the ordinary grades. **D is not a grade at all**; it is *The Late Call*, a condition
in which the reading has been made and the meaning has not yet arrived. It resolves. It always
resolves. But between the call and the resolution there is a quarter of a year in which the
shelter is living inside a word it cannot yet define.

**Tone & register.** Liturgical, measured, unsentimental. The vocabulary is the standing record:
*standing, reading, voice, quarter, sheet, call*. Prose should sound like a document read aloud in
a quiet room by someone who knows the room is listening. Never explain the four voices. They are
voices; they are not positions.

**Mystery & texture.** `Never rewrites verdict flags` (Non-Goals) is the card's spine: Standing is
a *reading of* the verdict, never a correction of it. What the verdict decided is done. Standing
only says what it sounded like afterwards.

**The second layer.** Four readings a year is a strange cadence for anything mechanical and a
familiar one for anything human: it is how a household takes stock, how a congregation listens,
how a ship's log is kept. The Standing Sheet is the game's one recurring act of *listening to
itself*. What makes it eerie is not the fourth grade. It is that the shelter must go on eating,
repairing and deciding while a word it cannot define hangs over the quarter — ordinary life under
an unresolved description.

---

## 1. Goal & Outcome

> *Design intent: give the endgame a quarterly pulse and never tell the player what the pulse
> means. Four voices, one sheet, and a fourth grade that is not a grade.*

- **Goal:** Pure Standing derivation, Standing Sheet page, four dated readings in four voices; Standing D (The Late Call) resolves into A/B/C.
- **Non-Goals:** Never rewrites verdict flags; no new save section; no stat penalties from lease tiers (relay/corridor read model only).
- **Start gate:** P2 accepted

---

## 1b. Texture, Mystery & Voice

*(Second prose pass. Narrative texture and writing guidance only — no authority, no claimed path,
no acceptance criterion. §6's register is unchanged; the fragments below are content candidates
and deliberate silences, not new recorded questions.)*

**What the player is never told.**

- The order the four voices speak in, and whether order implies rank. It does not explain itself, and the sheet must never rank them.
- What the four voices sound like. Diction is authored; timbre is forbidden. They are voices; they are not positions, and they are certainly not characters.
- Whether Standing D resolves toward the grade the shelter feared or the grade it deserved. It resolves. That is the entire promise, and the card keeps it.
- Whether the reading is *true* (P3-OM-3). It is derived from the resolved verdict and never corrects it. Description and truth are separate instruments and the card declines to compare their readings.

**Voice — sample fragments** (content candidates for `year_two_chapter.json` sentence tables /
`year_two_radio.json`).

> "First reading of the quarter, first voice: the shelter is standing. The voice does not say standing in what."

> "The second voice disagrees with the first. This is not a malfunction. This is why there is a second voice."

> "Standing: D. The call has been made; the meaning is still on its way. Live accordingly."

> "Fourth voice, fourth sheet. It reads the same word the other three read and leaves a different one in the margin."

**Design texture beats.**

- **Never reconcile the voices.** The disagreement is the instrument; consensus would break it into a score.
- **Never rewrite verdict flags.** Standing is downstream of a decision already made. It only says what the decision sounded like afterwards.
- **D is vocabulary, not punishment.** No stat penalty rides on it (Non-Goals). The pressure is entirely the feeling of living inside an undefined word that is guaranteed to resolve — eventually.
- **Date the sheets, never the voices.** Everything on the Standing Sheet is dated. Nothing on it is attributed.

**Third pass — four readings that disagree (texture only; §6's register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §6's register is unchanged; the fragments
below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Standing is not a score; it is four honest instruments pointed at one
shelter from four rooms, and the disagreement between them is information rather than error. Prose
should never average the readings — the friction between A, B, C and the strange patience of D is
the whole texture of a year being interpreted.

**What the readings leave lying around.**

> "Reading A and Reading C are both true. The disagreement is in what they were asked."

> "Standing D is printed at the bottom of the sheet in the same type as the others, and it is the
> only grade that waits."

> "Quarterly account: the shelter is understood to be one thing by the household and another by
> the archive. The sheet keeps both readings adjacent, which is the politics."

**Held silences (texture, not register rows).**

- What D is waiting for. The register holds the question (§6); the grade is patient and its
  patience is not modelled. Texture only.
- Whether the four readings know about each other. They are taken from different rooms; the sheet
  places them side by side and the placing is the only meeting they will ever have.

---

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

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P3-OM-1 | Who are the four voices? | The card authors four dated readings in four voices and never names or reconciles them. The disagreement is the instrument. | Never — texture by omission. |
| P3-OM-2 | What is The Late Call waiting for? | Standing D resolves into A/B/C by rule. Until then it is vocabulary the shelter lives inside. | Never — locked by the program's own structure. |
| P3-OM-3 | Does Standing describe the shelter or the verdict? | It is derived purely from the resolved verdict and `never rewrites verdict flags` (Non-Goals). Whether the description is *true* is not asserted. | Never — a rule, not a gap. |
| P3-OM-4 | Why four times a year? | The cadence is authored. No calendar or ritual explanation is offered. | Never — the artefact reads as custom. |
| P3-OM-5 | What is the evidence-gate waiver for? | The card wires an "evidence-gate waiver" without narrating its purpose. It is a mechanism, not a story. | The umbrella §6 decision register, if ever signed. |
