# Plan 37 — Excavation Sites Catalog (system exists, no data)

## Goal (2 lines)
Create `excavation_sites.json` for `ExcavationSystem` — the system implements depth, shoring,
and cave-in mechanics but is used only for starting shelter rooms (verified: no site catalog).
Add 8 deep-strata excavation sites: buried command vaults, utility tunnels, metro
interchanges, mine shafts, archive bunkers, sealed military complexes, drainage networks,
and forgotten storage chambers.

## Why (P2)
- Verified: `ExcavationSystem.cs` exists with depth/shoring/cave-in logic; no
  `excavation_sites.json` exists. The system is wired but has no destinations.
- Creates the underground-exploration tier: deep digs yield relics (feeds Plan 04), pre-war
  documents (feeds existing 17B), and sealed-archive lore (feeds W9 in roadmap 31).
- Pure DATA work — zero new Core code if a loader exists.

## Files to touch
- `Assets/StreamingAssets/Data/excavation_sites.json` (CREATE — 8 sites)
- Read-only: `Assets/Ashfall.Core/ExcavationSystem.cs` (confirm site schema: depth profile,
  shoring material cost, cave-in risk curve, loot table, hazard flags), `ExpeditionSystem.cs`
  (confirm how excavation sites connect to expedition dispatch — do they use `expeditions.json`
  ids or a separate dispatch path?)
- Check loader: `grep -rn "excavation_sites\|ExcavationSite\|LoadExcavation" Assets/Ashfall.Core/`

## Content grammar (per site)
- snake_case `id` with prefix `loc_` (reuse the location prefix — these are locations; confirm
  whether they also need entries in `locations.json` or only in the excavation catalog).
- depth_profile: list of strata (surface → sediment → rubble → concrete → vault_door →
  interior), each with a depth value and a cave-in risk multiplier.
- shoring_cost: materials per stratum (timber, steel beams, sandbags — existing `item_*` ids).
- loot_table: weighted list of `item_*` ids, biased toward relics (Plan 04), documents
  (existing 17B), and unique pre-war artifacts.
- hazard: spore_mold (feeds existing 09A disease), flood, gas_pocket, structural_collapse,
  radiation_hotspot, unexploded_ordnance.
- history: 2-3 sentence environmental-storytelling description (who built it, why it was
  sealed, what happened to the people inside). Grounded tone only.

## Steps
1. Read `ExcavationSystem.cs` end-to-end: confirm the site schema, the depth/shoring/cave-in
   math, the loot-resolution mechanism, and whether sites dispatch via `expeditions.json` or
   a separate path.
2. Confirm loader status; if missing, add a mechanical loader (same pattern as 33/34).
3. Author 8 sites with distinct identities:
   - Collapsed Civil Defense Command Vault (military, deep, high-radiation, command documents)
   - Utility Tunnel Network (urban, shallow, flood hazard, electrical salvage)
   - Buried Metro Interchange (urban, medium depth, structural collapse, commuter artifacts)
   - Abandoned Mine Shaft (rural, deep, gas pocket, mineral salvage + pre-war mining records)
   - Sealed Archive Bunker (scientific, deep, sealed door, pre-war research documents)
   - Drainage Network (urban, shallow, contaminated water, smuggler cache)
   - Forgotten Storage Chamber (industrial, medium, intact stockpile, expired supplies)
   - Pre-war Civilian Shelter (residential, shallow, tragic environmental storytelling)
4. Give each site: depth profile (3-6 strata), shoring cost per stratum, cave-in risk curve,
   loot table (5-10 weighted entries), 1-2 hazards, environmental-storytelling description.
5. Wire 4 sites into expedition dispatch (Plan 32) as subterranean destinations; confirm the
   dispatch path accepts excavation sites (or add the wiring if the system expects it).
6. Link 2 sites to spore-mold disease (existing 09A) as a depth hazard for unventilated digs.
7. Link 3 sites to relic loot tables (Plan 04) so excavation is the primary relic source.
8. Validate: `--data-integrity-selftest`; confirm a dig → shoring → cave-in roll → loot →
   breach loop works in a headless boot; save round-trip for in-progress digs.
9. xUnit: depth/cave-in determinism (seeded), shoring cost application, loot-table seeding,
   hazard application, save round-trip for in-progress excavation state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --expedition-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the dispatch-path question (step 1) is the hazard: if excavation sites need a
different dispatch path than `expeditions.json`, step 5 requires wiring. Confirm before
authoring.

## Definition of Done
- `excavation_sites.json` exists with 8 sites, all ids + loot ids resolving, dig loop works
  end-to-end, cave-in determinism pinned, save round-trip green, integrity + tests green.

## Follow-on
- Plan 04 (relic blueprints) — excavation is the primary relic source.
- Existing 11A (deep-strata excavation) — this plan implements it as a data task.
- Existing 17B (documents) — excavation yields pre-war archive documents.
- Existing 09A (disease) — spore-mold as a depth hazard.
