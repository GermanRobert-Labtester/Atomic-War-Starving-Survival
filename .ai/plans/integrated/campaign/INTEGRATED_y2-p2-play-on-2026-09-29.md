# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Feature / Task Plan: Year Two — P2 — Play On (chapter mechanism)

STATUS: FULLY INTEGRATED — APPROVED BY USER

> Approval: user, 2026-09-29, "I authorise the each seperate plan!" (umbrella: `.ai/plans/year-two-the-long-thaw-2026-09-29.md`). Derived from umbrella §5 (card P2); acceptance criteria, paths, and stop conditions there are binding. **Start gate:** predecessor package handoff accepted by the foreman, exact paths claimed in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` read. Rule 10 stop conditions apply.

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

## 0. Prologue — The Door

> *"Two buttons, on the last screen of a year. One of them is the game keeping its first promise.
> The other is the game asking whether you meant to keep playing."*

**SEAL HERE** is not the lesser option. It is the older one — the exact game as it was before any
of this was written, bit-identical, forever, for anyone who wants it. It is the reason PLAY ON can
be offered at all without being a betrayal.

**PLAY ON** is not a sequel and not an epilogue. It is a *continuance*: chapter 2 opens with no
terminal side effects, meaning nothing is spent, nothing is locked, and nothing that worked stops
working. The word on the button is the entire contract.

**Tone & register.** Quiet, ceremonial, two words long. The vocabulary is the choice itself:
*reading day, offer, chapter, seal*. Prose should be restrained enough to fit on a button and large
enough to feel like a decision. Never sell PLAY ON. Never apologise for SEAL HERE.

**Mystery & texture.** Non-Goals hands Year One ending *selection* to P1B and keeps it out of this
card — so P2 never learns which ending produced the Reading it is offering to continue past. The
mechanism is deliberately blind to the story. That blindness is why it can serve all of them.

**The second layer.** The door is the only element in the game that is also a moral object. It does
not reward, punish or measure; it asks, and it asks exactly once per campaign. The mechanism is
deliberately blind to which ending produced the Reading — that blindness is not a limitation to be
fixed later, it is the feature that lets the door mean the same thing to every shelter that reaches
it. A door that knew your story could not offer you a choice; it could only grade one. And grading is the one thing the door refuses to do, which is why it is
the only screen in the game that can be trusted.

---

## 1. Goal & Outcome

> *Design intent: a door with two equally honest answers. Design the offer so that either one
> feels like a decision and neither feels like a loss.*

- **Goal:** Profile Reading day offers PLAY ON / SEAL HERE; Play On opens chapter 2 with no terminal side effects; SEAL HERE equals legacy.
- **Non-Goals:** No new save section; no new routed panel; Year One ending selection is P1B's, not this package's.
- **Start gate:** P1B accepted

---

## 1b. Texture, Mystery & Voice

*(Second prose pass. Narrative texture and writing guidance only — no authority, no claimed path,
no acceptance criterion. §6's register is unchanged; the fragments below are content candidates
and deliberate silences, not new recorded questions.)*

**What the player is never told.**

- What the shelter is doing while the button is on screen. The pause is not authored and must not be. Whatever is happening, the game does not describe it — the moment belongs to the player and stays unmodelled.
- Which button was written first. The contract is symmetric; its history is not recorded. SEAL HERE is described as the older option and no draft order is ever asserted.
- What "keeping its first promise" means to a player who never saw the first version. The promise is behavioural, not nostalgic: bit-identical is the whole of it.
- Whether anyone in-fiction sees a door at all (P2-OM-2). The screen is a real screen. Any ceremony around it is content, and content belongs to P8.

**Voice — sample fragments** (candidate copy texture for `year_two_chapter.json` prose rows if the
current schema has one; otherwise texture only — the card adds constants and gains no prose field).

> "SEAL HERE. What you have been playing remains what you have been playing. Nothing is added to it after this line."

> "PLAY ON. Nothing you have built is spent. Nothing you have built is finished, either."

> "The door has been in the game since the first version. It was simply on the other side of a year."

> "Two buttons. The older one is the promise. The newer one is the question."

**Design texture beats.**

- **Never sell PLAY ON. Never apologise for SEAL HERE.** A door that leans is not a door; it is a funnel.
- **The word on the button is the contract.** "No terminal side effects" means nothing is spent, nothing is locked, nothing that worked stops working — and the wording must carry that without footnotes.
- **Blindness is service.** The mechanism never learns which ending it continues. That is why it can continue all of them, and why it must never be made cleverer.
- **Two is the whole vocabulary.** A third option would not deepen the choice; it would turn a decision back into a menu (P2-OM-3).

**Third pass — two words (texture only; §6's register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §6's register is unchanged; the fragments
below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Everything here must fit on a button and still feel like a sentence
with a history. The prose's job is to make the *pause* around the two words heavier than the words:
the contract is symmetric, the mechanism is blind, and the dignity of the door is that it never
leans. Keep the fragments ceremonial and two sizes too large for their furniture.

**What the moment leaves behind.**

> "The pause before the button is pressed is the only part of the campaign the game will never
> describe."

> "SEAL HERE is printed first, on the left. Whether left is older is not asserted anywhere, and the
> contract stays symmetric."

> "PLAY ON. The word on the button is the entire contract, and contracts are read once."

**Held silences (texture, not register rows).**

- What the shelter is doing while the button is on screen. The pause is not authored (§1b); the
  ceremony belongs to the player and must stay unmodelled. Texture only.
- Whether the door closes. Two buttons and one click (§1b); the screen does not animate a door and
  never will.

---

## 2. Claimed Paths & Affected Files
(Proposed; the foreman records the claim. `INT` = integrator-owned shared seam.)
- Assets/Ashfall.Core/Endgame/EndgameSystem.cs
- src/Host/EndgameHostSession.cs, src/Host/EndgameSaveStore.cs
- src/Main.Endgame.cs (INT co-sign)
- src/UI/ChroniclePanel.cs
- Assets/StreamingAssets/Data/year_two_chapter.json (new, constants)
- Assets/Ashfall.Core/HostCliRegistry.cs (INT: --year-two-chapter-selftest)
- Ashfall.Core.Tests/Endgame/YearTwoPlayOnTests.cs (new)

## 3. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search current source and data)
- [ ] Premise re-verified against current source (Rule 7); `WORKTREE_OWNERSHIP.md` re-read; no overlapping live claim
- [ ] Decisions this package depends on are signed (umbrella §6)

## 4. Implementation Steps
1. P2.1–P2.8 per umbrella §8 (legacy SEAL golden BEFORE editing OnCampaignSealed).

## 5. Verification
- [ ] Umbrella §5 acceptance criteria for P2 all met
- [ ] `bin/run-scoped-tests` on YearTwoPlayOnTests, EndgameSystemTests, Plan145UnifiedEndingHostIntegrationTests, Plan175MetaProgressionHostIntegrationTests; `--unified-ending-selftest`; `--year-two-chapter-selftest`; save round-trip; host build.
- [ ] Scoped tests via `bin/run-scoped-tests` (<30s); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in AI_AGENT_WORKFLOW format; `.ai/state.md` updated

## 6. Open Mysteries & Deliberate Silence (lore register — texture only, not acceptance criteria)

These questions are **intentionally unanswered** — not gaps, not TODOs. All binding criteria remain
in the umbrella §5/§6/§8. Any future plan that answers one must name the signed decision.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| P2-OM-1 | Which ending produced the Reading? | Non-Goals gives ending selection to P1B. P2 is deliberately blind to the story so it can serve all of them. | Never — the blindness is the design. |
| P2-OM-2 | What does SEAL HERE feel like to the shelter? | The card authors a mechanism that equals legacy exactly. It authors no ceremony and must not. | P8 content waves, if a seal is ever described. |
| P2-OM-3 | Why is there no third option? | Two buttons are authored and the contract is complete. A third would not be a choice; it would be a menu. | Never — locked by scope. |
| P2-OM-4 | Does the Reading know it is the last one? | The Reading day is a profile constant. Nothing in the mechanism marks it as final. | Never — a rule, not a gap. |
| P2-OM-5 | What does "no terminal side effects" promise *not* to do? | The card guarantees nothing is spent or locked. What it withholds is not enumerated. | The umbrella §4 architecture rules. |
