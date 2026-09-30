# Feature / Task Plan: Year Two — P4 — Generations (4a apprentices · 4b elders · 4c council · 4d registration)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P4); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

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

## 0. Prologue — Leave Unwritten

> *"The last choice in a life is not what happens to you. It is whether anyone is permitted to
> write it down."*

Four sub-packages and one sentence in the goal carries the whole ethic: **register / leave-unwritten**.
A survivor may put their name in the book or decline to. The game does not treat registration as
progress and non-registration as loss. It treats both as a decision a person is entitled to make
about their own record.

Children become apprentices. Apprentices become acting successors. Elders hand over — or do not.
A Council meets quarterly and its output is a `designations[]` list, which is the driest possible
way to say *someone has decided who comes next*. And consent is real: **no ratified successor from
a raised child before Day 721**, by design, because a childhood is not a hiring process.

**Tone & register.** Domestic, intergenerational, exact. The vocabulary is the household and the
registry: *apprentice, elder, handover, designation, council, consent, register, unwritten*. Prose
should sound like a family record kept by someone who knows that the blank entries are as
meaningful as the filled ones.

**Mystery & texture.** *Leave-unwritten* is the program's most literary mechanic. A gap in a
genealogy register is not an absence of data — it is a decision that was made and deliberately not
recorded. §6 keeps open what the gap means.

**The second layer.** A registry is the only kind of book where the blanks are the literature. The
card's mechanic — *register / leave-unwritten* — quietly asserts that a person's last act of
self-definition is a right and not a feature, and the game honours both answers equally. The
intergenerational arc is easily mistaken for a progression ladder; it is not one. Nothing about
apprenticeship pays out. What the chapter models is *competence passing between people*, and the
only score it keeps is the one nobody is allowed to total.

---

## 1. Goal & Outcome

> *Design intent: a community learning to hand things over. Consent is real and no one is rushed
> through a childhood into a job.*

- **Goal:** Children→apprentices→acting successors; elder handover; quarterly Council; register/leave-unwritten choice. No new ladder; consent is real.
- **Non-Goals:** No ratified successor from a raised child before Day 721; GenerationalSuccessionEngine 365 d/yr clock is not read; no new role ledger.
- **Start gate:** P2 accepted (4c also needs P0 DEC-Y2-07; 4d needs DEC-Y2-10)

---

## 1b. Texture, Mystery & Voice

*(Second prose pass. Narrative texture and writing guidance only — no authority, no claimed path,
no acceptance criterion. §6's register is unchanged; the fragments below are content candidates
and deliberate silences, not new recorded questions.)*

**What the player is never told.**

- Who keeps the register. A role exists; a person does not have to. The book is kept by whoever is willing to keep it, and that is never narrated.
- What an apprentice is told the night they become an acting successor — or whether that night differs in any way from every night before it.
- Whether the Council has a room. *Quarterly* is authored; a place is not (P4-OM-3). Deliberation stays unmodelled and the fiction must not furnish it.
- What an elder's final wish is *for* (P4-OM-4). The fiction inherits the heirloom's weight and never its explanation.

**Voice — sample fragments** (content candidates for the P8 prose surfaces; the apprenticeship
catalogue keeps catalog rows and gains no prose field).

> "Name: entered. Consent: given. The line is short because the decision was not."

> "Left unwritten, at the survivor's request. The page is not empty; the page is a decision."

> "The apprentice asked what happens after the handover. The elder said: you find out, which is the only answer that has ever been true."

> "Council, second quarter. Designations recorded. Deliberation not recorded — by custom, and by rule."

**Design texture beats.**

- **Leave-unwritten is a dignified verb.** Never render non-registration as missing content, a tooltip, or a nudge. Both answers are complete answers.
- **Day 721 is a horizon, not a delay.** No ratified successor from a raised child before it (F7 age floor). The fiction must never imply the shelter is waiting for one.
- **Children are capability, never peril.** Hard rule — no depiction of harm — and it is exactly why the arc reads as hope rather than tension.
- **The register does not reconcile.** Names, blanks and designations coexist without being resolved into a hierarchy. The book is not a scoreboard and must never become one.

**Third pass — the blank column (texture only; §6's register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §6's register is unchanged; the fragments
below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** A registry is the only kind of book where the blanks are the
literature. The card's verb — *Leave Unwritten* — is a form of authorship by restraint, and the
prose should treat every empty line as a sentence the shelter chose not to say about itself.

**What the registry leaves lying around.**

> "Registry line, left unwritten. The blank is the literature; the ruled line beneath it is the
> audience."

> "An apprentice is a technique with a face and a deadline."

> "Succession entry: prepared, unheld. The preparation is the inheritance."

**Held silences (texture, not register rows).**

- What the register would say if it were filled. Leave Unwritten is the card's whole verb; the
  filling is content's job and must never become the engine's. Texture only.
- Who inherits an unwritten line. Succession exists and is host-wired; the inheritance of blanks
  is folklore, and folklore is kept by saying-so.

---

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/StreamingAssets/Data/apprenticeship_catalog.json (extend)
- src/Host/ApprenticeshipHostSession.cs, src/UI/ApprenticeshipPanel.cs
- src/Host/AgingHostSession.cs and final-wish/heirloom host bindings (files from P0)
- Assets/Ashfall.Core/Survivors/SuccessionCouncilLedger.cs (new; nested in owner per DEC-Y2-07) + that owner's save DTO (additive)
- src/Host/GenealogyHostSession.cs or ApprenticeshipHostSession.cs
- registration owner (DEC-Y2-10)
- Ashfall.Core.Tests/Survivors/YearTwoSuccessionTests.cs, Ashfall.Core.Tests/Generations/YearTwoApprenticeLadderTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P4a.1–P4a.2, P4b.1–P4b.3, P4c.1–P4c.4, P4d.1–P4d.2, P4-x per umbrella §8; sub-packages sequential, each a separate handoff.

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P4 all met
- [ ] `bin/run-scoped-tests` on YearTwoApprenticeLadderTests, YearTwoSuccessionTests, Ashfall.Core.Tests/Generations/, Plan217Genealogy*, ApprenticeshipIntegrationTests; `--second-generation-milestones-selftest`, `--child-development-selftest`, `--aging-selftest`, `--genealogy-selftest`, `--apprenticeship-curriculum-selftest`.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P4-OM-1 | Why did someone choose *leave-unwritten*? | The card offers the choice and authors no motive. Non-registration is a decision, not a deficiency. | Never — a rule, not a gap. |
| P4-OM-2 | What is an *acting* successor, as opposed to a real one? | The ladder ends at acting successor because no ratified successor is possible before Day 721 (F7 age floor). The distinction is structural and unexplained in-fiction. | Never — locked by design. |
| P4-OM-3 | What does the Council decide that it does not write down? | `SuccessionCouncilLedger` stores designations. Deliberation is not modelled and must not be invented. | Never — texture by omission. |
| P4-OM-4 | What is an elder's final wish *for*? | 4b binds final-wish/heirloom host bindings from P0. Their meaning is not authored here. | The aging owner, if it ever documents them. |
| P4-OM-5 | Does a child raised in the shelter owe it anything? | No ladder, no role ledger, no ratified successor. Obligation is deliberately absent from the model. | Never — a rule, not a gap. |
| P4-OM-6 | Why does the succession clock run 365 days and go unread? | Non-Goals names `GenerationalSuccessionEngine`'s clock and declines to read it. The artefact is real and stays. | Never — the artefact reads as history. |
