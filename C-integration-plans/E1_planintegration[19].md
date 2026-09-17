---
PLAN_ID: E1-19
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 19
STATUS: READY_FOR_EXECUTION_WHEN_TRADE_ROUTE_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 192 — Player Trade Route Establishment"
SEQUENCE_FILENAME: "E1_planintegration[19].md"
PREVIOUS_FILENAME: "E1_planintegration[18].md"
NEXT_FILENAMES:
  - "E1_planintegration[20].md"
  - "E1_planintegration[21].md"
CATEGORY: LINK+TRADE+CARAVANS+MARKETS+FACTIONS+ROUTES
PRIMARY_INTENT: "Enable player-established trade corridors and scheduled caravans by composing existing world-route, caravan/vehicle, inventory, market, faction, debt, expedition, information, combat/defense, and outpost authorities without introducing a second economy, route topology, reputation system, combat resolver, or cargo ledger."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_MARKET_SYSTEM_FORBIDDEN: true
SECOND_CARAVAN_MOVEMENT_SYSTEM_FORBIDDEN: true
SECOND_WORLD_ROUTE_GRAPH_FORBIDDEN: true
SECOND_FACTION_REPUTATION_SYSTEM_FORBIDDEN: true
SECOND_COMBAT_RESOLVER_FORBIDDEN: true
SECOND_INVENTORY_LEDGER_FORBIDDEN: true
RUNTIME_RISK: VERY_HIGH
SAVE_RISK: VERY_HIGH
ECONOMY_RISK: VERY_HIGH
EXPLOIT_RISK: VERY_HIGH
MICROMANAGEMENT_RISK: HIGH
---

# E1 Plan Integration [19] — Player Trade Routes, Caravan Scheduling, Agreements, Route Risk, Market Handoffs, and Trade-Network Strategy

> **Sequence rule:** this file is `E1_planintegration[19].md`.
> The next files are `E1_planintegration[20].md`, `E1_planintegration[21].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 192 into an implementation-grade player trade-route programme.

The source plan identifies a strong gameplay gap: `TravelingCaravanSystem` already runs NPC caravans on
pre-authored routes and `TradeCaravanCatalog` already contains route definitions, but the player cannot
establish a persistent commercial corridor, reserve cargo, dispatch a caravan, negotiate a route-specific
agreement, defend the shipment, or build a network whose success and failure feed the economy.

The player-value goal is correct. The proposed architecture, however, risks duplicating several existing or
planned authorities:

- MarketSystem should continue to own prices, market stock behavior, settlement trade access, and trade
  settlement.
- Inventory should own cargo quantities and item identity.
- TravelingCaravanSystem / vehicle/expedition travel authorities should own caravan movement and arrival.
- World topology / route authority should own distance, travel path, closures, hazards, and route geometry.
- Faction standing / TradeStance should own settlement willingness and political access.
- Combat/defense/raid systems should own actual conflict and casualties.
- LedgerDebt should own debt.
- E1-3 information/rumor rails should own intelligence propagation.
- E1-10 outposts/waystations should own fixed route support sites.
- A generic `trade reputation` scalar must not become a second faction-standing system unless the repository
  already has a product-wide merchant reputation concept with a clear owner.

E1-19 therefore reframes the feature:

**a player trade route is a durable commercial contract and scheduling layer over a real world path; a player
caravan is a real canonical caravan/vehicle operation with reserved cargo, guards, destination market
instructions, and provenance.**

The route layer may own:

- route contract identity;
- source/destination settlement references;
- active commercial agreement references;
- dispatch policy/schedule;
- route-level accounting history/read model;
- trade-route lifecycle;
- operation receipts;
- route-specific authored policies not owned elsewhere.

It may not own:

- duplicate cargo inventories;
- world path truth;
- current market prices;
- faction standing;
- guard health/combat state;
- generic route hazard copied from the world;
- a second caravan movement clock;
- duplicate profit currency;
- a new global reputation score without an authority decision.

## 1. Source Intent Preserved

Plan 192 asks for:

- player-established routes;
- envoy/negotiation setup;
- cargo dispatch;
- guard assignment;
- recurring schedules;
- trade agreements;
- raids;
- route profitability;
- route upgrades;
- route map/UI;
- trade ledger;
- market integration;
- faction/stance integration;
- expedition discovery;
- debt integration;
- deterministic raids;
- old-save compatibility;
- events/quests;
- headless selftest.

E1-19 preserves these goals while replacing duplicated state with canonical references and transaction
contracts.

## 2. Core Architecture Thesis

```text
Known settlements + world route graph
        |
        v
