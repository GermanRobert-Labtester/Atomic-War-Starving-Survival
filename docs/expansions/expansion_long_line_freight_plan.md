# ASHFALL — THE LONG LINE: FREIGHT
### Run a caravan company · From a signed contract to a house that people wait for

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/long-line-freight-2026-09-29.md`
**Family:** "The world moves without you" — see `docs/expansions/expansion_world_moves_without_you_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. Specificity over adjectives. No magic, no real countries, wars, people, or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY** as in the Year Two bible.

> **Name collision — read first.** `docs/expansions/expansion_11_the_long_line_creative_pack.md` already uses "The Long Line" for a *civil-defence telephone trunk* (communication as infrastructure), and `EXPANSIONS_MASTER_CATALOG.md` lists an Expansion 11 with a `long_line` save section (no `long_line` section is registered in `SaveSectionRegistry.cs` today). That pack is *"creative proposal only — not canonical"*, and no `Ashfall.Core.LongLine` directory exists today. This document is the **freight** reading of the same phrase the user chose. To keep ids unambiguous every artifact here is `long_line_freight` / `LF-`. **DEC-LF-01** asks the foreman whether to keep both under a shared subtitle scheme, rename this one, or merge the two (the copper line and the road are one idea: *who is allowed to be reached, and who gets delivered to*).

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

The shelter can already sign a trade route. It pays a tariff, and once the cadence comes round the route is recorded as **on time**. It has never been late for a reason, because nothing can go wrong: there is no wagon to break, no driver to lose, no road to close. The trade contract is a subscription, not a business.

The Long Line: Freight turns the subscription into a company. The player's shelter becomes a **House** — a name on the side of a wagon — with rolling stock, crews, depots, a reputation with the people at both ends of a route, and a ledger that says, in the player's own accounting, what they owe and who is owed. The fantasy is not "be rich". It is **be the one who keeps the road open**: to be the reason a settlement three days east still has salt in a bad month, and to know it, because a driver's letter tells you.

### 1.2 Pillars

1. **A run is a journey, not a debit.** Every run has legs, weather, war, a crew, and a cargo that can arrive short.
2. **People are the vehicle.** Wagons break; drivers get sick, get hurt, get tired, get married, quit. Crews are survivors and hired hands owned by the existing survivor/duty systems.
3. **The ledger is the story.** Every run writes one line. A season of lines reads like a novel.
4. **The market answers.** A house that floods one good drives its price down and its own premium down with it (the existing saturation rules).
5. **Depots are places you already have.** Outposts and waystations are the network's nodes; custody of them does not change.
6. **Ship dark.** Existing route contracts keep working exactly as today until a Charter exists.

### 1.3 Not this

