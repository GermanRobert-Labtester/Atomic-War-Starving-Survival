# Plan 155 — Black Market & Underground Economy

## Goal

Create a black market and underground economy system where players can engage in illegal trade, contraband smuggling, and shadowy dealings for profit and advantage. Currently all trade is above-board through `MarketSystem` and `HoldfastTradeSession` — there is no illegal economy, no contraband, no black market dealers, no moral cost to profitable shadow dealings. This plan adds a morally ambiguous economic layer that creates strategic depth and emergent stories.

## Why

**Repository evidence:** `MarketSystem.cs` handles legal trade with deterministic demand walks. `HoldfastTradeSession.cs` provides faction-gated buy/sell. `FactionStanceEngine.cs` tracks trade stances. But all trade is overt and legal. No system exists for contraband, black market dealers, illegal goods, or underground economy. The cross-system agent confirmed: economy systems have no illegal/underground layer.

**What is missing:** Players cannot engage in illegal trade. There are no contraband goods, no black market dealers, no smuggling operations, no illegal services. All economic activity is above-board. This removes a major genre element of post-collapse survival — the shadow economy that thrives when institutions collapse.

**Why existing plans don't solve it:** Plan 13 (economy survival loop) adds goods and recipes but not illegal trade. Plan 56 (economy goods expansion) adds more legal goods. Plan 99 (hardcore economy tuning) adjusts difficulty but doesn't add black market. Plan 146 (radiation→economy) adds contaminated goods trade but not illegal economy. No plan addresses black market or underground economy mechanics.

**Player value:** Creates moral dilemmas (profitable but illegal trade), adds strategic depth (alternative economic options), generates emergent stories (deals gone wrong, snitches, rival dealers), and makes the economy feel more realistic (post-collapse economies always have black markets).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — legal market
- `Assets/Ashfall.Core/Economy/HoldfastTradeSession.cs` — faction trade
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` — faction trade stance
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — moral choices
- `Assets/StreamingAssets/Data/economy_goods.json` — trade goods
- NEW: `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`
- NEW: `Assets/StreamingAssets/Data/black_market_goods.json`

## Main Task 1 — Foundation / System Contract

1. Create `BlackMarketSystem.cs` in `Assets/Ashfall.Core/Economy/`
2. Define `BlackMarketDealer` DTO: `dealerId`, `name`, `location` (settlement/itinerant), `specialty` (drugs/weapons/intel/contraband), `trust` (0-100), `inventory` (list of goods), `prices` (map of good → price)
3. Define `ContrabandGood` DTO: `goodId`, `name`, `baseValue`, `illegality` (0-100), `detectionRisk` (0-100), `factionBanned` (list of factions that ban this good), `moralPenalty` (moral band delta)
4. Define `BlackMarketTransaction` DTO: `transactionId`, `dealerId`, `goodId`, `quantity`, `price`, `day`, `detected` bool, `consequences` (list)
5. Define `BlackMarketState` DTO: list of dealers, list of contraband goods, list of transactions, player reputation in underground, detection heat level
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define contraband categories:
   - **Drugs**: narcotics, stimulants, painkillers (high profit, high moral cost)
   - **Weapons**: illegal firearms, explosives, military gear (high profit, high risk)
   - **Intel**: stolen secrets, blackmail material, faction codes (medium profit, medium risk)
   - **Contraband**: restricted goods, stolen property, banned items (variable profit, variable risk)
   - **Services**: assassination, forgery, smuggling (high profit, extreme risk)
8. Define black market mechanics:
   - Dealers operate in shadows (specific locations, hidden meetings)
   - Trust required to access dealers (build reputation through small deals)
   - Contraband has detection risk (carrying, trading, using)
   - Detection leads to consequences (faction hostility, moral penalty, legal trouble)
   - Black market prices fluctuate based on supply/demand/risk
9. Define detection mechanics:
   - Carrying contraband: risk of search/detection
   - Trading contraband: risk of informant, undercover agent
   - Using contraband: risk of side effects, addiction, exposure
   - Detection heat increases with illegal activity
   - High heat: factions investigate, dealers become cautious
   - Extreme heat: player marked as criminal, bounty placed
10. Define moral consequences:
    - Illegal trade reduces moral band
    - Some goods have extreme moral cost (drugs, assassination)
    - Moral choices: profit vs. ethics
    - Reputation: known criminal vs. respected trader
11. Add deterministic seeding: black market outcomes use `ISeededRng`
12. Wire into `GameBootstrap`: `SetupBlackMarket`, `TickBlackMarket`, `SaveBlackMarket`
13. Create `BlackMarketDealerCatalogLoader` for dealer definitions
14. Create `ContrabandGoodCatalogLoader` for contraband definitions
15. Create UI hook: black market panel showing dealers, goods, heat level

## Main Task 2 — Implementation / Dealers / Goods / Detection / Consequences

1. Implement black market dealers:
   - Dealers have specialties (drugs, weapons, intel, contraband)
   - Dealers require trust to access (start with small deals)
   - Dealers have inventory that refreshes periodically
   - Dealers offer better prices for bulk/regular customers
   - Dealers can betray player if heat too high
2. Implement contraband goods:
   - **Drugs**: morphine, stimulant, hallucinogen (high profit, addiction risk)
   - **Weapons**: military rifle, explosives, armor (high profit, combat bonus)
   - **Intel**: faction codes, trade secrets, blackmail (medium profit, strategic value)
   - **Contraband**: stolen goods, restricted tech, banned books (variable)
   - **Services**: assassination contracts, forged documents, smuggling routes
3. Implement detection system:
   - Carrying contraband: 5% base detection per settlement entry
   - Trading contraband: 10% base detection per transaction
   - Detection modified by: heat level, faction security, informant chance
   - Detection leads to: goods confiscated, fine, faction hostility
   - Extreme detection: bounty placed, dealers refuse service
4. Implement heat management:
   - Heat increases with illegal activity (0-100)
   - Heat decays over time if no illegal activity
   - High heat (50+): dealers cautious, prices increase
   - Extreme heat (80+): dealers refuse service, factions investigate
   - Heat can be reduced: bribes, laying low, completing legal quests
5. Implement black market events:
   - "The Deal" — profitable but risky transaction
   - "The Sting" — undercover agent attempts to catch player
   - "The Snitch" — informant betrays player to authorities
   - "The Score" — big opportunity, high risk high reward
   - "The Heat" — factions investigate illegal activity
   - "The Fence" — dealer offers to launder money/goods
   - "The Betrayal" — dealer turns on player
6. Add black market quest hooks:
   - "The Kingpin" — build black market network
   - "The Undercover" — infiltrate black market for faction
   - "The Heist" — steal valuable contraband
   - "The Smuggle" — move contraband through checkpoints
   - "The Informant" — discover who's snitching
   - "The Redemption" — go legal after criminal career
   - "The Empire" — build criminal organization
7. Implement black market consequences:
   - Profit: significant economic gain
   - Detection: goods confiscated, fines, faction hostility
   - Moral: moral band decreases with illegal activity
   - Reputation: known criminal affects faction relations
   - Addiction: drug use/trafficking has health consequences
   - Violence: black market dealings can turn violent
8. Implement black market integration:
   - Contraband can be used (drugs consumed, weapons equipped)
   - Contraband affects stats (drugs boost then crash, weapons improve combat)
   - Contraband trade affects legal market (flood market, prices drop)
   - Black money can be laundered through legal trade (fee required)
9. Add UI: black market panel showing dealers, goods, heat level
10. Create black market journal: automatic log of transactions and events
11. Implement black market tutorial: first deal explains system
12. Add black market tooltips: hover over good shows risk/reward
13. Create 15 contraband goods and 10 dealers in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MarketSystem`: black market affects legal market prices
2. Connect to `HoldfastTradeSession`: factions react to contraband
3. Integrate with `FactionStanceEngine`: illegal activity reduces trust
4. Connect to `MoralChoiceSystem`: illegal trade affects moral band
5. Wire into `InventorySystem`: contraband items tracked separately
6. Connect to `NeedsSystem`: drugs affect needs (boost then crash)
7. Implement old-save compatibility: existing saves get empty black market state
8. Add deterministic seeding: black market outcomes use `ISeededRng`
9. Create exploit prevention: detection prevents infinite illegal trade
10. Add tests: transactions, detection, heat, consequences, save round-trip
11. Verify catalog integrity: all dealer/good IDs resolve
12. Test edge cases: no black market (no illegal activity), max heat (all dealers refuse)
13. Verify headless behavior: black market processes correctly without UI
14. Add data-integrity-selftest: black market definitions validate against catalogs
15. Create `--black-market-selftest` verb for CI validation

