# Plan 05 — Vinyl Record Catalog Expansion (1 → 20 albums)

## Goal (2 lines)
Give the fully-wired `VinylMoraleSystem` (turntable playback, duration tracking, shelter-wide
morale buffs, host session + UI panel) a real catalog: 20 collectible albums replacing today's
single generic `item_vinyl_collection`.

## Why (P1)
- System is LIVE_CORE + LIVE_GODOT with panel, host session, and save store already built —
  the content is the only missing layer (verified: exactly 1 vinyl item in `items.json`).
- Adds collectible scavenging motivation + a readable morale economy + pre-war cultural texture
  at zero Core-code cost.

## Files to touch
- `Assets/StreamingAssets/Data/items.json` — 20 `item_vinyl_*` entries (weight/stack/value per
  existing conventions)
- Vinyl catalog file consumed by `VinylMoraleSystem` — locate the loader's data source first
  (`grep -rn "VinylMorale" Assets/Ashfall.Core/` and check which JSON it reads; create
  `vinyl_records.json` with `schema_version` only if no catalog file exists)
- Scavenge/loot tables or location loot if vinyls should drop in the world (check
  `ScavengeEngine` data source before touching)

## Content grammar (per album)
- id: `item_vinyl_<genre>_<name>`; genre set: classical, blues, jazz, propaganda_broadcast
- distinct buff profile per album: morale delta, duration, cooldown; propaganda records may
  carry an ideological-friction side effect (hook into `IdeologicalFrictionSystem` only if the
  system already reads a field — otherwise keep buffs within existing schema; NO Core changes)
- titles/descriptions: fictional artists and labels, exhausted restrained tone, pre-exchange
  era. Skill `ashfall-write`. No real musicians, bands, or songs.

## Steps
1. Find the vinyl catalog loader + schema (host session `src/Host/VinylMoraleHostSession.cs`
  shows the binding path).
2. Author 20 albums (5 per genre) with escalating rarity; wire 3–5 into scavenge loot tables.
3. Validate ids, run integrity gate, run any vinyl/turntable selftest or panel snapshot.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
# if panel visuals changed: snapshots diff per snapshots/ workflow
```

## Risk
LOW — data-only; the one trap is inventing a new catalog file the loader doesn't read
(step 1 exists to prevent exactly that).

## Definition of Done
- 20 playable albums with distinct effects, discoverable in-world, integrity + tests green.