Not a logistics spreadsheet. Not a second economy. Not a fleet-management game. Not a replacement for the existing caravan NPCs, who remain the region's other travellers.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | Player route contracts: id, counterparty, cadence, base tariff, slots required, exclusive good, reliability score, runs completed/failed, goods out/in, suspend/cancel. | `Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs`, `TradeRouteContract.cs`; section `trade_routes` | LIVE |
| F2 | **A "run" is a tariff debit.** `TradeRouteHostSession.TickDay` debits the tariff and calls `RecordRunOutcome(..., OnTime, ...)`; on insufficient funds it records `Late`. It never rolls risk, never uses crews, and — by grep — `GoodsOut`/`GoodsIn` have **no consumer** outside the contract and its own system, so the goods legs are recorded data, not moved stock. | `src/Host/TradeRouteHostSession.cs` `TickDay` (L198–232); called daily from `src/Main.TradeRoutes.cs` L46 | **GAP (the central one)** |
| F3 | A transit-risk evaluator exists (raid, disruption, cargo attrition, escort mitigation, viability, summary) but the daily tick does **not** consult it; it is reachable through `EvaluateContractRisk` and the `--trade-route-risk-selftest`. | `Assets/Ashfall.Core/Economy/TradeRouteRiskBindingEngine.cs`; `TradeRouteHostSession.EvaluateContractRisk` (L60–78) | GAP (LIVE engine, unwired) |
| F4 | Saturation: +25% base exclusive premium, 2.5% saturation per unit, 5%/day recovery, ≤5 exclusive units/run, 3 units per slot. | `TradeRouteMonopolyEngine.cs` | LIVE (VERIFY tick wiring) |
| F5 | 10 authored routes, 4 origin/destination regions from the supply-tag set, `travel_days` 4–12, `base_risk_permille` 90–340, **`season_end_day` 280–360**. | `caravan_trade_routes.json` | LIVE (data) — season ends at Day ≤360 |
| F6 | NPC caravans (two catalogs, 4 each) arrive on schedules with manifests, stocks, guard strength, hazard outcomes and disruptions; separately persisted. | `caravans.json`, `merchant_caravans.json`; `CaravanTradeNetworkSystem.cs`; `TravelingCaravanSystem.cs`; section `caravan_trade_network` | LIVE |
| F7 | Weather embargoes block or slow routes and add decaying price shocks. | `trade_embargoes.json`; `TradeEmbargoSystem.cs` | LIVE |
| F8 | A vehicle seam (`vehicles.json`, modules, modifications, armor grades) exists for expeditions. | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`; `vehicles.json`, `vehicle_modules.json`, `vehicle_armor_grades.json` | LIVE (VERIFY whether a *wagon* archetype fits or a new *entry* in the same catalog is needed) |
| F9 | Credit and debt exist: `TradeCreditCoordinator`, `LoanSharkEnforcerEngine`, `FundsLedger` (with `ReasonRouteTariff`). | `Assets/Ashfall.Core/Economy/` | LIVE |
| F10 | Mercenaries exist as an authority (contracts, bounties). | `Assets/Ashfall.Core/Economy/MercenarySystem.cs` | LIVE (VERIFY whether *escort hire* is a supported contract type) |
| F11 | Outposts/waystations (custody signed and separate) can act as read-only nodes. | Year Two umbrella F10; `WaystationNetworkSystem.cs` | LIVE |
| F12 | Selftest verbs: `--trade-route-selftest`, `--trade-routes-selftest`, `--trade-route-risk-selftest`, `--caravan-selftest`, `--traveling-caravan-selftest`. | `src/Host/HostCli*.cs` | LIVE |
| F13 | Route `season_end_day` values end the network before Year Two even begins; Year Two P1 lifts the campaign horizon but the route catalog does not follow. | F5 + Year Two umbrella P1 | GAP (cross-plan) |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **F2 vs the promise.** The system says a run always succeeds. **Systems win:** the plan does not invent a parallel run system; it *adds the missing resolution step* into the existing `TickDay` path — one authority (`PlayerTradeRouteSystem` + its host session), now consulting the risk engine that already exists but isn't called.
- **F13.** Route seasons end at 360; the company must not silently die on the day Year Two starts. A *season table per route per year* is data, not code: `season_end_day` becomes a per-year read (DEC-LF-06).

---

# PART II — THE STORY

## 3. The Line

"The Line" is what the drivers call the road when they are tired of saying route names. It is not a place. It is the sum of every wagon on the map; if it is running, salt gets east. The Line has a rung ladder for the shelter's part in it.

### 3.1 The four rungs

| Rung | Paper | What the shelter is | Gate |
|---|---|---|---|
| **Contract** | One route, one counterparty. | A customer of the road. | Today's game. |
| **Charter** | A dated paper with a counterparty seal. Two routes; a house mark. | A carrier. Must run **one wagon** and **one crew** of its own. | Reliability Tier 2 on one route + a signed depot |
| **House** | A hall's name on the wagon, an exclusive good. | A house with two crews, hired escorts, a depot. | Reliability Tier 3 on two routes |
| **Line** | The company is a route other houses subcontract. | Others depend on it. | Tier 4 on one route + year-end audit |

Reliability tiers 1–4 already exist. The rung is *derived* from them plus owned assets; nothing new is stored except what a derivation cannot recompute.

### 3.2 A run, start to finish

1. **Manifest.** The player assembles cargo (from stock or purchase) and picks a **crew**, a **wagon** and an optional **escort**. The manifest preview shows the existing risk summary: raid probability, disruption, attrition, escort mitigation.
2. **Departure.** The run leaves on its cadence day. Stock leaves the shelter's inventory (the existing inventory owner).
3. **Legs.** Each day of `travel_days` is a **leg**. On a leg the run reads: weather (embargo/weather-gate), war (regional tension via *The Living Region*), sickness (quarantine via *The Plague Year*), road state (bridges, ferry). A leg may be quiet; a leg may raise a **wayside event** — a broken axle, a checkpoint, a stranded family, a bribe.
4. **Arrival.** The run resolves to the existing outcomes — **On time / Late / Failed** — plus cargo delivered (short or whole). Goods in return are added to stock. The tariff and income post to the funds ledger under existing reasons.
5. **The line in the book.** One line in the ledger: date, route, driver, weather, what was lost, what was said.

### 3.3 Rolling stock

- **Wagon** — an entry in the existing vehicle catalog family (DEC-LF-04). Attributes: load slots, wear, armor grade, a name. Wear is spent per leg; repair is a workshop job.
- **Draught** — carrier stock (mule, ox, motor) depending on fuel and feed: a real daily upkeep in the shelter's stores when the run is at home.
- **Marks** — the house mark on the wagon; NPC caravans and settlements recognise it. Marks are presentation over the existing standing record.

### 3.4 Crews

Crews are **survivors** and **hired hands**. A driver is a survivor with a role for the run (the existing duty-roster/assignment authority decides their availability). A run without a driver does not leave. Injuries and deaths are recorded through the survivor legacy/death systems already present.

The crew is where the story lives: **Marta** (drives the east run; writes home, or doesn't), **Ilya** (fixes anything; will not go near the river), **the hired escort** who asks, on the third day, whether she will be paid if the run fails. The point is not the names — it is that the crew can be lost, and that the player will remember who.

### 3.5 Depots and relays

Outposts and waystations become **depot nodes**: a run may rest, reload, hide, or be resupplied at a node the shelter already holds. This is a read-only relationship — the depot's custody and saves are untouched. A depot *shortens a leg's risk* and *extends a run's endurance*; it costs upkeep the outpost already pays.

### 3.6 The market answers

- Exclusive goods saturate; premium falls as the house floods. The house learns to **rotate goods** instead of ordering more.
- A route the house runs reliably draws **rivals** among the existing NPC caravans: the same faction's wagon appears a day earlier, with a slightly better price.
- A route the house *stops* running is felt: the counterparty's shortage shows on the Board (Living Region) and in the price.

### 3.7 Debt and consequence

Credit lines and the loan-shark enforcer already exist. A house that borrows to buy a wagon, and then loses two runs in a row, must decide what to give up: the wagon, the exclusive, or a person. The game never chooses.

### 3.8 The Ledger (the surface)

The company surface is the **existing trade-route screen** made honest: manifests, crews, wagons, and the ledger's last ten lines. It is not a new panel (DEC-LF-08).

## 4. The routes (ten, existing) — the story of each

The plan does **not** author new routes in v1. It gives each of the ten a voice.

| Route | The feel of the road | What breaks it |
|---|---|---|
| Compact Supply Line (settlement → iron basin, 6 d) | Best-kept road; boring; the training run. | A garrison changing hands. |
| Compact Iron Corridor (belt → settlement, 8 d) | Rail-side; the freight *sounds* like an occupation. | Rail cuts, tolls. |
| Compact Grain Express (settlement → ash flats, 5 d) | Short and dear; one hard river. | Weather; the ferry. |
| Scale Salt Artery (ash flats → settlement, 7 d) | Salt: life and leverage. | Embargo, the Scale's own price hand. |
| Scale Mercantile Circuit (settlement → deep coast, 10 d) | The long, honest run. | Coast weather; harbour tolls. |
| Scale Chemical Exchange (belt → ash flats, 6 d) | Dangerous cargo; the crew knows it. | Contamination; spills. |
| Flotilla Coastal Drift (deep coast → settlement, 9 d) | Half by water; half by luck. | Tide (*The Drowned Coast*). |
| Flotilla Salvage Run (deep coast → belt, 12 d) | Worth the most; loses the most. | Everything. |
| Flotilla Smuggler's Deep (deep coast → ash flats, 4 d) | Fast, exposed. | Patrols. |
| Compact Frontier Convoys (settlement → deep coast, 11 d) | Escorted; slow; safe until it is not. | A Tempest. |

## 5. Voice samples

- *Ledger, on time:* "Day 152. East run. Salt out, grain back. Marta drove. Two axle clamps replaced at the ferry. Nothing lost."
- *Ledger, failed:* "Day 173. Chemical exchange. We lost the crate on the second leg and the driver did not come back. The counterparty sent a note about the crate."
- *Driver's letter:* "Ilya says the wheel will hold. Ilya says that about everything."
- *Rival:* "A Scale wagon passed us on the third day. Somebody had painted over the old mark."

## 6. The year the line has to survive

Year One: the company is a habit. Year Two (see the Year Two bible): supply for the second shelter and waystations is a *client* of the house — the shelter can hire its own house for the convoy legs (the Year Two P6 convoy mode is the *demand*; this plan is one possible *supply*). Neither plan owns the other.

## 7. Content plan

- **W1 — Ledger lines:** 10 routes × 3 outcomes × 3 registers = 90 ledger lines.
- **W2 — Wayside events:** 24 events (breakdown ×6, checkpoint ×6, refugees ×4, bribe ×4, weather ×4).
- **W3 — Crew:** 12 authored driver/escort types with three letters each (36).
- **W4 — Rivals & marks:** 6 rival houses with mark descriptions; 20 rival lines.

## 8. Non-goals

No new economy. No new save section (nested in `trade_routes`, DEC-LF-03). No real-time convoy minigame. No new panel. No new routes in v1. No merging with Expansion 11 (DEC-LF-01 decides).

## 9. Risks

- **The tick becomes a simulation.** Bound: one deterministic resolution per run, at arrival (legs are *summarised*, not stepped, unless a wayside event triggers).
- **Runs break old saves.** Bound: contracts without a Charter continue on the old on-time path bit-for-bit (ship-dark parity).
- **Crews become micromanagement.** Bound: one driver + optional escort per run in v1.
- **Name confusion with Expansion 11.** Bound: `long_line_freight` ids and DEC-LF-01.
