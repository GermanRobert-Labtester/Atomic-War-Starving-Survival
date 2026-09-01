# Plan 192 — Player Trade Route Establishment

## Goal

Create a player trade route establishment system where players can establish, manage, and defend permanent trade routes between settlements, dispatch caravans on schedules, negotiate trade agreements, and build a trade network. Currently `TravelingCaravanSystem.cs` (268 lines) runs NPC caravans on pre-set routes that players can buy from when they arrive, but players cannot establish their own routes, dispatch caravans, or manage trade networks. `TradeCaravanCatalog.cs` (115 lines) has 18 static route definitions but they're not wired to player actions. The `trade_route_disrupted` feedback message exists but is never fired — orphaned text anticipating a feature never built. This plan transforms players from passive trade recipients into active trade network builders.

## Why

**Repository evidence:** Grep for `EstablishRoute`, `CreateTradeRoute`, `SendCaravan`, `TradeNetwork`, `PlayerTradeRoute` in Core returns ZERO matches. `TravelingCaravanSystem.cs` (268 lines) has `SpawnCaravan`, `DailyTick`, `TryBuyItem` — all NPC-focused. Player can only buy from passing caravans. `TradeCaravanCatalog.cs` (115 lines) has 18 route entries with `route_id`, `origin_hub`, `destination_hub`, `travel_days`, `hazard_index`, `primary_cargo_manifest` — static data not connected to live gameplay. Feedback message `trade_route_disrupted` is defined but never fired by any system.

**What is missing:** No player ability to establish trade routes. No caravan dispatch mechanic. No trade route network management. No route defense/raiding. No trade agreement negotiation. No route profitability tracking. No caravan scheduling. Players are passive recipients of NPC trade, not active traders.

**Why existing plans don't solve it:** Plan 131 (information network) mentions "trade route established" as a rumor source but doesn't implement route establishment. Plan 155 (black market) adds underground trade but not route management. Plan 134 (faction territory) adds supply lines but faction-controlled, not player-controlled. No plan addresses player trade route establishment.

**Player value:** Creates strategic depth (build profitable trade networks), adds economic gameplay (route management, defense, negotiation), generates emergent stories (caravan raids, trade wars, monopoly building), and makes economy feel player-driven rather than NPC-driven.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — NPC caravan system
- `Assets/Ashfall.Core/Narrative/TradeCaravanCatalog.cs` — static route data
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — market system
- `Assets/Ashfall.Core/Economy/TradeStance.cs` — trade stances
- NEW: `Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs`
- NEW: `Assets/StreamingAssets/Data/trade_route_agreements.json`

## Main Task 1 — Foundation / System Contract

1. Create `PlayerTradeRouteSystem.cs` in `Assets/Ashfall.Core/Economy/`
2. Define `TradeRoute` DTO: `routeId`, `routeName`, `originSettlementId`, `destinationSettlementId`, `distance` (travel days), `hazardLevel` (0-100), `establishedDay`, `establishedBySurvivorId`, `isActive` bool, `lastCaravanDay`, `caravanFrequency` (days between caravans), `routeProfit` (accumulated profit), `routeLosses` (accumulated losses from raids)
3. Define `PlayerCaravan` DTO: `caravanId`, `routeId`, `departureDay`, `arrivalDay`, `cargo` (list of item_ids with quantities), `guardSurvivorIds` (list), `caravanValue` (total cargo value), `status` (en_route/arrived/raided/lost), `raidRisk` (0-100)
4. Define `TradeAgreement` DTO: `agreementId`, `routeId`, `agreementType` (supply_contract/exclusive_trade/tariff_reduction/military_protection), `partnerSettlementId`, `terms` (description), `duration` (days), `benefits` (list), `status` (active/expired/cancelled)
5. Define `RouteRaid` DTO: `raidId`, `routeId`, `caravanId`, `raidDay`, `raiderFactionId`, `raidOutcome` (repelled/losted/heavy_losses), `cargoLost` (list), `guardCasualties` (list), `playerLoss` (value)
6. Define `PlayerTradeRouteState` DTO: list of trade routes, list of player caravans, list of trade agreements, list of route raids, total trade network value, trade reputation (0-100)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define route establishment mechanics:
   - Player must discover both settlements (expedition/quest)
   - Player must have trade agreement with both settlements (standing requirement)
   - Player must assign survivor to establish route (envoy/negotiator)
   - Establishment takes time (distance + negotiation days)
   - Establishment costs resources (trade goods, currency)
   - Success chance based on survivor skill + settlement relations
