# Plan 126 — Crossing Items Expansion (11 → 25 items)

## Goal (2 lines)
Expand `crossing_items.json` from 11 items to 25. The Crossing expansion's
item catalog (`ItemCatalogLoader.cs` confirmed live) defines
Crossing-specific items with full item properties (type, weight, trade value,
thirst/hunger/morale effects). 11 items for the charter settlement's economy
is thin; the Crossing's trade, arbitration, and black-market themes need
more unique items.

## Why (P2)
- Verified: `crossing_items.json` has 11 items in `items` array. Each has
  id, displayName, description, type, stackMax, weight, tradeValue,
  thirstRestore, hungerRestore, moraleEffect. `ItemCatalogLoader.cs`
  loads it.
- The Crossing is a trade-and-arbitration settlement. 11 items means the
  Crossing economy is sparse — the factions (Plan 120) want and offer
  items that don't exist yet, and the crises (Plan 115) reference items
  that aren't there.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/crossing_items.json` (expand `items` 11 → 25)
- Read-only: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` (confirm
  item DTO and valid `type` values)

## Content grammar (per item)
- `id`: snake_case, prefix `item_` (confirmed convention).
- `displayName`: evocative item name.
- `description`: 2–4 sentences in the established Crossing voice.
- `type`: item type string (confirm valid set in step 1 — Quest,
  Consumable, Tool, Trade, etc.).
- `stackMax`: integer max stack size.
- `weight`: float weight.
- `tradeValue`: integer trade value.
- `thirstRestore` / `hungerRestore` / `moraleEffect`: integer/float
  effects (0 if none).

## Steps
1. Read `ItemCatalogLoader.cs` to confirm the item DTO and all valid
   `type` values.
2. Inventory the 11 existing items. Identify which Crossing themes
   (trade, arbitration, black market, water, food, fuel, documents) lack
   items.
3. Author 14 new items:
   - `item_arbitration_token`: a token granting one arbitration hearing;
     Quest type, high trade value.
   - `item_charter_stamp`: an official stamp that validates a charter
     document; Quest type.
   - `item_weighbridge_chit`: a chit from the weighbridge recording a
     verified weight; Trade type.
   - `item_smuggled_medicine`: off-ledger medicine; Consumable, restores
     health, high value.
   - `item_crossing_bread`: dense ration bread baked at the granary;
     Consumable, restores hunger.
   - `item_lamp_oil_crossing`: Crossing-sourced lamp oil; Tool/Fuel type.
   - `item_filtered_water_crossing`: water from the Crossing committee's
     filtration; Consumable, restores thirst.
   - `item_quarantine_bands`: colored bands marking disease screening
     status; Quest type.
   - `item_granary_receipt`: a receipt for grain stored in the communal
     granary; Quest/Trade type.
   - `item_smugglers_ledger`: an off-ledger trade record; Quest type,
     contraband.
   - `item_rejection_notice`: an official notice of rejected
     arbitration; Quest type.
   - `item_crossing_map`: a map of the Crossing's internal routes and
     back channels; Tool type.
   - `item_black_market_pouch`: a pouch for carrying off-ledger goods
     discreetly; Tool type.
   - `item_charter_draft`: a draft charter amendment; Quest type, high
     political value.
4. Each item: distinct type, balanced weight/tradeValue/effects,
   description in the Crossing voice.
5. Cross-reference: every item id unique; every id follows `item_` prefix;
   no two items share the same displayName.
6. Wire 4 items to Plan 120 (crossing factions — factions want/offer new
  items).
7. Wire 3 items to Plan 115 (crossing encounters — encounters grant/
  require items).
8. Wire 2 items to Plan 126's sibling plans (Holdfast/Verdict items
  where Crossing items appear in cross-expansion trade).
9. Validate: `--data-integrity-selftest` (all item ids resolve).
10. xUnit: Crossing item catalog loads 25 items, all ids unique, all
    types valid, all descriptions non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `type` validation (step 1): if the item
loader enforces a specific enum, invalid types will fail. Confirm the
valid set before authoring.

## Definition of Done
- `crossing_items.json` has 25 items, all ids unique, all types valid, 4
  wired to crossing factions, 3 to crossing encounters, integrity +
  tests green.

## Follow-on
- Plan 120 (crossing factions) — factions want/offer new items.
- Plan 115 (crossing encounters) — encounters grant/require items.
- Plan 116 (deep lore locations) — Crossing items appear in Crossing
  location loot tables.
- Plan 99 (hardcore economy tuning) — Crossing items get price tiers.
- Plan 105 (trade specialties) — Crossing items match profession patterns.
