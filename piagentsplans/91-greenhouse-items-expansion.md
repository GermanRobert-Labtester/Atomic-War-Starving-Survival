# Plan 91 — Greenhouse Items Expansion (14 → 30 greenhouse crop & supply items)

## Goal (2 lines)
Expand `greenhouse_items.json` from 14 verified items to 30. The greenhouse
system defines crops, seeds, tools, and supplies for the shelter greenhouse —
each item has type, stack size, weight, and trade value. The greenhouse is a
confirmed live system (ItemCatalogLoader) but 14 items is too few for a
food-production pillar.

## Why (P2)
- Verified: `greenhouse_items.json` has 14 entries (id, displayName,
  description, type, stackMax, weight, tradeValue). The greenhouse system is
  confirmed live via `ItemCatalogLoader.cs` and `AssetRegistry.cs`.
- Creates the food-production pillar: the greenhouse is the shelter's
  long-term food source — crops, seeds, tools, fertilizers, and supplies.
  14 items covers basic seeds but misses tools, fertilizers, pest control,
  water management, and specialized crops. Without these, the greenhouse is
  just a seed list, not a production system.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/greenhouse_items.json` (expand 14 → 30 items)
- Read-only: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` (confirm
  schema and how greenhouse items are loaded)

## Content grammar (per item)
- snake_case `id` with prefix `item_seed_` or `item_greenhouse_` (confirmed
  prefix pattern).
- displayName: 1–3 words (Spore Capsule, Tuber Cutting, Irradiated Compost).
- description: 1–2 sentences describing the item and its greenhouse use.
  Match the existing grounded, agricultural tone.
- type: Material / Tool / Consumable / Reagent.
- stackMax: 5–50 (seeds stack high, tools stack low).
- weight: 0.01–2.0 (seeds are light, tools are heavy).
- tradeValue: 1–15 (common seeds are cheap, rare seeds and tools are
  valuable).
- Item categories: seeds/crops (existing), tools (new), fertilizers (new),
  pest control (new), water management (new), structural (new).

## Steps
1. Read `ItemCatalogLoader.cs` to confirm the schema and how greenhouse items
   are loaded (are they a separate catalog or merged into the main item
   catalog?).
2. Read the existing 14 items to confirm the quality bar and the
   `item_seed_*` naming convention.
3. Author 16 new items across 5 new categories:
   - Tools (4): trowel, pruning shears, watering can, hand cultivator.
   - Fertilizers (3): irradiated compost, ash fertilizer, fish emulsion.
   - Pest control (3): insecticidal soap, copper tape, neem oil extract.
   - Water management (3): drip irrigation kit, water filter cartridge,
     rainwater collector.
   - Structural (3): greenhouse glass panel, UV plastic sheeting, shade cloth.
4. Each item: distinct id, displayName, description, type, stackMax, weight,
   tradeValue. Match the existing agricultural tone.
5. Cross-reference: every item id unique; every id follows the `item_*`
   convention; check if any item ids need to resolve in items.json (if the
   greenhouse catalog is merged with the main catalog).
6. Wire 4 items into Plan 55 crafting recipes (greenhouse tools and
   irrigation kits are craftable).
7. Wire 3 items into Plan 46 scavenging tables (greenhouse glass, UV
   sheeting, and drip irrigation are scavenged from agricultural sites).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: greenhouse item catalog loads 30 items, all ids unique, stackMax
   and weight within valid ranges, all types are valid enum values.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is catalog merging (step 1): confirm whether
greenhouse items are loaded as a separate catalog or merged into the main
item catalog — if merged, ids must not collide with existing item ids.

## Definition of Done
- `greenhouse_items.json` has 30 items, all ids unique, 4 wired to crafting
  recipes, 3 wired to scavenging tables, integrity + tests green.

## Follow-on
- Plan 55 (crafting recipes) — greenhouse tools and irrigation are craftable.
- Plan 46 (scavenging) — greenhouse supplies are scavenged from agricultural
  sites.
- Plan 22 (foundry/greenhouse production) — this plan provides the greenhouse
  item data.
- Plan 76 (expedition destinations) — agricultural stations yield greenhouse
  supplies.
- Plan 71 (power grid rooms) — greenhouse room draws power.
