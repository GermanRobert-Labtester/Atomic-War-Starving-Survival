# Plan 213 — Survivor Barter & Informal Economy

## Goal

Create a survivor-to-survivor barter and informal economy system where survivors can trade items, favors, and services directly with each other outside the official shelter economy — creating an informal market with prices, negotiation, trust, and reputation. Currently `MarketSystem.Barter()` handles equal-value exchange with external traders, and all items live in shared shelter inventory — but there is no survivor-to-survivor trading, no informal economy, no personal negotiation, no favor-trading, no black market within the shelter. Survivors cannot trade with each other. This plan adds internal economic agency.

## Why

**Repository evidence:** Grep for `SurvivorBarter`, `InformalEconomy`, `InternalTrade`, `SurvivorTrade`, `PersonalTrade`, `BarterSystem`, `UndergroundMarket`, `SurvivorEconomy` in Core returns ZERO matches. `MarketSystem` handles external trade. All items in shared inventory. No survivor-to-survivor trading exists.

**What is missing:** No survivor-to-survivor barter. No informal economy. No personal negotiation. No favor-trading. No internal black market. No personal prices. No trade reputation between survivors. Survivors cannot trade with each other.

**Why existing plans don't solve it:** Plan 155 (Black Market) covers external underground economy. Plan 192 (Trade Routes) covers player trade route management. Plan 204 (Recruitment) mentions "trade for survivor" but not general barter. No plan addresses internal survivor-to-survivor trading.

**Player value:** Creates economic depth (survivors have economic agency), adds social dynamics (trade builds relationships), generates emergent stories (negotiations, disputes, black market), and makes the shelter feel like a real economy not just a resource pool.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — external market (complementary)
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — shared inventory
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships (trade trust)
- NEW: `Assets/Ashfall.Core/Economy/SurvivorBarterSystem.cs`
- NEW: `Assets/StreamingAssets/Data/barter_rules.json`

## Main Task 1 — Foundation / System Contract