## State / System Interaction Model

```text
Player engages in black market activity
├─ Find dealer (requires trust/reputation)
│  ├─ Small deals build trust
│  ├─ Dealers specialize (drugs/weapons/intel/contraband)
│  └─ Dealers have inventory, refresh periodically
├─ Transaction
│  ├─ Buy/sell contraband goods
│  ├─ Price based on risk, demand, trust
│  ├─ Detection risk (5-10% base per transaction)
│  └─ Heat increases with activity
├─ Detection
│  ├─ Carrying contraband: search risk
│  ├─ Trading: informant/undercover risk
│  ├─ Detected: goods confiscated, fine, hostility
│  └─ Extreme: bounty placed, dealers refuse
├─ Heat management
│  ├─ Heat increases with illegal activity
│  ├─ Heat decays over time (if clean)
│  ├─ High heat: dealers cautious, prices up
│  └─ Extreme heat: dealers refuse, factions investigate
├─ Consequences
│  ├─ Profit: economic gain
│  ├─ Moral: moral band decreases
│  ├─ Reputation: known criminal
│  ├─ Addiction: drug consequences
│  └─ Violence: dealings can turn violent
└─ Integration
   ├─ Contraband used (drugs, weapons)
   ├─ Black market affects legal market
   ├─ Money laundered through legal trade
   └─ Factions react to illegal activity
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --black-market-selftest
```

## Risk

**HIGH** — Black market complexity can overwhelm players if too many goods, dealers, and consequences exist. Risk of black market making legal economy irrelevant (why trade legally when black market is more profitable?). Mitigation: detection risk keeps black market dangerous, moral costs create meaningful choices, legal trade remains safer/more sustainable, and heat mechanics prevent spam.

## Definition of Done

- `BlackMarketSystem.cs` exists with full `CaptureState/RestoreState`
- 5 contraband categories implemented (drugs, weapons, intel, contraband, services)
- Black market dealer system functional (trust, inventory, specialties)
- Detection mechanics working (carrying, trading, heat)
- Heat management system tracking illegal activity
- Black market consequences (profit, moral, reputation, addiction, violence)
- Black market events and quest hooks
- Save/load round-trip tested
- Deterministic black market outcomes verified
- Old saves load without error
- 15 contraband goods + 10 dealers in data authority
- UI panel shows black market interface
- Cross-system integration (market, factions, moral choice, inventory, needs)

## Follow-On Opportunities

- Criminal organization system (build your own black market network)
- Law enforcement system (factions investigate and prosecute crime)
- Undercover operations (infiltrate black market for factions)
- Black market legacy (criminal reputation remembered in epilogue)
- Black market quests (heists, smuggling runs, dealer wars)
