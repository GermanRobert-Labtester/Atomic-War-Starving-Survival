# Plan 57 — Incident Expansion (5 → 25 shelter incidents)

## Goal (2 lines)
Expand `incidents.json` from 5 verified entries to 25 shelter incidents — random events
that fire during the shelter tick: radiation spikes, bunker breach attempts, equipment
failures, survivor disputes, supply discoveries, disease outbreaks, and external threats.
The incident system is wired but starved of content.

## Why (P2)
- Verified: `incidents.json` has 5 entries (`incident_radiation_spike`,
  `incident_bunker_breach`, etc. with title, bodyText, weight, minDay). The incident
  system fires during shelter ticks but 5 events is not enough for variety.
- Creates the shelter-tick-content pillar: incidents are the "what happens today" layer
  that makes the shelter feel alive between expeditions. Without variety, every day
  feels the same.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/incidents.json` (expand 5 → 25 incidents)
- Read-only: confirm the incident system consumer — `grep -rn "incidents.json\|Incident"
  Assets/Ashfall.Core/` to find the loader and confirm the schema (id, title, bodyText,
  weight, minDay, optional maxDay, optional choices, optional faction_link)

## Content grammar (per incident)
- snake_case `id` with prefix `incident_` (confirmed prefix).
- category: environmental / security / medical / social / equipment / supply / external /
  psychological.
- weight: relative probability (higher = more frequent).
- minDay / maxDay: when the incident can fire (early / mid / late campaign).
- bodyText: 1-2 sentences of grounded, urgent prose (what just happened). Skill
  `ashfall-write`.
- choices: optional 2-3 player choices with consequences (morale delta, resource cost,
  reputation shift, health risk).
- faction_link: optional `faction_*` id — some incidents involve faction activity near
  the shelter.
- system_link: optional — which system the incident affects (NeedsSystem, MedicalSystem,
  PowerGridSystem, CohortSystem, etc.).

## Steps
1. Read `incidents.json` to confirm the 5 existing entries and their schema.
2. Find the incident system consumer (loader + tick integration) to confirm how incidents
   fire, whether choices are supported, and which systems they can affect.
3. Author 20 new incidents across 8 categories:
   - Environmental (3): fallout storm approach, contaminated water table, ground tremor.
   - Security (3): perimeter breach attempt, unknown visitor at the door (feeds
     `door_encounters.json`), signal intercept near shelter.
   - Medical (3): disease outbreak (feeds existing 09A), chemical exposure, survivor
     collapse (feeds existing 09B).
   - Social (3): ration dispute (feeds existing 12B), ideological friction (feeds
     existing 25A), grief episode (feeds existing 27C).
   - Equipment (3): generator failure (feeds `power_grid.json`), air filter breakdown,
     water pipe burst.
   - Supply (2): cache discovered nearby, supply drop landed near shelter.
   - External (2): faction patrol spotted nearby (feeds Plan 45), refugee group
     approaching (feeds Plan 43/52).
   - Psychological (1): mass morale drop — anniversary of the exchange.
4. Give each incident: category, weight, minDay/maxDay, bodyText, optional choices with
   consequences, optional faction/system links.
5. Cross-reference: every `faction_*` link resolves; every system_link references an
   existing system; every choice consequence item/reputation delta is valid.
6. Wire 5 incidents to fire on specific campaign phases (early/mid/late) — the world
   changes as the campaign progresses.
7. Wire 3 incidents to faction activity (patrol spotted, faction radio intercept, faction
   supply caravan nearby — feeds Plan 44/45).
8. Validate: `--data-integrity-selftest`; confirm incidents fire during shelter ticks in
   a headless boot; confirm choices produce consequences.
9. xUnit: incident catalog loads, all references resolve, weight-based selection is
   deterministic (seeded), minDay/maxDay gates fire correctly, choices apply consequences,
   save round-trip preserves incident history.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is the choices field (step 2): if the incident system
doesn't support player choices, those are data-first and the wiring is a follow-on.

## Definition of Done
- `incidents.json` has 25 incidents (5 existing + 20 new), all references resolving, 5
  phase-gated, 3 faction-linked, incidents fire during shelter ticks, choices apply
  consequences, save round-trip green, integrity + tests green.

## Follow-on
- Plan 45 (patrols) — faction patrol incidents feed the faction-territory loop.
- Plan 43 (settlements) — refugee approach incidents feed settlement relations.
- Existing 09A/09B (medical) — disease and exposure incidents feed the medical system.
- Existing 12B (friction) — ration dispute and ideological friction incidents.
- Plan 71 (power grid) — equipment failure incidents feed the power grid.