9. Define caravan dispatch mechanics:
   - Player assigns cargo to caravan
   - Player assigns guard survivors (protection from raids)
   - Caravan departs on schedule
   - Travel time based on route distance
   - Raid risk based on route hazard level
   - Arrival triggers trade at destination
   - Return caravan brings profit + return goods
10. Define trade agreement types:
    - **Supply Contract**: regular resource exchange, guaranteed prices
    - **Exclusive Trade**: only player can trade on this route, higher profits
    - **Tariff Reduction**: reduced trade taxes, better margins
    - **Military Protection**: settlement provides guards, reduced raid risk
    - **Intelligence Sharing**: trade routes provide intel (Plan 131 integration)
11. Define raid mechanics:
    - Caravans have raid risk based on route hazard
    - Raiders attack based on faction hostility + route value
    - Guard survivors defend caravan (combat resolution)
    - Raid outcomes: repelled (no loss), lost (total cargo loss), heavy losses (partial loss)
    - Raid events logged
    - Player can increase guards to reduce loss risk
12. Define route profitability:
    - Profit = cargo value at destination - cargo value at origin - transport costs
    - Profit margin based on trade agreement benefits
    - Route profit accumulates over time
    - Unprofitable routes can be abandoned
    - Profitable routes attract raids (higher value = higher risk)
13. Define trade reputation:
    - Successful trades increase reputation
    - Failed trades (raids, broken agreements) decrease reputation
    - High reputation: better agreements, lower tariffs
    - Low reputation: worse agreements, higher tariffs
    - Reputation affects settlement willingness to trade
14. Add deterministic seeding: caravan raids use `ISeededRng`
15. Wire into `GameBootstrap`: `SetupPlayerTradeRoutes`, `TickPlayerTradeRoutes`, `SavePlayerTradeRoutes`

## Main Task 2 — Implementation / Routes / Caravans / Agreements / Raids / UI

1. Implement route establishment:
   - Player selects two settlements with trade access
   - Player assigns envoy survivor
   - Establishment process takes days
   - Success check based on skill + relations
   - Route created on success
   - Establishment event logged
2. Implement caravan dispatch:
   - Player selects route and cargo
   - Player assigns guards
   - Caravan departs on schedule
   - Travel progress tracked
   - Raid check during travel
   - Arrival triggers trade
   - Return caravan with profit
3. Implement trade agreements:
   - Player negotiates with settlement
   - Agreement terms defined (duration, benefits)
   - Agreement requires standing/reputation
   - Agreement active for duration
   - Agreement benefits applied to route
   - Agreement expiration/renewal
4. Implement raid system:
   - Raid chance calculated per caravan
   - Raid triggered by faction/raiders
   - Combat resolution (guards vs raiders)
   - Raid outcome determined
   - Cargo losses applied
   - Guard casualties tracked
   - Raid event logged
5. Implement route management:
   - Route list viewable in UI
   - Route status (active/inactive/abandoned)
   - Route profitability tracked
   - Route hazard level displayed
   - Route can be upgraded (better roads, checkpoints)
   - Route can be abandoned if unprofitable
6. Implement caravan scheduling:
   - Player sets caravan frequency (weekly, biweekly, monthly)
   - Caravans dispatch automatically on schedule
   - Player can manually dispatch extra caravans
   - Caravan capacity based on trade agreement
   - Caravan priority (which goods to transport first)
