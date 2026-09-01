# Plan 85 — Damaged Map Zones Expansion (3 → 12 treasure-map zones)

## Goal (2 lines)
Expand `damaged_map_zones.json` from 3 verified zones to 12. The damaged map
system defines treasure-map puzzles — each zone has map fragments the player
collects and assembles to reveal a hidden installation with unique loot. The
system is confirmed live (referenced in `ContentUtilizationScanner.cs`), but 3
zones means only 3 hidden installations to discover in the entire world.

## Why (P2)
- Verified: `damaged_map_zones.json` has 3 entries (zone_id, zone_name,
  total_fragments, hidden_installation_id, hidden_installation_name,
  installation_description, revealed_items, fragments with fragment_id, label,
  description). The system is confirmed live via ContentUtilizationScanner.
- Creates the cartographic-discovery pillar: damaged maps are the game's
  treasure-hunt system — collect fragments, assemble the map, reveal a hidden
  location, expedition there for unique loot. 3 zones is one session of
  discovery; 12 zones creates a sustained scavenging motivation across the full
  campaign.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/damaged_map_zones.json` (expand 3 → 12 zones)
- Read-only: grep for the consuming system to confirm how fragments are
  collected and how revealed_items resolve
- `Assets/StreamingAssets/Data/items.json` (revealed_items must resolve as
  item ids)

## Content grammar (per zone)
- snake_case `zone_id` (e.g. `industrial_district`, `suburban_heights` —
  descriptive, not prefixed).
- zone_name: evocative area name ("Industrial District", "Suburban Heights").
- total_fragments: 2–4 (number of map fragments needed to reveal the
  installation).
- hidden_installation_id: snake_case id for the revealed installation
  (e.g. `underground_fuel_depot`, `municipal_seed_vault`).
- hidden_installation_name: display name of the hidden installation.
- installation_description: 2–4 sentences describing the installation and why
  it's hidden (sealed bunker, collapsed access, forgotten basement, etc.).
- revealed_items: 2–5 item ids unlocked when the map is assembled — must
  resolve in items.json. These are the unique loot found at the installation.
- fragments: array of total_fragments objects, each with:
  - fragment_id: snake_case id (e.g. `damaged_map_industrial_1`).
  - label: short label for the fragment ("Northern Sector", "Sewer Grid").
  - description: 1–2 sentences describing what the fragment shows and its
    condition (burn damage, water damage, torn, faded).
- Diversity: each zone should be a distinct area type (industrial, suburban,
  military, scientific, rural, urban, waterfront, underground, etc.) with a
  thematically appropriate hidden installation.

## Steps
1. Grep for the consuming system (`grep -rn "damaged_map_zones\|DamagedMap" \
   Assets/Ashfall.Core/ src/`) to confirm how fragments are collected (are they
   items? scavenging results? event rewards?) and how revealed_items are
   granted.
2. Read `items.json` to confirm which item ids exist for revealed_items; note
   gaps for step 6.
3. Author 9 new zones across 9 area types:
   - Urban (2): collapsed hospital basement (medical cache), municipal archive
     vault (pre-war records and maps).
   - Rural (2): abandoned farm root cellar (seed bank and tools), forestry
     station bunker (chain-saw fuel and trapping gear).
   - Scientific (2): university lab basement (research samples and equipment),
     weather station vault (instruments and calibration tools).
   - Waterfront (1): dockyard warehouse (marine equipment and fuel).
   - Underground (1): metro maintenance tunnel (electrical parts and tools).
   - Military (1): decommissioned air-defense site (EMP-hardened equipment and
     communications gear).
4. Each zone: 2–4 fragments with distinct labels and descriptions. The
   fragments should tell a story when assembled — each piece adds context.
5. Each installation: unique revealed_items (2–5 per zone), thematically
   matched (hospital → medical supplies, military → weapons/comms, farm →
   seeds/tools).
6. Add any missing revealed item ids to `items.json` — only if a zone's
   revealed_items reference an item that does not exist.
7. Cross-reference: every zone_id unique; every fragment_id unique across all
   zones; every revealed_item resolves in items.json; every
   hidden_installation_id unique.
8. Wire 3 zones into Plan 76 expedition destinations (the hidden installations
   become expedition-reachable locations once revealed).
9. Wire 2 zones into Plan 46 scavenging tables (map fragments are scavenging
   loot in the corresponding area type).
10. Validate: `--data-integrity-selftest` (all ids resolve).
11. xUnit: damaged map catalog loads 12 zones, all zone_ids unique, all
    fragment_ids unique, all revealed_items resolve, total_fragments matches
    the fragments array length for each zone.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is fragment collection (step 1): confirm how
fragments are obtained before authoring — if they are items, the fragment_ids
may need to resolve in items.json.

## Definition of Done
- `damaged_map_zones.json` has 12 zones, all ids resolving, 3 wired to
  expedition destinations, 2 wired to scavenging tables, integrity + tests
  green.

## Follow-on
- Plan 76 (expedition destinations) — revealed installations become
  expedition-reachable locations.
- Plan 46 (scavenging tables) — map fragments are scavenging loot.
- Plan 47 (collectibles) — pre-war maps are collectible items.
- Plan 51 (environmental storytelling) — installation descriptions are
  environmental story entries.
- Plan 16 (cartography/infrastructure) — this plan deepens the cartography
  system.
