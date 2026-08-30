# Plan 82 — Verdict Investigation Sites Expansion (4 → 15 locations)

## Goal (2 lines)
Expand `verdict_locations.json` from 4 verified investigation sites to 15. The
Verdict system (`VerdictCatalogLoader.cs` confirmed live) defines remote
investigation sites the player can travel to — each has a description, danger
level, travel hours, and base radiation. The existing 4 sites are richly written
(seismometer pits, fuse bunkers, tape silos) but too few for a full
investigation campaign.

## Why (P2)
- Verified: `verdict_locations.json` has 4 entries (id, displayName, description,
  dangerLevel, travelHours, baseRadsPerHour). `VerdictCatalogLoader.cs` and
  `VerdictNpcSystem.cs` are confirmed live. The existing 4 sites are a connected
  narrative (geophone pit → twelve-gauge array → fuse world → tape silo) but the
  investigation trail ends there.
- Creates the investigation-arc pillar: Verdict sites are the game's deepest
  environmental-storytelling locations — each is a pre-war scientific/military
  site with a mystery to unravel. 4 sites is one arc; 15 sites creates a
  multi-arc investigation campaign with branching trails.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_locations.json` (expand 4 → 15 sites)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` (confirm
  schema and how locations are linked into investigation trails)
- `Assets/StreamingAssets/Data/verdict_npcs.json` (6 NPCs — some sites may
  reference NPC encounters)

## Content grammar (per site)
- snake_case `id` with prefix `loc_` (confirmed prefix).
- description: 3–6 sentences of dense environmental storytelling — physical
  detail, evidence, contradiction, mystery. Match the existing quality bar
  (the geophone pit and tape-silo descriptions are the model).
- dangerLevel: 3–10 (Verdict sites are remote and dangerous).
- travelHours: 3–12 (long travel is part of the cost).
- baseRadsPerHour: 20–60 (these are irradiated pre-war sites).
- Investigation arcs: group sites into 3–4 connected trails (each trail is a
  mystery the player unravels by visiting sites in sequence). The existing 4
  sites form the "Tempest Array" trail; add 2–3 more trails.
- Grounded tone: pre-war military, scientific, or governmental sites —
  seismometer arrays, weather stations, communications bunkers, survey sites,
  archive vaults. No fantasy, no supernatural.

## Steps
1. Read `VerdictCatalogLoader.cs` to confirm the schema and whether locations
   are linked into trails (is there a trail/arc field, or are trails implied by
   NPC dialogue and radio broadcasts?).
2. Read `verdict_npcs.json` to confirm which NPCs are site-linked and how they
   reference locations.
3. Read `verdict_radio.json` to confirm how radio broadcasts reference
   investigation sites (Plan 73 expands the faction radio corpus; Verdict radio
   is separate).
4. Author 11 new sites in 3 new investigation arcs:
   - Arc "The Coastal Survey" (4 sites): abandoned tide gauge, coastal
     meteorological station, cliff-top observation bunker, sealed marine lab.
   - Arc "The Interior Caches" (4 sites): forestry survey post, geological
     core-sample vault, river-gauging station, abandoned agricultural station.
   - Arc "The Border Wire" (3 sites): decommissioned signal relay, border
     checkpoint ruins, minefield observation tower.
5. Each site: dense description (match existing quality), dangerLevel,
   travelHours, baseRadsPerHour. Each arc tells a self-contained mystery
   (what was this site for? what happened here? what does the evidence reveal?).
6. Cross-reference: every loc_ id unique; check if any site should reference an
   existing verdict NPC (witness at the site).
7. Wire 2 sites into Plan 76 expedition destinations (coastal and border sites
   are reachable via expedition).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: verdict location catalog loads 15 sites, all ids unique, dangerLevel
   and travelHours within valid ranges, baseRadsPerHour realistic.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is description quality (step 5): the existing
sites set a very high bar — match it, do not write generic location descriptions.

## Definition of Done
- `verdict_locations.json` has 15 sites in 4 investigation arcs, all ids
  resolving, 2 wired to expedition destinations, integrity + tests green.

## Follow-on
- Plan 76 (expedition destinations) — 2 Verdict sites are expedition-reachable.
- Plan 73 (faction radio) — Verdict radio broadcasts reference investigation sites.
- Plan 51 (environmental storytelling) — Verdict sites are the deepest
  environmental-storytelling locations.
- Plan 84 (muster witnesses) — witnesses at Verdict sites provide testimony.
- Existing 18 (expansion deepening) — this plan deepens the Verdict expansion.