Trade route establishment proposal
        |
        +--> settlement access / faction stance
        +--> envoy availability / skill
        +--> agreement negotiation
        +--> world-path validation
        +--> cost/commitment validation
        |
        v
PlayerTradeRouteContract
        |
        +--> route ID
        +--> origin / destination
        +--> path reference
        +--> commercial agreement refs
        +--> dispatch policy
        +--> lifecycle
        +--> accounting provenance
        |
        v
Dispatch order
        |
        +--> inventory cargo reservation
        +--> caravan/vehicle asset
        +--> guard survivor assignments
        +--> world path
        +--> destination market instruction
        |
        v
Canonical caravan movement / encounter / combat
        |
        +--> delay / reroute / raid / loss / arrival
        |
        v
Market settlement
        |
        +--> actual sell/buy transactions
        +--> return cargo / currency
        |
        v
Route accounting read model
```

Trade-route code orchestrates. It does not simulate those systems again.

## 3. Architectural Corrections to the Source Plan

### 3.1 Route hazard must be queried, not copied

The source proposes `hazardLevel` inside `TradeRoute`. If world routes already have hazard, territory,
weather, closures, or security state, the trade route should reference the path and derive current risk. A
stored route-level hazard may be a baseline commercial risk modifier only if that fact is distinct from world
hazard.

### 3.2 Profit is an accounting result, not stored magic value

`routeProfit` and `routeLosses` may be derived from an immutable trade-ledger history. The route should not
increment a free-floating profit counter that can diverge from currency/inventory transactions.

### 3.3 Cargo must be canonical inventory

`PlayerCaravan.cargo` is a manifest/reference over canonical inventory transfers. No duplicate list becomes a
second item owner.

### 3.4 Raids use real combat/encounter resolution

Trade route code may schedule/trigger an encounter request based on canonical route risk. It does not calculate
guard casualties and cargo loss independently if a combat/defense authority already exists.

### 3.5 Trade reputation should not duplicate faction standing

Use existing settlement/faction standing, TradeStance, contract history, merchant trust, or a narrow
agreement-performance reputation if the repo proves a separate concept is needed. Do not create a global
0–100 `tradeReputation` by default.

### 3.6 Route upgrades need world-site ownership

Road improvements, guard posts, and waystations are world/location/outpost/construction assets. The trade
route references them; it does not store a local `upgradeLevel` that independently changes travel time/hazard.

### 3.7 Establishment success need not be a random binary check

Negotiation can be deterministic from standing, skill, cost, policy, and explicit concessions. If uncertainty
is desired, use canonical diplomacy/negotiation resolution with keyed deterministic RNG. Do not add a new
generic success roll solely for this feature.

### 3.8 Raids should not roll once per arbitrary tick

Encounter/raid risk should be tied to travel segments or journey risk resolution using a stable caravan/route
operation key, not repeatedly rolled on load or every frame/day.

## 4. Non-Negotiable Rules

- MarketSystem owns market prices, stock, settlement trading, and trade settlement.
- Inventory owns cargo items and quantities.
- Caravan/Vehicle/Expedition travel authority owns movement/progress/arrival.
- World graph/route authority owns path, distance, route topology, closures, terrain, and dynamic hazard.
- Faction/TradeStance owns political trade access and standing.
- Combat/Raid/Defense authority owns combat outcomes and casualties.
- LedgerDebt owns debt.
- E1-3 Information owns information/intel propagation.
- E1-10 Outposts/waystations own fixed support sites.
- E1-17 Maintenance owns maintainable route-support asset condition.
- PlayerTradeRoute owns commercial route contract, schedule policy, agreement references, dispatch provenance,
  and route-level accounting/read model only.
- A route may exist while temporarily unavailable due to world conditions.
- A route path is referenced/re-resolved, not copied into a second topology.
- Caravan cargo is reserved/transferred once.
- Cargo is credited/sold only on real arrival.
- Return goods/currency are credited only through actual destination-market settlement and return delivery
  policy.
- Route accounting must reconcile to actual inventory/currency transactions.
- Guards are canonical survivors with travel state.
- Dead/injured guards reconcile through health/combat authorities.
- Trade agreements are contracts referencing faction/settlement terms, not free-form unvalidated benefit
  strings.
- Exclusive trade cannot magically block NPC caravans unless market/faction policy supports exclusivity.
- Tariff reduction changes Market/Faction settlement terms through canonical policy.
- Military protection contributes escort/security through faction/defense authorities.
- Intelligence sharing hands information to E1-3; it does not duplicate intel state.
- Ordinary scheduling is deterministic.
- Raid encounter decisions cannot reroll on save/load.
- Route establishment cannot duplicate currency/items.
- Route abandonment cannot delete caravans already in transit.
- Old saves start with no player-owned route contracts.
- Existing NPC caravans remain functional.
- No global `totalTradeNetworkValue` authoritative scalar; derive it.
- No global `tradeReputation` unless an ADR proves it is not faction standing by another name.
- Recurring schedules require explicit cargo-selection/reservation policy and cannot create items.
- Auto-dispatch must stop safely when cargo, guards, vehicles, agreements, routes, or destination access fail.
- Player control should remain strategic rather than dispatch-panel busywork.
- First release should prove one route and one recurring caravan before five-route network breadth.

## 5. Acceptance Slices

### Slice A — One commercial route contract
Establish one route between two known/trade-accessible settlements using a real world path.

### Slice B — One manual caravan
Reserve cargo, assign caravan/guards, travel, arrive, settle through Market, return or complete.

### Slice C — Agreement and deterministic route risk
One route-specific commercial agreement plus one encounter/raid handoff.

### Slice D — Recurring dispatch
Auto-dispatch through explicit inventory/vehicle/guard policy with safe blocking.

### Slice E — Network, upgrades, broader content
Only after accounting, market, travel, save, and exploit rails are proven.

Do not begin with monopoly systems, ten agreements, twenty caravans, and route upgrades simultaneously.


---

## E1-19A — Premise verification and trade-route authority audit

**Goal:** Verify caravan, market, route graph, inventory, faction, debt, expedition, combat, outpost, information, save, and settlement authorities before creating player route state.

### Required substeps

1. Inspect `TravelingCaravanSystem`, `TradeCaravanCatalog`, `MarketSystem`, `TradeStance`, faction standing/branch coordinator, world-route/path systems, ExpeditionSystem, vehicle/caravan movement, inventory/cargo transfer, currency/economy, LedgerDebt, combat/raid/defense systems, E1-3 information network, E1-10 outposts/waystations, E1-17 maintenance, journal/quest, and save registry.
2. Determine whether NPC caravans already have persistent runtime IDs, manifests, travel progress, route IDs, and encounter resolution.
3. Determine whether player-owned caravan behavior can extend/reuse `TravelingCaravanSystem` rather than creating a second movement engine.
4. Determine how MarketSystem performs settlement buy/sell transactions and whether remote/automated settlement is supported.
5. Determine how trade access and settlement standing are represented.
6. Determine whether merchant reputation already exists.
7. Determine whether route hazards are dynamic and owned by world/faction/weather systems.
8. Determine whether cargo reservations/escrow exist.
9. Determine whether guards can travel as canonical survivors.
10. Search for dormant route-management, supply-line, convoy, recurring-shipment, contract, agreement, tariff, escort, exclusive-trade, and route-upgrade code.
11. Create `docs/systems/PLAYER_TRADE_ROUTE_AUTHORITY_MAP.md`.
12. Create intake duplicate-search evidence linking Plan 131/E1-3, Plan 134 territory, Plan 155 black market, E1-10 outposts, debt/economy plans, and current caravan rails.
13. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19B — Player trade-route ownership ADR

**Goal:** Define trade routes as commercial contracts over world paths and canonical caravan/market operations.

### Required substeps

1. Write an ADR comparing a new `PlayerTradeRouteSystem`, extension of `TravelingCaravanSystem`, and a generic recurring-logistics contract framework.
2. Define route-owned fields: route contract ID, origin/destination settlement refs, path/route ref, lifecycle, establishment provenance, agreement refs, dispatch policy, accounting refs, warning/operation receipts.
3. Explicitly exclude cargo ownership, market prices, faction standing, route geometry, current hazard, guard health, caravan movement progress if already owned elsewhere, and combat result.
4. Define NPC versus player caravan coexistence.
5. Define one dispatch command boundary.
6. Define rollback/feature flags.
7. Require second-tool architecture review.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19C — Settlement identity and trade-access contract

**Goal:** Use canonical settlement/location/faction identity for both route endpoints.

### Required substeps

1. Define origin/destination settlement IDs as references to the world/economy settlement authority.
2. Validate both settlements exist.
3. Validate discovered/known status through world/knowledge rails.
4. Validate market/trade access.
5. Validate faction/TradeStance requirements.
6. Reject same-origin/destination route unless loop trade is deliberately supported.
7. Handle settlement destruction/abandonment/control changes.
8. Add endpoint-validation tests.
9. Do not copy settlement names or standings into route state.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19D — World-path reference and route geometry

**Goal:** Reference real traversable world paths instead of storing a private `distance`/hazard truth.

### Required substeps

1. Resolve path through world topology/travel authority.
2. Store stable path/route reference or enough endpoint/path policy to re-resolve safely.
3. Query current travel time, closures, weather, faction control, and hazard dynamically.
4. Do not store copied current hazard.
5. Allow authored commercial route ID to reference one of the 18 static route definitions if they are canonical.
6. Define behavior when world path changes after route establishment.
7. Add tests for valid path, closure, reroute, destroyed node, hostile-control change, and save/load.
8. Expose path breakdown in debug/read model.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19E — Route lifecycle state machine

**Goal:** Represent commercial route status independently of temporary path/caravan state.

### Required substeps

1. Define Candidate, Negotiating, Active, Suspended, Blocked, Closing, Abandoned, Invalidated where each state has concrete behavior.
2. Do not use one `isActive` boolean for all cases.
3. Temporary road closure should usually Block/Suspend rather than delete route.
4. Agreement expiration may suspend dispatch without destroying route history.
5. Settlement destruction may invalidate route.
6. Abandonment is explicit player/system action.
7. Add transition tests.
8. Keep lifecycle idempotent.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19F — Route-establishment proposal

**Goal:** Validate that the player can create a commercial route before spending resources or envoy time.

### Required substeps

1. Require both endpoints known and trade-accessible.
2. Require a valid path.
3. Require any minimum standing/TradeStance policy from canonical owner.
4. Require no conflicting exclusive agreement if market/faction system exposes one.
5. Determine envoy/negotiator requirement.
6. Determine establishment costs/commitments through inventory/currency.
7. Return blocker reasons.
8. Do not mutate state.
9. Add tests for unknown settlement, no path, hostile stance, insufficient cost, duplicate route, and valid proposal.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19G — Envoy/negotiator assignment

**Goal:** Use canonical survivor assignment/travel/social/diplomacy rails for establishment work.

### Required substeps

1. Verify where envoy must physically travel, if at all.
2. Assign survivor through Duty/Expedition/Travel authority.
3. Validate skill/role/health/availability.
4. Do not store a copied survivor record.
5. Handle interruption, injury, death, reassignment, destination closure, and save/load.
6. Use skill authority for negotiation competence.
7. Add tests for valid envoy, unavailable survivor, travel interruption, and return.
8. Keep establishment route state referencing survivor ID only while operation is active.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19H — Negotiation resolution policy

**Goal:** Resolve route establishment/agreement terms through existing faction/market diplomacy rather than an isolated random roll.

### Required substeps

1. Audit negotiation/diplomacy/branch APIs.
2. Prefer deterministic eligibility + explicit concessions.
3. If uncertainty exists, use canonical negotiation resolver or dedicated keyed decision with survivor/settlement/operation ID.
4. Standing/TradeStance remains canonical.
5. Do not create local trade reputation to boost success without ADR.
6. Persist decision/terms so reload cannot reroll.
7. Add tests for exact threshold, favorable/unfavorable stance, concession, failure, success, and reload.
8. Provide reason trace.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19I — Establishment cost transaction

**Goal:** Consume trade goods/currency exactly once when route contract reaches the appropriate commitment point.

### Required substeps

1. Define whether cost is upfront, escrowed, or consumed on successful negotiation.
2. Use canonical inventory/currency transaction.
3. Use stable operation ID.
4. Return reserved resources on cancellation according to policy.
5. Do not duplicate payment in settlement market.
6. Add cancellation/retry/save tests.
7. Assert currency/item conservation.
8. Record cost provenance for accounting.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19J — Trade agreement contract schema

**Goal:** Define route-specific commercial terms as typed policies referencing owner systems.

### Required substeps

1. Define agreement ID, partner settlement, route ID, agreement type/policy ID, effective period, renewal policy, settlement/faction authority ref, market-term policy refs, protection/intel policy refs, and lifecycle.
2. Do not store free-form `benefits` as gameplay authority.
3. Allow localized human-readable terms separately.
4. Validate each policy consumer exists.
5. Version agreement schema.
6. Add integrity tests.
7. Start with one or two agreement types.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19K — Supply-contract integration

**Goal:** Represent guaranteed/regular exchange through Market/contract settlement rules.

### Required substeps

1. Define item/category, quantity band, schedule, price policy or price-lock reference, tolerance, and breach behavior.
2. MarketSystem owns transaction pricing/stock settlement.
3. Inventory owns delivered/returned goods.
4. Route scheduler owns dispatch timing.
5. Do not mint guaranteed goods without settlement transaction.
6. Handle destination shortage/market closure.
7. Add tests for fulfilled, partial, failed, expired, and renewed contract.
8. Use commitment/debt system if obligations are unpaid.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19L — Tariff-reduction integration

**Goal:** Apply lower tariffs through canonical market/faction fee policy.

### Required substeps

1. Audit tariff/fee handling.
2. Agreement references discount policy or exemption ID.
3. MarketSystem calculates final fee.
4. Do not subtract a local route percentage afterward.
5. Define duration/eligibility.
6. Add tests for active, expired, stacked/conflicting tariff rules, and save/load.
7. Keep price ownership canonical.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19M — Exclusive-trade boundary

**Goal:** Prevent 'exclusive trade' from becoming an unsupported global monopoly switch.

### Required substeps

1. Audit whether settlements/markets model exclusive contracts or trader access restrictions.
2. If supported, route agreement registers with that authority.
3. If unsupported, reinterpret exclusivity narrowly: preferred buyer/seller rights for specified goods, quotas, or contract priority.
4. Do not disable NPC caravans from the route locally.
5. Do not block unrelated market actors without canonical policy.
6. Add tests for supported exclusivity, narrow priority, conflict, expiry, and revoked standing.
7. Feature-gate monopoly mechanics.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19N — Military-protection agreement integration

**Goal:** Translate settlement protection into canonical escort/security support rather than a local raid-risk subtraction.

### Required substeps

1. Audit faction escort, guard, route-security, or defense systems.
2. Agreement may grant escort units, patrol coverage, safe-passage tag, or security modifier through canonical authority.
3. Trade route queries security during travel-risk resolution.
4. Do not subtract `raidRisk` directly if world/encounter system owns it.
5. Define availability/limits.
6. Add tests for active protection, absent escort, hostile faction, expired agreement, and save/load.
7. Keep casualties/combat external.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19O — Intelligence-sharing agreement

**Goal:** Hand route-derived observations into E1-3 information flow without copying intel state.

### Required substeps

1. Agreement enables specific information source/permission.
2. Caravan journey may produce sightings/route status/trade intelligence events.
3. E1-3 owns propagation/knowledge.
4. Do not store faction intel in TradeRoute.
5. Do not let intel reveal hidden facts beyond agreement policy.
6. Add tests with information system enabled/disabled.
7. Use stable provenance.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19P — No-generic-trade-reputation ADR

**Goal:** Decide whether the proposed global trade reputation is redundant with faction/settlement standing.

### Required substeps

1. Audit all standing/reputation concepts.
2. Prefer settlement/faction standing plus route-contract performance history.
3. If a merchant-guild/product-wide reputation exists, reuse it.
4. If a new narrow trade reliability metric is required, define exact owner, scope, consumers, and distinction from standing.
5. Do not create a 0–100 global scalar by default.
6. Do not let route profit automatically improve political standing.
7. Add boundary tests.
8. Document decision before implementation.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19Q — Route performance/reliability history

**Goal:** Track commercial performance as auditable history without replacing faction standing.

### Required substeps

1. Record on-time delivery, partial fulfillment, missed contract, breach, successful settlement, route closure, and loss events.
2. Use these as inputs only where agreement/faction policy explicitly consumes them.
3. Keep history bounded/aggregated.
4. Do not store a second relationship/reputation score unless ADR approves it.
5. Use operation IDs for dedupe.
6. Add tests for repeated delivery and breach.
7. Expose summary in UI.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19R — Caravan runtime ownership decision

**Goal:** Reuse the existing caravan/travel runtime for player-owned caravans wherever possible.

### Required substeps

1. Inspect `TravelingCaravanSystem` extensibility.
2. Determine whether owner/provenance can distinguish NPC versus player/contract caravans.
3. Reuse movement progress, arrival, delay, travel time, and save contract if architecture permits.
4. If existing NPC system is too specialized, extract a shared caravan runtime rather than duplicate movement logic.
5. Do not create two different caravan physics/travel clocks.
6. Define player dispatch metadata separately.
7. Add parity tests for NPC caravans after refactor.
8. Require architecture review before new `PlayerCaravan` runtime.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19S — Caravan identity and operation ID

**Goal:** Give every dispatched caravan/journey stable provenance through save/load and market settlement.

### Required substeps

1. Define caravan runtime ID plus dispatch operation ID.
2. Reference route contract, origin, destination, world path, vehicle/caravan asset, guard group, cargo reservation/manifest ID, agreement snapshot/ref, departure time, and settlement instruction ID.
3. Do not copy item objects.
4. Do not copy survivor health.
5. Define ownership/provenance as player trade route.
6. Add uniqueness/save tests.
7. Use operation ID across encounter/market/accounting events.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19T — Cargo reservation and manifest

**Goal:** Reserve real inventory cargo before dispatch and transfer ownership safely.

### Required substeps

1. Create manifest from canonical inventory item IDs/quantities/stacks.
2. Validate trade eligibility and reserved/locked state.
3. Reserve or move cargo into canonical caravan cargo container.
4. Do not duplicate manifest quantities as authoritative inventory.
5. Preserve item metadata/condition.
6. Handle cancellation before departure.
7. Handle partial capacity.
8. Add conservation tests.
9. Use manifest reference in route state.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19U — Caravan capacity boundary

**Goal:** Derive capacity from canonical vehicle/caravan/equipment authority rather than agreement magic.

### Required substeps

1. Audit caravan/vehicle cargo capacity.
2. Agreement may allow larger permitted shipment quota but cannot physically enlarge vehicle capacity.
3. Route upgrade/waystation may affect logistical throughput through real infrastructure.
4. Do not store one route `caravanCapacity` that overrides asset capacity.
5. Add tests for over-capacity, agreement quota, vehicle upgrade, and multiple vehicles.
6. Keep capacity explanation in UI.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19V — Guard assignment and survivor state

**Goal:** Assign guards through canonical roster/travel/combat systems.

### Required substeps

1. Validate survivor availability, health, skills, equipment, consent/policy.
2. Move them into caravan/travel state through canonical authority.
3. Do not clone survivors into route DTO.
4. Ensure they are absent from shelter duties while traveling.
5. Handle injury/death/missing/capture/return through health/combat/lifecycle.
6. Add tests for duplicate assignment, death, injury, route cancellation, and save/load.
7. Use guard group reference.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19W — Manual dispatch transaction

**Goal:** Create one caravan journey only after cargo, guards, vehicle, path, market, and agreement checks pass.

### Required substeps

1. Validate route active/not blocked.
2. Validate agreement/market access.
3. Validate cargo reservation.
4. Validate caravan asset/vehicle.
5. Validate guard assignment.
6. Validate path.
7. Validate destination settlement/market.
8. Create stable dispatch operation.
9. Commit cargo/assignments exactly once.
10. Hand journey to canonical caravan runtime.
11. Add double-click/retry/save tests.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19X — Caravan schedule policy

**Goal:** Represent recurring dispatch intent without auto-generating resources.

### Required substeps

1. Define schedule ID, route ID, cadence, next due time, cargo policy ref, guard policy ref, vehicle policy ref, enabled state, and blocked-reason tracking.
2. Use campaign time.
3. Do not store copied cargo.
4. Do not dispatch if prerequisites fail.
5. Advance next due deterministically according to policy.
6. Define missed dispatch behavior: skip, delay, or wait-until-ready.
7. Add schedule tests.
8. Start with one recurring schedule per route.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19Y — Cargo selection policy for auto-dispatch

**Goal:** Make automatic cargo choice explicit, bounded, and inventory-safe.

### Required substeps

1. Options may be fixed manifest, category/quantity target, surplus-above-reserve policy, or contract-required manifest.
2. Do not auto-sell critical reserves without configured policy.
3. Use inventory reservation and stock floors.
4. Market demand/prices may inform recommendations but should not silently churn inventory unless automation policy is explicit.
5. Add tests for shortage, reserved stock, item removed, full manifest, and changed contract.
6. Expose policy in UI.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19Z — Guard/vehicle policy for auto-dispatch

**Goal:** Prevent schedules from stealing critical survivors or vehicles unexpectedly.

### Required substeps

1. Define fixed assigned asset, pool, minimum guard strength, or manual-only policy.
2. Use canonical availability at dispatch time.
3. Do not preempt emergency duties without Duty/Governance policy.
4. Do not dispatch unfit survivors.
5. Block safely and notify.
6. Add tests for unavailable guards, damaged vehicle, emergency duty, and restored availability.
7. Keep automation transparent.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AA — Arrival and destination-market settlement

**Goal:** Settle cargo only through MarketSystem after real arrival.

### Required substeps

1. On canonical arrival, create market settlement transaction using manifest and agreement terms.
2. MarketSystem calculates accepted goods, prices, tariffs/fees, destination stock effects, and currency/return goods.
3. Do not calculate route profit before settlement.
4. Handle partial sale/rejection.
5. Handle market closure/standing change at arrival.
6. Create stable settlement transaction ID.
7. Add idempotency/save tests.
8. Return result to accounting/read model.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AB — Return cargo and return journey

**Goal:** Represent return goods/currency without teleporting value to the shelter unless the design explicitly treats settlement as final payout.

### Required substeps

1. Decide whether currency is credited at destination immediately or physically transported; align with existing economy abstraction.
2. Return goods require canonical caravan cargo and return travel.
3. Do not double-count sold cargo and return goods.
4. Use same or new journey operation with explicit provenance.
5. Handle raid on return leg.
6. Handle stranded/abandoned caravan.
7. Add conservation tests.
8. Document abstraction.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AC — Profitability accounting ledger

**Goal:** Compute route profitability from actual transactions and costs rather than a mutable profit scalar.

### Required substeps

1. Record route-attributed purchase cost, sale revenue, tariffs/fees, caravan operating costs if canonical, losses, repair/escort costs where attributable, and realized return value.
2. Do not estimate unsold inventory as realized profit unless mark-to-market is explicitly a separate metric.
3. Compute lifetime/period profit as derived ledger summary.
4. Use transaction IDs for dedupe.
5. Store compact immutable accounting entries or references.
6. Add reconciliation tests against economy ledger.
7. Define route-loss metric similarly.
8. Expose rolling period summaries.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AD — Network value read model

**Goal:** Derive network value rather than storing `totalTradeNetworkValue` as authoritative state.

### Required substeps

1. Define what value means if displayed: realized cumulative profit, active contract value, cargo-in-transit value, or market opportunity.
2. Prefer multiple labeled metrics over one ambiguous score.
3. Compute from canonical accounting, cargo, and contracts.
4. Do not use UI metric as reward/economy authority.
5. Add consistency tests.
6. Use in quests only if semantics are stable.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AE — Dynamic route risk query

**Goal:** Resolve current journey risk from world, faction, weather, information, security, and caravan state.

### Required substeps

1. Query path hazard from world route segments.
2. Query faction hostility/control.
3. Query weather.
4. Query known/unknown risk through E1-3 where appropriate for player UI.
5. Query escort/security coverage.
6. Query caravan value only as an encounter-interest factor if combat/encounter policy supports it.
7. Do not store a stale `hazardLevel` as current truth.
8. Return actual risk and known player estimate separately.
9. Add tests for changing territory/weather/security.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AF — Raid encounter decision

**Goal:** Trigger caravan raids through canonical travel/encounter/faction policy with deterministic keyed decisions.

### Required substeps

1. Use journey/segment/encounter-window operation key.
2. Do not reroll on load.
3. Do not roll every tick.
4. Faction hostility and route security enter the canonical encounter policy.
5. Caravan cargo value may influence targeted-raider event eligibility.
6. Persist encounter decision/result provenance.
7. Allow no encounter.
8. Add distribution and no-reroll tests.
9. Keep random generator partitioned.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AG — Raid/combat handoff

**Goal:** Resolve attack outcomes through existing combat/defense authority.

### Required substeps

1. Create combat/encounter participants from canonical guards/raiders.
2. Pass caravan/cargo context.
3. Combat owns injuries/deaths.
4. Inventory/combat loot resolution owns cargo loss.
5. Trade route records result refs only.
6. Do not calculate `repelled/lost/heavy_losses` from a local guard-power formula if combat authority exists.
7. Add tests for no guards, strong guards, retreat, casualties, total loss, partial cargo loss, and save/load.
8. Use stable correlation ID.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AH — Cargo-loss reconciliation

**Goal:** Ensure raid/theft/destruction removes real cargo exactly once.

### Required substeps

1. Use canonical inventory container/loot transaction.
2. Record lost/destroyed/recovered quantities by transaction ID.
3. Accounting consumes actual loss transaction.
4. Do not maintain a second `cargoLost` list as item owner.
5. Handle partial stacks/item metadata.
6. Add conservation tests.
7. Prevent reload duplication.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AI — Caravan-loss lifecycle

**Goal:** Reconcile vehicle, guards, cargo, route schedule, contract obligations, and history when a caravan is lost.

### Required substeps

1. Caravan runtime marks journey outcome.
2. Vehicle authority resolves vehicle loss/damage.
3. Health/lifecycle resolves guards.
4. Inventory resolves cargo.
5. Contract/commitment layer records missed delivery/breach where applicable.
6. Route remains unless policy suspends it.
7. Scheduler does not immediately spawn a free replacement.
8. Add tests for total loss, survivor returns without cargo, vehicle destroyed, agreement breach, and next schedule.
9. Record significant event once.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency transactions.
- Schedules and encounter decisions are deterministic/idempotent across save/load.

### Negative tests

- TradeRoute stores copied current market prices or world hazard.
- PlayerCaravan cargo becomes a second authoritative inventory list.
- A raid is resolved by a local guard-power formula while combat authority exists.
- A global trade reputation duplicates faction standing.
- Route upgrade locally subtracts travel time instead of using world infrastructure.
- Reload dispatches/sells the same cargo twice.
- Auto-dispatch creates cargo or steals reserved survival stock contrary to policy.
- NPC caravan behavior regresses after player-route integration.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Cargo/currency conservation passes.
- [ ] Travel/market settlement idempotency passes.
- [ ] Agreement/faction/debt tests pass.
- [ ] Raid/combat handoff passes.
- [ ] Schedule/time-skip tests pass.
- [ ] Economy/performance/micromanagement evidence is captured.

---

## E1-19AJ — TradeStance and faction-access integration

**Goal:** Use canonical political/trade relationships for establishment, dispatch, arrival, and renewal.

### Required substeps

1. Validate stance during route proposal.
2. Revalidate material access changes at dispatch/arrival where appropriate.
3. Do not cache standing as route truth.
4. Define grandfathered agreement behavior if faction stance worsens.
5. Allow suspension/revocation through canonical policy.
6. Add tests for neutral->friendly, friendly->hostile, treaty, embargo, and restored access.
7. Use reason codes.

### Trade-route invariants

- World route graph owns path, distance, closure, and hazard truth.
- MarketSystem owns prices and trade settlement.
- Inventory owns cargo; guards remain canonical survivors.
- Caravan/travel authority owns movement and arrival.
- Faction/TradeStance owns political trade access; no duplicate generic reputation.
- Combat/raid authority owns casualties and cargo-loss resolution.
- Route accounting reconciles to actual market/inventory/currency tr

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
