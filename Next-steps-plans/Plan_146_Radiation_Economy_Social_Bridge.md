# Plan 146 — Radiation → Economy & Social Bridge

## Goal

Connect radiation exposure to economic consequences (contaminated goods trade restrictions, price modifiers) and social dynamics (discrimination against irradiated survivors, faction reactions). Currently `RadiationSystem` modifies health and degrades gear but has zero connection to trade prices, faction trust, or NPC dialogue. This makes radiation a socially and economically meaningful status, not just a health bar.

## Why

**Repository evidence:** `RadiationSystem.cs` modifies health and degrades worn gear. `RadiationAfflictionHandlers.cs` bridges dose into the affliction system. `DecontaminationSystem.cs` reduces radiation. `DoseLedgerSystem.cs` tracks per-survivor dose history. But the cross-system agent confirmed: "Radiation does NOT affect economy (contaminated goods), social dynamics (discrimination), or quest availability." No economy file references radiation dose as a trade modifier. No social system queries radiation state.

**What is missing:** Irradiated survivors face no social consequences. Contaminated goods trade freely. Factions don't react to radiation levels. A survivor glowing with radiation walks into a trading post and nobody cares. Radiation is a personal health problem, not a social or economic one.

**Why existing plans don't solve it:** Plan 137 (needs→performance) connects needs to performance but not radiation to economy/social. Plan 135 (weather cascade) makes weather affect radiation but not vice versa. Plan 106 (dose items) adds radiation items but not social/economic integration. No plan addresses radiation→economy or radiation→social bridges.

**Player value:** Makes radiation management a strategic priority beyond health (trade restrictions, social stigma), creates meaningful decontamination decisions (clean survivors for trade/diplomacy), and generates emergent stories (a key trader refuses to deal with irradiated survivors).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation tracking
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — market prices
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` — faction trust
- `Assets/Ashfall.Core/Economy/HoldfastTradeSession.cs` — trade sessions
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — survivor relationships
- `Assets/StreamingAssets/Data/items.json` — item definitions
- NEW: `Assets/Ashfall.Core/Radiation/RadiationEconomyBridge.cs`
- NEW: `Assets/Ashfall.Core/Radiation/RadiationSocialBridge.cs`

## Main Task 1 — Foundation / System Contract

1. Create `RadiationEconomyBridge.cs` in `Assets/Ashfall.Core/Radiation/`
2. Create `RadiationSocialBridge.cs` in `Assets/Ashfall.Core/Radiation/`
3. Define `RadiationEconomyModifier` DTO: `itemId`, `contaminationThreshold`, `priceMultiplier` (0.1-2.0), `tradeBlocked` bool, `factionId` (optional)
4. Define `RadiationSocialModifier` DTO: `radiationLevel`, `socialPenalty` (0-100), `discriminationChance` (0-1.0), `factionReaction` (string)
5. Define `RadiationBridgeState` DTO: map of item → economy modifiers, map of survivor → social modifiers
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define economy modifier rules:
   - Contaminated food/water: -50% price (buy), +100% price (sell to unaware)
   - Irradiated equipment: -30% price, some factions refuse to buy
   - Contaminated trade goods: blocked by certain factions (health-conscious)
   - Clean goods from irradiated zone: -20% price (suspicion)
8. Define social modifier rules:
   - Low radiation (<20 mSv): no social effect
   - Moderate radiation (20-50 mSv): -10 social penalty, 5% discrimination chance
   - High radiation (50-100 mSv): -25 social penalty, 15% discrimination chance
   - Severe radiation (>100 mSv): -50 social penalty, 30% discrimination chance, faction reactions
9. Create `IRadiationEconomySink` interface for market/trade systems to query modifiers
10. Create `IRadiationSocialSink` interface for social/faction systems to query modifiers
11. Implement economy modifier application: market prices adjusted based on contamination
12. Implement social modifier application: survivor social interactions affected by radiation
13. Add deterministic calculation: modifiers are pure functions of radiation state (no RNG)
14. Wire into `GameBootstrap`: `SetupRadiationBridges`, `SaveRadiationBridges`

## Main Task 2 — Implementation / Economy / Social / Quest Integration

1. Implement contaminated goods trade:
   - Items have contamination level (0-100)
   - Contaminated items sell for less (buyers aware)
   - Some factions refuse contaminated goods entirely
   - Player can sell contaminated goods to unaware traders (moral choice)
   - Decontamination removes contamination (cost: resources, time)
2. Implement radiation price modifiers:
   - Clean goods from irradiated zone: -20% price (suspicion of contamination)
   - Verified clean goods: normal price
   - Contaminated goods: -50% to -90% price depending on level
   - Rare uncontaminated goods from hot zone: +50% price (scarcity premium)
3. Implement faction trade restrictions:
   - Health-conscious factions refuse contaminated goods
   - Military factions require clean equipment
   - Rebel factions accept contaminated goods (desperate)
   - Independent factions trade based on standing, not radiation
4. Implement survivor discrimination:
   - Irradiated survivors face social penalties in interactions
   - NPCs refuse to trade with heavily irradiated survivors
   - Faction envoys express concern about radiation exposure
   - Other survivors may avoid irradiated companions (affinity penalty)
5. Implement radiation social events:
   - "The Outcast" — survivor refused entry to settlement due to radiation
   - "The Clean Trader" — faction offers premium for verified clean goods
   - "The Desperate Deal" — faction accepts contaminated goods at discount
   - "The Decontamination Queue" — multiple survivors need cleaning, limited resources
6. Add radiation quest hooks:
   - "The Hot Zone Merchant" — trader offers rare goods from irradiated area
   - "The Clean Slate" — decontaminate key survivor before diplomatic meeting
   - "The Contaminated Cargo" — discover shipment is irradiated, what to do?
   - "The Radiation Refugees" — irradiated survivors seek shelter, moral choice
7. Implement radiation faction reactions:
   - Faction standing modified by survivor radiation levels
   - High-radiation survivors reduce faction trust
   - Decontaminated survivors restore trust
   - Some factions specialize in radiation treatment (trade opportunity)
8. Create radiation social UI:
   - Survivor panel shows radiation social penalty
   - Trade panel shows contamination status and price modifiers
   - Faction panel shows radiation-based trust modifiers
9. Add radiation journal: automatic log of radiation economic/social events
10. Implement radiation tutorial: first trade with contaminated goods explains system
11. Add radiation tooltips: hover over contaminated item shows trade effects
12. Create 20 radiation economy modifiers and 15 social modifiers in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MarketSystem`: prices adjusted by contamination
2. Connect to `HoldfastTradeSession`: trade restrictions enforced
3. Integrate with `FactionStanceEngine`: faction trust modified by radiation
4. Connect to `SurvivorRelationsSystem`: irradiated survivors face affinity penalties
5. Wire into `DecontaminationSystem`: decontamination removes economic/social penalties
6. Connect to `DoseLedgerSystem`: dose history tracks contamination exposure
7. Implement old-save compatibility: existing saves get empty bridge state
8. Add deterministic calculation: modifiers are pure functions of radiation state
9. Create exploit prevention: contamination is tracked, can't be hidden
10. Add tests: price modifiers, trade restrictions, social penalties, save round-trip
11. Verify catalog integrity: all item/faction IDs resolve
12. Test edge cases: no radiation (no modifiers), all survivors irradiated (max penalties)
13. Verify headless behavior: bridges process correctly without UI
14. Add data-integrity-selftest: radiation modifiers validate against item/faction catalogs
15. Create `--radiation-bridges-selftest` verb for CI validation