1. Create `SurvivorBarterSystem.cs` in `Assets/Ashfall.Core/Economy/`
2. Define `BarterOffer` DTO: `offerId`, `offererId` (survivor_id), `offeredItems` (list of item_ids from personal belongings), `requestedItems` (list of item_ids or favor description), `requestedFavor` (favor description or null), `minimumAcceptance` (0-100, how much less they'll accept), `expiresDay`, `status` (pending/accepted/countered/rejected/expired)
3. Define `BarterTrade` DTO: `tradeId`, `tradeType` (item_exchange/favor_exchange/mixed/gift), `participantA` (survivor_id), `participantB` (survivor_id), `itemsFromA` (list of item_ids), `itemsFromB` (list of item_ids), `favorFromA` (favor description or null), `favorFromB` (favor description or null), `agreedValue` (perceived value), `tradeDay`, `status` (completed/disputed/voided)
4. Define `InformalPrice` DTO: `priceId`, `itemId`, `sellerId` (survivor_id), `askingPrice` (in trade goods or favors), `priceType` (fixed/negotiable/free), `expiresDay`, `isAvailable` bool
5. Define `TradeReputation` DTO: `reputationId`, `traderA` (survivor_id), `traderB` (survivor_id), `trustLevel` (0-100), `tradeCount`, `disputeCount`, `lastTradeDay`, `reputationTags` (fair/dealing/honest/cheater)
6. Define `FavorOwed` DTO: `favorId`, `debtorId` (survivor_id who owes), `creditorId` (survivor_id who is owed), `favorDescription`, `owedDay`, `dueDay` (-1 if no deadline), `isRepaid` bool, `repaidDay` (-1 if unrepaid)
7. Define `SurvivorBarterState` DTO: list of active offers, list of completed trades, list of informal prices, list of trade reputations, list of favors owed, barter settings (barter enabled bool, max offers per survivor, informal market bool)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define barter mechanics:
   - Survivors can make offers to each other
   - Offers include items and/or favors
   - Offers can be accepted, rejected, or countered
   - Counter-offers modify terms
   - Trades logged
10. Define informal pricing:
    - Survivors set prices for their items
    - Prices in trade goods (not currency — no money in shelter)
    - Prices can be fixed, negotiable, or free
    - Prices expire after duration
    - Pricing logged
11. Define favor-trading:
    - Survivors can trade favors (services, help)
    - Favors owed tracked with deadlines
    - Unrepaid favors: trust damage
    - Favor trading logged
12. Define trade reputation:
    - Each survivor pair has trade reputation
    - Fair dealing: trust increases
    - Cheating/disputes: trust decreases
    - High trust: better deals, credit available
    - Low trust: refused trades, worse deals
    - Reputation logged
13. Define dispute mechanics:
    - Trades can be disputed (item not as described, favor not delivered)
    - Disputes resolved through mediation or authority
    - Disputes damage trade reputation
    - Disputes logged
14. Define gift economy:
    - Survivors can give items without expecting return
    - Gifts build social capital
    - Gifts logged
15. Add deterministic seeding: barter events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupSurvivorBarter`, `TickSurvivorBarter`, `SaveSurvivorBarter`

## Main Task 2 — Implementation / Offers / Trades / Prices / Reputation / Favors / UI

1. Implement barter offers:
   - Survivor makes offer to another
   - Offer includes items/favors
   - Offer accepted/rejected/countered
   - Offers logged
2. Implement informal pricing:
   - Survivor sets price for item
   - Price in trade goods
   - Price negotiable/fixed/free
   - Prices logged
3. Implement favor-trading:
   - Survivors trade favors
   - Favors owed tracked
   - Unrepaid: trust damage
   - Favors logged
4. Implement trade reputation:
   - Reputation per survivor pair
   - Fair dealing: trust up
   - Cheating: trust down
   - Reputation logged
5. Implement disputes:
   - Trades disputed
   - Disputes resolved
   - Reputation affected
   - Disputes logged
6. Implement gift economy:
   - Gifts given without expectation
   - Gifts build social capital
   - Gifts logged
7. Implement barter UI:
   - Barter panel: active offers, make new offer
   - Offer detail: items, favors, counter-offer
   - Trade log: history of trades
   - Price board: informal prices
   - Reputation panel: trade reputations
   - Favors panel: favors owed/owing
   - Dispute panel: active disputes
8. Create barter events:
    - "The Offer" — barter offer made
    - "The Trade" — trade completed
    - "The Counter" — counter-offer made
    - "The Dispute" — trade disputed
    - "The Favor" — favor traded
    - "The Gift" — gift given
    - "The Price" — informal price set
    - "The Reputation" — trade reputation changed
9. Add barter quest hooks:
    - "The Trader" — complete 20 trades
    - "The Negotiator" — successfully counter 10 offers
    - "The Generous" — give 10 gifts
    - "The Reliable" — repay all favors on time
    - "The Merchant" — set 30 informal prices
    - "The Trusted" — reach 90+ trade reputation with 5 survivors
    - "The Dispute Resolver" — resolve 5 trade disputes
10. Implement barter tutorial: first trade explains system
11. Add barter tooltips: hover over offer shows details
12. Create barter rules in data file
13. Implement barter persistence: offers/trades/reputations saved
14. Integrate with `Inventory`: items transferred via trade

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `Inventory`: items transferred via trade
2. Connect to `SurvivorRelationsSystem`: trade affects relationships
3. Integrate with `PersonalBelongingsSystem` (Plan 210): personal items traded
4. Connect to `InterpersonalConflictSystem` (Plan 202): disputes trigger conflicts
5. Wire into `InternalCommunicationSystem` (Plan 211): trade notices on boards
6. Connect to `ShelterReputationSystem` (Plan 207): trade reputation affects shelter reputation
7. Implement old-save compatibility: existing saves get no active offers/trades
8. Add deterministic seeding: barter events use `ISeededRng`
9. Create exploit prevention: trades are consensual, can't be gamed
10. Add tests: offers, trades, prices, reputation, favors, disputes, save round-trip
11. Verify all barter types work correctly
12. Test edge cases: no barter (current behavior), heavy barter (active market)
13. Verify headless behavior: barter processes correctly without UI
14. Add data-integrity-selftest: barter validates against survivor/item catalogs
15. Create `--survivor-barter-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --survivor-barter-selftest
```

## Risk

**LOW** — Barter is straightforward with clear inputs (offers) and outputs (trades, reputation). Risk of barter feeling like spreadsheet management. Mitigation: make trades meaningful (relationship effects), show clear consequences, and ensure barter feels like social interaction not just resource shuffling.

## Definition of Done

- `SurvivorBarterSystem.cs` exists with full `CaptureState/RestoreState`
- Barter offers (items, favors, counter-offers)
- Informal pricing (trade goods, negotiable/fixed/free)
- Favor-trading (owed, deadlines, trust effects)
- Trade reputation (per-pair trust, fair/cheater tags)
- Dispute mechanics (resolution, reputation damage)
- Gift economy (social capital)
- Barter events and quest hooks
- Save/load round-trip tested
- Deterministic barter events verified
- Old saves load with no active offers/trades
- Barter rules in data authority
- UI barter panel, offer detail, trade log, price board, reputation panel, favors panel, dispute panel
- Cross-system integration (inventory, relations, belongings, conflicts, communication, reputation)

## Follow-On Opportunities

- Barter specialization (survivors become expert traders/merchants)
- Barter legacy (famous trades remembered)
- Barter quests (specific trading goals)
- Barter events (massive trade fair, market crash)
- Barter trading (trade trade services with other settlements)
