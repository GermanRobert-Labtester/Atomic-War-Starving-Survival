# Feature / Task Plan: The Feeding Place (a gift to the wild at the yard's edge) & The Lamps (the analogue layer of light)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 5, plans 2 of 4 (subjects 51–52).

> **Subjects covered (2 of the 8 in the "counted days and tended edges" batch):**
> 51. **The Feeding Place** — a plate left at the yard's edge for whatever is out there: a gift
> to the wild, kept as a custom and never as a mechanic. (Prefix `FP`.)
> 52. **The Lamps** — the analogue layer of light: oil and candle, the lamp route at night, and
> the one lamp always lit in a room nobody uses. (Prefix `LP`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/plans/WILDLIFE_TRAPPING_FLAGSHIP_IMPLEMENTATION_LOG.md` (the harvest — the exact
> opposite), `.ai/plans/second-nature-and-ruins-of-the-before-2026-09-29.md` (the wild; species
> catalog read-only), `.ai/plans/evenings-and-memory-work-2026-09-29.md` (custom machinery; the
> outside crumb), `docs/plans/PLAN_22_GREENHOUSE_RUNTIME_ITEM_CONSUMPTION.md` (consumption
> precedents), the power-grid owner (electric light is *not* this plan).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Plate and the Flame

> *"A shelter's borders are drawn twice: once by what it keeps out, and once by what it sets
> outside the door on purpose."*

The feeding place is a flat stone at the edge of the yard and a plate on it. Every few days
someone puts something out — crusts, trimmings, the parts of a meal nobody claimed — and every
few days the plate is empty. Sometimes there are tracks. The tracks are recorded and never
attributed; the species catalog is right there and the ledger declines to fill in the blank. The
trapper works the other side of the same line, and both of them are kept by the same household
without contradiction, because the line is not between people and animals. It is between hunger
and hospitality.

The lamps are how the household holds the line at night. The grid owns the electric light; the
lamps own the flame — the oil route after dusk, the candle in the sick room window, the shelter's
one small argument with the dark that predates the power station and will outlast its next
failure. The lamp route is walked every evening in an order older than the rota. And in the
eastern bay, a room nobody uses, one lamp is always lit — filled, trimmed, burning, and
recorded in the format exactly like every other.

**Tone & register.** Hearth-edge quiet and forester-plain. The feeding place's vocabulary is
*plate, left, empty, tracks, kept*; the lamps' is *lit, filled, trimmed, route, flame*. Prose
should read like a household's end-of-day notes — unhurried, observant, unfooled. Nothing here
is magical. The wild is hungry; the flame is chemistry; the customs are older than both.

**Mystery & texture.** Two silences hold the pair. The feeding place's tracks are recorded and
**unattributed** — the ledger knows the species catalog exists and refuses to consult it (§12).
And the eastern bay's lamp is lit by no recorded hand: the route log shows it filled, trimmed,
burning, and the route's own keeper denies the bay is on the route (§12). Neither is a haunting.
Both are what happens when customs run longer than their paperwork.

**The second layer.** The plate and the flame are the two oldest payments a household makes to
what surrounds it: one to the living world, one to the dark. Neither buys anything measurable —
no tameness, no safety, no return. The plan's quiet thesis is that a shelter becomes a *place*
the day it starts paying these two debts, and that both payments are only real because nothing
compels them. The unattributed tracks and the off-route lamp are the receipts for gifts nobody
accounts for.

---

## 1. Goal & Outcome

> *Design intent: the player should see the plate empty in the morning and understand that
> something ate — and see the bay lamp burning and understand that something is kept.*

### 1.1 The Feeding Place (FP)

- **Goal:** A **Feeding Row** set (one authored stone: location id, cadence, allowed food tags
  from the existing item vocabulary) and a minimal **Plate Ledger** (placed-day, `empty_day`,
  `tracks_recorded` count, `attributed: false` permanently — DEC-FP-01); one verb — **Place**
  (ordinary inventory transaction: food leaves the stock and becomes "out"); and one derived
  **Plate Read** on the existing board surface. Consumption is *authored as absence*: the plate
  empties per its cadence row and no animal, species or entity is ever created, spawned or named
  (DEC-FP-02).
- **Outcome (observable):** on a fixed seed, placing crusts empties the stock through the
  inventory owner; the plate read shows "out since day N"; an `empty_day` arrives per cadence
  with a seeded `tracks_recorded` count that is never attributed; one optional `Heard` line may
  surface in the Living Region register ("the plate is taken again"); with no rows every owner
  behaves identically to today; save/load round-trips.
- **Non-Goals:** no wildlife mechanics (Wildlife Trapping and Second Nature keep their meanings);
  no species attribution, taming or population effects; no bait/hunting synergies of any kind; no
  food-chain modelling; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Lamps (LP)

- **Goal:** A **Lamp Route** table (≤ 12 lamp rows: lamp id, position, fill-cadence, `off_route`
  flag for the eastern bay) and a minimal **Lamp Ledger** (lit/trimmed/filled states as history
  facts); one duty — **Lamp-Walk** (existing duty-roster seam; consumes oil through the inventory
  owner); one derived **Route Read** (which lamps are lit, which need filling, in the route's
  authored order); and plain rendering beside the existing stock/board surface. Electric light is
  untouched: the lamps are the analogue layer (DEC-LP-01).
- **Outcome (observable):** on a fixed seed, the evening lamp-walk fills lamps per cadence and
  consumes oil through the inventory owner; the route read renders the authored order with each
  lamp's state; the `off_route` bay lamp renders `lit` with its off-route mark and no fill
  requirement; a failed walk renders plainly ("route unlit, one night") with no penalty beyond the
  dark itself; with no rows every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no lighting/power mechanics (grid owner keeps its meaning); no visibility or
  stealth effects of any kind (DEC-LP-02); no fuel-economy changes; no fire hazards (Exp. 23
  owns fire); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: the feeding place's plate may render
  one lamp line at dusk ("the plate is out; the edge lamp is lit") through a single string; and a
  lamp-walk may note the plate's state in the route read. One string each way, both shipped dark.

---

## 1b. Texture, Mystery & Voice

**Absence is the whole mechanic.**

The plate empties; nothing is ever shown eating. The design's discipline — no spawn, no species,
no entity (DEC-FP-02) — is what keeps the feeding place a *custom* instead of a *farm*. Prose
must honour the same rule: describe the plate before and the plate after, and never the middle.

**The route is older than the rota.**

The lamp order should feel inherited: named positions, an unvarying sequence, a walk that is
half errand and half observance. The `off_route` bay lamp renders flatly — the format's fairness
is the mystery's engine, exactly as in the birthday book and the night rota.

**What the player is never told.**

- What takes the food. The tracks are counted and never attributed (DEC-FP-01); the species
  catalog exists and the ledger refuses to consult it (§12).
- Who fills the eastern bay's lamp. It renders `lit`, it is marked `off_route`, and the route's
  own keeper denies the bay is walked (§12).
- Whether the plate and the trapper's line are the same line. Both are kept by one household
  (§0); the plan observes the coexistence and declines to reconcile the ethics.
- Why the route's order never changes. The positions are authored; the sequence's origin is not
  modelled and must not be.

**Voice — sample fragments (content candidates for `feeding_place_lines.json` / `lamp_route_lines.json`).**

> "Plate: out, day 61. Crusts and the end of the stew. The stone is at the edge because that is
> where the yard stops being ours." — plate read (FP)

> "Empty, day 63. Tracks recorded: two. Not attributed. The ledger's blank is deliberate and the
> blank is the point." — plate read (FP)

> "Someone leaves the good bread. The plate ledger does not record who leaves the good bread and
> the good bread keeps arriving." — plate read (FP)

> "Lamp route, dusk: gate, well, store, wall, gate. Five flames in the order the household has
> always walked them." — route read (LP)

> "Eastern bay: lit. Off-route mark. Filled — the log says filled — by the route's own hand, and
> the route's own hand says the bay is not on the route." — route read (LP)

**Design texture beats.**

- **No attribution, no species, no entity** (DEC-FP-01/02). The moment the tracks are named, the
  gift becomes a transaction and the wild becomes a pet.
- **The off-route lamp renders flatly** (DEC-LP-03). Same plain type as the gate lamp; the format
  refuses to editorialise.
- **A failed walk is plain, not punished** (inherits DEC-NS-01's spirit): "route unlit, one
  night" — the dark is the consequence.
- **Oil is ordinary stock.** The flame's cost is the same arithmetic as the stove's; the ritual
  is what the arithmetic is spent on.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the stone leaves lying around.**

> "Plate, cracked across and mended. It is the only dish in the house that gets mended instead of
> replaced, and the mending is on the underside where the guests cannot see it."

> "Track sheet: 'two, small, unattributed.' The sheet's blank column has been blank for two
> hundred days and the sheet has never been reprinted."

> "Snow, around the stone: marked once, walked back, unmarked again. Whoever checks the plate
> checks it the same way every time." — texture only

**What the flame leaves lying around.**

> "Oil measure, evening: five fills, one measure each. The measure is older than the holder; the
> holder is older than the rota; the rota is older than the record."

> "Trimming tin, on the route's second hook. Wicks are trimmed in the tin and the tin is emptied
> in the yard, in the same two steps, since before either step was written down."

> "Eastern bay, lamp glass, cleaned. Somebody cleans the glass of a lamp in a room nobody uses,
> and the cleaning is the most specific thing anyone does for that room."

**Held silences (texture, not register rows).**

- Whether the good bread's leaver and the bay lamp's filler are the same hand. Two unrecorded
  acts, one household (§12); the plan keeps the pairing unresolved and unasserted. Texture only.
- What the bay was before it was empty. Room history belongs to the Names of the Rooms register
  and is not consulted here.

---

## 1.4 Worked examples (non-normative)

**A plate's week (fixed seed).**

> Day 61 — `Place(crusts, trimmings)` through the inventory owner: the stock falls and the plate
> reads "out, day 61". Day 63's cadence resolves the row to `empty_day` with a seeded
> `tracks_recorded: 2`, `attributed: false` — permanently. One optional line surfaces in the
> `Heard` register: "the plate is taken again." The species catalog is not consulted at any
> point; the diff contains no entity of any kind.

**A lamp night (fixed seed).**

> Dusk, the `Lamp-Walk` duty walks the authored order: gate → well → store → wall → gate. Oil
> decrements five measures through the inventory owner. The route read renders each lamp's state
> in order; the eastern bay renders `lit` with its `off_route` mark and no fill requirement. The
> night it rains and the walk is skipped, the route reads "unlit, one night" — and the household
> navigates the yard in the dark, which is the only consequence the plan needs.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Inventory transactions express food leaving stock as ordinary consumption (greenhouse runtime consumption precedent). | `docs/plans/PLAN_22_GREENHOUSE_RUNTIME_ITEM_CONSUMPTION.md` | LIVE (plan exists) |
| E2 | Wildlife Trapping owns the harvest; Second Nature owns ecology and the species catalog; neither may be changed here. | `docs/plans/WILDLIFE_TRAPPING_FLAGSHIP_IMPLEMENTATION_LOG.md`; SN/RB plan | LIVE (per corpus) |
| E3 | Custom machinery hosts authored customs (EV seam; the outside crumb precedent — `ritual_crust_for_the_waste`, unattributed). | `.ai/plans/evenings-and-memory-work-2026-09-29.md` §1c | PROPOSED (plan is DRAFT) |
| E4 | Living Region `Heard` lines are a read-only surface for regional whispers (LR-P7 correction discipline). | `.ai/plans/living-region-2026-09-29.md` §1b | PROPOSED (plan is DRAFT) |
| E5 | Duty-roster posts exist (lamp-walk; Rounds / link crew / bell-duty precedents); oil is ordinary stock. | batch-3/4 duty posts; inventory owner | LIVE (per corpus) |
| E6 | Exp. 23 *The Alarm* owns fire hazards and emergency response; lamps must not touch it. | `docs/expansions/wave3/expansion_23_the_alarm_plan.md` | LIVE (design bible) |
| E7 | Power grid owns electric light; the Sky/others own visibility contexts. | grid owner; `.ai/plans/the-sky-2026-09-29.md` | LIVE |
| E8 | Whether an additive row-state ledger (plate/lamp history facts) survives checksum in the roster/inventory save owner. | codec / snapshot tests | **VERIFY (P0)** |
| E9 | Board render points for plate and route reads; band precedents. | batch-3/4 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Food stock | inventory owner | `Place` is an ordinary transaction; no new resource |
| Wildlife, species | Wildlife Trapping / Second Nature | nothing; attribution is refused by construction |
| Customs | EV | one optional feeding custom and one lamp custom |
| Regional whispers | Living Region | one optional `Heard` line; writes no news |
| Fire, hazards | Exp. 23 | nothing; lamps are chemistry, not hazards |
| Electric light | power-grid owner | nothing; the analogue layer is this plan's only concern |
| Plate/lamp state | — | `PlateLedger` + `LampRoute` (pure Core; row facts only) nested additively — **DEC-FP-03 / DEC-LP-04** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Edge/PlateLedger.cs` (new, pure), `Shelter/LampRoute.cs` (new, pure)
**Data:** `feeding_place_rows.json`, `lamp_route_rows.json`, `feeding_place_lines.json`, `lamp_route_lines.json`
**Host:** inventory host session (`INT`), roster host session (`INT`), board surface (`INT`)
**Presentation:** a "Plate" line and a "Route" band on the existing board/stock surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Edge/PlateLedgerTests.cs`, `Shelter/LampRouteTests.cs`, `Ashfall.Core.Tests/Save/PlateLampSaveTests.cs`

## 5. Packages

### FP-P0 — Premise audit (Auditor; read-only): close E8–E9; confirm the food-tag vocabulary; confirm the `Heard` hook wording with the Living Region owner.
### FP-P1 — Plate rows + ledger (Core + data): cadence, absence-resolutions, tracks-unattributed, validator row-level. **Accept:** no entity field exists anywhere (§6.3); determinism; round-trip.
### FP-P2 — Place verb + plate read (host): ordinary transaction; optional `Heard` string, dark. **Accept:** conservation; the species catalog is never consulted.
### LP-P1 — Lamp route + ledger (Core + data): ≤ 12 rows, `off_route` flag, fill-cadence, states as history facts. **Accept:** no visibility effect exists anywhere (§6.4).
### LP-P2 — Lamp-walk duty + route read (host): oil through inventory; plain flat rendering. **Accept:** failed walks render plainly with no penalty.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no plate rows and no lamp rows → inventory, wildlife, ecology, grid and board outputs identical on a saved corpus.
3. No-entity invariant: the diff contains no spawn, species attribution, taming or wildlife effect of any kind (DEC-FP-01/02).
4. No-visibility invariant: the lamps produce no stealth, visibility or hazard effect of any kind (DEC-LP-02).
5. Conservation: placed food and lamp oil balance exactly through the inventory owner (E8 closed at P0).
6. Determinism: identical cadence resolutions, track counts and route states on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Flat-rendering invariant: `off_route` and `unattributed` marks render in the same plain type as all other rows (DEC-FP-01, DEC-LP-03).
8. The registered silences of Wildlife Trapping, Second Nature, EV and Exp. 23 stay unanswered (§7).

## 7. Cross-plan boundaries
- **Wildlife Trapping / Second Nature:** the harvest and the wild are theirs; the feeding place is the household's *gift* and never an ecology input. The unattributed tracks are the deliberate seam.
- **Evenings and Memory Work:** custom machinery is theirs; the outside crumb (`ritual_crust_for_the_waste`) is a sibling custom and the two must never merge.
- **Living Region:** one `Heard` string at most; news is its authority.
- **Exp. 23 / power grid:** fire and electricity are theirs; the lamps are chemistry in glass and stay that way.
- **The Mending / The Loan Shelf (batches 3–4):** the mended plate's underside and the loaned lamp are read-only citations at most.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-FP-01 | Tracks are counted and never attributed; `attributed: false` is permanent. | tone/rule | Yes |
| DEC-FP-02 | No entity of any kind: the plate empties by authored cadence, not by an eater. | architecture | Yes |
| DEC-FP-03 | The plate ledger stores row facts only and nests additively. | architecture | Yes; confirm in P0 |
| DEC-FP-04 | The good bread's leaver is never recorded; the ledger's blank is deliberate. | tone | Yes — **needs canon note** |
| DEC-LP-01 | The lamps are the analogue layer; electric light is untouched. | boundary | Yes |
| DEC-LP-02 | No visibility, stealth or hazard effects of any kind. | rule | Yes |
| DEC-LP-03 | The `off_route` lamp renders flatly, like every other row. | tone | Yes — **needs canon note** |
| DEC-LP-04 | Lamp states are history facts in an additive ledger; no new save section. | architecture | Yes; confirm in P0 |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Feeding`, `Plate`, `LampRoute`, `Lantern`, `Torch`)
- [ ] Premise re-verified (Rule 7); Wildlife Trapping, SN/RB, EV and Exp. 23 registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-FP-01, DEC-LP-01)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing inventory, wildlife and roster tests (list from P0 selector)
- [ ] Inventory host selftest with plate/lamp rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any surface would require naming what takes the food; a lamp would need a visibility effect to justify itself; the off-route lamp cannot render flat by construction; consumption would touch the wildlife or ecology owners; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the plate outside the ledger and the flame ahead of the record. Any future
plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| FP-OM-1 | What takes the food? | Tracks counted, never attributed (DEC-FP-01); the species catalog exists and is refused by construction. | Never — deliberately sealed. |
| FP-OM-2 | Who leaves the good bread? | The ledger's blank is deliberate (DEC-FP-04); generosity is recorded as an act, never as an actor. | Never — a rule, not a gap. |
| FP-OM-3 | Is the feeding place older than the shelter? | The stone and the custom predate the record (§0); the plan observes and does not date. | Canon owner only, as a signed decision. |
| FP-OM-4 | How does the trapper sleep at night? | Both halves of one household (§0); the ethical seam is observed and deliberately unreconciled. | Never — texture by omission. |
| FP-OM-5 | Was the plate ever not taken? | Two hundred empty plates in the ledger; whether the series has a gap is not asserted (inherits NS-OM-2's discipline). | Never — tone-locked. |
| LP-OM-1 | Who fills the eastern bay's lamp? | `off_route`, rendered flatly (DEC-LP-03); the route's own keeper denies the bay and the format records both. | Never — deliberately sealed. |
| LP-OM-2 | What was the bay before it was empty? | Room history belongs to the Names of the Rooms register and is not consulted here. | The NR plan's owner (its register governs). |
| LP-OM-3 | Why is the route's order the order? | The positions are authored; the sequence's origin is unmodelled and must not be. | Never — texture by omission. |
| LP-OM-4 | Are the good bread and the bay lamp the same hand? | Two unrecorded acts, one household (§1c); the pairing is kept unresolved and must never be asserted (mystery-index §3 discipline). | Never — the pair stays open. |
| LP-OM-5 | Does anyone still know what the lamp route was for? | The walk is half errand, half observance (§1b); the purpose is not in the format and the household keeps the walk anyway. | Never — a rule, not a gap. |
