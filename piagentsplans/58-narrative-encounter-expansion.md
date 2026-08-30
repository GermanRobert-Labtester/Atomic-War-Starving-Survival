# Plan 58 — Narrative Encounter Expansion (3 → 25 encounters)

## Goal (2 lines)
Expand `narrative_encounters.json` from 3 verified entries to 25 multi-choice encounters.
The `NarrativeEncounterSystem` is fully implemented (with stealth/speed weight
multipliers, location requirements, morale/guilt consequences) but has almost no content.
The expansion file has 29, but the base catalog is nearly empty.

## Why (P2)
- Verified: `narrative_encounters.json` has 3 entries (`enc_dead_letter_office` and 2
  others); `narrative_encounters_expansion.json` has 29. The base catalog is nearly empty
  — the system is live but underfed.
- Creates the encounter-variety pillar: each encounter has a description, player choices,
  morale/guilt consequences, stealth/speed modifiers, and optional location requirements.
  More encounters mean expeditions and shelter life produce more memorable moments.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/narrative_encounters.json` (expand 3 → 25 encounters)
- Read-only: `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` (confirm
  encounter schema: id, title, description, category, baseWeight, stealthWeightMultiplier,
  speedWeightMultiplier, minDangerLevel, requiredLocationId, forceOnArrival, choices with
  choiceId, text, moraleDelta, guiltDelta, and any other consequence fields)

## Content grammar (per encounter)
- snake_case `id` with prefix `enc_` or `encounter_` (confirm accepted prefix from existing
  3 entries — they use `enc_`).
- category: Discovery / Combat / Social / Moral / Environmental / Medical / Scavenging /
  Rescue / Hazard.
- baseWeight: relative probability of the encounter firing.
- stealthWeightMultiplier / speedWeightMultiplier: how the player's approach affects the
  encounter probability (stealth reduces combat encounters, speed increases discovery).
- minDangerLevel: minimum location danger level for the encounter to fire.
- requiredLocationId: optional `loc_*` id — some encounters are location-specific.
- choices: 2-4 options, each with text, moraleDelta, guiltDelta, and optional item/resource
  consequences.
- description: 2-3 sentences of grounded, human prose. Skill `ashfall-write`.

## Steps
1. Read `NarrativeEncounterSystem.cs` to confirm the full encounter schema, how
   baseWeight/stealth/speed modifiers work, and how choices resolve.
2. Read the 3 existing entries + the 29 expansion entries to understand the content style
   and avoid duplication.
3. Author 22 new encounters across 9 categories:
   - Discovery (3): an abandoned radio station, a sealed bunker door, a pre-war vehicle
     cache.
   - Combat (3): a scavenger ambush, a feral dog attack, a faction patrol confrontation
     (feeds Plan 45/54).
   - Social (3): a lone survivor asking for directions, a trader offering a deal, a
     child separated from their group (feeds Plan 52).
   - Moral (3): a cache belonging to a dead family, a survivor too injured to help, a
     faction demanding a "tax" on scavenged goods.
   - Environmental (2): a structurally unsound building, a contaminated water source.
   - Medical (2): a field clinic with dying patients, a stash of expired medicine
     (feeds existing 09A).
   - Scavenging (2): a locked supply room, a vehicle with fuel but no keys.
   - Rescue (2): a trapped survivor under rubble, a drowning person in a frozen river
     (feeds Plan 50/52).
   - Hazard (2): an unexploded ordnance site, a radiation hotspot with salvageable goods.
4. Give each encounter: category, weights, danger level, choices with consequences,
   description.
5. Cross-reference: every `requiredLocationId` resolves to `locations.json` or Plan 32;
   every choice consequence `item_*` id exists.
6. Wire 5 encounters to Plan 32 expedition destinations (location-specific encounters).
7. Wire 3 encounters to Plan 45 faction patrols (combat/social encounters with faction
   patrols).
8. Validate: `--data-integrity-selftest`; confirm encounters fire with correct weight
   modifiers in a headless boot; confirm choices apply morale/guilt/item consequences.
9. xUnit: encounter catalog loads, all references resolve, weight-based selection is
   deterministic (seeded), stealth/speed modifiers apply, choices apply consequences, save
   round-trip preserves resolved encounters.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data, extending an existing catalog.

## Definition of Done
- `narrative_encounters.json` has 25 encounters (3 existing + 22 new), all references
  resolving, 5 location-specific, 3 faction-linked, encounters fire with correct weight
  modifiers, choices apply consequences, save round-trip green, integrity + tests green.

## Follow-on
- Plan 32 (expedition wiring) — location-specific encounters fire at destinations.
- Plan 45 (patrols) — faction patrol encounters.
- Plan 54 (combat catalog) — combat encounters use enemy definitions.
- Plan 52 (NPC arcs) — social encounters introduce recurring NPCs.
- Plan 49 (micro-locations) — some encounters fire at micro-location discoveries.