## State / System Interaction Model

```text
Survivor/item radiation state
├─ Economy bridge calculates modifiers
│  ├─ Contaminated items: price reduced, trade may be blocked
│  ├─ Clean goods from hot zone: price reduced (suspicion)
│  ├─ Rare clean goods: price increased (scarcity)
│  └─ Faction-specific restrictions enforced
├─ Social bridge calculates modifiers
│  ├─ Survivor radiation level → social penalty
│  ├─ Discrimination chance → NPC refusals
│  └─ Faction reactions → standing modifiers
├─ UI updated
│  ├─ Trade panel shows contamination/price effects
│  ├─ Survivor panel shows social penalty
│  └─ Faction panel shows radiation trust modifiers
└─ Downstream systems notified
   ├─ Market: prices adjusted
   ├─ Trade sessions: restrictions enforced
   ├─ Factions: trust modified
   └─ Social: interactions affected
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --radiation-bridges-selftest
```

## Risk

**MEDIUM** — Radiation penalties can frustrate players if too severe or unavoidable. Risk of discrimination feeling punitive rather than meaningful. Mitigation: keep penalties moderate (max -50% price, -50 social), provide decontamination options, allow moral choices (sell contaminated knowingly), and ensure clean alternatives exist.

## Definition of Done

- `RadiationEconomyBridge.cs` and `RadiationSocialBridge.cs` exist with full `CaptureState/RestoreState`
- Contaminated goods trade mechanics functional
- Radiation price modifiers applied to market
- Faction trade restrictions enforced
- Survivor discrimination mechanics working
- Radiation social events and quest hooks
- Save/load round-trip tested
- Deterministic modifier calculation verified
- Old saves load without error
- 20 economy modifiers + 15 social modifiers in data authority
- UI panels show radiation economic/social effects
- Cross-system integration (radiation, economy, factions, social, decontamination)

## Follow-On Opportunities

- Radiation mutation system (long-term exposure causes permanent changes)
- Radiation specialization (survivors become decontamination experts)
- Radiation black market (trade contaminated goods secretly)
- Radiation legacy (irradiated survivors gain unique traits)
- Radiation zones (permanent contaminated areas on map)
