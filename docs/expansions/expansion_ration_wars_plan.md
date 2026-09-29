# ASHFALL — THE RATION WARS
### The pantry as politics · Every tin has a name on it, and everyone can count

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/ration-wars-2026-09-29.md`
**Family:** "The Shelter Under Pressure" — `docs/expansions/expansion_shelter_under_pressure_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. Hunger is a ledger and a face, never a spectacle. No real countries, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

The shelter already knows how to be hungry. It has seven ration tiers from Full to None, four priority groups, four protocols, a resentment meter that climbs a tenth of a point a day when someone is fed less than the average, and a line at 0.85 where resentment becomes theft. Below that there is a last-resort menu with a corpse on it.

What it does not have is **a table**. Nobody stands at the end of the line with a ladle and a book. Nobody decides what *fair* means — the code decides for them: fair is the arithmetic mean. A carpenter who carried timber all day and a child who slept through it are, to the resentment meter, owed the same. And when the stores are short, nothing in the game can be *wrong by a little* — a tin can go missing, but nobody can be seen to have taken it, or to have counted badly, or to have counted honestly and been disbelieved.

The Ration Wars is the expansion where **the pantry becomes politics**. You choose the rule of the table. Someone keeps the book. Once a week the book is read aloud in the mess hall, and the number at the bottom is either the number everyone expected or it is not. The player's real job in a bad winter is not to find calories. It is to keep a room of hungry people believing that the count is true.

### 1.2 Pillars

1. **Fair is a rule you choose.** Equal, by work, by need, by rank, by lot. Each is defensible; each makes someone angry.
2. **The book is a person.** A quartermaster keeps it. Their honesty, their hunger and their loyalties are part of the count.
3. **Counting is power.** An audit, a public reading and a discrepancy move legitimacy more than a full pot does.
4. **Small wrongs first.** Between fairness and the corpse-menu there is a long staircase of shorted portions, second helpings and a tin behind a boiler. The player lives on the staircase.
5. **Real goods only.** Every missing tin is a tin that actually left a store and actually went somewhere. The ledger measures; it never invents.
6. **Ship dark.** No table rule, no quartermaster, no ledger rows — the shelter eats exactly as it does today.

### 1.3 Not this

Not a second food economy. Not a new inventory. Not a replacement for the rationing tiers, protocols, kitchen or desperation events — the Ration Wars sits *between* them and gives them a human face. Not a law book (that is *Shelter Governance*, which the Hoarding statute waits on). Not a starvation simulator: hunger already has a system.

---

## 2. What the code and data actually say (audit)

| Area | What exists | What is missing |
|---|---|---|
| Per-person conflict | `RationConflictSystem`: per-survivor allocation, fairness deviation 0.20 from the *average*, resentment +0.10/day and −0.03/day, confrontation 0.70, theft 0.85, morale −10 / −15, three events (`OnResentmentBuilt`, `OnRationConfrontation`, `OnRationsStolen`). Save state captured. | "Fair" is only the mean; no notion of *expected* portion per person or per rule. |
| Host allocation | `src/Host/RationConflictHostSession.cs`: the **only** caller of `SetAllocation`; it copies each survivor's priority bonus (Critical 1.30, High 1.15, Standard 1.0, Low 0.75) and the allocation is clamped to 0–1, so **three of the four groups land on exactly 1.0** and only the Low group can ever be short-changed in the meter's eyes. | No person ever *chooses* a portion; the ration *tier* never reaches the conflict meter on this path. |
| Tiers and protocols | `ResourceRationingSystem`: 7 tiers, 4 priority groups (Critical: medics/engineers/children; High: guards; Standard; Low: prisoners/idle visitors), 4 protocols, crisis declaration, allocation authorisation. | Tiers are per *resource*; conflict is per *survivor* — how the two meet is **VERIFY**. |
| Kitchen | `KitchenNutritionSystem` keeps a serving log (retention: 200, newest-K). | Serving log is history, not a reconciliation. |
| Justice | `CrimeType.Hoarding` exists. `wasteland_laws.json` has **four** laws (theft, assault, sabotage, treason). | Hoarding has no law; nothing ever produces a hoarding *incident*. |
| Last resort | `desperation_events.json`: three rows (corpse harvest, etc.), taboo levels, prion risk, mutiny pressure. | Nothing between "resentment" and "desperation". |
| People | 8 roles (medic, engineer, scout, leader, technician, scientist, diplomat, enforcer). Rooms: kitchen, storage bay, secure storage, common mess hall. | No quartermaster; no place where the count is read. |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Fairness baseline.** Conflict compares each person to the *mean allocation*, and — because priority bonuses clamp — cannot tell Critical, High and Standard apart. A rule like "by work" is only expressible if the baseline can be per person and allocations under a rule are distinguishable. → Additive optional "expected allocation" provider plus a rule-supplied served share; unset = mean = today. (DEC-RW-02)
2. **Ledger honesty needs sources.** A reconciliation is only meaningful if withdrawals can be told apart (served, crafted, stolen, spoiled). Whether the inventory port carries a reason is **VERIFY**. If it does not, the ledger scopes to the kitchen serving log and the pantry row of the storage bay only. (DEC-RW-03)
3. **Where a hoard lives.** A hoard must be a real transfer to a real owner. The personal-belongings owner (Plan 210) is the candidate; if it cannot hold consumables the hoard becomes "eaten extra" and the stash mechanic is dropped. (DEC-RW-05)
4. **Hoarding law.** Belongs to *Shelter Governance* (its statute book proposes Hoarding and Desertion rows). This plan consumes it if present and degrades to the quartermaster's discretion if not.

