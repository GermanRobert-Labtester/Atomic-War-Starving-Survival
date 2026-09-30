# ASHFALL — THE UNDERWORLD
### Smuggling, brokers and bounty hunters · The underworld keeps score. Now somebody comes to collect.

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/underworld-2026-09-29.md`
**Family:** "New Pressures and Places" — `docs/expansions/expansion_new_pressures_and_places_index.md` (subject 14 of the user's list).
**Tone lock (inherited):** cold, exhausted, human, restrained. Crime here is a market with a cruelty problem, never a tutorial: no real criminal instructions, no real drug-trade detail, no real countries or people.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.
**Name note:** the syndicate *The Quiet Counter* and Expansion 41 (*The Quiet*) and *The Quiet War* are three different things; ids are distinct.

---

> *"Nobody in this world trades goods. They trade the gap between what a thing is worth and what a
> person can bear to pay for it today."*
>
> The market did not end when the bombs fell. It went *downstairs*. What the shelter calls the
> underworld is three syndicates wearing one coat — and the difference between them is not what
> they sell, but what they remember about you.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

ASHFALL has an underworld and it is well kept. Three syndicates — **The Quiet Counter** (the folding-table people), **The Ash Market** (brokers who trade in scarcity itself), **The Cold Ledger** (they lend first) — sell seven kinds of illicit stock at a visible premium. There is trust and there is heat. There are loans, interest, a due date, a default. A default costs trust, adds heat, and — through the bounty owner — puts a *mark* on the shelter. Separately, a heat engine tracks how close a syndicate is to attention and knows how to relocate; a loan-shark engine escalates a debt through Grace, Delinquent and Defaulted and rolls an "enforcer raid risk" into a number. Elsewhere, the player can post and take contracts on a bounty board, with rival hunters ticking in the background.

Read the code with one question: *does anyone ever come?* The mark on the shelter is a standing penalty attached to a faction id; nothing reads the list of active marks. The heat engine's raid-risk and the enforcer's raid-trigger are public methods no gameplay code calls. "Bounty hunter" occurs once, in an event's flavour text. The underworld keeps score with great care. It never collects.

And nothing is ever *smuggled*. Goods do not cross a border against anyone's wishes; the word appears in faction names and flavour lines. The syndicates are ids, not people — a "contact discovered" flag, not a face across a table.

The Underworld is the expansion where **the shadow economy has people in it and consequences that walk up to the door.** You meet a broker, a person with a cut and a temper and a secret. You take a run — a manifest, a road, a checkpoint — and you find out what "risk premium" means when the person carrying it is one of yours. And when you default, or fence the wrong thing to the wrong hand, or simply become visible, someone who has been paid to find you starts to find you: first a rumour, then a stranger on the road, then a knock.

The promise: **every debt has a collector, every run has a checkpoint, and every deal has a person you will have to face again.**

Read the three kinds of person in this expansion and notice what they have in common. A **broker**
is not a shopkeeper; they are someone who knows what your name is worth in three districts and will
lend you that knowledge at a cut. A **run** is not an expedition; it is a manifest leaving with
somebody's cousin. And a **hunter** is not a soldier; a hunter is what a number looks like when it
finally stands up and walks to your gate.

The machinery of consequence has been here all along. Four engines run in the dark today with no
gameplay caller — the attention engine, the enforcer raid trigger, the contraband executor, the
bounty reader. Feeding them is not a feature addition. It is **waking something up** that has been
patient and unread since it was written.

### 1.2 Pillars

1. **Brokers are people.** Named, bound to a syndicate, with a cut, a temper and a secret.
2. **A run is a promise with a manifest.** Goods leave; goods arrive, or they don't.
3. **Scrutiny is read from the world.** Embargoes, patrols, the war — the checkpoint is as hard as the region is watched.
4. **The mark has a face.** An active bounty becomes a hunter with a trail you can read.
5. **Every debt has a collector.** Heat and enforcer risk finally have a consumer.
6. **Ship dark.** No brokers, no run rows, no hunter rows — the black market behaves exactly as today.

### 1.3 Not this

Not a crime simulator or a how-to. Not a second market, second bounty authority or third debt ledger. Not a "criminal path" with its own progression. Not gratuitous violence: hunters are people with contracts, and the player's answers include paying, hiding, dealing and leaving.

---

## 2. What the code and data actually say (audit)

| Area | What exists | What is missing |
|---|---|---|
| Syndicates | 3 syndicates, 7 stock entries, premiums, credit limits, access tiers (`black_market_inventory.json`); `BlackMarketSystem`: contact discovery, deterministic daily stock, price previews, buy/sell, loans, repay, trust and heat. | Syndicates are ids; contact discovery is a flag. |
| Debt → mark | On default: trust −30, heat +25, then a patrol bounty is issued through `FactionBountySystem`. | The mark is a standing record; **nothing reads active bounties**. |
| Heat | Two models. **Live:** `BlackMarketSystem` keeps a per-syndicate heat 0–100 (default adds +25, daily decay by profile). **Second:** `BlackMarketHeatAttentionEngine` (bands Calm→CriticalLockout, cooling, relocation, `GetRaidRiskPermille`, `CheckRaidTrigger`) is ticked daily but **never fed** in play — its only feeder, `AddSyndicateHeat`, is called from a self-test CLI. | The attention engine's raid methods have **no gameplay caller**, and its heat is always zero in a real campaign. |
| Loan shark | `LoanSharkEnforcerEngine`: escalation stages, sanction check, enforcer raid risk and trigger. Ticked daily. Separate `loan_shark` save. | `CheckEnforcerRaidTrigger` has **no caller**; its debts are a **second ledger** beside the black market's. |
| Bounties | `MercenarySystem`: 4 templates (assassination, capture, bounty hunt, beast cull), post/accept/claim, rival hunter id and progress. | Nothing ever hunts the *shelter*. |
| Contraband | `ContrabandClassification` (Unregulated→MilitaryHardware), fence action, stash claims, a "contraband broker" caravan at the shelter barter counter. | Classification never meets a checkpoint. |
| Border | `CrossingCatalog` names `faction_the_smugglers_court`; `TradeEmbargoSystem` gates routes by weather. | No inspection, no manifest, no run. |
| UI | `BlackMarketPanel`, `MercenaryBountyBoardPanel`. | No brokers, no hunters. |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Two debt ledgers.** `black_market` (`UnderworldDebtRecord`) and `loan_shark` (`LoanDebtRecord`) each track loans. → This plan **reads both** and adds a read-only combined player view; it merges nothing. (DEC-UW-04)
2. **A mark belongs to a faction, not a person.** Hunters need a target. → The target is the shelter's public *face* (its leader), never a random survivor and never a child. (DEC-UW-08)
3. **Expeditions carry nothing outbound.** Outbound is travel time only. → A run cannot ride an expedition; it is its own small in-transit record mirroring the engine's existing escrow and payout action types. (DEC-UW-03)
4. **Two heat models.** The live per-syndicate heat and the unfed attention engine. → The attention engine is *fed from* the live heat by one bridge; no second number is invented. (DEC-UW-09)

---

# PART II — THE SHADOW

## 3. Brokers

A **broker** is an authored person (roughly eight, spread over the three syndicates) with:

| Trait | Meaning |
|---|---|
| **Syndicate** | Who they answer to |
| **Territory** | The regions they can open |
| **Cut** | Their share of a deal; the player sees it |
| **Temper** | Patient, greedy, sentimental, frightened |
| **Secret** | What they would sell you for |
| **Standing** | Their trust in the shelter, derived from the syndicate's existing trust ledger plus their own deals |

A broker is met when the syndicate's contact is discovered (the existing call). They offer **runs** and introductions; they can be cultivated, paid, lied to, and — once — betrayed, by them or by you.

## 4. Scrutiny

**Scrutiny** is a number the player can read, per region, derived only from things that already exist: the embargo and route-blocked state, faction patrol pressure, the war, the shelter's own heat. It is **read-only** — this plan never writes to any of them. A high-scrutiny region makes checkpoints harder and premiums higher. It changes when the world does.

## 5. Runs

A **run** is a broker's job: carry this manifest (a few item ids and counts, classified) from here to there by this day. On dispatch the goods **leave inventory** through the standard atomic billing and sit in an in-transit record. On the due day the run resolves against scrutiny and the runner's skill (and vehicle, if any):

| Outcome | What happens |
|---|---|
| **Clean** | Delivered. Payout through the existing settlement path; trust up; a little heat down. |
| **Stopped** | Goods seized; a standing hit with the watching faction; the runner returns. |
| **Burned** | Goods lost, the runner hurt or held; heat up; a mark may follow. |

The runner is a survivor from the roster — through an expedition-like absence — or, if *Crews and Companions* exists, a party.

## 6. The Hunter

When the shelter carries an **active mark** — a defaulted loan, a fenced-and-traced item, a broken run — a **hunter** may take the trail. The hunter is an authored archetype (about six: the Patient, the Loud, the Collector, the Old Friend, the Ledger-Reader, the Pair) with a contract, a patience and a way of working.

The trail has four legs, each readable:

1. **Word.** A rumour reaches the shelter — through the news, the radio, a broker's warning. The player sees it as *Heard*, not *Seen*.
2. **Road.** A stranger on the approach; an expedition encounter, or a sighting from the watch.
3. **Door.** Arrival at the gate, as a *traveller* whose claim hides a purpose *(The Quiet War's adapter)*.
4. **Standoff.** The hunter and the shelter, with a bounded time to choose.

Arrival probability is drawn from the risk numbers that today go unread: the enforcer's trigger, and the live per-syndicate heat (with the attention engine fed from that same heat, so its bands finally mean something — one heat, never two).

## 7. What you can do about a hunter

- **Pay.** Repay through the existing repay call; the mark clears through the bounty owner's own resolve.
- **Deal.** The broker intercedes for a cut and a favour.
- **Hide.** Close the gate (a Gate Protocol, *The Plague Year*'s idea, without borrowing its state).
- **Turn it.** Post a counter-bounty on the hunter through the existing mercenary posting.
- **Lead it off.** A decoy runner draws it down a road.
- **Fight.** Through the existing encounter authority; expensive, visible, and the mark stays.
- **Leave.** Give the shelter's *face* away — a handover the player will remember.

## 8. The whole shape

The player who never touches the underworld sees nothing. The player who buys stim kits at the Quiet Counter learns a broker's name. The player who takes a loan from the Cold Ledger learns what "Delinquent" looks like from the other side. The player who runs a manifest for the Ash Market learns what scrutiny is. Any of them, a season later, can be looked at by a stranger who has been paid.

---

# PART III — HOW IT MEETS THE WORLD

## 9. Four stories

**Osei's cut.** The Ash Market broker takes eleven percent and tells you why. In the third week he takes twelve. In the fifth he stops telling you.

**A clean run.** Three crates, four days, a checkpoint at the weighbridge. It goes right, and nobody in the shelter can quite say why the mood is worse.

**The Collector.** He arrives on the eighth day after the default, on foot, and asks the watch for a cup of water. He is polite the whole time. He has all the time in the world.

**A counter-bounty.** The player posts a contract on the hunter. It is answered by a woman who has been looking for the same man for two years.

## 10. Voice samples

> **Broker.** *"Eleven. It was ten last year. The road got longer and I got older. You will find it is cheaper than being stopped."*

> **Watch log.** *Man on foot, north road, one bag, no hurry. Sat down by the culvert at noon. Still there.*

> **Ledger line.** *Day 96. Delinquent. Due: 40 units. Enforcer risk: 31 per cent. Nobody has come. Nobody has said nobody will.*

## 11. Boundaries with other expansions

| With | Boundary |
|---|---|
| **The Quiet War** | Hunters arrive through its gate adapter; a hunter's claim is its Claim/Truth pair; no shared state. |
| **The Long Line: Freight** | If it exists, a run may be a contraband contract on its route run; until then, the in-transit record stands alone. |
| **The Plague Year** | The embargo trigger and gate protocol are read only. |
| **The Living Region** | Scrutiny reads its region flags; hunters' rumours use its news grades. |
| **Crews and Companions** | Runs may be parties. |
| **Radio Free Ashfall** | A broker's warning is a short broadcast; no shared store. |
| **Shelter Governance** | A shelter that makes contraband illegal changes scrutiny for the shelter's own runs; the Assembly may order a broker turned away. |
| **The Sky** | Impact salvage can be fenced; classification applies as normal. |
| **Faith and Schism** | A pilgrim can be a hunter's cover, and a hunter can be a pilgrim — a claim, not a system. |

## 12. Content plan

- W1: brokers (8), hunters (6), run manifests, trail rumours.
- W2: checkpoint outcomes, standoff options.
- W3: counter-bounty and lead-off stories, the Old Friend, the Pair.
- W4: cross-plan hooks (gate, freight, crews) — ship dark until both ends exist.

## 13. Non-goals (restated)

No real-world crime instruction; no new market, bounty or debt authority; no change to premiums, heat or loan arithmetic; no auto-combat; no new save section; no new routed panel; no Unity.

## 14. Risks

| Risk | Mitigation |
|---|---|
| Glamorising crime | Every path has a cost and a person; no "criminal progression" track. |
| Hunter griefing | A trail has four legible legs, seeded arrival, and always an off-ramp. |
| Ledger confusion | Combined view is read-only and labelled by source. |
| Run bookkeeping | One manifest, one due day, one outcome. |
| Duplicate authority | Bounty, debt and market remain with their owners. |

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** Every debt in this expansion is denominated in *being known*: the broker
knows what your name is worth, the mark makes your face a commodity, the hunter is what happens
when the knowledge acquires a courier. The consequence engines ran unread in the dark for a long
time. Feeding them is not a feature addition — it is waking something up.

**What the expansion leaves lying around.**

> "Cut percentage, one column, no name. The ledger keeps books of the ledger."

> "A hunter's kit is never itemised — the four-leg form suggests practice, practice suggests a teacher, and the expansion stops at the first sentence."

> "Loan ledger B, beside loan ledger A. The combined view is labelled by source and promises nothing."

**Scenes the player may piece together.**

> "Word arrives in the third person; by the Door leg the sentence is in the second person. Grammar as escalation."

> "A broker raises the cut after a bad week. Characterisation with arithmetic — and arithmetic does not perjure itself."

**Held silences (texture — the register below is unchanged).**

- What a default looks like from the broker's side of the counter. `temper` implies it; `secret` forbids proving it.
- Whether the mark outlives the debt. The resolve path clears the mark; nothing anywhere says the remembering stops.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**What the Cold Ledger is keeping books *of*.** The three syndicates are differentiated by method.
Purpose is deliberately unauthored so each campaign can read its own into it.

**Who taught the hunters the four-leg form.** Archetypes are authored. Their training is not.
Naming a school turns dread into lore.

**Are there other debt ledgers besides the two?** The combined view is *labelled by source*. It does
not assert totality. It never promises those are all the ledgers.

**Why `CheckEnforcerRaidTrigger` was written before anyone called it.** An uncalled method that
models a consequence reads like a prepared one. This expansion wakes it. It does not explain who
prepared it.

**Does a broker ever *want* you to default?** `temper` implies it; `secret` forbids proving it.
Both readings survive and neither is rewarded.
