# Plan 44 — Faction Territory Map (19 factions × territory nodes)

## Goal (2 lines)
Create `faction_territory.json` — a catalog mapping all 19 existing factions to map
territory nodes, control points, and contested zones. Factions physically control regions
of the wasteland: their territory affects travel safety, trade prices, checkpoint
encounters, and which settlements answer to whom.

## Why (P2)
- Verified: `factions.json` has 19 factions but no territory data; `wasteland_map_v1.json`
  has only 6 nodes / 7 routes (existing 16A expands to 60). Factions have no physical
  presence on the map — they're lore entries, not world actors.
- Territory creates the faction-territory-trade-war chain: control a route → tax caravans
  → rival faction contests → patrol encounters → reputation shifts → territory shifts.
- Pure DATA work — extends the existing map and faction systems with a territory overlay.

## Files to touch
- `Assets/StreamingAssets/Data/faction_territory.json` (CREATE — 19 territory definitions)
- Read-only: `Assets/StreamingAssets/Data/factions.json` (19 factions — territory must
  reference `faction_*` ids), `Assets/StreamingAssets/Data/wasteland_map_v1.json` (6 nodes
  — territory references map node ids; existing 16A expands to 60, so this plan should
  reference both current and planned node ids), `Assets/StreamingAssets/Data/locations.json`
  (control points may reference `loc_*` ids)
- Check: `grep -rn "territory\|Territory\|faction_control" Assets/Ashfall.Core/` — does an
  existing system consume territory data? If not, data-first.

## Content grammar (per faction territory)
- snake_case `id` with prefix `territory_` or `faction_territory_` (confirm accepted prefix).
- faction: `faction_*` id (TIER-2 validation) — which faction controls this territory.
- controlled_nodes: list of map node ids (from `wasteland_map_v1.json` + planned 16A nodes).
- control_points: list of `loc_*` ids — physical locations the faction holds (checkpoints,
  strongholds, resource sites) that anchor their control.
- contested_with: list of `faction_*` ids — rival factions contesting this territory.
- control_strength: 0–100; decays if patrols aren't maintained (feeds Plan 45).
- trade_tax: percentage the faction levies on caravans passing through their territory
  (feeds existing 16B caravan economy).
- travel_safety: modifier to encounter probability in this territory (safe under friendly
  faction, dangerous under hostile faction).
- shift_trigger: what causes territory to change hands (faction war victory, settlement
  allegiance change, player action, debt default — feeds Plan 40).

## Steps
1. Read `factions.json` to inventory all 19 factions; classify each as territorial
   (controls land), nomadic (moves through land), or ideological (no fixed territory).
2. Read `wasteland_map_v1.json` to inventory the 6 existing nodes; read existing 16A plan
   to understand the planned 60-node expansion; reference both.
3. Confirm whether an existing system consumes territory data (step in Files section).
4. Author 19 territory definitions: for each territorial faction, assign controlled nodes,
   control points (from `locations.json`), contested rivals, control strength, trade tax,
   and travel safety. Nomadic factions get migration routes instead of fixed territory.
5. Create 5 contested zones where two rival factions both claim the same nodes — these
   are the flashpoints for faction war (existing 06C) and patrol encounters (Plan 45).
6. Cross-reference: every `faction_*` id resolves; every map node id exists (current or
   planned); every `loc_*` control point resolves; every `contested_with` faction exists.
7. Wire 3 territories into the caravan system (existing 16B) — trade tax applies to
   caravans passing through; travel safety modifies encounter probability.
8. Wire 2 territories into Plan 40 debt consequences — default on a faction debt shifts
   control strength in that faction's territory.
9. Validate: `--data-integrity-selftest`; confirm territories load and reference valid
   nodes/factions/locations in a headless boot.
10. xUnit: territory catalog loads, all references resolve, contested zones have exactly 2+
    claiming factions, trade tax applies to caravan routes, control strength is saved and
    round-trips.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is referencing map node ids that don't exist yet (16A
expansion not done). Reference current 6 nodes + use planned ids only if 16A is committed.

## Definition of Done
- `faction_territory.json` exists with 19 territory definitions + 5 contested zones, all
  references resolving, trade tax wired into caravans, control strength saved and
  round-trips, integrity + tests green.

## Follow-on
- Plan 45 (faction patrols) — patrols maintain control strength in territory.
- Plan 43 (settlements) — settlements anchor control points.
- Existing 06C (faction war) — contested zones are the war flashpoints.
- Existing 16B (caravans) — trade tax + travel safety.
- W43 in roadmap 31 (faction territorialization pillar).
