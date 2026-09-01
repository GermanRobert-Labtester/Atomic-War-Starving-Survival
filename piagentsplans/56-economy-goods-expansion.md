# Plan 56 — Economy Goods Expansion (16 → 40 trade goods)

## Goal (2 lines)
Expand `economy_goods.json` from 16 verified goods to 40, giving the dynamic pricing
system real market depth. Each good has a base price, volatility, elasticity, stack size,
and barter note — the economy system is fully implemented but the goods catalog is too
thin for trade to feel like a living market.

## Why (P2)
- Verified: `economy_goods.json` has 16 goods (`clean_water`, etc. with basePrice,
  volatility, elasticity, stackSize, weightKg, barterNote). The dynamic pricing system
  (`EconomySystem`, Plan 13 trade flow) uses these fields but 16 goods is not enough for a
  believable market.
- Creates the trade-depth pillar: more goods mean more trade routes (existing 16B
  caravans), more price arbitrage opportunities, and more reason to visit settlements
  (Plan 43) for goods the shelter lacks.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/economy_goods.json` (expand 16 → 40 goods)
- `Assets/StreamingAssets/Data/items.json` (every good must resolve as an `item_*` id)
- Read-only: `Assets/Ashfall.Core/Economy/` (confirm the goods schema and how the dynamic
  pricing system consumes volatility/elasticity)

## Content grammar (per good)
- snake_case `id` matching an existing `items.json` entry (TIER-2 validation).
- category: water / food / fuel / medicine / materials / tools / weapons / ammunition /
  luxury / information / salvage.
- basePrice: integer; the reference price before volatility.
- volatility: 0.0–1.0; how much the price swings per tick.
- elasticity: >1.0 = luxury (price-sensitive), <1.0 = necessity (price-inelastic).
- stackSize: trade stack size.
- weightKg: per-unit weight (affects caravan cargo capacity).
- barterNote: 1 sentence of grounded trade flavor (how this good is regarded in the
  wasteland). Skill `ashfall-write`.

## Steps
1. Read `economy_goods.json` to confirm the 16 existing goods and their schema.
2. Read the Economy system to confirm how volatility/elasticity drive price changes.
3. Read `items.json` to identify items that should be trade goods but aren't in the
   economy catalog yet (medicine, tools, ammo, salvage, luxury items).
4. Author 24 new goods across 8 categories:
   - Food (4): canned stew, dried meat, preserved vegetables, ration bars.
   - Fuel (3): diesel, gasoline, coal.
   - Medicine (4): antibiotics, painkillers, iodine pills, chelation agent.
   - Materials (4): scrap metal, timber, steel beam, concrete.
   - Tools (3): wrench, saw, welding kit.
   - Ammunition (3): 9mm, 556, 12gauge.
   - Luxury (2): cigarettes, alcohol (morale trade goods — high volatility).
   - Information (1): maps (high value, low weight — caravan arbitrage good).
5. Assign each good: basePrice, volatility, elasticity, stackSize, weightKg, barterNote.
6. Cross-reference: every good `id` exists in `items.json` (add missing items first).
7. Wire 8 new goods into Plan 43 settlement trade_goods/trade_needs — settlements
   export/import specific goods.
8. Wire 5 new goods into existing 16B caravan routes — caravans transport high-value,
   low-weight goods (medicine, information) more profitably than bulk goods (materials).
9. Validate: `--data-integrity-selftest`; confirm prices fluctuate per volatility in a
   headless boot; confirm settlement trade profiles resolve.
10. xUnit: goods catalog loads, all ids resolve, price volatility applies deterministically
    (seeded), elasticity affects price response to supply/demand, save round-trip
    preserves market state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `economy_goods.json` has 40 goods (16 existing + 24 new), all ids resolving, 8 wired
  into settlements, 5 wired into caravans, price volatility deterministic, save
  round-trip green, integrity + tests green.

## Follow-on
- Plan 43 (settlements) — trade goods/needs define settlement economies.
- Existing 16B (caravans) — goods drive caravan profitability.
- Plan 55 (recipes) — crafted outputs become trade goods.
- Plan 40 (debt) — goods are debt principals and collateral.
