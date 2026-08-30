# Plan 99 — Hardcore Economy Tuning Expansion (2 tiers → 8 tiers, 1 faction pref → 8, 1 price shock → 6)

## Goal (2 lines)
Expand `hardcore_economy_tuning.json` from 2 scarcity tiers, 1 faction
preference, and 1 price shock rule to 8 tiers, 8 faction preferences, and 6
price shock rules. The hardcore economy tuning system
(`HardcoreEconomyTuning.cs` confirmed live) defines dynamic pricing multipliers
by scarcity tier, faction trade preferences, and price shock events. The
current data covers only days 1–40 and one faction.

## Why (P2)
- Verified: `hardcore_economy_tuning.json` has 2 scarcity_tiers (Critical days
  1–15, High days 15–40), 1 faction_preference (central_garrison_remnants),
  and 1 price_shock_rule (PlumePassing).
  `HardcoreEconomyTuning.cs` and `HardcoreEconomyTuningDto.cs` are confirmed
  in Core.
- Creates the dynamic-economy pillar: the economy should shift across the
  full campaign — early scarcity (water, iodine), mid scarcity (medicine,
  fuel), late scarcity (seeds, tools, knowledge). Faction preferences should
  cover all major factions. Price shocks should cover multiple event types.
  2 tiers and 1 faction means the economy is static after day 40.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` (expand tiers
  2 → 8, faction prefs 1 → 8, price shocks 1 → 6)
- Read-only: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs` (confirm
  schema and how tiers/faction prefs/price shocks are applied)
- `Assets/StreamingAssets/Data/items.json` (affected_item_ids must resolve)

## Content grammar (per scarcity tier)
- `tier`: name (Critical, High, Moderate, Stable, Abundant, etc.).
- `multiplier`: 0.5–3.0 (price multiplier — >1 means expensive, <1 means
  cheap).
- `day_range_label`: human-readable day range ("Days 1-15", "Days 40-100").
- `affected_item_ids`: array of item ids affected by this tier. May include
  wildcards ("ammo_*").
- `rationale`: 1 sentence explaining why these items are scarce/abundant
  in this period.

## Content grammar (per faction preference)
- `faction_id`: must resolve to an existing faction.
- `buys_at_premium`: array of item ids/patterns the faction pays extra for.
- `refuses`: array of item ids the faction won't trade.
- `trade_currency`: 1 sentence describing what the faction trades in.

## Content grammar (per price shock rule)
- `kind`: event name (PlumePassing, ConvoyAmbush, FactionConflict,
  SeasonalScarcity, DiseaseOutbreak, FuelShortage).
- `multiplier`: 1.2–3.0 (price spike multiplier).
- `duration_days`: 1–7 (how long the shock lasts).
- `affected_item_ids`: array of item ids affected ("*" for all).
- `trigger`: 1 sentence describing what causes the shock.

## Steps
1. Read `HardcoreEconomyTuning.cs` to confirm the schema and how tiers are
   selected (by day range? by game state?), how faction preferences are
   applied (per-faction price modifier?), and how price shocks fire (event-
   triggered?).
2. Read `items.json` to confirm which item ids exist for affected_item_ids.
3. Author 6 new scarcity tiers covering the full campaign:
   - `Moderate` (days 40–100): medicine and tools rise, water stabilizes.
   - `Stable` (days 100–180): most goods normalize, seeds and knowledge
     become valuable.
   - `Late_Scarcity` (days 180–240): fuel and ammunition scarce, food
     pressure returns.
   - `Deep_Winter` (days 240–300): everything scarce, cold-weather gear
     critical.
   - `Endgame` (days 300+): rare items dominate trade, common items
     worthless.
4. Author 7 new faction preferences (one per major faction from Plan 98):
   the_scale, the_compact, the_underwrite, the_cutters, the_fleet,
   the_rebuilders, the_garrison. Each with distinct buys_at_premium and
   refuses.
5. Author 5 new price shock rules:
   - `ConvoyAmbush`: trade convoy attacked, supplies spike, 3 days.
   - `FactionConflict`: factions at war, route-danger premium, 5 days.
   - `SeasonalScarcity`: winter hoarding, fuel/food spike, 7 days.
   - `DiseaseOutbreak`: medical supplies spike, 4 days.
   - `FuelShortage`: fuel convoy delayed, fuel/ammo spike, 3 days.
6. Cross-reference: every affected_item_id resolves in items.json (or uses
   wildcard); every faction_id resolves to an existing faction.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: hardcore economy tuning catalog loads 8 tiers, 8 faction prefs, 6
   price shocks, all item ids resolve, multipliers within valid ranges.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is wildcard patterns (step 6): confirm the
system supports `ammo_*` style wildcards before using them.

## Definition of Done
- `hardcore_economy_tuning.json` has 8 tiers, 8 faction preferences, 6 price
  shocks, all ids resolving, integrity + tests green.

## Follow-on
- Plan 56 (economy goods) — goods are the items affected by tiers.
- Plan 98 (standing record factions) — faction preferences reference
  factions.
- Plan 48 (weather gates) — weather events trigger price shocks.
- Plan 77 (duty roster seasons) — seasons align with scarcity tiers.
- Plan 61 (trade screen scenarios) — scenarios reference faction preferences.
