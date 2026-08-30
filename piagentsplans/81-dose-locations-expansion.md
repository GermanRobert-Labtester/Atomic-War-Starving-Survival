# Plan 81 — Dose Locations Expansion (3 → 12 dose-ledger locations)

## Goal (2 lines)
Expand `dose_locations.json` from 3 verified locations to 12. The dose-ledger
system tracks radiation exposure at named locations within and around the
shelter — each location has a sector, risk level, and radiation dose. The
`DoseContentCatalog.cs` is confirmed live, but only 3 bunker-internal locations
are defined, leaving the surface and expedition sites invisible to the dose
ledger.

## Why (P2)
- Verified: `dose_locations.json` has 3 entries (id, displayName, sector,
  riskLevel, radiationUsv, description). `DoseContentCatalog.cs` is confirmed in
  Core. All 3 existing locations are sector `bunker` — the surface, expedition
  sites, and external zones have no dose tracking.
- Creates the radiation-cartography pillar: the dose ledger should cover every
  place a survivor can be exposed — bunker rooms, surface approaches, expedition
  destinations, faction checkpoints, ruins. Without surface locations, the
  player can't see the radiation cost of going outside.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/dose_locations.json` (expand 3 → 12 locations)
- Read-only: `Assets/Ashfall.Core/DoseContentCatalog.cs` (confirm schema and how
  locations are referenced by the dose-ledger system)
- `Assets/StreamingAssets/Data/expeditions.json` (expedition destination ids may
  cross-reference dose locations)

## Content grammar (per location)
- snake_case `id` with prefix `loc_` (confirmed prefix).
- Sector: bunker / surface / expedition / external / faction.
- riskLevel: 0–8 (0 = safe bunker room, 8 = hot zone).
- radiationUsv: 0.01–80.0 (microsieverts per hour — grounded, realistic range).
- description: 1–3 sentences of environmental text describing the location and
  why it has this dose level (contaminated water table, fallout deposit, damaged
  shielding, etc.).
- Diversity: bunker rooms (low dose, shielding), surface approaches (moderate,
  fallout residue), expedition sites (high, hot zones), faction checkpoints
  (variable, depends on proximity to ground zero).

## Steps
1. Read `DoseContentCatalog.cs` to confirm the location schema and how
   radiationUsv is applied to the dose ledger (per-hour accumulation? per-visit?).
2. Read the existing 3 locations to confirm the bunker-internal pattern.
3. Author 9 new locations across 4 sectors:
   - Surface (3): shelter exterior approach, surface observation post,
     contaminated water access point.
   - Expedition (3): irradiated forest edge, ruined hospital grounds,
     military depot perimeter.
   - External (2): frozen wetland crossing, burned woodland ridge.
   - Faction (1): garrison checkpoint gamma exterior.
4. Each location: distinct sector, riskLevel, radiationUsv, and description.
   Surface locations should have moderate dose (0.5–5.0 uSv/h); expedition sites
   should have high dose (5.0–80.0 uSv/h); faction checkpoints variable.
5. Cross-reference: every loc_ id unique; cross-reference 3 expedition dose
   locations with Plan 76 expedition destinations (matching loc_ ids).
6. Validate: `--data-integrity-selftest` (all ids resolve).
7. xUnit: dose location catalog loads 12 locations, all ids unique, riskLevel
   and radiationUsv within valid ranges, all 4 sectors represented.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is radiationUsv realism (step 4): keep doses
grounded — a hot zone is 10–80 uSv/h, not 10,000. Check existing values for the
project's baseline.

## Definition of Done
- `dose_locations.json` has 12 locations, all ids resolving, 4 sectors
  represented, 3 cross-referenced with Plan 76 expedition destinations,
  integrity + tests green.

## Follow-on
- Plan 76 (expedition destinations) — expedition dose locations match
  destination ids.
- Plan 48 (weather gates) — fallout storms increase surface location dose.
- Plan 46 (scavenging) — high-dose locations have better loot (risk-reward).
- Existing 09B (radiation system) — dose locations feed the radiation system.
- Plan 83 (weather seasons) — seasonal fallout shifts dose levels.
