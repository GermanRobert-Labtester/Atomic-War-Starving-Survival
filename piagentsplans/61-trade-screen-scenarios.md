# Plan 61 — Trade Screen Scenarios Expansion (3 → 15 scenarios)

## Goal (2 lines)
Expand `trade_screen_scenarios.json` from 3 verified entries to 15 trade scenarios. The
trade screen system (`TradeScreenPresenter`, `TradeScreenScenarios`, `TradeScreenSeam`)
is fully implemented but has only 3 scenarios — trade encounters are repetitive and lack
narrative variety.

## Why (P2)
- Verified: `trade_screen_scenarios.json` has 3 entries; `TradeScreenScenarios.cs` and
  `TradeScreenPresenter.cs` are fully implemented in `Assets/Ashfall.Core/Economy/`.
- Creates the trade-variety pillar: each trade scenario gives the encounter a context
  (a desperate trader, a faction supply run, a black-market exchange, a debt collection,
  a bulk deal) that affects prices, available goods, and negotiation options.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/trade_screen_scenarios.json` (expand 3 → 15 scenarios)
- Read-only: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` (confirm scenario
  schema: id, display name, trader type, available goods, price modifiers, negotiation
  options, faction link, special conditions)

## Content grammar (per scenario)
- snake_case `id` with prefix `trade_` or `scenario_` (confirm accepted prefix from
  existing 3 entries).
- trader_type: desperate_survivor / faction_quartermaster / black_market / caravan_merchant /
  debt_collector / bulk_dealer / smuggler / refugee_barter.
- available_goods: list of `item_*` ids the trader offers (subset of economy_goods).
- price_modifier: multiplier on base prices (desperate = 1.5, bulk = 0.8, black_market =
  variable).
- negotiation_options: available trade tell lines (feeds `trade_tell_lines.json` — Plan 62).
- faction_link: optional `faction_*` id — faction quartermasters only trade with allies.
- special_condition: min reputation, min day, required flag, or required item to unlock
  the scenario.
- description: 1-2 sentences of grounded trade flavor. Skill `ashfall-write`.

## Steps
1. Read `TradeScreenScenarios.cs` to confirm the scenario schema and how scenarios
   affect the trade screen.
2. Read the 3 existing scenarios to understand the structure and avoid duplication.
3. Author 12 new scenarios across 8 trader types:
   - 2 desperate survivors (high prices, rare goods, moral hook — they need medicine).
   - 2 faction quartermasters (faction-locked, military gear, requires reputation).
   - 2 black market (variable prices, contraband, risk of scam — feeds Plan 40 debt).
   - 2 caravan merchants (bulk goods, standard prices, follows Plan 43 settlement routes).
   - 1 debt collector (the player owes a debt — this scenario is the collection encounter;
     feeds Plan 40).
   - 1 bulk dealer (large quantities, discount for volume — feeds Plan 56 economy goods).
   - 1 smuggler (contraband, high risk, high reward — feeds Plan 45 faction patrols).
   - 1 refugee barter (the refugee has nothing of value but knows a location — feeds
     Plan 43/52).
4. Give each scenario: trader type, available goods, price modifier, negotiation options,
   faction link, special condition, description.
5. Cross-reference: every `item_*` good resolves to `items.json` + `economy_goods.json`;
   every `faction_*` link resolves; every `flag_*` condition resolves.
6. Wire 4 scenarios into Plan 43 settlement trade — each settlement has a default
   scenario type (trade post = caravan merchant, stronghold = quartermaster, refugee
   camp = refugee barter, community = bulk dealer).
7. Wire 2 scenarios into Plan 45 patrol encounters — smuggler and black-market
   scenarios appear when encountering faction patrols.
8. Validate: `--data-integrity-selftest`; confirm a trade scenario loads and modifies
   prices/availability in a headless boot.
9. xUnit: scenario catalog loads, all references resolve, price modifiers apply,
   faction locks block unauthorized trade, special conditions gate scenarios, save
   round-trip preserves trade state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `trade_screen_scenarios.json` has 15 scenarios (3 existing + 12 new), all references
  resolving, 4 wired to settlements, 2 wired to patrols, price modifiers apply, faction
  locks work, save round-trip green, integrity + tests green.

## Follow-on
- Plan 43 (settlements) — each settlement has a default trade scenario.
- Plan 45 (patrols) — smuggler and black-market scenarios on patrol encounters.
- Plan 40 (debt) — debt collector scenario.
- Plan 62 (trade tell lines) — negotiation options consume tell lines.
- Plan 56 (economy goods) — scenarios reference the expanded goods catalog.
