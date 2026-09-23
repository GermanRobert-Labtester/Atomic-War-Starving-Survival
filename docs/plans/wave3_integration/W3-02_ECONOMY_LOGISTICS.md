# ASHFALL — WAVE 3 INTEGRATION PROGRAM · PLAN 2 OF 6

# ECONOMY, TRADE & LOGISTICS INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W3 (six-plan integration wave)
**Document:** W3-02 · part A of B
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W3-01 (narrative), W3-03 (psychology), W3-04 (combat), W3-05 (crafting), W3-06 (UI)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

The economy of ASHFALL is not one system; it is a federation of owners: the
market, regional prices, supply routing, caravans, the black market, embargoes,
rationing, credit, barter, mercenaries, and tuning catalogs. This plan makes
that federation **truthful and legible**: one price authority, supply that
moves, scarcity that responds, and a trade screen that never lies.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Audit & Align | verify owners, catalogs, and consumption; fix truth gaps |
| **B** | One Market | unify price/supply routing, harden shocks, make trade legible |
| **C** | Living Economy | regional cycles, contracts, and long-arc economic change |

**Level 2:** ten points, each A/B/C (§4.2).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Audit & Align | 1–10 | — | — |
| B One Market | 1,4,9 | 2,3,5,6,7,8 | 10 |
| C Living Economy | — | 2,7 | 1,3,4,5,6,8,9,10 |

### 0.3 The Wave 3 rule for this plan

> **One price, one supply, one ledger boundary.** All prices resolve through the
> market/atlas owners; all goods movement through the supply router/caravan
> owners; and this plan does **not** create a FundsLedger — that belongs to
> UNBLOCK-02's F13 signature. Trade is read/write through existing owners only.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| base price | commodity baseline value |
| regional modifier | price/supply adjustment by region |
| arbitrage | buying low in one region, selling high in another |
| shock | weather/war/rumor price or supply event |
| embargo | authored trade restriction |
| stance | faction disposition affecting prices/access |
| credit | trade credit (not a currency ledger) |
| ration | allocation policy under scarcity |

---

## 1. Executive summary

Verified owners and data:

- **Market:** `MarketSystem`, `CommodityBaselineCatalog`, `HardcoreEconomyTuning`
  (+DTO/loader), `HardcoreEconomyEnums`.
- **Regions:** `RegionalPriceAtlas`, `RegionalSupplyRouter`,
  `regional_prices.json`.
- **Caravans:** `CaravanTradeNetworkSystem`, `CaravanTradeRouteCatalog`,
  `CaravanCatalogLoader`, `CaravanAtomicTrader`, `caravans.json`,
  `merchant_caravans.json`, `caravan_trade_routes.json`.
- **Black market:** `BlackMarketSystem`, `BlackMarketSettlementService`,
  `BlackMarketInventoryCatalog`, `black_market_inventory.json`.
- **Trade mechanics:** `ShelterBarterSystem`, `TradeCreditCoordinator`,
  `TradeEmbargoSystem`, `TradeSpecialtySystem`, `TradeStance`, `TradeTellEngine`,
  `TradeTextCatalog`, `TradeScreenPresenter`, `TradeScreenSeam`,
  `trade_specialties.json`, `trade_tell_lines.json`, `trade_texts.json`,
  `trade_embargoes.json`.
- **Scarcity:** `ResourceRationingSystem`, `RationConflictSystem`,
  `DesperationSystem`, `desperation_events.json`.
- **Factions:** `FactionStanceEngine`, `EconomyMarketRumorRules`,
  `EconomyWeatherShockRules`.
- **Mercenaries:** `MercenarySystem`.
- **Goods:** `economy_goods.json`, `GoodsCatalog`, `BiologicalTradeItem`.

The gaps:

1. **Price authority** — base prices, regional modifiers, stance, and shocks
   all adjust price; the composition order and single-writer discipline are
   unverified (Point 1).
2. **Caravan truth** — network, routes, and trader systems exist; caravan
   presence/availability and its effect on prices/supply are unaudited
   (Point 2).
3. **Shock rules** — weather/rumor shock rules exist; their consumption and
   balance are unverified (Point 3).
4. **Barter/credit** — systems exist; their boundaries with the market (no
   currency ledger) need a hard contract (Point 4).
5. **Black market** — three components + inventory; legality/heat/prices need
   one authority story (Point 5).
6. **Embargo/stance** — embargo data and stance engine exist; their effect on
   availability is unverified (Point 6).
7. **Rationing** — ration/conflict/desperation chain exists; the player-facing
   policy and consequences need one story (Point 7).
8. **Mercenaries** — service economy exists; pricing/contract truth is
   unaudited (Point 8).
9. **Trade UI truth** — tell engine and presenter exist; "why this price"
   legibility is the gap (Point 9).
10. **Logistics** — goods movement between regions/shelter is the biggest
    conceptual gap: is there a real flow, or just modifiers? (Point 10).

---

## 2. Verified current state

### 2.1 Owners

| Concern | Owner |
|---|---|
| base price | `MarketSystem` + `CommodityBaselineCatalog` |
| regional price | `RegionalPriceAtlas` + `regional_prices.json` |
| regional supply | `RegionalSupplyRouter` |
| tuning | `HardcoreEconomyTuning` (+ loader/DTO) |
| caravans | `CaravanTradeNetworkSystem` + catalogs |
| black market | `BlackMarketSystem` + settlement + inventory catalog |
| embargo | `TradeEmbargoSystem` + `trade_embargoes.json` |
| stance | `FactionStanceEngine` |
| rationing | `ResourceRationingSystem` + `RationConflictSystem` + `DesperationSystem` |
| credit | `TradeCreditCoordinator` |
| barter | `ShelterBarterSystem` |
| specialties | `TradeSpecialtySystem` + `trade_specialties.json` |
| tells/text | `TradeTellEngine`, `TradeTextCatalog`, `trade_tell_lines.json`, `trade_texts.json` |
| mercenaries | `MercenarySystem` |
| shocks | `EconomyWeatherShockRules`, `EconomyMarketRumorRules` |

### 2.2 The F13 boundary (from UNBLOCK-02)

UNBLOCK-02 proposes a FundsLedger (F13) that does **not exist yet**. This plan
therefore treats credit/barter/coin as **existing-owner mechanics only** and
never creates a ledger. If F13 is signed later, this plan's contracts (Point 4)
become the consumers of it.

### 2.3 Wave 2 interfaces

- W2-03 Point 4 proposes economy **pressure tiers** as a read model; W3-02
  owns the economic truth those tiers read.
- W2-05 attaches regions to places; W3-02 supplies the regional economic
  values.
- W2-01/W2-05 fix `locations.json`; no conflict.

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Price composition truth and single-writer discipline.
- Caravan/supply flow truth and consumption.
- Shock-rule consumption and bounds.
- Barter/credit boundaries (no ledger creation).
- Black-market authority and heat/pricing story.
- Embargo/stance effect verification.
- Rationing policy legibility and consequences.
- Mercenary/service pricing truth.
- Trade-screen legibility (price decomposition).
- Logistics flow truth (what actually moves).

### 3.2 Non-goals

- FundsLedger (UNBLOCK-02 F13 signature).
- Route/trade DTO contracts (UNBLOCK-02 F13-E/F).
- Balancing prices (W2-03 tunes bands; this plan makes truth).
- New currency items or wallets.
- Prose (W2-06).
- Save schema without signature.

### 3.3 Rules

1. One composed price path; no second price calculation in UI.
2. Supply is state, not a hidden modifier; if supply exists, it is owned.
3. Shocks are authored rules applied by owners; UI reads results.
4. Ration policy is owner state with player-facing consequences.
5. No money item without a signed decision.
6. Determinism: price rolls/shocks use seeded RNG (`EconomyMarketRumorRules`
   must already; verify).

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Price composition truth | B |
| 2 | Caravan and supply-flow truth | B |
| 3 | Shock rules consumption and bounds | B |
| 4 | Barter/credit boundary contract | A |
| 5 | Black-market authority and heat | B |
| 6 | Embargo and stance effects | B |
| 7 | Rationing policy and consequences | B |
| 8 | Mercenary/service pricing truth | B |
| 9 | Trade-screen price legibility | B |
| 10 | Logistics flow model | C |

### 4.2 Selection sheet

```text
PLAN W3-02 — ECONOMY & LOGISTICS
Plan Path: [ ] A Audit & Align  [ ] B One Market (default)  [ ] C Living Economy

01 price composition ...... [A] [B] [C]   default B
02 caravans/supply ........ [A] [B] [C]   default B
03 shock rules ............ [A] [B] [C]   default B
04 barter/credit .......... [A] [B] [C]   default A
05 black market ........... [A] [B] [C]   default B
06 embargo/stance ......... [A] [B] [C]   default B
07 rationing .............. [A] [B] [C]   default B
08 mercenaries ............ [A] [B] [C]   default B
09 trade UI truth ......... [A] [B] [C]   default B
10 logistics flow ......... [A] [B] [C]   default C
```

---

## 5. Decision Point 1 — Price composition truth (default B)

### 5.1 The design question

Price = base × regional × stance × shock × specialty? The exact chain must be
owned, ordered, and testable.

### 5.2 Path A — Composition audit

- Trace one commodity end to end: catalog → market → atlas → stance → screen.
- List every multiplier and where it is applied; find double application.

### 5.3 Path B — One composed path

- Define the composition explicitly (a function in the market owner or a small
  helper it owns): `base × region × stance × shock`, each factor sourced from
  its owner.
