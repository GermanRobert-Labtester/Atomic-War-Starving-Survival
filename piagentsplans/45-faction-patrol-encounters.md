# Plan 45 — Faction Patrol Encounters (15 patrol templates)

## Goal (2 lines)
Create `faction_patrols.json` — 15 patrol encounter templates tied to faction territory
(Plan 44). Patrols are the visible behavior of factions on the map: they man checkpoints,
escort caravans, raid rivals, and intercept travelers. The player encounters them on
expeditions through controlled or contested territory.

## Why (P2)
- Verified: no `faction_patrols.json` exists; factions have no behavioral presence on the
  map. Plan 44 gives them territory; this plan gives them patrols that act in that
  territory — the two plans form the faction-territorialization pillar.
- Patrols create the encounter layer for faction dynamics: checkpoint (tax or pass),
  caravan escort (trade protection), raid party (attack rival), press gang (recruitment),
  refugee eviction (moral dilemma). Each has player choices and consequences.
- Pure DATA work — extends the existing encounter system (existing 20C) with faction-
  specific patrol templates.

## Files to touch
- `Assets/StreamingAssets/Data/faction_patrols.json` (CREATE — 15 patrol templates)
- Read-only: `Assets/StreamingAssets/Data/faction_territory.json` (Plan 44 — patrols
  reference territory ids), `Assets/StreamingAssets/Data/factions.json` (19 factions),
  `Assets/StreamingAssets/Data/events.json` (77 events — confirm encounter schema and
  reuse the existing event structure; do not invent a parallel system)
- Check: `grep -rn "patrol\|Patrol\|encounter" Assets/Ashfall.Core/` — does the existing
  encounter system accept patrol data, or are patrols a new event category?

## Content grammar (per patrol template)
- snake_case `id` with prefix `encounter_` or `event_` (reuse existing event prefix —
  do not invent; patrols are a category of encounter, not a new system).
- patrol_type: checkpoint / caravan_escort / raid_party / press_gang / refugee_eviction /
  supply_run / reconnaissance / border_patrol.
- faction: `faction_*` id — which faction's patrol this is.
- territory: `territory_*` id (Plan 44) — where this patrol operates.
- trigger: enter_territory / random_tick / caravan_passing / rival_activity.
- player_choices: 2-4 options (comply, resist, negotiate, bribe, flee, report).
- requirements: reputation threshold, item bribe cost, skill check (Plan 33).
- immediate_outcome: per choice — pass freely, taxed, combat, reputation shift, item loss.
- delayed_consequence: per choice — faction remembers, rival faction reacts, territory
  control shifts, bounty issued (feeds Plan 40).
- strength: number of patrol members + equipment tier; affects combat difficulty if the
  player resists.

## Steps
1. Read `events.json` to confirm the existing encounter schema; patrols must match it —
   do not create a parallel event system. If the schema doesn't support patrol-specific
   fields (faction, territory, strength), extend the schema minimally or use the existing
   field set creatively.
2. Read `faction_territory.json` (Plan 44) to map which territories need patrols and which
   factions control them.
3. Confirm whether the existing encounter system accepts faction/territory fields (step in
   Files section). If not, this is data-first and the wiring is a follow-on.
4. Author 15 patrol templates across 8 patrol types:
   - 3 checkpoints (friendly tax, neutral toll, hostile interrogation)
   - 2 caravan escorts (trade protection — player can join or avoid)
   - 2 raid parties (attacking a rival settlement — moral choice: intervene or ignore)
   - 1 press gang (forcibly recruiting refugees — moral dilemma)
   - 1 refugee eviction (faction clearing a camp — humanitarian crisis hook)
   - 2 supply runs (faction logistics — player can ambush or trade)
   - 2 reconnaissance patrols (scouting contested territory — stealth encounter)
   - 2 border patrols (intercepting travelers at territory edges)
5. Give each template: faction, territory, trigger, 2-4 player choices, requirements,
   immediate outcomes, delayed consequences, and patrol strength.
6. Cross-reference: every `faction_*` id resolves; every `territory_*` id resolves to
   Plan 44; every `item_*` bribe cost exists; every `skill_*` check resolves to Plan 33.
7. Wire 5 patrol templates into the expedition encounter system (Plan 32) — patrols
   appear as encounters when traveling through the relevant territory.
8. Wire 3 patrol templates into the caravan system (existing 16B) — caravan escorts and
   supply runs appear along caravan routes.
9. Validate: `--data-integrity-selftest`; confirm patrols trigger in the correct territory
   in a headless boot; confirm player choices produce the correct outcomes.
10. xUnit: patrol catalog loads, territory references resolve, player choices produce
    correct immediate outcomes, delayed consequences fire on schedule, reputation shifts
    apply, save round-trip for patrol encounter state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the encounter-schema question (step 1) is the hazard: if the existing event
schema doesn't support faction/territory/strength fields, either extend the schema (minor
Core change) or map patrols onto existing fields. Confirm before authoring.

## Definition of Done
- `faction_patrols.json` exists with 15 patrol templates, all references resolving, 5 wired
  into expedition encounters, 3 wired into caravan routes, player choices produce correct
  outcomes, delayed consequences fire, save round-trip green, integrity + tests green.

## Follow-on
- Plan 44 (faction territory) — patrols maintain control strength in their territory.
- Plan 40 (debt) — bounties generate raid-party patrols.
- Existing 06C (faction war) — patrols escalate into war encounters.
- Existing 14 (raids) — raid-party patrols are the raid encounter source.
- Existing 20C (encounter tables) — patrols are a faction-specific encounter category.