---

# PART II — THE TABLE

## 3. The Rule of the Table

The player, with the Assembly's consent if *Shelter Governance* is present, chooses one **Table Rule** for the shelter. It sets *what a fair portion is for each person*, and resentment is measured against that instead of the mean.

| Rule | A fair portion is… | It fails when… |
|---|---|---|
| **Equal** | the same for everyone | the workers notice; children notice more |
| **By work** | scaled by the person's duty load | the sick and the old are on the wrong end |
| **By need** | uplifted for children, the injured, the pregnant | the strong feel they carry the shelter for free |
| **By rank** | guards and leaders first | the moment anyone finds out |
| **By lot** | random each day; nobody can say it was unfair, only unlucky | the unlucky one is a child |

Authored as data (`table_rules.json`): group multipliers, a work weight, and a *process fairness bonus* (a lottery is judged partly by its process, so resentment grows more slowly even when outcomes differ). Changing the rule is a **decision with a cost**: it takes a day to announce, and the first three days after a change carry a small resentment surcharge while people learn the new arithmetic.

## 4. The Quartermaster and the Book

**A post, not a role.** The quartermaster is a survivor assigned to the pantry through the existing duty roster. If no one holds the post, nothing changes — the ledger is exact, as today.

The **Book** is a daily reconciliation the game *already has the numbers for*: what the stores held at dawn, what arrived, what the kitchen served, what spoiled, what they hold at dusk. Anything left over is the **unexplained**. Today the unexplained is nearly always zero, because nothing takes food quietly. With a quartermaster in post, three things can make it non-zero:

- **Theft** — the existing event fires; the Book notes it.
- **Skim** — the quartermaster takes a little, for a child, for a friend, for themselves. Probability rises with the ration tier, with their own hunger, and with who is depending on them. The units genuinely leave the store.
- **Miscount** — a tired or unskilled keeper writes the wrong number. The store is right; the Book is wrong. The difference is what people argue about.

The Book shows a **reported** figure. The store holds the **true** one. When they differ, the shelter has a problem it cannot see yet.

## 5. Audit, Count, and legitimacy

