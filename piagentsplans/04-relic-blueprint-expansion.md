# Plan 04 — Workshop Relic Blueprint Expansion (6 → 30 relics)

## Goal (2 lines)
Exploit the fully-implemented, underused `WorkshopReverseEngineeringSystem` by expanding
`relic_recipes.json` from 6 verified entries to 30 pre-war technical schematics — pure DATA
work, zero new Core code.

## Why this first (P1)
- Registry §20 lists relic blueprints as a top safe extension; live count verified = **6**.
- Cheapest large content win in the project: data-only, no determinism risk, no save change.
- Creates a mid-game progression spine (relic → teardown → blueprint → craftable tech).

## Files to touch
- `Assets/StreamingAssets/Data/relic_recipes.json` (add 24 entries)
- `Assets/StreamingAssets/Data/items.json` (new `item_relic_*` source relics + unlocked
  outputs, if not already present — check existing ids first)
- Read-only: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` (confirm schema:
  teardown yield, tool wear, blueprint unlock fields), `CatalogIntegrityValidator` prefixes

## Content grammar (per entry)
- snake_case id with known prefix (`item_` for relics/outputs; recipe key per existing file shape)
- teardown inputs/outputs must resolve to real `items.json` ids (TIER-1/TIER-2 validation)
- grounded tone only: automated turret schematics, hydroponic nutrient synthesizer,
  micro-dosimeter, water-condenser coil, signal amplifier, battery reconditioner, etc.
  No magic, no real countries, no pre-war brand names of real companies.
- Writing voice via skill `ashfall-write`; entry generation via `ashfall-data-add`.

## Steps
1. Read the 6 existing entries → extract the exact JSON schema (fields, ranges).
2. Check which output items already exist in `items.json`; author missing ones in the same commit.
3. Draft 24 relics in 3 tiers (common salvage / rare pre-war tech / unique military vault)
   with escalating teardown difficulty and tool-wear cost.
4. Validate: `--data-integrity-selftest` 0 errors; loader binding test; one headless
   reverse-engineering run per tier (existing demo/selftest if available).

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — data-only; id collisions are the main hazard (validator catches them).

## Definition of Done
- 30 relic entries, all ids resolving, integrity gate green, distinct tiers with meaningful
  unlocks consumed by `CraftingSystem` recipes.
