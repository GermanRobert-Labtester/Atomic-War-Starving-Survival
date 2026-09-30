# Feature / Task Plan: The Visits Paid (outbound calls on neighbours) & The Market's Third Stall (the ordinary commerce everyone passes)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 7, plans 1 of 4 (subjects 65–66).

> **Subjects covered (2 of the 8 in the "outward spring" batch):**
> 65. **The Visits Paid** — the shelter pays calls: who goes, what is brought, how long is
> stayed, and the caller's side of the hospitality ledger. (Prefix `VP`.)
> 66. **The Market's Third Stall** — the ordinary stall at the market: the same wares, the same
> keeper, the same price of attention, every market day. (Prefix `TS`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` (the Guest Book keeps the *door's*
> ledger; the Visits keep the *caller's*), `.ai/plans/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md`
> (a call may carry one envelope), `docs/expansions/wave7/expansion_45_the_envoy_plan.md`
> (diplomacy — visits are not diplomacy), `.ai/plans/the-underworld-2026-09-29.md` (premiums and
> syndicates are theirs; the third stall is the anti-drama of trade), the barter/inventory owners
> (all transactions are theirs).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Call and the Stall

> *"Hospitality is not what happens when strangers arrive. It is what happens when yours go
> out."*

A visit is a small expedition with no objective: two people walk to the wet road, carry
something, sit for an afternoon, and walk home. Nothing about it is efficient and everything
about it is load-bearing — the visit book records destination, bearer-roles, what was brought and
how long was stayed, and it is the caller's side of the ledger the Guest Book keeps at the door.
The two books are never merged. One is what the house *owes*; the other is what the house *pays*.

The third stall is the market's furniture. Between the knife-grinder and the woman with the eggs
there is a stall with the same three goods every market day: cord, candles, and a jar of
something nobody buys. Its prices are plain, its keeper is a role and not a person, and its wares
never change. Every plan in the corpus is about the dramatic corners of trade — premiums,
truces, embargoes — and the third stall is about the other ninety percent: commerce so ordinary
it is load-bearing, kept by someone whose name the record does not need.

**Tone & register.** Courteous and counter-plain. The visits' vocabulary is *called, brought,
stayed, returned, owed*; the stall's is *cord, candle, jar, price, market day*. Prose for the
visits should read like a social ledger kept by someone who knows that showing up is the whole
art; prose for the stall like a price list written in a careful hand and never revised.

**Mystery & texture.** Two silences hold the pair. The visit book contains a **standing
call to a place no row names** — a destination kept in the book's own shorthand, paid every
season like the rest (§12). And the third stall's **jar is never sold and never refilled** — it
is priced, it is full, and the record's `sold` column for it is empty since the first market day
(§12). Neither is a puzzle. Both are what happens when courtesy and commerce run longer than
their paperwork.

**The second layer.** Both subjects are *the unglamorous half of relationship*. A call is
attention paid in advance of need; a stall is trust priced in advance of sale. The visit book and
the price list are the two documents a household keeps *for other people* — one says "we came",
one says "this is what it costs" — and both are written to be read across a counter or a
doorway. The plan's quiet thesis is that a shelter's character is decided at its edges: what it
carries out, and what it lets stand in its market.

---

## 1. Goal & Outcome

> *Design intent: the player should read the visit book and see a season of paid calls — and
> pass the third stall and see commerce so ordinary it holds the market up.*

### 1.1 The Visits Paid (VP)

- **Goal:** A **Visit Rows** table (≤ 12 rows: destination id (existing settlement/location ids
  or `shorthand` for the anomaly), bearer-roles (never names — DEC-VP-02), `brought` (item kinds
  from a closed list), `stayed_days`, `seasonal` flag) and a derived **Visit Read** (this
  season's calls, owed vs. paid, the standing call rendered flatly); one verb — **Call** (an
  outbound visit: its bearer-roles and `brought` goods move through the ordinary inventory
  transaction, and the row is written); one optional carriage hook (caravan/guest seams, batch-3
  discipline: one boolean, shipped dark).
- **Outcome (observable):** on a fixed seed, `Call` writes one row and moves its goods through
  the inventory owner; the visit read renders the season's calls with owed/paid standing; the
  `shorthand` destination renders flatly like every other row; a carried envelope renders its
  address string and nothing else (envelope discipline inherited); with no rows every owner
  behaves identically to today; save/load round-trips.
- **Non-Goals:** no travel mechanics (expeditions own movement); no diplomacy, reputation or
  faction effects (Exp. 45 / Plan 207 keep their meanings); no gift-value or favour economy; no
  resolution of the shorthand destination; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Market's Third Stall (TS)

- **Goal:** A **Stall Row** set (≤ 3 rows of stalls; the third authored as the ordinary one: its
  three goods (`cord`, `candle`, `jar`), plain prices through the existing price authority,
  `keeper_role` (never a name — DEC-TS-02), `sold_jar: never` flag) and a derived **Stall Read**
  (today's goods, prices, this market's plain tally); one optional hook: a purchase resolves
  through the *existing* barter/inventory verb unchanged and writes one tally line. No stall
  mechanics exist (DEC-TS-01).
- **Outcome (observable):** on a fixed seed, each market day renders the stall's goods and
  prices in the price authority's own values; a purchase moves stock through the existing verb
  and writes one tally line; the jar renders its `sold_jar: never` flag flatly beside its price;
  with no stall rows every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no pricing authority (price owners keep their meanings); no premium, credit or
  heat mechanics (The Underworld keeps its meanings); no trader NPCs or dialogue; no restock
  mechanics (the wares never change — DEC-TS-03); no new save section; no new routed panel; no
  Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a call may render one stall tally line
  ("called at the wet road; cord bought at the third stall") as one read-only string; and a stall
  tally may cite a visit's season. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**Showing up is the whole art.**

The visit book's rows are social fact: who went (as roles), what was carried, how long was
stayed. Prose must never score the visits — no favour counters, no warmth effects — because the
moment a call is worth points it stops being a call.

**The ninety percent of trade.**

The stall's plainness is the design. Cord, candle, jar. Prices from the real price authority,
unchanged. The prose should treat the stall the way the Cabinet treats its shelf: materially,
without romance — and let the jar's empty `sold` column do its own quiet work.

**What the player is never told.**

- Where the standing call is paid. `shorthand` is a destination the book keeps in its own hand
  (§12); the row renders flatly and the plan declines to expand it.
- Who the third stall's keeper is. A role, never a name (DEC-TS-02); the keeper is a counter
  habit and the record is fine with that.
- What is in the jar. Priced, full, never sold (§12); its contents are not in the vocabulary and
  must never be added.
- Whether the visits are repaid. The book tracks owed and paid; whether the far books balance is
  not asserted and the two ledgers never meet (§7).

**Voice — sample fragments (content candidates for `visit_lines.json` / `stall_lines.json`).**

> "Called at the wet road. Brought: cloth and two candles. Stayed: the afternoon, which is the
> correct length for a call and everybody knows it." — visit book (VP)

> "Standing call, paid in the autumn like always. The destination is written in the book's own
> shorthand. The book's shorthand is the book's business." — visit book (VP)

> "Third stall: cord, candles, and the jar. The jar has been priced since the first market day
> and the price has never been the reason nobody buys it." — stall read (TS)

> "Market day 41. Cord sold. Candle sold. Jar: the column says 'never,' in the same hand as
> everything else." — stall read (TS)

**Design texture beats.**

- **No favour mechanics, ever** (DEC-VP-01): a visit is a row and a row is all it is.
- **The stall has no mechanics** (DEC-TS-01): purchases resolve through existing verbs; the
  stall is a *place of commerce*, not a system.
- **Roles only, never names** (DEC-VP-02, DEC-TS-02): the corpus's no-names discipline holds at
  the counter and the doorway.
- **The flat-render family grows again**: `shorthand` and `sold_jar: never` join the plain-type
  ranks, rendered exactly like their neighbours.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the call leaves lying around.**

> "Visit book, spring: four calls paid, two owed. The book's arithmetic is social and balances
> like social arithmetic does." — texture only

> "What was brought, listed as kinds and not as values. Cloth is cloth. The book refuses to
> weigh the cloth's meaning."

> "The walked-in boots by the door, evening of a call day. Two pairs, mud on the wrong sides,
> which is what walking together looks like to a boot." — texture only

**What the stall leaves lying around.**

> "Price list, careful hand, never revised. Two prices have been crossed out in the same years as
> the Fair's tallies and the crossings-out agree with the stone." — texture only

> "The jar's shelf, dusted daily. A jar that is never sold requires more dusting than one that is,
> and the keeper's discipline is the market's quietest miracle." — texture only

> "Change tin: exact weights, exact change. The tin's tidiness is the stall's entire marketing."

**Held silences (texture, not register rows).**

- Whether the shorthand destination is a place at all. The book keeps its own hand (§12); the
  row is paid seasonally like the rest and the plan declines to expand it. Texture only.
- What the jar would cost if someone bought it. The price is set and the column says `never`
  (§1c); the two facts have never been tested together and must not be.

---

## 1.4 Worked examples (non-normative)

**A season of calls (fixed seed).**

> Spring: `Call(wet_road)` — bearer-roles `envoy-2`, brought `cloth ×1, candle ×2` (goods move
> through the inventory owner), stayed 1 day. Summer: the standing call — `shorthand` destination,
> `seasonal`, brought `cord`, rendered flatly like every row. The visit read renders the season:
> paid 5, owed 2, and one envelope carried to the smoke (one address string, no contents). No
> reputation state changes anywhere in the diff.

**A market day (fixed seed).**

> The stall read renders: cord (price from the authority), candle (ditto), jar — price shown,
> `sold_jar: never` beside it in the same plain type. A purchase of cord resolves through the
> existing barter verb and writes one tally line. The crossings-out on the price list agree with
> the Fair's tally stone and neither surface cites the other.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The Guest Book's stay/borrow/departure rows are the door-side ledger (batch-3 GB-P1). | `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E2 | Letter envelope discipline: address and weight, no contents (batch-4 LT DEC-LT-01). | `.ai/plans/story-expansion-batch-4/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E3 | Barter/inventory transactions resolve through existing verbs; price authorities exist (Underworld E1–E2; Fair premiums). | `.ai/plans/the-underworld-2026-09-29.md` E1–E2; batch-3 FA plan | LIVE / PROPOSED |
| E4 | Expeditions own travel; caravan/guest carriage hooks precedented (batch-3/4 seams). | `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` §4 | PROPOSED (plans are DRAFT) |
| E5 | Exp. 45 owns diplomacy/protocol; Plan 207 owns reputation. | `docs/expansions/wave7/expansion_45_the_envoy_plan.md`; `docs/plans/PLAN_207_SHELTER_REPUTATION_INTEGRATION_LOG.md` | LIVE |
| E6 | The Fair's tally stone derives public prices and stores nothing (batch-3 FA DEC-FA-04). | `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E7 | Whether `shorthand` destination rows can exist without colliding with settlement id namespaces. | additive-precedent discipline (batch-6 `off_edge`) | **VERIFY (P0)** |
| E8 | Board/market render points for visit and stall reads. | batch-3–6 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Travel, movement | expeditions / caravans | one optional carriage boolean; nothing else |
| Items, stock | inventory owner | `Call` goods move as its own transactions |
| Prices | price authorities | nothing; the stall renders their values |
| Premiums, credit, heat | The Underworld | nothing |
| Diplomacy, reputation | Exp. 45 / Plan 207 | nothing; visits move no standing |
| Door-side hospitality | Guest Book (batch 3) | nothing; the two ledgers never merge |
| Visit/stall folk state | — | `VisitRows` + `StallRows` (pure Core; row facts only) — **DEC-VP-03 / DEC-TS-03** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Social/VisitRows.cs` (new, pure), `Economy/StallRows.cs` (new, pure)
**Data:** `visit_rows.json`, `stall_rows.json`, `visit_lines.json`, `stall_lines.json`
**Host:** inventory host session (`INT`), market/board surface (`INT`), carriage hooks (`INT`)
**Presentation:** a "Calls" band on the existing social surface; a "Third Stall" line on the existing market surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Social/VisitRowsTests.cs`, `Economy/StallRowsTests.cs`, `Ashfall.Core.Tests/Save/VisitStallSaveTests.cs`

## 5. Packages

### VP-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm `shorthand` rows cannot collide with settlement ids; confirm the visit verb touches no reputation surface.
### VP-P1 — Visit rows + read (Core + data): closed `brought` kinds, bearer-roles, seasonal flags, validator row-level. **Accept:** no favour/reputation field exists anywhere (§6.3); determinism; round-trip.
### VP-P2 — Call verb + calls band (host): goods through inventory; carriage hooks dark. **Accept:** conservation; the shorthand row renders flatly.
### TS-P1 — Stall rows + read (Core + data): ≤ 3 rows, three goods, plain prices, `sold_jar: never`, validator row-level. **Accept:** no stall mechanic exists anywhere (§6.4); prices come from the authority unchanged.
### TS-P2 — Tally line + market line (host, dark): purchases resolve through existing verbs. **Accept:** one tally line per purchase; no restock path exists.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no visit rows and no stall rows → expeditions, barter, prices, reputation and board outputs identical on a saved corpus.
3. No-favour invariant: the diff contains no favour, warmth, reputation or gift-value field of any kind (DEC-VP-01).
4. No-stall-mechanic invariant: prices are the authority's own values; no premium, restock or trader field exists (DEC-TS-01/03).
5. Flat-render invariant: `shorthand` and `sold_jar: never` render in the same plain type as every other row (DEC-VP-04, DEC-TS-04).
6. Attribution invariant: bearers and keepers are roles, never names (DEC-VP-02, DEC-TS-02).
7. Conservation: call goods and stall purchases balance exactly through the inventory owner.
8. The Guest Book's register, The Underworld's premiums and Exp. 45's protocol stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Guest Book (batch 3):** the door-side ledger is its; the visit book is the caller's. Two books, never merged (§0).
- **The Underworld / Fair (batch 3):** premiums, syndicates and truces are theirs; the third stall is deliberately the *other ninety percent* of trade.
- **Exp. 45 / Plan 207:** visits are courtesy, not diplomacy; the diff moves no standing.
- **The Letter-Writing (batch 4):** one envelope per call at most; envelope discipline inherited whole.
- **Living Region:** settlements are its ids; the `shorthand` destination is the book's own hand and touches no namespace.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-VP-01 | No favour, reputation or gift-value mechanics of any kind. | rule | Yes |
| DEC-VP-02 | Bearers are roles, never names. | tone | Yes |
| DEC-VP-03 | Visit rows are social facts in an additive table; no new save section. | architecture | Yes; confirm in P0 |
| DEC-VP-04 | The `shorthand` destination renders flatly and is never expanded. | tone | Yes — **needs canon note** |
| DEC-TS-01 | The stall has no mechanics: purchases resolve through existing verbs. | rule | Yes |
| DEC-TS-02 | The keeper is a role, never a name. | tone | Yes |
| DEC-TS-03 | The wares never change; no restock path exists. | tone/rule | Yes — **needs canon note** |
| DEC-TS-04 | `sold_jar: never` renders flatly beside its price. | tone | Yes |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `VisitBook`, `CallLedger`, `ThirdStall`, `MarketStall`, `StallRow`)
- [ ] Premise re-verified (Rule 7); the Guest Book and Underworld registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-VP-01, DEC-TS-01)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing barter, inventory and reputation tests (list from P0 selector)
- [ ] Barter host selftest with stall rows on and off asserting zero premium changes (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a visit would need a favour or reputation effect to render; the stall would need any mechanic to justify its rows; the shorthand destination would collide with a settlement namespace; a purchase would bypass the existing verbs; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the call older than its courtesy and the stall plainer than its price. Any
future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| VP-OM-1 | Where is the standing call paid? | `shorthand` is the book's own hand (DEC-VP-04); the row renders flatly and is never expanded. | Never — deliberately sealed. |
| VP-OM-2 | Are the visits repaid? | The book tracks owed and paid; whether the far books balance is unasserted and the ledgers never meet (§7). | Never — a rule, not a gap. |
| VP-OM-3 | Who decided the afternoon is the correct length for a call? | Everyone knows it (§1b); the convention has no author and inherits EV's custom-provenance silence. | EV plan's owner (its register governs). |
| VP-OM-4 | Why do the walked-in boots have mud on the wrong sides? | Texture (§1c); what walking together looks like to a boot is the prose's joke and the record's silence. | Never — texture by omission. |
| TS-OM-1 | What is in the jar? | Priced, full, never sold (§12); contents are not in the vocabulary and must never be added. | Never — deliberately sealed. |
| TS-OM-2 | Who is the keeper? | A role, never a name (DEC-TS-02); the keeper is a counter habit. | Never — a rule, not a gap. |
| TS-OM-3 | What would the jar cost if someone bought it? | The price is set and the column says `never` (§1c); the two facts have never been tested together. | Never — tone-locked. |
| TS-OM-4 | Do the price list's crossings-out and the tally stone ever disagree? | They have always agreed (§1c); neither cites the other and the plan declines to make it a mechanism. | Never — texture by omission. |
| TS-OM-5 | Why do the wares never change? | DEC-TS-03 is a tone rule; whether the stall cannot or will not restock is not modelled and must not be. | Never — the pair with VP-OM-1 stays unresolved (mystery-index §3 discipline). |
