# ASHFALL — FAITH AND SCHISM
### Belief movements split and fight · A shelter can survive its dark, but not always its answers

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/faith-and-schism-2026-09-29.md`
**Family:** "New Pressures and Places" — `docs/expansions/expansion_new_pressures_and_places_index.md` (subject 13 of the user's list).
**Tone lock (inherited):** cold, exhausted, human, restrained. Every movement is fictional and every believer is a person with a reason; no real religions, countries, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.
**Name note:** *The Faithful and the Fractured* is Expansion 13 (the ritual calendar) and is already integrated. This plan builds on it and does not replace it.

---

> *"A faith is a story a shelter agrees to keep telling. A schism is the moment somebody notices the
> story has a hole in it — and keeps the hole."*
>
> Nothing here is a real religion and nothing here is a real argument. What this expansion stages is
> the small, terrible mechanics of *disagreement under scarcity*: two people who agree on every fact
> and still cannot share a bunk.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

ASHFALL already believes in things. Three movements are authored with creeds, comforts and blind spots: **the Ash Witnesses** (the Exchange was human consequence; every flake of ash is a named debt), **the Rebuilders** (meaning is forged with the wrench and the apprentice's slate), **the Listeners** (the silence above is an illusion of broken antennas). Each survivor can convert, carry conviction, run hot on fervor, or sink into dissent. Shrines can be raised. Rites can be demanded and missed. The code even has a ladder of doctrinal tension with named rungs: *Argument, Ostracism, WorkRefusal, PropertyDamage, AssaultThreat* — and one last rung, **Schism**.

The last rung is never reached. The ladder is capped one step below it. And the rungs it *does* reach do a single thing: the host writes a line in the journal — *"Doctrinal tension inside the shelter has reached a new stage: Ostracism."* — and moves on. Nobody stops speaking to anyone. No one refuses a shift. Nothing breaks. The fight is a sentence.

Faith and Schism is the expansion where **belief becomes a way a shelter can come apart.** The rungs bite. A movement that has been asked the same unanswerable question for too long *splits* — into two authored halves that both believe they are the true one, and that share bunks, a mess hall and a ration line. The player cannot make the disagreement go away. They can decide how the shelter lives with it.

The promise: **you will lead people who each believe they are right about something that cannot be proved, and you will choose what that costs.**

The **Question** is the heart of it. A movement asks its own blind spot out loud — once, formally,
with a season on the clock — and the player answers. Whatever they answer becomes a *position*, and
positions have owners, and owners have rooms, and rooms have bunks. That is the whole catastrophe.
It starts with seating.

Every movement is authored as **reasonable**, including the one that splits away. That is not
fairness for its own sake: a schism whose sect reads as a villain is a failure of authoring. The
partition rule is seeded and weighted — the draw decides *who leaves*, never *who was right*. Give
each half its best sentence.

### 1.2 Pillars

1. **Every movement has a question it cannot answer.** The split forms around the blind spot the creed already names.
2. **The ladder bites.** Each rung has a bounded, visible cost through an existing owner — never just a line of text.
3. **A schism is two true halves.** Not a villain and a hero; two readings of the same grief.
4. **The player answers before the split.** There is a Question, and there is a window in which answering well matters.
5. **Coexistence is a mechanism, not a mood.** Bunks, rooms, rites and edicts are things you actually do.
6. **Ship dark.** No sect rows, no schism — the ladder and rites behave exactly as today.

### 1.3 Not this

Not a religion simulator, a doctrine editor, or a holy war. No real belief systems and no proselytising the player. Not a second belief authority: `ZealotrySystem` owns who believes what; this plan adds *what happens when belief collides*. Not another ritual calendar (Expansion 13 owns rites). Not a duplicate of the morale-contagion "schism" (a despair event in a *duty-role* subgroup) — this one is about *creed*.

---

## 2. What the code and data actually say (audit)

| Area | What exists | What is missing |
|---|---|---|
| Movements | 3 authored (`belief_movements.json`: creed, comfort, blind spots, practices, conflict profiles, faction leaning) and 3 mechanical profiles (`wasteland_religions.json`: conversion, fervor decay/gain, cohesion, despair resistance, fanaticism threshold, dissent tolerance, ritual items, shrine rooms, broadcast profile). | Nothing can split; no sect rows. |
| Believers | `ZealotrySystem`: conviction, fervor, dissent, role (Adherent/Devout/Leader), crisis, shrines, ritual demands and failures, broadcast reach, conversion. | Dissent grows but has no destination. |
| Ladder | `ZealotEscalationStage` None→…→AssaultThreat→**Schism**. Advances only while an opposing fervent pair exists. **`Schism` is unreachable** (the advance is clamped to AssaultThreat). The only subscriber to the stage event **writes a journal line**. | Every rung is cosmetic. |
| Friction | `IdeologicalFrictionSystem`: conflict table (in code), roommate compatibility ticking. | Sects have no place in the conflict table; roommate friction is never a player choice. |
| Rites | 19 authored rituals; a cooldown ledger; the calendar engine (Expansion 13). | Rites are never shared or contested. |
| Faction bridge | `BeliefStanceBridge`: belief → bounded faction-trust proposals (budgeted per belief, faction, day). | A split never reaches the world. |
| Other "schisms" | Morale contagion (duty-role subgroup despair, cooldown 21 days); governance incident type `IdeologicalSchism`; lineage event kind "schism". | None is connected to creed. |
| Events | `ideological_events.json`: 8 (theological clash, superstition–science schism, formal mediation quest…). | Not gated by real belief state. |
| UI | `BeliefsPanel`. | No verbs. |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Three "schisms" already exist** (escalation stage, morale-contagion event, governance incident type). → One belief schism, named and scoped; the morale one stays about *despair in duty roles*; on the same day the two must not both fire from the same cause. (DEC-FS-06)
2. **Conflict pairs live in code.** `IdeologicalFrictionSystem.ConflictGroups` is a static table; sect pairs must not require editing it per sect. → An additive data field on the profile; P0 decides whether the static table is bypassed or extended once. (DEC-FS-03)
3. **The stage cap.** The reason `Schism` is unreachable may be deliberate (a guard against a runaway). → P0 reads git history and tests before lifting it. (DEC-FS-05)

---

# PART II — THE SPLIT

## 3. The Question

Each movement carries a **Question** — the thing its creed cannot answer, taken from its own authored blind spots.

| Movement | The Question |
|---|---|
| **Ash Witnesses** | *Is pain owed?* — Is illness, hunger, injury a penance to be accepted, or a wrong to be mended? |
| **Rebuilders** | *Who is worth the risk?* — Do we send people into danger for tools and materials, or keep only what we can safely keep? |
| **Listeners** | *Do we answer?* — When the static repeats a number, do we follow it, or shut the dial? |

When dissent and tension pass a threshold (see §4), the shelter is **asked the Question** as an authored event with three stances. Ignoring it is a stance too.

## 4. The rule of the split

A movement can split when **all** hold: at least four believers; its Leader's dissent ≥ 60 *or* the Devout average ≥ 50; the ladder at *Ostracism* or above for four consecutive days; a Question unanswered for a season (data). A **seeded roll** (a stream keyed by day and movement) decides *when*, once per movement per 60 days.

The split is a **partition**: believers are drawn to the parent or to its **sect** by a weighted seeded draw over their own conviction, dissent, role and relationships to the sect's likely Leader. Each side needs at least two members; otherwise there is no schism — only an expulsion. Nobody is created; existing believers change *belief id* through the belief owner's own public method.

## 5. Sects (authored, dormant until a split)

| Parent | Splits into | What each half hears |
|---|---|---|
| **Ash Witnesses** | **The Penitents** — pain is owed; comfort is a lie | **The Name-Keepers** — remember the dead; do not punish the living |
| **Rebuilders** | **The Bench** — anything repairable, any risk | **The Foundation** — only what lasts, no dead men for scrap |
| **Listeners** | **The Open Ear** — follow every signal | **The Shut Dial** — the silence is safer; the static is a lure |

Each sect is a full profile row (fervor, cohesion, tolerance, shrine tags, rite set) with a parent link and a data-declared conflict with its parent. Each half also inherits a share of the rites: the parent keeps the shrine; the sect asks for a room.

## 6. The ladder, made real

Each rung applies a bounded effect through an existing owner (authored table, `schism_stage_effects.json`):

| Rung | What it does |
|---|---|
| **Argument** | A small morale cost on both camps through the morale owner. |
| **Ostracism** | Pairwise relationship dips between opposing believers through the friction owner; opposing rooms feel it first. |
| **Work refusal** | Opposing believers may decline a shift, raised as the governance owner's existing *ideological schism* incident. |
| **Property damage** | A sabotage incident to the justice owner, with a clue. |
| **Assault threat** | An **offer** to the existing encounter authority — never an automatic fight. |
| **Schism** | The partition of §4. |

## 7. What you can do

- **Answer the Question.** Three stances, each with a cost: side with one reading, hold both, or refuse to answer.
- **Mediate.** The authored formal-mediation quest; a costly, slow, sometimes-successful way to buy days.
- **A joint rite.** A shared observance from the rite set; the cooldown ledger already exists. It cools the ladder but cannot end the schism.
- **Grant a room.** A shrine for the sect through the existing shrine tags.
- **Bunk by creed.** Rooms assigned so that opposing believers do not share a bunk (the roommate friction system already ticks this). It lowers friction and raises isolation.
- **An edict.** Tolerate, or ban a sect's public rites, as a policy through the governance owner *(Shelter Governance)*. A ban is a fight you chose.
- **Exile a leader.** Through the justice owner. It is not clean.
- **Let it run.** It is an option.

## 8. How a schism ends

1. **Reconciled** — rare; a joint rite, a mediation and a shared crisis; the sect folds back.
2. **Cold peace** — both stay, bunked apart; a permanent, low friction.
3. **Departure** — the sect leaves the shelter with its people; it may appear later as a settlement.
4. **Purge** — a ban and its consequences.

## 9. Faith outward

Movements do not stop at the door. **Pilgrims** and **missionaries** arrive through the gate (with *The Quiet War*'s adapter, if present) with a claim tied to a movement. A schism inside becomes news outside; the *faction bridge* moves standing by a bounded budget; other settlements react by their own leaning. A child born in the shelter takes a parent's creed and may take the sect's *(Year Two)*.

---

# PART III — HOW IT MEETS THE WORLD

## 10. Four stories

**The Question.** Listeners have been logging a repeating number for eleven days. Nine of them want to send a party to the coordinates. Four want the dial turned off. The shelter is asked the Question, and the player's answer is what the shelter will remember.

**The Bench and the Foundation.** A salvage run costs a man his hand for a cracked lathe. In the mess hall two apprentices who used to sit together take opposite ends of a table. Nobody says why. The Foundation asks for the west store as a room.

**Two ways to sweep.** The Penitents sweep the airlock in silence; the Name-Keepers read names aloud over the same broom. On the fourth day the shared broom is gone.

**The bunks.** You can assign bunks by creed. It works. On the ninth night a child asks why the corridor is quieter than it used to be.

## 11. Voice samples

> **Journal.** *Day 143. Tension inside the shelter has reached Work Refusal. Three of the Foundation would not go up for the salvage. Nobody was punished. Everybody noticed.*

> **Overheard.** *"Pain is owed." — "By who?" — "By us." — "Then who is it owed to?"*

> **Rite log.** *A joint rite was held in the common room. Attendance: everyone. Warmth: less than hoped.*

## 12. Boundaries with other expansions

| With | Boundary |
|---|---|
| **Expansion 13 (rites)** | Rites and the cooldown ledger are theirs; this plan adds sect rite sets and joint rites via additive rows. |
| **Shelter Governance** | Edicts (tolerate/ban) use its policy scope `belief`; if it is absent, only a leader's private word — no law. |
| **The Quiet War** | Pilgrims and missionaries arrive via the shared gate adapter; no shared state. |
| **The Deep** | Anomalies feed faith interpretations (the Listeners hear the Gallery); read-only. |
| **The Sky** | The Dead Hand pings are what the Open Ear follows; read-only. |
| **Radio Free Ashfall** | Broadcast reach exists in the belief owner; the player's station may feed it through that method only. |
| **Year Two** | Children inherit creed; sect choice at coming-of-age is a later hook. |
| **The Living Region** | Settlement leanings may read movement ids; no LR state is written. |
| **The Record Keepers** | A schism is a record: the Keeper's slant is not added; custody applies. |

## 13. Content plan

- W1: sect rows (6), Questions (3), the stage effects table.
- W2: stance answers, mediation lines, bunk-by-creed lines.
- W3: pilgrims and missionaries, endings text, departure settlements.
- W4: cross-plan hooks (Deep, Sky, Year Two, Governance) — ship dark until both ends exist.

## 14. Non-goals (restated)

No real religion; no new belief authority, ritual ledger or conflict authority; no auto-combat; no change to conversion maths, fervor decay or the morale-contagion schism; no new routed panel; no Unity.

## 15. Risks

| Risk | Mitigation |
|---|---|
| Punching down a faith | Both halves are authored as reasonable; costs are symmetric; the player never *wins*. |
| Runaway ladder | Each rung bounded; a cooldown per movement; `Schism` requires a season of unanswered Question. |
| Double schism with morale contagion | Explicit precedence rule (DEC-FS-06). |
| Static table edits | Data-driven conflict pairs (DEC-FS-03). |
| Tone | Fictional movements only; no proselytising language. |

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** A faith is a story a shelter agrees to keep telling; a schism is the moment
somebody keeps the hole. Both halves are authored as reasonable — a sect that reads as a villain
is a failure of authoring — and the seeded draw decides *who leaves*, never who was right. The
horror is not that someone is wrong. It is that everyone can count.

**What the expansion leaves lying around.**

> "Question board, 109 days. The chalk has been replaced twice. The question has not."

> "Joint rite programme: two song lists stapled together by someone who wanted them to be one list."

> "Edict, drafted and not issued. The drawer is the document's whole future."

**Scenes the player may piece together.**

> "They asked for a room. We gave the east room. We called it generosity and they called it distance, and both were describing the same door."

> "Exile has the longest confirmation beat in the panel and the plainest wording. The plainness is the mercy."

**Held silences (texture — the register below is unchanged).**

- What the movement's blind spot looks like from inside. The Question names it; the naming is the mechanic; the seeing is unmodelled.
- Whether the nineteen rites were ever one calendar. The sequence reads as tradition; it is documented as nothing.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**Why the escalation ladder is clamped unreachable at Schism.** The ceiling may be a guard or a
wound. This document refuses to prejudge it — that refusal is the expansion's tone in miniature: we
do not know whether the ceiling was built to protect you.

**What each movement cannot ask about itself.** The authored Questions name a *blind spot*, not a
doctrine. Filling the doctrine in would make the sect a caricature.

**Whether the ritual calendar predates the movements.** Nineteen rites exist and their sequence is
not asserted anywhere. The order reads as tradition; it is not documented as history.

**Whether belief schism and morale schism are the same event seen twice.** They are ruled never to
fire from one cause on one day. That is not the same as saying they are unrelated.

**Why opposing pairs live in a static in-code table.** The world's enmities were hard-coded. The
plan offers `conflicts_with[]` as a fix and declines to explain the origin.
