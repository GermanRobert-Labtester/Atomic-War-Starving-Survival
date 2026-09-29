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

> **Editorial polish (prose pass, non-contractual):** the Prologue and texture notes below are
> narrative texture and writing guidance only. They change no acceptance criterion, no claimed path,
> no decision and no verification step. All binding criteria remain in the umbrella
> (`year-two-the-long-thaw-2026-09-29.md`) §5/§6/§8.

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

---

## 1. Goal & Outcome

> *Design intent: give the endgame a quarterly pulse and never tell the player what the pulse
> means. Four voices, one sheet, and a fourth grade that is not a grade.*

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
