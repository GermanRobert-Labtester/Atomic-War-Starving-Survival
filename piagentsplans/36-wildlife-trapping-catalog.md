# Plan 36 — Wildlife Trapping Catalog (system exists, no data)

## Goal (2 lines)
Create `wildlife_trapping_catalog.json` for `WildlifeTrappingSystem` — the system is fully
implemented and save-supported but has **zero data** (verified: file missing). Add 10 trap
types and 15 prey entries that turn trapping into a viable food-acquisition loop tied to
migration windows (Plan 35) and seasonal weather.

## Why (P2)
- Verified: `WildlifeTrappingSystem.cs` exists in Core; no trapping catalog file exists.
- Creates a renewable food source that competes with scavenging and hunting: traps are
  passive (set-and-return) but consume materials and have bycatch/disease risks.
- Pure DATA work — zero new Core code if a loader exists.

## Files to touch
- `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` (CREATE — 10 traps + 15 prey)
- Read-only: `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` (confirm trap schema: trap id,
  material cost, setup time, check interval, catch probability, durability, prey list),
  `Assets/StreamingAssets/Data/items.json` (trap material costs must resolve to real items)
- Check loader: `grep -rn "wildlife_trapping\|WildlifeTrapping\|trap" Assets/Ashfall.Core/`

## Content grammar (per trap)
- snake_case `id` with prefix `trap_` or `item_trap_` (confirm accepted prefix — do not invent).
- materials: list of `item_*` ids consumed on setup (rope, wire, timber, bait).
- catch_probability: base chance modified by prey presence (linked to Plan 35 migration data).
- durability: number of catches before the trap breaks (consumes replacement materials).
- bycatch: chance of catching something unintended (irradiated fauna, diseased animal, human
  intruder → moral/event hook).

## Content grammar (per prey)
- snake_case `id` with prefix `prey_` or `wildlife_` (confirm accepted prefix).
- food_yield: item id produced (e.g. `item_raw_meat`, `item_dried_fish`).
- migration_link: optional `migration_*` id from Plan 35 — prey only available during that
  migration window in that zone.
- disease_risk: chance of contaminated catch (feeds Plan 112 disease content and Plan 09A response).
- seasonal: which seasons the prey is active (links to existing 19C seasonal cadence).

## Steps
1. Read `WildlifeTrappingSystem.cs` end-to-end: confirm the trap schema, the catch-resolution
   logic, the durability decay, and the save DTO shape.
2. Confirm loader status; if missing, add a mechanical loader (same pattern as 33/34).
3. Author 10 traps: snare, deadfall, pit trap, net trap, fish trap, cage trap, bird snare,
   conibear (body-grip), box trap, improvised wire snare. Each with distinct material cost,
   catch probability, and prey list.
4. Author 15 prey entries: rabbit, hare, deer, wild_boar, fox, rat, pigeon, crow, fish_perch,
   fish_pike, irradiated_squirrel, contaminated_fowl, feral_dog, muskrat, hedgehog. Each with
   food yield, migration link (where applicable), disease risk, seasonal window.
5. Cross-reference: every material `item_*` id resolves; every `migration_*` link resolves to
   Plan 35 entries; every food-yield `item_*` id exists in `items.json` (add if missing).
6. Wire 3 traps into expedition loot or shelter-crafting recipes so traps are obtainable.
7. Validate: `--data-integrity-selftest`; confirm a trap set → check → catch → yield loop
   works in a headless boot; confirm migration-linked prey only appears in-window.
8. xUnit: trap setup consumes materials, catch probability respects migration window, disease
   risk applies on contaminated catch, durability decays, save round-trip preserves trap state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — data-only if loader exists. The one trap is inventing prey `item_*` outputs that don't
exist in `items.json` (step 5 prevents this).

## Definition of Done
- `wildlife_trapping_catalog.json` exists with 10 traps + 15 prey, all ids resolving, trap
   loop works end-to-end, migration-linked prey respects windows, save round-trip green,
   integrity + tests green.

## Follow-on
- Plan 35 (migration) feeds prey availability windows.
- Existing 13B (hunting loop) and Plan 28's ecological-forecast layer consume trapping data.
- Bycatch (human intruder) creates moral encounter hooks (feeds Plan 49 encounters).