7. Implement trade network UI:
   - Route map: show all established routes
   - Route detail: profitability, hazard, agreements
   - Caravan dispatch panel: assign cargo, guards
   - Agreement panel: negotiate, view, renew
   - Trade ledger: profit/loss history
   - Raid log: raid history
8. Implement route upgrades:
   - Player can invest in route infrastructure
   - Upgrades: road improvements, guard posts, waystations
   - Upgrades reduce hazard level
   - Upgrades reduce travel time
   - Upgrades increase caravan capacity
9. Implement trade reputation system:
   - Reputation tracked per settlement
   - Successful trades increase reputation
   - Failed trades decrease reputation
   - Reputation affects agreement terms
   - Reputation displayed in UI
10. Create trade route events:
    - "The Route" — trade route established
    - "The Caravan" — caravan dispatched
    - "The Arrival" — caravan arrived successfully
    - "The Raid" — caravan raided
    - "The Loss" — caravan lost
    - "The Agreement" — trade agreement signed
    - "The Profit" — route profitable
    - "The Network" — trade network expanded
11. Add trade route quest hooks:
    - "The Merchant" — establish 5 trade routes
    - "The Convoy" — dispatch 20 caravans safely
    - "The Negotiator" — sign 10 trade agreements
    - "The Defender" — repel 5 caravan raids
    - "The Monopoly" — exclusive trade on 3 routes
    - "The Network" — build trade network worth 1000
    - "The Reputation" — reach max trade reputation
12. Implement trade tutorial: first trade agreement explains system
13. Add trade tooltips: hover over route shows profitability, hazard
14. Create trade route definitions in data file
15. Implement trade persistence: routes/caravans/agreements saved

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `TravelingCaravanSystem`: player caravans coexist with NPC caravans
2. Connect to `MarketSystem`: trade route cargo affects market prices
3. Integrate with `TradeStance`: agreements require specific stances
4. Connect to `FactionBranchCoordinator`: raids tied to faction hostility
5. Wire into `ExpeditionSystem`: expeditions discover new trade settlements
6. Connect to `LedgerDebtSystem`: trade debts affect agreements
7. Implement old-save compatibility: existing saves get empty trade network
8. Add deterministic seeding: raids use `ISeededRng`
9. Create exploit prevention: raids are risk-based, can't be gamed
10. Add tests: route establishment, caravan dispatch, agreements, raids, profitability, save round-trip
11. Verify all route types work correctly
12. Test edge cases: no routes (empty network), many routes (complex network)
13. Verify headless behavior: trade routes process correctly without UI
14. Add data-integrity-selftest: trade routes validate against settlement/item catalogs
15. Create `--player-trade-route-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --player-trade-route-selftest
```

## Risk

**LOW** — Trade routes are straightforward with clear inputs (cargo, guards, agreements) and outputs (profit, raids). Risk of trade management becoming tedious. Mitigation: auto-dispatch options, clear profitability indicators, raid warnings, and ensure trade feels rewarding not burdensome.

## Definition of Done

- `PlayerTradeRouteSystem.cs` exists with full `CaptureState/RestoreState`
- Route establishment mechanic (discovery + agreement + envoy)
- Caravan dispatch system (cargo, guards, scheduling)
- 4 trade agreement types (supply, exclusive, tariff, protection)
- Raid mechanics (risk, combat, outcomes, losses)
- Route profitability tracking
- Trade reputation system
- Route upgrades (infrastructure investment)
- Trade route events and quest hooks
- Save/load round-trip tested
- Deterministic raids verified
- Old saves load with empty trade network
- Trade route definitions in data authority
- UI route map, dispatch panel, agreement panel, trade ledger
- Cross-system integration (caravans, market, trade stance, factions, expedition, ledger)

## Follow-On Opportunities

- Trade route specialization (specialized cargo types)
- Trade route legacy (famous routes remembered)
- Trade route quests (specific route goals)
- Trade route events (trade fairs, market crashes)
- Trade route trading (sell/buy routes between players)