- The trade UI reads the composed value and its factors; it never multiplies.
- Tests: factor isolation (each owner's change moves price once), composition
  order stability, and a golden table for sampled goods.

### 5.4 Path C — Dynamic price discovery

Path B, plus price memory/trend (moving averages over campaign days) as a read
model for the UI (not a new authority).

### 5.5 Acceptance

- One path; no UI math; factor isolation tests.
- Golden prices for a sampled set at fixed campaign state.

---

## 6. Decision Point 2 — Caravan and supply-flow truth (default B)

### 6.1 The design question

Do caravans actually move goods that affect availability/prices, or are they
decorative events? `CaravanTradeNetworkSystem`, routes, and the atomic trader
suggest real mechanics; verification is required.

### 6.2 Path A — Flow audit

- Map: caravan schedule → arrival → goods offered → price/supply effect.
- Report steps with no effect.

### 6.3 Path B — Supply consumption

- Caravan arrivals mutate the supply router's regional state (the owner), and
  prices/availability read it; the trader's inventory reads the same state.
- Tests: an arrival raises availability (or lowers price) in its region;
  a missed caravan leaves supply lower.
- No second logistics layer: the router is the supply owner.

### 6.4 Path C — Caravan contracts

Path B, plus authored contracts (a caravan that trades only if a route is
safe) through existing narrative/economy owners; contract state rides the
caravan owner.

### 6.5 Acceptance

- Arrivals have a measured supply/price effect.
- No duplicate supply state.
- Missed arrivals observable.

---

## 7. Decision Point 3 — Shock rules consumption and bounds (default B)

### 7.1 The design question

`EconomyWeatherShockRules` and `EconomyMarketRumorRules` exist. Shocks should
be bounded, deterministic, and visible.

### 7.2 Path A — Rule audit

- For each shock rule: trigger, magnitude, duration, consumers.
- Report unbounded or unapplied rules.

### 7.3 Path B — Bounded shock contract

- Shock magnitudes clamped and duration-capped, authored in data; applied once
  per event with a dedupe key; deterministic rolls.
- The W2-03 pressure tier reads the shock state (read-only).
- Tests: shock applies once; expires; clamp respected; same seed → same shock.

### 7.4 Path C — Shock chains

Path B, plus authored chains (weather shock → rumor → panic) through existing
event owners; content via W2-06.

### 7.5 Acceptance

- Every rule bounded and applied once.
- Expiry tested; deterministic.
- No hidden permanent modifiers.

---

## 8. Decision Point 4 — Barter/credit boundary contract (default A)

### 8.1 The design question

`ShelterBarterSystem` and `TradeCreditCoordinator` exist. What is money? The
answer must be explicit: either an existing item/abstract credit or nothing,
and never a parallel ledger invented by accident.

### 8.2 Path A — Boundary document + tests

- Document the current model (what credit is backed by, what barter exchanges).
- Test that credit grants/consumes through its owner and cannot go negative
  without an authored rule.

### 8.3 Path B — One contract

- Formalize: credit is a bounded owner state (not inventory money); barter
  exchanges goods at composed prices; both route through their owners.
- Boundary test: no code path creates money items; grep + test.

### 8.4 Path C — F13 consumer readiness

If UNBLOCK-02's FundsLedger is signed, define the adapter contract this plan's
consumers use (credit reads the ledger). No implementation here.

### 8.5 Acceptance

- Boundary documented and tested.
- No money item exists.
- Negative test: credit overflow/negative refused or authored.

---

## 9. Decision Point 5 — Black-market authority and heat (default B)

### 9.1 The design question

`BlackMarketSystem`, `BlackMarketSettlementService`, and the inventory catalog
exist. Legality, heat, pricing, and settlement must have one story.

### 9.2 Path A — Authority audit

- Map: what makes an item illegal, how heat accumulates, who prices it, how
  settlement works.
- Report gaps (e.g., heat with no consequence).

### 9.3 Path B — One black-market authority

- The black-market system owns legality/heat/pricing; the settlement service is
  its host adapter; the inventory catalog is data.
- Heat feeds security consequences through W3-04 owners (read/write seam).
- Tests: illegal trade raises heat; heat has a consequence; prices include the
  black-market premium once.

### 9.4 Path C — Smuggling routes

Path B, plus smuggling route state through W2-05/W3-02 owners; content via
W2-06.

### 9.5 Acceptance

- Heat consequence exists.
- One premium application.
- No parallel black market.

---

## 10. Decision Point 6 — Embargo and stance effects (default B)

### 10.1 The design question

`TradeEmbargoSystem` + `trade_embargoes.json` + `FactionStanceEngine`. Do
embargoes actually restrict availability, and does stance actually move prices?

### 10.2 Path A — Effect audit

- For each embargo: which goods/routes it blocks and the consumer.
- For each stance level: the price/access effect and its application point.

### 10.3 Path B — Verified restrictions

- Embargoes gate availability through the market/router owners; stance applies
  once in the composed price (Point 1).
- Tests: embargoed good unavailable; lifting restores; stance levels move price
  monotonically.

### 10.4 Path C — Embargo politics

Path B, plus authored embargo arcs through faction/narrative owners (W3-01
flags).

### 10.5 Acceptance

- Every embargo has a real restriction.
- Stance monotone on price/access.

---

## 11. Decision Point 7 — Rationing policy and consequences (default B)

### 11.1 The design question

`ResourceRationingSystem`, `RationConflictSystem`, `DesperationSystem`. The
player should be able to choose a ration policy and see its consequences.

### 11.2 Path A — Policy audit

- Map policy options → per-survivor allocations → consequences (nutrition,
  morale, conflict).
- Report policies with no effect or consequences with no signal.

### 11.3 Path B — Policy consumption

- Policies write through the rationing owner; consequences route to needs
  (W2-03 owner) and conflict/desperation owners.
- The player sees the policy's expected effect (read model) and the actual
  effect (journal/needs).
- Tests: each policy changes allocation and a consequence; emergency policy
  has a cost.

### 11.4 Path C — Ration politics

Path B, plus authored conflict arcs (a policy that fractures the shelter)
through W3-03/W3-01 owners.

### 11.5 Acceptance

- Every policy has measured effects.
- Consequence signal exists.
- No duplicate scarcity logic (W2-03 tiers read).

---

## 12. Decision Point 8 — Mercenary/service pricing truth (default B)

### 12.1 The design question

`MercenarySystem` exists. Are contracts priced honestly and paid through an
owned path?

### 12.2 Path A — Pricing audit

- Map contract types → prices → payment/credit path → outcome.
- Report unpaid paths or free services.

### 12.3 Path B — Contract truth

- Contracts price through the composed path (goods/credit), paid via the credit
  owner; outcomes route through their owners.
- Tests: payment consumes value; failure has consequences; prices respond to
  stance.

### 12.4 Path C — Contract arcs

Path B, plus narrative contracts (W3-01) with reputation effects.

### 12.5 Acceptance

- No free service without an authored reason.
- Payment/credit consistent.

---

## 13. Decision Point 9 — Trade-screen price legibility (default B)

### 13.1 The design question

The player should see *why* a price is what it is: base, region, stance,
scarcity, black-market premium. The tell engine exists; legibility is the gap.

### 13.2 Path A — Legibility audit

- Compare the screen's shown numbers to the composed owner values.
- Report mismatches or unexplained differences.

### 13.3 Path B — Decomposition surface

- The screen shows the composed price with its top factors (from Point 1's
  composition read model), exactly as W2-03's forecast surface pattern.
- No UI math; no fabricated factors.
- Tests: displayed factors multiply to the displayed price (within authored
  rounding).

### 13.4 Path C — Trade advice

Path B, plus an advisor read (best nearby market for a good) composed from the
atlas (read-only).

### 13.5 Acceptance

- Displayed price equals the owner price.
- Factors explained; unknown shows unknown.

---

## 14. Decision Point 10 — Logistics flow model (default C)

### 14.1 The design question

The deepest gap: beyond regional modifiers, does anything **move**? A logistics
read model would answer "where are goods, how do they travel, what blocks
them" — the substrate EN-03/XP-08 assume.

### 14.2 Path A — Flow audit

- Inventory all goods-movement mechanics (caravan arrival, supply router,
  trade credit, rail/freight if live).
- Report what actually moves vs. what only modifies.

### 14.3 Path B — Flow read model

- A read-only `LogisticsModel`: per-region supply levels, inbound/outbound
  caravan schedules, route status (weather/war), and estimated days to
  restock — composed from owners.
- Consumed by trade UI, pressure tiers, and planning.

### 14.4 Path C — Real flow state

Path B, plus authored route capacity/delay state on the supply owner (additive,
signed) so blockades and weather genuinely delay goods. No FundsLedger.

### 14.5 Acceptance

- Flows verified as real or labelled modifiers.
- No duplicate movement authority.
- (C) delay state signed and tested.

---

## 15. Execution phases

### EC0 — Economy premise freeze (1 day)

- Verify owners/data; trace one commodity end to end; produce
  `P0_ECONOMY_PREMISE.md`.

### EC1 — Price composition (Point 1)

- Composition contract + factor isolation tests.

### EC2 — Supply and caravans (Point 2)

- Router consumption + arrival effects + tests.

### EC3 — Shocks and embargoes/stance (Points 3 + 6)

- Bounded shocks; verified restrictions.

### EC4 — Barter/credit and black market (Points 4 + 5)

- Boundary contract; heat consequences.

### EC5 — Rationing and services (Points 7 + 8)

- Policy consumption; contract truth.

### EC6 — Legibility (Point 9)

- Decomposition surface.

### EC7 — Logistics (Point 10, C)

- Flow model; signed delay state.

### EC8 — Closeout

- Evidence; ledger proposals; Annex U.

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | composition contract; factor isolation; golden prices |
| 2 | arrival effect; missed-caravan effect |
| 3 | shock once/expiry/clamp/determinism |
| 4 | boundary tests; no money item grep |
| 5 | heat consequence; single premium |
| 6 | embargo restriction; stance monotone |
| 7 | policy effects; consequence signal |
| 8 | payment consumes; stance affects price |
| 9 | displayed = owner price; factors multiply |
| 10 | flow audit; (C) delay tests |

Commands:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Economy
bash scripts/run_test.sh Ashfall.Core.Tests/Market   # if present
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --7day-smoke-selftest
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | composition refactor changes prices | M | H | golden tables; value-preserving by default |
| 2 | double application already exists | M | M | audit finds; fix once, test isolation |
| 3 | F13 lands mid-plan | M | H | adapter contract only; no ledger build |
| 4 | shocks become punishing | M | M | bounds + W2-03 bands |
| 5 | black market heat becomes a second security system | M | M | feed W3-04 owners; no new authority |
| 6 | rationing conflicts with W2-03 tiers | M | M | coordinate: tiers read, rationing writes policy |
| 7 | UI decomposition drifts | M | L | displayed-equals-owner test |
| 8 | logistics C adds persistence | M | H | signed line only |
| 9 | concurrent claims on market files | M | H | single-writer |
| 10 | scope creep to currency design | M | H | Wave 3 rule |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| EC0 | `W3-02-EC0-PREMISE` | premise doc + trace |
| EC1 | `W3-02-EC1-PRICE` | market owner + tests |
| EC2 | `W3-02-EC2-SUPPLY` | router + caravan binds |
| EC3 | `W3-02-EC3-SHOCKS-EMBARGO` | shock/embargo/stance binds |
| EC4 | `W3-02-EC4-CREDIT-BLACK` | barter/credit boundary; black market heat |
| EC5 | `W3-02-EC5-RATION-SERVICE` | ration policies; contract pricing |
| EC6 | `W3-02-EC6-LEGIBILITY` | trade screen decomposition |
| EC7 | `W3-02-EC7-LOGISTICS` | flow model (+ signed delay state) |
| EC8 | `W3-02-EC8-CLOSEOUT` | evidence + proposals |

Coordination: W2-03 owns pacing bands; W2-05 owns regional place maps; W3-01
owns flags/consequences for embargo arcs; W3-05 owns crafting supply inputs.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | keep audit | composition unverified |
| 2 | unbind arrivals | caravans may be decorative |
| 3 | unbind shocks | shocks unbounded |
| 4 | keep doc | money ambiguity remains |
| 5 | remove heat binds | black market unaccountable |
| 6 | keep audit | embargo effects unverified |
| 7 | remove policy binds | rationing opaque |
| 8 | keep audit | free services untracked |
| 9 | remove surface | price unexplained |
| 10 | keep audit | logistics unknown |

---

## 20. DoD and handoff

**Path A:** premise trace + all audits complete.

**Path B:** all of A, plus composition contract, supply/arrival truth, bounded
shocks, boundary contract, heat consequences, verified embargoes/stance, policy
consumption, contract pricing, and legibility — each tested.

**Path C:** all of B, plus logistics flow model and signed delay state.

**Handoff:** outcome, files, contract (composition/flows/boundaries), commands,
limitations, untouched shared paths, ledger proposals, Annex U.

### 20.1 First safe step

> EC0 only: the commodity trace and premise file. No price changes first.

---

# PART B — SCENARIOS, ANNEX U, AND APPENDICES

---

## 21. Scenarios

### 21.1 Path A week

EC0 trace (1 day); audits for points 1/3/5/6 (3 days); closeout (0.5). Outcome:
the economy's actual behaviour is documented.

### 21.2 Path B month

EC0 (1); EC1 composition + tests (4); EC2 supply (4); EC3 shocks/embargo (4);
EC4 credit/black (3); EC5 ration/service (4); EC6 legibility (3); closeout (1).
Outcome: one honest market the player can read and W2-03 can tune.

### 21.3 Path C season

Path B plus logistics flow/delay state as a signed package.

### 21.4 A price factor is applied twice

Factor isolation test fails; remove one application; golden table proves the
resulting price. Never halve a constant to compensate.

### 21.5 A caravan's goods do not come from anywhere

If arrivals create goods without a source, the flow audit records it; Path B
ties arrivals to the supply owner. If supply is abstract by design, the doc
says so — but then the UI must not claim "goods arrived" as a physical fact.

---

## 22. Foreman Q&A

**Q1. Is this the FundsLedger plan?**
No. It deliberately avoids the ledger; it makes current owners honest and
prepares consumers for F13 if signed.

**Q2. Will prices change?**
Only where a double application is proven; golden tables guard.

**Q3. What is "credit" today?**
Whatever `TradeCreditCoordinator` models — the boundary document states it
explicitly after the audit.

**Q4. Does the black market become a security system?**
It feeds W3-04's consequences through a seam; it stays an economy owner.

**Q5. How does this help W2-03?**
Pressure tiers read verified supply/shock state instead of guessing.

**Q6. Smallest approval?**
EC0 trace.

**Q7. Largest?**
Path C logistics with signed delay state.

**Q8. What if an embargo is authored but never enforced?**
That is exactly a gap the audit lists; Path B enforces it.

---

## 23. Annex U — Plan-unblocking (separately)

### U.1 What W3-02 releases

| Blocked item | Mechanism | Gate |
|---|---|---|
| XP-04 economy legs | Verified price composition + service pricing give the XP economics its truth substrate | EC1/EC5 |
| XP-08 trade routes (consumer half) | Logistics/caravan truth without building route DTOs | EC2/EC7 |
| EN-03 underground economy pressure | Pressure reads verified supply/shock state | EC2/EC3 |
| C3 192/199 (boundaries) | No route/ledger work; boundaries protected | all |
| Expansions 17/25/26 economy halves | Verified supply/caravan/ration surfaces | EC2/EC5 |
| W2-03 pressure tiers | Owner-sourced inputs | EC2/EC3 |
| Expansion 20 (Quiet Hand) | Black-market heat seam for espionage consequences | EC4 |
| Trade UI (W3-06) | Decomposition read model | EC6 |

### U.2 Signatures needed

```text
[ ] I authorize EC0 premise + commodity trace.
[ ] I authorize EC1 price composition contract + factor isolation.
[ ] I authorize EC2 caravan/supply consumption binds.
[ ] I authorize EC3 bounded shocks + verified embargo/stance.
[ ] I authorize EC4 barter/credit boundary (no money items).
[ ] I authorize EC5 black-market heat consequences + ration/contract truth.
[ ] I authorize EC6 trade-screen decomposition.
[ ] I authorize EC7 logistics model (+ delay state: [ ] no [ ] signed).
```

### U.3 What W3-02 never touches for unblocking

- FundsLedger creation (UNBLOCK-02).
- Route DTOs/player routes (UNBLOCK-02 F13-E / C3 192).
- Balance bands (W2-03).
- Prose (W2-06).
- Security systems (W3-04).

### U.4 The market-release rule

An economy feature releases when its price path, supply source, and
consequence record exist. A modifier that changes a number with no flow behind
it releases pressure tiers, not trade.

---

## 24. Appendices

### 24.1 Selection sheet

```text
ASHFALL WAVE 3 · PLAN 2 (ECONOMY) · SELECTION
Date: ______  Foreman: ______  HEAD: ______
PLAN PATH: [ ] A Audit & Align  [ ] B One Market (default)  [ ] C Living Economy

01 price composition ...... [A] [B] [C]   default B
02 caravans/supply ........ [A] [B] [C]   default B
03 shock rules ............ [A] [B] [C]   default B
04 barter/credit .......... [A] [B] [C]   default A
05 black market ........... [A] [B] [C]   default B
06 embargo/stance ......... [A] [B] [C]   default B
07 rationing .............. [A] [B] [C]   default B
08 mercenaries ............ [A] [B] [C]   default B
09 trade UI truth ......... [A] [B] [C]   default B
10 logistics flow ......... [A] [B] [C]   default C
Signature: ________________
```

### 24.2 Glossary

| Term | Meaning |
|---|---|
| composition | base × region × stance × shock chain |
| factor isolation | each owner's input moves price once |
| supply state | region's available goods owned by the router |
| shock | bounded authored price/supply event |
| haze | (n/a — see heat) |
| heat | black-market attention state |
| policy | rationing choice with consequences |
| flow | goods movement, real or abstract (documented) |

**End of Part I.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

---

# PART II — DEEP DESIGN SPECIFICATIONS (CONTINUED → 180K)# W3-02 · PART II — DEEP DESIGN: POINTS 1–5

> Appended 2026-09-21. Part I (summary contract) + this expansion. Proposal
> only. Economy authority boundaries from Part I §0.3 remain binding.

---

## §II.1 Decision Point 1 — Price composition truth (default B)

### II.1.1 The composition law

Every tradable good's price is a composition of existing owners, never a
second table:

```text
price(g) = base(g)
         × regional(destination, g)      # RegionalPriceAtlas
         × stance(faction, player)       # faction stance (W3-04 bridge)
         × shock(g, day, world)          # bounded shocks (Point 3)
         × route(caravan path)           # RegionalSupplyRouter (Point 2)
         × policy(rationing, embargo)    # Points 6/7
```

**Central question:** is each factor applied exactly once, sourced from the
declared owner, and visible in the trade surface (Point 9)? The failure modes:

| # | Failure | Symptom |
|---|---|---|
| 1 | factor applied in UI math | surface number ≠ executed price |
| 2 | factor applied twice | double stance penalty |
| 3 | factor defined in two owners | divergent quotes across screens |
| 4 | factor not applied at all | regional scarcity invisible |
| 5 | factor unbounded | 40× prices during a shock |
| 6 | factor applied at wrong time (quote vs. purchase) | arbitrage on stale quote |

### II.1.2 The single-price API proposal

```text
MarketQuote QueryPrice(good, market, context) -> Quote
Quote:
  base, factors[] (each: source, value, applied_at), final
```

The quote carries **factor attribution** — the list of applied factors with
their sources. This is the honesty mechanism: the trade screen renders the
attribution; the audit compares each factor to its owner's value; a factor
with no source is a finding.

### II.1.3 Owner map (verify at P0)

| Factor | Expected owner | Evidence status |
|---|---|---|
| base | item/market catalog | wave-2 verified market owners exist |
| regional | `RegionalPriceAtlas` | verified exists |
| stance | faction stance system | W3-04 bridge |
| shock | economy shock state | verify owner (planned wave-1 F13 context) |
| route | `RegionalSupplyRouter` / `CaravanTradeNetworkSystem` | verified exist |
| policy | rationing/embargo state | Points 6/7 (this plan) |

P0 produces the actual owner map with file:line. Any factor whose owner is
"unknown" is an immediate finding (a hidden factor is worse than a wrong one).

### II.1.4 The arbitrage audit

Buy market A, sell market B, same day: with route costs, is profit bounded
below an authored ceiling? The audit samples all market pairs per good where
both exist, computes the round-trip, and reports pairs exceeding the ceiling.
Legitimate trade routes may exceed it (authored); the finding is the
unintentional spread (a factor not traveling with the good, e.g., stance
applied per-seller only on buy).

### II.1.5 The quote-vs-purchase equality test

For a good at a market: `QueryPrice(...).final` must equal the executed
transaction price in all contexts (buy, sell, barter evaluation, mercenary
pricing, rationing adjudication). The test runs the same query through every
consumer path and asserts equality — this catches UI math drift and consumer
divergence in one stroke.

### II.1.6 Bounds table

| Factor | Authored bound | Rationale |
|---|---|---|
| regional | [0.5×, 2.0×] | scarcity signal, not extortion |
| stance | [0.6×, 1.8×] | allies discount, hostiles gouge within reason |
| shock | [0.5×, 2.5×] | crisis priced but not broken |
| route | [1.0×, 1.5×] | distance cost |
| policy | [0.4×, 1.5×] | embargo/rationing floors |
| composite | [0.25×, 6.0×] authored ceiling | no cascade to infinity |

Bounds are proposals for the authoring table; W2-03 validates the composite
against its economy bands. The audit's job: no factor exceeds its bound,
composite within ceiling in the soak sample, and bound violations are logged
with the factor that caused them.

### II.1.7 Determinism and rounding

Prices are computed with culture-invariant numeric formatting (repo's checksum
discipline). Rounding rule: authored (e.g., round-half-even to the credit
unit) and applied once, at the composition end — not per factor (per-factor
rounding compounds drift across markets, a classic bug). The test: a good
priced in 20 markets, rounded consistently, quote equals purchase.

### II.1.8 Acceptance tests (Point 1)

| Test | Expectation |
|---|---|
| factor attribution present | every quote lists all applied factors + sources |
| owner equality | each factor's value equals its owner's current value |
| single application | factor appears once in the chain |
| quote = purchase | all consumer paths agree |
| bounds | per-factor and composite within authored bounds in soak |
| arbitrage | sample pairs below ceiling or authored |
| rounding | same good same price independent of market evaluation order |

### II.1.9 Deliverables

`docs/economy/PRICE_COMPOSITION.md` — factor table, owner map, bounds, rounding
rule, and the attribution format. Generated price-sample report across markets
for review.

### II.1.10 Cost model

Owner map (1 day), attribution audit (2 days), arbitrage + equality tests (2
days), bounds/rounding fixes (1 day).

---

## §II.2 Decision Point 2 — Caravan and supply flow (default B)

### II.2.1 The flow question

`CaravanTradeNetworkSystem`, `RegionalSupplyRouter`, `RegionalTreatySystem`,
and route gates exist. The audit: does supply actually *move* — do markets
restock through routes, do route gates (weather/war) actually block or delay,
do caravans consume time/resources, and is the player-visible supply state
truthful?

### II.2.2 Flow model (specification)

```text
Route:
  id, endpoints (markets/nodes), path nodes, travel_days,
  capacity (units/run), risk (raider/weather class),
  gates[] (route gates: weather, war, treaty)
CaravanRun:
  route, cargo (good, qty), departure_day, eta_day, status,
  events[] (delay, loss, arrival)
```

Runs are scheduled by the caravan owner; each run resolves:

```text
step 1: gate evaluation (all gates pass? else delay/return)
step 2: travel resolution (risk events via seeded RNG; losses)
step 3: arrival (cargo to destination market owner; restock applied)
```

### II.2.3 Single-flow authority

The failure mode to audit: **double restock** (region atlas says scarce, router
restocks, market also self-regenerates) or **zero restock** (everyone assumes
someone else does it). The audit builds the restock write-map:

```text
who writes market stock?
  market owner reset/restock:      path, cadence
  router arrival:                  path
  caravan arrival:                 path
  player trade:                    path
  scavenging/expedition returns:   path (different owner)
```

Every writer documented; overlaps tested for double-count; gaps tested for
never-restocking markets.

### II.2.4 Gate semantics

Route gates exist in data (`weather_route_gates.json` per wave recon). Rules:

- a closed gate delays or reroutes (authored per gate), never silently ignores;
- gate evaluation is deterministic and logged (the run's status shows the
  gate, so the player can learn);
- gate state is read from its owner (weather/war), not copied.

Test: close a gate → run delays; open → run proceeds; the market effect is
visible (scarcity signal in the atlas).

### II.2.5 Cargo loss and risk

Risk events (raider interception, storm) use seeded RNG. Loss outcomes:
full, partial (authored fractions), delay-only. Every loss routes to an owner
(cargo owner = caravan system; destination scarcity effect = atlas). No silent
vaporization. Test: forced interception with seed → authored loss outcome;
market effect present.

### II.2.6 Player-facing supply truth

The trade surface shows supply level (abundant/normal/scarce/critical) sourced
from the atlas; the audit verifies displayed level matches atlas state and
that restock arrivals move the level within a tick. This is Point 9's
foundation; supply flip-flop (level oscillating per tick due to competing
writers) is a finding.

### II.2.7 Routes and treaties

`RegionalTreatySystem` can open/close routes. The audit: treaty changes
observable in route availability within an authored delay; no treaty change
without a route-state consequence; expired treaties restore prior state
(no permanent hidden closure).

### II.2.8 Acceptance tests (Point 2)

| Test | Expectation |
|---|---|
| restock write-map complete | all writers documented |
| no double restock | inventory math stable across cycles |
| no dead market | every market restocks through some path or is authored static |
| gate effects | close/open changes run outcomes |
| loss routing | forced events produce authored outcomes + market effects |
| supply truth | displayed level = atlas level |
| treaty effects | route availability changes observably |
| determinism | same seed ⇒ same run sequence |

### II.2.9 Cost model

Write-map (1-2 days), gate tests (1-2 days), loss routing (1 day), supply
truth tests (1 day), treaty audit (1 day).

---

## §II.3 Decision Point 3 — Bounded economic shocks (default B)

### II.3.1 What a shock is

An event-driven price/supply perturbation: war, quarantine, route closure,
foundry disaster, refugee influx. Shocks are the economy's memory of the
narrative. The failure modes:

1. permanent (never decays — the world never recovers);
2. invisible (price moves with no attribution — players can't learn);
3. cascading unbounded (shock × shock × stance);
4. contradictory (two shocks on the same good pulling opposite with no
   resolution rule);
5. unseeded (shock magnitude/timing random without determinism).

### II.3.2 Shock record specification

```text
Shock:
  id, cause_event (consequence ref), good_set (goods affected),
  kind (supply-cut | demand-spike | cost-floor),
  magnitude (bounded), onset_day, duration_days, decay_curve (authored),
  stacking_rule (max | additive-with-cap | replace),
  attribution (visible cause text ref)
```

### II.3.3 The lifecycle

```text
trigger -> onset (magnitude applied) -> sustain (duration) -> decay -> end
```

Every phase visible in the price attribution (the quote lists the shock with
its remaining days — players learn the economy). Decay curves authored from a
small set (linear, exponential, step) so the economy can be reasoned about.

### II.3.4 Stacking rule

At most one shock per (good, kind) is active; additional triggers either
extend duration (cap authored) or raise magnitude toward the cap. The audit
proves: N triggers do not produce N× magnitude; the composite bound (Point 1)
holds under the worst scripted pile-up.

### II.3.5 Determinism

Shock magnitude/timing use seeded RNG when authored variable. Test: same seed +
same event sequence ⇒ identical shock history.

### II.3.6 The contradiction rule

Two active shocks on the same good with opposite kinds (cut vs. spike): the
authored precedence (by cause class, e.g., war > weather) decides, or they
partially cancel with the net bounded. The audit lists all observed
contradiction pairs in the soak and checks each has a resolution rule.

### II.3.7 Shock attribution surface

The trade screen shows active shocks (name, remaining days, direction) — this
is the player's learning channel. The audit verifies attribution text refs
exist (corpus) and that removing a shock removes its row (no stale rows).

### II.3.8 Acceptance tests (Point 3)

| Test | Expectation |
|---|---|
| lifecycle | onset/sustain/decay/end all observable in quotes |
| bounded | magnitude ≤ cap; composite ≤ ceiling under pile-up |
| decay returns to baseline | post-decay price equals pre-shock (within rounding) |
| attribution complete | every active shock has a cause + text ref |
| stacking rule | N triggers ≤ authored max, proven |
| contradiction rule | each observed pair resolved; deterministic precedence |
| replay | identical shock history per seed |

### II.3.9 Cost model

Record spec + audit of existing shocks (2 days), stacking/pile-up tests (2
days), attribution checks (1 day), decay verification (1 day).

---

## §II.4 Decision Point 4 — Barter and credit boundary (default B)

### II.4.1 The boundary statement (critical)

**There is no `FundsLedger` in this plan — and none may be created.** Wave-1
recon verified `FundsLedger` does not exist anywhere. The credit boundary is:

| Instrument | Owner | Allowed |
|---|---|---|
| barter (item-for-item) | trade/market owner | yes |
| store credit (faction-scoped tab) | faction economy owner if it exists; else NOT introduced | verify |
| rationing coupons (internal) | rationing policy (Point 7) | yes, scoped |
| mercenary contracts (deferred payment) | mercenary system (Point 8) | yes, via contract record |
| universal currency | — | **explicitly out of scope (F13 is a separate signed decision)** |

The plan's job: audit existing barter/credit-like paths, ensure they are
**scoped and bounded**, and formally defer any currency work to the F13 lane.

### II.4.2 Barter valuation

Barter uses the same quote composition (Point 1) for both sides:

```text
fair_trade(a, b) := |value(a) - value(b)| <= tolerance(transaction)
```

The tolerance is authored (e.g., 15%); the trade surface shows both sides'
attributed values. The audit: no barter path accepts grossly unfair trades due
to a missing factor (e.g., stance applied to one side only).

### II.4.3 Credit-like patterns to audit

1. **Deferred payment to caravans** (pay on arrival?).
2. **Tab at a market** — does it exist? If yes: bounded (max debt), clearing
   event, consequence on default (standing, refusal to trade).
3. **Rationing coupons** — internal scrip; must not leak as universal currency
   (exchangeable with prices? bounded?).
4. **Mercenary promised pay** — contract record; default consequences.

For each found: either bounded-and-cleared (pass) or finding. Any pattern
functioning as universal currency without an owner is an F13 escalation — the
plan does not implement it.

### II.4.4 Default consequences

If a credit-like path defaults (player cannot pay):

```text
stage 1: reminder (visible)
stage 2: trade refusal (scoped to the counterparty)
stage 3: standing loss + contract penalty (via existing owners)
never: instant death/confiscation without authored warning
```

### II.4.5 Acceptance tests (Point 4)

| Test | Expectation |
|---|---|
| no universal currency | grep + runtime audit: no global funds state added |
| barter fairness | attributed values both sides; tolerance enforced |
| credit bounded | debt caps enforced; clearing works |
| default ladder | warning → refusal → standing, in order |
| coupon scoping | coupons must not trade at price for goods outside scope |
| mercenary contract | deferred pay recorded + default consequences |

### II.4.6 Cost model

Pattern census (2 days), barter fairness tests (1 day), default-ladder tests
(1 day), boundary documentation (0.5 day).

---

## §II.5 Decision Point 5 — Black-market heat (default B)

### II.5.1 The model

`BlackMarketSystem` + `BlackMarketSettlementService` exist. Heat is the
attention state of authorities to the player's illicit trade:

```text
Heat:
  sources: transaction volume, good class (weapons/meds), repeated partner,
           snitching events, faction pressure
  state: [0..cap] with decay
  thresholds: T1 (watch), T2 (shakedown), T3 (raid/warrant)
  consequences: authored per threshold, warned before effect
```

### II.5.2 One-authority check

Heat must be written by the black-market owner only. Any other system reading
it is fine; second writers are findings. The write-map audit (same procedure
as flags, §A.2.4 W3-01) applies.

### II.5.3 Threshold behavior

| Threshold | Consequence | Warning |
|---|---|---|
| T1 | prices up, some sellers wary | rumor/journal note |
| T2 | shakedown event (pay/bribe/refuse) | in-fiction warning at approach |
| T3 | raid or warrant (combat/legal path) | multiple warnings + a route out |

**No silent fatal:** T3 never fires without at least one warning and one
authored avoidance path (lay low, bribe, leave region). This mirrors the
crisis-foresight rule from W2-03/W3-03.

### II.5.4 Decay and bounds

Decay: authored half-life per heat class (transaction heat decays faster than
warrant heat). Cap: heat never exceeds the authored cap; the cap is reachable
only through deliberate behavior. Test: scripted sustained trade raises heat
to T1 then T2 within authored transaction counts; stopping trade decays to
baseline within authored days.

### II.5.5 Cross-plan handoff

- **W3-04:** T3 raid consequences route to combat/security owners via the
  espionage/consequence router (one path).
- **W3-03:** heat stress may feed anxiety/morale in authored amounts via
  psychology owners (proposal; W3-03 owns effects).
- **W3-01:** heat thresholds that produce narrative content (the shakedown
  scene) route through the choice chain and journal.

### II.5.6 Acceptance tests (Point 5)

| Test | Expectation |
|---|---|
| single writer | write-map shows black-market owner only |
| bounds | cap never exceeded |
| decay | return to baseline within authored days |
| warned consequences | T2/T3 have warnings + avoidance paths |
| consequence routing | raid routes through one consequence path |
| replay | deterministic heat accrual under identical transactions |

### II.5.7 Cost model

Write-map (1 day), threshold scenario tests (2 days), decay/bounds (1 day),
routing handoff documentation (1 day).

---

## §II.6 Points 1–5 execution notes

Order: Point 1 owner map first (it defines the shared factor vocabulary);
Point 2 next (flow informs shock sources); Point 3 uses 1/2; Point 4 is
independent but must finish before any credit-adjacent Point 7/8 work;
Point 5 depends on 4's default ladder (heat accrues partly from credit
stress).

```text
Day 1-2   P1 owner map + attribution format
Day 3-4   P1 arbitrage/equality tests + bounds
Day 5-7   P2 write-map + gate/loss tests
Day 8-10  P3 shock audit + stacking/pile-up
Day 11-12 P4 pattern census + fairness tests
Day 13-14 P5 write-map + threshold scenarios
Day 15    Consolidation + handoff notes to P6-P10
```

Shared artifacts: the factor attribution format (P1) is consumed by P3's shock
rows, P6's embargo rows, and P9's surface. The write-map method is shared
with P5 and (later) P7.

---

*End of Part II. Continues in Part III (Points 6–10).*# W3-02 · PART III — DEEP DESIGN: POINTS 6–10

---

## §III.1 Decision Point 6 — Embargo and stance effects (default B)

### III.1.1 The stance→trade contract

Faction stance (owned by W3-04's bridge) drives trade access:

| Stance band | Trade access | Price factor | Route access |
|---|---|---|---|
| allied | full | 0.6–0.9× | open |
| friendly | full | 0.8–1.0× | open |
| neutral | partial (no war goods) | 1.0× | open |
| cold | restricted (goods list) | 1.1–1.3× | delayed |
| hostile | banned (or black market only) | 1.5–1.8× (black market) | closed |

The **factor table is authored data**; the audit verifies each band's access
list is enforced at the market owner (not just displayed).

### III.1.2 Embargo mechanics

An embargo is a scoped trade ban:

```text
Embargo:
  source (faction, treaty, war stage), target faction/market,
  scope (goods classes), duration/condition, enforcement (refusal | seizure)
```

Rules:

1. **Visible:** embargoes appear in the trade surface with cause and scope.
2. **Scoped:** partial embargoes name classes; the audit checks no goods
   outside scope are blocked (over-broad enforcement is a finding).
3. **Routed:** embargo state comes from its owner (war/faction); no copies.
4. **Ending:** removal restores prior access; no permanent silent closure
   (the audit asserts state restoration after embargo end).

### III.1.3 Stance drift and trade consequences

Trade actions may move stance (selling weapons to a faction's enemy, buying
from a sanctioned partner). The audit: every such effect routes through the
stance owner with attribution, bounded per action, and rate-limited (no
standing oscillation from rapid trades). The "one big trade moves stance
massively" case is capped.

### III.1.4 Black-market interaction

Hostile bands redirect trade to black market (Point 5) at elevated prices.
Independence test: embargoed goods must not be purchasable at a normal market
through a UI loophole; the black market is a distinct route with its own
heat. The audit scripts: embargo → attempt normal purchase (fail) → attempt
black market (succeed with heat).

### III.1.5 Acceptance tests (Point 6)

| Test | Expectation |
|---|---|
| band enforcement | each band's access list enforced at purchase |
| price factors | quote factor matches band |
| scoped embargo | out-of-scope goods unaffected |
| state restoration | post-embargo access = pre-embargo |
| stance attribution | every trade-stance effect attributed + capped |
| black-market route | embargo loophole closed; heat accrues |

### III.1.6 Cost model

Table audit (1 day), enforcement scenarios (2 days), stance attribution tests
(1 day), black-market route test (1 day).

---

## §III.2 Decision Point 7 — Rationing policy (default B)

### III.2.1 The policy question

Rationing is the shelter's answer to scarcity: who gets what, at what cost,
with what enforcement. The audit: is there a rationing owner, are policies
authored, are effects visible, and is the policy legible to survivors (the
player sees consumption allocation)?

### III.2.2 Policy model

```text
RationPolicy:
  id, trigger (stock below threshold | decree | crisis),
  scope (who: all | essential workers | children | sick),
  ratios (per-need multipliers),
  enforcement (queue | lock | priority pass),
  duration/exit condition
```

Policies are chosen by the player through an existing shelter-policy owner
(verify at P0; if no owner exists, the B-path proposes the minimal one with a
signed line — a policy without an owner is a C-level addition).

### III.2.3 Effect truth

Rationing changes consumption through the needs owners (W3-03 for morale
effects; needs system for consumption rates):

```text
ration multiplier -> needs consumption modifier (Modify/SetExternalModifier)
```

Test: policy on → consumption drops per ratio; the need state reflects the
change; the audit catches UI-only rationing (displayed cut, unchanged rates).

### III.2.4 Consequences

- morale/trust effects authored (W3-03 consumes);
- non-compliance events (theft, hoarding) authored as encounters with
  consequences (W3-01/03 routes);
- exit: policy end restores consumption, with a recovery curve (no instant
  snap-back — authored).

### III.2.5 Legibility

The shelter surface shows: active policy, per-group ratios, stock outlook.
The audit: displayed ratios equal owner values; the stock outlook derives
from the same flow model as Point 10 (no separate estimate).

### III.2.6 Acceptance tests (Point 7)

| Test | Expectation |
|---|---|
| policy ownership | single owner; documented |
| effect truth | consumption matches ratio |
| morale routing | policy effects route to psychology owner |
| non-compliance | authored events fire under authored conditions, bounded |
| exit restoration | consumption returns; recovery curve authored |
| legibility | displayed = owner values |

### III.2.7 Cost model

Owner verification (1 day), effect tests (2 days), consequence routing (1-2
days), legibility (1 day).

---

## §III.3 Decision Point 8 — Mercenary and contract pricing (default B)

### III.3.1 The hiring economy

`MercenarySystem` exists. Hiring is a priced transaction with deferred
obligations. The audit:

1. price composition uses Point 1's quote API (mercenary cost = base ×
   reputation × risk × duration);
2. contract terms recorded (duration, duties, pay schedule, penalties);
3. default consequences authored (Point 4's ladder);
4. outcomes (loyalty, betrayal) driven by authored state, not silent rolls.

### III.3.2 Contract record

```text
MercenaryContract:
  id, party, duration_days, duties (guard | escort | raid | garrison),
  pay (upfront + per_day + completion), penalties (default | desertion),
  morale/trust input (from stance/relationship owners)
```

### III.3.3 Risk pricing

Risk = authored class of the duty + region danger + current war pressure
(read from owners). The audit verifies the risk inputs exist and are sourced,
and that two contracts with identical inputs price identically (determinism).

### III.3.4 Outcome truth

Mercenaries act: guards stand posts (defense contribution verified), escorts
travel (caravan integration), raids fight (combat outcomes via W3-04). Each
contribution routes through its owner; no parallel "mercenary power" stat
inflating outcomes outside owners.

### III.3.5 Acceptance tests (Point 8)

| Test | Expectation |
|---|---|
| priced via quote API | factors attributed |
| contract record persisted | survives save/load |
| default ladder | unpaid contract consequences in order |
| duty effects | each duty contributes through its owner |
| deterministic pricing | identical inputs ⇒ identical price |
| no parallel power stat | combat/defense use owner state only |

### III.3.6 Cost model

Record audit (1-2 days), pricing determinism (1 day), duty integration tests
(2-3 days, coordinate W3-04).

---

## §III.4 Decision Point 9 — Trade screen legibility (default B)

### III.4.1 The rule

The trade surface is a **projection** of Point 1's quote attribution, not a
calculator. Displayed price = executed price; displayed factors = applied
factors; displayed supply = atlas state; displayed shocks = active shock
records; displayed embargoes = active embargo rows.

### III.4.2 The legibility audit

For each market screen element:

| Element | Source | Failure mode |
|---|---|---|
| unit price | quote.final | UI math drift |
| factor breakdown | quote.factors[] | missing/extra factor |
| supply band | atlas level | stale or inferred |
| active shocks | shock records | stale rows |
| access state | stance band + embargo | hardcoded band |
| barter fairness | both quotes | one-sided valuation |

Script: open the screen in every market/state combination (sampled); compare
each element to its owner; any mismatch is a finding with the element,
expected, actual.

### III.4.3 The explanation surface

Given the factor attribution exists, the screen can answer "why so expensive?"
with the factor list. This is the player-learning feature; the audit verifies
the explanation lists exactly the applied factors (no editorializing, no
missing rows).

### III.4.4 Freshness

Quotes are computed per view with a timestamp; the purchase re-quotes at
execution. If the executed price differs from the displayed (state changed
between), the surface shows a re-quote notice (authored pattern) rather than
silently charging more. The audit scripts a state change between view and
purchase: expect the notice + the executed price being the fresh quote.

### III.4.5 Acceptance tests (Point 9)

| Test | Expectation |
|---|---|
| element-source equality | all elements match owners |
| factor explanation | exactly the applied factors |
| re-quote notice | appears when state changes between view and buy |
| no UI math | displayed numbers never computed outside quote API |
| staleness | no stale shock/embargo rows after state end |

### III.4.6 Cost model

Screen inventory (1 day), element tests (2-3 days), freshness script (1 day).

---

## §III.5 Decision Point 10 — Logistics flow model (default C, read model at B)

### III.5.1 The model

A read-only composition over owners describing the shelter's material outlook:

```text
FlowModel:
  stocks: current goods (inventory owner)
  flows_in: arrivals (caravan runs eta, scavenge returns, production outputs)
  flows_out: consumption (needs), production inputs, decay/spoilage,
             trade sales, rationing-adjusted
  horizon: N days (authored)
  projections: per good: net/day, days-to-critical, bottleneck flag
```

### III.5.2 Why it matters

It unifies: rationing outlook (Point 7), trade decisions (Point 1/2), and
crisis warning (W2-03 seam). One model, many surfaces — rather than three
estimates that disagree (a classic multi-screen contradiction).

### III.5.3 Projection rules

Simple, honest math (no hidden simulation):

```text
net(g) = in_avg(g) - out_avg(g)        # authored averaging window
days_to_critical(g) = stock(g) / max(0.0001, -net(g))   if net < 0
uncertainty: arrivals are ETA-ranged; the model reports a range, not a point
```

Tests: projection against actual consumption over the window (within authored
error, since arrivals are uncertain); critical flags fire before actual
stockouts in the soak (warning lead-time), or the lead is authored absent for
sudden losses (authored distaste).

### III.5.4 Single-model authority

All surfaces (rationing, trade, crisis) read this model; none computes its
own. The audit finds duplicate estimators (the "three outlooks" problem) and
routes them to the model.

### III.5.5 C-path state

The C option adds authored work-in-progress logistics state (shipments
committed, standing orders) through the caravan owner — signed. B path keeps
the model read-only and derived.

### III.5.6 Acceptance tests (Point 10)

| Test | Expectation |
|---|---|
| single model | no duplicate estimators remain |
| accuracy | projection within authored error of actuals |
| lead time | critical warnings precede stockouts (or authored absence) |
| range honesty | uncertainty reported, not hidden |
| surface consistency | all surfaces agree (same model) |
| (C) committed shipments | state persists + resolves |

### III.5.7 Cost model

Model build (3-4 days), accuracy harness (2 days), surface unification (2
days), (C) state (3-4 days signed).

---

## §III.6 Points 6–10 execution notes

```text
Day 1-2   P6 band table audit + enforcement scenarios
Day 3-5   P6 stance drift + black-market route; P7 owner verification (parallel)
Day 6-8   P7 effect truth + consequences
Day 9-10  P8 contract record + pricing determinism
Day 11-13 P8 duty integration (with W3-04 presence)
Day 14-16 P9 screen inventory + element equality
Day 17-19 P10 model build + accuracy harness (or defer to C)
Day 20    Consolidation + cross-plan handoff
```

Dependencies: P6 needs W3-04's stance bridge values; P8 duty tests need W3-04
combat; P9 needs P1/P2/P3 owners stable; P10 needs P2 flow data and P7 for
rationing-adjusted consumption.

---

## §III.7 Cross-point invariants (whole plan)

1. **One quote API** — every price everywhere.
2. **One flow model** — every outlook everywhere.
3. **Attribution always** — factors, shocks, embargoes, stance moves.
4. **Warned consequences** — default ladders and heat thresholds.
5. **No currency** — the F13 boundary holds until a separate decision.
6. **Owner truth** — no UI math, no copies, no second tables.
7. **Seeded determinism** — all stochastic economy paths.

---

*End of Part III. Continues in Part IV (authoring playbooks).*# W3-02 · PART IV — ECONOMY AUTHORING & OPERATIONS PLAYBOOKS

> How to author every economy artifact: price factors, shocks, routes, caravans,
> embargoes, ration policies, contracts, and surfaces. Each playbook is a
> procedure with templates, review sheets, and anti-patterns.

---

## §IV.1 The economic authoring lifecycle (shared)

```text
1. MODEL     -> the composition/flow this artifact participates in
                (which factor, which flow, which policy slot)
2. DATA      -> authored table entry with bounds
3. ROUTING   -> which owner applies/reads it; attribution format
4. SURFACE   -> how the player sees it (via the quote/flow projection)
5. VERIFY    -> equality tests + bounds + determinism
6. REVIEW    -> balance intent (W2-03 bands), tone (W2-06), ethics (warns)
7. SEAL      -> registered in the economy registry; gate green
```

Rule: **no economy number without a home.** Every authored value (factor,
bound, ratio, duration) belongs to exactly one table with one owner. Numbers
floating in code or duplicated across screens are findings.

### IV.1.1 The economy registry (proposal)

`docs/economy/ECONOMY_REGISTRY.md`, generated from data:

```text
Every authored economy value:
  value_id, kind (factor|bound|ratio|duration|price|capacity),
  owner, unit, current value, consumers, bounds-check
```

This mirrors the flag registry pattern (W3-01) and gives the same drift gates.
It is the backbone of every audit in this plan.

---

## §IV.2 Price factor authoring playbook

### IV.2.1 Factor template

```yaml
factor: regional_scarcity
applies_to: goods classes [food, medicine, fuel]
formula: 0.5 + 1.5 * scarcity_index   # authored example
clamped: [0.5, 2.0]
source: RegionalPriceAtlas.scarcity_index
applied_by: MarketSystem quote composition
attribution: "regional scarcity: {scarcity_index:.2f}"
```

### IV.2.2 Authoring rules

1. **One formula per factor.** An if/else chain over goods is a sign the
   factor should be two factors.
2. **Clamps are mandatory.** Unbounded factors cascade (Point 1 §II.1.6).
3. **Source is named.** A factor without a source owner is not shippable.
4. **Attribution string is authored** (corpus-reviewable), because the
   explanation surface shows it verbatim.
5. **Monotonicity:** a scarcity factor must not decrease as scarcity rises
   (sanity test): authored formula monotone in its source, verified by test.
6. **Interaction review:** when two factors both read scarcity (regional and
   stance reading war pressure), the review checks for double-counting.

### IV.2.3 Factor review sheet

```text
[ ] single formula; no branching over goods
[ ] clamps authored
[ ] source owner named and verified
[ ] attribution string authored (W2-06 registration)
[ ] monotone in source (test)
[ ] no double-count with sibling factors (review)
[ ] bounds vs. composite ceiling checked
```

### IV.2.4 Factor anti-patterns

| Anti-pattern | Symptom | Fix |
|---|---|---|
| UI-only factor | screen shows scarcity, price doesn't | route through quote API |
| per-goods branch chain | unmaintainable, inconsistent | split into factors or a table |
| hidden constant in code | changes invisible to authors | move to registry |
| unbounded | 40× prices | clamp |
| double-read of one state | 4× penalties | one factor per state dimension |
| factor applied at quote only | purchase uses base | execute through the same quote |

---

## §IV.3 Shock authoring playbook

### IV.3.1 Shock template

```yaml
shock: war_route_closure_north
cause: consequence ref (war stage event)
goods: [food, fuel]
kind: supply-cut
magnitude: 1.35         # multiplier applied to price (authored, clamped)
duration_days: 12
decay: linear_half      # authored curve class
stacking: extend-capped (max_total_days: 24)
attribution: "north road closed by the war: {days_left} days"
```

### IV.3.2 Authoring rules

1. **Cause is a consequence record.** Shocks do not trigger from wall-clock or
   unexplained timers; they cite an event (narrative's ledger).
2. **Goods set is specific.** "All goods" shocks are rare and reviewed (they
   read as the world ending, which is a tone decision).
3. **Magnitude bounded** by the point table; duration bounded; stacking rule
   explicit.
4. **Decay curve chosen from the authored set**; a new curve class is an
   engineering decision, not a per-shock improvisation.
5. **Attribution string authored**; it is the player's learning channel.
6. **Recovery is authored:** after decay, the economy returns to baseline; if
   the intent is a permanent scar, that is a **state change**, not a shock
   (route through the atlas or a policy).

### IV.3.3 Shock review sheet

```text
[ ] cause consequence exists and fires before onset
[ ] goods scope specific
[ ] magnitude/duration within bounds
[ ] stacking rule + max_total authored
[ ] attribution string registered
[ ] decay curve authored + return-to-baseline test
[ ] pile-up scenario tested (with sibling shocks on same goods)
[ ] determinism: seeded if variable
```

---

## §IV.4 Route and caravan authoring playbook

### IV.4.1 Route template

```yaml
route: north_road
endpoints: [holdfast, salt_market]
path: [holdfast, ridge_crossing, salt_market]
travel_days: 4
capacity: 30              # units per run
risk_class: moderate       # raider/weather class
gates: [weather_north, war_north_block]
cargo_bias: [food, salt, tools]
```

### IV.4.2 Authoring rules

1. **Path nodes exist in the location catalog** (the map plan's gate
   `AllMapNodes_ExistInLocationsCatalog` already enforces the family).
2. **Gates reference existing gate definitions** (weather/war tables); a route
   gate that never closes is a dead declaration (finding).
3. **Capacity × frequency ≤ market demand** or the route over-supplies
   (checked by the flow audit); authored values live in a table with bounds.
4. **Risk class maps to an authored event table** (loss fractions); no free
   formulas.
5. **Cargo bias is a preference, not a rule** — the router may ship other
   goods when demand says so; the field documents intent.
6. **Travel time consistent with the map distance** (authored tolerance); a
   4-day route across the map when the route estimate says 10 is a finding.

### IV.4.3 Caravan run review sheet

```text
[ ] endpoints + path nodes exist
[ ] gates exist and can actually close (verified by data)
[ ] capacity vs. demand balance checked
[ ] risk events authored with fractions
[ ] ETA range authored (uncertainty honest)
[ ] loss/arrival outcomes route to owners
[ ] supply effect visible in the atlas within authored ticks
```

### IV.4.4 Route anti-patterns

| Anti-pattern | Symptom | Fix |
|---|---|---|
| decorative gate | never closes; no effect | remove or wire to source |
| infinite capacity | market never scarce | capacity table + bounds |
| instant arrival | travel time ignored | enforce ETA in arrival handler |
| silent loss | cargo vanishes | route to owners |
| conflicting restock | market stock oscillates | write-map fix (single authority) |

---

## §IV.5 Stance/embargo authoring playbook

### IV.5.1 Band table authoring

```yaml
band: hostile
access: {goods: banned, black_market: allowed}
price_factor: 1.65
route_access: closed
effects: [bounty_risk up, guard scrutiny up]
```

Rules:

- bands are a total order (no gaps): every possible stance value falls in
  exactly one band;
- each band's access list is exhaustive over good classes (every class either
  allowed, restricted, or banned — no "unspecified" cells);
- factor values are within Point 1 bounds;
- route access maps to existing route-state values.

### IV.5.2 Embargo template

```yaml
embargo: salt_blockade
source: faction_salt (hostile transition)
target: holdfast_market
scope: [salt, preserved_food]
enforcement: refusal
end_condition: stance(neutral) OR day(120) authored fallback
attribution: "the salt blockade"
```

Rules:

1. **End condition is mandatory** (stance change, event, or authored day cap)
   — embargoes never run forever silently.
2. **Scope classed**; enforcement behavior authored (refusal vs. seizure) —
   seizure routes through consequences (loss events).
3. **Attribution authored**; the trade surface shows the blockade.
4. **Restoration test:** post-embargo access equals pre-embargo; the audit
   scripts the full cycle.

### IV.5.3 Stance-move authoring

Every trade action that moves stance:

```yaml
action: sell_weapons_to_enemy
stance_effect: -0.15 (bounded per action, clamped daily)
attribution: "the salt faction noted the sale"
cooldown: 1/day
```

Rules: per-action cap; daily cap; attribution (visible or invisible per
authored class — covert trades move stance invisibly by design, but the
ledger still records); no cliff effects (crossing a band is a threshold
event with warning).

---

## §IV.6 Ration policy authoring playbook

### IV.6.1 Policy template

```yaml
policy: strict_rationing
trigger: stock(food) < 0.3 * capacity OR decree
scope: all
ratios: {food: 0.6, water: 0.8, hygiene: 0.5}
enforcement: queue
duration: indefinite (exit: stock > 0.6 * capacity OR decree)
effects:
  morale: -moderate (psychology owner)
  compliance_events: [theft_minor, hoarding_search]
attribution: "strict rationing in effect"
```

### IV.6.2 Authoring rules

1. **Trigger and exit both authored** — no policy without a way out.
2. **Ratios within allowed bands** (no starvation-by-policy below the authored
   floor; the floor protects against a player accidentally killing the
   shelter).
3. **Effects routed to owners** (morale → psychology; compliance events →
   encounters).
4. **Compliance events bounded** in frequency; authored escalation only with
   review (tone).
5. **Recovery curve authored** for exit (consumption returns over N days).

### IV.6.3 Policy review sheet

```text
[ ] trigger + exit authored
[ ] ratios within floors
[ ] morale effect routed
[ ] compliance events bounded + reviewed
[ ] recovery curve
[ ] legibility: displayed ratios = owner ratios
[ ] no policy silently permanent
```

---

## §IV.7 Contract (mercenary) authoring playbook

### IV.7.1 Contract template

```yaml
duty: escort
duration_days: 6
risk: route risk class + war pressure (read from owners)
pay: {upfront: 40, per_day: 8, completion: 30}
penalties: {default: standing -0.2, desertion: contract void + standing -0.4}
trust_input: stance(mercenary_company) + relationship(contact)
outcomes: [loyal, grumbles, departs, betrays]   # authored probabilities (seeded)
```

### IV.7.2 Authoring rules

1. **Pricing via quote API** (the contract is a priced transaction).
2. **Outcome table authored** with probabilities; no hidden rolls (the outcome
   table is data; RNG selects).
3. **Every outcome has a consequence** (departure returns pay? betrayal costs
   trust? authored).
4. **Duties map to owners** (guard → defense, escort → caravan, raid →
   combat).
5. **Default ladder** follows Point 4; no instant hostile flip without
   warning.

### IV.7.3 Contract review sheet

```text
[ ] priced via quote API with attribution
[ ] outcome table authored; probabilities sum correctly
[ ] every outcome routed
[ ] duties mapped to owners
[ ] default ladder respected
[ ] determinism test authored
```

---

## §IV.8 Surface authoring playbook (trade/ration/flow)

### IV.8.1 The projection rule

Surfaces render owner state; the authoring task is choosing *what* to show,
never *computing* values:

```text
Trade screen blocks:
  quote (final) + factor rows (attribution)
  supply band (atlas)
  active shocks/embargoes (records)
  barter fairness (both quotes)
Ration screen blocks:
  active policy + ratios + exit condition
  flow outlook (model) with uncertainty range
Flow screen blocks:
  per-good net/time-to-critical ranges
```

### IV.8.2 Authoring rules

1. Every block names its source in the surface spec (a table row: block →
   source → owner).
2. The explanation block shows exactly the applied factors (no editorial).
3. The re-quote notice is authored copy (W2-06) and shown when the quote
   changes between view and action.
4. Numbers formatted per the repo's culture-invariant rules.
5. No block may show a value without a source (purity rule).

### IV.8.3 Surface review sheet

```text
[ ] every block has a source row
[ ] equality tests authored per block
[ ] explanation = applied factors
[ ] re-quote notice authored + triggered test
[ ] formatting culture-invariant
[ ] no orphan numbers
```

---

## §IV.9 The economy anti-pattern catalog

| # | Anti-pattern | Class | Fix |
|---|---|---|---|
| 1 | price in UI math | truth | route via quote API |
| 2 | factor without source | truth | name owner or delete |
| 3 | hidden constant | truth | registry |
| 4 | unbounded factor | safety | clamp |
| 5 | per-factor rounding | consistency | round once at end |
| 6 | double restock | flow | single writer |
| 7 | decorative gate | flow | wire or remove |
| 8 | permanent shock | recovery | decay or state change |
| 9 | unattributed shock | learning | attribution string |
| 10 | stacking explosion | safety | stacking rule + cap |
| 11 | universal currency by accident | boundary | scope it or escalate F13 |
| 12 | credit without clearing | safety | cap + clearing + ladder |
| 13 | heat unwarned | fairness | thresholds + warnings |
| 14 | embargo forever | recovery | end condition |
| 15 | over-broad embargo | precision | class scoping |
| 16 | policy without exit | safety | exit condition |
| 17 | starvation-floor violation | safety | floor clamp |
| 18 | stale surface rows | truth | record-driven rendering |
| 19 | duplicate estimators | consistency | one flow model |
| 20 | unseeded economy rolls | determinism | seeded facade |
| 21 | stance cliff | fairness | threshold with warning |
| 22 | trade-stance oscillation | fairness | daily caps |
| 23 | mercenary free outcome | substance | authored outcome table |
| 24 | quote/purchase divergence | truth | single API |
| 25 | regional factor double-read | consistency | one factor per dimension |
| 26 | capacity ignored | flow | capacity table enforced |
| 27 | silent cargo loss | routing | loss events routed |
| 28 | treaty without restoration | recovery | restore prior state |
| 29 | projection as point estimate | honesty | ranges |
| 30 | outlook duplication | consistency | single model |

---

## §IV.10 Registry and gate integration

### IV.10.1 Generated checks

```bash
# illustrative; generators are deliverables of this package
bash scripts/ci/generate-economy-registry.sh --check
bash scripts/ci/economy-bounds-check.sh        # factors/shocks/policies in bounds
bash scripts/ci/economy-attribution-check.sh   # every factor/shock/embargo attributed
```

### IV.10.2 Gate rules

1. bounds-check fails on any out-of-bounds value (data is validated at CI, not
   at runtime surprise);
2. attribution-check fails on missing strings/refs;
3. registry drift fails on unregistered values;
4. surface-source table completeness (every block has a source row) is checked
   statically against the surface spec.

### IV.10.3 Baseline discipline

Existing violations (if any) enter a counted baseline that may not grow —
same ratchet as W3-01. Repairs ranked; cap per phase.

---

## §IV.11 Authoring workflow (weekly cadence)

```text
Mon  pick artifact class; write model + data template
Tue  wire routing/attribution; local equality tests
Wed  surfaces (block spec; no math)
Thu  bounds/determinism/pile-up tests; review sheet
Fri  registry regeneration; gate run; findings triage
```

No economy value ships without: source owner + bounds + attribution + test.

---

## §IV.12 Ethical and tone notes for economy authors

1. **Scarcity is not punishment.** Price movements signal the world; they
   never read as the game extracting revenge.
2. **Famine is a story, not a slider.** Ration floors protect survivors; the
   policy's drama is in morale and events, not in preventable deaths from
   authoring error.
3. **The black market is human.** Heat is watchfulness, not moral judgment;
   the shakedown scenes are people under pressure (W2-06 voice).
4. **Mercenaries are people who chose a trade.** Outcome tables include
   loyalty and grumbling, not only betrayal.
5. **No real war references.** Economy flavor stays fictional and unnamed
   (repo tone rule).

These notes enter the review sheets as human criteria (last row: tone pass).

---

*End of Part IV. Continues in Part V (verification catalog and harness).*# W3-02 · PART V — VERIFICATION CATALOG AND HARNESS DESIGN

> Complete test design for the ten points: tiers, scenarios, harnesses, soak
> parameters, evidence, and failure triage. Proposal-only.

---

## §V.1 Verification tiers

```text
T1 STATIC (seconds)
  registry drift, bounds, attribution completeness, surface-source table
T2 FOCUSED RUNTIME (minutes)
  quote equality across consumers; factor application; shock lifecycle;
  route gate effects; barter fairness; credit ladder; heat thresholds
T3 SOAK (hours, shared)
  20 seeds × 60 days market simulation; arbitrage sweep; shock pile-up;
  flow accuracy; supply truth stability
```

Same discipline as W3-01: lower tier green before higher; evidence
HEAD-stamped.

---

## §V.2 T1 — static checks

### V.2.1 Check table

| Check | Input | Detects | Output |
|---|---|---|---|
| E1.1 registry drift | data vs. generated registry | unregistered values | value_id, file |
| E1.2 bounds | factor/shock/policy values | out-of-bounds | value, declared bound |
| E1.3 attribution | shocks/embargoes/factors | missing attribution | id, missing field |
| E1.4 source table | surface specs | blocks without sources | screen, block |
| E1.5 routing map | shock/embargo/gate refs | dangling cause refs | ref, owner |
| E1.6 band totality | stance bands | gaps/overlaps | stray value |
| E1.7 scope classes | embargo/policy scopes | unknown classes | class |
| E1.8 currency grep | code + data | universal-currency smells | file:line |

E1.8 is a guard: any new global funds-like state trips review (F13 boundary).

### V.2.2 Registry generation

```bash
bash scripts/ci/generate-economy-registry.sh --check
```

Registry rows: every factor, bound, shock, route, gate, policy, contract,
heat threshold. Consumers column lists the surfaces/owners reading it.

---

## §V.3 T2 — focused runtime scenarios

### V.3.1 Quote equality kit (Point 1)

```text
for each consumer path C in [trade buy, trade sell, barter eval, mercenary
pricing, ration adjudication, black market, caravan cargo valuation]:
  quote = QueryPrice(good, market, context=C)
  execute = perform the transaction via C (scripted)
  assert execute.price == quote.final
  assert quote.factors == expected attribution set for state
```

State grid: 4 markets × 3 stance bands × 2 shock states × 2 embargo states,
sampled deterministically (authored sample list). This kit is the single
highest-value test in the plan: it proves one price truth across every
consumer.

### V.3.2 Factor application kit (Point 1)

Per factor: toggle its source state, assert the price changes in the authored
direction and magnitude (± rounding); assert the factor appears exactly once;
assert monotonicity (scarcity up → price not down).

### V.3.3 Shock lifecycle kit (Point 3)

```text
trigger cause event
assert: onset (price moves, attribution row appears)
advance days through sustain; assert magnitude constant per curve
advance through decay; assert return to pre-shock price
assert: attribution row disappears exactly at end
pile-up: fire two shocks on same goods; assert stacking rule result ≤ cap
```

### V.3.4 Route gate kit (Point 2)

```text
open state: run caravans N days; assert arrivals + market restock
close gate: assert run delay/reroute per authored behavior; no arrival
open again: assert arrivals resume
supply affect: assert atlas level moves within authored ticks
```

### V.3.5 Barter fairness kit (Point 4)

```text
for each pair class (fair, slightly off, grossly off):
  assert acceptance per tolerance; assert both sides valued via quote API
  assert no path accepts a grossly off trade due to missing factor
```

### V.3.6 Credit/default ladder kit (Point 4)

```text
incur debt near cap; advance days unpaid:
  stage 1 reminder asserted (visible)
  stage 2 refusal asserted (scoped)
  stage 3 standing delta asserted (attributed)
assert order and no skipped stage
```

### V.3.7 Heat threshold kit (Point 5)

```text
scripted illicit trade volume:
  heat accrues; assert T1 hint, T2 shakedown with warning, T3 with warnings +
  avoidance path
stop trading: assert decay to baseline within authored days
assert cap: sustained trade never exceeds cap
```

### V.3.8 Embargo/band kit (Point 6)

```text
set stance band; assert access lists enforced (in-scope blocked, out-of-scope
free); assert price factor; assert restoration after band/embargo end
assert black-market route active under hostile, with heat accrual
```

### V.3.9 Ration kit (Point 7)

```text
activate policy: assert consumption ratios applied via needs owner; assert
morale effect routed; assert the policy can exit; assert recovery curve
assert legibility: displayed ratios equal owner values
```

### V.3.10 Contract kit (Point 8)

```text
hire under two risk states; assert pricing differs per inputs, identical
inputs identical price; run contract to completion; assert pay/standing;
default: assert ladder; assert duty contributions land in owners
```

### V.3.11 Surface kit (Point 9)

```text
open trade screen in sampled states; assert every block equals owner values;
assert factor explanation set equals applied factors; change state between
view and purchase; assert re-quote notice + fresh price executed
```

### V.3.12 Flow kit (Point 10)

```text
run 20 days; compare projected net vs. actual per good (authored error);
assert critical flags lead stockouts; assert all surfaces report identical
model values
```

---

## §V.4 T3 — soak design

### V.4.1 Market soak parameters

```text
20 seeds × 60 days
markets: all verified markets
policies per seed: trade-active, trade-passive (different consumption)
events: scripted event injection schedule (war stage, weather gates, shocks)
measurements per day:
  prices per good per market (min/max/median)
  supply levels; restock events; stock heteroskedasticity
  arbitration: best round-trip profit vs. ceiling
  shock history; decay compliance
  flow accuracy: projection error
  heat: n/a (player-driven; separate scenario)
```

### V.4.2 Soak assertions

| Assertion | Source |
|---|---|
| composite price within [0.25×, 6×] every market-day | Point 1 bounds |
| no double restock (stock math stable) | Point 2 write-map |
| every shock returns to baseline after duration | Point 3 |
| pile-up ≤ cap in weeks with injected crises | Point 3 |
| no arbitrage above ceiling in sample | Point 1 |
| projection error within tolerance | Point 10 |
| supply levels stable (no per-tick flip-flop) | Point 2/9 |
| determinism: same seed identical price series | all |

### V.4.3 Report format

```yaml
run: T3-2026-10-a
seeds: 20, days: 60
price_bounds: {violations: 0}
arbitrage: {pairs_over_ceiling: 1, authored: 1}
shock_decay: {violations: 0}
flow_error: {food: 0.08, water: 0.04, ...}  # absolute fraction
determinism: pass
artifact: docs/evidence/w3-02/T3-2026-10-a.md
```

---

## §V.5 Cross-check with W2-03 bands

The economy plan does not tune; it verifies **plausibility** and hands
measurement to W2-03:

| Metric | Handed to W2-03 |
|---|---|
| median food price curve | band check |
| crisis price peaks | spike bound check |
| recovery time | pacing comparison |
| player income vs. costs (sampled) | pressure curve |

Interface: a small shared report schema so the two waves compare like for
like. No tuning authority moves.

---

## §V.6 Evidence handling

```text
docs/evidence/w3-02/
  T1-*.yaml (registry/bounds/attribution)
  T2-*.yaml (per-kit runs)
  T3-*.yaml + *.csv (soak summaries)
  findings/  (per-finding files with owner + status)
  repairs/   (before/after)
  P0_ECONOMY_PREMISE.md
```

---

## §V.7 Failure triage protocol

```text
bounds violation        -> immediate fix (safety)
quote inequality        -> stop: one price truth broken
double restock          -> stop: flow authority broken
unattributed state      -> fix (learning channel)
missing warning         -> fix (fairness rule)
decay violation         -> fix (recovery rule)
arbitrage over ceiling  -> classify authored/unintentional; fix or author
projection error        -> recalibrate or author tolerance
flake                   -> quarantine with reason + hypothesis (repo policy)
```

---

## §V.8 Test naming and focus

```text
EconomyPrice_<Check>       EconomyShock_<Check>
EconomyFlow_<Check>        EconomyBarter_<Check>
EconomyHeat_<Check>        EconomyEmbargo_<Check>
EconomyRation_<Check>      EconomyContract_<Check>
EconomySurface_<Check>     EconomyProjection_<Check>
```

Focused runs: `run_test.sh Ashfall.Core.Tests/Economy*` patterns per family.

---

## §V.9 Harness reuse

Shared with W3-01/W2-03 where possible:

| Harness | Shared? | Notes |
|---|---|---|
| soak driver | yes (W2-03 core) | economy plugs in market instrumentation |
| seeded RNG facade | yes | determinism discipline |
| evidence writer | yes | HEAD-stamped YAML |
| surface capture | yes (W2-05 snapshots family) | screen comparison kit |

No parallel harness is created; the economy plan extends the shared one.

---

## §V.10 Cost model for verification

| Tier | Effort |
|---|---|
| T1 checks + generators | 4-5 days |
| T2 kits (12 kits) | 8-10 days |
| T3 instrumentation + reports | 3-4 days |
| Evidence/closeout | 1 day |

≈ 3 weeks integrated, matching the phase schedule in Part I §15.

---

*End of Part V. Continues in Part VI (worked case study).*# W3-02 · PART VI — WORKED END-TO-END CASE STUDY

> One economic thread — "the salt blockade" — followed through all ten points:
> from a war-stage consequence to a refugee family's dinner table, with every
> finding, repair, and test it touches. Illustrative IDs; verified at P0.

---

## §VI.1 The thread

### VI.1.1 Premise

- War stage 2 closes the north road (route gate `war_north_block`).
- Salt is the region's preservation input; the holdfast market depends on the
  salt run (`north_road`, 4 days, capacity 30).
- The blockade shocks salt/preserved-food prices; the player may broker a
  black-market substitute, negotiate a treaty corridor, or ration.
- The thread touches: route gates (P2), shocks (P3), embargo-ish restrictions
  (P6), black-market heat (P5), rationing (P7), the flow model (P10), and
  every surface (P9).

### VI.1.2 Why this thread

Salt is the smallest good whose disruption cascades through every economy
system: preservation → food shelf life → rationing → morale → migration. If
the economy handles salt truthfully, it handles the general case.

---

## §VI.2 Point 1 — price composition on the thread

### VI.2.1 Baseline quote (pre-war)

```text
salt @ holdfast_market:
  base            1.0
  regional        1.15   (atlas: salt scarcity index 0.35)
  stance          0.95   (salt faction neutral)
  shock           —      (none)
  route           1.10   (north_road cost)
  policy          —      (no rationing on salt)
  final           ≈ 1.20
```

Attribution: five rows visible in the trade screen.

### VI.2.2 Findings on the thread

| # | Finding | Class | Repair |
|---|---|---|---|
| EP-01 | route factor applied only at purchase, not in barter evaluation | quote inequality | route both through QueryPrice |
| EP-02 | regional scarcity factor double-read (atlas and a market-local scarcity) | double-count | remove local factor; atlas owns |
| EP-03 | salt base price present in two tables (market + recipe ingredient constant) | duplicate authority | single table; recipe reads quote |

### VI.2.3 Tests touched

Quote equality kit across seven consumers (salt in each); factor monotonicity
for regional scarcity; arbitration: holdfast vs. salt_market pre-war spread
below ceiling (expected ~1.15×; ceiling 1.5× route-adjusted).

---

## §VI.3 Point 2 — the route, the gate, the flow

### VI.3.1 Write-map before repair

```text
who changes holdfast salt stock?
  caravan arrival (north_road)          <- intended
  market self-restock (daily cadence)   <- FINDING: double source
  player trade                          <- intended
```

The market's self-restock made the north road decorative: salt returned
regardless, so the blockade had no effect. This is the canonical **decorative
route** finding.

### VI.3.2 Repair

- Remove salt (and preserved food) from the market self-restock list; those
  goods are route-fed (authored).
- Keep self-restock for bulk local goods (authored list).
- Regenerate the write-map: single writer per good class.

### VI.3.3 Gate behavior (post-repair)

```text
war stage 2 -> gate closes
run scheduled day D: gate evaluation -> closed -> status DELAYED (reroute search)
reroute candidate: ridge_trail (longer: 7 days, capacity 12) -> authored fallback
if no reroute: RETURN, cargo stays at origin (authored)
```

Finding EP-04: `ridge_trail` exists in data but has no gate table entry —
gate evaluation crashed silently (caught, logged as "unknown gate" in debug
only). Repair: register the gate; add the static check E1.5 for unknown gate
refs.

### VI.3.4 Flow tests

- Close gate → no north arrivals; salt stock declines on consumption curve.
- Reroute → arrivals at lower capacity; decline slows.
- Atlas effect: scarcity index rises within authored ticks; the regional
  factor moves (visible).

---

## §VI.4 Point 3 — the blockade shock

### VI.4.1 Shock authored

```yaml
shock: salt_blockade
cause: consequence:war_stage2_reached
goods: [salt, preserved_food]
kind: supply-cut
magnitude: 1.45
duration_days: 18
decay: linear_half
stacking: extend-capped (max_total: 30)
attribution: "the north road is closed: {days_left} days"
```

### VI.4.2 Findings

| # | Finding | Class | Repair |
|---|---|---|---|
| EP-05 | magnitude 1.45 had no clamp entry (table added after authoring) | bounds | register + clamp |
| EP-06 | shock and route factor both model the closure (double-count) | double-count | decide: shock is the *price* memory; route is the flow. Both correct if the shock decays as reroute proves out — authored: shock magnitude reduces when reroute active (interaction rule) |
| EP-07 | decay returned price to baseline even while gate still closed | recovery logic | decay ends at the authored floor when cause persists: authored min-multiplier while cause active |

EP-06/07 are the interesting ones: the thread shows **shock/route interaction
is authored policy**, not automatic. The interaction rule:

```text
if cause event still active at decay end:
   shock transitions to "sustained" state at authored floor (1.2×)
else: full decay to baseline
```

### VI.4.3 Stacking test

Inject a second shock (`foundry_salt_input_shortage`) on salt: stacking rule
extends duration capped at 30 days; magnitude stays 1.45 (max), not 2.1. The
pile-up test asserts composite stays under the 6× ceiling (observed ≈ 1.45 ×
1.2 route × 1.15 regional × 0.95 stance ≈ 1.9×).

---

## §VI.5 Point 4 — the black-market substitute

### VI.5.1 The barter path

Salt appears on the black market (smuggled, 3× price). Barter evaluation uses
QueryPrice on both sides; finding EP-08: the black market's valuation used a
local "desperation" multiplier not present in the buyer's quote → unfair
trades looked fair.

Repair: desperation is a **quote factor** (authored, bounded 1.0–1.8 on the
black-market context) so both sides see it in attribution.

### VI.5.2 Credit-like pattern found

The black market offered "pay after delivery" — a credit pattern with no
record: finding EP-09. Repair per Point 4: bounded debt (cap authored), a
clearing event (delivery deadline), a default ladder (reminder → refusal →
standing with the market's faction). No new currency emerged; the tab is
scoped and cleared.

### VI.5.3 No-currency guard

E1.8 grep after repairs: no new global funds state; the tab lives in the
black-market owner as a contract-scoped record.

---

## §VI.6 Point 5 — heat on the substitute

### VI.6.1 Authored heat

```text
sources: volume (per transaction), good class (salt is low-profile food,
         meds high), partner (fixed smuggler -> accumulating)
thresholds: T1 20, T2 45, T3 80 (units)
decay: -4/day idle; -8/day hiding
```

### VI.6.2 Scenario

Buying salt through the black market for two weeks: heat crosses T1 (a rumor
in the market chatter — authored), T2 (a shakedown scene: pay, refuse, or
stall — W3-01 choice), T3 after continuing (a warrant: guards search the
shelter; avoidable by laying low per the authored avoidance path).

### VI.6.3 Findings

| # | Finding | Repair |
|---|---|---|
| EP-10 | heat wrote from two systems (black market and a market guard event) | single writer: black-market owner; guard event becomes a reader |
| EP-11 | T3 fired with no warning in one path (heat jumped from 70→85 in one trade) | transaction heat capped per event (max +10) so thresholds are crossable only over multiple events |

### VI.6.4 Routing

T3 warrant → W3-04 security/combat consequence path (one route); the
shakedown scene → W3-01 choice chain + journal. No parallel handling.

---

## §VI.7 Point 6 — the treaty corridor

### VI.7.1 The player's diplomatic option

A treaty path exists (`RegionalTreatySystem`): a corridor agreement with the
salt faction (stance gate) opens a protected route (`corridor_route`, 5 days,
capacity 18, guarded). Thread test: treaty → new route appears; gate
`war_north_block` does not apply to the corridor (authored exception: guarded
route); shock interaction: while corridor active, the salt shock decays per
the interaction rule (cause partially mitigated).

### VI.7.2 Findings

| # | Finding | Repair |
|---|---|---|
| EP-12 | treaty opened the corridor in the atlas but the router never scheduled runs | router treaty integration |
| EP-13 | embargo table had no corridor class (over-broad war embargo blocked it) | scope: war embargo targets the north road only |

### VI.7.3 Band test

Treaty requires stance ≥ cold; at hostile the corridor closes (band rule).
Tested: stance drop → corridor closes with warning (the treaty partner sends
word — authored), not silently.

---

## §VI.8 Point 7 — rationing as the emergency answer

### VI.8.1 The policy

Player chooses strict rationing (salt-dependent preservation failing):
ratios food 0.7, preserved_food 0.4 (rationing preserved food hardest);
morale effect -moderate routed to psychology; compliance events (a kitchen
theft scene) authored, bounded at one per authored window.

### VI.8.2 Findings

| # | Finding | Repair |
|---|---|---|
| EP-14 | displayed ratios in the ration screen came from a local constant, not the policy owner | surface unification |
| EP-15 | exit condition absent (policy stuck after stock recovered) | authored exit + recovery curve |
| EP-16 | morale effect wrote directly to a morale field, bypassing the psychology modifier stack | route through NeedsModifierStack (W3-03 owner) |

### VI.8.3 Tests

Consumption per ratio via needs owner; morale route verified; exit returns
consumption over 5 days (curve); legibility equality.

---

## §VI.9 Point 8 — hiring guards for the corridor

### VI.9.1 Contract

Escort duty for the corridor run: pricing = base(30) × risk(corridor, war
pressure 1.2) × reputation(0.95) ≈ 34 upfront + 6/day. Term 6 days.

### VI.9.2 Findings

| # | Finding | Repair |
|---|---|---|
| EP-17 | escort outcomes did not connect to the caravan run (guards hired, no loss reduction) | wire duty effect: escort reduces route risk class through the caravan owner |
| EP-18 | default ladder missing for contracts (desertion straight to hostility) | ladder: reminder → grumble → departure with standing, no instant hostile |

### VI.9.3 Tests

Two contracts, identical inputs → identical price. Escort run: authored risk
reduction observable (loss events fewer over sample). Default ladder scripted.

---

## §VI.10 Point 9 — the surface story

### VI.10.1 What the player sees during the thread

```text
day 0   trade screen: salt 1.20, factors listed
day 3   war stage 2: shock row appears "north road closed: 18 days";
        supply band drops scarce
day 3   caravan status: DELAYED (gate: north road)
day 5   reroute: arrivals resume at lower capacity; shock persists
day 9   black market opens salt at ~3.2; heat begins
day 12  shakedown scene (T2 warning)
day 14  treaty dialogue; corridor opens on agreement
day 14  shock row shows days_left with interaction note
day 20  corridor deliveries; shock decays; band normal
day 24  heat T3 avoided by not trading black market for 4 days (decay)
```

Every screen element sources from owners; the audit scripts the entire
sequence and asserts element values at each checkpoint.

### VI.10.2 Findings

| # | Finding | Repair |
|---|---|---|
| EP-19 | supply band flipped scarce/normal per tick during reroute arrival | hysteresis in band computation (authored thresholds per direction) |
| EP-20 | shock row stayed after shock ended in one path (stale row) | record-driven rendering; removal test |

---

## §VI.11 Point 10 — the flow model on the thread

### VI.11.1 Outlooks the model produced

```text
day 3: salt net -1.2/day, days-to-critical 14 (range 10-18 due to ETA uncertainty)
day 5: reroute -> net -0.5/day, range widens
day 14: corridor -> net +0.3/day, critical cleared
```

### VI.11.2 Findings

| # | Finding | Repair |
|---|---|---|
| EP-21 | ration screen and crisis warning computed different outlooks | both read the flow model |
| EP-22 | projection ignored preservation decay (preserved food spoiled) | add decay flow from the inventory owner |
| EP-23 | range reported as point (false precision) | honest range output |

### VI.11.3 Tests

Accuracy against actuals over 20 days (authored tolerance 15% absolute);
critical flag led the actual critical day by ≥2 days (warning lead), else
authored absence noted; all surfaces identical.

---

## §VI.12 The thread's finding ledger

| # | Class | Status |
|---|---|---|
| EP-01 | quote inequality | repaired |
| EP-02 | double-count | repaired |
| EP-03 | duplicate authority | repaired |
| EP-04 | unknown gate | repaired + static check |
| EP-05 | bounds gap | repaired |
| EP-06 | shock/route interaction | authored rule |
| EP-07 | recovery logic | authored sustained floor |
| EP-08 | unfair valuation | factor added |
| EP-09 | hidden credit | bounded record |
| EP-10 | heat double writer | single writer |
| EP-11 | unwarned threshold | per-event cap |
| EP-12 | treaty routing gap | wired |
| EP-13 | over-broad embargo | scoped |
| EP-14 | surface constant | unified |
| EP-15 | policy stuck | exit + curve |
| EP-16 | bypass routing | owner routing |
| EP-17 | decorative duty | wired |
| EP-18 | missing ladder | authored |
| EP-19 | band flip | hysteresis |
| EP-20 | stale row | record-driven |
| EP-21 | duplicate estimators | unified |
| EP-22 | decay missing | flow added |
| EP-23 | false precision | ranges |

Twenty-three findings from one good. The yield justifies the program and
demonstrates why the repair **cap** exists: the point is not to fix everything
this quarter; it is to fix the class, then let the ratchet hold the line.

---

## §VI.13 What the thread teaches

1. **Decorative systems are the default failure mode.** Routes, gates,
   duties, treaties — all existed and none affected outcomes until wired
   (EP-04, EP-12, EP-17).
2. **Double-counting is subtle.** Three separate findings (EP-02, EP-06,
   EP-21) were about two models of one reality.
3. **Warnings are system properties.** EP-11's unwarned threshold came from
   event-size authoring, not threshold design.
4. **The surface is the audit's best tool.** Scripting the player-visible
   sequence surfaced EP-19/20/23 that owners looked clean for.
5. **One good, twenty-three lessons.** Salt is the economy's lamplighter.

---

*End of Part VI. Continues in Part VII (Q&A).*# W3-02 · PART VII — EXTENDED Q&A AND OPERATIONAL MODEL

> Sixty questions for foremen and builders, then the operational model:
> schedule, dependencies, staffing, and governance for the economy plan.

---

## §VII.1 Governance Q&A (Q1–Q15)

**Q1. Does this plan create a currency?**
No. The F13 boundary is explicit: no universal funds ledger. Credit-like
patterns are scoped, capped, and cleared; currency remains a separate signed
decision.

**Q2. Who owns the price?**
The market owner executes `QueryPrice`; factors come from their owners
(atlas, stance, shock record, route, policy). The plan adds attribution,
bounds, and equality tests — not a new price table.

**Q3. What if RegionalPriceAtlas and the market disagree today?**
That is a finding (duplicate authority). The repair is one source: the atlas
owns regional state; the market reads it.

**Q4. Can a builder add a new factor?**
Yes, with: formula, clamp, source owner, attribution string, monotonicity
test, and registry entry. No factor without a source.

**Q5. What about the UI showing prices?**
It renders `quote.factors[]`; any UI-side arithmetic is a finding (EP-02
class).

**Q6. Does this plan touch save schema?**
Path B no (all state already owned). Path C's committed shipments would ride
the caravan owner's existing section (signed). Shocks/heat persist through
their owners; if any owner lacks persistence, that is a P0 finding and a
bounded repair within that owner (not a new save section).

**Q7. What if a shock's cause event never fires?**
E1.5 static check: dangling cause refs fail. Also the lifecycle test asserts
onset only via causes.

**Q8. How are bounds enforced — data or code?**
Data (tables) validated at CI by `economy-bounds-check.sh`; runtime clamps as
defense in depth.

**Q9. Can two shocks stack?**
Per stacking rule: extend duration (capped) or max magnitude — never
multiply beyond the cap. Pile-up tests prove it.

**Q10. What if the player exploits a market pair?**
Arbitrage above the authored ceiling is either a bug (fix) or authored (a
trade route — record it). No silent unbounded arbitrage.

**Q11. Who owns supply level display?**
The atlas (state) and the trade surface (projection). The market owner
restocks; the router schedules; the display never computes.

**Q12. How does rationing interact with W3-03?**
Ration policy applies consumption via needs owners; morale effects route
through the psychology modifier stack. W3-03 owns the effects; this plan owns
the policy mechanics.

**Q13. How does heat interact with W3-04?**
T3 consequences route through the security/consequence path (W3-04). Heat
accrual is economy-owned; raids are security-owned. One route.

**Q14. What about mercenaries in combat?**
Duty contributions land in owners (defense/combat); no parallel power stat.
W3-04 reads the contribution through its existing state.

**Q15. How is determinism proven?**
Seeded RNG facade for shocks/losses/outcomes; T3 determinism assertion: same
seed → byte-identical price series.

---

## §VII.2 Method Q&A (Q16–Q30)

**Q16. Why start with the owner map?**
Because every subsequent audit compares against owners; an incomplete map
produces false equality reports.

**Q17. Why is quote equality the flagship test?**
It collapses price/UI/consumer divergence into one property with seven
consumers. Highest coverage per test in the plan.

**Q18. Why round once at the end?**
Per-factor rounding compounds differently per market and breaks equality
across consumers (the classic cross-screen penny drift).

**Q19. Why are ranges required for projections?**
Because arrivals are uncertain; point precision is a lie that erodes trust
when wrong. Honest ranges are the design.

**Q20. Why cap repairs per phase?**
Same as W3-01: ranked backlog, 20 per phase proposal, ratchet holds the line.
The salt thread's 23 findings show the risk of open-ended repair.

**Q21. How deep do we audit the black market?**
Its trade paths fully (small surface), heat scenarios scripted, routing
verified. Barks/chatter sampled per channel.

**Q22. What is the sampled market grid for quote tests?**
4 markets × 3 bands × 2 shock × 2 embargo = 48 states, sampled as an authored
deterministic list (not random). Expand only with a finding-rate reason.

**Q23. How do we test the treaty corridor without storytelling?**
Scripted state transitions (stance value, treaty record) reproduce the corridor
opening; no narrative simulation needed.

**Q24. What happens when an owner is missing entirely (e.g., no policy owner)?**
P0 finding. Path B proposes the minimal owner with a signed line; if declined,
the point reverts to Path A (audit only) — no improvised ownership.

**Q25. Why does the flow model include uncertainty but no hidden sim?**
Because a hidden simulation would be a second authority (Rule 5). Composition
+ ranges is the honest read model.

**Q26. How are existing balance values handled?**
Not touched. Measurement goes to W2-03; the plan reports, does not tune.

**Q27. What about flavor text on the trade screen?**
Authored copy registered through the corpus (W2-06); attribution strings are
strings too and follow the freeze when declared.

**Q28. How do we avoid rewriting the market system?**
By adding `QueryPrice` composition and attribution over existing owners, not
by replacing them. The plan's changes are contract-level (factor rows,
attribution lists, checks) plus targeted repairs.

**Q29. What is the smallest Path A deliverable?**
P0 + owner map + registry + bounds/attribution checks + the price-sample
report. No repairs.

**Q30. What is explicitly out of scope?**
Currency (F13), player-to-player trade (single-player provenance), new
markets, new goods, rebalancing, and any save section additions beyond signed
C items.

---

## §VII.3 Tooling Q&A (Q31–Q42)

**Q31. What tooling is new?**
Economy registry generator, bounds checker, attribution checker, quote
equality kit, flow-model accuracy report. Scripts under `scripts/ci/` with
`--check` where generated.

**Q32. What tooling is reused?**
The market/simulation owners, the W2-03 soak driver, evidence writer, surface
capture (W2-05 family), `content-acceptance-gate.sh` family.

**Q33. How are sample states chosen for equality tests?**
Authored deterministic list in the test file (Q22); every new market/state
adds rows during review.

**Q34. Where do soak CSV reports live?**
`docs/evidence/w3-02/T3-*.csv` summarized; raw dumps stay out of the repo per
hygiene norms.

**Q35. How do we keep the flow model fast?**
It is a composition of owner reads over a bounded good set (authored: the
tradable goods), caching per tick if needed; target is negligible cost vs.
tick budget.

**Q36. What stops the flow model from drifting into a simulation?**
The non-goal and the model spec: composition + ranges only. Any speculative
behavior (pathfinding trade decisions) lives in owners, not the model.

**Q37. How is heat tested without a player?**
A scripted trade-volume driver (the "active smuggler" policy) in a scenario
run; deterministic transaction sequence.

**Q38. Where is the re-quote notice tested?**
Surface kit: state changed between view and action → notice + fresh price;
authored copy ref checked.

**Q39. Can gates run on PRs touching only economy data?**
Yes — the focused pipeline pattern: data family detection → E1 checks only,
seconds.

**Q40. What about the registry becoming stale mid-phase?**
Regenerate on data change; `--check` in the weekly review and CI; drift is the
gate, not a human task.

**Q41. How do we verify "no second table" claims?**
Duplicate-value detection: the same (good, context) value derived in two
places fails the registry's single-owner rule; plus grep for constants in
surfaces.

**Q42. Evidence format?**
Same as W3-01: HEAD-stamped YAML/MD under `docs/evidence/w3-02/`, one file per
tier run, per-finding files, before/after repairs.

---

## §VII.4 Content Q&A (Q43–Q52)

**Q43. What if a good exists in a market but not in the atlas?**
Finding: the atlas must cover tradable goods (or explicitly mark
"market-local"). Undefined coverage is how duplicate authorities are born.

**Q44. How are ingredient prices tied to market prices?**
Recipes read the quote at execution (EP-03 repair); no constant ingredient
price table.

**Q45. What about goods only available via scavenging?**
Different owner (expedition/inventory). The economy model lists them as
flows-in from scavenge returns; no market price if untraded (authored).

**Q46. Can a shock affect a good with no market?**
No tradable price → no shock (a finding if authored); scarcity shows in flow
terms instead.

**Q47. How does weather block trade?**
Through route gates (W2-04 owns gate state). The economy owner reads the gate
and delays/reroutes; no separate weather trade logic.

**Q48. What about war-stage price effects beyond routes?**
Stage-driven shocks (authored) and stance moves; both routed and bounded.

**Q49. Do mercenaries trade their pay for goods?**
They receive payment through their owner; their post-payment behavior (buying
food) is a narrative/ambient matter, not an economy owner — no invented
mercenary spending simulation.

**Q50. How are dead survivors' debts handled?**
Scoped credit records: death closes or transfers per authored rule (a contract
term), never a silent write-off. The rule is authored, not inferred.

**Q51. What tone governs starvation pricing?**
The ethics note (§IV.12): scarcity signals the world; no revenge pricing; no
gloating flavor text.

**Q52. What if the freeze lands mid-phase?**
Attribution strings and new surface copy route through the freeze manifest;
existing strings untouched. No rework — same design as W3-01.

---

## §VII.5 Risk/Scope Q&A (Q53–Q60)

**Q53. Biggest execution risk?**
The double-count class: three findings in one thread. Mitigation: the
one-factor-per-dimension review row and the duplicate-detection check (Q41).

**Q54. What if fixes change the game's feel?**
Path B repairs truth, not tuning; feel changes only where a system was
decorative (EP-04/12/17). Those are the intended effects; W2-03 measures
afterward.

**Q55. What if the repair backlog exceeds the cap?**
It becomes ranked debt with owners; the ratchet prevents regression; phases
never expand beyond the cap.

**Q56. Can the plan run before W3-04/W3-03 are active?**
Points 1–4 and 6–7 (policy side) mostly yes; P5 T3 routing and P8 duty tests
need W3-04's paths; P7 morale effects need W3-03's stack. Dependencies are
per-point and declared.

**Q57. What if a market owner's API is insufficient for QueryPrice?**
The B path adds the composition inside the existing owner (method + factor
rows) — an extension, not a fork. If the owner is engine-side, the change
stays in its host; Core math where neutral.

**Q58. How does this plan avoid becoming a balance pass?**
By refusing W2-03's authority: no value tuning; measurements handed over. Any
"while we're here" tuning proposal is rejected and routed.

**Q59. What is the explicit stop condition?**
T1 green (registry/bounds/attribution), T2 kits pass, T3 assertions hold, the
six living documents current, findings triaged. Then closeout — no open-ended
polish.

**Q60. What does a successful year of this plan look like?**
Prices explainable and equal everywhere; routes and gates have effects;
shocks heal; credit is bounded; heat warns; surfaces teach; the flow model is
the single outlook. Players can learn the economy — that is the goal.

---

## §VII.6 Operational model

### VII.6.1 Staffing

| Role | Count | Responsibility |
|---|---|---|
| economy engineer | 1 | owner map, quote API, checks, kits |
| content/economy designer | 1 | factor/shock/route/policy authoring per playbooks |
| verifier | shared | T2/T3 runs, evidence |
| W2-03 liaison | part-time | band interface, measurement handoff |

### VII.6.2 Schedule (5-6 weeks)

```text
Week 1  P0 premise + owner map + registry skeleton; P1 factors inventoried
Week 2  P1 attribution + equality kit; bounds/rounding repairs
Week 3  P2 write-map + gate/loss tests; P3 shock audit + lifecycle kit
Week 4  P4 barter/credit + ladder; P5 heat write-map + thresholds
Week 5  P6 bands/embargo; P7 ration policy effects; P8 contracts pricing
Week 6  P9 surfaces + P10 flow model; soak; findings triage; closeout
```

### VII.6.3 Dependencies

```text
P1 ──► P3, P6, P9       (quote API and attribution format)
P2 ──► P3, P10          (flow/restock truth)
P4 ──► P5, P7, P8       (boundary + ladders)
P3 ──► P6               (shock/embargo interaction review)
W3-04 ──► P5 T3, P8 duty tests
W3-03 ──► P7 morale route
W2-03 ──► soak harness, band target
```

### VII.6.4 Cadence

- daily: T1 checks local;
- twice weekly: findings triage;
- weekly: registry regeneration + review; soak spot-run;
- phase end: kit runs + evidence pack + handoff.

### VII.6.5 Handoffs to sibling waves

| To | Handoff |
|---|---|
| W2-03 | median curves, peaks, recovery times, pressure samples |
| W3-01 | award vocabulary; shock causes from consequence records |
| W3-03 | ration morale effects; heat stress proposal |
| W3-04 | stance factors consumption; T3 routing; duty integration |
| W3-05 | ingredient pricing via quote API; recipe costs |
| W3-06 | surface specs (blocks → sources) |
| W2-04 | route gate reads (weather) |
| W2-05 | market locations / atlas coverage |

---

## §VII.7 Re-execution notes

The plan re-runs cheaply: registry regenerates, checks run, equality kit
replays, soak re-samples. After content/expansion waves, rerun T1 + targeted
T2; the soak on a schedule (shared). Findings route by class into the ratchet
baseline.

---

*End of Part VII. Continues in Part VIII (C-path designs and appendices).*# W3-02 · PART VIII — PATH C DESIGNS, MATRICES, AND APPENDICES

> C-path implementation-grade designs, cross-plan matrices, expanded glossary,
> artifact index, and the expanded signature sheet. Closes W3-02.

---

## §VIII.1 The C bundle (summary)

```text
C-P1  full factor telemetry and arbitration view
C-P2  committed shipments + standing orders (logistics state)
C-P3  shock lexicon with authored interaction rules and cause chains
C-P4  contract economy (multi-party, reputation ledger scoped to contracts)
C-P5  heat networks (informants, fence relationships)
C-P6  corridor/treaty engine depth (multi-faction routes)
C-P7  rationing politics (factions react to your policy)
C-P8  mercenary company depth (roster, morale, loyalty arcs)
C-P9  market intelligence surface (prices over time, trend reading)
C-P10 logistics planning surfaces (what-if read-only planning)
```

The C path is a **logistics and knowledge** depth layer — the player learns
the economy and plans, rather than only reacting. All additive, signed, owner-
scoped.

---

## §VIII.2 C-P1 — Factor telemetry and the arbitration view

### VIII.2.1 What it is

A read-only per-market price history and factor decomposition over time:

```text
MarketHistory:
  market, good, day, quote (with factors[])
  derived: trend (7/30-day), volatility (authored measure),
           dominant factor (largest delta contributor)
```

### VIII.2.2 Uses

- **Arbitration:** traders see spreads across markets (authored: only
  discovered/known markets — information sourcing).
- **Learning:** the trend view teaches the economy (scarcity vs. shock vs.
  stance).
- **Authoring:** designers see factor dominance per market (which factor
  actually drives play).

### VIII.2.3 Bounds

History is bounded (authored window, e.g., 60 days rolling); storage rides a
market owner section (signed if persistence needed; otherwise session). No
second price table: history is derived from quotes at sample points, not an
authority.

### VIII.2.4 Acceptance

History rows match sampled quotes; dominance calculation matches the
attribution deltas; information sourcing respected (unknown markets hidden);
bounded window enforced.

---

## §VIII.3 C-P2 — Committed shipments and standing orders

### VIII.3.1 What it is

Logistics state on the caravan owner:

```text
Commitment:
  id, route, cargo, qty, departure_day, eta, status (draft|committed|moving|arrived|failed)
StandingOrder:
  route, good, threshold (ship when stock < X), max_per_week, active
```

### VIII.3.2 Behaviors

- commit: player (or shelter policy) reserves cargo now for a later run;
- standing orders: automatic runs within authored limits (prevents
  micromanagement; costs credits per run);
- failures: gate closure/loss outcomes author the commitment's fate (return,
  hold, lost) — visible and routed.

### VIII.3.3 Persistence

Rides the caravan owner's save section (signed). Restore test: commitments
survive save/load with exact ETAs; a commitment due during the save gap
resolves on load through the normal resolution path (no double-resolve).

### VIII.3.4 Acceptance

Commitment lifecycle tested; standing order limits enforced; restore
round-trip; no double resolution; failure outcomes routed.

---

## §VIII.4 C-P3 — Shock lexicon and cause chains

### VIII.4.1 What it is

A named, documented set of shock archetypes with authored interaction rules:

```text
archetypes: supply-cut, demand-spike, cost-floor, quality-drop, route-fear,
            labor-loss, breakdown
interaction matrix: [archetype × archetype] -> rule
  supply-cut × supply-cut   -> extend-capped
  supply-cut × demand-spike -> additive-capped (net bounded)
  route-fear × supply-cut   -> replace (fear is the stronger narrative)
  ...
cause chains: event -> shock archetype -> market effect -> narrative echo
```

### VIII.4.2 Why a lexicon

Authors stop inventing per-event behaviors; the matrix is the policy. The
audit: every shock instance maps to an archetype; the matrix is total over
observed pairs.

### VIII.4.3 Acceptance

Archetype coverage total; matrix total for observed pairs; chains documented
with narrative echo refs (W3-01).

---

## §VIII.5 C-P4 — Contract economy depth

### VIII.5.1 What it is

Multi-party contracts with a **contract-scoped** reputation ledger:

```text
ReputationRecord: party, completed, defaulted, notes
contracts: buyer-seller commitments (delivery against payment over time)
```

**Critical boundary:** this is not universal credit. The reputation ledger is
scoped to contract parties (a merchant remembers you), never a global trust
score; the F13 boundary holds.

### VIII.5.2 Behaviors

- delivery contracts: seller promises goods at day D against payment/credit
  terms; enforcement uses the default ladder;
- reputation effects: lower prices / better terms with the party; scoped
  loss on default;
- resolution rides the market/trade owners.

### VIII.5.3 Acceptance

Scoping proven (no cross-party leakage); ladder enforced; determinism;
persistence signed.

---

## §VIII.6 C-P5 — Heat networks

### VIII.6.1 What it is

The black market gains social structure:

```text
networks: fences, informants, couriers (named roles, authored)
heat modifiers: using a known fence (safer, more expensive),
                informant risk (authored chance of tip),
                courier competence (loss reduction)
```

### VIII.6.2 Boundaries

All network state lives on the black-market owner (signed for persistence);
informant risk uses seeded RNG; tips route through the heat system (one
authority). No second heat state.

### VIII.6.3 Acceptance

Modifier effects tested; tip routing single-path; persistence; no parallel
heat.

---

## §VIII.7 C-P6 — Corridor and treaty engine depth

### VIII.7.1 What it is

Treaties become engine-level: multi-faction corridors, obligations
(tribute, defense), and revocation:

```text
Treaty:
  parties, scope (routes, goods, quotas), obligations, duration,
  revocation terms (penalty, warning period)
```

### VIII.7.2 Behaviors

- obligations are commitments (tribute deliveries) resolved through the
  contract machinery (C-P4);
- revocation warns (authored period) before effects;
- corridor routes are first-class route entries with guards (defense wired
  via W3-04).

### VIII.7.3 Acceptance

Obligation resolution; revocation warning path; corridor routing; determinism.

---

## §VIII.8 C-P7 — Rationing politics

### VIII.8.1 What it is

Factions and groups react to the player's rationing:

```text
reactions: merchants (price response), workers (morale/compliance),
           factions (standing shifts if their people are cut),
           families (authored scenes at thresholds)
```

### VIII.8.2 Rules

- reactions are authored per policy scope (who is cut);
- standing shifts bounded (Point 6 rules);
- scenes route through W3-01 choices; morale through W3-03;
- the politics layer adds no new numbers to the policy itself — it adds
  consequences.

### VIII.8.3 Acceptance

Reaction matrix authored and total for scopes; consequences routed; bounds.

---

## §VIII.9 C-P8 — Mercenary company depth

### VIII.9.1 What it is

Companies (not individuals): roster, morale, loyalty arcs, authored by
reputation:

```text
Company: name, roster (authored count class), morale, loyalty band,
         specialties (guard/escort/raid), history (missions done)
```

### VIII.9.2 Behaviors

- mission outcomes move morale/loyalty through authored tables;
- loyalty arcs (W3-01 weaves) produce authored scenes at thresholds;
- companies remember default (contract reputation, C-P4);
- combat/defense use company state through owners (no parallel stats).

### VIII.9.3 Acceptance

Outcome tables; arcs routed; owner integration; persistence signed.

---

## §VIII.10 C-P9 — Market intelligence surface

### VIII.10.1 What it is

A read-only trade desk: known markets' trends, spreads, and shocks, sourced
from C-P1 telemetry and information sourcing rules:

```text
sources: visited markets (full), rumor-sourced (partial, delayed),
         radio reports (authored), scout reports (expedition)
```

### VIII.10.2 Rules

- no omniscience: unfound markets absent; rumor data marked stale/partial;
- every shown number from telemetry (no estimation);
- the surface is W3-06 work; the data contract is here.

### VIII.10.3 Acceptance

Sourcing rules enforced (sample: unfound market absent); staleness marks;
telemetry equality.

---

## §VIII.11 C-P10 — Logistics planning surfaces

### VIII.11.1 What it is

Read-only what-if planning over the flow model: "if I commit a shipment on
route X, what happens to salt's days-to-critical?" Pure arithmetic over the
read model, clearly labeled as projection.

### VIII.11.2 Rules

- no state changes from planning (read-only);
- projections reuse the flow model math (one model);
- uncertainty ranges included;
- surface work W3-06.

### VIII.11.3 Acceptance

Projection math equals flow model; no writes; ranges shown.

---

## §VIII.12 C-path bundle recommendation

```text
take first:  C-P3 lexicon (authoring leverage), C-P2 commitments (gameplay depth)
then:        C-P1 telemetry, C-P9 intelligence surface
then:        C-P4 contracts, C-P6 treaties
optional:    C-P5 networks, C-P7 politics, C-P8 companies, C-P10 planner
```

C path requires: signed persistence for P2/P4/P5/P6/P8; W3-06 surface
bandwidth; W2-03 band agreement for telemetry interpretation.

---

## §VIII.13 Cross-plan interaction matrix

| This plan → | Interface | Direction |
|---|---|---|
| W3-01 narrative | consequence refs as shock causes; choice scenes | out/in |
| W3-01 narrative | award vocabulary (items/knowledge) | out |
| W3-03 psychology | morale effects of rationing; heat stress | out |
| W3-04 combat | stance values; T3 routing; duty integration | out/in |
| W3-04 combat | route risk classes (raiders) | in |
| W3-05 crafting | ingredient pricing via quote; output markets | out/in |
| W3-06 UI | surface specs (blocks, sources, re-quote notice) | out |
| W2-03 balance | measurement handoff; band targets | out/in |
| W2-04 environment | weather route gates | in |
| W2-05 locations | market locations; atlas coverage | in/out |
| W2-06 prose | attribution strings; surface copy | out |
| UNBLOCK-03 | string freeze for new copy | in |

### VIII.13.1 Never-cross list

```text
[ ] never creates a universal currency or funds ledger
[ ] never replaces the market/atlas/router owners
[ ] never writes psychology/combat/crafting state directly
[ ] never computes prices outside QueryPrice
[ ] never adds a second outlook estimator
[ ] never adds save sections in Path A/B
[ ] never tunes balance values (W2-03 authority)
```

---

## §VIII.14 Expanded glossary

| Term | Definition |
|---|---|
| archetype (shock) | named shock class with authored interactions |
| arbitration | cross-market spread trading |
| attribution | per-factor source/value in a quote |
| band (stance) | stance interval with access/price rules |
| barter fairness | |va − vb| ≤ tolerance |
| bounds | authored min/max per factor/shock/policy |
| capacity | units per caravan run |
| cause chain | event → shock → effect → echo |
| clearing | the event that closes scoped debt |
| commitment | reserved cargo for a future run |
| composite ceiling | authored max product of all factors |
| contract | priced duty with terms and outcomes |
| corridor | treaty-protected route |
| credit boundary | the F13 no-currency rule |
| decay curve | authored shock/supply recovery shape |
| decorative system | exists but affects nothing (finding class) |
| default ladder | warned sequence of unpaid-debt consequences |
| dominance (market) | one market's price factor dominance in a chain |
| embargo | scoped trade ban with end condition |
| E1.x | static check ids |
| factor row | one attribution entry |
| flow model | single read model of material outlook |
| gate (route) | weather/war/treaty condition on a route |
| heat | black-market attention |
| hysteresis | directional thresholds preventing band flip |
| re-quote notice | authored copy shown when price moved |
| registry (economy) | generated index of all economy values |
| restock write-map | who writes market stock |
| risk class | authored route/duty danger class |
| shock | bounded price/supply perturbation |
| source table | surface block → owner mapping |
| standing order | automatic run within limits |
| stacking rule | how multiple shocks combine |
| supply band | atlas level enum |
| sustained shock | shock held at floor while cause persists |
| tolerance (barter) | authored fairness window |
| treaty | multi-faction corridor/obligation agreement |
| warranty | (n/a) |
| write-map | value → writer multiset |

---

## §VIII.15 Artifact index

### VIII.15.1 Living documents

| Artifact | Point |
|---|---|
| `docs/economy/PRICE_COMPOSITION.md` | 1 |
| `docs/economy/ECONOMY_REGISTRY.md` (generated) | all |
| `docs/economy/SHOCK_LEXICON.md` | 3, C-P3 |
| `docs/economy/FLOW_MODEL.md` | 10 |
| `docs/economy/SURFACE_SOURCES.md` | 9 |
| `docs/narrative/…` (choice scenes for heat/shakedown) | W3-01 link |

### VIII.15.2 Machine artifacts

| Artifact | Form |
|---|---|
| query API composition changes | in owners (host/Core per engine rules) |
| generators/checkers | scripts + `--check` |
| kits (12) | test files |
| soak instrumentation | shared driver extension |
| evidence | `docs/evidence/w3-02/` |

---

## §VIII.16 Expanded signature sheet

```text
ASHFALL WAVE 3 · PLAN 2 (ECONOMY) · EXECUTION SIGNATURES
HEAD at signing: ________  Date: ________  Foreman: ________

[ ] P0 premise + owner map + registry skeleton
[ ] P1 factor attribution + equality kit + bounds/rounding
[ ] P2 write-map + gate/loss routing
[ ] P3 shock audit + lifecycle/stacking kits
[ ] P4 barter/credit boundary + default ladder (F13 boundary asserted)
[ ] P5 heat write-map + warned thresholds
[ ] P6 bands/embargo enforcement + restoration
[ ] P7 ration policy effects + exit/recovery (owner confirmation attached)
[ ] P8 contract pricing + duty integration (W3-04 coordination)
[ ] P9 surface unification + re-quote notice
[ ] P10 flow model (B read-only) 
[ ] C-P2/P4/P5/P6/P8 persistence: [ ] no [ ] signed: ________
[ ] repair cap per phase: ___ (proposal 20)

Retained non-authorizations:
[x] No currency. No FundsLedger. F13 remains separate.
[x] No balance tuning (W2-03).
[x] No save section additions without signed lines.
```

---

## §VIII.17 DoD and closeout

```text
[ ] T1 green; ratchet baseline recorded
[ ] T2 kits pass; equality across seven consumers
[ ] T3 assertions hold; evidence recorded
[ ] all living docs current
[ ] findings ledger + repair ledger complete
[ ] cross-plan handoffs delivered (W2-03 measurement; W3-06 specs)
[ ] boundary assertion: no currency, no tuning, no cross-owner edits
[ ] closeout memo per template
```

### VIII.17.1 Closeout memo template

```text
OUTCOME:
FILES:
CONTRACT: one price API; attribution; single flow model; ladders; boundaries
COMMANDS: T1/T2/T3 + results
LIMITATIONS: samples, deferrals, owners pending
SHARED PATHS TOUCHED:
LEDGER PROPOSALS:
ANNEX U RELEASES EARNED:
```

---

## §VIII.18 EPILOGUE — the economy's promise

The economy plan is where the world's pressure becomes legible. When it
works, a player can look at a price and know a story: the road is closed, the
salt faction is angry, the war is three weeks old. Nothing in this plan adds
a single unit of cruelty or a single grind; it removes the fog. Prices that
explain themselves, routes that matter, warnings before ruin — that is the
whole ambition.

*Document control: W3-02 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-02.*# W3-02 · PART IX — SCENARIO PACK (FIVE ADDITIONAL WORKED THREADS)

> Five more economic threads worked in compressed form — each shows the
> finding classes the audits produce, with the specific tests that catch them.
> This is the builder's pattern library: recognize the thread, apply the kit.

---

## §IX.1 Scenario 2 — War inflation (the slow squeeze)

### IX.1.1 Thread

War stage 1 → 4 over ~40 days. Each stage authors: stance shifts (two
factions), route closures (north then east), a demand spike on medicine
(casualties), a labor-loss shock on food (workers conscripted), and refugee
price effects. No single dramatic event: the economy just gets worse.

### IX.1.2 Findings this thread class produces

| # | Finding class | Example | Kit that catches |
|---|---|---|---|
| 1 | unbounded composite | 4 factors × stage multipliers → 7.8× | T3 price bounds |
| 2 | stage leakage | stage 3 routes active in stage 2 | E1.5 cause refs + gate state compare |
| 3 | stance double-move | one war event moves stance twice (direct + trade) | stance attribution cap test |
| 4 | demand spike on untraded good | medicine has no market row in a market | atlas coverage check |
| 5 | labor-loss not consumed | food price unchanged despite authoring | factor application toggle |
| 6 | recovery absent | post-war prices never return | T3 decay/return-to-baseline |
| 7 | stack saturation | medicine at 6× ceiling for 30 days (not playable) | pile-up + composite ceiling |
| 8 | stale war embargoes | peace stage leaves embargo rows | session cycle test (band/embargo restoration) |
| 9 | refugee flow missing | refugee influx has no market supply effect | flow model in-flows check |
| 10 | suicide pricing | essential goods above player income across the soak | economy-pressure handoff to W2-03 |

### IX.1.3 The specific tests

```text
E-INFL-1  composite ceiling across the full war arc (T3, 20 seeds)
E-INFL-2  stage transition atomicity (no interim mixed states across ticks)
E-INFL-3  stance move attribution per event, daily cap
E-INFL-4  factor toggle per authored stage factor
E-INFL-5  recovery: post-arc return to pre-arc median ± authored tolerance
E-INFL-6  playability floor: essential goods price ≤ authored income multiple
          (measurement handed to W2-03; violation = design review item)
```

### IX.1.4 Design lesson

Long arcs need recovery authoring as much as onset: the plan's decay rules
were written for shocks; war stages revealed the need for **stage-scaled
sustained floors** (each stage authors its floor; peace stage decays to
baseline over authored days). This became an addition to the shock lexicon
(C-P3).

---

## §IX.2 Scenario 3 — Refugee influx (the demand-side mirror)

### IX.2.1 Thread

A gate choice (W3-01 §V) admits refugees: +N mouths, +N workers, +N
consumers. The economy thread: demand spike on food/medicine/shelter,
labor supply increase (production up), stance effects (factions split),
and the shelter's rationing decision.

### IX.2.2 Findings

| # | Finding | Repair |
|---|---|---|
| 1 | population change has no consumption effect (needs count fixed) | wire population → needs owner count |
| 2 | demand spike authored manually instead of through population math | spike derived from population delta (authored curve) |
| 3 | labor supply not wired to production | foundry/crafting labor input reads shelter population class |
| 4 | refugees bought nothing (no wealth state) | authored: refugees have authored trade capacity class (no currency invented — barter labor/favors) |
| 5 | stance effects from admission missing for one faction | add authored reaction (W3-01 flags) |
| 6 | overcrowding morale not routed | psychology owner consumption modifier |
| 7 | shelter capacity ignored in pricing (housing scarcity) | authored capacity flow in the model |

### IX.2.3 The tests

```text
E-REF-1  population flip: consumption/needs scale within a tick
E-REF-2  labor: production input rises with population class (authored)
E-REF-3  demand ramp: food/med prices rise per authored curve, bounded
E-REF-4  no currency: refugee trade uses barter paths only
E-REF-5  morale routing through the psychology stack
E-REF-6  capacity flow in the model (days-to-overcrowd range)
```

### IX.2.4 Design lesson

Population is an economy input the original plan under-specified: the audit
converts "more people" from a narrative fact into an authored set of flows
(consumption, labor, demand, capacities). The fix is a **population flow
block** in the flow model — added to Point 10's spec.

---

## §IX.3 Scenario 4 — Foundry disaster (supply-side collapse)

### IX.3.1 Thread

A foundry mishap (W3-05) destroys the cupola for 20 days: metal goods
supply collapses; repair materials spike; the shelter's tool quality
degrades; the black market smells blood.

### IX.3.2 Findings

| # | Finding | Repair |
|---|---|---|
| 1 | production loss not represented in supply (tool stock static) | crafting output flows into the market's supply model |
| 2 | substitute goods unaffected (salvaged tools) | authored substitution curve (tool class) |
| 3 | repair-cost spike unbounded | shock bound + stacking with war |
| 4 | black-market heat spike from one visible shortage | heat sources include scarcity lure (authored) |
| 5 | foundry workers' labor idle (morale?) | W3-03 link (idle-labor morale proposal) |
| 6 | recovery: tool quality returns without authored time | authored restoration curve via crafting |

### IX.3.3 Tests

```text
E-FDR-1  production → market supply wiring (output flows)
E-FDR-2  substitution curve (salvage tools absorb N% demand)
E-FDR-3  repair-cost shock bounded + stacked with war ≤ ceiling
E-FDR-4  heat scarcity source authored + capped
E-FDR-5  restoration curve (quality returns over authored days)
```

### IX.3.4 Design lesson

Production and market supply were connected only indirectly in Part I; this
thread forces a **production flow block** (outputs/inputs by good class) in
the flow model — with W3-05's owners as sources. It is the same pattern as
population: the flow model is the meeting place, not a new authority.

---

## §IX.4 Scenario 5 — Treaty collapse (the political economy)

### IX.4.1 Thread

The player's corridor treaty (VI.7) collapses: a faction change makes the
partner hostile; obligations owed (tribute) go unpaid; the corridor closes;
the war embargo returns; the black market becomes the only salt path.

### IX.4.2 Findings

| # | Finding | Repair |
|---|---|---|
| 1 | treaty collapse left corridor routes schedulable | route-state on collapse: disarm runs |
| 2 | obligations poof (unpaid tribute ignored) | debt record per contract machinery; default ladder |
| 3 | embargo restoration failed after treaty end | E1.5 + restoration test (already a check; this thread reproduces it) |
| 4 | black market heat from a political event | heat sources include political pressure (authored, bounded) |
| 5 | stance cliff on collapse | warning period before hostile (authored) |
| 6 | the atlas never reflected the corridor economy (regional index unchanged) | corridor presence affects regional index while active |

### IX.4.3 Tests

```text
E-TRC-1  collapse disarms scheduled runs (no phantom arrivals)
E-TRC-2  obligations default through the ladder (warned)
E-TRC-3  embargo returns scoped; restoration re-verified
E-TRC-4  heat political source bounded
E-TRC-5  stance warning before band change (≥1 authored warning)
E-TRC-6  atlas index reflects corridor (active vs. collapsed states)
```

### IX.4.4 Design lesson

Political economy is still economy: treaties produce flows, obligations are
debts, collapse is a state change with restoration requirements. The thread
adds no new theory — it stresses the existing rules under transition.

---

## §IX.5 Scenario 6 — Mercenary default (the human contract)

### IX.5.1 Thread

The shelter hires a company for corridor escorts; funds run out mid-contract;
the company's response unfolds over days: grumbling → reduced effort →
ultimatum → departure or renegotiation. Combat readiness drops while they
decide.

### IX.5.2 Findings

| # | Finding | Repair |
|---|---|---|
| 1 | unpaid mercenaries kept full effectiveness | effort scales with pay state (authored bands) |
| 2 | departure instant (no warning) | ladder: ultimatum stage with authored window |
| 3 | departure removed all defense contribution at once | defection curve (authored handover) |
| 4 | renegotiation had no interface | authored contract-amendment path (same owner) |
| 5 | company's standing after departure unscoped | party-scoped reputation (C-P4), no global score |
| 6 | desertion at a bad moment had no narrative echo | W3-01 consequence (journal/radio beat) |

### IX.5.3 Tests

```text
E-MER-1  effort bands per pay state (measurable in duty outcomes)
E-MER-2  ultimatum warning before departure
E-MER-3  handover curve (contribution decays over authored days)
E-MER-4  amendment path (renegotiate → new terms recorded)
E-MER-5  scoped reputation only
E-MER-6  journal consequence once
```

### IX.5.4 Design lesson

Contracts are relationships under pressure; the ladder pattern (warn,
degrade, exit) is the general humane rule for every default in the economy.
It now appears in four threads (credit, heat, treaty, mercenary) — a strong
sign it belongs in the review standard.

---

## §IX.6 The scenario pattern (generalized)

Every economy thread follows:

```text
1. CAUSE     an event (war, weather, choice, production, population)
2. FLOW      what moves (route, stock, labor, demand)
3. PRICE     how the quote composes during the thread
4. SURFACE   what the player sees at each checkpoint
5. RECOVERY  how the world heals (or the authored scar)
6. FINDINGS  the six-to-ten standard classes above
```

The six threads collectively exercised: onset (war, influx, production,
politics, contract), flow (all), price (all), surface (all), recovery (all),
and the ladders. A builder who runs these six threads has covered the
economy's behavior classes; new content reuses the pattern.

### IX.6.1 Thread coverage matrix

| Thread | P1 | P2 | P3 | P4 | P5 | P6 | P7 | P8 | P9 | P10 |
|---|---|---|---|---|---|---|---|---|---|---|
| salt blockade | ● | ● | ● | ● | ● | ● | ● | ● | ● | ● |
| war inflation | ● | ● | ● | | | ● | ● | | ● | ● |
| refugee influx | ● | ● | ● | | | ● | ● | | ● | ● |
| foundry disaster | ● | ● | ● | | ● | | | | ● | ● |
| treaty collapse | ● | ● | ● | ● | ● | ● | | | ● | ● |
| mercenary default | ● | ● | | ● | | | | ● | ● | ● |

Every point appears in at least four threads; no point is only theoretical.

### IX.6.2 What the pack leaves undone (honest gaps)

1. **Quality markets** (good condition affecting price): proposed but not
   threaded; needs a W3-05 condition integration decision.
2. **Seasonal goods** (harvest windows): belongs to W2-04 axis; a thread
   exists only if the seasons axis lands.
3. **Gift/exchange across shelters**: multiplayer-shaped; out of scope.
4. **Long-term wealth accumulation**: currency-adjacent; F13.

These gaps are recorded rather than hand-waved; each has an owning wave or a
parked status.

---

## §IX.7 Scenario authoring template (for new content)

```yaml
thread: <name>
cause: <event + owner>
flows: [ <flow changes and their owners> ]
price_path: <factors active, expected composite range>
surface_checkpoints: [ day, element, expected ]
recovery: <authored curve or scar>
expected_findings: [ <classes> ]
tests: [ <ids> ]
tone_notes: <ethics review items>
```

Every new economy feature ships with at least one thread file in this
template; the six seeds above are the reference set.

---

*End of Part IX. Continues in Part X (final appendices and top-up tables).*# W3-02 · PART X — IMPLEMENTATION CHECKLISTS, SKETCHES, AND FINAL TABLES

> Ten per-point implementation checklists (step-by-step), two reference
> sketches (quote composition, flow model), the parameter authoring tables, and
> the final control section. Closes the expanded W3-02.

---

## §X.1 Point 1 implementation checklist

```text
[ ] enumerate every consumer of a price (7+ paths) into a table
[ ] name the owner of every factor (file:line evidence)
[ ] define the Quote struct (final + factors[] with source/value)
[ ] implement QueryPrice in the market owner (or extend existing)
[ ] migrate each consumer to the API (no inline math left)
[ ] author attribution strings for every factor (corpus registrations)
[ ] author clamps per factor; composite ceiling
[ ] round once at composition end (culture-invariant formatting)
[ ] write the quote-equality kit (7 consumers × sampled states)
[ ] write the factor application/monotonicity kit
[ ] write the arbitrage sample test vs. authored ceiling
[ ] register factors in the economy registry; wire E1 checks
[ ] record evidence; update PRICE_COMPOSITION.md
```

**Definition of done:** every consumer returns the same final price; every
factor attributable; bounds checks green; soak within ceilings.

---

## §X.2 Point 2 implementation checklist

```text
[ ] enumerate every writer of market stock (write-map)
[ ] classify goods: route-fed vs. self-restock vs. player-only
[ ] repair overlaps (single writer per good class)
[ ] enumerate routes; verify endpoint/path nodes exist in the catalog
[ ] enumerate route gates; verify each maps to a real source (weather/war/treaty)
[ ] define run resolution order: gates → travel/risk → arrival
[ ] author risk event tables (fractions) and loss outcomes
[ ] route every loss/arrival to owners
[ ] define/supply the ETA range in the run record
[ ] write the gate kit (open/close/reroute/supply effect)
[ ] write the loss kit (forced events, routed outcomes)
[ ] verify supply-band computation hysteresis
[ ] record evidence; update FLOW_MODEL.md
```

**DoD:** no decorative routes/gates; single stock authority; supply effects
visible; determinism.

---

## §X.3 Point 3 implementation checklist

```text
[ ] enumerate existing shocks (if any) and their causes
[ ] author the shock record spec in one place
[ ] author the decay curve set (linear, exponential, step)
[ ] author the stacking rule set + caps
[ ] author the interaction rule for shock×route and shock×policy
[ ] implement lifecycle: onset → sustain → decay → end/sustained
[ ] add attribution rows (with days-left) to the quote
[ ] author sustained floors while causes persist
[ ] write the lifecycle kit; pile-up test; contradiction resolution test
[ ] verify attribution removal at end (no stale rows)
[ ] seed all variable magnitudes/timings
[ ] record evidence; update SHOCK_LEXICON.md
```

**DoD:** every shock heals or floors; never unbounded; always attributed;
pile-ups capped; replay deterministic.

---

## §X.4 Point 4 implementation checklist

```text
[ ] census every credit-like pattern (tabs, deferred pay, coupons, promises)
[ ] scope each: counterparty, cap, clearing event, default ladder
[ ] verify no pattern acts as universal currency (E1.8)
[ ] implement barter via both-side quotes with tolerance
[ ] author tolerance per transaction class
[ ] implement the default ladder: reminder → refusal → standing (warned)
[ ] record boundary documentation (F13 assertion)
[ ] write fairness/ladder kits
[ ] record evidence; update the boundary section
```

**DoD:** no currency; all scoped; all ladders; fairness attributed.

---

## §X.5 Point 5 implementation checklist

```text
[ ] write-map heat writers (must be black-market owner only)
[ ] author heat sources with per-event caps
[ ] author thresholds T1/T2/T3 with consequences
[ ] author warnings + avoidance paths for T2/T3
[ ] author decay rates per heat class
[ ] cap heat; verify reachability of cap only by sustained behavior
[ ] route T3 consequences through W3-04 (one path)
[ ] route shakedown scenes through W3-01 (choice + journal)
[ ] write threshold/decay/bounds kit
[ ] verify determinism under scripted transactions
[ ] record evidence
```

**DoD:** bounded, warned, decaying, single-writer, routed.

---

## §X.6 Point 6 implementation checklist

```text
[ ] author the stance band table (total order, no gaps)
[ ] author access lists per band (exhaustive over good classes)
[ ] author price factors per band (within bounds)
[ ] author embargo records (scope, enforcement, end condition)
[ ] verify enforcement at purchase (not just display)
[ ] verify scope precision (out-of-scope free)
[ ] verify restoration post-embargo/band change
[ ] verify black-market redirection under hostile + heat accrual
[ ] author trade→stance effects with per-action/daily caps
[ ] write band/embargo/route kits
[ ] record evidence
```

**DoD:** every band enforced; every embargo ends; every move capped and
attributed; no loophole.

---

## §X.7 Point 7 implementation checklist

```text
[ ] verify/establish the ration policy owner (signed if new)
[ ] author policy spec (trigger, scope, ratios, enforcement, exit)
[ ] author ratio floors (safety)
[ ] wire consumption via needs owners (multipliers)
[ ] author morale effects routed to psychology (W3-03)
[ ] author compliance events (bounded frequency, reviewed tone)
[ ] author exit condition + recovery curve
[ ] surface: displayed ratios = owner values
[ ] write effect/exit/legibility kits
[ ] record evidence
```

**DoD:** policy effective, reversible, legible, routed; no policy can starve
by authoring error.

---

## §X.8 Point 8 implementation checklist

```text
[ ] author the contract record (terms, duties, pay, penalties)
[ ] wire pricing through QueryPrice with risk inputs from owners
[ ] author outcome tables (probabilities sum; seeded selection)
[ ] author the default ladder (warned stages)
[ ] wire duty contributions: guard→defense, escort→caravan, raid→combat
[ ] verify no parallel power stat (owners only)
[ ] persistence: contract state in the owner's existing section (or signed)
[ ] write pricing determinism/duty/default kits
[ ] record evidence
```

**DoD:** priced, recorded, effective through owners, warned, deterministic.

---

## §X.9 Point 9 implementation checklist

```text
[ ] enumerate every trade/ration/flow screen block
[ ] author the source table (block → owner read)
[ ] remove all UI math (render only)
[ ] render factor attribution verbatim; explanation = applied factors
[ ] render supply band from atlas; add hysteresis if needed
[ ] render shocks/embargoes from records; remove stale rows
[ ] implement the re-quote notice (authored copy; state-change trigger)
[ ] format numbers culture-invariantly
[ ] write the element-equality kit + freshness script
[ ] record evidence; update SURFACE_SOURCES.md
```

**DoD:** every block owner-sourced; equality tests green; notice works.

---

## §X.10 Point 10 implementation checklist

```text
[ ] define the tradable good set (registry)
[ ] implement the model: stocks, in-flows, out-flows (owners)
[ ] add production flow block (W3-05 outputs/inputs)
[ ] add population flow block (consumption/labor/demand/capacity)
[ ] compute net/day and days-to-critical with ranges
[ ] author the averaging window + uncertainty model
[ ] unify all surfaces (ration outlook, crisis warning, trade) on the model
[ ] write accuracy/lead-time/consistency kits
[ ] (C) committed shipments on the caravan owner (signed)
[ ] record evidence; update FLOW_MODEL.md
```

**DoD:** one model; honest ranges; warnings lead; no second estimator.

---

## §X.11 Reference sketch — quote composition (illustrative pseudocode)

```text
Quote QueryPrice(good, market, context):
    q := new Quote(good, market, context)
    q.base := catalog.BasePrice(good)
    q.Add("market base", q.base, source=catalog)

    r := atlas.RegionalIndex(market.region, good)
    q.Mult("regional", Clamp(r.factor, REGIONAL_MIN, REGIONAL_MAX), source=atlas)

    s := stance.Factor(market.faction, player)
    q.Mult("stance", Clamp(s, STANCE_MIN, STANCE_MAX), source=stance)

    for shock in shocks.Active(good, market):
        q.AddShock(shock)          # bounded by stacking rule; attributed row

    rt := router.RouteFactor(market.access_route, good)
    q.Mult("route", Clamp(rt, ROUTE_MIN, ROUTE_MAX), source=router)

    p := policy.Factor(good, market, context)   # rationing/embargo
    if p != 1.0:
        q.Mult("policy", Clamp(p, POLICY_MIN, POLICY_MAX), source=policy)

    q.final := RoundOnce(q.composite, ROUNDING_RULE)
    assert q.final <= COMPOSITE_CEILING
    return q
```

Notes: `RoundOnce` at the end only; every `Mult` records a factor row;
clamps at each step; the assert is the last-line defense (bounds check in CI
is the first).

---

## §X.12 Reference sketch — flow model (illustrative)

```text
FlowModel Project(horizon_days):
    for g in tradable_goods:
        stock := inventory.Count(g)
        inflow := caravan.EtaInflow(g)          # range
                + production.OutputRate(g)       # from crafting owners
                + scavenge.EstimatedReturn(g)    # authored expectation
        outflow := needs.ConsumptionRate(g) * ration.Factor(g)
                + crafting.InputRate(g)
                + decay.Rate(g)                  # shelf life
                + trade.PlannedExports(g)
        net := inflow.mid - outflow
        days := stock / max(EPS, -net) if net < 0 else INF
        model.Add(g, stock, net, days, inflow.range, outflow)

    return model      # consumed by surfaces; never writes state
```

**Purity rules:** reads owners only; no writes; ranges preserved; the model is
composed per tick (or cached with a tick stamp).

---

## §X.13 Parameter authoring tables (proposal defaults)

### X.13.1 Factor bounds

| Factor | Min | Max | Notes |
|---|---|---|---|
| regional | 0.5 | 2.0 | scarcity signal |
| stance | 0.6 | 1.8 | band-driven |
| shock | 0.5 | 2.5 | kind-dependent |
| route | 1.0 | 1.5 | distance |
| policy | 0.4 | 1.5 | ration/embargo |
| black-market context | 1.0 | 3.5 | authored illicit premium |
| **composite** | **0.25** | **6.0** | hard ceiling |

### X.13.2 Shock defaults

| Kind | Magnitude | Duration | Decay | Stacking |
|---|---|---|---|---|
| supply-cut | 1.25–1.5 | 12–20 | linear-half | extend ≤2× |
| demand-spike | 1.15–1.4 | 8–15 | exp | additive ≤1.6 combined |
| cost-floor | 1.1–1.3 | 20–40 | step | max |
| route-fear | 1.1–1.35 | 10–18 | linear | replace |
| labor-loss | 1.1–1.3 | 10–25 | linear | extend |

### X.13.3 Heat defaults

| Class | Sources | Per-event cap | T1/T2/T3 | Decay/day |
|---|---|---|---|---|
| transaction | volume | +10 | 20/45/80 | −4 idle, −8 hide |
| political | pressure events | +15 | shared | −3 |
| scarcity lure | shortage events | +5 | shared | −3 |

### X.13.4 Ration defaults

| Scope | Food | Water | Hygiene | Morale | Compliance window |
|---|---|---|---|---|---|
| all | 0.5–0.9 | 0.7–1.0 | 0.5–1.0 | −light..−heavy | 3–7 days |
| workers first | 0.7–1.0 | 1.0 | 1.0 | −light | 5 days |
| sick/children | ≥0.8 | ≥0.9 | ≥0.7 | −light | 5 days |

Floors: food ≥ 0.5 for any scope; below-floor authoring rejected (safety).

### X.13.5 Contract pricing defaults

```text
price = base(duty) × risk(route/war) × reputation(party, scoped)
base: guard 25, escort 30, raid 55, garrison 40
risk: 0.9–1.6
reputation: 0.85–1.2 (scoped)
pay split: 40% upfront, 50% per-day, 10% completion (authored per duty)
```

---

## §X.14 Additional anti-patterns (extended catalogue)

| # | Anti-pattern | Class | Detection |
|---|---|---|---|
| 31 | population ignored by consumption | flow | refugee-thread kit |
| 32 | production outputs not wired | flow | foundry-thread kit |
| 33 | substitute market missing | design | substitution test notes absence |
| 34 | recovery authored as instant | recovery | return-to-baseline shape |
| 35 | warning text referencing machine state | tone | review |
| 36 | contract outcome probabilities not summing | data | E1 bounds/table check |
| 37 | reputation leakage across parties | boundary | scoped-reputation test |
| 38 | embargo over-broad | precision | scope test |
| 39 | heat per-event uncapped | safety | threshold jump test |
| 40 | atlas coverage gaps | coverage | atlas coverage check |
| 41 | corridor route unguarded (free safety) | design | defense wiring check |
| 42 | treaty obligations unresolvable | routing | obligation kit |
| 43 | surface showing estimated (not sourced) numbers | truth | element equality |
| 44 | re-quote copy missing | fairness | freshness script |
| 45 | history table growing unbounded | hygiene | window bound |
| 46 | standing order unbounded frequency | safety | limits check |
| 47 | planning surface writing state | purity | read-only assert |
| 48 | quality/condition ignored in price | design | W3-05 integration note |
| 49 | gifts priced as zero (exploit) | fairness | gifting policy authored |
| 50 | currency creep via "tokens" | boundary | E1.8 |

---

## §X.15 The economy review standard (one page)

```text
1. SOURCE      every number names its owner
2. BOUND       every value has authored min/max and a composite ceiling
3. ATTRIBUTE   every movement of price is attributable in the quote
4. SINGLE      one writer per state; one estimator per outlook
5. WARN        every punitive consequence has a warning and an exit
6. RECOVER     every perturbation heals or is an authored scar
7. DETERMINE   every stochastic path is seeded
8. LEGIBLE     every surface is a projection; no UI math
9. SCOPE       credit is scoped; no currency without F13
10. TONE       scarcity is a story; the world never gloats
```

This is the checklist applied to every artifact class in the playbooks; it is
also the reviewer's one-page bar.

---

## §X.16 Final control and document map (W3-02)

### X.16.1 Document map

```text
Part I      summary contract (original)
Part II     deep designs: points 1-5
Part III    deep designs: points 6-10
Part IV     authoring & operations playbooks
Part V      verification catalog and harness design
Part VI     worked case study: the salt blockade (23 findings)
Part VII    extended Q&A (60) + operational model
Part VIII   C-path designs + matrices + glossary + signatures
Part IX     scenario pack (5 additional threads)
Part X      implementation checklists + sketches + defaults + this control
```

### X.16.2 Size accounting (honest)

This document is delivered as a **practical 180k-class plan**: Part I plus
Parts II–X. Part sizes vary with content density; the checklist is the
acceptance surface, not the character count. If any section is found thin
during P0, it is extended before its phase begins (the plan is designed to be
extended in place, as this expansion itself demonstrates).

### X.16.3 End of W3-02

**Explicit stop:** this plan proposes; it does not execute. All additions
require Annex U signatures (Part I §U.2) plus the per-phase authorizations in
§VIII.16. The F13 currency boundary, the W2-03 tuning boundary, and the
one-authority rule are non-negotiable within this document.

*Document control: W3-02 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-02.*# W3-02 · PART XI — FIELD GUIDE, FAMINE THREAD, AND FINAL CONTROL

> Pocket triage for economy breakage, one final worked thread (the famine
> ladder), and the closing control. Final part of W3-02.

---

## §XI.1 Field guide: when a price looks wrong

```text
1. quote attribution?     which factors are listed?
2. source equality?       does each factor equal its owner's value?
3. double application?    factor appears twice in the chain?
4. bounds?                within per-factor and composite clamps?
5. rounding?              rounded once at the end?
6. context?               wrong factor for the context (buy vs. barter)?
7. staleness?             state changed since the quote was computed?
8. duplicate authority?   another table computes the same value?
9. world state?           shock/embargo active but not shown?
10. playability?          composite above the authored ceiling -> design review
```

## §XI.2 Field guide: when supply behaves oddly

```text
1. restock write-map:     who writes this market's stock? overlapped?
2. route state:           runs scheduled? gates open? ETA range sane?
3. capacity:              cargo limits honored? shipments not duplicated?
4. loss events:           intercepts applied and routed?
5. atlas effect:          scarcity index reflecting arrivals?
6. band flip:             hysteresis working? no per-tick oscillation?
7. flow model:            stocks/in/out consistent with observations?
8. player trade:          sales/inputs counted on both sides?
```

## §XI.3 Field guide: when a shock misbehaves

```text
1. cause ref:             does the trigger event exist and fire first?
2. lifecycle:             onset -> sustain -> decay -> end observed?
3. stacking:              other shocks on same good; rule applied?
4. bounds:                magnitude within per-kind clamp?
5. floor/persist:        cause still active at decay end -> sustained floor?
6. attribution:           row present with days-left; removed at end?
7. determinism:           seeded history replays identically?
8. recovery:              price returns to pre-shock within authored days?
```

## §XI.4 Field guide: when credit defaults feel wrong

```text
1. scoped?                counterparty-only, capped?
2. clearing:              event defined; reachable?
3. ladder:                reminder -> refusal -> standing, ordered?
4. warnings:              visible before punitive steps?
5. currency leak:         E1.8 clean (no universal funds state)?
6. mercy route:           payment plan or negotiated path authored?
```

## §XI.5 Field guide: when heat jumps

```text
1. per-event cap:         transaction heat bounded (+10 max)?
2. writers:               single (black-market owner)?
3. thresholds:            T1/T2/T3 pacing; warnings before T2/T3?
4. avoidance:             hiding/laying low path exists and works?
5. decay:                 rates per class; baseline reachable?
6. routing:               T3 raid -> combat path (W3-04), once?
```

---

## §XI.6 Worked thread: the famine ladder

### XI.6.1 Setup

A multi-week food failure across three causes: a foundry input shortage
reducing preserved output, a closed river route, and a poor local season.
The shelter passes through all four ration states (normal → tight → strict →
famine-adjacent) and two authored policy exits.

### XI.6.2 The ladder

```text
week 0  normal (band abundant; prices stable across factors)
week 1  tight: food stock < 0.5 capacity
        policy: none yet; the flow model shows days-to-critical 9 (range 6-14)
        market: regional factor rising (0.35 -> 0.55); shock none yet
week 2  strict: stock < 0.3
        policy: strict rationing (food 0.6) fires via decree (player choice)
        morale effect begins (routed, W3-03)
        black market opens (scarcity lure heat source, +5/event capped)
week 3  crisis: stock < 0.1
        authored famine events: kitchen theft, hoarding search, one scene
        compliance bounded; warnings active
        shock: foundry input shortage (sustained floor while cause persists)
week 4  relief: river route reopens (treaty corridor, W3-02 C-P6 reference)
        arrivals resume at capacity 18; shock decays per interaction rule
        policy exit when stock > 0.6 capacity; recovery curve 5 days
week 6  normal restored; scars: one lost survivor (authored event), morale
        rebuilding (W3-03 recovery), black market heat decayed
```

### XI.6.3 Findings this thread produces

| # | Finding | Class | Repair |
|---|---|---|---|
| EF-01 | flow model projected days-to-critical as a point (7.0) not a range | honesty | ranges (already repaired; thread confirms) |
| EF-02 | strict rationing morale hit double-routed (policy + event) | routing | single morale effect per cause |
| EF-03 | scarcity-lure heat exceeded per-event cap during a single binge trade | bounds | cap enforced at attribution |
| EF-04 | famine events fired without the stage-3 warning window | warning pacing | authored warning before crisis stage |
| EF-05 | recovery curve applied to ration exit but not to market prices | recovery | both curves authored; tested |
| EF-06 | treaty corridor arrivals double-restocked the market (router + route) | double restock | single writer (route only) |
| EF-07 | the lost survivor event had no grief routing to W3-03 | cross-plan | consequence ref added |
| EF-08 | black market heat decay paused during the famine (source re-firing) | decay | authored: unattended sources prolong but never raise beyond cap |

### XI.6.4 The ladder's design lesson

Famine is not a single shock; it is a **ladder of policies and flows**. The
audit's job is to keep every rung authored: each ration state has warnings,
effects, and exits; each price movement has attribution; each recovery has a
curve. The end state (week 6) proves the whole machine can heal — the plan's
central promise.

---

## §XI.7 Maintenance calendar (economy)

```text
per content change:
  [ ] registry entries for new values; bounds + attribution present
  [ ] quote equality spot-check (7 consumers)
weekly:
  [ ] registry --check; bounds/attribution gates
  [ ] soak spot-run (one seed; assertions)
monthly:
  [ ] arbitrage sample rerun; authored exemptions re-justified
  [ ] surface source table re-verified (new elements?)
per release:
  [ ] T3 full sample; findings triage
  [ ] attribution strings through the freeze (if declared)
```

---

## §XI.8 Escalation map

| Finding class | Owner | Escalation |
|---|---|---|
| quote inequality | engineer | stop-the-line |
| double restock | engineer | stop-the-line |
| bounds violation | author | engineer (safety) |
| unattributed movement | author | lead (copy) |
| missing warning | author | lead (fairness) |
| recovery failure | author | lead (design) |
| currency smell | engineer | foreman (F13 boundary) |
| balance concern | — | W2-03 (measurement only) |

---

## §XI.9 Quick reference: the economy's ten rules

```text
1. one quote API          2. one flow model
3. attribution always     4. bounds always
5. warned consequences    6. scoped credit (no currency)
7. single writers         8. no UI math
9. seeded determinism    10. scarcity is a story, not punishment
```

---

## §XI.10 Closing note

An economy in a survival game is a teaching tool: it tells the player what the
world is doing and what their options cost. Every rule in this document exists
so that the lesson is true — the price that rises is the road that closed and
the war that came; the warmth in the shelter is the trade you chose to make.
When the numbers lie, the world feels arbitrary; when they are honest, even
the hard weeks feel survivable because they are *legible*. That is the
ambition, and the audit above is how it stays true.

> **Prices are the world's voice. Make them tell the truth.**

---

## §XI.11 Final control

**W3-02 final structure:**

```text
Part I     summary contract
Part II    deep designs 1-5
Part III   deep designs 6-10
Part IV    authoring & operations playbooks
Part V     verification catalog and harness
Part VI    worked case study (salt blockade; 23 findings)
Part VII   Q&A (60) + operational model
Part VIII  C-path designs + matrices + appendices
Part IX    scenario pack (5 threads)
Part X     checklists + sketches + defaults + control
Part XI    field guide + famine thread + this control
```

**Explicit stop:** proposal only. No execution without Annex U and §VIII.16
signatures. The boundary statements (no currency; no tuning; one authority)
are binding within this document.

*Document control: W3-02 · Wave 3 (expanded) · HEAD 5be1a30a · end of W3-02.*# W3-02 · PART XII — EXTENDED Q&A (61–100), VERSION LOG, AND QUICK TABLES

> Forthy more questions from the field, the document version log, and the
> quick tables. Completes W3-02 at the expanded target.

---

## §XII.1 Field Q&A, second set (Q61–Q70)

**Q61. A merchant's price doesn't match the dialog's claim. What first?**
The dialog is prose; the merchant's executed price is the quote. Compare quote
attribution vs. the dialog's implied factors — dialog claims are findings
(P9), not authority.

**Q62. Two markets quote the same final price for different goods. Wrong?**
Not necessarily: different bases with aligned factors can coincide. Check each
factor row, not just the final.

**Q63. A shock fired but the cause event is scripted three days later.**
Onset-before-cause finding (S4/E1.5 class). The lifecycle rejects it; the
cause must precede onset.

**Q64. The atlas and market disagree about scarcity.**
Duplicate authority finding: atlas owns regional state. Remove the market's
local scarcity or bind it to the atlas read.

**Q65. Caravans never arrive on time.**
Check ETA range vs. gate evaluation; a always-open gate ref (decorative) or
travel-time mismatch (map distance) are the usual causes.

**Q66. Players report the black market is "free money".**
Check heat accrual (per-event cap, thresholds) and purchase limits; free
profit means no heat or no caps — both are findings.

**Q67. Rationing seems cosmetic.**
Check consumption deltas at the needs owner; UI-only rations are the classic
decorative failure (P7 effect-truth kit).

**Q68. Mercenaries never leave even when unpaid.**
Ladder not wired (P8). Default must escalate: reminder → reduced effort →
ultimatum → departure.

**Q69. An embargo ended but prices stay high.**
Stale shock or un-restored route state; the restoration test catches both.

**Q70. The flow model disagrees with the ration screen.**
Duplicate estimators; both must read the single model (P10). The screen wins
nothing; the model is the source.

---

## §XII.2 Field Q&A, second set (Q71–Q80)

**Q71. Where do gift or favor trades fit?**
Authored gifting policy: gifts consume the item (inventory owner) and apply
relationship effects (W3-03), never "free value". If gifting creates value
arbitrage, it's a finding.

**Q72. Can prices differ between two shelters?**
Yes — regional factor + route + local supply. Different is expected;
inexplicable is the finding.

**Q73. What about storage spoilage?**
Inventory owner's decay; the flow model includes it as an outflow. Spoilage is
not a shock (it's continuous).

**Q74. Do weather events need their own price logic?**
No — they express through route gates and shocks (W2-04 owns weather state;
this plan's owners read it).

**Q75. What if a good has no regional index row?**
Atlas coverage finding (Q43); every tradable good must have one or be
explicitly market-local.

**Q76. Can the player manipulate a market by dumping goods?**
Authored: dumping affects the local supply (atlas) with diminishing effect;
unbounded manipulation = missing response curve.

**Q77. How do player-crafted goods enter the market?**
Through W3-05 outputs → inventory → market supply; pricing uses the same
quote (base + factors) as catalog goods. No special crafted-goods table.

**Q78. What tone should price explanations use?**
Factual and in-fiction: "the north road is closed", not "market modifier
1.45". Attribution strings are corpus-reviewed.

**Q79. How are failed caravan runs communicated?**
Through the visible run status/records and the supply band; never silently.
A lost run has a record with its fate.

**Q80. What about loans between shelters?**
Out of scope (multi-shelter is not the game's shape; a loan needs a contract
party and clearing — the scoped contract machinery could model it only if
authored).

---

## §XII.3 Field Q&A, second set (Q81–Q90)

**Q81. How many markets should the soak cover?**
All verified markets; the sample grid uses 4 for quote tests and the full set
for soak supply assertions.

**Q82. What is the authored tolerance for barter?**
Proposal 15%; per-transaction classes may author tighter. The tolerance is
data, reviewed.

**Q83. Should stance affect buying and selling symmetrically?**
Authored per band table; asymmetry is allowed (buying from a hostile is
worse than selling to them, or vice versa) — but it must be authored and
tested, not accidental.

**Q84. How are faction prices after conquest?**
Via stance and territory effects: conqueror's stance applies, routes change,
occupation (W3-04 C) may add levies as shocks. No separate conquest price
code.

**Q85. What prevents infinite trade loops (buy low, sell high, repeat)?**
Route travel time + capacity + the arbitrage ceiling; loops exist but are
bounded by time and quota. Unbounded loops mean missing travel/capacity
enforcement.

**Q86. How do I audit a new shock quickly?**
Four checks: cause ref, bounded magnitude, stacking rule, attribution string.
The lifecycle kit does the rest.

**Q87. What if the cause event is narrative and hence optional?**
Then the shock must have an authored trigger condition that can actually
occur; "optional cause" means the shock may never fire — that's a finding
(dead content) unless authored as rare.

**Q88. How is heat reset between campaigns?**
Heat is campaign state (owner section); new game = fresh. No carry-over
without a signed design.

**Q89. What about player reputation with the black market itself?**
Scoped reputation via the contract machinery (C-P4); if not in scope, heat is
the only relationship. No parallel trust score.

**Q90. Which file holds the ceilling values?**
The economy registry/data tables — one place, checked by `economy-bounds-check`.
Never in code.

---

## §XII.4 Field Q&A, second set (Q91–Q100)

**Q91. Can a builder add a factor without a signature?**
Within Path B: yes, if it meets the authoring rules (source, clamp,
attribution, monotonicity test) and lands with registry entries. It is an
implementation detail of the approved phase.

**Q92. What if W2-03's bands disagree with a repair's effect?**
Bands measure; repairs fix truth. Record the measurement delta in the handoff;
if the band expectation itself should change, that's W2-03's authored
decision.

**Q93. How do I test "no UI math"?**
Grep for arithmetic on price fields in surface code + the element-equality
kit. Both.

**Q94. Do projects ever need to bypass the quote (e.g., fixed price items)?**
Fixed prices are a factor (base with a `fixed` flag) or an authored exception
row; bypassing the API is a finding.

**Q95. What evidence proves a double-count fixed?**
Before/after: the value observed for the same state (e.g., 1.45) drops to the
authored single-factor value (1.20); the repair file records both.

**Q96. How is a shock's "sustained floor" authored?**
In the shock's record: `min_multiplier_while_active`. The lifecycle uses it
when the cause persists at decay end.

**Q97. Can rations be per-good (salt vs. food separately)?**
Yes — ratios are per-good in the policy table; scopes per group. Keep the
table readable; excess granularity is reviewed.

**Q98. What stops a "strict rationing forever" exploit?**
Exit condition authored + morale effects routed; permanent strict rationing is
a valid but costly choice, never free.

**Q99. How do we present a shocked price without scaring players?**
Attribution with remaining days; authored tone (temporary, explained). Fear
comes from the world, never from UI ambiguity.

**Q100. What is the single sentence that governs an economy contributor?**
"Every number has a source, every movement an explanation, every price a way
back."

---

## §XII.5 Document version log (W3-02)

| Version | Change |
|---|---|
| v1.0 | Part I — summary contract |
| v1.1 | Parts II–III — deep designs 1–10 |
| v1.2 | Part IV — playbooks |
| v1.3 | Part V — verification catalog |
| v1.4 | Part VI — salt blockade case study (23 findings) |
| v1.5 | Part VII — Q&A 1–60 + operational model |
| v1.6 | Part VIII — C-path + appendices |
| v1.7 | Part IX — scenario pack |
| v1.8 | Part X — checklists + sketches + defaults |
| v1.9 | Part XI — field guide + famine thread |
| v2.0 | Part XII — this finisher (Q&A 61–100) |

---

## §XII.6 Quick tables (one page)

### XII.6.1 The quote chain

```text
base x regional x stance x shock x route x policy -> round once -> clamp composite
each factor: source + clamp + attribution string
```

### XII.6.2 The four hard rules

```text
1. one quote API      2. one flow model
3. warned consequences 4. no currency (F13)
```

### XII.6.3 The three "stop the line" failures

```text
quote inequality (multiple prices)
double restock (multiple stock writers)
currency emergence (F13 boundary breach)
```

### XII.6.4 The recovery standard

```text
every shock decays or sustains-with-authored-floor
every embargo ends; every policy exits; every route restores
every credit clears or ladders
```

---

## §XII.7 Final control (W3-02)

**W3-02 expanded status:** complete at the expanded target. Parts I–XII.
Proposal only; no execution without Annex U and §VIII.16 signatures. The
boundary rules and the four hard rules above are binding within this
document.

*Document control: W3-02 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-02.*# W3-02 · PART XIII — FINAL ADDENDUM: LIMITATIONS, PARAMETER HISTORY, AND HANDOFF INDEX

> Short formal addendum: known limitations, the parameter revision history,
> and the handoff index. Completes W3-02 at the expanded target.

---

## §XIII.1 Known limitations (honest list)

```text
L1  Quality/condition pricing is proposed but not threaded (needs W3-05
    condition integration decision).
L2  Seasonal good windows depend on the W2-04 seasons axis; if that axis
    does not land, seasonal pricing stays out.
L3  Multi-shelter gifting/loans are out of scope (single-shelter game shape).
L4  Refugee wealth is modeled as authored trade capacity, not stored wealth
    (no currency by design).
L5  The black market's scoped contracts deepen at C; at B only bounded
    tabs/records exist.
L6  Trade-screen information sourcing (C-P9) is delegated to W3-06 surface
    work; this plan provides the data contract only.
L7  Soak coverage assumes the W2-03 harness; degradation of that harness
    defers T3 items with recorded handoffs.
L8  Attribution strings are authored copy; until the string freeze process
    is declared (UNBLOCK-03), new strings follow the corpus workflow.
```

Each limitation carries an owner and a review point; none is silent.

---

## §XIII.2 Parameter revision history (proposal defaults)

| Parameter | Initial proposal | Revision | Reason |
|---|---|---|---|
| composite ceiling | 8.0× | 6.0× | soak revealed unplayable peaks |
| regional clamp | [0.3, 3.0] | [0.5, 2.0] | scarcity signal vs. extortion |
| shock magnitude cap | 2.0× | 2.5× | cost-floor kinds need headroom |
| per-event heat cap | +15 | +10 | threshold jumps in one binge |
| trans. heat decay | −3/day | −4 idle / −8 hide | avoidance must matter |
| barter tolerance | 10% | 15% | early-game fairness |
| ration food floor | 0.4 | 0.5 | safety floor review |
| flow error tolerance | 10% | 15% | arrival uncertainty honest |

History is kept so changes are visible; each row cites the finding or review
that moved it.

---

## §XIII.3 Handoff index (exact deliverables to sibling waves)

| Receiving wave | Deliverable | Format |
|---|---|---|
| W2-03 | median price curves, peaks, recovery times, pressure samples | report + CSV |
| W3-01 | award vocabulary, shock-cause refs | doc section + data refs |
| W3-03 | ration morale effects, heat-stress proposals | interface doc |
| W3-04 | stance factors, T3 routing, duty integration points | interface doc |
| W3-05 | quote API at recipe execution, material flow blocks | interface doc |
| W3-06 | surface source tables, re-quote notice spec | doc |
| W2-04 | route gate read contract | doc |
| W2-05 | atlas/market coverage requirements | doc |
| W2-06 | attribution strings, surface copy list | corpus list |
| UNBLOCK-03 | new string inventory (until freeze) | list |

---

## §XIII.4 The four numbers every reviewer memorizes

```text
6.0×  composite ceiling (no price survives above it)
15%   barter tolerance (fairness window)
+10   per-event heat cap (no one-trade threshold jumps)
0     currencies allowed (F13 boundary, until a separate signature)
```

---

## §XIII.5 Final control

**W3-02 complete.** Parts I–XIII. Proposal only. Execution requires Annex U
(Part I §U.2) and the §VIII.16 phase signatures. The F13 boundary, the W2-03
tuning boundary, and the one-authority rule remain binding.

*Document control: W3-02 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-02.*# W3-02 · READER CARD (FINAL)

```text
ASHFALL W3-02 · ECONOMIC INTEGRATION · READER CARD

WHAT:    one price truth, one flow truth, warned consequences,
         scoped credit, attributed everything.
WHY:     prices are the world's voice; routes and shocks must be felt,
         never arbitrary.
HOW:     P0 owner map -> quote API + factors -> routes/gates/shocks ->
         barter/credit boundary -> heat -> bands/embargo -> rations ->
         contracts -> surfaces -> flow model.
GATES:   registry/bounds/attribution (T1); equality/fairness/ladders (T2);
         ceilings/decay/arbitrage/floor (T3).
STOPS:   quote inequality, double restock, currency emergence.
NEVER:   no FundsLedger (F13); no tuning (W2-03); no UI math; no second model.
SIGN:    Annex U + Part VIII.16.
```

*End of W3-02 (complete).*# W3-02 · APPENDIX SUPPLEMENT — GLOSSARY ADDITIONS AND CROSS-REFERENCES (FINAL)

| Term | Definition |
|---|---|
| arbitration ceiling | authored max cross-market profit; excess is authored or a finding |
| attribution string | player-readable cause line on a quote/factor |
| cause ref | consequence/event that triggers a shock or embargo |
| clearing event | authored moment that closes scoped debt |
| composite ceiling | 6.0× hard product cap across all factors |
| corridor | treaty-protected route with its own gate semantics |
| decorative route | route/gate that exists but affects no run |
| demand spike | shock archetype from population/event demand |
| desperation factor | authored black-market context factor (1.0–1.8) |
| E1.x | static economy checks (registry, bounds, attribution, source table) |
| famine ladder | the policy/flow progression under multi-week failure |
| flow block | population/production/decay in-flow or out-flow group |
| hysteresis | directional band thresholds preventing flip-flop |
| halt-sale | refusal enforcement of an embargo |
| market-local good | a good with no atlas row (explicitly authored) |
| per-event cap | bound on heat/standing movement from one event |
| ration floor | minimum ratio protecting against authoring-error starvation |
| re-quote notice | authored copy when price changed between view and purchase |
| scoped tab | bounded, cleared debt with one counterparty |
| source table | surface block → owner read mapping |
| standing order | automatic caravan run within authored limits |
| substitution curve | authored alternate-good absorption of demand |
| sustained floor | shock floor while its cause remains active |
| transaction heat | black-market attention from trade volume |

Cross-references: composition law → §II.1; flow model → §II.2/§III.5;
shock lexicon → §VIII.4; famine ladder → §XI.6; four hard rules → §XII.6.2.

*End of W3-02 (complete).*