- **The Audit** — a second person counts the stores. It costs half a day of one survivor's time and reveals the true figure to the player, not to the room. What the player does next is the game.
- **The Count** — a weekly public reading in the common mess hall. It raises legitimacy when the number is what people expected and lowers it when it is not; a shortage *honestly announced* costs less than a shortage discovered. (Legitimacy is written only through the existing leadership/politics owner's public API — **VERIFY** in P0.)
- **The Discrepancy** — the unexplained, once known, is an *incident*: it feeds the tribunal as evidence clues (existing `evidenceClues` machinery) and, if *Shelter Governance* has a Hoarding statute, becomes a chargeable act.

## 6. The staircase between fairness and the corpse-menu

Hard Table events (authored, data-driven, ~12 at first) sit on the existing event path, gated by tier, resentment and the Book, and **always before** the desperation menu:

- **The Short Tin.** A count that is one tin short, in front of everyone.
- **Second Portion.** Someone is seen taking it. Not a thief. A person.
- **The Quartermaster's Thumb.** The Book is right; the store is not.
- **The Lottery Loser.** By lot, and a child drew nothing three days running.
- **A Fast for the Dead.** A voluntary austerity, which the shelter can choose — a ration cut that *raises* morale and cohesion once, if the dead were recent.
- **Children First, Again.** The Need rule holds, and the workers say nothing, loudly.

The desperation menu stays what it is: the last thing, never the first, and never softened.

## 7. Feast and fast

The shelter's celebrations already exist. The Ration Wars lets a celebration *cost food* (a feast burns surplus for a resentment reset and a morale lift) and lets the Assembly *vote to fast*. Both go through the existing celebration and policy owners. **VERIFY** in P0 whether celebrations can declare a cost; if not, feast is a Table Rule flag and nothing more.

---

# PART III — HOW IT MEETS THE WORLD

## 8. Four stories

**The winter Ilya kept the book.** The quartermaster is the shelter's honest old engineer. In the ninth week he shorts himself for a child and writes it down wrong so nobody will argue. The Count is a tin short and the player has the audit report in hand. Read it out loud, or fix the number in private?

**By lot.** The player chooses the lottery because nobody can hate a coin. For eleven days it works. On the twelfth, the coin has taken three portions from the same person, and everyone has stopped meeting the eye of the one it favoured.

**The tin behind the boiler.** The Book balances. The store balances. The Audit finds nothing. And yet the resentment meter for one survivor sits at 0.83, three hundredths short of theft, and their portion is exactly what the rule says it should be. Some hungers the arithmetic does not reach.

**The Count that went well.** Ration Half, three weeks without a delivery, and the Count comes out exactly as read. Nobody cheers. But the room is quiet in a different way, and legitimacy has moved in a direction the player has not seen it move in weeks.

## 9. Voice samples

> **Book, day 132.** *Opened: 214. Received: 0. Served: 61. Spoiled: 3. Closed: 149. —I. (Written twice. The first time I got 150.)*

> **Mess hall.** *He reads it slowly. Nobody looks at the tins. Nobody looks at anyone.*

> **Overheard.** *"By work, then. And when I can't lift?" — "Then you'll be counted by need, and you'll hate that too."*

> **The audit.** *Two fewer than the book. Not stolen: nothing was forced. Not spoiled: the shelf is dry. Just two, gone quietly, in a week when a child's cough got worse.*

## 10. Boundaries with other expansions

| With | Boundary |
|---|---|
| **Shelter Governance** | The Assembly consents to a Table Rule change and owns the Hoarding statute; the Ration Wars only consumes them. |
| **The Long Siege** | A siege applies a *Siege Table* preset (a Table Rule plus a tier); the Ration Wars owns the rule, the Siege owns when it is imposed. |
| **The Plague Year** | Quarantine wards get rations through the same table; a ward is a Priority group, not a new line. |
| **The Record Keepers** | The Book is a *record*; the Keeper's slant may colour how it is written, and custody applies. |
| **The Quiet War** | A quartermaster can be an informant's lever (a hungry child is leverage); no shared state. |
| **Year Two** | Outposts eat from their own stores; supply runs are Year Two's. The Ration Wars covers the *shelter's* table only. |
| **The Deep Works** | A cold store in a held drift may improve spoilage — only through an existing storage modifier, if one exists (P0). |

## 11. Content plan

- W1: Table Rules (5), first six Hard Table events, Book lines.
- W2: Quartermaster voices, Audit and Count lines, Discrepancy tribunal clues.
- W3: Feast and fast content, remaining events (target ≈ 12).
- W4: Cross-plan hooks (siege table, ward priority) — ship dark until both ends exist.

## 12. Non-goals (restated)

No new food type, inventory, stockpile or serving log; no change to tier arithmetic, priority groups or desperation content; no second resentment meter; no law rows (Governance owns them); no new routed panel; no Unity.

## 13. Risks

| Risk | Mitigation |
|---|---|
| Ledger noise (no withdrawal reasons) | Fall back to the kitchen serving log only; P0 decides. |
| A hoard that creates food | Hoard only as a real transfer; otherwise dropped (DEC-RW-05). |
| Bookkeeping fatigue | The Book is a daily line and a weekly Count; never a spreadsheet. |
| Feels like punishment for being hungry | The staircase is a story of small choices; every step has a way to be kind. |
| Overlap with Governance | Hoarding law consumed, not authored; soft dependency. |
